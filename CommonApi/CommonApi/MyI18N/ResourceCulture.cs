using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace CommonApi.MyI18N {
    public class ResourceCulture {
        public EnumLanguage Language {
            set {
                ci = new CultureInfo(languagePostfixes[value]);
                language = value;
            }
            get { return language; }
        }
        private EnumLanguage language;

        public ResourceCulture(string baseName, Assembly assembly) {
            ci = CultureInfo.InstalledUICulture;
            rm = new ResourceManager(baseName, assembly);
        }

        public string GetString(string id) {
            string result = "";
            try {
                result = rm.GetString(id, ci);
                if (result == null) {
                    return $"invalid resource id={id}";
                } else {
                    return result;
                }
            } catch {
                return $"exceptional resource id={id}";
            }
        }

        protected CultureInfo ci;
        protected readonly ResourceManager rm;
        private Dictionary<EnumLanguage, string> languagePostfixes = new Dictionary<EnumLanguage, string> {
            { EnumLanguage.ENGLISH, "en-US" },
            { EnumLanguage.简体中文, "zh-CN" },
            { EnumLanguage.繁體中文, "zh-Hant" }
        };
    }
}
