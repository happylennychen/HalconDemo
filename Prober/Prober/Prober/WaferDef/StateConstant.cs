using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prober.WaferDef{
    public enum State
    {
        Ready,
        Busy,
        Stop,
        Pause,
    }

    public enum SignalColor
    {
        RED,
        YELLOW,
        GREEN
    }
}
