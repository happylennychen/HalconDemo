namespace ProberApi.MyConstant {
    public static class SharedObjectKey {
        //allInstruments
        public const string ALL_INSTRUMENTS = nameof(ALL_INSTRUMENTS);        
        public const string INSTRUMENTS_USAGE_LIST = nameof(INSTRUMENTS_USAGE_LIST);

        //motor controllers & stages
        public const string STAGE_LIST = nameof(STAGE_LIST);
        public const string STAGE_AXIS_USAGE_DICT = nameof(STAGE_AXIS_USAGE_DICT);        
        public const string STAGE_JOG_UTILITY = nameof(STAGE_JOG_UTILITY);
        public const string LEISAI_DM3000_INSTANCE = nameof(LEISAI_DM3000_INSTANCE);

        public const string QUERY_LIST = nameof(QUERY_LIST);           
        public const string REQUEST_STATUS_BOARD = nameof(REQUEST_STATUS_BOARD);        
        public const string CONFIG_BOARD = nameof(CONFIG_BOARD);
        //red green light
        public const string RED_GREEN_LIGHT_BOARD = nameof(RED_GREEN_LIGHT_BOARD);

        //actions
        public const string ACTION_REPORT_MESSAGE = nameof(ACTION_REPORT_MESSAGE);
        public const string ACTION_SET_GUI_ENABLE_STATUS = nameof(ACTION_SET_GUI_ENABLE_STATUS);
        public const string ACTION_SET_GUI_CONTROL_MODE = nameof(ACTION_SET_GUI_CONTROL_MODE);

        //coupling basic       
        public const string COUPLING_IN_CONFIG = nameof(COUPLING_IN_CONFIG);
        public const string COUPLING_FEEDBACK_CONFIG = nameof(COUPLING_FEEDBACK_CONFIG);
        public const string COUPLING_FEEDBACK_GUI_MONITORING = nameof(COUPLING_FEEDBACK_GUI_MONITORING);
        public const string COUPLING_DISABLED_FEEDBACK_MONITORING = nameof(COUPLING_DISABLED_FEEDBACK_MONITORING);
        public const string COUPLING_RESTORE_FEEDBACK_MONITORING = nameof(COUPLING_RESTORE_FEEDBACK_MONITORING);

        //cross coupling 2D
        public const string CROSS_COUPLING_2D_PARAMETERS = nameof(CROSS_COUPLING_2D_PARAMETERS);
        public const string CROSS_COUPLING_2D_STEPPED_CHART = nameof(CROSS_COUPLING_2D_STEPPED_CHART);
        public const string CROSS_COUPLING_2D_TRIGGERED_CHART = nameof(CROSS_COUPLING_2D_TRIGGERED_CHART);

        //spiral coupling 2D
        public const string SPIRAL_COUPLING_2D_PARAMETERS = nameof(SPIRAL_COUPLING_2D_PARAMETERS);
        public const string SPIRAL_COUPLING_2D_CHART_CONFIG = nameof(SPIRAL_COUPLING_2D_CHART_CONFIG);

        //request
        public const string TEMPLATE_REQUEST_LIST = nameof(TEMPLATE_REQUEST_LIST);
        
        //checker
        public const string AXIS_USAGE_ID_VALID_CHECKER = nameof(AXIS_USAGE_ID_VALID_CHECKER);
        public const string INSTRUMENT_USAGE_ID_VALID_CHECKER = nameof(INSTRUMENT_USAGE_ID_VALID_CHECKER);

        public const string HAS_CONNECTED_INSTRUMENTS = nameof(HAS_CONNECTED_INSTRUMENTS);
    }
}
