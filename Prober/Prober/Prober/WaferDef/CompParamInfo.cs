using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prober.WaferDef
{
    [Serializable]
    public class CompParamInfo
    {
        public ushort coderReverse { get; set; }    //编码器是否翻转
        public ushort sevonLevel { get; set; }      //伺服使能电平
        public int limitPos { get; set; }        //负限位相对移动距离
        public int homeOffset { get; set; }      //回零偏移量

        public double distancePerPulse {  get; set; }   //脉冲移动距离
        public double minVel {  get; set; }        //最小速度
        public double maxVel { get; set; }          //最大速度

        public int posLimit { get; set; }        //正软限位
        public int negLimit { get; set; }        //负软限位

        public ushort numPos { get; set; }          //补偿段数
        public int posStart { get; set; }        //补偿起点脉冲数
        public int posDis { get; set; }          //补偿总长度对应的脉冲数
        public string compDataPath { get; set; }    //补偿文件路径
    }
}
