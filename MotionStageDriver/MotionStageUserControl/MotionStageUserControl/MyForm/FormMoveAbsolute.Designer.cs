namespace MyMotionStageUserControl.MyForm {
    partial class FormMoveAbsolute {
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
            this.lblAbsPos = new System.Windows.Forms.Label();
            this.tbAbsPos = new System.Windows.Forms.TextBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblAbsPos
            // 
            this.lblAbsPos.AutoSize = true;
            this.lblAbsPos.Location = new System.Drawing.Point(12, 22);
            this.lblAbsPos.Name = "lblAbsPos";
            this.lblAbsPos.Size = new System.Drawing.Size(47, 12);
            this.lblAbsPos.TabIndex = 0;
            this.lblAbsPos.Text = "Abs Pos";
            // 
            // tbAbsPos
            // 
            this.tbAbsPos.Location = new System.Drawing.Point(65, 19);
            this.tbAbsPos.Name = "tbAbsPos";
            this.tbAbsPos.Size = new System.Drawing.Size(158, 21);
            this.tbAbsPos.TabIndex = 1;
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(229, 22);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(17, 12);
            this.lblUnit.TabIndex = 2;
            this.lblUnit.Text = "um";
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(252, 17);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 3;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // FormMoveAbsolute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 61);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.tbAbsPos);
            this.Controls.Add(this.lblAbsPos);
            this.Name = "FormMoveAbsolute";
            this.Text = "Move Absolute";
            this.Load += new System.EventHandler(this.FormMoveAbsolute_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAbsPos;
        private System.Windows.Forms.TextBox tbAbsPos;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.Button btnGo;
    }
}