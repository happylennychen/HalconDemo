namespace Prober.Forms
{
    partial class FormHeightDisplay
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
            this.label2 = new System.Windows.Forms.Label();
            this.txt_HeightLeft_Laser = new System.Windows.Forms.TextBox();
            this.txt_HeightLeft_Cap = new System.Windows.Forms.TextBox();
            this.txt_HeightRight_Laser = new System.Windows.Forms.TextBox();
            this.txt_HeightRight_Cap = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "左侧Fiber高度(um)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "右侧Fiber高度(um)";
            // 
            // txt_HeightLeft_Laser
            // 
            this.txt_HeightLeft_Laser.Location = new System.Drawing.Point(123, 39);
            this.txt_HeightLeft_Laser.Name = "txt_HeightLeft_Laser";
            this.txt_HeightLeft_Laser.ReadOnly = true;
            this.txt_HeightLeft_Laser.Size = new System.Drawing.Size(87, 21);
            this.txt_HeightLeft_Laser.TabIndex = 1;
            // 
            // txt_HeightLeft_Cap
            // 
            this.txt_HeightLeft_Cap.Location = new System.Drawing.Point(216, 39);
            this.txt_HeightLeft_Cap.Name = "txt_HeightLeft_Cap";
            this.txt_HeightLeft_Cap.ReadOnly = true;
            this.txt_HeightLeft_Cap.Size = new System.Drawing.Size(87, 21);
            this.txt_HeightLeft_Cap.TabIndex = 1;
            // 
            // txt_HeightRight_Laser
            // 
            this.txt_HeightRight_Laser.Location = new System.Drawing.Point(123, 66);
            this.txt_HeightRight_Laser.Name = "txt_HeightRight_Laser";
            this.txt_HeightRight_Laser.ReadOnly = true;
            this.txt_HeightRight_Laser.Size = new System.Drawing.Size(87, 21);
            this.txt_HeightRight_Laser.TabIndex = 1;
            // 
            // txt_HeightRight_Cap
            // 
            this.txt_HeightRight_Cap.Location = new System.Drawing.Point(216, 66);
            this.txt_HeightRight_Cap.Name = "txt_HeightRight_Cap";
            this.txt_HeightRight_Cap.ReadOnly = true;
            this.txt_HeightRight_Cap.Size = new System.Drawing.Size(87, 21);
            this.txt_HeightRight_Cap.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(136, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "激光测高仪";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(227, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "电容测高仪";
            // 
            // FormHeightDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 101);
            this.Controls.Add(this.txt_HeightRight_Cap);
            this.Controls.Add(this.txt_HeightRight_Laser);
            this.Controls.Add(this.txt_HeightLeft_Cap);
            this.Controls.Add(this.txt_HeightLeft_Laser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormHeightDisplay";
            this.Text = "FormHeightDisplay";
            this.Load += new System.EventHandler(this.FormHeightDisplay_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_HeightLeft_Laser;
        private System.Windows.Forms.TextBox txt_HeightLeft_Cap;
        private System.Windows.Forms.TextBox txt_HeightRight_Laser;
        private System.Windows.Forms.TextBox txt_HeightRight_Cap;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}