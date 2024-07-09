using System;
using System.Text;
using System.Collections.Generic;
using MyInstruments.MyEnum;
using MyInstruments.MyVisaDriver;
using System.Threading;
using System.Linq;

namespace MyInstruments.MyAltimeter
{
    public sealed class AmKeyenceCL3000:StandaloneAm
    {
        public string visaAddress = string.Empty;

        public AmKeyenceCL3000(string id) : base(id)
        {
            Vendor = EnumInstrumentVendor.KEYENCE.ToString();
            SupportedModels = new HashSet<string> { "_CL3000" };
        }

        public override void Connect(string visaResource)
        {
            LOGGER.Debug($"call AmKeyenceCL3000.Connect({visaResource})");
            this.visaAddress = visaResource;
            this.visaDriver = VisaDriverFactory.CreateInstance();
            this.visaDriver.Connect(visaResource);
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            //增加结束符设置            

            this.visaDriver.SetTimeoutInS(30);
            this.visaDriver.SetTerminationCharacter(Convert.ToByte('\r'));
            this.visaDriver.EnableTerminationCharacter(true);            
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
        }

        public override void Disconnect()
        {
            LOGGER.Debug($"call AmKeyenceCL3000.Disconnect()");
            this.visaDriver?.Disconnect();
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel)
        {
            ;
        }

        public override bool GetHeight(int port,out double height) 
         {
            lock(mutex)
            {
                height = double.NaN;
                try 
                {                    
                    string cmd = "MS,0,1\r";

                    string ret = string.Empty;
                    //ret = visaDriver.QueryLine(cmd);
                    //System.Text.Encoding.UTF8.GetBytes(str)
                    visaDriver.WriteBytes(System.Text.Encoding.UTF8.GetBytes(cmd));
                    byte[] data = visaDriver.ReadBytes();
                    ret = System.Text.Encoding.UTF8.GetString(data);
                    string str = ret.Split(',')[1];

                    height = Convert.ToDouble(str) * 1000;
                    return true;
                }
                catch (Exception ex)
                {
                    LOGGER.Debug($"call Keyence Get Hegith Exception {ex.Message}，try connect");
                    Disconnect();
                    Thread.Sleep(200);
                    Connect(visaAddress);
                    return false;
                }                
            }           
        }

        public override bool EnterMeasureMode()
        {
            string cmd = "R0\r";

            string ret = visaDriver.QueryLine(cmd);
            if (string.IsNullOrEmpty(ret))
            {
                return false;
            }

            return true;
        }

        private IVisaDriver visaDriver;
        private readonly object mutex = new object();
    }
}
