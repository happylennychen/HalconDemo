using System;
using System.Threading.Tasks;
using System.Windows.Forms;

using MyMotionStageDriver.MyEnum;
using MyMotionStageDriver.MyStageAxis;

namespace MyMotionStageUserControl.MyForm {
    public partial class FormMoveAbsolute : Form {
        public FormMoveAbsolute(StageAxis stageAxis, Action actUpdateCurrPos) {
            InitializeComponent();
            this.stageAxis = stageAxis;
            this.actUpdateCurrPos = actUpdateCurrPos;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void FormMoveAbsolute_Load(object sender, EventArgs e) {
            switch (stageAxis.AxisType) {
                case EnumAxisType.LINEAR:
                    lblUnit.Text = "um";
                    break;
                case EnumAxisType.ROTATION:
                    lblUnit.Text = "°";
                    break;
            }
        }

        private async void btnGo_Click(object sender, EventArgs e) {
            if (!double.TryParse(tbAbsPos.Text, out double newPosition)) {
                MessageBox.Show("Absolute position is invalid!\nIt should be a double.");
                tbAbsPos.Focus();
                return;
            }
            btnGo.Enabled = false;
            stageAxis.StartFastTimerRefreshPosition();
            await Task.Run(() => {
                stageAxis.MoveAbsolute(newPosition);
            });
            stageAxis.StopTimerRefreshPosition();
            actUpdateCurrPos();
            btnGo.Enabled = true;
        }

        private readonly StageAxis stageAxis;
        private readonly Action actUpdateCurrPos;
    }
}
