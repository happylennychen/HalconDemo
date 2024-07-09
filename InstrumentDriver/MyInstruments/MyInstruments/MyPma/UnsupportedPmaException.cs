using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInstruments.MyPma {
    public sealed class UnsupportedPmaException : Exception {
        public UnsupportedPmaException(string vendor, string model) : base($"Unsupported PMA instrument: vendor={vendor}, model={model}") { }
    }
}
