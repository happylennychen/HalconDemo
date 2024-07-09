using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTN_A_Alignment
{
    /// <summary>
    /// 扫描耦合，用于初耦合
    /// </summary>
    public class ScanAlignment
    {
        private static bool isStop = false;
        private static bool isFinished = true;
        internal static string errorMsg = string.Empty;

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <returns>错误信息</returns>
        public static string GetErrorMessage()
        {
            return errorMsg;
        }

        /// <summary>
        /// 停止运行
        /// </summary>
        public static void Stop()
        {
            isStop = true;
            while (!isFinished)
            {
            }
            isStop = false;
        }


        private static bool IsContinueRunning()
        {
            if (isStop)
            {
                errorMsg = "扫描耦合被停止！";
                return false;
            }
            return true;
        }


        /// <summary>
        /// 螺旋描耦合方法
        /// </summary>
        /// <param name="Info">耦合输入信息</param>
        /// <param name="result">耦合结果</param>
        /// <returns>是否成功</returns>
        public static bool SpiralScanAlignment(ScanAlignmentInfo Info, out ScanAlignmentResult result)
        {
            result = new ScanAlignmentResult();
            try
            {
                isFinished = false;
                if (Info.MoveDir1Func == null || Info.MoveDir2Func == null || Info.GetPowerFunc == null)
                {
                    errorMsg = "扫描耦合，注册信息不完整。";
                    return false;
                }
                int count = Info.Count();
                int dir = 1;
                double pw = 0;
                for (int i = 0; i < count + 1; i++)
                {
                    if (i == 0)
                    {
                        result.Dir1PosList.Add(0);
                        result.Dir2PosList.Add(0);
                        pw = Info.GetPowerFunc();
                        result.PowerList.Add(pw);
                        if (Info.IsThread && pw > Info.ThresholdPower)
                            return true;
                        continue;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        if (!IsContinueRunning())
                            return false;
                        if (!Info.MoveDir1Func(Info.Step * dir))
                        {
                            errorMsg = "扫描耦合，运动轴1失败001。";
                            return false;
                        }
                        result.Dir1PosList.Add(result.Dir1PosList.Last() + Info.Step * dir);
                        result.Dir2PosList.Add(result.Dir2PosList.Last());
                        pw = Info.GetPowerFunc();
                        result.PowerList.Add(pw);
                        if (Info.IsThread && pw > Info.ThresholdPower)
                            return true;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        if (!IsContinueRunning())
                            return false;
                        if (!Info.MoveDir2Func(Info.Step * dir))
                        {
                            errorMsg = "扫描耦合，运动轴2失败002。";
                            return false;
                        }
                        result.Dir1PosList.Add(result.Dir1PosList.Last());
                        result.Dir2PosList.Add(result.Dir2PosList.Last() + Info.Step * dir);
                        pw = Info.GetPowerFunc();
                        result.PowerList.Add(pw);
                        if (Info.IsThread && pw > Info.ThresholdPower)
                            return true;
                    }
                    dir = -dir;
                }
                double max = result.PowerList.Max();
                int index = result.PowerList.IndexOf(max);
                double pos1 = result.Dir1PosList[index];
                double pos2 = result.Dir2PosList[index];
                double dis1 = pos1 - (Info.Range + Info.Step);
                double dis2 = pos2 - (Info.Range + Info.Step);
                if (!Info.MoveDir1Func(dis1))
                {
                    errorMsg = "扫描耦合，运动轴1失败003。";
                    return false;
                }
                if (!Info.MoveDir2Func(dis2))
                {
                    errorMsg = "扫描耦合，运动轴2失败004。";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = "扫描耦合发生异常。" + ex.Message;
                return false;
            }
            finally
            {
                isFinished = true;
            }
        }


        /// <summary>
        /// 线扫描
        /// </summary>
        /// <param name="Info">耦合输入信息</param>
        /// <param name="result">耦合结果</param>
        /// <returns>是否成功</returns>
        public static bool LineScanAlignment(ScanAlignmentInfo Info, out ScanAlignmentResult result)
        {
            result = new ScanAlignmentResult();
            try
            {
                isFinished = false;
                if (Info.MoveDir1Func == null || Info.MoveDir2Func == null || Info.GetPowerFunc == null)
                {
                    errorMsg = "扫描耦合，注册信息不完整。";
                    return false;
                }
                int count = Info.Count();
                if (!Info.MoveDir1Func(-Info.Range - Info.Step))
                {
                    errorMsg = "扫描耦合，运动轴1失败001。";
                    return false;
                }
                if (!Info.MoveDir2Func(-Info.Range))
                {
                    errorMsg = "扫描耦合，运动轴2失败002。";
                    return false;
                }

                int dir = 1;
                for (int i = 0; i < count; i++)
                {
                    for (int j = 0; j < count; j++)
                    {
                        if (!IsContinueRunning())
                            return false;
                        if (!Info.MoveDir1Func(Info.Step * dir))
                        {
                            errorMsg = "扫描耦合，运动轴1失败003。";
                            return false;
                        }
                        result.Dir1PosList.Add((-Info.Range + j * Info.Step) * dir);
                        result.Dir2PosList.Add(-Info.Range + i * Info.Step);
                        double pw = Info.GetPowerFunc();
                        result.PowerList.Add(pw);
                        if (pw > Info.ThresholdPower)
                            return true;
                    }
                    if (!Info.MoveDir1Func(Info.Step * dir))
                    {
                        errorMsg = "扫描耦合，运动轴1失败004。";
                        return false;
                    }
                    if (!Info.MoveDir2Func(Info.Step))
                    {
                        errorMsg = "扫描耦合，运动轴2失败005。";
                        return false;
                    }
                    dir = -dir;
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = "扫描耦合发生异常。" + ex.Message;
                return false;
            }
            finally
            {
                isFinished = true;
            }
        }

    }

    /// <summary>
    /// 扫描耦合
    /// </summary>
    public class ScanAlignmentInfo
    {
        /// <summary>
        /// 步长
        /// </summary>
        public double Step { get; set; }


        /// <summary>
        /// 范围
        /// </summary>
        public double Range { get; set; }

        /// <summary>
        /// 是否有阈值判断
        /// </summary>
        public bool IsThread { get; set; }

        /// <summary>
        /// 退出阈值
        /// </summary>
        public double ThresholdPower { get; set; }

        /// <summary>
        /// 起始点的方向1绝对位置
        /// </summary>
        public double AbsDir1 { get; set; }


        /// <summary>
        /// 起始点的方向2绝对位置
        /// </summary>
        public double AbsDir2 { get; set; }


        /// <summary>
        /// 注册运动方向1
        /// </summary>
        public Func<double, bool> MoveDir1Func;


        /// <summary>
        /// 注册运动方向2
        /// </summary>
        public Func<double, bool> MoveDir2Func;


        /// <summary>
        /// 注册获取功率
        /// </summary>
        public Func<double> GetPowerFunc;


        /// <summary>
        /// 获取扫描点数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return (int)(2 * Range / Step + 1.1);
        }
    }


    /// <summary>
    /// 扫描耦合信息
    /// </summary>
    public class ScanAlignmentResult
    {
        /// <summary>
        /// 方向1位置集合
        /// </summary>
        public List<double> Dir1PosList { get; set; } = new List<double>();
        /// <summary>
        /// 方向2位置集合
        /// </summary>
        public List<double> Dir2PosList { get; set; } = new List<double>();
        /// <summary>
        /// 方向3位置集合
        /// </summary>
        public List<double> PowerList { get; set; } = new List<double>();

        /// <summary>
        /// 获取结果中的最大值信息
        /// </summary>
        /// <param name="maxPower">最大功率</param>
        /// <param name="maxDir1Pos">最大功率对应方向1位置</param>
        /// <param name="maxDire2Pos">最大功率对应方向2位置</param>
        /// <returns>是否异常</returns>
        public bool GetMaxPowerInfo(out double maxPower, out double maxDir1Pos, out double maxDire2Pos)
        {
            maxPower = 0;
            maxDir1Pos = 0;
            maxDire2Pos = 0;
            try
            {
                maxPower = PowerList.Max();
                int index = PowerList.IndexOf(maxPower);
                maxDir1Pos = Dir1PosList[index];
                maxDire2Pos = Dir2PosList[index];
                return true;
            }
            catch (Exception ex)
            {
                ScanAlignment.errorMsg = "扫描耦合获取最大功率信息异常！" + ex.Message;
                return false;
            }
        }
    }

}
