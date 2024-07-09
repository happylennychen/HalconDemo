using MyInstruments.MyVisaDriver;
using System.Collections.Generic;

namespace MyInstruments.MyTec
{
    public abstract class StandaloneTec : Instrument, ITec
    {
        public StandaloneTec(string id):base(id) { }

        /// <summary>
        /// 设置某个端口的TEC目标温度
        /// </summary>
        /// <param name="port"></param>
        /// <param name="temperature"></param>
        public abstract bool SetTemp(int port, double temperature);

        /// <summary>
        /// 查询某个端口的TEC温度
        /// </summary>
        /// <param name="port"></param>
        /// <returns>某个端口的温度</returns>
        public abstract bool GetTemp(int port, out double temperature);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tempList"></param>
        /// <returns></returns>
        public abstract bool GetTempAll(out List<double> tempList);

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="temperature"></param>
        /// <returns></returns>
        public abstract bool GetTempSet(int port, out double temperature);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        public abstract bool SetTecEnable(int port, bool isEnable);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        public abstract bool GetTecEnable(int port, out bool isEnable);

        /// <summary>
        /// 欧姆龙写使能
        /// </summary>
        public abstract bool  EnableWrite();
    }
}
