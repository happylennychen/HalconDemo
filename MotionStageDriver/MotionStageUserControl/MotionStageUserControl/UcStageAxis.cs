using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

using CommonApi.MyI18N;
using CommonApi.MyUtility;

using MyMotionStageDriver.MyEnum;
using MyMotionStageDriver.MyStageAxis;

using MyMotionStageUserControl.MyForm;

using NLog;

namespace MyMotionStageUserControl {
    public partial class UcStageAxis : UserControl {
        internal UcStageAxis(EnumLanguage language, StageAxis stageAxis) {
            InitializeComponent();

            frc = new FormResourceCulture(this.GetType().FullName, Assembly.GetExecutingAssembly());
            frc.Language = language;

            this.stageAxis = stageAxis;
            tbCurrPos.ReadOnly = true;
            this.ContextMenuStrip = contextMenu;

            timerRefreshPosition.Enabled = false;
            timerRefreshPosition.Tick += TimerRefreshPosition_Tick;
            stageAxis.AttachActionRefreshGuiPosition(StartFastTimerRefreshPosition, StartSlowTimerRefreshPosition, StopTimerRefreshPosition, GuiUpdatePosition);
        }

        public StageAxis CurrentStageAxis { get { return this.stageAxis; } }

        public void GuiUpdatePosition() {
            this.BeginInvoke(new Action(() => {
                tbCurrPos.Text = stageAxis.Position().ToString("f3");
            }));
        }

        public void GuiUpdateAccumulatedRelativeMoveDistance(double relMoveDistance) {
            this.BeginInvoke(new Action(() => {
                if (tabControl.SelectedTab == tpAccumRelMov) {
                    accumulatedRelativeMoveDistance += relMoveDistance;
                    tbAccumulatedRelMov.Text = accumulatedRelativeMoveDistance.ToString("f3");
                }
            }));
        }

        public void Enable(bool isEnabled) {
            this.Enabled = isEnabled;
        }

        private void ucStageAxis_Load(object sender, EventArgs e) {
            this.lblAxisId.Text = stageAxis.AxisId;
            switch (stageAxis.AxisType) {
                case EnumAxisType.LINEAR:
                    break;
                case EnumAxisType.ROTATION:
                    gbBase.BackColor = Color.MediumVioletRed;
                    break;
            }

            if (stageAxis.PositiveDirection == EnumAxisPositiveDirection.None) {
                this.pbPositiveDirection.Visible = false;
            } else {
                this.pbPositiveDirection.Visible = true;
                this.pbPositiveDirection.SizeMode = PictureBoxSizeMode.StretchImage;
                toolTip.SetToolTip(this.pbPositiveDirection, "+ direction");
                switch (stageAxis.PositiveDirection) {
                    case EnumAxisPositiveDirection.LEFT:
                        this.pbPositiveDirection.Image = Properties.Resources.left;
                        break;
                    case EnumAxisPositiveDirection.RIGHT:
                        this.pbPositiveDirection.Image = Properties.Resources.right;
                        break;
                    case EnumAxisPositiveDirection.UP:
                        this.pbPositiveDirection.Image = Properties.Resources.up;
                        break;
                    case EnumAxisPositiveDirection.DOWN:
                        this.pbPositiveDirection.Image = Properties.Resources.down;
                        break;
                    case EnumAxisPositiveDirection.FORWARD:
                        this.pbPositiveDirection.Image = Properties.Resources.forward;
                        break;
                    case EnumAxisPositiveDirection.BACKWARD:
                        this.pbPositiveDirection.Image = Properties.Resources.backward;
                        break;
                }
            }

            SettingTimerRefreshPosition(NORMAL_REFRESH_INTERVAL_IN_MS);
            timerRefreshPosition.Enabled = false;

            HashSet<Control> excludedControls = new HashSet<Control>{
                lblAxisId
                , btnStepNegative
                , btnStepPositive
            };
            frc.ExcludedControls(excludedControls);
            frc.EnumerateControl(this);
        }

        private async void btnStepPositive_Click(object sender, EventArgs e) {           
            try {
                string windowName = frc.GetString("txtMainWindowName");
                IntPtr hWnd = FindWindow(null, windowName);

                StageMoveInfo moveInfo = new StageMoveInfo();
                moveInfo.AxisName = lblAxisId.Text;
                moveInfo.dir = 1;

                moveInfo.distance = Convert.ToDouble(tbRelMov.Text);

                IntPtr moveInfoPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(StageMoveInfo)));
                Marshal.StructureToPtr(moveInfo, moveInfoPtr, false);
                SendMessage(hWnd, WM_AXIS_MOVE, 0, moveInfoPtr);

                await StepRelMove(EnumSteppedControllerChannelDirection.POSITIVE);
            }  catch (Exception ex) { 
                MessageBox.Show($"step move error: {ex.Message}");
            }           
        }

        private async void btnStepNegative_Click(object sender, EventArgs e) {
            try {
                string windowName = frc.GetString("txtMainWindowName");
                IntPtr hWnd = FindWindow(null, windowName);

                StageMoveInfo moveInfo = new StageMoveInfo();
                moveInfo.AxisName = lblAxisId.Text;
                moveInfo.dir = -1;
                moveInfo.distance = Convert.ToDouble(tbRelMov.Text);

                IntPtr moveInfoPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(StageMoveInfo)));
                Marshal.StructureToPtr(moveInfo, moveInfoPtr, false);
                SendMessage(hWnd, WM_AXIS_MOVE, 0, moveInfoPtr);

                await StepRelMove(EnumSteppedControllerChannelDirection.NEGATIVE);
            }
            catch(Exception ex) {
                MessageBox.Show($"step move error: {ex.Message}");
            }            
        }

        private async Task StepRelMove(EnumSteppedControllerChannelDirection direction) {
            if (!double.TryParse(tbRelMov.Text, out double distance)) {
                MessageBox.Show("Distance is invalid!");
                tbRelMov.Focus();
                return;
            }

            switch (direction) {
                case EnumSteppedControllerChannelDirection.POSITIVE:
                    btnStepPositive.Enabled = false;
                    break;
                case EnumSteppedControllerChannelDirection.NEGATIVE:
                    btnStepNegative.Enabled = false;
                    break;
            }
            if (direction == EnumSteppedControllerChannelDirection.NEGATIVE) {
                distance *= -1.0;
            }

            bool bRet = true;

            await Task.Run(() => {
                bRet = stageAxis.MoveRelative(distance);
            });
            GuiUpdatePosition();
            double tps = stageAxis.GetTravelPerPulse();
            if ((int)Math.Round(distance / tps) >= 1) {
                GuiUpdateAccumulatedRelativeMoveDistance(distance);
            }

            switch (direction) {
                case EnumSteppedControllerChannelDirection.POSITIVE:
                    btnStepPositive.Enabled = true;
                    break;
                case EnumSteppedControllerChannelDirection.NEGATIVE:
                    btnStepNegative.Enabled = true;
                    break;
            }

            if (!bRet) {
                this.BeginInvoke(new Action(() => {
                    MessageBox.Show("move error, please check if reach limited position");
                }));
            }
        }

        private void tbRelMov_Enter(object sender, EventArgs e) {
            tbRelMov.BackColor = Color.Moccasin;
        }

        private void tbRelMov_Leave(object sender, EventArgs e) {
            tbRelMov.BackColor = Color.White;
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e) {
            accumulatedRelativeMoveDistance = 0.0;
            tbAccumulatedRelMov.Text = accumulatedRelativeMoveDistance.ToString("f3"); ;
        }

        private void tbRelMov_KeyUp(object sender, KeyEventArgs e) {
            if (!e.KeyCode.Equals(Keys.Up) && !e.KeyCode.Equals(Keys.Down)) {
                return;
            }

            switch (e.KeyCode) {
                case Keys.Up:
                    btnStepPositive.PerformClick();
                    break;
                case Keys.Down:
                    btnStepNegative.PerformClick();
                    break;
            }
            tbRelMov.SelectionStart = tbRelMov.Text.Length;
            tbRelMov.ScrollToCaret();
        }

        private void miSetMotionSpeed_Click(object sender, EventArgs e) {
            FormSetSpeed form = new FormSetSpeed(stageAxis);
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }

        private void miSetRealtimeSpeed_Click(object sender, EventArgs e) {
            FormSetRealtimeSpeed form = new FormSetRealtimeSpeed(stageAxis);
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }

        private void miMoveAbsolute_Click(object sender, EventArgs e) {
            FormMoveAbsolute form = new FormMoveAbsolute(stageAxis, GuiUpdatePosition);
            form.StartPosition = FormStartPosition.CenterParent;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.ShowDialog();
        }

        private void miSetMotionRange_Click(object sender, EventArgs e) {
            FormSetMotionRange form = new FormSetMotionRange(stageAxis);
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }

        private async void miHoming_Click(object sender, EventArgs e) {
            DialogResult dialogResult = MessageBox.Show("Are you sure to do homing?", "Confirm", MessageBoxButtons.YesNo);
            if (dialogResult.Equals(DialogResult.No)) {
                return;
            }

            this.ContextMenuStrip = null;
            btnStepNegative.Enabled = false;
            btnStepPositive.Enabled = false;
            StartFastTimerRefreshPosition();

            bool homingResult = false;
            await Task.Run(() => {
                homingResult = stageAxis.Homing();
            });

            StopTimerRefreshPosition();
            GuiUpdatePosition();
            btnStepNegative.Enabled = true;
            btnStepPositive.Enabled = true;
            this.ContextMenuStrip = contextMenu;

            string message;
            if (homingResult) {
                message = "Homing has been successfully completed!";
            } else {
                message = "Failed to do homing! Please look up log for detailed reason.";
            }
            MessageBox.Show(message);
        }

        private async void miStop_Click(object sender, EventArgs e) {
            DialogResult dialogResult = MessageBox.Show("Are you sure to stop?", "Confirm", MessageBoxButtons.YesNo);
            if (dialogResult.Equals(DialogResult.No)) {
                return;
            }

            this.ContextMenuStrip = null;
            btnStepNegative.Enabled = false;
            btnStepPositive.Enabled = false;
            await Task.Run(() => {
                stageAxis.Stop();
            });

            GuiUpdatePosition();
            btnStepNegative.Enabled = true;
            btnStepPositive.Enabled = true;
            this.ContextMenuStrip = contextMenu;
        }

        private void StartFastTimerRefreshPosition() {
            SettingTimerRefreshPosition(NORMAL_REFRESH_INTERVAL_IN_MS);
            timerRefreshPosition.Start();
            //timerRefreshPosition.Enabled = true;
        }

        public void StartSlowTimerRefreshPosition() {
            SettingTimerRefreshPosition(SLOW_REFRESH_INTERVAL_IN_MS);
            timerRefreshPosition.Start();
            //timerRefreshPosition.Enabled = true;
        }

        private void SettingTimerRefreshPosition(int intervalInMs) {
            timerRefreshPosition.Interval = intervalInMs;
        }

        public void StopTimerRefreshPosition() {
            timerRefreshPosition.Stop();
            //timerRefreshPosition.Enabled = false;
        }

        private void TimerRefreshPosition_Tick(object sender, EventArgs e) {
            timerRefreshPosition.Stop();
            tbCurrPos.Text = stageAxis.Position().ToString("f3");
            timerRefreshPosition.Start();
        }

        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        private readonly StageAxis stageAxis;
        private readonly System.Windows.Forms.Timer timerRefreshPosition = new System.Windows.Forms.Timer();
        private const int NORMAL_REFRESH_INTERVAL_IN_MS = 1000;
        private readonly int SLOW_REFRESH_INTERVAL_IN_MS = 3 * NORMAL_REFRESH_INTERVAL_IN_MS;
        private readonly ToolTip toolTip = new ToolTip();
        private readonly FormResourceCulture frc;
        private double accumulatedRelativeMoveDistance = 0.0;

        public const int WM_AXIS_MOVE = 0xF200;

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr IParam);


        [DllImport("User32.dll")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
    }
}
