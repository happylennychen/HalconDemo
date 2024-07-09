using System;

namespace Prober.WaferDef {
    [Serializable]
    public class EquipmentCalibrationInfo
    {
        /*********************机台名称******************/
        public string DeviceName { get; set; } = "XH001";

        /***********************************************/
        public double PdPlat_ChuckX { get; set; }
        public double PdPlat_ChuckY {  get; set; }
        public double PdPlat_ChuckZ { get; set; }  

        /*********************基准项校准时 记录的角度******************/
        public double BaseItem_LeftSX { get; set; }
        public double BaseItem_LeftSY { get; set; }
        public double BaseItem_LeftSZ { get; set; }
        public double BaseItem_RightSX { get; set; }
        public double BaseItem_RightSY { get; set; }
        public double BaseItem_RightSZ { get; set; } 

        /*********************角度调整工位******************/
        //左侧角度调整相机工位
        public double LSX_CcdX { get; set; }
        public double LSX_CcdY { get; set; }
        public double LSX_CcdZ { get; set; }
        public double LSX_ChuckX { get; set; }
        public double LSX_ChuckY { get; set; }
        public double LSX_LeftX { get; set; }
        public double LSX_LeftY { get; set; }
        public double LSX_LeftZ { get; set; }
        public double LSX_Exp { get; set; } = 4000;

        public double LSY_CcdX { get; set; }
        public double LSY_CcdY { get; set; }
        public double LSY_CcdZ { get; set; }
        public double LSY_ChuckX { get; set; }
        public double LSY_ChuckY { get; set; }
        public double LSY_LeftX { get; set; }
        public double LSY_LeftY { get; set; }
        public double LSY_LeftZ { get; set; }
        public double LSY_Exp { get; set; } = 4000;

        public double LSZ_CcdX { get; set; }
        public double LSZ_CcdY { get; set; }
        public double LSZ_CcdZ { get; set; }
        public double LSZ_ChuckX { get; set; }
        public double LSZ_ChuckY { get; set; }
        public double LSZ_LeftX { get; set; }
        public double LSZ_LeftY { get; set; }
        public double LSZ_LeftZ { get; set; }
        public double LSZ_Exp { get; set; } = 4000;

        //右侧角度校准时的位置信息
        public double RSX_CcdX { get; set; }
        public double RSX_CcdY { get; set; }
        public double RSX_CcdZ { get; set; }
        public double RSX_ChuckX { get; set; }
        public double RSX_ChuckY { get; set; }
        public double RSX_RightX { get; set; }
        public double RSX_RightY { get; set; }
        public double RSX_RightZ { get; set; }
        public double RSX_Exp { get; set; } = 4000;

        public double RSY_CcdX { get; set; }
        public double RSY_CcdY { get; set; }
        public double RSY_CcdZ { get; set; }
        public double RSY_ChuckX { get; set; }
        public double RSY_ChuckY { get; set; }
        public double RSY_RightX { get; set; }
        public double RSY_RightY { get; set; }
        public double RSY_RightZ { get; set; }
        public double RSY_Exp { get; set; } = 4000;

        public double RSZ_CcdX { get; set; }
        public double RSZ_CcdY { get; set; }
        public double RSZ_CcdZ { get; set; }
        public double RSZ_ChuckX { get; set; }
        public double RSZ_ChuckY { get; set; }
        public double RSZ_RightX { get; set; }
        public double RSZ_RightY { get; set; }
        public double RSZ_RightZ { get; set; }
        public double RSZ_Exp { get; set; } = 4000;

        /*********************下料位置******************/
        public double Remove_ChuckX { get; set; } = double.NaN;
        public double Remove_ChuckY { get; set; } = double.NaN;
        public double Remove_ChuckZ { get; set; } = double.NaN;


        /*********************基准位置******************/
        /// <summary>
        /// 这个也是测试时晶圆的工位
        /// </summary>
        public double ChuckZ { get; set; } = double.NaN;
        public bool IsChuckCalibrated()
        {
            return !(ChuckZ == double.NaN);
        }

        ///  测高仪测量晶圆时 X2坐标
        /// </summary>
        public double Base_ChuckX { get; set; } = double.NaN;//晶圆扫描，测试高度时用
        /// <summary>
        ///  测高仪测量晶圆时 Y2坐标
        /// </summary>
        public double Base_ChuckY { get; set; } = double.NaN;//晶圆扫描，测试高度时用

        public double Base_ChuckSZ {  get; set; } = double.NaN; //

        public double Base_CcdX { get; set; } = double.NaN;//测高仪的工作位置，相机和测高仪同轴
        public double Base_CcdY { get; set; } = double.NaN;

        public double Base_CcdZ { get; set; } = double.NaN;
        //测高仪工位
        public double HSenserWorkU { get; set; } = 0;

        //放大倍数
        public double Base_Zoom { get; set; } = 2;

        public double Base_Exp { get; set; } = 4000;
        /*********************安全工位******************/
        public double Safe_LeftZ { get; set; } = -5000;
        public double Safe_RightZ { get; set; } = -5000;

        public double Safe_ChuckZ { get; set; } = 500;

        public double Safe_LeftX { get; set; } = 100;
        public double Safe_LeftY { get; set; } = 100;
        public double Safe_RightX { get; set; } = -100;
        public double Safe_RightY { get; set; } = 100;

        public double Safe_CcdZ { get; set; } = -100;
        public double Safe_U { get; set; } = -100;

        /*********************探针位置******************/
        public double ProbeWaferContactZ0 { get; set; } = 5500;
        public double ProbeCrimpingDepth { get; set; }
        public double ProbeContactCCDPos { get; set; }

        public string HomeType { get; set; } = "1";

        //Chuck平整度和安全高度相关设置
        public double ChuckHeightDelta { get; set; } = 30;

        //电容测高仪相关设置
        public double CapAltAdjustHeight { get; set; } = 30;
        public double CapAltAdjustLimit { get; set; } = 1;
        public double CapAltLockHeightRight { get; set; } = 100;
        public double CapAltLockHeightLeft { get; set; } = 100;
        public double CapAltAdjustStopThresh { get;set; } = 10;
        public double CapAmTolerance { get; set;} = 5;

        //tec 系数
        public double TecCoeffK { get; set; } = 1.24495;
        public double TecCoeffB { get; set; } = 7.0927;

        public string HeightScanMode { get; set; } = "0";

        public string MonitTestCondition { get; set; } = "0";
        public string ChuckSeperateHeight { get; set; } = "500";
    }
}
