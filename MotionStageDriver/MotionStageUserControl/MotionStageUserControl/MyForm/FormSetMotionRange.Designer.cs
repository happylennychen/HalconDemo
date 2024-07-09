namespace MyMotionStageUserControl.MyForm {
    partial class FormSetMotionRange {
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
            this.gbLowerLimit = new System.Windows.Forms.GroupBox();
            this.lblLowerLimitUnit = new System.Windows.Forms.Label();
            this.tbLowerLimit = new System.Windows.Forms.TextBox();
            this.rbLowerLimitDefine = new System.Windows.Forms.RadioButton();
            this.rbLowerLimitNone = new System.Windows.Forms.RadioButton();
            this.gbUpperLimit = new System.Windows.Forms.GroupBox();
            this.lblUpperLimitUnit = new System.Windows.Forms.Label();
            this.tbUpperLimit = new System.Windows.Forms.TextBox();
            this.rbUpperLimitDefine = new System.Windows.Forms.RadioButton();
            this.rbUpperLimitNone = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.gbLowerLimit.SuspendLayout();
            this.gbUpperLimit.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbLowerLimit
            // 
            this.gbLowerLimit.Controls.Add(this.lblLowerLimitUnit);
            this.gbLowerLimit.Controls.Add(this.tbLowerLimit);
            this.gbLowerLimit.Controls.Add(this.rbLowerLimitDefine);
            this.gbLowerLimit.Controls.Add(this.rbLowerLimitNone);
            this.gbLowerLimit.Location = new System.Drawing.Point(24, 23);
            this.gbLowerLimit.Name = "gbLowerLimit";
            this.gbLowerLimit.Size = new System.Drawing.Size(294, 75);
            this.gbLowerLimit.TabIndex = 0;
            this.gbLowerLimit.TabStop = false;
            this.gbLowerLimit.Text = "Lower Limit";
            // 
            // lblLowerLimitUnit
            // 
            this.lblLowerLimitUnit.AutoSize = true;
            this.lblLowerLimitUnit.Location = new System.Drawing.Point(242, 46);
            this.lblLowerLimitUnit.Name = "lblLowerLimitUnit";
            this.lblLowerLimitUnit.Size = new System.Drawing.Size(29, 12);
            this.lblLowerLimitUnit.TabIndex = 3;
            this.lblLowerLimitUnit.Text = "unit";
            // 
            // tbLowerLimit
            // 
            this.tbLowerLimit.Location = new System.Drawing.Point(88, 41);
            this.tbLowerLimit.Name = "tbLowerLimit";
            this.tbLowerLimit.Size = new System.Drawing.Size(148, 21);
            this.tbLowerLimit.TabIndex = 2;
            // 
            // rbLowerLimitDefine
            // 
            this.rbLowerLimitDefine.AutoSize = true;
            this.rbLowerLimitDefine.Location = new System.Drawing.Point(23, 42);
            this.rbLowerLimitDefine.Name = "rbLowerLimitDefine";
            this.rbLowerLimitDefine.Size = new System.Drawing.Size(59, 16);
            this.rbLowerLimitDefine.TabIndex = 1;
            this.rbLowerLimitDefine.TabStop = true;
            this.rbLowerLimitDefine.Text = "Define";
            this.rbLowerLimitDefine.UseVisualStyleBackColor = true;
            // 
            // rbLowerLimitNone
            // 
            this.rbLowerLimitNone.AutoSize = true;
            this.rbLowerLimitNone.Location = new System.Drawing.Point(23, 19);
            this.rbLowerLimitNone.Name = "rbLowerLimitNone";
            this.rbLowerLimitNone.Size = new System.Drawing.Size(47, 16);
            this.rbLowerLimitNone.TabIndex = 0;
            this.rbLowerLimitNone.TabStop = true;
            this.rbLowerLimitNone.Text = "None";
            this.rbLowerLimitNone.UseVisualStyleBackColor = true;
            // 
            // gbUpperLimit
            // 
            this.gbUpperLimit.Controls.Add(this.lblUpperLimitUnit);
            this.gbUpperLimit.Controls.Add(this.tbUpperLimit);
            this.gbUpperLimit.Controls.Add(this.rbUpperLimitDefine);
            this.gbUpperLimit.Controls.Add(this.rbUpperLimitNone);
            this.gbUpperLimit.Location = new System.Drawing.Point(24, 113);
            this.gbUpperLimit.Name = "gbUpperLimit";
            this.gbUpperLimit.Size = new System.Drawing.Size(294, 74);
            this.gbUpperLimit.TabIndex = 1;
            this.gbUpperLimit.TabStop = false;
            this.gbUpperLimit.Text = "Upper Limit";
            // 
            // lblUpperLimitUnit
            // 
            this.lblUpperLimitUnit.AutoSize = true;
            this.lblUpperLimitUnit.Location = new System.Drawing.Point(242, 45);
            this.lblUpperLimitUnit.Name = "lblUpperLimitUnit";
            this.lblUpperLimitUnit.Size = new System.Drawing.Size(29, 12);
            this.lblUpperLimitUnit.TabIndex = 3;
            this.lblUpperLimitUnit.Text = "unit";
            // 
            // tbUpperLimit
            // 
            this.tbUpperLimit.Location = new System.Drawing.Point(88, 40);
            this.tbUpperLimit.Name = "tbUpperLimit";
            this.tbUpperLimit.Size = new System.Drawing.Size(148, 21);
            this.tbUpperLimit.TabIndex = 2;
            // 
            // rbUpperLimitDefine
            // 
            this.rbUpperLimitDefine.AutoSize = true;
            this.rbUpperLimitDefine.Location = new System.Drawing.Point(23, 41);
            this.rbUpperLimitDefine.Name = "rbUpperLimitDefine";
            this.rbUpperLimitDefine.Size = new System.Drawing.Size(59, 16);
            this.rbUpperLimitDefine.TabIndex = 1;
            this.rbUpperLimitDefine.TabStop = true;
            this.rbUpperLimitDefine.Text = "Define";
            this.rbUpperLimitDefine.UseVisualStyleBackColor = true;
            // 
            // rbUpperLimitNone
            // 
            this.rbUpperLimitNone.AutoSize = true;
            this.rbUpperLimitNone.Location = new System.Drawing.Point(23, 18);
            this.rbUpperLimitNone.Name = "rbUpperLimitNone";
            this.rbUpperLimitNone.Size = new System.Drawing.Size(47, 16);
            this.rbUpperLimitNone.TabIndex = 0;
            this.rbUpperLimitNone.TabStop = true;
            this.rbUpperLimitNone.Text = "None";
            this.rbUpperLimitNone.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(243, 200);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FormSetMotionRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 236);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.gbUpperLimit);
            this.Controls.Add(this.gbLowerLimit);
            this.Name = "FormSetMotionRange";
            this.Text = "Set Motion Range";
            this.Load += new System.EventHandler(this.FormSetMotionRange_Load);
            this.gbLowerLimit.ResumeLayout(false);
            this.gbLowerLimit.PerformLayout();
            this.gbUpperLimit.ResumeLayout(false);
            this.gbUpperLimit.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbLowerLimit;
        private System.Windows.Forms.RadioButton rbLowerLimitNone;
        private System.Windows.Forms.RadioButton rbLowerLimitDefine;
        private System.Windows.Forms.TextBox tbLowerLimit;
        private System.Windows.Forms.Label lblLowerLimitUnit;
        private System.Windows.Forms.GroupBox gbUpperLimit;
        private System.Windows.Forms.RadioButton rbUpperLimitNone;
        private System.Windows.Forms.RadioButton rbUpperLimitDefine;
        private System.Windows.Forms.TextBox tbUpperLimit;
        private System.Windows.Forms.Label lblUpperLimitUnit;
        private System.Windows.Forms.Button btnOk;
    }
}