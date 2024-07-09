using MyMotionStageDriver.MyMotionController;

namespace MyMotionStageDriver.MyMotionControllerChannel {
    internal sealed class MotionControllerChannelImpl : AbstractMotionControllerChannel {
        internal MotionControllerChannelImpl(AbstractMotionController motionController, string boardAddress, string channelId) : base(motionController, boardAddress, channelId) {
        }

        public override bool MoveRelative(double distance, bool isStageWithGrattingRuler = false) {
            return motionController.MoveRelative(boardAddress, channelId, distance, isStageWithGrattingRuler);
        }

        public override short SetAxisSpeed(double minVel, double maxVel, double acc) {
            return motionController.SetAxisSpeed(boardAddress, channelId, minVel, maxVel, acc);
        }

        public override void GetAxisSpeed(ref double minVel, ref double maxVel, ref double acc)
        {
            motionController.GetAxisSpeed(boardAddress, channelId, ref minVel, ref maxVel, ref acc);
        }

        public override short SetAxisSpeedEx(double minVel, double maxVel, double acc) {
            return motionController.SetAxisSpeedEx(boardAddress, channelId, minVel, maxVel, acc);
        }

        public override void MoveAbsolute(double position, bool isStageWithGrattingRuler = false) {
            motionController.MoveAbsolute(boardAddress, channelId, position, isStageWithGrattingRuler);
        }

        public override bool Homing() {
            return motionController.Homing(boardAddress, channelId);
        }

        public override double GetPosition() {
            return motionController.GetPosition(boardAddress, channelId);
        }

        public override void Stop() {
            motionController.Stop(boardAddress, channelId);
        }

        public override void EmergencyStop() {
            motionController.EmergencyStop(boardAddress, channelId);
        }

        public override void SetSpeed(int speedInPps) {
            motionController.SetSpeed(boardAddress, channelId, speedInPps);
        }

        public override void EnableLeadcrewComp(ushort enable)
        {
            motionController.EnableLeadcrewComp(boardAddress, channelId, enable);
        }

        public override void ClearEncoderPos() {
            motionController.ClearEncoderPos(boardAddress, channelId);
        }

        public override void SetHomeOffset(double homeOffset) {
            motionController.SetHomeOffset(boardAddress, channelId, homeOffset);
        }

        public override void SetSoftLimit(ushort enable, ushort source, ushort action, int negLimit, int posLimit)
        {
            motionController.SetSoftLimit(boardAddress, channelId, enable, source, action, negLimit, posLimit);
        }

        public override void SetEncoderDir(ushort dir) {
            motionController.SetEncoderDir(boardAddress, channelId, dir);
        }

        public override void SetLeadcrewCompConfig(ushort numPos, int posStart, int posDis, int[] comPos, int[] comNeg)
        {
            motionController.SetLeadcrewCompConfig(boardAddress, channelId, numPos, posStart, posDis, comPos, comNeg);
        }


        public override bool Homing_XH(ushort coderReverse, ushort sevonLevel, int limitPos, int homeOffset, double minVel, double maxVel)
        {
            return motionController.Homing_XH(boardAddress,channelId, coderReverse, sevonLevel, limitPos, homeOffset,minVel,maxVel);
        }

        public override bool Homing_XH_Z(ushort coderReverse, ushort sevonLevel, int limitPos, int homeOffset, double minVel, double maxVel) {
            return motionController.Homing_XH_Z(boardAddress, channelId, coderReverse, sevonLevel, limitPos, homeOffset, minVel, maxVel);
        }

        public override int GetSpeed() {
            return motionController.GetSpeed(boardAddress, channelId);
        }
        public override double GetTravelPerPulse() {
            return motionController.GetTravelPerPulse(boardAddress, channelId);
        }
    }
}
