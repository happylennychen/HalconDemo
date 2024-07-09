using System.Collections.Generic;

namespace MyInstruments.MyTec
{
    public interface ITec
    {
        /// <summary>
        /// 设置某个端口的TEC目标温度
        /// </summary>
        /// <param name="port"></param>
        /// <param name="temperature"></param>
        bool SetTemp(int port, double temperature);

        /// <summary>
        /// 查询某个端口的TEC温度
        /// </summary>
        /// <param name="port"></param>
        /// <returns>某个端口的温度</returns>
        bool GetTemp(int port, out double temperature);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="temperature"></param>
        /// <returns></returns>
        bool GetTempSet(int port, out double temperature);

        /// <summary>
        /// 获取所有通道温度
        /// </summary>
        /// <param name="tempList"></param>
        /// <returns></returns>
        bool GetTempAll(out List<double> tempList);

        /// <summary>
        /// 设置TEC使能
        /// </summary>
        /// <param name="port"></param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        bool SetTecEnable(int port, bool isEnable);

        /// <summary>
        /// 获取TEC状态
        /// </summary>
        /// <param name="port"></param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        bool GetTecEnable(int port, out bool isEnable); 

        /// <summary>
        /// 欧姆龙写使能
        /// </summary>
        bool EnableWrite();

    }
}
