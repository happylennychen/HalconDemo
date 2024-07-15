using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using CommonApi.MyUtility;
using MyMotionStageDriver.MyStageAxis;
using Prober.Constant;
using System.Timers;
using MyInstruments.MyTec;
using MyInstruments.MyAltimeter;
using MyInstruments.MyEnum;
using MyInstruments.MyOpm;
using MyInstruments.MyUtility;
using MyInstruments;
using System.Threading;
using Prober.WaferDef;
using MathNet.Numerics.Providers.LinearAlgebra;
using Prober.Request;
using ProberApi.MyConstant;
using MyInstruments.MyLed;

namespace Prober.Forms
{
    public partial class FormOtherDisplay : Form
    {
        ConcurrentDictionary<string, object> sharedObject;
        private readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        public readonly Dictionary<string, StageAxis> stageAxisDic = new Dictionary<string, StageAxis>();
        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        private readonly System.Timers.Timer timerUpdateRedGreenLights = new System.Timers.Timer();
        StandaloneTec tecChuck;
        StandaloneLed led;
        private readonly List<InstrumentUsage> instrumentUsageList;
        private readonly Dictionary<string, Instrument> instruments;
        private double tecCoeffK = 1.24495;
        private double tecCoeffB = 7.0927;
        List<string> axisUseList = new List<string>();
        public Action AbortCommunication;

        public FormOtherDisplay(ConcurrentDictionary<string, object> sharedObjects)
        {
            InitializeComponent();

            sharedObject = sharedObjects;

            //创建定时任务，刷新当前温度状态，周期0.5秒
            //timerUpdateRedGreenLights.Interval = 500;
            //timerUpdateRedGreenLights.Elapsed += OnTimerRefreshTemperatureInfo;
            //timerUpdateRedGreenLights.Enabled = true;

            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            stageAxisUsages = tempObj as Dictionary<string, StageAxis>;

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            //获取轴
            GetStageAxisDic();
            //获取Chuck
            //var getResult2 = GetInstrument("chuck_temperature");
            //getResult2 = GetInstrument("led_chuck");

            GetCoeff();
        }

        public void GetCoeff()
        {
            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
            if (info != null)
            {
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
                        tecChuck = instrument as StandaloneTec;
                    }                   
                    else
                    {
                        errorText = $"{instrumentUsage.InstrumentId.ToString()} is not a valid instrument id of temperature stable monitor!";
                        LOGGER.Error(errorText);
                        return (false, errorText, null);
                    }
                    break;
                case EnumInstrumentCategory.LED:
                    if (instrumentUsage.InstrumentId == "led_chuck")
                    {
                        led = instrument as StandaloneLed;
                    }
                    else
                    {
                        errorText = $"{instrumentUsage.InstrumentId.ToString()} is not a valid instrument id of led!";
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

        public bool GetStageAxisDic()
        {            
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

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            foreach (var stage in stageAxisDic)
            {
                stage.Value.Stop();
            }

            for(int i = 0; i < stageAxisDic.Count;i++) {
                stageAxisDic[axisUseList[i]].GuiUpdatePosition();
            }

            AbortCommunication();
        }

        private bool UpdateLedStatus(double data)
        {
            //data转换为整数
            int value = (int)(data * 10);
            return led.SetValue(value, 1);
        }

        private void OnTimerRefreshTemperatureInfo(object sender, ElapsedEventArgs e)
        {
            try
            {
                //刷新设置温度
                GetTemperature(1,out double tempOmron);
                tempOmron = Math.Round(tempOmron * tecCoeffK - tecCoeffB, 1);

                //刷新实际温度
                sharedObject.TryGetValue(PrivateSharedObjectKey.TEC_TEMPERATURE, out object value);
                string tempSet = value as string;

                this.BeginInvoke(new Action(() => {
                    txt_TempRead.Text = tempOmron.ToString();
                    txt_TempSet.Text = tempSet;
                    UpdateLedStatus(Convert.ToDouble(tempOmron));
                }));
            }
            catch (Exception ex)
            {
                LOGGER.Error(ex.ToString());
            }            
        }

        private bool GetTemperature(int channel, out double Value)
        {
            Value = double.NaN;

            for (int i = 0; i < 5; i++)
            {
                if (tecChuck.GetTemp(channel, out Value))
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

        private void FormOtherDisplay_Load(object sender, EventArgs e)
        {
            ;
        }

        private void chb_CCDGoWithChuck_CheckedChanged(object sender, EventArgs e)
        {
            bool checkState = chb_CCDGoWithChuck.Checked;   
            sharedObject.AddOrUpdate(PrivateSharedObjectKey.CCD_GOWITH_CHUCK, checkState, (key, oldValue) => checkState);
        }
    }
}
