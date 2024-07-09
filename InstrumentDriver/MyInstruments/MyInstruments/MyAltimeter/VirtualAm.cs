using System;
using System.Collections.Generic;
using System.Threading;

namespace MyInstruments.MyAltimeter
{
    public sealed class VirtualAm:StandaloneAm
    {
        public VirtualAm(string id) : base(id)
        {
            Vendor = "Virtual am vendor";
            SupportedModels = new HashSet<string> { "Virtual am model" };
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel)
        {
            //[gyh]: to be implemented!
            //throw new NotImplementedException();
        }

        public override void Connect(string visaResource)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualAm.Connect({visaResource})");
        }

        public override void Disconnect()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualAm.Disconnect()");
        }

        public override bool GetHeight(int port, out double height)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualAs.GetHeight()");

            Random random = new Random();
            double value = random.NextDouble();
            height = value * 0.001;

            return true;
        }

        public override bool EnterMeasureMode()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info("call VirtualAs.EnterMeasureMode()");
            return true;
        }
    }
}
