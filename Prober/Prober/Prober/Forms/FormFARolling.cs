using CommonApi.MyCommunication;
using CommonApi.MyEnum;
using CommonApi.MyUtility;
using HalconDotNet;
using MyInstruments;
using MyInstruments.MyAltimeter;
using MyInstruments.MyEnum;
using MyInstruments.MyOpm;
using MyInstruments.MyOs;
using MyInstruments.MyUtility;
using MyMotionStageDriver.MyStageAxis;
using NLog;
using Prober.Constant;
using ProberApi.MyConstant;
using ProberApi.MyRequest;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Prober.Forms
{
    public partial class FormFARolling : Form
    {
        bool isStopRolling = false;
        private ConcurrentDictionary<string, object> sharedObjects;
        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        private readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        private readonly Dictionary<string, StageAxis> stageAxisDic = new Dictionary<string, StageAxis>();
        private readonly Dictionary<string, Instrument> instruments;
        private readonly List<InstrumentUsage> instrumentUsageList;
        private IOpm opm;
        private StandaloneOs m_Os;
        private string Power1_Slot = string.Empty;
        private string Power2_Slot = string.Empty;
        private string Power1_Channel = string.Empty;   
        private string Power2_Channel = string.Empty;
        private bool isUseSwitch = false;
        private bool isRightFA = false;

        public FormFARolling(ConcurrentDictionary<string, object> sharedObjects)
        {
            InitializeComponent();

            this.sharedObjects = sharedObjects;
            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            stageAxisUsages = tempObj as Dictionary<string, StageAxis>;

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            //获取轴
            GetStageAxisDic();

            //获取仪表
            var getResult2 = GetInstrument("pma1_2_1_opm");
            getResult2 = GetInstrument("pma1_2_2_opm");
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
                case EnumInstrumentCategory.OPM:
                    if (instrumentUsage.UsageId == "pma1_2_1_opm")
                    {
                        opm = instrumentUsage.TheInstrument as IOpm;
                        Power1_Slot = instrumentUsage.Slot;
                        Power1_Channel = instrumentUsage.Channel;   
                    }
                    else if (instrumentUsage.UsageId == "pma1_2_2_opm")
                    {
                        opm = instrumentUsage.TheInstrument as IOpm;
                        Power2_Slot = instrumentUsage.Slot;
                        Power2_Channel = instrumentUsage.Channel;
                    }
                    else
                    {
                        errorText = $"{instrumentUsage.InstrumentId.ToString()} is not a valid instrument id of FA Rolling!";
                        LOGGER.Error(errorText);
                        return (false, errorText, null);
                    }
                    break;
                default:
                    errorText = $"{instrumentUsage.InstrumentCategory.ToString()} is not a valid instrument category of FA Rolling!";
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

        private void btn_StartRolling_Click(object sender, EventArgs e) {
            int switchPort1 = Convert.ToInt32(num_SwitchPort1.Value);
            int switchPort2 = Convert.ToInt32(num_SwitchPort2.Value);
            bool isSingleU = chb_IsSingleU.Checked;
            string addr = txt_OsAddr.Text;
            string info = string.Empty;
            double PortGap = Convert.ToDouble(txt_OpticalChannelGap.Text);
            isUseSwitch = chb_IsUseSwitch.Checked;
            isRightFA = chb_IsRightFA.Checked; 

            Task.Run(() => {
                try {
                    if (isUseSwitch)
                    {
                        m_Os = StandaloneOsFactory.CreateInstance("OS", "JDSU", "_SB");
                        m_Os.Connect(addr);
                    }

                    if (isSingleU)
                    {
                        if (!SingleURolling(switchPort1, switchPort2, PortGap)) {
                            info = $"1U Rolling Fail";
                        } else {
                            info = $"1U Rolling Success";
                        }
                    } else {
                        if (!DoubleURolling(switchPort1, switchPort2, PortGap)) {
                            info = $"2U Rolling Fail";
                        } else {
                            info = $"2U Rolling Success";
                        }
                    }

                    if (isUseSwitch)
                    {
                        m_Os.Disconnect();
                    }
                       
                    LOGGER.Info(info);
                    Invoke(new Action(() => { MessageBox.Show(info); }));
                } catch (Exception ex) {
                    info = $"FA Rolling exception {ex.Message}";
                    LOGGER.Error(info);
                    Invoke(new Action(() => { MessageBox.Show(info); }));
                    return;
                }
            });
        }

        private bool SingleURolling(int switchPort1, int switchPort2, double PortGap)
        {
            //位置A耦合
            if (!SingleCoupling(switchPort1, Power1_Slot, Power1_Channel, out double PosX1, out double PosY1, out double Power1))
            {
                return false;
            }

            //移动到最远测试端口 
            stageAxisDic[MyStageAxisKey.LEFT_Y].MoveRelative(PortGap);  //注意正负
            stageAxisDic[MyStageAxisKey.LEFT_Y].GuiUpdatePosition();
            Thread.Sleep(1000);
        
            //位置B耦合
            if (!SingleCoupling(switchPort2, Power2_Slot, Power2_Channel, out double PosX2, out double PosY2, out double Power2))
            {
                return false;
            }

            //计算角度偏差，并旋转
            CalRollingAngleSingleU(PosX1, PosY1, PosX2, PosY2, out double angel);
            if (Math.Abs(angel) > 1) {
                Invoke(new Action(() => {
                    MessageBox.Show("旋转角度超过1度，请先在辅助光学平台上完成角度粗调后再进行Rolling自动调整");
                }));
                return false;
            }
            stageAxisDic[MyStageAxisKey.LEFT_SZ].MoveRelative(angel);   //调试确认正负
            stageAxisDic[MyStageAxisKey.LEFT_SZ].GuiUpdatePosition();
            Thread.Sleep(1000);

            //位置B耦合
            if (!SingleCoupling(switchPort2, Power2_Slot, Power2_Channel, out double PosX3, out double PosY3, out double Power3))
            {
                return false;
            }

            //移动到最远测试端口 
            stageAxisDic[MyStageAxisKey.LEFT_Y].MoveRelative(-PortGap);  //反向运动
            stageAxisDic[MyStageAxisKey.LEFT_Y].GuiUpdatePosition();
            Thread.Sleep(1000);

            //位置B耦合
            if (!SingleCoupling(switchPort1, Power1_Slot, Power1_Channel, out double PosX4, out double PosY4, out double Power4))
            {
                return false;
            }

            string info = $"FA Rolling 后，近端最佳光功率{Power3},远端最佳光功率{Power4},差值{Power3 - Power4}";
            ReportMessage(info);

            return true;
        }

        private bool DoubleURolling(int switchPort1, int switchPort2,double PortGap) {            
            //通道1耦合
            if(!SingleCoupling(switchPort1, Power1_Slot, Power1_Channel,out double PosX1,out double PosY1, out double Power1)) {
                return false;
            }                       

            //通道2耦合
            if (!SingleCoupling(switchPort2, Power2_Slot, Power2_Channel, out double PosX2, out double PosY2, out double Power2)) {
                return false;
            }

            //计算角度偏差，并旋转
            CalRollingAngleDoubleU(PosX1, PosY1, PosX2, PosY2, PortGap, out double angel);
            if (Math.Abs(angel) > 1) {
                Invoke(new Action(() => {
                    MessageBox.Show("旋转角度超过1度，请先在辅助光学平台上完成角度粗调后再进行Rolling自动调整");
                }));
                return false;
            }
            stageAxisDic[MyStageAxisKey.LEFT_SZ].MoveRelative(-angel);   //调试确认正负
            stageAxisDic[MyStageAxisKey.LEFT_SZ].GuiUpdatePosition();
            Thread.Sleep(1000);

            //通道2耦合
            if (!SingleCouplingEx(switchPort2, Power2_Slot, Power2_Channel, out double PosX3, out double PosY3, out double Power3, switchPort1, Power1_Slot, Power1_Channel,out double Power3_1)) {
                return false;
            }

            //通道1耦合
            if (!SingleCouplingEx(switchPort1, Power1_Slot, Power1_Channel, out double PosX4, out double PosY4, out double Power4, switchPort2, Power2_Slot, Power2_Channel, out double Power4_1)) {
                return false;
            }

            return true;
        }

        private bool SingleCoupling(int switchPort,string pmSlot, string pmChannel,out double PosX, out double PosY, out double PowerCoupling)
        {
            PosX = 0;
            PosY = 0;
            PowerCoupling = 0;

            //光开关切换，槽位号预留，暂时无用
            if (isUseSwitch)
            {
                if (!m_Os.SwitchToChannel(1, switchPort))
                {
                    Invoke(new Action(() =>
                    {
                        string Info = $"光开关切换至通道{switchPort}失败";
                        ReportMessage(Info);
                        MessageBox.Show(Info);
                    }));
                    return false;
                }
            }
            Thread.Sleep(500);
            if (isStopRolling) { return false; }

            //开始通道耦合
            if (!FACoupling(pmSlot, pmChannel))
            {
                string Info = $"Power Meter 通道 {pmChannel}耦合失败\"";
                Invoke(new Action(() => { MessageBox.Show(Info); }));
                ReportMessage(Info);
                return false;
            }

            Thread.Sleep(500);
            //读取光功率和位置坐标
            PosX = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_X].Position(), 2);
            PosY = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Y].Position(), 2);
            PowerCoupling = opm.OpmFetchPower(pmSlot, pmChannel);

            string info = $"Power Meter 通道{pmChannel}耦合最佳位置X:{PosX} Y:{PosY},MaxPower:{PowerCoupling}";
            ReportMessage(info);

            return true;
        }

        //2U
        private bool SingleCouplingEx(int couplePort, string pmSlot, string pmChannel, out double PosX, out double PosY, out double PowerCoupling,int comparePort, string compareSlot, string compareChannel, out double PowerCompare)
        {
            PosX = 0;
            PosY = 0;
            PowerCoupling = 0;
            PowerCompare = 0;

            //光开关切换，槽位号预留，暂时无用
            if (isUseSwitch)
            {
                if (!m_Os.SwitchToChannel(1, couplePort))
                {
                    Invoke(new Action(() =>
                    {
                        string Info = $"光开关切换至通道{couplePort}失败";
                        ReportMessage(Info);
                        MessageBox.Show(Info);
                    }));
                    return false;
                }
            }

            Thread.Sleep(500);
            if (isStopRolling) { return false; }

            //开始通道耦合
            if (!FACoupling(pmSlot, pmChannel)) {
                string Info = $"Power Meter 通道 {pmChannel}耦合失败\"";
                Invoke(new Action(() => { MessageBox.Show(Info); }));
                ReportMessage(Info);
                return false;
            }

            Thread.Sleep(500);
            //读取光功率和位置坐标
            PosX = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_X].Position(), 2);
            PosY = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Y].Position(), 2);
            PowerCoupling = opm.OpmFetchPower(pmSlot, pmChannel);

            if (isUseSwitch)
            {
                if (!m_Os.SwitchToChannel(1, comparePort))
                {
                    Invoke(new Action(() =>
                    {
                        string Info = $"光开关切换至通道{comparePort}失败";
                        ReportMessage(Info);
                        MessageBox.Show(Info);
                    }));
                    return false;
                }
            }

            Thread.Sleep(500);
            PowerCompare = opm.OpmFetchPower(compareSlot, compareChannel);

            string info = $"Power Meter 通道{pmChannel}耦合最佳位置X:{PosX} Y:{PosY},MaxPower:{PowerCoupling},参考通道{compareChannel} 光功率:{PowerCompare},功率差值:{PowerCoupling - PowerCompare}";
            ReportMessage(info);

            return true;
        }      


        private void CalRollingAngleDoubleU(double x1, double y1, double x2, double y2,double portGap,out double angle)
        {
            if (chb_HDir.Checked == false)
            {
                if (x2 == x1)
                {
                    angle = 0;
                }
                else
                {
                    angle = Math.Asin((x2 - x1) / portGap) * 180 / 3.1415926;
                }
            }
            else
            {
                if (y2 == y1)
                {
                    angle = 0;
                }
                else
                {
                    angle = Math.Asin((y2 - y1) / portGap) * 180 / 3.1415926;
                }
            }           

            ReportMessage($"Rolling Result X1:{x1} Y1:{y1} X2:{x2} Y2:{y2},Angle:{angle}");
        }

        private void CalRollingAngleSingleU(double x1, double y1, double x2, double y2, out double angle)
        {
            if (y2 == y1)  {
                angle = 0;
            } else {
                angle = Math.Atan((x2 - x1) / (y2 - y1))*180/3.1415926;
            }

            ReportMessage($"Rolling Result X1:{x1} Y1:{y1} X2:{x2} Y2:{y2},Angle:{angle}");
        }

        private bool FACoupling(string slot, string channel)
        {
            try
            {
                FACouplingEx(slot, channel);
            }
            catch (Exception ex)
            {
                ReportMessage(ex.Message);
                LOGGER.Error(ex.Message);
                return false;
            }

            return true;
        }


        private void FACouplingEx(string slot, string channel)
        {
            RequestCrossCoupling2d request = new RequestCrossCoupling2d(sharedObjects);
            string parameter = "cross_o_feedback,coupling_in_x,coupling_in_y,SHOW_GUI=true;SETTING_IN_INSTRUMENT=false;SETTING_FEEDBACK_INSTRUMENT=false;SAVING_RAW_DATA=true;COARSE_MOTION_RANGE=20.0;COARSE_STEP=0.2;ENABLED_REFINED_TRAVELING=true;REFINED_MOTION_RANGE=10.0;REFINED_STEP=0.1;IS_TRIGGERED=true;";
            if (isRightFA)
            {
                parameter = "cross_o_feedback,coupling_out_x,coupling_out_y,SHOW_GUI=true;SETTING_IN_INSTRUMENT=false;SETTING_FEEDBACK_INSTRUMENT=false;SAVING_RAW_DATA=true;COARSE_MOTION_RANGE=20.0;COARSE_STEP=0.2;ENABLED_REFINED_TRAVELING=true;REFINED_MOTION_RANGE=10.0;REFINED_STEP=0.1;IS_TRIGGERED=true;";
            }

            //远端耦合
            if ((slot == Power2_Slot) && (channel == Power2_Slot)) {
                parameter = "cross_o_feedback_2,coupling_in_x,coupling_in_y,SHOW_GUI=true;SETTING_IN_INSTRUMENT=false;SETTING_FEEDBACK_INSTRUMENT=false;SAVING_RAW_DATA=true;COARSE_MOTION_RANGE=20.0;COARSE_STEP=0.2;ENABLED_REFINED_TRAVELING=true;REFINED_MOTION_RANGE=10.0;REFINED_STEP=0.1;IS_TRIGGERED=true;";
                if (isRightFA)
                {
                    parameter = "cross_o_feedback_2,coupling_out_x,coupling_out_y,SHOW_GUI=true;SETTING_IN_INSTRUMENT=false;SETTING_FEEDBACK_INSTRUMENT=false;SAVING_RAW_DATA=true;COARSE_MOTION_RANGE=20.0;COARSE_STEP=0.2;ENABLED_REFINED_TRAVELING=true;REFINED_MOTION_RANGE=10.0;REFINED_STEP=0.1;IS_TRIGGERED=true;";
                }
            }            
            
            var result = request.TryUpdateParameters(parameter);
            if (result.isOk) {
                /*
                await Task.Run(() => {
                    request.Run();
                });
                */
                List<Task> task = new List<Task>();
                var t1 = Task.Run(() => { request.Run(); });
                task.Add(t1);
                Task.WaitAll(task.ToArray());
            } else {
                throw new Exception($"Coupling Error:{result.errorMessage}");
            }
        }

        private void btn_StopRolling_Click(object sender, EventArgs e)
        {
            isStopRolling = true;
            MessageBox.Show("停止Rolling");
        }

        private void ReportMessage(string message)
        {
            this.BeginInvoke(new Action(() => {
                rtxt_Info.AppendText(Environment.NewLine + $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] : {message}");
            }));
        }

        private void rtbMsgBox_TextChanged(object sender, EventArgs e)
        {
            rtxt_Info.SelectionStart = rtxt_Info.Text.Length;
            rtxt_Info.ScrollToCaret();
        }

        private void chb_IsSingleU_CheckedChanged(object sender, EventArgs e) {
            //txt_OpticalChannelGap.Enabled = chb_IsSingleU.Checked;
        }

        private void FormFARolling_Load(object sender, EventArgs e) {
            //txt_OpticalChannelGap.Enabled = chb_IsSingleU.Checked;
        }
    }
}
