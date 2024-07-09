using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prober.Constant
{
    internal class ZoomTable
    {
        public static readonly Dictionary<string, double> zoomToValueDic = new Dictionary<string, double>
        {
            { "0.7",548},
            { "1.0",4778},
            { "1.5",7849},
            { "2.0",10498},
            { "2.5",13299},
            { "3.0",15358},
            { "3.5",16888},
            { "4.0",18228},
            { "4.5",19347}
        };
    }

    public enum SignalColor {
        Red,
        Yellow,
        Green
    }

    public class CompensateData {
        public double xData = double.NaN;
        public double yData = double.NaN;
        public  CompensateData(double xData, double yData) {
            this.xData = xData;
            this.yData = yData;
        }
    }
}
