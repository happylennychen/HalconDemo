namespace MyMotionStageUserControl {
    partial class UcStageAxis {
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.gbBase = new System.Windows.Forms.GroupBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpAbsPos = new System.Windows.Forms.TabPage();
            this.tbCurrPos = new System.Windows.Forms.TextBox();
            this.tpAccumRelMov = new System.Windows.Forms.TabPage();
            this.tbAccumulatedRelMov = new System.Windows.Forms.TextBox();
            this.pbPositiveDirection = new System.Windows.Forms.PictureBox();
            this.btnStepPositive = new System.Windows.Forms.Button();
            this.lblRelMove = new System.Windows.Forms.Label();
            this.tbRelMov = new System.Windows.Forms.TextBox();
            this.btnStepNegative = new System.Windows.Forms.Button();
            this.lblAxisId = new System.Windows.Forms.Label();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miMoveAbsolute = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miSetMotionSpeed = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miSetMotionRange = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miHoming = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.miStop = new System.Windows.Forms.ToolStripMenuItem();
            this.gbBase.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tpAbsPos.SuspendLayout();
            this.tpAccumRelMov.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPositiveDirection)).BeginInit();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbBase
            // 
            this.gbBase.Controls.Add(this.tabControl);
            this.gbBase.Controls.Add(this.pbPositiveDirection);
            this.gbBase.Controls.Add(this.btnStepPositive);
            this.gbBase.Controls.Add(this.lblRelMove);
            this.gbBase.Controls.Add(this.tbRelMov);
            this.gbBase.Controls.Add(this.btnStepNegative);
            this.gbBase.Controls.Add(this.lblAxisId);
            this.gbBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbBase.Location = new System.Drawing.Point(0, 0);
            this.gbBase.Name = "gbBase";
            this.gbBase.Size = new System.Drawing.Size(200, 117);
            this.gbBase.TabIndex = 0;
            this.gbBase.TabStop = false;
            this.gbBase.Text = "Axis";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpAbsPos);
            this.tabControl.Controls.Add(this.tpAccumRelMov);
            this.tabControl.Location = new System.Drawing.Point(5, 18);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(153, 61);
            this.tabControl.TabIndex = 9;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tpAbsPos
            // 
            this.tpAbsPos.Controls.Add(this.tbCurrPos);
            this.tpAbsPos.Location = new System.Drawing.Point(4, 22);
            this.tpAbsPos.Name = "tpAbsPos";
            this.tpAbsPos.Padding = new System.Windows.Forms.Padding(3);
            this.tpAbsPos.Size = new System.Drawing.Size(145, 35);
            this.tpAbsPos.TabIndex = 0;
            this.tpAbsPos.Text = "Abs Pos";
            this.tpAbsPos.UseVisualStyleBackColor = true;
            // 
            // tbCurrPos
            // 
            this.tbCurrPos.BackColor = System.Drawing.SystemColors.WindowText;
            this.tbCurrPos.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbCurrPos.ForeColor = System.Drawing.Color.Gold;
            this.tbCurrPos.Location = new System.Drawing.Point(6, 4);
            this.tbCurrPos.Name = "tbCurrPos";
            this.tbCurrPos.Size = new System.Drawing.Size(133, 26);
            this.tbCurrPos.TabIndex = 3;
            this.tbCurrPos.Text = "1234.56";
            // 
            // tpAccumRelMov
            // 
            this.tpAccumRelMov.Controls.Add(this.tbAccumulatedRelMov);
            this.tpAccumRelMov.Location = new System.Drawing.Point(4, 22);
            this.tpAccumRelMov.Name = "tpAccumRelMov";
            this.tpAccumRelMov.Padding = new System.Windows.Forms.Padding(3);
            this.tpAccumRelMov.Size = new System.Drawing.Size(145, 35);
            this.tpAccumRelMov.TabIndex = 1;
            this.tpAccumRelMov.Text = "Accum Rel Mov";
            this.tpAccumRelMov.UseVisualStyleBackColor = true;
            // 
            // tbAccumulatedRelMov
            // 
            this.tbAccumulatedRelMov.BackColor = System.Drawing.Color.ForestGreen;
            this.tbAccumulatedRelMov.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbAccumulatedRelMov.ForeColor = System.Drawing.Color.White;
            this.tbAccumulatedRelMov.Location = new System.Drawing.Point(6, 5);
            this.tbAccumulatedRelMov.Name = "tbAccumulatedRelMov";
            this.tbAccumulatedRelMov.ReadOnly = true;
            this.tbAccumulatedRelMov.Size = new System.Drawing.Size(133, 26);
            this.tbAccumulatedRelMov.TabIndex = 0;
            this.tbAccumulatedRelMov.Text = "123.45";
            // 
            // pbPositiveDirection
            // 
            this.pbPositiveDirection.Location = new System.Drawing.Point(162, 41);
            this.pbPositiveDirection.Name = "pbPositiveDirection";
            this.pbPositiveDirection.Size = new System.Drawing.Size(30, 30);
            this.pbPositiveDirection.TabIndex = 8;
            this.pbPositiveDirection.TabStop = false;
            // 
            // btnStepPositive
            // 
            this.btnStepPositive.Location = new System.Drawing.Point(162, 83);
            this.btnStepPositive.Name = "btnStepPositive";
            this.btnStepPositive.Size = new System.Drawing.Size(29, 23);
            this.btnStepPositive.TabIndex = 7;
            this.btnStepPositive.Text = "+";
            this.btnStepPositive.UseVisualStyleBackColor = true;
            this.btnStepPositive.Click += new System.EventHandler(this.btnStepPositive_Click);
            // 
            // lblRelMove
            // 
            this.lblRelMove.AutoSize = true;
            this.lblRelMove.Location = new System.Drawing.Point(7, 88);
            this.lblRelMove.Name = "lblRelMove";
            this.lblRelMove.Size = new System.Drawing.Size(47, 12);
            this.lblRelMove.TabIndex = 6;
            this.lblRelMove.Text = "Rel Mov";
            this.lblRelMove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbRelMov
            // 
            this.tbRelMov.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRelMov.Location = new System.Drawing.Point(91, 81);
            this.tbRelMov.Name = "tbRelMov";
            this.tbRelMov.Size = new System.Drawing.Size(68, 26);
            this.tbRelMov.TabIndex = 5;
            this.tbRelMov.Enter += new System.EventHandler(this.tbRelMov_Enter);
            this.tbRelMov.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbRelMov_KeyUp);
            this.tbRelMov.Leave += new System.EventHandler(this.tbRelMov_Leave);
            // 
            // btnStepNegative
            // 
            this.btnStepNegative.Location = new System.Drawing.Point(58, 83);
            this.btnStepNegative.Name = "btnStepNegative";
            this.btnStepNegative.Size = new System.Drawing.Size(29, 23);
            this.btnStepNegative.TabIndex = 4;
            this.btnStepNegative.Text = "-";
            this.btnStepNegative.UseVisualStyleBackColor = true;
            this.btnStepNegative.Click += new System.EventHandler(this.btnStepNegative_Click);
            // 
            // lblAxisId
            // 
            this.lblAxisId.AutoSize = true;
            this.lblAxisId.BackColor = System.Drawing.Color.Yellow;
            this.lblAxisId.Location = new System.Drawing.Point(37, 0);
            this.lblAxisId.Name = "lblAxisId";
            this.lblAxisId.Size = new System.Drawing.Size(41, 12);
            this.lblAxisId.TabIndex = 0;
            this.lblAxisId.Text = "      ";
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMoveAbsolute,
            this.toolStripSeparator1,
            this.miSetMotionSpeed,
            this.toolStripSeparator2,
            this.miSetMotionRange,
            this.toolStripSeparator3,
            this.miHoming,
            this.toolStripSeparator4,
            this.miStop});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(182, 160);
            // 
            // miMoveAbsolute
            // 
            this.miMoveAbsolute.Name = "miMoveAbsolute";
            this.miMoveAbsolute.Size = new System.Drawing.Size(181, 22);
            this.miMoveAbsolute.Text = "Move Absolute";
            this.miMoveAbsolute.Click += new System.EventHandler(this.miMoveAbsolute_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
            // 
            // miSetMotionSpeed
            // 
            this.miSetMotionSpeed.Name = "miSetMotionSpeed";
            this.miSetMotionSpeed.Size = new System.Drawing.Size(181, 22);
            this.miSetMotionSpeed.Text = "Set Motion Speed";
            this.miSetMotionSpeed.Click += new System.EventHandler(this.miSetMotionSpeed_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(178, 6);
            // 
            // miSetMotionRange
            // 
            this.miSetMotionRange.Name = "miSetMotionRange";
            this.miSetMotionRange.Size = new System.Drawing.Size(181, 22);
            this.miSetMotionRange.Text = "Set Motion Range";
            this.miSetMotionRange.Click += new System.EventHandler(this.miSetMotionRange_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(178, 6);
            // 
            // miHoming
            // 
            this.miHoming.Name = "miHoming";
            this.miHoming.Size = new System.Drawing.Size(181, 22);
            this.miHoming.Text = "Homing";
            this.miHoming.Click += new System.EventHandler(this.miHoming_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(178, 6);
            // 
            // miStop
            // 
            this.miStop.Name = "miStop";
            this.miStop.Size = new System.Drawing.Size(181, 22);
            this.miStop.Text = "Stop";
            this.miStop.Click += new System.EventHandler(this.miStop_Click);
            // 
            // UcStageAxis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenu;
            this.Controls.Add(this.gbBase);
            this.Name = "UcStageAxis";
            this.Size = new System.Drawing.Size(200, 117);
            this.Load += new System.EventHandler(this.ucStageAxis_Load);
            this.gbBase.ResumeLayout(false);
            this.gbBase.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tpAbsPos.ResumeLayout(false);
            this.tpAbsPos.PerformLayout();
            this.tpAccumRelMov.ResumeLayout(false);
            this.tpAccumRelMov.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPositiveDirection)).EndInit();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbBase;
        private System.Windows.Forms.Label lblAxisId;
        private System.Windows.Forms.Button btnStepNegative;
        private System.Windows.Forms.TextBox tbRelMov;
        private System.Windows.Forms.Label lblRelMove;
        private System.Windows.Forms.Button btnStepPositive;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem miMoveAbsolute;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miSetMotionSpeed;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem miSetMotionRange;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem miHoming;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem miStop;
        private System.Windows.Forms.PictureBox pbPositiveDirection;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpAbsPos;
        private System.Windows.Forms.TabPage tpAccumRelMov;
        private System.Windows.Forms.TextBox tbCurrPos;
        private System.Windows.Forms.TextBox tbAccumulatedRelMov;
    }
}
