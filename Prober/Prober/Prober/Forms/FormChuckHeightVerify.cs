using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonApi.MyUtility;
using MyInstruments;
using MyInstruments.MyAltimeter;
using MyInstruments.MyEnum;
using MyInstruments.MyUtility;
using MyMotionStageDriver.MyStageAxis;
using NLog;
using Prober.Constant;
using Prober.WaferDef;
using ProberApi.MyConstant;

namespace Prober.Forms
{
    public partial class FormChuckHeightVerify : Form
    {
        StandaloneAm altimeter;
        StandaloneAm altimeterLD;
        StandaloneAm altimeterCap;

        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        private readonly Dictionary<string, Instrument> instruments;
        private readonly List<InstrumentUsage> instrumentUsageList;
        private readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        private readonly Dictionary<string, StageAxis> stageAxisDic = new Dictionary<string, StageAxis>();
        string CurrentWaferType = string.Empty;

        public FormChuckHeightVerify(ConcurrentDictionary<string, object> sharedObjects, string CurWaferType)
        {
            InitializeComponent();

            CurrentWaferType = CurWaferType;

            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            stageAxisUsages = tempObj as Dictionary<string, StageAxis>;

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            //获取仪表
            var getResult2 = GetInstrument("ld_altimeter");
            getResult2 = GetInstrument("cap_altimeter");

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
                    if (instrumentUsage.InstrumentId == "altimeter_ld")
                    {
                        altimeterLD = instrument as StandaloneAm;
                        altimeter = altimeterLD;
                    }
                    else if (instrumentUsage.InstrumentId == "altimeter_cap")
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

        private void btn_Sample_1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string index = btn.Name.Split('_')[2];

            var lblX2 = panel_HeightVerify.Controls["lbl_ChuckX_" + index] as Label;
            var lblY2 = panel_HeightVerify.Controls["lbl_ChuckY_" + index] as Label;
            var lblH = panel_HeightVerify.Controls["lbl_Height_" + index] as Label;

            lblX2.Text = stageAxisDic[MyStageAxisKey.CHUCK_X].Position().ToString("f2");
            lblY2.Text = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position().ToString("f2");

            if (!GetHeight(Cap_Altimeter_Channel.Left_Channel, out double height))
            {
                MessageBox.Show(this, "高度采集失败", "Info:");
                return;
            }

            lblH.Text = height.ToString("f2");
            MessageBox.Show(this, "采样成功。", "Info:");
        }

        private bool GetHeight(int channel, out double Value)
        {
            Value = double.NaN;

            for (int i = 0; i < 5; i++)
            {
                if (altimeter.GetHeight(channel, out Value))
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

        private void chb_AmType_Cap_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_AmType_Cap.Checked == true)
            {
                altimeter = altimeterCap;
            }
            else
            {
                altimeter = altimeterLD;
            }
        }

        public void SetLRAxisSpeedToNormal()
        {
            double maxSpeed = stageAxisDic[MyStageAxisKey.LEFT_X].speedMaxConfig;
            double minSpeed = stageAxisDic[MyStageAxisKey.LEFT_X].speedMinConfig;
            double accTime = stageAxisDic[MyStageAxisKey.LEFT_X].accConfig;
            stageAxisDic[MyStageAxisKey.LEFT_X].SetAxisSpeed(minSpeed, maxSpeed, accTime);

            maxSpeed = stageAxisDic[MyStageAxisKey.LEFT_Y].speedMaxConfig;
            minSpeed = stageAxisDic[MyStageAxisKey.LEFT_Y].speedMinConfig;
            accTime = stageAxisDic[MyStageAxisKey.LEFT_Y].accConfig;
            stageAxisDic[MyStageAxisKey.LEFT_Y].SetAxisSpeed(minSpeed, maxSpeed, accTime);

            maxSpeed = stageAxisDic[MyStageAxisKey.RIGHT_X].speedMaxConfig;
            minSpeed = stageAxisDic[MyStageAxisKey.RIGHT_X].speedMinConfig;
            accTime = stageAxisDic[MyStageAxisKey.RIGHT_X].accConfig;
            stageAxisDic[MyStageAxisKey.RIGHT_X].SetAxisSpeed(minSpeed, maxSpeed, accTime);

            maxSpeed = stageAxisDic[MyStageAxisKey.RIGHT_Y].speedMaxConfig;
            minSpeed = stageAxisDic[MyStageAxisKey.RIGHT_Y].speedMinConfig;
            accTime = stageAxisDic[MyStageAxisKey.RIGHT_Y].accConfig;
            stageAxisDic[MyStageAxisKey.RIGHT_Y].SetAxisSpeed(minSpeed, maxSpeed, accTime);
        }

        public void MoveAllToSafePos(EquipmentCalibrationInfo calInfo)
        {
            SetLRAxisSpeedToNormal();

            //相机退回到安全位置
            var t0 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(calInfo.Safe_CcdZ);
            });

            var t8 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(calInfo.Safe_ChuckZ);
            });
            Task.WaitAll(new Task[] { t0, t8 });

            stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

            //测高仪退回到安全位置
            var t1 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.HEIGHT_U].MoveAbsolute(calInfo.Safe_U);
            });
            //左右六轴Z1/Z3
            var t2 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.LEFT_Z].MoveAbsolute(calInfo.Safe_LeftZ);
            });
            var t3 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveAbsolute(calInfo.Safe_RightZ);
            });
            Task.WaitAll(new Task[] { t1, t2, t3 });
            stageAxisDic[MyStageAxisKey.HEIGHT_U].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();

            //左右六轴X1/Y1/X3/Y3
            var t4 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.LEFT_X].MoveAbsolute(calInfo.Safe_LeftX);
            });
            var t5 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.LEFT_Y].MoveAbsolute(calInfo.Safe_LeftY);
            });
            var t6 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.RIGHT_X].MoveAbsolute(calInfo.Safe_RightX);
            });
            var t7 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.RIGHT_Y].MoveAbsolute(calInfo.Safe_RightY);
            });
            Task.WaitAll(new Task[] { t4, t5, t6, t7 });
            stageAxisDic[MyStageAxisKey.LEFT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Y].GuiUpdatePosition();
        }


        private void btn_MoveToInitPos_Click(object sender, EventArgs e)
        {
            Task.Run(() => {
                try {
                    var equipmentInfo = ConfigMgr.LoadEquipmentCalibration();
                    MoveAllToSafePos(equipmentInfo);

                    stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipmentInfo.ChuckZ);
                    stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

                    //激光测高仪需要移动
                    if (!chb_AmType_Cap.Checked) {
                        stageAxisDic[MyStageAxisKey.HEIGHT_U].MoveAbsolute(equipmentInfo.HSenserWorkU);
                        stageAxisDic[MyStageAxisKey.HEIGHT_U].GuiUpdatePosition();
                        Invoke(new Action(() => { MessageBox.Show(this, "高度校准位置初始化成功！", "提示："); }));
                    } else {
                        Invoke(new Action(() => { MessageBox.Show(this, "请将电容测高仪移动到Wafer中心位置！", "提示："); }));
                    }
                }
                catch(Exception ex) {
                    Invoke(new Action(() => { MessageBox.Show(this, $"高度校准位置初始化异常:{ex.Message}！", "提示："); }));
                }
            });
            
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            ChuckHeightVerifyInfo info = ConfigMgr.LoadHeightVerifyInfoByType(CurrentWaferType);
            if (info == null) {
                MessageBox.Show("请选择晶圆类型");
                return;
            }

            //检查当前高度是否满足要求
            if (!checkHeightInfo()) {
                return;
            }

            info.WaferType = CurrentWaferType;
            UIClass.ControlToObject(panel_HeightVerify, info);
            ConfigMgr.SaveHeightVerifyInfo(info);
            MessageBox.Show(this, "高度校准位置保存成功！", "提示：");            
        }

        private bool checkHeightInfo()
        {
            double h1 = Math.Round(Convert.ToDouble(lbl_Height_1.Text),2);
            double h2 = Math.Round(Convert.ToDouble(lbl_Height_2.Text), 2);
            double h3 = Math.Round(Convert.ToDouble(lbl_Height_3.Text), 2);
            double h4 = Math.Round(Convert.ToDouble(lbl_Height_4.Text), 2);
            double h5 = Math.Round(Convert.ToDouble(lbl_Height_5.Text), 2);
            double threshold = Convert.ToDouble(lbl_Height_5.Text);
            List<double> data = new List<double> { h1, h2, h3, h4, h5};
            double maxData = data.Max();
            double minData = data.Min();

            if (Math.Abs(maxData - minData) > threshold) {
                MessageBox.Show($"5个基准位置之间高度偏差超过门限{threshold}");
                return false;
            }

            return true;
        }

        private void btn_Load_Click(object sender, EventArgs e)
        {
            ChuckHeightVerifyInfo info = ConfigMgr.LoadHeightVerifyInfoByType(CurrentWaferType);
            if (info != null)
            {
                UIClass.ObjectToControl(info, panel_HeightVerify);
                MessageBox.Show(this, "高度校准位置加载完成！", "提示：");
            }
            else
            {
                MessageBox.Show("请选择晶圆类型");
                return;
            }
        }

        private void FormChuckHeightVerify_Load(object sender, EventArgs e)
        {
            ChuckHeightVerifyInfo info = ConfigMgr.LoadHeightVerifyInfoByType(CurrentWaferType);
            if (info != null)
            {
                UIClass.ObjectToControl(info, panel_HeightVerify);
            }
        }
    }
}
