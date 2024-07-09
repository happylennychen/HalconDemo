namespace Prober.WaferDef {
    partial class ControlDie
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
            this.lbl_Name = new System.Windows.Forms.Label();
            this.lbl_Ordinate = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_Reference = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_Name
            // 
            this.lbl_Name.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(168)))), ((int)(((byte)(220)))));
            this.lbl_Name.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_Name.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Name.Location = new System.Drawing.Point(0, 0);
            this.lbl_Name.Name = "lbl_Name";
            this.lbl_Name.Size = new System.Drawing.Size(74, 20);
            this.lbl_Name.TabIndex = 0;
            this.lbl_Name.Text = "1#";
            this.lbl_Name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Ordinate
            // 
            this.lbl_Ordinate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(168)))), ((int)(((byte)(220)))));
            this.lbl_Ordinate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Ordinate.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Ordinate.Location = new System.Drawing.Point(0, 20);
            this.lbl_Ordinate.Name = "lbl_Ordinate";
            this.lbl_Ordinate.Size = new System.Drawing.Size(74, 54);
            this.lbl_Ordinate.TabIndex = 1;
            this.lbl_Ordinate.Text = "(1,1)";
            this.lbl_Ordinate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_Ordinate.Click += new System.EventHandler(this.lbl_Ordinate_Click);
            this.lbl_Ordinate.MouseEnter += new System.EventHandler(this.lbl_Ordinate_MouseEnter);
            this.lbl_Ordinate.MouseLeave += new System.EventHandler(this.lbl_Ordinate_MouseLeave);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbl_Reference);
            this.panel1.Controls.Add(this.lbl_Ordinate);
            this.panel1.Controls.Add(this.lbl_Name);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(74, 74);
            this.panel1.TabIndex = 2;
            // 
            // lbl_Reference
            // 
            this.lbl_Reference.BackColor = System.Drawing.Color.White;
            this.lbl_Reference.Location = new System.Drawing.Point(57, 58);
            this.lbl_Reference.Name = "lbl_Reference";
            this.lbl_Reference.Size = new System.Drawing.Size(16, 16);
            this.lbl_Reference.TabIndex = 2;
            this.lbl_Reference.Text = "R";
            this.lbl_Reference.Visible = false;
            // 
            // ControlDie
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(168)))), ((int)(((byte)(220)))));
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ControlDie";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(80, 80);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_Name;
        private System.Windows.Forms.Label lbl_Ordinate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_Reference;
    }
}
