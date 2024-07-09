namespace Prober.Forms
{
    partial class FormOtherSetting
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
            this.chb_EnableChuckSafePosMonitor = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chb_EnableChuckSafePosMonitor
            // 
            this.chb_EnableChuckSafePosMonitor.AutoSize = true;
            this.chb_EnableChuckSafePosMonitor.Checked = true;
            this.chb_EnableChuckSafePosMonitor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_EnableChuckSafePosMonitor.Location = new System.Drawing.Point(114, 139);
            this.chb_EnableChuckSafePosMonitor.Name = "chb_EnableChuckSafePosMonitor";
            this.chb_EnableChuckSafePosMonitor.Size = new System.Drawing.Size(144, 16);
            this.chb_EnableChuckSafePosMonitor.TabIndex = 0;
            this.chb_EnableChuckSafePosMonitor.Text = "托盘安全高度监控使能";
            this.chb_EnableChuckSafePosMonitor.UseVisualStyleBackColor = true;
            this.chb_EnableChuckSafePosMonitor.CheckedChanged += new System.EventHandler(this.chb_EnableChuckSafePosMonitor_CheckedChanged);
            // 
            // FormOtherSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 421);
            this.Controls.Add(this.chb_EnableChuckSafePosMonitor);
            this.Name = "FormOtherSetting";
            this.Text = "其他设置";
            this.Load += new System.EventHandler(this.FormOtherSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chb_EnableChuckSafePosMonitor;
    }
}