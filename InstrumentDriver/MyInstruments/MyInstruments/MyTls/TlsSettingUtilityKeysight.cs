using System;
using System.Collections.Generic;

using MyInstruments.MyEnum;

namespace MyInstruments.MyTls {
    public sealed class TlsSettingUtilityKeysight : ITlsSettingUtility {

        public (bool isOk, string errorText) IsSettingValid(string key, string value) {
            string errorText = string.Empty;
            if (!Enum.TryParse(key, out EnumKeysightTlsSetting enumTlsSetting)) {
                errorText = $"Setting key(={key}) is invalid for TLS!";
                return (false, errorText);
            }

            switch (enumTlsSetting) {
                case EnumKeysightTlsSetting.WAVELENGTH:
                    if (!double.TryParse(value, out double wavelengthInNm)) {
                        errorText = $"setting value={value} is invalid for {EnumKeysightTlsSetting.WAVELENGTH.ToString()} of Keysight TLS!";
                        return (false, errorText);
                    }
                    break;
                case EnumKeysightTlsSetting.POWER:
                    if (!double.TryParse(value, out double powerInDbm)) {
                        errorText = $"setting value={value} is invalid for {EnumKeysightTlsSetting.POWER.ToString()} of Keysight TLS!";
                        return (false, errorText);
                    }
                    break;
                case EnumKeysightTlsSetting.POWER_UNIT_TYPE:
                    if (!Enum.TryParse(value, out EnumOpticPowerUnitType powerUnitType)) {
                        errorText = $"setting value={value} is invalid for {EnumKeysightTlsSetting.POWER_UNIT_TYPE.ToString()} of Keysight TLS!";
                        return (false, errorText);
                    }
                    break;
                case EnumKeysightTlsSetting.IS_TURNED_ON:
                    if (bool.TryParse(value, out bool isTurnedOn)) {
                        errorText = $"setting value={value} is invalid for {EnumKeysightTlsSetting.IS_TURNED_ON.ToString()} of Keysight TLS!";
                        return (false, errorText);
                    }
                    break;
            }

            return (true, string.Empty);
        }

        public bool BatchSetting(ITls tls, string slot, string channel, Dictionary<string, string> settings) {
            foreach (var setting in settings) {
                string key = setting.Key;
                string value = setting.Value;
                Enum.TryParse(key, out EnumKeysightTlsSetting enumTlsSetting);

                switch (enumTlsSetting) {
                    case EnumKeysightTlsSetting.WAVELENGTH:
                        double.TryParse(value, out double wavelengthInNm);
                        tls.TlsSetWavelength(slot, channel, wavelengthInNm);
                        break;
                    case EnumKeysightTlsSetting.POWER:
                        double.TryParse(value, out double powerInDbm);
                        tls.TlsSetPower(slot, channel, powerInDbm);
                        break;
                    case EnumKeysightTlsSetting.POWER_UNIT_TYPE:
                        Enum.TryParse(value, out EnumOpticPowerUnitType powerUnitType);
                        tls.TlsSetPowerUnitType(slot, channel, powerUnitType);
                        break;
                    case EnumKeysightTlsSetting.IS_TURNED_ON:
                        bool.TryParse(value, out bool isTurnedOn);
                        if (isTurnedOn) {
                            tls.TlsTurnOn(slot, channel);
                        } else {
                            tls.TlsTurnOff(slot, channel);
                        }
                        break;
                }
            }

            return true;
        }
    }
}
