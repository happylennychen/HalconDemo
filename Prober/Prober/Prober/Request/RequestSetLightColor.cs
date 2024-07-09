using CommonApi.MyEnum;
using CommonApi.MyUtility;
using MyMotionStageDriver.MyMotionController.Leisai;
using MyMotionStageDriver.MyStageAxis;
using Prober.WaferDef;
using ProberApi.MyConstant;
using ProberApi.MyRequest;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prober.Request {
    internal class RequestSetLightColor : AbstractRequest {
        public RequestSetLightColor(ConcurrentDictionary<string, object> sharedObjects) : base(sharedObjects) {
            RequestType = PrivateRequestType.SET_LIGHT_COLOR;
            sharedObjects.TryGetValue(SharedObjectKey.LEISAI_DM3000_INSTANCE, out object tempObj);
            motion = tempObj as MotionControllerLeisaiDmc3000;

            sharedObjects.TryGetValue(PrivateSharedObjectKey.IO_INFO, out tempObj);
            IO_Info = tempObj as IOInfo;
        }

        public override (int responseId, string runResult, object attachedData) Run() {
            try {
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Red.ToString(), true);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Green.ToString(), true);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Yellow.ToString(), true);
                if (color == SignalColor.RED.ToString().ToUpper()) {
                    motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Red.ToString(), false);
                }  else if (color == SignalColor.GREEN.ToString().ToUpper()) {
                    motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Green.ToString(), false);
                }  else {
                    motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Yellow.ToString(), false);
                }

                return ((int)EnumResponseId.PASS, string.Empty, null);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return ((int)EnumResponseId.OCCURRED_EXCEPTION, ex.Message, null);
            }             
        }

        public override (bool isOk, string errorMessage) TryUpdateParameters(string parameters) {
            string INVALID_PARAMETERS = $"call RequestMoveToDut.TryUpdateParameters({parameters})";

            string[] parts = parameters.Trim().Split(',');
            string errorText = string.Empty;
            if (parts.Length != 1) {
                errorText = $"{INVALID_PARAMETERS} --- parameter list should be: Red or Green or Yellow!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

             color = parts[0].Trim().ToUpper();

            if (!Enum.TryParse(color,out SignalColor enumColor)) {
                errorText = $"{INVALID_PARAMETERS} --- parameter list should be: Red or Green or Yellow!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            return (true, string.Empty);
        }

        public override AbstractRequest DeepCopyDefaultInstance() {
            RequestSetLightColor result = new RequestSetLightColor(sharedObjects);
            return result;
        }

        private MotionControllerLeisaiDmc3000 motion;
        private string color = string.Empty;
        private IOInfo IO_Info;
    }
}
