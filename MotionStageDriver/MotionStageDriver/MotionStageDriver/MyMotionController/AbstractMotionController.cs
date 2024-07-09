using System.Xml.Linq;

using CommonApi.MyUtility;

using NLog;

namespace MyMotionStageDriver.MyMotionController {
    public abstract class AbstractMotionController {
        internal AbstractMotionController(string typId, string vendor, string model) {
            TypeId = typId;
            Vendor = vendor;
            Model = model;
            Initialized = false;
        }

        public string TypeId { get; private set; }
        public string Vendor { get; private set; }
        public string Model { get; private set; }
        public bool Initialized { get; protected set; }

        public abstract bool Initialize(XElement xeMotionController);
        public abstract bool Close();

        public abstract bool MoveRelative(string boardAddress, string channelId, double distance,bool isStageWithGrattingRuler = false);
        public abstract void MoveAbsolute(string boardAddress, string channelId, double position, bool isStageWithGrattingRuler = false);
        public abstract bool Homing(string boardAddress, string channelId);
        public abstract double GetPosition(string boardAddress, string channelId);
        public abstract void Stop(string boardAddress, string channelId);
        public abstract void EmergencyStop(string boardAddress, string channelId);     
        public abstract void SetSpeed(string boardAddress, string channelId, int speedInPps);
        public abstract int GetSpeed(string boardAddress, string channelId);
        public abstract double GetTravelPerPulse(string boardAddress, string channelId);
        public abstract bool Homing_XH(string boardAddress, string channelId, ushort coderReverse, ushort sevonLevel, int limitPos, int homeOffset, double minVel, double maxVel);
        public abstract bool Homing_XH_Z(string boardAddress, string channelId, ushort coderReverse, ushort sevonLevel, int limitPos, int homeOffset, double minVel, double maxVel);
        public abstract void EnableLeadcrewComp(string boardAddress, string channelId, ushort enable);
        public abstract void SetEncoderDir(string boardAddress, string channelId, ushort dir);
        public abstract void ClearEncoderPos(string boardAddress, string channelId);
        public abstract void SetHomeOffset(string boardAddress, string channelId,double homeOffset);

        public abstract void SetSoftLimit(string boardAddress, string channelId, ushort enable, ushort source,ushort action, int negLimit,int posLimit);
        public abstract void SetLeadcrewCompConfig(string boardAddress, string channelId, ushort numPos, int posStart, int posDis, int[] compPos, int[] compNeg);

        public abstract short SetAxisSpeed(string cardNo, string channelId, double minVel, double maxVel, double acc);
        public abstract short SetAxisSpeedEx(string cardNo, string channelId, double minVel, double maxVel, double acc);
        public abstract void GetAxisSpeed(string cardNo, string channelId, ref double minVel,ref double maxVel, ref double acc);
        //[gyh],2023-12-24,暂时屏蔽soft limit相关接口
        //public abstract bool EnableSoftLowerLimit(string boardAddress, string channelId, double limit);
        //public abstract bool EnableSoftUpperLimit(string boardAddress, string channelId, double limit);
        //public abstract void DisableSoftLowerLimit(string boardAddress, string channelId);
        //public abstract void DisableSoftUpperLimit(string boardAddress, string channelId);
        //public abstract double GetSoftLowerLimit(string boardAddress, string channelId);
        //public abstract double GetSoftUpperLimit(string boardAddress, string channelId);
        //public abstract bool IsAtSoftLimitNow(string boardAddress, string channelId);

        public abstract void ThrowExceptionWhenAxisBoardAddressIsInvalid(string boardAddress);
        public abstract void ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(string boardAddress, string channelId);

        protected static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        //[attention]: here is full set of trigger out setting items for all vendors of motion controller.
        public enum EnumTriggerOutSetting {
            //valid for Leisai DMC 3000
            STEP,           
            START_POSITION,

            //...
        }
    }
}
