using System.Collections.Generic;

namespace MyInstruments.MyTls {
    public interface ITlsSettingUtility {
        (bool isOk, string errorText) IsSettingValid(string key, string value);
        bool BatchSetting(ITls tls, string slot, string channel, Dictionary<string, string> settings);
    }
}
