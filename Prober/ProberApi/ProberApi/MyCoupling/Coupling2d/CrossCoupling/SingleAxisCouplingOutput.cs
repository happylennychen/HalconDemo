using System.Collections.Generic;
 
namespace ProberApi.MyCoupling.Coupling2d.CrossCoupling {
    public sealed class SingleAxisCouplingOutput {
        public static SingleAxisCouplingOutput FactoryFail(string stageId, string axisId) {
            return new SingleAxisCouplingOutput(false, stageId, axisId, null, null, -1,0,0,0,0,0);
        }

        public static SingleAxisCouplingOutput FactorySuccess(string stageId, string axisId, List<double> axisPositionList, List<double> feedbackList, int peakIndex, double curvePeakPower, double readPeakPower,double weightPeakPower,double curvePeakPos, double weightPeakPos) {
            return new SingleAxisCouplingOutput(true, stageId, axisId, axisPositionList, feedbackList, peakIndex, curvePeakPower, readPeakPower,weightPeakPower,curvePeakPos,weightPeakPos);
        }

        public bool IsOk { get; }
        public string StageId { get; }
        public string AxisId { get; }
        public List<double> AxisPositionList { get; }
        public List<double> FeedbackList { get; }
        public int PeakIndex { get; }
        public double curvePeakPower { get; set; }  
        public double readPeakPower {  get; set; }
        public double weightPeakPower { get; set; }
        public double curvePeakPos { get; set; }
        public double weightPeakPos {  get; set; }  

        private SingleAxisCouplingOutput(bool isOk, string stageId, string axisId, List<double> axisPositionList, List<double> feedbackList, int peakIndex, double curvePeakPower,double readPeakPower,double weightPeakPower,double curvePeakPos, double weightPeakPos) {
            this.IsOk = isOk;
            this.StageId = stageId;
            this.AxisId = axisId;
            this.AxisPositionList = axisPositionList;
            this.FeedbackList = feedbackList;
            this.PeakIndex = peakIndex;
            this.curvePeakPower = curvePeakPower;
            this.readPeakPower = readPeakPower;
            this.weightPeakPower = weightPeakPower;
            this.curvePeakPos = curvePeakPos;
            this.weightPeakPos = weightPeakPos;
        }
    }
}
