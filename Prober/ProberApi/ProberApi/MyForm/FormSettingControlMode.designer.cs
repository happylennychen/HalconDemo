namespace ProberApi.MyForm {
    partial class FormSettingControlMode {
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
            this.gbControlMode = new System.Windows.Forms.GroupBox();
            this.rbRemoteFirst = new System.Windows.Forms.RadioButton();
            this.rbLocalOnly = new System.Windows.Forms.RadioButton();
            this.lblLastControlMode = new System.Windows.Forms.Label();
            this.tbLastControlMode = new System.Windows.Forms.TextBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.gbControlMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbControlMode
            // 
            this.gbControlMode.Controls.Add(this.rbRemoteFirst);
            this.gbControlMode.Controls.Add(this.rbLocalOnly);
            this.gbControlMode.Location = new System.Drawing.Point(34, 27);
            this.gbControlMode.Name = "gbControlMode";
            this.gbControlMode.Size = new System.Drawing.Size(287, 82);
            this.gbControlMode.TabIndex = 0;
            this.gbControlMode.TabStop = false;
            this.gbControlMode.Text = "Control Mode";
            // 
            // rbRemoteFirst
            // 
            this.rbRemoteFirst.AutoSize = true;
            this.rbRemoteFirst.Location = new System.Drawing.Point(153, 38);
            this.rbRemoteFirst.Name = "rbRemoteFirst";
            this.rbRemoteFirst.Size = new System.Drawing.Size(95, 16);
            this.rbRemoteFirst.TabIndex = 1;
            this.rbRemoteFirst.TabStop = true;
            this.rbRemoteFirst.Text = "Remote First";
            this.rbRemoteFirst.UseVisualStyleBackColor = true;
            // 
            // rbLocalOnly
            // 
            this.rbLocalOnly.AutoSize = true;
            this.rbLocalOnly.Location = new System.Drawing.Point(35, 38);
            this.rbLocalOnly.Name = "rbLocalOnly";
            this.rbLocalOnly.Size = new System.Drawing.Size(83, 16);
            this.rbLocalOnly.TabIndex = 0;
            this.rbLocalOnly.TabStop = true;
            this.rbLocalOnly.Text = "Local Only";
            this.rbLocalOnly.UseVisualStyleBackColor = true;
            this.rbLocalOnly.CheckedChanged += new System.EventHandler(this.rbLocalOnly_CheckedChanged);
            // 
            // lblLastControlMode
            // 
            this.lblLastControlMode.Location = new System.Drawing.Point(12, 146);
            this.lblLastControlMode.Name = "lblLastControlMode";
            this.lblLastControlMode.Size = new System.Drawing.Size(154, 18);
            this.lblLastControlMode.TabIndex = 1;
            this.lblLastControlMode.Text = "Last Control Mode";
            this.lblLastControlMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbLastControlMode
            // 
            this.tbLastControlMode.Location = new System.Drawing.Point(172, 143);
            this.tbLastControlMode.Name = "tbLastControlMode";
            this.tbLastControlMode.Size = new System.Drawing.Size(122, 21);
            this.tbLastControlMode.TabIndex = 2;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(172, 182);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(122, 23);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // FormSettingControlMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 229);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.tbLastControlMode);
            this.Controls.Add(this.lblLastControlMode);
            this.Controls.Add(this.gbControlMode);
            this.Name = "FormSettingControlMode";
            this.Text = "Setting Control Mode";
            this.Load += new System.EventHandler(this.FormSettingControlMode_Load);
            this.gbControlMode.ResumeLayout(false);
            this.gbControlMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbControlMode;
        private System.Windows.Forms.RadioButton rbLocalOnly;
        private System.Windows.Forms.RadioButton rbRemoteFirst;
        private System.Windows.Forms.Label lblLastControlMode;
        private System.Windows.Forms.TextBox tbLastControlMode;
        private System.Windows.Forms.Button btnApply;
    }
}