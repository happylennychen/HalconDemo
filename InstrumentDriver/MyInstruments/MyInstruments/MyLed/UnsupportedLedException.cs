using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInstruments.MyLed
{
    internal class UnsupportedLedException : Exception
    {
        public UnsupportedLedException(string vendor, string model) : base($"Unsupported LED instrument: vendor={vendor}, model={model}") { }
    }
}
