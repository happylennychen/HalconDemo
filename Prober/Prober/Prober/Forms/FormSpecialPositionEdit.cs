using CommonApi.MyUtility;
using MyMotionStageDriver.MyStageAxis;
using NLog;
using Prober.Constant;
using Prober.WaferDef;
using ProberApi.MyConstant;
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

namespace Prober
{
    public partial class FormSpecialPositionEdit : Form
    {

        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        protected static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        private readonly Dictionary<string, StageAxis> stageAxisDic = new Dictionary<string, StageAxis>();
        private readonly ConcurrentDictionary<string, object> sharedObjects;

        public FormSpecialPositionEdit(ConcurrentDictionary<string, object> sharedObjects)
        {
            InitializeComponent();

            this.sharedObjects = sharedObjects;

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


        private void btn_Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "(*.xml)|*.xml";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            txt_filePath.Text = dlg.FileName;

            SpecialPositionInfo info = ConfigMgr.LoadSpecialPosition(txt_filePath.Text);
            if (info != null) {
                UIClass.ObjectToControl(info, panelSpecialPos);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            txt_ChuckX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2).ToString();
            txt_ChuckY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2).ToString();
            txt_ChuckZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Z].Position(), 2).ToString();
            txt_ChuckSZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_SZ].Position(), 5).ToString();

            txt_LeftX.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_X].Position(), 2).ToString();
            txt_LeftY.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Y].Position(), 2).ToString();
            txt_LeftZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_Z].Position(), 2).ToString();
            txt_LeftSX.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_SX].Position(), 5).ToString();
            txt_LeftSY.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_SY].Position(), 5).ToString();
            txt_LeftSZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.LEFT_SZ].Position(), 5).ToString();

            txt_RightX.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_X].Position(), 2).ToString();
            txt_RightY.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_Y].Position(), 2).ToString();
            txt_RightZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_Z].Position(), 2).ToString();
            txt_RightSX.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_SX].Position(), 5).ToString();
            txt_RightSY.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_SY].Position(), 5).ToString();
            txt_RightSZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.RIGHT_SZ].Position(), 5).ToString();

            txt_CcdX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_X].Position(), 2).ToString();
            txt_CcdY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Y].Position(), 2).ToString();
            txt_CcdZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Z].Position(), 2).ToString();
            txt_AltimeterU.Text = Math.Round(stageAxisDic[MyStageAxisKey.HEIGHT_U].Position(), 2).ToString();

            SpecialPositionInfo info = new SpecialPositionInfo();
            UIClass.ControlToObject(panelSpecialPos, info);

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "(*.xml)|*.xml";
            if (dlg.ShowDialog() != DialogResult.OK) {
                return;
            }

            string file = dlg.FileName;
            info.filePath = file;
            txt_filePath.Text = file;

            if (ConfigMgr.SaveSpecialPosition(file, info)) {
                MessageBox.Show(this, "保存成功", "Info:");
            } else {
                MessageBox.Show(this, "保存失败", "Info:");
            }               
        }

        private void btn_Move_Click(object sender, EventArgs e)
        {
            //获取当前位置信息
            if(txt_filePath.Text == string.Empty) {
                MessageBox.Show("请先选择目标文件");
                return;
            }

            //获取坐标信息
            SpecialPositionInfo info = new SpecialPositionInfo();
            UIClass.ControlToObject(panelSpecialPos, info);            

            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_X].MoveAbsolute(info.LeftX); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Y].MoveAbsolute(info.LeftY); });
            task.Add(t2);
            var t3 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_Z].MoveAbsolute(info.LeftZ); });
            task.Add(t3);
            
            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_SX].MoveAbsolute(info.LeftSX); });
            task.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_SY].MoveAbsolute(info.LeftSY); });
            task.Add(t5);
            var t6 = Task.Run(() => { stageAxisDic[MyStageAxisKey.LEFT_SZ].MoveAbsolute(info.LeftSZ); });
            task.Add(t6);
            Task.WaitAll(task.ToArray());

            MessageBox.Show("Left FA轴运动结束");
        }

        private void btn_MoveRightFA_Click(object sender, EventArgs e) {
            //获取当前位置信息
            if (txt_filePath.Text == string.Empty) {
                MessageBox.Show("请先选择目标文件");
                return;
            }

            //获取坐标信息
            SpecialPositionInfo info = new SpecialPositionInfo();
            UIClass.ControlToObject(panelSpecialPos, info);

            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_X].MoveAbsolute(info.RightX); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Y].MoveAbsolute(info.RightY); });
            task.Add(t2);
            var t3 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveAbsolute(info.RightZ); });
            task.Add(t3);

            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_SX].MoveAbsolute(info.RightSX); });
            task.Add(t4);
            var t5 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_SY].MoveAbsolute(info.RightSY); });
            task.Add(t5);
            var t6 = Task.Run(() => { stageAxisDic[MyStageAxisKey.RIGHT_SZ].MoveAbsolute(info.RightSZ); });
            task.Add(t6);
            Task.WaitAll(task.ToArray());

            MessageBox.Show("Right FA轴运动结束");
        }

        private void btn_MoveChuck_Click(object sender, EventArgs e) {
            //获取当前位置信息
            if (txt_filePath.Text == string.Empty) {
                MessageBox.Show("请先选择目标文件");
                return;
            }

            //获取坐标信息
            SpecialPositionInfo info = new SpecialPositionInfo();
            UIClass.ControlToObject(panelSpecialPos, info);

            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(info.ChuckX); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(info.ChuckY); });
            task.Add(t2);
            var t3 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(info.ChuckZ); });
            task.Add(t3);

            var t4 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CHUCK_SZ].MoveAbsolute(info.ChuckSZ); });
            Task.WaitAll(task.ToArray());

            MessageBox.Show("Chuck轴运动结束");
        }

        private void btn_MoveCCD_Click(object sender, EventArgs e) {
            //获取当前位置信息
            if (txt_filePath.Text == string.Empty) {
                MessageBox.Show("请先选择目标文件");
                return;
            }

            //获取坐标信息
            SpecialPositionInfo info = new SpecialPositionInfo();
            UIClass.ControlToObject(panelSpecialPos, info);

            List<Task> task = new List<Task>();
            var t1 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_X].MoveAbsolute(info.CcdX); });
            task.Add(t1);
            var t2 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Y].MoveAbsolute(info.CcdY); });
            task.Add(t2);
            var t3 = Task.Run(() => { stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(info.CcdZ); });
            task.Add(t3);

            MessageBox.Show("CCD轴运动结束");
        }

        private void btn_MoveHeightU_Click(object sender, EventArgs e) {
            //获取当前位置信息
            if (txt_filePath.Text == string.Empty) {
                MessageBox.Show("请先选择目标文件");
                return;
            }

            //获取坐标信息
            SpecialPositionInfo info = new SpecialPositionInfo();
            UIClass.ControlToObject(panelSpecialPos, info);

            stageAxisDic[MyStageAxisKey.HEIGHT_U].MoveAbsolute(info.AltimeterU);
            MessageBox.Show("测高仪U轴运动结束");
        }
    }
}
