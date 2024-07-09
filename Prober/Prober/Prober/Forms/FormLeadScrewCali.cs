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
using MyInstruments.MyUtility;
using MyMotionStageDriver.MyStageAxis;
using NLog;
using Prober.Constant;
using ProberApi.MyConstant;

namespace Prober.Forms {
    public partial class FormLeadScrewCali : Form {

        bool isStopRecord = false;

        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        private readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        private readonly Dictionary<string, StageAxis> stageAxisDic = new Dictionary<string, StageAxis>();

        public FormLeadScrewCali(ConcurrentDictionary<string, object> sharedObjects) {
            InitializeComponent();

            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            stageAxisUsages = tempObj as Dictionary<string, StageAxis>;

            //获取轴
            GetStageAxisDic();
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

        private void btn_StartX_Click(object sender, EventArgs e) {
            double posStart = Convert.ToDouble(txt_CompStart.Text);
            double posStop = Convert.ToDouble(txt_CompStop.Text);
            double posStep = Convert.ToDouble(txt_CompStep.Text);
            double disAdjust = Convert.ToDouble(txt_CompAdjustDis.Text);
            int delay = Convert.ToInt32(txt_CompDelay.Text);
            int loop = Convert.ToInt32(txt_CompTimes.Text);
            int moveCount = (int)((posStop - posStart) / posStep + 1.1);
            double dstPos = 0;
            isStopRecord = false;

            string AxisName = cmb_Axis.SelectedItem.ToString();
            if (AxisName == string.Empty) {
                MessageBox.Show("请选择轴");
                return;
            }

            Task.Run(() => {
                try { 
                    //循环次数
                    for (int i = 0; i < loop; i++) {

                        this.BeginInvoke(new Action(() => {
                            txt_Log.AppendText(Environment.NewLine + $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}]  轴{AxisName} 第 {i} 轮测试开始...");
                        }));
#if true
                        //正向移动
                        dstPos = posStart - disAdjust;
                        MoveAbsByName(AxisName, dstPos, delay);
#endif
                        for (int j = 0; j < moveCount; j++) {

                            if (isStopRecord) { return;}

                            dstPos = posStart + j * posStep;
                            MoveAbsByName(AxisName, dstPos, delay);
                        }
#if true
                        //反向移动
                        dstPos = posStop + disAdjust;
                        MoveAbsByName(AxisName, dstPos, delay);

                        for (int j = 0; j < moveCount; j++) {
                            if (isStopRecord) { return; }

                            dstPos = posStop - j * posStep;
                            MoveAbsByName(AxisName, dstPos, delay);
                        }
#endif
                        this.BeginInvoke(new Action(() => {
                            txt_Log.AppendText(Environment.NewLine + $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}]  轴{AxisName} 第 {i} 轮测试完毕...");
                        }));
                    }
                    Invoke(new Action(() => { MessageBox.Show("测试结束"); }));
                }
                catch(Exception ex) {
                    Invoke(new Action(() => { MessageBox.Show($"测试异常:{ex.Message}"); }));
                }
            });
        }

        private void MoveAbsByName(string name,double pos,int waitTime) {
            if (name == "X") {
                stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(pos);
                stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            }
            else if (name == "Y") {
                stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(pos);
                stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
            }
            else if (name == "Z") {
                stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(pos);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();
            }
            else {
                return;
            }

            this.BeginInvoke(new Action(() => {
                txt_Log.AppendText(Environment.NewLine + $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}]  CurPos: {pos}");
            }));

            Thread.Sleep(waitTime * 1000);
        }

        private void btn_StopX_Click(object sender, EventArgs e) {
            if (isStopRecord != true) {
                if (MessageBox.Show("确定要停止测试吗？", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }

            isStopRecord = true;
        }
    }
}
