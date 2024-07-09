using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using CommonApi.MyConstant;
using CommonApi.MyEnum;
using CommonApi.MyUtility;

using MyMotionStageDriver.MyStageAxis;

using ProberApi.MyConstant;

namespace ProberApi.MyRequest {
    public sealed class RequestAxisMoveRelative : AbstractRequest {
        public RequestAxisMoveRelative(ConcurrentDictionary<string, object> sharedObjects) : base(sharedObjects) {
            RequestType = CommonRequestType.AXIS_MOVE_REL;
            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            this.stageAxisUsages = tempObj as Dictionary<string, StageAxis>;
        }

        public override (int responseId, string runResult, object attachedData) Run() {
            lockSet.Clear();
            lockSet.Add(stageAxis.TransactionLock);

            criticalSection.Enter();
            try {
                stageAxis.MoveRelative(distance);
                stageAxis.GuiUpdatePosition();
                return ((int)EnumResponseId.PASS, string.Empty, null);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return ((int)EnumResponseId.OCCURRED_EXCEPTION, ex.Message, null);
            } finally {
                criticalSection.Leave();
            }            
        }

        public override (bool isOk, string errorMessage) TryUpdateParameters(string parameters) {
            string INVALID_PARAMETERS = $"input parameters(={parameters}) is invalid";

            string[] parts = parameters.Trim().Split(',');
            string errorText = string.Empty;
            if (parts.Length != 2) {
                errorText = $"{INVALID_PARAMETERS} --- parameter list should be: axisUsageId,distance!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            string axisUsageId = parts[0].Trim();
            string strDistance = parts[1].Trim();

            if (!axisUsageIdValidChecker.Check(axisUsageId)) {
                errorText = $"{INVALID_PARAMETERS} --- axisUsageId(={axisUsageId}) does not exist!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }
            this.stageAxis = stageAxisUsages[axisUsageId];

            if (!double.TryParse(strDistance, out distance)) {
                errorText = $"{INVALID_PARAMETERS} --- distance(={strDistance}) is not a valid distance!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            return (true, string.Empty);
        }

        public override AbstractRequest DeepCopyDefaultInstance() {
            RequestAxisMoveRelative result = new RequestAxisMoveRelative(sharedObjects);
            return result;            
        }

        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        private StageAxis stageAxis;
        private double distance;
    }
}
