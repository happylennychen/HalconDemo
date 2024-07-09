using MyInstruments.MyTec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInstruments.MyLed
{
    public abstract class StandaloneLed : Instrument, ILed
    {
        public StandaloneLed(string id) : base(id) { }

        public abstract bool SetValue(int data, byte pointNum);
    }
}
