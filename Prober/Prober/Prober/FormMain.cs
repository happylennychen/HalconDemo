using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Timers;

using CommonApi.MyConstant;
using CommonApi.MyI18N;
using CommonApi.MyUtility;

using MyMotionStageDriver.MyEnum;
using MyMotionStageDriver.MyMotionController;
using MyMotionStageDriver.MyStageAxis;

using NLog;

using Prober.Constant;
using Prober.Forms;
using Prober.InitializeConfiguration;
using Prober.WaferDef;

using ProberApi.MyBoard;
using ProberApi.MyCommunication;
using ProberApi.MyConstant;
using ProberApi.MyCoupling;
using ProberApi.MyEnum;
using ProberApi.MyForm;
using ProberApi.MyInitialization;
using ProberApi.MyQuery;
using ProberApi.MyUtility;
using Prober.Request;
using CommonApi.MyEnum;
using MyMotionStageDriver.MyMotionController.Leisai;
using HalconDotNet;
using Microsoft.Win32;
using MyInstruments.MyEnum;
using MyInstruments.MyTec;
using MyInstruments.MyUtility;
using MyInstruments;
using MyInstruments.MyLed;
using Prober.Properties;
using MyMotionStageUserControl;
using System.Runtime.InteropServices;

namespace Prober {
    public partial class FormMain : Form {
        public FormMain() {
            InitializeComponent();
            frc = new FormResourceCulture(this.GetType().FullName, Assembly.GetExecutingAssembly());
            rc = new ResourceCulture($"{Assembly.GetExecutingAssembly().GetName().Name}.GlobalStrings", Assembly.GetExecutingAssembly());

            Action<string> actReportMessage = ReportMessage;
            sharedObjects.AddOrUpdate(SharedObjectKey.ACTION_REPORT_MESSAGE, actReportMessage, (key, oldValue) => actReportMessage);
            Action<bool> actSetGuiEnableStatus = SetGuiEnableStatus;
            sharedObjects.AddOrUpdate(SharedObjectKey.ACTION_SET_GUI_ENABLE_STATUS, actSetGuiEnableStatus, (key, oldValue) => actSetGuiEnableStatus);
            Action<string> actSetGuiControlMode = SetGuiControlMode;
            sharedObjects.AddOrUpdate(SharedObjectKey.ACTION_SET_GUI_CONTROL_MODE, actSetGuiControlMode, (key, oldValue) => actSetGuiControlMode);

            sharedObjects.AddOrUpdate(SharedObjectKey.HAS_CONNECTED_INSTRUMENTS, false, (key, oldValue) => false);
            sharedObjects.AddOrUpdate(SharedObjectKey.QUERY_LIST, allSupportedQueries, (key, oldValue) => allSupportedQueries);
            sharedObjects.AddOrUpdate(SharedObjectKey.REQUEST_STATUS_BOARD, requestStatusBoard, (key, oldValue) => requestStatusBoard);
            sharedObjects.AddOrUpdate(SharedObjectKey.CONFIG_BOARD, configBoard, (key, oldValue) => configBoard);
            sharedObjects.AddOrUpdate(SharedObjectKey.RED_GREEN_LIGHT_BOARD, redGreenLightBoard, (key, oldValue) => redGreenLightBoard);
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.HEIGHT_SCAN_MODE, "0", (key, oldValue) => "0");
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.MONIT_TEST_CONDITION, "0", (key, oldValue) => "0");
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_SEPERATE_HEIGHT, "500", (key, oldValue) => "500");
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POSITION_IS_SAFE, true, (key, oldValue) => true);
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POSITION_MONITOR_ENABLE, true, (key, oldValue) => true);
            
            List<string> redGreenLightKeyList = CommonRedGreenLigthKey.GetAllKeys();
            redGreenLightKeyList.AddRange(MyRedGreenLigthKey.GetAllKeys());
            foreach (string key in redGreenLightKeyList) {
                redGreenLightBoard.AddOrUpdateLight(key, false);
            }

            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.WAFER_UPLOAD_STATUE, false, (key, oldValue) => false);
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CCD_GOWITH_CHUCK, false, (key, oldValue) => false);

            bool checkState = false;
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CCD_GOWITH_CHUCK, checkState, (key, oldValue) => checkState);
        }

        private void FormMain_Load(object sender, EventArgs e) {
            var getResult = GetGuiLanguage();
            if (!getResult.isOk) {
                MessageBox.Show("Failed to get GUI language!\nPlease look up log for detailed reason!");
                this.Close();
            }
            frc.Language = getResult.guiLanguage;
            rc.Language = getResult.guiLanguage;
            stageJogInitializer = new MyMotionStageUserControl.MyUtility.MyInitialization(frc.Language, motionStageInitializer.stageUtility.StageList);
            
            var excludedStatusLabels = new HashSet<ToolStripStatusLabel> {
                slblUser,
                slblRole,
                slblLoginTime,
                slblControlMode
            };
            frc.ExcludedToolStripStatusLabels(excludedStatusLabels);
            frc.EnumerateControl(this);
            
            rtbMsgBox.ReadOnly = true;
            rtbMsgBox.BackColor = Color.Gray;
            GenerateContextMenuAtMsgBox();

            timerUpdateRedGreenLights.Interval = 200;
            timerUpdateRedGreenLights.Elapsed += OnTimedEventUpdateRedGreenLight;
            timerUpdateRedGreenLights.Enabled = false;

            timerDealWithCapHeight.Interval = 200;
            timerDealWithCapHeight.Elapsed += OnTimedEventDealWithCapHeight;
            timerDealWithCapHeight.Enabled = false;

            timerSafeHeightMonitor.Interval = 500;
            timerSafeHeightMonitor.Elapsed += OnTimerEventSafeHeightMonitor;
            timerSafeHeightMonitor.Enabled = false;

            miControlMode.Enabled = false;
            miFunction.Enabled = false;
            miSetting.Enabled = true;
            miCalibration.Enabled = false;
            miDebug.Enabled = false;
            
            //更新高度扫描方式
            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
            if (info != null) {
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.HEIGHT_SCAN_MODE, info.HeightScanMode, (key, oldValue) => info.HeightScanMode);
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.MONIT_TEST_CONDITION, info.MonitTestCondition, (key, oldValue) => info.MonitTestCondition);
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_SEPERATE_HEIGHT, info.ChuckSeperateHeight, (key, oldValue) => info.ChuckSeperateHeight);
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POSITION_SAFE, info.ProbeWaferContactZ0, (key, oldValue) => info.ProbeWaferContactZ0);
            }                       
        }

        private void miExit_Click(object sender, EventArgs e) {
            var dialogResult = MessageBox.Show(frc.GetString("txtSureToExit"), rc.GetString("txtConfirm"), MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.OK) {
                this.Close();
            }
        }

        private (bool isOk, EnumLanguage guiLanguage) GetGuiLanguage() {
            try {
                XElement xeRoot = XElement.Load(@"Configuration/config.xml");
                XElement xeLanguage = xeRoot.Element("language");
                string strLanguage = xeLanguage.Value.Trim();
                if (string.IsNullOrEmpty(strLanguage)) {
                    LOGGER.Error($"Configuration/config.xml, <language></language>, {rc.GetString("txtContentShouldNotBeEmpty")}");
                    return (false, EnumLanguage.ENGLISH);
                }
                string strLanguageUpper = strLanguage.ToUpperInvariant();
                if (!Enum.TryParse(strLanguageUpper, out EnumLanguage enumLanguage)) {
                    List<string> supportedLanguageList = new List<string>();
                    foreach (EnumLanguage language in Enum.GetValues(typeof(EnumLanguage))) {
                        supportedLanguageList.Add(language.ToString());
                    }
                    string allSupportedLanguages = string.Join(",", supportedLanguageList);
                    LOGGER.Error($"Configuration/config.xml, <language>{strLanguage}</language>, {rc.GetString("txtIsNotSupported!")} {frc.GetString("txtAllSupportedLanguagesAre")} [{allSupportedLanguages}]");
                    return (false, EnumLanguage.ENGLISH);
                }

                return (true, enumLanguage);
            } catch (Exception ex) {
                MyLogUtility.GenerateExceptionLog(ex);
                return (false, EnumLanguage.ENGLISH);
            }
        }

        private void ShowCamera(ConcurrentDictionary<string, object> shareObject) {
            formCamera = new FormCamera(shareObject);
            formCamera.ReportMessage = this.ReportMessage;
            formCamera.Show();
            
            //多屏显示
            Screen[] screens = Screen.AllScreens;
            if (screens.Length > 1) {
                foreach (Screen screen in screens) {
                    if (!screen.Primary) {
                        formCamera.Left = screen.WorkingArea.Location.X;
                        formCamera.Top = screen.WorkingArea.Location.Y;

                        //移动到副屏
                        Rectangle SceenArea = Screen.GetWorkingArea(formCamera);
                        formCamera.Left += SceenArea.Width / 2 - formCamera.Width / 2;
                        formCamera.Top += SceenArea.Height / 2 - formCamera.Height / 2;
                        formCamera.WindowState = FormWindowState.Maximized;
                        //frmCamera.WindowState = FormWindowState.Normal;
                    }
                }
            }
        }

        private void MiMsgBoxClear_Click(object sender, EventArgs e) {
            rtbMsgBox.Clear();
        }

        private void MiMsgBoxCopy_Click(object sender, EventArgs e) {
            rtbMsgBox.Copy();
        }

        private void rtbMsgBox_TextChanged(object sender, EventArgs e) {
            rtbMsgBox.SelectionStart = rtbMsgBox.Text.Length;
            rtbMsgBox.ScrollToCaret();
        }

        private void miAbout_Click(object sender, EventArgs e) {
            var assembly = Assembly.GetExecutingAssembly();

            var attributeCompany = (AssemblyCompanyAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute));
            string company = attributeCompany.Company;
            var attributeProduct = (AssemblyProductAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyProductAttribute));
            string product = attributeProduct.Product;
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true);
            string copyright = string.Empty;
            if (attributes.Length > 0) {
                var attribute = attributes[0] as AssemblyCopyrightAttribute;
                copyright = attribute.Copyright;
            }
            copyright = $"{copyright} 2019-{DateTime.Now.Year}";
            string softwareVersion = assembly.GetName().Version.ToString();

            var aboutInfo = new Dictionary<EnumAboutInfo, string> {
                { EnumAboutInfo.PRODUCT, product },
                { EnumAboutInfo.COPYRIGHT, copyright },
                { EnumAboutInfo.SOFTWARE_VERSION, softwareVersion }
            };

            FormAbout form = new FormAbout(frc.Language, aboutInfo);
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }

        private async void miInitialize_Click(object sender, EventArgs e) {
            string message = string.Empty;
            miInitialize.Enabled = false;           

            List<string> initializationSteps = new List<string> {
                frc.GetString("txtInitCommonConfig"),
                frc.GetString("txtInitInstruments"),
                frc.GetString("txtInitMotionStages"),
                frc.GetString("txtInitStageJogs"),
                frc.GetString("txtInitCoupling"),
                frc.GetString("txtInitRequests"),
                frc.GetString("txtInitQueries"),
                frc.GetString("txtInitGui")
            };
            FormInitializationProgress form = new FormInitializationProgress(frc.Language, initializationSteps, Properties.Resources.right_arrow);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();

            (bool isOk, string errorText) initResult;
            bool result = true;
            int index = 0;
            try {
                initResult = await Task.Run(() => {
                    index = 0;
                    InitializingConfiguration initConfig = new InitializingConfiguration(configBoard, SetGuiControlMode);
                    form.BeginRow(index);
                    result = initConfig.Run();
                    if (result) {
                        message = $"{frc.GetString("txtInitCommonConfig")} {frc.GetString("txtIsSuccessful")}";
                        ReportMessage(message);
                        LOGGER.Info(message);
                    } else {
                        message = $"{frc.GetString("txtInitCommonConfig")} {frc.GetString("txtIsFailed")}";
                        ReportMessage(message);
                        LOGGER.Error(message);
                        return (false, message);
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(0.5));
                    form.EndRow(index, true);

                    index = 1;
                    form.BeginRow(index);
                    result = initializingInstruments.Run();
                    if (result) {
                        message = $"{frc.GetString("txtInitInstruments")} {frc.GetString("txtIsSuccessful")}";
                        ReportMessage(message);
                        LOGGER.Info(message);
                        sharedObjects.AddOrUpdate(SharedObjectKey.ALL_INSTRUMENTS, initializingInstruments.MyInstrumentsUtility.Instruments, (key, oldValue) => initializingInstruments.MyInstrumentsUtility.Instruments);
                        sharedObjects.AddOrUpdate(SharedObjectKey.INSTRUMENTS_USAGE_LIST, initializingInstruments.MyInstrumentsUtility.InstrumentsUsages, (key, oldValue) => initializingInstruments.MyInstrumentsUtility.InstrumentsUsages);
                        InstrumentUsageIdValidChecker checker = new InstrumentUsageIdValidChecker(initializingInstruments.MyInstrumentsUtility.InstrumentsUsages);
                        sharedObjects.AddOrUpdate(SharedObjectKey.INSTRUMENT_USAGE_ID_VALID_CHECKER, checker, (key, oldValue) => checker);
                        sharedObjects.AddOrUpdate(SharedObjectKey.HAS_CONNECTED_INSTRUMENTS, true, (key, oldValue) => true);                        
                    } else {
                        message = $"{frc.GetString("txtInitInstruments")} {frc.GetString("txtIsFailed")}";
                        ReportMessage(message);
                        LOGGER.Error(message);
                        return (false, message);
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(0.5));
                    form.EndRow(index, true);

                    if (!UpdateTecStatus(out message)) {
                        return (false, message);
                    }

                    index = 2;
                    form.BeginRow(index);
                    result = motionStageInitializer.Run();
                    if (result) {
                        message = $"{frc.GetString("txtInitMotionStages")} {frc.GetString("txtIsSuccessful")}";
                        ReportMessage(message);
                        LOGGER.Info(message);
                        sharedObjects.AddOrUpdate(SharedObjectKey.STAGE_LIST, motionStageInitializer.stageUtility.StageList, (key, oldValue) => motionStageInitializer.stageUtility.StageList);
                        sharedObjects.AddOrUpdate(SharedObjectKey.STAGE_AXIS_USAGE_DICT, motionStageInitializer.stageAxisUsageUtility.StageAxisUsages, (key, oldValue) => motionStageInitializer.stageAxisUsageUtility.StageAxisUsages);
                        List<AbstractMotionController> motionControllerList = motionStageInitializer.motionControllerDict.Values.ToList();
                        foreach (AbstractMotionController motionController in motionControllerList) {
                            if (motionController.Vendor.Equals(EnumMotionControllerVendor.LEISAI.ToString()) && motionController.Model.Equals(EnumLeisaiModel.DMC3000.ToString())) {
                                sharedObjects.AddOrUpdate(SharedObjectKey.LEISAI_DM3000_INSTANCE, motionController, (key, oldValue) => motionController);
                                break;
                            }
                        }
                        AxisUsageIdValidChecker checker = new AxisUsageIdValidChecker(motionStageInitializer.stageAxisUsageUtility.StageAxisUsages);
                        sharedObjects.AddOrUpdate(SharedObjectKey.AXIS_USAGE_ID_VALID_CHECKER, checker, (key, oldValue) => checker);

                        UpdateStageSpeed(motionStageInitializer.stageAxisUsageUtility.StageAxisUsages);
                    } else {
                        message = $"{frc.GetString("txtInitMotionStages")} {frc.GetString("txtIsFailed")}";
                        ReportMessage(message);
                        LOGGER.Error(message);
                        return (false, message);
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(0.5));
                    form.EndRow(index, true);

                    index = 3;
                    form.BeginRow(index);
                    result = stageJogInitializer.Initialize();
                    if (result) {
                        message = $"{frc.GetString("txtInitStageJogs")} {frc.GetString("txtIsSuccessful")}";
                        ReportMessage(message);
                        LOGGER.Info(message);
                    } else {
                        message = $"{frc.GetString("txtInitStageJogs")} {frc.GetString("txtIsFailed")}";
                        ReportMessage(message);
                        LOGGER.Error(message);
                        return (false, message);
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(0.5));
                    form.EndRow(index, true);

                    index = 4;
                    form.BeginRow(index);
                    InitializingConfigCoupling initConfigCoupling = new InitializingConfigCoupling(sharedObjects);
                    result = initConfigCoupling.Run();
                    if (result) {
                        message = $"{frc.GetString("txtInitCoupling")} {frc.GetString("txtIsSuccessful")}";
                        ReportMessage(message);
                        LOGGER.Info(message);
                    } else {
                        message = $"{frc.GetString("txtInitCoupling")} {frc.GetString("txtIsFailed")}";
                        ReportMessage(message);
                        LOGGER.Error(message);
                        return (false, message);
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(0.5));
                    form.EndRow(index, true);

                    index = 5;
                    form.BeginRow(index);
                    InitializingRequest initConfigRequest = new InitializingRequest(sharedObjects);
                    result = initConfigRequest.Run();
                    if (result) {
                        message = $"{frc.GetString("txtInitRequests")} {frc.GetString("txtIsSuccessful")}";
                        ReportMessage(message);
                        LOGGER.Info(message);
                    } else {
                        message = $"{frc.GetString("txtInitRequests")} {frc.GetString("txtIsFailed")}";
                        ReportMessage(message);
                        LOGGER.Error(message);
                        return (false, message);
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(0.5));
                    form.EndRow(index, true);

                    index = 6;
                    form.BeginRow(index);
                    InitializingQuery initializingQuery = new InitializingQuery(sharedObjects);
                    result = initializingQuery.Run();
                    if (result) {
                        this.allSupportedQueries.Clear();
                        this.allSupportedQueries.AddRange(initializingQuery.AllSupportedQueries);
                        message = $"{frc.GetString("txtInitQueries")} {frc.GetString("txtIsSuccessful")}";
                        ReportMessage(message);
                        LOGGER.Info(message);
                    } else {
                        message = $"{frc.GetString("txtInitQueries")} {frc.GetString("txtIsFailed")}";
                        ReportMessage(message);
                        LOGGER.Error(message);
                        form.EndRow(index, false);
                        return (false, message);
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(0.5));
                    form.EndRow(index, true);

                    index = 7;
                    form.BeginRow(index);
                    result = InitializeGui();
                    if (result) {
                        message = $"{frc.GetString("txtInitGui")} {frc.GetString("txtIsSuccessful")}";
                        ReportMessage(message);
                        LOGGER.Info(message);
                    } else {
                        message = $"{frc.GetString("txtInitGui")} {frc.GetString("txtIsFailed")}";
                        ReportMessage(message);
                        LOGGER.Error(message);
                        form.EndRow(index, false);
                        return (false, message);
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(0.5));
                    form.EndRow(index, true);

                    //[gyh]: 待重构，放到更合适位置！
                    if (!TryCreateCouplingRawDataDirectory()) {
                        message = frc.GetString("txtFailedToCreateCouplingRawDataDir");
                        ReportMessage(message);
                        LOGGER.Error(message);
                        return (false, message);
                    }                    

                    return (true, string.Empty);
                });
            } finally {
                if (!result) {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
                form.Close();
            }

            if (initResult.isOk) {
                var authenticateResult = Authenticate();
                if (!authenticateResult.isOk) {
                    MessageBox.Show($"{frc.GetString("txtAuthenticationIsFailed")}\n{rc.GetString("txtLookupLog")}");
                    this.Close();
                }
                if (authenticateResult.canceledAuthentication) {
                    MessageBox.Show($"{frc.GetString("txtAuthenticationIsCanceled")}\n{frc.GetString("txtProgramWillBeEnded")}");
                    this.Close();
                    return;
                }

                serverSide = new MyServerSideImpl(frc.Language, sharedObjects, SetGuiEnableStatus);
                serverSide.AttachReportMessage(ReportMessage);

                slblLoginTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                MessageBox.Show(frc.GetString("txtAuthenticationIsPassed"));

                miFunction.Enabled = true;
                miControlMode.Enabled = true;
                miSetting.Enabled = true;
                miCalibration.Enabled = true;
                miDebug.Enabled = true;

                if (waferHandle.NeedHoming())
                {
                    SetUiState(false);
                    MessageBox.Show("机台重启后需要回零");
                }

                ShowCamera(sharedObjects);                

                TryBeginListening();                
            } else {
                MessageBox.Show(initResult.errorText, rc.GetString("txtError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="stageAxisUsages"></param>
        private void UpdateStageSpeed(Dictionary<string, StageAxis> stageAxisUsages) {
            double minVal = 0;
            double maxVal = 0;
            double accVal = 0;
            foreach (var one in stageAxisUsages) {                
                string key = one.Key;
                StageAxis value = one.Value;
                value.GetAxisSpeed(ref minVal, ref maxVal, ref accVal);
                value.SetStageSpeedConfig(minVal, maxVal, accVal);
            }
        }

        private bool UpdateTecStatus(out string message) {
            message = string.Empty;

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out object tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            var getResult2 = GetInstrument("chuck_temperature");            

            //获取TEC状态
            if (!GetTecStatus(1, out bool isOn))  {
                message = "获取TEC状态失败";
                ReportMessage(message);
                return false;
            }

            //获取温度设置值
            if(!GetTecTempSet(1, out double tempOmron)) {
                message = "获取TEC设置温度失败";
                ReportMessage(message);
                return false;
            }
            GetChuckTemp(tempOmron, out double temperature);

            string tecStatue = "OFF";
            if (isOn) {
                tecStatue = "ON";               
            } 

            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.TEC_STATUS, tecStatue, (key, OldValue) => tecStatue);
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.TEC_TEMPERATURE,temperature.ToString(), (key, OldValue) => temperature.ToString());        
            return true;    
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

        public void GetChuckTemp(double tempOmron,out double tempChuck) {
            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
            if (info != null) {
                tempChuck = tempOmron * info.TecCoeffK - info.TecCoeffB;
                tempChuck = Math.Round(tempChuck, 1);
            } else {
                tempChuck = tempOmron;
            }
        }

        private bool GetTecTempSet(int port, out double temperature)
        {
            temperature = 0;
            for (int i = 0; i < 5; i++)
            {
                if (tec.GetTempSet(port, out temperature))
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

        private bool TryCreateCouplingRawDataDirectory() {
            string couplingRawDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CouplingRawData");
            try {
                if (!Directory.Exists(couplingRawDataPath)) {
                    Directory.CreateDirectory(couplingRawDataPath);
                }

                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        private void TryBeginListening() {
            configBoard.TryGetValue(ConfigKey.CONTROL_MODE, out object tempObj1);
            string strControlMode = tempObj1 as string;
            Enum.TryParse(strControlMode, out EnumControlMode enumControlMode);
            if (enumControlMode == EnumControlMode.LOCAL_ONLY) {
                return;
            }

            configBoard.TryGetValue(ConfigKey.LISTENING_IP, out object tempObj2);
            configBoard.TryGetValue(ConfigKey.LISTENING_PORT, out object tempObj3);
            string strIpV4 = tempObj2 as string;
            int port = (int)tempObj3;
            try {
                serverSide.Listen(strIpV4, port, 1);
            } catch (SocketException se) {
                if ((se.SocketErrorCode != SocketError.Interrupted) && (se.SocketErrorCode != SocketError.OperationAborted)) {
                    ReportMessage($"{frc.GetString("txtListeningOccurredSocketException")}\n{rc.GetString("txtLookupLog")}");
                }
                LOGGER.Error($"Listening(ip={strIpV4},port={port}) occurred exception: {se.Message}, SocketErrorCode={se.SocketErrorCode}");
                SetGuiControlMode(EnumControlMode.LOCAL_ONLY.ToString());
                return;
            } catch (Exception e) {
                ReportMessage($"{frc.GetString("txtListeningOccurredException")}\n{rc.GetString("txtLookupLog")}");
                LOGGER.Error($"Listening(ip={strIpV4},port={port}) occurred exception: {e.Message}");
                SetGuiControlMode(EnumControlMode.LOCAL_ONLY.ToString());
                return;
            }
        }

        private void ReportMessage(string message) {
            this.BeginInvoke(new Action(() => {                
                string info = Environment.NewLine + $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] : {message}";
                rtbMsgBox.AppendText(info);
                AddTestInfo(info);

                int rows = rtbMsgBox.Lines.Length;
                if (rows > 1000) {
                    rtbMsgBox.Clear();
                }                
            }));
        }

        private void AddTestInfo(string message) {
            string logPath = $"log\\{DateTime.Now.ToString("yyyyMMdd")}_TestProcessInfo.txt";
            System.IO.File.AppendAllText(logPath, message);
        }

        private void MessageBoxShow(string message)
        {
            this.BeginInvoke(new Action(() =>
            {
                MessageBox.Show(message);   
            }));
        }

        private (bool isOk, bool canceledAuthentication) Authenticate() {
            FormAuthenticate formAuthenticate = new FormAuthenticate(frc.Language);
            var initResult = formAuthenticate.Initialize();
            if (!initResult.isOk) {
                return (false, false);
            }

            if (!initResult.enabled) {
                slblUser.Text = frc.GetString("txtDefaultUser");
                slblUser.Text = frc.GetString("txtDefaultRole");
                return (true, false);
            }

            formAuthenticate.StartPosition = FormStartPosition.CenterParent;
            DialogResult dr = formAuthenticate.ShowDialog();
            if (dr == DialogResult.OK) {
                if (string.IsNullOrEmpty(formAuthenticate.UserName)) {
                    slblUser.Text = frc.GetString("txtDefaultUser");
                } else {
                    slblUser.Text = formAuthenticate.UserName;
                }
                slblRole.Text = formAuthenticate.Role;
                hasAuthenticated = true;
                return (true, false);
            } else {
                return (true, true);
            }
        }

        private void GenerateContextMenuAtMsgBox() {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem miCopy = new MenuItem(frc.GetString("txtCopy"));
            miCopy.Click += MiMsgBoxCopy_Click;
            contextMenu.MenuItems.Add(miCopy);

            MenuItem miClear = new MenuItem(frc.GetString("txtClear"));
            miClear.Click += MiMsgBoxClear_Click;
            contextMenu.MenuItems.Add(miClear);

            rtbMsgBox.ContextMenu = contextMenu;
        }

        private void SetGuiEnableStatus(bool isEnabled) {
            this.BeginInvoke(new Action(() => {
                miFile.Enabled = isEnabled;
                //miFunction.Enabled = isEnabled;
                miOpenCamera.Enabled = true;
                miControlInstrument.Enabled = isEnabled;
                miSampleRecord.Enabled = isEnabled;
                miStageHoming.Enabled = isEnabled;
                miEditTestPosition.Enabled = isEnabled; 
                miOtherSetting.Enabled = isEnabled; 

                miCalibration.Enabled = isEnabled;  

                //tabControl_WaferMotion.Enabled = isEnabled;
                panelWaferMap.Enabled = isEnabled;
                foreach(Control ctl in tabPage_positionCali.Controls) {
                    FormPositionCali formPositionCali = ctl as FormPositionCali;
                    formPositionCali.EnableGUI(isEnabled);
                }
                panelRealTimeDisplay.Enabled = isEnabled;
                panelCoupling.Enabled = isEnabled;
                panelStageJogs.Enabled = isEnabled;
            }));
        }

        private void SetGuiControlMode(string controlMode) {
            this.BeginInvoke(new Action(() => {
                slblControlMode.Text = controlMode;
            }));
        }

        private void miControlMode_Click(object sender, EventArgs e) {
            FormSettingControlMode form = new FormSettingControlMode(frc.Language, sharedObjects, serverSide);
            form.StartPosition = FormStartPosition.CenterParent;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.ShowDialog();
        }

        private void miGuiLanguage_Click(object sender, EventArgs e)
        {
            FormSettingGuiLanguage form = new FormSettingGuiLanguage(frc.Language, sharedObjects);
            form.StartPosition = FormStartPosition.CenterParent;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.ShowDialog();
        }

        private bool InitializeGui() {
            //显示Stage
            InitializingConfigurationGui initializeConfigurationGui = new InitializingConfigurationGui(sharedObjects, stageJogInitializer.stageJogUtility, panelStageJogs);
            if (!initializeConfigurationGui.Run()) {
                return false;
            }

            InitializeGuiRedGreenLightBoard();
            InitializeWaferMapArea();
            InitializeCouplingMonitoringArea();
            InitializeCouplingArea();
            InitializeHeightMonitoringArea();
            InitOtheDisplayMonitorArea();

            return true;
        }

        private void InitializeHeightMonitoringArea()
        {
            FormHeightDisplay form = new FormHeightDisplay(sharedObjects);
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            this.BeginInvoke(new Action(() => {
                panelHeightDisplay.Controls.Add(form);
                form.Show();
            }));            
        }

        private void InitOtheDisplayMonitorArea() {
            FormOtherDisplay form = new FormOtherDisplay(sharedObjects);
            form.AbortCommunication = this.AbortCommunication;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            this.BeginInvoke(new Action(() => {
                panelOtherDisplay.Controls.Add(form);
                form.Show();
            }));
        }

        private void InitializeCouplingMonitoringArea() {
            FormCouplingFeedbackMonitoring form = new FormCouplingFeedbackMonitoring(frc.Language, sharedObjects);
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            this.BeginInvoke(new Action(() => {
                panelRealTimeDisplay.Controls.Add(form);
                form.Show();
            }));
        }

        private void InitializeGuiRedGreenLightBoard() {
            FormRedGreenLightBoard form = new FormRedGreenLightBoard(sharedObjects, Properties.Resources.RedLight, Properties.Resources.GreenLight);
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            this.BeginInvoke(new Action(() => {
                panelRedGreenLightBoard.Controls.Add(form);
                form.Show();
            }));

            IO_Info = ConfigMgr.LoadIoInfo();
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.IO_INFO, IO_Info, (key, oldValue) => IO_Info);

            sharedObjects.TryGetValue(SharedObjectKey.LEISAI_DM3000_INSTANCE, out object tempObj);
            motion = tempObj as MotionControllerLeisaiDmc3000;            

            //端口映射
            SetIOMap(motion);

            //IO状态
            SetIOStatus(motion);

            timerUpdateRedGreenLights.Enabled = true;
        }

        private void SetIOStatus(MotionControllerLeisaiDmc3000 motion)
        {
            //待机状态
            motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Red.ToString(), true);
            motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Green.ToString(), true);
            motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Yellow.ToString(), false);

            //相机光源打开
            motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Light.ToString(), false);

            //软件打开，默认晶圆的IO吸附是打开的，提示用户如果没有晶圆需要关闭
/*
            motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.SizeAll.ToString(), true);
            motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Chip.ToString(), true);
            motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size2.ToString(), true);
            motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size4.ToString(), true);
            motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size6.ToString(), true);
            motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size8.ToString(), true);

            motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size12.ToString(), false);
            motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.RfPlat.ToString(), false);
            motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.PdPlat.ToString(), false);      
*/
        }

        private void checkGasPressureStatus(MotionControllerLeisaiDmc3000 motion)
        {

        }

        private void SetIOMap(MotionControllerLeisaiDmc3000 motion)
        {                      
            //一旦触发，ChuckXYZ也不能动
            motion.SetChannelIoMap(0, 6, 0, 6, 0, 0.005);            
            motion.SetChannelIoMap(0, 7, 3, 6, 0, 0.005);
            motion.SetChannelIoMap(1, 6, 3, 6, 0, 0.005);
            //一旦触发，左FAXYZ也不能动
            motion.SetChannelIoMap(0, 0, 3, 6, 0, 0.005);
            motion.SetChannelIoMap(0, 1, 3, 6, 0, 0.005);
            motion.SetChannelIoMap(0, 2, 1, 6, 0, 0.005);
            //一旦触发，右FAXYZ也不能动
            motion.SetChannelIoMap(0, 3, 3, 6, 0, 0.005);
            motion.SetChannelIoMap(0, 4, 3, 6, 0, 0.005);
            motion.SetChannelIoMap(0, 5, 1, 6, 0, 0.005);

            ////左FAXYZ有效电平高
            motion.SetEmgMode(0, 0, 1, 1);
            motion.SetEmgMode(0, 1, 1, 1);
            motion.SetEmgMode(0, 2, 0, 1);
            //右FAXYZ有效电平高
            motion.SetEmgMode(0, 3, 1, 1);
            motion.SetEmgMode(0, 4, 1, 1);
            motion.SetEmgMode(0, 5, 0, 1);
            //ChuckXYZ有效电平高
            motion.SetEmgMode(0, 6, 0, 1);
            motion.SetEmgMode(0, 7, 1, 1);
            motion.SetEmgMode(1, 6, 1, 1);

        }

        private void OnTimerEventSafeHeightMonitor(object sender, ElapsedEventArgs e) {
            try {
                //
                double curPos = waferHandle.GetCurChuckPos();

                //安全高度预留30um余量（接触高度视觉偏差+晶圆平整度偏差+余量）
                sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_POSITION_SAFE, out object posSafe);
                double safePosValue = (double)posSafe - 30;

                bool isSafe = curPos < safePosValue ? true : false;
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POSITION_IS_SAFE, isSafe, (key, oldValue) => isSafe);

                //处理告警
                sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_POSITION_MONITOR_ENABLE, out object enable);
                bool enableMonitor = (bool)enable;
                if (enableMonitor && !isSafe)
                {
                    waferHandle.DealWithChuckSafePos(true);
                }
                else
                {
                    waferHandle.DealWithChuckSafePos(false);
                }

            } catch(Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
            }        
        }

        private void OnTimedEventDealWithCapHeight(object sender, ElapsedEventArgs e) {
            try {
                double leftHeight = 0;
                double rightHeight = 0;
                double HeightLimitPosLeft = 0;
                double HeightLimitPosRight = 0;
                double heightStopThresh = 0;
                string temp = string.Empty;
                bool isLeftLock = false;
                bool isRightLock = false;

                EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
                if (info == null) {
                    return;
                }

                HeightLimitPosLeft = info.CapAltLockHeightLeft;
                HeightLimitPosRight = info.CapAltLockHeightRight;
                heightStopThresh = info.CapAltAdjustStopThresh;

                sharedObjects.TryGetValue(PrivateSharedObjectKey.LEFT_CAP_HEIGHT, out var tempObj);
                if (tempObj != null) {
                    temp = tempObj as string;
                    HeightLimitPosLeft = Convert.ToDouble(temp) - heightStopThresh -20;
                }

                sharedObjects.TryGetValue(PrivateSharedObjectKey.LEFT_CAP_ALTIMETER_REALTIME, out tempObj);
                if (tempObj != null) {
                    string leftH = tempObj.ToString();
                    leftHeight = Convert.ToDouble(leftH);
                    if (leftHeight < HeightLimitPosLeft) {
                        //Chuck和左右六轴Z轴急停
                        waferHandle.DealWithCapAliAlarm(true);
                        string errInfo = $"Left Cap Altimeter Base Height{leftH},Current {leftHeight} StopLimit{HeightLimitPosLeft}";
                        ReportMessage(errInfo);
                        LOGGER.Error(errInfo);
                        isLeftLock = true;
                    } else {
                        isLeftLock = false;
                    }
                }

                sharedObjects.TryGetValue(PrivateSharedObjectKey.RIGHT_CAP_HEIGHT, out tempObj);
                if (tempObj != null) {
                    temp = tempObj as string;
                    HeightLimitPosRight = Convert.ToDouble(temp) - heightStopThresh - 20;
                }

                sharedObjects.TryGetValue(PrivateSharedObjectKey.RIGHT_CAP_ALTIMETER_REALTIME, out tempObj);
                if (tempObj != null) {
                    string rightH = tempObj.ToString();
                    rightHeight = Convert.ToDouble(rightH);
                    if (rightHeight < HeightLimitPosRight) {
                        //Chuck和左右六轴Z轴急停
                        waferHandle.DealWithCapAliAlarm(true);
                        string errInfo = $"Right Cap Altimeter Base Height{rightH},Current {rightHeight} StopLimit{HeightLimitPosRight}";
                        ReportMessage(errInfo);
                        LOGGER.Error(errInfo);
                        isRightLock = true;
                    } else {
                        isRightLock = false;
                    }
                }

                if ((!isRightLock) && (!isLeftLock)) {
                    waferHandle.DealWithCapAliAlarm(false);
                }
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
            }
        }

        private void OnTimedEventUpdateRedGreenLight(object sender, ElapsedEventArgs e)
        {
            List<string> keys = MyRedGreenLigthKey.GetAllKeys();
            bool value = false;                     
            
            foreach (string key in keys)
            {
                if (key == MyRedGreenLigthKey.FA_SENSOR)
                {
                    value = !motion.ReadInput("0", "0");
                }
                else if (key == MyRedGreenLigthKey.DIE_CHUCK_VACCUM)
                {
                    value = motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.Chip.ToString());
                }
                else if (key == MyRedGreenLigthKey.WAFER_CHUCK_VACCUM)
                {
                    value = motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.Size8.ToString());
                    value |= motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.Size2.ToString());
                    value |= motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.Size4.ToString());
                    value |= motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.Size6.ToString());
                    value |= motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.Size12.ToString());
                }
                else if (key == MyRedGreenLigthKey.AUXILIARY_TABLE_VACCUM)
                {
                    value = !motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.RfPlat.ToString());
                }
                else if (key == MyRedGreenLigthKey.INPUT_PRESURE_VACCUM) {
                    value = !motion.ReadInput(IO_Info.CardIndex.ToString(), IO_Info.VaccumInput.ToString());
                    value &= !motion.ReadInput(IO_Info.CardIndex.ToString(), IO_Info.PressureInput2.ToString());
                    value &= !motion.ReadInput(IO_Info.CardIndex.ToString(), IO_Info.PressureInput1.ToString());
                }
                else if (key == MyRedGreenLigthKey.SAFE_HEIGHT_CHUCK)
                {
                    sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_POSITION_IS_SAFE, out object isSafe);
                    value = (bool)isSafe;
                }
                else if (key == MyRedGreenLigthKey.TEC_ONOFF)
                {
                    sharedObjects.TryGetValue(PrivateSharedObjectKey.TEC_STATUS, out object status);
                    string tecStatus = status as string;
                    value = (tecStatus == "ON");                   
                }
                else
                {
                    continue;
                }

                redGreenLightBoard.AddOrUpdateLight(key, value);
            }
            
        }

        private void InitializeCouplingArea() {
            FormCoupling form = new FormCoupling(frc.Language, sharedObjects);
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            this.BeginInvoke(new Action(() => {
                panelCoupling.Controls.Add(form);
                form.Show();
            }));
        }
        
        private void InitializeWaferMapArea() {
            this.BeginInvoke(new Action(() => {
                ctlWafer_Main.InitWaferTypeList(ConfigMgr.GetWaferMapNames());
                ctlWafer_Main.ChangeMode(false);                                      
                panelWaferMap.Controls.Add(ctlWafer_Main);

                waferHandle = new WaferManual(sharedObjects);
                waferHandle.ReportMessage = this.ReportMessage;
                waferHandle.MessageBoxShow = this.MessageBoxShow;
                waferHandle.SetSelectedDie = ctlWafer_Main.SetSelectedDie;
                waferHandle.SetSelectedDieIndexByHome = ctlWafer_Main.SetSelectedDieIndexByHome;
                waferHandle.SetDieHighLight = ctlWafer_Main.SetDieHighLight;                
                waferHandle.SetDieHighLightWithIndex = ctlWafer_Main.SetDieHighLightWithIndex;
                waferHandle.SetDieHighLightWithIndexByHome = ctlWafer_Main.SetDieHighLightWithIndexByHome;
                waferHandle.SetDieReference = ctlWafer_Main.SetDieReference;
                waferHandle.SetDieReferenceWithIndex = ctlWafer_Main.SetDieReferenceWithIndex;
                ctlWafer_Main.UploadWafer = waferHandle.DoWaferUpload;
                ctlWafer_Main.DownloadWafer = waferHandle.DoWaferDownload;
                ctlWafer_Main.MoveToMarkPos = waferHandle.MoveToMarkPos;
                ctlWafer_Main.HeightVerify = waferHandle.DoWaferHeightVerify;
                ctlWafer_Main.ParentFormName = this.Text;

                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.WAFER_HANDLE, waferHandle, (key, oldValue) => waferHandle);

                FormPositionCali form = new FormPositionCali(sharedObjects);
                form.TopLevel = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;
                form.GetSelectedDie = ctlWafer_Main.GetSelectedDie;
                this.BeginInvoke(new Action(() => {
                    tabPage_positionCali.Controls.Add(form);
                    form.Show();
                }));

                timerDealWithCapHeight.Enabled = true;
                timerSafeHeightMonitor.Enabled = true;
            }));            
        }

        private void ToolStripMenuItem_OpenCamera_Click(object sender, EventArgs e) {
            if (formCamera == null || formCamera.IsDisposed) {
                formCamera = new FormCamera(sharedObjects);
                formCamera.ReportMessage = this.ReportMessage;
                formCamera.Show();

                Screen[] screens = Screen.AllScreens;
                if (screens.Length > 1) {
                    foreach (Screen screen in screens) {
                        if (!screen.Primary) {
                            formCamera.Left = screen.WorkingArea.Location.X;
                            formCamera.Top = screen.WorkingArea.Location.Y;

                            //移动到副屏
                            Rectangle SceenArea = Screen.GetWorkingArea(formCamera);
                            formCamera.Left += SceenArea.Width / 2 - formCamera.Width / 2;
                            formCamera.Top += SceenArea.Height / 2 - formCamera.Height / 2;
                            formCamera.WindowState = FormWindowState.Maximized;
                            //frmCamera.WindowState = FormWindowState.Normal;
                        }
                    }
                }
            } else {
                if (formCamera.WindowState == FormWindowState.Minimized) {
                    formCamera.WindowState = FormWindowState.Normal;
                }
                formCamera.Activate();
            }
        }
		
		private void FormMain_FormClosing(object sender, FormClosingEventArgs e) {
            if (miInitialize.Enabled) {
                e.Cancel = false;
                return;
            }

            if (hasAuthenticated) {
                DialogResult dr = MessageBox.Show(frc.GetString("txtSureToExit"), rc.GetString("txtConfirm"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.Cancel) {
                    e.Cancel = true;
                } else {
                    e.Cancel = false;
                }
            }
        }

        public const int WM_GOTO = 0xF112;
        public const int WM_AXIS_MOVE = 0xF200;
        protected override void DefWndProc(ref Message m){
            switch (m.Msg) {
                case WM_GOTO: 
                    DieGotoProcess();
                    break;
                case WM_AXIS_MOVE:
                    StageMoveInfo moveInfo = (StageMoveInfo)Marshal.PtrToStructure(m.LParam, typeof(StageMoveInfo));
                    CcdMoveWithChuck(moveInfo);
                    break;
                default:                    
                    base.DefWndProc(ref m);
                    break;                   
            }
        }

        private void CcdMoveWithChuck(StageMoveInfo moveInfo)
        {
            string axisId = moveInfo.AxisName;
            int moveDir = moveInfo.dir;
            double distance = moveInfo.distance;

            //1运动的是Chuck的Z轴
            if (axisId == "chuck_stage_z")
            {
                //2使能了关联
                sharedObjects.TryGetValue(PrivateSharedObjectKey.CCD_GOWITH_CHUCK, out object tempObj);
                bool isMoveTogether = (bool)tempObj;
                if (isMoveTogether)
                {
                    waferHandle.moveCcd(moveDir * distance);
                }                
            }
        }

        private void DieGotoProcess()
        {
            WaferMapInfo map = ConfigMgr.LoadWaferMapInfoByType(ctlWafer_Main.GetWaferType());
            if (map == null)
            {
                MessageBox.Show(this, "请选择一个晶圆类型", "Info:");
                return;
            }

            var selectDie = ctlWafer_Main.GetSelectedDie();
            var preDie = ctlWafer_Main.GetPreSelectedDie();

            if ((preDie == null) || (selectDie == null))
            {
                MessageBox.Show(this, "请选择一个Reticle", "Info:");
                return;
            }

            waferHandle.MoveToNextReticle(selectDie, preDie, map);
        }

        private void ToolStripMenuItem_ControlInstrument_Click(object sender, EventArgs e) {
            if (frmInstrumentDlg == null || frmInstrumentDlg.IsDisposed) {
                frmInstrumentDlg = new FormInstrumentControl(sharedObjects);
                frmInstrumentDlg.Show();
            }  else  {
                if (frmInstrumentDlg.WindowState == FormWindowState.Minimized) {
                    frmInstrumentDlg.WindowState = FormWindowState.Normal;
                }
                frmInstrumentDlg.Activate();
            }
        }

        private void ToolStripMenuItem_SampleRecord_Click(object sender, EventArgs e) {
            if (frmDebug == null || frmDebug.IsDisposed) {                
                frmDebug = new FormDebug(sharedObjects);
                frmDebug.Show();
            } else {
                if (frmDebug.WindowState == FormWindowState.Minimized) {
                    frmDebug.WindowState = FormWindowState.Normal;
                }
                frmDebug.Activate();
            }                
        }

        private void ToolStripMenuItem_waferMapping_Click(object sender, EventArgs e) {
            if (frmWaferMapping == null || frmWaferMapping.IsDisposed) {
                frmWaferMapping = new FormMapping(sharedObjects);
                frmWaferMapping.Show();

                frmWaferMapping.UpdateWaferTypeList = UpdateWaferTypeList;
            } else {
                if (frmWaferMapping.WindowState == FormWindowState.Minimized) {
                    frmWaferMapping.WindowState = FormWindowState.Normal;
                }
                frmWaferMapping.Activate();
            }                
        }

        private void ToolStripMenuItem_WaferMoveTest_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_stageHoming_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要设备初始化吗？在确认没有干涉或者光纤拉扯的后点击确定，开始机台回零。", "提示：", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
            if (info == null)
            {
                MessageBox.Show($"未找到机台校准文件");
                return;
            }
            if(info.HomeType == "1") {
                waferHandle.DoHoming(true);
            }
            else {
                waferHandle.DoHoming(false);
            }

            SetUiState(true);
        }
		

        public void SetUiState(bool isEnable)
        {
            this.BeginInvoke(new Action(() =>
            {
                miWaferMapping.Enabled = isEnable;
                miSystemCalibration.Enabled = isEnable;
                miSampleRecord.Enabled = isEnable;
                tabControl_WaferMotion.Enabled = isEnable;
                panelStageJogs.Enabled = isEnable;
                panelCoupling.Enabled = isEnable;
            }));
        }

        private void UpdateWaferTypeList() {
            ctlWafer_Main.InitWaferTypeList(ConfigMgr.GetWaferMapNames());
        }

        private void ToolStripMenuItem_systemCalibration_Click(object sender, EventArgs e) {
            /*
            if (frmCalibration == null || frmCalibration.IsDisposed) {
                frmCalibration = new FormCalibration(sharedObjects, ctlWafer_Main.GetWaferType());
                frmCalibration.Show();
            } else {
                if (frmCalibration.WindowState == FormWindowState.Minimized) {
                    frmCalibration.WindowState = FormWindowState.Normal;
                }
                frmCalibration.Activate();  
            }
            */
            frmCalibration = new FormCalibration(sharedObjects, ctlWafer_Main.GetWaferType());
            frmCalibration.Show();
        }

        private void AbortCommunication() {
            FormSettingControlMode form = new FormSettingControlMode(frc.Language, sharedObjects, serverSide);
            form.SwitchToLocalOnly(false);
        }

        private readonly ConcurrentDictionary<string, object> sharedObjects = new ConcurrentDictionary<string, object>();
        private MyServerSide serverSide;

        private readonly List<AbstractQuery> allSupportedQueries = new List<AbstractQuery>();
        //private readonly QueryBoard queryBoard = new QueryBoard();
        private readonly RequestStatusBoard requestStatusBoard = new RequestStatusBoard();
        private readonly ConcurrentDictionary<string, object> configBoard = new ConcurrentDictionary<string, object>();
        private readonly RedGreenLightBoard redGreenLightBoard = new RedGreenLightBoard();

        private readonly MyInstruments.MyInitialization.InitializingInstruments initializingInstruments = new MyInstruments.MyInitialization.InitializingInstruments();
        private readonly MyMotionStageDriver.MyInitialization.InitializingMotionStages motionStageInitializer = new MyMotionStageDriver.MyInitialization.InitializingMotionStages();
        private MyMotionStageUserControl.MyUtility.MyInitialization stageJogInitializer;

        private FormCamera formCamera = null;
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        private readonly FormResourceCulture frc;
        private readonly ResourceCulture rc;

        private bool hasAuthenticated = false;
        private ControlWafer ctlWafer_Main = new ControlWafer();
        private WaferManual waferHandle = null;

        //FormIOControl frmIODlg = null;
        FormInstrumentControl frmInstrumentDlg = null; 
        FormDebug frmDebug = null;
        FormMapping frmWaferMapping = null;
        FormCalibration frmCalibration = null;
        FormLeadscrewCompensate frmLeadComp = null;
        FormLeadScrewCali frmLeadCali = null;
        FormOtherSetting frmOthersetting = null;
        FormSpecialPositionEdit frmSpecialPositionEdit = null;
        private readonly System.Timers.Timer timerUpdateRedGreenLights = new System.Timers.Timer();
        private readonly System.Timers.Timer timerDealWithCapHeight = new System.Timers.Timer();
        private readonly System.Timers.Timer timerSafeHeightMonitor = new System.Timers.Timer();
        private MotionControllerLeisaiDmc3000 motion;
        private IOInfo IO_Info = new IOInfo();
        private List<InstrumentUsage> instrumentUsageList;
        private Dictionary<string, Instrument> instruments;
        private StandaloneTec tec;

        private void miMoveDut_Click(object sender, EventArgs e) {
#if false
            Task.Run(() => {
                for (int i = 0; i < 10000; i++) {
                    waferHandle.RecordHeightInfo();
                    Thread.Sleep(100);
                }
            });            
#endif
            RequestMoveToDut request = new RequestMoveToDut(sharedObjects);

            FormMoveToDutTest frm = new FormMoveToDutTest();
            if (frm.ShowDialog() != DialogResult.Yes)
            {
                string parameter = $"{frm.dutName},{frm.subDieName}";
                var result = request.TryUpdateParameters(parameter);
                if (result.isOk)
                {
                    var runResult = request.Run();
                    if (runResult.responseId != (int)EnumResponseId.PASS)
                    {
                        MessageBox.Show($"运动失败:{runResult.runResult}");
                    }
                }
                else
                {
                    MessageBox.Show($"运动失败:{result.errorMessage}");
                }
            }            
        }

        private void miLeadScewCompensate_Click(object sender, EventArgs e)
        {
            if (frmLeadComp == null || frmLeadComp.IsDisposed)
            {
                frmLeadComp = new FormLeadscrewCompensate();
                frmLeadComp.Show();
            }
            else
            {
                if (frmLeadComp.WindowState == FormWindowState.Minimized)
                {
                    frmLeadComp.WindowState = FormWindowState.Normal;
                }
                frmLeadComp.Activate();
            }
        }

        private void mileadScrewCalibration_Click(object sender, EventArgs e) {
            
            if (frmLeadCali == null || frmLeadCali.IsDisposed) {
                frmLeadCali = new FormLeadScrewCali(sharedObjects);
                frmLeadCali.Show();
            } else {
                if (frmLeadCali.WindowState == FormWindowState.Minimized) {
                    frmLeadCali.WindowState = FormWindowState.Normal;
                }
                frmLeadCali.Activate();
            }
        }

        private void stageStopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            waferHandle.StopAllAxis();
        }

        private void otherSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            if (frmOthersetting == null || frmOthersetting.IsDisposed)
            {
                frmOthersetting = new FormOtherSetting(sharedObjects);
                frmOthersetting.Show();
            }
            else
            {
                if (frmOthersetting.WindowState == FormWindowState.Minimized)
                {
                    frmOthersetting.WindowState = FormWindowState.Normal;
                }
                frmOthersetting.Activate();
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

        private void setTestPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmSpecialPositionEdit == null || frmSpecialPositionEdit.IsDisposed) {
                frmSpecialPositionEdit = new FormSpecialPositionEdit(sharedObjects);
                frmSpecialPositionEdit.Show();
            } else {
                if (frmSpecialPositionEdit.WindowState == FormWindowState.Minimized) {
                    frmSpecialPositionEdit.WindowState = FormWindowState.Normal;
                }
                frmSpecialPositionEdit.Activate();
            }
        }

        private void tabControl_WaferMotion_TabIndexChanged(object sender, EventArgs e) {
            ;
        }
    }
}
