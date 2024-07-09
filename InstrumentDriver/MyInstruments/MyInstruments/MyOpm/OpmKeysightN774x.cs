using MyInstruments.MyEnum;
using MyInstruments.MyException;
using MyInstruments.MyUtility;
using MyInstruments.MyVisaDriver;

using System;
using System.Collections.Generic;
using System.Threading;

namespace MyInstruments.MyOpm {
    public sealed class OpmKeysightN774x : StandaloneOpmWithTriggerIn {
        public OpmKeysightN774x(string id) : base(id) {
            Vendor = EnumInstrumentVendor.KEYSIGHT.ToString();
            SupportedModels = new HashSet<string> { "N774x" };
        }

        public override double OpmFetchPower(string slot, string channel) {
            string command = $"FETC{slot}:POW?";
            string response = visaDriver.QueryLine(command);
            return Convert.ToDouble(response);
        }

        public override double OpmReadPower(string slot, string channel) {
            int try_times = 0;
            double result = 0;
            string command = $"READ{slot}:POW?";
            while (true) {
                try {
                    string response = visaDriver.QueryLine(command);
                    result = Convert.ToDouble(response);
                    if (result > OpmConstant.UPPER_OPTIC_POWER_VALUE) {
                        result = OpmConstant.INVALID_OPTIC_POWER_VALUE;
                    } else {
                        break;
                    }
                } catch (Exception ex) {
                    ++try_times;
                    if (try_times > InstrumentConstant.MAX_RETRY_TIMES) {
                        throw ex;
                    }
                    Thread.Sleep(300);
                }
            }
            return result;
        }

        public override void OpmSetAverageTime(string slot, string channel, double averageTime, EnumOpmTimeUnitType timeUnitType) {
            string command = $"SENS{slot}:POW:ATIM {averageTime}{timeUnitType.ToString()}";
            visaDriver.WriteLine(command);
        }

        public override void OpmSetWavelength(string slot, string channel, double waveLengthInNm) {
            string command = $"SENS{slot}:POW:WAV {waveLengthInNm}NM";
            visaDriver.WriteLine(command);
        }

        public override void OpmSetPowerUnitType(string slot, string channel, EnumOpticPowerUnitType powerUnitType) {
            string flag = string.Empty;
            switch (powerUnitType) {
                case EnumOpticPowerUnitType.DBM:
                    flag = "1";
                    break;
                case EnumOpticPowerUnitType.W:
                    flag = "0";
                    break;
            }

            string command = $"SENS{slot}:POW:UNIT {flag}";
            visaDriver.WriteLine(command);
        }

        public override void OpmSetPowerRange(string slot, string channel, int rangeInDbm) {
            int actualRangeIndBm = 0;
            if (rangeInDbm % 10 == 0) {
                actualRangeIndBm = rangeInDbm;
            } else {
                int quotient = rangeInDbm / 10;
                actualRangeIndBm = (quotient + 1) * 10;
            }

            string command = $"SENS{slot}:POW:RANG {actualRangeIndBm}DBM";
            visaDriver.WriteLine(command);
        }

        public override void OpmSetPowerRangeType(string slot, string channel, EnumOpmPowerRangeType powerRangeType) {
            string flag = string.Empty;
            switch (powerRangeType) {
                case EnumOpmPowerRangeType.MANUAL:
                    flag = "0";
                    break;
                case EnumOpmPowerRangeType.AUTO:
                    flag = "1";
                    break;
            }
            string command = $"SENS{slot}:POW:RANG:AUTO {flag}";
            visaDriver.WriteLine(command);
        }

        public override void OpmSetNoTriggerMode(string slot, string channel)
        {
            return;
        }

        public override void OpmSetSamplePoint(string slot, string channel, int num, double averageTimeInMs) {
            return;
        }

        public override void Connect(string visaResource) {
            this.visaDriver = VisaDriverFactory.CreateInstance();
            this.visaDriver.Connect(visaResource);
        }

        public override void Disconnect() {
            this.visaDriver?.Disconnect();
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel) {
            if (!int.TryParse(channel, out int intChannel)) {
                throw new InvalidSlotOrChannelOfInstrumentException(slot, channel, this.Vendor, this.CurrentModel);
            }

            if (!(intChannel == 4 || intChannel == 8)) {
                throw new InvalidSlotOrChannelOfInstrumentException(slot, channel, this.Vendor, this.CurrentModel);
            }
        }

        public override bool OpmBatchSetting(string slot, string channel, Dictionary<string, string> settings) {
            OpmSettingUtilityKeysight settingUtility = new OpmSettingUtilityKeysight();
            return settingUtility.BatchSetting(this, slot, channel, settings);
        }

        public override (bool isOk, string errorText) OpmAreSettingsValid(Dictionary<string, string> settings) {            
            OpmSettingUtilityKeysight settingUtility = new OpmSettingUtilityKeysight();
            foreach (var setting in settings) {
                string key = setting.Key;
                string value = setting.Value;
                var checkResult = settingUtility.IsSettingValid(key, value);
                if (!checkResult.isOk) {
                    return (false, checkResult.errorText);
                }
            }

            return (true, string.Empty);
        }

        //[gyh]: to be implemented!
        public override (bool isOk, string errorText) CheckTriggerInSettings(Dictionary<string, string> settings) {
            throw new NotImplementedException();
        }

        public override (bool isOk, string errorText) IsTriggerInSettingKeyMissed(Dictionary<string, string> settings) {
            throw new NotImplementedException();
        }

        public override bool PrepareTriggeringIn(Dictionary<string, string> settings) {
            throw new NotImplementedException();
        }

        public override bool StartTriggeringIn() {
            throw new NotImplementedException();
        }

        public override void WaitForTriggeringInCompleted() {
            throw new NotImplementedException();
        }

        public override (bool isOk, List<double> data) GetTriggeringInData() {
            throw new NotImplementedException();
        }

        private IVisaDriver visaDriver;
    }
}
