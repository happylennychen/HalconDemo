using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInstruments.MyOs
{
    public sealed class UnsupportedOsException : Exception
    {
        public UnsupportedOsException(string vendor, string model) : base($"Unsupported Os instrument: vendor={vendor}, model={model}") { }
    }
}
