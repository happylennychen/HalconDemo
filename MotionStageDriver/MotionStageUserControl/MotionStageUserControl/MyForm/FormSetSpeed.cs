using System;
using System.Windows.Forms;

using MyMotionStageDriver.MyEnum;
using MyMotionStageDriver.MyStageAxis;

namespace MyMotionStageUserControl.MyForm {
    public partial class FormSetSpeed : Form {
        double speedNormal;
        double speedFast;
        double speedSlow;
        double accConfig;

        public FormSetSpeed(StageAxis stageAxis) {
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

        private void FormSetSpeed_Load(object sender, EventArgs e) {

            //stageAxis.GetAxisSpeed(ref minSpeedConfig, ref speedNormal, ref accConfig);
            speedFast = 2 * stageAxis.speedMaxConfig;
            speedSlow = 0.5 * stageAxis.speedMaxConfig;
            speedNormal = stageAxis.speedMaxConfig;
            accConfig = stageAxis.accConfig;
            cmbSpeed.SelectedIndex = 1; //Normal Speed
            /*
                        int index = cmbSpeed.Items.IndexOf(defaultSpeedInPps);
                        if (index < 0) {
                            cmbSpeed.Items.Add(defaultSpeedInPps);

                        }
                        cmbSpeed.SelectedIndex = cmbSpeed.Items.IndexOf(defaultSpeedInPps);
            */

        }

        private void btnOk_Click(object sender, EventArgs e) {
            if (!EnumMotionSpeed.TryParse(cmbSpeed.Text, out EnumMotionSpeed speedSelect)) {
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

            //stageAxis.SetSpeed(speedInPps);
            stageAxis.SetAxisSpeed(speed / 2, speed, accConfig);
            MessageBox.Show("Axis speed is modified successfully!");
            Close();
        }

        private readonly StageAxis stageAxis;        
    }
}
