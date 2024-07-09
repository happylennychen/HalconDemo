using System;

using MyMotionStageDriver.MyStageAxis;

namespace ProberApi.MyCoupling.Coupling2d.CrossCoupling.Stepped {
    public sealed class SingleAxisCouplingSteppedInput {
        public static SingleAxisCouplingSteppedInput Factory(StageAxis stageAxis, double motionRange, double step, Func<double> getFeedback, int continuousDeclinePointNumber,bool isWeight, double threshold) {
            return new SingleAxisCouplingSteppedInput(stageAxis, motionRange, step, getFeedback, continuousDeclinePointNumber, isWeight, threshold);
        }

        private SingleAxisCouplingSteppedInput(StageAxis stageAxis, double motionRange, double step, Func<double> getFeedback, int continuousDeclinePointNumber,bool isWeight,double threshold) {
            StageAxis = stageAxis;
            MotionRange = motionRange;
            Step = step;
            GetFeedback = getFeedback;
            ContinuousDeclinePointNumber = continuousDeclinePointNumber;
            IsWeight = isWeight;
            Threshold = threshold;  
        }

        public StageAxis StageAxis { get; }
        public double MotionRange { get; }
        public double Step { get; }
        public Func<double> GetFeedback { get; }
        public int ContinuousDeclinePointNumber { get; }
        public bool IsWeight { get; }

        public double Threshold { get; }
    }
}
