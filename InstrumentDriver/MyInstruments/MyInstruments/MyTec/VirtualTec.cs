using System;
using System.Collections.Generic;
using System.Threading;

namespace MyInstruments.MyTec
{
    public sealed class VirtualTec:StandaloneTec
    {
        public  VirtualTec(string id):base(id)
        {
            Vendor = "Virtual tec vendor";
            SupportedModels = new HashSet<string> { "Virutal tec model" };
        }

        public override void Connect(string visaResource)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualTec.Connect({visaResource})");
        }

        public override void Disconnect()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualTec.Disconnect()");
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel)
        {
            ;
        }

        public override bool SetTemp(int port, double temperature)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualTec.SetTemp({port},{temperature})");
            return true;
        }

        public override bool GetTempSet(int port, out double temperature)
        {
            Random random = new Random();
            double value = random.NextDouble();
            temperature = value * 0.1;
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualTec.GetTempSet({port},{temperature})");
            return true;
        }

        public override bool GetTemp(int port, out double temperature)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualTec.GetTemp({port})");

            Random random = new Random();
            double value = random.NextDouble();
            temperature = value * 0.001;

            return true;
        }

        public override bool GetTempAll(out List<double> tempList) {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualTec.GetTempAll");

            tempList = new List<double>();  
            Random random = new Random();
            double value = random.NextDouble();
            tempList.Add(value * 0.001);

            return true;
        }

        public override bool SetTecEnable(int port, bool isEnable)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualTec.SetTecEnable()");

            return true;
        }

        public override bool GetTecEnable(int port, out bool isEnable)
        {
            isEnable = true;
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualTec.GetTecEnable()");

            return true;
        }

        public override bool EnableWrite()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualTec.EnableWrite()");

            return true;
        }
    }
}
