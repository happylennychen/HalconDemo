using CommonApi.MyUtility;
using MyInstruments;
using MyInstruments.MyAltimeter;
using MyInstruments.MyEnum;
using MyInstruments.MyTec;
using MyInstruments.MyUtility;
using NLog;
using ProberApi.MyConstant;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Prober.WaferDef;
using Prober.Request;
using System.Drawing;
using MathNet.Numerics;

namespace Prober.Forms {
    public partial class FormTecControl : Form
    {
        private readonly List<InstrumentUsage> instrumentUsageList;
        private readonly Dictionary<string, Instrument> instruments;
        private readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        private StandaloneTec tec;
        private bool isMonitor = false;
        private double tecCoeffK = 1.24495;
        private double tecCoeffB = 7.0927;
        private ConcurrentDictionary<string, object> sharedObject;

        public FormTecControl(ConcurrentDictionary<string, object> sharedObjects)
        {
            InitializeComponent();

            this.sharedObject = sharedObjects;

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out object tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            var getResult2 = GetInstrument("chuck_temperature");

            GetCoeff();
        }

        public void GetCoeff() {
            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
            if (info != null) {
                tecCoeffK = info.TecCoeffK;
                tecCoeffB = info.TecCoeffB;
            }
        }

        public (bool isOk, string errorText, Instrument instrument) GetInstrument(string instrumentUsageId)
        {
            string errorText = string.Empty;
            var list = this.instrumentUsageList.Where(x => x.UsageId.Equals(instrumentUsageId)).ToList();
            InstrumentUsage instrumentUsage = list.First();
            if (list == null)
            {
                errorText = $"GetInstrumentUsage(={instrumentUsageId}) does not exist!";
                LOGGER.Error(errorText);
                return (false, errorText, null);
            }

            Instrument instrument = instruments[instrumentUsage.InstrumentId];
            switch (instrumentUsage.InstrumentCategory)
            {                
                case EnumInstrumentCategory.TEC:
                    if (instrumentUsage.InstrumentId == "tec_chuck")
                    {
                        tec = instrument as StandaloneTec;
                    }
                    else if (instrumentUsage.InstrumentId == "tec_grat")
                    {
                        tec = instrument as StandaloneTec;
                    }
                    else
                    {
                        errorText = $"{instrumentUsage.InstrumentId.ToString()} is not a valid instrument id of temperature stable monitor!";
                        LOGGER.Error(errorText);
                        return (false, errorText, null);
                    }
                    break;
                default:
                    errorText = $"{instrumentUsage.InstrumentCategory.ToString()} is not a valid instrument category of temperature stable monitor!";
                    LOGGER.Error(errorText);
                    return (false, errorText, null);
            }

            return (true, string.Empty, instrument);
        }

        private void btn_SetTemp_Click(object sender, System.EventArgs e)
        {
            double temp = Convert.ToDouble(num_TempSet.Value);

            if ((temp > 150) || (temp < 10))
            {
                MessageBox.Show("温度设置超出范围");
            }
            try
            {
                double tempSet = Math.Round((temp + tecCoeffB) / tecCoeffK,1);
                if (!tec.SetTemp(1, tempSet))
                {
                    MessageBox.Show("设置温度失败");
                }

                sharedObject.AddOrUpdate(PrivateSharedObjectKey.TEC_TEMPERATURE, num_TempSet.Value.ToString(), (key, OldValue) => num_TempSet.Value.ToString());
            }
            catch(Exception ex)
            {
                MessageBox.Show($"设置温度异常:{ex.Message}");
            }            
        }

        private void btn_TurnOnTEC_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!tec.SetTecEnable(1, true))
                {
                    MessageBox.Show("打开控温失败");
                }

                btn_TurnOnTEC.BackColor = Color.FromArgb(230, 244, 241);
                btn_TurnOffTEC.BackColor = SystemColors.Control;

                string tecStatue = "ON";
                sharedObject.AddOrUpdate(PrivateSharedObjectKey.TEC_STATUS, tecStatue, (key, OldValue) => tecStatue);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开控温异常:{ex.Message}");
            }
        }

        private void btn_TurnOffTEC_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!tec.SetTecEnable(1, false))
                {
                    MessageBox.Show("关闭控温失败");
                }

                btn_TurnOffTEC.BackColor = Color.FromArgb(230, 244, 241);
                btn_TurnOnTEC.BackColor = SystemColors.Control;

                string tecStatue = "OFF";
                sharedObject.AddOrUpdate(PrivateSharedObjectKey.TEC_STATUS, tecStatue, (key, OldValue) => tecStatue);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"关闭控温异常:{ex.Message}");
            }
        }

        private void btn_StartMonitor_Click(object sender, System.EventArgs e)
        {
            double temp = 0;
            isMonitor = true;

            foreach (var item in chart_Temp.Series)
            {
                item.Points.Clear();
            }

            btn_StartMonitor.BackColor = Color.FromArgb(230, 244, 241);
            btn_StopMonitor.BackColor = SystemColors.Control;
            btn_StartMonitor.Enabled = false;   

            Task.Run(() =>
            {
                try
                {
                    while (isMonitor)
                    {
                        if(!GetTemperature(1,out temp))
                        {
                            Invoke(new Action(() => { MessageBox.Show("读取温度失败"); }));
                            return;
                        }

                        temp = temp * tecCoeffK - tecCoeffB;

                        temp = Math.Round(temp,1);

                        BeginInvoke(new Action(() =>
                        {
                            chart_Temp.Series[0].Points.AddXY(DateTime.Now, temp);
                            txt_CurTempRead.Text = temp.ToString();
                        }));

                        Thread.Sleep(200);
                    }
                }
                catch(Exception ex ) 
                {
                    Invoke(new Action(() => { MessageBox.Show($"温度监控异常：{ex.Message}"); }));
                }
                finally
                {
                    Invoke(new Action(() => { btn_StartMonitor.Enabled = true; }));                    
                }
            });
        }

        private bool GetTemperature(int channel, out double Value)
        {
            Value = double.NaN;

            for (int i = 0; i < 5; i++)
            {
                if (tec.GetTemp(channel, out Value))
                {
                    return true;
                }
                else
                {
                    Thread.Sleep(500);
                }
            }

            return false;
        }

        private void btn_StopMonitor_Click(object sender, System.EventArgs e)
        {
            btn_StopMonitor.BackColor = Color.FromArgb(230, 244, 241);
            btn_StartMonitor.BackColor = SystemColors.Control;
            btn_StartMonitor.Enabled = true;

            isMonitor = false;
        }

        private void FormTecControl_Load(object sender, EventArgs e) {
            //获取温度值
            sharedObject.TryGetValue(PrivateSharedObjectKey.TEC_TEMPERATURE, out object value);
            if (value != null)
            {
                string temperatue = value as string;
                num_TempSet.Value = Convert.ToDecimal(temperatue);
            }

            sharedObject.TryGetValue(PrivateSharedObjectKey.TEC_STATUS, out value);
            string status = value as string;
            if (status == "ON")
            {                
                btn_TurnOnTEC.BackColor = Color.FromArgb(230, 244, 241);
                btn_TurnOffTEC.BackColor = SystemColors.Control;
            }
            else
            {
                btn_TurnOffTEC.BackColor = Color.FromArgb(230, 244, 241);
                btn_TurnOnTEC.BackColor = SystemColors.Control;
            }
/*
            if (!GetTemperature(1, out double temp)) {               
                return;
            }

            temp = temp * tecCoeffK - tecCoeffB;
            temp = Math.Round(temp, 1);
            txt_CurTempRead.Text = temp.ToString();
*/
        }

        private bool GetTecStatus(int port, out bool isOn)
        {
            isOn = false;
            for (int i = 0; i < 5; i++)
            {
                if (tec.GetTecEnable(port, out isOn))
                {
                    return true;
                }
                else
                {
                    Thread.Sleep(500);
                }
            }

            return false;
        }

        private void FormTecControl_FormClosing(object sender, FormClosingEventArgs e) {
            isMonitor = false;
        }

        private void toolStripMenuItemSaveData_Click(object sender, EventArgs e) {
            try {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "(*.csv)|*.csv";
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                string file = dlg.FileName;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Temperature");
                foreach (var item in chart_Temp.Series[0].Points) {
                    sb.AppendLine($"{item.YValues[0]}");
                }
                File.WriteAllText(file, sb.ToString());
                MessageBox.Show(this, "保存成功。", "Info");
            } catch (Exception ex) {
                MessageBox.Show(this, "保存失败。" + ex.Message, "Info");
            }
        }
    }
}
