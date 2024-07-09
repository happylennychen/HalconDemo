namespace Prober.Forms
{
    partial class FormFARolling
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
            this.num_SwitchPort1 = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_OsAddr = new System.Windows.Forms.TextBox();
            this.num_SwitchPort2 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_StartRolling = new System.Windows.Forms.Button();
            this.rtxt_Info = new System.Windows.Forms.RichTextBox();
            this.btn_StopRolling = new System.Windows.Forms.Button();
            this.chb_IsSingleU = new System.Windows.Forms.CheckBox();
            this.txt_1 = new System.Windows.Forms.Label();
            this.txt_OpticalChannelGap = new System.Windows.Forms.TextBox();
            this.chb_HDir = new System.Windows.Forms.CheckBox();
            this.chb_IsRightFA = new System.Windows.Forms.CheckBox();
            this.chb_IsUseSwitch = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.num_SwitchPort1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_SwitchPort2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "光功率计通道1对应光开关通道";
            // 
            // num_SwitchPort1
            // 
            this.num_SwitchPort1.Location = new System.Drawing.Point(240, 62);
            this.num_SwitchPort1.Name = "num_SwitchPort1";
            this.num_SwitchPort1.Size = new System.Drawing.Size(132, 21);
            this.num_SwitchPort1.TabIndex = 1;
            this.num_SwitchPort1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.num_SwitchPort1.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt_OsAddr);
            this.groupBox2.Controls.Add(this.num_SwitchPort2);
            this.groupBox2.Controls.Add(this.num_SwitchPort1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(39, 45);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(409, 140);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // txt_OsAddr
            // 
            this.txt_OsAddr.Location = new System.Drawing.Point(107, 26);
            this.txt_OsAddr.Name = "txt_OsAddr";
            this.txt_OsAddr.Size = new System.Drawing.Size(265, 21);
            this.txt_OsAddr.TabIndex = 2;
            this.txt_OsAddr.Text = "GPIB0::12::INSTR";
            this.txt_OsAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // num_SwitchPort2
            // 
            this.num_SwitchPort2.Location = new System.Drawing.Point(240, 104);
            this.num_SwitchPort2.Name = "num_SwitchPort2";
            this.num_SwitchPort2.Size = new System.Drawing.Size(132, 21);
            this.num_SwitchPort2.TabIndex = 1;
            this.num_SwitchPort2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.num_SwitchPort2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(167, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "光功率计通道2对应光开关通道";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "光开关地址";
            // 
            // btn_StartRolling
            // 
            this.btn_StartRolling.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_StartRolling.Location = new System.Drawing.Point(469, 79);
            this.btn_StartRolling.Name = "btn_StartRolling";
            this.btn_StartRolling.Size = new System.Drawing.Size(83, 41);
            this.btn_StartRolling.TabIndex = 3;
            this.btn_StartRolling.Text = "Start Rolling";
            this.btn_StartRolling.UseVisualStyleBackColor = false;
            this.btn_StartRolling.Click += new System.EventHandler(this.btn_StartRolling_Click);
            // 
            // rtxt_Info
            // 
            this.rtxt_Info.Location = new System.Drawing.Point(39, 217);
            this.rtxt_Info.Name = "rtxt_Info";
            this.rtxt_Info.Size = new System.Drawing.Size(616, 215);
            this.rtxt_Info.TabIndex = 4;
            this.rtxt_Info.Text = "";
            this.rtxt_Info.TextChanged += new System.EventHandler(this.rtbMsgBox_TextChanged);
            // 
            // btn_StopRolling
            // 
            this.btn_StopRolling.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_StopRolling.Location = new System.Drawing.Point(572, 79);
            this.btn_StopRolling.Name = "btn_StopRolling";
            this.btn_StopRolling.Size = new System.Drawing.Size(83, 41);
            this.btn_StopRolling.TabIndex = 3;
            this.btn_StopRolling.Text = "Stop Rolling";
            this.btn_StopRolling.UseVisualStyleBackColor = false;
            this.btn_StopRolling.Click += new System.EventHandler(this.btn_StopRolling_Click);
            // 
            // chb_IsSingleU
            // 
            this.chb_IsSingleU.AutoSize = true;
            this.chb_IsSingleU.Location = new System.Drawing.Point(473, 167);
            this.chb_IsSingleU.Name = "chb_IsSingleU";
            this.chb_IsSingleU.Size = new System.Drawing.Size(36, 16);
            this.chb_IsSingleU.TabIndex = 5;
            this.chb_IsSingleU.Text = "1U";
            this.chb_IsSingleU.UseVisualStyleBackColor = true;
            this.chb_IsSingleU.Visible = false;
            this.chb_IsSingleU.CheckedChanged += new System.EventHandler(this.chb_IsSingleU_CheckedChanged);
            // 
            // txt_1
            // 
            this.txt_1.AutoSize = true;
            this.txt_1.Location = new System.Drawing.Point(472, 56);
            this.txt_1.Name = "txt_1";
            this.txt_1.Size = new System.Drawing.Size(89, 12);
            this.txt_1.TabIndex = 0;
            this.txt_1.Text = "光口距离（um）";
            // 
            // txt_OpticalChannelGap
            // 
            this.txt_OpticalChannelGap.Location = new System.Drawing.Point(572, 52);
            this.txt_OpticalChannelGap.Name = "txt_OpticalChannelGap";
            this.txt_OpticalChannelGap.Size = new System.Drawing.Size(83, 21);
            this.txt_OpticalChannelGap.TabIndex = 6;
            this.txt_OpticalChannelGap.Text = "1750";
            // 
            // chb_HDir
            // 
            this.chb_HDir.AutoSize = true;
            this.chb_HDir.Location = new System.Drawing.Point(571, 168);
            this.chb_HDir.Name = "chb_HDir";
            this.chb_HDir.Size = new System.Drawing.Size(72, 16);
            this.chb_HDir.TabIndex = 7;
            this.chb_HDir.Text = "水平方向";
            this.chb_HDir.UseVisualStyleBackColor = true;
            // 
            // chb_IsRightFA
            // 
            this.chb_IsRightFA.AutoSize = true;
            this.chb_IsRightFA.Location = new System.Drawing.Point(473, 145);
            this.chb_IsRightFA.Name = "chb_IsRightFA";
            this.chb_IsRightFA.Size = new System.Drawing.Size(60, 16);
            this.chb_IsRightFA.TabIndex = 8;
            this.chb_IsRightFA.Text = "右侧FA";
            this.chb_IsRightFA.UseVisualStyleBackColor = true;
            // 
            // chb_IsUseSwitch
            // 
            this.chb_IsUseSwitch.AutoSize = true;
            this.chb_IsUseSwitch.Location = new System.Drawing.Point(571, 146);
            this.chb_IsUseSwitch.Name = "chb_IsUseSwitch";
            this.chb_IsUseSwitch.Size = new System.Drawing.Size(84, 16);
            this.chb_IsUseSwitch.TabIndex = 9;
            this.chb_IsUseSwitch.Text = "使用光开关";
            this.chb_IsUseSwitch.UseVisualStyleBackColor = true;
            // 
            // FormFARolling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 474);
            this.Controls.Add(this.chb_IsUseSwitch);
            this.Controls.Add(this.chb_IsRightFA);
            this.Controls.Add(this.chb_HDir);
            this.Controls.Add(this.txt_OpticalChannelGap);
            this.Controls.Add(this.chb_IsSingleU);
            this.Controls.Add(this.rtxt_Info);
            this.Controls.Add(this.btn_StopRolling);
            this.Controls.Add(this.btn_StartRolling);
            this.Controls.Add(this.txt_1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormFARolling";
            this.Text = "FormFARolling";
            this.Load += new System.EventHandler(this.FormFARolling_Load);
            ((System.ComponentModel.ISupportInitialize)(this.num_SwitchPort1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_SwitchPort2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown num_SwitchPort1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown num_SwitchPort2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_StartRolling;
        private System.Windows.Forms.RichTextBox rtxt_Info;
        private System.Windows.Forms.Button btn_StopRolling;
        private System.Windows.Forms.TextBox txt_OsAddr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chb_IsSingleU;
        private System.Windows.Forms.Label txt_1;
        private System.Windows.Forms.TextBox txt_OpticalChannelGap;
        private System.Windows.Forms.CheckBox chb_HDir;
        private System.Windows.Forms.CheckBox chb_IsRightFA;
        private System.Windows.Forms.CheckBox chb_IsUseSwitch;
    }
}