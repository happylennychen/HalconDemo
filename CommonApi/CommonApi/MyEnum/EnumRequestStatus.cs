namespace CommonApi.MyEnum {
    public enum EnumRequestStatus {
        READY = -2,
        RUNNING = -1,
        COMPLETED_PASS = 0,
        COMPLETED_FAIL = 1,
        COMPLETED_EXCEPTION = 2,

        //[gyh]: request是否支持超时？
        COMPLETED_TIMEOUT = 3,
    }
}
