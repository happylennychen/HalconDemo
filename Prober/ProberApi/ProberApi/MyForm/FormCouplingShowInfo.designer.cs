namespace ProberApi.MyForm {
    partial class FormCouplingShowInfo {
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
            this.lblVisaResource = new System.Windows.Forms.Label();
            this.tbVisaResource = new System.Windows.Forms.TextBox();
            this.lblSlot = new System.Windows.Forms.Label();
            this.tbSlot = new System.Windows.Forms.TextBox();
            this.tbChannel = new System.Windows.Forms.TextBox();
            this.lblChannel = new System.Windows.Forms.Label();
            this.tbInstrumentCategory = new System.Windows.Forms.TextBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblVendorModel = new System.Windows.Forms.Label();
            this.tbVendorModel = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblVisaResource
            // 
            this.lblVisaResource.Location = new System.Drawing.Point(42, 51);
            this.lblVisaResource.Name = "lblVisaResource";
            this.lblVisaResource.Size = new System.Drawing.Size(139, 18);
            this.lblVisaResource.TabIndex = 0;
            this.lblVisaResource.Text = "Visa Resource";
            this.lblVisaResource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbVisaResource
            // 
            this.tbVisaResource.Location = new System.Drawing.Point(190, 48);
            this.tbVisaResource.Name = "tbVisaResource";
            this.tbVisaResource.Size = new System.Drawing.Size(436, 21);
            this.tbVisaResource.TabIndex = 1;
            // 
            // lblSlot
            // 
            this.lblSlot.Location = new System.Drawing.Point(42, 78);
            this.lblSlot.Name = "lblSlot";
            this.lblSlot.Size = new System.Drawing.Size(139, 18);
            this.lblSlot.TabIndex = 2;
            this.lblSlot.Text = "Slot";
            this.lblSlot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbSlot
            // 
            this.tbSlot.Location = new System.Drawing.Point(190, 75);
            this.tbSlot.Name = "tbSlot";
            this.tbSlot.Size = new System.Drawing.Size(100, 21);
            this.tbSlot.TabIndex = 3;
            // 
            // tbChannel
            // 
            this.tbChannel.Location = new System.Drawing.Point(190, 104);
            this.tbChannel.Name = "tbChannel";
            this.tbChannel.Size = new System.Drawing.Size(100, 21);
            this.tbChannel.TabIndex = 4;
            // 
            // lblChannel
            // 
            this.lblChannel.Location = new System.Drawing.Point(42, 107);
            this.lblChannel.Name = "lblChannel";
            this.lblChannel.Size = new System.Drawing.Size(139, 18);
            this.lblChannel.TabIndex = 5;
            this.lblChannel.Text = "Channel";
            this.lblChannel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbInstrumentCategory
            // 
            this.tbInstrumentCategory.Location = new System.Drawing.Point(190, 131);
            this.tbInstrumentCategory.Name = "tbInstrumentCategory";
            this.tbInstrumentCategory.Size = new System.Drawing.Size(100, 21);
            this.tbInstrumentCategory.TabIndex = 6;
            // 
            // lblCategory
            // 
            this.lblCategory.Location = new System.Drawing.Point(42, 134);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(139, 18);
            this.lblCategory.TabIndex = 7;
            this.lblCategory.Text = "Category";
            this.lblCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVendorModel
            // 
            this.lblVendorModel.Location = new System.Drawing.Point(40, 24);
            this.lblVendorModel.Name = "lblVendorModel";
            this.lblVendorModel.Size = new System.Drawing.Size(141, 18);
            this.lblVendorModel.TabIndex = 8;
            this.lblVendorModel.Text = "Vendor && Model";
            this.lblVendorModel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbVendorModel
            // 
            this.tbVendorModel.Location = new System.Drawing.Point(190, 21);
            this.tbVendorModel.Name = "tbVendorModel";
            this.tbVendorModel.Size = new System.Drawing.Size(436, 21);
            this.tbVendorModel.TabIndex = 9;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(526, 132);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormCouplingShowInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 168);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tbVendorModel);
            this.Controls.Add(this.lblVendorModel);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.tbInstrumentCategory);
            this.Controls.Add(this.lblChannel);
            this.Controls.Add(this.tbChannel);
            this.Controls.Add(this.tbSlot);
            this.Controls.Add(this.lblSlot);
            this.Controls.Add(this.tbVisaResource);
            this.Controls.Add(this.lblVisaResource);
            this.Name = "FormCouplingShowInfo";
            this.Load += new System.EventHandler(this.FormCouplingShowCouplingInInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblVisaResource;
        private System.Windows.Forms.TextBox tbVisaResource;
        private System.Windows.Forms.Label lblSlot;
        private System.Windows.Forms.TextBox tbSlot;
        private System.Windows.Forms.TextBox tbChannel;
        private System.Windows.Forms.Label lblChannel;
        private System.Windows.Forms.TextBox tbInstrumentCategory;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblVendorModel;
        private System.Windows.Forms.TextBox tbVendorModel;
        private System.Windows.Forms.Button btnClose;
    }
}