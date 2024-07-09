using CommonApi.MyUtility;
using MyInstruments.MyUtility;
using MyInstruments;
using MyMotionStageDriver.MyStageAxis;
using NLog;
using Prober.Constant;
using Prober.WaferDef;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProberApi.MyConstant;
using System.Collections.Concurrent;
using Prober.Request;
using HalconDotNet;
using MyInstruments.MyAltimeter;
using MyInstruments.MyEnum;
using System.Threading;
using MyInstruments.MyCamera;
using System.IO;
using MathNet.Numerics;
using System.Runtime.Remoting.Messaging;

namespace Prober.Forms
{
    public partial class FormPositionCali : Form
    {
        State MotionState = State.Ready;
        public WaferManual waferHandle = null;
        private List<ItemCalPosInfo> TestDiesCalPos;
        public HCalibrationInfo HeightCalibrationInfo = new HCalibrationInfo();
        private readonly Dictionary<string, Instrument> instruments;
        private readonly List<InstrumentUsage> instrumentUsageList;

        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        private readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        private readonly Dictionary<string, StageAxis> stageAxisDic = new Dictionary<string, StageAxis>();

        Action<string> ReportMessage;
        public Func<DieInfo> GetSelectedDie;
        private ConcurrentDictionary<string, object> sharedObjects;
        public PlatCalibrate WaferMotionPlat { get; set; } = new PlatCalibrate("WaferMotion");
        StandaloneAm altimeterCap;
        private StandaloneCamera camera;
        string basicDieName = string.Empty; 

        public Dictionary<string, CompensateData> padCompensate = null;

        public FormPositionCali(ConcurrentDictionary<string, object> sharedObjects)
        {
            InitializeComponent();

            this.sharedObjects  = sharedObjects;
            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            stageAxisUsages = tempObj as Dictionary<string, StageAxis>;

            sharedObjects.TryGetValue(SharedObjectKey.ACTION_REPORT_MESSAGE, out tempObj);
            this.ReportMessage = tempObj as Action<string>;

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            //获取轴
            GetStageAxisDic();

            var getResult2 = GetInstrument("cap_altimeter");
            getResult2 = GetInstrument("top_camera");

            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_HANDLE, out tempObj);
            waferHandle = tempObj as WaferManual;
        }

        public void EnableGUI(bool enable) {
            panel_SubDieInfo.Enabled = enable;
            panel_AssistMark.Enabled = enable;
            panel_SubDieOrdinary.Enabled = enable;
            panel_LeftFA_HeightCali.Enabled = enable;
            panel_RightFA_HeightCali.Enabled = enable;
            panel_ProbeCard_Cali.Enabled = enable;                   
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
                case EnumInstrumentCategory.CCD:
                    camera = instrument as StandaloneCamera;
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

        private void btn_CalPosition_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckBeforeCalPos())
                {
                    return;
                }

                sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_TYPE, out object value);
                if (value == null)
                {
                    MessageBox.Show("WaferType is Null");
                    return;
                }
                string waferType = value as string;

                sharedObjects.TryGetValue(PrivateSharedObjectKey.HEIGHT_SCAN_MODE, out value);
                string ScanMode = value as string;
                bool useFixPoint = ScanMode == "1" ? true : false;

                //wafer map信息
                var map = ConfigMgr.LoadWaferMapInfoByType(waferType);
                if (map == null)
                {
                    MessageBox.Show(this, "不存在该晶圆的信息", "Info:");
                    return;
                }

                if (chb_UseCapAltimeter.Checked)
                {
                    sharedObjects.AddOrUpdate(PrivateSharedObjectKey.IS_CAP_ALTIMETER, "1", (key, oldValue) => "1");
                    map.isUserCapAltimeter = true;
                    if (rbtn_Left.Checked)
                    {
                        map.isUserCapAltimeterLeft = true;
                        sharedObjects.AddOrUpdate(PrivateSharedObjectKey.USE_LEFT_CAP_ALTIMETER, "1", (key, oldValue) => "1");
                    }
                    else
                    {
                        map.isUserCapAltimeterLeft = false;
                        sharedObjects.AddOrUpdate(PrivateSharedObjectKey.USE_LEFT_CAP_ALTIMETER, "0", (key, oldValue) => "0");
                    }
                }
                else
                {
                    sharedObjects.AddOrUpdate(PrivateSharedObjectKey.IS_CAP_ALTIMETER, "0", (key, oldValue) => "0");
                    HeightCalibrationInfo = ConfigMgr.LoadHCalibration(waferType, ScanMode);
                    map.isUserCapAltimeter = false;
                }

                ConfigMgr.SaveWaferMapInfobyType(map);
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.WAFER_MAP, map, (key, oldValue) => map);

                //基准位置
                var itemCalibrate = ConfigMgr.LoadSubdiePosCalibrateInfo();
                if (itemCalibrate == null)
                {
                    MessageBox.Show(this, "基准项校准信息不存在", "Info:");
                    return;
                }

                //一个Reticle内所有subdie的晶圆坐标
                var items = dgv_Items.DataSource as List<SubdieOrdinary>;
                if (items == null || items.Count < 1)
                {
                    MessageBox.Show(this, "没有测试项信息", "Info:");
                    return;
                }

                if (!subdieOrdinaryCheck(items))
                {
                    return;
                }

                //获取所有需要测试的晶圆配置
                //List<bool[]> SubDieTestState = ConfigMgr.LoadWaferTestState(txt_testDieFilePath.Text);
                List<bool[]> SubDieTestState = new List<bool[]>();

                for(int dieCount = 0; dieCount < map.DieCount;dieCount++)
                {
                    bool[] state = new bool[items.Count];
                    for(int j = 0; j < items.Count; j++) {
                        state[j] = true;
                    }
                    SubDieTestState.Add(state);
                }

                EquipmentCalibrationInfo equipmentInfo = ConfigMgr.LoadEquipmentCalibration();
                if (equipmentInfo == null)
                {
                    MessageBox.Show(this, "读取系统校准文件失败", "Info:");
                    return;
                }               

                string errMsg = string.Empty;
                TestDiesCalPos = CalTestItemsPositionOnStageUseRandomReticle(map, SubDieTestState, items, itemCalibrate, equipmentInfo, out errMsg, useFixPoint);

                if (TestDiesCalPos == null)
                {
                    MessageBox.Show(this, $"计算失败:{errMsg}", "Info:");
                }
                else
                {
                    //增加计算出来的坐标个数和测试die个数对比
                    string calInfo = $"计算完成，测试Die个数{SubDieTestState.Count},计算出有效位置个数{TestDiesCalPos.Count}";
                    MessageBox.Show(this, calInfo, "Info:");
                    sharedObjects.AddOrUpdate(PrivateSharedObjectKey.SUBDIE_POS, TestDiesCalPos, (key, oldValue) => TestDiesCalPos);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"坐标计算异常:{ex.Message}");
            }            
        }          

        private bool HeightCheck(List<ItemCalPosInfo> TestDiesCalPos, double chuckHeightLimit, out string subError) {
            subError = string.Empty;
            List<double> heightList = new List<double>();
            foreach (var item in TestDiesCalPos) {
                heightList.Add(item.ChuckZ);
            }

            double Max = heightList.Max();
            int maxIndex = heightList.IndexOf(Max);
            double Min = heightList.Min();
            int minIndex = heightList.IndexOf(Min);
            if (Math.Abs(Max - Min) > chuckHeightLimit) {
                subError = string.Format("晶圆高度差异超过{0}，最大值{1},最小值{2},index {3},{4}", chuckHeightLimit, Max, Min, maxIndex + 1, minIndex+ 1);
                return false;
            }

            return true;
        }        

        public List<ItemCalPosInfo> CalTestItemsPositionOnStageUseRandomReticle(WaferMapInfo MapInfo, List<bool[]> DieTestStatus, List<SubdieOrdinary> TestItems, SubdiePosCaliInfo itemCalibrate, EquipmentCalibrationInfo equipmentInfo, out string subError,bool useFixPoint = false)
        {
            subError = string.Empty;

            string retInfo = string.Empty;
            if (TestItems.FirstOrDefault(t => t.Subname == itemCalibrate.SubDieName) == null)
            {
                subError = "测试项中不包含基准测试项，请先校准基准项";
                return null;
            }

            List<ItemCalPosInfo> posList = new List<ItemCalPosInfo>();
            //对选择的die进行排序
            List<DieInfo> sortedDies = new List<DieInfo>();
            SortDies(ref sortedDies, MapInfo);

            //采用手选测试点
            var calibrateDie = GetSelectedDie();
            if (calibrateDie == null) {
                MessageBox.Show(this, "请选择Die。", "Info:");
                return null;
            }           

            posList = CalDoubleSideWithProbeDiePosWithAnyReticle(MapInfo, DieTestStatus, equipmentInfo, sortedDies, calibrateDie, itemCalibrate, TestItems, out subError, out retInfo, useFixPoint);
            if (posList != null) {
                if (!HeightCheck(posList, equipmentInfo.ChuckHeightDelta, out subError)) {
                    posList = null;
                }                
            }    
            
            if (!string.IsNullOrEmpty(retInfo)) {
                MessageBox.Show(retInfo);

                //增加保存参数计算信息
                SavePosCaculateInfo(MapInfo, retInfo);
            }

            return posList;
        }

        private void SavePosCaculateInfo(WaferMapInfo MapInfo, string retInfo)
        {
            string path = "PosCaliInfo";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            retInfo = retInfo + Environment.NewLine + Environment.NewLine;

            string logPath = $"{path}\\Wafer_{MapInfo.Type}_{DateTime.Now.ToString("yyyyMMdd")}.txt";
            System.IO.File.AppendAllText(logPath, retInfo);
        }        

        //全场景测试
        private List<ItemCalPosInfo> CalDoubleSideWithProbeDiePosWithAnyReticle(WaferMapInfo MapInfo, List<bool[]> DieTestStatus, EquipmentCalibrationInfo EquipmentInfo, List<DieInfo> sortedDies, DieInfo calibrateDie, SubdiePosCaliInfo itemCalibrate, List<SubdieOrdinary> TestItems, out string subError, out string retInfo, bool useFixPoint = false) 
        {
            subError = string.Empty;
            retInfo = string.Empty;
            List<ItemCalPosInfo> posList = new List<ItemCalPosInfo>();

            int dieIndex = 0;
            int SubdieIndex = 0;
            double FAZ0 = 0;

            //标定点运动坐标转换为完整晶圆坐标
            if (!WaferMotionPlat.WaferPointConvert(itemCalibrate.Chuck_AxisX, itemCalibrate.Chuck_AxisY, out double waferPosXBase, out double waferPosYBase))
            {
                subError = "计算测试项运动坐标->晶圆坐标转换失败。";
                return null;
            }

            if (!MapInfo.isUserCapAltimeter)
            {
                HeightCalibrationInfo.GetHeight(MapInfo.Height_ChuckX_Left, MapInfo.Height_ChuckY_Left, out FAZ0, useFixPoint);
            }

            /*
            double deltaWGLX = itemCalibrate.LeftX - itemCalibrate.ChuckX;
            double deltaWGLY = itemCalibrate.LeftY - itemCalibrate.ChuckY;
            double deltaWGRX = itemCalibrate.RightX - itemCalibrate.ChuckX;
            double deltaWGRY = itemCalibrate.RightY - itemCalibrate.ChuckY;
            */
            foreach (var die in sortedDies)
            {
                SubdieIndex = 0;
                string ordinate = die.OrdName;
                double waferX0 = (die.ColumnIndex - calibrateDie.ColumnIndex) * MapInfo.DieWidth;
                double waferY0 = -(die.RowIndex - calibrateDie.RowIndex) * MapInfo.DieHeight;

                foreach (var subDie in TestItems)
                {
                    if (!DieTestStatus[dieIndex][SubdieIndex])
                    {
                        SubdieIndex++;
                        continue;
                    }

                    ItemCalPosInfo posInfo = new ItemCalPosInfo();
                    posInfo.DieOrdinate = ordinate;
                    posInfo.SubDieName = subDie.Subname;
                    posInfo.Die = die;

                    double waferX1 = subDie.ChuckX - (itemCalibrate.ChuckX) + waferX0 + waferPosXBase;
                    double waferY1 = subDie.ChuckY - (itemCalibrate.ChuckY) + waferY0 + waferPosYBase;
                    if (!WaferMotionPlat.PlatPointConvert(waferX1, waferY1, out double chuckX, out double chuckY))
                    {
                        subError = "计算测试项坐标->坐标转换失败。";
                        return null;
                    }

                    if (!MapInfo.isUserCapAltimeter)
                    {
                        double FAZ1 = 0;
                        //边缘的采用距离最近的点高度代替                        
                        double dis = Math.Sqrt((chuckX - HeightCalibrationInfo.CenterX) * (chuckX - HeightCalibrationInfo.CenterX) + (chuckY - HeightCalibrationInfo.CenterY) * (chuckY - HeightCalibrationInfo.CenterY));
                        if (dis > HeightCalibrationInfo.ArearCircle)
                        {
                            HeightCalibrationInfo.GetDisMinPoint(chuckX, chuckY, out FAZ1, useFixPoint);
                        }else
                        {
                            if (!HeightCalibrationInfo.GetHeight(chuckX, chuckY, out FAZ1, useFixPoint))
                            {
                                retInfo += $"{die.OrdName} {subDie.Subname} 位置 X: {chuckX} Y: {chuckY} 高度计算失败 " + Environment.NewLine;
                                SubdieIndex++;
                                continue;
                            }
                        }                       
                        
                        posInfo.ChuckZ = EquipmentInfo.ProbeWaferContactZ0 + EquipmentInfo.ProbeCrimpingDepth + FAZ0 - FAZ1;
                    }
                    else
                    {
                        posInfo.ChuckZ = EquipmentInfo.ProbeWaferContactZ0 + EquipmentInfo.ProbeCrimpingDepth - EquipmentInfo.CapAltAdjustHeight;
                    }
                        
                    posInfo.ChuckX = chuckX;
                    posInfo.ChuckY = chuckY;
                    /*
                    posInfo.LeftX = itemCalibrate.Left_AxisX + subDie.LeftX - subDie.ChuckX - deltaWGLX;
                    posInfo.LeftY = itemCalibrate.Left_AxisY + subDie.LeftY - subDie.ChuckY - deltaWGLY;
                    posInfo.Right_X = itemCalibrate.Right_AxisX + subDie.RightX - subDie.ChuckX - deltaWGRX;
                    posInfo.Right_Y = itemCalibrate.Right_AxisY + subDie.RightY - subDie.ChuckY - deltaWGRY;
                    */
                    posInfo.LeftX = itemCalibrate.Left_AxisX + subDie.LeftX - itemCalibrate.LeftX;
                    posInfo.LeftY = itemCalibrate.Left_AxisY + subDie.LeftY - itemCalibrate.LeftY;
                    posInfo.Right_X = itemCalibrate.Right_AxisX + subDie.RightX - itemCalibrate.RightX;
                    posInfo.Right_Y = itemCalibrate.Right_AxisY + subDie.RightY - itemCalibrate.RightY;

                    posList.Add(posInfo);
                    SubdieIndex++;
                }

                dieIndex++;
            }

            return posList;
        }

        private void SortDies(ref List<DieInfo> sortedDies, WaferMapInfo MapInfo)
        {
            for (int i = 0; i < MapInfo.Dies.Count; i++)
            {
                var tempDie = MapInfo.Dies.FirstOrDefault(t => t.Name == $"{i + 1}#");
                sortedDies.Add(tempDie);
            }
        }

        private bool CheckBeforeMakePadMask(out WaferMapInfo map)
        {
            map = null;

            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_UPLOAD_STATUE, out object value);
            bool IsWaferLoad = (bool)value;
            if (!IsWaferLoad)
            {
                MessageBox.Show(this, "请先进行上料操作。", "Info:");
                return false;
            }

            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_TYPE, out value);
            if (value == null) {
                MessageBox.Show(this, "清选择晶圆类型。", "Info:");
                return false;
            }
            string waferType = value as string;

            map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            if (map == null)
            {
                MessageBox.Show(this, "不存在该晶圆的信息", "Info:");
                return false;
            }

            if (MotionState != State.Ready)
            {
                MessageBox.Show(this, "设备未准备好，请稍后再试。", "Info:");
                return false;
            }

            return true;
        }

        private bool CheckBeforeCalPos() {
            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_UPLOAD_STATUE, out object value);
            bool IsWaferLoad = (bool)value;
            if (!IsWaferLoad) {
                MessageBox.Show(this, "请先进行上料操作。", "Info:");
                return false;
            }

            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_TYPE, out value);
            if (value == null) {
                MessageBox.Show(this, "清选择晶圆类型。", "Info:");
                return false;
            }
            string waferType = value as string;            

            var map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            if (map == null) {
                MessageBox.Show(this, "不存在该晶圆的信息", "Info:");
                return false;
            }

            if (map.isUseMarkPad) {
                if (DialogResult.OK == MessageBox.Show("请确认已经更新图像模板，点击OK继续，否则取消", "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)) {
                    if (!IsPadMarkValid(map, out string errInfo)) {
                        MessageBox.Show(this, errInfo, "Info:");
                        return false;
                    }

                    padCompensate = new Dictionary<string, CompensateData>();
                    padCompensate.Clear();
                    sharedObjects.AddOrUpdate(PrivateSharedObjectKey.PAD_COMPENSATE_DIC,padCompensate,(key, oldValue) => padCompensate);
                } else {
                    return false;
                }
            }

            if (!map.isUserCapAltimeter) {
                sharedObjects.TryGetValue(PrivateSharedObjectKey.HEIGHT_SCAN_MODE, out object mode);
                string ScanMode = mode as string;   
                if (!ConfigMgr.isHcalibrationFileExist(waferType, ScanMode)) {
                    MessageBox.Show(this, "晶圆扫描高度信息不存在", "Info:");
                    return false;
                }
            }

            WaferMotionPlat = waferHandle.WaferMotionPlat;

             if (IsItemAngleChanged()) {
                 MessageBox.Show(this, "角度已经调整，请重新标定基准项目。", "Info:");
                 return false;
             }

            var items = dgv_Items.DataSource as List<SubdieOrdinary>;
            if (items == null || items.Count < 1)
            {
                MessageBox.Show(this, "没有测试项信息", "Info:");
                return false;
            }

            return true;
        }

        public bool subdieOrdinaryCheck(List<SubdieOrdinary> item) {
            if (basicDieName == string.Empty) {
                MessageBox.Show(this, "未设置基准参考点", "Info:");
                return false;
            }

            //1 找到basicDieName
            SubdieOrdinary basic = item.FirstOrDefault(t => t.Subname == basicDieName); 
            if (basic == null) {
                MessageBox.Show(this, $"未设置基准参考点:{basicDieName}", "Info:");
                return false;
            }

            double LeftFa2PadMaxX = basic.LeftX - basic.ChuckX;
            double RightFa2PadMaxX = basic.RightX - basic.ChuckX;
            double LeftFa2RightFaMaxX = basic.LeftX - basic.RightX;
            double LeftFa2PadMinX = LeftFa2PadMaxX;
            double RightFa2PadMinX = RightFa2PadMaxX;
            double LeftFa2RightFaMinX = LeftFa2RightFaMaxX;

            double LeftFa2PadMaxY = basic.LeftY - basic.ChuckY;
            double RightFa2PadMaxY = basic.RightY - basic.ChuckY;
            double LeftFa2RightFaMaxY = basic.LeftY - basic.RightY;
            double LeftFa2PadMinY = LeftFa2PadMaxY;
            double RightFa2PadMinY = RightFa2PadMaxY;
            double LeftFa2RightFaMinY = LeftFa2RightFaMaxY;

            //2 比较所有die与参考die的左右fa和chuck坐标最大最小差异
            foreach (SubdieOrdinary die in item) {
                if((die.LeftX - die.ChuckX) <= LeftFa2PadMinX) {
                    LeftFa2PadMinX = die.LeftX - die.ChuckX;
                }
                if ((die.LeftX - die.ChuckX) > LeftFa2PadMaxX) {
                    LeftFa2PadMaxX = die.LeftX - die.ChuckX;
                }

                if ((die.RightX - die.ChuckX) <= RightFa2PadMinX) {
                    RightFa2PadMinX = die.RightX - die.ChuckX;
                }
                if ((die.RightX - die.ChuckX) > RightFa2PadMaxX) {
                    RightFa2PadMaxX = die.RightX - die.ChuckX;
                }

                if ((die.LeftX - die.RightX) <= LeftFa2RightFaMinX) {
                    LeftFa2RightFaMinX = die.LeftX - die.RightX;
                }
                if ((die.LeftX - die.RightX) > LeftFa2RightFaMaxX) {
                    LeftFa2RightFaMaxX = die.LeftX - die.RightX;
                }

                //Y
                if ((die.LeftY - die.ChuckY) <= LeftFa2PadMinY) {
                    LeftFa2PadMinY = die.LeftY - die.ChuckY;
                }
                if ((die.LeftY - die.ChuckY) > LeftFa2PadMaxY) {
                    LeftFa2PadMaxY = die.LeftY - die.ChuckY;
                }

                if ((die.RightY - die.ChuckY) <= RightFa2PadMinY) {
                    RightFa2PadMinY = die.RightY - die.ChuckY;
                }
                if ((die.RightY - die.ChuckY) > RightFa2PadMaxY) {
                    RightFa2PadMaxY = die.RightY - die.ChuckY;
                }

                if ((die.LeftY - die.RightY) <= LeftFa2RightFaMinY) {
                    LeftFa2RightFaMinY = die.LeftY - die.RightY;
                }
                if ((die.LeftY - die.RightY) > LeftFa2RightFaMaxY)  {
                    LeftFa2RightFaMaxY = die.LeftY - die.RightY;
                }
            }   
            
            double leftFaShiftXMax = Math.Round(LeftFa2PadMaxX - (basic.LeftX - basic.ChuckX),2);
            double rightFaShiftXMax = Math.Round(RightFa2PadMaxX - (basic.RightX - basic.ChuckX), 2);
            double leftFa2RightFaShiftXMax = Math.Round(LeftFa2RightFaMaxX - (basic.LeftX - basic.RightX), 2);
            double leftFaShiftYMax = Math.Round(LeftFa2PadMaxY - (basic.LeftY - basic.ChuckY), 2);
            double rightFaShiftYMax = Math.Round(RightFa2PadMaxY - (basic.RightY - basic.ChuckY), 2);
            double leftFa2RightFaShiftYMax = Math.Round(LeftFa2RightFaMaxY - (basic.LeftY - basic.RightY), 2);

            double leftFaShiftXMin = Math.Round(LeftFa2PadMinX - (basic.LeftX - basic.ChuckX), 2);
            double rightFaShiftXMin = Math.Round(RightFa2PadMinX - (basic.RightX - basic.ChuckX), 2);
            double leftFa2RightFaShiftXMin = Math.Round(LeftFa2RightFaMinX - (basic.LeftX - basic.RightX), 2);
            double leftFaShiftYMin = Math.Round(LeftFa2PadMinY - (basic.LeftY - basic.ChuckY), 2);
            double rightFaShiftYMin = Math.Round(RightFa2PadMinY - (basic.RightY - basic.ChuckY), 2);
            double leftFa2RightFaShiftYMin = Math.Round(LeftFa2RightFaMinY - (basic.LeftY - basic.RightY), 2);

            string info1 = $"Left FA与Pad之间X距离变化范围{leftFaShiftXMin}:{leftFaShiftXMax}, Left FA与Pad之间Y距离变化范围{leftFaShiftYMin}:{leftFaShiftYMax}" ;
            string info2 = $"Right FA与Pad之间X距离变化范围{rightFaShiftXMin}:{rightFaShiftXMax}, Right FA与Pad之间Y距离变化范围{rightFaShiftYMin}:{rightFaShiftYMax}";
            string info3 = $"Left FA与Right FA之间X距离变化范围{leftFa2RightFaShiftXMin}:{leftFa2RightFaShiftXMax}, Left FA与Right FA之间Y距离变化范围{leftFa2RightFaShiftYMin}:{leftFa2RightFaShiftYMax}";

            string info = info1 + Environment.NewLine + info2 + Environment.NewLine + info3 + Environment.NewLine+"点击Yes继续，否则退出";
            if (DialogResult.Yes == MessageBox.Show(info, "Info", MessageBoxButtons.YesNo)){
                return true;
            }

            return false;
        }

        public bool IsPadMarkValid(WaferMapInfo MapInfo, out string errMsg)
        {
            errMsg = string.Empty;
            //1：PAD模板文件存在
            string filePath = $"Configuration\\Wafer\\{MapInfo.Type}_Pad.shm";
            if (!File.Exists(filePath)) {
                errMsg = "第一模板信息文件不存在";
                return false;
            }

            filePath = $"Configuration\\Wafer\\{MapInfo.Type}_Pad2.shm";
            if (!File.Exists(filePath)) {
                errMsg = "第二模板信息文件不存在";
                return false;
            }

            //2：MAP图中两个模板的XY坐标必须一致，且不为NAN
            if (double.IsNaN(MapInfo.MarkPadChuckX) || double.IsNaN(MapInfo.MarkPadChuckY) || double.IsNaN(MapInfo.MarkPadChuckX_2) || double.IsNaN(MapInfo.MarkPadChuckY_2))
            {
                errMsg = "模板信息不完整";
                return false;
            }

            if ((Math.Abs(MapInfo.MarkPadChuckX - MapInfo.MarkPadChuckX_2) > 1) || (Math.Abs(MapInfo.MarkPadChuckY - MapInfo.MarkPadChuckY_2) > 1))
            {
                errMsg = $"模板1和模板2的Chuck坐标不一致，模板1:{MapInfo.MarkPadChuckX},{MapInfo.MarkPadChuckY} 模板2:{MapInfo.MarkPadChuckX_2},{MapInfo.MarkPadChuckY_2}";
                return false;
            }

            return true;
        }

        private void btn_Import_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "(*.csv)|*.csv";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            string name = dlg.FileName;
            var items = ConfigMgr.LoadSubdieOrdinaryFromCSV(name);
            if (items == null)
            {
                MessageBox.Show(this, "导入失败!" + "\r\n" + ConfigMgr.ErrorMsg, "提示：");
            }
            else
            {
                dgv_Items.DataSource = items;
                if (ConfigMgr.SaveSubdieOrdinaryToXML(items))
                {
                    MessageBox.Show(this, "导入成功！", "提示：");
                }
                else
                {
                    MessageBox.Show(this, "导入失败！", "提示：");
                }
            }
        }

        private void chbox_IsFAMove_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btn_SetItemAsBase_Click(object sender, EventArgs e)
        {
            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_UPLOAD_STATUE, out object value);
            bool IsWaferLoad = (bool)value;
            if (!IsWaferLoad)
            {
                MessageBox.Show(this, "请先进行上料操作。", "Info:");
                return;
            }

            var calibrateDie = GetSelectedDie();
            if (calibrateDie == null)
            {
                MessageBox.Show(this, "请选择Die", "Info:");
                return ;
            }

            if (dgv_Items.SelectedRows.Count < 1)
            {
                MessageBox.Show(this, "请选择一个基准Subdie。", "Info:");
                return;
            }

            var row = dgv_Items.SelectedRows[0];
            string name = row.Cells[0].Value.ToString();
            basicDieName = name;
            if (MessageBox.Show($"确定要 {name} 设置为基准吗?", "Ask:", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            
            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_TYPE, out value);
            string waferType = value as string;
            var map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            if (map == null)
            {
                MessageBox.Show(this, "加载晶圆信息失败。", "Info:");
                return;
            }
            
            waferHandle.SetDieHighLight(calibrateDie);
            waferHandle.SetDieReference(calibrateDie);

            SubdiePosCaliInfo info = new SubdiePosCaliInfo();
            info.SubDieName = name;
            info.ReticleName = calibrateDie.OrdName;

            info.ChuckX = Convert.ToDouble(row.Cells[1].Value);
            info.ChuckY = Convert.ToDouble(row.Cells[2].Value);
            info.LeftX = Convert.ToDouble(row.Cells[3].Value);
            info.LeftY = Convert.ToDouble(row.Cells[4].Value);
            info.RightX = Convert.ToDouble(row.Cells[5].Value);
            info.RightY = Convert.ToDouble(row.Cells[6].Value);                 

            info.Chuck_AxisX = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2);
            info.Chuck_AxisY = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2);
            info.Left_AxisX = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_X].Position(), 2);
            info.Left_AxisY = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Y].Position(), 2);
            info.Right_AxisX = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_X].Position(), 2);
            info.Right_AxisY = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_Y].Position(), 2);
            string infoShow = $"设置基准位置: LeftX {info.Left_AxisX}, LeftY {info.Left_AxisY},ChuckX {info.Chuck_AxisX},ChuckY {info.Chuck_AxisY},RightX {info.Right_AxisX}, RightY {info.Right_AxisY}";
             
            ReportMessage(infoShow);

            UIClass.ObjectToControl(info, panel_SubDieOrdinary);
            ConfigMgr.SaveSubdiePosCalibrateInfo(info);
        }

        private void btn_MoveToBase_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要运动到基准位置吗？", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            var itemCal = ConfigMgr.LoadSubdiePosCalibrateInfo();
            if (itemCal == null)
            {
                MessageBox.Show(this, "未设定基准位置。", "Info:");
                return;
            }

            waferHandle.DoMoveToBasePos(itemCal);
        }

        private bool GetHeightEx(int channel, out double Value) {
            Value = double.NaN;

            for (int i = 0; i < 5; i++) {
                if (altimeterCap.GetHeight(channel, out Value)) {
                    return true;
                } else {
                    Thread.Sleep(500);
                }
            }

            return false;
        }

        private bool GetHeight(int channel, out double Value) {
            Value = double.NaN;

            List<double> points = new List<double>();

            for (int i = 0; i < 5; i++) {
                if (altimeterCap.GetHeight(channel, out double temp)) {
                    points.Add(temp);
                } else {
                    return false;
                }
            }

            points.RemoveAt(points.IndexOf(points.Max()));
            points.RemoveAt(points.IndexOf(points.Min()));
            Value = points.Average();
            return false;
        }

        private void btn_SetItem_LeftHeight_Click(object sender, EventArgs e)
        {
            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_UPLOAD_STATUE, out object value);
            bool IsWaferLoad = (bool)value;
            double height = 0;
            if (!IsWaferLoad)
            {
                MessageBox.Show(this, "请先进行上料操作。", "Info:");
                return;
            }

            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_TYPE, out value);
            string waferType = value as string;
            var map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            if (map == null)
            {
                MessageBox.Show(this, "加载晶圆信息失败。", "Info:");
                return;
            }

            txt_Height_LeftZ.Text = (Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Z].Position(), 2)).ToString();
            txt_Height_ChuckX_Left.Text = (Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2)).ToString();
            txt_Height_ChuckY_Left.Text = (Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2)).ToString();
            if(chb_UseCapAltimeter.Checked && rbtn_Left.Checked)
            {
                GetHeight(Cap_Altimeter_Channel.Left_Channel, out height);
                if ((height > 600) || height < 1)
                {
                    MessageBox.Show($"电容测高仪数据异常:{height}");
                    return;
                }
                map.isUserCapAltimeter = true;  

                double chuckPos = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Z].Position(), 2);
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POS_PRE, chuckPos, (key, oldValue)=> chuckPos);
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.LEFT_CAP_HEIGHT, height, (key, oldValue) => height);
            }
            else
            {
                map.isUserCapAltimeter = false;
            }
#if false
            //debug代码start
            GetHeight(Cap_Altimeter_Channel.Left_Channel, out height);
            double chuckPosDebug = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Z].Position(), 2);
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POS_PRE, chuckPosDebug, (key, oldValue) => chuckPosDebug);
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.LEFT_CAP_HEIGHT, height, (key, oldValue) => height);
            //debug代码end
#endif
            txt_Height_LeftCap.Text = Math.Round(height, 2).ToString();

            map.Height_LeftZ = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Z].Position(), 2);
            map.Height_ChuckX_Left = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2);
            map.Height_ChuckY_Left = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2);
            map.Height_LeftCap = Math.Round(height, 2);

            string info = $"设置左侧FA位置: LeftZ {map.Height_LeftZ}, ChuckX {map.Height_ChuckX_Left},ChuckY {map.Height_ChuckY_Left},Cap {map.Height_LeftCap}";
            ReportMessage(info);
            ConfigMgr.SaveWaferMapInfobyType(map);
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.WAFER_MAP, map, (key, oldValue) => map);
        }

        private void btn_SetItem_RightHeight_Click(object sender, EventArgs e)
        {
            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_UPLOAD_STATUE, out object value);
            bool IsWaferLoad = (bool)value;
            double height = 0;
            if (!IsWaferLoad)
            {
                MessageBox.Show(this, "请先进行上料操作。", "Info:");
                return;
            }

            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_TYPE, out value);
            string waferType = value as string;
            var map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            if (map == null)
            {
                MessageBox.Show(this, "加载晶圆信息失败。", "Info:");
                return;
            }

            txt_Height_RightZ.Text = (Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_Z].Position(), 2)).ToString();
            txt_Height_ChuckX_Right.Text = (Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2)).ToString();
            txt_Height_ChuckY_Right.Text = (Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2)).ToString();
            if (chb_UseCapAltimeter.Checked && rbtn_Right.Checked)
            {
                GetHeight(Cap_Altimeter_Channel.Right_Channel, out height);
                if ((height > 600) || height < 1)
                {
                    MessageBox.Show($"电容测高仪数据异常:{height}");
                    return;
                }
                map.isUserCapAltimeter = true;
                double chuckPos = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Z].Position(), 2);
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POS_PRE, chuckPos, (key, oldValue) => chuckPos);
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.RIGHT_CAP_HEIGHT, height, (key, oldValue) => height);
            }
            else
            {
                map.isUserCapAltimeter = false;
            }

#if false
            //debug 代码 start
            GetHeight(Cap_Altimeter_Channel.Right_Channel, out height);
            double chuckPosDebug = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Z].Position(), 2);
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POS_PRE, chuckPosDebug, (key, oldValue) => chuckPosDebug);
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.RIGHT_CAP_HEIGHT, height, (key, oldValue) => height);
            //debug 代码 end
#endif
            txt_Height_RightCap.Text = Math.Round(height, 2).ToString();

            map.Height_RightZ = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_Z].Position(), 2);
            map.Height_ChuckX_Right = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2);
            map.Height_ChuckY_Right = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2);
            map.Height_RightCap = Math.Round(height, 2);

            string info = $"设置右侧FA位置: LeftZ {map.Height_RightZ}, ChuckX {map.Height_ChuckX_Right},ChuckY {map.Height_ChuckY_Right},Cap {map.Height_RightCap}";
            ReportMessage(info);
            ConfigMgr.SaveWaferMapInfobyType(map);
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.WAFER_MAP, map, (key, oldValue) => map);
        }

        private void btn_SetProberCardPressDepth_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"确定要设定扎针深度为{num_ProbeCrimpingDepth.Value}um 吗？", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
            if (info == null)
            {
                ReportMessage("下料前检查失败:加载校准文件失败");
                MessageBox.Show(this, "加载校准文件失败！", "提示：");
                return;
            }

            info.ProbeCrimpingDepth = (double)num_ProbeCrimpingDepth.Value;
            if (ConfigMgr.SaveEquipmentCalibration(info))
            {
                ReportMessage($"探针压接深度设置{info.ProbeCrimpingDepth}成功");
                MessageBox.Show(this, $"探针压接深度设置{info.ProbeCrimpingDepth}成功", "Info:");
            }
            else
            {
                ReportMessage($"探针压接深度设置{info.ProbeCrimpingDepth}失败");
                MessageBox.Show(this, $"探针压接深度设置{info.ProbeCrimpingDepth}失败", "Info:");
            }
        }

        private void btn_SetProberCardContactPos_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定设置当前Chuck高度为晶圆和探针接触高度吗？。", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
            if (info == null)
            {
                ReportMessage("下料前检查失败:加载校准文件失败");
                MessageBox.Show(this, "加载校准文件失败！", "提示：");
                return;
            }

            double curChuckZ = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Z].Position(), 2);
            if (curChuckZ - info.ChuckZ <= 500)
            {
                ReportMessage($"当前接触高度{curChuckZ}太低，需要大于{(info.ChuckZ + 500)}");
                MessageBox.Show(this, $"当前接触高度{curChuckZ}太低，需要大于{info.ChuckZ + 500}", "Info:");
                return;
            }

            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POSITION_SAFE, curChuckZ, (key, oldValue) => curChuckZ);

            info.ProbeWaferContactZ0 = curChuckZ;
            txt_ProbeWaferContactZ0.Text = curChuckZ.ToString();
            double CcdPos = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Z].Position(), 2);
            info.ProbeContactCCDPos = CcdPos;
            txt_ProbeContactCCDPos.Text = CcdPos.ToString();    
            if (ConfigMgr.SaveEquipmentCalibration(info))
            {
                ReportMessage($"探针压接位置 {curChuckZ}设置成功");
                MessageBox.Show(this, "探针压接位置设置成功", "Info:");
            }
            else
            {
                MessageBox.Show(this, "探针压接位置设置失败", "Info:");
            }
        }

        private void btn_Probe_Test_Click(object sender, EventArgs e)
        {
            if (MotionState != State.Ready)
            {
                MessageBox.Show(this, "设备未准备好，请稍后再试。", "Info:");
                return;
            }

            if (MessageBox.Show("确定要运动到压针位置吗？", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            var equipCalibrate = ConfigMgr.LoadEquipmentCalibration();
            if (equipCalibrate.ProbeWaferContactZ0 < equipCalibrate.ChuckZ + 500)
            {
                MessageBox.Show(this, $"请先设置接触高度。", "Info:");
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    MotionState = State.Busy;
                    waferHandle.MoveToProbeCardTestPos(equipCalibrate);
                    Invoke(new Action(() =>
                    {
                        ReportMessage("运动到压针位置OK");
                        MessageBox.Show("运动到压针位置OK");
                    }));
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() =>
                    {
                        ReportMessage($"运动到压针位置异常:{ex.Message}");
                        MessageBox.Show($"运动到压针位置异常:{ex.Message}");
                    }));
                    return;
                }
                finally
                {
                    MotionState = State.Ready;
                }
            });
        }

        private void btn_Probe_Contact_Click(object sender, EventArgs e)
        {
            if (MotionState != State.Ready)
            {
                MessageBox.Show(this, "设备未准备好，请稍后再试。", "Info:");
                return;
            }

            if (MessageBox.Show("确定要运动到探针接触位置吗？", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            var equipCalibrate = ConfigMgr.LoadEquipmentCalibration();
            if (equipCalibrate.ProbeWaferContactZ0 < equipCalibrate.ChuckZ + 500)
            {
                MessageBox.Show(this, $"请先设置接触高度。", "Info:");
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    MotionState = State.Busy;
                    waferHandle.MoveToProbeCardContactPos(equipCalibrate);
                    Invoke(new Action(() =>
                    {
                        ReportMessage("运动到探针接触位置OK");
                        MessageBox.Show("运动到探针接触位置OK");
                    }));
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() =>
                    {
                        ReportMessage($"运动到探针接触位置异常:{ex.Message}");
                        MessageBox.Show($"运动到探针接触位置异常:{ex.Message}");
                    }));
                    return;
                }
                finally
                {
                    MotionState = State.Ready;
                }
            });
        }

        private void btn_Probe_Seperate_Click(object sender, EventArgs e)
        {
            if (MotionState != State.Ready)
            {
                MessageBox.Show(this, "设备未准备好，请稍后再试。", "Info:");
                return;
            }

            if (MessageBox.Show("确定要运动到探针分离位置吗？", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            var equipCalibrate = ConfigMgr.LoadEquipmentCalibration();
            Task.Run(() =>
            {
                try
                {
                    MotionState = State.Busy;
                    waferHandle.MoveToProbeCardSeperatePos(equipCalibrate);
                    Invoke(new Action(() =>
                    {
                        ReportMessage("运动到探针分离位置OK");
                        MessageBox.Show("运动到探针分离位置OK");
                    }));
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() =>
                    {
                        ReportMessage($"运动到探针分离位置异常:{ex.Message}");
                        MessageBox.Show($"运动到探针分离位置异常:{ex.Message}");
                    }));
                    return;
                }
                finally
                {
                    MotionState = State.Ready;
                }
            });
        }

        private bool CheckBeforeItemMove()
        {
            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_UPLOAD_STATUE, out object value);
            bool IsWaferLoad = (bool)value;

            if (!IsWaferLoad)
            {
                MessageBox.Show(this, "请先进行上料操作。", "Info:");
                return false;
            }

            if (dgv_Items.SelectedRows.Count < 1)
            {
                MessageBox.Show(this, "请先选择测试结构。", "Info:");
                return false;
            }

            if (MotionState != State.Ready)
            {
                MessageBox.Show(this, "设备未准备好。", "Info:");
                return false;
            }

            if (TestDiesCalPos == null)
            {
                MessageBox.Show(this, "请先计算位置。", "Info:");
                return false;
            }

            return true;
        }

        public bool IsItemAngleChanged()
        {
            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.EQUIPMENT_CALIBRATION_FILE, info, (key, oldValue) => info);

            double sx1 = stageAxisDic[MyStageAxisKey.LEFT_SX].Position();
            double sy1 = stageAxisDic[MyStageAxisKey.LEFT_SY].Position();
            double sz1 = stageAxisDic[MyStageAxisKey.LEFT_SZ].Position();
            double sx3 = stageAxisDic[MyStageAxisKey.RIGHT_SX].Position();
            double sy3 = stageAxisDic[MyStageAxisKey.RIGHT_SY].Position();
            double sz3 = stageAxisDic[MyStageAxisKey.RIGHT_SZ].Position();
            double step = 0.001;

            return Math.Abs(sx1 - info.BaseItem_LeftSX) > step || Math.Abs(sy1 - info.BaseItem_LeftSY) > step || Math.Abs(sz1 - info.BaseItem_LeftSZ) > step
                || Math.Abs(sx3 - info.BaseItem_RightSX) > step || Math.Abs(sy3 - info.BaseItem_RightSY) > step || Math.Abs(sz3 - info.BaseItem_RightSZ) > step;
        }

        private void dgv_Items_DoubleClick(object sender, EventArgs e)
        {
            if (!CheckBeforeItemMove())
            {
                return;
            }

            DieInfo selectDie = GetSelectedDie();
            if (selectDie == null)
            {
                MessageBox.Show(this, "请选择Die。", "Info:");
                return;
            }

            string subDieName = dgv_Items.SelectedRows[0].Cells[0].Value.ToString();
            string ordinate = selectDie.OrdName;
            var itemCalPos = TestDiesCalPos.FirstOrDefault(t => t.SubDieName == subDieName && t.DieOrdinate == ordinate);
            if (itemCalPos == null)
            {
                MessageBox.Show(this, "位置信息不存在。", "Info:");
                return;
            }

            if (MessageBox.Show("确定要运动到该位置吗？", "提示：", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            bool isStageMove = chbox_IsFAMove.Checked;

            Task.Run(() =>
            {
                try
                {
                    MotionState = State.Busy;
                    if(!waferHandle.MoveToCaliSubDie(itemCalPos, isStageMove, out string errInfo))
                    {
                        Invoke(new Action(() => { MessageBox.Show(this, $"运动失败:{errInfo}", "Warning:"); }));
                    }
                    //stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(-10);
                    //stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
                    //stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(itemCalPos.ChuckZ);
                    //stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

                    waferHandle.SetSelectedDie(selectDie.RowIndex, selectDie.ColumnIndex);
                    waferHandle.SetDieHighLight(selectDie);

                    Invoke(new Action(() => { MessageBox.Show(this, "运动完成", "Info:"); }));
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() => { MessageBox.Show(this, $"运动异常:{ex.Message}", "Info:"); }));
                }
                finally
                {
                    MotionState = State.Ready;
                }
            });
        }

        private void FormPositionCali_Load(object sender, EventArgs e)
        {
            //显示Chuck位置
            var itemCal = ConfigMgr.LoadSubdiePosCalibrateInfo();
            if (itemCal != null) {
                UIClass.ObjectToControl(itemCal, panel_SubDieOrdinary);
            }

            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_TYPE, out object value);
            string waferType = value as string;

            //显示左右六轴位置
            WaferMapInfo map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            if (map != null) {
                UIClass.ObjectToControl(map, panel_LeftFA_HeightCali);
                UIClass.ObjectToControl(map, panel_RightFA_HeightCali);

                if (map.isUseMarkPad) {
                    chb_UsePadMark.Checked = true;
                    gbox_MarkPad1.Enabled = true;
                    gbox_MarkPad2.Enabled = true;
                }  else  {
                    chb_UsePadMark.Checked = false;
                    gbox_MarkPad1.Enabled = false;
                    gbox_MarkPad2.Enabled = false;
                }

                UIClass.ObjectToControl(map, panel_AssistMark);
               
                chb_UseCapAltimeter.Checked = map.isUserCapAltimeter;
                gbox_AmSelect.Enabled = map.isUserCapAltimeter;
                rbtn_Left.Checked = map.isUserCapAltimeterLeft;
                rbtn_Left.Enabled = map.isUserCapAltimeter;
                rbtn_Right.Checked = !map.isUserCapAltimeterLeft;
                rbtn_Right.Enabled = map.isUserCapAltimeter;
            }

            //现在探针接触位置&压针深度
            EquipmentCalibrationInfo equipmentInfo = ConfigMgr.LoadEquipmentCalibration();
            if (equipmentInfo != null) {
                UIClass.ObjectToControl(equipmentInfo, panel_ProbeCard_Cali);
            }
        }        

        private void btn_SelectPadMark_Click(object sender, EventArgs e)
        {
            if (!CheckBeforeMakePadMask(out WaferMapInfo map)) {
                return;
            }

            Task.Run(() => {
                try {
                    MotionState = State.Busy;

                    Invoke(new Action(() => { this.Cursor = Cursors.WaitCursor; }));
                    //AutoFocus(50);
                    camera.SignlShot(out HObject image);
                    Invoke(new Action(() => { this.Cursor = Cursors.Default; }));

                    FormSelectWaferMark frm = new FormSelectWaferMark(image);
                    if (frm.ShowDialog() != DialogResult.Yes) {
                        Invoke(new Action(() => {
                            ReportMessage("用户未正常保存Mark点");
                            MessageBox.Show(this, "用户未正常保存Mark点", "提示：");
                        }));
                        return;
                    }

                    if (frm.IsFinished) {
                        Invoke(new Action(() =>
                        {
                            map.MarkPadRow = Math.Round(frm.row.D, 2);
                            map.MarkPadColumn = Math.Round(frm.col.D, 2);

                            HOperatorSet.WriteShapeModel(frm.id, $"Configuration\\Wafer\\{map.Type}_Pad.shm");
                            map.MarkPadChuckX = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2);
                            map.MarkPadChuckY = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2);

                            txt_MarkPadRow.Text = map.MarkPadRow.ToString(); 
                            txt_MarkPadColumn.Text = map.MarkPadColumn.ToString();
                            txt_MarkPadChuckX.Text = map.MarkPadChuckX.ToString();
                            txt_MarkPadChuckY.Text = map.MarkPadChuckY.ToString();  
                            ConfigMgr.SaveWaferMapInfobyType(map);
                            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.WAFER_MAP, map, (key, oldValue) => map);
                        })); 
                    }
                } catch(Exception ex) {
                    Invoke(new Action(() => {
                        ReportMessage($"Mark点选择异常:{ex.Message}");
                        MessageBox.Show(this, $"Mark点选择异常:{ex.Message}", "提示：");
                    }));
                } finally {
                    MotionState = State.Ready;
                }
            });
        }

        public void AutoFocus(double step)
        {
            int dir = 1;
            HObject image = new HObject();
            double preShape = 0;
            double curShape = 0;

            for (int i = 0; i < 20; i++)
            {
                image.Dispose();
                Thread.Sleep(500);
                camera.SignlShot(out image);
                curShape = VisionMgr.GetImageShape(image);
                if (preShape == 0)
                    preShape = curShape;

                if (curShape < preShape)
                {
                    //result.stageAxis.MoveRelative(-dir * step);
                    stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(-dir * step);
                    stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

                    dir = -dir;
                    if (dir == 1 || (dir == -1 && i > 2))
                        break;
                }
                else
                {
                    preShape = curShape;
                }
                stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(dir * step);
                stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
            }
        }

        private void btn_SelectPadMark2_Click(object sender, EventArgs e)
        {
            if (!CheckBeforeMakePadMask(out WaferMapInfo map))
            {
                return;
            }

            Task.Run(() => {
                try
                {
                    MotionState = State.Busy;

                    Invoke(new Action(() => { this.Cursor = Cursors.WaitCursor; }));
                    //AutoFocus(50);
                    camera.SignlShot(out HObject image);
                    Invoke(new Action(() => { this.Cursor = Cursors.Default; }));

                    FormSelectWaferMark frm = new FormSelectWaferMark(image);
                    if (frm.ShowDialog() != DialogResult.Yes)
                    {
                        Invoke(new Action(() => {
                            ReportMessage("用户未正常保存Mark点");
                            MessageBox.Show(this, "用户未正常保存Mark点", "提示：");
                        }));
                        return;
                    }

                    if (frm.IsFinished)
                    {
                        Invoke(new Action(() =>
                        {
                            map.MarkPadRow_2 = Math.Round(frm.row.D, 2);
                            map.MarkPadColumn_2 = Math.Round(frm.col.D, 2);

                            HOperatorSet.WriteShapeModel(frm.id, $"Configuration\\Wafer\\{map.Type}_Pad2.shm");
                            map.MarkPadChuckX_2 = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2);
                            map.MarkPadChuckY_2 = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2);

                            txt_MarkPadRow_2.Text = map.MarkPadRow_2.ToString();
                            txt_MarkPadColumn_2.Text = map.MarkPadColumn_2.ToString();
                            txt_MarkPadChuckX_2.Text = map.MarkPadChuckX_2.ToString();
                            txt_MarkPadChuckY_2.Text = map.MarkPadChuckY_2.ToString();
                            ConfigMgr.SaveWaferMapInfobyType(map);
                            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.WAFER_MAP, map, (key, oldValue) => map);
                        }));
                    }
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() => {
                        ReportMessage($"Mark点选择异常:{ex.Message}");
                        MessageBox.Show(this, $"Mark点选择异常:{ex.Message}", "提示：");
                    }));
                }
                finally
                {
                    MotionState = State.Ready;
                }
            });
        }

        private void chb_UsePadMark_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_UsePadMark.Checked)
            {
                gbox_MarkPad1.Enabled = true;
                gbox_MarkPad2.Enabled = true;
                chb_isOcrFirstReticleOnly.Enabled = true;
            }
            else
            {
                gbox_MarkPad1.Enabled = false;
                gbox_MarkPad2.Enabled = false;
                chb_isOcrFirstReticleOnly.Enabled = false;
            }

            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_TYPE, out object value);
            string waferType = value as string;

            var map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            if (map == null) {
                MessageBox.Show(this, "不存在该晶圆的信息", "Info:");
                return;
            }

            map.isUseMarkPad = chb_UsePadMark.Checked;
            ConfigMgr.SaveWaferMapInfobyType(map);
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.WAFER_MAP, map, (key, oldValue) => map);
        }

        private void chb_UseCapAltimeter_CheckedChanged(object sender, EventArgs e) {
            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_TYPE, out object value);
            if (value == null) {
                MessageBox.Show("WaferType is Null");
                return;
            }
            string waferType = value as string;

            //wafer map信息
            var map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            if (map == null) {
                MessageBox.Show(this, "不存在该晶圆的信息", "Info:");
                return;
            }

            if (chb_UseCapAltimeter.Checked) {
                gbox_AmSelect.Enabled = true;
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.IS_CAP_ALTIMETER, "1", (key, oldValue) => "1");
            } else {
                gbox_AmSelect.Enabled = false;
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.IS_CAP_ALTIMETER, "0", (key, oldValue) => "0");
            }

            ConfigMgr.SaveWaferMapInfobyType(map);
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.WAFER_MAP, map, (key, oldValue) => map);
        }

        private void rbtn_Left_CheckedChanged(object sender, EventArgs e)
        {
            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_TYPE, out object value);
            if (value == null) {
                MessageBox.Show("WaferType is Null");
                return;
            }
            string waferType = value as string;

            //wafer map信息
            var map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            if (map == null) {
                MessageBox.Show(this, "不存在该晶圆的信息", "Info:");
                return;
            }

            if (rbtn_Left.Checked) {
                map.isUserCapAltimeterLeft = true;
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.USE_LEFT_CAP_ALTIMETER, "1", (key, oldValue) => "1");
            }  else {
                map.isUserCapAltimeterLeft = false;
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.USE_LEFT_CAP_ALTIMETER, "0", (key, oldValue) => "0");
            }

            ConfigMgr.SaveWaferMapInfobyType(map);
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.WAFER_MAP, map, (key, oldValue) => map);
        }

        private void rbtn_Right_CheckedChanged(object sender, EventArgs e) {
            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_TYPE, out object value);
            if (value == null) {
                MessageBox.Show("WaferType is Null");
                return;
            }
            string waferType = value as string;

            //wafer map信息
            var map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            if (map == null) {
                MessageBox.Show(this, "不存在该晶圆的信息", "Info:");
                return;
            }

            if (rbtn_Right.Checked) {
                map.isUserCapAltimeterLeft = false;
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.USE_LEFT_CAP_ALTIMETER, "0", (key, oldValue) => "0");
            } else {
                map.isUserCapAltimeterLeft = true;
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.USE_LEFT_CAP_ALTIMETER, "1", (key, oldValue) => "1");
            }

            ConfigMgr.SaveWaferMapInfobyType(map);
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.WAFER_MAP, map, (key, oldValue) => map);
        }

        private void chb_isOcrFirstReticleOnly_CheckedChanged(object sender, EventArgs e)
        {
            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_TYPE, out object value);
            string waferType = value as string;

            var map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            if (map == null)
            {
                MessageBox.Show(this, "不存在该晶圆的信息", "Info:");
                return;
            }

            map.isOcrFirstReticleOnly = chb_isOcrFirstReticleOnly.Checked;
            ConfigMgr.SaveWaferMapInfobyType(map);
            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.WAFER_MAP, map, (key, oldValue) => map);
        }
    }
}
