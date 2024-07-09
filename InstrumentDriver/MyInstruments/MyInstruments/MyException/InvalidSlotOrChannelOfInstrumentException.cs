using System;

namespace MyInstruments.MyException {
    public sealed class InvalidSlotOrChannelOfInstrumentException : Exception{
        public InvalidSlotOrChannelOfInstrumentException(string slot, string channel, string vendor, string model): base($"slot(={slot}) or channel(={channel}) is invalid of instrument(vendor={vendor},model={model})!") { }
    }
}
