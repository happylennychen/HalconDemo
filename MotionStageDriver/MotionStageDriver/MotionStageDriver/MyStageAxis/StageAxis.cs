using System;
using System.Threading;

using CommonApi.MyUtility;

using MyMotionStageDriver.MyEnum;
using MyMotionStageDriver.MyMotionController;
using MyMotionStageDriver.MyMotionControllerChannel;

namespace MyMotionStageDriver.MyStageAxis {
    public sealed class StageAxis {
        public string StageId { get; set; }
        public string AxisId { get; set; }
        public EnumAxisType AxisType { get; set; }
        public EnumAxisPositiveDirection PositiveDirection { get; set; }
        public double Backlash { get; set; }
        public AutoResetEvent TransactionLock { get; } = new AutoResetEvent(true);
        public AbstractMotionController MotionController { get { return this.channel.MotionController; } }
        public string MotionControllerBoardAddress { get { return this.channel.BoardAddress; } }
        public string MotionControllerChannelId { get { return this.channel.ChannelId; } }

        //public bool EnableSoftLowerLimit {
        //    get { return channel.EnableSoftLowerLimit; }
        //    set { channel.EnableSoftLowerLimit = value; }
        //}
        //public double SoftLowerLimit {
        //    get { return channel.SoftLowerLimit; }
        //    set { channel.SoftLowerLimit = value; }
        //}
        //public bool EnableSoftUpperLimit {
        //    get { return channel.EnableSoftUpperLimit; }
        //    set { channel.EnableSoftUpperLimit = value; }
        //}
        //public double SoftUpperLimit {
        //    get { return channel.SoftUpperLimit; }
        //    set { channel.SoftUpperLimit = value; }
        //}

        public Action StartFastTimerRefreshPosition { get { return startFastTimerRefreshPosition; } }
        public Action StartSlowTimerRefreshPosition { get { return startSlowTimerRefreshPosition; } }
        public Action StopTimerRefreshPosition { get { return stopTimerRefreshPosition; } }
        public Action GuiUpdatePosition { get { return guiUpdatePosition; } }

        private bool isSafeHeightLock = false;
        private bool isLock = false;
        private bool isZAxis = false;
        private bool enableMoveUp = false; 
        private bool stageWithGrattingRuler = false;
        public double speedMaxConfig;
        public double speedMinConfig;
        public double accConfig;


        public void SetStageSpeedConfig(double speedMin, double speedMax, double acc) {
            this.speedMaxConfig = speedMax;
            this.speedMinConfig = speedMin;
            this.accConfig = acc;
        }

        /// <summary>
        /// speedMinConfig 暂时没用
        /// </summary>
        /// <param name="speed"></param>
        public void SetSpeedSpecial(EnumMotionSpeed speed){
            if (speed == EnumMotionSpeed.FAST) {
                SetAxisSpeed(speedMinConfig, speedMaxConfig * 2,accConfig);    
            } else if (speed == EnumMotionSpeed.NORMAL) {
                SetAxisSpeed(speedMinConfig, speedMaxConfig, accConfig);
            } else {
                SetAxisSpeed(speedMinConfig, speedMaxConfig / 2, accConfig);
            }
        }
        
        public void SetLockState(bool isLock) {
            this.isLock = isLock;
        }

        public void SetSafeHeightLock(bool isLock) {
            this.isSafeHeightLock = isLock; 
        }

        public void SetZAxis(bool isZAxis) {
            this.isZAxis = isZAxis; 
        }

        public void EnabelMoveUp(bool enableMoveUp) {
            this.enableMoveUp = enableMoveUp;   
        }

        public void SetStageWithGrattingRuler(bool stageWithGrattingRuler) {
            this.stageWithGrattingRuler = stageWithGrattingRuler;  
        }

        internal StageAxis(AbstractMotionControllerChannel channel) {
            this.channel = channel;
        }

        public bool MoveRelative(double distance) {
            if (((!isLock) || (isLock && isZAxis && enableMoveUp && (distance > 0))
                || (isLock && isZAxis && (!enableMoveUp) && (distance < 0)))&& ((!isSafeHeightLock))) {
                return channel.MoveRelative(distance, stageWithGrattingRuler);
            }

            return true;
        }
        public void MoveAbsolute(double position) {
            if (!isLock && !isSafeHeightLock) {
                channel.MoveAbsolute(position, stageWithGrattingRuler);
            }            
        }
        public bool Homing() {
            return channel.Homing();
        }

        public double Position() {
            return channel.GetPosition();
        }

        public void Stop() {
            channel.Stop();
        }

        public void EmergencyStop() {
            channel.EmergencyStop();
        }

        public double GetTravelPerPulse() {
            return channel.GetTravelPerPulse();
        }
        public int GetSpeed() {
            return channel.GetSpeed();
        }

        public void SetSpeed(int speedInPps) {
            channel.SetSpeed(speedInPps);
        }

        public void EnableLeadcrewComp(ushort enable)
        {
            channel.EnableLeadcrewComp(enable);
        }

        public void ClearEncoderPos() {
            channel.ClearEncoderPos();
        }

        public void SetEncoderDir(ushort dir) {
            channel.SetEncoderDir(dir);
        }

        public void SetHomeOffset(double homeOffset) {
            channel.SetHomeOffset(homeOffset);
        }

        public void SetSoftLimit(ushort enable, ushort source, ushort action, int negLimit, int posLimit)
        {
            channel.SetSoftLimit(enable, source, action, negLimit, posLimit);
        }

        public void SetLeadcrewCompConfig(ushort numPos, int posStart, int posDis, int[] comPos, int[] comNeg)
        {
            channel.SetLeadcrewCompConfig(numPos, posStart, posDis, comPos, comNeg);
        }

        public bool Homing_XH(ushort coderReverse, ushort sevonLevel, int limitPos, int homeOffset, double minVel, double maxVel)
        {
            return channel.Homing_XH(coderReverse, sevonLevel, limitPos, homeOffset,minVel,maxVel);
        }

        public bool Homing_XH_Z(ushort coderReverse, ushort sevonLevel, int limitPos, int homeOffset, double minVel, double maxVel) {
            return channel.Homing_XH_Z(coderReverse, sevonLevel, limitPos, homeOffset, minVel, maxVel);
        }

        public short SetAxisSpeed(double minVel, double maxVel, double acc) {
            return channel.SetAxisSpeed(minVel, maxVel, acc);   
        }

        public void GetAxisSpeed(ref double minVel, ref double maxVel, ref double acc) {
            channel.GetAxisSpeed(ref minVel, ref maxVel, ref acc);
        }

        public short SetAxisSpeedEx(double minVel, double maxVel, double acc) {
            return channel.SetAxisSpeedEx(minVel, maxVel, acc);
        }

        public void MoveRelativeWithBacklash(double distance,double gap,int dir) {
            if (dir < 0) {
                channel.MoveRelative(distance - gap);
                channel.MoveRelative(gap);
            }
            else {
                channel.MoveRelative(gap);
                channel.MoveRelative(distance - gap);                
            }            
        }

        public void OvercomeBacklash() {
#if false
            if (MyStaticUtility.DoesTwoDoublesEqual(Backlash, 0.0)) {
                return;
            }

            //[gyh] 这里需反转2次，因此需要记录last direction。待后续其他同事完善！
            channel.MoveRelative(-1 * this.Backlash);
            channel.MoveRelative(this.Backlash);            
#endif
            
            double gap = 4;
            channel.MoveRelative(-1 * gap);
            channel.MoveRelative(gap);            
        }

        public void OvercomeBacklash(int dir, double gap) {
            if (dir < 0) {
                channel.MoveRelative(gap);
                channel.MoveRelative(-gap);
            }
            else {
                channel.MoveRelative(-gap);
                channel.MoveRelative(gap);
            }
        }

            public void AttachActionRefreshGuiPosition(Action startFastTimerRefreshPosition, Action startSlowTimerRefreshPosition, Action stopTimerRefreshPosition, Action guiUpdatePosition) {
            this.startFastTimerRefreshPosition = startFastTimerRefreshPosition;
            this.startSlowTimerRefreshPosition = startSlowTimerRefreshPosition;
            this.stopTimerRefreshPosition = stopTimerRefreshPosition;
            this.guiUpdatePosition = guiUpdatePosition;
        }

        private readonly AbstractMotionControllerChannel channel;
        private Action startFastTimerRefreshPosition;
        private Action startSlowTimerRefreshPosition;
        private Action stopTimerRefreshPosition;
        private Action guiUpdatePosition;
    }
}
