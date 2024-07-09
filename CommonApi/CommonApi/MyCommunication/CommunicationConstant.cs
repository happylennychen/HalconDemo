namespace CommonApi.MyCommunication {
    public static class CommunicationConstant {
        public const string SAY_GOODBYE = "*GOODBYE!";
        public const string QUERY = "*QUERY?";
        public const string QUERY_IF_COMPLETED = "*OPC?";
        public const string REQUEST = "*REQUEST!";
        public const string WAIT_FOR_COMPLETED = "*WAIT_FOR_COMPLETED!";

        public const int BYTE_NUM_OF_LENGTH = 4;
        public const int BUFF_SIZE = 64 * 1024;

        public const int LISTENING_PORT_MIN = 50000;
        public const int LISTENING_PORT_MAX = 65535;
    }
}
