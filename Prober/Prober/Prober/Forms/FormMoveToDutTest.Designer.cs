namespace Prober.Forms
{
    partial class FormMoveToDutTest
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
            this.label1 = new System.Windows.Forms.Label();
            this.txt_ReticleName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_SubDieNo = new System.Windows.Forms.TextBox();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "ReticleName";
            // 
            // txt_ReticleName
            // 
            this.txt_ReticleName.Location = new System.Drawing.Point(149, 53);
            this.txt_ReticleName.Name = "txt_ReticleName";
            this.txt_ReticleName.Size = new System.Drawing.Size(100, 21);
            this.txt_ReticleName.TabIndex = 1;
            this.txt_ReticleName.Text = "F05";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "SubDieNo";
            // 
            // txt_SubDieNo
            // 
            this.txt_SubDieNo.Location = new System.Drawing.Point(149, 88);
            this.txt_SubDieNo.Name = "txt_SubDieNo";
            this.txt_SubDieNo.Size = new System.Drawing.Size(100, 21);
            this.txt_SubDieNo.TabIndex = 2;
            this.txt_SubDieNo.Text = "D1";
            // 
            // btn_OK
            // 
            this.btn_OK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OK.Image = global::Prober.Properties.Resources.勾选;
            this.btn_OK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_OK.Location = new System.Drawing.Point(55, 156);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(78, 37);
            this.btn_OK.TabIndex = 3;
            this.btn_OK.Text = "确认";
            this.btn_OK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_Move_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.Image = global::Prober.Properties.Resources.删除2;
            this.btn_Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Cancel.Location = new System.Drawing.Point(181, 156);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(78, 37);
            this.btn_Cancel.TabIndex = 3;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // FormMoveToDutTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 219);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.txt_SubDieNo);
            this.Controls.Add(this.txt_ReticleName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormMoveToDutTest";
            this.Text = "FormMoveToDutTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_ReticleName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_SubDieNo;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Cancel;
    }
}