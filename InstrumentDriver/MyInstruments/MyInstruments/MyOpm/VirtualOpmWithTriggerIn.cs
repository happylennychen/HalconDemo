using System;
using System.Collections.Generic;
using System.Threading;

using CommonApi.MyUtility;

using MyInstruments.MyEnum;

namespace MyInstruments.MyOpm {
    public sealed class VirtualOpmWithTriggerIn : StandaloneOpmWithTriggerIn {
        public VirtualOpmWithTriggerIn(string id) : base(id) {
            Vendor = "Virtual opm vendor";
            SupportedModels = new HashSet<string> { "Virtual opm model" };
        }

        public override double OpmFetchPower(string slot, string channel) {
            LOGGER.Info($"call VirtualOpmWithTriggerIn[{visaResource}].OpmFetchPower({slot}, {channel})");
            Thread.Sleep(TimeSpan.FromMilliseconds(10));

            double value = MyRandomUtility.GetRealRandomDouble();
            double result = 0;
            if (isUnitDbm) {
                result = 100 * value - 100.0;
            } else {
                result = value * 0.001;
            }
            return result;
        }

        public override double OpmReadPower(string slot, string channel) {
            LOGGER.Info($"call VirtualOpmWithTriggerIn[{visaResource}].OpmReadPower({slot}, {channel})");
            Thread.Sleep(TimeSpan.FromMilliseconds(averageTimeInMs));

            double value = MyRandomUtility.GetRealRandomDouble();
            double result = 0;
            if (isUnitDbm) {
                result = 100 * value - 100.0;
            } else {
                result = value * 0.001;
            }
            return result;
        }

        public override void OpmSetAverageTime(string slot, string channel, double averageTime, EnumOpmTimeUnitType timeUnitType) {
            LOGGER.Info($"call VirtualOpmWithTriggerIn[{visaResource}].OpmSetAverageTime({slot}, {channel}, {averageTime}, {timeUnitType.ToString()})");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
        }

        public override void OpmSetPowerRange(string slot, string channel, int rangeInDbm) {
            LOGGER.Info($"call VirtualOpmWithTriggerIn[{visaResource}].OpmSetRange({slot}, {channel}, {rangeInDbm})");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
        }

        public override void OpmSetPowerRangeType(string slot, string channel, EnumOpmPowerRangeType powerRangeType) {
            LOGGER.Info($"call VirtualOpmWithTriggerIn[{visaResource}].OpmSetRangeAuto({slot}, {channel}, {powerRangeType.ToString()})");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
        }

        public override void OpmSetNoTriggerMode(string slot, string channel)
        {
            LOGGER.Info($"call VirtualOpmWithTriggerIn[{visaResource}].OpmSetNoTriggerMode({slot}, {channel})");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
        }

        public override void OpmSetSamplePoint(string slot, string channel, int num, double averageTimeInMs)
        {
            LOGGER.Info($"call VirtualOpmWithTriggerIn[{visaResource}].OpmSetSamplePoint({slot}, {channel}, {num}，{averageTimeInMs})");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
        }

        public override void OpmSetPowerUnitType(string slot, string channel, EnumOpticPowerUnitType powerUnitType) {
            LOGGER.Info($"call VirtualOpmWithTriggerIn[{visaResource}].OpmSetUnitDbm({slot}, {channel}, {powerUnitType.ToString()})");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));

            isUnitDbm = true;
        }

        public override void OpmSetWavelength(string slot, string channel, double waveLengthInNm) {
            LOGGER.Info($"call VirtualOpmWithTriggerIn[{visaResource}].OpmSetWavelength({slot}, {channel}, {waveLengthInNm})");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
        }

        public override void Connect(string visaResource) {
            LOGGER.Info($"call VirtualOpmWithTriggerIn.Connect({visaResource})");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            this.visaResource = visaResource;
        }

        public override void Disconnect() {
            LOGGER.Info($"call VirtualOpmWithTriggerIn[{visaResource}].Disconnect()");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel) {
        }

        public override bool OpmBatchSetting(string slot, string channel, Dictionary<string, string> settings) {
            LOGGER.Info($"call VirtualOpmWithTriggerIn[{visaResource}].OpmInitialSetting({slot}, {channel}, ...)");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            return true;
        }

        public override (bool isOk, string errorText) OpmAreSettingsValid(Dictionary<string, string> settings) {
            LOGGER.Info($"call VirtualOpmWithTriggerIn[{visaResource}].OpmAreSettingsValid(...)");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            return (true, string.Empty);
        }

        public override (bool isOk, string errorText) CheckTriggerInSettings(Dictionary<string, string> settings) {
            LOGGER.Info($"call VirtualOpmWithTriggerIn[{visaResource}].CheckTriggerInSettings(...)");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            return (true, string.Empty);
        }

        public override (bool isOk, string errorText) IsTriggerInSettingKeyMissed(Dictionary<string, string> settings) {
            LOGGER.Info($"call VirtualOpmWithTriggerIn[{visaResource}].IsTriggerInSettingKeyMissed(...)");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            return (true, string.Empty);
        }

        public override bool PrepareTriggeringIn(Dictionary<string, string> settings) {
            LOGGER.Info($"call VirtualOpmWithTriggerIn[{visaResource}].PrepareTriggeringIn(...)");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            return true;
        }

        public override bool StartTriggeringIn() {
            LOGGER.Info($"call VirtualOpmWithTriggerIn[{visaResource}].StartTriggeringIn()");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            return true;
        }

        public override void WaitForTriggeringInCompleted() {
            LOGGER.Info($"call VirtualOpmWithTriggerIn[{visaResource}].WaitForTriggeringInCompleted()");
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        public override (bool isOk, List<double> data) GetTriggeringInData() {
            return (true, new List<double>());
        }

        private bool isUnitDbm = true;
        private int averageTimeInMs = 50;
        private string visaResource = string.Empty;
    }
}
