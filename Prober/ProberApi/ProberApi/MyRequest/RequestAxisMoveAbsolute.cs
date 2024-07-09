using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using CommonApi.MyConstant;
using CommonApi.MyEnum;
using CommonApi.MyUtility;

using MyMotionStageDriver.MyStageAxis;

using ProberApi.MyConstant;

namespace ProberApi.MyRequest {
    public sealed class RequestAxisMoveAbsolute : AbstractRequest {
        public RequestAxisMoveAbsolute(ConcurrentDictionary<string, object> sharedObjects) : base(sharedObjects) {
            RequestType = CommonRequestType.AXIS_MOVE_ABS;
            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            this.stageAxisUsages = tempObj as Dictionary<string, StageAxis>;
        }

        public override (int responseId, string runResult, object attachedData) Run() {
            lockSet.Clear();
            lockSet.Add(stageAxis.TransactionLock);

            criticalSection.Enter();
            try {
                stageAxis.MoveAbsolute(newPosition);
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

            string errorText = string.Empty;
            string[] parts = parameters.Trim().Split(',');
            if (parts.Length != 2) {
                errorText = $"{INVALID_PARAMETERS} --- parameter list should be: axisUsageId,absPosition";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            string axisUsageId = parts[0].Trim();
            string strNewPosition = parts[1].Trim();

            if (!axisUsageIdValidChecker.Check(axisUsageId)) {
                errorText = $"{INVALID_PARAMETERS} --- axisUsageId(={axisUsageId}) does not exist!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }
            this.stageAxis = stageAxisUsages[axisUsageId];

            if (!double.TryParse(strNewPosition, out this.newPosition)) {
                errorText = $"{INVALID_PARAMETERS} --- newPosition(={strNewPosition}) is not a valid absolute position!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            return (true, string.Empty);
        }

        public override AbstractRequest DeepCopyDefaultInstance() {
            RequestAxisMoveAbsolute result = new RequestAxisMoveAbsolute(sharedObjects);
            return result;            
        }

        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        private StageAxis stageAxis;
        private double newPosition;
    }
}
