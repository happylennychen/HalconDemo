using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using CommonApi.MyEnum;
using CommonApi.MyUtility;

using NLog;

using ProberApi.MyConstant;
using ProberApi.MyUtility;

namespace ProberApi.MyRequest {
    public abstract class AbstractRequest {
        public AbstractRequest(ConcurrentDictionary<string, object> sharedObjects) {
            this.sharedObjects = sharedObjects;            
            sharedObjects.TryGetValue(SharedObjectKey.AXIS_USAGE_ID_VALID_CHECKER, out object tempObj);
            axisUsageIdValidChecker = tempObj as AxisUsageIdValidChecker;
            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENT_USAGE_ID_VALID_CHECKER, out tempObj);
            instrumentUsageIdValidChecker = tempObj as InstrumentUsageIdValidChecker;
            lockSet = new HashSet<AutoResetEvent>();
            criticalSection = new MyCriticalSection(lockSet);
            SerialNumber = Guid.NewGuid().ToString().ToUpperInvariant();
            Status = EnumRequestStatus.READY;
        }

        public abstract AbstractRequest DeepCopyDefaultInstance();

        public abstract (int responseId, string runResult, object attachedData) Run();

        public abstract (bool isOk, string errorMessage) TryUpdateParameters(string parameters);
        
        public string RequestType { get; protected set; }

        internal string SerialNumber { get; private set; }
        internal EnumRequestStatus Status { get; set; }

        protected readonly ConcurrentDictionary<string, object> sharedObjects;
        protected readonly AxisUsageIdValidChecker axisUsageIdValidChecker;
        protected readonly InstrumentUsageIdValidChecker instrumentUsageIdValidChecker;
        protected readonly HashSet<AutoResetEvent> lockSet;
        protected readonly MyCriticalSection criticalSection;        
        protected static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);       
    }
}
