using System;

namespace Prober.WaferDef {
    [Serializable]
    public class DLInterporationInfo
    {
        public RecordPointInfo LeftTop { get; set; } 
        public RecordPointInfo RightTop { get; set; } 
        public RecordPointInfo LeftDown { get; set; } 
        public RecordPointInfo RightDown { get; set; } 
    }

    [Serializable]
    public class FitLineInfo
    {
        public int Row { get; set; } = -1;
        public int Column { get; set; } = -1;
        public double A { get; set; }
        public double B { get; set; }
    }

    [Serializable]
    public class RecordPointInfo
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double Z_Row { get; set; }
        public double Z_Col { get; set; }
        public double Z_Fianl { get => (Z_Row + Z_Col) / 2; }

        public double WaferX { get; set; }
        public double WaferY { get; set; }

        public int Order { get;set; }
    }
}
