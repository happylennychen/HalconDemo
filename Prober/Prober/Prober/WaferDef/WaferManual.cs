using CommonApi.MyUtility;
using MyInstruments.MyEnum;
using MyInstruments.MyUtility;
using MyInstruments;
using MyMotionStageDriver.MyStageAxis;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyInstruments.MyCamera;
using MyInstruments.MyElecLens;
using ProberApi.MyConstant;
using Prober.Constant;
using HalconDotNet;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Dynamic;
using MathNet.Numerics.RootFinding;
using System.Windows.Forms.DataVisualization.Charting;
using Prober.Request;
using System.Xml.Linq;
using MyInstruments.MyAltimeter;
using MyInstruments.MyTec;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Runtime.InteropServices.WindowsRuntime;
using Prober.Forms;
using NLog.Layouts;
using System.Data.SqlClient;
using ProberApi.MyUtility;
using MyMotionStageUserControl;
using CommonApi.MyEnum;
using MyInstruments.MyOpm;
using MyInstruments.MyOs;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Prober.WaferDef {
    public sealed class WaferManual {
        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        private readonly Dictionary<string, Instrument> instruments;
        private readonly List<InstrumentUsage> instrumentUsageList;
        private readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        public readonly Dictionary<string, StageAxis> stageAxisDic = new Dictionary<string, StageAxis>();
        public PlatCalibrate VisionMotionCalibrate { get; set; } = new PlatCalibrate("VisionMotion");
        public PlatCalibrate WaferMotionPlat { get; set; } = new PlatCalibrate("WaferMotion");

        private StandaloneCamera camera;
        private StandaloneElecLens eLens;

        public Action<WaferMapInfo> UpdateWaferMap = null;
        public Action<string> ReportMessage;
        public Action<string> MessageBoxShow = null;
        public Action<int, int> SetSelectedDie;
        public Action<int, int> SetSelectedDieIndexByHome;
        public Action<DieInfo> SetDieHighLight;
        public Action<int, int> SetDieHighLightWithIndex;
        public Action<int, int> SetDieHighLightWithIndexByHome;
        public Action<DieInfo> SetDieReference;
        public Action<int, int> SetDieReferenceWithIndex;
        public Action<bool> SetUiState;
        public State MotionState = State.Ready;
        int shotDelay = 200;
        public List<ItemCalPosInfo> TestDiesCalPos;
        public bool IsWaferLoad = false;
        public string waferType = string.Empty;
        private ConcurrentDictionary<string, object> sharedObjects;
        StandaloneAm altimeter;
        StandaloneAm altimeterLD;
        StandaloneAm altimeterCap;
        StandaloneTec tecChuck;
        StandaloneTec tecGrat;
        private IOpm opm;
        private string Power1_Slot = string.Empty;
        private string Power2_Slot = string.Empty;
        private string Power1_Channel = string.Empty;
        private string Power2_Channel = string.Empty;
        private double tecCoeffK = 1.24495;
        private double tecCoeffB = 7.0927;

        public WaferManual(ConcurrentDictionary<string, object> sharedObjects) {
            this.sharedObjects = sharedObjects;
            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            stageAxisUsages = tempObj as Dictionary<string, StageAxis>;

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            //获取仪表
            var getResult2 = GetInstrument("top_camera");
            getResult2 = GetInstrument("elens_zoom");
            getResult2 = GetInstrument("ld_altimeter");
            getResult2 = GetInstrument("cap_altimeter");
            getResult2 = GetInstrument("chuck_temperature");
            getResult2 = GetInstrument("grating_temperature");
            getResult2 = GetInstrument("pma1_2_2_opm");

            //获取轴
            GetStageAxisDic();

            //设置CHUCK带光栅尺的轴
            stageAxisDic[MyStageAxisKey.CHUCK_X].SetStageWithGrattingRuler(true);
            stageAxisDic[MyStageAxisKey.CHUCK_Y].SetStageWithGrattingRuler(true);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].SetStageWithGrattingRuler(true);
            GetCoeff();
        }

        public void GetCoeff() {
            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
            if (info != null) {
                tecCoeffK = info.TecCoeffK;
                tecCoeffB = info.TecCoeffB;
            }
        }

        public bool GetStageAxisDic() {
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

            for (int i = 0; i < axisUseList.Count; i++) {
                var result = GetStageAxis(axisUseList[i]);
                if (!result.isOK) {
                    return false;
                }
                stageAxisDic.Add(axisUseList[i], result.stageAxis);
            }

            return true;
        }

        public (bool isOk, string errorText, Instrument instrument) GetInstrument(string instrumentUsageId) {
            string errorText = string.Empty;
            var list = this.instrumentUsageList.Where(x => x.UsageId.Equals(instrumentUsageId)).ToList();
            InstrumentUsage instrumentUsage = list.First();
            if (list == null) {
                errorText = $"GetInstrumentUsage(={instrumentUsageId}) does not exist!";
                LOGGER.Error(errorText);
                return (false, errorText, null);
            }

            Instrument instrument = instruments[instrumentUsage.InstrumentId];
            switch (instrumentUsage.InstrumentCategory) {
                case EnumInstrumentCategory.CCD:
                    camera = instrument as StandaloneCamera;
                    break;
                case EnumInstrumentCategory.ELENS:
                    eLens = instrument as StandaloneElecLens;
                    break;
                case EnumInstrumentCategory.TEC:
                    if (instrumentUsage.InstrumentId == "tec_chuck") {
                        tecChuck = instrument as StandaloneTec;
                    } else if (instrumentUsage.InstrumentId == "tec_grat") {
                        tecGrat = instrument as StandaloneTec;
                    } else {
                        errorText = $"{instrumentUsage.InstrumentId.ToString()} is not a valid instrument id of temperature stable monitor!";
                        LOGGER.Error(errorText);
                        return (false, errorText, null);
                    }
                    break;
                case EnumInstrumentCategory.ALTIMETER:
                    if (instrumentUsage.InstrumentId == "altimeter_ld") {
                        altimeterLD = instrument as StandaloneAm;
                        altimeter = altimeterLD;
                    } else if (instrumentUsage.InstrumentId == "altimeter_cap") {
                        altimeterCap = instrument as StandaloneAm;
                    } else {
                        errorText = $"{instrumentUsage.InstrumentId.ToString()} is not a valid instrument id of temperature stable monitor!";
                        LOGGER.Error(errorText);
                        return (false, errorText, null);
                    }
                    break;
                case EnumInstrumentCategory.OPM:
                    if (instrumentUsage.UsageId == "pma1_2_1_opm") {
                        opm = instrumentUsage.TheInstrument as IOpm;
                        Power1_Slot = instrumentUsage.Slot;
                        Power1_Channel = instrumentUsage.Channel;
                    } else if (instrumentUsage.UsageId == "pma1_2_2_opm") {
                        opm = instrumentUsage.TheInstrument as IOpm;
                        Power2_Slot = instrumentUsage.Slot;
                        Power2_Channel = instrumentUsage.Channel;
                    } else {
                        errorText = $"{instrumentUsage.InstrumentId.ToString()} is not a valid instrument id of FA Rolling!";
                        LOGGER.Error(errorText);
                        return (false, errorText, null);
                    }
                    break;
                default:
                    errorText = $"{instrumentUsage.InstrumentCategory.ToString()} is not a valid instrument category of coupling feedback!";
                    LOGGER.Error(errorText);
                    ReportMessage(errorText);
                    return (false, errorText, null);
            }

            return (true, string.Empty, instrument);
        }

        internal (bool isOK, string errorMessage, StageAxis stageAxis) GetStageAxis(string axisUsageId) {
            string errorText = string.Empty;
            string INVALID_PARAMETERS = $"input parameters(={axisUsageId}) is invalid";

            if (!stageAxisUsages.ContainsKey(axisUsageId)) {
                errorText = $"{INVALID_PARAMETERS}";
                LOGGER.Error(errorText);
                return (false, errorText, null);
            }

            return (true, null, stageAxisUsages[axisUsageId]);
        }

        public bool AutoMap(WaferMapInfo map, out string errMsg) {
            errMsg = string.Empty;

            //加载图象校准信息
            var plat = ConfigMgr.LoadVisionMotionPlatInfo();
            VisionMotionCalibrate.GeneratePlatCalibration(plat);
            //加载机台校准信息
            var equipCalibrateInfo = ConfigMgr.LoadEquipmentCalibration();

            //运动到安全位置
            MoveAllToSafePos(equipCalibrateInfo);
            if (MotionState == State.Stop) { errMsg = "用户点击停止Mapping"; return false; }

            //调整相机设置
            if (!SetCcdValue(map.MarkExposure, map.MarkZoom, out errMsg)) {
                return false;
            }

            if (MotionState == State.Stop) { errMsg = "用户点击停止Mapping"; return false; }
            //运动到Mark点
            MoveToMark(map);

            if (MotionState == State.Stop) { errMsg = "用户点击停止Mapping"; return false; }
            //chuck Z轴运动到指定高度
            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrateInfo.ChuckZ);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

            //获取芯片尺寸,并旋转角度，以Y为依据摆正晶圆
            if (!GetDieSizeAuto(map, out errMsg)) {
                return false;
            }

            //获取晶圆表面的map分布
            if (!GetWaferMapAuto(map, out errMsg)) {
                return false;
            }

            //回到mark位置，方便后续操作
            MoveToMark(map);

            return true;
        }

        private bool GetWaferMapAuto(WaferMapInfo map, out string errMsg) {
            errMsg = string.Empty;
            map.Dies.Clear();

            ReportMessage("开始自动扫描Map");

            HOperatorSet.ReadShapeModel($"Configuration\\Wafer\\{map.Type}.shm", out HTuple modleID);
            //回到mark位置，方便后续操作
            MoveToMark(map);

            //先找出当前行的中间位置
            if (!MoveToCurRowCenter(map, modleID, out errMsg)) {
                return false;
            }

            //从下到上识别出行数
            if (!GetDieRowCount(map, modleID, out errMsg)) {
                return false;
            }

            //沿着行向下扫描,获取晶圆上所有的die分布
            if (!ScanMap(map, modleID, out errMsg)) {
                return false;
            }

            //计算Map最大列数
            var allCols = map.Dies.Select(t => t.ColumnIndex).ToArray();
            map.DieColumns = allCols.Max() - allCols.Min() + 1;
            map.CtlCols = map.DieColumns + 2;

            //重新计算所有die在控件中的列坐标
            int deltaCol = 1 - allCols.Min();
            foreach (var item in map.Dies) {
                item.ColumnIndex += deltaCol;
            }
            map.MarkDieColumnIndex += deltaCol;

            ReportMessage("自动扫描Map完成");
            return true;
        }

        public bool ScanMap(WaferMapInfo map, HTuple modleID, out string errMsg) {
            errMsg = string.Empty;
            ReportMessage("开始全盘扫描晶圆分布");

            int dir = -1;
            int scanCount = 20;
            double rowStartX2 = stageAxisDic[MyStageAxisKey.CHUCK_X].Position();
            double rowStartY2 = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position();

            for (int i = 1; i <= map.DieRows; i++) {
                if (MotionState == State.Stop) { errMsg = "用户点击停止Mapping"; return false; }
                MoveChuckXYAbs(rowStartX2, (rowStartY2 + map.DieHeight * (i - 1)));

                int colIndex = 0;
                dir = -1;
                bool isPostive = false;
                for (int j = 0; j < scanCount; j++) {
                    if (MotionState == State.Stop) { errMsg = "用户点击停止Mapping"; return false; }
                    Thread.Sleep(shotDelay);
                    if (!camera.SignlShot(out HObject image)) {
                        errMsg = "相机拍照失败";
                        return false;
                    }

                    //此处有可能找不到mark点，是正常的
                    FindMark(image, modleID, out double row, out double col, false);
                    if (row != 0 && col != 0) {
                        DieInfo die = new DieInfo();
                        die.RowIndex = i;
                        die.ColumnIndex = -dir * colIndex;
                        die.isFullDie = true;
                        map.Dies.Add(die);
                    } else {
                        colIndex = 0;
                        dir = -dir;
                        if (isPostive)
                            break;
                        stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(rowStartX2);
                        stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
                        isPostive = true;
                    }
                    stageAxisDic[MyStageAxisKey.CHUCK_X].MoveRelative(dir * map.DieWidth);
                    stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
                    colIndex++;
                }
            }

            ReportMessage("全盘扫描晶圆分布结束");
            return true;
        }

        public bool GetDieRowCount(WaferMapInfo map, HTuple modleID, out string errMsg) {
            errMsg = string.Empty;
            ReportMessage("开始扫描晶圆行数");

            //获取当前Y坐标
            double PosInit = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position();

            int dir = 1;
            int scanCount = 20;
            int start = -1;
            int stop = -1;
            int stepCount = 0;
            for (int i = 0; i < scanCount; i++) {
                if (MotionState == State.Stop) { errMsg = "用户点击停止Mapping"; return false; }
                stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveRelative(dir * map.DieHeight);
                stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();

                Thread.Sleep(shotDelay);
                if (!camera.SignlShot(out HObject image)) {
                    errMsg = "相机拍照失败";
                    return false;
                }

                //if (!VisionMgr.FindPlatMark(image, modleID, out double row, out double col))
                FindMark(image, modleID, out double row, out double col, false);
                if (row == 0 || col == 0) {
                    if (start == -1) {
                        start = stepCount;
                        dir = -dir;
                        stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(PosInit);
                        stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
                        stepCount = 0;
                        continue;
                    } else if (stop == -1) {
                        stop = -stepCount;
                        double posDst = PosInit + dir * stepCount * map.DieHeight;
                        stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(posDst);
                        stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
                        map.MarkDieRowIndex = stepCount + 1;
                        map.DieRows = start - stop + 1;
                        map.CtlRows = map.DieRows + 2;
                        ReportMessage($"扫描晶圆行数:{map.DieRows}");
                        return true;
                    }
                }

                stepCount++;
            }

            errMsg = "查找行数次数超限";
            return false;
        }

        public bool MoveToCurRowCenter(WaferMapInfo map, HTuple modleID, out string errMsg) {
            errMsg = string.Empty;
            ReportMessage("开始运动到晶圆中心位置");

            int dir = -1;
            int scanCount = 20;
            int start = -1;
            int stop = -1;
            int stepCount = 0;

            for (int i = 0; i < scanCount; i++) {
                if (MotionState == State.Stop) { errMsg = "用户点击停止Mapping"; return false; }

                stageAxisDic[MyStageAxisKey.CHUCK_X].MoveRelative(dir * map.DieWidth);
                stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
                Thread.Sleep(shotDelay);
                if (!camera.SignlShot(out HObject image)) {
                    errMsg = "相机拍照失败";
                    return false;
                }

                //if (!VisionMgr.FindPlatMark(image, modleID, out double row, out double col))
                FindMark(image, modleID, out double row, out double col, false);
                if (row == 0 || col == 0) {
                    if (start == -1) {
                        start = stepCount;
                        dir = -dir;
                        MoveToMark(map);
                        stepCount = 0;
                        continue;
                    } else if (stop == -1) {
                        stop = -stepCount;

                        stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(map.MarkChuckX + -(stop + start) / 2 * map.DieWidth);
                        stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
                        map.MarkDieColumnIndex = -(stop + start) / 2;
                        return true;
                    }
                }
                stepCount++;
            }

            errMsg = "查找行中心次数超限";
            return false;
        }

        public bool GetDieSizeAuto(WaferMapInfo map, out string errMsg) {
            errMsg = string.Empty;
            ReportMessage("开始自动计算Die长度和宽度");

            VisionMotionCalibrate.PlatPointConvert(map.MarkRow, map.MarkColumn, out double baseX2, out double baseY2);
            Thread.Sleep(shotDelay);
            camera.SignlShot(out HObject image);
            HOperatorSet.ReadShapeModel($"Configuration\\Wafer\\{map.Type}.shm", out HTuple modleID);
            FindMark(image, modleID, out double row1, out double col1, true);

            //计算出第一个mark点的坐标
            VisionMotionCalibrate.PlatPointConvert(row1, col1, out double x2_1, out double y2_1);
            double die1_X2 = stageAxisDic[MyStageAxisKey.CHUCK_X].Position() + baseX2 - x2_1;
            double die1_Y2 = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position() + baseY2 - y2_1;

            //计算Die宽度
            if (!GetDieWidth(map, modleID, baseX2, baseY2, die1_X2, die1_Y2, out errMsg)) {
                return false;
            }

            //回到原点
            MoveToMark(map);
            if (MotionState == State.Stop) { errMsg = "用户点击停止Mapping"; return false; }

            //计算Die高度，同时旋转角度
            if (!GetDieHeight(map, modleID, baseX2, baseY2, die1_X2, die1_Y2, out errMsg)) {
                return false;
            }

            ReportMessage("自动计算Die长度和宽度完成");

            return true;
        }

        public bool GetDieHeight(WaferMapInfo map, HTuple modleID, double baseX2, double baseY2, double die1_X2, double die1_Y2, out string errMsg) {
            errMsg = string.Empty;
            double row2 = 0;
            double col2 = 0;
            int scanCount = 100;
            int dir = -1;
            //double _cameraFieldHeight = 3672 * 2 / 3.0 / 1.5;
            double _cameraFieldHeight = 3672 * 2 / 3.0 ;

            for (int i = 0; i < scanCount; i++) {
                row2 = 0;
                col2 = 0;
                if (MotionState == State.Stop) { errMsg = "用户点击停止Mapping"; return false; }

                stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveRelative(dir * _cameraFieldHeight);
                stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
                Thread.Sleep(shotDelay);
                camera.SignlShot(out HObject image);
                FindMark(image, modleID, out row2, out col2, false);

                if (row2 != 0 && col2 != 0) //找到下一个
                {
                    VisionMotionCalibrate.PlatPointConvert(row2, col2, out double x2_2, out double y2_2);
                    double die2_X2 = stageAxisDic[MyStageAxisKey.CHUCK_X].Position() + baseX2 - x2_2;
                    double die2_Y2 = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position() + baseY2 - y2_2;
                    map.DieHeight = Math.Sqrt((die2_X2 - die1_X2) * (die2_X2 - die1_X2) + (die2_Y2 - die1_Y2) * (die2_Y2 - die1_Y2));
                    map.DieHeight = Math.Round(map.DieHeight, 2);

                    HOperatorSet.AngleLl(-die2_Y2, die2_X2, -die1_Y2, die1_X2, 0, 0, -map.DieHeight, 0, out HTuple waferAngle);
                    HOperatorSet.TupleDeg(waferAngle, out waferAngle);

                    ReportMessage($"Wafer旋转角度:{Math.Round(waferAngle.D, 5)}");
                    if (Math.Abs(waferAngle.D) > 3) {
                        errMsg = $"晶圆旋转角度 {waferAngle.D} 过大";
                        return false;
                    }
                    if (Math.Abs(waferAngle.D) > 0.01) {
                        stageAxisDic[MyStageAxisKey.CHUCK_SZ].MoveRelative(waferAngle.D);
                        stageAxisDic[MyStageAxisKey.CHUCK_SZ].GuiUpdatePosition();
                    }

                    break;
                }

                if (i == scanCount / 2 - 1)  //反向查找
                {
                    MoveToMark(map);
                    dir = -dir;
                }

                if (dir == scanCount - 1)  //超过次数，判定失败
                {
                    errMsg = "高度失败查找次数超过门限";
                    return false;
                }
            }

            ReportMessage($"计算Die高度：{map.DieHeight}");
            return true;
        }

        public bool GetDieWidth(WaferMapInfo map, HTuple modleID, double baseX2, double baseY2, double die1_X2, double die1_Y2, out string errMsg) {
            errMsg = string.Empty;
            double row2 = 0;
            double col2 = 0;

            //double _cameraFieldWidth = 5496 * 2 / 3.0 / 2.25;
            double _cameraFieldWidth = 5496 * 2 / 3.0 ;
            //识别出die的宽度
            int dir = -1;
            int scanCount = 100;
            for (int i = 0; i < scanCount; i++) {
                row2 = 0;
                col2 = 0;

                if (MotionState == State.Stop) { errMsg = "用户点击停止Mapping"; return false; }

                stageAxisDic[MyStageAxisKey.CHUCK_X].MoveRelative(dir * _cameraFieldWidth);
                stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
                Thread.Sleep(shotDelay);
                camera.SignlShot(out HObject image);
                //此处找不到Mark点是正常的，异常需要屏蔽掉
                FindMark(image, modleID, out row2, out col2, false);

                if (row2 != 0 && col2 != 0) //找到下一个
                {
                    VisionMotionCalibrate.PlatPointConvert(row2, col2, out double x2_2, out double y2_2);
                    double die2_X2 = stageAxisDic[MyStageAxisKey.CHUCK_X].Position() + baseX2 - x2_2;
                    double die2_Y2 = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position() + baseY2 - y2_2;
                    map.DieWidth = Math.Sqrt((die2_X2 - die1_X2) * (die2_X2 - die1_X2) + (die2_Y2 - die1_Y2) * (die2_Y2 - die1_Y2));
                    map.DieWidth = Math.Round(map.DieWidth, 2);
                    break;
                }
                if (i == scanCount / 2 - 1)  //反向查找
                {
                    MoveToMark(map);
                    dir = -dir;
                }
                if (i == scanCount - 1)  //超过次数，判定失败
                {
                    errMsg = "高度失败查找次数超过门限";
                    return false;
                }
            }

            ReportMessage($"计算Die宽度：{map.DieWidth}");
            return true;
        }

        public bool SetCcdValue(double exposure, double zoom, out string errMsg) {
            errMsg = string.Empty;

            if (!camera.SetExposure(exposure)) {
                errMsg = "设置曝光值{exposure}失败";
                return false;
            }

            if (!eLens.SetZoom(zoom)) {
                errMsg = "设置放大倍数{zoom}失败";
                return false;
            }

            return true;
        }

        public bool DoMoveToBasePos(SubdiePosCaliInfo itemCal) {
            bool bRet = false;
            if (MotionState != State.Ready) {
                ReportMessage($"运动到基准位置失败:{EValue.NOREADYINFO}");
                return false;
            }

            Task.Run(() => {
                try {
                    MotionState = State.Busy;
                    ReportMessage("开始运动到基准位置");

                    if (MoveToBasePos(itemCal)) {
                        ReportMessage("运动到基准位置完成");
                        bRet = true;
                    } else {
                        ReportMessage("运动到基准位置失败");
                        bRet = false;
                    }
                } catch (Exception ex) {
                    ReportMessage($"运动到基准位置异常:{ex.Message}");
                    bRet = false;
                } finally {
                    MotionState = State.Ready;
                }
            });

            return bRet;
        }

        public bool MoveToBasePos(SubdiePosCaliInfo itemCal) {
            var equipCalibrate = ConfigMgr.LoadEquipmentCalibration();

            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Z].MoveAbsolute(equipCalibrate.Safe_LeftZ); });
            task.Add(t1);            
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveAbsolute(equipCalibrate.Safe_RightZ); });
            task.Add(t2); 
            Task.WaitAll(task.ToArray());

            stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();

            //stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(-500);
            sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_SEPERATE_HEIGHT, out object seperateHeight);
            string sepLen = seperateHeight as string;
            double zMoveLen = Convert.ToDouble(sepLen);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(-1 * zMoveLen);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

            List<Task> task2 = new List<Task>();
            var t3 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_X].MoveAbsolute(itemCal.Left_AxisX); });
            task2.Add(t3);
            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Y].MoveAbsolute(itemCal.Left_AxisY); });
            task2.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(itemCal.Chuck_AxisX); });
            task2.Add(t5);
            var t6 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(itemCal.Chuck_AxisY); });
            task2.Add(t6);
#if false
            var t7 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_X].MoveAbsolute(equipCalibrate.Base_CcdX); });
            task2.Add(t7);
            var t8 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Y].MoveAbsolute(equipCalibrate.Base_CcdY); });
            task2.Add(t8);
#endif
            
            var t9 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_X].MoveAbsolute(itemCal.Right_AxisX); });
            task2.Add(t9);
            var t10 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Y].MoveAbsolute(itemCal.Right_AxisY); });
            task2.Add(t10);
            
            Task.WaitAll(task2.ToArray());

            stageAxisDic[MyStageAxisKey.LEFT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Y].GuiUpdatePosition();

            stageAxisDic[MyStageAxisKey.RIGHT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Y].GuiUpdatePosition();

            //stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(500);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(zMoveLen);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

            try {
                ShowCurReticle(itemCal.ReticleName);
                SetReticleHighLight(itemCal.ReticleName);
                SetReticleReference(itemCal.ReticleName);
            } catch (Exception ex) {
                LOGGER.Error(ex);
            }

            return true;
        }
		
	    public void SetReticleHighLight(string dieName)
        {
            if (dieName.Contains(","))
            {
                string[] split = dieName.Split('(', ',', ')');
                int X = Convert.ToInt32(split[1]);
                int Y = Convert.ToInt32(split[2]);
                SetDieHighLightWithIndexByHome(X, Y);
            }
            else
            {
                DieNameToOrdinary(dieName, out int x, out int y);
                SetDieHighLightWithIndexByHome(y, x);
            }
        }

        public void SetReticleReference(string dieName)
        {
            if (dieName.Contains(","))
            {
                string[] split = dieName.Split('(', ',', ')');
                int X = Convert.ToInt32(split[1]);
                int Y = Convert.ToInt32(split[2]);
                SetDieReferenceWithIndex(X, Y);
            }
            else
            {
                DieNameToOrdinary(dieName, out int x, out int y);
                SetDieReferenceWithIndex(y, x);
            }
        }

        public void ShowCurReticle(string dieName)
        {
            if(dieName.Contains(","))
            {
                string[] split = dieName.Split('(', ',', ')');                
                int X = Convert.ToInt32(split[1]);
                int Y = Convert.ToInt32(split[2]);
                SetSelectedDieIndexByHome(X, Y);
            }
            else
            {
                DieNameToOrdinary(dieName, out int x, out int y);
                SetSelectedDieIndexByHome(y,x);
            }
        }

        public void DieNameToOrdinary(string dieName, out int x, out int y)
        {
            x = 0;
            y = 0;
            try
            {
                x = dieName.ElementAt(0) - 'A';
                y = Convert.ToInt32(dieName.Substring(1, 2)) - 1;
            }
            catch (Exception ex)
            {
                throw (new Exception($"DieNameToOrdinary;{ex.Message}"));
            }

        }

        public bool DoWaferDownload(string waferType = null) {
            bool bRet = false;

            if (MotionState != State.Ready) {
                ReportMessage($"下料前检查失败:{EValue.NOREADYINFO}");
                return false;
            }

            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
            if (info == null) {
                ReportMessage("下料前检查失败:加载校准文件失败");
                return false;
            }

            Task.Run(() => {
                try {
                    MotionState = State.Busy;
                    ReportMessage("开始运动到下料位置");

                    if (MoveToRemovePosition(info, out string ErrMsg)) {
                        ReportMessage("运动到下料位置完成");
                        MessageBoxShow("运动到下料位置完成");
                        bRet = true;
                    } else {
                        ReportMessage($"运动到下料位置失败:{ErrMsg}");
                        MessageBoxShow($"运动到下料位置失败:{ErrMsg}");
                        bRet = false;
                    }
                } catch (Exception ex) {
                    ReportMessage($"下料异常:{ex.Message}");
                    MessageBoxShow($"下料异常:{ex.Message}");
                    bRet = false;
                } finally {
                    IsWaferLoad = false;
                    sharedObjects.AddOrUpdate(PrivateSharedObjectKey.WAFER_UPLOAD_STATUE, IsWaferLoad, (key, oldValue) => IsWaferLoad);
                    MotionState = State.Ready;
                }
            });

            return bRet;
        }

        //下料
        public bool MoveToRemovePosition(EquipmentCalibrationInfo equipCalibrate, out string errMsg) {
            errMsg = string.Empty;

            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Z].MoveAbsolute(equipCalibrate.Safe_LeftZ); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveAbsolute(equipCalibrate.Safe_RightZ); });
            task.Add(t2);
            var t3 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.Remove_ChuckZ); });
            task.Add(t3);
            Task.WaitAll(task.ToArray());
            stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

            List<Task> task2 = new List<Task>();
            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(equipCalibrate.Remove_ChuckX); });
            task2.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(equipCalibrate.Remove_ChuckY); });
            task2.Add(t5);
            Task.WaitAll(task2.ToArray());
            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();

            return true;
        }

        public bool DoWaferUpload(string waferType) {
            bool bRet = false;
            bool isWaferInfoValid = false;
            string errMsg = string.Empty;
            if (!BeforeUploadCheck(waferType, out isWaferInfoValid, out errMsg)) {
                return false;
            }

            this.waferType = waferType;

            var map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            Task.Run(() => {
                try {
                    MotionState = State.Busy;
                    if (AutoUploadWafer(map, isWaferInfoValid, false, out errMsg)) {
                        ConfigMgr.SaveWaferMapInfobyType(map);
                        ReportMessage("上料完成");
                        MessageBoxShow("上料完成");
                        IsWaferLoad = true;
                        bRet = true;                        
                    } else {
                        ReportMessage($"上料失败:{errMsg}");
                        MessageBoxShow($"上料失败:{errMsg}");
                        IsWaferLoad = false;
                        bRet = false;
                    }
                } catch (Exception ex) {
                    ReportMessage($"上料异常:{ex.Message}");
                    MessageBoxShow($"上料异常:{ex.Message}");
                    IsWaferLoad = false;
                    bRet = false;
                } finally {
                    MotionState = State.Ready;
                    sharedObjects.AddOrUpdate(PrivateSharedObjectKey.WAFER_UPLOAD_STATUE, IsWaferLoad, (key, oldValue) => IsWaferLoad);
                    sharedObjects.AddOrUpdate(PrivateSharedObjectKey.WAFER_TYPE, waferType, (key, oldValue) => waferType);
                    sharedObjects.AddOrUpdate(PrivateSharedObjectKey.WAFER_MAP,map, (key, oldValue) => map);
                }
            });

            return bRet;
        }

        public bool BeforeUploadCheck(string waferType, out bool isWaferInfoValid, out string ErrMsg) {
            ErrMsg = string.Empty;
            isWaferInfoValid = false;

            //1:机台运动未停止
            if (MotionState != State.Ready) {
                ErrMsg = $"上料操作检查失败:{EValue.NOREADYINFO}";
                ReportMessage(ErrMsg);
                return false;
            }

            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
            if (info == null) {
                ErrMsg = "上料操作检查失败:加载校准文件失败";
                ReportMessage(ErrMsg);
                return false;
            }

            //2:wafer类型存在，并初步检查标定项目是否存在
            var map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            if (map == null) {
                ErrMsg = $"上料操作检查失败:不存在{waferType}晶圆类型";
                ReportMessage(ErrMsg);
                return false;
            }

            bool isMarkExist = File.Exists($"Configuration\\Wafer\\{map.Type}.shm");
            bool isRefDieSelected = map.IsRefDieSelecteValid();
            isWaferInfoValid = isMarkExist && isRefDieSelected;
            if (!isWaferInfoValid) {
                ErrMsg = "上料操作检查:Wafer的基准信息不全或参数错误,将运行到默认上料位置";
                ReportMessage(ErrMsg);
            }

            return true;
        }

        public void SetLRAxisSpeedToNormal()
        {
            double maxSpeed = stageAxisDic[MyStageAxisKey.LEFT_X].speedMaxConfig;
            double minSpeed = stageAxisDic[MyStageAxisKey.LEFT_X].speedMinConfig;
            double accTime = stageAxisDic[MyStageAxisKey.LEFT_X].accConfig;
            stageAxisDic[MyStageAxisKey.LEFT_X].SetAxisSpeed(minSpeed, maxSpeed,accTime);

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

        //上料
        public bool AutoUploadWafer(WaferMapInfo map, bool iswaferInfoValid, bool isUpdateReticleSize, out string errMsg) {
            errMsg = string.Empty;
            var equipCalibrate = ConfigMgr.LoadEquipmentCalibration();
            var plat = ConfigMgr.LoadVisionMotionPlatInfo();
            VisionMotionCalibrate.GeneratePlatCalibration(plat);            

            //运动到安全位置
            MoveAllToSafePos(equipCalibrate);

            //没有选择mark点的晶圆运动到chuck中心位置。选择了的运动到mark点位置。
            if (!iswaferInfoValid) {
                MoveToDefaultPos(equipCalibrate);
            } else {
                HOperatorSet.ReadShapeModel($"Configuration\\Wafer\\{map.Type}.shm", out HTuple id);
                VisionMotionCalibrate.PlatPointConvert(map.MarkRow, map.MarkColumn, out double x2_0, out double y2_0);

                if (!eLens.SetZoom(map.MarkZoom)) {
                    errMsg = "相机倍率设置失败";
                    return false;
                }

                if (!camera.SetExposure(map.MarkExposure)) {
                    errMsg = "相机曝光值设置失败";
                    return false;
                }                

                MoveToMark(map);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.ChuckZ);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

                Thread.Sleep(500);
                //增加白平衡设置
                camera.SetParameter("BalanceWhiteAuto", "Continuous");//Off -关闭                
                Thread.Sleep(2000);
                camera.SetParameter("BalanceWhiteAuto", "Off");//Off -关闭

                //识别角度
                //if (!AdjustWaferAngle(map, id, x2_0, y2_0, out errMsg))
                if (!AdjustWaferAngleEx(map, id, x2_0, y2_0, out errMsg)) {
                    errMsg = "晶圆对齐失败 " + errMsg;
                    return false;
                }

                //校准晶圆的4个mark点，计算出变换矩阵。
                if (!GetWaferMotionMatrix(map, isUpdateReticleSize, out string subError)) {
                    errMsg = "识别晶圆参考Die失败。\n" + subError;
                    return false;
                }

                //获取Wafer上的mark的偏差值
                if (!GetWaferMarkDeltaPosition(map, id, x2_0, y2_0, out errMsg)) {
                    return false;
                }

                MoveToMark(map);

                SetSelectedDie(map.MarkDieRowIndex, map.MarkDieColumnIndex);
                SetDieHighLightWithIndex(map.MarkDieRowIndex, map.MarkDieColumnIndex);
            }

            return true;
        }

        public bool DoWaferHeightVerify(string waferType) {
            bool bRet = false;
            string errMsg = string.Empty;

            var map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            if (map == null) {
                errMsg = $"高度校验失败:不存在{waferType}晶圆类型";
                ReportMessage(errMsg);
                MessageBox.Show(errMsg);
                return false;
            }

            Task.Run(() => {
                try {
                    MotionState = State.Busy;
                    if (ChuckHeightVerify(map, out errMsg)) {
                        ReportMessage("上料完成");
                        bRet = true;
                    } else {
                        ReportMessage(errMsg);
                        bRet = false;
                    }
                } catch (Exception ex) {
                    errMsg = $"高度校验异常:{ex.Message}";
                    ReportMessage(errMsg);
                    bRet = false;
                } finally {
                    MotionState = State.Ready;
                }
            });

            return bRet;
        }

        public bool ChuckHeightVerify(WaferMapInfo map, out string errInfo) {
            errInfo = string.Empty;
            bool verifyRet = true;

            ChuckHeightVerifyInfo chuckHeightVerifyInfo = ConfigMgr.LoadHeightVerifyInfoByType(map.Type);
            if (chuckHeightVerifyInfo == null) {
                errInfo = "Chuck 5点高度信息文件不存在";
                return false;
            }

            //加载机台校准信息
            var equipCalibrateInfo = ConfigMgr.LoadEquipmentCalibration();

            //运动到安全位置
            MoveAllToSafePos(equipCalibrateInfo);

            //CHUCK运动到默认高度
            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrateInfo.ChuckZ);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

            //伸出测高仪
            stageAxisDic[MyStageAxisKey.HEIGHT_U].MoveAbsolute(equipCalibrateInfo.HSenserWorkU);
            stageAxisDic[MyStageAxisKey.HEIGHT_U].GuiUpdatePosition();

            //开始走位到标定的位置
            GetChuckHeightInfo(chuckHeightVerifyInfo, out List<Double> xPos, out List<Double> yPos, out List<Double> hInfo);

            List<double> heightList = new List<double>();
            double temp = 0;
            double gap = 0;
            for (int i = 0; i < xPos.Count; i++) {
                MoveChuckXYAbs(xPos[i], yPos[i]);
                Thread.Sleep(500);
                if (!GetHeight(1, out temp)) {
                    errInfo = "测高仪读取失败";
                    return false;
                }
                heightList.Add(temp);
                gap = Math.Abs(temp - hInfo[i]);
                ReportMessage($"X:{xPos[i]} Y:{yPos[i]} CurHeight:{temp} VerifyHeight:{hInfo[i]} Diff:{gap}um");
            }

            //数据比较
            verifyRet = CompHeightInfo(xPos, yPos, heightList, hInfo, chuckHeightVerifyInfo.ThresholdInner, chuckHeightVerifyInfo.ThresholdBetween, out errInfo);

            //测高仪归位
            stageAxisDic[MyStageAxisKey.HEIGHT_U].MoveAbsolute(equipCalibrateInfo.Safe_U);
            stageAxisDic[MyStageAxisKey.HEIGHT_U].GuiUpdatePosition();

            //运动到Mark点
            MoveToMark(map);

            return verifyRet;
        }

        private bool GetHeight(int channel, out double Value) {
            Value = double.NaN;

            for (int i = 0; i < 5; i++) {
                if (altimeter.GetHeight(channel, out Value)) {
                    return true;
                } else {
                    Thread.Sleep(500);
                }
            }

            return false;
        }

        private bool GetHeightWithCapAltimeter(int channel, out double Value) {
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

        private bool GetHeightWithLdAltimeter(int channel, out double Value) {
            Value = double.NaN;

            for (int i = 0; i < 5; i++) {
                if (altimeterLD.GetHeight(channel, out Value)) {
                    return true;
                } else {
                    Thread.Sleep(500);
                }
            }

            return false;
        }

        private bool GetGrattingTemp(int channel, out double Value) {
            Value = double.NaN;

            for (int i = 0; i < 5; i++) {
                if (tecGrat.GetTemp(channel, out Value)) {
                    return true;
                } else {
                    Thread.Sleep(500);
                }
            }

            return false;
        }

        private bool GetGratTemperature(out double ValueCh1, out double ValueCh2, out double ValueCh3, out double ValueCh4) {
            ValueCh1 = double.NaN;
            ValueCh2 = double.NaN;
            ValueCh3 = double.NaN;
            ValueCh4 = double.NaN;

            List<double> tempList = new List<double>();
            for (int i = 0; i < 5; i++) {
                if (tecGrat.GetTempAll(out tempList)) {
                    ValueCh1 = tempList[0];
                    ValueCh2 = tempList[1];
                    ValueCh3 = tempList[2];
                    ValueCh4 = tempList[3];

                    return true;
                } else {
                    Thread.Sleep(500);
                }
            }

            return false;
        }

        private bool GetOmronTemp(int channel, out double Value) {
            Value = double.NaN;

            for (int i = 0; i < 5; i++) {
                if (tecChuck.GetTemp(channel, out Value)) {
                    return true;
                } else {
                    Thread.Sleep(500);
                }
            }

            return false;
        }

        private bool GetHeightWithCapAltimeterUseAverage(int channel, int averageTime, int delayTimeMs, out double Value) {
            Value = double.NaN;
            List<double> points = new List<double>();

            Thread.Sleep(delayTimeMs);
            for (int i = 0; i < averageTime; i++) {
                if (GetHeightWithCapAltimeter(channel, out double temp)) {
                    points.Add(temp);
                    ReportMessage($"Height Read {temp}");
                    Thread.Sleep(50);
                } else {
                    Value = double.NaN;
                    return false;
                }
            }

            points.RemoveAt(points.IndexOf(points.Max()));
            points.RemoveAt(points.IndexOf(points.Min()));
            Value = points.Average();
            return true;
        }

        public bool CompHeightInfo(List<Double> xPos, List<Double> yPos, List<double> CurHeight, List<double> StandHeight, double ThresholdInner, double ThresholdBetween, out string errInfo) {
            errInfo = string.Empty;
            double gap = 0;
            bool ret = true;

            //单点比较
            for (int i = 0; i < CurHeight.Count; i++) {
                gap = Math.Abs(CurHeight[i] - StandHeight[i]);
                if (gap > ThresholdInner) {
                    errInfo += $"ChuckX:{xPos[i]},ChuckY:{yPos[i]} 当前Heigh{CurHeight[i]}与模板高度{StandHeight[i]}相差{gap}过大" + Environment.NewLine;
                    ret = false;
                }
            }

            //平整度比较
            double maxHeight = CurHeight.Max();
            double minHeight = CurHeight.Min();
            gap = Math.Abs(maxHeight - minHeight);
            if (gap > ThresholdBetween) {
                errInfo += $"晶圆高度偏差{gap},超过{ThresholdBetween}" + Environment.NewLine;
                ret = false;
            }

            return ret;
        }

        public void GetChuckHeightInfo(ChuckHeightVerifyInfo chuckHeightVerifyInfo, out List<double> X, out List<double> Y, out List<double> H) {
            X = new List<double>();
            Y = new List<double>();
            H = new List<double>();

            X.Add(chuckHeightVerifyInfo.ChuckX_1);
            X.Add(chuckHeightVerifyInfo.ChuckX_2);
            X.Add(chuckHeightVerifyInfo.ChuckX_3);
            X.Add(chuckHeightVerifyInfo.ChuckX_4);
            X.Add(chuckHeightVerifyInfo.ChuckX_5);

            Y.Add(chuckHeightVerifyInfo.ChuckY_1);
            Y.Add(chuckHeightVerifyInfo.ChuckY_2);
            Y.Add(chuckHeightVerifyInfo.ChuckY_3);
            Y.Add(chuckHeightVerifyInfo.ChuckY_4);
            Y.Add(chuckHeightVerifyInfo.ChuckY_5);

            H.Add(chuckHeightVerifyInfo.Height_1);
            H.Add(chuckHeightVerifyInfo.Height_2);
            H.Add(chuckHeightVerifyInfo.Height_3);
            H.Add(chuckHeightVerifyInfo.Height_4);
            H.Add(chuckHeightVerifyInfo.Height_5);
        }

        public bool GetWaferMarkDeltaPosition(WaferMapInfo map, HTuple id, double x2_0, double y2_0, out string msg) {
            msg = string.Empty;
            //识别mark的偏差
            MoveToMark(map);
            Thread.Sleep(500);

            camera.SignlShot(out HObject image);
            FindMark(image, id, out double row, out double col, true);
            VisionMotionCalibrate.PlatPointConvert(row, col, out double x2_1, out double y2_1);
            map.MarkDeltaChuckX = x2_0 - x2_1;
            map.MarkDeltaChuckY = y2_0 - y2_1;
            image.Dispose();

            ReportMessage($"上料后晶圆位置偏差:X {Math.Round(map.MarkDeltaChuckX, 2)} Y {Math.Round(map.MarkDeltaChuckY, 2)}");
            return true;
        }

        private bool GetWaferMotionMatrix(WaferMapInfo map, bool isUpdateReticleSize, out string error) {
            error = string.Empty;
            ReportMessage("开始晶圆四个参考点扫描");

            map.WaferMotionPlat = new PlatInfo(4);
            HOperatorSet.ReadShapeModel($"Configuration\\Wafer\\{map.Type}.shm", out HTuple modleID);
            VisionMotionCalibrate.PlatPointConvert(map.MarkRow, map.MarkColumn, out double x2_0, out double y2_0);

            int[] rowNums = new int[4];
            int[] colNums = new int[4];
            for (int i = 0; i < 4; i++) {
                int tempRow = Convert.ToInt32(map.GetType().GetProperty($"RefDieRowIndex{i + 1}").GetValue(map));
                int tempColumn = Convert.ToInt32(map.GetType().GetProperty($"RefDieColumnIndex{i + 1}").GetValue(map));
                rowNums[i] = tempRow;
                colNums[i] = tempColumn;
                double tempX2 = map.MarkChuckX + map.DieWidth * (map.MarkDieColumnIndex - tempColumn);
                double tempY2 = map.MarkChuckY + map.DieHeight * (tempRow - map.MarkDieRowIndex);
                double waferX2 = map.DieWidth * (tempColumn - map.MarkDieColumnIndex);
                double waferY2 = map.DieHeight * (map.MarkDieRowIndex - tempRow);

                MoveChuckXYAbs(tempX2, tempY2);
                Thread.Sleep(500);
                camera.SignlShot(out HObject image);
                FindMark(image, modleID, out double row, out double col, true);
                VisionMotionCalibrate.PlatPointConvert(row, col, out double x2_1, out double y2_1);
                map.WaferMotionPlat.Qx[i] = tempX2 + x2_0 - x2_1;
                map.WaferMotionPlat.Qy[i] = tempY2 + y2_0 - y2_1;
                map.WaferMotionPlat.Px[i] = waferX2;
                map.WaferMotionPlat.Py[i] = waferY2;
            }
            if (!WaferMotionPlat.GeneratePlatCalibration(map.WaferMotionPlat)) {
                error = "生成校准信息失败。";
                return false;
            }

            if (!WaferMotionPlat.GenerateMotionToWaferCalibration(map.WaferMotionPlat)) {
                error = "生成运动->晶圆校准信息失败。";
                return false;
            }

            //根据4个定位点重新计算一下die的宽度和高度
            double totalWidth = Math.Pow(map.WaferMotionPlat.Qx[3] - map.WaferMotionPlat.Qx[0], 2) + Math.Pow(map.WaferMotionPlat.Qy[3] - map.WaferMotionPlat.Qy[0], 2);
            totalWidth = Math.Sqrt(totalWidth);
            totalWidth = totalWidth / (colNums[3] - colNums[0]);
            double totalHeight = Math.Pow(map.WaferMotionPlat.Qx[1] - map.WaferMotionPlat.Qx[0], 2) + Math.Pow(map.WaferMotionPlat.Qy[1] - map.WaferMotionPlat.Qy[0], 2);
            totalHeight = Math.Sqrt(totalHeight);
            totalHeight = totalHeight / (rowNums[1] - rowNums[0]);

            ReportMessage($"晶圆4个参考点计算的宽-高为:{Math.Round(totalWidth, 2)},{Math.Round(totalHeight, 2)}，与Map标定时的偏差为:{Math.Round(map.DieWidth - totalWidth, 2)},{Math.Round(map.DieHeight - totalHeight, 2)}");

            //重新计算的芯片宽度信息仅供参考，后续根据重复性针对性的做一些事情

            bool isDieWidthOk = Math.Abs(map.DieWidth - totalWidth) < 0.1;
            if (!isDieWidthOk) {
                map.DieWidth = Math.Round(totalWidth, 2);
            }
            bool isDieHeightOk = Math.Abs(map.DieHeight - totalHeight) < 0.1;
            if (!isDieHeightOk) {
                map.DieHeight = Math.Round(totalHeight, 2);
            }
            if (!isDieHeightOk || !isDieWidthOk) {
                //重新更新一下Map变换矩阵
                for (int i = 0; i < 4; i++) {
                    int tempRow = Convert.ToInt32(map.GetType().GetProperty($"RefDieRowIndex{i + 1}").GetValue(map));
                    int tempColumn = Convert.ToInt32(map.GetType().GetProperty($"RefDieColumnIndex{i + 1}").GetValue(map));
                    double waferX2 = map.DieWidth * (tempColumn - map.MarkDieColumnIndex);
                    double waferY2 = map.DieHeight * (map.MarkDieRowIndex - tempRow);
                    map.WaferMotionPlat.Px[i] = waferX2;
                    map.WaferMotionPlat.Py[i] = waferY2;
                }
                if (!WaferMotionPlat.GeneratePlatCalibration(map.WaferMotionPlat)) {
                    error = "生成校准信息失败。";
                    return false;
                }

                if (!WaferMotionPlat.GenerateMotionToWaferCalibration(map.WaferMotionPlat)) {
                    error = "生成运动->晶圆校准信息失败。";
                    return false;
                }

                ConfigMgr.SaveWaferMapInfobyType(map);
            }

            ReportMessage("晶圆四个参考点扫描结束");
            return true;
        }

        public void MoveChuckXYAbs(double xPos, double yPos) {
            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(xPos); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(yPos); });
            task.Add(t2);
            Task.WaitAll(task.ToArray());

            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
        }

        public void MoveChuckXYRel(double xPos, double yPos) {
            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveRelative(xPos); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveRelative(yPos); });
            task.Add(t2);
            Task.WaitAll(task.ToArray());

            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
        }

        public bool AdjustWaferAngleEx(WaferMapInfo map, HTuple modleID, double x2_0, double y2_0, out string msg) {
            msg = string.Empty;
            int markSum = 0;
            double x2_Top = 0;
            double y2_Top = 0;
            double x2_Bottom = 0;
            double y2_Bottom = 0;

            ReportMessage("开始晶圆角度调整");
            var plat = ConfigMgr.LoadVisionMotionPlatInfo();
            VisionMotionCalibrate.GeneratePlatCalibration(plat);
            camera.SetExposure(map.MarkExposure);
            AutoFocus(50);
            //Thread.Sleep(200);
            double ccdZPos = stageAxisDic[MyStageAxisKey.CCD_Z].Position();
            //保存当前焦距
            map.MarkCcdZ = ccdZPos;


            //找到最底端Mark点
            for (int i = 1; i <= map.DieRows; i++) {
                if (MotionState == State.Stop) { msg = "用户点击停止Mapping"; return false; }

                stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveRelative(map.DieHeight);
                stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
                Thread.Sleep(shotDelay);
                if (!camera.SignlShot(out HObject image)) {
                    msg = "相机拍照失败";
                    return false;
                }
                //此处有可能找不到mark点，是正常的
                FindMark(image, modleID, out double row, out double col, false);
                if (row != 0 && col != 0) {
                    //记录下此时的位置信息,即为最底端Mark点
                    VisionMotionCalibrate.PlatPointConvert(row, col, out double x2_1, out double y2_1);
                    x2_Bottom = stageAxisDic[MyStageAxisKey.CHUCK_X].Position() + x2_0 - x2_1;
                    y2_Bottom = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position() + y2_0 - y2_1;
                    markSum++;
                } else {
                    break;
                }
            }

            //增加mark点拍照
            MoveToMark(map);
            Thread.Sleep(shotDelay);
            if (!camera.SignlShot(out HObject imageTmp))
            {
                msg = "相机拍照失败";
                return false;
            }
            FindMark(imageTmp, modleID, out double rowTmp, out double colTmp, false);
            VisionMotionCalibrate.PlatPointConvert(rowTmp, colTmp, out double x2_Tmp, out double y2_Tmp);
            x2_Top = stageAxisDic[MyStageAxisKey.CHUCK_X].Position() + x2_0 - x2_Tmp;
            y2_Top = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position() + y2_0 - y2_Tmp;

            ReportMessage($"第一步找到{markSum}个结构");

            //找到最顶端Mark点
            for (int i = 1; i <= map.DieRows; i++) {
                if (MotionState == State.Stop) { msg = "用户点击停止Mapping"; return false; }
                stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveRelative(-map.DieHeight);
                stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
                Thread.Sleep(shotDelay);
                if (!camera.SignlShot(out HObject image)) {
                    msg = "相机拍照失败";
                    return false;
                }
                //此处有可能找不到mark点，是正常的
                FindMark(image, modleID, out double row, out double col, false);
                if (row != 0 && col != 0) {
                    //记录下此时的位置信息,即为最顶端Mark点
                    VisionMotionCalibrate.PlatPointConvert(row, col, out double x2_2, out double y2_2);
                    x2_Top = stageAxisDic[MyStageAxisKey.CHUCK_X].Position() + x2_0 - x2_2;
                    y2_Top = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position() + y2_0 - y2_2;
                    markSum++;
                } else {
                    break;
                }
            }
            ReportMessage($"共找到{markSum}个结构");

            HOperatorSet.AngleLl(-y2_Top, x2_Top, -y2_Bottom, x2_Bottom, 0, 0, -map.DieHeight * markSum, 0, out HTuple waferAngle);
            HOperatorSet.TupleDeg(waferAngle, out waferAngle);           

            if ((Math.Abs(waferAngle.D) > 5) || (markSum == 0)) {
                msg = $"晶圆偏差角度过大{waferAngle.D}";
                return false;
            }
            if (Math.Abs(waferAngle.D) > 0.0005) {
                double gap = 0.01;
                if (waferAngle.D > 0) {
                    stageAxisDic[MyStageAxisKey.CHUCK_SZ].MoveRelative(waferAngle.D + gap);
                    stageAxisDic[MyStageAxisKey.CHUCK_SZ].MoveRelative(-gap);
                } else {
                    stageAxisDic[MyStageAxisKey.CHUCK_SZ].MoveRelative(waferAngle.D - gap);
                    stageAxisDic[MyStageAxisKey.CHUCK_SZ].MoveRelative(gap);
                }
            }
            stageAxisDic[MyStageAxisKey.CHUCK_SZ].GuiUpdatePosition();

            ReportMessage($"晶圆角度调整结束，调整角度：{Math.Round(waferAngle.D, 5)}");
            return true;
        }

        public bool AdjustWaferAngle(WaferMapInfo map, HTuple id, double x2_0, double y2_0, out string msg) {
            msg = string.Empty;
            ReportMessage("开始晶圆角度调整");

            var plat = ConfigMgr.LoadVisionMotionPlatInfo();
            VisionMotionCalibrate.GeneratePlatCalibration(plat);
            //识别角度
            camera.SetExposure(map.MarkExposure);
            //AutoFocus(50);
            Thread.Sleep(500);
            camera.SignlShot(out HObject image);
            FindMark(image, id, out double rowStart, out double colStart, true);
            VisionMotionCalibrate.PlatPointConvert(rowStart, colStart, out double x2_1, out double y2_1);
            double x2_Start = stageAxisDic[MyStageAxisKey.CHUCK_X].Position() + x2_0 - x2_1;
            double y2_Start = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position() + y2_0 - y2_1;

            stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveRelative(-map.DieHeight);
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
            Thread.Sleep(500);
            image.Dispose();
            camera.SignlShot(out image);
            FindMark(image, id, out double rowStop, out double colStop, true);
            VisionMotionCalibrate.PlatPointConvert(rowStop, colStop, out double x2_2, out double y2_2);
            double x2_Stop = stageAxisDic[MyStageAxisKey.CHUCK_X].Position() + x2_0 - x2_2;
            double y2_Stop = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position() + y2_0 - y2_2;

            HOperatorSet.AngleLl(-y2_Stop, x2_Stop, -y2_Start, x2_Start, 0, 0, -map.DieHeight, 0, out HTuple waferAngle);
            HOperatorSet.TupleDeg(waferAngle, out waferAngle);

            if (Math.Abs(waferAngle.D) > 5) {
                msg = "晶圆偏差角度过大";
                return false;
            }
            if (Math.Abs(waferAngle.D) > 0.01) {
                stageAxisDic[MyStageAxisKey.CHUCK_SZ].MoveRelative(waferAngle.D);
                stageAxisDic[MyStageAxisKey.CHUCK_SZ].GuiUpdatePosition();
            }

            ReportMessage($"晶圆角度调整结束，调整角度：{Math.Round(waferAngle.D, 5)}");
            return true;
        }

        public void FindMark(HObject image, HTuple id, out double row, out double col, bool throwException = true) {
            row = 0;
            col = 0;
            try {
                VisionMgr.FindPlatMark(image, id, out row, out col);
            } catch (Exception ex) {
                if (throwException) {
                    throw ex;
                }
            }
        }

        public void AutoFocus(double step) {
            int dir = 1;
            HObject image = new HObject();
            double preShape = 0;
            double curShape = 0;

            for (int i = 0; i < 20; i++) {
                image.Dispose();
                Thread.Sleep(500);
                camera.SignlShot(out image);
                curShape = VisionMgr.GetImageShape(image);
                if (preShape == 0)
                    preShape = curShape;

                if (curShape < preShape) {
                    //result.stageAxis.MoveRelative(-dir * step);
                    stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(-dir * step);
                    stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

                    dir = -dir;
                    if (dir == 1 || (dir == -1 && i > 2))
                        break;
                } else {
                    preShape = curShape;
                }
                stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(dir * step);
                stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
            }
        }

        public void MoveToMark(WaferMapInfo map) {
            ReportMessage("开始运动到Mark点位置");

            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_X].MoveAbsolute(map.MarkCcdX); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Y].MoveAbsolute(map.MarkCcdY); });
            task.Add(t2);
            var t3 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(map.MarkCcdZ); });
            task.Add(t3);
            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(map.MarkChuckX); });
            task.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(map.MarkChuckY); });
            task.Add(t5);
            Task.WaitAll(task.ToArray());

            stageAxisDic[MyStageAxisKey.CCD_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();

            ReportMessage("运动到Mark点位置结束");
        }

        public void MoveToDefaultPos(EquipmentCalibrationInfo equipCalibrate) {
            ReportMessage("开始运动到默认位置");

            //stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(-500);
            sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_SEPERATE_HEIGHT, out object seperateHeight);
            string sepLen = seperateHeight as string;
            double zMoveLen = Convert.ToDouble(sepLen);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(-1 * zMoveLen);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.HEIGHT_U].MoveAbsolute(equipCalibrate.Safe_U);
            stageAxisDic[MyStageAxisKey.HEIGHT_U].GuiUpdatePosition();

            List<Task> task = new List<Task>();
            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(equipCalibrate.Base_ChuckX); });
            task.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(equipCalibrate.Base_ChuckY); });
            task.Add(t5);
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_X].MoveAbsolute(equipCalibrate.Base_CcdX); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Y].MoveAbsolute(equipCalibrate.Base_CcdY); });
            task.Add(t2);
            var t3 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(equipCalibrate.Base_CcdZ); });
            task.Add(t3);
            Task.WaitAll(task.ToArray());

            camera.SetExposure(equipCalibrate.Base_Exp);
            eLens.SetZoom(equipCalibrate.Base_Zoom);
            Thread.Sleep(500);
            //增加白平衡设置
            camera.SetParameter("BalanceWhiteAuto", "Continuous");//Off -关闭                
            Thread.Sleep(2000);
            camera.SetParameter("BalanceWhiteAuto", "Off");//Off -关闭

            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.ChuckZ);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

            ReportMessage("运动到默认位置结束");
        }

        public void UpdateGUIPos(List<string> axisList) {
            foreach (var axis in axisList) {
                stageAxisDic[axis].GuiUpdatePosition();
            }
        }

        public bool IsAltimeterInSafePosition(double safePos) {
            double curU = stageAxisDic[MyStageAxisKey.HEIGHT_U].Position();
            if (curU >= safePos && curU != 0) {
                return true;
            }

            return false;
        }

        public void MoveAllToSafePos(EquipmentCalibrationInfo equipCalibrate,bool isScanHeight = false)
        {
            ReportMessage("开始运动到安全位置");

            SetLRAxisSpeedToNormal();

            if (isScanHeight) {
                List<Task> task = new List<Task>();
                var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(equipCalibrate.Safe_CcdZ); });
                task.Add(t1);
                var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.Safe_ChuckZ); });
                task.Add(t2);
                Task.WaitAll(task.ToArray());
            } else {
                if (!IsAltimeterInSafePosition(equipCalibrate.Safe_U)) {
                    List<Task> task = new List<Task>();
                    var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(equipCalibrate.Safe_CcdZ); });
                    task.Add(t1);
                    var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.Safe_ChuckZ); });
                    task.Add(t2);
                    Task.WaitAll(task.ToArray());
                } else {
                    var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.Safe_ChuckZ); });
                    Task.WaitAll(new Task[] { t2 });
                }
            }

            stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(equipCalibrate.Safe_CcdZ);

            stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();            

            List<Task> task2 = new List<Task>();
            var t3 = Task.Run(() => { stageAxisDic[MyStageAxisKey.HEIGHT_U].MoveAbsolute(equipCalibrate.Safe_U); });
            task2.Add(t3);
            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Z].MoveAbsolute(equipCalibrate.Safe_LeftZ); });
            task2.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveAbsolute(equipCalibrate.Safe_RightZ); });
            task2.Add(t5);
            Task.WaitAll(task2.ToArray());
            stageAxisDic[MyStageAxisKey.HEIGHT_U].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();

            List<Task> task3 = new List<Task>();
            var t6 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_X].MoveAbsolute(equipCalibrate.Safe_LeftX); });
            task3.Add(t6);
            var t7 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Y].MoveAbsolute(equipCalibrate.Safe_LeftY); });
            task3.Add(t7);
            var t8 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_X].MoveAbsolute(equipCalibrate.Safe_RightX); });
            task3.Add(t8);
            var t9 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Y].MoveAbsolute(equipCalibrate.Safe_RightY); });
            task3.Add(t9);
            Task.WaitAll(task3.ToArray());
            stageAxisDic[MyStageAxisKey.LEFT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Y].GuiUpdatePosition();

            ReportMessage("运动到安全位置完成");
        }

        public bool AxisHome(string name) {
            return stageAxisDic[name].Homing();
        }

        public bool AxisHome_XH(string name) {
            CompParamInfo info;
            bool bRet = false;

            if (name == MyStageAxisKey.CHUCK_X) {
                info = ConfigMgr.LoadLeadscrwCompInfo("X");
                stageAxisDic[name].SetSoftLimit(0, 1, 0, info.negLimit, info.posLimit);
                bRet = stageAxisDic[name].Homing_XH(info.coderReverse, info.sevonLevel, info.limitPos, info.homeOffset, info.minVel, info.maxVel);
                stageAxisDic[name].SetSoftLimit(1, 1, 0, info.negLimit, info.posLimit);
            } else if (name == MyStageAxisKey.CHUCK_Y) {
                info = ConfigMgr.LoadLeadscrwCompInfo("Y");
                stageAxisDic[name].SetSoftLimit(0, 1, 0, info.negLimit, info.posLimit);
                bRet = stageAxisDic[name].Homing_XH(info.coderReverse, info.sevonLevel, info.limitPos, info.homeOffset, info.minVel, info.maxVel);
                stageAxisDic[name].SetSoftLimit(1, 1, 0, info.negLimit, info.posLimit);
            } else {
                info = ConfigMgr.LoadLeadscrwCompInfo("Z");
                stageAxisDic[name].SetSoftLimit(0, 1, 0, info.negLimit, info.posLimit);
                bRet = stageAxisDic[name].Homing_XH_Z(info.coderReverse, info.sevonLevel, info.limitPos, info.homeOffset, info.minVel, info.maxVel);
                stageAxisDic[name].SetSoftLimit(1, 1, 0, info.negLimit, info.posLimit);
            }

            return bRet;
        }


        public void DisableXYLeadCrewComp() {
            CompParamInfo info;
            info = ConfigMgr.LoadLeadscrwCompInfo("X");
            stageAxisDic[MyStageAxisKey.CHUCK_X].EnableLeadcrewComp(0);

            info = ConfigMgr.LoadLeadscrwCompInfo("Y");
            stageAxisDic[MyStageAxisKey.CHUCK_Y].EnableLeadcrewComp(0);
        }

        

        public void Axis_SetLeadcrewCompConfig(string name) {
            CompParamInfo info;
            if (name == MyStageAxisKey.CHUCK_X) {
                info = ConfigMgr.LoadLeadscrwCompInfo("X");
            } else if (name == MyStageAxisKey.CHUCK_Y) {
                info = ConfigMgr.LoadLeadscrwCompInfo("Y");
            } else {
                info = ConfigMgr.LoadLeadscrwCompInfo("Z");
            }

            string filePath = info.compDataPath;

            List<double> data1 = null;
            List<double> data2 = null;
            GetOffsetData_rtl(filePath, ref data1, ref data2);

            data2.Reverse();

            ///***
            data2 = data1;
            ///***

            int[] pulPos = new int[data1.Count];
            int[] pulNeg = new int[data1.Count];

            for (int i = 0; i < data1.Count; i++) {
                pulPos[i] = (int)(-1 * data1[i] / info.distancePerPulse);
                pulNeg[i] = (int)(-1 * data2[i] / info.distancePerPulse);
            }

            //解析文件
            stageAxisDic[name].EnableLeadcrewComp(1);
            stageAxisDic[name].SetLeadcrewCompConfig(info.numPos, info.posStart, info.posDis, pulPos, pulNeg);
            stageAxisDic[name].EnableLeadcrewComp(1);
        }

        public static void GetOffsetData_rtl(string path, ref List<double> PosList, ref List<double> NegList) {
            PosList = new List<double>();
            NegList = new List<double>();
            List<double> List0 = new List<double>();

            string[] bb;
            double da1;

            var str = File.ReadAllLines(path);
            bool bz = false;
            foreach (var row in str) {
                if (row == "") {
                    continue;
                }
                bb = row.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (bb.Length == 3) {
                    if (bb[0].Contains("Run")) {
                        bz = true;
                        continue;
                    }
                } else {
                    ;
                }

                if (bb[0].Contains("ENVIRONMENT")) {
                    break;
                }

                if (bz) {
                    da1 = Convert.ToDouble(bb[2]);

                    List0.Add(da1);
                }
            }

            for (int i = 0; i < List0.Count / 2; i++) {
                PosList.Add(List0[i]);
            }
            for (int i = List0.Count / 2; i < List0.Count; i++) {
                NegList.Add(List0[i]);
            }
        }

        public bool MoveToMarkPosByWaferType(WaferMapInfo map, out string errMsg) {
            errMsg = string.Empty;

            var equipCalibrate = ConfigMgr.LoadEquipmentCalibration();
            if (equipCalibrate == null) {
                errMsg = "加载校准文件失败";
                return false;
            }

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

            if (!camera.SetExposure(map.MarkExposure)) {
                errMsg = "设置曝光值失败";
                return false;
            }

            if (!eLens.SetZoom(map.MarkZoom)) {
                errMsg = "设置放大倍数失败";
                return false;
            }
            return true;
        }

        public bool MoveToMarkPos(string waferType) {
            return MoveToMarkWithChuckZMove(waferType, out string errMsg);
        }

        public bool MoveToMarkWithChuckZMove(string waferType, out string errMsg) {
            errMsg = string.Empty;

            var equipCalibrate = ConfigMgr.LoadEquipmentCalibration();
            if (equipCalibrate == null) {
                errMsg = "设备校准信息不存在";
                return false;
            }

            var map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            if (map == null) {
                errMsg = "WaferMap类型不存在";
                return false;
            }

            SetSelectedDie(map.MarkDieRowIndex, map.MarkDieColumnIndex);
            SetDieHighLightWithIndex(map.MarkDieRowIndex, map.MarkDieColumnIndex);

            //Z降 500
            //stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(-500);
            sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_SEPERATE_HEIGHT, out object seperateHeight);
            string sepLen = seperateHeight as string;
            double zMoveLen = Convert.ToDouble(sepLen);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(-1 * zMoveLen);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

            //U轴缩到安全位置
            stageAxisDic[MyStageAxisKey.HEIGHT_U].MoveAbsolute(equipCalibrate.Safe_U);

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

            //stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(500);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(zMoveLen);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

            return true;
        }

        public bool MoveToProbeCardTestPos(EquipmentCalibrationInfo equipCalibrate) {
            double curChuckZ = stageAxisDic[MyStageAxisKey.CHUCK_Z].Position(); ;
            double deltaZ = equipCalibrate.ProbeWaferContactZ0 - curChuckZ + equipCalibrate.ProbeCrimpingDepth;

            //Chuck&CCD上移
            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.ProbeWaferContactZ0 + equipCalibrate.ProbeCrimpingDepth);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(deltaZ);
            stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

            return true;
        }

        public bool MoveToProbeCardContactPos(EquipmentCalibrationInfo equipCalibrate) {
            double curChuckZ = stageAxisDic[MyStageAxisKey.CHUCK_Z].Position(); ;
            double deltaZ = equipCalibrate.ProbeWaferContactZ0 - curChuckZ;

            //Chuck&CCD上移
            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.ProbeWaferContactZ0);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(deltaZ);
            stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

            return true;
        }

        public bool MoveToProbeCardSeperatePos(EquipmentCalibrationInfo equipCalibrate) {
            /*
            double curChuckZ = stageAxisDic[MyStageAxisKey.CHUCK_Z].Position(); 
            double deltaZ = equipCalibrate.ChuckZ - curChuckZ;

            //Chuck&CCD下移
            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.ChuckZ);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(deltaZ);
            stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
            */
            sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_SEPERATE_HEIGHT, out object seperateHeight);
            string sepLen = seperateHeight as string;
            double zMoveLen = Convert.ToDouble(sepLen);

            double curChuckZ = stageAxisDic[MyStageAxisKey.CHUCK_Z].Position();
            double deltaZ = equipCalibrate.ProbeWaferContactZ0 - zMoveLen - curChuckZ;

            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.ProbeWaferContactZ0 - zMoveLen);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(deltaZ);
            stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

            return true;
        }

        public bool MoveToCaliSubDie(ItemCalPosInfo itemCalPos, bool isMoveFA, out string errInfo)
        {
            errInfo = string.Empty;

            sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_POSITION_MONITOR_ENABLE, out object enable);
            bool enableMonitor = (bool)enable;
            if (enableMonitor)
            {
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POSITION_MONITOR_ENABLE, false, (key, oldValue) => false);
                DealWithChuckSafePos(false);
            }

            //stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(-500);
            sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_SEPERATE_HEIGHT, out object seperateHeight);
            string sepLen = seperateHeight as string;
            double zMoveLen = Convert.ToDouble(sepLen);
            //stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(-1 * zMoveLen);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(itemCalPos.ChuckZ - zMoveLen);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(itemCalPos.ChuckX); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(itemCalPos.ChuckY); });
            task.Add(t2);

            //左右探针移动先后顺序可能需要根据场景确认下
            if (isMoveFA) {
                var t3 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_X].MoveAbsolute(itemCalPos.LeftX); });
                task.Add(t3);
                var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Y].MoveAbsolute(itemCalPos.LeftY); });
                task.Add(t4);                
                var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_X].MoveAbsolute(itemCalPos.Right_X); });
                task.Add(t5);
                var t6 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Y].MoveAbsolute(itemCalPos.Right_Y); });
                task.Add(t6);                
            }

            Task.WaitAll(task.ToArray());

            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Y].GuiUpdatePosition();            
            stageAxisDic[MyStageAxisKey.RIGHT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Y].GuiUpdatePosition();

            //函数内部判断是否使用图像补偿XY
            if(!WafePosAdjustWithCCD(itemCalPos, out errInfo)) {
                if (enableMonitor) {
                    sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POSITION_MONITOR_ENABLE, true, (key, oldValue) => true);
                    DealWithChuckSafePos(true);
                }
                return false;
            }           

            //Z轴处理，带电容和不带电容补偿两种方式
            sharedObjects.TryGetValue(PrivateSharedObjectKey.IS_CAP_ALTIMETER, out object isCapAm);
            string isCapAltimeter = isCapAm as string;
            if (isCapAltimeter == "0") {
                stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(itemCalPos.ChuckZ);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
#if false
                //临时增加debug代码start
                AdjustHeightWithCapAltimeter(waferType, itemCalPos, out errInfo);
                //AdjustHeightWithCapAltimeterRight(waferType, itemCalPos, out errInfo);
                //临时增加debug代码end
#endif
            } else {
                AdjustHeightWithCapAltimeter(itemCalPos, out errInfo);               
            }

            sharedObjects.TryGetValue(PrivateSharedObjectKey.MONIT_TEST_CONDITION, out object isMonitor);
            string isSave = isMonitor as string;
            if (isSave == "1") {
                RecordHeightInfo(itemCalPos);
            }

            if (enableMonitor) {
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POSITION_MONITOR_ENABLE, true, (key, oldValue) => true);
                DealWithChuckSafePos(true);
            }

            return true;
        }

        public bool WafePosAdjustWithCCD(ItemCalPosInfo itemCalPos, out string errInfo)
        {
            errInfo = string.Empty;

            //wafer map信息
            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_MAP, out var tempObj);            
            if (tempObj == null)
            {
                errInfo = "Map Info Is Empty";
                return false;
            }
            WaferMapInfo map = tempObj as WaferMapInfo;

            if (map.isUseMarkPad)
            {
                if (map.isOcrFirstReticleOnly)
                {
                    if (!StageMarkPadComp(map, itemCalPos, out errInfo))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!StageMarkPadComp(map, out errInfo))
                    {
                        return false;
                    }
                }               
            }

            return true;
        }   
        
        public double GetCurChuckPos()
        {
            return stageAxisDic[MyStageAxisKey.CHUCK_Z].Position();
        }

        public bool DealWithChuckSafePos(bool isLock)
        {
            stageAxisDic[MyStageAxisKey.CHUCK_X].SetSafeHeightLock(isLock);
            stageAxisDic[MyStageAxisKey.CHUCK_Y].SetSafeHeightLock(isLock);

            return true;
        }

        public void moveCcd(double dis)
        {
            stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(dis);
            stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
        }

        public bool DealWithCapAliAlarm(bool isLock) {
            if (isLock) {
                //1，锁定所有轴
                foreach (var stage in stageAxisDic) {
                    stage.Value.SetLockState(true);
                }

                //2，对Z轴进行特殊处理
                stageAxisDic[MyStageAxisKey.LEFT_Z].SetZAxis(true);
                stageAxisDic[MyStageAxisKey.LEFT_Z].EnabelMoveUp(true);

                stageAxisDic[MyStageAxisKey.RIGHT_Z].SetZAxis(true);
                stageAxisDic[MyStageAxisKey.RIGHT_Z].EnabelMoveUp(true);

                stageAxisDic[MyStageAxisKey.CHUCK_Z].SetZAxis(true);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].EnabelMoveUp(false);
            } else {
                foreach(var stage in stageAxisDic) {
                    stage.Value.SetLockState(false);
                }
            }

            return true;
        }

        public void StopAllAxis()
        {
            foreach (var stage in stageAxisDic)
            {
                stage.Value.Stop();
            }
        }

        public bool DoHoming(bool isLeadscrewComp = false) {
            bool bRet = false;
            if (MotionState != State.Ready) {
                ReportMessage($"回零失败:{EValue.NOREADYINFO}");
                return false;
            }

            Task.Run(() => {
                try {
                    MotionState = State.Busy;
                    ReportMessage("开始回零");

                    if (Homing(isLeadscrewComp)) {
                        ReportMessage("回零完成");
                        MessageBoxShow("回零完成");
                        bRet = true;
                    } else {
                        ReportMessage("回零失败");
                        MessageBoxShow("回零失败");
                        bRet = false;
                    }
                } catch (Exception ex) {
                    ReportMessage($"回零异常:{ex.Message}");
                    MessageBoxShow($"回零异常:{ex.Message}");
                    bRet = false;
                } finally {
                    MotionState = State.Ready;
                }
            });

            return bRet;
        }
#if false
        public bool Homing(bool isLeadcrewComp = false) {
            if (!isLeadcrewComp) {

                /*
                AxisHome_XH(MyStageAxisKey.CHUCK_X);
                stageAxisDic[MyStageAxisKey.CHUCK_X].EnableLeadcrewComp(0);
                */

                /*
                AxisHome_XH(MyStageAxisKey.CHUCK_Y);
                stageAxisDic[MyStageAxisKey.CHUCK_Y].EnableLeadcrewComp(0);
                */
                /*
                CompParamInfo info = ConfigMgr.LoadLeadscrwCompInfo("Z");

                stageAxisDic[MyStageAxisKey.CHUCK_Z].SetEncoderDir(info.coderReverse);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].SetHomeOffset(info.homeOffset);
                AxisHome(MyStageAxisKey.CHUCK_Z);
                Thread.Sleep(3000);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].ClearEncoderPos();     
                */
                AxisHome_XH(MyStageAxisKey.CHUCK_Z);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].EnableLeadcrewComp(0);

            }
            else {
                /*
                AxisHome_XH(MyStageAxisKey.CHUCK_X);
                Axis_SetLeadcrewCompConfig(MyStageAxisKey.CHUCK_X);
                */

                /*
                AxisHome_XH(MyStageAxisKey.CHUCK_Y);
                Axis_SetLeadcrewCompConfig(MyStageAxisKey.CHUCK_Y);                
                */

                /*
                CompParamInfo info = ConfigMgr.LoadLeadscrwCompInfo("Z");

                stageAxisDic[MyStageAxisKey.CHUCK_Z].SetEncoderDir(info.coderReverse);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].SetHomeOffset(info.homeOffset);
                AxisHome(MyStageAxisKey.CHUCK_Z);
                Thread.Sleep(3000);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].ClearEncoderPos();  
                */
                AxisHome_XH(MyStageAxisKey.CHUCK_Z);
                Axis_SetLeadcrewCompConfig(MyStageAxisKey.CHUCK_Z);
            }

            return true;
        }
#endif

        public void HomingAllZAxis(bool isLeadcrewComp = false) {
            List<Task> htask = new List<Task>();
            var ht1 = Task.Run(() => AxisHome(MyStageAxisKey.CCD_Z));
            htask.Add(ht1);
            var ht2 = Task.Run(() => AxisHome(MyStageAxisKey.LEFT_Z));
            htask.Add(ht2);
            var ht3 = Task.Run(() => AxisHome(MyStageAxisKey.RIGHT_Z));
            htask.Add(ht3);
            Task.WaitAll(htask.ToArray());
            stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();

#if false
            if (!isLeadcrewComp) {
                ReportMessage("开始Chuck Z轴非补偿模式的回零");
                AxisHome_XH(MyStageAxisKey.CHUCK_Z);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].EnableLeadcrewComp(0);
            } else {
                ReportMessage("开始Chuck Z轴补偿模式的回零");
                AxisHome_XH(MyStageAxisKey.CHUCK_Z);
                Axis_SetLeadcrewCompConfig(MyStageAxisKey.CHUCK_Z);
            }
#endif
            ReportMessage("开始Chuck Z轴非补偿模式的回零");
            AxisHome_XH(MyStageAxisKey.CHUCK_Z);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].EnableLeadcrewComp(0);

            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
        }

        public void HomingLRAxis() {
            stageAxisDic[MyStageAxisKey.LEFT_X].SetAxisSpeed(stageAxisDic[MyStageAxisKey.LEFT_X].speedMinConfig, stageAxisDic[MyStageAxisKey.LEFT_X].speedMaxConfig, stageAxisDic[MyStageAxisKey.LEFT_X].accConfig);
            stageAxisDic[MyStageAxisKey.LEFT_Y].SetAxisSpeed(stageAxisDic[MyStageAxisKey.LEFT_Y].speedMinConfig, stageAxisDic[MyStageAxisKey.LEFT_Y].speedMaxConfig, stageAxisDic[MyStageAxisKey.LEFT_Y].accConfig);
            stageAxisDic[MyStageAxisKey.RIGHT_X].SetAxisSpeed(stageAxisDic[MyStageAxisKey.RIGHT_X].speedMinConfig, stageAxisDic[MyStageAxisKey.RIGHT_X].speedMaxConfig, stageAxisDic[MyStageAxisKey.RIGHT_X].accConfig);
            stageAxisDic[MyStageAxisKey.RIGHT_Y].SetAxisSpeed(stageAxisDic[MyStageAxisKey.RIGHT_Y].speedMinConfig, stageAxisDic[MyStageAxisKey.RIGHT_Y].speedMaxConfig, stageAxisDic[MyStageAxisKey.RIGHT_Y].accConfig);

            List<Task> htask2 = new List<Task>();
            var ht5 = Task.Run(() => AxisHome(MyStageAxisKey.LEFT_X));
            htask2.Add(ht5);
            var ht6 = Task.Run(() => AxisHome(MyStageAxisKey.RIGHT_X));
            htask2.Add(ht6);
            var ht7 = Task.Run(() => AxisHome(MyStageAxisKey.LEFT_Y));
            htask2.Add(ht7);
            var ht8 = Task.Run(() => AxisHome(MyStageAxisKey.RIGHT_Y));
            htask2.Add(ht8);
            Task.WaitAll(htask2.ToArray());
            stageAxisDic[MyStageAxisKey.LEFT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Y].GuiUpdatePosition();
        }

        public void HomingCCDHeight() {
            List<Task> htask3 = new List<Task>();
            var ht11 = Task.Run(() => AxisHome(MyStageAxisKey.HEIGHT_U));
            htask3.Add(ht11);
            var ht12 = Task.Run(() => AxisHome(MyStageAxisKey.CCD_X));
            htask3.Add(ht12);
            var ht13 = Task.Run(() => AxisHome(MyStageAxisKey.CCD_Y));
            htask3.Add(ht13);
            Task.WaitAll(htask3.ToArray());
            stageAxisDic[MyStageAxisKey.HEIGHT_U].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Y].GuiUpdatePosition();
        }

        public void HomingChuckXY(bool isLeadcrewComp = false) {
            if (!isLeadcrewComp) {
                ReportMessage("开始Chuck X轴非补偿模式的回零");
                AxisHome_XH(MyStageAxisKey.CHUCK_X);
                stageAxisDic[MyStageAxisKey.CHUCK_X].EnableLeadcrewComp(0);
                stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();

                ReportMessage("开始Chuck Y轴非补偿模式的回零");
                AxisHome_XH(MyStageAxisKey.CHUCK_Y);
                stageAxisDic[MyStageAxisKey.CHUCK_Y].EnableLeadcrewComp(0);
                stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
            } else {
                ReportMessage("开始Chuck X轴补偿模式的回零");
                AxisHome_XH(MyStageAxisKey.CHUCK_X);
                Axis_SetLeadcrewCompConfig(MyStageAxisKey.CHUCK_X);
                stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();

                ReportMessage("开始Chuck Y轴补偿模式的回零");
                AxisHome_XH(MyStageAxisKey.CHUCK_Y);
                Axis_SetLeadcrewCompConfig(MyStageAxisKey.CHUCK_Y);
                stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
            }
        }

        public void HomingRotateAxis() {
            List<Task> htask4 = new List<Task>();
            var ht14 = Task.Run(() => AxisHome(MyStageAxisKey.LEFT_SX));
            htask4.Add(ht14);
            var ht15 = Task.Run(() => AxisHome(MyStageAxisKey.LEFT_SY));
            htask4.Add(ht15);
            var ht16 = Task.Run(() => AxisHome(MyStageAxisKey.LEFT_SZ));
            htask4.Add(ht16);
            var ht17 = Task.Run(() => AxisHome(MyStageAxisKey.RIGHT_SX));
            htask4.Add(ht17);
            var ht18 = Task.Run(() => AxisHome(MyStageAxisKey.RIGHT_SY));
            htask4.Add(ht18);
            var ht19 = Task.Run(() => AxisHome(MyStageAxisKey.RIGHT_SZ));
            htask4.Add(ht19);
            var ht20 = Task.Run(() => AxisHome(MyStageAxisKey.CHUCK_SZ));
            htask4.Add(ht20);
            Task.WaitAll(htask4.ToArray());
            stageAxisDic[MyStageAxisKey.LEFT_SX].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_SY].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_SZ].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_SX].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_SY].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_SZ].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_SZ].GuiUpdatePosition();
        }

        public void RecoverRotateAxisPos(EquipmentCalibrationInfo eInfo) {
            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_SX].MoveAbsolute(eInfo.BaseItem_LeftSX); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_SY].MoveAbsolute(eInfo.BaseItem_LeftSY); });
            task.Add(t2);
            var t3 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_SZ].MoveAbsolute(eInfo.BaseItem_LeftSZ); });
            task.Add(t3);
            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_SX].MoveAbsolute(eInfo.BaseItem_RightSX); });
            task.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_SY].MoveAbsolute(eInfo.BaseItem_RightSY); });
            task.Add(t5);
            var t6 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_SZ].MoveAbsolute(eInfo.BaseItem_RightSZ); });
            task.Add(t6);
            var t7 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_SZ].MoveAbsolute(eInfo.Base_ChuckSZ); });
            task.Add(t7);
            Task.WaitAll(task.ToArray());

            stageAxisDic[MyStageAxisKey.LEFT_SX].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_SY].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_SZ].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_SX].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_SY].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_SZ].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_SZ].GuiUpdatePosition();
        }

        public void RecoerCCDHeightAxisPos(EquipmentCalibrationInfo eInfo) {
            List<Task> task2 = new List<Task>();
            var t7 = Task.Run(() => { stageAxisDic[MyStageAxisKey.HEIGHT_U].MoveAbsolute(eInfo.Safe_U); });
            task2.Add(t7);
            var t8 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(eInfo.Safe_ChuckZ); });
            task2.Add(t8);
            var t9 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(eInfo.Safe_CcdZ); });
            task2.Add(t9);
            var t10 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_X].MoveAbsolute(eInfo.Base_CcdX); });
            task2.Add(t10);
            var t11 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Y].MoveAbsolute(eInfo.Base_CcdY); });
            task2.Add(t11);
            Task.WaitAll(task2.ToArray());

            stageAxisDic[MyStageAxisKey.HEIGHT_U].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Y].GuiUpdatePosition();
        }

        public void RecoverLRAxis(EquipmentCalibrationInfo eInfo) {
            List<Task> task3 = new List<Task>();
            var t12 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_X].MoveAbsolute(eInfo.Safe_LeftX); });
            task3.Add(t12);
            var t13 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Y].MoveAbsolute(eInfo.Safe_LeftY); });
            task3.Add(t13);
            var t14 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Z].MoveAbsolute(eInfo.Safe_LeftZ); });
            task3.Add(t14);
            var t15 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_X].MoveAbsolute(eInfo.Safe_RightX); });
            task3.Add(t15);
            var t16 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Y].MoveAbsolute(eInfo.Safe_RightY); });
            task3.Add(t16);
            var t17 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveAbsolute(eInfo.Safe_RightZ); });
            task3.Add(t17);
            Task.WaitAll(task3.ToArray());

            stageAxisDic[MyStageAxisKey.LEFT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();
        }

        public void RecoverChuckXY(EquipmentCalibrationInfo eInfo) {
            List<Task> task4 = new List<Task>();
            var t18 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(eInfo.Base_ChuckX); });
            task4.Add(t18);
            var t19 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(eInfo.Base_ChuckY); });
            task4.Add(t19);
            var t20 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_SZ].MoveAbsolute(eInfo.Base_ChuckSZ); });
            task4.Add(t20);
            Task.WaitAll(task4.ToArray());

            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_SZ].GuiUpdatePosition();
        }

        public bool Homing(bool isLeadcrewComp = false) {
            //增加设置轴运动速度命令
            ReportMessage("开始相机、左右轴的Z方向回零");
            HomingAllZAxis(isLeadcrewComp);

            ReportMessage("开始左右轴的XY方向回零");
            HomingLRAxis();

            ReportMessage("开始相机XY方向、测高仪回零");
            HomingCCDHeight();

            ReportMessage("开始Chuck X/Y回零");
            HomingChuckXY(isLeadcrewComp);

            //角度轴回零
            ReportMessage("开始左右轴旋转角度方向的回零");
            HomingRotateAxis();

            //角度轴运动到正限位处，然后回走一半的距离
            DealWithRotateAxis();

            var eInfo = ConfigMgr.LoadEquipmentCalibration();
            if (eInfo != null) {
                ReportMessage("开始恢复机台各轴到初始标定位置");
                RecoverRotateAxisPos(eInfo);
                RecoerCCDHeightAxisPos(eInfo);
                RecoverLRAxis(eInfo);
                RecoverChuckXY(eInfo);
            }

            return true;
        }

        public bool DealWithRotateAxis() {

            //轴运动到正限位
            RotateAxisMoveToPosLimit();

            //读取轴的位置坐标，反向运动一半的距离
            RotateAxisMoveToRevHalfPos();

            return true;
        }

        public bool RotateAxisMoveToPosLimit() {
            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_SX].MoveRelative(20); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_SY].MoveRelative(20); });
            task.Add(t2);
            var t3 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_SZ].MoveRelative(20); });
            task.Add(t3);
            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_SX].MoveRelative(20); });
            task.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_SY].MoveRelative(20); });
            task.Add(t5);
            var t6 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_SZ].MoveRelative(20); });
            task.Add(t6);
            var t7 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_SZ].MoveRelative(25); });
            task.Add(t7);
            Task.WaitAll(task.ToArray());

            stageAxisDic[MyStageAxisKey.LEFT_SX].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_SY].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_SZ].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_SX].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_SY].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_SZ].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_SZ].GuiUpdatePosition();

            return true;
        }

        public bool RotateAxisMoveToRevHalfPos() {
            double pos_lsx = stageAxisDic[MyStageAxisKey.LEFT_SX].Position();
            double pos_lsy = stageAxisDic[MyStageAxisKey.LEFT_SY].Position();
            double pos_lsz = stageAxisDic[MyStageAxisKey.LEFT_SZ].Position();
            double pos_rsx = stageAxisDic[MyStageAxisKey.RIGHT_SX].Position();
            double pos_rsy = stageAxisDic[MyStageAxisKey.RIGHT_SY].Position();
            double pos_rsz = stageAxisDic[MyStageAxisKey.RIGHT_SZ].Position();
            double pos_chucksz = stageAxisDic[MyStageAxisKey.CHUCK_SZ].Position();
            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_SX].MoveRelative(-pos_lsx / 2.0); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_SY].MoveRelative(-pos_lsy / 2.0); });
            task.Add(t2);
            var t3 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_SZ].MoveRelative(-pos_lsz / 2.0); });
            task.Add(t3);

            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_SX].MoveRelative(-pos_rsx / 2.0); });
            task.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_SY].MoveRelative(-pos_rsy / 2.0); });
            task.Add(t5);
            var t6 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_SZ].MoveRelative(-pos_rsz / 2.0); });
            task.Add(t6);
            var t7 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_SZ].MoveRelative(-pos_chucksz / 2.0); });
            task.Add(t7);

            Task.WaitAll(task.ToArray());

            stageAxisDic[MyStageAxisKey.LEFT_SX].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_SY].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_SZ].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_SX].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_SY].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_SZ].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_SZ].GuiUpdatePosition();

            return true;
        }

        public bool MoveToDut(ItemCalPosInfo itemCalPos, out string errInfo) {
            errInfo = string.Empty;
            //string firstReticleName = "F02";
            //string firstSubDieName = "D1";            

            sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_POSITION_MONITOR_ENABLE, out object enable);
            bool enableMonitor = (bool)enable;
            if (enableMonitor)
            {
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POSITION_MONITOR_ENABLE, false, (key, oldValue) => false);
                DealWithChuckSafePos(false);
            }

            //stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(-500);
            sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_SEPERATE_HEIGHT, out object seperateHeight);
            string sepLen = seperateHeight as string;
            double zMoveLen = Convert.ToDouble(sepLen);
            //stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(-1 * zMoveLen);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(itemCalPos.ChuckZ - zMoveLen);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(itemCalPos.ChuckX); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(itemCalPos.ChuckY); });
            task.Add(t2);
            var t3 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_X].MoveAbsolute(itemCalPos.LeftX); });
            task.Add(t3);
            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Y].MoveAbsolute(itemCalPos.LeftY); });
            task.Add(t4);            
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_X].MoveAbsolute(itemCalPos.Right_X); });
            task.Add(t5);
            var t6 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Y].MoveAbsolute(itemCalPos.Right_Y); });
            task.Add(t6);          
            Task.WaitAll(task.ToArray());

            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Y].GuiUpdatePosition();            
            stageAxisDic[MyStageAxisKey.RIGHT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Y].GuiUpdatePosition();           

            //函数内部判断是否使用图像补偿XY
            if (!WafePosAdjustWithCCD(itemCalPos, out errInfo)) {
                if (enableMonitor)
                {
                    sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POSITION_MONITOR_ENABLE, true, (key, oldValue) => true);
                    DealWithChuckSafePos(true);
                }
                return false;
            }

            sharedObjects.TryGetValue(PrivateSharedObjectKey.IS_CAP_ALTIMETER, out object isCapAm);
            string isCapAltimeter = isCapAm as string;
            if (isCapAltimeter == "0") {
                stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(itemCalPos.ChuckZ);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
                //临时增加debug代码start
#if false
                if ( (firstReticleName == itemCalPos.DieOrdinate)&& (firstSubDieName == itemCalPos.SubDieName)) {
                    AdjustHeightWithCapAltimeter(waferType, itemCalPos, out errInfo);
                   // AdjustHeightWithCapAltimeterRight(waferType, itemCalPos, out errInfo);
                }        
#endif
                //临时增加debug代码end
            } else {
                AdjustHeightWithCapAltimeter(itemCalPos, out errInfo);                
            }

            sharedObjects.TryGetValue(PrivateSharedObjectKey.MONIT_TEST_CONDITION, out object isMonitor);
            string isSave = isMonitor as string;
            if (isSave == "1")
            {
                RecordHeightInfo(itemCalPos);
            }

            if (enableMonitor)
            {
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POSITION_MONITOR_ENABLE, true, (key, oldValue) => true);
                DealWithChuckSafePos(true);
            }

            return true;         
        }

        public bool StageMarkPadComp(WaferMapInfo map, out string errInfo)
        {
            errInfo = string.Empty;

            if (!map.isUseMarkPad)
            {
                return true;
            }

            //1，相机下降500um
            //stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(-500);
            sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_SEPERATE_HEIGHT, out object seperateHeight);
            string sepLen = seperateHeight as string;
            double zMoveLen = Convert.ToDouble(sepLen);
            stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(-1 * zMoveLen);
            stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

            try
            {
                //2，相机拍照获取标记点和实际像素偏移量
                HOperatorSet.ReadShapeModel($"Configuration\\Wafer\\{map.Type}_Pad.shm", out HTuple id);
                VisionMotionCalibrate.PlatPointConvert(map.MarkPadRow, map.MarkPadColumn, out double x2_0, out double y2_0);

                camera.SetExposure(map.MarkExposure);
                //AutoFocus(50);
                Thread.Sleep(200);
                camera.SignlShot(out HObject image);

                if (!VisionMgr.FindPlatMark(image, id, out double rowStart, out double colStart))
                {
                    //采用第二模板
                    HOperatorSet.ReadShapeModel($"Configuration\\Wafer\\{map.Type}_Pad2.shm", out id);
                    VisionMotionCalibrate.PlatPointConvert(map.MarkPadRow_2, map.MarkPadColumn_2, out x2_0, out y2_0);
                    camera.SignlShot(out image);

                    if (!VisionMgr.FindPlatMark(image, id, out rowStart, out colStart))
                    {
                        errInfo = "查找PAD Mark点失败";
                        return false;
                    }
                }
                VisionMotionCalibrate.PlatPointConvert(rowStart, colStart, out double x2_1, out double y2_1);

                //3，计算出距离变化量
                double x2_offset = x2_0 - x2_1;
                double y2_offset = y2_0 - y2_1;

                string msg = string.Format(" X补偿量:{0},Y轴补偿量:{1}", x2_offset.ToString("f2"), y2_offset.ToString("f2"));
                ReportMessage(msg);

                if (Math.Abs(x2_offset) > 30 || Math.Abs(y2_offset) > 30)
                {
                    errInfo = "XY轴补偿量过大";
                    return false;
                }

                //4，XY移动偏移量
                List<Task> task = new List<Task>();
                var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveRelative(x2_offset); });
                task.Add(t1);
                var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveRelative(y2_offset); });
                task.Add(t2);
                Task.WaitAll(task.ToArray());

                stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
                stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
            }
            catch (Exception ex)
            {
                LOGGER.Error(ex);
                return false;
            }
            finally
            {
                //5，相机抬起来
                //stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(500);
                stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(zMoveLen);
                stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
            }

            return true;
        }

        public bool StageMarkPadComp(WaferMapInfo map, ItemCalPosInfo itemCalPos, out string errInfo)
        {
            errInfo = string.Empty;
            double x2_offset = 0;
            double y2_offset = 0;
            double zMoveLen = 0;

            if (!map.isUseMarkPad)
            {
                return true;
            }

            string reticleName = itemCalPos.DieOrdinate;
            sharedObjects.TryGetValue(PrivateSharedObjectKey.PAD_COMPENSATE_DIC, out object temp);
            if (temp == null)
            {
                return false;
            }
            Dictionary<string, CompensateData> dicCompData = (Dictionary<string, CompensateData>)temp;

            try
            {
                //如果当前Reticle已经视校过，则直接调用相应的补偿量;否则视教
                if (dicCompData.ContainsKey(reticleName))
                {
                    x2_offset = dicCompData[reticleName].xData;
                    y2_offset = dicCompData[reticleName].yData;
                }
                else
                {
                    //1，相机下降500um
                    //stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(-500);
                    sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_SEPERATE_HEIGHT, out object seperateHeight);
                    string sepLen = seperateHeight as string;
                    zMoveLen = Convert.ToDouble(sepLen);
                    stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(-1 * zMoveLen);
                    stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

                    //2，相机拍照获取标记点和实际像素偏移量
                    HOperatorSet.ReadShapeModel($"Configuration\\Wafer\\{map.Type}_Pad.shm", out HTuple id);
                    VisionMotionCalibrate.PlatPointConvert(map.MarkPadRow, map.MarkPadColumn, out double x2_0, out double y2_0);

                    camera.SetExposure(map.MarkExposure);
                    //AutoFocus(50);
                    Thread.Sleep(200);
                    camera.SignlShot(out HObject image);

                    if (!VisionMgr.FindPlatMark(image, id, out double rowStart, out double colStart))
                    {
                        //采用第二模板
                        HOperatorSet.ReadShapeModel($"Configuration\\Wafer\\{map.Type}_Pad2.shm", out id);
                        VisionMotionCalibrate.PlatPointConvert(map.MarkPadRow_2, map.MarkPadColumn_2, out x2_0, out y2_0);
                        camera.SignlShot(out image);

                        if (!VisionMgr.FindPlatMark(image, id, out rowStart, out colStart))
                        {
                            errInfo = "查找PAD Mark点失败";
                            return false;
                        }
                    }
                    VisionMotionCalibrate.PlatPointConvert(rowStart, colStart, out double x2_1, out double y2_1);

                    //3，计算出距离变化量
                    x2_offset = x2_0 - x2_1;
                    y2_offset = y2_0 - y2_1;

                    string msg = string.Format(" X补偿量:{0},Y轴补偿量:{1}", x2_offset.ToString("f2"), y2_offset.ToString("f2"));
                    ReportMessage(msg);

                    if (Math.Abs(x2_offset) > 30 || Math.Abs(y2_offset) > 30)
                    {
                        errInfo = "XY轴补偿量过大";
                        return false;
                    }

                    CompensateData compData = new CompensateData(x2_offset, y2_offset);
                    dicCompData.Add(reticleName, compData);
                }

                //4，XY移动偏移量
                List<Task> task = new List<Task>();
                var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveRelative(x2_offset); });
                task.Add(t1);
                var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveRelative(y2_offset); });
                task.Add(t2);
                Task.WaitAll(task.ToArray());

                stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
                stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
            }
            catch (Exception ex)
            {
                LOGGER.Error(ex);
                return false;
            }
            finally
            {
                if (!dicCompData.ContainsKey("reticleName"))
                {
                    //5，相机抬起来
                    //stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(500);
                    stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(zMoveLen);
                }

                stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
            }

            return true;
        }

        public bool AdjustHeightWithCapAltimeter(string waferType, ItemCalPosInfo itemCalPos, out string errInfo) {
            errInfo = string.Empty;

            //加载wafermap，获取参考芯片的Chuck高度，LeftZ轴高度，电容测高仪基准高度
            sharedObjects.TryGetValue(PrivateSharedObjectKey.LEFT_CAP_HEIGHT, out object value);
            string baseCapValue = value.ToString();
            double baseCapHeight = Convert.ToDouble(baseCapValue);

            sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_POS_PRE, out value);
            string chuckPos = value.ToString();
            double waferBaseHeight = Convert.ToDouble(chuckPos);   

            var map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            double baseLeftZ = map.Height_LeftZ;

            sharedObjects.TryGetValue(PrivateSharedObjectKey.EQUIPMENT_CALIBRATION_FILE, out object info);
            EquipmentCalibrationInfo equipCalibrateInfo = info as EquipmentCalibrationInfo;
            if (null == equipCalibrateInfo) {
                errInfo = "加载校准文件失败";
                ReportMessage(errInfo);
                return false;
            }

            //读取电容测高仪的值
            if (!GetHeightWithCapAltimeterUseAverage(Cap_Altimeter_Channel.Left_Channel, 10, 2000, out double capHeight)) {
                errInfo = "Read Channel2 CapAltimeter Value Failed";
                ReportMessage(errInfo);
                return false;
            }

            double gap = capHeight - baseCapHeight;
            if (Math.Abs(gap) <= equipCalibrateInfo.CapAltAdjustLimit) {
                return true;
            }

            if (Math.Abs(gap) > equipCalibrateInfo.CapAltAdjustStopThresh) {
                errInfo = $"After 1th Adjust Height Gap {gap} is too big ";
                ReportMessage(errInfo);
                return false;
            }

            //8:如果高度超过+-1um，从步骤3开始重复走，最多重试3次，超过3次认为异常
            for (int i = 0; i < 3; i++) {
                stageAxisDic[MyStageAxisKey.LEFT_Z].MoveRelative(-1 * gap);
                stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();

                //5:回读此时电容测高仪数据
                if (!GetHeightWithCapAltimeterUseAverage(Cap_Altimeter_Channel.Left_Channel, 10, 1000, out capHeight)) {
                    errInfo = "Read Channel2 CapAltimeter Value Failed";
                    ReportMessage(errInfo);
                    return false;
                }
                string reportMsg = $"{itemCalPos.DieOrdinate} {itemCalPos.SubDieName} X:{itemCalPos.ChuckX} Y:{itemCalPos.ChuckY} Z:{itemCalPos.ChuckZ} CapHeight:{capHeight} BaseHeight:{baseCapHeight}";
                ReportMessage(reportMsg);

                //6:计算此时测高仪高度和第一个标定位置的高度偏差
                gap = capHeight - baseCapHeight;

                if (Math.Abs(gap) > equipCalibrateInfo.CapAltAdjustStopThresh) {
                    errInfo = $"After 1th Adjust Height Gap {gap} is too big ";
                    ReportMessage(errInfo);
                    return false;
                }

                //7:如果高度偏差小于+-1um，结束
                if (Math.Abs(gap) <= equipCalibrateInfo.CapAltAdjustLimit) {
                    //double ZPos = stageAxisDic[MyStageAxisKey.CHUCK_Z].Position();
                    //sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POS_PRE, ZPos, (key, oldValue) => ZPos);
                    return true;
                }
            }

            return false;
        }

        public bool AdjustHeightWithCapAltimeterRight(string waferType, ItemCalPosInfo itemCalPos, out string errInfo)
        {
            errInfo = string.Empty;

            //加载wafermap，获取参考芯片的Chuck高度，LeftZ轴高度，电容测高仪基准高度
            sharedObjects.TryGetValue(PrivateSharedObjectKey.RIGHT_CAP_HEIGHT, out object value);
            string baseCapValue = value.ToString();
            double baseCapHeight = Convert.ToDouble(baseCapValue);

            sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_POS_PRE, out value);
            string chuckPos = value.ToString();
            double waferBaseHeight = Convert.ToDouble(chuckPos);

            var map = ConfigMgr.LoadWaferMapInfoByType(waferType);
            double baseZ = map.Height_RightZ;

            sharedObjects.TryGetValue(PrivateSharedObjectKey.EQUIPMENT_CALIBRATION_FILE, out object info);
            EquipmentCalibrationInfo equipCalibrateInfo = info as EquipmentCalibrationInfo;
            if (null == equipCalibrateInfo)
            {
                errInfo = "加载校准文件失败";
                ReportMessage(errInfo);
                return false;
            }

            //读取电容测高仪的值
            if (!GetHeightWithCapAltimeterUseAverage(Cap_Altimeter_Channel.Right_Channel, 10, 2000, out double capHeight))
            {
                errInfo = "Read Channel2 CapAltimeter Value Failed";
                ReportMessage(errInfo);
                return false;
            }

            double gap = capHeight - baseCapHeight;
            if (Math.Abs(gap) <= equipCalibrateInfo.CapAltAdjustLimit)
            {
                return true;
            }

            if (Math.Abs(gap) > equipCalibrateInfo.CapAltAdjustStopThresh)
            {
                errInfo = $"After 1th Adjust Height Gap {gap} is too big ";
                ReportMessage(errInfo);
                return false;
            }

            //8:如果高度超过+-1um，从步骤3开始重复走，最多重试3次，超过3次认为异常
            for (int i = 0; i < 3; i++)
            {
                stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveRelative(-1 * gap);
                stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();

                //5:回读此时电容测高仪数据
                if (!GetHeightWithCapAltimeterUseAverage(Cap_Altimeter_Channel.Right_Channel, 10, 1000, out capHeight))
                {
                    errInfo = "Read Channel2 CapAltimeter Value Failed";
                    ReportMessage(errInfo);
                    return false;
                }
                string reportMsg = $"{itemCalPos.DieOrdinate} {itemCalPos.SubDieName} X:{itemCalPos.ChuckX} Y:{itemCalPos.ChuckY} Z:{itemCalPos.ChuckZ} CapHeight:{capHeight} BaseHeight:{baseCapHeight}";
                ReportMessage(reportMsg);

                //6:计算此时测高仪高度和第一个标定位置的高度偏差
                gap = capHeight - baseCapHeight;

                if (Math.Abs(gap) > equipCalibrateInfo.CapAltAdjustStopThresh)
                {
                    errInfo = $"After 1th Adjust Height Gap {gap} is too big ";
                    ReportMessage(errInfo);
                    return false;
                }

                //7:如果高度偏差小于+-1um，结束
                if (Math.Abs(gap) <= equipCalibrateInfo.CapAltAdjustLimit)
                {
                    //double ZPos = stageAxisDic[MyStageAxisKey.CHUCK_Z].Position();
                    //sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POS_PRE, ZPos, (key, oldValue) => ZPos);
                    return true;
                }
            }

            return false;
        }

        public bool AdjustHeightWithCapAltimeter(ItemCalPosInfo itemCalPos, out string errInfo)
        {
            errInfo = string.Empty;
            string baseCapValue = "0";
            double baseCapHeight = 0;
            string reportMsg = string.Empty;
            int capAltChannel = Cap_Altimeter_Channel.Left_Channel;

            sharedObjects.TryGetValue(PrivateSharedObjectKey.EQUIPMENT_CALIBRATION_FILE, out object info);
            EquipmentCalibrationInfo equipCalibrateInfo = info as EquipmentCalibrationInfo;
            if (null == equipCalibrateInfo) {
                errInfo = "加载校准文件失败";
                ReportMessage(errInfo);
                return false;
            }

            sharedObjects.TryGetValue(PrivateSharedObjectKey.USE_LEFT_CAP_ALTIMETER, out object isLeftCapAm);
            string isLeftCapAltimeter = isLeftCapAm as string;
            if (isLeftCapAltimeter == "1")
            {
                capAltChannel = Cap_Altimeter_Channel.Left_Channel;
                sharedObjects.TryGetValue(PrivateSharedObjectKey.LEFT_CAP_HEIGHT, out object valueL);
                baseCapValue = valueL.ToString();
            }
            else
            {
                capAltChannel = Cap_Altimeter_Channel.Right_Channel;
                sharedObjects.TryGetValue(PrivateSharedObjectKey.RIGHT_CAP_HEIGHT, out object valueR);
                baseCapValue = valueR.ToString();
            }
            
            baseCapHeight = Convert.ToDouble(baseCapValue);
            //1:托盘抬高到第一个标定位置下方30um处,计算坐标时高度默认就是这个值
            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(itemCalPos.ChuckZ);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
            
            double capHeight = 0;
            string chuckPos = "0";
            //2:读取此时电容测高仪数据
            if (!GetHeightWithCapAltimeterUseAverage(capAltChannel, 5, 300,out capHeight))
            {
                errInfo = "Read Channel2 CapAltimeter Value Failed";
                ReportMessage(errInfo);
                return false;                    
            }
            reportMsg = $"{itemCalPos.DieOrdinate} {itemCalPos.SubDieName} X:{itemCalPos.ChuckX} Y:{itemCalPos.ChuckY} Z:{itemCalPos.ChuckZ} CapHeight:{capHeight} BaseHeight:{baseCapHeight}";
            ReportMessage(reportMsg);

            //2.1 如果测高仪数据超过标定高度+5um，则认为此处测高仪已经处于边缘，此时直接使用上一个芯片时的Chuck高度数据
            if (capHeight > (baseCapHeight + equipCalibrateInfo.CapAltAdjustHeight + equipCalibrateInfo.CapAmTolerance))
            {
                sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_POS_PRE, out object value);
                chuckPos = value.ToString();
                stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(Convert.ToDouble(chuckPos));
                stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
                return true;
            }
            //2.2 反之，按照步骤3走
            else
            {
                //3:计算此时测高仪高度和第一个标定位置的高度偏差
                double gap = capHeight - baseCapHeight;               
                //4:直接移动偏移量
                stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(gap);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

                //5:回读此时电容测高仪数据
                if (!GetHeightWithCapAltimeterUseAverage(capAltChannel, 5, 50, out capHeight))
                {
                    errInfo = "Read Channel2 CapAltimeter Value Failed";
                    ReportMessage(errInfo);
                    return false;
                }
                reportMsg = $"{itemCalPos.DieOrdinate} {itemCalPos.SubDieName} X:{itemCalPos.ChuckX} Y:{itemCalPos.ChuckY} Z:{itemCalPos.ChuckZ} CapHeight:{capHeight} BaseHeight:{baseCapHeight}";
                ReportMessage(reportMsg);

                //6:计算此时测高仪高度和第一个标定位置的高度偏差
                gap = capHeight - baseCapHeight;

                //7:如果高度偏差小于+-2um，结束
                if (Math.Abs(gap) <= equipCalibrateInfo.CapAltAdjustLimit)
                {
                    double ZPos = stageAxisDic[MyStageAxisKey.CHUCK_Z].Position();
                    sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POS_PRE, ZPos,(key,oldValue)=>ZPos);
                    return true;
                }
                else
                {
                    if (Math.Abs(gap) > equipCalibrateInfo.CapAltAdjustStopThresh)
                    {
                        errInfo = $"After 1th Adjust Height Gap {gap} is too big ";
                        ReportMessage(errInfo);
                        return false;
                    }

                    //8:如果高度超过+-2um，从步骤3开始重复走，最多重试3次，超过3次认为异常
                    for (int i = 0; i < 3; i++)
                    {
                        stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(gap);
                        stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

                        //5:回读此时电容测高仪数据
                        if (!GetHeightWithCapAltimeterUseAverage(capAltChannel, 5, 50, out capHeight))
                        {
                            errInfo = "Read Channel2 CapAltimeter Value Failed";
                            ReportMessage(errInfo);
                            return false;
                        }
                        reportMsg = $"{itemCalPos.DieOrdinate} {itemCalPos.SubDieName} X:{itemCalPos.ChuckX} Y:{itemCalPos.ChuckY} Z:{itemCalPos.ChuckZ} CapHeight:{capHeight} BaseHeight:{baseCapHeight}";
                        ReportMessage(reportMsg);

                        //6:计算此时测高仪高度和第一个标定位置的高度偏差
                        gap = capHeight - baseCapHeight;

                        if (Math.Abs(gap) > equipCalibrateInfo.CapAltAdjustStopThresh)
                        {
                            errInfo = $"After 1th Adjust Height Gap {gap} is too big ";
                            ReportMessage(errInfo);
                            return false;
                        }

                        //7:如果高度偏差小于+-2um，结束
                        if (Math.Abs(gap) <= equipCalibrateInfo.CapAltAdjustLimit)
                        {
                            double ZPos = stageAxisDic[MyStageAxisKey.CHUCK_Z].Position();
                            sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POS_PRE, ZPos, (key, oldValue) => ZPos);
                            return true;
                        }                        
                    }
                }
            }

            return false;
        }

        public bool isCapAmPosValid(EquipmentCalibrationInfo info, ItemCalPosInfo itemCalPos)
        {
            double dis = Math.Sqrt((itemCalPos.ChuckX - info.Base_ChuckX) * (itemCalPos.ChuckX - info.Base_ChuckX) + (itemCalPos.ChuckY - info.Base_ChuckY) * (itemCalPos.ChuckY - info.Base_ChuckY));
            if(dis > 85000)
            {
                return false;
            }

            return true;
        }

        public void RecordHeightInfo(ItemCalPosInfo itemCalPos) {
            try {
                string path = "HeightRecord";
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }

                string file = path + "\\HeightInfo_MoveToDut.csv";
                if (!File.Exists(file)) {
                    File.AppendAllText(file, "Date,ReticleName,DieName,ChuckX,ChuckY,ChuckZ,LeftX,LeftY,ChuckX_Read,ChuckY_Read,ChuckZ_Read,LeftX_Read,LeftY_Read,LeftZ_Read,Chuck_T,Chuck_G1,Chuck_G2,Chuck_G3,Chuck_G4,Height_Laser,Height_CapL,Height_CapR,PowerMonitor" + Environment.NewLine);
                }

                double ChuckX = Math.Round(itemCalPos.ChuckX, 2);
                double ChuckY = Math.Round(itemCalPos.ChuckY, 2);
                double ChuckZ = Math.Round(itemCalPos.ChuckZ, 2);
                double LeftX = Math.Round(itemCalPos.LeftX, 2);
                double LeftY = Math.Round(itemCalPos.LeftY, 2);
                double ChuckX_Read = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2);
                double ChuckY_Read = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2);
                double ChuckZ_Read = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Z].Position(), 2);
                double LeftX_Read = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_X].Position(), 2);
                double LeftY_Read = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Y].Position(), 2);
                double LeftZ_Read = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Z].Position(), 2);
                double ChuckT_Read = 0;
                double GratT1_Read = 0;
                double GratT2_Read = 0;
                double GratT3_Read = 0;
                double GratT4_Read = 0;
                double Height_LD = 0;
                double Height_CapL = 0;
                double Height_CapR = 0;
                double PowerMonitor = 0;

                GetOmronTemp(1, out ChuckT_Read);
#if false
                GetGrattingTemp(1, out GratT1_Read);
                GetGrattingTemp(2, out GratT2_Read); 
                GetGrattingTemp(3, out GratT3_Read);
                GetGrattingTemp(4, out GratT4_Read);
#endif
                GetGratTemperature(out GratT1_Read, out GratT2_Read, out GratT3_Read, out GratT4_Read);
                GetHeightWithLdAltimeter(1, out Height_LD);
                Thread.Sleep(1000);
                GetHeightWithCapAltimeter(Cap_Altimeter_Channel.Left_Channel, out Height_CapL);
                GetHeightWithCapAltimeter(Cap_Altimeter_Channel.Right_Channel, out Height_CapR);
                PowerMonitor = opm.OpmFetchPower(Power2_Slot, Power2_Channel);

                ChuckT_Read = Math.Round(ChuckT_Read * tecCoeffK - tecCoeffB,1);

                string reticleName = itemCalPos.DieOrdinate;
                string dieName = itemCalPos.SubDieName;

                File.AppendAllText(file, $"{DateTime.Now.ToString()},{reticleName},{dieName},{ChuckX},{ChuckY},{ChuckZ},{LeftX},{LeftY},{ChuckX_Read},{ChuckY_Read},{ChuckZ_Read},{LeftX_Read},{LeftY_Read},{LeftZ_Read},{ChuckT_Read},{GratT1_Read},{GratT2_Read},{GratT3_Read},{GratT4_Read},{Height_LD},{Height_CapL},{Height_CapR},{PowerMonitor}" + Environment.NewLine);
            } catch (Exception ex) {
                LOGGER.Error(ex.Message);                   
            }
        }

        public void RecordHeightInfo() {
            try {
                string path = "HeightRecord";
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }

                string file = path + "\\HeightInfo_MoveToDut.csv";
                if (!File.Exists(file)) {
                    File.AppendAllText(file, "Date,ReticleName,DieName,ChuckX,ChuckY,ChuckZ,LeftX,LeftY,ChuckX_Read,ChuckY_Read,ChuckZ_Read,LeftX_Read,LeftY_Read,LeftZ_Read,Chuck_T,Chuck_G1,Chuck_G2,Chuck_G3,Chuck_G4,Height_Laser,Height_CapL,Height_CapR,PowerMonitor" + Environment.NewLine);
                }

                double ChuckX = 0;
                double ChuckY = 0;
                double ChuckZ = 0;
                double LeftX = 0;
                double LeftY = 0;
                double ChuckX_Read = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2);
                double ChuckY_Read = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2);
                double ChuckZ_Read = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Z].Position(), 2);
                double LeftX_Read = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_X].Position(), 2);
                double LeftY_Read = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Y].Position(), 2);
                double LeftZ_Read = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Z].Position(), 2);
                double ChuckT_Read = 0;
                double GratT1_Read = 0;
                double GratT2_Read = 0;
                double GratT3_Read = 0;
                double GratT4_Read = 0;
                double Height_LD = 0;
                double Height_CapL = 0;
                double Height_CapR = 0;
                double PowerMonitor = 0;
                /*
                    tecChuck.GetTemp(1, out ChuckT_Read);
                    tecGrat.GetTemp(1, out GratT1_Read);
                    tecGrat.GetTemp(2, out GratT2_Read);
                    tecGrat.GetTemp(3, out GratT3_Read);
                    tecGrat.GetTemp(4, out GratT4_Read);
                    altimeterLD.GetHeight(1, out Height_LD);                
                    altimeterCap.GetHeight(2, out Height_CapL);
                    altimeterCap.GetHeight(1, out Height_CapR);
                */
                GetOmronTemp(1, out ChuckT_Read);
                GetGrattingTemp(1, out GratT1_Read);
                GetGrattingTemp(2, out GratT2_Read);
                GetGrattingTemp(3, out GratT3_Read);
                GetGrattingTemp(4, out GratT4_Read);
                GetHeightWithLdAltimeter(1, out Height_LD);
                GetHeightWithCapAltimeter(Cap_Altimeter_Channel.Left_Channel, out Height_CapL);
                GetHeightWithCapAltimeter(Cap_Altimeter_Channel.Right_Channel, out Height_CapR);
                PowerMonitor = opm.OpmFetchPower(Power2_Slot, Power2_Channel);

                ChuckT_Read = Math.Round(ChuckT_Read * tecCoeffK - tecCoeffB,1);

                string reticleName = "";
                string dieName = "";

                File.AppendAllText(file, $"{DateTime.Now.ToString()},{reticleName},{dieName},{ChuckX},{ChuckY},{ChuckZ},{LeftX},{LeftY},{ChuckX_Read},{ChuckY_Read},{ChuckZ_Read},{LeftX_Read},{LeftY_Read},{LeftZ_Read},{ChuckT_Read},{GratT1_Read},{GratT2_Read},{GratT3_Read},{GratT4_Read},{Height_LD},{Height_CapL},{Height_CapR},{PowerMonitor}" + Environment.NewLine);
            } catch (Exception ex) {
                LOGGER.Error(ex.Message);
            }
        }

        public bool NeedHoming() {
            bool res = true;
            foreach (var item in stageAxisDic.Values) {
                if (Math.Abs(item.Position()) > 1) {
                    res = false;
                    break;
                }
            }

            return res;
        }

        public bool AutoClearProber(Action<ClearProberInfo> updateCtl, out string error) {
            error = string.Empty;

            var proberInfo = ConfigMgr.LoadClearProberInfo();
            var paperInfo = proberInfo.PaperPosList.FirstOrDefault(t => !t.IsUsed);
            if (paperInfo == null) {
                error = "清针纸位置已经使用完。";
                LOGGER.Warn("AutoClearProber();" + error);
                return false;
            }

            //加载机台校准信息
            var equipCalibrateInfo = ConfigMgr.LoadEquipmentCalibration();
            //运动到安全位置
            MoveAllToSafePos(equipCalibrateInfo);

            //move to clean position
            MoveToClearProber(proberInfo, -200);

            double z2Up = proberInfo.ChuckZ + proberInfo.Depth;
            double z2Down = proberInfo.ChuckZ - 200;
            double baseX2 = stageAxisDic[MyStageAxisKey.CHUCK_X].Position();
            double baseY2 = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position();

            for (int i = 0; i < proberInfo.Times; i++) {
                var nextPaperInfo = proberInfo.PaperPosList.FirstOrDefault(t => !t.IsUsed);
                if (nextPaperInfo == null) {
                    error = "清针纸位置已经使用完。";
                    LOGGER.Warn("AutoClearProber();" + error);
                    return false;
                }

                double deltaX2 = -(nextPaperInfo.ColumnIndex) * proberInfo.ColumnGap;
                double deltaY2 = (nextPaperInfo.RowIndex) * proberInfo.RowGap;

                List<Task> task = new List<Task>();
                var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(baseX2 + deltaX2); });
                task.Add(t1);
                var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(baseY2 + deltaY2); });
                task.Add(t2);
                Task.WaitAll(task.ToArray());

                stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
                stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();

                Thread.Sleep(500);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(z2Up);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
                Thread.Sleep(500);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(z2Down);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
                nextPaperInfo.IsUsed = true;
            }

            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrateInfo.Safe_ChuckZ);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

            ConfigMgr.SaveClearProberInfo(proberInfo);
            updateCtl(proberInfo);

            return true;
        }

        private void MoveToClearProber(ClearProberInfo proberInfo, double deltaZ2) {
            var equipCalibrate = ConfigMgr.LoadEquipmentCalibration();
            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipCalibrate.Safe_ChuckZ);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(proberInfo.ChuckX); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(proberInfo.ChuckY); });
            task.Add(t2);
            var t3 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_X].MoveAbsolute(proberInfo.CcdX); });
            task.Add(t3);
            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Y].MoveAbsolute(proberInfo.CcdY); });
            task.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(proberInfo.CcdZ); });
            task.Add(t5);
            Task.WaitAll(task.ToArray());

            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(proberInfo.ChuckZ + deltaZ2);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
        }

        public void MoveToNextReticle(DieInfo selectDie, DieInfo preDie, WaferMapInfo map) {
            int rowSelect = selectDie.RowIndex;
            int colSelect = selectDie.ColumnIndex;
            int rowPre = preDie.RowIndex;
            int colPre = preDie.ColumnIndex;

            double rowMove = (rowSelect - rowPre) * map.DieHeight;
            double colMove = (colSelect - colPre) * map.DieWidth;

            if (MotionState != State.Ready) {
                ReportMessage($"从{preDie.OrdName}运动到:{selectDie.OrdName}失败:{EValue.NOREADYINFO}");
                return;
            }

            Task.Run(() => {
                try {
                    MotionState = State.Busy;
                    //stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(-500);
                    sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_SEPERATE_HEIGHT, out object seperateHeight);
                    string sepLen = seperateHeight as string;
                    double zMoveLen = Convert.ToDouble(sepLen);
                    stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(-1 * zMoveLen);
                    stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

                    MoveChuckXYRel(-colMove, rowMove);

                    //stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(500);
                    stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveRelative(zMoveLen);
                    stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

                    SetDieHighLight(selectDie);
                    ReportMessage($"从{preDie.OrdName}运动到:{selectDie.OrdName}完成");
                } catch (Exception ex) {
                    ReportMessage($"从{preDie.OrdName}运动到:{selectDie.OrdName}运动异常:{ex.Message}");
                    return;
                } finally {
                    MotionState = State.Ready;
                }
            });
        }
    }
}
