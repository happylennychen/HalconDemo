using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

using CommonApi.MyI18N;
using CommonApi.MyUtility;

using NLog;

namespace ProberApi.MyForm {
    public partial class FormSettingGuiLanguage : Form {
        public FormSettingGuiLanguage(EnumLanguage language, ConcurrentDictionary<string, object> sharedObjects) {
            InitializeComponent();
            frc = new FormResourceCulture(this.GetType().FullName, Assembly.GetExecutingAssembly());
            frc.Language = language;
            rc = new ResourceCulture($"{Assembly.GetExecutingAssembly().GetName().Name}.GlobalStrings", Assembly.GetExecutingAssembly());
            rc.Language = language;
        }

        private void FormSettingGuiLanguage_Load(object sender, EventArgs e) {
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            frc.EnumerateControl(this);

            List<EnumLanguage> allLanguages = new List<EnumLanguage>();
            foreach (EnumLanguage language in Enum.GetValues(typeof(EnumLanguage))) {
                cmbAllSupportedLanguages.Items.Add(language);
                allLanguages.Add(language);
            }

            cmbAllSupportedLanguages.SelectedIndex = allLanguages.IndexOf(frc.Language);
        }

        private void cmbAllSupportedLanguages_SelectedIndexChanged(object sender, EventArgs e) {
            EnumLanguage choosenLanguage = (EnumLanguage)cmbAllSupportedLanguages.SelectedItem;
            btnSet.Enabled = !(choosenLanguage == frc.Language);
        }

        private void btnSet_Click(object sender, EventArgs e) {
            string choosenLanguage = cmbAllSupportedLanguages.SelectedItem.ToString();
            if (SaveGuiLanguageToConfigurationFile(choosenLanguage)) {
                MessageBox.Show($"{frc.GetString("txtModificationIsSuccessful")}\n{frc.GetString("txtModificationTakeEffect")}");
                this.Close();
            } else {
                MessageBox.Show($"{frc.GetString("txtModificationIsFailed")}\n{rc.GetString("txtLookupLog")}");
            }
        }

        private bool SaveGuiLanguageToConfigurationFile(string choosenLanguage) {
            const string configFilePath = @"Configuration/config.xml";
            try {
                XDocument xDoc = XDocument.Load(configFilePath);
                XElement xeRoot = xDoc.Root;
                XElement xeLanguage = xeRoot.Element("language");
                xeLanguage.SetValue(choosenLanguage);
                xDoc.Save(configFilePath);
                return true;
            } catch (Exception ex) {
                LOGGER.Error(ex.Message);
                return false;
            }
        }

        private readonly FormResourceCulture frc;
        private readonly ResourceCulture rc;
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
