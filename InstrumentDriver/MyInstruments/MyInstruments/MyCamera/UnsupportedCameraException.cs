using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInstruments.MyCamera
{
    public sealed class UnsupportedCameraException:Exception
    {
        public UnsupportedCameraException(string vendor, string model) : base($"Unsupported Camera instrument: vendor={vendor}, model={model}") { }
    }
}
