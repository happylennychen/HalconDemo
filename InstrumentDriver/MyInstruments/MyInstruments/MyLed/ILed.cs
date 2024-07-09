using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInstruments.MyLed
{
    public interface ILed
    {
    /// <summary>
    /// 
    /// 
    /// </summary>
    /// <param name="data">整数</param>
    /// <param name="pointNum">小数点个数</param>
    /// <returns></returns>
        bool SetValue(int data, byte pointNum);
    }
}
