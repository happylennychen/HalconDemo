using MyMotionStageDriver.MyMotionController;
using MyMotionStageDriver.MyMotionControllerChannel;

namespace MyMotionStageDriver.MyStageAxis {
    public static class StageAxisFactory {
        public static StageAxis CreateInstance(AbstractMotionController motionController, string boardAddress, string channelId) {
            AbstractMotionControllerChannel channel = MotionControllerChannelFactory.CreateInstance(motionController, boardAddress, channelId);
            return new StageAxis(channel);
        }
    }
}
