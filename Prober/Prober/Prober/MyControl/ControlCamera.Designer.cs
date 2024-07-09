namespace Prober.MyControl
{
    partial class ControlCamera
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.hw_Window = new HalconDotNet.HWindowControl();
            this.btn_cross_horizon_1 = new System.Windows.Forms.Button();
            this.btn_cross_vetical_4 = new System.Windows.Forms.Button();
            this.lbl_Cross_H = new System.Windows.Forms.Label();
            this.lbl_Cross_V = new System.Windows.Forms.Label();
            this.panel_tool = new System.Windows.Forms.Panel();
            this.btn_LineRotate = new System.Windows.Forms.Button();
            this.btn_Rotate = new System.Windows.Forms.Button();
            this.num_ExStep = new System.Windows.Forms.NumericUpDown();
            this.cmb_Zoom = new System.Windows.Forms.ComboBox();
            this.btn_MaxMin = new System.Windows.Forms.Button();
            this.btn_FullScreen = new System.Windows.Forms.Button();
            this.btn_SaveImamge = new System.Windows.Forms.Button();
            this.btn_Reduce = new System.Windows.Forms.Button();
            this.btn_Increase = new System.Windows.Forms.Button();
            this.btn_RefLine = new System.Windows.Forms.Button();
            this.btn_ExitMeasure = new System.Windows.Forms.Button();
            this.btn_MeasureAngle = new System.Windows.Forms.Button();
            this.btn_MeasureDistance = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lbl_MeasureValue = new System.Windows.Forms.Label();
            this.btn_AutoBalance = new System.Windows.Forms.Button();
            this.panel_tool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_ExStep)).BeginInit();
            this.SuspendLayout();
            // 
            // hw_Window
            // 
            this.hw_Window.BackColor = System.Drawing.Color.Black;
            this.hw_Window.BorderColor = System.Drawing.Color.Black;
            this.hw_Window.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hw_Window.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hw_Window.Location = new System.Drawing.Point(0, 0);
            this.hw_Window.Name = "hw_Window";
            this.hw_Window.Size = new System.Drawing.Size(954, 850);
            this.hw_Window.TabIndex = 0;
            this.hw_Window.WindowSize = new System.Drawing.Size(954, 850);
            this.hw_Window.HMouseWheel += new HalconDotNet.HMouseEventHandler(this.hw_Window_HMouseWheel);
            this.hw_Window.Load += new System.EventHandler(this.FrmCamera_Load);
            // 
            // btn_cross_horizon_1
            // 
            this.btn_cross_horizon_1.BackColor = System.Drawing.Color.Red;
            this.btn_cross_horizon_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_cross_horizon_1.Location = new System.Drawing.Point(3, 99);
            this.btn_cross_horizon_1.Name = "btn_cross_horizon_1";
            this.btn_cross_horizon_1.Size = new System.Drawing.Size(920, 5);
            this.btn_cross_horizon_1.TabIndex = 1;
            this.btn_cross_horizon_1.UseVisualStyleBackColor = false;
            this.btn_cross_horizon_1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_cross_horizon_1_MouseDown);
            this.btn_cross_horizon_1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btn_cross_horizon_1_MouseMove);
            this.btn_cross_horizon_1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_cross_horizon_1_MouseUp);
            // 
            // btn_cross_vetical_4
            // 
            this.btn_cross_vetical_4.BackColor = System.Drawing.Color.Red;
            this.btn_cross_vetical_4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_cross_vetical_4.Location = new System.Drawing.Point(101, 19);
            this.btn_cross_vetical_4.Name = "btn_cross_vetical_4";
            this.btn_cross_vetical_4.Size = new System.Drawing.Size(5, 750);
            this.btn_cross_vetical_4.TabIndex = 2;
            this.btn_cross_vetical_4.Text = "button2";
            this.btn_cross_vetical_4.UseVisualStyleBackColor = false;
            this.btn_cross_vetical_4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_cross_vetical_4_MouseDown);
            this.btn_cross_vetical_4.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btn_cross_vetical_4_MouseMove);
            this.btn_cross_vetical_4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_cross_vetical_4_MouseUp);
            // 
            // lbl_Cross_H
            // 
            this.lbl_Cross_H.BackColor = System.Drawing.Color.OrangeRed;
            this.lbl_Cross_H.Location = new System.Drawing.Point(392, 400);
            this.lbl_Cross_H.Name = "lbl_Cross_H";
            this.lbl_Cross_H.Size = new System.Drawing.Size(100, 1);
            this.lbl_Cross_H.TabIndex = 3;
            this.lbl_Cross_H.Visible = false;
            // 
            // lbl_Cross_V
            // 
            this.lbl_Cross_V.BackColor = System.Drawing.Color.OrangeRed;
            this.lbl_Cross_V.Location = new System.Drawing.Point(442, 351);
            this.lbl_Cross_V.Name = "lbl_Cross_V";
            this.lbl_Cross_V.Size = new System.Drawing.Size(1, 100);
            this.lbl_Cross_V.TabIndex = 4;
            this.lbl_Cross_V.Visible = false;
            // 
            // panel_tool
            // 
            this.panel_tool.Controls.Add(this.btn_AutoBalance);
            this.panel_tool.Controls.Add(this.btn_LineRotate);
            this.panel_tool.Controls.Add(this.btn_Rotate);
            this.panel_tool.Controls.Add(this.num_ExStep);
            this.panel_tool.Controls.Add(this.cmb_Zoom);
            this.panel_tool.Controls.Add(this.btn_MaxMin);
            this.panel_tool.Controls.Add(this.btn_FullScreen);
            this.panel_tool.Controls.Add(this.btn_SaveImamge);
            this.panel_tool.Controls.Add(this.btn_Reduce);
            this.panel_tool.Controls.Add(this.btn_Increase);
            this.panel_tool.Controls.Add(this.btn_RefLine);
            this.panel_tool.Controls.Add(this.btn_ExitMeasure);
            this.panel_tool.Controls.Add(this.btn_MeasureAngle);
            this.panel_tool.Controls.Add(this.btn_MeasureDistance);
            this.panel_tool.Location = new System.Drawing.Point(301, 12);
            this.panel_tool.Name = "panel_tool";
            this.panel_tool.Size = new System.Drawing.Size(613, 43);
            this.panel_tool.TabIndex = 5;
            // 
            // btn_LineRotate
            // 
            this.btn_LineRotate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_LineRotate.BackgroundImage = global::Prober.Properties.Resources.add;
            this.btn_LineRotate.Location = new System.Drawing.Point(526, 8);
            this.btn_LineRotate.Name = "btn_LineRotate";
            this.btn_LineRotate.Size = new System.Drawing.Size(32, 32);
            this.btn_LineRotate.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btn_LineRotate, "旋转线");
            this.btn_LineRotate.UseVisualStyleBackColor = false;
            this.btn_LineRotate.Click += new System.EventHandler(this.btn_LineRotate_Click);
            // 
            // btn_Rotate
            // 
            this.btn_Rotate.BackgroundImage = global::Prober.Properties.Resources.refresh;
            this.btn_Rotate.Location = new System.Drawing.Point(488, 7);
            this.btn_Rotate.Name = "btn_Rotate";
            this.btn_Rotate.Size = new System.Drawing.Size(32, 32);
            this.btn_Rotate.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btn_Rotate, "旋转90度");
            this.btn_Rotate.UseVisualStyleBackColor = true;
            this.btn_Rotate.Click += new System.EventHandler(this.btn_Rotate_Click);
            // 
            // num_ExStep
            // 
            this.num_ExStep.BackColor = System.Drawing.SystemColors.Control;
            this.num_ExStep.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.num_ExStep.Location = new System.Drawing.Point(261, 13);
            this.num_ExStep.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.num_ExStep.Name = "num_ExStep";
            this.num_ExStep.Size = new System.Drawing.Size(69, 21);
            this.num_ExStep.TabIndex = 2;
            this.num_ExStep.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // cmb_Zoom
            // 
            this.cmb_Zoom.BackColor = System.Drawing.SystemColors.Control;
            this.cmb_Zoom.FormattingEnabled = true;
            this.cmb_Zoom.Location = new System.Drawing.Point(85, 14);
            this.cmb_Zoom.Name = "cmb_Zoom";
            this.cmb_Zoom.Size = new System.Drawing.Size(57, 20);
            this.cmb_Zoom.TabIndex = 1;
            this.toolTip1.SetToolTip(this.cmb_Zoom, "放大倍率设置");
            this.cmb_Zoom.SelectedIndexChanged += new System.EventHandler(this.cmb_Zoom_SelectedIndexChanged);
            // 
            // btn_MaxMin
            // 
            this.btn_MaxMin.BackColor = System.Drawing.SystemColors.Control;
            this.btn_MaxMin.BackgroundImage = global::Prober.Properties.Resources.Max;
            this.btn_MaxMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_MaxMin.FlatAppearance.BorderSize = 0;
            this.btn_MaxMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_MaxMin.Location = new System.Drawing.Point(450, 7);
            this.btn_MaxMin.Name = "btn_MaxMin";
            this.btn_MaxMin.Size = new System.Drawing.Size(32, 32);
            this.btn_MaxMin.TabIndex = 0;
            this.btn_MaxMin.Tag = "0";
            this.toolTip1.SetToolTip(this.btn_MaxMin, "窗口最大/最小化");
            this.btn_MaxMin.UseVisualStyleBackColor = false;
            this.btn_MaxMin.Click += new System.EventHandler(this.btn_MaxMin_Click);
            // 
            // btn_FullScreen
            // 
            this.btn_FullScreen.BackColor = System.Drawing.SystemColors.Control;
            this.btn_FullScreen.BackgroundImage = global::Prober.Properties.Resources.方向;
            this.btn_FullScreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_FullScreen.FlatAppearance.BorderSize = 0;
            this.btn_FullScreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_FullScreen.Location = new System.Drawing.Point(412, 7);
            this.btn_FullScreen.Name = "btn_FullScreen";
            this.btn_FullScreen.Size = new System.Drawing.Size(32, 32);
            this.btn_FullScreen.TabIndex = 0;
            this.btn_FullScreen.Tag = "0";
            this.toolTip1.SetToolTip(this.btn_FullScreen, "全屏");
            this.btn_FullScreen.UseVisualStyleBackColor = false;
            this.btn_FullScreen.Click += new System.EventHandler(this.btn_FullScreen_Click);
            // 
            // btn_SaveImamge
            // 
            this.btn_SaveImamge.BackColor = System.Drawing.SystemColors.Control;
            this.btn_SaveImamge.BackgroundImage = global::Prober.Properties.Resources.download;
            this.btn_SaveImamge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_SaveImamge.FlatAppearance.BorderSize = 0;
            this.btn_SaveImamge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SaveImamge.Location = new System.Drawing.Point(374, 7);
            this.btn_SaveImamge.Name = "btn_SaveImamge";
            this.btn_SaveImamge.Size = new System.Drawing.Size(32, 32);
            this.btn_SaveImamge.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btn_SaveImamge, "保存图片");
            this.btn_SaveImamge.UseVisualStyleBackColor = false;
            this.btn_SaveImamge.Click += new System.EventHandler(this.btn_SaveImamge_Click);
            // 
            // btn_Reduce
            // 
            this.btn_Reduce.BackColor = System.Drawing.SystemColors.Control;
            this.btn_Reduce.BackgroundImage = global::Prober.Properties.Resources.LightDe;
            this.btn_Reduce.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_Reduce.FlatAppearance.BorderSize = 0;
            this.btn_Reduce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Reduce.Location = new System.Drawing.Point(218, 6);
            this.btn_Reduce.Name = "btn_Reduce";
            this.btn_Reduce.Size = new System.Drawing.Size(32, 32);
            this.btn_Reduce.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btn_Reduce, "降低亮度");
            this.btn_Reduce.UseVisualStyleBackColor = false;
            this.btn_Reduce.Click += new System.EventHandler(this.btn_Reduce_Click);
            // 
            // btn_Increase
            // 
            this.btn_Increase.BackColor = System.Drawing.SystemColors.Control;
            this.btn_Increase.BackgroundImage = global::Prober.Properties.Resources.LightAdd;
            this.btn_Increase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_Increase.FlatAppearance.BorderSize = 0;
            this.btn_Increase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Increase.Location = new System.Drawing.Point(336, 7);
            this.btn_Increase.Name = "btn_Increase";
            this.btn_Increase.Size = new System.Drawing.Size(32, 32);
            this.btn_Increase.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btn_Increase, "增加亮度");
            this.btn_Increase.UseVisualStyleBackColor = false;
            this.btn_Increase.Click += new System.EventHandler(this.btn_Increase_Click);
            // 
            // btn_RefLine
            // 
            this.btn_RefLine.BackColor = System.Drawing.SystemColors.Control;
            this.btn_RefLine.BackgroundImage = global::Prober.Properties.Resources.网格__1_;
            this.btn_RefLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_RefLine.FlatAppearance.BorderSize = 0;
            this.btn_RefLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_RefLine.Location = new System.Drawing.Point(185, 7);
            this.btn_RefLine.Name = "btn_RefLine";
            this.btn_RefLine.Size = new System.Drawing.Size(32, 32);
            this.btn_RefLine.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btn_RefLine, "参考线显示与隐藏");
            this.btn_RefLine.UseVisualStyleBackColor = false;
            this.btn_RefLine.Click += new System.EventHandler(this.btn_RefLine_Click);
            // 
            // btn_ExitMeasure
            // 
            this.btn_ExitMeasure.BackColor = System.Drawing.SystemColors.Control;
            this.btn_ExitMeasure.BackgroundImage = global::Prober.Properties.Resources.clear__4_;
            this.btn_ExitMeasure.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_ExitMeasure.FlatAppearance.BorderSize = 0;
            this.btn_ExitMeasure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ExitMeasure.Location = new System.Drawing.Point(147, 7);
            this.btn_ExitMeasure.Name = "btn_ExitMeasure";
            this.btn_ExitMeasure.Size = new System.Drawing.Size(32, 32);
            this.btn_ExitMeasure.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btn_ExitMeasure, "退出测量");
            this.btn_ExitMeasure.UseVisualStyleBackColor = false;
            this.btn_ExitMeasure.Click += new System.EventHandler(this.btn_ExitMeasure_Click);
            // 
            // btn_MeasureAngle
            // 
            this.btn_MeasureAngle.BackColor = System.Drawing.SystemColors.Control;
            this.btn_MeasureAngle.BackgroundImage = global::Prober.Properties.Resources.size1;
            this.btn_MeasureAngle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_MeasureAngle.FlatAppearance.BorderSize = 0;
            this.btn_MeasureAngle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_MeasureAngle.Location = new System.Drawing.Point(47, 7);
            this.btn_MeasureAngle.Name = "btn_MeasureAngle";
            this.btn_MeasureAngle.Size = new System.Drawing.Size(32, 32);
            this.btn_MeasureAngle.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btn_MeasureAngle, "角度测量");
            this.btn_MeasureAngle.UseVisualStyleBackColor = false;
            this.btn_MeasureAngle.Click += new System.EventHandler(this.btn_MeasureAngle_Click);
            // 
            // btn_MeasureDistance
            // 
            this.btn_MeasureDistance.BackColor = System.Drawing.SystemColors.Control;
            this.btn_MeasureDistance.BackgroundImage = global::Prober.Properties.Resources.测量;
            this.btn_MeasureDistance.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_MeasureDistance.FlatAppearance.BorderSize = 0;
            this.btn_MeasureDistance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_MeasureDistance.Location = new System.Drawing.Point(9, 7);
            this.btn_MeasureDistance.Name = "btn_MeasureDistance";
            this.btn_MeasureDistance.Size = new System.Drawing.Size(32, 32);
            this.btn_MeasureDistance.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btn_MeasureDistance, "距离测量");
            this.btn_MeasureDistance.UseVisualStyleBackColor = false;
            this.btn_MeasureDistance.Click += new System.EventHandler(this.btn_MeasureDistance_Click);
            // 
            // lbl_MeasureValue
            // 
            this.lbl_MeasureValue.AutoSize = true;
            this.lbl_MeasureValue.Location = new System.Drawing.Point(148, 54);
            this.lbl_MeasureValue.Name = "lbl_MeasureValue";
            this.lbl_MeasureValue.Size = new System.Drawing.Size(0, 12);
            this.lbl_MeasureValue.TabIndex = 6;
            // 
            // btn_AutoBalance
            // 
            this.btn_AutoBalance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_AutoBalance.Location = new System.Drawing.Point(564, 8);
            this.btn_AutoBalance.Name = "btn_AutoBalance";
            this.btn_AutoBalance.Size = new System.Drawing.Size(32, 32);
            this.btn_AutoBalance.TabIndex = 5;
            this.btn_AutoBalance.Text = "AB";
            this.toolTip1.SetToolTip(this.btn_AutoBalance, "自动白平衡");
            this.btn_AutoBalance.UseVisualStyleBackColor = false;
            this.btn_AutoBalance.Click += new System.EventHandler(this.btn_AutoBalance_Click);
            // 
            // ControlCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_MeasureValue);
            this.Controls.Add(this.btn_cross_vetical_4);
            this.Controls.Add(this.panel_tool);
            this.Controls.Add(this.lbl_Cross_V);
            this.Controls.Add(this.lbl_Cross_H);
            this.Controls.Add(this.btn_cross_horizon_1);
            this.Controls.Add(this.hw_Window);
            this.Name = "ControlCamera";
            this.Size = new System.Drawing.Size(954, 850);
            this.panel_tool.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.num_ExStep)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HalconDotNet.HWindowControl hw_Window;
        private System.Windows.Forms.Button btn_cross_horizon_1;
        private System.Windows.Forms.Button btn_cross_vetical_4;
        private System.Windows.Forms.Label lbl_Cross_H;
        private System.Windows.Forms.Label lbl_Cross_V;
        private System.Windows.Forms.Panel panel_tool;
        private System.Windows.Forms.Button btn_MeasureDistance;
        private System.Windows.Forms.Button btn_MaxMin;
        private System.Windows.Forms.Button btn_FullScreen;
        private System.Windows.Forms.Button btn_SaveImamge;
        private System.Windows.Forms.Button btn_Reduce;
        private System.Windows.Forms.Button btn_Increase;
        private System.Windows.Forms.Button btn_RefLine;
        private System.Windows.Forms.Button btn_ExitMeasure;
        private System.Windows.Forms.Button btn_MeasureAngle;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown num_ExStep;
        private System.Windows.Forms.ComboBox cmb_Zoom;
        private System.Windows.Forms.Button btn_Rotate;
        private System.Windows.Forms.Label lbl_MeasureValue;
        private System.Windows.Forms.Button btn_LineRotate;
        private System.Windows.Forms.Button btn_AutoBalance;
    }
}
