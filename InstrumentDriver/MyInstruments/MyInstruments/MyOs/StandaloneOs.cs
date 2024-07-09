using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInstruments.MyOs
{
    public abstract class StandaloneOs:Instrument,IOs
    {
        protected StandaloneOs(string id) : base(id) { }

        public abstract bool Reset();
        public abstract bool SwitchToChannel(int slot, int channel);    
        public abstract bool GetCurChannel(int slot, out int channel);

    }
}
