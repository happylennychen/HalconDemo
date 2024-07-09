using MyMotionStageDriver.MyEnum;
using MyMotionStageDriver.MyStageAxis;
using System;
using System.Windows.Forms;

namespace MyMotionStageUserControl.MyForm {
    public partial class FormSetRealtimeSpeed : Form {
        double speedNormal;
        double speedFast;
        double speedSlow;

        public FormSetRealtimeSpeed(StageAxis stageAxis) {
            InitializeComponent();

            this.stageAxis = stageAxis;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            cmbSpeed.Items.Clear();
            switch (stageAxis.AxisType) {
                case EnumAxisType.LINEAR:
                    cmbSpeed.Items.Add(EnumMotionSpeed.FAST);
                    cmbSpeed.Items.Add(EnumMotionSpeed.NORMAL);
                    cmbSpeed.Items.Add(EnumMotionSpeed.SLOW);
                    break;
                case EnumAxisType.ROTATION:
                    cmbSpeed.Items.Add(EnumMotionSpeed.FAST);
                    cmbSpeed.Items.Add(EnumMotionSpeed.NORMAL);
                    cmbSpeed.Items.Add(EnumMotionSpeed.SLOW);
                    break;
            }
        }

        private void FormSetRealtimeSpeed_Load(object sender, EventArgs e) {
            speedFast = 2 * stageAxis.speedMaxConfig;
            speedSlow = 0.5 * stageAxis.speedMaxConfig;
            speedNormal = stageAxis.speedMaxConfig;
            cmbSpeed.SelectedIndex = 1; //Normal Speed
        }

        private void btnOk_Click(object sender, EventArgs e) {
            if (!EnumMotionSpeed.TryParse(cmbSpeed.Text, out EnumMotionSpeed speedSelect))            {
                MessageBox.Show("Speed is invalid!\n");
                cmbSpeed.Focus();
                return;
            }

            double speed = 0;
            if (speedSelect == EnumMotionSpeed.FAST) {
                speed = speedFast;
            } else if (speedSelect == EnumMotionSpeed.NORMAL) {
                speed = speedNormal;
            } else {
                speed = speedSlow;
            }

            int speed_tps = (int)speed;
            stageAxis.SetSpeed(speed_tps);
            MessageBox.Show("Axis speed is modified successfully!");
            Close();
        }

        private readonly StageAxis stageAxis;        
    }
}
