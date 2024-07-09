using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using CommonApi.MyConstant;
using CommonApi.MyEnum;
using CommonApi.MyUtility;

using MyMotionStageDriver.MyStageAxis;

using ProberApi.MyConstant;

namespace ProberApi.MyRequest {
    public sealed class RequestAxisSetSpeed : AbstractRequest {
        public RequestAxisSetSpeed(ConcurrentDictionary<string, object> sharedObjects) : base(sharedObjects) {
            RequestType = CommonRequestType.AXIS_SET_SPEED;
            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            stageAxisUsages = tempObj as Dictionary<string, StageAxis>;
        }

        public override AbstractRequest DeepCopyDefaultInstance() {
            return new RequestAxisSetSpeed(this.sharedObjects);
        }

        public override (int responseId, string runResult, object attachedData) Run() {
            StageAxis stageAxis = stageAxisUsages[axisUsageId];

            lockSet.Clear();
            lockSet.Add(stageAxis.TransactionLock);

            criticalSection.Enter();
            try {                
                stageAxis.SetSpeed(speedInPps);
                return ((int)EnumResponseId.PASS, string.Empty, null);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return ((int)EnumResponseId.OCCURRED_EXCEPTION, ex.Message, null);
            } finally {
                criticalSection.Leave();
            }                   
        }

        public override (bool isOk, string errorMessage) TryUpdateParameters(string parameters) {
            string[] parts = parameters.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) {
                return (false, $"{parameters} --- paramter count should be 2!");
            }
            this.axisUsageId = parts[0];
            if (!stageAxisUsages.ContainsKey(this.axisUsageId)) {
                return (false, $"{this.axisUsageId} --- Invalid stage axis usage ID!");
            }

            string strSpeed = parts[1];
            if (!int.TryParse(strSpeed, out speedInPps)) {
                return (false, $"{strSpeed} --- axis speed should be a positive integer!");
            }
            if (speedInPps <= 0) {
                return (false, $"{strSpeed} --- axis speed should be a positive integer!");
            }

            return (true, string.Empty);
        }

        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        private string axisUsageId;
        private int speedInPps;
    }
}
