using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInstruments.MyAltimeter
{
    public sealed class UnsupportedAltimeterException:Exception
    {
        public UnsupportedAltimeterException(string vendor, string model) : base($"Unsupported Altimeter instrument: vendor={vendor}, model={model}") { }
    }
}
