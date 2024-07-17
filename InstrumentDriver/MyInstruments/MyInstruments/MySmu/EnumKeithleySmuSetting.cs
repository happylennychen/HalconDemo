namespace MyInstruments.MySmu {
    public enum EnumKeithleySmuSetting {  
        SOURCE_TYPE = 0,
        SOURCE_LIMIT,
        SOURCE_LEVEL,        
           
        MEASURE_TYPE = 100,
        MEASURE_COUNT,
        MEASURE_DELAY,
        MEASURE_NPLC,
        MEASURE_IS_AUTORANGE,
        MEASURE_RANGE,
        
        IS_TURNED_ON = 200,
        //[cla]:支持羲禾项目中的命令
        RESET,
        SOURCE_HIGHC,
        SOURCE_RANGE,
        SENSE_MODE,
        SOURCE_OFF_MODE,
        SOURCE_OFF_TYPE,
        SOURCE_OFF_LIMIT,
    }
}
