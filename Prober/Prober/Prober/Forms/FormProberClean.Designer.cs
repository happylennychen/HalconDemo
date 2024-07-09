namespace Prober.Forms
{
    partial class FormProberClean
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numClearProber_RowGap = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numClearProber_ColumnGap = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numClearProber_Depth = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numClearProber_Times = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numClearProber_RowCount = new System.Windows.Forms.NumericUpDown();
            this.numClearProber_ColumnCount = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtClearProber_ChuckX = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtClearProber_ChuckY = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtClearProber_ChuckZ = new System.Windows.Forms.TextBox();
            this.txtClearProber_Exp = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtClearProber_CcdX = new System.Windows.Forms.TextBox();
            this.txtClearProber_CcdZ = new System.Windows.Forms.TextBox();
            this.txtClearProber_CcdY = new System.Windows.Forms.TextBox();
            this.btn_ClearProber_Update = new System.Windows.Forms.Button();
            this.btnClearProber_Save = new System.Windows.Forms.Button();
            this.btnClearProber_Load = new System.Windows.Forms.Button();
            this.btnClearProber_Reset = new System.Windows.Forms.Button();
            this.btnClearProber_Run = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel_CleanProber = new System.Windows.Forms.Panel();
            this.txtClearProber_Zoom = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.table_ClearPaper = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.numClearProber_RowGap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numClearProber_ColumnGap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numClearProber_Depth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numClearProber_Times)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numClearProber_RowCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numClearProber_ColumnCount)).BeginInit();
            this.panel_CleanProber.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "请针设置";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "行间距(um)";
            // 
            // numClearProber_RowGap
            // 
            this.numClearProber_RowGap.Location = new System.Drawing.Point(102, 47);
            this.numClearProber_RowGap.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numClearProber_RowGap.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numClearProber_RowGap.Name = "numClearProber_RowGap";
            this.numClearProber_RowGap.Size = new System.Drawing.Size(93, 21);
            this.numClearProber_RowGap.TabIndex = 1;
            this.numClearProber_RowGap.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(219, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "列间距(um)";
            // 
            // numClearProber_ColumnGap
            // 
            this.numClearProber_ColumnGap.Location = new System.Drawing.Point(290, 47);
            this.numClearProber_ColumnGap.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numClearProber_ColumnGap.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numClearProber_ColumnGap.Name = "numClearProber_ColumnGap";
            this.numClearProber_ColumnGap.Size = new System.Drawing.Size(93, 21);
            this.numClearProber_ColumnGap.TabIndex = 1;
            this.numClearProber_ColumnGap.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(403, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "压针深度(um)";
            // 
            // numClearProber_Depth
            // 
            this.numClearProber_Depth.Location = new System.Drawing.Point(486, 47);
            this.numClearProber_Depth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numClearProber_Depth.Name = "numClearProber_Depth";
            this.numClearProber_Depth.Size = new System.Drawing.Size(81, 21);
            this.numClearProber_Depth.TabIndex = 1;
            this.numClearProber_Depth.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(585, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "重复次数：";
            // 
            // numClearProber_Times
            // 
            this.numClearProber_Times.Location = new System.Drawing.Point(656, 47);
            this.numClearProber_Times.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numClearProber_Times.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numClearProber_Times.Name = "numClearProber_Times";
            this.numClearProber_Times.Size = new System.Drawing.Size(93, 21);
            this.numClearProber_Times.TabIndex = 1;
            this.numClearProber_Times.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "行数";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(219, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "列数";
            // 
            // numClearProber_RowCount
            // 
            this.numClearProber_RowCount.Location = new System.Drawing.Point(102, 81);
            this.numClearProber_RowCount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numClearProber_RowCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numClearProber_RowCount.Name = "numClearProber_RowCount";
            this.numClearProber_RowCount.Size = new System.Drawing.Size(93, 21);
            this.numClearProber_RowCount.TabIndex = 1;
            this.numClearProber_RowCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numClearProber_ColumnCount
            // 
            this.numClearProber_ColumnCount.Location = new System.Drawing.Point(290, 81);
            this.numClearProber_ColumnCount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numClearProber_ColumnCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numClearProber_ColumnCount.Name = "numClearProber_ColumnCount";
            this.numClearProber_ColumnCount.Size = new System.Drawing.Size(93, 21);
            this.numClearProber_ColumnCount.TabIndex = 1;
            this.numClearProber_ColumnCount.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(31, 127);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "请针位置";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(31, 160);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "Chuck_X";
            // 
            // txtClearProber_ChuckX
            // 
            this.txtClearProber_ChuckX.Location = new System.Drawing.Point(102, 157);
            this.txtClearProber_ChuckX.Name = "txtClearProber_ChuckX";
            this.txtClearProber_ChuckX.Size = new System.Drawing.Size(75, 21);
            this.txtClearProber_ChuckX.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(201, 163);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "Chuck_Y";
            // 
            // txtClearProber_ChuckY
            // 
            this.txtClearProber_ChuckY.Location = new System.Drawing.Point(256, 158);
            this.txtClearProber_ChuckY.Name = "txtClearProber_ChuckY";
            this.txtClearProber_ChuckY.Size = new System.Drawing.Size(75, 21);
            this.txtClearProber_ChuckY.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(369, 164);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "Chuck_Z";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(524, 164);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "Exposure";
            // 
            // txtClearProber_ChuckZ
            // 
            this.txtClearProber_ChuckZ.Location = new System.Drawing.Point(426, 157);
            this.txtClearProber_ChuckZ.Name = "txtClearProber_ChuckZ";
            this.txtClearProber_ChuckZ.Size = new System.Drawing.Size(75, 21);
            this.txtClearProber_ChuckZ.TabIndex = 2;
            // 
            // txtClearProber_Exp
            // 
            this.txtClearProber_Exp.Location = new System.Drawing.Point(584, 157);
            this.txtClearProber_Exp.Name = "txtClearProber_Exp";
            this.txtClearProber_Exp.Size = new System.Drawing.Size(75, 21);
            this.txtClearProber_Exp.TabIndex = 2;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(31, 199);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(35, 12);
            this.label13.TabIndex = 0;
            this.label13.Text = "CCD_X";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(369, 203);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(35, 12);
            this.label14.TabIndex = 0;
            this.label14.Text = "CCD_Z";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(201, 202);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 12);
            this.label15.TabIndex = 0;
            this.label15.Text = "CCD_Y";
            // 
            // txtClearProber_CcdX
            // 
            this.txtClearProber_CcdX.Location = new System.Drawing.Point(102, 196);
            this.txtClearProber_CcdX.Name = "txtClearProber_CcdX";
            this.txtClearProber_CcdX.Size = new System.Drawing.Size(75, 21);
            this.txtClearProber_CcdX.TabIndex = 2;
            // 
            // txtClearProber_CcdZ
            // 
            this.txtClearProber_CcdZ.Location = new System.Drawing.Point(426, 196);
            this.txtClearProber_CcdZ.Name = "txtClearProber_CcdZ";
            this.txtClearProber_CcdZ.Size = new System.Drawing.Size(75, 21);
            this.txtClearProber_CcdZ.TabIndex = 2;
            // 
            // txtClearProber_CcdY
            // 
            this.txtClearProber_CcdY.Location = new System.Drawing.Point(256, 197);
            this.txtClearProber_CcdY.Name = "txtClearProber_CcdY";
            this.txtClearProber_CcdY.Size = new System.Drawing.Size(75, 21);
            this.txtClearProber_CcdY.TabIndex = 2;
            // 
            // btn_ClearProber_Update
            // 
            this.btn_ClearProber_Update.BackgroundImage = global::Prober.Properties.Resources.定位;
            this.btn_ClearProber_Update.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_ClearProber_Update.FlatAppearance.BorderSize = 0;
            this.btn_ClearProber_Update.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ClearProber_Update.Location = new System.Drawing.Point(707, 180);
            this.btn_ClearProber_Update.Name = "btn_ClearProber_Update";
            this.btn_ClearProber_Update.Size = new System.Drawing.Size(32, 32);
            this.btn_ClearProber_Update.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btn_ClearProber_Update, "获取清针位置");
            this.btn_ClearProber_Update.UseVisualStyleBackColor = true;
            this.btn_ClearProber_Update.Click += new System.EventHandler(this.btn_ClearProber_Update_Click);
            // 
            // btnClearProber_Save
            // 
            this.btnClearProber_Save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btnClearProber_Save.Location = new System.Drawing.Point(121, 262);
            this.btnClearProber_Save.Name = "btnClearProber_Save";
            this.btnClearProber_Save.Size = new System.Drawing.Size(103, 31);
            this.btnClearProber_Save.TabIndex = 3;
            this.btnClearProber_Save.Text = "保存参数";
            this.btnClearProber_Save.UseVisualStyleBackColor = false;
            this.btnClearProber_Save.Click += new System.EventHandler(this.btnClearProber_Save_Click);
            // 
            // btnClearProber_Load
            // 
            this.btnClearProber_Load.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btnClearProber_Load.Location = new System.Drawing.Point(267, 262);
            this.btnClearProber_Load.Name = "btnClearProber_Load";
            this.btnClearProber_Load.Size = new System.Drawing.Size(103, 31);
            this.btnClearProber_Load.TabIndex = 3;
            this.btnClearProber_Load.Text = "加载参数";
            this.btnClearProber_Load.UseVisualStyleBackColor = false;
            this.btnClearProber_Load.Click += new System.EventHandler(this.btnClearProber_Load_Click);
            // 
            // btnClearProber_Reset
            // 
            this.btnClearProber_Reset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btnClearProber_Reset.Location = new System.Drawing.Point(424, 262);
            this.btnClearProber_Reset.Name = "btnClearProber_Reset";
            this.btnClearProber_Reset.Size = new System.Drawing.Size(103, 31);
            this.btnClearProber_Reset.TabIndex = 3;
            this.btnClearProber_Reset.Text = "重置请针纸";
            this.btnClearProber_Reset.UseVisualStyleBackColor = false;
            this.btnClearProber_Reset.Click += new System.EventHandler(this.btnClearProber_Reset_Click);
            // 
            // btnClearProber_Run
            // 
            this.btnClearProber_Run.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.btnClearProber_Run.Location = new System.Drawing.Point(584, 262);
            this.btnClearProber_Run.Name = "btnClearProber_Run";
            this.btnClearProber_Run.Size = new System.Drawing.Size(103, 31);
            this.btnClearProber_Run.TabIndex = 3;
            this.btnClearProber_Run.Text = "执行自动清针";
            this.btnClearProber_Run.UseVisualStyleBackColor = false;
            this.btnClearProber_Run.Click += new System.EventHandler(this.btnClearProber_Run_Click);
            // 
            // panel_CleanProber
            // 
            this.panel_CleanProber.Controls.Add(this.btn_ClearProber_Update);
            this.panel_CleanProber.Controls.Add(this.txtClearProber_Zoom);
            this.panel_CleanProber.Controls.Add(this.txtClearProber_Exp);
            this.panel_CleanProber.Controls.Add(this.txtClearProber_CcdY);
            this.panel_CleanProber.Controls.Add(this.txtClearProber_ChuckY);
            this.panel_CleanProber.Controls.Add(this.txtClearProber_CcdZ);
            this.panel_CleanProber.Controls.Add(this.txtClearProber_ChuckZ);
            this.panel_CleanProber.Controls.Add(this.txtClearProber_CcdX);
            this.panel_CleanProber.Controls.Add(this.txtClearProber_ChuckX);
            this.panel_CleanProber.Controls.Add(this.numClearProber_Times);
            this.panel_CleanProber.Controls.Add(this.numClearProber_Depth);
            this.panel_CleanProber.Controls.Add(this.numClearProber_ColumnCount);
            this.panel_CleanProber.Controls.Add(this.numClearProber_ColumnGap);
            this.panel_CleanProber.Controls.Add(this.numClearProber_RowCount);
            this.panel_CleanProber.Controls.Add(this.numClearProber_RowGap);
            this.panel_CleanProber.Controls.Add(this.label6);
            this.panel_CleanProber.Controls.Add(this.label5);
            this.panel_CleanProber.Controls.Add(this.label7);
            this.panel_CleanProber.Controls.Add(this.label16);
            this.panel_CleanProber.Controls.Add(this.label12);
            this.panel_CleanProber.Controls.Add(this.label15);
            this.panel_CleanProber.Controls.Add(this.label10);
            this.panel_CleanProber.Controls.Add(this.label4);
            this.panel_CleanProber.Controls.Add(this.label14);
            this.panel_CleanProber.Controls.Add(this.label11);
            this.panel_CleanProber.Controls.Add(this.label13);
            this.panel_CleanProber.Controls.Add(this.label9);
            this.panel_CleanProber.Controls.Add(this.label3);
            this.panel_CleanProber.Controls.Add(this.label2);
            this.panel_CleanProber.Controls.Add(this.label8);
            this.panel_CleanProber.Controls.Add(this.label1);
            this.panel_CleanProber.Location = new System.Drawing.Point(19, 16);
            this.panel_CleanProber.Name = "panel_CleanProber";
            this.panel_CleanProber.Size = new System.Drawing.Size(772, 233);
            this.panel_CleanProber.TabIndex = 5;
            // 
            // txtClearProber_Zoom
            // 
            this.txtClearProber_Zoom.Location = new System.Drawing.Point(584, 193);
            this.txtClearProber_Zoom.Name = "txtClearProber_Zoom";
            this.txtClearProber_Zoom.Size = new System.Drawing.Size(75, 21);
            this.txtClearProber_Zoom.TabIndex = 2;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(524, 200);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(29, 12);
            this.label16.TabIndex = 0;
            this.label16.Text = "Zoom";
            // 
            // table_ClearPaper
            // 
            this.table_ClearPaper.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.table_ClearPaper.ColumnCount = 5;
            this.table_ClearPaper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.table_ClearPaper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.table_ClearPaper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.table_ClearPaper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.table_ClearPaper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.table_ClearPaper.Location = new System.Drawing.Point(19, 314);
            this.table_ClearPaper.Name = "table_ClearPaper";
            this.table_ClearPaper.RowCount = 10;
            this.table_ClearPaper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.table_ClearPaper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.table_ClearPaper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.table_ClearPaper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.table_ClearPaper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.table_ClearPaper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.table_ClearPaper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.table_ClearPaper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.table_ClearPaper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.table_ClearPaper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.table_ClearPaper.Size = new System.Drawing.Size(772, 264);
            this.table_ClearPaper.TabIndex = 6;
            // 
            // FormProberClean
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 590);
            this.Controls.Add(this.table_ClearPaper);
            this.Controls.Add(this.panel_CleanProber);
            this.Controls.Add(this.btnClearProber_Run);
            this.Controls.Add(this.btnClearProber_Reset);
            this.Controls.Add(this.btnClearProber_Load);
            this.Controls.Add(this.btnClearProber_Save);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormProberClean";
            this.Text = "FormProberClean";
            ((System.ComponentModel.ISupportInitialize)(this.numClearProber_RowGap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numClearProber_ColumnGap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numClearProber_Depth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numClearProber_Times)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numClearProber_RowCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numClearProber_ColumnCount)).EndInit();
            this.panel_CleanProber.ResumeLayout(false);
            this.panel_CleanProber.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numClearProber_RowGap;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numClearProber_ColumnGap;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numClearProber_Depth;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numClearProber_Times;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numClearProber_RowCount;
        private System.Windows.Forms.NumericUpDown numClearProber_ColumnCount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtClearProber_ChuckX;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtClearProber_ChuckY;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtClearProber_ChuckZ;
        private System.Windows.Forms.TextBox txtClearProber_Exp;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtClearProber_CcdX;
        private System.Windows.Forms.TextBox txtClearProber_CcdZ;
        private System.Windows.Forms.TextBox txtClearProber_CcdY;
        private System.Windows.Forms.Button btn_ClearProber_Update;
        private System.Windows.Forms.Button btnClearProber_Save;
        private System.Windows.Forms.Button btnClearProber_Load;
        private System.Windows.Forms.Button btnClearProber_Reset;
        private System.Windows.Forms.Button btnClearProber_Run;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel_CleanProber;
        private System.Windows.Forms.TextBox txtClearProber_Zoom;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TableLayoutPanel table_ClearPaper;
    }
}