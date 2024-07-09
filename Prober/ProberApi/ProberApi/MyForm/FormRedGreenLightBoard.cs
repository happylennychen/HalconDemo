using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Windows.Forms;

using CommonApi.MyUtility;

using NLog;

using ProberApi.MyBoard;
using ProberApi.MyConstant;

namespace ProberApi.MyForm {
    public partial class FormRedGreenLightBoard : Form {
        public FormRedGreenLightBoard(ConcurrentDictionary<string, object> sharedObjects, Bitmap redLight, Bitmap greenLight) {
            InitializeComponent();
            this.redLight = redLight;
            this.greenLight = greenLight;

            sharedObjects.TryGetValue(SharedObjectKey.RED_GREEN_LIGHT_BOARD, out object tempObj);
            redGreenLightBoard = tempObj as RedGreenLightBoard;

            timerRefreshGui.Interval = 200;
            timerRefreshGui.Elapsed += OnTimedEventRefreshGui;
            timerRefreshGui.Enabled = false;
            /*
            timerUpdateRedGreenLights.Interval = 2000;
            timerUpdateRedGreenLights.Elapsed += OnTimedEventUpdateRedGreenLight;
            timerUpdateRedGreenLights.Enabled = false;
            */
        }

        private void FormRedGreenLightBoard_Load(object sender, EventArgs e) {
            List<string> keyList = redGreenLightBoard.Keys.ToList();

            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.ColumnCount = redGreenLightBoard.Count;
            tableLayoutPanel.RowCount = 2;

            for (int i = 0; i < tableLayoutPanel.ColumnCount; i++) {
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            }
            for (int i = 0; i < tableLayoutPanel.RowCount; i++) {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            }
            for (int i = 0; i < tableLayoutPanel.ColumnCount; i++) {
                Label label = new Label();
                string content = keyList[i];
                label.Text = content;
                label.Font = new Font("Arial", 10, FontStyle.Bold);
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Anchor = AnchorStyles.None;
                label.BorderStyle = BorderStyle.Fixed3D;
                tableLayoutPanel.Controls.Add(label, i, 0);

                PictureBox pictureBox = new PictureBox();
                pictureBox.Width = 40;
                pictureBox.Height = 40;
                pictureBox.Image = this.redLight;
                pictureBox.Anchor = AnchorStyles.None;
                pictureBox.BorderStyle = BorderStyle.Fixed3D;
                tableLayoutPanel.Controls.Add(pictureBox, i, 1);

                pictureBoxDict.Add(content, pictureBox);
            }

            this.Controls.Add(tableLayoutPanel);

            timerRefreshGui.Enabled = true;
            //timerUpdateRedGreenLights.Enabled = true;
        }

        private void OnTimedEventRefreshGui(object sender, ElapsedEventArgs e) {
            try {
                List<(string key, bool isGreen)> info = redGreenLightBoard.GetAll();
                foreach (var one in info) {
                    PictureBox pictureBox = pictureBoxDict[one.key];
                    this.BeginInvoke(new Action(() => {
                        if (one.isGreen) {
                            pictureBox.Image = this.greenLight;
                        } else {
                            pictureBox.Image = this.redLight;
                        }
                    }));
                }
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
            }
        }
/*
        private void OnTimedEventUpdateRedGreenLight(object sender, ElapsedEventArgs e) {
            List<string> keys = redGreenLightBoard.Keys.ToList();

            Random random = new Random();
            foreach (string key in keys) {
                int randomInt = random.Next();
                bool value;
                if (randomInt % 2 == 0) {
                    value = true;
                } else {
                    value = false;
                }

                redGreenLightBoard.AddOrUpdateLight(key, value);
            }
        }
*/
        private readonly RedGreenLightBoard redGreenLightBoard;
        private readonly Bitmap redLight;
        private readonly Bitmap greenLight;
        private readonly Dictionary<string, PictureBox> pictureBoxDict = new Dictionary<string, PictureBox>();
        private readonly System.Timers.Timer timerRefreshGui = new System.Timers.Timer();
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);

        //---------------------------------------------------------------
        //[gyh], 2023-12-27: 验证gui刷新的demo code，正式发布前应删除
        //private readonly System.Timers.Timer timerUpdateRedGreenLights = new System.Timers.Timer();
    }
}
