using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonApi.MyUtility;
using HalconDotNet;
using NLog;

namespace Prober.WaferDef {
    public class ConfigMgr
    {
        private readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);

        #region 高度扫描基准位置
        private static string _HeightScanBaiscPos = "Configuration\\Wafer\\HeightScanBasicPos_{0}.xml";
        public static List<HeightScanBasicPosition> LoadHeightScanBasicPosition(string waferType)
        {
            string file = string.Format(_HeightScanBaiscPos, waferType);
            if (!File.Exists(file))
            {
                List<HeightScanBasicPosition> list = new List<HeightScanBasicPosition>();
                SaveHeightScanBasicPosition(waferType, list);
            }
            return FileHelper.LoadXml(file, typeof(List<HeightScanBasicPosition>)) as List<HeightScanBasicPosition>;
        }

        public static bool SaveHeightScanBasicPosition(string waferType, List<HeightScanBasicPosition> info)
        {
            string file = string.Format(_HeightScanBaiscPos, waferType);
            return FileHelper.SaveXml(file, info, typeof(List<HeightScanBasicPosition>));
        }
        #endregion

        #region IO配置
        private static string _IOFile = "Configuration\\IO.xml";
        public static IOInfo LoadIoInfo()
        {
            if (!File.Exists(_IOFile))
            {
                IOInfo safe = new IOInfo();
                SaveIoInfo(safe);
            }
            return FileHelper.LoadXml(_IOFile, typeof(IOInfo)) as IOInfo;
        }

        public static bool SaveIoInfo(IOInfo io)
        {
            return FileHelper.SaveXml(_IOFile, io, io.GetType());
        }
        #endregion

        #region 保存位置信息
        public static SpecialPositionInfo LoadSpecialPosition(string file) {
            if (!File.Exists(file)) {
                SpecialPositionInfo info = new SpecialPositionInfo();
                SaveSpecialPosition(file, info);
            }
            return FileHelper.LoadXml(file, typeof(SpecialPositionInfo)) as SpecialPositionInfo;
        }

        public static bool SaveSpecialPosition(string file, SpecialPositionInfo info) {
            return FileHelper.SaveXml(file, info, info.GetType());
        }

        #endregion


        #region 光功率计Trigger手动设置
        private static string _PmTriggerFile = "Configuration\\PmTriggerSetting.xml";
        public static PmTriggerSetting LoadPmTriggerInfo()
        {
            if (!File.Exists(_PmTriggerFile))
            {
                PmTriggerSetting setting = new PmTriggerSetting();
                SavePmTriggerInfo(setting);
            }
            return FileHelper.LoadXml(_PmTriggerFile, typeof(PmTriggerSetting)) as PmTriggerSetting;
        }

        public static bool SavePmTriggerInfo(PmTriggerSetting setting)
        {
            return FileHelper.SaveXml(_PmTriggerFile, setting, setting.GetType());
        }
        #endregion

        #region 清针信息
        private static string _clearFile = "Configuration\\ClearProber.xml";
        public static ClearProberInfo LoadClearProberInfo()
        {
            if (!File.Exists(_clearFile))
            {
                ClearProberInfo com = new ClearProberInfo();
                SaveClearProberInfo(com);
            }
            return FileHelper.LoadXml(_clearFile, typeof(ClearProberInfo)) as ClearProberInfo;
        }

        public static bool SaveClearProberInfo(ClearProberInfo info)
        {
            return FileHelper.SaveXml(_clearFile, info, typeof(ClearProberInfo));
        }
        #endregion

        #region 高度校准信息
        private static string _HCalibrationFile = "Configuration\\Wafer\\HCalibrationInfo_{0}_{1}.xml";
        private static readonly object mutexHCali = new object();
        public static bool isHcalibrationFileExist(string waferType, string ScanMode = "0")
        {
            string file = string.Format(_HCalibrationFile, waferType, ScanMode);
            if (!File.Exists(file))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 加载高度扫描信息
        /// </summary>
        /// <returns></returns>
        public static HCalibrationInfo LoadHCalibration(string waferType, string ScanMode = "0")
        {
            lock(mutexHCali) {
                string file = string.Format(_HCalibrationFile, waferType,ScanMode);
                if (!File.Exists(file)) {
                    HCalibrationInfo temp = new HCalibrationInfo();
                    if (!SaveHCalibration(waferType, temp, ScanMode))
                        return null;
                }
                HCalibrationInfo info = FileHelper.LoadXml(file, typeof(HCalibrationInfo)) as HCalibrationInfo;
                return info;
            }
            
        }

        public static bool SaveHCalibration(string waferType, HCalibrationInfo info, string ScanMode = "0")
        {
            lock (mutexHCali) {
                string file = string.Format(_HCalibrationFile, waferType, ScanMode);
                return FileHelper.SaveXml(file, info, typeof(HCalibrationInfo));
            }
        }
        #endregion

        #region 螺距补偿信息
        private static string _LeadscrewCompAxisXFile = "Configuration\\LeadscrewCompAxisXInfo.xml";
        private static string _LeadscrewCompAxisYFile = "Configuration\\LeadscrewCompAxisYInfo.xml";
        private static string _LeadscrewCompAxisZFile = "Configuration\\LeadscrewCompAxisZInfo.xml";
        public static CompParamInfo LoadLeadscrwCompInfo(string axisType)
        {
            string filePath = string.Empty;
            if (!GetLeadscrewFilePath(axisType, out filePath))
            {
                return null;
            }

            if (!File.Exists(filePath))
            {
                CompParamInfo temp = new CompParamInfo();
                if (!SaveLeadscrwCompInfo(axisType, temp))
                    return null;
            }
            CompParamInfo info = FileHelper.LoadXml(filePath, typeof(CompParamInfo)) as CompParamInfo;
            return info;
        }

        public static bool GetLeadscrewFilePath(string axisType,out string  filePath)
        {
            filePath = string.Empty;

            if (axisType == "X")
            {
                filePath = _LeadscrewCompAxisXFile;
            }
            else if (axisType == "Y")
            {
                filePath = _LeadscrewCompAxisYFile;
            }
            else if (axisType == "Z")
            {
                filePath = _LeadscrewCompAxisZFile;
            }
            else
            {
                return false;
            }

            return true;
        }

        public static bool SaveLeadscrwCompInfo(string axisType, CompParamInfo info)
        {
            string filePath = string.Empty;
            if (!GetLeadscrewFilePath(axisType, out filePath))
            {
                return false;
            }

            return FileHelper.SaveXml(filePath, info, typeof(CompParamInfo));
        }
        #endregion

        #region 设备校准信息
        private static string _equipmentCalibrationFile = "Configuration\\EquipmentCalibrationInfo.xml";
        private static readonly object mutexEquipInfo = new object();
        public static EquipmentCalibrationInfo LoadEquipmentCalibration()
        {
            lock(mutexEquipInfo) {
                if (!File.Exists(_equipmentCalibrationFile)) {
                    EquipmentCalibrationInfo temp = new EquipmentCalibrationInfo();
                    if (!SaveEquipmentCalibration(temp))
                        return null;
                }
                EquipmentCalibrationInfo info = FileHelper.LoadXml(_equipmentCalibrationFile, typeof(EquipmentCalibrationInfo)) as EquipmentCalibrationInfo;
                return info;
            }
        }

        public static bool SaveEquipmentCalibration(EquipmentCalibrationInfo info)
        {
            lock (mutexEquipInfo) {
                return FileHelper.SaveXml(_equipmentCalibrationFile, info, typeof(EquipmentCalibrationInfo));
            }
        }
        #endregion

        #region 4点矩阵
        private static string _waferMotionFile = "Configuration\\WaferMotion.xml";
        public static PlatInfo LoadWaferMotionPlatInfo() {
            return FileHelper.LoadXml(_waferMotionFile, typeof(PlatInfo)) as PlatInfo;
        }

        public static bool SaveWaferMotionPlatInfo(PlatInfo plat) {
            bool b = FileHelper.SaveXml(_waferMotionFile, plat, typeof(PlatInfo));
            string msg = FileHelper.ErrorMsg;
            return b;
        }
        #endregion

        #region 平面校准信息
        private static string _visionMotionFile = "Configuration\\VisionMotion.xml";
        /// <summary>
        /// 加载相机和运行控制卡的变换关系
        /// </summary>
        /// <returns></returns>
        public static PlatInfo LoadVisionMotionPlatInfo()
        {
            return FileHelper.LoadXml(_visionMotionFile, typeof(PlatInfo)) as PlatInfo;
        }
        public static bool SaveVisionMotionPlatInfo(PlatInfo plat)
        {
           bool b = FileHelper.SaveXml(_visionMotionFile, plat, typeof(PlatInfo));
            string msg = FileHelper.ErrorMsg;
            return b;
        }        
        #endregion


        #region 通讯配置文件
        private static string _comFile = "Configuration\\com.xml";
        public static CommunicationInfo LoadComInfo()
        {
            if (!File.Exists(_comFile))
            {
                CommunicationInfo com = new CommunicationInfo();
                SaveComInfo(com);
            }
            return FileHelper.LoadXml(_comFile, typeof(CommunicationInfo)) as CommunicationInfo;
        }

        public static bool SaveComInfo(CommunicationInfo info)
        {
            return FileHelper.SaveXml(_comFile, info, typeof(CommunicationInfo));
        }
        #endregion       


        #region 光源信息
        private static string _lightFile = "Configuration\\light.xml";
        public static LightInfo LoadLightInfo()
        {
            if (!File.Exists(_lightFile))
            {
                LightInfo com = new LightInfo();
                SaveLightInfo(com);
            }
            return FileHelper.LoadXml(_lightFile, typeof(LightInfo)) as LightInfo;
        }

        public static bool SaveLightInfo(LightInfo info)
        {
            return FileHelper.SaveXml(_lightFile, info, typeof(LightInfo));
        }
        #endregion        

        #region 辅助Chuck
        private static string _AssistChuckFile = "Configuration\\AssistChuckConfig.xml";
        public static AssistChuckPosInfo LoadAssistChuckInfo()
        {
            if (!File.Exists(_AssistChuckFile))
            {
                AssistChuckPosInfo com = new AssistChuckPosInfo();
                SaveAssistChuckConfigInfo(com);
            }
            return FileHelper.LoadXml(_AssistChuckFile, typeof(AssistChuckPosInfo)) as AssistChuckPosInfo;
        }

        public static bool SaveAssistChuckConfigInfo(AssistChuckPosInfo info)
        {
            return FileHelper.SaveXml(_AssistChuckFile, info, typeof(AssistChuckPosInfo));
        }
        #endregion

        public static string ErrorMsg = string.Empty;
        
        #region 加载晶圆打标位置信息
        public static List<bool[]> LoadWaferTestState(string name)
        {
            ErrorMsg = string.Empty;
            Int32 colnum = 0;
            try
            {
                var lines = File.ReadAllLines(name);
                List<bool[]> list = new List<bool[]>();
                List<bool[]> listRet = new List<bool[]>();
                for (int i = 1; i < lines.Length; i++)
                {                    
                    try
                    {
                        string[] split = lines[i].Split(',');
                        colnum = split.Count() - 1;
                        bool[] data = new bool[colnum];

                        for (int j = 1; j < split.Count(); j++)
                        {
                            if (split[j] == "1")
                            {
                                data[j - 1] = true;
                            }
                            else if (split[j] == "0")
                            {
                                data[j - 1] = false;
                            }
                            else
                            {
                                ErrorMsg = $"配置文件{i + 1},{j + 1}有非0和1的数";             
                                return null;
                            }
                        }

                        list.Add(data);
                    }
                    catch (Exception)
                    {
                        ErrorMsg = $"读取第{i + 1}行数据时出错，请检查数据。";
                        return null;
                    }                    
                }

                //List进行行列转换 , 
                for (int die = 0; die < colnum; die++)
                {
                    bool[] temp = new bool[list.Count];
                    for (int subdie = 0; subdie < list.Count; subdie++)
                    {
                        temp[subdie] = list[subdie][die];
                    }

                    listRet.Add(temp);
                }

                return listRet;
            }
            catch (Exception ex)
            {
                ErrorMsg = "读取测试项发生异常：" + ex.Message;
                //YTN_Log.Error(ex, "ConfigMgr.LoadWaferTestState() error!");
                return null;
            }
        }
        #endregion

        #region    高度校验信息
        private static string _heightVerifyPath = "Configuration\\Wafer\\";
        public static ChuckHeightVerifyInfo LoadHeightVerifyInfoByType(string type)
        {
            if (!File.Exists(_heightVerifyPath + $"{type}_heightVerify.xml"))
            {
                ChuckHeightVerifyInfo info = new ChuckHeightVerifyInfo();
                info.WaferType = type;  
                SaveHeightVerifyInfo(info);
            }

            return FileHelper.LoadXml(_heightVerifyPath + $"{type}_heightVerify.xml", typeof(ChuckHeightVerifyInfo)) as ChuckHeightVerifyInfo;
        }

        public static bool SaveHeightVerifyInfo(ChuckHeightVerifyInfo info)
        {
            return FileHelper.SaveXml(_heightVerifyPath + $"{info.WaferType}_heightVerify.xml", info, typeof(ChuckHeightVerifyInfo));
        }

        #endregion

        #region 晶圆Map信息
        private static string _mapPath = "Configuration\\Wafer\\";
        private static readonly object mutexMapInfo = new object();
        public static WaferMapInfo LoadWaferMapInfoByType(string type)
        {
            lock (mutexMapInfo) {
                if (!File.Exists(_mapPath + $"{type}_map.xml"))
                    return null;
                var map = FileHelper.LoadXml(_mapPath + $"{type}_map.xml", typeof(WaferMapInfo)) as WaferMapInfo;
                return map;
            }            
        }

        public static bool SaveWaferMapInfobyType(WaferMapInfo info)
        {
            lock (mutexMapInfo) {
                return FileHelper.SaveXml(_mapPath + $"{info.Type}_map.xml", info, info.GetType());
            }
        }

        public static string[] GetWaferMapNames()
        {
            string[] files = Directory.GetFiles(_mapPath);
            var selectFiles = files.Where(t => t.IndexOf("_map.xml") != -1);
            return selectFiles.Select(t => Path.GetFileNameWithoutExtension(t).Replace("_map", "")).ToArray();
        }

        public static bool IsWaferTypeExist(string type)
        {
            return File.Exists(_mapPath + $"{type}_map.xml");
        }

        public static void DeleteWaferMapByType(string type)
        {
            File.Delete(_mapPath + $"{type}_map.xml");

            if (File.Exists($"Configuration\\Wafer\\{type}.shm"))
            {
                File.Delete($"Configuration\\Wafer\\{type}.shm");
            }

            if (File.Exists($"Configuration\\Wafer\\HCalibrationInfo_{type}.xml"))
            {
                File.Delete($"Configuration\\Wafer\\HCalibrationInfo_{type}.xml");
            }
        }
        #endregion

        #region 4种测试场景对应的坐标
        private const string _SubDieOrdinaryFileCSV = "TemplateDoc\\SubdieOrdinary.csv";
        private static string _SubdiePosFileXML = "configuration\\SubdieOrdinary.xml";
        public static bool SaveSubdieOrdinaryToXML(List<SubdieOrdinary> list)
        {
            return FileHelper.SaveXml(_SubdiePosFileXML, list, typeof(List<SubdieOrdinary>));
        }

        public static List<SubdieOrdinary> LoadSubdieOrdinaryFromCSV(string name = _SubDieOrdinaryFileCSV)
        {
            ErrorMsg = string.Empty;

            try
            {
                var lines = File.ReadAllLines(name);
                List<SubdieOrdinary> list = new List<SubdieOrdinary>();
                List<string> existList = new List<string>();

                for (int i = 1; i < lines.Length; i++)
                {
                    SubdieOrdinary item = new SubdieOrdinary();
                    var split = lines[i].Split(',');

                    item.Subname = split[0];
                    if (existList.Contains($"{item.Subname}"))
                    {
                        ErrorMsg = $"测试项{item.Subname}已经存在，请重新命名。";
                        return null;
                    }

                    item.ChuckX = Convert.ToDouble(split[1]);
                    item.ChuckY = Convert.ToDouble(split[2]);
                    item.LeftX = Convert.ToDouble(split[3]);
                    item.LeftY = Convert.ToDouble(split[4]);
                    item.RightX = Convert.ToDouble(split[5]);
                    item.RightY = Convert.ToDouble(split[6]);                    
                    list.Add(item);
                }

                return list;
            }
            catch (Exception ex)
            {
                ErrorMsg = "LoadSubdieOrdinaryFromCSV异常：" + ex.Message;
                return null;
            }
        }
        #endregion

        #region 参考Subdie晶圆坐标和轴坐标标定结果
        private static string _SubdiePosCalibrateFile = "configuration\\SubdiePosCalibrate.xml";
        public static void DeleteSubdiePosCalibrateInfo()
        {
            if (File.Exists(_SubdiePosCalibrateFile))
            {
                File.Delete(_SubdiePosCalibrateFile);
            }
        }
        public static SubdiePosCaliInfo LoadSubdiePosCalibrateInfo()
        {
            return FileHelper.LoadXml(_SubdiePosCalibrateFile, typeof(SubdiePosCaliInfo)) as SubdiePosCaliInfo;
        }
        public static bool SaveSubdiePosCalibrateInfo(SubdiePosCaliInfo info)
        {
            bool b = FileHelper.SaveXml(_SubdiePosCalibrateFile, info, typeof(SubdiePosCaliInfo));
            string msg = FileHelper.ErrorMsg;
            return b;
        }
        #endregion
    }
}
