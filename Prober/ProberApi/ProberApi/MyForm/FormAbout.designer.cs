namespace ProberApi.MyForm {
    partial class FormAbout {
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
            this.lblTitleProduct = new System.Windows.Forms.Label();
            this.lblTitleSoftwareVersion = new System.Windows.Forms.Label();
            this.tbSoftwareVersion = new System.Windows.Forms.TextBox();
            this.lblTitleCopyright = new System.Windows.Forms.Label();
            this.tbCopyright = new System.Windows.Forms.TextBox();
            this.tbProduct = new System.Windows.Forms.TextBox();
            this.tbSwFrameworkVersion = new System.Windows.Forms.TextBox();
            this.lblTitleSwFrameworkVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitleProduct
            // 
            this.lblTitleProduct.Location = new System.Drawing.Point(21, 17);
            this.lblTitleProduct.Name = "lblTitleProduct";
            this.lblTitleProduct.Size = new System.Drawing.Size(159, 18);
            this.lblTitleProduct.TabIndex = 0;
            this.lblTitleProduct.Text = "Product";
            this.lblTitleProduct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTitleSoftwareVersion
            // 
            this.lblTitleSoftwareVersion.Location = new System.Drawing.Point(25, 80);
            this.lblTitleSoftwareVersion.Name = "lblTitleSoftwareVersion";
            this.lblTitleSoftwareVersion.Size = new System.Drawing.Size(155, 18);
            this.lblTitleSoftwareVersion.TabIndex = 1;
            this.lblTitleSoftwareVersion.Text = "Software Version";
            this.lblTitleSoftwareVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbSoftwareVersion
            // 
            this.tbSoftwareVersion.Location = new System.Drawing.Point(186, 77);
            this.tbSoftwareVersion.Name = "tbSoftwareVersion";
            this.tbSoftwareVersion.Size = new System.Drawing.Size(218, 21);
            this.tbSoftwareVersion.TabIndex = 2;
            // 
            // lblTitleCopyright
            // 
            this.lblTitleCopyright.Location = new System.Drawing.Point(23, 111);
            this.lblTitleCopyright.Name = "lblTitleCopyright";
            this.lblTitleCopyright.Size = new System.Drawing.Size(157, 18);
            this.lblTitleCopyright.TabIndex = 3;
            this.lblTitleCopyright.Text = "Copyright";
            this.lblTitleCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbCopyright
            // 
            this.tbCopyright.Location = new System.Drawing.Point(186, 108);
            this.tbCopyright.Name = "tbCopyright";
            this.tbCopyright.Size = new System.Drawing.Size(218, 21);
            this.tbCopyright.TabIndex = 4;
            // 
            // tbProduct
            // 
            this.tbProduct.Location = new System.Drawing.Point(186, 14);
            this.tbProduct.Name = "tbProduct";
            this.tbProduct.Size = new System.Drawing.Size(218, 21);
            this.tbProduct.TabIndex = 5;
            // 
            // tbSwFrameworkVersion
            // 
            this.tbSwFrameworkVersion.Location = new System.Drawing.Point(186, 45);
            this.tbSwFrameworkVersion.Name = "tbSwFrameworkVersion";
            this.tbSwFrameworkVersion.Size = new System.Drawing.Size(218, 21);
            this.tbSwFrameworkVersion.TabIndex = 6;
            // 
            // lblTitleSwFrameworkVersion
            // 
            this.lblTitleSwFrameworkVersion.Location = new System.Drawing.Point(23, 48);
            this.lblTitleSwFrameworkVersion.Name = "lblTitleSwFrameworkVersion";
            this.lblTitleSwFrameworkVersion.Size = new System.Drawing.Size(157, 18);
            this.lblTitleSwFrameworkVersion.TabIndex = 7;
            this.lblTitleSwFrameworkVersion.Text = "Software Framework Version";
            this.lblTitleSwFrameworkVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 139);
            this.Controls.Add(this.lblTitleSwFrameworkVersion);
            this.Controls.Add(this.tbSwFrameworkVersion);
            this.Controls.Add(this.tbProduct);
            this.Controls.Add(this.tbCopyright);
            this.Controls.Add(this.lblTitleCopyright);
            this.Controls.Add(this.tbSoftwareVersion);
            this.Controls.Add(this.lblTitleSoftwareVersion);
            this.Controls.Add(this.lblTitleProduct);
            this.Name = "FormAbout";
            this.Text = "About";
            this.Load += new System.EventHandler(this.FormAbout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitleProduct;
        private System.Windows.Forms.Label lblTitleSoftwareVersion;
        private System.Windows.Forms.TextBox tbSoftwareVersion;
        private System.Windows.Forms.Label lblTitleCopyright;
        private System.Windows.Forms.TextBox tbCopyright;
        private System.Windows.Forms.TextBox tbProduct;
        private System.Windows.Forms.TextBox tbSwFrameworkVersion;
        private System.Windows.Forms.Label lblTitleSwFrameworkVersion;
    }
}