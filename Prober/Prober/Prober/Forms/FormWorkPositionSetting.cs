using CommonApi.MyUtility;
using MyInstruments;
using MyInstruments.MyCamera;
using MyInstruments.MyElecLens;
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
using System.Linq;
using System.Windows.Forms;

namespace Prober.Forms
{
    public partial class FormWorkPositionSetting : Form
    {
        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        protected static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);

        private readonly Dictionary<string, Instrument> instruments;
        private readonly List<InstrumentUsage> instrumentUsageList;        

        private StandaloneCamera camera;
        private StandaloneElecLens eLens;
        private readonly Dictionary<string, StageAxis> stageAxisDic = new Dictionary<string, StageAxis>();
        private ConcurrentDictionary<string, object> sharedObject;

        public FormWorkPositionSetting(ConcurrentDictionary<string, object> sharedObjects)
        {
            InitializeComponent();

            this.sharedObject = sharedObjects;   

            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            stageAxisUsages = tempObj as Dictionary<string, StageAxis>;

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            //获取仪表
            var getResult2 = GetInstrument("top_camera");
            var getResult4 = GetInstrument("elens_zoom");

            //获取轴
            GetStageAxisDic();
        }

        private void FormWorkPositionSetting_Load(object sender, EventArgs e) {
            //加载map类型
            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
            if (info != null) {
                UIClass.ObjectToControl(info, tab_WorkPlace);
            }            
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
                case EnumInstrumentCategory.ELENS:
                    eLens = instrument as StandaloneElecLens;
                    break;
                default:
                    errorText = $"{instrumentUsage.InstrumentCategory.ToString()} is not a valid instrument category of coupling feedback!";
                    LOGGER.Error(errorText);
                    return (false, errorText, null);
            }

            return (true, string.Empty, instrument);
        }        

        internal (bool isOK,string errorMessage, StageAxis stageAxis) GetStageAxis(string axisUsageId)
        {
            string errorText = string.Empty;
            string INVALID_PARAMETERS = $"input parameters(={axisUsageId}) is invalid";
                        
            if (!stageAxisUsages.ContainsKey(axisUsageId))
            {
                errorText = $"{INVALID_PARAMETERS}";
                LOGGER.Error(errorText);
                return (false, errorText, null);
            }
            
            return (true,null, stageAxisUsages[axisUsageId]);
        }
        
        private void btn_Load_Click(object sender, EventArgs e)
        {
            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
            if (info != null) {
                UIClass.ObjectToControl(info, tab_WorkPlace);
            }            
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();

            UIClass.ControlToObject(tab_WorkPlace, info);
            if (ConfigMgr.SaveEquipmentCalibration(info))
            {
                sharedObject.AddOrUpdate(PrivateSharedObjectKey.HEIGHT_SCAN_MODE, txt_HeightScanMode.Text, (key, oldValue) => txt_HeightScanMode.Text);
                sharedObject.AddOrUpdate(PrivateSharedObjectKey.MONIT_TEST_CONDITION, txt_MonitTestCondition.Text, (key, oldValue) => txt_MonitTestCondition.Text);
                sharedObject.AddOrUpdate(PrivateSharedObjectKey.CHUCK_SEPERATE_HEIGHT, txt_ChuckSeperateHeight.Text, (key, oldValue) => txt_ChuckSeperateHeight.Text);
                MessageBox.Show(this, "保存成功！", "提示：");
            }
            else
            {
                MessageBox.Show(this, "保存失败！", "提示：");
            }
        }

        private void btn_Location_ChuckZ_Click(object sender, EventArgs e)
        {                        
            txt_ChuckZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Z].Position(), 2).ToString();
        }

        private void btn_BaseChuckXY_Click(object sender, EventArgs e)
        {            
            txt_Base_ChuckX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2).ToString();
            txt_Base_ChuckY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2).ToString();
            txt_Base_ChuckSZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_SZ].Position(),5).ToString();
        }

        private void btn_BaseCamera_Click(object sender, EventArgs e)
        {
            txt_Base_CcdX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_X].Position(), 2).ToString();            
            txt_Base_CcdY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Y].Position(), 2).ToString();            
            txt_Base_CcdZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Z].Position(), 2).ToString();

            camera.GetBaseParameters(out double ex, out double g, out double gama);
            txt_Base_Exp.Text = ex.ToString();

            eLens.GetZoom(out double zoomValue);
            txt_Base_Zoom.Text = Math.Round(zoomValue,0).ToString();
        }

        private void btn_HSenserWorkU_Click(object sender, EventArgs e)
        {            
            txt_HSenserWorkU.Text = Math.Round(stageAxisDic[MyStageAxisKey.HEIGHT_U].Position(), 2).ToString();
        }

        private void btn_LSX1_Click(object sender, EventArgs e)
        {            
            txt_LSX_CcdX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_X].Position(), 2).ToString();            
            txt_LSX_CcdY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Y].Position(), 2).ToString();
            txt_LSX_CcdZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Z].Position(), 2).ToString();
            
            txt_LSX_LeftX.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_X].Position(), 2).ToString();
            txt_LSX_LeftY.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Y].Position(), 2).ToString();            
            txt_LSX_LeftZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Z].Position(), 2).ToString();
            
            txt_LSX_ChuckX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2).ToString();            
            txt_LSX_ChuckY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2).ToString();
            
            camera.GetBaseParameters(out double ex, out double g, out double gama);
            txt_LSX_Exp.Text = ex.ToString();  
        }

        private void btn_LSY1_Click(object sender, EventArgs e)
        {            
            txt_LSY_CcdX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_X].Position(), 2).ToString();            
            txt_LSY_CcdY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Y].Position(), 2).ToString();            
            txt_LSY_CcdZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Z].Position(), 2).ToString();
            
            txt_LSY_LeftX.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_X].Position(), 2).ToString();            
            txt_LSY_LeftY.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Y].Position(), 2).ToString();            
            txt_LSY_LeftZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Z].Position(), 2).ToString();
            
            txt_LSY_ChuckX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2).ToString();            
            txt_LSY_ChuckY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2).ToString();

            camera.GetBaseParameters(out double ex, out double g, out double gama);
            txt_LSY_Exp.Text = ex.ToString();
        }

        private void btn_LSZ1_Click(object sender, EventArgs e)
        {            
            txt_LSZ_CcdX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_X].Position(), 2).ToString();
            txt_LSZ_CcdY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Y].Position(), 2).ToString();            
            txt_LSZ_CcdZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Z].Position(), 2).ToString();

            txt_LSZ_LeftX.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_X].Position(), 2).ToString();
            txt_LSZ_LeftY.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Y].Position(), 2).ToString();
            txt_LSZ_LeftZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Z].Position(), 2).ToString();

            txt_LSZ_ChuckX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2).ToString();
            txt_LSZ_ChuckY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2).ToString();

            camera.GetBaseParameters(out double ex, out double g, out double gama);
            txt_LSZ_Exp.Text = ex.ToString();
        }

        private void btn_RSX3_Click(object sender, EventArgs e)
        {
            txt_RSX_CcdX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_X].Position(), 2).ToString();
            txt_RSX_CcdY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Y].Position(), 2).ToString();
            txt_RSX_CcdZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Z].Position(), 2).ToString();

            txt_RSX_RightX.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_X].Position(), 2).ToString();
            txt_RSX_RightY.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_Y].Position(), 2).ToString();
            txt_RSX_RightZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_Z].Position(), 2).ToString();

            txt_RSX_ChuckX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2).ToString();
            txt_RSX_ChuckY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2).ToString();

            camera.GetBaseParameters(out double ex, out double g, out double gama);
            txt_RRSX_Exp.Text = ex.ToString();
        }

        private void btn_RSY3_Click(object sender, EventArgs e)
        {
            txt_RSY_CcdX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_X].Position(), 2).ToString();
            txt_RSY_CcdY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Y].Position(), 2).ToString();
            txt_RSY_CcdZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Z].Position(), 2).ToString();

            txt_RSY_RightX.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_X].Position(), 2).ToString();
            txt_RSY_RightY.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_Y].Position(), 2).ToString();
            txt_RSY_RightZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_Z].Position(), 2).ToString();

            txt_RSY_ChuckX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2).ToString();
            txt_RSY_ChuckY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2).ToString();

            camera.GetBaseParameters(out double ex, out double g, out double gama);
            txt_RSY_Exp.Text = ex.ToString();
        }

        private void btn_RSZ3_Click(object sender, EventArgs e)
        {
            txt_RSZ_CcdX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_X].Position(), 2).ToString();
            txt_RSZ_CcdY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Y].Position(), 2).ToString();
            txt_RSZ_CcdZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Z].Position(), 2).ToString();

            txt_RSZ_RightX.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_X].Position(), 2).ToString();
            txt_RSZ_RightY.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_Y].Position(), 2).ToString();
            txt_RSZ_RightZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_Z].Position(), 2).ToString();

            txt_RSZ_ChuckX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2).ToString();
            txt_RSZ_ChuckY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2).ToString();

            camera.GetBaseParameters(out double ex, out double g, out double gama);
            txt_RSZ_Exp.Text = ex.ToString();
        }

        private void btn_Work_Remove_Click(object sender, EventArgs e)
        {
            txt_Remove_ChuckX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2).ToString();
            txt_Remove_ChuckY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2).ToString();
            txt_Remove_ChuckZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Z].Position(), 2).ToString();
        }

        private void btn_Work_Safe_Click(object sender, EventArgs e)
        {
            txt_Safe_LeftX.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_X].Position(), 2).ToString();
            txt_Safe_LeftY.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Y].Position(), 2).ToString();
            txt_Safe_LeftZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Z].Position(), 2).ToString();

            txt_Safe_RightX.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_X].Position(), 2).ToString();
            txt_Safe_RightY.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_Y].Position(), 2).ToString();
            txt_Safe_RightZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_Z].Position(), 2).ToString();

            txt_Safe_ChuckZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Z].Position(), 2).ToString();
            txt_Safe_CcdZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Z].Position(), 2).ToString();
            txt_Safe_U.Text = Math.Round(stageAxisDic[MyStageAxisKey.HEIGHT_U].Position(), 2).ToString();
        }

        private void btn_ClearProbe_Set_Click(object sender, EventArgs e)
        {
            ;
        }

        private void btn_PDPlat_Set_Click(object sender, EventArgs e)
        {
            txt_PdPlat_ChuckX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2).ToString();
            txt_PdPlat_ChuckY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2).ToString();
            txt_PdPlat_ChuckZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Z].Position(), 2).ToString();
        }
    }
}
