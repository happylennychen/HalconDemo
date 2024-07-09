using System;
using System.Collections.Generic;
using System.Linq;

using CommonApi.MyUtility;

using ProberApi.MyCoupling.CouplingDataHandling;

namespace ProberApi.MyCoupling.Coupling2d.CrossCoupling.Stepped {
    public sealed class SingleAxisCouplingStepped {
        public List<double> PositionList { get; } = new List<double>();
        public List<double> FeedbackList { get; } = new List<double>();

        public SingleAxisCouplingStepped(SingleAxisCouplingSteppedInput input) {
            this.input = input;
            this.stepNumber = CrossCouplingUtility.GetStepNumber(input.MotionRange, input.Step);
            IsWeight = input.IsWeight;  
        }

        public SingleAxisCouplingOutput Run(string chartSeriesLegendText,bool isShowPeak,bool isFineSweep = false) {
            actGuiBegin?.Invoke(chartSeriesLegendText);
            PositionList.Clear();
            FeedbackList.Clear();

            double curvePeakPos = 0;
            double weightPeakPower = 0;
            double weightPeakPos = 0;

            //设置速度
            input.StageAxis.SetAxisSpeedEx(250, 1000, 0.05);
            int peakIndex = 0;

            for (int k = 0; k < 3; k++) {
                double accumulatedRelMove = -1.0 * input.MotionRange / 2;
                double tempMove = accumulatedRelMove;
                input.StageAxis.MoveRelative(accumulatedRelMove);
                input.StageAxis.OvercomeBacklash();
                double beginPosition = input.StageAxis.Position();
                double currentFeedback = input.GetFeedback();
                PositionList.Add(beginPosition);
                FeedbackList.Add(currentFeedback);
                actGuiAddAPoint?.Invoke(accumulatedRelMove, currentFeedback);

                for (int i = 1; i <= stepNumber; i++) {
                    accumulatedRelMove += input.Step;
                    input.StageAxis.MoveRelative(input.Step);
                    double currentPosition = beginPosition + i * input.Step;
                    currentFeedback = input.GetFeedback();
                    PositionList.Add(currentPosition);
                    FeedbackList.Add(currentFeedback);
                    actGuiAddAPoint?.Invoke(accumulatedRelMove, currentFeedback);

                    if (EnabledContinuousDeclineFeedbackChecking()) {
                        if (FeedbackList.Count >= input.ContinuousDeclinePointNumber) {
                            List<double> checkList = FeedbackList.GetRange(FeedbackList.Count - input.ContinuousDeclinePointNumber, input.ContinuousDeclinePointNumber);
                            if (MyStaticUtility.IsMonotonicDecreasing(checkList)) {
                                actAxisChartContinuousDeclineFloor?.Invoke(FeedbackList.Count - 1);
                                break;
                            }
                        }
                    }
                }
                
                double peakFeedback;
                if (handler != null) {
                    var handleResult = handler.Handle(PositionList, FeedbackList);
                    if (handleResult.isOk) {
                        peakPosition = handleResult.peakAbsolutePosition;
                        PositionList.Add(peakPosition);
                        peakFeedback = input.GetFeedback.Invoke();
                        FeedbackList.Add(peakFeedback);
                        peakIndex = PositionList.Count - 1;
                    } else {
                        peakFeedback = FeedbackList.Max();
                        peakIndex = FeedbackList.IndexOf(FeedbackList.Max());
                        peakPosition = PositionList[peakIndex];
                    }
                } else {
                    if (isFineSweep && IsWeight) {
                        DealwithData(PositionList, FeedbackList,input.Threshold,out weightPeakPos,out weightPeakPower);
                        peakFeedback = FeedbackList.Max();                        
                        peakIndex = FeedbackList.IndexOf(FeedbackList.Max());                        
                        curvePeakPos = PositionList[peakIndex];
                        peakPosition = weightPeakPos;

                    } else {
                        peakFeedback = FeedbackList.Max();
                        peakIndex = FeedbackList.IndexOf(FeedbackList.Max());
                        curvePeakPos = PositionList[peakIndex];
                        peakPosition = PositionList[peakIndex];
                        weightPeakPos = peakPosition;
                        weightPeakPower = peakFeedback;                        
                    }
                }

                if (peakIndex < 3) {
                    input.StageAxis.MoveRelative(tempMove * 2);
                    continue;
                }
                if (peakIndex > FeedbackList.Count - 3) {
                    continue;
                }

                break;
            }            

            //设置速度
            input.StageAxis.SetAxisSpeedEx(500, 1000, 0.05);

            actGuiEnded?.Invoke(isShowPeak);
            return SingleAxisCouplingOutput.FactorySuccess(input.StageAxis.StageId, input.StageAxis.AxisId, PositionList, FeedbackList, peakIndex, FeedbackList.Max(),0,weightPeakPower,curvePeakPos, weightPeakPos);
        }

        private void DealwithData(List<double> posList, List<double> powerList, double threshold, out double peakPos, out double peakPower) {
            peakPos = 0;
            peakPower = 0;

            double maxPower = powerList.Max();
            int maxPowerIndex = powerList.IndexOf(maxPower);

            double powerThresh = maxPower - threshold;
            double powerEmpress = 0;
            double powerSum = 0;
            for (int i = 0; i < powerList.Count; i++)  {
                if (powerList[i] >= powerThresh) {
                    double powerMw = Math.Pow(10.0, powerList[i] / 10.0);
                    powerEmpress += powerMw * posList[i];
                    powerSum += powerMw;
                }
            }

            //加权平均后的最佳位置
            double posEmpress = powerEmpress / powerSum;
            peakPos = posEmpress;

            //查找位置最接近加权平均位置处的光功率值
            double minValue = 9999;
            int indexEmpress = 0;
            for (int j = 0; j < posList.Count; j++) {
                if (Math.Abs(posList[j] - posEmpress) <= minValue) {
                    minValue = Math.Abs(posList[j] - posEmpress);
                    indexEmpress = j;
                }
            }

            peakPower = powerList[indexEmpress];
        }


        public void GoToPeak() {
            /*
            int peakIndex = FeedbackList.IndexOf(FeedbackList.Max());
            double peakPosition = PositionList[peakIndex];
            */
            double gap = 4;
            input.StageAxis.MoveRelative(gap);
            input.StageAxis.MoveAbsolute(peakPosition);
            //input.StageAxis.OvercomeBacklash();
        }

        public void AttatchGuiAction(Action<string> actGuiBegin, Action<double, double> actGuiAddAPoint, Action<bool> actGuiEnded, Action<int> actAxisChartContinuousDeclineFloor) {
            this.actGuiBegin = actGuiBegin;
            this.actGuiAddAPoint = actGuiAddAPoint;
            this.actGuiEnded = actGuiEnded;
            this.actAxisChartContinuousDeclineFloor = actAxisChartContinuousDeclineFloor;
        }

        public void AttachCouplingDataHandling(ISingleAxisCouplingDataHandling handler) {
            this.handler = handler;
        }

        private bool EnabledContinuousDeclineFeedbackChecking() {
            return input.ContinuousDeclinePointNumber > 0;
        }

        private readonly SingleAxisCouplingSteppedInput input;
        private readonly int stepNumber;
        private Action<string> actGuiBegin;
        private Action<double, double> actGuiAddAPoint;
        private Action<bool> actGuiEnded;
        private Action<int> actAxisChartContinuousDeclineFloor;
        private ISingleAxisCouplingDataHandling handler;
        private bool IsWeight;
        private double peakPosition;
    }
}
