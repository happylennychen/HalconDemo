namespace ProberApi.MyForm {
    partial class FormCoupling {
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
            this.tcCoupling = new System.Windows.Forms.TabControl();
            this.tpCross2d = new System.Windows.Forms.TabPage();
            this.tpSpiral2d = new System.Windows.Forms.TabPage();
            this.panelCross2D = new System.Windows.Forms.Panel();
            this.panelSpiral2D = new System.Windows.Forms.Panel();
            this.tcCoupling.SuspendLayout();
            this.tpCross2d.SuspendLayout();
            this.tpSpiral2d.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcCoupling
            // 
            this.tcCoupling.Controls.Add(this.tpCross2d);
            this.tcCoupling.Controls.Add(this.tpSpiral2d);
            this.tcCoupling.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcCoupling.Location = new System.Drawing.Point(0, 0);
            this.tcCoupling.Name = "tcCoupling";
            this.tcCoupling.SelectedIndex = 0;
            this.tcCoupling.Size = new System.Drawing.Size(1084, 361);
            this.tcCoupling.TabIndex = 0;
            // 
            // tpCross2d
            // 
            this.tpCross2d.Controls.Add(this.panelCross2D);
            this.tpCross2d.Location = new System.Drawing.Point(4, 22);
            this.tpCross2d.Name = "tpCross2d";
            this.tpCross2d.Padding = new System.Windows.Forms.Padding(3);
            this.tpCross2d.Size = new System.Drawing.Size(1076, 335);
            this.tpCross2d.TabIndex = 0;
            this.tpCross2d.Text = "Cross Coupling 2D";
            this.tpCross2d.UseVisualStyleBackColor = true;
            // 
            // tpSpiral2d
            // 
            this.tpSpiral2d.Controls.Add(this.panelSpiral2D);
            this.tpSpiral2d.Location = new System.Drawing.Point(4, 22);
            this.tpSpiral2d.Name = "tpSpiral2d";
            this.tpSpiral2d.Padding = new System.Windows.Forms.Padding(3);
            this.tpSpiral2d.Size = new System.Drawing.Size(1076, 335);
            this.tpSpiral2d.TabIndex = 1;
            this.tpSpiral2d.Text = "Spiral Coupling 2D";
            this.tpSpiral2d.UseVisualStyleBackColor = true;
            // 
            // panelCross2D
            // 
            this.panelCross2D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCross2D.Location = new System.Drawing.Point(3, 3);
            this.panelCross2D.Name = "panelCross2D";
            this.panelCross2D.Size = new System.Drawing.Size(1070, 329);
            this.panelCross2D.TabIndex = 0;
            // 
            // panelSpiral2D
            // 
            this.panelSpiral2D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSpiral2D.Location = new System.Drawing.Point(3, 3);
            this.panelSpiral2D.Name = "panelSpiral2D";
            this.panelSpiral2D.Size = new System.Drawing.Size(1070, 329);
            this.panelSpiral2D.TabIndex = 0;
            // 
            // FormCoupling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 361);
            this.Controls.Add(this.tcCoupling);
            this.Name = "FormCoupling";
            this.Text = "FormCoupling";
            this.Load += new System.EventHandler(this.FormCoupling_Load);
            this.tcCoupling.ResumeLayout(false);
            this.tpCross2d.ResumeLayout(false);
            this.tpSpiral2d.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcCoupling;
        private System.Windows.Forms.TabPage tpCross2d;
        private System.Windows.Forms.TabPage tpSpiral2d;
        private System.Windows.Forms.Panel panelCross2D;
        private System.Windows.Forms.Panel panelSpiral2D;
    }
}