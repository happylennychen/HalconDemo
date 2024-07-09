using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using YTN_Logger;

namespace YTN_CALIBRATION
{
    public class PlatCalibrate
    {
        private string _fileName;
        public HTuple _curMatrix;

        public PlatCalibrate()
        {

        }

        public PlatCalibrate(string name)
        {
            _fileName = AppDomain.CurrentDomain.BaseDirectory + "config\\" + name;
            if (File.Exists(_fileName))
                ReadCalibration();
        }

        /// <summary>
        /// 创建平面校准矩阵
        /// </summary>
        /// <param name="plat">坐标信息</param>
        /// <returns></returns>
        public bool GeneratePlatCalibration(PlatInfo plat)
        {
            try
            {
                HOperatorSet.VectorToHomMat2d(plat.Px, plat.Py, plat.Qx, plat.Qy, out _curMatrix);
                return true;
            }
            catch (Exception ex)
            {
                YTN_Log.Error(ex, "GeneratePlatCalibration() Error!");
                return false;
            }
        }

        public bool PlatPointConvert(double px, double py, out double qx, out double qy)
        {
            qx = 0;
            qy = 0;
            try
            {
                if (_curMatrix == null)
                {
                    YTN_Log.Warnning("PlatPointConvert() Error!转换矩阵不能为空！");
                    return false;
                }
                HOperatorSet.AffineTransPoint2d(_curMatrix, px, py, out HTuple qxTemp, out HTuple qyTemp);
                qx = qxTemp.D;
                qy = qyTemp.D;
                return true;
            }
            catch (Exception ex)
            {
                YTN_Log.Error(ex, "PlatPointConvert() Error!");
                return false;
            }
        }

        /// <summary>
        /// 读取校准矩阵
        /// </summary>
        /// <returns>是否成功</returns>
        public bool ReadCalibration()
        {
            HTuple itemHandle = new HTuple();
            HTuple fileHandle = new HTuple();
            try
            {
                HOperatorSet.OpenFile(_fileName + ".mat", "output_binary", out fileHandle);
                HOperatorSet.FreadSerializedItem(fileHandle, out itemHandle);
                HOperatorSet.DeserializeHomMat2d(itemHandle, out _curMatrix);
                return true;
            }
            catch (Exception ex)
            {
                YTN_Log.Error(ex, "ReadCalibration() Error!");
                return false;
            }
            finally
            {
                itemHandle.Dispose();
                HOperatorSet.CloseFile(fileHandle);
            }
        }

        /// <summary>
        /// 写校准矩阵
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="matrix">矩阵</param>
        /// <returns></returns>
        public bool WriteCalibration(string file)
        {
            if (_curMatrix == null)
            {
                YTN_Log.Warnning("WriteCalibration() Error!转换矩阵不能为空！");
                return false;
            }
            HTuple itemHandle = new HTuple();
            HTuple fileHandle = new HTuple();

            try
            {
                HOperatorSet.SerializeHomMat2d(_fileName + ".mat", out itemHandle);
                HOperatorSet.OpenFile(file, "output_binary", out fileHandle);
                HOperatorSet.FwriteSerializedItem(fileHandle, itemHandle);
                return true;
            }
            catch (Exception ex)
            {
                YTN_Log.Error(ex, "ReadCalibration() Error!");
                return false;
            }
            finally
            {
                itemHandle.Dispose();
                HOperatorSet.CloseFile(fileHandle);
            }
        }
    }

    /// <summary>
    /// 平面坐标信息
    /// </summary>
    public class PlatInfo
    {
        public PlatInfo()
        {

        }

        public PlatInfo(int count)
        {
            Px = new double[count];
            Py = new double[count];
            Qx = new double[count];
            Qy = new double[count];
        }

        /// <summary>
        /// 图像x坐标数组
        /// </summary>
        public double[] Px { get; set; }
        /// <summary>
        /// 图像y坐标数组
        /// </summary>
        public double[] Py { get; set; }
        /// <summary>
        /// 滑台x坐标数组
        /// </summary>
        public double[] Qx { get; set; }
        /// <summary>
        /// 滑台y坐标数组
        /// </summary>
        public double[] Qy { get; set; }
    }
}
