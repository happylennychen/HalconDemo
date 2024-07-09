using Prober.WaferDef;

namespace Prober.Forms
{
    partial class FormMapping
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMapping));
            this.tab_Wafer = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.rtbMsgBox = new System.Windows.Forms.RichTextBox();
            this.pn_WaferInfo = new System.Windows.Forms.Panel();
            this.btn_SetRef3 = new System.Windows.Forms.Button();
            this.btn_SetRef2 = new System.Windows.Forms.Button();
            this.btn_SetRef4 = new System.Windows.Forms.Button();
            this.btn_SetRef1 = new System.Windows.Forms.Button();
            this.btn_MoveToMark = new System.Windows.Forms.Button();
            this.btn_ConfirmRefDie = new System.Windows.Forms.Button();
            this.btn_SelectMark = new System.Windows.Forms.Button();
            this.txt_DieCount = new System.Windows.Forms.TextBox();
            this.txt_DieHeight = new System.Windows.Forms.TextBox();
            this.txt_MarkDieName = new System.Windows.Forms.TextBox();
            this.txt_MarkCcdZ = new System.Windows.Forms.TextBox();
            this.txt_MarkChuckX = new System.Windows.Forms.TextBox();
            this.txt_MarkZoom = new System.Windows.Forms.TextBox();
            this.txt_MarkExposure = new System.Windows.Forms.TextBox();
            this.txt_MarkRow = new System.Windows.Forms.TextBox();
            this.txt_DieWidth = new System.Windows.Forms.TextBox();
            this.txt_DieColumns = new System.Windows.Forms.TextBox();
            this.txt_MarkCcdY = new System.Windows.Forms.TextBox();
            this.txt_MarkCcdX = new System.Windows.Forms.TextBox();
            this.txt_MarkChuckY = new System.Windows.Forms.TextBox();
            this.txt_MarkColumn = new System.Windows.Forms.TextBox();
            this.txt_DieRows = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lbl_RefDie4Name = new System.Windows.Forms.Label();
            this.lbl_RefDie3Name = new System.Windows.Forms.Label();
            this.lbl_RefDie2Name = new System.Windows.Forms.Label();
            this.lbl_RefDie1Name = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_StopMap = new System.Windows.Forms.Button();
            this.btn_AutoMap = new System.Windows.Forms.Button();
            this.btn_Download = new System.Windows.Forms.Button();
            this.btn_Upload = new System.Windows.Forms.Button();
            this.btn_DeleteWaferType = new System.Windows.Forms.Button();
            this.btn_CreateWaferType = new System.Windows.Forms.Button();
            this.cbox_WaferType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ctlWafer_Main = new Prober.WaferDef.ControlWafer();
            this.tab_Wafer.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.pn_WaferInfo.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab_Wafer
            // 
            this.tab_Wafer.Controls.Add(this.tabPage1);
            this.tab_Wafer.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tab_Wafer.Location = new System.Drawing.Point(8, 8);
            this.tab_Wafer.Margin = new System.Windows.Forms.Padding(4);
            this.tab_Wafer.Name = "tab_Wafer";
            this.tab_Wafer.SelectedIndex = 0;
            this.tab_Wafer.Size = new System.Drawing.Size(631, 932);
            this.tab_Wafer.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.rtbMsgBox);
            this.tabPage1.Controls.Add(this.pn_WaferInfo);
            this.tabPage1.Controls.Add(this.btn_StopMap);
            this.tabPage1.Controls.Add(this.btn_AutoMap);
            this.tabPage1.Controls.Add(this.btn_Download);
            this.tabPage1.Controls.Add(this.btn_Upload);
            this.tabPage1.Controls.Add(this.btn_DeleteWaferType);
            this.tabPage1.Controls.Add(this.btn_CreateWaferType);
            this.tabPage1.Controls.Add(this.cbox_WaferType);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label30);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(623, 903);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "晶圆信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // rtbMsgBox
            // 
            this.rtbMsgBox.Location = new System.Drawing.Point(10, 569);
            this.rtbMsgBox.Name = "rtbMsgBox";
            this.rtbMsgBox.ReadOnly = true;
            this.rtbMsgBox.Size = new System.Drawing.Size(598, 307);
            this.rtbMsgBox.TabIndex = 8;
            this.rtbMsgBox.Text = "";
            this.rtbMsgBox.TextChanged += new System.EventHandler(this.rtbMsgBox_TextChanged);
            // 
            // pn_WaferInfo
            // 
            this.pn_WaferInfo.Controls.Add(this.btn_SetRef3);
            this.pn_WaferInfo.Controls.Add(this.btn_SetRef2);
            this.pn_WaferInfo.Controls.Add(this.btn_SetRef4);
            this.pn_WaferInfo.Controls.Add(this.btn_SetRef1);
            this.pn_WaferInfo.Controls.Add(this.btn_MoveToMark);
            this.pn_WaferInfo.Controls.Add(this.btn_ConfirmRefDie);
            this.pn_WaferInfo.Controls.Add(this.btn_SelectMark);
            this.pn_WaferInfo.Controls.Add(this.txt_DieCount);
            this.pn_WaferInfo.Controls.Add(this.txt_DieHeight);
            this.pn_WaferInfo.Controls.Add(this.txt_MarkDieName);
            this.pn_WaferInfo.Controls.Add(this.txt_MarkCcdZ);
            this.pn_WaferInfo.Controls.Add(this.txt_MarkChuckX);
            this.pn_WaferInfo.Controls.Add(this.txt_MarkZoom);
            this.pn_WaferInfo.Controls.Add(this.txt_MarkExposure);
            this.pn_WaferInfo.Controls.Add(this.txt_MarkRow);
            this.pn_WaferInfo.Controls.Add(this.txt_DieWidth);
            this.pn_WaferInfo.Controls.Add(this.txt_DieColumns);
            this.pn_WaferInfo.Controls.Add(this.txt_MarkCcdY);
            this.pn_WaferInfo.Controls.Add(this.txt_MarkCcdX);
            this.pn_WaferInfo.Controls.Add(this.txt_MarkChuckY);
            this.pn_WaferInfo.Controls.Add(this.txt_MarkColumn);
            this.pn_WaferInfo.Controls.Add(this.txt_DieRows);
            this.pn_WaferInfo.Controls.Add(this.label18);
            this.pn_WaferInfo.Controls.Add(this.label16);
            this.pn_WaferInfo.Controls.Add(this.label14);
            this.pn_WaferInfo.Controls.Add(this.label12);
            this.pn_WaferInfo.Controls.Add(this.label8);
            this.pn_WaferInfo.Controls.Add(this.label6);
            this.pn_WaferInfo.Controls.Add(this.label9);
            this.pn_WaferInfo.Controls.Add(this.label19);
            this.pn_WaferInfo.Controls.Add(this.label17);
            this.pn_WaferInfo.Controls.Add(this.lbl_RefDie4Name);
            this.pn_WaferInfo.Controls.Add(this.lbl_RefDie3Name);
            this.pn_WaferInfo.Controls.Add(this.lbl_RefDie2Name);
            this.pn_WaferInfo.Controls.Add(this.lbl_RefDie1Name);
            this.pn_WaferInfo.Controls.Add(this.label15);
            this.pn_WaferInfo.Controls.Add(this.label1);
            this.pn_WaferInfo.Controls.Add(this.label13);
            this.pn_WaferInfo.Controls.Add(this.label11);
            this.pn_WaferInfo.Controls.Add(this.label7);
            this.pn_WaferInfo.Controls.Add(this.label5);
            this.pn_WaferInfo.Controls.Add(this.label20);
            this.pn_WaferInfo.Controls.Add(this.label10);
            this.pn_WaferInfo.Controls.Add(this.label4);
            this.pn_WaferInfo.Location = new System.Drawing.Point(8, 100);
            this.pn_WaferInfo.Margin = new System.Windows.Forms.Padding(4);
            this.pn_WaferInfo.Name = "pn_WaferInfo";
            this.pn_WaferInfo.Size = new System.Drawing.Size(598, 391);
            this.pn_WaferInfo.TabIndex = 7;
            // 
            // btn_SetRef3
            // 
            this.btn_SetRef3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_SetRef3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SetRef3.Location = new System.Drawing.Point(207, 322);
            this.btn_SetRef3.Margin = new System.Windows.Forms.Padding(4);
            this.btn_SetRef3.Name = "btn_SetRef3";
            this.btn_SetRef3.Size = new System.Drawing.Size(86, 33);
            this.btn_SetRef3.TabIndex = 5;
            this.btn_SetRef3.Text = "设置基准3";
            this.btn_SetRef3.UseVisualStyleBackColor = false;
            this.btn_SetRef3.Click += new System.EventHandler(this.btn_SetRef3_Click);
            // 
            // btn_SetRef2
            // 
            this.btn_SetRef2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_SetRef2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SetRef2.Location = new System.Drawing.Point(30, 322);
            this.btn_SetRef2.Margin = new System.Windows.Forms.Padding(4);
            this.btn_SetRef2.Name = "btn_SetRef2";
            this.btn_SetRef2.Size = new System.Drawing.Size(86, 33);
            this.btn_SetRef2.TabIndex = 5;
            this.btn_SetRef2.Text = "设置基准2";
            this.btn_SetRef2.UseVisualStyleBackColor = false;
            this.btn_SetRef2.Click += new System.EventHandler(this.btn_SetRef2_Click);
            // 
            // btn_SetRef4
            // 
            this.btn_SetRef4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_SetRef4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SetRef4.Location = new System.Drawing.Point(207, 277);
            this.btn_SetRef4.Margin = new System.Windows.Forms.Padding(4);
            this.btn_SetRef4.Name = "btn_SetRef4";
            this.btn_SetRef4.Size = new System.Drawing.Size(86, 33);
            this.btn_SetRef4.TabIndex = 5;
            this.btn_SetRef4.Text = "设置基准4";
            this.btn_SetRef4.UseVisualStyleBackColor = false;
            this.btn_SetRef4.Click += new System.EventHandler(this.btn_SetRef4_Click);
            // 
            // btn_SetRef1
            // 
            this.btn_SetRef1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_SetRef1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SetRef1.Location = new System.Drawing.Point(30, 279);
            this.btn_SetRef1.Margin = new System.Windows.Forms.Padding(4);
            this.btn_SetRef1.Name = "btn_SetRef1";
            this.btn_SetRef1.Size = new System.Drawing.Size(86, 33);
            this.btn_SetRef1.TabIndex = 5;
            this.btn_SetRef1.Text = "设置基准1";
            this.btn_SetRef1.UseVisualStyleBackColor = false;
            this.btn_SetRef1.Click += new System.EventHandler(this.btn_SetRef1_Click);
            // 
            // btn_MoveToMark
            // 
            this.btn_MoveToMark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_MoveToMark.Location = new System.Drawing.Point(398, 221);
            this.btn_MoveToMark.Margin = new System.Windows.Forms.Padding(4);
            this.btn_MoveToMark.Name = "btn_MoveToMark";
            this.btn_MoveToMark.Size = new System.Drawing.Size(111, 33);
            this.btn_MoveToMark.TabIndex = 5;
            this.btn_MoveToMark.Text = "运动到Mark";
            this.btn_MoveToMark.UseVisualStyleBackColor = false;
            this.btn_MoveToMark.Click += new System.EventHandler(this.btn_MoveToMark_Click);
            // 
            // btn_ConfirmRefDie
            // 
            this.btn_ConfirmRefDie.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_ConfirmRefDie.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ConfirmRefDie.Image = global::Prober.Properties.Resources.勾选;
            this.btn_ConfirmRefDie.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_ConfirmRefDie.Location = new System.Drawing.Point(384, 322);
            this.btn_ConfirmRefDie.Margin = new System.Windows.Forms.Padding(4);
            this.btn_ConfirmRefDie.Name = "btn_ConfirmRefDie";
            this.btn_ConfirmRefDie.Size = new System.Drawing.Size(111, 33);
            this.btn_ConfirmRefDie.TabIndex = 5;
            this.btn_ConfirmRefDie.Text = "确认选择";
            this.btn_ConfirmRefDie.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_ConfirmRefDie.UseVisualStyleBackColor = false;
            this.btn_ConfirmRefDie.Click += new System.EventHandler(this.btn_ConfirmRefDie_Click);
            // 
            // btn_SelectMark
            // 
            this.btn_SelectMark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_SelectMark.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SelectMark.Image = global::Prober.Properties.Resources.标记;
            this.btn_SelectMark.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_SelectMark.Location = new System.Drawing.Point(254, 216);
            this.btn_SelectMark.Margin = new System.Windows.Forms.Padding(4);
            this.btn_SelectMark.Name = "btn_SelectMark";
            this.btn_SelectMark.Size = new System.Drawing.Size(111, 33);
            this.btn_SelectMark.TabIndex = 5;
            this.btn_SelectMark.Text = "选取Mark";
            this.btn_SelectMark.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_SelectMark.UseVisualStyleBackColor = false;
            this.btn_SelectMark.Click += new System.EventHandler(this.btn_SelectMark_Click);
            // 
            // txt_DieCount
            // 
            this.txt_DieCount.Location = new System.Drawing.Point(424, 68);
            this.txt_DieCount.Margin = new System.Windows.Forms.Padding(4);
            this.txt_DieCount.Name = "txt_DieCount";
            this.txt_DieCount.ReadOnly = true;
            this.txt_DieCount.Size = new System.Drawing.Size(92, 24);
            this.txt_DieCount.TabIndex = 4;
            // 
            // txt_DieHeight
            // 
            this.txt_DieHeight.Location = new System.Drawing.Point(109, 66);
            this.txt_DieHeight.Margin = new System.Windows.Forms.Padding(4);
            this.txt_DieHeight.Name = "txt_DieHeight";
            this.txt_DieHeight.ReadOnly = true;
            this.txt_DieHeight.Size = new System.Drawing.Size(81, 24);
            this.txt_DieHeight.TabIndex = 4;
            // 
            // txt_MarkDieName
            // 
            this.txt_MarkDieName.Location = new System.Drawing.Point(266, 180);
            this.txt_MarkDieName.Margin = new System.Windows.Forms.Padding(4);
            this.txt_MarkDieName.Name = "txt_MarkDieName";
            this.txt_MarkDieName.ReadOnly = true;
            this.txt_MarkDieName.Size = new System.Drawing.Size(92, 24);
            this.txt_MarkDieName.TabIndex = 4;
            // 
            // txt_MarkCcdZ
            // 
            this.txt_MarkCcdZ.Location = new System.Drawing.Point(424, 180);
            this.txt_MarkCcdZ.Margin = new System.Windows.Forms.Padding(4);
            this.txt_MarkCcdZ.Name = "txt_MarkCcdZ";
            this.txt_MarkCcdZ.ReadOnly = true;
            this.txt_MarkCcdZ.Size = new System.Drawing.Size(92, 24);
            this.txt_MarkCcdZ.TabIndex = 4;
            // 
            // txt_MarkChuckX
            // 
            this.txt_MarkChuckX.Location = new System.Drawing.Point(266, 122);
            this.txt_MarkChuckX.Margin = new System.Windows.Forms.Padding(4);
            this.txt_MarkChuckX.Name = "txt_MarkChuckX";
            this.txt_MarkChuckX.ReadOnly = true;
            this.txt_MarkChuckX.Size = new System.Drawing.Size(92, 24);
            this.txt_MarkChuckX.TabIndex = 4;
            // 
            // txt_MarkZoom
            // 
            this.txt_MarkZoom.Location = new System.Drawing.Point(109, 216);
            this.txt_MarkZoom.Margin = new System.Windows.Forms.Padding(4);
            this.txt_MarkZoom.Name = "txt_MarkZoom";
            this.txt_MarkZoom.ReadOnly = true;
            this.txt_MarkZoom.Size = new System.Drawing.Size(81, 24);
            this.txt_MarkZoom.TabIndex = 4;
            // 
            // txt_MarkExposure
            // 
            this.txt_MarkExposure.Location = new System.Drawing.Point(109, 180);
            this.txt_MarkExposure.Margin = new System.Windows.Forms.Padding(4);
            this.txt_MarkExposure.Name = "txt_MarkExposure";
            this.txt_MarkExposure.ReadOnly = true;
            this.txt_MarkExposure.Size = new System.Drawing.Size(81, 24);
            this.txt_MarkExposure.TabIndex = 4;
            // 
            // txt_MarkRow
            // 
            this.txt_MarkRow.Location = new System.Drawing.Point(109, 122);
            this.txt_MarkRow.Margin = new System.Windows.Forms.Padding(4);
            this.txt_MarkRow.Name = "txt_MarkRow";
            this.txt_MarkRow.ReadOnly = true;
            this.txt_MarkRow.Size = new System.Drawing.Size(81, 24);
            this.txt_MarkRow.TabIndex = 4;
            // 
            // txt_DieWidth
            // 
            this.txt_DieWidth.Location = new System.Drawing.Point(109, 34);
            this.txt_DieWidth.Margin = new System.Windows.Forms.Padding(4);
            this.txt_DieWidth.Name = "txt_DieWidth";
            this.txt_DieWidth.ReadOnly = true;
            this.txt_DieWidth.Size = new System.Drawing.Size(81, 24);
            this.txt_DieWidth.TabIndex = 4;
            // 
            // txt_DieColumns
            // 
            this.txt_DieColumns.Location = new System.Drawing.Point(424, 34);
            this.txt_DieColumns.Margin = new System.Windows.Forms.Padding(4);
            this.txt_DieColumns.Name = "txt_DieColumns";
            this.txt_DieColumns.ReadOnly = true;
            this.txt_DieColumns.Size = new System.Drawing.Size(92, 24);
            this.txt_DieColumns.TabIndex = 4;
            // 
            // txt_MarkCcdY
            // 
            this.txt_MarkCcdY.Location = new System.Drawing.Point(424, 153);
            this.txt_MarkCcdY.Margin = new System.Windows.Forms.Padding(4);
            this.txt_MarkCcdY.Name = "txt_MarkCcdY";
            this.txt_MarkCcdY.ReadOnly = true;
            this.txt_MarkCcdY.Size = new System.Drawing.Size(92, 24);
            this.txt_MarkCcdY.TabIndex = 4;
            // 
            // txt_MarkCcdX
            // 
            this.txt_MarkCcdX.Location = new System.Drawing.Point(424, 122);
            this.txt_MarkCcdX.Margin = new System.Windows.Forms.Padding(4);
            this.txt_MarkCcdX.Name = "txt_MarkCcdX";
            this.txt_MarkCcdX.ReadOnly = true;
            this.txt_MarkCcdX.Size = new System.Drawing.Size(92, 24);
            this.txt_MarkCcdX.TabIndex = 4;
            // 
            // txt_MarkChuckY
            // 
            this.txt_MarkChuckY.Location = new System.Drawing.Point(266, 153);
            this.txt_MarkChuckY.Margin = new System.Windows.Forms.Padding(4);
            this.txt_MarkChuckY.Name = "txt_MarkChuckY";
            this.txt_MarkChuckY.ReadOnly = true;
            this.txt_MarkChuckY.Size = new System.Drawing.Size(92, 24);
            this.txt_MarkChuckY.TabIndex = 4;
            // 
            // txt_MarkColumn
            // 
            this.txt_MarkColumn.Location = new System.Drawing.Point(109, 153);
            this.txt_MarkColumn.Margin = new System.Windows.Forms.Padding(4);
            this.txt_MarkColumn.Name = "txt_MarkColumn";
            this.txt_MarkColumn.ReadOnly = true;
            this.txt_MarkColumn.Size = new System.Drawing.Size(81, 24);
            this.txt_MarkColumn.TabIndex = 4;
            // 
            // txt_DieRows
            // 
            this.txt_DieRows.Location = new System.Drawing.Point(266, 32);
            this.txt_DieRows.Margin = new System.Windows.Forms.Padding(4);
            this.txt_DieRows.Name = "txt_DieRows";
            this.txt_DieRows.ReadOnly = true;
            this.txt_DieRows.Size = new System.Drawing.Size(92, 24);
            this.txt_DieRows.TabIndex = 4;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(366, 186);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(50, 16);
            this.label18.TabIndex = 3;
            this.label18.Text = "Z4(um)";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(366, 128);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(52, 16);
            this.label16.TabIndex = 3;
            this.label16.Text = "X4(um)";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(204, 128);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 16);
            this.label14.TabIndex = 3;
            this.label14.Text = "X2(um)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 158);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 16);
            this.label12.TabIndex = 3;
            this.label12.Text = "Column(pix)";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(371, 38);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 16);
            this.label8.TabIndex = 3;
            this.label8.Text = "列数";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(204, 38);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 16);
            this.label6.TabIndex = 3;
            this.label6.Text = "行数";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(317, 74);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 16);
            this.label9.TabIndex = 3;
            this.label9.Text = "Die数量(pcs)";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(204, 186);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(63, 16);
            this.label19.TabIndex = 3;
            this.label19.Text = "Die Name";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(366, 159);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(51, 16);
            this.label17.TabIndex = 3;
            this.label17.Text = "Y4(um)";
            // 
            // lbl_RefDie4Name
            // 
            this.lbl_RefDie4Name.Location = new System.Drawing.Point(313, 286);
            this.lbl_RefDie4Name.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_RefDie4Name.Name = "lbl_RefDie4Name";
            this.lbl_RefDie4Name.Size = new System.Drawing.Size(52, 16);
            this.lbl_RefDie4Name.TabIndex = 3;
            this.lbl_RefDie4Name.Text = "NA";
            // 
            // lbl_RefDie3Name
            // 
            this.lbl_RefDie3Name.Location = new System.Drawing.Point(313, 328);
            this.lbl_RefDie3Name.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_RefDie3Name.Name = "lbl_RefDie3Name";
            this.lbl_RefDie3Name.Size = new System.Drawing.Size(52, 16);
            this.lbl_RefDie3Name.TabIndex = 3;
            this.lbl_RefDie3Name.Text = "NA";
            // 
            // lbl_RefDie2Name
            // 
            this.lbl_RefDie2Name.Location = new System.Drawing.Point(138, 328);
            this.lbl_RefDie2Name.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_RefDie2Name.Name = "lbl_RefDie2Name";
            this.lbl_RefDie2Name.Size = new System.Drawing.Size(52, 16);
            this.lbl_RefDie2Name.TabIndex = 3;
            this.lbl_RefDie2Name.Text = "NA";
            // 
            // lbl_RefDie1Name
            // 
            this.lbl_RefDie1Name.Location = new System.Drawing.Point(138, 286);
            this.lbl_RefDie1Name.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_RefDie1Name.Name = "lbl_RefDie1Name";
            this.lbl_RefDie1Name.Size = new System.Drawing.Size(52, 16);
            this.lbl_RefDie1Name.TabIndex = 3;
            this.lbl_RefDie1Name.Text = "NA";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(204, 159);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(51, 16);
            this.label15.TabIndex = 3;
            this.label15.Text = "Y2(um)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 221);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Zoom";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(19, 185);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(60, 16);
            this.label13.TabIndex = 3;
            this.label13.Text = "Exposure";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 127);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 16);
            this.label11.TabIndex = 3;
            this.label11.Text = "Row(pix)";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(17, 70);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 16);
            this.label7.TabIndex = 3;
            this.label7.Text = "Die高度(um)";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(17, 38);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "Die宽度(um)";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(1, 255);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(97, 17);
            this.label20.TabIndex = 3;
            this.label20.Text = "Wafer基准Die";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(16, 102);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 17);
            this.label10.TabIndex = 3;
            this.label10.Text = "Mark信息";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(17, 13);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Die信息";
            // 
            // btn_StopMap
            // 
            this.btn_StopMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_StopMap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_StopMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_StopMap.Image = global::Prober.Properties.Resources.Excel__2_;
            this.btn_StopMap.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_StopMap.Location = new System.Drawing.Point(274, 514);
            this.btn_StopMap.Margin = new System.Windows.Forms.Padding(4);
            this.btn_StopMap.Name = "btn_StopMap";
            this.btn_StopMap.Size = new System.Drawing.Size(111, 33);
            this.btn_StopMap.TabIndex = 5;
            this.btn_StopMap.Text = "停止Map";
            this.btn_StopMap.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_StopMap.UseVisualStyleBackColor = false;
            this.btn_StopMap.Click += new System.EventHandler(this.btn_StopMap_Click);
            // 
            // btn_AutoMap
            // 
            this.btn_AutoMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_AutoMap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_AutoMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_AutoMap.Image = global::Prober.Properties.Resources.scanning;
            this.btn_AutoMap.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_AutoMap.Location = new System.Drawing.Point(97, 514);
            this.btn_AutoMap.Margin = new System.Windows.Forms.Padding(4);
            this.btn_AutoMap.Name = "btn_AutoMap";
            this.btn_AutoMap.Size = new System.Drawing.Size(111, 33);
            this.btn_AutoMap.TabIndex = 6;
            this.btn_AutoMap.Text = "自动Map";
            this.btn_AutoMap.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_AutoMap.UseVisualStyleBackColor = false;
            this.btn_AutoMap.Click += new System.EventHandler(this.btn_AutoMap_Click);
            // 
            // btn_Download
            // 
            this.btn_Download.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_Download.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Download.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Download.Image = global::Prober.Properties.Resources.download1;
            this.btn_Download.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Download.Location = new System.Drawing.Point(189, 59);
            this.btn_Download.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Download.Name = "btn_Download";
            this.btn_Download.Size = new System.Drawing.Size(86, 33);
            this.btn_Download.TabIndex = 5;
            this.btn_Download.Text = "下料";
            this.btn_Download.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.btn_Download, "托盘运动到下料位置");
            this.btn_Download.UseVisualStyleBackColor = false;
            this.btn_Download.Click += new System.EventHandler(this.btn_Download_Click);
            // 
            // btn_Upload
            // 
            this.btn_Upload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_Upload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_Upload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Upload.Image = global::Prober.Properties.Resources.upload__1_;
            this.btn_Upload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Upload.Location = new System.Drawing.Point(42, 59);
            this.btn_Upload.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Upload.Name = "btn_Upload";
            this.btn_Upload.Size = new System.Drawing.Size(86, 33);
            this.btn_Upload.TabIndex = 6;
            this.btn_Upload.Text = "上料";
            this.btn_Upload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.btn_Upload, "托盘运动到上料位置");
            this.btn_Upload.UseVisualStyleBackColor = false;
            this.btn_Upload.Click += new System.EventHandler(this.btn_Upload_Click);
            // 
            // btn_DeleteWaferType
            // 
            this.btn_DeleteWaferType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_DeleteWaferType.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_DeleteWaferType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_DeleteWaferType.Image = global::Prober.Properties.Resources.删除2;
            this.btn_DeleteWaferType.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_DeleteWaferType.Location = new System.Drawing.Point(485, 15);
            this.btn_DeleteWaferType.Margin = new System.Windows.Forms.Padding(4);
            this.btn_DeleteWaferType.Name = "btn_DeleteWaferType";
            this.btn_DeleteWaferType.Size = new System.Drawing.Size(86, 33);
            this.btn_DeleteWaferType.TabIndex = 5;
            this.btn_DeleteWaferType.Text = "删除";
            this.btn_DeleteWaferType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.btn_DeleteWaferType, "删除当前选中的WaferMap");
            this.btn_DeleteWaferType.UseVisualStyleBackColor = false;
            this.btn_DeleteWaferType.Click += new System.EventHandler(this.btn_DeleteWaferType_Click);
            // 
            // btn_CreateWaferType
            // 
            this.btn_CreateWaferType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btn_CreateWaferType.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_CreateWaferType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_CreateWaferType.Image = global::Prober.Properties.Resources.添加_创建;
            this.btn_CreateWaferType.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_CreateWaferType.Location = new System.Drawing.Point(377, 15);
            this.btn_CreateWaferType.Margin = new System.Windows.Forms.Padding(4);
            this.btn_CreateWaferType.Name = "btn_CreateWaferType";
            this.btn_CreateWaferType.Size = new System.Drawing.Size(86, 33);
            this.btn_CreateWaferType.TabIndex = 6;
            this.btn_CreateWaferType.Text = "创建";
            this.btn_CreateWaferType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.btn_CreateWaferType, "创建新的WaferMap");
            this.btn_CreateWaferType.UseVisualStyleBackColor = false;
            this.btn_CreateWaferType.Click += new System.EventHandler(this.btn_CreateWaferType_Click);
            // 
            // cbox_WaferType
            // 
            this.cbox_WaferType.FormattingEnabled = true;
            this.cbox_WaferType.Location = new System.Drawing.Point(96, 20);
            this.cbox_WaferType.Margin = new System.Windows.Forms.Padding(4);
            this.cbox_WaferType.Name = "cbox_WaferType";
            this.cbox_WaferType.Size = new System.Drawing.Size(230, 24);
            this.cbox_WaferType.TabIndex = 4;
            this.cbox_WaferType.SelectedIndexChanged += new System.EventHandler(this.cbox_WaferType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 25);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "晶圆类型";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(16, 521);
            this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(38, 17);
            this.label30.TabIndex = 3;
            this.label30.Text = "操作";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tab_Wafer);
            this.panel1.Location = new System.Drawing.Point(1104, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(645, 944);
            this.panel1.TabIndex = 4;
            // 
            // ctlWafer_Main
            // 
            this.ctlWafer_Main.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctlWafer_Main.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctlWafer_Main.Location = new System.Drawing.Point(12, 12);
            this.ctlWafer_Main.Name = "ctlWafer_Main";
            this.ctlWafer_Main.Radius = 542;
            this.ctlWafer_Main.Size = new System.Drawing.Size(1085, 936);
            this.ctlWafer_Main.TabIndex = 5;
            // 
            // FormMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1756, 961);
            this.Controls.Add(this.ctlWafer_Main);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMapping";
            this.Text = "Mapping";
            this.Load += new System.EventHandler(this.FormMapping_Load);
            this.tab_Wafer.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.pn_WaferInfo.ResumeLayout(false);
            this.pn_WaferInfo.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tab_Wafer;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel pn_WaferInfo;
        private System.Windows.Forms.Button btn_SetRef3;
        private System.Windows.Forms.Button btn_SetRef2;
        private System.Windows.Forms.Button btn_SetRef4;
        private System.Windows.Forms.Button btn_SetRef1;
        private System.Windows.Forms.Button btn_MoveToMark;
        private System.Windows.Forms.Button btn_ConfirmRefDie;
        private System.Windows.Forms.Button btn_SelectMark;
        private System.Windows.Forms.TextBox txt_DieCount;
        private System.Windows.Forms.TextBox txt_DieHeight;
        private System.Windows.Forms.TextBox txt_MarkDieName;
        private System.Windows.Forms.TextBox txt_MarkCcdZ;
        private System.Windows.Forms.TextBox txt_MarkChuckX;
        private System.Windows.Forms.TextBox txt_MarkExposure;
        private System.Windows.Forms.TextBox txt_MarkRow;
        private System.Windows.Forms.TextBox txt_DieWidth;
        private System.Windows.Forms.TextBox txt_DieColumns;
        private System.Windows.Forms.TextBox txt_MarkCcdY;
        private System.Windows.Forms.TextBox txt_MarkCcdX;
        private System.Windows.Forms.TextBox txt_MarkChuckY;
        private System.Windows.Forms.TextBox txt_MarkColumn;
        private System.Windows.Forms.TextBox txt_DieRows;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lbl_RefDie4Name;
        private System.Windows.Forms.Label lbl_RefDie3Name;
        private System.Windows.Forms.Label lbl_RefDie2Name;
        private System.Windows.Forms.Label lbl_RefDie1Name;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_StopMap;
        private System.Windows.Forms.Button btn_AutoMap;
        private System.Windows.Forms.Button btn_Download;
        private System.Windows.Forms.Button btn_Upload;
        private System.Windows.Forms.Button btn_DeleteWaferType;
        private System.Windows.Forms.Button btn_CreateWaferType;
        private System.Windows.Forms.ComboBox cbox_WaferType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Panel panel1;
        private ControlWafer ctlWafer_Main;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.RichTextBox rtbMsgBox;
        private System.Windows.Forms.TextBox txt_MarkZoom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
    }
}