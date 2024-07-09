using System.Collections.Generic;

using MyInstruments.MyEnum;

namespace MyInstruments.MyOpm {
    public interface IOpm {
        double OpmFetchPower(string slot, string channel);
        double OpmReadPower(string slot, string channel);        
        void OpmSetAverageTime(string slot, string channel, double averageTime, EnumOpmTimeUnitType timeUnitType);
        void OpmSetWavelength(string slot, string channel, double waveLengthInNm);
        void OpmSetPowerUnitType(string slot, string channel, EnumOpticPowerUnitType powerUnitType);        
        void OpmSetPowerRangeType(string slot, string channel, EnumOpmPowerRangeType powerRangeType);        
        void OpmSetPowerRange(string slot, string channel, int rangeInDbm);
        void OpmSetNoTriggerMode(string slot, string channel);
        void OpmSetSamplePoint(string slot, string channel, int num, double averageTimeInMs);
        //--------------------------------------------------------------------------------------
        (bool isOk, string errorText) OpmAreSettingsValid(Dictionary<string, string> settings);
        bool OpmBatchSetting(string slot, string channel, Dictionary<string, string> settings);
    }
}
