using System;
using System.Windows.Forms;
using CommonApi.MyUtility;
using MyMotionStageDriver.MyStageAxis;

using MyMotionStageUserControl.MyConstant;

using NLog;


namespace MyMotionStageUserControl.MyForm {
    public partial class FormSetMotionRange : Form {
        public FormSetMotionRange(StageAxis stageAxis) {
            InitializeComponent();
            this.stageAxis = stageAxis;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }
      
        private void FormSetMotionRange_Load(object sender, EventArgs e) {
            lblLowerLimitUnit.Text = stageAxis.AxisType == MyMotionStageDriver.MyEnum.EnumAxisType.LINEAR ? JogConstant.LINEAR_AXIS_UNIT : JogConstant.ROTATION_AXIS_UNIT;
            lblUpperLimitUnit.Text = stageAxis.AxisType == MyMotionStageDriver.MyEnum.EnumAxisType.LINEAR ? JogConstant.LINEAR_AXIS_UNIT : JogConstant.ROTATION_AXIS_UNIT;

            //if (stageAxis.EnableSoftUpperLimit) {
            //    rbUpperLimitDefine.Checked = true;
            //    tbUpperLimit.Text = stageAxis.SoftUpperLimit.ToString();                
            //} else {
            //    rbUpperLimitNone.Checked = true;
            //}

            //if (stageAxis.EnableSoftLowerLimit) {
            //    rbLowerLimitDefine.Checked = true;
            //    tbLowerLimit.Text = stageAxis.SoftLowerLimit.ToString();
            //} else {
            //    rbLowerLimitNone.Checked = true;
            //}
        }
       
        private void btnOk_Click(object sender, EventArgs e) {
            if (!CheckInput()) {
                return;
            }

            //if (rbLowerLimitDefine.Checked) {
            //    double lowerLimit = Convert.ToDouble(tbLowerLimit.Text);
            //    stageAxis.EnableSoftLowerLimit = true;
            //    stageAxis.SoftLowerLimit = lowerLimit;
            //    LOGGER.Info($"Soft lower limit of axis[{stageAxis.StageId}, {stageAxis.AxisId}] is set to {lowerLimit}");
            //} else {
            //    stageAxis.EnableSoftLowerLimit = false;
            //    LOGGER.Info($"Soft lower limit of axis[{stageAxis.StageId}, {stageAxis.AxisId}] is disabled.");
            //}

            //if (rbUpperLimitDefine.Checked) {
            //    double upperLimit = Convert.ToDouble(tbUpperLimit.Text);
            //    stageAxis.EnableSoftUpperLimit = true;
            //    stageAxis.SoftUpperLimit = upperLimit;
            //    LOGGER.Info($"Soft upper limit of axis[{stageAxis.StageId}, {stageAxis.AxisId}] is set to {upperLimit}");
            //} else {
            //    stageAxis.EnableSoftUpperLimit = false;
            //    LOGGER.Info($"Soft upper limit of axis[{stageAxis.StageId}, {stageAxis.AxisId}] is disabled.");
            //}

            MessageBox.Show("Axis motion ranges are modified successfully!");
            Close();
        }

        private bool CheckInput() {
            if (rbLowerLimitDefine.Checked) {
                if (!double.TryParse(tbLowerLimit.Text, out double lowerLimit)) {
                    MessageBox.Show("Lower limit is invalid!");
                    tbLowerLimit.Focus();
                    return false;
                }
            }

            if (!rbUpperLimitDefine.Checked) {
                if (!double.TryParse(tbUpperLimit.Text, out double upperLimit)) {
                    MessageBox.Show("Upper limit is invalid!");
                    tbUpperLimit.Focus();
                    return false;
                }
            }

            return true;
        }

        private readonly StageAxis stageAxis;
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
