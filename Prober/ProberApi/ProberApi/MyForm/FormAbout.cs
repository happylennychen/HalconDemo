using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using CommonApi.MyI18N;

using ProberApi.MyEnum;

namespace ProberApi.MyForm {
    public partial class FormAbout : Form {
        public FormAbout(EnumLanguage language, Dictionary<EnumAboutInfo, string> aboutInfo) {
            InitializeComponent();
            this.aboutInfo = aboutInfo;
            frc = new FormResourceCulture(this.GetType().FullName, Assembly.GetExecutingAssembly());
            frc.Language = language;
        }

        private void FormAbout_Load(object sender, EventArgs e) {
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            tbProduct.ReadOnly = true;
            tbProduct.BackColor = Color.LightGray;
            tbCopyright.ReadOnly = true;
            tbCopyright.BackColor = Color.LightGray;
            tbSoftwareVersion.ReadOnly = true;
            tbSoftwareVersion.BackColor = Color.LightGray;
            tbSwFrameworkVersion.ReadOnly = true;
            tbSwFrameworkVersion.BackColor = Color.LightGray;

            tbProduct.Text = aboutInfo[EnumAboutInfo.PRODUCT];
            Assembly assembly = Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;
            tbSwFrameworkVersion.Text = version.ToString();
            tbSoftwareVersion.Text = aboutInfo[EnumAboutInfo.SOFTWARE_VERSION]; 
            tbCopyright.Text = aboutInfo[EnumAboutInfo.COPYRIGHT];

            frc.EnumerateControl(this);
        }

        private readonly Dictionary<EnumAboutInfo, string> aboutInfo;
        private readonly FormResourceCulture frc;
    }
}
