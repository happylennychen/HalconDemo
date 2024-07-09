using System.Collections.Generic;

namespace MyInstruments.MyOpm {
    public interface IOpmSettingUtility {
        (bool isOk, string errorText) IsSettingValid(string key, string value);
        bool BatchSetting(IOpm opm, string slot, string channel, Dictionary<string, string> settings);
    }
}
