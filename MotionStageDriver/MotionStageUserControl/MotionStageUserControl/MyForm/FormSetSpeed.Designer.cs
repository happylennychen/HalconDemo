namespace MyMotionStageUserControl.MyForm {
    partial class FormSetSpeed {
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
            this.lblSpeed = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.cmbSpeed = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(12, 15);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(35, 12);
            this.lblSpeed.TabIndex = 0;
            this.lblSpeed.Text = "Speed";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(269, 10);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // cmbSpeed
            // 
            this.cmbSpeed.FormattingEnabled = true;
            this.cmbSpeed.Location = new System.Drawing.Point(53, 12);
            this.cmbSpeed.Name = "cmbSpeed";
            this.cmbSpeed.Size = new System.Drawing.Size(129, 20);
            this.cmbSpeed.TabIndex = 4;
            // 
            // FormSetSpeed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 45);
            this.Controls.Add(this.cmbSpeed);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblSpeed);
            this.Name = "FormSetSpeed";
            this.Text = "Set Speed";
            this.Load += new System.EventHandler(this.FormSetSpeed_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ComboBox cmbSpeed;
    }
}