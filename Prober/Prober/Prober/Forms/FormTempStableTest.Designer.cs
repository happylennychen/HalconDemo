namespace Prober.Forms
{
    partial class FormTempStableTest
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series13 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series14 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series15 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series16 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series17 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series18 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.num_Record_Interval = new System.Windows.Forms.NumericUpDown();
            this.num_Record_Time = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Record_View = new System.Windows.Forms.Button();
            this.btn_Record_Stop = new System.Windows.Forms.Button();
            this.btn_Record_Start = new System.Windows.Forms.Button();
            this.lbl_Chuck_Temp = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_AxisZ_Temp = new System.Windows.Forms.Label();
            this.chbox_IsMonitorH = new System.Windows.Forms.CheckBox();
            this.chb_AmType_Cap = new System.Windows.Forms.CheckBox();
            this.chb_TecType_Grat = new System.Windows.Forms.CheckBox();
            this.lbl_Info = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_AxisX_Temp = new System.Windows.Forms.Label();
            this.lbl_AxisY_Temp = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lbl_Enviroment_Temp = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lbl_Am_Ld = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lbl_Am_CapL = new System.Windows.Forms.Label();
            this.lbl_Am_CapR = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbl_Power_Ch1 = new System.Windows.Forms.Label();
            this.lbl_Power_Ch2 = new System.Windows.Forms.Label();
            this.chart_Power = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_Temp = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_Height = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txt_TestStartTime = new System.Windows.Forms.TextBox();
            this.txt_TestEllapseTime = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.num_Record_Interval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Record_Time)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Power)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Temp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Height)).BeginInit();
            this.SuspendLayout();
            // 
            // num_Record_Interval
            // 
            this.num_Record_Interval.Location = new System.Drawing.Point(147, 86);
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
            this.num_Record_Interval.TabIndex = 4;
            this.num_Record_Interval.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // num_Record_Time
            // 
            this.num_Record_Time.DecimalPlaces = 1;
            this.num_Record_Time.Location = new System.Drawing.Point(147, 50);
            this.num_Record_Time.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.num_Record_Time.Name = "num_Record_Time";
            this.num_Record_Time.Size = new System.Drawing.Size(120, 21);
            this.num_Record_Time.TabIndex = 5;
            this.num_Record_Time.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "采样间隔(ms)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "监控时长(min)";
            // 
            // btn_Record_View
            // 
            this.btn_Record_View.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_Record_View.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Record_View.Location = new System.Drawing.Point(235, 380);
            this.btn_Record_View.Name = "btn_Record_View";
            this.btn_Record_View.Size = new System.Drawing.Size(75, 32);
            this.btn_Record_View.TabIndex = 8;
            this.btn_Record_View.Text = "查看数据";
            this.btn_Record_View.UseVisualStyleBackColor = false;
            this.btn_Record_View.Click += new System.EventHandler(this.btn_Record_View_Click);
            // 
            // btn_Record_Stop
            // 
            this.btn_Record_Stop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_Record_Stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Record_Stop.Location = new System.Drawing.Point(119, 380);
            this.btn_Record_Stop.Name = "btn_Record_Stop";
            this.btn_Record_Stop.Size = new System.Drawing.Size(75, 32);
            this.btn_Record_Stop.TabIndex = 9;
            this.btn_Record_Stop.Text = "停止";
            this.btn_Record_Stop.UseVisualStyleBackColor = false;
            this.btn_Record_Stop.Click += new System.EventHandler(this.btn_Record_Stop_Click);
            // 
            // btn_Record_Start
            // 
            this.btn_Record_Start.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_Record_Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Record_Start.Location = new System.Drawing.Point(10, 380);
            this.btn_Record_Start.Name = "btn_Record_Start";
            this.btn_Record_Start.Size = new System.Drawing.Size(75, 32);
            this.btn_Record_Start.TabIndex = 10;
            this.btn_Record_Start.Text = "开始";
            this.btn_Record_Start.UseVisualStyleBackColor = false;
            this.btn_Record_Start.Click += new System.EventHandler(this.btn_Record_Start_Click);
            // 
            // lbl_Chuck_Temp
            // 
            this.lbl_Chuck_Temp.Location = new System.Drawing.Point(107, 163);
            this.lbl_Chuck_Temp.Name = "lbl_Chuck_Temp";
            this.lbl_Chuck_Temp.Size = new System.Drawing.Size(46, 12);
            this.lbl_Chuck_Temp.TabIndex = 7;
            this.lbl_Chuck_Temp.Text = "N/A";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 163);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Chuck温度(℃)";
            // 
            // lbl_AxisZ_Temp
            // 
            this.lbl_AxisZ_Temp.Location = new System.Drawing.Point(107, 232);
            this.lbl_AxisZ_Temp.Name = "lbl_AxisZ_Temp";
            this.lbl_AxisZ_Temp.Size = new System.Drawing.Size(46, 12);
            this.lbl_AxisZ_Temp.TabIndex = 7;
            this.lbl_AxisZ_Temp.Text = "N/A";
            // 
            // chbox_IsMonitorH
            // 
            this.chbox_IsMonitorH.AutoSize = true;
            this.chbox_IsMonitorH.Location = new System.Drawing.Point(201, 483);
            this.chbox_IsMonitorH.Name = "chbox_IsMonitorH";
            this.chbox_IsMonitorH.Size = new System.Drawing.Size(108, 16);
            this.chbox_IsMonitorH.TabIndex = 11;
            this.chbox_IsMonitorH.Text = "监控测高仪高度";
            this.chbox_IsMonitorH.UseVisualStyleBackColor = true;
            this.chbox_IsMonitorH.Visible = false;
            // 
            // chb_AmType_Cap
            // 
            this.chb_AmType_Cap.AutoSize = true;
            this.chb_AmType_Cap.Location = new System.Drawing.Point(152, 483);
            this.chb_AmType_Cap.Name = "chb_AmType_Cap";
            this.chb_AmType_Cap.Size = new System.Drawing.Size(84, 16);
            this.chb_AmType_Cap.TabIndex = 12;
            this.chb_AmType_Cap.Text = "电容测高仪";
            this.chb_AmType_Cap.UseVisualStyleBackColor = true;
            this.chb_AmType_Cap.Visible = false;
            this.chb_AmType_Cap.CheckedChanged += new System.EventHandler(this.chb_AmType_Cap_CheckedChanged);
            // 
            // chb_TecType_Grat
            // 
            this.chb_TecType_Grat.AutoSize = true;
            this.chb_TecType_Grat.Location = new System.Drawing.Point(79, 483);
            this.chb_TecType_Grat.Name = "chb_TecType_Grat";
            this.chb_TecType_Grat.Size = new System.Drawing.Size(84, 16);
            this.chb_TecType_Grat.TabIndex = 13;
            this.chb_TecType_Grat.Text = "监控光栅尺";
            this.chb_TecType_Grat.UseVisualStyleBackColor = true;
            this.chb_TecType_Grat.Visible = false;
            this.chb_TecType_Grat.CheckedChanged += new System.EventHandler(this.chb_TecType_Grat_CheckedChanged);
            // 
            // lbl_Info
            // 
            this.lbl_Info.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbl_Info.Location = new System.Drawing.Point(19, 121);
            this.lbl_Info.Name = "lbl_Info";
            this.lbl_Info.Size = new System.Drawing.Size(272, 23);
            this.lbl_Info.TabIndex = 14;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.button1.Enabled = false;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(35, 485);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(17, 10);
            this.button1.TabIndex = 10;
            this.button1.Text = "开始";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Visible = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.button2.Enabled = false;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(12, 485);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(17, 10);
            this.button2.TabIndex = 10;
            this.button2.Text = "开始";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Visible = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.button3.Enabled = false;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(59, 485);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(17, 10);
            this.button3.TabIndex = 10;
            this.button3.Text = "开始";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 186);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "轴X温度(℃)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 210);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "轴Y温度(℃)";
            // 
            // lbl_AxisX_Temp
            // 
            this.lbl_AxisX_Temp.Location = new System.Drawing.Point(107, 186);
            this.lbl_AxisX_Temp.Name = "lbl_AxisX_Temp";
            this.lbl_AxisX_Temp.Size = new System.Drawing.Size(46, 12);
            this.lbl_AxisX_Temp.TabIndex = 7;
            this.lbl_AxisX_Temp.Text = "N/A";
            // 
            // lbl_AxisY_Temp
            // 
            this.lbl_AxisY_Temp.Location = new System.Drawing.Point(107, 210);
            this.lbl_AxisY_Temp.Name = "lbl_AxisY_Temp";
            this.lbl_AxisY_Temp.Size = new System.Drawing.Size(46, 12);
            this.lbl_AxisY_Temp.TabIndex = 7;
            this.lbl_AxisY_Temp.Text = "N/A";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 232);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 12);
            this.label8.TabIndex = 6;
            this.label8.Text = "轴Z温度(℃)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 255);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 6;
            this.label9.Text = "腔体温度(℃)";
            // 
            // lbl_Enviroment_Temp
            // 
            this.lbl_Enviroment_Temp.Location = new System.Drawing.Point(107, 253);
            this.lbl_Enviroment_Temp.Name = "lbl_Enviroment_Temp";
            this.lbl_Enviroment_Temp.Size = new System.Drawing.Size(46, 12);
            this.lbl_Enviroment_Temp.TabIndex = 7;
            this.lbl_Enviroment_Temp.Text = "N/A";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(159, 163);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 12);
            this.label11.TabIndex = 6;
            this.label11.Text = "激光测高仪um)";
            // 
            // lbl_Am_Ld
            // 
            this.lbl_Am_Ld.Location = new System.Drawing.Point(266, 161);
            this.lbl_Am_Ld.Name = "lbl_Am_Ld";
            this.lbl_Am_Ld.Size = new System.Drawing.Size(46, 12);
            this.lbl_Am_Ld.TabIndex = 7;
            this.lbl_Am_Ld.Text = "N/A";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(159, 185);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(101, 12);
            this.label13.TabIndex = 6;
            this.label13.Text = "左电容测高仪(um)";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(159, 205);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(101, 12);
            this.label14.TabIndex = 6;
            this.label14.Text = "右电容测高仪(um)";
            // 
            // lbl_Am_CapL
            // 
            this.lbl_Am_CapL.Location = new System.Drawing.Point(266, 183);
            this.lbl_Am_CapL.Name = "lbl_Am_CapL";
            this.lbl_Am_CapL.Size = new System.Drawing.Size(46, 12);
            this.lbl_Am_CapL.TabIndex = 7;
            this.lbl_Am_CapL.Text = "N/A";
            // 
            // lbl_Am_CapR
            // 
            this.lbl_Am_CapR.Location = new System.Drawing.Point(266, 203);
            this.lbl_Am_CapR.Name = "lbl_Am_CapR";
            this.lbl_Am_CapR.Size = new System.Drawing.Size(46, 12);
            this.lbl_Am_CapR.TabIndex = 7;
            this.lbl_Am_CapR.Text = "N/A";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(160, 232);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "通道1光功率(dBm)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(160, 255);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "通道2光功率(dBm)";
            // 
            // lbl_Power_Ch1
            // 
            this.lbl_Power_Ch1.Location = new System.Drawing.Point(267, 232);
            this.lbl_Power_Ch1.Name = "lbl_Power_Ch1";
            this.lbl_Power_Ch1.Size = new System.Drawing.Size(46, 12);
            this.lbl_Power_Ch1.TabIndex = 7;
            this.lbl_Power_Ch1.Text = "N/A";
            // 
            // lbl_Power_Ch2
            // 
            this.lbl_Power_Ch2.Location = new System.Drawing.Point(267, 253);
            this.lbl_Power_Ch2.Name = "lbl_Power_Ch2";
            this.lbl_Power_Ch2.Size = new System.Drawing.Size(46, 12);
            this.lbl_Power_Ch2.TabIndex = 7;
            this.lbl_Power_Ch2.Text = "N/A";
            // 
            // chart_Power
            // 
            chartArea4.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea4.AxisY.IsStartedFromZero = false;
            chartArea4.AxisY2.IsStartedFromZero = false;
            chartArea4.Name = "ChartArea1";
            this.chart_Power.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chart_Power.Legends.Add(legend4);
            this.chart_Power.Location = new System.Drawing.Point(371, 8);
            this.chart_Power.Name = "chart_Power";
            series10.ChartArea = "ChartArea1";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series10.Legend = "Legend1";
            series10.Name = "通道1(dBm)";
            series10.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series11.ChartArea = "ChartArea1";
            series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series11.Legend = "Legend1";
            series11.Name = "通道2(dBm)";
            series11.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series11.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            this.chart_Power.Series.Add(series10);
            this.chart_Power.Series.Add(series11);
            this.chart_Power.Size = new System.Drawing.Size(688, 230);
            this.chart_Power.TabIndex = 15;
            this.chart_Power.Text = "chart1";
            // 
            // chart_Temp
            // 
            chartArea5.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea5.AxisY.IsStartedFromZero = false;
            chartArea5.AxisY2.IsStartedFromZero = false;
            chartArea5.Name = "ChartArea1";
            this.chart_Temp.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.chart_Temp.Legends.Add(legend5);
            this.chart_Temp.Location = new System.Drawing.Point(371, 244);
            this.chart_Temp.Name = "chart_Temp";
            series12.ChartArea = "ChartArea1";
            series12.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series12.Legend = "Legend1";
            series12.Name = "Chuck(°C)";
            series12.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series12.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series13.ChartArea = "ChartArea1";
            series13.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series13.Legend = "Legend1";
            series13.Name = "轴X(°C)";
            series13.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series14.ChartArea = "ChartArea1";
            series14.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series14.Legend = "Legend1";
            series14.Name = "轴Y(°C)";
            series14.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series15.ChartArea = "ChartArea1";
            series15.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series15.Legend = "Legend1";
            series15.Name = "轴Z(°C)";
            series15.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series16.ChartArea = "ChartArea1";
            series16.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series16.Legend = "Legend1";
            series16.Name = "腔体(°C)";
            series16.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            this.chart_Temp.Series.Add(series12);
            this.chart_Temp.Series.Add(series13);
            this.chart_Temp.Series.Add(series14);
            this.chart_Temp.Series.Add(series15);
            this.chart_Temp.Series.Add(series16);
            this.chart_Temp.Size = new System.Drawing.Size(688, 216);
            this.chart_Temp.TabIndex = 15;
            this.chart_Temp.Text = "chart1";
            // 
            // chart_Height
            // 
            chartArea6.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea6.AxisY.IsStartedFromZero = false;
            chartArea6.AxisY2.IsStartedFromZero = false;
            chartArea6.Name = "ChartArea1";
            this.chart_Height.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.chart_Height.Legends.Add(legend6);
            this.chart_Height.Location = new System.Drawing.Point(371, 466);
            this.chart_Height.Name = "chart_Height";
            series17.ChartArea = "ChartArea1";
            series17.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series17.Legend = "Legend1";
            series17.Name = "左侧(um)";
            series17.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series18.ChartArea = "ChartArea1";
            series18.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series18.Legend = "Legend1";
            series18.Name = "右侧(um)";
            series18.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series18.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            this.chart_Height.Series.Add(series17);
            this.chart_Height.Series.Add(series18);
            this.chart_Height.Size = new System.Drawing.Size(688, 223);
            this.chart_Height.TabIndex = 15;
            this.chart_Height.Text = "chart1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(295, 563);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 12);
            this.label10.TabIndex = 6;
            this.label10.Text = "电容测高仪";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(321, 324);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 12);
            this.label12.TabIndex = 6;
            this.label12.Text = "温度";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(321, 83);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(44, 12);
            this.label15.TabIndex = 6;
            this.label15.Text = "光功率";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(19, 299);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 12);
            this.label16.TabIndex = 6;
            this.label16.Text = "测试开始时间";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(19, 327);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(83, 12);
            this.label17.TabIndex = 6;
            this.label17.Text = "已测试时间(S)";
            // 
            // txt_TestStartTime
            // 
            this.txt_TestStartTime.Location = new System.Drawing.Point(119, 296);
            this.txt_TestStartTime.Name = "txt_TestStartTime";
            this.txt_TestStartTime.ReadOnly = true;
            this.txt_TestStartTime.Size = new System.Drawing.Size(141, 21);
            this.txt_TestStartTime.TabIndex = 16;
            // 
            // txt_TestEllapseTime
            // 
            this.txt_TestEllapseTime.Location = new System.Drawing.Point(119, 323);
            this.txt_TestEllapseTime.Name = "txt_TestEllapseTime";
            this.txt_TestEllapseTime.ReadOnly = true;
            this.txt_TestEllapseTime.Size = new System.Drawing.Size(142, 21);
            this.txt_TestEllapseTime.TabIndex = 16;
            // 
            // FormTempStableTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 695);
            this.Controls.Add(this.txt_TestEllapseTime);
            this.Controls.Add(this.txt_TestStartTime);
            this.Controls.Add(this.chart_Height);
            this.Controls.Add(this.chart_Temp);
            this.Controls.Add(this.chart_Power);
            this.Controls.Add(this.lbl_Info);
            this.Controls.Add(this.chb_TecType_Grat);
            this.Controls.Add(this.chb_AmType_Cap);
            this.Controls.Add(this.chbox_IsMonitorH);
            this.Controls.Add(this.btn_Record_View);
            this.Controls.Add(this.btn_Record_Stop);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_Record_Start);
            this.Controls.Add(this.lbl_Am_CapR);
            this.Controls.Add(this.lbl_Am_Ld);
            this.Controls.Add(this.lbl_Am_CapL);
            this.Controls.Add(this.lbl_Power_Ch2);
            this.Controls.Add(this.lbl_Enviroment_Temp);
            this.Controls.Add(this.lbl_Power_Ch1);
            this.Controls.Add(this.lbl_AxisZ_Temp);
            this.Controls.Add(this.lbl_AxisY_Temp);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.lbl_AxisX_Temp);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lbl_Chuck_Temp);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.num_Record_Interval);
            this.Controls.Add(this.num_Record_Time);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormTempStableTest";
            ((System.ComponentModel.ISupportInitialize)(this.num_Record_Interval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Record_Time)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Power)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Temp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Height)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown num_Record_Interval;
        private System.Windows.Forms.NumericUpDown num_Record_Time;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Record_View;
        private System.Windows.Forms.Button btn_Record_Stop;
        private System.Windows.Forms.Button btn_Record_Start;
        private System.Windows.Forms.Label lbl_Chuck_Temp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_AxisZ_Temp;
        private System.Windows.Forms.CheckBox chbox_IsMonitorH;
        private System.Windows.Forms.CheckBox chb_AmType_Cap;
        private System.Windows.Forms.CheckBox chb_TecType_Grat;
        private System.Windows.Forms.Label lbl_Info;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl_AxisX_Temp;
        private System.Windows.Forms.Label lbl_AxisY_Temp;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbl_Enviroment_Temp;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbl_Am_Ld;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lbl_Am_CapL;
        private System.Windows.Forms.Label lbl_Am_CapR;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbl_Power_Ch1;
        private System.Windows.Forms.Label lbl_Power_Ch2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Power;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Temp;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Height;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txt_TestStartTime;
        private System.Windows.Forms.TextBox txt_TestEllapseTime;
    }
}