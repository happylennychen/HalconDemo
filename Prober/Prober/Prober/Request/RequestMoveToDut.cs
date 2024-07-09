using CommonApi.MyEnum;
using CommonApi.MyUtility;
using MyMotionStageDriver.MyStageAxis;
using Prober.Constant;
using Prober.WaferDef;
using ProberApi.MyConstant;
using ProberApi.MyRequest;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Prober.Request {
    internal class RequestMoveToDut : AbstractRequest {
        public RequestMoveToDut(ConcurrentDictionary<string, object> sharedObjects) : base(sharedObjects) {
            RequestType = PrivateRequestType.MOVE_TO_DUT;
            sharedObjects.TryGetValue(PrivateSharedObjectKey.SUBDIE_POS, out object tempObj);
            this.posList = tempObj as List<ItemCalPosInfo>;
            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_HANDLE, out tempObj);
            this.waferHandle = tempObj as WaferManual;
            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out tempObj);
            stageAxisUsages = tempObj as Dictionary<string, StageAxis>;

            //获取轴
            GetStageAxisDic();
        }

        public override (int responseId, string runResult, object attachedData) Run() {
            lockSet.Clear();
            lockSet.Add(stageAxisDic[MyStageAxisKey.CHUCK_Z].TransactionLock);
            lockSet.Add(stageAxisDic[MyStageAxisKey.CHUCK_X].TransactionLock);
            lockSet.Add(stageAxisDic[MyStageAxisKey.CHUCK_Y].TransactionLock);
            lockSet.Add(stageAxisDic[MyStageAxisKey.LEFT_X].TransactionLock);
            lockSet.Add(stageAxisDic[MyStageAxisKey.LEFT_Y].TransactionLock);
            lockSet.Add(stageAxisDic[MyStageAxisKey.CCD_Z].TransactionLock);            
            lockSet.Add(stageAxisDic[MyStageAxisKey.RIGHT_X].TransactionLock);
            lockSet.Add(stageAxisDic[MyStageAxisKey.RIGHT_Y].TransactionLock);

            criticalSection.Enter();

            try
            {
                if (waferHandle.MotionState != State.Ready)  {
                    return ((int)EnumResponseId.FAIL, "Stage Busy", null);
                }
                
                if (waferHandle.MoveToDut(subDiePos, out string errInfo)) {
                    waferHandle.SetSelectedDie(subDiePos.Die.RowIndex, subDiePos.Die.ColumnIndex);
                    waferHandle.SetDieHighLight(subDiePos.Die);
                    return ((int)EnumResponseId.PASS, string.Empty, null);
                } else {
                    return ((int)EnumResponseId.FAIL, errInfo, null);
                }
            }
            catch(Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return ((int)EnumResponseId.OCCURRED_EXCEPTION, ex.Message, null);
            } finally {
                waferHandle.MotionState = State.Ready;
                criticalSection.Leave();
            }
        }

        public override (bool isOk, string errorMessage) TryUpdateParameters(string parameters) {
            string INVALID_PARAMETERS = $"call RequestMoveToDut.TryUpdateParameters({parameters})";

            string[] parts = parameters.Trim().Split(',');
            string errorText = string.Empty;
            if (parts.Length != 2)  {
                errorText = $"{INVALID_PARAMETERS} --- parameter list should be: reticleName,subdieName!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            string reticleName = parts[0].Trim();
            string subDieName = parts[1].Trim();

            if (posList == null) {
                errorText = $"{INVALID_PARAMETERS} --- positionList does not exist!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            var itemCalPos = posList.FirstOrDefault(t => t.SubDieName == subDieName && t.DieOrdinate == reticleName);
            if (itemCalPos == null) {
                errorText = $"{INVALID_PARAMETERS} --- position(={reticleName} {subDieName}) does not exist!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            this.subDiePos = itemCalPos;
            this.reticleName = reticleName;
            this.subDieName = subDieName;

            return (true, string.Empty);                      
        }       

        public override AbstractRequest DeepCopyDefaultInstance() {
            RequestMoveToDut result = new RequestMoveToDut(sharedObjects);
            return result;
        }

        public bool GetStageAxisDic() {
            List<string> axisUseList = new List<string>();
            axisUseList.Add(MyStageAxisKey.LEFT_X);
            axisUseList.Add(MyStageAxisKey.LEFT_Y);
            axisUseList.Add(MyStageAxisKey.LEFT_Z);
            axisUseList.Add(MyStageAxisKey.LEFT_SX);
            axisUseList.Add(MyStageAxisKey.LEFT_SY);
            axisUseList.Add(MyStageAxisKey.LEFT_SZ);

            axisUseList.Add(MyStageAxisKey.RIGHT_X);
            axisUseList.Add(MyStageAxisKey.RIGHT_Y);
            axisUseList.Add(MyStageAxisKey.RIGHT_Z);
            axisUseList.Add(MyStageAxisKey.RIGHT_SX);
            axisUseList.Add(MyStageAxisKey.RIGHT_SY);
            axisUseList.Add(MyStageAxisKey.RIGHT_SZ);

            axisUseList.Add(MyStageAxisKey.CCD_X);
            axisUseList.Add(MyStageAxisKey.CCD_Y);
            axisUseList.Add(MyStageAxisKey.CCD_Z);
            axisUseList.Add(MyStageAxisKey.HEIGHT_U);

            axisUseList.Add(MyStageAxisKey.CHUCK_X);
            axisUseList.Add(MyStageAxisKey.CHUCK_Y);
            axisUseList.Add(MyStageAxisKey.CHUCK_Z);
            axisUseList.Add(MyStageAxisKey.CHUCK_SZ);

            for (int i = 0; i < axisUseList.Count; i++) {
                var result = GetStageAxis(axisUseList[i]);
                if (!result.isOK) {
                    return false;
                }
                stageAxisDic.Add(axisUseList[i], result.stageAxis);
            }

            return true;
        }

        internal (bool isOK, string errorMessage, StageAxis stageAxis) GetStageAxis(string axisUsageId) {
            string errorText = string.Empty;
            string INVALID_PARAMETERS = $"input parameters(={axisUsageId}) is invalid";

            if (!stageAxisUsages.ContainsKey(axisUsageId)) {
                errorText = $"{INVALID_PARAMETERS}";
                LOGGER.Error(errorText);
                return (false, errorText, null);
            }

            return (true, null, stageAxisUsages[axisUsageId]);
        }

        private List<ItemCalPosInfo> posList = null;
        private ItemCalPosInfo subDiePos = null;
        private WaferManual waferHandle = null;
        private string reticleName = string.Empty;
        private string subDieName = string.Empty;
        public readonly Dictionary<string, StageAxis> stageAxisDic = new Dictionary<string, StageAxis>();
        private readonly Dictionary<string, StageAxis> stageAxisUsages;
    }
}
