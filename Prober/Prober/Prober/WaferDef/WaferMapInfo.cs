using Prober.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace Prober.WaferDef {
    [Serializable]
    public class WaferMapInfo
    {
        public int IsRotation { get; set; }
        public bool isUserCapAltimeter { get; set; } = false;
        public bool isUserCapAltimeterLeft { get; set; } = true;   

        /***************晶圆基本信息**********************/
        public string Type { get; set; } = string.Empty;
        //Die信息
        public int CtlRows { get; set; }
        public int CtlCols { get; set; }
        public double DieWidth { get; set; }
        public double DieHeight { get; set; }
        public int DieRows { get; set; }
        public int DieColumns { get; set; }
        public int DieCount { get => Dies.Count; }

        //Mark信息
        public string MarkDieName
        {
            get
            {
                if(NameType == 1) {
                    string dieOrdName = DieOrdinaryToName(MarkDieColumnIndex - HomeDieColIndex, MarkDieRowIndex - HomeDieRowIndex);
                    var die = Dies.FirstOrDefault(t => t.OrdName == dieOrdName);
                    if (die != null) {
                        return dieOrdName;
                    }
                } else {
                    var die = Dies.FirstOrDefault(t => t.RowIndex == MarkDieRowIndex && t.ColumnIndex == MarkDieColumnIndex);
                    if (die != null) {
                        return $"({die.X},{die.Y})";
                    }
                }
               
                return "NA";
            }
        }

        /***************Mark点相关**********************/
        public int MarkDieRowIndex { get; set; }
        public int MarkDieColumnIndex { get; set; }
        public double MarkRow { get; set; }
        public double MarkColumn { get; set; }
        public double MarkChuckX { get; set; }
        public double MarkChuckY { get; set; }
        public double MarkCcdX { get; set; }
        public double MarkCcdY { get; set; }
        public double MarkCcdZ { get; set; }
        public double MarkDeltaChuckX { get; set; }
        public double MarkDeltaChuckY { get; set; }
        public double MarkExposure { get; set; }

        public double MarkZoom { get; set; }

        /***************预留**********************/
        public bool isUseMarkPad { get; set; } = false;
        public bool isOcrFirstReticleOnly { get; set; } = false;
        public double MarkPadRow { get; set; } = double.NaN;
        public double MarkPadColumn { get; set; } = double.NaN;
        public double MarkPadChuckX { get; set; } = double.NaN;
        public double MarkPadChuckY { get; set; } = double.NaN;

        public double MarkPadRow_2 { get; set; } = double.NaN;
        public double MarkPadColumn_2 { get; set; } = double.NaN;
        public double MarkPadChuckX_2 { get; set; } = double.NaN;
        public double MarkPadChuckY_2 { get; set; } = double.NaN;

        /*
        //subDie的标定信息
        public string RefSubDieDieName
        {
            get
            {
                var die = Dies.FirstOrDefault(t => t.RowIndex == RefSubDieRowindex && t.ColumnIndex == RefSubDieColumnIndex);
                if (die != null)
                {
                    return $"({die.X},{die.Y})";
                }
                return "NA";
            }
        }
        public int RefSubDieRowindex { get; set; }
        public int RefSubDieColumnIndex { get; set; }
        public double RefSubDieX { get; set; }
        public double RefSubDieY { get; set; }
        public double RefSubDieChuckX2 { get; set; }
        public double RefSubDieChuckY2 { get; set; }

        public double RefSubDieChuckZ2 { get; set; }

        public double RefSubDieFAX1 { get; set; }
        public double RefSubDieFAY1 { get; set; }
        public double RefSubDieFAZ1 { get; set; }
        public string RefSubDieName { get; set; } = string.Empty;
        */

        /***************4点标定相关**********************/
        public PlatInfo WaferMotionPlat { get; set; }

        //4个校准wafer的Die坐标
        public string DieOrdinaryToName(int x, int y) {

            char a = (char)('A' + x);
            string b = (y + 1).ToString("00");

            string dieName = a + b;
            return dieName;

        }
        public string RefDie1Name
        {
            get
            {
                if (NameType == 1) {
                    string dieOrdName = DieOrdinaryToName(RefDieColumnIndex1 - HomeDieColIndex, RefDieRowIndex1 - HomeDieRowIndex);
                    var die = Dies.FirstOrDefault(t => t.OrdName == dieOrdName);
                    if (die != null) {
                        return dieOrdName;
                    }
                }
                else {
                    var die = Dies.FirstOrDefault(t => t.RowIndex == RefDieRowIndex1 && t.ColumnIndex == RefDieColumnIndex1);
                    if (die != null) {
                        return $"({die.X},{die.Y})";
                    }
                }
                
                return "NA";
            }
        }
        public string RefDie2Name
        {
            get
            {
                if (NameType == 1) {
                    string dieOrdName = DieOrdinaryToName(RefDieColumnIndex2 - HomeDieColIndex, RefDieRowIndex2 - HomeDieRowIndex);
                    var die = Dies.FirstOrDefault(t => t.OrdName == dieOrdName);
                    if (die != null) {
                        return dieOrdName;
                    }
                } else {
                    var die = Dies.FirstOrDefault(t => t.RowIndex == RefDieRowIndex2 && t.ColumnIndex == RefDieColumnIndex2);
                    if (die != null) {
                        return $"({die.X},{die.Y})";
                    }
                }
                return "NA";
            }
        }
        public string RefDie3Name
        {
            get
            {
                if (NameType == 1) {
                    string dieOrdName = DieOrdinaryToName(RefDieColumnIndex3 - HomeDieColIndex, RefDieRowIndex3 - HomeDieRowIndex);
                    var die = Dies.FirstOrDefault(t => t.OrdName == dieOrdName);
                    if (die != null) {
                        return dieOrdName;
                    }
                } else {
                    var die = Dies.FirstOrDefault(t => t.RowIndex == RefDieRowIndex3 && t.ColumnIndex == RefDieColumnIndex3);
                    if (die != null) {
                        return $"({die.X},{die.Y})";
                    }
                }
                return "NA";
            }
        }
        public string RefDie4Name
        {
            get
            {
                if (NameType == 1) {
                    string dieOrdName = DieOrdinaryToName(RefDieColumnIndex4 - HomeDieColIndex, RefDieRowIndex4 - HomeDieRowIndex);
                    var die = Dies.FirstOrDefault(t => t.OrdName == dieOrdName);
                    if (die != null) {
                        return dieOrdName;
                    }
                } else {
                    var die = Dies.FirstOrDefault(t => t.RowIndex == RefDieRowIndex4 && t.ColumnIndex == RefDieColumnIndex4);
                    if (die != null) {
                        return $"({die.X},{die.Y})";
                    }
                }
                return "NA";
            }
        }

        public bool IsRefDieSelecteValid()
        {
            return RefDie1Name!=RefDie2Name && RefDie1Name!=RefDie2Name && RefDie1Name != RefDie3Name && RefDie1Name != RefDie4Name && RefDie2Name != RefDie3Name && RefDie2Name != RefDie4Name && RefDie3Name != RefDie4Name;
        }
        public int RefDieRowIndex1 { get; set; }
        public int RefDieRowIndex2 { get; set; }
        public int RefDieRowIndex3 { get; set; }
        public int RefDieRowIndex4 { get; set; }
        public int RefDieColumnIndex1 { get; set; }
        public int RefDieColumnIndex2 { get; set; }
        public int RefDieColumnIndex3 { get; set; }
        public int RefDieColumnIndex4 { get; set; }

        //Wafer高度信息
        public double Height_LeftZ {  get; set; }   
        public double Height_ChuckX_Left {  get; set; }
        public double Height_ChuckY_Left { get; set; }  
        public double Height_RightZ {  get; set; }  
        public double Height_ChuckX_Right {  get; set; }
        public double Height_ChuckY_Right { get; set; }

        public double Height_LeftCap {  get; set; } 
        public double Height_RightCap {  get; set; }


        public List<DieInfo> Dies { get; set; } = new List<DieInfo>();

        public string HomeDieName { get; set; } = string.Empty;
        public int HomeDieRowIndex { get; set; }
        public int HomeDieColIndex { get; set; }
        public List<SubDiePositionInfo> SubDies { get; set; } = new List<SubDiePositionInfo>();

        public int GetDieX(DieInfo die)
        {
            return die.ColumnIndex + 1;
        }

        public int GetDieY(DieInfo die)
        {
            return Dies.Count - die.RowIndex;
        }

        public DieInfo GetDieByName(string name)
        {
            try
            {
                if (NameType == 1) {
                    return Dies.FirstOrDefault(t => t.OrdName == name);
                }
                else {                    
                    string[] split = name.Split('(', ',', ')');
                    int x = Convert.ToInt32(split[1]);
                    int y = Convert.ToInt32(split[2]);
                    return Dies.FirstOrDefault(t => t.X == x && t.Y == y);
                }                
            }
            catch(Exception)
            {
                return null;
            }
        }

        public int NameType { set; get; } = 1;
    }
}
