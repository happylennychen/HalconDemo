namespace Prober.Forms {
    partial class FormTecControl
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart_Temp = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemSaveData = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_SetTemp = new System.Windows.Forms.Button();
            this.num_TempSet = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_CurTempRead = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_StopMonitor = new System.Windows.Forms.Button();
            this.btn_StartMonitor = new System.Windows.Forms.Button();
            this.btn_TurnOffTEC = new System.Windows.Forms.Button();
            this.btn_TurnOnTEC = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Temp)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_TempSet)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart_Temp
            // 
            chartArea1.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea1.AxisY.IsStartedFromZero = false;
            chartArea1.Name = "ChartArea1";
            this.chart_Temp.ChartAreas.Add(chartArea1);
            this.chart_Temp.ContextMenuStrip = this.contextMenuStrip;
            this.chart_Temp.Location = new System.Drawing.Point(5, 16);
            this.chart_Temp.Name = "chart_Temp";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "Series1";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            this.chart_Temp.Series.Add(series1);
            this.chart_Temp.Size = new System.Drawing.Size(629, 383);
            this.chart_Temp.TabIndex = 0;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSaveData});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(101, 26);
            // 
            // toolStripMenuItemSaveData
            // 
            this.toolStripMenuItemSaveData.Name = "toolStripMenuItemSaveData";
            this.toolStripMenuItemSaveData.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItemSaveData.Text = "保存";
            this.toolStripMenuItemSaveData.Click += new System.EventHandler(this.toolStripMenuItemSaveData_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "温度（℃）";
            // 
            // btn_SetTemp
            // 
            this.btn_SetTemp.BackColor = System.Drawing.SystemColors.Control;
            this.btn_SetTemp.Location = new System.Drawing.Point(113, 57);
            this.btn_SetTemp.Name = "btn_SetTemp";
            this.btn_SetTemp.Size = new System.Drawing.Size(75, 33);
            this.btn_SetTemp.TabIndex = 3;
            this.btn_SetTemp.Text = "设置";
            this.btn_SetTemp.UseVisualStyleBackColor = false;
            this.btn_SetTemp.Click += new System.EventHandler(this.btn_SetTemp_Click);
            // 
            // num_TempSet
            // 
            this.num_TempSet.DecimalPlaces = 1;
            this.num_TempSet.Location = new System.Drawing.Point(17, 65);
            this.num_TempSet.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.num_TempSet.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.num_TempSet.Name = "num_TempSet";
            this.num_TempSet.Size = new System.Drawing.Size(65, 21);
            this.num_TempSet.TabIndex = 4;
            this.num_TempSet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.num_TempSet.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.txt_CurTempRead);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btn_StopMonitor);
            this.panel1.Controls.Add(this.btn_StartMonitor);
            this.panel1.Controls.Add(this.btn_TurnOffTEC);
            this.panel1.Controls.Add(this.btn_TurnOnTEC);
            this.panel1.Controls.Add(this.btn_SetTemp);
            this.panel1.Controls.Add(this.num_TempSet);
            this.panel1.Location = new System.Drawing.Point(640, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(199, 383);
            this.panel1.TabIndex = 5;
            // 
            // txt_CurTempRead
            // 
            this.txt_CurTempRead.Location = new System.Drawing.Point(121, 346);
            this.txt_CurTempRead.Name = "txt_CurTempRead";
            this.txt_CurTempRead.Size = new System.Drawing.Size(67, 21);
            this.txt_CurTempRead.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(22, 346);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "当前温度(℃)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 244);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "温度监控";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "控温开关";
            // 
            // btn_StopMonitor
            // 
            this.btn_StopMonitor.BackColor = System.Drawing.SystemColors.Control;
            this.btn_StopMonitor.Location = new System.Drawing.Point(113, 276);
            this.btn_StopMonitor.Name = "btn_StopMonitor";
            this.btn_StopMonitor.Size = new System.Drawing.Size(75, 33);
            this.btn_StopMonitor.TabIndex = 3;
            this.btn_StopMonitor.Text = "结束";
            this.btn_StopMonitor.UseVisualStyleBackColor = false;
            this.btn_StopMonitor.Click += new System.EventHandler(this.btn_StopMonitor_Click);
            // 
            // btn_StartMonitor
            // 
            this.btn_StartMonitor.BackColor = System.Drawing.SystemColors.Control;
            this.btn_StartMonitor.Location = new System.Drawing.Point(15, 276);
            this.btn_StartMonitor.Name = "btn_StartMonitor";
            this.btn_StartMonitor.Size = new System.Drawing.Size(75, 33);
            this.btn_StartMonitor.TabIndex = 3;
            this.btn_StartMonitor.Text = "开始";
            this.btn_StartMonitor.UseVisualStyleBackColor = false;
            this.btn_StartMonitor.Click += new System.EventHandler(this.btn_StartMonitor_Click);
            // 
            // btn_TurnOffTEC
            // 
            this.btn_TurnOffTEC.BackColor = System.Drawing.SystemColors.Control;
            this.btn_TurnOffTEC.Location = new System.Drawing.Point(113, 168);
            this.btn_TurnOffTEC.Name = "btn_TurnOffTEC";
            this.btn_TurnOffTEC.Size = new System.Drawing.Size(75, 33);
            this.btn_TurnOffTEC.TabIndex = 3;
            this.btn_TurnOffTEC.Text = "关闭";
            this.btn_TurnOffTEC.UseVisualStyleBackColor = false;
            this.btn_TurnOffTEC.Click += new System.EventHandler(this.btn_TurnOffTEC_Click);
            // 
            // btn_TurnOnTEC
            // 
            this.btn_TurnOnTEC.BackColor = System.Drawing.SystemColors.Control;
            this.btn_TurnOnTEC.Location = new System.Drawing.Point(15, 168);
            this.btn_TurnOnTEC.Name = "btn_TurnOnTEC";
            this.btn_TurnOnTEC.Size = new System.Drawing.Size(75, 33);
            this.btn_TurnOnTEC.TabIndex = 3;
            this.btn_TurnOnTEC.Text = "打开";
            this.btn_TurnOnTEC.UseVisualStyleBackColor = false;
            this.btn_TurnOnTEC.Click += new System.EventHandler(this.btn_TurnOnTEC_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.chart_Temp);
            this.panel2.Location = new System.Drawing.Point(7, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(849, 417);
            this.panel2.TabIndex = 6;
            // 
            // FormTecControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(858, 433);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormTecControl";
            this.Text = "FormTecDisplay";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTecControl_FormClosing);
            this.Load += new System.EventHandler(this.FormTecControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart_Temp)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.num_TempSet)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Temp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_SetTemp;
        private System.Windows.Forms.NumericUpDown num_TempSet;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_TurnOffTEC;
        private System.Windows.Forms.Button btn_TurnOnTEC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_StopMonitor;
        private System.Windows.Forms.Button btn_StartMonitor;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSaveData;
        private System.Windows.Forms.TextBox txt_CurTempRead;
        private System.Windows.Forms.Label label4;
    }
}