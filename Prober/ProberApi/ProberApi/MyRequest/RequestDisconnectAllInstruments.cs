using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using CommonApi.MyConstant;
using CommonApi.MyEnum;
using CommonApi.MyUtility;

using MyInstruments;

using ProberApi.MyBoard;
using ProberApi.MyConstant;

namespace ProberApi.MyRequest {
    public sealed class RequestDisconnectAllInstruments : AbstractRequest {
        public RequestDisconnectAllInstruments(ConcurrentDictionary<string, object> sharedObjects) : base(sharedObjects) {
            RequestType = CommonRequestType.DISCONNECT_ALL_INSTRUMENTS;
                        
            sharedObjects.TryGetValue(SharedObjectKey.RED_GREEN_LIGHT_BOARD, out object tempObj);
            this.redGreenLightBoard = tempObj as RedGreenLightBoard;
            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out tempObj);
            this.instruments = tempObj as Dictionary<string, Instrument>;
        }

        public override AbstractRequest DeepCopyDefaultInstance() {
            RequestDisconnectAllInstruments result = new RequestDisconnectAllInstruments(sharedObjects);
            return result;
        }

        public override (int responseId, string runResult, object attachedData) Run() {
            sharedObjects.TryGetValue(SharedObjectKey.HAS_CONNECTED_INSTRUMENTS, out object tempObj);
            bool hasConnectedInstruments = (bool)tempObj;
            if (!hasConnectedInstruments) {
                return ((int)EnumResponseId.PASS, string.Empty, null);
            }
            
            lockSet.Clear();
            foreach (var one in instruments) {
                Instrument instrument = one.Value;
                lockSet.Add(instrument.TransactionLock);
            }

            criticalSection.Enter();
            try {
                foreach (var one in instruments) {
                    Instrument instrument = one.Value;
                    instrument.Disconnect();
                    Thread.Sleep(TimeSpan.FromMilliseconds(100));
                }

                sharedObjects.AddOrUpdate(SharedObjectKey.HAS_CONNECTED_INSTRUMENTS, false, (key, oldValue) => false);                
                redGreenLightBoard.AddOrUpdateLight(CommonRedGreenLigthKey.CONNECTED_ALL_INSTRUMENT, false);

                return ((int)EnumResponseId.PASS, string.Empty, null);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return ((int)EnumResponseId.OCCURRED_EXCEPTION, ex.Message, null);
            } finally {
                criticalSection.Leave();
            }
        }

        public override (bool isOk, string errorMessage) TryUpdateParameters(string parameters) {
            return (true, string.Empty);
        }
               
        private readonly RedGreenLightBoard redGreenLightBoard;
        private readonly Dictionary<string, Instrument> instruments;
    }
}
