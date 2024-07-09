using System;
using System.Collections.Generic;
using System.Linq;

namespace Prober.WaferDef {
    [Serializable]
    public class HCalibrationInfo
    {
        public double ArearCircle { get; set; } = 95000;

        public double ReticleWidth { get; set; } 
        public double ReticleHeight { get; set; }

        public List<HeightScanBasicPosition> BasicPoints { get; set; } = new List<HeightScanBasicPosition>();

        public double HeightThreshold {  get; set; }    
        public double RowSpace { get; set; } = 5000;
        public double ColSpace { get; set; } = 5000;

        public double CenterX { get; set; }
        public double CenterY { get; set; }

        public List<RecordPointInfo> Points { get; set; } = new List<RecordPointInfo>();

        public List<FitLineInfo> Lines { get; set; } = new List<FitLineInfo>();


        public int GetRowCount()
        {
            return (int)(ArearCircle * 2 / RowSpace + 1.1);// (int)(-(RightTop.X - LeftTop.X) / RowSpace + 1.1);

        }

        public int GetColCount()
        {
            return (int)(ArearCircle * 2 / ColSpace + 1.1);
        }

        public bool GetDisMinPoint(double x, double y, out double h, bool useFixPoint = true)
        {
            //在标记点中查找与目标点距离最近的点的高度作为该点的高度
            h = 0;
            double minDis = 500000;
            double maxDis = 0;
            double tempDis = 0;
            RecordPointInfo recordPointInfo = null;

            foreach (RecordPointInfo info in Points)
            {
                tempDis = Math.Sqrt((info.X - x) * (info.X - x) + (info.Y - y) * (info.Y - y));
                if (tempDis <= minDis)
                {
                    minDis = tempDis;
                    recordPointInfo = info;
                }

                if (tempDis >= maxDis)
                {
                    maxDis = tempDis;
                }
            }

            h = recordPointInfo.Z;
            return true;
        }

        public bool GetHeight(double x, double y, out double h,bool useFixPoint = true)
        {
            h = 0;
            if (Points.Count == 0)
                return false;
            int rowTemp = GetRowCount() % 2 == 0 ? GetRowCount() / 2 : GetRowCount() / 2 + 1;
            int colTemp = GetColCount() % 2 == 0 ? GetColCount() / 2 : GetColCount() / 2 + 1;

            if (useFixPoint) {
                rowTemp = GetRowCountByReticle() % 2 == 0 ? GetRowCountByReticle() / 2 : GetRowCountByReticle() / 2 + 1;
                colTemp = GetColCountByReticle() % 2 == 0 ? GetColCountByReticle() / 2 : GetColCountByReticle() / 2 + 1;
            }
            
            var rows = Points.Where(t => t.Row == rowTemp).ToArray();
            var cols = Points.Where(t => t.Column == colTemp).ToArray();
            int leftRowIndex = 0;
            int upColIndex = 0;
            
            if (x > rows[0].X)
            {
                upColIndex = rows[0].Column - 1;
            }
            else if (x < rows[rows.Length - 1].X)
            {
                upColIndex = rows[rows.Length - 1].Column + 1;
            }
            else
            {
                for (int i = 0; i < rows.Length - 1; i++)
                {
                    if (rows[i].X > x && rows[i + 1].X <= x)
                    {
                        upColIndex = rows[i].Column;
                        break;
                    }
                }
            }

            if (y < cols[0].Y)
            {
                leftRowIndex = cols[0].Row - 1;
            }
            else if (y > cols[cols.Length - 1].Y)
            {
                leftRowIndex = cols[cols.Length - 1].Row + 1;
            }
            else
            {
                for (int i = 0; i < cols.Length - 1; i++)
                {
                    if (cols[i].Y <= y && cols[i + 1].Y > y)
                    {
                        leftRowIndex = cols[i].Row;
                        break;
                    }
                }
            }
           
            DLInterporationInfo info = new DLInterporationInfo();
#if false
            //新的计算方式
            info.LeftTop = Points.FirstOrDefault(t => t.Row == leftRowIndex && t.Column == upColIndex);
            info.LeftDown = Points.FirstOrDefault(t => t.Row == leftRowIndex + 1 && t.Column == upColIndex);
            info.RightTop = Points.FirstOrDefault(t => t.Row == leftRowIndex && t.Column == upColIndex + 1);
            info.RightDown = Points.FirstOrDefault(t => t.Row == leftRowIndex + 1 && t.Column == upColIndex + 1);
            List<double> hList = new List<double>(); ///...

            if (info.LeftTop!=null)    
            {
                hList.Add(info.LeftTop.Z);
            }
            if (info.LeftDown != null)
            {
                hList.Add(info.LeftDown.Z);
            }
            if (info.RightTop!=null)
            {
                hList.Add(info.RightTop.Z);
            }
            if (info.RightDown!=null)
            {
                hList.Add(info.RightDown.Z);
            }
            if (hList.Count == 0)
                return false;
            if (hList.Count < 4)
            {
                h = hList.Average();
                return true;
            }
#endif
            #region 旧的计算方式
            int index = 0;
            while (info.LeftTop == null || info.LeftDown == null || info.RightTop == null || info.RightDown == null) {
                if (index > 10) {
                    return GetDisMinPoint(x, y, out h, useFixPoint);
                }
                info.LeftTop = Points.FirstOrDefault(t => t.Row == leftRowIndex && t.Column == upColIndex);
                info.LeftDown = Points.FirstOrDefault(t => t.Row == leftRowIndex + 1 && t.Column == upColIndex);
                info.RightTop = Points.FirstOrDefault(t => t.Row == leftRowIndex && t.Column == upColIndex + 1);
                info.RightDown = Points.FirstOrDefault(t => t.Row == leftRowIndex + 1 && t.Column == upColIndex + 1);

                if (info.RightDown == null && info.RightTop == null && info.LeftDown == null) {
                    upColIndex--;
                    leftRowIndex--;
                } else if (info.RightDown == null && info.LeftTop == null && info.LeftDown == null) {
                    leftRowIndex--;
                    upColIndex++;
                } else if (info.RightDown == null && info.RightTop == null && info.LeftDown == null) {
                    leftRowIndex++;
                    upColIndex++;
                } else if (info.RightDown == null && info.LeftTop == null && info.LeftDown == null) {
                    leftRowIndex++;
                    upColIndex--;
                } else if (info.LeftTop == null && info.RightTop == null) {
                    leftRowIndex++;
                } else if (info.LeftDown == null && info.RightDown == null) {
                    leftRowIndex--;
                } else if (info.LeftDown == null && info.LeftTop == null) {
                    upColIndex++;
                } else if (info.RightDown == null && info.RightTop == null) {
                    upColIndex--;
                } else if (info.LeftTop == null || info.LeftDown == null) {
                    upColIndex++;
                } else if (info.RightTop == null || info.RightDown == null) {
                    upColIndex--;
                }
                index++;
            }
            #endregion

            return DoubleLIneInterpolation.GetHeight(info, x, y, out h);
        }       

        public int GetRowCountByReticle()
        {
            return (int)(ArearCircle * 2 / ReticleHeight + 1.1);
        }

        public int GetColCountByReticle()
        {
            return (int)(ArearCircle * 2 / ReticleWidth + 1.1);
        }

        public bool GeneratePointInfoWithBasicPos()
        {
            int rowCount = GetRowCountByReticle();
            int colCount = GetColCountByReticle();
            double startX0 = 0;
            double startY0 = 0;
            double startWaferX0 = 0;
            double startWaferY0 = 0;

            startX0 = BasicPoints[0].X + ReticleWidth * (colCount / 2);
            startY0 = BasicPoints[0].Y - ReticleHeight * (rowCount / 2);
            startWaferX0 = -ReticleWidth * (colCount / 2);
            startWaferY0 = ReticleHeight * (rowCount / 2);

            Points.Clear();
            for (int j = 0; j < rowCount; j++)
            {
                for (int i = 0; i < colCount; i++)
                {
                    for (int k = 0; k < BasicPoints.Count;k++)
                    {
                        RecordPointInfo info = new RecordPointInfo();
                        info.X = startX0 - ReticleWidth * i + BasicPoints[k].X - BasicPoints[0].X;
                        info.Y = startY0 + ReticleHeight * j + BasicPoints[k].Y - BasicPoints[0].Y;
                        info.WaferX = startWaferX0 + ReticleWidth * i - BasicPoints[k].X + BasicPoints[0].X; 
                        info.WaferY = startWaferY0 - ReticleHeight * j - BasicPoints[k].Y + BasicPoints[0].Y;

                        info.Row = j + 1;
                        info.Column = i + 1;
                        info.Order = k + 1;

                        double r = Math.Sqrt((info.X - CenterX) * (info.X - CenterX) + (info.Y - CenterY) * (info.Y - CenterY));
                        if (r > ArearCircle)
                            continue;
                        Points.Add(info);
                    }
                }
            }
            
            return true;
        }


        public bool GeneratePointInfo()
        {
            int rowCount = GetRowCount();
            int colCount = GetColCount();
            double startX0 = 0;
            double startY0 = 0;
            double startWaferX0 = 0;
            double startWaferY0 = 0;

            startX0 = CenterX + RowSpace * (colCount / 2);
            startY0 = CenterY - ColSpace * (rowCount / 2);

            startWaferX0 = -RowSpace * (colCount / 2);
            startWaferY0 = ColSpace * (rowCount / 2);

            if (colCount % 2 == 1)
            {
                startX0 += RowSpace / 2;
                startWaferX0 -= RowSpace / 2;
            }
            if (rowCount % 2 == 1)
            {
                startY0 -= ColSpace / 2;
                startWaferY0 += ColSpace / 2;
            }

            Points.Clear();
            for (int j = 0; j < rowCount; j++)
            {
                for (int i = 0; i < colCount; i++)
                {
                    RecordPointInfo info = new RecordPointInfo();
                    info.X = startX0 - RowSpace * i;
                    info.Y = startY0 + ColSpace * j;
                    info.WaferX = startWaferX0 + RowSpace * i;
                    info.WaferY = startWaferY0 - ColSpace * j;
                    info.Row = j + 1;
                    info.Column = i + 1;
                    double r = Math.Sqrt((info.X - CenterX) * (info.X - CenterX) + (info.Y - CenterY) * (info.Y - CenterY));
                    if (r > ArearCircle)
                        continue;
                    Points.Add(info);
                }
            }
            return true;
        }

        public void GenerateLines()
        {
            int rowCount = GetRowCount();
            int colCount = GetColCount();

            Lines.Clear();
            for (int i = 0; i < rowCount; i++)
            {
                var ps = Points.Where(t => t.Row == i + 1);
                if (ps == null || ps.Count() < 1)
                    continue;
                if (i==25)
                {

                }
                double[] x = ps.Select(t => t.X).ToArray();
                double[] y = ps.Select(t => t.Z).ToArray();
                NumericalCal.LineFit(x, y, out double a, out double b);
                foreach (var p in ps)
                {
                    p.Z_Row = a * p.X + b;
                }
                FitLineInfo line = new FitLineInfo();
                line.Row = i + 1;
                line.A = a;
                line.B = b;
                Lines.Add(line);
            }

            for (int i = 0; i < colCount; i++)
            {
                var ps = Points.Where(t => t.Column == i + 1);
                if (ps == null || ps.Count() < 1)
                    continue;
                double[] x = ps.Select(t => t.Y).ToArray();
                double[] y = ps.Select(t => t.Z).ToArray();
                NumericalCal.LineFit(x, y, out double a, out double b);
                foreach (var p in ps)
                {
                    p.Z_Col = a * p.Y + b;
                }
                FitLineInfo line = new FitLineInfo();
                line.Column = i + 1;
                line.A = a;
                line.B = b;
                Lines.Add(line);
            }
        }

        public void GenerateLinesEx() {
            int rowCount = GetRowCountByReticle();
            int colCount = GetColCountByReticle();

            Lines.Clear();
            for (int i = 0; i < rowCount; i++) {
                var ps = Points.Where(t => t.Row == i + 1);
                if (ps == null || ps.Count() < 1)
                    continue;
                if (i == 25) {

                }
                double[] x = ps.Select(t => t.X).ToArray();
                double[] y = ps.Select(t => t.Z).ToArray();
                if (x.Count() <= 1) {
                    continue;
                }
                NumericalCal.LineFit(x, y, out double a, out double b);
                foreach (var p in ps) {
                    p.Z_Row = a * p.X + b;
                }
                FitLineInfo line = new FitLineInfo();
                line.Row = i + 1;
                line.A = a;
                line.B = b;
                Lines.Add(line);
            }

            for (int i = 0; i < colCount; i++) {
                var ps = Points.Where(t => t.Column == i + 1);
                if (ps == null || ps.Count() < 1)
                    continue;
                double[] x = ps.Select(t => t.Y).ToArray();
                double[] y = ps.Select(t => t.Z).ToArray();
                if (x.Count() <= 1) {
                    continue;
                }
                NumericalCal.LineFit(x, y, out double a, out double b);
                foreach (var p in ps) {
                    p.Z_Col = a * p.Y + b;
                }
                FitLineInfo line = new FitLineInfo();
                line.Column = i + 1;
                line.A = a;
                line.B = b;
                Lines.Add(line);
            }
        }
    }
}
