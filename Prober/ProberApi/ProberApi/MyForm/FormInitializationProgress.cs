using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using CommonApi.MyI18N;

namespace Prober.Forms {
    public partial class FormInitializationProgress  : Form {
        public FormInitializationProgress(EnumLanguage language, List<string> titleList, Bitmap indicatingArrow) {
            InitializeComponent();            
            this.titleList = titleList;
            this.indicatingArrow = indicatingArrow;
            this.ControlBox = false;
            frc = new FormResourceCulture(this.GetType().FullName, Assembly.GetExecutingAssembly());
            frc.Language = language;
        }

        private void FormInitializationProgress_Load(object sender, EventArgs e) {
            frc.EnumerateControl(this);

            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.RowCount = titleList.Count;
            tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            for (int i = 0; i < tableLayoutPanel.RowCount; i++) {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, HEIGHT_PER_ROW));
            }
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, PICTURE_BOX_WIDTH + 10));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            for (int i = 0; i < tableLayoutPanel.RowCount; i++) {
                PictureBox pictureBox = new PictureBox();
                pictureBoxeList.Add(pictureBox);
                pictureBox.Image = indicatingArrow;
                pictureBox.Height = HEIGHT_PER_ROW;
                pictureBox.Width = PICTURE_BOX_WIDTH;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Visible = false;
                tableLayoutPanel.Controls.Add(pictureBox, 0, i);

                string title = titleList[i];
                Label label = new Label();
                labelList.Add(label);
                label.Text = title;
                label.Font = new Font("Courier New", HEIGHT_PER_ROW - 20, FontStyle.Regular);
                label.AutoSize = true;
                tableLayoutPanel.Controls.Add(label, 1, i);
            }

            this.Controls.Add(tableLayoutPanel);
            this.Height = HEIGHT_PER_ROW * titleList.Count + 30;
            this.Width = 1000;            
        }

        public void BeginRow(int rowIndex) {
            PictureBox pictureBox = pictureBoxeList[rowIndex];
            Label label = labelList[rowIndex];
            this.BeginInvoke(new Action(() => {
                pictureBox.Visible = true;
                label.ForeColor = Color.Red;
            }));
        }

        public void EndRow(int rowIndex, bool isOk) {
            PictureBox pictureBox = pictureBoxeList[rowIndex];
            Label label = labelList[rowIndex];
            Color color = Color.Green;
            if (!isOk) {
                color = Color.Red;
            }
            this.BeginInvoke(new Action(() => {
                pictureBox.Visible = false;
                label.ForeColor = color;
            }));
        }

        private readonly FormResourceCulture frc;
        private readonly List<string> titleList;
        private readonly Bitmap indicatingArrow;
        private readonly List<PictureBox> pictureBoxeList = new List<PictureBox>();
        private readonly List<Label> labelList = new List<Label>();
        private const int HEIGHT_PER_ROW = 50;
        private const int PICTURE_BOX_WIDTH = 100;
    }
}
