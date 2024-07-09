using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prober.Request
{
    public static class PrivateSharedObjectKey
    {
        public const string SUBDIE_POS = nameof(SUBDIE_POS);
        public const string WAFER_HANDLE = nameof(WAFER_HANDLE);    
        public const string WAFER_TYPE = nameof(WAFER_TYPE);    
        public const string WAFER_UPLOAD_STATUE = nameof(WAFER_UPLOAD_STATUE);  
        public const string CCD_MIRRO_FUNC  = nameof(CCD_MIRRO_FUNC);
        public const string IS_CAP_ALTIMETER = nameof(IS_CAP_ALTIMETER);
        public const string CHUCK_POS_PRE = nameof(CHUCK_POS_PRE);
        public const string LEFT_CAP_HEIGHT = nameof(LEFT_CAP_HEIGHT);
        public const string RIGHT_CAP_HEIGHT = nameof(RIGHT_CAP_HEIGHT);
        public const string LEFT_CAP_ALTIMETER_REALTIME = nameof(LEFT_CAP_ALTIMETER_REALTIME);
        public const string RIGHT_CAP_ALTIMETER_REALTIME = nameof(RIGHT_CAP_ALTIMETER_REALTIME);
        public const string WAFER_MAP = nameof(WAFER_MAP);
        public const string EQUIPMENT_CALIBRATION_FILE = nameof(EQUIPMENT_CALIBRATION_FILE);   
        public const string HEIGHT_SCAN_MODE = nameof(HEIGHT_SCAN_MODE);
        public const string MONIT_TEST_CONDITION = nameof(MONIT_TEST_CONDITION);
        public const string CHUCK_SEPERATE_HEIGHT = nameof(CHUCK_SEPERATE_HEIGHT);
        public const string CHUCK_POSITION_SAFE = nameof(CHUCK_POSITION_SAFE);
        public const string CHUCK_POSITION_IS_SAFE = nameof(CHUCK_POSITION_IS_SAFE);
        public const string CHUCK_POSITION_MONITOR_ENABLE = nameof(CHUCK_POSITION_MONITOR_ENABLE);
        public const string IO_INFO = nameof(IO_INFO);
        public const string TEC_STATUS = nameof(TEC_STATUS);
        public const string TEC_TEMPERATURE = nameof(TEC_TEMPERATURE);
        public const string CCD_GOWITH_CHUCK = nameof(CCD_GOWITH_CHUCK);
        public const string USE_LEFT_CAP_ALTIMETER = nameof(USE_LEFT_CAP_ALTIMETER);
        public const string PAD_COMPENSATE_DIC = nameof(PAD_COMPENSATE_DIC);
    }
}
