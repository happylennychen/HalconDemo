using System;

namespace Prober.WaferDef {
    [Serializable]
    public class SpecialPositionInfo {
        public string filePath { get; set; }
        public double ChuckX { get; set; }
        public double ChuckY { get; set; }
        public double ChuckZ { get; set; }
        public double ChuckSZ { get; set; }
        public double LeftX { get; set; }
        public double LeftY { get; set; }
        public double LeftZ { get; set; }
        public double LeftSX { get; set; }
        public double LeftSY { get; set; }
        public double LeftSZ { get; set; }
        public double RightX { get; set; }
        public double RightY { get; set; }
        public double RightZ { get; set; }
        public double RightSX { get; set; }
        public double RightSY { get; set; }
        public double RightSZ{ get; set; }
        public double CcdX { get; set; }
        public double CcdY { get; set; }
        public double CcdZ { get; set; }
        public double AltimeterU { get; set; }
    }
}
