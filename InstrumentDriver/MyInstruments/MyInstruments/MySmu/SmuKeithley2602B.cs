using System;
using System.Collections.Generic;
using System.Linq;

using CommonApi.MyTrigger;
using CommonApi.MyUtility;

using Keithley.Ke26XXA.Interop;

using MyInstruments.MyEnum;
using MyInstruments.MyException;

namespace MyInstruments.MySmu {
    public sealed class SmuKeithley2602B : StandaloneSmu, ITriggerIn {
        public SmuKeithley2602B(string id) : base(id) {
            Vendor = EnumInstrumentVendor.KEITHLEY.ToString();
            SupportedModels = new HashSet<string> { EnumModelKeithley._2602B.ToString() };

            sourceTypes.Clear();
            measureTypes.Clear();
        }

        public override void Connect(string visaResource) {
            if (keithley2602b.Initialized) {
                return;
            }

            //keithley2602b.Initialize(visaResource, true, false, "QueryInstrStatus=True");
            keithley2602b.Initialize(visaResource, false, false, "QueryInstrStatus=False"); //陈军修改，不回读

            //用下面的代码进行了一下测试，证明修改没问题
            //TurnOn("1", "A");
            //SetMeasurementType("1", "A", EnumSmuMeasurementType.CURRENT);
            //double ret = Measure("1", "A");
        }

        public override void Disconnect() {
            keithley2602b.Close();
        }

        public override void SetSourceType(string slot, string channel, EnumSmuSourceType sourceType) {
            ThrowExceptionWhenSlotOrChannelInvalid(slot, channel);
            string regularChannel = GetRegularChannelString(channel);
            switch (sourceType) {
                case EnumSmuSourceType.VOLTAGE:
                    keithley2602b.Source.Function[regularChannel] = Ke26XXASourceFunctionEnum.Ke26XXASourceFunctionDCVolts;
                    break;
                case EnumSmuSourceType.CURRENT:
                    keithley2602b.Source.Function[regularChannel] = Ke26XXASourceFunctionEnum.Ke26XXASourceFunctionDCAmps;
                    break;
            }
            if (sourceTypes.ContainsKey(regularChannel)) {
                sourceTypes[regularChannel] = sourceType;
            } else {
                sourceTypes.Add(regularChannel, sourceType);
            }
        }

        public override void SetMeasurementType(string slot, string channel, EnumSmuMeasurementType measurementType) {
            ThrowExceptionWhenSlotOrChannelInvalid(slot, channel);
            string regularChannel = GetRegularChannelString(channel);
            if (measureTypes.ContainsKey(regularChannel)) {
                measureTypes[regularChannel] = measurementType;
            } else {
                measureTypes.Add(regularChannel, measurementType);
            }
        }

        public override void SetSourceLevel(string slot, string channel, double level) {
            ThrowExceptionWhenSlotOrChannelInvalid(slot, channel);
            string regularChannel = GetRegularChannelString(channel);
            var sourceType = GetSourceType(regularChannel);
            switch (sourceType) {
                case EnumSmuSourceType.CURRENT:
                    keithley2602b.Source.Current.Level[regularChannel] = level;
                    break;
                case EnumSmuSourceType.VOLTAGE:
                    keithley2602b.Source.Voltage.Level[regularChannel] = level;
                    break;
            }
        }

        public override double Measure(string slot, string channel) {
            ThrowExceptionWhenSlotOrChannelInvalid(slot, channel);

            string regularChannel = GetRegularChannelString(channel);
            var measureType = GetMeasureType(regularChannel);
            switch (measureType) {
                case EnumSmuMeasurementType.CURRENT:
                    return keithley2602b.Measurement.Current.Measure(regularChannel);
                case EnumSmuMeasurementType.VOLTAGE:
                    return keithley2602b.Measurement.Voltage.Measure(regularChannel);
                default: //never happen
                    return 0;
            }
        }

        public override void TurnOff(string slot, string channel) {
            ThrowExceptionWhenSlotOrChannelInvalid(slot, channel);
            string regularChannel = GetRegularChannelString(channel);
            keithley2602b.Source.OutputEnabled[regularChannel] = false;
        }

        public override void TurnOn(string slot, string channel) {
            ThrowExceptionWhenSlotOrChannelInvalid(slot, channel);
            string regularChannel = GetRegularChannelString(channel);
            keithley2602b.Source.OutputEnabled[regularChannel] = true;
        }

        public override void SetSourceLimit(string slot, string channel, double limit) {
            string regularChannel = GetRegularChannelString(channel);
            var sourceType = GetSourceType(regularChannel);
            switch (sourceType) {
                case EnumSmuSourceType.CURRENT:
                    keithley2602b.Source.Current.Limit[regularChannel] = limit;
                    break;
                case EnumSmuSourceType.VOLTAGE:
                    keithley2602b.Source.Voltage.Limit[regularChannel] = limit;
                    break;
            }
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel) {
            if (!Enum.TryParse(GetRegularChannelString(channel), out EnumChannels enumChannel)) {
                LOGGER.Error($"channel(={channel}) is invalid for SMU Keithley 2602B!");
                throw new InvalidSlotOrChannelOfInstrumentException(slot, channel, this.Vendor, this.CurrentModel);
            }
        }

        public override (bool isOk, string errorText) SmuAreSettingsValid(Dictionary<string, string> settings) {
            foreach (var setting in settings) {
                string key = setting.Key;
                string value = setting.Value;
                var checkResult = IsSettingValid(key, value);
                if (!checkResult.isOk) {
                    return checkResult;
                }
            }

            return (true, string.Empty);
        }

        public override bool SmuBatchSetting(string slot, string channel, Dictionary<string, string> settings) {
            ThrowExceptionWhenSlotOrChannelInvalid(slot, channel);

            string regularChannel = GetRegularChannelString(channel);
            var tryConvertResult = TryConvertToEnumSettings(settings);
            if (!tryConvertResult.isOk) {
                return false;
            }
            Dictionary<EnumKeithleySmuSetting, string> enumSettings = tryConvertResult.result;

            if (enumSettings.ContainsKey(EnumKeithleySmuSetting.SOURCE_TYPE)) {
                string strSourceType = enumSettings[EnumKeithleySmuSetting.SOURCE_TYPE];
                if (!Enum.TryParse(strSourceType, out EnumSmuSourceType enumSourceType)) {
                    LOGGER.Error($"Source type(={strSourceType}) is invalid of Keithley 2602B!");
                    return false;
                }
                SetSourceType(slot, channel, enumSourceType);
            }
            if (enumSettings.ContainsKey(EnumKeithleySmuSetting.MEASURE_TYPE)) {
                string strMeasureType = enumSettings[EnumKeithleySmuSetting.MEASURE_TYPE];
                if (!Enum.TryParse(strMeasureType, out EnumSmuMeasurementType enumMeasureType)) {
                    LOGGER.Error($"Measurement type(={strMeasureType}) is invalid of Keithley 2602B!");
                    return false;
                }
                SetMeasurementType(slot, channel, enumMeasureType);
            }

            foreach (var setting in enumSettings) {
                EnumKeithleySmuSetting settingKey = setting.Key;
                string strValue = setting.Value;

                EnumSmuSourceType sourceType;
                EnumSmuMeasurementType measureType;
                switch (settingKey) {
                    case EnumKeithleySmuSetting.SOURCE_TYPE:
                        break;
                    case EnumKeithleySmuSetting.SOURCE_LIMIT:
                        sourceType = GetSourceType(regularChannel);
                        double sourceLimit = ConvertToDoubleValue(strValue, slot, regularChannel, settingKey);
                        switch (sourceType) {
                            case EnumSmuSourceType.CURRENT:
                                keithley2602b.Source.Current.Limit[regularChannel] = sourceLimit;
                                break;
                            case EnumSmuSourceType.VOLTAGE:
                                keithley2602b.Source.Voltage.Limit[regularChannel] = sourceLimit;
                                break;
                        }
                        break;
                    case EnumKeithleySmuSetting.SOURCE_LEVEL:
                        sourceType = GetSourceType(regularChannel);
                        double sourceLevel = ConvertToDoubleValue(strValue, slot, regularChannel, settingKey);
                        switch (sourceType) {
                            case EnumSmuSourceType.CURRENT:
                                keithley2602b.Source.Current.Level[regularChannel] = sourceLevel;
                                break;
                            case EnumSmuSourceType.VOLTAGE:
                                keithley2602b.Source.Voltage.Level[regularChannel] = sourceLevel;
                                break;
                        }
                        break;
                    case EnumKeithleySmuSetting.MEASURE_TYPE:
                        break;
                    //[gyh]: measure protection，如何实现？
                    //:SENS:CURR:PROT:LEV 1.0
                    //:SENS:VOLT:PROT:LEV 10
                    //[gyh] RE：虽然上面2条SCPI命令的命令头是Sense，但其含义却是指对source进行限制，即
                    //:SENS:CURR:PROT:LEV 1.0  ---- source current limit = 1.0A
                    //:SENS:VOLT:PROT:LEV 10  ---- source voltage limit = 10V
                    case EnumKeithleySmuSetting.MEASURE_COUNT:
                        int measureCount = ConvetToIntValue(strValue, slot, regularChannel, settingKey);
                        keithley2602b.Measurement.Count[regularChannel] = measureCount;
                        break;
                    case EnumKeithleySmuSetting.MEASURE_DELAY:
                        double measureDelay = ConvertToDoubleValue(strValue, slot, regularChannel, settingKey);
                        keithley2602b.Measurement.Delay[regularChannel] = measureDelay;
                        break;
                    case EnumKeithleySmuSetting.MEASURE_NPLC:
                        double nplc = ConvertToDoubleValue(strValue, slot, regularChannel, settingKey);
                        keithley2602b.Measurement.NPLC[regularChannel] = nplc;
                        break;
                    case EnumKeithleySmuSetting.MEASURE_IS_AUTORANGE:
                        measureType = GetMeasureType(regularChannel);
                        bool isMeasureAutoRange = ConvertToBooleanValue(strValue, slot, regularChannel, settingKey);
                        switch (measureType) {
                            case EnumSmuMeasurementType.CURRENT:
                                keithley2602b.Measurement.Current.AutoRangeEnabled[regularChannel] = isMeasureAutoRange;
                                break;
                            case EnumSmuMeasurementType.VOLTAGE:
                                keithley2602b.Measurement.Voltage.AutoRangeEnabled[regularChannel] = isMeasureAutoRange;
                                break;
                        }
                        break;
                    case EnumKeithleySmuSetting.MEASURE_RANGE:
                        measureType = GetMeasureType(regularChannel);
                        double measureRange = ConvertToDoubleValue(strValue, slot, regularChannel, settingKey);
                        switch (measureType) {
                            case EnumSmuMeasurementType.CURRENT:
                                keithley2602b.Measurement.Current.Range[regularChannel] = measureRange;
                                break;
                            case EnumSmuMeasurementType.VOLTAGE:
                                keithley2602b.Measurement.Voltage.Range[regularChannel] = measureRange;
                                break;
                        }
                        break;
                    //case EnumKeithleySmuSetting.MEASURE_AUTOZERO:
                    //    var autoZero = ConvertToAutoZeroEnum(strValue, slot, regularChannel, settingKey);
                    //    keithley2602b.Measurement.AutoZero[regularChannel] = autoZero;
                    //    break;

                    case EnumKeithleySmuSetting.IS_TURNED_ON:
                        bool isEnabled = ConvertToBooleanValue(strValue, slot, regularChannel, settingKey);
                        keithley2602b.Source.OutputEnabled[regularChannel] = isEnabled;
                        break;
                    default:
                        break;
                }
            }

            return true;
        }

        public (bool isOk, string errorText) IsTriggerInSettingKeyMissed(Dictionary<string, string> settings) {
            HashSet<string> validKeySet = new HashSet<string>();
            foreach (EnumTriggerCommonSetting setting in Enum.GetValues(typeof(EnumTriggerCommonSetting))) {
                validKeySet.Add(setting.ToString());
            }
            foreach (EnumTriggerInstrument slotChannel in Enum.GetValues(typeof(EnumTriggerInstrument))) {
                validKeySet.Add(slotChannel.ToString());
            }
            foreach (EnumTriggerInSetting setting in Enum.GetValues(typeof(EnumTriggerInSetting))) {
                validKeySet.Add(setting.ToString());
            }

            string errorText = string.Empty;
            var inputKeySet = settings.Keys.ToHashSet();
            if (!validKeySet.SetEquals(inputKeySet)) {
                string validKeys = $"{string.Join(",", validKeySet)}";
                string inputKeys = $"{string.Join(",", inputKeySet)}";
                errorText = $"Current keys of trigger-in-settings is invalid! Valid keys are {{{validKeys}}}; Current keys are{{{inputKeys}}}.";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            return (true, string.Empty);
        }

        public (bool isOk, string errorText) CheckTriggerInSettings(Dictionary<string, string> settings) {
            string errorText = string.Empty;
            foreach (var setting in settings) {
                if (setting.Key.Equals(EnumTriggerInSetting.NPLC.ToString())) {
                    if (!double.TryParse(settings[setting.Key], out double nplc)) {
                        errorText = $"In trigger-in-settings, {EnumTriggerInSetting.NPLC.ToString()}(={settings[EnumTriggerInSetting.NPLC.ToString()]}) should be a double!";
                        LOGGER.Error(errorText);
                        return (false, errorText);
                    }
                } else if (setting.Key.Equals(EnumTriggerInSetting.TIME_IN_MS_PER_STEP.ToString())) {
                    if (!int.TryParse(settings[setting.Key], out int timeInMsPerStep)) {
                        errorText = $"In trigger-in-settings, {EnumTriggerInSetting.TIME_IN_MS_PER_STEP.ToString()}(={settings[EnumTriggerInSetting.TIME_IN_MS_PER_STEP.ToString()]}) should be an integer!";
                        LOGGER.Error(errorText);
                        return (false, errorText);
                    }
                } else if (setting.Key.Equals(EnumTriggerInSetting.RANGE.ToString())) {
                    if (!double.TryParse(settings[setting.Key], out double range)) {
                        errorText = $"In trigger-in-settings, {EnumTriggerInSetting.RANGE.ToString()}(={settings[EnumTriggerInSetting.RANGE.ToString()]}) should be a double!";
                        LOGGER.Error(errorText);
                        return (false, errorText);
                    }
                }
            }

            return (true, string.Empty);
        }

        public bool PrepareTriggeringIn(Dictionary<string, string> settings) {
            string slot = settings[EnumTriggerInstrument.SLOT.ToString()];
            string channel = settings[EnumTriggerInstrument.CHANNEL.ToString()];
            int.TryParse(settings[EnumTriggerCommonSetting.STEP_NUMBER.ToString()], out int stepNumber);
            double.TryParse(settings[EnumTriggerInSetting.NPLC.ToString()], out double nplc);
            int.TryParse(settings[EnumTriggerInSetting.TIME_IN_MS_PER_STEP.ToString()], out int timeInMsPerStep);
            double.TryParse(settings[EnumTriggerInSetting.RANGE.ToString()], out double range);

            try {
                //[gyh]: to be added---setting before trigger;
                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        public bool StartTriggeringIn() {
            try {
                //[gyh]: to be added---setting before trigger;
                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        public void WaitForTriggeringInCompleted() {
            //[gyh]: to be added---setting before trigger;
        }

        public (bool isOk, List<double> data) GetTriggeringInData() {
            //[gyh]: to be added---setting before trigger;
            throw new NotImplementedException();
        }

        #region private
        private (bool isOk, string errorText) IsSettingValid(string key, string value) {
            string errorText = string.Empty;
            if (!Enum.TryParse(key, out EnumKeithleySmuSetting enumSmuSetting)) {
                errorText = $"setting key={key} is invalid for Keighley SMU!";
                return (false, errorText);
            }

            switch (enumSmuSetting) {
                case EnumKeithleySmuSetting.SOURCE_TYPE:
                    if (!Enum.TryParse(value, out EnumSmuSourceType enumSourceType)) {
                        errorText = $"setting value={value} is invalid for {EnumKeithleySmuSetting.SOURCE_TYPE.ToString()} of SMU Keithley 2602B!"; ;
                        return (false, errorText);
                    }
                    break;
                case EnumKeithleySmuSetting.SOURCE_LIMIT:
                    if (!double.TryParse(value, out double sourceLimit)) {
                        errorText = $"setting value={value} is invalid for {EnumKeithleySmuSetting.SOURCE_LIMIT.ToString()} of SMU Keithley 2602B!"; ;
                        return (false, errorText);
                    }
                    break;
                case EnumKeithleySmuSetting.SOURCE_LEVEL:
                    if (!double.TryParse(value, out double sourceLevel)) {
                        errorText = $"setting value={value} is invalid for {EnumKeithleySmuSetting.SOURCE_LEVEL.ToString()} of SMU Keithley 2602B!"; ;
                        return (false, errorText);
                    }
                    break;
                case EnumKeithleySmuSetting.MEASURE_TYPE:
                    if (!Enum.TryParse(value, out EnumSmuMeasurementType enumMeasureType)) {
                        errorText = $"setting value={value} is invalid for {EnumKeithleySmuSetting.MEASURE_TYPE.ToString()} of SMU Keithley 2602B!"; ;
                        return (false, errorText);
                    }
                    break;
                case EnumKeithleySmuSetting.MEASURE_COUNT:
                    if (!int.TryParse(value, out int measureCount)) {
                        errorText = $"setting value={value} is invalid for {EnumKeithleySmuSetting.MEASURE_COUNT.ToString()} of SMU Keithley 2602B!"; ;
                        return (false, errorText);
                    }
                    break;
                case EnumKeithleySmuSetting.MEASURE_DELAY:
                    if (!double.TryParse(value, out double measureDelay)) {
                        errorText = $"setting value={value} is invalid for {EnumKeithleySmuSetting.MEASURE_DELAY.ToString()} of SMU Keithley 2602B!"; ;
                        return (false, errorText);
                    }
                    break;
                case EnumKeithleySmuSetting.MEASURE_NPLC:
                    if (!double.TryParse(value, out double nplc)) {
                        errorText = $"setting value={value} is invalid for {EnumKeithleySmuSetting.MEASURE_NPLC.ToString()} of SMU Keithley 2602B!"; ;
                        return (false, errorText);
                    }
                    break;
                case EnumKeithleySmuSetting.MEASURE_IS_AUTORANGE:
                    if (!bool.TryParse(value, out bool measureIsAutoRange)) {
                        errorText = $"setting value={value} is invalid for {EnumKeithleySmuSetting.MEASURE_IS_AUTORANGE.ToString()} of SMU Keithley 2602B!"; ;
                        return (false, errorText);
                    }
                    break;
                case EnumKeithleySmuSetting.MEASURE_RANGE:
                    if (!double.TryParse(value, out double measureRange)) {
                        errorText = $"setting value={value} is invalid for {EnumKeithleySmuSetting.MEASURE_RANGE.ToString()} of SMU Keithley 2602B!"; ;
                        return (false, errorText);
                    }
                    break;
                case EnumKeithleySmuSetting.IS_TURNED_ON:
                    if (!bool.TryParse(value, out bool isTurnedOn)) {
                        errorText = $"setting value={value} is invalid for {EnumKeithleySmuSetting.IS_TURNED_ON.ToString()} of SMU Keithley 2602B!"; ;
                        return (false, errorText);
                    }
                    break;
            }

            return (true, string.Empty);
        }

        private EnumSmuSourceType GetSourceType(string regularChannel) {
            if (!sourceTypes.ContainsKey(regularChannel)) {
                throw new Exception($"[{EnumInstrumentCategory.SMU}] {EnumInstrumentVendor.KEYSIGHT}-{EnumModelKeithley._2602B} channel={regularChannel} has not set source type.");
            }
            return sourceTypes[regularChannel];
        }

        private EnumSmuMeasurementType GetMeasureType(string regularChannel) {
            if (!measureTypes.ContainsKey(regularChannel)) {
                throw new Exception($"[{EnumInstrumentCategory.SMU}] {EnumInstrumentVendor.KEYSIGHT}-{EnumModelKeithley._2602B} channel={regularChannel} has not set measurement type.");
            }
            return measureTypes[regularChannel];
        }

        private double ConvertToDoubleValue(string strValue, string slot, string regularChannel, EnumKeithleySmuSetting settingKey) {
            double result = 0;
            if (!double.TryParse(strValue, out result)) {
                throw new InvalidInstrumentSettingValueException(
                    EnumInstrumentCategory.SMU.ToString(),
                    EnumInstrumentVendor.KEITHLEY.ToString(),
                    EnumModelKeithley._2602B.ToString(),
                    slot,
                    regularChannel,
                    settingKey.ToString(),
                    strValue);
            }

            return result;
        }

        private int ConvetToIntValue(string strValue, string slot, string regularChannel, EnumKeithleySmuSetting settingKey) {
            int result = 0;
            if (!int.TryParse(strValue, out result)) {
                throw new InvalidInstrumentSettingValueException(
                    EnumInstrumentCategory.SMU.ToString(),
                    EnumInstrumentVendor.KEITHLEY.ToString(),
                    EnumModelKeithley._2602B.ToString(),
                    slot,
                    regularChannel,
                    settingKey.ToString(),
                    strValue);
            }

            return result;
        }

        private bool ConvertToBooleanValue(string strValue, string slot, string regularChannel, EnumKeithleySmuSetting settingKey) {
            bool result;
            if (!bool.TryParse(strValue, out result)) {
                throw new InvalidInstrumentSettingValueException(
                    EnumInstrumentCategory.SMU.ToString(),
                    EnumInstrumentVendor.KEITHLEY.ToString(),
                    EnumModelKeithley._2602B.ToString(),
                    slot,
                    regularChannel,
                    settingKey.ToString(),
                    strValue);
            }

            return result;
        }

        private Ke26XXAAutoZeroEnum ConvertToAutoZeroEnum(string strValue, string slot, string regularChannel, EnumKeithleySmuSetting settingKey) {
            Ke26XXAAutoZeroEnum result;
            if (!Enum.TryParse(strValue, out result)) {
                throw new InvalidInstrumentSettingValueException(
                    EnumInstrumentCategory.SMU.ToString(),
                    EnumInstrumentVendor.KEITHLEY.ToString(),
                    EnumModelKeithley._2602B.ToString(),
                    slot,
                    regularChannel,
                    settingKey.ToString(),
                    strValue);
            }

            return result;
        }

        private string GetRegularChannelString(string channel) {
            return channel.ToUpperInvariant().Trim();
        }

        private (bool isOk, Dictionary<EnumKeithleySmuSetting, string> result) TryConvertToEnumSettings(Dictionary<string, string> settings) {
            Dictionary<EnumKeithleySmuSetting, string> result = new Dictionary<EnumKeithleySmuSetting, string>();

            foreach (var one in settings) {
                string key = one.Key;
                string value = one.Value;

                if (!Enum.TryParse(key, out EnumKeithleySmuSetting enumSmuSetting)) {
                    LOGGER.Error($"Setting key={key} of Keithley 2602B is invalid!");
                    return (false, null);
                }

                result.Add(enumSmuSetting, value);
            }

            return (true, result);
        }

        #endregion

        private readonly IKe26XXA keithley2602b = new Ke26XXA();
        private readonly Dictionary<string, EnumSmuSourceType> sourceTypes = new Dictionary<string, EnumSmuSourceType>();
        private readonly Dictionary<string, EnumSmuMeasurementType> measureTypes = new Dictionary<string, EnumSmuMeasurementType>();

        public enum EnumChannels {
            A,
            B
        }

        public enum EnumAutoZeroType {
            OFF,
            ONCE,
            AUTO
        }

        public enum EnumTriggerInSetting {
            NPLC,
            TIME_IN_MS_PER_STEP,
            RANGE
        }
    }
}
