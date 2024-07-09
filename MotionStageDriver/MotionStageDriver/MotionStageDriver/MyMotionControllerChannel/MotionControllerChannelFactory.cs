using MyMotionStageDriver.MyMotionController;

namespace MyMotionStageDriver.MyMotionControllerChannel {
    public static class MotionControllerChannelFactory {
        public static AbstractMotionControllerChannel CreateInstance(AbstractMotionController motionController, string boardAddress, string channelId) {
            motionController.ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            return new MotionControllerChannelImpl(motionController, boardAddress, channelId);
        }
    }
}
