using MathNet.Numerics;
using MathNet.Numerics.LinearRegression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using YTN_Logger;


namespace YTN_CALIBRATION
{
    public class NumericalCal
    {
        /// <summary>
        /// 直线拟合
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool LineFit(double[] x, double[] y, out double a, out double b)
        {
           
            //(0,10,50) (10,10,100
            //(5,0,100) (15,0,50) 

            //LineInterpolation()
            a = 0;
            b = 0;
            try
            {
                double[] res = Fit.Polynomial(x, y, 1);
                a = Math.Round(res[1], 6, MidpointRounding.ToEven);
                b = Math.Round(res[0], 6, MidpointRounding.ToEven);
                return true;
            }
            catch (Exception ex)
            {
                YTN_Log.Error(ex, "LineFit() error!");
                return false;
            }
        }


        /// <summary>
        /// 多项式拟合
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static double[] PolyFit(double[] x, double[] y, int order)
        {
            try
            {
                double[] res = Fit.Polynomial(x, y, order);
                return res;
            }
            catch (Exception ex)
            {
                YTN_Log.Error(ex, "PolyFit() error!");
                return null;
            }
        }

        /// <summary>
        /// 线性差值,x,y数组的长度为2
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <returns></returns>
        public static bool LineInterpolation(double[] x, double[] y, double x0, out double y0)
        {
            y0 = 0;
            try
            {
                var res = Interpolate.Linear(x, y);
                y0 = res.Interpolate(x0);
                return true;
            }
            catch (Exception ex)
            {
                YTN_Log.Error(ex, "LineInterpolation() error!");
                return false;
            }
        }

        /// <summary>
        /// 多项式差值，x,y数组的个数比方程次数大1
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <returns></returns>
        public static bool PolyInterpolation(double[] x, double[] y, double x0, out double y0)
        {
            y0 = 0;
            try
            {
                var res = Interpolate.Polynomial(x, y);
                y0 = res.Interpolate(x0);
                return true;
            }
            catch (Exception ex)
            {
                YTN_Log.Error(ex, "PolyInterpolation() error!");
                return false;
            }
        }


        /// <summary>
        /// 数值微分
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <returns></returns>
        public static bool NumDifferentiate(double[] x, double[] y, out double[] x0, out double[] y0)
        {
            x0 = new double[x.Length];
            y0 = new double[x.Length];
            try
            {
                for (int i = 1; i < x.Length - 1; i++)
                {
                    x0[i - 1] = x[i];
                    double b = (x[i + 1] - x[i - 1]);
                    if (b == 0)
                    {
                        YTN_Log.Warnning("NumDifferentiate(),被除数为0！");
                        return false;
                    }
                    y0[i - 1] = (y[i + 1] - y[i - 1]) / b;
                }
                return true;
            }
            catch (Exception ex)
            {
                YTN_Log.Error(ex, "NumDifferentiate() error!");
                return false;
            }
        }

        /// <summary>
        /// 求方程的跟，
        /// </summary>
        /// <param name="coefficients">方程的序数，按次数从小达到排列</param>
        /// <param name="root"></param>
        /// <returns></returns>
        public static bool CalRoots(double[] coefficients,out double[] root)
        {
            root = null;
            try
            {
                Complex[] plex = FindRoots.Polynomial(coefficients);
                root = new double[plex.Length];
                for (int i = 0; i < plex.Length; i++)
                {
                    root[i] = plex[i].Real;
                }
                return true;
            }
            catch (Exception ex)
            {
                YTN_Log.Error(ex, "CalRoots() error!");
                return false;
            }
        }
    }
}
