using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prober.WaferDef {
    public class IOInfo
    {
        public int CardIndex { get; set; } = 0;
        public int SizeAll { get; set; } = 0;
        public int Chip { get; set; } = 1;
        public int Size2 { get; set; } = 2;
        public int Size4 { get; set; } = 3;
        public int Size6 { get; set; } = 4;
        public int Size8 { get; set; } = 5;
        public int Size12 { get; set; } = 6;
        public int RfPlat { get; set; } = 7;
        public int PdPlat {  get; set; } = 8;   
                  
        public int Red { get; set; } = 10;        
        public int Yellow { get; set; } = 11;
        public int Green { get; set; } = 12;        
        public int Light { get; set; } = 13;

        public int VaccumInput { get; set; } = 3;
        public int PressureInput1 { get; set; } = 4;
        public int PressureInput2 { get; set; } = 5;
    }
}
