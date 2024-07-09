using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;

namespace Prober.WaferDef {
    [Serializable]
    public class HeightCalibrate
    {
        private string _name;

        public HeightCalibrate()
        {

        }

        public HeightCalibrate(string name)
        {
            _name = name;
            Load();
        }

        public double Left_X { get; set; }
        public double Left_Z { get; set; }
        public double Left_Y { get; set; }
        public double Chuck_X { get; set; }
        public double Chuck_Z { get; set; }
        public double Chuck_Y { get; set; }
        public double Right_X { get; set; }
        public double Right_Z { get; set; }
        public double Right_Y { get; set; }
        public double Ccd_X { get; set; }
        public double Ccd_Z { get; set; }
        public double Ccd_Y { get; set; }

        private double[] equation;

        public bool Save()
        {
            bool b = FileHelper.SaveXml(_name + ".xml", this, this.GetType()); 
            string msg = FileHelper.ErrorMsg;
            return b;
        }

        public bool GenerateEquation()
        {
            double[] row1 = GetRowNum(Left_X, Left_Y, Left_Z);
            double[] row2 = GetRowNum(Chuck_X, Chuck_Y, Chuck_Z);
            double[] row3 = GetRowNum(Right_X, Right_Y, Right_Z);
            double[] row4 = GetRowNum(Ccd_X, Ccd_Y, Ccd_Z);

            DenseMatrix A = DenseMatrix.OfArray(new double[,]
            {
                {  row1[1] , row1[2] , row1[3] , row1[4] },
                {  row2[1] , row2[2] , row2[3] , row2[4] },
                {  row3[1] , row3[2] , row3[3] , row3[4] },
                {  row4[1] , row4[2] , row4[3] , row4[4] }
            });
            DenseVector B = new DenseVector(new double[] { row1[0], row2[0], row3[0], row4[0] });
            Vector<double> result = A.LU().Solve(B);
            equation = result.ToArray();
            return true;
        }

        public double GetHeight(double x, double y)
        {
            double a = 1;
            double b = equation[2];
            double c = x * x + y * y + equation[0] * x + equation[1] * y + equation[3];
            double delta = Math.Sqrt(b * b - 4 * a * c);
            double res1 = Math.Round((-b + delta) / 2,2);
            double res2 = Math.Round((-b - delta) / 2,2);
            double res = 0;
            if (res1 > 0 && res2 > 0)
            {
                res = Math.Min(res1, res2);
            }
            else
            {
                res = Math.Max(res1, res2);
            }
            return res;
        }

        private double[] GetRowNum(double x, double y, double z)
        {
            double[] res = new double[5];
            res[0] = -x * x - y * y - z * z;
            res[1] = x;
            res[2] = y;
            res[3] = z;
            res[4] = 1;
            return res;
        }

        public bool Load()
        {
            var info = FileHelper.LoadXml(_name + ".xml", this.GetType()) as HeightCalibrate;

            if (info != null)
            {
                this.Left_X = info.Left_X;
                this.Left_Y = info.Left_Y;
                this.Left_Z = info.Left_Z;
                this.Chuck_X = info.Chuck_X;
                this.Chuck_Y = info.Chuck_Y;
                this.Chuck_Z = info.Chuck_Z;
                this.Right_X = info.Right_X;
                this.Right_Y = info.Right_Y;
                this.Right_Z = info.Right_Z;
                this.Ccd_X = info.Ccd_X;
                this.Ccd_Y = info.Ccd_Y;
                this.Ccd_Z = info.Ccd_Z;
                return GenerateEquation();
            }
            return false;
        }
    }
}
