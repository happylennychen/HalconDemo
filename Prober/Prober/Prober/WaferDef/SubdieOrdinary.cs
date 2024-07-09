using Prober.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prober.WaferDef
{
    public class SubdiePosCaliInfo
    {
        public string ReticleName { get; set; }
        public string SubDieName { get; set; } = string.Empty;
        public double ChuckX { get; set; }
        public double ChuckY { get; set; }
        public double LeftX { get; set; }
        public double LeftY { get; set; }
        public double RightX { get; set; }
        public double RightY { get; set; }
        public double Chuck_AxisX { get; set; }
        public double Chuck_AxisY { get; set; }
        public double Left_AxisX { get; set; }
        public double Left_AxisY { get; set; }
        public double Right_AxisX { get; set; }
        public double Right_AxisY { get; set; }        
    }    

    public class SubdieOrdinary
    {
        public string Subname { get; set; }
        public double ChuckX { get; set; }
        public double ChuckY { get; set; }
        public double LeftX { get; set; }
        public double LeftY { get; set; }
        public double RightX { get; set; }
        public double RightY { get; set; }        
    }

    public class ItemCalPosInfo
    {
        public DieInfo Die { get; set; }
        public string DieOrdinate { get; set; }
        public string SubDieName { get; set; }
        //chuck
        public double ChuckX { get; set; }
        public double ChuckY { get; set; }
        public double ChuckZ { get; set; }
        //左6轴
        public double LeftX { get; set; }
        public double LeftY { get; set; }
        //右6轴
        public double Right_X { get; set; }
        public double Right_Y { get; set; }
        //dieMark坐标
        public double DieMarkX2 { get; set; }
        public double DieMarkY2 { get; set; }
    }
}
