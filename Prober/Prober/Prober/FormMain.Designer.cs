namespace Prober {
    partial class FormMain {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miInitialize = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.miControlMode = new System.Windows.Forms.ToolStripMenuItem();
            this.miGuiLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.miFunction = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenCamera = new System.Windows.Forms.ToolStripMenuItem();
            this.miControlInstrument = new System.Windows.Forms.ToolStripMenuItem();
            this.miSampleRecord = new System.Windows.Forms.ToolStripMenuItem();
            this.miStageHoming = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditTestPosition = new System.Windows.Forms.ToolStripMenuItem();
            this.miOtherSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.miCalibration = new System.Windows.Forms.ToolStripMenuItem();
            this.miWaferMapping = new System.Windows.Forms.ToolStripMenuItem();
            this.miSystemCalibration = new System.Windows.Forms.ToolStripMenuItem();
            this.miLeadScewCompensate = new System.Windows.Forms.ToolStripMenuItem();
            this.mileadScrewCalibration = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.miDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.miMoveDut = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.slblUserTitle = new System.Windows.Forms.ToolStripStatusLabel();
            this.slblUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.slblRoleTitle = new System.Windows.Forms.ToolStripStatusLabel();
            this.slblRole = new System.Windows.Forms.ToolStripStatusLabel();
            this.slblLoginTimeTitle = new System.Windows.Forms.ToolStripStatusLabel();
            this.slblLoginTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.slblControlModeTitle = new System.Windows.Forms.ToolStripStatusLabel();
            this.slblControlMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl_WaferMotion = new System.Windows.Forms.TabControl();
            this.tabPage_WaferMap = new System.Windows.Forms.TabPage();
            this.panelWaferMap = new System.Windows.Forms.Panel();
            this.tabPage_positionCali = new System.Windows.Forms.TabPage();
            this.rtbMsgBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.panelRedGreenLightBoard = new System.Windows.Forms.Panel();
            this.panelOtherDisplay = new System.Windows.Forms.Panel();
            this.panelHeightDisplay = new System.Windows.Forms.Panel();
            this.panelRealTimeDisplay = new System.Windows.Forms.Panel();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.panelCoupling = new System.Windows.Forms.Panel();
            this.panelStageJogs = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.statusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl_WaferMotion.SuspendLayout();
            this.tabPage_WaferMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miSetting,
            this.miFunction,
            this.miCalibration,
            this.miHelp,
            this.miDebug});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1904, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miInitialize,
            this.toolStripSeparator1,
            this.miExit});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(39, 21);
            this.miFile.Text = "File";
            // 
            // miInitialize
            // 
            this.miInitialize.Name = "miInitialize";
            this.miInitialize.Size = new System.Drawing.Size(123, 22);
            this.miInitialize.Text = "Initialize";
            this.miInitialize.Click += new System.EventHandler(this.miInitialize_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(120, 6);
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(123, 22);
            this.miExit.Text = "Exit";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // miSetting
            // 
            this.miSetting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miControlMode,
            this.miGuiLanguage});
            this.miSetting.Name = "miSetting";
            this.miSetting.Size = new System.Drawing.Size(60, 21);
            this.miSetting.Text = "Setting";
            // 
            // miControlMode
            // 
            this.miControlMode.Name = "miControlMode";
            this.miControlMode.Size = new System.Drawing.Size(159, 22);
            this.miControlMode.Text = "Control Mode";
            this.miControlMode.Click += new System.EventHandler(this.miControlMode_Click);
            // 
            // miGuiLanguage
            // 
            this.miGuiLanguage.Name = "miGuiLanguage";
            this.miGuiLanguage.Size = new System.Drawing.Size(159, 22);
            this.miGuiLanguage.Text = "GUI Language";
            this.miGuiLanguage.Click += new System.EventHandler(this.miGuiLanguage_Click);
            // 
            // miFunction
            // 
            this.miFunction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOpenCamera,
            this.miControlInstrument,
            this.miSampleRecord,
            this.miStageHoming,
            this.miEditTestPosition,
            this.miOtherSetting});
            this.miFunction.Name = "miFunction";
            this.miFunction.Size = new System.Drawing.Size(68, 21);
            this.miFunction.Text = "Function";
            // 
            // miOpenCamera
            // 
            this.miOpenCamera.Name = "miOpenCamera";
            this.miOpenCamera.Size = new System.Drawing.Size(185, 22);
            this.miOpenCamera.Text = "Open Camera";
            this.miOpenCamera.Click += new System.EventHandler(this.ToolStripMenuItem_OpenCamera_Click);
            // 
            // miControlInstrument
            // 
            this.miControlInstrument.Name = "miControlInstrument";
            this.miControlInstrument.Size = new System.Drawing.Size(185, 22);
            this.miControlInstrument.Text = "Instrument Control";
            this.miControlInstrument.Click += new System.EventHandler(this.ToolStripMenuItem_ControlInstrument_Click);
            // 
            // miSampleRecord
            // 
            this.miSampleRecord.Name = "miSampleRecord";
            this.miSampleRecord.Size = new System.Drawing.Size(185, 22);
            this.miSampleRecord.Text = "Data Sample";
            this.miSampleRecord.Click += new System.EventHandler(this.ToolStripMenuItem_SampleRecord_Click);
            // 
            // miStageHoming
            // 
            this.miStageHoming.Name = "miStageHoming";
            this.miStageHoming.Size = new System.Drawing.Size(185, 22);
            this.miStageHoming.Text = "StageHoming";
            this.miStageHoming.Click += new System.EventHandler(this.ToolStripMenuItem_stageHoming_Click);
            // 
            // miEditTestPosition
            // 
            this.miEditTestPosition.Name = "miEditTestPosition";
            this.miEditTestPosition.Size = new System.Drawing.Size(185, 22);
            this.miEditTestPosition.Text = "Edit Test Position";
            this.miEditTestPosition.Click += new System.EventHandler(this.setTestPositionToolStripMenuItem_Click);
            // 
            // miOtherSetting
            // 
            this.miOtherSetting.Name = "miOtherSetting";
            this.miOtherSetting.Size = new System.Drawing.Size(185, 22);
            this.miOtherSetting.Text = "Other Setting";
            this.miOtherSetting.Click += new System.EventHandler(this.otherSettingToolStripMenuItem_Click);
            // 
            // miCalibration
            // 
            this.miCalibration.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miWaferMapping,
            this.miSystemCalibration,
            this.miLeadScewCompensate,
            this.mileadScrewCalibration});
            this.miCalibration.Name = "miCalibration";
            this.miCalibration.Size = new System.Drawing.Size(83, 21);
            this.miCalibration.Text = "Calibration";
            // 
            // miWaferMapping
            // 
            this.miWaferMapping.Name = "miWaferMapping";
            this.miWaferMapping.Size = new System.Drawing.Size(210, 22);
            this.miWaferMapping.Text = "Wafer Mapping";
            this.miWaferMapping.Click += new System.EventHandler(this.ToolStripMenuItem_waferMapping_Click);
            // 
            // miSystemCalibration
            // 
            this.miSystemCalibration.Name = "miSystemCalibration";
            this.miSystemCalibration.Size = new System.Drawing.Size(210, 22);
            this.miSystemCalibration.Text = "System Calibration";
            this.miSystemCalibration.Click += new System.EventHandler(this.ToolStripMenuItem_systemCalibration_Click);
            // 
            // miLeadScewCompensate
            // 
            this.miLeadScewCompensate.Name = "miLeadScewCompensate";
            this.miLeadScewCompensate.Size = new System.Drawing.Size(210, 22);
            this.miLeadScewCompensate.Text = "LeadScew Compensate";
            this.miLeadScewCompensate.Click += new System.EventHandler(this.miLeadScewCompensate_Click);
            // 
            // mileadScrewCalibration
            // 
            this.mileadScrewCalibration.Name = "mileadScrewCalibration";
            this.mileadScrewCalibration.Size = new System.Drawing.Size(210, 22);
            this.mileadScrewCalibration.Text = "LeadScrew Calibration";
            this.mileadScrewCalibration.Click += new System.EventHandler(this.mileadScrewCalibration_Click);
            // 
            // miHelp
            // 
            this.miHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAbout});
            this.miHelp.Name = "miHelp";
            this.miHelp.Size = new System.Drawing.Size(47, 21);
            this.miHelp.Text = "Help";
            // 
            // miAbout
            // 
            this.miAbout.Name = "miAbout";
            this.miAbout.Size = new System.Drawing.Size(111, 22);
            this.miAbout.Text = "About";
            this.miAbout.Click += new System.EventHandler(this.miAbout_Click);
            // 
            // miDebug
            // 
            this.miDebug.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMoveDut});
            this.miDebug.Name = "miDebug";
            this.miDebug.Size = new System.Drawing.Size(59, 21);
            this.miDebug.Text = "Debug";
            // 
            // miMoveDut
            // 
            this.miMoveDut.Name = "miMoveDut";
            this.miMoveDut.Size = new System.Drawing.Size(138, 22);
            this.miMoveDut.Text = "Move DUT";
            this.miMoveDut.Click += new System.EventHandler(this.miMoveDut_Click);
            // 
            // statusBar
            // 
            this.statusBar.AutoSize = false;
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slblUserTitle,
            this.slblUser,
            this.slblRoleTitle,
            this.slblRole,
            this.slblLoginTimeTitle,
            this.slblLoginTime,
            this.slblControlModeTitle,
            this.slblControlMode});
            this.statusBar.Location = new System.Drawing.Point(0, 1039);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(1904, 22);
            this.statusBar.TabIndex = 1;
            this.statusBar.Text = "statusStrip1";
            // 
            // slblUserTitle
            // 
            this.slblUserTitle.Name = "slblUserTitle";
            this.slblUserTitle.Size = new System.Drawing.Size(42, 17);
            this.slblUserTitle.Text = "User: ";
            // 
            // slblUser
            // 
            this.slblUser.Name = "slblUser";
            this.slblUser.Size = new System.Drawing.Size(32, 17);
            this.slblUser.Text = "????";
            // 
            // slblRoleTitle
            // 
            this.slblRoleTitle.Name = "slblRoleTitle";
            this.slblRoleTitle.Size = new System.Drawing.Size(41, 17);
            this.slblRoleTitle.Text = "Role: ";
            // 
            // slblRole
            // 
            this.slblRole.Name = "slblRole";
            this.slblRole.Size = new System.Drawing.Size(32, 17);
            this.slblRole.Text = "????";
            // 
            // slblLoginTimeTitle
            // 
            this.slblLoginTimeTitle.Name = "slblLoginTimeTitle";
            this.slblLoginTimeTitle.Size = new System.Drawing.Size(79, 17);
            this.slblLoginTimeTitle.Text = "Login Time: ";
            // 
            // slblLoginTime
            // 
            this.slblLoginTime.Name = "slblLoginTime";
            this.slblLoginTime.Size = new System.Drawing.Size(32, 17);
            this.slblLoginTime.Text = "????";
            // 
            // slblControlModeTitle
            // 
            this.slblControlModeTitle.Name = "slblControlModeTitle";
            this.slblControlModeTitle.Size = new System.Drawing.Size(97, 17);
            this.slblControlModeTitle.Text = "Control Mode: ";
            // 
            // slblControlMode
            // 
            this.slblControlMode.Name = "slblControlMode";
            this.slblControlMode.Size = new System.Drawing.Size(32, 17);
            this.slblControlMode.Text = "????";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(1904, 1014);
            this.splitContainer1.SplitterDistance = 767;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl_WaferMotion);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.rtbMsgBox);
            this.splitContainer2.Size = new System.Drawing.Size(767, 1014);
            this.splitContainer2.SplitterDistance = 597;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControl_WaferMotion
            // 
            this.tabControl_WaferMotion.Controls.Add(this.tabPage_WaferMap);
            this.tabControl_WaferMotion.Controls.Add(this.tabPage_positionCali);
            this.tabControl_WaferMotion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_WaferMotion.Location = new System.Drawing.Point(0, 0);
            this.tabControl_WaferMotion.Name = "tabControl_WaferMotion";
            this.tabControl_WaferMotion.SelectedIndex = 0;
            this.tabControl_WaferMotion.Size = new System.Drawing.Size(763, 593);
            this.tabControl_WaferMotion.TabIndex = 1;
            this.tabControl_WaferMotion.TabIndexChanged += new System.EventHandler(this.tabControl_WaferMotion_TabIndexChanged);
            // 
            // tabPage_WaferMap
            // 
            this.tabPage_WaferMap.Controls.Add(this.panelWaferMap);
            this.tabPage_WaferMap.Location = new System.Drawing.Point(4, 22);
            this.tabPage_WaferMap.Name = "tabPage_WaferMap";
            this.tabPage_WaferMap.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_WaferMap.Size = new System.Drawing.Size(755, 567);
            this.tabPage_WaferMap.TabIndex = 0;
            this.tabPage_WaferMap.Text = "WaferMap";
            this.tabPage_WaferMap.UseVisualStyleBackColor = true;
            // 
            // panelWaferMap
            // 
            this.panelWaferMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWaferMap.Location = new System.Drawing.Point(3, 3);
            this.panelWaferMap.Name = "panelWaferMap";
            this.panelWaferMap.Size = new System.Drawing.Size(749, 561);
            this.panelWaferMap.TabIndex = 0;
            // 
            // tabPage_positionCali
            // 
            this.tabPage_positionCali.Location = new System.Drawing.Point(4, 22);
            this.tabPage_positionCali.Name = "tabPage_positionCali";
            this.tabPage_positionCali.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_positionCali.Size = new System.Drawing.Size(722, 567);
            this.tabPage_positionCali.TabIndex = 1;
            this.tabPage_positionCali.Text = "PositionCalculate";
            this.tabPage_positionCali.UseVisualStyleBackColor = true;
            // 
            // rtbMsgBox
            // 
            this.rtbMsgBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbMsgBox.Location = new System.Drawing.Point(0, 0);
            this.rtbMsgBox.Name = "rtbMsgBox";
            this.rtbMsgBox.Size = new System.Drawing.Size(763, 409);
            this.rtbMsgBox.TabIndex = 0;
            this.rtbMsgBox.Text = "";
            this.rtbMsgBox.TextChanged += new System.EventHandler(this.rtbMsgBox_TextChanged);
            // 
            // splitContainer3
            // 
            this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer3.Size = new System.Drawing.Size(1133, 1014);
            this.splitContainer3.SplitterDistance = 222;
            this.splitContainer3.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.panelRedGreenLightBoard);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.panelOtherDisplay);
            this.splitContainer4.Panel2.Controls.Add(this.panelHeightDisplay);
            this.splitContainer4.Panel2.Controls.Add(this.panelRealTimeDisplay);
            this.splitContainer4.Size = new System.Drawing.Size(1133, 222);
            this.splitContainer4.SplitterDistance = 114;
            this.splitContainer4.TabIndex = 0;
            // 
            // panelRedGreenLightBoard
            // 
            this.panelRedGreenLightBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRedGreenLightBoard.Location = new System.Drawing.Point(0, 0);
            this.panelRedGreenLightBoard.Name = "panelRedGreenLightBoard";
            this.panelRedGreenLightBoard.Size = new System.Drawing.Size(1129, 110);
            this.panelRedGreenLightBoard.TabIndex = 0;
            // 
            // panelOtherDisplay
            // 
            this.panelOtherDisplay.Location = new System.Drawing.Point(821, 0);
            this.panelOtherDisplay.Name = "panelOtherDisplay";
            this.panelOtherDisplay.Size = new System.Drawing.Size(331, 100);
            this.panelOtherDisplay.TabIndex = 2;
            // 
            // panelHeightDisplay
            // 
            this.panelHeightDisplay.Location = new System.Drawing.Point(500, 0);
            this.panelHeightDisplay.Name = "panelHeightDisplay";
            this.panelHeightDisplay.Size = new System.Drawing.Size(289, 100);
            this.panelHeightDisplay.TabIndex = 1;
            // 
            // panelRealTimeDisplay
            // 
            this.panelRealTimeDisplay.Location = new System.Drawing.Point(0, 0);
            this.panelRealTimeDisplay.Name = "panelRealTimeDisplay";
            this.panelRealTimeDisplay.Size = new System.Drawing.Size(474, 100);
            this.panelRealTimeDisplay.TabIndex = 0;
            // 
            // splitContainer5
            // 
            this.splitContainer5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.panelCoupling);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.panelStageJogs);
            this.splitContainer5.Size = new System.Drawing.Size(1133, 788);
            this.splitContainer5.SplitterDistance = 323;
            this.splitContainer5.TabIndex = 0;
            // 
            // panelCoupling
            // 
            this.panelCoupling.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCoupling.Location = new System.Drawing.Point(0, 0);
            this.panelCoupling.Name = "panelCoupling";
            this.panelCoupling.Size = new System.Drawing.Size(1129, 319);
            this.panelCoupling.TabIndex = 0;
            // 
            // panelStageJogs
            // 
            this.panelStageJogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStageJogs.Location = new System.Drawing.Point(0, 0);
            this.panelStageJogs.Name = "panelStageJogs";
            this.panelStageJogs.Size = new System.Drawing.Size(1129, 457);
            this.panelStageJogs.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1061);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "Prober";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl_WaferMotion.ResumeLayout(false);
            this.tabPage_WaferMap.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem miInitialize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.ToolStripMenuItem miHelp;
        private System.Windows.Forms.ToolStripMenuItem miAbout;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panelWaferMap;
        private System.Windows.Forms.RichTextBox rtbMsgBox;
        private System.Windows.Forms.ToolStripStatusLabel slblUserTitle;
        private System.Windows.Forms.ToolStripStatusLabel slblUser;
        private System.Windows.Forms.ToolStripStatusLabel slblRoleTitle;
        private System.Windows.Forms.ToolStripStatusLabel slblRole;
        private System.Windows.Forms.ToolStripStatusLabel slblLoginTimeTitle;
        private System.Windows.Forms.ToolStripStatusLabel slblLoginTime;
        private System.Windows.Forms.ToolStripStatusLabel slblControlModeTitle;
        private System.Windows.Forms.ToolStripStatusLabel slblControlMode;
        private System.Windows.Forms.ToolStripMenuItem miFunction;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.Panel panelRealTimeDisplay;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.Panel panelCoupling;
        private System.Windows.Forms.Panel panelStageJogs;
        private System.Windows.Forms.ToolStripMenuItem miSetting;
        private System.Windows.Forms.ToolStripMenuItem miControlMode;
        private System.Windows.Forms.Panel panelRedGreenLightBoard;
        private System.Windows.Forms.ToolStripMenuItem miOpenCamera;
        private System.Windows.Forms.ToolStripMenuItem miControlInstrument;
        private System.Windows.Forms.ToolStripMenuItem miSampleRecord;
        private System.Windows.Forms.ToolStripMenuItem miGuiLanguage;
        private System.Windows.Forms.ToolStripMenuItem miDebug;
        private System.Windows.Forms.ToolStripMenuItem miMoveDut;
        private System.Windows.Forms.ToolStripMenuItem miCalibration;
        private System.Windows.Forms.ToolStripMenuItem miWaferMapping;
        private System.Windows.Forms.ToolStripMenuItem miSystemCalibration;
        private System.Windows.Forms.ToolStripMenuItem miStageHoming;
        private System.Windows.Forms.TabControl tabControl_WaferMotion;
        private System.Windows.Forms.TabPage tabPage_WaferMap;
        private System.Windows.Forms.TabPage tabPage_positionCali;
        private System.Windows.Forms.Panel panelHeightDisplay;
        private System.Windows.Forms.ToolStripMenuItem miLeadScewCompensate;
        private System.Windows.Forms.ToolStripMenuItem mileadScrewCalibration;
        private System.Windows.Forms.ToolStripMenuItem miOtherSetting;
        private System.Windows.Forms.Panel panelOtherDisplay;
        private System.Windows.Forms.ToolStripMenuItem miEditTestPosition;
    }
}

