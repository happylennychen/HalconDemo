using System;
using System.Collections.Generic;
using System.Threading;

namespace MyInstruments.MyElecLens
{
    public sealed class VirtualElecLens:StandaloneElecLens
    {
        public VirtualElecLens(string id):base(id)
        {
            Vendor = "Virtual elecLens vendor";
            SupportedModels = new HashSet<string> { "Virtual elecLens model" };
        }

        public override void Connect(string visaResource)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualElecLens.Connect({visaResource})");
        }

        public override void Disconnect()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualElecLens.Disconnect()");
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel)
        {
            ;
        }

        public override bool Home()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualElecLens.Home()");
            return true;
        }

        public override bool SetZoom(double value)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualElecLens.Move({value})");
            return true;
        }

        public override bool GetZoom(out double value)
        {
            value = 2;
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualElecLens.GetZoom({value})");
            return true;
        }

        public override bool IsMoveComplete()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info("call VirtualElecLens.IsMoveComplete()");
            return true;
        }
    }
}
