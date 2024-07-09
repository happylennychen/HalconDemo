using System.Collections.Concurrent;

namespace ProberApi.MyCoupling.CouplingParameter {
    public sealed class SpiralCoupling2dParameter : AbstractCouplingParameter {
        public SpiralCoupling2dParameter(ConcurrentDictionary<string, object> sharedObjects) : base(sharedObjects) {
        }

        public double MotionRange { get; set; } = 0.0;
        public double Step { get; set; } = 0.0;
        public double FeedbackThreshold { get; set; } = 0.0;

        public override AbstractCouplingParameter DeepCopy() {
            SpiralCoupling2dParameter result = new SpiralCoupling2dParameter(this.SharedObjects);
            result.ShowGui = this.ShowGui;
            result.SettingCouplingInInstrument = this.SettingCouplingInInstrument;
            result.SettingCouplingFeedbackInstrument = this.SettingCouplingFeedbackInstrument;
            result.SavingRawData = this.SavingRawData;
            result.DealwithWeight = this.DealwithWeight; 
            result.Threshold = this.Threshold;

            result.EnabledCouplingIn = this.EnabledCouplingIn;
            result.CouplingInId = this.CouplingInId;
            result.CouplingFeedbackId = this.CouplingFeedbackId;

            result.MotionRange = this.MotionRange;
            result.Step = this.Step;
            result.FeedbackThreshold = this.FeedbackThreshold;

            return result;
        }

        public enum EnumOverwrittenParameterId {
            MOTION_RANGE,          //double
            STEP,                  //double
            FEEDBACK_THRESHOLD     //double            
        }
    }
}
