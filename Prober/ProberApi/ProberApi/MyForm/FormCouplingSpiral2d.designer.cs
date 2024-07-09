namespace ProberApi.MyForm {
    partial class FormCouplingSpiral2d {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.gbAxisPair = new System.Windows.Forms.GroupBox();
            this.tbAxisesPairList = new System.Windows.Forms.TextBox();
            this.btnAddAxisPair = new System.Windows.Forms.Button();
            this.cmbSecondAxis = new System.Windows.Forms.ComboBox();
            this.lbl2ndAxis = new System.Windows.Forms.Label();
            this.lbl1stAxis = new System.Windows.Forms.Label();
            this.cmbFirstAxis = new System.Windows.Forms.ComboBox();
            this.ckbSettingFeedbackInstrument = new System.Windows.Forms.CheckBox();
            this.ckbSettingCouplingInInstrument = new System.Windows.Forms.CheckBox();
            this.btnCouplingFeedbackInfo = new System.Windows.Forms.Button();
            this.btnCouplingInInfo = new System.Windows.Forms.Button();
            this.cmbParameterId = new System.Windows.Forms.ComboBox();
            this.lblParameterId = new System.Windows.Forms.Label();
            this.gbTraveling = new System.Windows.Forms.GroupBox();
            this.tbStep = new System.Windows.Forms.TextBox();
            this.lblStep = new System.Windows.Forms.Label();
            this.tbMotionRange = new System.Windows.Forms.TextBox();
            this.lblRange = new System.Windows.Forms.Label();
            this.gbFeedback = new System.Windows.Forms.GroupBox();
            this.tbFeedbackThreshold = new System.Windows.Forms.TextBox();
            this.lblThreshold = new System.Windows.Forms.Label();
            this.btnDoCoupling2d = new System.Windows.Forms.Button();
            this.ckbSaveRawData = new System.Windows.Forms.CheckBox();
            this.ckbDataWithWeight = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.gbAxisPair.SuspendLayout();
            this.gbTraveling.SuspendLayout();
            this.gbFeedback.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart
            // 
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(12, 12);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(530, 321);
            this.chart.TabIndex = 0;
            this.chart.Text = "chart1";
            // 
            // gbAxisPair
            // 
            this.gbAxisPair.Controls.Add(this.tbAxisesPairList);
            this.gbAxisPair.Controls.Add(this.btnAddAxisPair);
            this.gbAxisPair.Controls.Add(this.cmbSecondAxis);
            this.gbAxisPair.Controls.Add(this.lbl2ndAxis);
            this.gbAxisPair.Controls.Add(this.lbl1stAxis);
            this.gbAxisPair.Controls.Add(this.cmbFirstAxis);
            this.gbAxisPair.Location = new System.Drawing.Point(552, 10);
            this.gbAxisPair.Name = "gbAxisPair";
            this.gbAxisPair.Size = new System.Drawing.Size(513, 118);
            this.gbAxisPair.TabIndex = 14;
            this.gbAxisPair.TabStop = false;
            this.gbAxisPair.Text = "Coupling Axises Pair";
            // 
            // tbAxisesPairList
            // 
            this.tbAxisesPairList.Location = new System.Drawing.Point(270, 20);
            this.tbAxisesPairList.Multiline = true;
            this.tbAxisesPairList.Name = "tbAxisesPairList";
            this.tbAxisesPairList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbAxisesPairList.Size = new System.Drawing.Size(237, 82);
            this.tbAxisesPairList.TabIndex = 12;
            // 
            // btnAddAxisPair
            // 
            this.btnAddAxisPair.Location = new System.Drawing.Point(218, 33);
            this.btnAddAxisPair.Name = "btnAddAxisPair";
            this.btnAddAxisPair.Size = new System.Drawing.Size(46, 23);
            this.btnAddAxisPair.TabIndex = 11;
            this.btnAddAxisPair.Text = ">>";
            this.btnAddAxisPair.UseVisualStyleBackColor = true;
            this.btnAddAxisPair.Click += new System.EventHandler(this.btnAddAxisPair_Click);
            // 
            // cmbSecondAxis
            // 
            this.cmbSecondAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSecondAxis.FormattingEnabled = true;
            this.cmbSecondAxis.Location = new System.Drawing.Point(65, 50);
            this.cmbSecondAxis.Name = "cmbSecondAxis";
            this.cmbSecondAxis.Size = new System.Drawing.Size(147, 20);
            this.cmbSecondAxis.TabIndex = 10;
            // 
            // lbl2ndAxis
            // 
            this.lbl2ndAxis.Location = new System.Drawing.Point(6, 52);
            this.lbl2ndAxis.Name = "lbl2ndAxis";
            this.lbl2ndAxis.Size = new System.Drawing.Size(53, 18);
            this.lbl2ndAxis.TabIndex = 9;
            this.lbl2ndAxis.Text = "2nd Axis";
            this.lbl2ndAxis.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl1stAxis
            // 
            this.lbl1stAxis.Location = new System.Drawing.Point(6, 22);
            this.lbl1stAxis.Name = "lbl1stAxis";
            this.lbl1stAxis.Size = new System.Drawing.Size(53, 18);
            this.lbl1stAxis.TabIndex = 8;
            this.lbl1stAxis.Text = "1st Axis";
            this.lbl1stAxis.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbFirstAxis
            // 
            this.cmbFirstAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFirstAxis.FormattingEnabled = true;
            this.cmbFirstAxis.Location = new System.Drawing.Point(65, 20);
            this.cmbFirstAxis.Name = "cmbFirstAxis";
            this.cmbFirstAxis.Size = new System.Drawing.Size(147, 20);
            this.cmbFirstAxis.TabIndex = 7;
            // 
            // ckbSettingFeedbackInstrument
            // 
            this.ckbSettingFeedbackInstrument.AutoSize = true;
            this.ckbSettingFeedbackInstrument.Location = new System.Drawing.Point(857, 168);
            this.ckbSettingFeedbackInstrument.Name = "ckbSettingFeedbackInstrument";
            this.ckbSettingFeedbackInstrument.Size = new System.Drawing.Size(186, 16);
            this.ckbSettingFeedbackInstrument.TabIndex = 23;
            this.ckbSettingFeedbackInstrument.Text = "Setting Feedback Instrument";
            this.ckbSettingFeedbackInstrument.UseVisualStyleBackColor = true;
            // 
            // ckbSettingCouplingInInstrument
            // 
            this.ckbSettingCouplingInInstrument.AutoSize = true;
            this.ckbSettingCouplingInInstrument.Location = new System.Drawing.Point(857, 138);
            this.ckbSettingCouplingInInstrument.Name = "ckbSettingCouplingInInstrument";
            this.ckbSettingCouplingInInstrument.Size = new System.Drawing.Size(204, 16);
            this.ckbSettingCouplingInInstrument.TabIndex = 22;
            this.ckbSettingCouplingInInstrument.Text = "Setting Coupling In Instrument";
            this.ckbSettingCouplingInInstrument.UseVisualStyleBackColor = true;
            // 
            // btnCouplingFeedbackInfo
            // 
            this.btnCouplingFeedbackInfo.Location = new System.Drawing.Point(691, 164);
            this.btnCouplingFeedbackInfo.Name = "btnCouplingFeedbackInfo";
            this.btnCouplingFeedbackInfo.Size = new System.Drawing.Size(150, 23);
            this.btnCouplingFeedbackInfo.TabIndex = 21;
            this.btnCouplingFeedbackInfo.Text = "Coupling Feedback Info";
            this.btnCouplingFeedbackInfo.UseVisualStyleBackColor = true;
            this.btnCouplingFeedbackInfo.Click += new System.EventHandler(this.btnCouplingFeedbackInfo_Click);
            // 
            // btnCouplingInInfo
            // 
            this.btnCouplingInInfo.Location = new System.Drawing.Point(556, 164);
            this.btnCouplingInInfo.Name = "btnCouplingInInfo";
            this.btnCouplingInInfo.Size = new System.Drawing.Size(129, 23);
            this.btnCouplingInInfo.TabIndex = 20;
            this.btnCouplingInInfo.Text = "Coupling In Info";
            this.btnCouplingInInfo.UseVisualStyleBackColor = true;
            this.btnCouplingInInfo.Click += new System.EventHandler(this.btnCouplingInInfo_Click);
            // 
            // cmbParameterId
            // 
            this.cmbParameterId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParameterId.FormattingEnabled = true;
            this.cmbParameterId.Location = new System.Drawing.Point(691, 135);
            this.cmbParameterId.Name = "cmbParameterId";
            this.cmbParameterId.Size = new System.Drawing.Size(150, 20);
            this.cmbParameterId.TabIndex = 19;
            this.cmbParameterId.SelectedIndexChanged += new System.EventHandler(this.cmbParameterId_SelectedIndexChanged);
            // 
            // lblParameterId
            // 
            this.lblParameterId.Location = new System.Drawing.Point(555, 137);
            this.lblParameterId.Name = "lblParameterId";
            this.lblParameterId.Size = new System.Drawing.Size(131, 18);
            this.lblParameterId.TabIndex = 18;
            this.lblParameterId.Text = "Coupling Parameter ID";
            this.lblParameterId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbTraveling
            // 
            this.gbTraveling.Controls.Add(this.tbStep);
            this.gbTraveling.Controls.Add(this.lblStep);
            this.gbTraveling.Controls.Add(this.tbMotionRange);
            this.gbTraveling.Controls.Add(this.lblRange);
            this.gbTraveling.Location = new System.Drawing.Point(549, 206);
            this.gbTraveling.Name = "gbTraveling";
            this.gbTraveling.Size = new System.Drawing.Size(292, 54);
            this.gbTraveling.TabIndex = 24;
            this.gbTraveling.TabStop = false;
            this.gbTraveling.Text = "Traveling";
            // 
            // tbStep
            // 
            this.tbStep.Location = new System.Drawing.Point(229, 20);
            this.tbStep.Name = "tbStep";
            this.tbStep.Size = new System.Drawing.Size(42, 21);
            this.tbStep.TabIndex = 3;
            // 
            // lblStep
            // 
            this.lblStep.Location = new System.Drawing.Point(170, 23);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(53, 18);
            this.lblStep.TabIndex = 2;
            this.lblStep.Text = "Step(um)";
            this.lblStep.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbMotionRange
            // 
            this.tbMotionRange.Location = new System.Drawing.Point(113, 20);
            this.tbMotionRange.Name = "tbMotionRange";
            this.tbMotionRange.Size = new System.Drawing.Size(42, 21);
            this.tbMotionRange.TabIndex = 1;
            // 
            // lblRange
            // 
            this.lblRange.Location = new System.Drawing.Point(6, 23);
            this.lblRange.Name = "lblRange";
            this.lblRange.Size = new System.Drawing.Size(101, 18);
            this.lblRange.TabIndex = 0;
            this.lblRange.Text = "Motion Range(um)";
            this.lblRange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbFeedback
            // 
            this.gbFeedback.Controls.Add(this.tbFeedbackThreshold);
            this.gbFeedback.Controls.Add(this.lblThreshold);
            this.gbFeedback.Location = new System.Drawing.Point(855, 206);
            this.gbFeedback.Name = "gbFeedback";
            this.gbFeedback.Size = new System.Drawing.Size(205, 54);
            this.gbFeedback.TabIndex = 25;
            this.gbFeedback.TabStop = false;
            this.gbFeedback.Text = "Feedback";
            // 
            // tbFeedbackThreshold
            // 
            this.tbFeedbackThreshold.Location = new System.Drawing.Point(71, 20);
            this.tbFeedbackThreshold.Name = "tbFeedbackThreshold";
            this.tbFeedbackThreshold.Size = new System.Drawing.Size(128, 21);
            this.tbFeedbackThreshold.TabIndex = 1;
            // 
            // lblThreshold
            // 
            this.lblThreshold.Location = new System.Drawing.Point(6, 23);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new System.Drawing.Size(59, 18);
            this.lblThreshold.TabIndex = 0;
            this.lblThreshold.Text = "Threshold";
            this.lblThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnDoCoupling2d
            // 
            this.btnDoCoupling2d.Location = new System.Drawing.Point(928, 282);
            this.btnDoCoupling2d.Name = "btnDoCoupling2d";
            this.btnDoCoupling2d.Size = new System.Drawing.Size(128, 23);
            this.btnDoCoupling2d.TabIndex = 26;
            this.btnDoCoupling2d.Text = "Do Coupling 2D";
            this.btnDoCoupling2d.UseVisualStyleBackColor = true;
            this.btnDoCoupling2d.Click += new System.EventHandler(this.btnDoCoupling2d_Click);
            // 
            // ckbSaveRawData
            // 
            this.ckbSaveRawData.AutoSize = true;
            this.ckbSaveRawData.Location = new System.Drawing.Point(721, 289);
            this.ckbSaveRawData.Name = "ckbSaveRawData";
            this.ckbSaveRawData.Size = new System.Drawing.Size(102, 16);
            this.ckbSaveRawData.TabIndex = 27;
            this.ckbSaveRawData.Text = "Save Raw Data";
            this.ckbSaveRawData.UseVisualStyleBackColor = true;
            // 
            // ckbDataWithWeight
            // 
            this.ckbDataWithWeight.AutoSize = true;
            this.ckbDataWithWeight.Location = new System.Drawing.Point(578, 289);
            this.ckbDataWithWeight.Name = "ckbDataWithWeight";
            this.ckbDataWithWeight.Size = new System.Drawing.Size(114, 16);
            this.ckbDataWithWeight.TabIndex = 28;
            this.ckbDataWithWeight.Text = "Dealwith Weight";
            this.ckbDataWithWeight.UseVisualStyleBackColor = true;
            // 
            // FormCouplingSpiral2d
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 343);
            this.Controls.Add(this.ckbDataWithWeight);
            this.Controls.Add(this.ckbSaveRawData);
            this.Controls.Add(this.btnDoCoupling2d);
            this.Controls.Add(this.gbFeedback);
            this.Controls.Add(this.gbTraveling);
            this.Controls.Add(this.ckbSettingFeedbackInstrument);
            this.Controls.Add(this.ckbSettingCouplingInInstrument);
            this.Controls.Add(this.btnCouplingFeedbackInfo);
            this.Controls.Add(this.btnCouplingInInfo);
            this.Controls.Add(this.cmbParameterId);
            this.Controls.Add(this.lblParameterId);
            this.Controls.Add(this.gbAxisPair);
            this.Controls.Add(this.chart);
            this.Name = "FormCouplingSpiral2d";
            this.Text = "Spiral coupling 2D";
            this.Load += new System.EventHandler(this.FormSpiralCoupling2d_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.gbAxisPair.ResumeLayout(false);
            this.gbAxisPair.PerformLayout();
            this.gbTraveling.ResumeLayout(false);
            this.gbTraveling.PerformLayout();
            this.gbFeedback.ResumeLayout(false);
            this.gbFeedback.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.GroupBox gbAxisPair;
        private System.Windows.Forms.TextBox tbAxisesPairList;
        private System.Windows.Forms.Button btnAddAxisPair;
        private System.Windows.Forms.ComboBox cmbSecondAxis;
        private System.Windows.Forms.Label lbl2ndAxis;
        private System.Windows.Forms.Label lbl1stAxis;
        private System.Windows.Forms.ComboBox cmbFirstAxis;
        private System.Windows.Forms.CheckBox ckbSettingFeedbackInstrument;
        private System.Windows.Forms.CheckBox ckbSettingCouplingInInstrument;
        private System.Windows.Forms.Button btnCouplingFeedbackInfo;
        private System.Windows.Forms.Button btnCouplingInInfo;
        private System.Windows.Forms.ComboBox cmbParameterId;
        private System.Windows.Forms.Label lblParameterId;
        private System.Windows.Forms.GroupBox gbTraveling;
        private System.Windows.Forms.TextBox tbStep;
        private System.Windows.Forms.Label lblStep;
        private System.Windows.Forms.TextBox tbMotionRange;
        private System.Windows.Forms.Label lblRange;
        private System.Windows.Forms.GroupBox gbFeedback;
        private System.Windows.Forms.TextBox tbFeedbackThreshold;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.Button btnDoCoupling2d;
        private System.Windows.Forms.CheckBox ckbSaveRawData;
        private System.Windows.Forms.CheckBox ckbDataWithWeight;
    }
}