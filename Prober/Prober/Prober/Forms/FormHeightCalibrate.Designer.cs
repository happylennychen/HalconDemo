namespace Prober.Forms
{
    partial class FormHeightCalibrate
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
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.num_RowSpace = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.num_ColSpace = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.num_ArearCircle = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_CenterX2 = new System.Windows.Forms.TextBox();
            this.txt_CenterY2 = new System.Windows.Forms.TextBox();
            this.chart_H = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lv_H = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_HeightCalibration_Save = new System.Windows.Forms.Button();
            this.btn_exposeCSV = new System.Windows.Forms.Button();
            this.btn_RunHeightCalibrate = new System.Windows.Forms.Button();
            this.btn_StopHeightCalibrate = new System.Windows.Forms.Button();
            this.panel_HeightCalibrate = new System.Windows.Forms.Panel();
            this.btn_HeightCalibration_Loading = new System.Windows.Forms.Button();
            this.chb_AltimeterTypeCap = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.num_RowSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_ColSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_ArearCircle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_H)).BeginInit();
            this.panel_HeightCalibrate.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(35, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "采样间距(um)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(188, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "行:";
            // 
            // num_RowSpace
            // 
            this.num_RowSpace.Location = new System.Drawing.Point(249, 18);
            this.num_RowSpace.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.num_RowSpace.Name = "num_RowSpace";
            this.num_RowSpace.Size = new System.Drawing.Size(100, 21);
            this.num_RowSpace.TabIndex = 1;
            this.num_RowSpace.Value = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(405, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "列:";
            // 
            // num_ColSpace
            // 
            this.num_ColSpace.Location = new System.Drawing.Point(466, 18);
            this.num_ColSpace.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.num_ColSpace.Name = "num_ColSpace";
            this.num_ColSpace.Size = new System.Drawing.Size(100, 21);
            this.num_ColSpace.TabIndex = 1;
            this.num_ColSpace.Value = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(614, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "有效区域半径:";
            // 
            // num_ArearCircle
            // 
            this.num_ArearCircle.Location = new System.Drawing.Point(703, 18);
            this.num_ArearCircle.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.num_ArearCircle.Name = "num_ArearCircle";
            this.num_ArearCircle.Size = new System.Drawing.Size(100, 21);
            this.num_ArearCircle.TabIndex = 1;
            this.num_ArearCircle.Value = new decimal(new int[] {
            95000,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(35, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "晶圆中心坐标(um)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(192, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "X:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(409, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "Y:";
            // 
            // txt_CenterX2
            // 
            this.txt_CenterX2.Location = new System.Drawing.Point(253, 44);
            this.txt_CenterX2.Name = "txt_CenterX2";
            this.txt_CenterX2.ReadOnly = true;
            this.txt_CenterX2.Size = new System.Drawing.Size(100, 21);
            this.txt_CenterX2.TabIndex = 2;
            // 
            // txt_CenterY2
            // 
            this.txt_CenterY2.Location = new System.Drawing.Point(470, 44);
            this.txt_CenterY2.Name = "txt_CenterY2";
            this.txt_CenterY2.ReadOnly = true;
            this.txt_CenterY2.Size = new System.Drawing.Size(100, 21);
            this.txt_CenterY2.TabIndex = 2;
            // 
            // chart_H
            // 
            chartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisX.IsStartedFromZero = false;
            chartArea1.AxisX.LineWidth = 2;
            chartArea1.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisY.LineWidth = 2;
            chartArea1.Name = "ChartArea1";
            this.chart_H.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.chart_H.Legends.Add(legend1);
            this.chart_H.Location = new System.Drawing.Point(38, 71);
            this.chart_H.Name = "chart_H";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Legend = "Legend1";
            series1.MarkerSize = 8;
            series1.Name = "Series1";
            this.chart_H.Series.Add(series1);
            this.chart_H.Size = new System.Drawing.Size(622, 568);
            this.chart_H.TabIndex = 3;
            this.chart_H.Text = "chart1";
            // 
            // lv_H
            // 
            this.lv_H.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lv_H.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lv_H.GridLines = true;
            this.lv_H.HideSelection = false;
            this.lv_H.Location = new System.Drawing.Point(678, 74);
            this.lv_H.Name = "lv_H";
            this.lv_H.Size = new System.Drawing.Size(302, 467);
            this.lv_H.TabIndex = 4;
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
            this.columnHeader2.Text = "X2";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Y2";
            this.columnHeader3.Width = 80;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "H2";
            this.columnHeader4.Width = 80;
            // 
            // btn_HeightCalibration_Save
            // 
            this.btn_HeightCalibration_Save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_HeightCalibration_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_HeightCalibration_Save.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_HeightCalibration_Save.Location = new System.Drawing.Point(866, 554);
            this.btn_HeightCalibration_Save.Name = "btn_HeightCalibration_Save";
            this.btn_HeightCalibration_Save.Size = new System.Drawing.Size(101, 37);
            this.btn_HeightCalibration_Save.TabIndex = 5;
            this.btn_HeightCalibration_Save.Text = "生成采样信息";
            this.btn_HeightCalibration_Save.UseVisualStyleBackColor = false;
            this.btn_HeightCalibration_Save.Click += new System.EventHandler(this.btn_HeightCalibration_Save_Click);
            // 
            // btn_exposeCSV
            // 
            this.btn_exposeCSV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_exposeCSV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_exposeCSV.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_exposeCSV.Location = new System.Drawing.Point(879, 28);
            this.btn_exposeCSV.Name = "btn_exposeCSV";
            this.btn_exposeCSV.Size = new System.Drawing.Size(101, 37);
            this.btn_exposeCSV.TabIndex = 5;
            this.btn_exposeCSV.Text = "导出到CSV";
            this.btn_exposeCSV.UseVisualStyleBackColor = false;
            this.btn_exposeCSV.Click += new System.EventHandler(this.btn_exposeCSV_Click);
            // 
            // btn_RunHeightCalibrate
            // 
            this.btn_RunHeightCalibrate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_RunHeightCalibrate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_RunHeightCalibrate.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_RunHeightCalibrate.Location = new System.Drawing.Point(702, 605);
            this.btn_RunHeightCalibrate.Name = "btn_RunHeightCalibrate";
            this.btn_RunHeightCalibrate.Size = new System.Drawing.Size(101, 37);
            this.btn_RunHeightCalibrate.TabIndex = 5;
            this.btn_RunHeightCalibrate.Text = "开始采样";
            this.btn_RunHeightCalibrate.UseVisualStyleBackColor = false;
            this.btn_RunHeightCalibrate.Click += new System.EventHandler(this.btn_RunHeightCalibrate_Click);
            // 
            // btn_StopHeightCalibrate
            // 
            this.btn_StopHeightCalibrate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_StopHeightCalibrate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_StopHeightCalibrate.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_StopHeightCalibrate.Location = new System.Drawing.Point(866, 605);
            this.btn_StopHeightCalibrate.Name = "btn_StopHeightCalibrate";
            this.btn_StopHeightCalibrate.Size = new System.Drawing.Size(101, 37);
            this.btn_StopHeightCalibrate.TabIndex = 5;
            this.btn_StopHeightCalibrate.Text = "停止采样";
            this.btn_StopHeightCalibrate.UseVisualStyleBackColor = false;
            this.btn_StopHeightCalibrate.Click += new System.EventHandler(this.btn_StopHeightCalibrate_Click);
            // 
            // panel_HeightCalibrate
            // 
            this.panel_HeightCalibrate.Controls.Add(this.btn_HeightCalibration_Loading);
            this.panel_HeightCalibrate.Controls.Add(this.chb_AltimeterTypeCap);
            this.panel_HeightCalibrate.Controls.Add(this.btn_exposeCSV);
            this.panel_HeightCalibrate.Controls.Add(this.btn_StopHeightCalibrate);
            this.panel_HeightCalibrate.Controls.Add(this.btn_RunHeightCalibrate);
            this.panel_HeightCalibrate.Controls.Add(this.btn_HeightCalibration_Save);
            this.panel_HeightCalibrate.Controls.Add(this.lv_H);
            this.panel_HeightCalibrate.Controls.Add(this.chart_H);
            this.panel_HeightCalibrate.Controls.Add(this.txt_CenterY2);
            this.panel_HeightCalibrate.Controls.Add(this.txt_CenterX2);
            this.panel_HeightCalibrate.Controls.Add(this.num_ArearCircle);
            this.panel_HeightCalibrate.Controls.Add(this.num_ColSpace);
            this.panel_HeightCalibrate.Controls.Add(this.num_RowSpace);
            this.panel_HeightCalibrate.Controls.Add(this.label4);
            this.panel_HeightCalibrate.Controls.Add(this.label7);
            this.panel_HeightCalibrate.Controls.Add(this.label3);
            this.panel_HeightCalibrate.Controls.Add(this.label6);
            this.panel_HeightCalibrate.Controls.Add(this.label2);
            this.panel_HeightCalibrate.Controls.Add(this.label5);
            this.panel_HeightCalibrate.Controls.Add(this.label1);
            this.panel_HeightCalibrate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_HeightCalibrate.Location = new System.Drawing.Point(0, 0);
            this.panel_HeightCalibrate.Name = "panel_HeightCalibrate";
            this.panel_HeightCalibrate.Size = new System.Drawing.Size(1012, 664);
            this.panel_HeightCalibrate.TabIndex = 6;
            // 
            // btn_HeightCalibration_Loading
            // 
            this.btn_HeightCalibration_Loading.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_HeightCalibration_Loading.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_HeightCalibration_Loading.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_HeightCalibration_Loading.Location = new System.Drawing.Point(702, 554);
            this.btn_HeightCalibration_Loading.Name = "btn_HeightCalibration_Loading";
            this.btn_HeightCalibration_Loading.Size = new System.Drawing.Size(101, 37);
            this.btn_HeightCalibration_Loading.TabIndex = 7;
            this.btn_HeightCalibration_Loading.Text = "加载采样信息";
            this.btn_HeightCalibration_Loading.UseVisualStyleBackColor = false;
            this.btn_HeightCalibration_Loading.Click += new System.EventHandler(this.btn_HeightCalibration_Loading_Click);
            // 
            // chb_AltimeterTypeCap
            // 
            this.chb_AltimeterTypeCap.AutoSize = true;
            this.chb_AltimeterTypeCap.Enabled = false;
            this.chb_AltimeterTypeCap.Location = new System.Drawing.Point(616, 47);
            this.chb_AltimeterTypeCap.Name = "chb_AltimeterTypeCap";
            this.chb_AltimeterTypeCap.Size = new System.Drawing.Size(84, 16);
            this.chb_AltimeterTypeCap.TabIndex = 6;
            this.chb_AltimeterTypeCap.Text = "电容测高仪";
            this.chb_AltimeterTypeCap.UseVisualStyleBackColor = true;
            this.chb_AltimeterTypeCap.CheckedChanged += new System.EventHandler(this.chb_AltimeterType_CheckedChanged);
            // 
            // FormHeightCalibrate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 664);
            this.Controls.Add(this.panel_HeightCalibrate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormHeightCalibrate";
            this.Text = "FormHeightCalibrate";
            ((System.ComponentModel.ISupportInitialize)(this.num_RowSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_ColSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_ArearCircle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_H)).EndInit();
            this.panel_HeightCalibrate.ResumeLayout(false);
            this.panel_HeightCalibrate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown num_RowSpace;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown num_ColSpace;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown num_ArearCircle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_CenterX2;
        private System.Windows.Forms.TextBox txt_CenterY2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_H;
        private System.Windows.Forms.ListView lv_H;
        private System.Windows.Forms.Button btn_HeightCalibration_Save;
        private System.Windows.Forms.Button btn_exposeCSV;
        private System.Windows.Forms.Button btn_RunHeightCalibrate;
        private System.Windows.Forms.Button btn_StopHeightCalibrate;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Panel panel_HeightCalibrate;
        private System.Windows.Forms.CheckBox chb_AltimeterTypeCap;
        private System.Windows.Forms.Button btn_HeightCalibration_Loading;
    }
}