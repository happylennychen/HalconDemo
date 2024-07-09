using System.Collections.Generic;

namespace Prober.Constant {
    public static class MyRedGreenLigthKey {
        public const string FA_SENSOR = "FA Sensor";        
        public const string DIE_CHUCK_VACCUM = "Die Chuck vacuum";
        public const string WAFER_CHUCK_VACCUM = "Wafer Chuck vacuum";
        public const string AUXILIARY_TABLE_VACCUM = "Auxiliary table vacuum";
        public const string INPUT_PRESURE_VACCUM = "Input presure vaccum";
        public const string SAFE_HEIGHT_CHUCK = "SafeHeight Chuck";
        public const string TEC_ONOFF = "TecOnOff";

        public static List<string> GetAllKeys() {
            var result = new List<string>() {
                FA_SENSOR,                
                DIE_CHUCK_VACCUM,
                WAFER_CHUCK_VACCUM,
                AUXILIARY_TABLE_VACCUM,
                INPUT_PRESURE_VACCUM,
                SAFE_HEIGHT_CHUCK,
                TEC_ONOFF
            };

            return result;
        }
    }

    public static class MyStageAxisKey
    {
        public const string LEFT_X = "coupling_in_x";
        public const string LEFT_Y = "coupling_in_y";
        public const string LEFT_Z = "coupling_in_z";
        public const string LEFT_SX = "coupling_in_sx";
        public const string LEFT_SY = "coupling_in_sy";
        public const string LEFT_SZ = "coupling_in_sz";

        public const string RIGHT_X = "coupling_out_x";
        public const string RIGHT_Y = "coupling_out_y";
        public const string RIGHT_Z = "coupling_out_z";
        public const string RIGHT_SX = "coupling_out_sx";
        public const string RIGHT_SY = "coupling_out_sy";
        public const string RIGHT_SZ = "coupling_out_sz";

        public const string CCD_X = "ccd_x";
        public const string CCD_Y = "ccd_y";
        public const string CCD_Z = "ccd_z";
        public const string HEIGHT_U = "height_u";

        public const string CHUCK_X = "chuck_x";
        public const string CHUCK_Y = "chuck_y";
        public const string CHUCK_Z = "chuck_z";
        public const string CHUCK_SZ = "chuck_sz";
    }

    public static class Cap_Altimeter_Channel {
        public const int Left_Channel = 2;
        public const int Right_Channel = 1;
    }
}
