using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

using CommonApi.MyI18N;
using CommonApi.MyTrigger;

namespace ProberApi.MyForm {
    public partial class FormCouplingFeedbackTriggerInSetting : Form {
        public FormCouplingFeedbackTriggerInSetting(EnumLanguage language, Dictionary<string, string> settingDict, ITriggerIn triggerIn) {
            InitializeComponent();
            frc = new FormResourceCulture(this.GetType().FullName, Assembly.GetExecutingAssembly());
            frc.Language = language;
            rc = new ResourceCulture($"{Assembly.GetExecutingAssembly().GetName().Name}.GlobalStrings", Assembly.GetExecutingAssembly());
            rc.Language = language;

            this.settingDict = settingDict;
            this.triggerIn = triggerIn;
        }

        private void FormCouplingFeedbackTriggerInSetting_Load(object sender, System.EventArgs e) {
            frc.EnumerateControl(this);

            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            const int MARGIN = 20;
            const int HEIGHT = 30;
            const int WIDTH = 200;

            this.Width = 500;
            this.Height = (settingDict.Count + 1) * (HEIGHT + MARGIN) + 3 * MARGIN;
            int middle = this.Width / 2;

            int index = 0;
            keyTextboxDict.Clear();

            int lastLabelTop = 0;
            foreach (var one in settingDict) {
                Label label = new Label();
                label.Text = one.Key;
                label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                label.AutoSize = false;
                label.Width = WIDTH;
                label.Height = HEIGHT;
                label.Left = middle - MARGIN - label.Width;
                label.Top = MARGIN + index * (MARGIN + label.Height);
                lastLabelTop = label.Top;

                TextBox textBox = new TextBox();
                textBox.TextAlign = HorizontalAlignment.Left;
                textBox.Top = label.Top;
                textBox.Left = middle + MARGIN;
                textBox.Height = HEIGHT;
                textBox.Width = WIDTH;
                textBox.Text = one.Value;

                ++index;
                keyTextboxDict.Add(one.Key, textBox);

                this.Controls.Add(label);
                this.Controls.Add(textBox);
            }

            Button btnOk = new Button();
            btnOk.Text = rc.GetString("txtOk");
            btnOk.Click += BtnOk_Click;
            btnOk.Width = WIDTH;
            btnOk.Height = HEIGHT;
            btnOk.Top = lastLabelTop + HEIGHT + MARGIN;
            btnOk.Left = middle - MARGIN - btnOk.Width;

            Button btnCancel = new Button();
            btnCancel.Text = rc.GetString("txtCancel");
            btnCancel.Click += BtnCancel_Click;
            btnCancel.Width = WIDTH;
            btnCancel.Height = HEIGHT;
            btnCancel.Top = btnOk.Top;
            btnCancel.Left = middle + MARGIN;

            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);
        }

        private void BtnCancel_Click(object sender, System.EventArgs e) {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void BtnOk_Click(object sender, System.EventArgs e) {
            Dictionary<string, string> tempDict = new Dictionary<string, string>();
            foreach (var one in keyTextboxDict) {
                string key = one.Key;
                TextBox textBox = one.Value;
                if (string.IsNullOrEmpty(textBox.Text.Trim())) {
                    MessageBox.Show(frc.GetString("txtValueShouldNotBeEmpty"));
                    textBox.Focus();
                    return;
                }
                tempDict.Add(key, textBox.Text.Trim());
            }

            var checkResult = triggerIn.CheckTriggerInSettings(tempDict);
            if (!checkResult.isOk) {
                MessageBox.Show(checkResult.errorText); //[gyh]: 待国际化
                return;
            }

            foreach (var one in tempDict) {
                settingDict[one.Key] = one.Value;
            }

            this.Close();
            this.DialogResult = DialogResult.OK;
        }

        private readonly Dictionary<string, string> settingDict;
        private readonly ITriggerIn triggerIn;
        private readonly Dictionary<string, TextBox> keyTextboxDict = new Dictionary<string, TextBox>();
        private readonly FormResourceCulture frc;
        private readonly ResourceCulture rc;
    }
}
