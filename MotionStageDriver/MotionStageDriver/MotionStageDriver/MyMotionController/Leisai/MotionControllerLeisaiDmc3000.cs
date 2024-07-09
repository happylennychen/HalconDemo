using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

using CommonApi.MyTrigger;
using CommonApi.MyUtility;

using MyMotionStageDriver.MyEnum;

namespace MyMotionStageDriver.MyMotionController.Leisai {
    public sealed class MotionControllerLeisaiDmc3000 : AbstractMotionController, IDigitalIo, ITriggerOut {
        public MotionControllerLeisaiDmc3000(string typeId)
            : base(typeId, EnumMotionControllerVendor.LEISAI.ToString(), EnumLeisaiModel.DMC3000.ToString()) {
        }

        public override bool Initialize(XElement xeMotionController) {
            if (Initialized) {
                return true;
            }

            var initResult = WrapperOfLeisaiDmc3000Api.Initialize();
            if (!initResult.isOk) {
                return false;
            }
            if (!ExtractBoardChannelDict(xeMotionController, TypeId, initResult.cardNumber)) {
                return false;
            }
            GenerateMutexDict();
            this.wrapperOfLeisaiDm3000Api = new WrapperOfLeisaiDmc3000Api(this.cardChannelMutexes);

            var checkResult = CheckIfAllBoradsAllChannelsSettingsValid(xeMotionController);
            if (!checkResult.isOk) {
                return false;
            }
            if (!SettingAllBoardsAllChannels(checkResult.settings)) {
                return false;
            }

            Initialized = true;
            return true;
        }

        public override bool Close() {
            if (!Initialized) {
                return true;
            }

            return WrapperOfLeisaiDmc3000Api.Close();
        }

        public override short SetAxisSpeed(string boardAddress, string channelId, double minVel, double maxVel, double acc) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);

            return wrapperOfLeisaiDm3000Api.SetAxisSpeed(cardNo, axisNo, minVel, maxVel, acc);
        }

        public override void GetAxisSpeed(string boardAddress, string channelId, ref double minVel, ref double maxVel, ref double acc)
        {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);

            wrapperOfLeisaiDm3000Api.GetAxisSpeed(cardNo, axisNo, ref minVel, ref maxVel, ref acc);
        }

        public override short SetAxisSpeedEx(string boardAddress, string channelId, double minVel, double maxVel, double acc) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);

            return wrapperOfLeisaiDm3000Api.SetAxisSpeedEx(cardNo, axisNo, minVel, maxVel, acc);
        }

        public override double GetTravelPerPulse(string boardAddress, string channelId) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);
            double distancePerPulse = distancePerPulseConfig[cardNo][axisNo];
            return distancePerPulse;
        }
        public override bool MoveRelative(string boardAddress, string channelId, double distance, bool isStageWithGrattingRuler = false) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);
            double distancePerPulse = distancePerPulseConfig[cardNo][axisNo];
           

            ///***添加补偿，只针对chuckXY  
            ////X2轴
            //if (!_isInitCompensate) {
            //    InitCompensate();
            //    _isInitCompensate = true;
            //}
            //double curPos = GetPosition(boardAddress, channelId);
            //bool isPos = distance > 0;
            //if (boardAddress == "1") {
            //    if (channelId == "6") {
            //        var start = XComp.GetSendSetPosition(curPos, isPos);
            //        var end = XComp.GetSendSetPosition(curPos+distance, isPos);
            //        distance = end - start;
            //        _isX2Dir_Pos = isPos;
            //    }
            //}
            ////Y2轴
            //if (boardAddress == "0") {
            //    if (channelId == "7") {
            //        var start = YComp.GetSendSetPosition(curPos, isPos);
            //        var end = YComp.GetSendSetPosition(curPos + distance, isPos);
            //        distance = end - start; 
            //    }
            //    _isX2Dir_Pos = isPos;
            //}
            ///***添加补偿，只针对chuckXY  

            int deltaPulseNumber = (int)Math.Round(distance / distancePerPulse);
            if (deltaPulseNumber == 0) {
                return true;
            }
            return wrapperOfLeisaiDm3000Api.MoveRelative(cardNo, axisNo, deltaPulseNumber, isStageWithGrattingRuler);
        }

#if false
        ///...手动补偿
        private bool _isInitCompensate;
        private bool _isX2Dir_Pos;
        private bool _isY2Dir_Pos;
#endif
        private PitchCompensation XComp = new PitchCompensation();
        private PitchCompensation YComp = new PitchCompensation();
        public void InitCompensate() {
            List<double> posList = new List<double>();
            List<double> negList = new List<double>();
            GetOffsetData_rtl(@"C:\Users\Administrator\Desktop\激光干涉数据\_X_11111.rtl", ref posList, ref negList);
            XComp.GenerateCompensateInfo(0, 350000, 1400, posList.ToArray(), negList.ToArray());
            posList = new List<double>();
            negList = new List<double>();
            GetOffsetData_rtl(@"C:\Users\Administrator\Desktop\激光干涉数据\_Y_11111.rtl", ref posList, ref negList);
            YComp.GenerateCompensateInfo(0, 350000, 1400, posList.ToArray(), negList.ToArray());
        }

        public static void GetOffsetData_rtl(string path, ref List<double> PosList, ref List<double> NegList) {
            PosList = new List<double>();
            NegList = new List<double>();
            List<double> List0 = new List<double>();

            string[] bb;
            double da1;

            var str = File.ReadAllLines(path);
            bool bz = false;
            foreach (var row in str) {
                if (row == "") {
                    continue;
                }
                bb = row.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (bb.Length == 3) {
                    if (bb[0].Contains("Run")) {
                        bz = true;
                        continue;
                    }
                } else {
                    ;
                }

                if (bb[0].Contains("ENVIRONMENT")) {
                    break;
                }

                if (bz) {
                    da1 = Convert.ToDouble(bb[2]);

                    List0.Add(da1);
                }
            }

            for (int i = 0; i < List0.Count / 2; i++) {
                PosList.Add(List0[i]);
            }
            for (int i = List0.Count / 2; i < List0.Count; i++) {
                NegList.Add(List0[i]);
            }
        }
        ///...手动补偿

        public override void MoveAbsolute(string boardAddress, string channelId, double position, bool isStageWithGrattingRuler = false) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);
            double distancePerPulse = distancePerPulseConfig[cardNo][axisNo];

            ///***添加补偿，只针对chuckXY  
            ////X2轴
            //if (!_isInitCompensate) {
            //    InitCompensate();
            //    _isInitCompensate = true;
            //}
            //double curPos = GetPosition(boardAddress, channelId);
            //bool isPos = position > curPos;
            //if (boardAddress == "1") {
            //    if (channelId == "6") {
            //        position = XComp.GetSendSetPosition(distancePerPulse, isPos);
            //    }
            //    _isX2Dir_Pos = isPos;
            //}
            ////Y2轴
            //if (boardAddress == "0") {
            //    if (channelId == "7") {
            //        position = YComp.GetSendSetPosition(distancePerPulse, isPos);
            //    }
            //    _isY2Dir_Pos = isPos;
            //}
            ///***添加补偿，只针对chuckXY  


            int absolutePositionInPulseNumber = (int)Math.Round(position / distancePerPulse);
            wrapperOfLeisaiDm3000Api.MoveAbsolute(cardNo, axisNo, absolutePositionInPulseNumber,isStageWithGrattingRuler);
        }

        public override bool Homing(string boardAddress, string channelId) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);
            return wrapperOfLeisaiDm3000Api.Homing(cardNo, axisNo);
        }

        public override double GetPosition(string boardAddress, string channelId) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);
            int absolutePositionInPulseNumber = wrapperOfLeisaiDm3000Api.GetPositionInPulseNumber(cardNo, axisNo);
            double distancePerPulse = distancePerPulseConfig[cardNo][axisNo];

            var resDis = absolutePositionInPulseNumber * distancePerPulse;

            ///***添加补偿，只针对chuckXY  
            ////X2轴
            //if (!_isInitCompensate) {
            //    InitCompensate();
            //    _isInitCompensate = true;
            //}
            //if (boardAddress == "1") {
            //    if (channelId == "6") {//224955.  224926.
            //        resDis = XComp.GetRelPosition(resDis, _isX2Dir_Pos);
            //    }
            //}
            ////Y2轴
            //if (boardAddress == "0") {
            //    if (channelId == "7") {
            //        resDis = YComp.GetRelPosition(resDis, _isY2Dir_Pos);
            //    }
            //}
            ///***添加补偿，只针对chuckXY  

            return resDis;
        }

        public override void Stop(string boardAddress, string channelId) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);
            wrapperOfLeisaiDm3000Api.Stop(cardNo, axisNo);
        }

        public override void EmergencyStop(string boardAddress, string channelId) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);
            wrapperOfLeisaiDm3000Api.EmergencyStop(cardNo, axisNo);
        }

        public override void SetSpeed(string boardAddress, string channelId, int speedInPps) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);
            wrapperOfLeisaiDm3000Api.ChangeSpeed(cardNo, axisNo, speedInPps);
        }

        public override void SetLeadcrewCompConfig(string boardAddress, string channelId, ushort numPos, int posStart, int posDis, int[] compPos, int[] compNeg) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);
            wrapperOfLeisaiDm3000Api.SetLeadcrewCompConfig(cardNo, axisNo, numPos, posStart, posDis, compPos, compNeg);
        }

        public override void EnableLeadcrewComp(string boardAddress, string channelId, ushort enable) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);
            wrapperOfLeisaiDm3000Api.EnableLeadcrewComp(cardNo, axisNo, enable);
        }

        public override void ClearEncoderPos(string boardAddress, string channelId) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);
            wrapperOfLeisaiDm3000Api.ClearEncoderPos(cardNo, axisNo);
        }

        public override void SetHomeOffset(string boardAddress, string channelId, double homeOffset) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);
            wrapperOfLeisaiDm3000Api.SetHomeOffset(cardNo, axisNo, homeOffset);
        }

        public override void SetSoftLimit(string boardAddress, string channelId, ushort enable, ushort source, ushort action, int negLimit, int posLimit) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);
            wrapperOfLeisaiDm3000Api.SetSoftLimit(cardNo, axisNo, enable, source, action, negLimit, posLimit);
        }

        public override void SetEncoderDir(string boardAddress, string channelId, ushort dir) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);
            wrapperOfLeisaiDm3000Api.SetEncoderDir(cardNo, axisNo, dir);
        }

        public override bool Homing_XH(string boardAddress, string channelId, ushort coderReverse, ushort sevonLevel, int limitPos, int homeOffset, double minVel, double maxVel) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);
            return wrapperOfLeisaiDm3000Api.Homing_XH(cardNo, axisNo, coderReverse, sevonLevel, limitPos, homeOffset, minVel, maxVel);
        }

        public override bool Homing_XH_Z(string boardAddress, string channelId, ushort coderReverse, ushort sevonLevel, int limitPos, int homeOffset, double minVel, double maxVel) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);
            return wrapperOfLeisaiDm3000Api.Homing_XH_Z(cardNo, axisNo, coderReverse, sevonLevel, limitPos, homeOffset, minVel, maxVel);
        }

        public override int GetSpeed(string boardAddress, string channelId) {
            ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort axisNo = Convert.ToUInt16(channelId);
            return wrapperOfLeisaiDm3000Api.GetSpeed(cardNo, axisNo);
        }

        public bool ReadInput(string boardAddress, string ioChannelId) {
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort ioNo = Convert.ToUInt16(ioChannelId);
            short result = wrapperOfLeisaiDm3000Api.ReadIoInput(cardNo, ioNo);
            return result != 0;
        }

        public void WriteOutput(string boardAddress, string ioChannelId, bool value) {
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort ioNo = Convert.ToUInt16(ioChannelId);
            ushort output = (ushort)(value ? 1 : 0);
            wrapperOfLeisaiDm3000Api.WriteIoOutput(cardNo, ioNo, output);
        }

        public bool ReadOutput(string boardAddress, string ioChannelId) {
            ushort cardNo = Convert.ToUInt16(boardAddress);
            ushort ioNo = Convert.ToUInt16(ioChannelId);
            short result = wrapperOfLeisaiDm3000Api.ReadIoOutput(cardNo, ioNo);
            return result != 0;
        }

        public override void ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(string boardAddress, string channelId) {
            string exceptionMessage = $"Board address(={boardAddress}) of Leisai DMC3000 is invalid! It should be a ushort and <= {MAX_BOARD_ID}!";
            if (!ushort.TryParse(boardAddress, out ushort cardNo)) {
                throw new Exception(exceptionMessage);
            }
            if (cardNo > MAX_BOARD_ID) {
                throw new Exception(exceptionMessage);
            }
            if (!boardChannelDict.ContainsKey(cardNo)) {
                exceptionMessage = $"Board address(={boardAddress}) of Leisai DMC3000 does not exist!";
                throw new Exception(exceptionMessage);
            }

            exceptionMessage = $"Axis No(={channelId}) of Leisai DMC3000 is invalid! It should be a ushort and <= {MAX_AXIS_CHANNEL_ID}!";
            if (!ushort.TryParse(channelId, out ushort axisNo)) {
                throw new Exception(exceptionMessage);
            }
            if (axisNo > MAX_AXIS_CHANNEL_ID) {
                throw new Exception(exceptionMessage);
            }
            HashSet<ushort> channelIdSet = boardChannelDict[cardNo];
            if (!channelIdSet.Contains(axisNo)) {
                exceptionMessage = $"Axis No(={channelId}) @board address(={boardAddress}) of Leisai DMC3000 does not exist!";
                throw new Exception(exceptionMessage);
            }
        }

        public override void ThrowExceptionWhenAxisBoardAddressIsInvalid(string boardAddress) {
            string exceptionMessage = $"Board address(={boardAddress}) of Leisai DMC3000 is invalid! It should be a ushort and <= {MAX_BOARD_ID}!";
            if (!ushort.TryParse(boardAddress, out ushort cardNo)) {
                throw new Exception(exceptionMessage);
            }
            if (cardNo > MAX_BOARD_ID) {
                throw new Exception(exceptionMessage);
            }
            if (!boardChannelDict.ContainsKey(cardNo)) {
                exceptionMessage = $"Board address(={boardAddress}) of Leisai DMC3000 does not exist!";
                throw new Exception(exceptionMessage);
            }
        }

        public void ThrowExceptionWhenIoBoardAddressOrChannelIdIsInvalid(string boardAddress, string ioChannelId) {
            ThrowExceptionWhenAxisBoardAddressIsInvalid(boardAddress);

            string exceptionMessage = $"IO No(={ioChannelId}) of Leisai DMC3000 is invalid! It should be a ushort and <= {MAX_IO_CHANNEL_ID}!";
            if (!ushort.TryParse(ioChannelId, out ushort ioNo)) {
                throw new Exception(exceptionMessage);
            }
            if (ioNo > MAX_IO_CHANNEL_ID) {
                throw new Exception(exceptionMessage);
            }
        }

        public bool SetChannelIoMap(ushort boardAddress, ushort ioChannelId, ushort ioType, ushort mapIoType, ushort mapIoIndex, double filter) {
            short reuslt = wrapperOfLeisaiDm3000Api.SetChannelIoMap(boardAddress, ioChannelId, ioType, mapIoType, mapIoIndex, filter);
            return (reuslt == 0);
        }

        public bool SetEmgMode(ushort boardAddress, ushort ioChannelId, ushort enable, ushort emgLogic) {
            short reuslt = wrapperOfLeisaiDm3000Api.SetEmgMode(boardAddress, ioChannelId, enable, emgLogic);
            return (reuslt == 0);
        }

        public (bool isOk, string errorText) IsTriggerOutSettingKeyMissed(Dictionary<string, string> settings) {
            HashSet<string> validKeySet = new HashSet<string>();
            foreach (EnumTriggerCommonSetting setting in Enum.GetValues(typeof(EnumTriggerCommonSetting))) {
                validKeySet.Add(setting.ToString());
            }
            foreach (EnumTriggerMotionController setting in Enum.GetValues(typeof(EnumTriggerMotionController))) {
                validKeySet.Add(setting.ToString());
            }
            foreach (EnumTriggerOutSetting setting in Enum.GetValues(typeof(EnumTriggerOutSetting))) {
                validKeySet.Add(setting.ToString());
            }

            string errorText = string.Empty;
            var inputKeySet = settings.Keys.ToHashSet();
            if (!validKeySet.SetEquals(inputKeySet)) {
                string validKeys = $"{string.Join(",", validKeySet)}";
                string inputKeys = $"{string.Join(",", inputKeySet)}";
                errorText = $"Current keys of trigger-out-settings is invalid! Valid keys are {{{validKeys}}}; Current keys are{{{inputKeys}}}.";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            return (true, string.Empty);
        }

        public (bool isOk, string errorText) CheckTriggerOutSettings(Dictionary<string, string> settings) {
            string errorText = string.Empty;
            string boardAddress = settings[EnumTriggerMotionController.BOARD_ADDRESS.ToString()];
            try {
                ThrowExceptionWhenAxisBoardAddressIsInvalid(boardAddress);
            } catch (Exception) {
                errorText = $"In trigger-out-settings(Motion controller Leisai DMC3000), {EnumTriggerMotionController.BOARD_ADDRESS.ToString()} value(={settings[EnumTriggerMotionController.BOARD_ADDRESS.ToString()]}) is invalid!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            string channelId = settings[EnumTriggerMotionController.CHANNEL_ID.ToString()];
            try {
                ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);
            } catch (Exception) {
                errorText = $"In trigger-out-settings(Motion controller Leisai DMC3000), either {EnumTriggerMotionController.BOARD_ADDRESS.ToString()} value(={boardAddress}) or {EnumTriggerMotionController.CHANNEL_ID.ToString()} value(={channelId}) is invalid!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            if (!int.TryParse(settings[EnumTriggerCommonSetting.STEP_NUMBER.ToString()], out int stepNumber)) {
                errorText = $"In trigger-out-settings(Motion controller Leisai DMC3000), {EnumTriggerCommonSetting.STEP_NUMBER.ToString()}(={settings[EnumTriggerCommonSetting.STEP_NUMBER.ToString()]}) should be an integer!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            if (!double.TryParse(settings[EnumTriggerOutSetting.START_POSITION.ToString()], out double startPosition)) {
                errorText = $"In trigger-out-settings(Motion controller Leisai DMC3000), {EnumTriggerOutSetting.START_POSITION.ToString()}(={settings[EnumTriggerOutSetting.START_POSITION.ToString()]}) should be a double!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            if (!double.TryParse(settings[EnumTriggerOutSetting.STEP.ToString()], out double step)) {
                errorText = $"In trigger-out-settings(Motion controller Leisai DMC3000), {EnumTriggerOutSetting.STEP.ToString()}(={settings[EnumTriggerOutSetting.STEP.ToString()]}) should be a double!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            return (true, string.Empty);
        }

        public bool PrepareTriggeringOut(Dictionary<string, string> settings) {
            triggeringBoardAddress = settings[EnumTriggerMotionController.BOARD_ADDRESS.ToString()];
            triggeringChannelId = settings[EnumTriggerMotionController.CHANNEL_ID.ToString()];
            int.TryParse(settings[EnumTriggerCommonSetting.STEP_NUMBER.ToString()], out int stepNumber);
            double.TryParse(settings[EnumTriggerOutSetting.START_POSITION.ToString()], out double startPosition);
            double.TryParse(settings[EnumTriggerOutSetting.STEP.ToString()], out double step);
            ushort.TryParse(triggeringBoardAddress, out ushort cardNo);
            ushort.TryParse(triggeringChannelId, out ushort channelId);
            double distancePerPulse = distancePerPulseConfig[cardNo][channelId];

            moveStep = step;

            triggeringData = new List<double>();
            for (int i = 0; i < stepNumber; i++) {
                triggeringData.Add(startPosition + (1 + i) * step); //[gyh]: 特别注意此行！
            }

            relMoveDisPulse = (int)Math.Round((stepNumber + 2) * step / distancePerPulse);

            const ushort HCMP = 3;
            const ushort CMP_LOGIG = 1;
            const ushort CMP_SOURCE = 0;            
            const ushort CMP_ENABLE = 5;
            const int TIME_IN_US = 100;

            try {
                wrapperOfLeisaiDm3000Api.HcmpSetConfig(cardNo, channelId, HCMP, CMP_SOURCE, CMP_LOGIG, TIME_IN_US);
                wrapperOfLeisaiDm3000Api.HcmpSetMode(cardNo, HCMP, CMP_ENABLE);
                wrapperOfLeisaiDm3000Api.HcmpClearPoints(cardNo, HCMP);
                wrapperOfLeisaiDm3000Api.HcmpSetLiner(cardNo, HCMP, (int)(Math.Round(step / distancePerPulse)), stepNumber);
                wrapperOfLeisaiDm3000Api.HcmpAddPoint(cardNo, HCMP, (int)(Math.Round((startPosition + step) / distancePerPulse))); //[gyh]: 特别注意此行！
                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        public bool StartTriggeringOut() {
            //[gyh]
            //???如何开始运动！

            try {
                ushort.TryParse(triggeringBoardAddress, out ushort cardNo);
                ushort.TryParse(triggeringChannelId, out ushort channelId);
                if (relMoveDisPulse == 0) {
                    return true;
                }

                //设置低速模式,先按照步长0.2us，速度20us/s速度设置，tpp = 0.05;
                wrapperOfLeisaiDm3000Api.SetAxisSpeedEx(cardNo, channelId, 0, moveStep * 10000, 0.1);
                wrapperOfLeisaiDm3000Api.MoveRelative(cardNo, channelId, relMoveDisPulse,false);
                //恢复高速模式
                wrapperOfLeisaiDm3000Api.SetAxisSpeedEx(cardNo, channelId, 0, 20000, 0.01);

            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }

            return true;
        }

        public void WaitForTriggeringOutCompleted() {
            //[gyh]
            //???            
        }

        public (bool isOk, List<double> data) GetTriggeringOutData() {
            return (true, triggeringData);
        }

        #region private 

        private void GenerateMutexDict() {
            cardChannelMutexes.Clear();
            foreach (var one in boardChannelDict) {
                ushort cardNo = one.Key;
                Dictionary<ushort, object> channelMutexes = new Dictionary<ushort, object>();
                foreach (ushort axisNo in one.Value) {
                    channelMutexes.Add(axisNo, new object());
                }
                cardChannelMutexes.Add(cardNo, channelMutexes);
            }
        }

        private bool ExtractBoardChannelDict(XElement xeMotionController, string typeId, short discoverdCardNumber) {
            List<XElement> xeBoardList = xeMotionController.Elements("motion_controller_board").ToList();
            boardChannelDict.Clear();
            foreach (XElement xeBoard in xeBoardList) {
                string strBoardAddress = xeBoard.Attribute("address").Value.Trim();
                if (string.IsNullOrEmpty(strBoardAddress)) {
                    LOGGER.Error($"In Configuration/config_motion_stages.xml, <motion_controllers><motion_controller type_id={typeId} vendor=LEISAI model=DMC3000> <motion_controller_board address=>, board address should not be empty!");
                    return false;
                }
                if (!ushort.TryParse(strBoardAddress, out ushort cardNo)) {
                    LOGGER.Error($"In Configuration/config_motion_stages.xml, <motion_controllers><motion_controller type_id={typeId} vendor=LEISAI model=DMC3000> <motion_controller_board address={strBoardAddress}>, board address(card No) should be a unsigned short integer!");
                    return false;
                }
                if (cardNo >= discoverdCardNumber) {
                    LOGGER.Error($"In Configuration/config_motion_stages.xml, <motion_controllers><motion_controller type_id={typeId} vendor=LEISAI model=DMC3000> <motion_controller_board address={strBoardAddress}>, board address(card No={strBoardAddress}) is invalid! Discovered card number is {discoverdCardNumber}.");
                    return false;
                }
                if (boardChannelDict.ContainsKey(cardNo)) {
                    LOGGER.Error($"In Configuration/config_motion_stages.xml, <motion_controllers><motion_controller type_id={typeId} vendor=LEISAI model=DMC3000> <motion_controller_board address={strBoardAddress}>, board address={strBoardAddress} is duplicated!");
                    return false;
                }

                XElement xeChannels = xeBoard.Element("channels");
                List<XElement> xeChannelList = xeChannels.Elements("channel").ToList();

                HashSet<ushort> channelIdSet = new HashSet<ushort>();
                foreach (XElement xeChannel in xeChannelList) {
                    string strChannelId = xeChannel.Attribute("id").Value.Trim();
                    if (string.IsNullOrEmpty(strChannelId)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <motion_controllers><motion_controller type_id={typeId} vendor=LEISAI model=DMC3000> <motion_controller_board address={strBoardAddress}><channels><channel id=>, channel id should not be empty!");
                        return false;
                    }

                    if (!ushort.TryParse(strChannelId, out ushort channelId)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <motion_controllers><motion_controller type_id={typeId} vendor=LEISAI model=DMC3000> <motion_controller_board address={strBoardAddress}><channels><channel id={strChannelId}>, channel id should be a unsigned short integer!");
                        return false;
                    }

                    if (channelId > MAX_AXIS_CHANNEL_ID) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <motion_controllers><motion_controller type_id={typeId} vendor=LEISAI model=DMC3000> <motion_controller_board address={strBoardAddress}><channels><channel id={strChannelId}>, channel id should not be larger than {MAX_AXIS_CHANNEL_ID}!");
                        return false;
                    }

                    if (channelIdSet.Contains(channelId)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <motion_controllers><motion_controller type_id={typeId} vendor=LEISAI model=DMC3000> <motion_controller_board address={strBoardAddress}><channels><channel id={strChannelId}>, channel id={strChannelId} is duplicated!");
                        return false;
                    }
                    channelIdSet.Add(channelId);
                }
                boardChannelDict.Add(cardNo, channelIdSet);
            }

            return true;
        }

        private (bool isOk, Dictionary<ushort, Dictionary<ushort, Dictionary<EnumInitialSetting, string>>> settings) CheckIfAllBoradsAllChannelsSettingsValid(XElement xeMotionController) {
            Dictionary<ushort, Dictionary<ushort, Dictionary<EnumInitialSetting, string>>> allBoardsAllChannelsSettings = new Dictionary<ushort, Dictionary<ushort, Dictionary<EnumInitialSetting, string>>>();

            try {
                List<XElement> xeBoardList = xeMotionController.Elements("motion_controller_board").ToList();
                foreach (XElement xeBoard in xeBoardList) {
                    string strBoardAddress = xeBoard.Attribute("address").Value.Trim();
                    ushort cardNo = Convert.ToUInt16(strBoardAddress);
                    XElement xeBoardSettings = xeBoard.Element("settings");
                    if (!IsBoardSettingsValid(cardNo, xeBoardSettings)) {
                        return (false, null);
                    }
                    if (!SettingBoard(cardNo, xeBoardSettings)) {
                        return (false, null);
                    }

                    XElement xeChannels = xeBoard.Element("channels");
                    List<XElement> xeChannelList = xeChannels.Elements("channel").ToList();
                    var allChannelsPerBoardSettings = new Dictionary<ushort, Dictionary<EnumInitialSetting, string>>();
                    foreach (XElement xeChannel in xeChannelList) {
                        string strChannelId = xeChannel.Attribute("id").Value.Trim();
                        ushort channelId = Convert.ToUInt16(strChannelId);
                        XElement xeSettings = xeChannel.Element("settings");
                        var checkResult = CheckIfChannelSettingsValid(cardNo, channelId, xeSettings);
                        if (!checkResult.isOk) {
                            return (false, null);
                        }
                        allChannelsPerBoardSettings.Add(channelId, checkResult.config);
                    }

                    allBoardsAllChannelsSettings.Add(cardNo, allChannelsPerBoardSettings);
                }
            } catch (Exception e) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(e));
                return (false, null);
            }

            return (true, allBoardsAllChannelsSettings);
        }

        private bool IsBoardSettingsValid(ushort cardNo, XElement xeSettings) {
            return true;
        }

        private bool SettingBoard(ushort cardNo, XElement xeBoardSettings) {
            return true;
        }

        private (bool isOk, Dictionary<EnumInitialSetting, string> config) CheckIfChannelSettingsValid(ushort cardNo, ushort channelId, XElement xeSettings) {
            try {
                List<XElement> xeSettingList = xeSettings.Elements("setting").ToList();
                Dictionary<EnumInitialSetting, string> config = new Dictionary<EnumInitialSetting, string>();
                foreach (XElement xeSetting in xeSettingList) {
                    string strKey = xeSetting.Attribute("key").Value.ToUpperInvariant().Trim();
                    if (string.IsNullOrEmpty(strKey)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <motion_controllers><motion_controller id={this.TypeId}><motion_controller_board address={cardNo}><channels><channel id={channelId}><settings><setting key=>, key should not be empty!");
                        return (false, null);
                    }
                    if (!Enum.TryParse(strKey, out EnumInitialSetting settingKey)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <motion_controllers><motion_controller id={this.TypeId}><motion_controller_board address={cardNo}><channels><channel id={channelId}><settings><setting key={strKey}>, key(={strKey}) is invalid!");
                        return (false, null);
                    }
                    string strValue = xeSetting.Attribute("value").Value.Trim();
                    if (string.IsNullOrEmpty(strValue)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <motion_controllers><motion_controller id={this.TypeId}><motion_controller_board address={cardNo}><channels><channel id={channelId}><settings><setting key={strKey} value=>, value should not be empty!");
                        return (false, null);
                    }
                    config.Add(settingKey, strValue);
                }

                return (true, config);
            } catch (Exception e) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(e));
                return (false, null);
            }
        }

        private bool SettingAllBoardsAllChannels(Dictionary<ushort, Dictionary<ushort, Dictionary<EnumInitialSetting, string>>> settings) {
            distancePerPulseConfig.Clear();
            foreach (var one in settings) {
                ushort cardNo = one.Key;
                Dictionary<ushort, Dictionary<EnumInitialSetting, string>> allChannelsSetting = one.Value;

                Dictionary<ushort, double> distancePerPulseAtCard = new Dictionary<ushort, double>();
                distancePerPulseConfig.Add(cardNo, distancePerPulseAtCard);
                foreach (var two in allChannelsSetting) {
                    ushort channelId = two.Key;
                    Dictionary<EnumInitialSetting, string> oneChannelSetting = two.Value;
                    if (!SettingChannel(cardNo, channelId, oneChannelSetting)) {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool SettingChannel(ushort cardNo, ushort channelId, Dictionary<EnumInitialSetting, string> setting) {
            string strValue = setting[EnumInitialSetting.PULSE_OUT_MODE];
            ushort pulseOutMode = Convert.ToUInt16(strValue);
            //dmc_set_pulse_outmode()，see user guide chinese version P123
            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_pulse_outmode(cardNo, channelId, pulseOutMode);
            if (result != 0) {
                LOGGER.Error($"Failed to set Leisai Dm3000 {EnumInitialSetting.PULSE_OUT_MODE.ToString()}={pulseOutMode}.");
                LOGGER.Error($"ImportingLeisaiDm3000DllOfC.dmc_set_pulse_outmode({cardNo}, {channelId}, {pulseOutMode}) returns {result}.");
                return false;
            }

            strValue = setting[EnumInitialSetting.ELECTRICAL_LEVEL_OF_ORIGIN];
            ushort eLevel = Convert.ToUInt16(strValue);
            //dmc_set_home_pin_logic(), see user guide chinese version P124
            double reservedValueOfFilter = 0.0;
            result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_home_pin_logic(cardNo, channelId, eLevel, reservedValueOfFilter);
            if (result != 0) {
                LOGGER.Error($"Failed to set Leisai Dm3000 {EnumInitialSetting.ELECTRICAL_LEVEL_OF_ORIGIN.ToString()}={eLevel}.");
                LOGGER.Error($"ImportingLeisaiDm3000DllOfC.dmc_set_home_pin_logic({cardNo}, {channelId}, {eLevel}, 0) returns {result}.");
                return false;
            }

            strValue = setting[EnumInitialSetting.HOMING_DIRECTION];
            ushort homeDirection = Convert.ToUInt16(strValue);
            strValue = setting[EnumInitialSetting.HOMING_VELOCITY];
            ushort homeVelocity = Convert.ToUInt16(strValue);
            strValue = setting[EnumInitialSetting.HOMING_MODE];
            ushort homeMode = Convert.ToUInt16(strValue);
            ushort reservedValueOfEzCount = 0;
            //dmc_set_homemode(), see user guide chinese version P62
            result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_homemode(cardNo, channelId, homeDirection, homeVelocity, homeMode, reservedValueOfEzCount);
            if (result != 0) {
                LOGGER.Error($"Setting Leisai Dm3000 homing mode is failed.");
                LOGGER.Error($"ImportingLeisaiDm3000DllOfC.dmc_set_homemode({cardNo}, {channelId}, {homeDirection}, {homeVelocity}, {homeMode}, {reservedValueOfEzCount}) returns {result}.");
                return false;
            }

            strValue = setting[EnumInitialSetting.EL_SIGNAL_ENABLED_STATUS];
            ushort elEnableStatus = Convert.ToUInt16(strValue);
            strValue = setting[EnumInitialSetting.EL_SIGNAL_VALID_ELECTRICAL_LEVEL];
            ushort elElectricalLevel = Convert.ToUInt16(strValue);
            ushort elMode = 0; //brake mode of EL，0 means immediately stop when arrive positive or negative limit 
            //dmc_set_el_mode(), see user guide chinese version P129
            result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_el_mode(cardNo, channelId, elEnableStatus, elElectricalLevel, elMode);
            if (result != 0) {
                LOGGER.Error($"Setting Leisai Dm3000 el mode is failed.");
                LOGGER.Error($"ImportingLeisaiDm3000DllOfC.dmc_set_el_mode({cardNo}, {channelId}, {elEnableStatus}, {elElectricalLevel}, 0) returns {result}.");
                return false;
            }

            strValue = setting[EnumInitialSetting.IS_LIMIT_REVERSE];
            bool isLimitReverse = Convert.ToBoolean(strValue);
            if (isLimitReverse) {
                //dmc_set_el_mode(), see user guide chinese version P102
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_axis_io_map(cardNo, channelId, 0, 1, channelId, 0.01);
                if (result != 0) {
                    LOGGER.Error($"Setting Leisai Dm3000 axis io map is failed.");
                    LOGGER.Error($"ImportingLeisaiDm3000DllOfC.dmc_set_axis_io_map({cardNo}, {channelId}, 0, 1, {channelId}, 0.01) returns {result}.");
                    return false;
                }
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_axis_io_map(cardNo, channelId, 1, 0, channelId, 0.01);
                if (result != 0) {
                    LOGGER.Error($"Setting Leisai Dm3000 axis io map is failed.");
                    LOGGER.Error($"ImportingLeisaiDm3000DllOfC.dmc_set_axis_io_map({cardNo}, {channelId}, 1, 0, {channelId}, 0.01) returns {result}.");
                    return false;
                }
            } else {
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_axis_io_map(cardNo, channelId, 0, 0, channelId, 0.01);
                if (result != 0) {
                    LOGGER.Error($"Setting Leisai Dm3000 axis io map is failed.");
                    LOGGER.Error($"ImportingLeisaiDm3000DllOfC.dmc_set_axis_io_map({cardNo}, {channelId}, 0, 0, {channelId}, 0.01) returns {result}.");
                    return false;
                }
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_axis_io_map(cardNo, channelId, 1, 1, channelId, 0.01);
                if (result != 0) {
                    LOGGER.Error($"Setting Leisai Dm3000 axis io map is failed.");
                    LOGGER.Error($"ImportingLeisaiDm3000DllOfC.dmc_set_axis_io_map({cardNo}, {channelId}, 1, 1, {channelId}, 0.01) returns {result}.");
                    return false;
                }
            }

            strValue = setting[EnumInitialSetting.S_TIME];
            ushort reservedSMode = 0;
            double stime = Convert.ToDouble(strValue);
            //dmc_set_s_profile(), see user guide chinese version P138
            result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_s_profile(cardNo, channelId, reservedSMode, stime);
            if (result != 0) {
                LOGGER.Error($"Setting Leisai Dm3000 stime is failed.");
                LOGGER.Error($"ImportingLeisaiDm3000DllOfC.dmc_set_s_profile({cardNo}, {channelId}, 0, {stime}) returns {result}.");
                return false;
            }

            strValue = setting[EnumInitialSetting.MINIMUM_VELOCITY];
            double minVelocity = Convert.ToDouble(strValue);
            strValue = setting[EnumInitialSetting.MAXIMUM_VELOCITY];
            double maxVelocity = Convert.ToDouble(strValue);
            strValue = setting[EnumInitialSetting.ACCELERATED_VELOCITY];
            double acceleratedVelocity = Convert.ToDouble(strValue);
            double deceleratedVelocity = acceleratedVelocity;
            double stopVelocity = minVelocity;
            //dmc_set_profile(), see user guide chinese version P137
            result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_profile(cardNo, channelId, minVelocity, maxVelocity, acceleratedVelocity, deceleratedVelocity, stopVelocity);
            if (result != 0) {
                LOGGER.Error($"Setting Leisai Dm3000 channel's all kinds of speed is failed.");
                LOGGER.Error($"ImportingLeisaiDm3000DllOfC.dmc_set_profile({cardNo}, {channelId}, {minVelocity}, {maxVelocity}, {acceleratedVelocity}, {deceleratedVelocity}, {stopVelocity}) returns {result}.");
                return false;
            }

            strValue = setting[EnumInitialSetting.DISTANCE_PER_PULSE];
            double distancePerPulse = Convert.ToDouble(strValue);
            var distancePerPulseAtCard = distancePerPulseConfig[cardNo];
            if (distancePerPulseAtCard.ContainsKey(channelId)) {
                distancePerPulseAtCard[channelId] = distancePerPulse;
            } else {
                distancePerPulseAtCard.Add(channelId, distancePerPulse);
            }

            return true;
        }

        #endregion

        private readonly Dictionary<ushort, HashSet<ushort>> boardChannelDict = new Dictionary<ushort, HashSet<ushort>>();
        private readonly Dictionary<ushort, Dictionary<ushort, object>> cardChannelMutexes = new Dictionary<ushort, Dictionary<ushort, object>>();
        private readonly Dictionary<ushort, Dictionary<ushort, double>> distancePerPulseConfig = new Dictionary<ushort, Dictionary<ushort, double>>();
        private WrapperOfLeisaiDmc3000Api wrapperOfLeisaiDm3000Api;

        private string triggeringBoardAddress;
        private string triggeringChannelId;
        private List<double> triggeringData;
        private int relMoveDisPulse = 0;

        private double moveStep = 0;

        internal const ushort MAX_AXIS_CHANNEL_ID = 11;
        internal const ushort MAX_BOARD_ID = 7;
        internal const ushort MAX_IO_CHANNEL_ID = 16;

        public enum EnumInitialSetting {
            PULSE_OUT_MODE,
            ELECTRICAL_LEVEL_OF_ORIGIN,
            HOMING_MODE,
            HOMING_DIRECTION,
            HOMING_VELOCITY,
            EL_SIGNAL_ENABLED_STATUS,
            EL_SIGNAL_VALID_ELECTRICAL_LEVEL,
            IS_LIMIT_REVERSE,
            S_TIME,

            DISTANCE_PER_PULSE,
            MINIMUM_VELOCITY,
            MAXIMUM_VELOCITY,
            ACCELERATED_VELOCITY
        }
    }
}
