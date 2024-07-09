using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyInstruments.MyOs
{
    public sealed class VirtualOs:StandaloneOs
    {
        public VirtualOs(string id) : base(id)
        {
            Vendor = "Virtual Os vendor";
            SupportedModels = new HashSet<string> { "Virtual Os model" };
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel)
        {
            //[gyh]: to be implemented!
            //throw new NotImplementedException();
        }

        public override void Connect(string visaResource)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualOs.Connect({visaResource})");
        }

        public override void Disconnect()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualOs.Disconnect()");
        }

        public override bool Reset()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualOs.Reset()");
            return true;
        }

        public override bool SwitchToChannel(int slot, int channel)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualOs.SwitchToChannel({slot},{channel})");
            return true;
        }

        public override bool GetCurChannel(int slot, out int channel)
        {
            channel = 1;
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualOs.GetCurChannel({slot},{channel})");
            return true;
        }
    }
}
