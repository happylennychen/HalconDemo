using System;

using MyInstruments.MyEnum;
using MyInstruments.MyOpm;
using MyInstruments.MySmu;
using MyInstruments.MyTls;

namespace MyInstruments.MyUtility {
    public static class CheckingInstrumentSettingItem {
        public static (bool isOk, string errorText) Check(EnumInstrumentCategory instrumentCategory, string settingKey, string settingValue) {
            (bool isOk, string errorText) result = (false, string.Empty);
            switch (instrumentCategory) {
                case EnumInstrumentCategory.TLS:
                    result = CheckTls(settingKey, settingValue);
                    break;
                case EnumInstrumentCategory.OPM:
                    result = CheckOpm(settingKey, settingValue);
                    break;
                case EnumInstrumentCategory.SMU:
                    result = CheckSmu(settingKey, settingValue);
                    break;
            }

            return result;
        }

        public static (bool isOk, string errorText) CheckTls(string settingKey, string settingValue) {
            if (!Enum.TryParse(settingKey, out EnumKeysightTlsSetting enumSetting)) {
                return (false, $"settingKey(={settingKey}) is not a valid setting key of TLS!");
            }

            switch (enumSetting) {
                case EnumKeysightTlsSetting.WAVELENGTH:
                    if (!double.TryParse(settingValue, out double wavelength)) {
                        return (false, $"{EnumKeysightTlsSetting.WAVELENGTH.ToString()}(={settingValue}) should be a positive double!");
                    }
                    if (wavelength <= 0) {
                        return (false, $"{EnumKeysightTlsSetting.WAVELENGTH.ToString()}(={settingValue}) should be a positive double!");
                    }
                    break;
                case EnumKeysightTlsSetting.POWER:
                    if (!double.TryParse(settingValue, out double power)) {
                        return (false, $"{EnumKeysightTlsSetting.POWER.ToString()}(={settingValue}) should be a double!");
                    }
                    break;
                case EnumKeysightTlsSetting.IS_TURNED_ON:
                    if (!bool.TryParse(settingValue, out bool isTurnedOn)) {
                        return (false, $"{EnumKeysightTlsSetting.IS_TURNED_ON}(={settingValue}) should be a boolean!");
                    }
                    break;
            }

            return (true, string.Empty);
        }

        public static (bool isOk, string errorText) CheckOpm(string settingKey, string settingValue) {
            if (!Enum.TryParse(settingKey, out EnumKeysightOpmSetting enumSetting)) {
                return (false, $"settingKey(={settingKey}) is not a valid setting key of OPM!");
            }

            switch (enumSetting) {
                case EnumKeysightOpmSetting.WAVELENGTH:
                    if (!double.TryParse(settingValue, out double wavelength)) {
                        return (false, $"{EnumKeysightOpmSetting.WAVELENGTH.ToString()}(={settingValue}) should be a positive double!");
                    }
                    if (wavelength <= 0) {
                        return (false, $"{EnumKeysightOpmSetting.WAVELENGTH.ToString()}(={settingValue}) should be a positive double!");
                    }
                    break;
                case EnumKeysightOpmSetting.AVERAGE_TIME:
                    if (!double.TryParse(settingValue, out double averageTime)) {
                        return (false, $"{EnumKeysightOpmSetting.AVERAGE_TIME.ToString()}(={settingValue}) should be a positive double!");
                    }
                    if (averageTime <= 0) {
                        return (false, $"{EnumKeysightOpmSetting.AVERAGE_TIME.ToString()}(={settingValue}) should be a positive double!");
                    }
                    break;
                case EnumKeysightOpmSetting.RANGE:
                    if (!double.TryParse(settingValue, out double range)) {
                        return (false, $"{EnumKeysightOpmSetting.RANGE.ToString()}(={settingValue}) should be a double!");
                    }
                    break;
                case EnumKeysightOpmSetting.RANGE_MODE:
                    if (!Enum.TryParse(settingValue, out EnumOpmPowerRangeType rangeType)) {
                        return (false, $"{EnumKeysightOpmSetting.RANGE_MODE.ToString()}(={settingValue}) should be type=EnumOpmPowerRangeType!");
                    }
                    break;
                case EnumKeysightOpmSetting.UNIT_TYPE:
                    if (!Enum.TryParse(settingValue, out EnumOpticPowerUnitType unitType)) {
                        return (false, $"{EnumKeysightOpmSetting.UNIT_TYPE.ToString()}(={settingValue}) should be type=EnumOpmPowerUnitType!");
                    }
                    break;
            }

            return (true, string.Empty);
        }

        public static (bool isOk, string errorText) CheckSmu(string settingKey, string settingValue) {
            if (!Enum.TryParse(settingKey, out EnumKeithleySmuSetting enumSetting)) {
                return (false, $"settingKey(={settingKey}) is not a valid setting key of SMU!");
            }

            switch (enumSetting) {
                case EnumKeithleySmuSetting.SOURCE_TYPE:
                case EnumKeithleySmuSetting.MEASURE_TYPE:
                    break;
                case EnumKeithleySmuSetting.SOURCE_LIMIT:
                    if (!double.TryParse(settingValue, out double sourceLimit)) {
                        return (false, $"{EnumKeithleySmuSetting.SOURCE_LIMIT.ToString()}(={settingValue}) should be a double!");
                    }
                    break;
                case EnumKeithleySmuSetting.SOURCE_LEVEL:
                    if (!double.TryParse(settingValue, out double sourceLevel)) {
                        return (false, $"{EnumKeithleySmuSetting.SOURCE_LEVEL.ToString()}(={settingValue}) should be a double!");
                    }
                    break;
                case EnumKeithleySmuSetting.MEASURE_COUNT:
                    if (!int.TryParse(settingValue, out int measureCount)) {
                        return (false, $"{EnumKeithleySmuSetting.MEASURE_COUNT.ToString()}(={settingValue}) should be an integer!");
                    }
                    break;
                case EnumKeithleySmuSetting.MEASURE_DELAY:
                    if (!double.TryParse(settingValue, out double measureDelay)) {
                        return (false, $"{EnumKeithleySmuSetting.MEASURE_DELAY.ToString()}(={settingValue}) should be a double!");
                    }
                    break;
                case EnumKeithleySmuSetting.MEASURE_NPLC:
                    if (!double.TryParse(settingValue, out double nplc)) {
                        return (false, $"{EnumKeithleySmuSetting.MEASURE_NPLC.ToString()}(={settingValue}) should be a positive double!");
                    }
                    if (nplc <= 0.0) {
                        return (false, $"{EnumKeithleySmuSetting.MEASURE_NPLC.ToString()}(={settingValue}) should be a positive double!");
                    }
                    break;
                case EnumKeithleySmuSetting.MEASURE_IS_AUTORANGE:
                    if (!bool.TryParse(settingValue, out bool isAutoRange)) {
                        return (false, $"{EnumKeithleySmuSetting.MEASURE_IS_AUTORANGE.ToString()}(={settingValue}) should be a boolean!");
                    }
                    break;
                case EnumKeithleySmuSetting.MEASURE_RANGE:
                    if (!double.TryParse(settingValue, out double range)) {
                        return (false, $"{EnumKeithleySmuSetting.MEASURE_RANGE.ToString()}(={settingValue}) should be a double!");
                    }
                    break;
                case EnumKeithleySmuSetting.IS_TURNED_ON:
                    if (!bool.TryParse(settingValue, out bool isTurnedOn)) {
                        return (false, $"{EnumKeithleySmuSetting.IS_TURNED_ON}(={settingValue}) should be a boolean!");
                    }
                    break;
            }

            return (true, string.Empty);
        }
    }
}
