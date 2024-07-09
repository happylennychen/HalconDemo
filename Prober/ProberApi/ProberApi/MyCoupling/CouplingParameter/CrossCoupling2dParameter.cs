using System.Collections.Concurrent;
using System.Collections.Generic;

using ProberApi.MyConstant;

namespace ProberApi.MyCoupling.CouplingParameter {
    public sealed class CrossCoupling2dParameter : AbstractCouplingParameter {
        public CrossCoupling2dParameter(ConcurrentDictionary<string, object> sharedObjects) : base(sharedObjects) {
        }

        public double CoarseMotionRange { get; set; }
        public double CoarseStep { get; set; }
        public bool EnabledRefinedTraveling { get; set; } = false;
        public double RefinedMotionRange { get; set; } = 0;
        public double RefinedStep { get; set; } = 0;
        public Dictionary<string, string> NecessaryTriggerInSettings { get; private set; }
        public override string CouplingFeedbackId {
            get {
                return base.CouplingFeedbackId;
            }
            set {
                base.CouplingFeedbackId = value;
                if (SharedObjects.TryGetValue(SharedObjectKey.COUPLING_FEEDBACK_CONFIG, out object tempObj)) {
                    var dict = tempObj as Dictionary<string, (string instrumentUsageId, Dictionary<string, string> initSettingDict, Dictionary<string, string> triggerInSettingDict)>;
                    var config = dict[CouplingFeedbackId];
                    NecessaryTriggerInSettings = config.triggerInSettingDict;
                }
            }
        }

        public override AbstractCouplingParameter DeepCopy() {
            CrossCoupling2dParameter result = new CrossCoupling2dParameter(this.SharedObjects);
            result.ShowGui = this.ShowGui;
            result.SettingCouplingInInstrument = this.SettingCouplingInInstrument;
            result.SettingCouplingFeedbackInstrument = this.SettingCouplingFeedbackInstrument;
            result.SavingRawData = this.SavingRawData;
            result.DealwithWeight = this.DealwithWeight;
            result.Threshold = this.Threshold;
            result.RawDataFileNamePrefix = this.RawDataFileNamePrefix;

            result.EnabledCouplingIn = this.EnabledCouplingIn;
            result.CouplingInId = this.CouplingInId;
            result.CouplingFeedbackId = this.CouplingFeedbackId;

            result.CoarseMotionRange = this.CoarseMotionRange;
            result.CoarseStep = this.CoarseStep;
            result.EnabledRefinedTraveling = this.EnabledRefinedTraveling;
            result.RefinedMotionRange = this.RefinedMotionRange;
            result.RefinedStep = this.RefinedStep;
            result.NecessaryTriggerInSettings = new Dictionary<string, string>();
            foreach (var one in this.NecessaryTriggerInSettings) {
                result.NecessaryTriggerInSettings.Add(one.Key, one.Value);
            }

            return result;
        }

        public enum EnumOverwrittenParameterId {
            COARSE_MOTION_RANGE,          //double
            COARSE_STEP,                  //double
            ENABLED_REFINED_TRAVELING,    //bool
            REFINED_MOTION_RANGE,         //double
            REFINED_STEP                  //double            
        }
    }
}
