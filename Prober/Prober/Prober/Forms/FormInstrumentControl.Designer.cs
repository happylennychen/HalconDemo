namespace Prober.Forms
{
    partial class FormInstrumentControl
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
            this.tabCtrl_Instrument = new System.Windows.Forms.TabControl();
            this.page_IOControl = new System.Windows.Forms.TabPage();
            this.page_TempControl = new System.Windows.Forms.TabPage();
            this.page_PmSetting = new System.Windows.Forms.TabPage();
            this.tabCtrl_Instrument.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabCtrl_Instrument
            // 
            this.tabCtrl_Instrument.Controls.Add(this.page_IOControl);
            this.tabCtrl_Instrument.Controls.Add(this.page_TempControl);
            this.tabCtrl_Instrument.Controls.Add(this.page_PmSetting);
            this.tabCtrl_Instrument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrl_Instrument.Location = new System.Drawing.Point(0, 0);
            this.tabCtrl_Instrument.Name = "tabCtrl_Instrument";
            this.tabCtrl_Instrument.SelectedIndex = 0;
            this.tabCtrl_Instrument.Size = new System.Drawing.Size(969, 584);
            this.tabCtrl_Instrument.TabIndex = 0;
            // 
            // page_IOControl
            // 
            this.page_IOControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.page_IOControl.Location = new System.Drawing.Point(4, 22);
            this.page_IOControl.Name = "page_IOControl";
            this.page_IOControl.Padding = new System.Windows.Forms.Padding(3);
            this.page_IOControl.Size = new System.Drawing.Size(961, 558);
            this.page_IOControl.TabIndex = 0;
            this.page_IOControl.Text = "IO控制";
            this.page_IOControl.UseVisualStyleBackColor = true;
            // 
            // page_TempControl
            // 
            this.page_TempControl.Location = new System.Drawing.Point(4, 22);
            this.page_TempControl.Name = "page_TempControl";
            this.page_TempControl.Padding = new System.Windows.Forms.Padding(3);
            this.page_TempControl.Size = new System.Drawing.Size(961, 558);
            this.page_TempControl.TabIndex = 1;
            this.page_TempControl.Text = "温度控制";
            this.page_TempControl.UseVisualStyleBackColor = true;
            // 
            // page_PmSetting
            // 
            this.page_PmSetting.Location = new System.Drawing.Point(4, 22);
            this.page_PmSetting.Name = "page_PmSetting";
            this.page_PmSetting.Padding = new System.Windows.Forms.Padding(3);
            this.page_PmSetting.Size = new System.Drawing.Size(961, 558);
            this.page_PmSetting.TabIndex = 2;
            this.page_PmSetting.Text = "光功率计设置";
            this.page_PmSetting.UseVisualStyleBackColor = true;
            // 
            // FormInstrumentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 584);
            this.Controls.Add(this.tabCtrl_Instrument);
            this.Name = "FormInstrumentControl";
            this.Text = "设备控制";
            this.Load += new System.EventHandler(this.FormInstrumentControl_Load);
            this.tabCtrl_Instrument.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabCtrl_Instrument;
        private System.Windows.Forms.TabPage page_IOControl;
        private System.Windows.Forms.TabPage page_TempControl;
        private System.Windows.Forms.TabPage page_PmSetting;
    }
}