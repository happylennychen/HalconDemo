using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

using CommonApi.MyI18N;
using CommonApi.MyUtility;

using NLog;

using ProberApi.MyCommunication;
using ProberApi.MyConstant;
using ProberApi.MyEnum;

namespace ProberApi.MyForm
{
    public partial class FormSettingControlMode : Form
    {
        public FormSettingControlMode(EnumLanguage language, ConcurrentDictionary<string, object> sharedObjects, MyServerSide serverSide)
        {
            InitializeComponent();
            frc = new FormResourceCulture(this.GetType().FullName, Assembly.GetExecutingAssembly());
            frc.Language = language;
            rc = new ResourceCulture($"{Assembly.GetExecutingAssembly().GetName().Name}.GlobalStrings", Assembly.GetExecutingAssembly());
            rc.Language = language;
            this.sharedObjects = sharedObjects;
            this.serverSide = serverSide;

            sharedObjects.TryGetValue(SharedObjectKey.ACTION_REPORT_MESSAGE, out object tempObj);
            actReportMessage = tempObj as Action<string>;
            sharedObjects.TryGetValue(SharedObjectKey.ACTION_SET_GUI_ENABLE_STATUS, out tempObj);
            actSetGuiEnableStatus = tempObj as Action<bool>;
            sharedObjects.TryGetValue(SharedObjectKey.ACTION_SET_GUI_CONTROL_MODE, out tempObj);
            actSetGuiControlMode = tempObj as Action<string>;
            sharedObjects.TryGetValue(SharedObjectKey.CONFIG_BOARD, out tempObj);
            settingBoard = tempObj as ConcurrentDictionary<string, object>;
        }

        private void FormSettingControlMode_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            settingBoard.TryGetValue(ConfigKey.CONTROL_MODE, out object tempObj1);
            string strControlMode = tempObj1 as string;
            Enum.TryParse(strControlMode, out lastConfirmedEnumControlMode);

            switch (lastConfirmedEnumControlMode)
            {
                case EnumControlMode.LOCAL_ONLY:
                    rbLocalOnly.Checked = true;
                    break;
                case EnumControlMode.REMOTE_FIRST:
                    rbRemoteFirst.Checked = true;
                    break;
            }

            tbLastControlMode.ReadOnly = true;
            tbLastControlMode.BackColor = Color.LightGray;
            tbLastControlMode.ForeColor = Color.Red;
            tbLastControlMode.Text = strControlMode;

            btnApply.Enabled = false;
            frc.EnumerateControl(this);
        }

        private void rbLocalOnly_CheckedChanged(object sender, EventArgs e)
        {
            EnumControlMode newEnumControlMode;
            if (rbLocalOnly.Checked)
            {
                newEnumControlMode = EnumControlMode.LOCAL_ONLY;
            }
            else
            {
                newEnumControlMode = EnumControlMode.REMOTE_FIRST;
            }

            btnApply.Enabled = newEnumControlMode != lastConfirmedEnumControlMode;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (rbLocalOnly.Checked)
            {
                DialogResult dgResult = MessageBox.Show(frc.GetString("txtQuestionSwitchToLocal"), rc.GetString("txtConfirm"), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dgResult == DialogResult.Cancel)
                {
                    return;
                }
                SwitchToLocalOnly(true);
            }
            else
            {
                DialogResult dgResult = MessageBox.Show(frc.GetString("txtQuestionSwitchToRemote"), rc.GetString("txtConfirm"), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dgResult == DialogResult.Cancel)
                {
                    return;
                }
                SwitchToRemoteFirst();
            }
        }

        public void SwitchToLocalOnly(bool ifUpdateConfigFile)
        {
            if (serverSide.IsListening())
            {
                MyServerSide copy = new MyServerSideImpl(rc.Language, sharedObjects, actSetGuiEnableStatus);
                copy.AttachReportMessage(serverSide.ReportMessage);
                serverSide.AbortHandling();
                serverSide.Close();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                serverSide = copy;
            }

            actSetGuiEnableStatus(true);
            lastConfirmedEnumControlMode = EnumControlMode.LOCAL_ONLY;
            settingBoard.AddOrUpdate(ConfigKey.CONTROL_MODE, lastConfirmedEnumControlMode.ToString(), (key, oldValue) => lastConfirmedEnumControlMode.ToString());
            btnApply.Enabled = false;
            tbLastControlMode.Text = lastConfirmedEnumControlMode.ToString();
            actSetGuiControlMode(lastConfirmedEnumControlMode.ToString());

            if (ifUpdateConfigFile)
            {
                if (!SaveControlModeToConfigFile(lastConfirmedEnumControlMode.ToString()))
                {
                    MessageBox.Show($"{frc.GetString("txtFailedToSaveControlMode")}\n{rc.GetString("txtLookupLog")}", rc.GetString("txtFail"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SwitchToRemoteFirst()
        {
            string listeningIp = settingBoard[ConfigKey.LISTENING_IP] as string;
            int listeningPort = (int)settingBoard[ConfigKey.LISTENING_PORT];
            lastConfirmedEnumControlMode = EnumControlMode.REMOTE_FIRST;
            settingBoard.AddOrUpdate(ConfigKey.CONTROL_MODE, lastConfirmedEnumControlMode.ToString(), (key, oldValue) => lastConfirmedEnumControlMode.ToString());
            btnApply.Enabled = false;
            if (!SaveControlModeToConfigFile(lastConfirmedEnumControlMode.ToString()))
            {
                MessageBox.Show($"{frc.GetString("txtFailedToSaveControlMode")}\n{rc.GetString("txtLookupLog")}", rc.GetString("txtFail"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            tbLastControlMode.Text = lastConfirmedEnumControlMode.ToString();
            actSetGuiControlMode(EnumControlMode.REMOTE_FIRST.ToString());
            actReportMessage($"{frc.GetString("txtBeginListening")}({listeningIp}:{listeningPort})......");
            bool occurredException = false;
            try
            {
                serverSide.Listen(listeningIp, listeningPort, 1);
            }
            catch (SocketException se)
            {
                if ((se.SocketErrorCode != SocketError.Interrupted) && (se.SocketErrorCode != SocketError.OperationAborted))
                {
                    actReportMessage($"{frc.GetString("txtListening")}(ip={listeningIp},port={listeningPort}) {frc.GetString("txtOccurredException")} {rc.GetString("txtLookupLog")}");
                }
                LOGGER.Error($"Listening(ip={listeningIp},port={listeningPort}) occurred SocketException: {se.Message}, SocketErrorCode={se.SocketErrorCode}");
                occurredException = true;
            }
            catch (Exception ex)
            {
                actReportMessage($"{frc.GetString("txtListening")}(ip={listeningIp},port={listeningPort}) {frc.GetString("txtOccurredException")} {rc.GetString("txtLookupLog")}");
                LOGGER.Error($"Listening(ip={listeningIp},port={listeningPort}) occurred Exception: {ex.Message}.");
                occurredException = true;
            }
            finally
            {
                if (occurredException)
                {
                    actSetGuiEnableStatus(true);
                    lastConfirmedEnumControlMode = EnumControlMode.LOCAL_ONLY;
                    settingBoard.AddOrUpdate(ConfigKey.CONTROL_MODE, lastConfirmedEnumControlMode.ToString(), (key, oldValue) => lastConfirmedEnumControlMode.ToString());
                    if (SaveControlModeToConfigFile(lastConfirmedEnumControlMode.ToString()))
                    {
                        rbLocalOnly.Checked = true;
                        tbLastControlMode.Text = lastConfirmedEnumControlMode.ToString();
                        actSetGuiControlMode(lastConfirmedEnumControlMode.ToString());
                    }
                }
            }
        }

        private bool SaveControlModeToConfigFile(string newControlMode)
        {
            string configFilePath = @"Configuration/config.xml";
            try
            {
                XDocument xdoc = XDocument.Load(configFilePath);
                XElement xeRoot = xdoc.Element("config");
                XElement xeControlMode = xeRoot.Element("control_mode");
                xeControlMode.Value = newControlMode;
                xdoc.Save(configFilePath);

                return true;
            }
            catch (Exception ex)
            {
                LOGGER.Error(ex.Message);
                return false;
            }
        }

        private readonly ConcurrentDictionary<string, object> sharedObjects;
        private MyServerSide serverSide;
        private readonly ConcurrentDictionary<string, object> settingBoard;
        private readonly Action<string> actReportMessage;
        private readonly Action<bool> actSetGuiEnableStatus;
        private readonly Action<string> actSetGuiControlMode;
        private EnumControlMode lastConfirmedEnumControlMode;
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        private readonly FormResourceCulture frc;
        private readonly ResourceCulture rc;
    }
}
