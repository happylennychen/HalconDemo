using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prober.WaferDef
{
    public class EValue
    {
        public const string NOREADYINFO = "设备未准备好，请稍后再试！";

        public static string GetFileDate()
        {
            return DateTime.Now.ToString("yyyyMMdd"); //yyyyMMddHHmmss
        }
    }
}
