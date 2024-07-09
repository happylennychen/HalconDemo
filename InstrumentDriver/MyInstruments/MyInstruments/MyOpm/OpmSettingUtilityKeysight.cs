using System;
using System.Collections.Generic;

using MyInstruments.MyEnum;

namespace MyInstruments.MyOpm {
    public class OpmSettingUtilityKeysight : IOpmSettingUtility {
        public (bool isOk, string errorText) IsSettingValid(string key, string value) {
            string errorText = string.Empty;
            if (!Enum.TryParse(key, out EnumKeysightOpmSetting enumOpmSetting)) {
                errorText = $"setting key={key} is invalid for keysight OPM!";
                return (false, errorText);
            }

            switch (enumOpmSetting) {
                case EnumKeysightOpmSetting.WAVELENGTH:
                    if (!double.TryParse(value, out double wavelengthInNm)) {
                        errorText = $"setting value={value} is invalid for {EnumKeysightOpmSetting.WAVELENGTH.ToString()} of Keysight OPM!";
                        return (false, errorText);
                    }
                    break;
                case EnumKeysightOpmSetting.AVERAGE_TIME:
                    if (!double.TryParse(value, out double averageTimeInMs)) {
                        errorText = $"setting value={value} is invalid for {EnumKeysightOpmSetting.AVERAGE_TIME.ToString()} of Keysight OPM!";
                        return (false, errorText);
                    }
                    break;
                case EnumKeysightOpmSetting.RANGE:
                    if (!int.TryParse(value, out int rangeInDbm)) {
                        errorText = $"Setting value={value} is invalid for {EnumKeysightOpmSetting.RANGE.ToString()} of keysight OPM!";
                        return (false, errorText);
                    }
                    break;
                case EnumKeysightOpmSetting.RANGE_MODE:
                    if (!Enum.TryParse(value, out EnumOpmPowerRangeType enumPowerRangeType)) {
                        errorText = $"Setting value={value} is invalid for {EnumKeysightOpmSetting.RANGE_MODE.ToString()} of keysight OPM!";
                        return (false, errorText);
                    }
                    break;
                case EnumKeysightOpmSetting.UNIT_TYPE:
                    if (!Enum.TryParse(value, out EnumOpticPowerUnitType opmPowerUnitType)) {
                        errorText = $"Setting value={value} is invalid for {EnumKeysightOpmSetting.UNIT_TYPE.ToString()} of keysight OPM!";
                        return (false, errorText);
                    }
                    break;
            }

            return (true, string.Empty);
        }

        public bool BatchSetting(IOpm opm, string slot, string channel, Dictionary<string, string> settings) {
            foreach (var setting in settings) {
                string key = setting.Key;
                string value = setting.Value;
                Enum.TryParse(key, out EnumKeysightOpmSetting enumOpmSetting);

                switch (enumOpmSetting) {
                    case EnumKeysightOpmSetting.WAVELENGTH:
                        double.TryParse(value, out double wavelengthInNm);                            
                        opm.OpmSetWavelength(slot, channel, wavelengthInNm);
                        break;
                    case EnumKeysightOpmSetting.AVERAGE_TIME:
                        double.TryParse(value, out double averageTimeInMs);                            
                        opm.OpmSetAverageTime(slot, channel, averageTimeInMs, EnumOpmTimeUnitType.MS);
                        break;
                    case EnumKeysightOpmSetting.RANGE:
                        int.TryParse(value, out int rangeInDbm);                            
                        opm.OpmSetPowerRange(slot, channel, rangeInDbm);
                        break;
                    case EnumKeysightOpmSetting.RANGE_MODE:
                        Enum.TryParse(value, out EnumOpmPowerRangeType enumPowerRangeType);                            
                        opm.OpmSetPowerRangeType(slot, channel, enumPowerRangeType);
                        break;
                    case EnumKeysightOpmSetting.UNIT_TYPE:
                        Enum.TryParse(value, out EnumOpticPowerUnitType opmPowerUnitType);                            
                        opm.OpmSetPowerUnitType(slot, channel, opmPowerUnitType);
                        break;
                }
            }

            return true;
        }
    }
}
