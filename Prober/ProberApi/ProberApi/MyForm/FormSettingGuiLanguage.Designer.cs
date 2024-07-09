namespace ProberApi.MyForm {
    partial class FormSettingGuiLanguage {
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
            this.lblLanguage = new System.Windows.Forms.Label();
            this.cmbAllSupportedLanguages = new System.Windows.Forms.ComboBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblLanguage
            // 
            this.lblLanguage.Location = new System.Drawing.Point(15, 21);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(88, 18);
            this.lblLanguage.TabIndex = 0;
            this.lblLanguage.Text = "Language";
            this.lblLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbAllSupportedLanguages
            // 
            this.cmbAllSupportedLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAllSupportedLanguages.FormattingEnabled = true;
            this.cmbAllSupportedLanguages.Location = new System.Drawing.Point(109, 21);
            this.cmbAllSupportedLanguages.Name = "cmbAllSupportedLanguages";
            this.cmbAllSupportedLanguages.Size = new System.Drawing.Size(142, 20);
            this.cmbAllSupportedLanguages.TabIndex = 1;
            this.cmbAllSupportedLanguages.SelectedIndexChanged += new System.EventHandler(this.cmbAllSupportedLanguages_SelectedIndexChanged);
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(257, 19);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 23);
            this.btnSet.TabIndex = 2;
            this.btnSet.Text = "PrepareTrigger";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // FormSettingGuiLanguage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 61);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.cmbAllSupportedLanguages);
            this.Controls.Add(this.lblLanguage);
            this.Name = "FormSettingGuiLanguage";
            this.Text = "FormSettingGuiLanguage";
            this.Load += new System.EventHandler(this.FormSettingGuiLanguage_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.ComboBox cmbAllSupportedLanguages;
        private System.Windows.Forms.Button btnSet;
    }
}