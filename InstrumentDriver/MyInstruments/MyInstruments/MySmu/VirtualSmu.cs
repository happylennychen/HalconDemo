using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using CommonApi.MyTrigger;
using CommonApi.MyUtility;

namespace MyInstruments.MySmu {
    public sealed class VirtualSmu : StandaloneSmu, ITriggerIn {
        public VirtualSmu(string id) : base(id) {
            Vendor = "Virtual smu vendor";
            SupportedModels = new HashSet<string> { "Virtual smu model" };
        }

        public override void Connect(string visaResource) {
            this.visaResource = visaResource;
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualSmu.Connect({visaResource})");
        }

        public override void Disconnect() {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualSmu[{visaResource}].Disconnect()");
        }

        public override void SetSourceLevel(string slot, string channel, double level) {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualSmu[{visaResource}].SetSourceLevel({slot}, {channel}, {level})");
        }

        public override void TurnOff(string slot, string channel) {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualSmu[{visaResource}].TurnOff({slot}, {channel})");
        }

        public override void TurnOn(string slot, string channel) {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualSmu[{visaResource}].TurnOn({slot}, {channel})");
        }

        public override double Measure(string slot, string channel) {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            LOGGER.Info($"call VirtualSmu[{visaResource}].Measure({slot}, {channel})");
            return MyRandomUtility.GetRealRandomDouble();
        }

        public override void SetSourceType(string slot, string channel, EnumSmuSourceType sourceType) {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualSmu[{visaResource}].SetSourceType({slot}, {channel}, {sourceType.ToString()})");
        }

        public override void SetMeasurementType(string slot, string channel, EnumSmuMeasurementType measurementType) {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualSmu[{visaResource}].SetMeasurementType({slot}, {channel}, {measurementType.ToString()})");
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel) {
        }

        public override void SetSourceLimit(string slot, string channel, double limit) {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualSmu[{visaResource}].SetSourceLimit({slot}, {channel}, {limit})");
        }

        public override (bool isOk, string errorText) SmuAreSettingsValid(Dictionary<string, string> settings) {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualSmu[{visaResource}].SmuAreSettingsValid(...)");
            return (true, string.Empty);
        }

        public override bool SmuBatchSetting(string slot, string channel, Dictionary<string, string> settings) {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualSmu[{visaResource}].SmuBatchSetting({slot}, {channel}, settings)");
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
                        errorText = $"In trigger-in-settings, {SmuKeithley2602B.EnumTriggerInSetting.RANGE.ToString()}(={settings[EnumTriggerInSetting.RANGE.ToString()]}) should be a double!";
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
            triggerInPointNumber = stepNumber;
            double.TryParse(settings[EnumTriggerInSetting.NPLC.ToString()], out double nplc);
            int.TryParse(settings[EnumTriggerInSetting.TIME_IN_MS_PER_STEP.ToString()], out int timeInMsPerStep);
            double.TryParse(settings[EnumTriggerInSetting.RANGE.ToString()], out double range);

            try {
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
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        public (bool isOk, List<double> data) GetTriggeringInData() {
            List<double> data = new List<double>();
            for (int i = 0; i < triggerInPointNumber; i++) {
                data.Add(MyRandomUtility.GetRealRandomDouble());
            }

            return (true, data);
        }

        private string visaResource = string.Empty;
        private int triggerInPointNumber;

        public enum EnumTriggerInSetting {
            NPLC,
            TIME_IN_MS_PER_STEP,
            RANGE
        }
    }
}
