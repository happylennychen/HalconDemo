using System.Collections.Generic;

using MyInstruments.MyEnum;

namespace MyInstruments.MyOpm {
    public abstract class StandaloneOpm : Instrument, IOpm {
        public StandaloneOpm(string id) : base(id) { }

        public abstract double OpmFetchPower(string slot, string channel);
        public abstract double OpmReadPower(string slot, string channel);
        public abstract void OpmSetAverageTime(string slot, string channel, double averageTime, EnumOpmTimeUnitType timeUnitType);
        public abstract void OpmSetPowerRange(string slot, string channel, int rangeInDbm);
        public abstract void OpmSetPowerRangeType(string slot, string channel, EnumOpmPowerRangeType powerRangeType);
        public abstract void OpmSetPowerUnitType(string slot, string channel, EnumOpticPowerUnitType powerUnitType);
        public abstract void OpmSetWavelength(string slot, string channel, double waveLengthInNm);
        public abstract void OpmSetNoTriggerMode(string slot, string channel);
        public abstract void OpmSetSamplePoint(string slot, string channel, int num, double averageTimeInMs);

        public abstract (bool isOk, string errorText) OpmAreSettingsValid(Dictionary<string, string> settings);
        public abstract bool OpmBatchSetting(string slot, string channel, Dictionary<string, string> settings);
    }
}
