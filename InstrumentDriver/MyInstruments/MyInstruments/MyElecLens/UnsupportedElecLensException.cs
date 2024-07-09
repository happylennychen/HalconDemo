using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInstruments.MyElecLens
{
    public sealed class UnsupportedElecLensException:Exception
    {
        public UnsupportedElecLensException(string vendor, string model) : base($"Unsupported ElecLens instrument: vendor={vendor}, model={model}") { }
    }
}
