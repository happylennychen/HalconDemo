namespace Prober.Forms
{
    partial class FormHeightScan
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lv_H = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chart_H = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btn_Add = new System.Windows.Forms.Button();
            this.btn_HeightCalibration_Loading = new System.Windows.Forms.Button();
            this.btn_HeightCalibration_Save = new System.Windows.Forms.Button();
            this.btn_RunHeightCalibrate = new System.Windows.Forms.Button();
            this.btn_StopHeightCalibrate = new System.Windows.Forms.Button();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_ReticleWidth = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_ArearCircle = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_HeightThreshold = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_CenterX2 = new System.Windows.Forms.TextBox();
            this.txt_ReticleHeight = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_CenterY2 = new System.Windows.Forms.TextBox();
            this.chb_AltimeterTypeCap = new System.Windows.Forms.CheckBox();
            this.dgv_BasicSamplePoint = new System.Windows.Forms.DataGridView();
            this.panel_HeightCalibrate = new System.Windows.Forms.Panel();
            this.btn_InitPos = new System.Windows.Forms.Button();
            this.btn_AutoSerch = new System.Windows.Forms.Button();
            this.rtbMsgBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.chart_H)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_BasicSamplePoint)).BeginInit();
            this.panel_HeightCalibrate.SuspendLayout();
            this.SuspendLayout();
            // 
            // lv_H
            // 
            this.lv_H.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lv_H.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lv_H.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.lv_H.GridLines = true;
            this.lv_H.HideSelection = false;
            this.lv_H.Location = new System.Drawing.Point(893, 23);
            this.lv_H.Name = "lv_H";
            this.lv_H.Size = new System.Drawing.Size(302, 224);
            this.lv_H.TabIndex = 0;
            this.lv_H.UseCompatibleStateImageBehavior = false;
            this.lv_H.View = System.Windows.Forms.View.Details;
            this.lv_H.SelectedIndexChanged += new System.EventHandler(this.lv_H_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "NO.";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "X";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Y";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 80;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "H";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 80;
            // 
            // chart_H
            // 
            chartArea3.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea3.AxisX.LineWidth = 2;
            chartArea3.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea3.AxisY.LineWidth = 2;
            chartArea3.Name = "ChartArea1";
            this.chart_H.ChartAreas.Add(chartArea3);
            legend3.Enabled = false;
            legend3.Name = "Legend1";
            this.chart_H.Legends.Add(legend3);
            this.chart_H.Location = new System.Drawing.Point(12, 78);
            this.chart_H.Name = "chart_H";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series3.Legend = "Legend1";
            series3.MarkerSize = 8;
            series3.Name = "Series1";
            this.chart_H.Series.Add(series3);
            this.chart_H.Size = new System.Drawing.Size(580, 470);
            this.chart_H.TabIndex = 1;
            this.chart_H.Text = "chart1";
            // 
            // btn_Add
            // 
            this.btn_Add.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_Add.Location = new System.Drawing.Point(758, 254);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(95, 31);
            this.btn_Add.TabIndex = 4;
            this.btn_Add.Text = "手动添加";
            this.btn_Add.UseVisualStyleBackColor = false;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // btn_HeightCalibration_Loading
            // 
            this.btn_HeightCalibration_Loading.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_HeightCalibration_Loading.Location = new System.Drawing.Point(937, 254);
            this.btn_HeightCalibration_Loading.Name = "btn_HeightCalibration_Loading";
            this.btn_HeightCalibration_Loading.Size = new System.Drawing.Size(95, 31);
            this.btn_HeightCalibration_Loading.TabIndex = 4;
            this.btn_HeightCalibration_Loading.Text = "加载采样信息";
            this.btn_HeightCalibration_Loading.UseVisualStyleBackColor = false;
            this.btn_HeightCalibration_Loading.Click += new System.EventHandler(this.btn_HeightCalibration_Loading_Click);
            // 
            // btn_HeightCalibration_Save
            // 
            this.btn_HeightCalibration_Save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_HeightCalibration_Save.Location = new System.Drawing.Point(1063, 254);
            this.btn_HeightCalibration_Save.Name = "btn_HeightCalibration_Save";
            this.btn_HeightCalibration_Save.Size = new System.Drawing.Size(95, 31);
            this.btn_HeightCalibration_Save.TabIndex = 4;
            this.btn_HeightCalibration_Save.Text = "生成采样信息";
            this.btn_HeightCalibration_Save.UseVisualStyleBackColor = false;
            this.btn_HeightCalibration_Save.Click += new System.EventHandler(this.btn_HeightCalibration_Save_Click);
            // 
            // btn_RunHeightCalibrate
            // 
            this.btn_RunHeightCalibrate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_RunHeightCalibrate.Location = new System.Drawing.Point(937, 291);
            this.btn_RunHeightCalibrate.Name = "btn_RunHeightCalibrate";
            this.btn_RunHeightCalibrate.Size = new System.Drawing.Size(95, 31);
            this.btn_RunHeightCalibrate.TabIndex = 4;
            this.btn_RunHeightCalibrate.Text = "开始采样";
            this.btn_RunHeightCalibrate.UseVisualStyleBackColor = false;
            this.btn_RunHeightCalibrate.Click += new System.EventHandler(this.btn_RunHeightCalibrate_Click);
            // 
            // btn_StopHeightCalibrate
            // 
            this.btn_StopHeightCalibrate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_StopHeightCalibrate.Location = new System.Drawing.Point(1063, 291);
            this.btn_StopHeightCalibrate.Name = "btn_StopHeightCalibrate";
            this.btn_StopHeightCalibrate.Size = new System.Drawing.Size(95, 31);
            this.btn_StopHeightCalibrate.TabIndex = 4;
            this.btn_StopHeightCalibrate.Text = "停止采样";
            this.btn_StopHeightCalibrate.UseVisualStyleBackColor = false;
            this.btn_StopHeightCalibrate.Click += new System.EventHandler(this.btn_StopHeightCalibrate_Click);
            // 
            // btn_Clear
            // 
            this.btn_Clear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_Clear.Location = new System.Drawing.Point(758, 291);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(95, 31);
            this.btn_Clear.TabIndex = 4;
            this.btn_Clear.Text = "清空";
            this.btn_Clear.UseVisualStyleBackColor = false;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(33, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "Reticle信息(um)";
            // 
            // txt_ReticleWidth
            // 
            this.txt_ReticleWidth.Location = new System.Drawing.Point(69, 27);
            this.txt_ReticleWidth.Name = "txt_ReticleWidth";
            this.txt_ReticleWidth.ReadOnly = true;
            this.txt_ReticleWidth.Size = new System.Drawing.Size(80, 21);
            this.txt_ReticleWidth.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(325, 32);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 12);
            this.label8.TabIndex = 5;
            this.label8.Text = "晶圆半径(um)";
            // 
            // txt_ArearCircle
            // 
            this.txt_ArearCircle.Location = new System.Drawing.Point(429, 23);
            this.txt_ArearCircle.Name = "txt_ArearCircle";
            this.txt_ArearCircle.Size = new System.Drawing.Size(81, 21);
            this.txt_ArearCircle.TabIndex = 6;
            this.txt_ArearCircle.Text = "97000";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(325, 59);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 12);
            this.label9.TabIndex = 5;
            this.label9.Text = "高度差门限(um)";
            // 
            // txt_HeightThreshold
            // 
            this.txt_HeightThreshold.Location = new System.Drawing.Point(429, 52);
            this.txt_HeightThreshold.Name = "txt_HeightThreshold";
            this.txt_HeightThreshold.Size = new System.Drawing.Size(81, 21);
            this.txt_HeightThreshold.TabIndex = 6;
            this.txt_HeightThreshold.Text = "30";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(180, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "晶圆中心坐标(um)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "高度";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 5;
            this.label7.Text = "宽度";
            // 
            // txt_CenterX2
            // 
            this.txt_CenterX2.Location = new System.Drawing.Point(204, 30);
            this.txt_CenterX2.Name = "txt_CenterX2";
            this.txt_CenterX2.ReadOnly = true;
            this.txt_CenterX2.Size = new System.Drawing.Size(80, 21);
            this.txt_CenterX2.TabIndex = 6;
            // 
            // txt_ReticleHeight
            // 
            this.txt_ReticleHeight.Location = new System.Drawing.Point(69, 50);
            this.txt_ReticleHeight.Name = "txt_ReticleHeight";
            this.txt_ReticleHeight.ReadOnly = true;
            this.txt_ReticleHeight.Size = new System.Drawing.Size(80, 21);
            this.txt_ReticleHeight.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(184, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "Y";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(184, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "X";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(605, 7);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 5;
            this.label10.Text = "基准信息录入";
            // 
            // txt_CenterY2
            // 
            this.txt_CenterY2.Location = new System.Drawing.Point(204, 54);
            this.txt_CenterY2.Name = "txt_CenterY2";
            this.txt_CenterY2.ReadOnly = true;
            this.txt_CenterY2.Size = new System.Drawing.Size(80, 21);
            this.txt_CenterY2.TabIndex = 6;
            // 
            // chb_AltimeterTypeCap
            // 
            this.chb_AltimeterTypeCap.AutoSize = true;
            this.chb_AltimeterTypeCap.Location = new System.Drawing.Point(516, 55);
            this.chb_AltimeterTypeCap.Name = "chb_AltimeterTypeCap";
            this.chb_AltimeterTypeCap.Size = new System.Drawing.Size(84, 16);
            this.chb_AltimeterTypeCap.TabIndex = 7;
            this.chb_AltimeterTypeCap.Text = "电容测高仪";
            this.chb_AltimeterTypeCap.UseVisualStyleBackColor = true;
            this.chb_AltimeterTypeCap.CheckedChanged += new System.EventHandler(this.chb_AltimeterTypeCap_CheckedChanged);
            // 
            // dgv_BasicSamplePoint
            // 
            this.dgv_BasicSamplePoint.AllowUserToAddRows = false;
            this.dgv_BasicSamplePoint.AllowUserToDeleteRows = false;
            this.dgv_BasicSamplePoint.AllowUserToResizeColumns = false;
            this.dgv_BasicSamplePoint.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            this.dgv_BasicSamplePoint.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgv_BasicSamplePoint.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Times New Roman", 10.5F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_BasicSamplePoint.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgv_BasicSamplePoint.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.Column3,
            this.Column4});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Times New Roman", 10.5F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_BasicSamplePoint.DefaultCellStyle = dataGridViewCellStyle9;
            this.dgv_BasicSamplePoint.Location = new System.Drawing.Point(607, 25);
            this.dgv_BasicSamplePoint.MultiSelect = false;
            this.dgv_BasicSamplePoint.Name = "dgv_BasicSamplePoint";
            this.dgv_BasicSamplePoint.RowTemplate.Height = 23;
            this.dgv_BasicSamplePoint.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_BasicSamplePoint.Size = new System.Drawing.Size(272, 222);
            this.dgv_BasicSamplePoint.TabIndex = 8;
            // 
            // panel_HeightCalibrate
            // 
            this.panel_HeightCalibrate.Controls.Add(this.rtbMsgBox);
            this.panel_HeightCalibrate.Controls.Add(this.btn_AutoSerch);
            this.panel_HeightCalibrate.Controls.Add(this.btn_InitPos);
            this.panel_HeightCalibrate.Controls.Add(this.dgv_BasicSamplePoint);
            this.panel_HeightCalibrate.Controls.Add(this.chb_AltimeterTypeCap);
            this.panel_HeightCalibrate.Controls.Add(this.txt_CenterY2);
            this.panel_HeightCalibrate.Controls.Add(this.label1);
            this.panel_HeightCalibrate.Controls.Add(this.label10);
            this.panel_HeightCalibrate.Controls.Add(this.label6);
            this.panel_HeightCalibrate.Controls.Add(this.label5);
            this.panel_HeightCalibrate.Controls.Add(this.txt_ReticleHeight);
            this.panel_HeightCalibrate.Controls.Add(this.txt_CenterX2);
            this.panel_HeightCalibrate.Controls.Add(this.label7);
            this.panel_HeightCalibrate.Controls.Add(this.label3);
            this.panel_HeightCalibrate.Controls.Add(this.label4);
            this.panel_HeightCalibrate.Controls.Add(this.txt_HeightThreshold);
            this.panel_HeightCalibrate.Controls.Add(this.label9);
            this.panel_HeightCalibrate.Controls.Add(this.txt_ArearCircle);
            this.panel_HeightCalibrate.Controls.Add(this.label8);
            this.panel_HeightCalibrate.Controls.Add(this.txt_ReticleWidth);
            this.panel_HeightCalibrate.Controls.Add(this.label2);
            this.panel_HeightCalibrate.Controls.Add(this.btn_Clear);
            this.panel_HeightCalibrate.Controls.Add(this.btn_StopHeightCalibrate);
            this.panel_HeightCalibrate.Controls.Add(this.btn_RunHeightCalibrate);
            this.panel_HeightCalibrate.Controls.Add(this.btn_HeightCalibration_Save);
            this.panel_HeightCalibrate.Controls.Add(this.btn_HeightCalibration_Loading);
            this.panel_HeightCalibrate.Controls.Add(this.btn_Add);
            this.panel_HeightCalibrate.Controls.Add(this.chart_H);
            this.panel_HeightCalibrate.Controls.Add(this.lv_H);
            this.panel_HeightCalibrate.Location = new System.Drawing.Point(13, 6);
            this.panel_HeightCalibrate.Name = "panel_HeightCalibrate";
            this.panel_HeightCalibrate.Size = new System.Drawing.Size(1198, 562);
            this.panel_HeightCalibrate.TabIndex = 8;
            // 
            // btn_InitPos
            // 
            this.btn_InitPos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_InitPos.Location = new System.Drawing.Point(630, 254);
            this.btn_InitPos.Name = "btn_InitPos";
            this.btn_InitPos.Size = new System.Drawing.Size(95, 31);
            this.btn_InitPos.TabIndex = 9;
            this.btn_InitPos.Text = "初始化位置";
            this.btn_InitPos.UseVisualStyleBackColor = false;
            this.btn_InitPos.Click += new System.EventHandler(this.btn_InitPos_Click);
            // 
            // btn_AutoSerch
            // 
            this.btn_AutoSerch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_AutoSerch.Location = new System.Drawing.Point(630, 291);
            this.btn_AutoSerch.Name = "btn_AutoSerch";
            this.btn_AutoSerch.Size = new System.Drawing.Size(95, 31);
            this.btn_AutoSerch.TabIndex = 10;
            this.btn_AutoSerch.Text = "自动搜索";
            this.btn_AutoSerch.UseVisualStyleBackColor = false;
            this.btn_AutoSerch.Click += new System.EventHandler(this.btn_AutoSerch_Click);
            // 
            // rtbMsgBox
            // 
            this.rtbMsgBox.Location = new System.Drawing.Point(613, 338);
            this.rtbMsgBox.Name = "rtbMsgBox";
            this.rtbMsgBox.ReadOnly = true;
            this.rtbMsgBox.Size = new System.Drawing.Size(563, 209);
            this.rtbMsgBox.TabIndex = 11;
            this.rtbMsgBox.Text = "";
            this.rtbMsgBox.TextChanged += new System.EventHandler(this.rtbMsgBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(891, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "高度扫描信息";
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column2.HeaderText = "X";
            this.Column2.Name = "Column2";
            this.Column2.Width = 80;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Y";
            this.Column3.Name = "Column3";
            this.Column3.Width = 80;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "H";
            this.Column4.Name = "Column4";
            this.Column4.Width = 80;
            // 
            // FormHeightScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1223, 576);
            this.Controls.Add(this.panel_HeightCalibrate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormHeightScan";
            this.Text = "FormHeightScan";
            this.Load += new System.EventHandler(this.FormHeightScan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart_H)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_BasicSamplePoint)).EndInit();
            this.panel_HeightCalibrate.ResumeLayout(false);
            this.panel_HeightCalibrate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lv_H;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_H;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Button btn_HeightCalibration_Loading;
        private System.Windows.Forms.Button btn_HeightCalibration_Save;
        private System.Windows.Forms.Button btn_RunHeightCalibrate;
        private System.Windows.Forms.Button btn_StopHeightCalibrate;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_ReticleWidth;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_ArearCircle;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt_HeightThreshold;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_CenterX2;
        private System.Windows.Forms.TextBox txt_ReticleHeight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt_CenterY2;
        private System.Windows.Forms.CheckBox chb_AltimeterTypeCap;
        private System.Windows.Forms.DataGridView dgv_BasicSamplePoint;
        private System.Windows.Forms.Panel panel_HeightCalibrate;
        private System.Windows.Forms.Button btn_InitPos;
        private System.Windows.Forms.Button btn_AutoSerch;
        private System.Windows.Forms.RichTextBox rtbMsgBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
    }
}