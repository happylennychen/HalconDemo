namespace SimulatedInstrument {
    partial class FormMain {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            components = new System.ComponentModel.Container();
            label1 = new Label();
            tbIdn = new TextBox();
            label2 = new Label();
            tbIp = new TextBox();
            tbPort = new TextBox();
            label3 = new Label();
            btnListening = new Button();
            rtbMsgBox = new RichTextBox();
            contextMenu = new ContextMenuStrip(components);
            cmiClearAll = new ToolStripMenuItem();
            contextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 23);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(36, 17);
            label1.TabIndex = 0;
            label1.Text = "*IDN";
            // 
            // tbIdn
            // 
            tbIdn.Location = new Point(64, 20);
            tbIdn.Margin = new Padding(2, 3, 2, 3);
            tbIdn.Name = "tbIdn";
            tbIdn.Size = new Size(478, 23);
            tbIdn.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(42, 51);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(19, 17);
            label2.TabIndex = 2;
            label2.Text = "IP";
            // 
            // tbIp
            // 
            tbIp.Location = new Point(64, 48);
            tbIp.Margin = new Padding(2, 3, 2, 3);
            tbIp.Name = "tbIp";
            tbIp.Size = new Size(99, 23);
            tbIp.TabIndex = 3;
            // 
            // tbPort
            // 
            tbPort.Location = new Point(64, 76);
            tbPort.Margin = new Padding(2, 3, 2, 3);
            tbPort.Name = "tbPort";
            tbPort.Size = new Size(99, 23);
            tbPort.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(28, 79);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(32, 17);
            label3.TabIndex = 5;
            label3.Text = "Port";
            // 
            // btnListening
            // 
            btnListening.Location = new Point(173, 76);
            btnListening.Margin = new Padding(2, 3, 2, 3);
            btnListening.Name = "btnListening";
            btnListening.Size = new Size(93, 25);
            btnListening.TabIndex = 6;
            btnListening.Text = "Listening...";
            btnListening.UseVisualStyleBackColor = true;
            btnListening.Click += btnInitialize_Click;
            // 
            // rtbMsgBox
            // 
            rtbMsgBox.BackColor = SystemColors.ControlLight;
            rtbMsgBox.Dock = DockStyle.Bottom;
            rtbMsgBox.Location = new Point(0, 114);
            rtbMsgBox.Margin = new Padding(2, 3, 2, 3);
            rtbMsgBox.Name = "rtbMsgBox";
            rtbMsgBox.ReadOnly = true;
            rtbMsgBox.Size = new Size(582, 366);
            rtbMsgBox.TabIndex = 7;
            rtbMsgBox.Text = "";
            rtbMsgBox.TextChanged += rtbMsgBox_TextChanged;
            // 
            // contextMenu
            // 
            contextMenu.Items.AddRange(new ToolStripItem[] { cmiClearAll });
            contextMenu.Name = "contextMenu";
            contextMenu.Size = new Size(181, 48);
            // 
            // cmiClearAll
            // 
            cmiClearAll.Name = "cmiClearAll";
            cmiClearAll.Size = new Size(180, 22);
            cmiClearAll.Text = "Clear All";
            cmiClearAll.Click += cmiClearAll_Click;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(582, 480);
            Controls.Add(rtbMsgBox);
            Controls.Add(btnListening);
            Controls.Add(label3);
            Controls.Add(tbPort);
            Controls.Add(tbIp);
            Controls.Add(label2);
            Controls.Add(tbIdn);
            Controls.Add(label1);
            Margin = new Padding(2, 3, 2, 3);
            Name = "FormMain";
            Text = "Simulate XXX";
            Load += Form1_Load;
            contextMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox tbIdn;
        private Label label2;
        private TextBox tbIp;
        private TextBox tbPort;
        private Label label3;
        private Button btnListening;
        private RichTextBox rtbMsgBox;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem cmiClearAll;
    }
}