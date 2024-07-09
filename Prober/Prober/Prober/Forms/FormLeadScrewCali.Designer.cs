namespace Prober.Forms {
    partial class FormLeadScrewCali {
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
            this.label1 = new System.Windows.Forms.Label();
            this.txt_CompStart = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_CompStop = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_CompStep = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_CompAdjustDis = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_CompDelay = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_CompTimes = new System.Windows.Forms.TextBox();
            this.btn_StartX = new System.Windows.Forms.Button();
            this.btn_StopX = new System.Windows.Forms.Button();
            this.txt_Log = new System.Windows.Forms.RichTextBox();
            this.cmb_Axis = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "起始位置(um)";
            // 
            // txt_CompStart
            // 
            this.txt_CompStart.Location = new System.Drawing.Point(128, 78);
            this.txt_CompStart.Name = "txt_CompStart";
            this.txt_CompStart.Size = new System.Drawing.Size(75, 21);
            this.txt_CompStart.TabIndex = 1;
            this.txt_CompStart.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "结束位置(um)";
            // 
            // txt_CompStop
            // 
            this.txt_CompStop.Location = new System.Drawing.Point(128, 107);
            this.txt_CompStop.Name = "txt_CompStop";
            this.txt_CompStop.Size = new System.Drawing.Size(75, 21);
            this.txt_CompStop.TabIndex = 1;
            this.txt_CompStop.Text = "348000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "补偿间距(um)";
            // 
            // txt_CompStep
            // 
            this.txt_CompStep.Location = new System.Drawing.Point(128, 134);
            this.txt_CompStep.Name = "txt_CompStep";
            this.txt_CompStep.Size = new System.Drawing.Size(75, 21);
            this.txt_CompStep.TabIndex = 1;
            this.txt_CompStep.Text = "2000";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(233, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "调整距离(um)";
            // 
            // txt_CompAdjustDis
            // 
            this.txt_CompAdjustDis.Location = new System.Drawing.Point(316, 78);
            this.txt_CompAdjustDis.Name = "txt_CompAdjustDis";
            this.txt_CompAdjustDis.Size = new System.Drawing.Size(75, 21);
            this.txt_CompAdjustDis.TabIndex = 1;
            this.txt_CompAdjustDis.Text = "500";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(233, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "延时时间(s)";
            // 
            // txt_CompDelay
            // 
            this.txt_CompDelay.Location = new System.Drawing.Point(316, 107);
            this.txt_CompDelay.Name = "txt_CompDelay";
            this.txt_CompDelay.Size = new System.Drawing.Size(75, 21);
            this.txt_CompDelay.TabIndex = 1;
            this.txt_CompDelay.Text = "3";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(233, 138);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "循环次数";
            // 
            // txt_CompTimes
            // 
            this.txt_CompTimes.Location = new System.Drawing.Point(316, 134);
            this.txt_CompTimes.Name = "txt_CompTimes";
            this.txt_CompTimes.Size = new System.Drawing.Size(75, 21);
            this.txt_CompTimes.TabIndex = 1;
            this.txt_CompTimes.Text = "1";
            // 
            // btn_StartX
            // 
            this.btn_StartX.Location = new System.Drawing.Point(434, 79);
            this.btn_StartX.Name = "btn_StartX";
            this.btn_StartX.Size = new System.Drawing.Size(75, 31);
            this.btn_StartX.TabIndex = 2;
            this.btn_StartX.Text = "开始";
            this.btn_StartX.UseVisualStyleBackColor = true;
            this.btn_StartX.Click += new System.EventHandler(this.btn_StartX_Click);
            // 
            // btn_StopX
            // 
            this.btn_StopX.Location = new System.Drawing.Point(434, 124);
            this.btn_StopX.Name = "btn_StopX";
            this.btn_StopX.Size = new System.Drawing.Size(75, 31);
            this.btn_StopX.TabIndex = 2;
            this.btn_StopX.Text = "停止";
            this.btn_StopX.UseVisualStyleBackColor = true;
            this.btn_StopX.Click += new System.EventHandler(this.btn_StopX_Click);
            // 
            // txt_Log
            // 
            this.txt_Log.Location = new System.Drawing.Point(560, 36);
            this.txt_Log.Name = "txt_Log";
            this.txt_Log.Size = new System.Drawing.Size(692, 484);
            this.txt_Log.TabIndex = 3;
            this.txt_Log.Text = "";
            // 
            // cmb_Axis
            // 
            this.cmb_Axis.FormattingEnabled = true;
            this.cmb_Axis.Items.AddRange(new object[] {
            "X",
            "Y",
            "Z"});
            this.cmb_Axis.Location = new System.Drawing.Point(127, 39);
            this.cmb_Axis.Name = "cmb_Axis";
            this.cmb_Axis.Size = new System.Drawing.Size(75, 20);
            this.cmb_Axis.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(46, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "轴号";
            // 
            // FormLeadScrewCali
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1277, 531);
            this.Controls.Add(this.cmb_Axis);
            this.Controls.Add(this.btn_StopX);
            this.Controls.Add(this.btn_StartX);
            this.Controls.Add(this.txt_CompStep);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_Log);
            this.Controls.Add(this.txt_CompStop);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_CompStart);
            this.Controls.Add(this.txt_CompTimes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_CompDelay);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_CompAdjustDis);
            this.Controls.Add(this.label6);
            this.Name = "FormLeadScrewCali";
            this.Text = "FormLeadScrewCali";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_CompStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_CompStop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_CompStep;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_CompAdjustDis;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_CompDelay;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_CompTimes;
        private System.Windows.Forms.Button btn_StartX;
        private System.Windows.Forms.Button btn_StopX;
        private System.Windows.Forms.RichTextBox txt_Log;
        private System.Windows.Forms.ComboBox cmb_Axis;
        private System.Windows.Forms.Label label7;
    }
}