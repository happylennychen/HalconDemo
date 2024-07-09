using CommonApi.MyUtility;
using MyInstruments;
using MyInstruments.MyAltimeter;
using MyInstruments.MyEnum;
using MyInstruments.MyUtility;
using MyMotionStageDriver.MyStageAxis;
using NLog;
using Prober.Constant;
using Prober.Request;
using Prober.WaferDef;
using ProberApi.MyConstant;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prober.Forms
{
    public partial class FormHeightDisplay : Form
    {
        StandaloneAm altimeterCap;
        private readonly Dictionary<string, Instrument> instruments;
        private readonly List<InstrumentUsage> instrumentUsageList;
        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        private readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        private readonly Dictionary<string, StageAxis> stageAxisDic = new Dictionary<string, StageAxis>();
        private WaferMapInfo map;
        private HCalibrationInfo heightInfo;
        private EquipmentCalibrationInfo equipInfo;
        ConcurrentDictionary<string, object> sharedObjects;

        public FormHeightDisplay(ConcurrentDictionary<string, object> sharedObjects)
        {
            InitializeComponent();

            this.sharedObjects = sharedObjects; 

            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            stageAxisUsages = tempObj as Dictionary<string, StageAxis>;

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            //获取仪表
            var getResult2 = GetInstrument("cap_altimeter");

            //获取轴
            GetStageAxisDic();
        }

        public bool GetStageAxisDic()
        {
            List<string> axisUseList = new List<string>();
            axisUseList.Add(MyStageAxisKey.LEFT_X);
            axisUseList.Add(MyStageAxisKey.LEFT_Y);
            axisUseList.Add(MyStageAxisKey.LEFT_Z);
            axisUseList.Add(MyStageAxisKey.LEFT_SX);
            axisUseList.Add(MyStageAxisKey.LEFT_SY);
            axisUseList.Add(MyStageAxisKey.LEFT_SZ);

            axisUseList.Add(MyStageAxisKey.RIGHT_X);
            axisUseList.Add(MyStageAxisKey.RIGHT_Y);
            axisUseList.Add(MyStageAxisKey.RIGHT_Z);
            axisUseList.Add(MyStageAxisKey.RIGHT_SX);
            axisUseList.Add(MyStageAxisKey.RIGHT_SY);
            axisUseList.Add(MyStageAxisKey.RIGHT_SZ);

            axisUseList.Add(MyStageAxisKey.CCD_X);
            axisUseList.Add(MyStageAxisKey.CCD_Y);
            axisUseList.Add(MyStageAxisKey.CCD_Z);
            axisUseList.Add(MyStageAxisKey.HEIGHT_U);

            axisUseList.Add(MyStageAxisKey.CHUCK_X);
            axisUseList.Add(MyStageAxisKey.CHUCK_Y);
            axisUseList.Add(MyStageAxisKey.CHUCK_Z);
            axisUseList.Add(MyStageAxisKey.CHUCK_SZ);

            for (int i = 0; i < axisUseList.Count; i++)
            {
                var result = GetStageAxis(axisUseList[i]);
                if (!result.isOK)
                {
                    return false;
                }
                stageAxisDic.Add(axisUseList[i], result.stageAxis);
            }

            return true;
        }

        internal (bool isOK, string errorMessage, StageAxis stageAxis) GetStageAxis(string axisUsageId)
        {
            string errorText = string.Empty;
            string INVALID_PARAMETERS = $"input parameters(={axisUsageId}) is invalid";

            if (!stageAxisUsages.ContainsKey(axisUsageId))
            {
                errorText = $"{INVALID_PARAMETERS}";
                LOGGER.Error(errorText);
                return (false, errorText, null);
            }

            return (true, null, stageAxisUsages[axisUsageId]);
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
                case EnumInstrumentCategory.ALTIMETER:
                    if (instrumentUsage.InstrumentId == "altimeter_cap")
                    {
                        altimeterCap = instrument as StandaloneAm;
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

        

        private Tuple<double, double> CalFAAndWaferDistance()
        {
            equipInfo = ConfigMgr.LoadEquipmentCalibration();

            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_TYPE, out object value);
            if (value == null) {
                return null;
            }
            string waferType = value as string;

            sharedObjects.TryGetValue(PrivateSharedObjectKey.HEIGHT_SCAN_MODE, out  value);
            string ScanMode = value as string;

            bool useFixPoint = ScanMode == "1" ? true:false;

            heightInfo = ConfigMgr.LoadHCalibration(waferType, ScanMode);
            if (heightInfo == null || heightInfo.Points.Count < 1) {
                return null;
            }

            map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            if (heightInfo == null || equipInfo == null || map == null) {
                return null;
            }
            
            double chuckX = stageAxisDic[MyStageAxisKey.CHUCK_X].Position();
            double chuckY = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position();
            double checkZ = stageAxisDic[MyStageAxisKey.CHUCK_Z].Position();
            double LeftZ = stageAxisDic[MyStageAxisKey.LEFT_Z].Position();          
            heightInfo.GetHeight(chuckX, chuckY, out double h1, useFixPoint);
            heightInfo.GetHeight(map.Height_ChuckX_Left, map.Height_ChuckY_Left, out double baseH, useFixPoint);
            double base_disLeft = map.Height_LeftZ;
            //double wafer_delta = equipInfo.ChuckZ - checkZ;
            double wafer_delta = equipInfo.ProbeWaferContactZ0 + equipInfo.ProbeCrimpingDepth - checkZ;
            double res_disLeft = (LeftZ - base_disLeft) - (h1 - baseH) + wafer_delta;

            double res_disRight = 99999;
            double RightZ = stageAxisDic[MyStageAxisKey.RIGHT_Z].Position();
            double base_disRight = map.Height_RightZ;
            res_disRight = (RightZ - base_disRight) - (h1 - baseH) + wafer_delta;

            Tuple<double, double> res = new Tuple<double, double>(res_disLeft, res_disRight);
            return res;
        }

        private void FormHeightDisplay_Load(object sender, EventArgs e)
        {
#if true
            //创建任务，监控和显示高度信息
            Task.Factory.StartNew(() => 
            {
                try
                {
                    while (true)
                    {
                        //读取通道1和2的电容测高仪高度信息
                        double hegihtCapLeft = 9999;
                        double hegihtCapRight = 9999;
                        double heightLaserLeft = 9999;
                        double heightLaserRight = 9999;

                        if (altimeterCap != null)
                        {
                            if (!GetHeight(Cap_Altimeter_Channel.Left_Channel, out hegihtCapLeft))
                            {
                                Invoke(new Action(() => { MessageBox.Show("Left 测高仪读取失败"); }));
								LOGGER.Error("高度监控程序 Left 测高仪读取失败");
                            }
                            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.LEFT_CAP_ALTIMETER_REALTIME, hegihtCapLeft, (key, oldValue) => hegihtCapLeft);
                            
                            if (!GetHeight(Cap_Altimeter_Channel.Right_Channel, out hegihtCapRight))
                            {
                                Invoke(new Action(() => { MessageBox.Show("Right 测高仪读取失败"); }));
                                LOGGER.Error("高度监控程序 Right 测高仪读取失败");
                            }
                            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.RIGHT_CAP_ALTIMETER_REALTIME, hegihtCapRight, (key, oldValue) => hegihtCapRight);
                        }
                        
                        Tuple<double, double> disTuple = CalFAAndWaferDistance();
                        if (disTuple != null)
                        {
                            heightLaserLeft = Math.Round(disTuple.Item1,2);
                            heightLaserRight = Math.Round(disTuple.Item2,2);
                        }
                        
                        Invoke(new Action(() =>
                        {
                            txt_HeightLeft_Cap.Text = hegihtCapLeft.ToString("F2");
                            txt_HeightRight_Cap.Text = hegihtCapRight.ToString("F2");
                            txt_HeightLeft_Laser.Text = heightLaserLeft.ToString("F2");
                            txt_HeightRight_Laser.Text = heightLaserRight.ToString("F2");
                        }));

                        Thread.Sleep(200);
                    }
                }
                catch(Exception ex) 
                {
                    Invoke(new Action(() => { MessageBox.Show($"高度监控异常:{ex.Message}"); }));                    
                    LOGGER.Error($"高度监控程序 高度监控异常:{ex.Message}");
                }                              
            });
#endif
        }

        private bool GetHeight(int channel, out double Value)
        {
            Value = double.NaN;

            for (int i = 0; i < 5; i++)
            {
                if (altimeterCap.GetHeight(channel, out Value))
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
    }
}
