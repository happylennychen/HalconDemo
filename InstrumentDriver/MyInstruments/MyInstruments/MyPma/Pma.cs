using MyInstruments.MyEnum;
using MyInstruments.MyOpm;
using MyInstruments.MyTls;

using System.Collections.Generic;

namespace MyInstruments.MyPma {
    public abstract class Pma : Instrument, IOpm, ITls {
        protected Pma(string id) : base(id) {
        }

        public abstract double OpmFetchPower(string slot, string channel);        
        public abstract double OpmReadPower(string slot, string channel);
        public abstract void OpmSetAverageTime(string slot, string channel, double averageTime, EnumOpmTimeUnitType timeUnitType);
        public abstract void OpmSetPowerRange(string slot, string channel, int rangeInDbm);
        public abstract void OpmSetPowerRangeType(string slot, string channel, EnumOpmPowerRangeType powerRangeType);
        public abstract void OpmSetPowerUnitType(string slot, string channel, EnumOpticPowerUnitType powerUnitType);        
        public abstract void OpmSetWavelength(string slot, string channel, double waveLengthInNm);                
        public abstract (bool isOk, string errorText) OpmAreSettingsValid(Dictionary<string, string> settings);
        public abstract bool OpmBatchSetting(string slot, string channel, Dictionary<string, string> settings);
        public abstract void OpmSetNoTriggerMode(string slot, string channel);
        public abstract void OpmSetSamplePoint(string slot, string channel,int num, double averageTimeInMs);

        public abstract void TlsSetPowerUnitType(string slot, string channel, EnumOpticPowerUnitType powerUnitType);
        public abstract EnumOpticPowerUnitType TlsGetPowerUnitType(string slot, string channel);
        public abstract double TlsGetPower(string slot, string channel);
        public abstract double TlsGetWavelength(string slot, string channel);
        public abstract bool TlsIsTurnedOn(string slot, string channel);
        public abstract void TlsSetPower(string slot, string channel, double powerInDbm);
        public abstract void TlsSetWavelength(string slot, string channel, double waveLengthInNm);
        public abstract void TlsTurnOff(string slot, string channel);
        public abstract void TlsTurnOn(string slot, string channel);
        public abstract (bool isOk, string errorText) TlsAreSettingsValid(Dictionary<string, string> settings);
        public abstract bool TlsBatchSetting(string slot, string channel, Dictionary<string, string> settings);
        
        //public abstract void OsSwitchTo(string slot, string channel, string inPort, string outPort);
        //public abstract void OaSetAttenuation(string slot, string channel, double attenuationInDbm);
    }
}
