using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTN_TOOL_File;

namespace YTN_CALIBRATION
{
    /// <summary>
    /// 角度校准
    /// </summary>
    public class AngleCalibrate
    {
        /// <summary>
        /// 角度校准配置文件名称
        /// </summary>
        public string ConfigName = string.Empty;
        /// <summary>
        /// 角度校准信息
        /// </summary>
        public AngleCalibrateInfo AngleCalInfo = null;


        private HTuple _motionConvert;
        private HTuple _angleConvert;

        private HTuple _centerX;
        private HTuple _centerY;

        //把第一个作为起始位置，第一个点在角度为0的时候的XY
        private HTuple _startX;
        private HTuple _startY;

        /// <summary>
        /// 角度运动委托
        /// </summary>
        public Func<double, bool> MoveAngle;
        /// <summary>
        /// X方向运动委托
        /// </summary>
        public Func<double, bool> MoveX;
        /// <summary>
        /// Y方向运动委托
        /// </summary>
        public Func<double, bool> MoveY;



        /// <summary>
        /// 根据配置文件名字构造一个
        /// </summary>
        /// <param name="name"></param>
        public AngleCalibrate(string name)
        {
            ConfigName = name;
            AngleCalInfo = AngleCalibrateInfo.Load(name);

            //生成角度补偿信息
            if (AngleCalInfo != null)
            {
                HOperatorSet.GenContourPolygonXld(out HObject contour, AngleCalInfo.PointX.ToArray(), AngleCalInfo.PointY.ToArray());
                HOperatorSet.FitCircleContourXld(contour, "geotukey", -1, 0, 0, 3, 2, out _centerX, out _centerY, out HTuple r, out HTuple start, out HTuple end, out HTuple point);
                //把第一个作为起始位置，第一个点在角度为0的时候
                _startX = AngleCalInfo.PointX[0];
                _startY = AngleCalInfo.PointY[0];
            }

        }


        /// <summary>
        /// 计算角度旋转后的补偿，通过枚举的方式
        /// </summary>
        /// <param name="startAngle">起始角度</param>
        /// <param name="endAngle">目标角度</param>
        /// <param name="deltaX"></param>
        /// <param name="deltaZ"></param>
        /// <returns></returns>
        public bool CalAngleCompensate(double startAngle, double endAngle, out double deltaX, out double deltaZ)
        {
            deltaX = 0;
            deltaZ = 0;

            if (Math.Abs(startAngle) > 5 || Math.Abs(endAngle) > 5)
            {
                return false; ;
            }

            try
            {
                if (AngleCalInfo.AngleList.Count < 1)
                {
                    return false;
                }
                var angle_Start_1 = AngleCalInfo.AngleList.FirstOrDefault(t => t.R > startAngle - 0.05);
                int index1 = AngleCalInfo.AngleList.IndexOf(angle_Start_1);
                var angle_Stop_1 = AngleCalInfo.AngleList[index1 + 1];
                if (!NumericalCal.LineInterpolation(new double[] { angle_Start_1.R, angle_Stop_1.R }, new double[] { angle_Start_1.X, angle_Stop_1.X }, startAngle, out double x1))
                    return false;
                if (!NumericalCal.LineInterpolation(new double[] { angle_Start_1.R, angle_Stop_1.R }, new double[] { angle_Start_1.Z, angle_Stop_1.Z }, startAngle, out double z1))
                    return false;


                var angle_Start_2 = AngleCalInfo.AngleList.FirstOrDefault(t => t.R > endAngle - 0.05);
                int index2 = AngleCalInfo.AngleList.IndexOf(angle_Start_2);
                if (index2 > AngleCalInfo.AngleList.Count - 2)
                {
                    return false;
                }
                var angle_Stop_2 = AngleCalInfo.AngleList[index2 + 1];
                if (!NumericalCal.LineInterpolation(new double[] { angle_Start_2.R, angle_Stop_2.R }, new double[] { angle_Start_2.X, angle_Stop_2.X }, endAngle, out double x2))
                    return false;
                if (!NumericalCal.LineInterpolation(new double[] { angle_Start_2.R, angle_Stop_2.R }, new double[] { angle_Start_2.Z, angle_Stop_2.Z }, endAngle, out double z2))
                    return false;


                deltaX = x1 - x2;
                deltaZ = z1 - z2;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        ///// <summary>
        ///// 计算角度旋转后的补偿 ,通过旋转中心的形式
        ///// </summary>
        ///// <param name="startAngle">起始角度</param>
        ///// <param name="endAngle">目标角度</param>
        ///// <param name="deltaX"></param>
        ///// <param name="deltaY"></param>
        ///// <returns></returns>
        //public bool CalAngleCompensate(double startAngle, double endAngle, out double deltaX, out double deltaY)
        //{
        //    deltaX = 0;
        //    deltaY = 0;

        //    HOperatorSet.HomMat2dIdentity(out HTuple hom2d);
        //    double delta = endAngle - startAngle;

        //    HOperatorSet.HomMat2dRotate(hom2d, new HTuple(startAngle).TupleRad(), _centerX, _centerY, out HTuple homR1);
        //    HOperatorSet.HomMat2dRotate(hom2d, new HTuple(endAngle).TupleRad(), _centerX, _centerY, out HTuple homR2);

        //    HOperatorSet.AffineTransPoint2d(homR1, _startX, _startY, out HTuple x1, out HTuple y1);
        //    HOperatorSet.AffineTransPoint2d(homR2, _startX, _startY, out HTuple x2, out HTuple y2);

        //    deltaX = x2 - x1;
        //    deltaY = y2 - y1;
        //    return true;
        //}


        /// <summary>
        /// 计算补偿并移动位置
        /// </summary>
        /// <param name="startAngle">当前角度</param>
        /// <param name="endAngle">目标角度</param>
        /// <returns></returns>
        public bool CalCompensateAndMove(double startAngle, double endAngle)
        {
            if (!CalAngleCompensate(startAngle, endAngle, out double deltaX, out double deltaY))
            {
                return false;
            }
            if (!MoveAngle(endAngle - startAngle))
            {
                return false;
            }
            if (!MoveX(deltaX))
            {
                return false;
            }
            if (!MoveY(deltaY))
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 图像识别处Fiber点的位置。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool GetFiberPos(out double x, out double y)
        {
            x = 0;
            y = 0;

            return true;
        }

    }

    /// <summary>
    /// 角度校准参数信息
    /// </summary>
    public class AngleCalibrateInfo
    {
        public double BaseAngle { get; set; }

        public List<AngleInfo> AngleList { get; set; } = new List<AngleInfo>();

        /// <summary>
        /// 存放FA或者fiber上的角度位置，最少3个
        /// </summary>
        public List<double> Angle { get; set; } = new List<double>();
        /// <summary>
        /// 存放FA或者fiber上的特征点的X坐标，最少3个，需要保存转换到世界坐标的
        /// </summary>
        public List<double> RowList { get; set; } = new List<double>();
        /// <summary>
        /// 存放FA或者fiber上的特征点的Y坐标，最少3个，需要保存转换到世界坐标的
        /// </summary>
        public List<double> ColList { get; set; } = new List<double>();
        /// <summary>
        /// 存放FA或者fiber上的特征点的X坐标，最少3个，需要保存转换到世界坐标的
        /// </summary>
        public List<double> PointX { get; set; } = new List<double>();
        /// <summary>
        /// 存放FA或者fiber上的特征点的Y坐标，最少3个，需要保存转换到世界坐标的
        /// </summary>
        public List<double> PointY { get; set; } = new List<double>();

        /// <summary>
        /// 平面校准信息
        /// </summary>
        public PlatInfo Plate { get; set; } = new PlatInfo(4);

        ///// <summary>
        ///// FA或者Fiber运动轴图像坐标Row
        ///// </summary>
        //public List<double> Px { get; set; } = new List<double>();
        ///// <summary>
        ///// FA或者Fiber运动轴图像坐标Column
        ///// </summary>
        //public List<double> Py { get; set; } = new List<double>();
        ///// <summary>
        ///// FA或者Fiber运动轴实际坐标X
        ///// </summary>
        //public List<double> Qx { get; set; } = new List<double>();
        ///// <summary>
        ///// FA或者Fiber运动轴实际坐标Y
        ///// </summary>
        //public List<double> Qy { get; set; } = new List<double>();

        /// <summary>
        /// 根据名字保存角度校准信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Save(string name)
        {
            return FileHelper.SaveXml($"config\\{name}.xml", this, this.GetType());
        }

        /// <summary>
        /// 根据名字加载角度校准信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static AngleCalibrateInfo Load(string name)
        {
            return FileHelper.LoadXml($"config\\Angle_{name}.xml", typeof(AngleCalibrateInfo)) as AngleCalibrateInfo;
        }

    }

    public class AngleInfo
    {
        public double R { get; set; }
        public double X { get; set; }
        public double Z { get; set; }
    }
}
