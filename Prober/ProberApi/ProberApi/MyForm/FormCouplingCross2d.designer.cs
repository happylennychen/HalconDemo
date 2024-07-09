namespace ProberApi.MyForm {
    partial class FormCouplingCross2d {
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btnCoupling2D = new System.Windows.Forms.Button();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.gbTraveling = new System.Windows.Forms.GroupBox();
            this.tbRefinedStep = new System.Windows.Forms.TextBox();
            this.lblRefinedStep = new System.Windows.Forms.Label();
            this.tbRefinedMotionRange = new System.Windows.Forms.TextBox();
            this.lblRefinedRange = new System.Windows.Forms.Label();
            this.ckbEnableRefined = new System.Windows.Forms.CheckBox();
            this.tbCoarseStep = new System.Windows.Forms.TextBox();
            this.lblCoarseStep = new System.Windows.Forms.Label();
            this.tbCoarseMotionRange = new System.Windows.Forms.TextBox();
            this.lblCoarseRange = new System.Windows.Forms.Label();
            this.gbAxisPair = new System.Windows.Forms.GroupBox();
            this.tbAxisesPairList = new System.Windows.Forms.TextBox();
            this.btnAddAxisPair = new System.Windows.Forms.Button();
            this.cmbSecondAxis = new System.Windows.Forms.ComboBox();
            this.lbl2ndAxis = new System.Windows.Forms.Label();
            this.lbl1stAxis = new System.Windows.Forms.Label();
            this.cmbFirstAxis = new System.Windows.Forms.ComboBox();
            this.ckbSaveRawData = new System.Windows.Forms.CheckBox();
            this.ckbIsTriggered = new System.Windows.Forms.CheckBox();
            this.gbCouplingParameter = new System.Windows.Forms.GroupBox();
            this.ckbSettingFeedbackInstrument = new System.Windows.Forms.CheckBox();
            this.ckbSettingCouplingInInstrument = new System.Windows.Forms.CheckBox();
            this.btnCouplingFeedbackInfo = new System.Windows.Forms.Button();
            this.btnCouplingInInfo = new System.Windows.Forms.Button();
            this.cmbParameterId = new System.Windows.Forms.ComboBox();
            this.lblParameterId = new System.Windows.Forms.Label();
            this.gbTrigger = new System.Windows.Forms.GroupBox();
            this.btnTriggerCouplingFeedbackSetting = new System.Windows.Forms.Button();
            this.ckbDataWithWeight = new System.Windows.Forms.CheckBox();
            this.labelThreshold = new System.Windows.Forms.Label();
            this.txt_PeakThreshold = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.gbTraveling.SuspendLayout();
            this.gbAxisPair.SuspendLayout();
            this.gbCouplingParameter.SuspendLayout();
            this.gbTrigger.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCoupling2D
            // 
            this.btnCoupling2D.Location = new System.Drawing.Point(920, 294);
            this.btnCoupling2D.Name = "btnCoupling2D";
            this.btnCoupling2D.Size = new System.Drawing.Size(150, 23);
            this.btnCoupling2D.TabIndex = 1;
            this.btnCoupling2D.Text = "Do coupling 2D";
            this.btnCoupling2D.UseVisualStyleBackColor = true;
            this.btnCoupling2D.Click += new System.EventHandler(this.btnCoupling2D_Click);
            // 
            // chart
            // 
            chartArea2.AxisY.IsStartedFromZero = false;
            chartArea2.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea2);
            this.chart.Dock = System.Windows.Forms.DockStyle.Left;
            legend2.Name = "Legend1";
            this.chart.Legends.Add(legend2);
            this.chart.Location = new System.Drawing.Point(0, 0);
            this.chart.Name = "chart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart.Series.Add(series2);
            this.chart.Size = new System.Drawing.Size(551, 336);
            this.chart.TabIndex = 2;
            this.chart.Text = "chart1";
            // 
            // gbTraveling
            // 
            this.gbTraveling.Controls.Add(this.tbRefinedStep);
            this.gbTraveling.Controls.Add(this.lblRefinedStep);
            this.gbTraveling.Controls.Add(this.tbRefinedMotionRange);
            this.gbTraveling.Controls.Add(this.lblRefinedRange);
            this.gbTraveling.Controls.Add(this.ckbEnableRefined);
            this.gbTraveling.Controls.Add(this.tbCoarseStep);
            this.gbTraveling.Controls.Add(this.lblCoarseStep);
            this.gbTraveling.Controls.Add(this.tbCoarseMotionRange);
            this.gbTraveling.Controls.Add(this.lblCoarseRange);
            this.gbTraveling.Location = new System.Drawing.Point(557, 186);
            this.gbTraveling.Name = "gbTraveling";
            this.gbTraveling.Size = new System.Drawing.Size(513, 72);
            this.gbTraveling.TabIndex = 10;
            this.gbTraveling.TabStop = false;
            this.gbTraveling.Text = "Traveling";
            // 
            // tbRefinedStep
            // 
            this.tbRefinedStep.Location = new System.Drawing.Point(465, 43);
            this.tbRefinedStep.Name = "tbRefinedStep";
            this.tbRefinedStep.Size = new System.Drawing.Size(42, 21);
            this.tbRefinedStep.TabIndex = 8;
            // 
            // lblRefinedStep
            // 
            this.lblRefinedStep.Location = new System.Drawing.Point(358, 43);
            this.lblRefinedStep.Name = "lblRefinedStep";
            this.lblRefinedStep.Size = new System.Drawing.Size(101, 18);
            this.lblRefinedStep.TabIndex = 7;
            this.lblRefinedStep.Text = "Refined Step(um)";
            this.lblRefinedStep.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbRefinedMotionRange
            // 
            this.tbRefinedMotionRange.Location = new System.Drawing.Point(296, 43);
            this.tbRefinedMotionRange.Name = "tbRefinedMotionRange";
            this.tbRefinedMotionRange.Size = new System.Drawing.Size(42, 21);
            this.tbRefinedMotionRange.TabIndex = 6;
            // 
            // lblRefinedRange
            // 
            this.lblRefinedRange.Location = new System.Drawing.Point(141, 46);
            this.lblRefinedRange.Name = "lblRefinedRange";
            this.lblRefinedRange.Size = new System.Drawing.Size(149, 18);
            this.lblRefinedRange.TabIndex = 5;
            this.lblRefinedRange.Text = "Refined Motion Range(um)";
            this.lblRefinedRange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckbEnableRefined
            // 
            this.ckbEnableRefined.AutoSize = true;
            this.ckbEnableRefined.Location = new System.Drawing.Point(5, 48);
            this.ckbEnableRefined.Name = "ckbEnableRefined";
            this.ckbEnableRefined.Size = new System.Drawing.Size(108, 16);
            this.ckbEnableRefined.TabIndex = 4;
            this.ckbEnableRefined.Text = "Enable Refined";
            this.ckbEnableRefined.UseVisualStyleBackColor = true;
            this.ckbEnableRefined.CheckedChanged += new System.EventHandler(this.ckbEnableRefine_CheckedChanged);
            // 
            // tbCoarseStep
            // 
            this.tbCoarseStep.Location = new System.Drawing.Point(465, 14);
            this.tbCoarseStep.Name = "tbCoarseStep";
            this.tbCoarseStep.Size = new System.Drawing.Size(42, 21);
            this.tbCoarseStep.TabIndex = 3;
            // 
            // lblCoarseStep
            // 
            this.lblCoarseStep.Location = new System.Drawing.Point(364, 14);
            this.lblCoarseStep.Name = "lblCoarseStep";
            this.lblCoarseStep.Size = new System.Drawing.Size(95, 18);
            this.lblCoarseStep.TabIndex = 2;
            this.lblCoarseStep.Text = "Coarse Step(um)";
            this.lblCoarseStep.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbCoarseMotionRange
            // 
            this.tbCoarseMotionRange.Location = new System.Drawing.Point(296, 14);
            this.tbCoarseMotionRange.Name = "tbCoarseMotionRange";
            this.tbCoarseMotionRange.Size = new System.Drawing.Size(42, 21);
            this.tbCoarseMotionRange.TabIndex = 1;
            // 
            // lblCoarseRange
            // 
            this.lblCoarseRange.Location = new System.Drawing.Point(147, 17);
            this.lblCoarseRange.Name = "lblCoarseRange";
            this.lblCoarseRange.Size = new System.Drawing.Size(143, 18);
            this.lblCoarseRange.TabIndex = 0;
            this.lblCoarseRange.Text = "Coarse Motion Range(um)";
            this.lblCoarseRange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbAxisPair
            // 
            this.gbAxisPair.Controls.Add(this.tbAxisesPairList);
            this.gbAxisPair.Controls.Add(this.btnAddAxisPair);
            this.gbAxisPair.Controls.Add(this.cmbSecondAxis);
            this.gbAxisPair.Controls.Add(this.lbl2ndAxis);
            this.gbAxisPair.Controls.Add(this.lbl1stAxis);
            this.gbAxisPair.Controls.Add(this.cmbFirstAxis);
            this.gbAxisPair.Location = new System.Drawing.Point(557, 12);
            this.gbAxisPair.Name = "gbAxisPair";
            this.gbAxisPair.Size = new System.Drawing.Size(513, 92);
            this.gbAxisPair.TabIndex = 13;
            this.gbAxisPair.TabStop = false;
            this.gbAxisPair.Text = "Coupling Axises Pair";
            // 
            // tbAxisesPairList
            // 
            this.tbAxisesPairList.Location = new System.Drawing.Point(270, 20);
            this.tbAxisesPairList.Multiline = true;
            this.tbAxisesPairList.Name = "tbAxisesPairList";
            this.tbAxisesPairList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbAxisesPairList.Size = new System.Drawing.Size(237, 60);
            this.tbAxisesPairList.TabIndex = 12;
            // 
            // btnAddAxisPair
            // 
            this.btnAddAxisPair.Location = new System.Drawing.Point(218, 38);
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
            this.cmbSecondAxis.Location = new System.Drawing.Point(65, 60);
            this.cmbSecondAxis.Name = "cmbSecondAxis";
            this.cmbSecondAxis.Size = new System.Drawing.Size(147, 20);
            this.cmbSecondAxis.TabIndex = 10;
            // 
            // lbl2ndAxis
            // 
            this.lbl2ndAxis.Location = new System.Drawing.Point(6, 57);
            this.lbl2ndAxis.Name = "lbl2ndAxis";
            this.lbl2ndAxis.Size = new System.Drawing.Size(53, 18);
            this.lbl2ndAxis.TabIndex = 9;
            this.lbl2ndAxis.Text = "2nd Axis";
            this.lbl2ndAxis.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl1stAxis
            // 
            this.lbl1stAxis.Location = new System.Drawing.Point(6, 20);
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
            // ckbSaveRawData
            // 
            this.ckbSaveRawData.Location = new System.Drawing.Point(787, 263);
            this.ckbSaveRawData.Name = "ckbSaveRawData";
            this.ckbSaveRawData.Size = new System.Drawing.Size(126, 19);
            this.ckbSaveRawData.TabIndex = 18;
            this.ckbSaveRawData.Text = "Save Raw Data";
            this.ckbSaveRawData.UseVisualStyleBackColor = true;
            // 
            // ckbIsTriggered
            // 
            this.ckbIsTriggered.AutoSize = true;
            this.ckbIsTriggered.Location = new System.Drawing.Point(5, 0);
            this.ckbIsTriggered.Name = "ckbIsTriggered";
            this.ckbIsTriggered.Size = new System.Drawing.Size(84, 16);
            this.ckbIsTriggered.TabIndex = 19;
            this.ckbIsTriggered.Text = "By Trigger";
            this.ckbIsTriggered.UseVisualStyleBackColor = true;
            this.ckbIsTriggered.CheckedChanged += new System.EventHandler(this.ckbIsTriggered_CheckedChanged);
            // 
            // gbCouplingParameter
            // 
            this.gbCouplingParameter.Controls.Add(this.ckbSettingFeedbackInstrument);
            this.gbCouplingParameter.Controls.Add(this.ckbSettingCouplingInInstrument);
            this.gbCouplingParameter.Controls.Add(this.btnCouplingFeedbackInfo);
            this.gbCouplingParameter.Controls.Add(this.btnCouplingInInfo);
            this.gbCouplingParameter.Controls.Add(this.cmbParameterId);
            this.gbCouplingParameter.Controls.Add(this.lblParameterId);
            this.gbCouplingParameter.Location = new System.Drawing.Point(557, 105);
            this.gbCouplingParameter.Name = "gbCouplingParameter";
            this.gbCouplingParameter.Size = new System.Drawing.Size(513, 80);
            this.gbCouplingParameter.TabIndex = 20;
            this.gbCouplingParameter.TabStop = false;
            this.gbCouplingParameter.Text = "Coupling Parameter";
            // 
            // ckbSettingFeedbackInstrument
            // 
            this.ckbSettingFeedbackInstrument.AutoSize = true;
            this.ckbSettingFeedbackInstrument.Location = new System.Drawing.Point(306, 52);
            this.ckbSettingFeedbackInstrument.Name = "ckbSettingFeedbackInstrument";
            this.ckbSettingFeedbackInstrument.Size = new System.Drawing.Size(186, 16);
            this.ckbSettingFeedbackInstrument.TabIndex = 23;
            this.ckbSettingFeedbackInstrument.Text = "Setting Feedback Instrument";
            this.ckbSettingFeedbackInstrument.UseVisualStyleBackColor = true;
            // 
            // ckbSettingCouplingInInstrument
            // 
            this.ckbSettingCouplingInInstrument.AutoSize = true;
            this.ckbSettingCouplingInInstrument.Location = new System.Drawing.Point(306, 22);
            this.ckbSettingCouplingInInstrument.Name = "ckbSettingCouplingInInstrument";
            this.ckbSettingCouplingInInstrument.Size = new System.Drawing.Size(204, 16);
            this.ckbSettingCouplingInInstrument.TabIndex = 22;
            this.ckbSettingCouplingInInstrument.Text = "Setting Coupling In Instrument";
            this.ckbSettingCouplingInInstrument.UseVisualStyleBackColor = true;
            // 
            // btnCouplingFeedbackInfo
            // 
            this.btnCouplingFeedbackInfo.Location = new System.Drawing.Point(140, 48);
            this.btnCouplingFeedbackInfo.Name = "btnCouplingFeedbackInfo";
            this.btnCouplingFeedbackInfo.Size = new System.Drawing.Size(150, 23);
            this.btnCouplingFeedbackInfo.TabIndex = 21;
            this.btnCouplingFeedbackInfo.Text = "Coupling Feedback Info";
            this.btnCouplingFeedbackInfo.UseVisualStyleBackColor = true;
            this.btnCouplingFeedbackInfo.Click += new System.EventHandler(this.btnCouplingFeedbackInfo_Click);
            // 
            // btnCouplingInInfo
            // 
            this.btnCouplingInInfo.Location = new System.Drawing.Point(5, 48);
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
            this.cmbParameterId.Location = new System.Drawing.Point(140, 19);
            this.cmbParameterId.Name = "cmbParameterId";
            this.cmbParameterId.Size = new System.Drawing.Size(150, 20);
            this.cmbParameterId.TabIndex = 19;
            this.cmbParameterId.SelectedIndexChanged += new System.EventHandler(this.cmbParameterId_SelectedIndexChanged);
            // 
            // lblParameterId
            // 
            this.lblParameterId.Location = new System.Drawing.Point(3, 19);
            this.lblParameterId.Name = "lblParameterId";
            this.lblParameterId.Size = new System.Drawing.Size(131, 18);
            this.lblParameterId.TabIndex = 18;
            this.lblParameterId.Text = "Parameter ID";
            this.lblParameterId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbTrigger
            // 
            this.gbTrigger.Controls.Add(this.btnTriggerCouplingFeedbackSetting);
            this.gbTrigger.Controls.Add(this.ckbIsTriggered);
            this.gbTrigger.Location = new System.Drawing.Point(557, 264);
            this.gbTrigger.Name = "gbTrigger";
            this.gbTrigger.Size = new System.Drawing.Size(224, 67);
            this.gbTrigger.TabIndex = 21;
            this.gbTrigger.TabStop = false;
            // 
            // btnTriggerCouplingFeedbackSetting
            // 
            this.btnTriggerCouplingFeedbackSetting.Location = new System.Drawing.Point(8, 33);
            this.btnTriggerCouplingFeedbackSetting.Name = "btnTriggerCouplingFeedbackSetting";
            this.btnTriggerCouplingFeedbackSetting.Size = new System.Drawing.Size(210, 23);
            this.btnTriggerCouplingFeedbackSetting.TabIndex = 20;
            this.btnTriggerCouplingFeedbackSetting.Text = "Coupling Feedback Setting";
            this.btnTriggerCouplingFeedbackSetting.UseVisualStyleBackColor = true;
            this.btnTriggerCouplingFeedbackSetting.Click += new System.EventHandler(this.btnTriggerCouplingFeedbackSetting_Click);
            // 
            // ckbDataWithWeight
            // 
            this.ckbDataWithWeight.AutoSize = true;
            this.ckbDataWithWeight.Location = new System.Drawing.Point(787, 286);
            this.ckbDataWithWeight.Name = "ckbDataWithWeight";
            this.ckbDataWithWeight.Size = new System.Drawing.Size(114, 16);
            this.ckbDataWithWeight.TabIndex = 22;
            this.ckbDataWithWeight.Text = "Dealwith Weight";
            this.ckbDataWithWeight.UseVisualStyleBackColor = true;
            // 
            // labelThreshold
            // 
            this.labelThreshold.AutoSize = true;
            this.labelThreshold.Location = new System.Drawing.Point(787, 308);
            this.labelThreshold.Name = "labelThreshold";
            this.labelThreshold.Size = new System.Drawing.Size(59, 12);
            this.labelThreshold.TabIndex = 23;
            this.labelThreshold.Text = "Threshold";
            // 
            // txt_PeakThreshold
            // 
            this.txt_PeakThreshold.Location = new System.Drawing.Point(853, 303);
            this.txt_PeakThreshold.Name = "txt_PeakThreshold";
            this.txt_PeakThreshold.Size = new System.Drawing.Size(46, 21);
            this.txt_PeakThreshold.TabIndex = 24;
            this.txt_PeakThreshold.Text = "3";
            // 
            // FormCouplingCross2d
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 336);
            this.Controls.Add(this.txt_PeakThreshold);
            this.Controls.Add(this.labelThreshold);
            this.Controls.Add(this.ckbDataWithWeight);
            this.Controls.Add(this.gbTrigger);
            this.Controls.Add(this.gbCouplingParameter);
            this.Controls.Add(this.ckbSaveRawData);
            this.Controls.Add(this.gbAxisPair);
            this.Controls.Add(this.gbTraveling);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.btnCoupling2D);
            this.Name = "FormCouplingCross2d";
            this.Text = "Cross coupling 2D";
            this.Load += new System.EventHandler(this.FormCrossCoupling2d_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.gbTraveling.ResumeLayout(false);
            this.gbTraveling.PerformLayout();
            this.gbAxisPair.ResumeLayout(false);
            this.gbAxisPair.PerformLayout();
            this.gbCouplingParameter.ResumeLayout(false);
            this.gbCouplingParameter.PerformLayout();
            this.gbTrigger.ResumeLayout(false);
            this.gbTrigger.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCoupling2D;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.GroupBox gbTraveling;
        private System.Windows.Forms.Label lblCoarseRange;
        private System.Windows.Forms.TextBox tbCoarseMotionRange;
        private System.Windows.Forms.Label lblCoarseStep;
        private System.Windows.Forms.TextBox tbCoarseStep;
        private System.Windows.Forms.CheckBox ckbEnableRefined;
        private System.Windows.Forms.Label lblRefinedRange;
        private System.Windows.Forms.TextBox tbRefinedMotionRange;
        private System.Windows.Forms.Label lblRefinedStep;
        private System.Windows.Forms.TextBox tbRefinedStep;
        private System.Windows.Forms.GroupBox gbAxisPair;
        private System.Windows.Forms.ComboBox cmbSecondAxis;
        private System.Windows.Forms.Label lbl2ndAxis;
        private System.Windows.Forms.Label lbl1stAxis;
        private System.Windows.Forms.ComboBox cmbFirstAxis;
        private System.Windows.Forms.Button btnAddAxisPair;
        private System.Windows.Forms.TextBox tbAxisesPairList;
        private System.Windows.Forms.CheckBox ckbSaveRawData;
        private System.Windows.Forms.CheckBox ckbIsTriggered;
        private System.Windows.Forms.GroupBox gbCouplingParameter;
        private System.Windows.Forms.CheckBox ckbSettingFeedbackInstrument;
        private System.Windows.Forms.CheckBox ckbSettingCouplingInInstrument;
        private System.Windows.Forms.Button btnCouplingFeedbackInfo;
        private System.Windows.Forms.Button btnCouplingInInfo;
        private System.Windows.Forms.ComboBox cmbParameterId;
        private System.Windows.Forms.Label lblParameterId;
        private System.Windows.Forms.GroupBox gbTrigger;
        private System.Windows.Forms.Button btnTriggerCouplingFeedbackSetting;
        private System.Windows.Forms.CheckBox ckbDataWithWeight;
        private System.Windows.Forms.Label labelThreshold;
        private System.Windows.Forms.TextBox txt_PeakThreshold;
    }
}