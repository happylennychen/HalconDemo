using HalconDotNet;
using MyInstruments.MyEnum;
using MyInstruments.MyVisaDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyInstruments.MyOs
{
    public sealed class OsJdsuSB:StandaloneOs
    {
        public OsJdsuSB(string id) : base(id)
        {
            Vendor = EnumInstrumentVendor.JDSU.ToString();
            SupportedModels = new HashSet<string> { "_SB" };
        }

        public override void Connect(string visaResource)
        {
            LOGGER.Debug($"call Jdsu.Connect({visaResource})");
            this.visaDriver = VisaDriverFactory.CreateInstance();
            this.visaDriver.Connect(visaResource);
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
        }

        public override void Disconnect()
        {
            LOGGER.Debug($"call Jdsu.Disconnect()");
            this.visaDriver?.Disconnect();
        }

        public override bool Reset()
        {
            string cmd = "RESET";
            visaDriver.WriteLine(cmd); 
            return true;
        }

        public override bool SwitchToChannel(int slot, int channel)
        {
            // CLOSE 1            
            string cmd = $"CLOSE {channel}";
            visaDriver.WriteLine(cmd);
            return true;
        }

        public override bool GetCurChannel(int slot, out int channel)
        {
            channel = 0;

            try
            {
                //CLOSE? 
                string cmd = $"CLOSE?";
                string ret = visaDriver.QueryLine(cmd);

                if (string.IsNullOrEmpty(ret))
                {
                    return false;
                }

                channel = Convert.ToInt32(ret);
                return true;
            }
            catch(Exception ex) 
            {
                LOGGER.Error($"Os GetCurChannel exception:{ex.Message}");
                return false;
            }            
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel)
        {
            ;
        }

        private IVisaDriver visaDriver;
    }
}
