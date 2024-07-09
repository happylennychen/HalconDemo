namespace MyMotionStageDriver.MyMotionController {
    public interface IDigitalIo {
        bool ReadInput(string boardAddress, string ioChannelId);
        void WriteOutput(string boardAddress, string ioChannelId, bool value);
        bool ReadOutput(string boardAddress, string ioChannelId);
    }
}
