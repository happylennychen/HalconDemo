using System;

namespace MyInstruments.MyException {
    public sealed class UnsupportedInstrumentException : Exception {
        public UnsupportedInstrumentException(string vendor, string model) : base($"Unsupported instrument: vendor={vendor}, model={model}") {
        }
    }
}
