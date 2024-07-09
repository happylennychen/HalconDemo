using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Windows.Forms;

using CommonApi.MyI18N;

namespace ProberApi.MyForm {
    public partial class FormCoupling : Form {
        public FormCoupling(EnumLanguage language, ConcurrentDictionary<string, object> sharedObjects) {
            InitializeComponent();

            this.sharedObjects = sharedObjects;
            frc = new FormResourceCulture(this.GetType().FullName, Assembly.GetExecutingAssembly());
            frc.Language = language;
        }

        private void FormCoupling_Load(object sender, EventArgs e) {
            frc.EnumerateControl(this);

            FormCouplingSpiral2d formSpiral = new FormCouplingSpiral2d(frc.Language, sharedObjects, ActiveSpiral2DPage);
            formSpiral.TopLevel = false;
            formSpiral.FormBorderStyle = FormBorderStyle.None;
            formSpiral.Dock = DockStyle.Fill;
            this.BeginInvoke(new Action(() => {
                panelSpiral2D.Controls.Add(formSpiral);
                formSpiral.Show();
            }));

            FormCouplingCross2d formCross = new FormCouplingCross2d(frc.Language, sharedObjects, ActiveCross2dPage);
            formCross.TopLevel = false;
            formCross.FormBorderStyle = FormBorderStyle.None;
            formCross.Dock = DockStyle.Fill;
            this.BeginInvoke(new Action(() => {
                panelCross2D.Controls.Add(formCross);
                formCross.Show();
            }));

            tcCoupling.SelectedIndex = 0;
        }

        public void ActiveSpiral2DPage() {
            this.BeginInvoke((Action)(() => {
                tcCoupling.SelectedTab = tpSpiral2d;
            }));

        }

        public void ActiveCross2dPage() {
            this.BeginInvoke((Action)(() => {
                tcCoupling.SelectedTab = tpCross2d;
            }));
        }

        private readonly ConcurrentDictionary<string, object> sharedObjects;
        private readonly FormResourceCulture frc;
    }
}
