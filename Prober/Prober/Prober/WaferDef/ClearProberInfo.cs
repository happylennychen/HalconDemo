using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prober.WaferDef
{
    public class ClearProberInfo
    {
        public int RowGap { get; set; } = 50;
        public int ColumnGap { get; set; } = 100;
        public double Depth { get; set; } = 10;
        public int Times { get; set; } = 1;
        public int RowCount { get; set; } = 1;
        public int ColumnCount { get; set; } = 50;

        public double ChuckX { get; set; }
        public double ChuckY { get; set; }
        public double ChuckZ { get; set; }
        public double Exp { get; set; }
        public double CcdX { get; set; }
        public double CcdY { get; set; }
        public double CcdZ { get; set; }
        public double Zoom { get; set; }

        public List<ClearPaperPos> PaperPosList { get; set; } = new List<ClearPaperPos>();

        /// <summary>
        /// 重置清针纸状态
        /// </summary>
        public void InitClearPaper()
        {
            PaperPosList.Clear();
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    ClearPaperPos pos = new ClearPaperPos();
                    pos.RowIndex = i;
                    pos.ColumnIndex = j;
                    pos.IsUsed = false;
                    PaperPosList.Add(pos);
                }
            }
        }
    }

    public class ClearPaperPos
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public bool IsUsed { get; set; }
    }
}
