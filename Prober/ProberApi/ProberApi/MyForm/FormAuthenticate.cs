using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

using AuthenticationSpi;

using CommonApi.MyI18N;
using CommonApi.MyUtility;

using NLog;

namespace ProberApi.MyForm {
    public partial class FormAuthenticate : Form {
        public string UserName { get; private set; }
        public string Role { get; private set; }

        public FormAuthenticate(EnumLanguage language) {
            InitializeComponent();
            frc = new FormResourceCulture(this.GetType().FullName, Assembly.GetExecutingAssembly());
            frc.Language = language;
            rc = new ResourceCulture($"{Assembly.GetExecutingAssembly().GetName().Name}.GlobalStrings", Assembly.GetExecutingAssembly());
            rc.Language = language;
            this.TopMost = true;
        }

        public (bool isOk, bool enabled) Initialize() {
            try {
                XElement xeRoot = XElement.Load(@"Configuration/config_static_information.xml");
                XElement xeAuthentication = xeRoot.Element("authentication");
                string strEnabled = xeAuthentication.Attribute("enabled").Value.Trim();
                if (string.IsNullOrEmpty(strEnabled)) {
                    LOGGER.Error($"Configuration/config_static_info.xml, <authentication enabled=>, enabled {rc.GetString("txtShouldNotBeEmpty")}");
                    return (false, false);
                }
                if (!bool.TryParse(strEnabled, out bool enabled)) {
                    LOGGER.Error($"Configuration/config_static_info.xml, <authentication enabled={strEnabled}>, enabled(={strEnabled}) {rc.GetString("txtShouldBeBoolean")}");
                    return (false, false);
                }
                if (!enabled) {
                    return (true, false);
                }

                XElement xeDllShortFileName = xeAuthentication.Element("dll_short_file_name");
                string dllShortFileName = xeDllShortFileName.Value.Trim();
                if (string.IsNullOrEmpty(dllShortFileName)) {
                    LOGGER.Error($"Configuration/config_static_info.xml, <authentication><dll_short_file_name></dll_short_file_name>, it {rc.GetString("txtShouldNotBeEmpty")}");
                    return (false, false);
                }
                if (!dllShortFileName.ToLowerInvariant().EndsWith(".dll")) {
                    LOGGER.Error($"Configuration/config_static_info.xml, <authentication><dll_short_file_name>{dllShortFileName}</dll_short_file_name>, it {rc.GetString("txtShouldEndWithDll")}");
                    return (false, false);
                }
                string dllFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dllShortFileName);
                if (!File.Exists(dllFullPath)) {
                    LOGGER.Error($"Configuration/config_static_info.xml, <authentication><dll_short_file_name>{dllShortFileName}</dll_short_file_name>, {dllFullPath} {rc.GetString("txtDoesNotExist")}");
                    return (false, false);
                }
                Assembly assembly = Assembly.LoadFrom(dllFullPath);
                if (assembly == null) {
                    LOGGER.Error($"Configuration/config_static_info.xml, <authentication><dll_short_file_name>{dllShortFileName}</dll_short_file_name>, {rc.GetString("txtLoading")} {dllFullPath} {rc.GetString("txtIsFailed")}");
                    return (false, false);
                }

                XElement xeFullClassName = xeAuthentication.Element("full_class_name");
                string fullClassName = xeFullClassName.Value.Trim();
                if (string.IsNullOrEmpty(fullClassName)) {
                    LOGGER.Error($"Configuration/config_static_info.xml, <authentication><full_class_name></<full_class_name>>, it {rc.GetString("txtShouldNotBeEmpty")}");
                    return (false, false);
                }
                Type type = assembly.GetType(fullClassName);
                if (type == null) {
                    LOGGER.Error($"Configuration/config_static_info.xml, <authentication><full_class_name>{fullClassName}</<full_class_name>>, {rc.GetString("txtLoading")} class {fullClassName} {rc.GetString("txtIsFailed")}");
                    return (false, false);
                }
                object instance = Activator.CreateInstance(type);
                this.myAuthenticate = instance as MyAuthenticate;
                if (this.myAuthenticate == null) {
                    LOGGER.Error($"Configuration/config_static_info.xml, <authentication><full_class_name>{fullClassName}</<full_class_name>>, {rc.GetString("txtCreatingInstance")}({fullClassName}) {rc.GetString("txtIsFailed")}");
                    return (false, false);
                }
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return (false, false);
            }

            return (true, true);
        }

        private void FormAuthenticate_Load(object sender, EventArgs e) {
            this.ControlBox = false;
            tbPassword.PasswordChar = '*';

            frc.EnumerateControl(this);
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e) {
            string username = tbUserName.Text.Trim();
            string password = tbPassword.Text.Trim();

            var result = myAuthenticate.Authenticate(username, password);
            if (!result.isOk) {
                MessageBox.Show(frc.GetString("txtInvalidUserOrPassword"));
                tbUserName.Focus();
                return;
            }

            UserName = username;
            Role = result.role.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private MyAuthenticate myAuthenticate;
        private readonly FormResourceCulture frc;
        private readonly ResourceCulture rc;
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
