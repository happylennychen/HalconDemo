using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;
using YTN_TOOL_File;

namespace YTN_CALIBRATION
{
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

        public double X1 { get; set; }
        public double Z1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Z2 { get; set; }
        public double Y2 { get; set; }
        public double X3 { get; set; }
        public double Z3 { get; set; }
        public double Y3 { get; set; }
        public double X4 { get; set; }
        public double Z4 { get; set; }
        public double Y4 { get; set; }

        private double[] equation;

        public bool Save()
        {
            bool b = FileHelper.SaveXml(_name + ".xml", this, this.GetType()); 
            string msg = FileHelper.ErrorMsg;
            return b;
        }

        public bool GenerateEquation()
        {
            double[] row1 = GetRowNum(X1, Y1, Z1);
            double[] row2 = GetRowNum(X2, Y2, Z2);
            double[] row3 = GetRowNum(X3, Y3, Z3);
            double[] row4 = GetRowNum(X4, Y4, Z4);

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
                this.X1 = info.X1;
                this.Y1 = info.Y1;
                this.Z1 = info.Z1;
                this.X2 = info.X2;
                this.Y2 = info.Y2;
                this.Z2 = info.Z2;
                this.X3 = info.X3;
                this.Y3 = info.Y3;
                this.Z3 = info.Z3;
                this.X4 = info.X4;
                this.Y4 = info.Y4;
                this.Z4 = info.Z4;
                return GenerateEquation();
            }
            return false;
        }
    }
}
