using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prober.WaferDef
{
    [Serializable]
    public class ChuckHeightVerifyInfo
    {
        public double ChuckX_1 { get; set; } = double.NaN;
        public double ChuckY_1 { get; set; } = double.NaN;
        public double Height_1 { get; set; } = double.NaN;

        public double ChuckX_2 { get; set; } = double.NaN;
        public double ChuckY_2 { get; set; } = double.NaN;
        public double Height_2 { get; set; } = double.NaN;

        public double ChuckX_3 { get; set; } = double.NaN;
        public double ChuckY_3 { get; set; } = double.NaN;
        public double Height_3 { get; set; } = double.NaN;

        public double ChuckX_4 { get; set; } = double.NaN;
        public double ChuckY_4 { get; set; } = double.NaN;
        public double Height_4 { get; set; } = double.NaN;

        public double ChuckX_5 { get; set; } = double.NaN;
        public double ChuckY_5 { get; set; } = double.NaN;
        public double Height_5 { get; set; } = double.NaN;

        public double ThresholdBetween { get; set; } = 25;
        public double ThresholdInner { get; set; } = 10;

        public string WaferType { get; set; } = string.Empty;
    }
}
