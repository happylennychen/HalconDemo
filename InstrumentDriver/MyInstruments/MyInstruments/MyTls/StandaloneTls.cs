using System.Collections.Generic;

using MyInstruments.MyEnum;

namespace MyInstruments.MyTls {
    public abstract class StandaloneTls : Instrument, ITls {
        public StandaloneTls(string id) : base(id) {
        }

        public abstract void TlsSetPowerUnitType(string slot, string channel, EnumOpticPowerUnitType powerUnitType);
        public abstract EnumOpticPowerUnitType TlsGetPowerUnitType(string slot, string channel);

        public abstract double TlsGetPower(string slot, string channel);
        public abstract void TlsSetPower(string slot, string channel, double powerInDbm);

        public abstract double TlsGetWavelength(string slot, string channel);
        public abstract void TlsSetWavelength(string slot, string channel, double waveLengthInNm);

        public abstract bool TlsIsTurnedOn(string slot, string channel);                
        public abstract void TlsTurnOff(string slot, string channel);
        public abstract void TlsTurnOn(string slot, string channel);

        public abstract (bool isOk, string errorText) TlsAreSettingsValid(Dictionary<string, string> settings);

        public abstract bool TlsBatchSetting(string slot, string channel, Dictionary<string, string> settings);        
    }
}
