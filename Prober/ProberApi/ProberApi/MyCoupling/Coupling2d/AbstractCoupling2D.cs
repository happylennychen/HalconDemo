using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using CommonApi.MyUtility;

using MyInstruments;
using MyInstruments.MyUtility;

using MyMotionStageDriver.MyStageAxis;

using NLog;

using ProberApi.MyConstant;
using ProberApi.MyCoupling.CouplingParameter;

namespace ProberApi.MyCoupling.Coupling2d {
    public abstract class AbstractCoupling2D {
        public AbstractCouplingParameter CommonParameter { get; private set; }
        public StageAxis FirstAxis { get; private set; }
        public StageAxis SecondAxis { get; private set; }

        public abstract (int responseId, object data) Run();
        public abstract AbstractCoupling2D DeepCopy();

        public AbstractCoupling2D(AbstractCouplingParameter parameter) {
            this.CommonParameter = parameter;
            this.sharedObjects = parameter.SharedObjects;
            parameter.SharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out object tempObj);
            this.instruments = tempObj as Dictionary<string, Instrument>;
            parameter.SharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsages = tempObj as List<InstrumentUsage>;

            List<InstrumentUsage> instrumentUsageList = null;
            if (CommonParameter.EnabledCouplingIn) {
                this.CommonParameter.SharedObjects.TryGetValue(SharedObjectKey.COUPLING_IN_CONFIG, out tempObj);
                Dictionary<string, (string instrumentUsageId, Dictionary<string, string> settings)> couplingInConfigDict = tempObj as Dictionary<string, (string instrumentUsageId, Dictionary<string, string> settings)>;
                this.couplingInConfig = couplingInConfigDict[CommonParameter.CouplingInId];
                instrumentUsageList = instrumentUsages.Where(x => x.UsageId.Equals(couplingInConfig.instrumentUsageId)).ToList();
                this.couplingInInstrumentUsage = instrumentUsageList.First();
                this.couplingInInstrument = instruments[couplingInInstrumentUsage.InstrumentId];                
            } else {
                couplingInInstrument = null;
                funcCouplingInBatchSetting = null;
            }

            CommonParameter.SharedObjects.TryGetValue(SharedObjectKey.COUPLING_FEEDBACK_CONFIG, out tempObj);
            var feedbackConfigDict = tempObj as Dictionary<string, (string instrumentUsageId, Dictionary<string, string> initSettingDict, Dictionary<string, string> triggerInSettingDict)>;
            this.feedbackConfig = feedbackConfigDict[CommonParameter.CouplingFeedbackId];
            instrumentUsageList = instrumentUsages.Where(x => x.UsageId.Equals(feedbackConfig.instrumentUsageId)).ToList();
            couplingFeedbackInstrumentUsage = instrumentUsageList.First();
            couplingFeedbackInstrument = instruments[couplingFeedbackInstrumentUsage.InstrumentId];
        }

        public void SetAxises(StageAxis firstAxis, StageAxis secondAxis) {
            this.FirstAxis = firstAxis;
            this.SecondAxis = secondAxis;
        }

        public bool Prepare() {            
            if (CommonParameter.EnabledCouplingIn) {
                CouplingInUtility couplingInUtility = new CouplingInUtility(couplingInInstrumentUsage, couplingInConfig.settings);
                var runResult1 = couplingInUtility.Run();
                if (!runResult1.isOk) {
                    return false;
                }
                funcCouplingInBatchSetting = runResult1.funcCouplingInBatchSetting;
            } 
                        
            CouplingFeedbackUtility feedbackUtility = new CouplingFeedbackUtility(couplingFeedbackInstrumentUsage);
            var getResult1 = feedbackUtility.GetFuncGetFeedback();
            if (!getResult1.isOk) {
                return false;
            }
            funcGetFeedback = getResult1.funcGetFeedback;
            var getResult2 = feedbackUtility.GetFuncBatchSetting(feedbackConfig.initSettingDict);
            funcFeedbackBatchSetting = getResult2.funcBatchSetting;

            if (CommonParameter.ShowGui) {
                EnabledGui();
            }

            return true;
        }

        public virtual void EnabledGui() {
            sharedObjects.TryGetValue(SharedObjectKey.COUPLING_DISABLED_FEEDBACK_MONITORING, out object tempObj);
            this.actDisableMonitoringFeedback = tempObj as Action;
            sharedObjects.TryGetValue(SharedObjectKey.COUPLING_RESTORE_FEEDBACK_MONITORING, out tempObj);
            this.actRestoreMonitoringFeedback = tempObj as Action;
        }

        protected readonly ConcurrentDictionary<string, object> sharedObjects;
        protected readonly Dictionary<string, Instrument> instruments;
        protected readonly List<InstrumentUsage> instrumentUsages;
        protected readonly (string instrumentUsageId, Dictionary<string, string> settings) couplingInConfig;
        protected readonly InstrumentUsage couplingInInstrumentUsage;
        protected readonly Instrument couplingInInstrument;

        protected readonly (string instrumentUsageId, Dictionary<string, string> initSettingDict, Dictionary<string, string> triggerInSettingDict) feedbackConfig;
        protected readonly InstrumentUsage couplingFeedbackInstrumentUsage;
        protected readonly Instrument couplingFeedbackInstrument;
                       
        protected Func<bool> funcCouplingInBatchSetting;
        protected Func<double> funcGetFeedback;
        protected Func<bool> funcFeedbackBatchSetting;
        protected Action actDisableMonitoringFeedback;
        protected Action actRestoreMonitoringFeedback;

        protected AutoResetEvent chartLock;
        protected static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
