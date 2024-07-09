namespace Prober.Forms
{
    partial class FormSelectWaferMark
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectWaferMark));
            this.hw_Base = new HalconDotNet.HWindowControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_Rectangle = new System.Windows.Forms.Button();
            this.btn_Ok = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hw_Base
            // 
            this.hw_Base.BackColor = System.Drawing.Color.Black;
            this.hw_Base.BorderColor = System.Drawing.Color.Black;
            this.hw_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hw_Base.ImagePart = new System.Drawing.Rectangle(0, 0, 5496, 3672);
            this.hw_Base.Location = new System.Drawing.Point(0, 0);
            this.hw_Base.Name = "hw_Base";
            this.hw_Base.Size = new System.Drawing.Size(836, 659);
            this.hw_Base.TabIndex = 0;
            this.hw_Base.WindowSize = new System.Drawing.Size(836, 659);
            this.hw_Base.HMouseWheel += new HalconDotNet.HMouseEventHandler(this.hw_Base_HMouseWheel);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.hw_Base);
            this.panel1.Location = new System.Drawing.Point(4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(836, 659);
            this.panel1.TabIndex = 1;
            // 
            // btn_Rectangle
            // 
            this.btn_Rectangle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Rectangle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Rectangle.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Rectangle.Image = global::Prober.Properties.Resources.矩形;
            this.btn_Rectangle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Rectangle.Location = new System.Drawing.Point(863, 80);
            this.btn_Rectangle.Name = "btn_Rectangle";
            this.btn_Rectangle.Size = new System.Drawing.Size(103, 34);
            this.btn_Rectangle.TabIndex = 2;
            this.btn_Rectangle.Text = "选取Mark";
            this.btn_Rectangle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Rectangle.UseVisualStyleBackColor = true;
            this.btn_Rectangle.Click += new System.EventHandler(this.btn_Rectangle_Click);
            // 
            // btn_Ok
            // 
            this.btn_Ok.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Ok.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Ok.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Ok.Image = global::Prober.Properties.Resources.勾选;
            this.btn_Ok.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Ok.Location = new System.Drawing.Point(863, 134);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(103, 34);
            this.btn_Ok.TabIndex = 2;
            this.btn_Ok.Text = "确认";
            this.btn_Ok.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Ok.UseVisualStyleBackColor = true;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Close.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Close.Image = global::Prober.Properties.Resources.删除2;
            this.btn_Close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Close.Location = new System.Drawing.Point(863, 194);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(103, 34);
            this.btn_Close.TabIndex = 2;
            this.btn_Close.Text = "取消";
            this.btn_Close.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // FormSelectWaferMark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(988, 668);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.btn_Ok);
            this.Controls.Add(this.btn_Rectangle);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSelectWaferMark";
            this.Text = "Mark特征选择对话框";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSelectWaferMark_FormClosing);
            this.Load += new System.EventHandler(this.FrmSelectWaferMark_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private HalconDotNet.HWindowControl hw_Base;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_Rectangle;
        private System.Windows.Forms.Button btn_Ok;
        private System.Windows.Forms.Button btn_Close;
    }
}