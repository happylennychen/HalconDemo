using CommonApi.MyUtility;

using MyMotionStageDriver.MyMotionController;

using NLog;

namespace MyMotionStageDriver.MyMotionControllerChannel {
    public abstract class AbstractMotionControllerChannel {
        //public bool EnableSoftLowerLimit {
        //    get {
        //        return enabledSoftLowerLimit;
        //    }
        //    set {
        //        enabledSoftLowerLimit = value;
        //        if (!enabledSoftLowerLimit) {
        //            motorController.DisableSoftLowerLimit(channelId);
        //        }
        //    }
        //}
        //public double SoftLowerLimit {
        //    get {
        //        return motorController.GetSoftLowerLimit(channelId);
        //    }
        //    set {
        //        if (EnableSoftLowerLimit) {
        //            motorController.EnableSoftLowerLimit(channelId, value);
        //        } else {
        //            motorController.DisableSoftLowerLimit(channelId);
        //        }
        //    }
        //}
        //public bool EnableSoftUpperLimit {
        //    get {
        //        return enabledSoftUpperLimit;
        //    }
        //    set {
        //        enabledSoftUpperLimit = value;
        //        if (!enabledSoftUpperLimit) {
        //            motorController.DisableSoftUpperLimit(channelId);
        //        }
        //    }
        //}
        //public double SoftUpperLimit {
        //    get {
        //        return motorController.GetSoftUpperLimit(channelId);
        //    }
        //    set {
        //        if (enabledSoftUpperLimit) {
        //            motorController.EnableSoftUpperLimit(channelId, value);
        //        }
        //    }
        //}
        //public bool IsAtSoftLimitNow {
        //    get {
        //        return motorController.IsAtSoftLimitNow(channelId);
        //    }
        //}

        public abstract bool MoveRelative(double distanceInUm, bool isStageWithGrattingRuler=false);
        public abstract void MoveAbsolute(double positionInUm, bool isStageWithGrattingRuler=false);
        public abstract bool Homing();
        public abstract double GetPosition();
        public abstract void Stop();
        public abstract void EmergencyStop();
        public abstract void SetSpeed(int speedInPps);
        public abstract int GetSpeed();

        public abstract double GetTravelPerPulse();
        public AbstractMotionController MotionController { get { return this.motionController; } }
        public string BoardAddress { get { return this.boardAddress; } }
        public string ChannelId { get { return this.channelId; } }
        public abstract bool Homing_XH(ushort coderReverse, ushort sevonLevel, int limitPos, int homeOffset, double minVel, double maxVel);
        public abstract bool Homing_XH_Z(ushort coderReverse, ushort sevonLevel, int limitPos, int homeOffset, double minVel, double maxVel);

        public abstract void EnableLeadcrewComp(ushort enable);
        public abstract void ClearEncoderPos();
        public abstract void SetEncoderDir(ushort dir);
        public abstract void SetHomeOffset(double homeOffset);

        public abstract void SetSoftLimit(ushort enable, ushort source, ushort action, int negLimit, int posLimit);

        public abstract void SetLeadcrewCompConfig(ushort numPos, int posStart, int posDis, int[] comPos, int[] comNeg);

        public abstract short SetAxisSpeed(double minVel, double maxVel, double acc);
        public abstract short SetAxisSpeedEx(double minVel, double maxVel, double acc);
        public abstract void GetAxisSpeed(ref double minvel, ref double maxVel, ref double acc);

        internal AbstractMotionControllerChannel(AbstractMotionController motionController, string boardAddress, string channelId) {
            this.motionController = motionController;
            this.boardAddress = boardAddress;
            this.channelId = channelId;
        }

        protected readonly AbstractMotionController motionController;
        protected readonly string boardAddress;
        protected readonly string channelId;
        protected static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        
        //private bool enabledSoftUpperLimit = false;
        //private bool enabledSoftLowerLimit = false;
    }
}
