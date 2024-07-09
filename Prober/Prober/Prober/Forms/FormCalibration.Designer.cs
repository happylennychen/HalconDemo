namespace Prober.Forms
{
    partial class FormCalibration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCalibration));
            this.tabCtrl_SystemCalibration = new System.Windows.Forms.TabControl();
            this.page_WorkPlace = new System.Windows.Forms.TabPage();
            this.page_VisionCamera = new System.Windows.Forms.TabPage();
            this.page_HeightCalibrate = new System.Windows.Forms.TabPage();
            this.Page_AngelCalibrate = new System.Windows.Forms.TabPage();
            this.Page_ProberClean = new System.Windows.Forms.TabPage();
            this.Page_HeightVerify = new System.Windows.Forms.TabPage();
            this.page_HeightScan = new System.Windows.Forms.TabPage();
            this.page_FaRolling = new System.Windows.Forms.TabPage();
            this.tabCtrl_SystemCalibration.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabCtrl_SystemCalibration
            // 
            this.tabCtrl_SystemCalibration.Controls.Add(this.page_WorkPlace);
            this.tabCtrl_SystemCalibration.Controls.Add(this.page_VisionCamera);
            this.tabCtrl_SystemCalibration.Controls.Add(this.page_HeightCalibrate);
            this.tabCtrl_SystemCalibration.Controls.Add(this.Page_AngelCalibrate);
            this.tabCtrl_SystemCalibration.Controls.Add(this.Page_ProberClean);
            this.tabCtrl_SystemCalibration.Controls.Add(this.Page_HeightVerify);
            this.tabCtrl_SystemCalibration.Controls.Add(this.page_HeightScan);
            this.tabCtrl_SystemCalibration.Controls.Add(this.page_FaRolling);
            this.tabCtrl_SystemCalibration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrl_SystemCalibration.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Bold);
            this.tabCtrl_SystemCalibration.Location = new System.Drawing.Point(0, 0);
            this.tabCtrl_SystemCalibration.Margin = new System.Windows.Forms.Padding(4);
            this.tabCtrl_SystemCalibration.Name = "tabCtrl_SystemCalibration";
            this.tabCtrl_SystemCalibration.SelectedIndex = 0;
            this.tabCtrl_SystemCalibration.Size = new System.Drawing.Size(1686, 839);
            this.tabCtrl_SystemCalibration.TabIndex = 0;
            // 
            // page_WorkPlace
            // 
            this.page_WorkPlace.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.page_WorkPlace.Location = new System.Drawing.Point(4, 26);
            this.page_WorkPlace.Margin = new System.Windows.Forms.Padding(4);
            this.page_WorkPlace.Name = "page_WorkPlace";
            this.page_WorkPlace.Padding = new System.Windows.Forms.Padding(4);
            this.page_WorkPlace.Size = new System.Drawing.Size(1678, 809);
            this.page_WorkPlace.TabIndex = 0;
            this.page_WorkPlace.Text = "工位设置";
            this.page_WorkPlace.UseVisualStyleBackColor = true;
            // 
            // page_VisionCamera
            // 
            this.page_VisionCamera.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.page_VisionCamera.Location = new System.Drawing.Point(4, 26);
            this.page_VisionCamera.Margin = new System.Windows.Forms.Padding(4);
            this.page_VisionCamera.Name = "page_VisionCamera";
            this.page_VisionCamera.Padding = new System.Windows.Forms.Padding(4);
            this.page_VisionCamera.Size = new System.Drawing.Size(1263, 809);
            this.page_VisionCamera.TabIndex = 1;
            this.page_VisionCamera.Text = "图像坐标系校准";
            this.page_VisionCamera.UseVisualStyleBackColor = true;
            // 
            // page_HeightCalibrate
            // 
            this.page_HeightCalibrate.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.page_HeightCalibrate.Location = new System.Drawing.Point(4, 26);
            this.page_HeightCalibrate.Name = "page_HeightCalibrate";
            this.page_HeightCalibrate.Padding = new System.Windows.Forms.Padding(3);
            this.page_HeightCalibrate.Size = new System.Drawing.Size(1263, 809);
            this.page_HeightCalibrate.TabIndex = 2;
            this.page_HeightCalibrate.Text = "高度扫描";
            this.page_HeightCalibrate.UseVisualStyleBackColor = true;
            // 
            // Page_AngelCalibrate
            // 
            this.Page_AngelCalibrate.Location = new System.Drawing.Point(4, 26);
            this.Page_AngelCalibrate.Name = "Page_AngelCalibrate";
            this.Page_AngelCalibrate.Padding = new System.Windows.Forms.Padding(3);
            this.Page_AngelCalibrate.Size = new System.Drawing.Size(1263, 809);
            this.Page_AngelCalibrate.TabIndex = 3;
            this.Page_AngelCalibrate.Text = "FA角度和高度位置标定";
            this.Page_AngelCalibrate.UseVisualStyleBackColor = true;
            // 
            // Page_ProberClean
            // 
            this.Page_ProberClean.Location = new System.Drawing.Point(4, 26);
            this.Page_ProberClean.Name = "Page_ProberClean";
            this.Page_ProberClean.Padding = new System.Windows.Forms.Padding(3);
            this.Page_ProberClean.Size = new System.Drawing.Size(1263, 809);
            this.Page_ProberClean.TabIndex = 4;
            this.Page_ProberClean.Text = "自动清针";
            this.Page_ProberClean.UseVisualStyleBackColor = true;
            // 
            // Page_HeightVerify
            // 
            this.Page_HeightVerify.Location = new System.Drawing.Point(4, 26);
            this.Page_HeightVerify.Name = "Page_HeightVerify";
            this.Page_HeightVerify.Padding = new System.Windows.Forms.Padding(3);
            this.Page_HeightVerify.Size = new System.Drawing.Size(1263, 809);
            this.Page_HeightVerify.TabIndex = 5;
            this.Page_HeightVerify.Text = "高度校验";
            this.Page_HeightVerify.UseVisualStyleBackColor = true;
            // 
            // page_HeightScan
            // 
            this.page_HeightScan.Location = new System.Drawing.Point(4, 26);
            this.page_HeightScan.Name = "page_HeightScan";
            this.page_HeightScan.Padding = new System.Windows.Forms.Padding(3);
            this.page_HeightScan.Size = new System.Drawing.Size(1263, 809);
            this.page_HeightScan.TabIndex = 6;
            this.page_HeightScan.Text = "高度扫描-定点";
            this.page_HeightScan.UseVisualStyleBackColor = true;
            // 
            // page_FaRolling
            // 
            this.page_FaRolling.Location = new System.Drawing.Point(4, 26);
            this.page_FaRolling.Name = "page_FaRolling";
            this.page_FaRolling.Padding = new System.Windows.Forms.Padding(3);
            this.page_FaRolling.Size = new System.Drawing.Size(1263, 809);
            this.page_FaRolling.TabIndex = 7;
            this.page_FaRolling.Text = "FA光轴角度标定";
            this.page_FaRolling.UseVisualStyleBackColor = true;
            // 
            // FormCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1686, 839);
            this.Controls.Add(this.tabCtrl_SystemCalibration);
            this.Font = new System.Drawing.Font("Times New Roman", 10.5F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormCalibration";
            this.Text = "机台校准";
            this.tabCtrl_SystemCalibration.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabCtrl_SystemCalibration;
        private System.Windows.Forms.TabPage page_WorkPlace;
        private System.Windows.Forms.TabPage page_VisionCamera;
        private System.Windows.Forms.TabPage page_HeightCalibrate;
        private System.Windows.Forms.TabPage Page_AngelCalibrate;
        private System.Windows.Forms.TabPage Page_ProberClean;
        private System.Windows.Forms.TabPage Page_HeightVerify;
        private System.Windows.Forms.TabPage page_HeightScan;
        private System.Windows.Forms.TabPage page_FaRolling;
    }
}