using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInstruments.MyVisaDriver
{
    public class ComConfig
    {
        public string BaudRate = "9600";
        public string DataBit = "8";
        public string StopBits = "1";
        public string ParityBit = "N";

        public ComConfig(string baudRate, string dataBit, string stopBit,string parityBit)
        {
            BaudRate = baudRate;
            DataBit = dataBit;  
            StopBits = stopBit;
            ParityBit = parityBit;
        }
    }
}
