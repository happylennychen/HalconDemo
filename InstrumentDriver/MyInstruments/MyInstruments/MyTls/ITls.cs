using System.Collections.Generic;

using MyInstruments.MyEnum;

namespace MyInstruments.MyTls {
    public interface ITls {
        void TlsSetPower(string slot, string channel, double powerInDbm);
        double TlsGetPower(string slot, string channel);
        void TlsSetWavelength(string slot, string channel, double waveLengthInNm);
        double TlsGetWavelength(string slot, string channel);
        void TlsSetPowerUnitType(string slot, string channel, EnumOpticPowerUnitType powerUnitType);
        EnumOpticPowerUnitType TlsGetPowerUnitType(string slot, string channel);
        bool TlsIsTurnedOn(string slot, string channel);                
        void TlsTurnOff(string slot, string channel);
        void TlsTurnOn(string slot, string channel);

        //--------------------------------------------------------------------------------------
        (bool isOk, string errorText) TlsAreSettingsValid(Dictionary<string, string> settings);
        bool TlsBatchSetting(string slot, string channel, Dictionary<string, string> settings);
    }
}
