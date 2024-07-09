namespace Prober.Forms
{
    partial class FormPowerStableTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.num_Record_Time = new System.Windows.Forms.NumericUpDown();
            this.num_Record_Interval = new System.Windows.Forms.NumericUpDown();
            this.lbl_Record_Value = new System.Windows.Forms.Label();
            this.btn_Record_Start = new System.Windows.Forms.Button();
            this.btn_Record_Stop = new System.Windows.Forms.Button();
            this.btn_Record_View = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.num_Pm_Slot = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.num_Pm_Channel = new System.Windows.Forms.NumericUpDown();
            this.lbl_Info = new System.Windows.Forms.Label();
            this.chart_Power = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_TestStartTime = new System.Windows.Forms.TextBox();
            this.txt_TestEllapseTime = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.num_Record_Time)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Record_Interval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Pm_Slot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Pm_Channel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Power)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "监控时长(min)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "采样间隔(ms)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "当前功率";
            // 
            // num_Record_Time
            // 
            this.num_Record_Time.DecimalPlaces = 1;
            this.num_Record_Time.Location = new System.Drawing.Point(162, 30);
            this.num_Record_Time.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.num_Record_Time.Name = "num_Record_Time";
            this.num_Record_Time.Size = new System.Drawing.Size(120, 21);
            this.num_Record_Time.TabIndex = 1;
            this.num_Record_Time.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // num_Record_Interval
            // 
            this.num_Record_Interval.Location = new System.Drawing.Point(162, 66);
            this.num_Record_Interval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.num_Record_Interval.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.num_Record_Interval.Name = "num_Record_Interval";
            this.num_Record_Interval.Size = new System.Drawing.Size(120, 21);
            this.num_Record_Interval.TabIndex = 1;
            this.num_Record_Interval.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lbl_Record_Value
            // 
            this.lbl_Record_Value.Location = new System.Drawing.Point(160, 174);
            this.lbl_Record_Value.Name = "lbl_Record_Value";
            this.lbl_Record_Value.Size = new System.Drawing.Size(100, 23);
            this.lbl_Record_Value.TabIndex = 2;
            this.lbl_Record_Value.Text = "N/A";
            // 
            // btn_Record_Start
            // 
            this.btn_Record_Start.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_Record_Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Record_Start.Location = new System.Drawing.Point(37, 295);
            this.btn_Record_Start.Name = "btn_Record_Start";
            this.btn_Record_Start.Size = new System.Drawing.Size(75, 32);
            this.btn_Record_Start.TabIndex = 3;
            this.btn_Record_Start.Text = "开始";
            this.btn_Record_Start.UseVisualStyleBackColor = false;
            this.btn_Record_Start.Click += new System.EventHandler(this.btn_Record_Start_Click);
            // 
            // btn_Record_Stop
            // 
            this.btn_Record_Stop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_Record_Stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Record_Stop.Location = new System.Drawing.Point(124, 295);
            this.btn_Record_Stop.Name = "btn_Record_Stop";
            this.btn_Record_Stop.Size = new System.Drawing.Size(75, 32);
            this.btn_Record_Stop.TabIndex = 3;
            this.btn_Record_Stop.Text = "停止";
            this.btn_Record_Stop.UseVisualStyleBackColor = false;
            this.btn_Record_Stop.Click += new System.EventHandler(this.btn_Record_Stop_Click);
            // 
            // btn_Record_View
            // 
            this.btn_Record_View.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_Record_View.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Record_View.Location = new System.Drawing.Point(212, 295);
            this.btn_Record_View.Name = "btn_Record_View";
            this.btn_Record_View.Size = new System.Drawing.Size(75, 32);
            this.btn_Record_View.TabIndex = 3;
            this.btn_Record_View.Text = "查看数据";
            this.btn_Record_View.UseVisualStyleBackColor = false;
            this.btn_Record_View.Click += new System.EventHandler(this.btn_Record_View_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "功率计槽位";
            // 
            // num_Pm_Slot
            // 
            this.num_Pm_Slot.Location = new System.Drawing.Point(162, 104);
            this.num_Pm_Slot.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.num_Pm_Slot.Name = "num_Pm_Slot";
            this.num_Pm_Slot.Size = new System.Drawing.Size(120, 21);
            this.num_Pm_Slot.TabIndex = 1;
            this.num_Pm_Slot.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(35, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "功率计通道";
            // 
            // num_Pm_Channel
            // 
            this.num_Pm_Channel.Location = new System.Drawing.Point(162, 142);
            this.num_Pm_Channel.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.num_Pm_Channel.Name = "num_Pm_Channel";
            this.num_Pm_Channel.Size = new System.Drawing.Size(120, 21);
            this.num_Pm_Channel.TabIndex = 1;
            this.num_Pm_Channel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbl_Info
            // 
            this.lbl_Info.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbl_Info.Location = new System.Drawing.Point(35, 197);
            this.lbl_Info.Name = "lbl_Info";
            this.lbl_Info.Size = new System.Drawing.Size(252, 23);
            this.lbl_Info.TabIndex = 4;
            // 
            // chart_Power
            // 
            chartArea1.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea1.AxisY.IsStartedFromZero = false;
            chartArea1.Name = "ChartArea1";
            this.chart_Power.ChartAreas.Add(chartArea1);
            this.chart_Power.Location = new System.Drawing.Point(307, 30);
            this.chart_Power.Name = "chart_Power";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "Series1";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            this.chart_Power.Series.Add(series1);
            this.chart_Power.Size = new System.Drawing.Size(563, 335);
            this.chart_Power.TabIndex = 5;
            this.chart_Power.Text = "chart1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(35, 234);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "测试开始时间";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(35, 263);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "已测试时间(S)";
            // 
            // txt_TestStartTime
            // 
            this.txt_TestStartTime.Location = new System.Drawing.Point(141, 231);
            this.txt_TestStartTime.Name = "txt_TestStartTime";
            this.txt_TestStartTime.ReadOnly = true;
            this.txt_TestStartTime.Size = new System.Drawing.Size(146, 21);
            this.txt_TestStartTime.TabIndex = 7;
            // 
            // txt_TestEllapseTime
            // 
            this.txt_TestEllapseTime.Location = new System.Drawing.Point(141, 258);
            this.txt_TestEllapseTime.Name = "txt_TestEllapseTime";
            this.txt_TestEllapseTime.ReadOnly = true;
            this.txt_TestEllapseTime.Size = new System.Drawing.Size(146, 21);
            this.txt_TestEllapseTime.TabIndex = 7;
            // 
            // FormPowerStableTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 407);
            this.Controls.Add(this.txt_TestEllapseTime);
            this.Controls.Add(this.txt_TestStartTime);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chart_Power);
            this.Controls.Add(this.lbl_Info);
            this.Controls.Add(this.btn_Record_View);
            this.Controls.Add(this.btn_Record_Stop);
            this.Controls.Add(this.btn_Record_Start);
            this.Controls.Add(this.lbl_Record_Value);
            this.Controls.Add(this.num_Record_Interval);
            this.Controls.Add(this.num_Pm_Channel);
            this.Controls.Add(this.num_Pm_Slot);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.num_Record_Time);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormPowerStableTest";
            this.Text = "FormPowerStableTest";
            ((System.ComponentModel.ISupportInitialize)(this.num_Record_Time)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Record_Interval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Pm_Slot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Pm_Channel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Power)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown num_Record_Time;
        private System.Windows.Forms.NumericUpDown num_Record_Interval;
        private System.Windows.Forms.Label lbl_Record_Value;
        private System.Windows.Forms.Button btn_Record_Start;
        private System.Windows.Forms.Button btn_Record_Stop;
        private System.Windows.Forms.Button btn_Record_View;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown num_Pm_Slot;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown num_Pm_Channel;
        private System.Windows.Forms.Label lbl_Info;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Power;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_TestStartTime;
        private System.Windows.Forms.TextBox txt_TestEllapseTime;
    }
}