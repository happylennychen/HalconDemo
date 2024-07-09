using System;
using System.Collections.Generic;
using System.Threading;

using CommonApi.MyUtility;

using MyInstruments.MyEnum;

namespace MyInstruments.MyTls {
    public sealed class VirtualTlsWithTriggerOut : StandaloneTlsWithTriggerOut {
        public VirtualTlsWithTriggerOut(string id) : base(id) {
            Vendor = "Virtual TLS vendor";
            SupportedModels = new HashSet<string> { "Virtual TLS model" };
            isTurnedOn = false;
            powerInDbm = MyRandomUtility.GetRealRandomDouble();
            waveLengthInNm = 1310.0;
        }

        public override void Connect(string visaResource) {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.Connect({visaResource})");
        }

        public override void Disconnect() {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.Disconnect()");
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel) {
        }

        public override double TlsGetPower(string slot, string channel) {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.TlsGetPower({slot}, {channel})");
            return powerInDbm;
        }

        public override double TlsGetWavelength(string slot, string channel) {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.TlsGetWavelength({slot}, {channel})");
            return waveLengthInNm;
        }

        public override bool TlsIsTurnedOn(string slot, string channel) {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.TlsIsTurnedOn({slot}, {channel})");
            return isTurnedOn;
        }

        public override void TlsSetPower(string slot, string channel, double powerInDbm) {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.TlsSetPower({slot}, {channel}, {powerInDbm})");
            this.powerInDbm = powerInDbm;
        }

        public override void TlsSetWavelength(string slot, string channel, double waveLengthInNm) {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.TlsSetWavelength({slot}, {channel}, {waveLengthInNm})");
            this.waveLengthInNm = waveLengthInNm;
        }

        public override void TlsTurnOff(string slot, string channel) {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.TlsTurnOff({slot}, {channel})");
            isTurnedOn = false;
        }

        public override void TlsTurnOn(string slot, string channel) {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.TlsTurnOn({slot}, {channel})");
            isTurnedOn = true;
        }

        public override (bool isOk, string errorText) TlsAreSettingsValid(Dictionary<string, string> settings) {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.TlsAreSettingsValid(settings)");
            return (true, string.Empty);
        }

        public override bool TlsBatchSetting(string slot, string channel, Dictionary<string, string> settings) {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.TlsBatchSetting({slot}, {channel}, settings)");
            return true;
        }

        public override void TlsSetPowerUnitType(string slot, string channel, EnumOpticPowerUnitType powerUnitType) {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.TlsSetPowerUnitType({slot}, {channel}, {powerUnitType.ToString()})");
            this.powerUnitType = powerUnitType;
        }

        public override EnumOpticPowerUnitType TlsGetPowerUnitType(string slot, string channel) {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.TlsSetPowerUnitType({slot}, {channel})");
            return powerUnitType;
        }

        public override (bool isOk, string errorText) CheckTriggerOutSettings(Dictionary<string, string> settings) {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.CheckTriggerOutSettings(...)");
            return (true, string.Empty);
        }

        public override (bool isOk, List<double> data) GetTriggeringOutData() {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.GetTriggeringOutData()");
            return (true, new List<double>());
        }

        public override (bool isOk, string errorText) IsTriggerOutSettingKeyMissed(Dictionary<string, string> settings) {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.IsTriggerOutSettingKeyMissed(...)");
            return (true, string.Empty);
        }

        public override bool PrepareTriggeringOut(Dictionary<string, string> settings) {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.PrepareTriggeringOut(...)");
            return true;
        }

        public override bool StartTriggeringOut() {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            LOGGER.Info($"call VirtualTlsWithTriggerOut.StartTriggeringOut()");
            return true;
        }

        public override void WaitForTriggeringOutCompleted() {
            LOGGER.Info($"call VirtualTlsWithTriggerOut.WaitForTriggeringOutCompleted()");
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        private bool isTurnedOn;
        private double powerInDbm;
        private double waveLengthInNm;
        private EnumOpticPowerUnitType powerUnitType = EnumOpticPowerUnitType.DBM;
    }
}
