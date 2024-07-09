using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prober.WaferDef
{
    [Serializable]
    public class PmTriggerSetting
    {
        public int PmRange1 = -10;
        public int PmRange2 = -10;
        public double PmNplc1 = 0.5;
        public double PmNplc2 = 0.5;
        public double ScanRange1 = 20;
        public double ScanRange2 = 20;
        public double ScanStep1 = 0.2;
        public double ScanStep2 = 0.2;
    }
}
