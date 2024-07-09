using MyInstruments.MyEnum;
using NationalInstruments.DAQmx;
using System.Collections.Generic;

namespace MyInstruments.MyAltimeter
{
    public sealed class AmNI600X:StandaloneAm {
        public AmNI600X(string id) : base(id) {
            Vendor = EnumInstrumentVendor.NI.ToString();
            SupportedModels = new HashSet<string> { "_6000","_6002" };
        }

        public override void Connect(string visaResource)   {
            this.addr = visaResource;
        }

        public override void Disconnect() {
            ;
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel) {
            ;
        }

        public override bool GetHeight(int port, out double height) {
            height = 0;

            if (port <= 0) {
                return false;   
            }
            
            lock (mutex)  {
                Task myTask = new Task();
                double vol = 0;

                string portAddr = string.Format("{0}/ai{1}", addr, (port - 1));
                myTask.AIChannels.CreateVoltageChannel(portAddr, "",  AITerminalConfiguration.Rse, -10, 10, AIVoltageUnits.Volts);

                //Create a virtual channel
                AnalogMultiChannelReader reader = new AnalogMultiChannelReader(myTask.Stream);

                double[] data = reader.ReadSingleSample();
                vol = data[0];  

                height = vol * 500 / 10;

                return true;
            }
        }

        public override bool EnterMeasureMode()  {
            return true;
        }

        public string addr = string.Empty;
        private readonly object mutex = new object();
    }
}
