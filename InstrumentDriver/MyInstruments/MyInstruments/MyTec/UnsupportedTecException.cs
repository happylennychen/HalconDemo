using System;

namespace MyInstruments.MyTec
{
    public sealed class UnsupportedTecException:Exception
    {
        public UnsupportedTecException(string vendor, string model) : base($"Unsupported TEC instrument: vendor={vendor}, model={model}") { }
    }
}
