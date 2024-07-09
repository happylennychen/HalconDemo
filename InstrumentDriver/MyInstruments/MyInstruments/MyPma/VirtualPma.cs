using System;
using System.Collections.Generic;
using System.Threading;

using CommonApi.MyTrigger;
using CommonApi.MyUtility;

using MyInstruments.MyEnum;
using MyInstruments.MyOpm;
using MyInstruments.MyTls;

namespace MyInstruments.MyPma {
    public sealed class VirtualPma : Pma, ITriggerIn {
        public VirtualPma(string id) : base(id) {
            Vendor = "Virtual pma vendor";
            SupportedModels = new HashSet<string> { "Virtual pma model" };
            virtualOpm = new VirtualOpmWithTriggerIn(id);
            virtualTls = new VirtualTlsWithTriggerOut(id);
        }

        public override void Connect(string visaResource) {
            this.VisaResource = visaResource;
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualPma.Connect({visaResource})");
        }

        public override void Disconnect() {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualPma.Disconnect()");
        }

        public override double OpmFetchPower(string slot, string channel) {
            return virtualOpm.OpmFetchPower(slot, channel);
        }

        public override double OpmReadPower(string slot, string channel) {
            return virtualOpm.OpmReadPower(slot, channel);
        }

        public override void OpmSetAverageTime(string slot, string channel, double averageTime, EnumOpmTimeUnitType timeUnitType) {
            virtualOpm.OpmSetAverageTime(slot, channel, averageTime, timeUnitType);
        }

        public override void OpmSetPowerRange(string slot, string channel, int rangeInDbm) {
            virtualOpm.OpmSetPowerRange(slot, channel, rangeInDbm);
        }

        public override void OpmSetPowerRangeType(string slot, string channel, EnumOpmPowerRangeType powerRangeType) {
            virtualOpm.OpmSetPowerRangeType(slot, channel, powerRangeType);
        }

        public override void OpmSetNoTriggerMode(string slot, string channel)
        {
            virtualOpm.OpmSetNoTriggerMode(slot, channel);
        }

        public override void OpmSetSamplePoint(string slot, string channel,int num, double averageTimeInMs)
        {
            virtualOpm.OpmSetSamplePoint(slot, channel,num,averageTimeInMs);
        }

        public override void OpmSetPowerUnitType(string slot, string channel, EnumOpticPowerUnitType powerUnitType) {
            virtualOpm.OpmSetPowerUnitType(slot, channel, powerUnitType);
        }

        public override void OpmSetWavelength(string slot, string channel, double waveLengthInNm) {
            virtualOpm.OpmSetWavelength(slot, channel, waveLengthInNm);
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel) {
        }

        public override (bool isOk, string errorText) OpmAreSettingsValid(Dictionary<string, string> settings) {
            return virtualOpm.OpmAreSettingsValid(settings);
        }

        public override bool OpmBatchSetting(string slot, string channel, Dictionary<string, string> settings) {
            return virtualOpm.OpmBatchSetting(slot, channel, settings);
        }

        public override double TlsGetPower(string slot, string channel) {
            return virtualTls.TlsGetPower(slot, channel);
        }

        public override double TlsGetWavelength(string slot, string channel) {
            return virtualTls.TlsGetWavelength(slot, channel);
        }

        public override bool TlsIsTurnedOn(string slot, string channel) {
            return virtualTls.TlsIsTurnedOn(slot, channel);
        }

        public override void TlsSetPower(string slot, string channel, double powerInDbm) {
            virtualTls.TlsSetPower(slot, channel, powerInDbm);
        }

        public override void TlsSetWavelength(string slot, string channel, double waveLengthInNm) {
            virtualTls.TlsSetWavelength(slot, channel, waveLengthInNm);
        }

        public override void TlsTurnOff(string slot, string channel) {
            virtualTls.TlsTurnOff(slot, channel);
        }

        public override void TlsTurnOn(string slot, string channel) {
            virtualTls.TlsTurnOn(slot, channel);
        }

        public override (bool isOk, string errorText) TlsAreSettingsValid(Dictionary<string, string> settings) {
            return virtualTls.TlsAreSettingsValid(settings);
        }

        public override bool TlsBatchSetting(string slot, string channel, Dictionary<string, string> settings) {
            return virtualTls.TlsBatchSetting(slot, channel, settings);
        }

        public override void TlsSetPowerUnitType(string slot, string channel, EnumOpticPowerUnitType powerUnitType) {
            virtualTls.TlsSetPowerUnitType(slot, channel, powerUnitType);
        }

        public override EnumOpticPowerUnitType TlsGetPowerUnitType(string slot, string channel) {
            return virtualTls.TlsGetPowerUnitType(slot, channel);
        }

        public (bool isOk, string errorText) IsTriggerInSettingKeyMissed(Dictionary<string, string> settings) {
            LOGGER.Info($"Call VirtualPma.IsTriggerInSettingKeyMissed(...)");
            return (true, string.Empty);
        }

        public (bool isOk, string errorText) CheckTriggerInSettings(Dictionary<string, string> settings) {
            string errorText = string.Empty;
            foreach (var setting in settings) {
                string key = setting.Key;
                string value = setting.Value;

                if (!Enum.TryParse(key, out PmaKeysight816x.EnumTriggerInSetting enumKey)) {
                    errorText = $"VirtualPma trigger-in-settings, key(={key}) is not a valid!";
                    LOGGER.Error(errorText);
                    return (false, errorText);
                }

                switch (enumKey) {
                    case PmaKeysight816x.EnumTriggerInSetting.AVERAGE_TIME_IN_MS:
                        if (!int.TryParse(value, out int averageTimeInMs)) {
                            errorText = $"VirtualPma trigger-in-settings, {PmaKeysight816x.EnumTriggerInSetting.AVERAGE_TIME_IN_MS.ToString()}(={settings[PmaKeysight816x.EnumTriggerInSetting.AVERAGE_TIME_IN_MS.ToString()]}) should be an integer!";
                            LOGGER.Error(errorText);
                            return (false, errorText);
                        }
                        break;
                    case PmaKeysight816x.EnumTriggerInSetting.TIME_IN_MS_PER_POINT:
                        if (!int.TryParse(value, out int rangeInDbm)) {
                            errorText = $"VirtualPma trigger-in-settings, {PmaKeysight816x.EnumTriggerInSetting.RANGE_IN_DBM.ToString()}(={settings[PmaKeysight816x.EnumTriggerInSetting.RANGE_IN_DBM.ToString()]}) should be an integer!";
                            LOGGER.Error(errorText);
                            return (false, errorText);
                        }
                        break;
                    case PmaKeysight816x.EnumTriggerInSetting.RANGE_IN_DBM:
                        if (!int.TryParse(value, out int timeInMsPerPoint)) {
                            errorText = $"VirtualPma trigger-in-settings, {PmaKeysight816x.EnumTriggerInSetting.TIME_IN_MS_PER_POINT.ToString()}(={settings[PmaKeysight816x.EnumTriggerInSetting.TIME_IN_MS_PER_POINT.ToString()]}) should be an integer!";
                            LOGGER.Error(errorText);
                            return (false, errorText);
                        }
                        break;
                }
            }

            return (true, string.Empty);
        }

        public bool PrepareTriggeringIn(Dictionary<string, string> settings) {
            LOGGER.Info($"Call VirtualPma.PrepareTriggeringIn(...)");

            string triggerInSlot = settings[EnumTriggerInstrument.SLOT.ToString()];
            string triggerInChannel = settings[EnumTriggerInstrument.CHANNEL.ToString()];
            int.TryParse(settings[EnumTriggerCommonSetting.STEP_NUMBER.ToString()], out triggerStepNumber);
            int.TryParse(settings[PmaKeysight816x.EnumTriggerInSetting.AVERAGE_TIME_IN_MS.ToString()], out int averageTimeInMs);
            int.TryParse(settings[PmaKeysight816x.EnumTriggerInSetting.RANGE_IN_DBM.ToString()], out int rangeInDbm);
            int.TryParse(settings[PmaKeysight816x.EnumTriggerInSetting.TIME_IN_MS_PER_POINT.ToString()], out int timeInMsPerPoint);
            return true;
        }

        public bool StartTriggeringIn() {
            LOGGER.Info($"Call VirtualPma.StartTriggeringIn()");
            return true;
        }

        public void WaitForTriggeringInCompleted() {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            LOGGER.Info($"Call VirtualPma.WaitForTriggeringInCompleted()");
        }

        public (bool isOk, List<double> data) GetTriggeringInData() {
            List<double> data = new List<double>();
            for (int i = 0; i < triggerStepNumber; i++) {
                data.Add(MyRandomUtility.GetRealRandomDouble());
            }

            return (true, data);
        }

        //public override void OsSwitchTo(string slot, string channel, string inPort, string outPort) {
        //    virtualOs.OsSwitchTo(slot, channel, inPort, outPort);
        //}

        //public override void OaSetAttenuation(string slot, string channel, double attenuationInDbm) {
        //    virtualOa.OaSetAttenuation(slot, channel, attenuationInDbm);
        //}

        private readonly VirtualOpmWithTriggerIn virtualOpm;
        private readonly VirtualTlsWithTriggerOut virtualTls;
        //private readonly VirtualOa virtualOa = new VirtualOa();
        //private readonly VirtualOs virtualOs = new VirtualOs();

        private int triggerStepNumber;
    }
}
