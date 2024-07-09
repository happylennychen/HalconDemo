namespace Prober.Forms
{
    partial class FormOtherDisplay
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
            this.txt_TempSet = new System.Windows.Forms.TextBox();
            this.txt_TempRead = new System.Windows.Forms.TextBox();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.chb_CCDGoWithChuck = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "设置温度(℃)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "上报温度(℃)";
            // 
            // txt_TempSet
            // 
            this.txt_TempSet.Location = new System.Drawing.Point(107, 37);
            this.txt_TempSet.Name = "txt_TempSet";
            this.txt_TempSet.ReadOnly = true;
            this.txt_TempSet.Size = new System.Drawing.Size(48, 21);
            this.txt_TempSet.TabIndex = 1;
            // 
            // txt_TempRead
            // 
            this.txt_TempRead.Location = new System.Drawing.Point(107, 64);
            this.txt_TempRead.Name = "txt_TempRead";
            this.txt_TempRead.ReadOnly = true;
            this.txt_TempRead.Size = new System.Drawing.Size(48, 21);
            this.txt_TempRead.TabIndex = 1;
            // 
            // btn_Stop
            // 
            this.btn_Stop.BackColor = System.Drawing.Color.Red;
            this.btn_Stop.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Stop.Location = new System.Drawing.Point(177, 13);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(91, 72);
            this.btn_Stop.TabIndex = 2;
            this.btn_Stop.Text = "停止";
            this.btn_Stop.UseVisualStyleBackColor = false;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // chb_CCDGoWithChuck
            // 
            this.chb_CCDGoWithChuck.AutoSize = true;
            this.chb_CCDGoWithChuck.Location = new System.Drawing.Point(23, 13);
            this.chb_CCDGoWithChuck.Name = "chb_CCDGoWithChuck";
            this.chb_CCDGoWithChuck.Size = new System.Drawing.Size(132, 16);
            this.chb_CCDGoWithChuck.TabIndex = 3;
            this.chb_CCDGoWithChuck.Text = "Chuck和CCD同步运动";
            this.chb_CCDGoWithChuck.UseVisualStyleBackColor = true;
            this.chb_CCDGoWithChuck.CheckedChanged += new System.EventHandler(this.chb_CCDGoWithChuck_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chb_CCDGoWithChuck);
            this.panel1.Controls.Add(this.btn_Stop);
            this.panel1.Controls.Add(this.txt_TempRead);
            this.panel1.Controls.Add(this.txt_TempSet);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(13, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(281, 96);
            this.panel1.TabIndex = 4;
            // 
            // FormOtherDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 101);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormOtherDisplay";
            this.Text = "FormOtherDisplay";
            this.Load += new System.EventHandler(this.FormOtherDisplay_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_TempSet;
        private System.Windows.Forms.TextBox txt_TempRead;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.CheckBox chb_CCDGoWithChuck;
        private System.Windows.Forms.Panel panel1;
    }
}