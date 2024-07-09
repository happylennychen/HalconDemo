using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTN_CALIBRATION
{
    public class ILDataProcess
    {
        public static string ErrorMsg = string.Empty;

        /// <summary>
        /// 平滑曲线
        /// </summary>
        /// <param name="powers"></param>
        /// <param name="windowSize"></param>
        /// <param name="smoothedData"></param>
        /// <returns></returns>
        public static bool SmoothLine(double[] powers, int windowSize, out double[] smoothedData)
        {
            ErrorMsg = string.Empty;
            smoothedData = new double[powers.Length];
            try
            {
                for (int i = 0; i < powers.Length; i++)
                {
                    int start = Math.Max(0, i - windowSize + 1);
                    int end = i + 1;
                    end = Math.Min(powers.Length - 1, i + windowSize);
                    smoothedData[i] = powers.Skip(start).Take(end - start).Average();
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                return false;
            }
        }


        /// <summary>
        /// 多项式拟合
        /// </summary>
        /// <param name="wavs"></param>
        /// <param name="powers"></param>
        /// <param name="order"></param>
        /// <param name="fitData"></param>
        /// <returns></returns>
        public static bool FitPoly(double[] wavs, double[] powers, int order, out double[] fitData)
        {
            fitData = new double[wavs.Length];
            try
            {
                double[] res = Fit.Polynomial(wavs, powers, order);

                for (int i = 0; i < wavs.Length; i++)
                {
                    for (int j = 0; j < res.Length; j++)
                    {
                        fitData[i] += (Math.Pow(wavs[i], j) * res[j]);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                return false;
            }
        }


        public static bool CalculateSingleIL(double[] il, out double[] singleIL)
        {
            singleIL = new double[il.Length];
            try
            {
                singleIL = il.Select(t => t / 2).ToArray();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.ToString();
                return false;
            }
        }

        public static bool CalculateDeviceILByRef(double[] powerDevice, double[] powerRef, out double[] res)
        {
            res = new double[powerDevice.Length];
            if (powerDevice == null || powerRef == null)
            {
                ErrorMsg = "数据为NULL。";
                return false;
            }
            if (powerDevice.Length != powerRef.Length)
            {
                ErrorMsg = "数据长度不一致。";
                return false;
            }
            try
            {
                for (int i = 0; i < powerDevice.Length; i++)
                {
                    res[i] = powerDevice[i] - powerRef[i];
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.ToString();
                return false;
            }
        }

        public static bool CalculateDeviceILByRef(double[] powerDevice, double[] powerRef, int deviceCount, out double[] res)
        {
            res = new double[powerDevice.Length];
            if (powerDevice == null || powerRef == null)
            {
                ErrorMsg = "数据为NULL。";
                return false;
            }
            if (powerDevice.Length != powerRef.Length)
            {
                ErrorMsg = "数据长度不一致。";
                return false;
            }

            try
            {
                for (int i = 0; i < powerDevice.Length; i++)
                {
                    res[i] = (powerDevice[i] - powerRef[i]) / deviceCount;
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.ToString();
                return false;
            }
        }


        public static bool CalHeater(double[] curs, double[] powers, out HeaterCurveResult res)
        {
            res = new HeaterCurveResult();

            int gap = curs.Length / 10;
            List<double> pwList = new List<double>();

            for (int i = 0; i < powers.Length; i++)
            {
                int start = i * gap;
                int needCount = Math.Min(powers.Length - 1 - start, gap);
                if (start >= curs.Length - 1)
                    break;
                var selectedData = powers.Skip(start).Take(needCount);
                pwList.Add(selectedData.Max());
                pwList.Add(selectedData.Min());
            }
            pwList = pwList.OrderBy(t => t).ToList();
            double null_1 = pwList[0];
            double null_2 = pwList[1];

            int null1_index = powers.ToList().IndexOf(null_1);
            int null2_index = powers.ToList().IndexOf(null_2);

            if (null1_index < null2_index)
            {
                null_1 = pwList[1];
                null_2 = pwList[0];
                int temp = null1_index;
                null1_index = null2_index;
                null2_index = temp;
            }

            res.Null_1_power = null_1;
            res.Null_1_x = curs[null1_index];
            res.Null_2_Power = null_2;
            res.Null_2_x = curs[null2_index];

            double peak = pwList[pwList.Count - 1];
            int peak_index = powers.ToList().IndexOf(peak);
            if (peak_index < null1_index)
            {
                peak = pwList[pwList.Count - 2];
                peak_index = powers.ToList().IndexOf(peak);
            }

            res.Peak_Power = peak;
            res.Peak_x = curs[peak_index];

            res.ER_Power = res.Peak_Power - res.Null_1_power;
            res.ER_x = res.Peak_x - res.Null_1_x;

            res.FSR = res.Null_2_x - res.Null_1_x;

            //MathNet.Numerics.Differentiation.NumericalDerivative numerical = new MathNet.Numerics.Differentiation.NumericalDerivative();

            //numerical.EvaluateDerivative()


            return true;
        }


        /// <summary>
        /// Sin拟合
        /// </summary>
        /// <param name="xDatas"></param>
        /// <param name="yDatas"></param>
        /// <param name="fitDatas"></param>
        /// <returns></returns>
        public static bool FitSin(double[] xDatas, double[] yDatas, double initA, double initW, double initf, out double[] fitDatas)
        {
            fitDatas = new double[xDatas.Length];
            if (xDatas == null || yDatas == null)
            {
                ErrorMsg = "数据为NULL。";
                return false;
            }
            if (xDatas.Length != yDatas.Length)
            {
                ErrorMsg = "数据长度不一致。";
                return false;
            }
            try
            {
                Func<double, double, double, double, double> sinModel = (A, w, f, x) => A * Math.Sin(w * x + f);
                Func<double, double> resFunc = Fit.CurveFunc(xDatas, yDatas, sinModel, initA, initW, initf);

                for (int i = 0; i < xDatas.Length; i++)
                {
                    fitDatas[i] = resFunc(xDatas[i]);
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.ToString();
                return false;
            }
        }

        public static double[] CalMatriceResult(List<double[]> inputMatric, double[] outputMatric)
        {
            Matrix<double> designMatrix = Matrix<double>.Build.DenseOfRowArrays(inputMatric);
            Vector<double> outputVector = Vector<double>.Build.Dense(outputMatric);
            //Vector<double> solution = MultiDim(designMatrix, outputVector);
            var vec = designMatrix.QR().Solve(outputVector);
            return vec.ToArray();
        }
    }

    public class HeaterCurveResult
    {
        public double Null_1_power { get; set; }
        public double Null_2_Power { get; set; }
        public double Null_1_x { get; set; }
        public double Null_2_x { get; set; }

        public double Peak_Power { get; set; }
        public double Peak_x { get; set; }

        public double ER_Power { get; set; }
        public double ER_x { get; set; }

        public double FSR { get; set; }
    }

}
