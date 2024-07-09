using System;

namespace MyInstruments.MyException {
    public sealed class InvalidInstrumentSettingValueException : Exception{
        public InvalidInstrumentSettingValueException(string instrumentCategory, string vendor, string model, string slot, string channel, string settingKey, string value) : base($"{settingKey}={value}, {value} is invalid in [{instrumentCategory}] {vendor}-{model} slot={slot} channel={channel}!") { }
    }
}
