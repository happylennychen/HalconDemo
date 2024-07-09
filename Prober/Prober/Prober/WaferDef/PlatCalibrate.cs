using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.IO;

namespace Prober.WaferDef {
    public class PlatCalibrate
    {
        private string _fileName;
        public HTuple _curMatrix;
        public HTuple _mtwMartrix;

        public PlatCalibrate(string name)
        {
            _fileName = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\" + name;
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
            HOperatorSet.VectorToHomMat2d(plat.Px, plat.Py, plat.Qx, plat.Qy, out _curMatrix);
            return true;
        }

        /// <summary>
        /// 晶圆->运动坐标系转换矩阵
        /// </summary>
        /// <param name="plat"></param>
        /// <returns></returns>
        public bool GenerateMotionToWaferCalibration(PlatInfo plat)
        {
            HOperatorSet.VectorToHomMat2d(plat.Qx, plat.Qy, plat.Px, plat.Py, out _mtwMartrix);
            return true;
        }

        public bool PlatPointConvert(double px, double py, out double qx, out double qy)
        {
            qx = 0;
            qy = 0;
 
            if (_curMatrix == null)
            {
                //YTN_Log.Warnning("PlatPointConvert() Error!转换矩阵不能为空！");
                return false;
            }
            HOperatorSet.AffineTransPoint2d(_curMatrix, px, py, out HTuple qxTemp, out HTuple qyTemp);
            qx = qxTemp.D;
            qy = qyTemp.D;
            return true;
        }

        /// <summary>
        /// 运动坐标转换为晶圆坐标
        /// </summary>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <param name="qx"></param>
        /// <param name="qy"></param>
        /// <returns></returns>
        public bool WaferPointConvert(double px, double py, out double qx, out double qy)
        {
            qx = 0;
            qy = 0;

            if (_mtwMartrix == null)
            {
                //YTN_Log.Warnning("PlatPointConvert() Error!转换矩阵不能为空！");
                return false;
            }
            HOperatorSet.AffineTransPoint2d(_mtwMartrix, px, py, out HTuple qxTemp, out HTuple qyTemp);
            qx = qxTemp.D;
            qy = qyTemp.D;
            return true;
        }

        /// <summary>
        /// 读取校准矩阵
        /// </summary>
        /// <returns>是否成功</returns>
        public bool ReadCalibration()
        {
            bool bRet = false;
            HTuple itemHandle = new HTuple();
            HTuple fileHandle = new HTuple();
            try
            {
                HOperatorSet.OpenFile(_fileName + ".mat", "output_binary", out fileHandle);
                HOperatorSet.FreadSerializedItem(fileHandle, out itemHandle);
                HOperatorSet.DeserializeHomMat2d(itemHandle, out _curMatrix);
                bRet = true;
            }
            catch (Exception ex)
            {
                //YTN_Log.Error(ex, "ReadCalibration() Error!");
                itemHandle.Dispose();
                HOperatorSet.CloseFile(fileHandle);
                throw (ex);                
            }

            return bRet;
        }

        /// <summary>
        /// 写校准矩阵
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="matrix">矩阵</param>
        /// <returns></returns>
        public bool WriteCalibration(string file)
        {
            bool bRet = false;
            if (_curMatrix == null)
            {
                //YTN_Log.Warnning("WriteCalibration() Error!转换矩阵不能为空！");
                return false;
            }
            HTuple itemHandle = new HTuple();
            HTuple fileHandle = new HTuple();

            try
            {
                HOperatorSet.SerializeHomMat2d(_fileName + ".mat", out itemHandle);
                HOperatorSet.OpenFile(file, "output_binary", out fileHandle);
                HOperatorSet.FwriteSerializedItem(fileHandle, itemHandle);
                bRet = true;
            }
            catch (Exception ex)
            {
                //YTN_Log.Error(ex, "ReadCalibration() Error!");
                itemHandle.Dispose();
                HOperatorSet.CloseFile(fileHandle);
                throw (ex);
            }

            return bRet;
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
