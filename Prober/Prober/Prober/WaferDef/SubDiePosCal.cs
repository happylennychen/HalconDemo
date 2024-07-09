using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prober.WaferDef
{
    public partial class SubDiePosCal
    {        
        public PlatCalibrate WaferMotionPlat { get; set; } = new PlatCalibrate("WaferMotion");
        public HCalibrationInfo HCalibrationInfo = new HCalibrationInfo();
        private string waferType;

        public SubDiePosCal(string waferType) 
        {
            this.waferType = waferType;   
        }

        public List<ItemCalPosInfo> CalTestItemsPositionOnStage(WaferMapInfo MapInfo, List<bool[]> DieTestStatus, List<SubdieOrdinary> TestItems, SubdiePosCaliInfo itemCalibrate, EquipmentCalibrationInfo equipmentInfo, out string subError)
        {
            subError = string.Empty;
            if (TestItems.FirstOrDefault(t => t.Subname == itemCalibrate.SubDieName) == null)
            {
                subError = "测试项中不包含基准测试项，请先校准基准项";
                return null;
            }

            List<ItemCalPosInfo> posList = new List<ItemCalPosInfo>();
            //对选择的die进行排序
            List<DieInfo> sortedDies = new List<DieInfo>();
            SortDies(ref sortedDies, MapInfo);

            var calibrateDie = MapInfo.GetDieByName(MapInfo.MarkDieName);
            //单边无探针
            if (itemCalibrate.BaseIndex == 1)
            {
                posList = CalSingleSideWithoutProbeDiePos(MapInfo, DieTestStatus, equipmentInfo, sortedDies, calibrateDie, itemCalibrate, TestItems, out subError);
            }
            //单边有探针
            else if (itemCalibrate.BaseIndex == 2)
            {
                posList = CalSingleSideWithProbeDiePos(MapInfo, DieTestStatus, equipmentInfo, sortedDies, calibrateDie, itemCalibrate, TestItems, out subError);
            }
            //双边无探针
            else if (itemCalibrate.BaseIndex == 3)
            {
                posList = CalDoubleSideWithoutProbeDiePos(MapInfo, DieTestStatus, equipmentInfo, sortedDies, calibrateDie, itemCalibrate, TestItems, out subError);
            }
            //双边有探针
            else
            {
                posList = CalDoubleSideWithProbeDiePos(MapInfo, DieTestStatus, equipmentInfo, sortedDies, calibrateDie, itemCalibrate, TestItems, out subError);
            }

            return posList;
        }

        //单边无针
        private List<ItemCalPosInfo> CalSingleSideWithoutProbeDiePos(WaferMapInfo MapInfo, List<bool[]> DieTestStatus, EquipmentCalibrationInfo EquipmentInfo, List<DieInfo> sortedDies, DieInfo calibrateDie, SubdiePosCaliInfo itemCalibrate, List<SubdieOrdinary> TestItems, out string subError)
        {
            subError = string.Empty;
            List<ItemCalPosInfo> posList = new List<ItemCalPosInfo>();
            int dieIndex = 0;
            int SubdieIndex = 0;

            //获得mark点和第一个波导的位置差
            double markDeltaX = itemCalibrate.Chuck_AxisX - MapInfo.MarkChuckX;
            double markDeltaY = itemCalibrate.Chuck_AxisY - MapInfo.MarkChuckY;

            HCalibrationInfo.GetHeight(EquipmentInfo.Base_ChuckX, EquipmentInfo.Base_ChuckY, out double FAZ0);

            foreach (var die in sortedDies)
            {
                SubdieIndex = 0;
                string ordinate = die.OrdName;
                double waferX0 = (die.ColumnIndex - calibrateDie.ColumnIndex) * MapInfo.DieWidth;
                double waferY0 = -(die.RowIndex - calibrateDie.RowIndex) * MapInfo.DieHeight;

                if (!WaferMotionPlat.PlatPointConvert(waferX0, waferY0, out double dieMarkX2, out double DieMarkY2))
                {
                    subError = "计算测试项坐标->坐标转换失败。";
                    return null;
                }

                foreach (var subDie in TestItems)
                {
                    if (!DieTestStatus[dieIndex][SubdieIndex])
                    {
                        SubdieIndex++;
                        continue;
                    }

                    ItemCalPosInfo posInfo = new ItemCalPosInfo();
                    posInfo.DieOrdinate = ordinate;
                    posInfo.SubDieName = subDie.Subname;
                    posInfo.Die = die;

                    double waferX1 = subDie.LeftX - (itemCalibrate.LeftX) + waferX0;
                    double waferY1 = subDie.LeftY - (itemCalibrate.LeftY) + waferY0;
                    if (!WaferMotionPlat.PlatPointConvert(waferX1, waferY1, out double chuckX, out double chuckY))
                    {
                        subError = "计算测试项坐标->坐标转换失败。";
                        return null;
                    }

                    chuckX += markDeltaX;
                    chuckY += markDeltaY;

                    double dis = Math.Sqrt((chuckX - HCalibrationInfo.CenterX) * (chuckX - HCalibrationInfo.CenterX) + (chuckY - HCalibrationInfo.CenterY) * (chuckY - HCalibrationInfo.CenterY));
                    if (dis > HCalibrationInfo.ArearCircle)
                    {
                        SubdieIndex++;
                        continue;
                    }

                    if (!HCalibrationInfo.GetHeight(chuckX, chuckY, out double FAZ1))
                        return null; //测高仪器测量的距离Z方向和定义方向相反
                                     //FA补偿高度差异  FAZ1=10   FAZ0=5  说明FAZ1的点比FAZ0高 .EquipmentInfo.ChuckZ2

                    posInfo.ChuckZ = EquipmentInfo.ChuckZ + FAZ0 - FAZ1;  //托盘动 Fa动FAZ1 - FAZ0
                    posInfo.ChuckX = chuckX;
                    posInfo.ChuckY = chuckY;
                    posInfo.LeftX = itemCalibrate.Left_AxisX;
                    posInfo.LeftY = itemCalibrate.Left_AxisY;

                    posInfo.DieMarkX2 = dieMarkX2;
                    posInfo.DieMarkY2 = DieMarkY2;

                    posList.Add(posInfo);
                    SubdieIndex++;
                }

                dieIndex++;
            }

            return posList;
        }

        //双边无探针
        private List<ItemCalPosInfo> CalDoubleSideWithoutProbeDiePos(WaferMapInfo MapInfo, List<bool[]> DieTestStatus, EquipmentCalibrationInfo EquipmentInfo, List<DieInfo> sortedDies, DieInfo calibrateDie, SubdiePosCaliInfo itemCalibrate, List<SubdieOrdinary> TestItems, out string subError)
        {
            subError = string.Empty;
            List<ItemCalPosInfo> posList = new List<ItemCalPosInfo>();
            int dieIndex = 0;
            int SubdieIndex = 0;

            //获得mark点和第一个波导的位置差
            double markDeltaX = itemCalibrate.Chuck_AxisX - MapInfo.MarkChuckX;
            double markDeltaY = itemCalibrate.Chuck_AxisY - MapInfo.MarkChuckY;

            HCalibrationInfo.GetHeight(EquipmentInfo.Base_ChuckX, EquipmentInfo.Base_ChuckY, out double FAZ0);
            double deltaWGX = itemCalibrate.RightX - itemCalibrate.LeftX;
            double deltaWGY = itemCalibrate.RightY - itemCalibrate.LeftY;

            foreach (var die in sortedDies)
            {
                SubdieIndex = 0;
                string ordinate = $"({die.X},{die.Y})";
                double waferX0 = (die.ColumnIndex - calibrateDie.ColumnIndex) * MapInfo.DieWidth;
                double waferY0 = -(die.RowIndex - calibrateDie.RowIndex) * MapInfo.DieHeight;

                if (!WaferMotionPlat.PlatPointConvert(waferX0, waferY0, out double dieMarkX2, out double DieMarkY2))
                {
                    subError = "计算测试项坐标->坐标转换失败。";
                    return null;
                }

                foreach (var subDie in TestItems)
                {
                    if (!DieTestStatus[dieIndex][SubdieIndex])
                    {
                        SubdieIndex++;
                        continue;
                    }

                    ItemCalPosInfo posInfo = new ItemCalPosInfo();
                    posInfo.DieOrdinate = ordinate;
                    posInfo.SubDieName = subDie.Subname;
                    posInfo.Die = die;

                    double waferX1 = subDie.LeftX - (itemCalibrate.LeftX) + waferX0;
                    double waferY1 = subDie.LeftY - (itemCalibrate.LeftY) + waferY0;
                    if (!WaferMotionPlat.PlatPointConvert(waferX1, waferY1, out double chuckX, out double chuckY))
                    {
                        subError = "计算测试项坐标->坐标转换失败。";
                        return null;
                    }

                    chuckX += markDeltaX;
                    chuckY += markDeltaY;

                    double dis = Math.Sqrt((chuckX - HCalibrationInfo.CenterX) * (chuckX - HCalibrationInfo.CenterX) + (chuckY - HCalibrationInfo.CenterY) * (chuckY - HCalibrationInfo.CenterY));
                    if (dis > HCalibrationInfo.ArearCircle)
                    {
                        SubdieIndex++;
                        continue;
                    }

                    if (!HCalibrationInfo.GetHeight(chuckX, chuckY, out double FAZ1))
                        return null; //测高仪器测量的距离Z方向和定义方向相反
                                     //FA补偿高度差异  FAZ1=10   FAZ0=5  说明FAZ1的点比FAZ0高 .EquipmentInfo.ChuckZ2

                    posInfo.ChuckZ = EquipmentInfo.ChuckZ + FAZ0 - FAZ1;  //托盘动 Fa动FAZ1 - FAZ0
                    posInfo.ChuckX = chuckX;
                    posInfo.ChuckY = chuckY;
                    posInfo.LeftX = itemCalibrate.Left_AxisX;
                    posInfo.LeftY = itemCalibrate.Left_AxisY;
                    posInfo.Right_X = itemCalibrate.Right_AxisX + subDie.RightX - subDie.LeftX - deltaWGX;
                    posInfo.Right_Y = itemCalibrate.Right_AxisY + subDie.RightY - subDie.LeftY - deltaWGY;

                    posInfo.DieMarkX2 = dieMarkX2;
                    posInfo.DieMarkY2 = DieMarkY2;

                    posList.Add(posInfo);
                    SubdieIndex++;
                }

                dieIndex++;
            }

            return posList;
        }

        //单边有探针
        private List<ItemCalPosInfo> CalSingleSideWithProbeDiePos(WaferMapInfo MapInfo, List<bool[]> DieTestStatus, EquipmentCalibrationInfo EquipmentInfo, List<DieInfo> sortedDies, DieInfo calibrateDie, SubdiePosCaliInfo itemCalibrate, List<SubdieOrdinary> TestItems, out string subError)
        {
            subError = string.Empty;
            List<ItemCalPosInfo> posList = new List<ItemCalPosInfo>();

            int dieIndex = 0;
            int SubdieIndex = 0;
            //获得mark点和第一个波导的位置差
            double markDeltaX = itemCalibrate.Chuck_AxisX - MapInfo.MarkChuckX;
            double markDeltaY = itemCalibrate.Chuck_AxisY - MapInfo.MarkChuckY;
            HCalibrationInfo.GetHeight(EquipmentInfo.Base_ChuckX, EquipmentInfo.Base_ChuckY, out double FAZ0);

            double deltaWGLX = itemCalibrate.LeftX - itemCalibrate.PadX;
            double deltaWGLY = itemCalibrate.LeftY - itemCalibrate.PadY;
            foreach (var die in sortedDies)
            {
                SubdieIndex = 0;
                string ordinate = $"({die.X},{die.Y})";
                double waferX0 = (die.ColumnIndex - calibrateDie.ColumnIndex) * MapInfo.DieWidth;
                double waferY0 = -(die.RowIndex - calibrateDie.RowIndex) * MapInfo.DieHeight;

                if (!WaferMotionPlat.PlatPointConvert(waferX0, waferY0, out double dieMarkX2, out double DieMarkY2))
                {
                    subError = "计算测试项坐标->坐标转换失败。";
                    return null;
                }

                foreach (var subDie in TestItems)
                {
                    if (!DieTestStatus[dieIndex][SubdieIndex])
                    {
                        SubdieIndex++;
                        continue;
                    }

                    ItemCalPosInfo posInfo = new ItemCalPosInfo();
                    posInfo.DieOrdinate = ordinate;
                    posInfo.SubDieName = subDie.Subname;
                    posInfo.Die = die;

                    double waferX1 = subDie.PadX - (itemCalibrate.PadX) + waferX0;
                    double waferY1 = subDie.PadY - (itemCalibrate.PadY) + waferY0;
                    if (!WaferMotionPlat.PlatPointConvert(waferX1, waferY1, out double chuckX, out double chuckY))
                    {
                        subError = "计算测试项坐标->坐标转换失败。";
                        return null;
                    }
                    chuckX += markDeltaX;
                    chuckY += markDeltaY;

                    //边缘不测试
                    double dis = Math.Sqrt((chuckX - HCalibrationInfo.CenterX) * (chuckX - HCalibrationInfo.CenterX) + (chuckY - HCalibrationInfo.CenterY) * (chuckY - HCalibrationInfo.CenterY));
                    if (dis > HCalibrationInfo.ArearCircle)
                    {
                        SubdieIndex++;
                        continue;
                    }

                    HCalibrationInfo.GetHeight(chuckX, chuckY, out double FAZ1);//测高仪器测量的距离Z方向和定义方向相反
                    if (FAZ1 == 0)
                    {
                        subError = "计算测试项坐标->计算高度失败。";
                        return null;
                    }

                    posInfo.ChuckZ = EquipmentInfo.ChuckZ + FAZ0 - FAZ1;  //托盘动 Fa动FAZ1 - FAZ0
                    posInfo.ChuckX = chuckX;
                    posInfo.ChuckY = chuckY;
                    posInfo.LeftX = itemCalibrate.Left_AxisX + subDie.LeftX - subDie.PadX - deltaWGLX;
                    posInfo.LeftY = itemCalibrate.Left_AxisY + subDie.LeftY - subDie.PadY - deltaWGLY;

                    posInfo.DieMarkX2 = dieMarkX2;
                    posInfo.DieMarkY2 = DieMarkY2;

                    posList.Add(posInfo);
                    SubdieIndex++;
                }

                dieIndex++;
            }

            return posList;

        }

        //双边有探针
        private List<ItemCalPosInfo> CalDoubleSideWithProbeDiePos(WaferMapInfo MapInfo, List<bool[]> DieTestStatus, EquipmentCalibrationInfo EquipmentInfo, List<DieInfo> sortedDies, DieInfo calibrateDie, SubdiePosCaliInfo itemCalibrate, List<SubdieOrdinary> TestItems, out string subError)
        {
            subError = string.Empty;
            List<ItemCalPosInfo> posList = new List<ItemCalPosInfo>();

            int dieIndex = 0;
            int SubdieIndex = 0;
            //获得mark点和第一个波导的位置差
            double markDeltaX = itemCalibrate.Chuck_AxisX - MapInfo.MarkChuckX;
            double markDeltaY = itemCalibrate.Chuck_AxisY - MapInfo.MarkChuckY;
            HCalibrationInfo.GetHeight(EquipmentInfo.Base_ChuckX, EquipmentInfo.Base_ChuckY, out double FAZ0);

            double deltaWGLX = itemCalibrate.LeftX - itemCalibrate.PadX;
            double deltaWGLY = itemCalibrate.LeftY - itemCalibrate.PadY;
            double deltaWGRX = itemCalibrate.RightX - itemCalibrate.PadX;
            double deltaWGRY = itemCalibrate.RightY - itemCalibrate.PadY;
            foreach (var die in sortedDies)
            {
                SubdieIndex = 0;
                string ordinate = $"({die.X},{die.Y})";
                double waferX0 = (die.ColumnIndex - calibrateDie.ColumnIndex) * MapInfo.DieWidth;
                double waferY0 = -(die.RowIndex - calibrateDie.RowIndex) * MapInfo.DieHeight;

                if (!WaferMotionPlat.PlatPointConvert(waferX0, waferY0, out double dieMarkX2, out double DieMarkY2))
                {
                    subError = "计算测试项坐标->坐标转换失败。";
                    return null;
                }

                foreach (var subDie in TestItems)
                {
                    if (!DieTestStatus[dieIndex][SubdieIndex])
                    {
                        SubdieIndex++;
                        continue;
                    }

                    ItemCalPosInfo posInfo = new ItemCalPosInfo();
                    posInfo.DieOrdinate = ordinate;
                    posInfo.SubDieName = subDie.Subname;
                    posInfo.Die = die;

                    double waferX1 = subDie.PadX - (itemCalibrate.PadX) + waferX0;
                    double waferY1 = subDie.PadY - (itemCalibrate.PadY) + waferY0;
                    if (!WaferMotionPlat.PlatPointConvert(waferX1, waferY1, out double chuckX, out double chuckY))
                    {
                        subError = "计算测试项坐标->坐标转换失败。";
                        return null;
                    }
                    chuckX += markDeltaX;
                    chuckY += markDeltaY;

                    //边缘不测试
                    double dis = Math.Sqrt((chuckX - HCalibrationInfo.CenterX) * (chuckX - HCalibrationInfo.CenterX) + (chuckY - HCalibrationInfo.CenterY) * (chuckY - HCalibrationInfo.CenterY));
                    if (dis > HCalibrationInfo.ArearCircle)
                    {
                        SubdieIndex++;
                        continue;
                    }

                    HCalibrationInfo.GetHeight(chuckX, chuckY, out double FAZ1);//测高仪器测量的距离Z方向和定义方向相反
                    if (FAZ1 == 0)
                    {
                        subError = "计算测试项坐标->计算高度失败。";
                        return null;
                    }

                    posInfo.ChuckZ = EquipmentInfo.ChuckZ + FAZ0 - FAZ1;  //托盘动 Fa动FAZ1 - FAZ0
                    posInfo.ChuckX = chuckX;
                    posInfo.ChuckY = chuckY;
                    posInfo.LeftY = itemCalibrate.Left_AxisX + subDie.LeftX - subDie.PadX - deltaWGLX;
                    posInfo.LeftY = itemCalibrate.Left_AxisY + subDie.LeftY - subDie.PadY - deltaWGLY;
                    posInfo.Right_X = itemCalibrate.Right_AxisX + subDie.RightX - subDie.PadX - deltaWGRX;
                    posInfo.Right_Y = itemCalibrate.Right_AxisY + subDie.RightY - subDie.PadY - deltaWGRY;

                    posInfo.DieMarkX2 = dieMarkX2;
                    posInfo.DieMarkY2 = DieMarkY2;

                    posList.Add(posInfo);
                    SubdieIndex++;
                }

                dieIndex++;
            }

            return posList;
        }

        private void SortDies(ref List<DieInfo> sortedDies, WaferMapInfo MapInfo)
        {
            for (int i = 0; i < MapInfo.Dies.Count; i++)
            {
                var tempDie = MapInfo.Dies.FirstOrDefault(t => t.Name == $"{i + 1}#");
                sortedDies.Add(tempDie);
            }
        }
    }
}
