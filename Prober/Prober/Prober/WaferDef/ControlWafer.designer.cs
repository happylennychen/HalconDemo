using Prober;

namespace Prober.WaferDef {
    partial class ControlWafer
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.table_Main = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_HeightVerify = new System.Windows.Forms.Button();
            this.btn_Origin = new System.Windows.Forms.Button();
            this.btn_OrderZColRight = new System.Windows.Forms.Button();
            this.btn_OrderZColLeft = new System.Windows.Forms.Button();
            this.btn_OrderZRowLeft = new System.Windows.Forms.Button();
            this.btn_OrderZRowRight = new System.Windows.Forms.Button();
            this.cbox_WaferType = new System.Windows.Forms.ComboBox();
            this.btn_Download = new System.Windows.Forms.Button();
            this.btn_Upload = new System.Windows.Forms.Button();
            this.lbl_WaferType = new System.Windows.Forms.Label();
            this.btn_HalfDieSet = new System.Windows.Forms.Button();
            this.btn_goto = new System.Windows.Forms.Button();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.btn_Active = new System.Windows.Forms.Button();
            this.btn_Home = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // table_Main
            // 
            this.table_Main.BackColor = System.Drawing.Color.White;
            this.table_Main.ColumnCount = 4;
            this.table_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.table_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.table_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.table_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.table_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table_Main.Location = new System.Drawing.Point(0, 41);
            this.table_Main.Name = "table_Main";
            this.table_Main.RowCount = 2;
            this.table_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.table_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.table_Main.Size = new System.Drawing.Size(759, 652);
            this.table_Main.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.panel1.Controls.Add(this.btn_HeightVerify);
            this.panel1.Controls.Add(this.btn_Origin);
            this.panel1.Controls.Add(this.btn_OrderZColRight);
            this.panel1.Controls.Add(this.btn_OrderZColLeft);
            this.panel1.Controls.Add(this.btn_OrderZRowLeft);
            this.panel1.Controls.Add(this.btn_OrderZRowRight);
            this.panel1.Controls.Add(this.cbox_WaferType);
            this.panel1.Controls.Add(this.btn_Download);
            this.panel1.Controls.Add(this.btn_Upload);
            this.panel1.Controls.Add(this.lbl_WaferType);
            this.panel1.Controls.Add(this.btn_HalfDieSet);
            this.panel1.Controls.Add(this.btn_goto);
            this.panel1.Controls.Add(this.btn_Delete);
            this.panel1.Controls.Add(this.btn_Active);
            this.panel1.Controls.Add(this.btn_Home);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(759, 41);
            this.panel1.TabIndex = 1;
            this.toolTip1.SetToolTip(this.panel1, "颜色标记");
            // 
            // btn_HeightVerify
            // 
            this.btn_HeightVerify.BackgroundImage = global::Prober.Properties.Resources.WaveForm;
            this.btn_HeightVerify.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_HeightVerify.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_HeightVerify.Location = new System.Drawing.Point(381, 4);
            this.btn_HeightVerify.Name = "btn_HeightVerify";
            this.btn_HeightVerify.Size = new System.Drawing.Size(32, 32);
            this.btn_HeightVerify.TabIndex = 22;
            this.toolTip1.SetToolTip(this.btn_HeightVerify, "高度校验");
            this.btn_HeightVerify.UseVisualStyleBackColor = true;
            this.btn_HeightVerify.Click += new System.EventHandler(this.btn_HeightVerify_Click);
            // 
            // btn_Origin
            // 
            this.btn_Origin.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_Origin.BackgroundImage = global::Prober.Properties.Resources.定位;
            this.btn_Origin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Origin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Origin.Location = new System.Drawing.Point(45, 6);
            this.btn_Origin.Name = "btn_Origin";
            this.btn_Origin.Size = new System.Drawing.Size(32, 32);
            this.btn_Origin.TabIndex = 21;
            this.toolTip1.SetToolTip(this.btn_Origin, "回到标记点");
            this.btn_Origin.UseVisualStyleBackColor = false;
            this.btn_Origin.Click += new System.EventHandler(this.btn_Origin_Click);
            // 
            // btn_OrderZColRight
            // 
            this.btn_OrderZColRight.BackgroundImage = global::Prober.Properties.Resources.TestOrder1;
            this.btn_OrderZColRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OrderZColRight.Location = new System.Drawing.Point(340, 4);
            this.btn_OrderZColRight.Name = "btn_OrderZColRight";
            this.btn_OrderZColRight.Size = new System.Drawing.Size(32, 32);
            this.btn_OrderZColRight.TabIndex = 20;
            this.toolTip1.SetToolTip(this.btn_OrderZColRight, "从右上到左下");
            this.btn_OrderZColRight.UseVisualStyleBackColor = true;
            this.btn_OrderZColRight.Click += new System.EventHandler(this.btn_OrderZColRight_Click);
            // 
            // btn_OrderZColLeft
            // 
            this.btn_OrderZColLeft.BackgroundImage = global::Prober.Properties.Resources.TestOrder2;
            this.btn_OrderZColLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OrderZColLeft.Location = new System.Drawing.Point(306, 4);
            this.btn_OrderZColLeft.Name = "btn_OrderZColLeft";
            this.btn_OrderZColLeft.Size = new System.Drawing.Size(32, 32);
            this.btn_OrderZColLeft.TabIndex = 20;
            this.toolTip1.SetToolTip(this.btn_OrderZColLeft, "左上到右下");
            this.btn_OrderZColLeft.UseVisualStyleBackColor = true;
            this.btn_OrderZColLeft.Click += new System.EventHandler(this.btn_OrderZColLeft_Click);
            // 
            // btn_OrderZRowLeft
            // 
            this.btn_OrderZRowLeft.BackgroundImage = global::Prober.Properties.Resources.TestOrder4;
            this.btn_OrderZRowLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OrderZRowLeft.Location = new System.Drawing.Point(272, 4);
            this.btn_OrderZRowLeft.Name = "btn_OrderZRowLeft";
            this.btn_OrderZRowLeft.Size = new System.Drawing.Size(32, 32);
            this.btn_OrderZRowLeft.TabIndex = 20;
            this.toolTip1.SetToolTip(this.btn_OrderZRowLeft, "从右上到左下");
            this.btn_OrderZRowLeft.UseVisualStyleBackColor = true;
            this.btn_OrderZRowLeft.Click += new System.EventHandler(this.btn_OrderZRowLeft_Click);
            // 
            // btn_OrderZRowRight
            // 
            this.btn_OrderZRowRight.BackgroundImage = global::Prober.Properties.Resources.TestOrder3;
            this.btn_OrderZRowRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OrderZRowRight.Location = new System.Drawing.Point(238, 4);
            this.btn_OrderZRowRight.Name = "btn_OrderZRowRight";
            this.btn_OrderZRowRight.Size = new System.Drawing.Size(32, 32);
            this.btn_OrderZRowRight.TabIndex = 20;
            this.toolTip1.SetToolTip(this.btn_OrderZRowRight, "从左上到右下");
            this.btn_OrderZRowRight.UseVisualStyleBackColor = true;
            this.btn_OrderZRowRight.Click += new System.EventHandler(this.btn_OrderZRowRight_Click);
            // 
            // cbox_WaferType
            // 
            this.cbox_WaferType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbox_WaferType.FormattingEnabled = true;
            this.cbox_WaferType.Location = new System.Drawing.Point(501, 8);
            this.cbox_WaferType.Name = "cbox_WaferType";
            this.cbox_WaferType.Size = new System.Drawing.Size(167, 24);
            this.cbox_WaferType.TabIndex = 19;
            this.toolTip1.SetToolTip(this.cbox_WaferType, "晶圆类型选择");
            this.cbox_WaferType.SelectedIndexChanged += new System.EventHandler(this.cbox_WaferType_SelectedIndexChanged);
            // 
            // btn_Download
            // 
            this.btn_Download.BackgroundImage = global::Prober.Properties.Resources.download;
            this.btn_Download.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Download.Location = new System.Drawing.Point(714, 3);
            this.btn_Download.Name = "btn_Download";
            this.btn_Download.Size = new System.Drawing.Size(32, 32);
            this.btn_Download.TabIndex = 18;
            this.toolTip1.SetToolTip(this.btn_Download, "下料");
            this.btn_Download.UseVisualStyleBackColor = true;
            this.btn_Download.Click += new System.EventHandler(this.btn_Download_Click);
            // 
            // btn_Upload
            // 
            this.btn_Upload.BackgroundImage = global::Prober.Properties.Resources.upload__1_;
            this.btn_Upload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Upload.Location = new System.Drawing.Point(676, 3);
            this.btn_Upload.Name = "btn_Upload";
            this.btn_Upload.Size = new System.Drawing.Size(32, 32);
            this.btn_Upload.TabIndex = 18;
            this.toolTip1.SetToolTip(this.btn_Upload, "上料");
            this.btn_Upload.UseVisualStyleBackColor = true;
            this.btn_Upload.Click += new System.EventHandler(this.btn_Upload_Click);
            // 
            // lbl_WaferType
            // 
            this.lbl_WaferType.AutoSize = true;
            this.lbl_WaferType.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_WaferType.Location = new System.Drawing.Point(417, 11);
            this.lbl_WaferType.Name = "lbl_WaferType";
            this.lbl_WaferType.Size = new System.Drawing.Size(78, 17);
            this.lbl_WaferType.TabIndex = 17;
            this.lbl_WaferType.Text = "Wafer Type";
            // 
            // btn_HalfDieSet
            // 
            this.btn_HalfDieSet.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_HalfDieSet.BackgroundImage = global::Prober.Properties.Resources.标记;
            this.btn_HalfDieSet.Location = new System.Drawing.Point(196, 6);
            this.btn_HalfDieSet.Name = "btn_HalfDieSet";
            this.btn_HalfDieSet.Size = new System.Drawing.Size(32, 32);
            this.btn_HalfDieSet.TabIndex = 16;
            this.toolTip1.SetToolTip(this.btn_HalfDieSet, "设定晶圆上不完整Reticle");
            this.btn_HalfDieSet.UseVisualStyleBackColor = false;
            this.btn_HalfDieSet.Click += new System.EventHandler(this.btn_HalfDieSet_Click);
            // 
            // btn_goto
            // 
            this.btn_goto.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_goto.BackgroundImage = global::Prober.Properties.Resources.size;
            this.btn_goto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_goto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_goto.Location = new System.Drawing.Point(8, 6);
            this.btn_goto.Name = "btn_goto";
            this.btn_goto.Size = new System.Drawing.Size(32, 32);
            this.btn_goto.TabIndex = 14;
            this.toolTip1.SetToolTip(this.btn_goto, "Retical Goto功能");
            this.btn_goto.UseVisualStyleBackColor = false;
            this.btn_goto.Click += new System.EventHandler(this.btn_goto_Click);
            // 
            // btn_Delete
            // 
            this.btn_Delete.BackgroundImage = global::Prober.Properties.Resources.删除;
            this.btn_Delete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Delete.Location = new System.Drawing.Point(158, 6);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(32, 32);
            this.btn_Delete.TabIndex = 13;
            this.btn_Delete.Tag = "删除Retical";
            this.toolTip1.SetToolTip(this.btn_Delete, "禁用Map上的芯片");
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // btn_Active
            // 
            this.btn_Active.BackgroundImage = global::Prober.Properties.Resources.启用;
            this.btn_Active.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Active.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            this.btn_Active.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.btn_Active.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Active.Location = new System.Drawing.Point(120, 6);
            this.btn_Active.Name = "btn_Active";
            this.btn_Active.Size = new System.Drawing.Size(32, 32);
            this.btn_Active.TabIndex = 12;
            this.btn_Active.Tag = "加载本地测试项目";
            this.toolTip1.SetToolTip(this.btn_Active, "激活Map上的芯片");
            this.btn_Active.UseVisualStyleBackColor = true;
            this.btn_Active.Click += new System.EventHandler(this.btn_Active_Click);
            // 
            // btn_Home
            // 
            this.btn_Home.BackgroundImage = global::Prober.Properties.Resources.origin;
            this.btn_Home.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Home.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            this.btn_Home.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.btn_Home.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Home.Location = new System.Drawing.Point(80, 6);
            this.btn_Home.Name = "btn_Home";
            this.btn_Home.Size = new System.Drawing.Size(32, 32);
            this.btn_Home.TabIndex = 11;
            this.btn_Home.Tag = "加载本地测试项目";
            this.toolTip1.SetToolTip(this.btn_Home, "设定晶圆坐标基准位置");
            this.btn_Home.UseVisualStyleBackColor = true;
            this.btn_Home.Click += new System.EventHandler(this.btn_Home_Click);
            // 
            // ControlWafer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.table_Main);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ControlWafer";
            this.Size = new System.Drawing.Size(759, 693);
            this.Load += new System.EventHandler(this.ControlWafer_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel table_Main;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btn_Home;
        private System.Windows.Forms.Button btn_Active;
        private System.Windows.Forms.Button btn_goto;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.Button btn_HalfDieSet;
        private System.Windows.Forms.Button btn_Download;
        private System.Windows.Forms.Button btn_Upload;
        private System.Windows.Forms.ComboBox cbox_WaferType;
        private System.Windows.Forms.Label lbl_WaferType;
        private System.Windows.Forms.Button btn_OrderZColRight;
        private System.Windows.Forms.Button btn_OrderZColLeft;
        private System.Windows.Forms.Button btn_OrderZRowLeft;
        private System.Windows.Forms.Button btn_OrderZRowRight;
        private System.Windows.Forms.Button btn_Origin;
        private System.Windows.Forms.Button btn_HeightVerify;
    }
}
