using System.Collections.Generic;

using CommonApi.MyTrigger;

using MyMotionStageDriver.MyStageAxis;

namespace ProberApi.MyCoupling.Coupling2d.CrossCoupling.Triggered {
    public sealed class SingleAxisCouplingTriggeredInput {
        public StageAxis StageAxis { get; }
        public ITriggerOut TriggerOut { get; }
        public Dictionary<string, string> TriggerOutSettings { get; }
        public ITriggerIn TriggerIn { get; }
        public Dictionary<string, string> TriggerInSettings { get; }        
        public double MotionRange { get; }
        public double Step { get; }
        public bool IsWeight { get; }
        public double Threshold {  get; }

        public SingleAxisCouplingTriggeredInput(StageAxis stageAxis
            , ITriggerOut triggerOut
            , Dictionary<string, string> triggerOutSettings
            , ITriggerIn triggerIn
            , Dictionary<string, string> triggerInSettings
            , double motionRange
            , double step
            , bool isWeight
            ,double threshold) {
            this.StageAxis = stageAxis;
            this.TriggerOut = triggerOut;
            this.TriggerOutSettings = triggerOutSettings;
            this.TriggerIn = triggerIn;
            this.TriggerInSettings = triggerInSettings;
            this.MotionRange = motionRange;
            this.Step = step;
            this.IsWeight = isWeight;
            this.Threshold = threshold;
        }
    }
}
