using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using CommonApi.MyTrigger;

using MyMotionStageDriver.MyMotionController.Leisai;

namespace MyMotionStageDriver.MyMotionController.Virtual {
    public sealed class MotionControllerVirtual : AbstractMotionController, ITriggerOut {
        internal MotionControllerVirtual(string id) : base(id, VIRTUAL_VENDOR, VIRTUAL_MODEL) {
        }

        public override bool Initialize(XElement xeMotionController) {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"Call MotionControllerVirtual.Initialize(...)");

            positionDict.Clear();
            List<XElement> xeMotionControllerBoardList = xeMotionController.Elements("motion_controller_board").ToList();
            foreach (var xeMotionControllerBoard in xeMotionControllerBoardList) {
                string strBoardAddress = xeMotionControllerBoard.Attribute("address").Value;
                Dictionary<string, double> channelPosition = new Dictionary<string, double>();
                Dictionary<string, int> channelSpeed = new Dictionary<string, int>();
                for (int i = 0; i <= MotionControllerLeisaiDmc3000.MAX_AXIS_CHANNEL_ID; i++) {
                    channelPosition.Add(i.ToString(), 0.0);
                    channelSpeed.Add(i.ToString(), 10000);
                }
                positionDict.Add(strBoardAddress, channelPosition);
                speedDict.Add(strBoardAddress, channelSpeed);
            }
            return true;
        }

        public override bool Close() {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"Call MotionControllerVirtual.Close()");
            return true;
        }

        public override short SetAxisSpeed(string boardAddress, string channelId, double minVel, double maxVel, double acc) {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"Call MotionControllerVirtual.SetAxisSpeed()");
            return 1;
        }

        public override void GetAxisSpeed(string boardAddress, string channelId, ref double minVel, ref double maxVel, ref double acc) {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"Call MotionControllerVirtual.GetAxisSpeed()");
            return ;
        }

        public override short SetAxisSpeedEx(string boardAddress, string channelId, double minVel, double maxVel, double acc) {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"Call MotionControllerVirtual.SetAxisSpeedEx()");
            return 1;
        }

        public override bool MoveRelative(string boardAddress, string channelId, double distance, bool isStageWithGrattingRuler = false) {
            LOGGER.Info($"Call MotionControllerVirtual.MoveRelative({boardAddress}, {channelId}, {distance})");
            motionCompletedFlag = false;
            Task.Run(() => {
                Thread.Sleep(TimeSpan.FromMilliseconds(500)); //simulate time elapsed                     
                motionCompletedFlag = true;
            });
            WaitForCompleted(channelId,isStageWithGrattingRuler);

            double currentPosition = positionDict[boardAddress][channelId];
            positionDict[boardAddress][channelId] = currentPosition + distance;

            return true;
        }

        public override void MoveAbsolute(string boardAddress, string channelId, double position, bool isStageWithGrattingRuler = false) {
            LOGGER.Info($"Call MotionControllerVirtual.MoveAbsolute({boardAddress}, {channelId}, {position})");
            motionCompletedFlag = false;
            Task.Run(() => {
                Thread.Sleep(TimeSpan.FromMilliseconds(500)); //simulate time elapsed                    
                motionCompletedFlag = true;
            });
            WaitForCompleted(channelId, isStageWithGrattingRuler);
            positionDict[boardAddress][channelId] = position;
        }

        public override bool Homing(string boardAddress, string channelId) {
            LOGGER.Info($"Call MotionControllerVirtual.Homing({boardAddress}, {channelId})");
            motionCompletedFlag = false;
            Task.Run(() => {
                Thread.Sleep(TimeSpan.FromSeconds(3)); //simulate time elapsed
                motionCompletedFlag = true;
            });
            WaitForCompleted(channelId,false);
            positionDict[boardAddress][channelId] = 0;

            return true;
        }

        public override bool Homing_XH(string boardAddress, string channelId, ushort coderReverse, ushort sevonLevel, int limitPos, int homeOffset, double minVel, double maxVel)
        {
            LOGGER.Info($"Call MotionControllerVirtual.Homing_XH({boardAddress}, {channelId})");
            motionCompletedFlag = false;
            Task.Run(() => {
                Thread.Sleep(TimeSpan.FromSeconds(3)); //simulate time elapsed
                motionCompletedFlag = true;
            });
            WaitForCompleted(channelId,false);
            positionDict[boardAddress][channelId] = 0;

            return true;
        }

        public override bool Homing_XH_Z(string boardAddress, string channelId, ushort coderReverse, ushort sevonLevel, int limitPos, int homeOffset, double minVel, double maxVel) {
            LOGGER.Info($"Call MotionControllerVirtual.Homing_XH_Z({boardAddress}, {channelId})");
            motionCompletedFlag = false;
            Task.Run(() => {
                Thread.Sleep(TimeSpan.FromSeconds(3)); //simulate time elapsed
                motionCompletedFlag = true;
            });
            WaitForCompleted(channelId, false);
            positionDict[boardAddress][channelId] = 0;

            return true;
        }

        public override void EnableLeadcrewComp(string boardAddress, string channelId, ushort enable)
        {
            LOGGER.Info($"Call MotionControllerVirtual.EnableLeadcrewComp({boardAddress}, {channelId}, {enable})");
            Task.Run(() => {
                Thread.Sleep(TimeSpan.FromMilliseconds(500)); //simulate time elapsed                    
                motionCompletedFlag = true;
            });
        }

        public override void ClearEncoderPos(string boardAddress, string channelId) {
            LOGGER.Info($"Call MotionControllerVirtual.ClearEncoderPos({boardAddress}, {channelId})");
            Task.Run(() => {
                Thread.Sleep(TimeSpan.FromMilliseconds(500)); //simulate time elapsed                    
                motionCompletedFlag = true;
            });
        }

        public override void SetHomeOffset(string boardAddress, string channelId, double homeOffset) {
            LOGGER.Info($"Call MotionControllerVirtual.SetHomeOffset({boardAddress}, {channelId},{homeOffset})");
            Task.Run(() => {
                Thread.Sleep(TimeSpan.FromMilliseconds(500)); //simulate time elapsed                    
                motionCompletedFlag = true;
            });
        }

        public override void SetSoftLimit(string boardAddress, string channelId, ushort enable, ushort source, ushort action, int negLimit, int posLimit)
        {
            LOGGER.Info($"Call MotionControllerVirtual.SetSoftLimit({boardAddress},{channelId},{enable},{source},{action},{negLimit},{posLimit})");
            Task.Run(() => {
                Thread.Sleep(TimeSpan.FromMilliseconds(500)); //simulate time elapsed                    
                motionCompletedFlag = true;
            });
        }

        public override void SetEncoderDir(string boardAddress, string channelId, ushort dir) {
            LOGGER.Info($"Call MotionControllerVirtual.SetEncoderDir({boardAddress}, {channelId}, {dir})");
            Task.Run(() => {
                Thread.Sleep(TimeSpan.FromMilliseconds(500)); //simulate time elapsed                    
                motionCompletedFlag = true;
            });
        }

        public override void SetLeadcrewCompConfig(string boardAddress, string channelId, ushort numPos, int posStart, int posDis, int[] compPos, int[] compNeg)
        {
            LOGGER.Info($"Call MotionControllerVirtual.SetLeadcrewCompConfig({boardAddress}, {channelId}, {numPos}, {posStart}, {posDis})");
            Task.Run(() => {
                Thread.Sleep(TimeSpan.FromMilliseconds(500)); //simulate time elapsed                    
                motionCompletedFlag = true;
            });
        }

        public override double GetPosition(string boardAddress, string channelId) {
            LOGGER.Info($"Call MotionControllerVirtual.GetPosition({boardAddress},{channelId})");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            return positionDict[boardAddress][channelId];
        }

        //public override double GetSoftLowerLimit(string channelId) {
        //    object mutex = channelMutexDict[channelId];
        //    lock (mutex) {
        //        Thread.Sleep(TimeSpan.FromMilliseconds(100));
        //        LOGGER.Info($"Call SteppedMotorControllerVirtual[{this.TypeId}].GetSoftLowerLimit({channelId})");
        //        Random random = new Random();
        //        return random.NextDouble() * 10000;
        //    }
        //}
        //public override double GetSoftUpperLimit(string channelId) {
        //    object mutex = channelMutexDict[channelId];
        //    lock (mutex) {
        //        Thread.Sleep(TimeSpan.FromMilliseconds(100));
        //        LOGGER.Info($"Call SteppedMotorControllerVirtual[{this.TypeId}].GetSoftUpperLimit({channelId})");
        //        Random random = new Random();
        //        return random.NextDouble() * 10000;
        //    }
        //}

        //public override bool SettingAllChannels(XElement xeChannels) {
        //    Thread.Sleep(TimeSpan.FromMilliseconds(100));
        //    LOGGER.Info($"Call SteppedMotorControllerVirtual.SettingAllChannels(xeChannels)");

        //    ChannelIdSet.Clear();
        //    channelMutexDict.Clear();
        //    List<XElement> xeChannelList = xeChannels.Elements("channel").ToList();
        //    foreach (XElement xeChannel in xeChannelList) {
        //        string channelId = xeChannel.Attribute("id").Value.Trim();
        //        if (string.IsNullOrEmpty(channelId)) {
        //            LOGGER.Error($"In config_motion_stages.xml, <stepped_motor_controllers><stepped_motor_controller id={this.TypeId}><channels><channel id=>, channel id is empty!");
        //            return false;
        //        }
        //        if (ChannelIdSet.Contains(channelId)) {
        //            LOGGER.Error($"In config_motion_stages.xml, <stepped_motor_controllers><stepped_motor_controller id={this.TypeId}><channels><channel id={channelId}>, channel id is duplicated!");
        //            return false;
        //        }
        //        ChannelIdSet.Add(channelId);

        //        XElement xeSettings = xeChannel.Element("settings");
        //        List<XElement> xeSettingList = xeSettings.Elements("setting").ToList();
        //        foreach (XElement xeSetting in xeSettingList) {
        //            string key = xeSetting.Attribute("key").Value.Trim();
        //            string strValue = xeSetting.Attribute("value").Value.Trim();
        //            //......
        //        }
        //    }

        //    foreach (string channelId in ChannelIdSet) {
        //        object mutex = new object();
        //        channelMutexDict.Add(channelId, mutex);
        //    }

        //    return true;
        //}

        //public override bool IsAtSoftLimitNow(string channelId) {
        //    object mutex = channelMutexDict[channelId];
        //    lock (mutex) {
        //        Thread.Sleep(TimeSpan.FromMilliseconds(100));
        //        LOGGER.Info($"Call SteppedMotorControllerVirtual[{this.TypeId}].IsAtSoftLimitNow({channelId})");
        //        return false;
        //    }
        //}

        public override void Stop(string boardAddress, string channelId) {
            LOGGER.Info($"Call MotionControllerVirtual.Stop({boardAddress}, {channelId})");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
        }

        public override void EmergencyStop(string boardAddress, string channelId) {
            LOGGER.Info($"Call MotionControllerVirtual.EmergencyStop({boardAddress}, {channelId})");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
        }

        private void WaitForCompleted(string channelId,bool isStageWithGrattingRuler) {
            while (true) {
                Thread.Sleep(TimeSpan.FromMilliseconds(200));
                if (motionCompletedFlag) {
                    return;
                }
            }
        }

        public override void ThrowExceptionWhenAxisBoardAddressIsInvalid(string boardAddress) {
        }

        public override void ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(string boardAddress, string channelId) {
        }

        public override void SetSpeed(string boardAddress, string channelId, int speedInPps) {
            speedDict[boardAddress][channelId] = speedInPps;
            LOGGER.Info($"Call MotionControllerVirtual.SetSpeed({boardAddress}, {channelId}), {speedInPps}");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
        }

        public override int GetSpeed(string boardAddress, string channelId) {
            return speedDict[boardAddress][channelId];
        }

        public override double GetTravelPerPulse(string boardAddress, string channelId) {
            return 0.01;
        }
        public (bool isOk, string errorText) IsTriggerOutSettingKeyMissed(Dictionary<string, string> settings) {
            throw new NotImplementedException();
        }

        public (bool isOk, string errorText) CheckTriggerOutSettings(Dictionary<string, string> settings) {
            string errorText = string.Empty;
            string boardAddress = settings[EnumTriggerMotionController.BOARD_ADDRESS.ToString()];
            try {
                ThrowExceptionWhenAxisBoardAddressIsInvalid(boardAddress);
            } catch (Exception) {
                errorText = $"In trigger-out-settings(Virtual Motion controller Leisai DMC3000), {EnumTriggerMotionController.BOARD_ADDRESS.ToString()} value(={settings[EnumTriggerMotionController.BOARD_ADDRESS.ToString()]}) is invalid!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            string channelId = settings[EnumTriggerMotionController.CHANNEL_ID.ToString()];
            try {
                ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            } catch (Exception) {
                errorText = $"In trigger-out-settings(Virtual Motion controller Leisai DMC3000), either {EnumTriggerMotionController.BOARD_ADDRESS.ToString()} value(={boardAddress}) or {EnumTriggerMotionController.CHANNEL_ID.ToString()} value(={channelId}) is invalid!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            if (!int.TryParse(settings[EnumTriggerCommonSetting.STEP_NUMBER.ToString()], out int stepNumber)) {
                errorText = $"In trigger-out-settings(Virtual Motion controller Leisai DMC3000), {EnumTriggerCommonSetting.STEP_NUMBER.ToString()}(={settings[EnumTriggerCommonSetting.STEP_NUMBER.ToString()]}) should be an integer!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            if (!double.TryParse(settings[EnumTriggerOutSetting.START_POSITION.ToString()], out double startPosition)) {
                errorText = $"In trigger-out-settings(Virtual Motion controller Leisai DMC3000), {EnumTriggerOutSetting.START_POSITION.ToString()}(={settings[EnumTriggerOutSetting.START_POSITION.ToString()]}) should be a double!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            if (!double.TryParse(settings[EnumTriggerOutSetting.STEP.ToString()], out double step)) {
                errorText = $"In trigger-out-settings(Virtual Motion controller Leisai DMC3000), {EnumTriggerOutSetting.STEP.ToString()}(={settings[EnumTriggerOutSetting.STEP.ToString()]}) should be a double!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            return (true, string.Empty);
        }

        public bool PrepareTriggeringOut(Dictionary<string, string> settings) {
            LOGGER.Info($"Call MotionControllerVirtual.PrepareTriggeringOut(...)");

            string boardAddress = settings[EnumTriggerMotionController.BOARD_ADDRESS.ToString()];
            string channelId = settings[EnumTriggerMotionController.CHANNEL_ID.ToString()];
            int.TryParse(settings[EnumTriggerCommonSetting.STEP_NUMBER.ToString()], out triggerStepNumber);
            double.TryParse(settings[EnumTriggerOutSetting.STEP.ToString()], out triggerStep);
            double.TryParse(settings[EnumTriggerOutSetting.START_POSITION.ToString()], out triggerStartPosition);

            return true;
        }

        public bool StartTriggeringOut() {
            LOGGER.Info($"Call MotionControllerVirtual.StartTriggeringOut()");

            triggerData.Clear();
            isTriggerCompleted = false;
            for (int i = 0; i < triggerStepNumber; i++) {
                triggerData.Add(triggerStartPosition + i * triggerStep);
                Thread.Sleep(TimeSpan.FromMilliseconds(200));
            }
            isTriggerCompleted = true;
            return true;
        }

        public void WaitForTriggeringOutCompleted() {
            while (true) {
                Thread.Sleep(500);
                if (isTriggerCompleted) {
                    break;
                }
            }
        }

        public (bool isOk, List<double> data) GetTriggeringOutData() {
            return (true, triggerData);
        }

        internal const string VIRTUAL_VENDOR = "VIRTUAL_VENDOR";
        internal const string VIRTUAL_MODEL = "VIRTUAL_MODEL";
        private bool motionCompletedFlag = false;
        private double triggerStartPosition = 0.0;
        private int triggerStepNumber = 0;
        private double triggerStep = 0.0;
        private bool isTriggerCompleted = false;
        private readonly List<double> triggerData = new List<double>();

        private readonly Dictionary<string, Dictionary<string, double>> positionDict = new Dictionary<string, Dictionary<string, double>>();
        private readonly Dictionary<string, Dictionary<string, int>> speedDict = new Dictionary<string, Dictionary<string, int>>();
    }
}
