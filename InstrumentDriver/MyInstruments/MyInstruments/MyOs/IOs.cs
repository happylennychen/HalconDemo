using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInstruments.MyOs
{
    public interface IOs
    {
        bool SwitchToChannel(int slot,int port);
        bool Reset();  
        bool GetCurChannel(int slot, out int port);   
    }
}
