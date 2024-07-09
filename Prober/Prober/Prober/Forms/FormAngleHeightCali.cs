using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyMotionStageDriver.MyStageAxis;
using Prober.WaferDef;
using NLog;
using CommonApi.MyUtility;
using System.Collections.Concurrent;
using ProberApi.MyConstant;
using Prober.Constant;
using System.Windows.Forms.DataVisualization.Charting;
using System.Dynamic;
using HalconDotNet;
using MyInstruments.MyCamera;
using System.Drawing.Drawing2D;
using MyInstruments.MyUtility;
using MyInstruments;
using MyInstruments.MyEnum;
using Prober.Request;

namespace Prober.Forms
{
    public partial class FormAngleHeightCali : Form
    {
        State MotionState = State.Ready;
        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        private readonly Dictionary<string, StageAxis> stageAxisDic = new Dictionary<string, StageAxis>();
        protected static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        string CurrentWaferType = string.Empty;

        private readonly Dictionary<string, Instrument> instruments;
        private readonly List<InstrumentUsage> instrumentUsageList;
        private StandaloneCamera camera;
        private ConcurrentDictionary<string, object> sharedObjects;

        public FormAngleHeightCali(ConcurrentDictionary<string, object> sharedObjects, string waferType)
        {
            InitializeComponent();

            this.sharedObjects = sharedObjects;

            CurrentWaferType = waferType;
            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            stageAxisUsages = tempObj as Dictionary<string, StageAxis>;
            //获取轴
            GetStageAxisDic();

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            var getResult2 = GetInstrument("top_camera");
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
                case EnumInstrumentCategory.CCD:
                    camera = instrument as StandaloneCamera;
                    break;
                default:
                    errorText = $"{instrumentUsage.InstrumentCategory.ToString()} is not a valid instrument category of coupling feedback!";
                    LOGGER.Error(errorText);
                    return (false, errorText, null);
            }

            return (true, string.Empty, instrument);
        }


        private void btn_Save_Click(object sender, EventArgs e)
        {
            //加载equipment，修改角度信息后保存
            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();            
            UIClass.ControlToObject(panel_AngleHeight, info);
            ConfigMgr.SaveEquipmentCalibration(info);             
        }

        private void btn_Load_Click(object sender, EventArgs e)
        {
            //加载equipment校准文件并显示          
            var map = ConfigMgr.LoadWaferMapInfoByType(CurrentWaferType);
            if (map == null)
            {
                MessageBox.Show($"未找到当前类型Wafer {CurrentWaferType} 的Map信息");
                return;
            }
            else
            {
                UIClass.ObjectToControl(map, panel_AngleHeight);
            }

            //加载wafermap文件并显示
            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
            if (info == null)
            {
                MessageBox.Show($"未找到机台校准文件");
                return;
            }
            else
            {
                UIClass.ObjectToControl(info, panel_AngleHeight);
            }
        }

        private void btn_GetPos_Work_SX1_Click(object sender, EventArgs e)
        {
            txt_BaseItem_LeftSX.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_SX].Position(), 5).ToString();
        }

        private void btn_GetPos_Work_SZ1_Click(object sender, EventArgs e)
        {
            txt_BaseItem_LeftSZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_SZ].Position(), 5).ToString();
        }

        private void btn_GetPos_Work_SY1_Click(object sender, EventArgs e)
        {
            txt_BaseItem_LeftSY.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_SY].Position(), 5).ToString();
        }

        private void btn_MoveTo_Work_SX1_Click(object sender, EventArgs e)
        {
            Func<bool> func = () =>
            {
                var equipCalibrate = ConfigMgr.LoadEquipmentCalibration();
                if (!IsAltimeterInSafePosition(equipCalibrate.Safe_U))
                {
                    MoveAllToSafePos(equipCalibrate);
                }
                else
                {
                    List<Task> task = new List<Task>();
                    //var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Z].MoveAbsolute(equipCalibrate.Safe_LeftZ); });
                    //task.Add(t1);
                    var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveAbsolute(equipCalibrate.Safe_RightZ); });
                    task.Add(t2);
                    Task.WaitAll(task.ToArray());
                    stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();
                    stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();

                    stageAxisDic[MyStageAxisKey.RIGHT_X].MoveAbsolute(equipCalibrate.Safe_RightX);
                    stageAxisDic[MyStageAxisKey.RIGHT_X].GuiUpdatePosition();
                }

                stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(equipCalibrate.Safe_CcdZ);
                stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
                stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.ChuckZ);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
                stageAxisDic[MyStageAxisKey.LEFT_Z].MoveAbsolute(equipCalibrate.LSX_LeftZ);
                stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();
                //移动到SX标记位置
                moveToLeftSXCaliPos(equipCalibrate);

                camera.SetExposure(equipCalibrate.LSX_Exp);

                stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(equipCalibrate.LSX_CcdZ);
                stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
                return true;
            };

            DoWorkFunc(func, "Left θX调整工位");
        }

        public void moveToLeftSXCaliPos(EquipmentCalibrationInfo info)
        {           
            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_X].MoveAbsolute(info.LSX_CcdX); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Y].MoveAbsolute(info.LSX_CcdY); });
            task.Add(t2);
            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_X].MoveAbsolute(info.LSX_LeftX); });
            task.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Y].MoveAbsolute(info.LSX_LeftY); });
            task.Add(t5);
            var t6 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(info.LSX_ChuckX); });
            task.Add(t6);
            var t7 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(info.LSX_ChuckY); });
            task.Add(t7);

            Task.WaitAll(task.ToArray());

            stageAxisDic[MyStageAxisKey.CCD_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Y].GuiUpdatePosition();           
            stageAxisDic[MyStageAxisKey.LEFT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
        }

        public void moveToLeftSYCaliPos(EquipmentCalibrationInfo info)
        {           
            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_X].MoveAbsolute(info.LSY_CcdX); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Y].MoveAbsolute(info.LSY_CcdY); });
            task.Add(t2);            
            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_X].MoveAbsolute(info.LSY_LeftX); });
            task.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Y].MoveAbsolute(info.LSY_LeftY); });
            task.Add(t5);
            var t6 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(info.LSY_ChuckX); });
            task.Add(t6);
            var t7 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(info.LSY_ChuckY); });
            task.Add(t7);
            Task.WaitAll(task.ToArray());

            stageAxisDic[MyStageAxisKey.CCD_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Y].GuiUpdatePosition();
            //stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
        }

        public void moveToLeftSZCaliPos(EquipmentCalibrationInfo info)
        {            
            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_X].MoveAbsolute(info.LSZ_CcdX); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Y].MoveAbsolute(info.LSZ_CcdY); });
            task.Add(t2);
            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_X].MoveAbsolute(info.LSZ_LeftX); });
            task.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Y].MoveAbsolute(info.LSZ_LeftY); });
            task.Add(t5);
            var t6 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(info.LSZ_ChuckX); });
            task.Add(t6);
            var t7 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(info.LSZ_ChuckY); });
            task.Add(t7);
            Task.WaitAll(task.ToArray());

            stageAxisDic[MyStageAxisKey.CCD_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Y].GuiUpdatePosition();
            //stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
        }

        public void moveToRightSZCaliPos(EquipmentCalibrationInfo info)
        {           
            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_X].MoveAbsolute(info.RSZ_CcdX); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Y].MoveAbsolute(info.RSZ_CcdY); });
            task.Add(t2);
            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_X].MoveAbsolute(info.RSZ_RightX); });
            task.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Y].MoveAbsolute(info.RSZ_RightY); });
            task.Add(t5);
            var t6 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(info.RSZ_ChuckX); });
            task.Add(t6);
            var t7 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(info.RSZ_ChuckY); });
            task.Add(t7);
            Task.WaitAll(task.ToArray());

            stageAxisDic[MyStageAxisKey.CCD_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Y].GuiUpdatePosition();           
            stageAxisDic[MyStageAxisKey.RIGHT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();

        }

        public void moveToRightSYCaliPos(EquipmentCalibrationInfo info)
        {            
            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_X].MoveAbsolute(info.RSY_CcdX); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Y].MoveAbsolute(info.RSY_CcdY); });
            task.Add(t2);
            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_X].MoveAbsolute(info.RSY_RightX); });
            task.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Y].MoveAbsolute(info.RSY_RightY); });
            task.Add(t5);
            var t6 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(info.RSY_ChuckX); });
            task.Add(t6);
            var t7 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(info.RSY_ChuckY); });
            task.Add(t7);
            Task.WaitAll(task.ToArray());

            stageAxisDic[MyStageAxisKey.CCD_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
        }

        public void moveToRightSXCaliPos(EquipmentCalibrationInfo info)
        {            
            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_X].MoveAbsolute(info.RSX_CcdX); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Y].MoveAbsolute(info.RSX_CcdY); });
            task.Add(t2);
            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_X].MoveAbsolute(info.RSX_RightX); });
            task.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Y].MoveAbsolute(info.RSX_RightY); });
            task.Add(t5);
            var t6 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(info.RSX_ChuckX); });
            task.Add(t6);
            var t7 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(info.RSX_ChuckY); });
            task.Add(t7);
            Task.WaitAll(task.ToArray());

            stageAxisDic[MyStageAxisKey.CCD_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();

        }

        private void btn_MoveTo_Work_SZ1_Click(object sender, EventArgs e)
        {
            Func<bool> func = () =>
            {
                var equipCalibrate = ConfigMgr.LoadEquipmentCalibration();
                if (!IsAltimeterInSafePosition(equipCalibrate.Safe_U))
                {
                    MoveAllToSafePos(equipCalibrate);
                }
                else
                {
                    List<Task> task = new List<Task>();
                    //var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Z].MoveAbsolute(equipCalibrate.Safe_LeftZ); });
                    //task.Add(t1);
                    var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveAbsolute(equipCalibrate.Safe_RightZ); });
                    task.Add(t2);
                    Task.WaitAll(task.ToArray());
                    stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();
                    stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();

                    stageAxisDic[MyStageAxisKey.RIGHT_X].MoveAbsolute(equipCalibrate.Safe_RightX);
                    stageAxisDic[MyStageAxisKey.RIGHT_X].GuiUpdatePosition();
                }

                stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(equipCalibrate.Safe_CcdZ);
                stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

                stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.ChuckZ);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

                stageAxisDic[MyStageAxisKey.LEFT_Z].MoveAbsolute(equipCalibrate.LSZ_LeftZ);
                stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();

                //移动到SX标记位置
                moveToLeftSZCaliPos(equipCalibrate);                

                camera.SetExposure(equipCalibrate.LSZ_Exp);

                stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(equipCalibrate.LSZ_CcdZ);                              
                stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

                return true;
            };

            DoWorkFunc(func, "Left θZ调整工位");
        }

        private void btn_MoveTo_Work_SY1_Click(object sender, EventArgs e)
        {
            Func<bool> func = () =>
            {
                var equipCalibrate = ConfigMgr.LoadEquipmentCalibration();
                if (!IsAltimeterInSafePosition(equipCalibrate.Safe_U))
                {
                    MoveAllToSafePos(equipCalibrate);
                }
                else
                {
                    List<Task> task = new List<Task>();
                    //var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Z].MoveAbsolute(equipCalibrate.Safe_LeftZ); });
                    //task.Add(t1);
                    var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveAbsolute(equipCalibrate.Safe_RightZ); });
                    task.Add(t2);
                    Task.WaitAll(task.ToArray());
                    stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();
                    stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();

                    stageAxisDic[MyStageAxisKey.RIGHT_X].MoveAbsolute(equipCalibrate.Safe_RightX);
                    stageAxisDic[MyStageAxisKey.RIGHT_X].GuiUpdatePosition();
                }

                stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(equipCalibrate.Safe_CcdZ);
                stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
                stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.ChuckZ);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
                stageAxisDic[MyStageAxisKey.LEFT_Z].MoveAbsolute(equipCalibrate.LSY_LeftZ);
                stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();

                //移动到SX标记位置
                moveToLeftSYCaliPos(equipCalibrate);

                camera.SetExposure(equipCalibrate.LSY_Exp);

                //Z方向移动               
                stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(equipCalibrate.LSY_CcdZ);
                stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

                return true;
            };

            DoWorkFunc(func, "Left θY调整工位");
        }

        private void btn_Work_AngleLeft_Done_Click(object sender, EventArgs e)
        {
            if (MotionState != State.Ready)
            {
                MessageBox.Show(this, "设备未准备好。", "Info:");
                return;
            }

            if (MessageBox.Show("确定要运动到Mark点位置吗？", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    MotionState = State.Busy;
                    MoveToMarkPosByWaferType(out string errMsg);
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show(this, "运动完成", "Info:");
                    }));
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show(this, $"运动异常:{ex.Message}", "Info:");
                    }));
                }
                finally
                {
                    MotionState = State.Ready;
                }

            });
        }

        public bool IsAltimeterInSafePosition(double safePos)
        {
            double curU = stageAxisDic[MyStageAxisKey.HEIGHT_U].Position();
            if (curU >= safePos && curU != 0)
            {
                return true;
            }

            return false;
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

        private void btn_GetPos_Work_SX3_Click(object sender, EventArgs e)
        {
            txt_BaseItem_RightSX.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_SX].Position(), 5).ToString();
        }

        private void btn_GetPos_Work_SZ3_Click(object sender, EventArgs e)
        {
            txt_BaseItem_RightSZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_SZ].Position(), 5).ToString();
        }

        private void btn_GetPos_Work_SY3_Click(object sender, EventArgs e)
        {
            txt_BaseItem_RightSY.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_SY].Position(), 5).ToString();
        }

        private void btn_MoveTo_Work_SX3_Click(object sender, EventArgs e)
        {
            Func<bool> func = () =>
            {
                var equipCalibrate = ConfigMgr.LoadEquipmentCalibration();
                if (!IsAltimeterInSafePosition(equipCalibrate.Safe_U))
                {
                    MoveAllToSafePos(equipCalibrate);
                }
                else
                {
                    List<Task> task = new List<Task>();
                    var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Z].MoveAbsolute(equipCalibrate.Safe_LeftZ); });
                    task.Add(t1);
                    //var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveAbsolute(equipCalibrate.Safe_RightZ); });
                    //task.Add(t2);
                    Task.WaitAll(task.ToArray());
                    stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();
                    stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();

                    stageAxisDic[MyStageAxisKey.LEFT_X].MoveAbsolute(equipCalibrate.Safe_LeftX);
                    stageAxisDic[MyStageAxisKey.LEFT_X].GuiUpdatePosition();
                }

                //Z方向移动
                stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(equipCalibrate.Safe_CcdZ);
                stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

                stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.ChuckZ);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

                stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveAbsolute(equipCalibrate.RSX_RightZ);
                stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();


                //移动到SX标记位置
                moveToRightSXCaliPos(equipCalibrate);
				
				 camera.SetExposure(equipCalibrate.RSX_Exp);

                stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(equipCalibrate.RSX_CcdZ);
                stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

                return true;
            };

            DoWorkFunc(func, "Right θX调整工位");
        }

        private void btn_MoveTo_Work_SY3_Click(object sender, EventArgs e)
        {
            Func<bool> func = () =>
            {
                var equipCalibrate = ConfigMgr.LoadEquipmentCalibration();
                if (!IsAltimeterInSafePosition(equipCalibrate.Safe_U))
                {
                    MoveAllToSafePos(equipCalibrate);
                }
                else
                {
                    List<Task> task = new List<Task>();
                    var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Z].MoveAbsolute(equipCalibrate.Safe_LeftZ); });
                    task.Add(t1);
                    //var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveAbsolute(equipCalibrate.Safe_RightZ); });
                    //task.Add(t2);
                    Task.WaitAll(task.ToArray());
                    stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();
                    stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();

                    stageAxisDic[MyStageAxisKey.LEFT_X].MoveAbsolute(equipCalibrate.Safe_LeftX);
                    stageAxisDic[MyStageAxisKey.LEFT_X].GuiUpdatePosition();
                }

                stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(equipCalibrate.Safe_CcdZ);
                stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

                //Z方向移动
                stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.ChuckZ);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

                stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveAbsolute(equipCalibrate.RSY_RightZ);
                stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();

                //移动到SX标记位置
                moveToRightSYCaliPos(equipCalibrate);

                camera.SetExposure(equipCalibrate.RSY_Exp);

                stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(equipCalibrate.RSY_CcdZ);
                stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

                return true;
            };

            DoWorkFunc(func, "Right θY调整工位");
        }

        private void btn_MoveTo_Work_SZ3_Click(object sender, EventArgs e)
        {
            Func<bool> func = () =>
            {
                var equipCalibrate = ConfigMgr.LoadEquipmentCalibration();
                if (!IsAltimeterInSafePosition(equipCalibrate.Safe_U))
                {
                    MoveAllToSafePos(equipCalibrate);
                }
                else
                {
                    List<Task> task = new List<Task>();
                    var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Z].MoveAbsolute(equipCalibrate.Safe_LeftZ); });
                    task.Add(t1);
                    //var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveAbsolute(equipCalibrate.Safe_RightZ); });
                    //task.Add(t2);
                    Task.WaitAll(task.ToArray());
                    stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();
                    stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();

                    stageAxisDic[MyStageAxisKey.LEFT_X].MoveAbsolute(equipCalibrate.Safe_LeftX);
                    stageAxisDic[MyStageAxisKey.LEFT_X].GuiUpdatePosition();
                }

                //Z方向移动
                stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(equipCalibrate.Safe_CcdZ);
                stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

                stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.ChuckZ);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

                stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveAbsolute(equipCalibrate.RSZ_RightZ);
                stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();

                //移动到SX标记位置
                moveToRightSZCaliPos(equipCalibrate);

                camera.SetExposure(equipCalibrate.RSZ_Exp);

                stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(equipCalibrate.RSZ_CcdZ);
                stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();               
                
                return true;
            };

            DoWorkFunc(func, "Right θZ调整工位");
        }

        private void btn_Work_AngleRight_Done_Click(object sender, EventArgs e)
        {
            if (MotionState != State.Ready)
            {
                MessageBox.Show(this, "设备未准备好。", "Info:");
                return;
            }

            if (MessageBox.Show("确定要运动到Mark点位置吗？", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    MotionState = State.Busy;
                    MoveToMarkPosByWaferType(out string errMsg);
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show(this, "运动完成", "Info:");
                    }));
                }
                catch(Exception ex)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show(this, $"运动异常:{ex.Message}", "Info:");
                    }));
                }
                finally
                {
                    MotionState = State.Ready;
                }

            });
        }

        public bool MoveToMarkPosByWaferType(out string errMsg)
        {
            WaferMapInfo map = ConfigMgr.LoadWaferMapInfoByType(CurrentWaferType);
            EquipmentCalibrationInfo equipCalibrate = ConfigMgr.LoadEquipmentCalibration();
            errMsg = string.Empty;            

            stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(equipCalibrate.Safe_CcdZ);
            stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

            //测高仪退回到安全位置//左右六轴Z1/Z3
            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.HEIGHT_U].MoveAbsolute(equipCalibrate.Safe_U); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Z].MoveAbsolute(equipCalibrate.Safe_LeftZ); });
            task.Add(t2);
            var t3 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveAbsolute(equipCalibrate.Safe_RightZ); });
            task.Add(t3);
            Task.WaitAll(task.ToArray());
            stageAxisDic[MyStageAxisKey.HEIGHT_U].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();

            //左右六轴X1/Y1/X3/Y3            
            List<Task> task2 = new List<Task>();
            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_X].MoveAbsolute(equipCalibrate.Safe_LeftX); });
            task2.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Y].MoveAbsolute(equipCalibrate.Safe_LeftY); });
            task2.Add(t5);
            var t6 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_X].MoveAbsolute(equipCalibrate.Safe_RightX); });
            task2.Add(t6);
            var t7 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Y].MoveAbsolute(equipCalibrate.Safe_RightY); });
            task2.Add(t7);
            Task.WaitAll(task2.ToArray());
            stageAxisDic[MyStageAxisKey.LEFT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Y].GuiUpdatePosition();

            //Z降 500
            //stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(-500);
            sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_SEPERATE_HEIGHT, out object seperateHeight);
            string sepLen = seperateHeight as string;
            double zMoveLen = Convert.ToDouble(sepLen);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(-1 * zMoveLen);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

            //X4/Y4/Z4/ChuckZ            

            List<Task> task3 = new List<Task>();
            var t8 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_X].MoveAbsolute(map.MarkCcdX); });
            task3.Add(t8);
            var t9 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Y].MoveAbsolute(map.MarkCcdY); });
            task3.Add(t9);
            var t10 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(map.MarkCcdZ); });
            task3.Add(t10);
            var t11 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(map.MarkChuckX); });
            task3.Add(t11);
            var t12 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(map.MarkChuckY); });
            task3.Add(t12);
            Task.WaitAll(task3.ToArray());
            stageAxisDic[MyStageAxisKey.CCD_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();

            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.ChuckZ); 
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

            return true;
        }

        private void FormAngleHeightCali_Load(object sender, EventArgs e)
        {
            var map = ConfigMgr.LoadWaferMapInfoByType(CurrentWaferType);
            if (map == null)
            {               
                foreach (Control item in panel_AngleHeight.Controls)
                {
                    item.Enabled = true;
                }
            }
            else
            {
                UIClass.ObjectToControl(map, panel_AngleHeight);
            }            

            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
            if (info != null)
            {
                UIClass.ObjectToControl(info, panel_AngleHeight);
            }            
        }

        private void DoWorkFunc(Func<bool> func, string workName)
        {
            if (MotionState != State.Ready)
            {
                MessageBox.Show(this, "设备未准备好。", "Info:");
                return;
            }
            if (MessageBox.Show("确定要开始运动吗？请注意当前探针位置。", "提示：", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }             

            Task.Run(() =>
            {
                try
                {
                    MotionState = State.Busy;
                    if (func())
                    {
                        Invoke(new Action(() =>{MessageBox.Show(this, $"{workName}运动完成", "Info:");}));

                    }
                    else
                    {
                        Invoke(new Action(() => {MessageBox.Show(this, $"{workName}运动失败", "Info:");}));
                    }
                }
                catch(Exception ex) 
                {
                    Invoke(new Action(() => { MessageBox.Show(this, $"{workName}运动异常:{ex.Message}", "Info:"); }));
                }
                finally
                {
                    MotionState = State.Ready;
                }                
            });
        }
    }
}
