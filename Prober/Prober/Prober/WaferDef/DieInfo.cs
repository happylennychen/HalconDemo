using System;

namespace Prober.WaferDef {
    [Serializable]
    public class DieInfo
    {
        /// <summary>
        /// 芯片的名称 例如1# 2# 按照测试流程自动排序
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 芯片在Wafer上的X坐标
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// 芯片在Wafer上的Y坐标
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// 芯片在wafer控件上的行坐标
        /// </summary>
        public int RowIndex { get; set; }
        /// <summary>
        /// 芯片在wafer控件上的列坐标
        /// </summary>
        public int ColumnIndex { get; set; }

        //完整晶圆与否
        public bool isFullDie { get; set; }

        //Die名称
        public string OrdName { get; set; }
    }
}
