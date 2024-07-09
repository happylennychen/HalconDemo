namespace Prober.Forms
{
    partial class FormDebug
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDebug));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.page_PowerMonitor = new System.Windows.Forms.TabPage();
            this.page_TempMonitor = new System.Windows.Forms.TabPage();
            this.page_HeightMonitor = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.page_PowerMonitor);
            this.tabControl1.Controls.Add(this.page_TempMonitor);
            this.tabControl1.Controls.Add(this.page_HeightMonitor);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1326, 902);
            this.tabControl1.TabIndex = 0;
            // 
            // page_PowerMonitor
            // 
            this.page_PowerMonitor.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.page_PowerMonitor.Location = new System.Drawing.Point(4, 26);
            this.page_PowerMonitor.Name = "page_PowerMonitor";
            this.page_PowerMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.page_PowerMonitor.Size = new System.Drawing.Size(1318, 872);
            this.page_PowerMonitor.TabIndex = 0;
            this.page_PowerMonitor.Text = "光功率监控";
            this.page_PowerMonitor.UseVisualStyleBackColor = true;
            // 
            // page_TempMonitor
            // 
            this.page_TempMonitor.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.page_TempMonitor.Location = new System.Drawing.Point(4, 26);
            this.page_TempMonitor.Name = "page_TempMonitor";
            this.page_TempMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.page_TempMonitor.Size = new System.Drawing.Size(1162, 872);
            this.page_TempMonitor.TabIndex = 1;
            this.page_TempMonitor.Text = "温度监控";
            this.page_TempMonitor.UseVisualStyleBackColor = true;
            // 
            // page_HeightMonitor
            // 
            this.page_HeightMonitor.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.page_HeightMonitor.Location = new System.Drawing.Point(4, 26);
            this.page_HeightMonitor.Name = "page_HeightMonitor";
            this.page_HeightMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.page_HeightMonitor.Size = new System.Drawing.Size(1162, 872);
            this.page_HeightMonitor.TabIndex = 2;
            this.page_HeightMonitor.Text = "高度监控";
            this.page_HeightMonitor.UseVisualStyleBackColor = true;
            // 
            // FormDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1326, 902);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDebug";
            this.Text = "辅助功能";
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage page_PowerMonitor;
        private System.Windows.Forms.TabPage page_TempMonitor;
        private System.Windows.Forms.TabPage page_HeightMonitor;
    }
}