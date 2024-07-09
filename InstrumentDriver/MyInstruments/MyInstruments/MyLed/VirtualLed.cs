using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MyInstruments.MyLed
{
    public sealed class VirtualLed:StandaloneLed
    {
        public VirtualLed(string id) : base(id)
        {
            Vendor = "Virtual led vendor";
            SupportedModels = new HashSet<string> { "Virtual led model" };
        }

        public override void Connect(string visaResource)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualLed.Connect({visaResource})");
        }

        public override void Disconnect()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualLed.Disconnect()");
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel)
        {
            ;
        }

        public override bool SetValue(int data, byte pointNum)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualLed.SetValue({data},{pointNum})");
            return true;
        }
    }
}
