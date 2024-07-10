namespace Prober {
    partial class FormCamera
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCamera));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel_Camera = new System.Windows.Forms.TableLayoutPanel();
            this.panel_top = new System.Windows.Forms.Panel();
            this.hw_Top = new Prober.MyControl.ControlCamera();
            this.panel_Side = new System.Windows.Forms.Panel();
            this.tabCtrl_Side = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.hw_Front = new Prober.MyControl.ControlCamera();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.hw_Rear = new Prober.MyControl.ControlCamera();
            this.panel_Camera = new System.Windows.Forms.Panel();
            this.tableLayoutPanel_Camera.SuspendLayout();
            this.panel_top.SuspendLayout();
            this.panel_Side.SuspendLayout();
            this.tabCtrl_Side.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel_Camera.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel_Camera
            // 
            this.tableLayoutPanel_Camera.ColumnCount = 2;
            this.tableLayoutPanel_Camera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Camera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Camera.Controls.Add(this.panel_top, 0, 0);
            this.tableLayoutPanel_Camera.Controls.Add(this.panel_Side, 1, 0);
            this.tableLayoutPanel_Camera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_Camera.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_Camera.Name = "tableLayoutPanel_Camera";
            this.tableLayoutPanel_Camera.RowCount = 1;
            this.tableLayoutPanel_Camera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Camera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Camera.Size = new System.Drawing.Size(1920, 856);
            this.tableLayoutPanel_Camera.TabIndex = 0;
            // 
            // panel_top
            // 
            this.panel_top.Controls.Add(this.hw_Top);
            this.panel_top.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_top.Location = new System.Drawing.Point(3, 3);
            this.panel_top.Name = "panel_top";
            this.panel_top.Size = new System.Drawing.Size(954, 850);
            this.panel_top.TabIndex = 2;
            // 
            // hw_Top
            // 
            this.hw_Top.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hw_Top.Location = new System.Drawing.Point(0, 0);
            this.hw_Top.Name = "hw_Top";
            this.hw_Top.Size = new System.Drawing.Size(954, 850);
            this.hw_Top.TabIndex = 0;
            // 
            // panel_Side
            // 
            this.panel_Side.Controls.Add(this.tabCtrl_Side);
            this.panel_Side.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Side.Location = new System.Drawing.Point(963, 3);
            this.panel_Side.Name = "panel_Side";
            this.panel_Side.Size = new System.Drawing.Size(954, 850);
            this.panel_Side.TabIndex = 3;
            // 
            // tabCtrl_Side
            // 
            this.tabCtrl_Side.Controls.Add(this.tabPage1);
            this.tabCtrl_Side.Controls.Add(this.tabPage2);
            this.tabCtrl_Side.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrl_Side.Location = new System.Drawing.Point(0, 0);
            this.tabCtrl_Side.Name = "tabCtrl_Side";
            this.tabCtrl_Side.SelectedIndex = 0;
            this.tabCtrl_Side.Size = new System.Drawing.Size(954, 850);
            this.tabCtrl_Side.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.hw_Front);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(946, 824);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Front";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // hw_Front
            // 
            this.hw_Front.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hw_Front.Location = new System.Drawing.Point(3, 3);
            this.hw_Front.Name = "hw_Front";
            this.hw_Front.Size = new System.Drawing.Size(940, 818);
            this.hw_Front.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.hw_Rear);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(946, 824);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Rear";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // hw_Rear
            // 
            this.hw_Rear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hw_Rear.Location = new System.Drawing.Point(3, 3);
            this.hw_Rear.Name = "hw_Rear";
            this.hw_Rear.Size = new System.Drawing.Size(940, 818);
            this.hw_Rear.TabIndex = 0;
            // 
            // panel_Camera
            // 
            this.panel_Camera.Controls.Add(this.tableLayoutPanel_Camera);
            this.panel_Camera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Camera.Location = new System.Drawing.Point(0, 0);
            this.panel_Camera.Name = "panel_Camera";
            this.panel_Camera.Size = new System.Drawing.Size(1920, 856);
            this.panel_Camera.TabIndex = 1;
            // 
            // FormCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 856);
            this.Controls.Add(this.panel_Camera);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCamera";
            this.Text = "Camera";
            this.Load += new System.EventHandler(this.FrmCamera_Load);
            this.tableLayoutPanel_Camera.ResumeLayout(false);
            this.panel_top.ResumeLayout(false);
            this.panel_Side.ResumeLayout(false);
            this.tabCtrl_Side.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel_Camera.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Camera;
        private System.Windows.Forms.Panel panel_Camera;
        private System.Windows.Forms.Panel panel_top;
        public MyControl.ControlCamera hw_Top;
        private System.Windows.Forms.Panel panel_Side;
        private MyControl.ControlCamera hw_Front;
        private System.Windows.Forms.TabControl tabCtrl_Side;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private MyControl.ControlCamera hw_Rear;
    }
}