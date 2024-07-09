namespace Prober
{
    partial class FormSpecialPositionEdit
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
            this.btn_Load = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_MoveLeftFA = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_ChuckX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_ChuckY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_ChuckZ = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_ChuckSZ = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_LeftX = new System.Windows.Forms.TextBox();
            this.txt_LeftY = new System.Windows.Forms.TextBox();
            this.txt_LeftZ = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_LeftSX = new System.Windows.Forms.TextBox();
            this.txt_LeftSY = new System.Windows.Forms.TextBox();
            this.txt_LeftSZ = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txt_RightX = new System.Windows.Forms.TextBox();
            this.txt_RightSX = new System.Windows.Forms.TextBox();
            this.txt_RightY = new System.Windows.Forms.TextBox();
            this.txt_RightSY = new System.Windows.Forms.TextBox();
            this.txt_RightZ = new System.Windows.Forms.TextBox();
            this.txt_RightSZ = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txt_CcdX = new System.Windows.Forms.TextBox();
            this.txt_CcdY = new System.Windows.Forms.TextBox();
            this.txt_CcdZ = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txt_AltimeterU = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txt_filePath = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.panelSpecialPos = new System.Windows.Forms.Panel();
            this.btn_MoveRightFA = new System.Windows.Forms.Button();
            this.btn_MoveChuck = new System.Windows.Forms.Button();
            this.btn_MoveCCD = new System.Windows.Forms.Button();
            this.btn_MoveHeightU = new System.Windows.Forms.Button();
            this.panelSpecialPos.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Load
            // 
            this.btn_Load.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_Load.Location = new System.Drawing.Point(787, 36);
            this.btn_Load.Name = "btn_Load";
            this.btn_Load.Size = new System.Drawing.Size(130, 77);
            this.btn_Load.TabIndex = 0;
            this.btn_Load.Text = "加载历史位置";
            this.btn_Load.UseVisualStyleBackColor = false;
            this.btn_Load.Click += new System.EventHandler(this.btn_Load_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_Save.Location = new System.Drawing.Point(972, 36);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(130, 77);
            this.btn_Save.TabIndex = 1;
            this.btn_Save.Text = "保存当前位置";
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_MoveLeftFA
            // 
            this.btn_MoveLeftFA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_MoveLeftFA.Location = new System.Drawing.Point(787, 202);
            this.btn_MoveLeftFA.Name = "btn_MoveLeftFA";
            this.btn_MoveLeftFA.Size = new System.Drawing.Size(130, 41);
            this.btn_MoveLeftFA.TabIndex = 2;
            this.btn_MoveLeftFA.Text = "左FA运动到选定位置";
            this.btn_MoveLeftFA.UseVisualStyleBackColor = false;
            this.btn_MoveLeftFA.Click += new System.EventHandler(this.btn_Move_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Chuck X";
            // 
            // txt_ChuckX
            // 
            this.txt_ChuckX.Location = new System.Drawing.Point(92, 90);
            this.txt_ChuckX.Name = "txt_ChuckX";
            this.txt_ChuckX.ReadOnly = true;
            this.txt_ChuckX.Size = new System.Drawing.Size(75, 21);
            this.txt_ChuckX.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(208, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Chuck Y";
            // 
            // txt_ChuckY
            // 
            this.txt_ChuckY.Location = new System.Drawing.Point(270, 90);
            this.txt_ChuckY.Name = "txt_ChuckY";
            this.txt_ChuckY.ReadOnly = true;
            this.txt_ChuckY.Size = new System.Drawing.Size(75, 21);
            this.txt_ChuckY.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(382, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "Chuck Z";
            // 
            // txt_ChuckZ
            // 
            this.txt_ChuckZ.Location = new System.Drawing.Point(444, 90);
            this.txt_ChuckZ.Name = "txt_ChuckZ";
            this.txt_ChuckZ.ReadOnly = true;
            this.txt_ChuckZ.Size = new System.Drawing.Size(75, 21);
            this.txt_ChuckZ.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(565, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "Chuck θZ";
            // 
            // txt_ChuckSZ
            // 
            this.txt_ChuckSZ.Location = new System.Drawing.Point(627, 90);
            this.txt_ChuckSZ.Name = "txt_ChuckSZ";
            this.txt_ChuckSZ.ReadOnly = true;
            this.txt_ChuckSZ.Size = new System.Drawing.Size(75, 21);
            this.txt_ChuckSZ.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 163);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "Left X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(208, 163);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "Left Y";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(382, 163);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "Left Z";
            // 
            // txt_LeftX
            // 
            this.txt_LeftX.Location = new System.Drawing.Point(92, 157);
            this.txt_LeftX.Name = "txt_LeftX";
            this.txt_LeftX.ReadOnly = true;
            this.txt_LeftX.Size = new System.Drawing.Size(75, 21);
            this.txt_LeftX.TabIndex = 5;
            // 
            // txt_LeftY
            // 
            this.txt_LeftY.Location = new System.Drawing.Point(270, 157);
            this.txt_LeftY.Name = "txt_LeftY";
            this.txt_LeftY.ReadOnly = true;
            this.txt_LeftY.Size = new System.Drawing.Size(75, 21);
            this.txt_LeftY.TabIndex = 5;
            // 
            // txt_LeftZ
            // 
            this.txt_LeftZ.Location = new System.Drawing.Point(444, 157);
            this.txt_LeftZ.Name = "txt_LeftZ";
            this.txt_LeftZ.ReadOnly = true;
            this.txt_LeftZ.Size = new System.Drawing.Size(75, 21);
            this.txt_LeftZ.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(30, 199);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 3;
            this.label8.Text = "Left θX";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(208, 199);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 3;
            this.label9.Text = "Left θY";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(382, 199);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 3;
            this.label10.Text = "Left θZ";
            // 
            // txt_LeftSX
            // 
            this.txt_LeftSX.Location = new System.Drawing.Point(92, 193);
            this.txt_LeftSX.Name = "txt_LeftSX";
            this.txt_LeftSX.ReadOnly = true;
            this.txt_LeftSX.Size = new System.Drawing.Size(75, 21);
            this.txt_LeftSX.TabIndex = 5;
            // 
            // txt_LeftSY
            // 
            this.txt_LeftSY.Location = new System.Drawing.Point(270, 193);
            this.txt_LeftSY.Name = "txt_LeftSY";
            this.txt_LeftSY.ReadOnly = true;
            this.txt_LeftSY.Size = new System.Drawing.Size(75, 21);
            this.txt_LeftSY.TabIndex = 5;
            // 
            // txt_LeftSZ
            // 
            this.txt_LeftSZ.Location = new System.Drawing.Point(444, 193);
            this.txt_LeftSZ.Name = "txt_LeftSZ";
            this.txt_LeftSZ.ReadOnly = true;
            this.txt_LeftSZ.Size = new System.Drawing.Size(75, 21);
            this.txt_LeftSZ.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(30, 264);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 12);
            this.label11.TabIndex = 3;
            this.label11.Text = "Right X";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(30, 300);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 12);
            this.label12.TabIndex = 3;
            this.label12.Text = "Right θX";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(208, 264);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(47, 12);
            this.label13.TabIndex = 3;
            this.label13.Text = "Right Y";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(208, 300);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(59, 12);
            this.label14.TabIndex = 3;
            this.label14.Text = "Right θY";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(382, 264);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(47, 12);
            this.label15.TabIndex = 3;
            this.label15.Text = "Right Z";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(382, 300);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 12);
            this.label16.TabIndex = 3;
            this.label16.Text = "Right θZ";
            // 
            // txt_RightX
            // 
            this.txt_RightX.Location = new System.Drawing.Point(92, 258);
            this.txt_RightX.Name = "txt_RightX";
            this.txt_RightX.ReadOnly = true;
            this.txt_RightX.Size = new System.Drawing.Size(75, 21);
            this.txt_RightX.TabIndex = 5;
            // 
            // txt_RightSX
            // 
            this.txt_RightSX.Location = new System.Drawing.Point(92, 294);
            this.txt_RightSX.Name = "txt_RightSX";
            this.txt_RightSX.ReadOnly = true;
            this.txt_RightSX.Size = new System.Drawing.Size(75, 21);
            this.txt_RightSX.TabIndex = 5;
            // 
            // txt_RightY
            // 
            this.txt_RightY.Location = new System.Drawing.Point(270, 258);
            this.txt_RightY.Name = "txt_RightY";
            this.txt_RightY.ReadOnly = true;
            this.txt_RightY.Size = new System.Drawing.Size(75, 21);
            this.txt_RightY.TabIndex = 5;
            // 
            // txt_RightSY
            // 
            this.txt_RightSY.Location = new System.Drawing.Point(270, 294);
            this.txt_RightSY.Name = "txt_RightSY";
            this.txt_RightSY.ReadOnly = true;
            this.txt_RightSY.Size = new System.Drawing.Size(75, 21);
            this.txt_RightSY.TabIndex = 5;
            // 
            // txt_RightZ
            // 
            this.txt_RightZ.Location = new System.Drawing.Point(444, 258);
            this.txt_RightZ.Name = "txt_RightZ";
            this.txt_RightZ.ReadOnly = true;
            this.txt_RightZ.Size = new System.Drawing.Size(75, 21);
            this.txt_RightZ.TabIndex = 5;
            // 
            // txt_RightSZ
            // 
            this.txt_RightSZ.Location = new System.Drawing.Point(444, 294);
            this.txt_RightSZ.Name = "txt_RightSZ";
            this.txt_RightSZ.ReadOnly = true;
            this.txt_RightSZ.Size = new System.Drawing.Size(75, 21);
            this.txt_RightSZ.TabIndex = 5;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(30, 355);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(35, 12);
            this.label17.TabIndex = 3;
            this.label17.Text = "CCD X";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(208, 355);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(35, 12);
            this.label18.TabIndex = 3;
            this.label18.Text = "CCD Y";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(382, 355);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(35, 12);
            this.label19.TabIndex = 3;
            this.label19.Text = "CCD Z";
            // 
            // txt_CcdX
            // 
            this.txt_CcdX.Location = new System.Drawing.Point(92, 349);
            this.txt_CcdX.Name = "txt_CcdX";
            this.txt_CcdX.ReadOnly = true;
            this.txt_CcdX.Size = new System.Drawing.Size(75, 21);
            this.txt_CcdX.TabIndex = 5;
            // 
            // txt_CcdY
            // 
            this.txt_CcdY.Location = new System.Drawing.Point(267, 349);
            this.txt_CcdY.Name = "txt_CcdY";
            this.txt_CcdY.ReadOnly = true;
            this.txt_CcdY.Size = new System.Drawing.Size(75, 21);
            this.txt_CcdY.TabIndex = 5;
            // 
            // txt_CcdZ
            // 
            this.txt_CcdZ.Location = new System.Drawing.Point(444, 349);
            this.txt_CcdZ.Name = "txt_CcdZ";
            this.txt_CcdZ.ReadOnly = true;
            this.txt_CcdZ.Size = new System.Drawing.Size(75, 21);
            this.txt_CcdZ.TabIndex = 5;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(565, 358);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(11, 12);
            this.label20.TabIndex = 3;
            this.label20.Text = "U";
            // 
            // txt_AltimeterU
            // 
            this.txt_AltimeterU.Location = new System.Drawing.Point(627, 352);
            this.txt_AltimeterU.Name = "txt_AltimeterU";
            this.txt_AltimeterU.ReadOnly = true;
            this.txt_AltimeterU.Size = new System.Drawing.Size(75, 21);
            this.txt_AltimeterU.TabIndex = 5;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(30, 31);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(47, 12);
            this.label21.TabIndex = 3;
            this.label21.Text = "Chuck X";
            // 
            // txt_filePath
            // 
            this.txt_filePath.Location = new System.Drawing.Point(83, 28);
            this.txt_filePath.Name = "txt_filePath";
            this.txt_filePath.ReadOnly = true;
            this.txt_filePath.Size = new System.Drawing.Size(610, 21);
            this.txt_filePath.TabIndex = 6;
            // 
            // label22
            // 
            this.label22.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label22.Location = new System.Drawing.Point(30, 128);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(673, 2);
            this.label22.TabIndex = 7;
            // 
            // label23
            // 
            this.label23.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label23.Location = new System.Drawing.Point(30, 234);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(673, 2);
            this.label23.TabIndex = 7;
            // 
            // label24
            // 
            this.label24.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label24.Location = new System.Drawing.Point(30, 331);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(673, 2);
            this.label24.TabIndex = 7;
            // 
            // panelSpecialPos
            // 
            this.panelSpecialPos.Controls.Add(this.label24);
            this.panelSpecialPos.Controls.Add(this.label23);
            this.panelSpecialPos.Controls.Add(this.label22);
            this.panelSpecialPos.Controls.Add(this.txt_filePath);
            this.panelSpecialPos.Controls.Add(this.txt_ChuckSZ);
            this.panelSpecialPos.Controls.Add(this.txt_RightSZ);
            this.panelSpecialPos.Controls.Add(this.txt_LeftSZ);
            this.panelSpecialPos.Controls.Add(this.txt_RightZ);
            this.panelSpecialPos.Controls.Add(this.txt_LeftZ);
            this.panelSpecialPos.Controls.Add(this.txt_CcdZ);
            this.panelSpecialPos.Controls.Add(this.txt_ChuckZ);
            this.panelSpecialPos.Controls.Add(this.txt_RightSY);
            this.panelSpecialPos.Controls.Add(this.txt_LeftSY);
            this.panelSpecialPos.Controls.Add(this.txt_RightY);
            this.panelSpecialPos.Controls.Add(this.txt_LeftY);
            this.panelSpecialPos.Controls.Add(this.txt_CcdY);
            this.panelSpecialPos.Controls.Add(this.txt_ChuckY);
            this.panelSpecialPos.Controls.Add(this.txt_RightSX);
            this.panelSpecialPos.Controls.Add(this.txt_LeftSX);
            this.panelSpecialPos.Controls.Add(this.txt_RightX);
            this.panelSpecialPos.Controls.Add(this.txt_LeftX);
            this.panelSpecialPos.Controls.Add(this.txt_AltimeterU);
            this.panelSpecialPos.Controls.Add(this.txt_CcdX);
            this.panelSpecialPos.Controls.Add(this.txt_ChuckX);
            this.panelSpecialPos.Controls.Add(this.label4);
            this.panelSpecialPos.Controls.Add(this.label16);
            this.panelSpecialPos.Controls.Add(this.label10);
            this.panelSpecialPos.Controls.Add(this.label15);
            this.panelSpecialPos.Controls.Add(this.label7);
            this.panelSpecialPos.Controls.Add(this.label19);
            this.panelSpecialPos.Controls.Add(this.label3);
            this.panelSpecialPos.Controls.Add(this.label14);
            this.panelSpecialPos.Controls.Add(this.label9);
            this.panelSpecialPos.Controls.Add(this.label13);
            this.panelSpecialPos.Controls.Add(this.label6);
            this.panelSpecialPos.Controls.Add(this.label18);
            this.panelSpecialPos.Controls.Add(this.label2);
            this.panelSpecialPos.Controls.Add(this.label12);
            this.panelSpecialPos.Controls.Add(this.label8);
            this.panelSpecialPos.Controls.Add(this.label11);
            this.panelSpecialPos.Controls.Add(this.label5);
            this.panelSpecialPos.Controls.Add(this.label20);
            this.panelSpecialPos.Controls.Add(this.label17);
            this.panelSpecialPos.Controls.Add(this.label21);
            this.panelSpecialPos.Controls.Add(this.label1);
            this.panelSpecialPos.Location = new System.Drawing.Point(26, 29);
            this.panelSpecialPos.Name = "panelSpecialPos";
            this.panelSpecialPos.Size = new System.Drawing.Size(736, 391);
            this.panelSpecialPos.TabIndex = 8;
            // 
            // btn_MoveRightFA
            // 
            this.btn_MoveRightFA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_MoveRightFA.Location = new System.Drawing.Point(972, 202);
            this.btn_MoveRightFA.Name = "btn_MoveRightFA";
            this.btn_MoveRightFA.Size = new System.Drawing.Size(130, 41);
            this.btn_MoveRightFA.TabIndex = 9;
            this.btn_MoveRightFA.Text = "右FA运动到选定位置";
            this.btn_MoveRightFA.UseVisualStyleBackColor = false;
            this.btn_MoveRightFA.Click += new System.EventHandler(this.btn_MoveRightFA_Click);
            // 
            // btn_MoveChuck
            // 
            this.btn_MoveChuck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_MoveChuck.Location = new System.Drawing.Point(787, 292);
            this.btn_MoveChuck.Name = "btn_MoveChuck";
            this.btn_MoveChuck.Size = new System.Drawing.Size(130, 41);
            this.btn_MoveChuck.TabIndex = 10;
            this.btn_MoveChuck.Text = "Chuck运动到选定位置";
            this.btn_MoveChuck.UseVisualStyleBackColor = false;
            this.btn_MoveChuck.Click += new System.EventHandler(this.btn_MoveChuck_Click);
            // 
            // btn_MoveCCD
            // 
            this.btn_MoveCCD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_MoveCCD.Location = new System.Drawing.Point(972, 292);
            this.btn_MoveCCD.Name = "btn_MoveCCD";
            this.btn_MoveCCD.Size = new System.Drawing.Size(130, 41);
            this.btn_MoveCCD.TabIndex = 11;
            this.btn_MoveCCD.Text = "CCD运动到选定位置";
            this.btn_MoveCCD.UseVisualStyleBackColor = false;
            this.btn_MoveCCD.Click += new System.EventHandler(this.btn_MoveCCD_Click);
            // 
            // btn_MoveHeightU
            // 
            this.btn_MoveHeightU.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_MoveHeightU.Location = new System.Drawing.Point(787, 377);
            this.btn_MoveHeightU.Name = "btn_MoveHeightU";
            this.btn_MoveHeightU.Size = new System.Drawing.Size(147, 41);
            this.btn_MoveHeightU.TabIndex = 12;
            this.btn_MoveHeightU.Text = "测高仪运动到选定位置";
            this.btn_MoveHeightU.UseVisualStyleBackColor = false;
            this.btn_MoveHeightU.Click += new System.EventHandler(this.btn_MoveHeightU_Click);
            // 
            // FormSpecialPositionEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1169, 536);
            this.Controls.Add(this.btn_MoveHeightU);
            this.Controls.Add(this.btn_MoveCCD);
            this.Controls.Add(this.btn_MoveChuck);
            this.Controls.Add(this.btn_MoveRightFA);
            this.Controls.Add(this.panelSpecialPos);
            this.Controls.Add(this.btn_MoveLeftFA);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.btn_Load);
            this.Name = "FormSpecialPositionEdit";
            this.Text = "FormSpecialPositionEdit";
            this.panelSpecialPos.ResumeLayout(false);
            this.panelSpecialPos.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Load;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_MoveLeftFA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_ChuckX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_ChuckY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_ChuckZ;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_ChuckSZ;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_LeftX;
        private System.Windows.Forms.TextBox txt_LeftY;
        private System.Windows.Forms.TextBox txt_LeftZ;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt_LeftSX;
        private System.Windows.Forms.TextBox txt_LeftSY;
        private System.Windows.Forms.TextBox txt_LeftSZ;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txt_RightX;
        private System.Windows.Forms.TextBox txt_RightSX;
        private System.Windows.Forms.TextBox txt_RightY;
        private System.Windows.Forms.TextBox txt_RightSY;
        private System.Windows.Forms.TextBox txt_RightZ;
        private System.Windows.Forms.TextBox txt_RightSZ;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txt_CcdX;
        private System.Windows.Forms.TextBox txt_CcdY;
        private System.Windows.Forms.TextBox txt_CcdZ;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txt_AltimeterU;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txt_filePath;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Panel panelSpecialPos;
        private System.Windows.Forms.Button btn_MoveRightFA;
        private System.Windows.Forms.Button btn_MoveChuck;
        private System.Windows.Forms.Button btn_MoveCCD;
        private System.Windows.Forms.Button btn_MoveHeightU;
    }
}