using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommonApi.MyTrigger;
using CommonApi.MyUtility;

using NLog;

namespace ProberApi.MyCoupling.Coupling2d.CrossCoupling.Triggered {
    public sealed class SingleAxisCouplingTriggered {
        public SingleAxisCouplingTriggered(SingleAxisCouplingTriggeredInput input) {
            this.input = input;
            IsWeight = input.IsWeight;
        }

        public void AttatchGuiAction(Action<string> actAxisChartBegin, Action<List<(double x, double feedback)>> actAxisAddAllPoints, Action<bool> actGuiEnded) {
            this.actAxisChartBegin = actAxisChartBegin;
            this.actAxisAddAllPoints = actAxisAddAllPoints;
            this.actGuiEnded = actGuiEnded;
        }

        public SingleAxisCouplingOutput Run(string chartSeriesLegendText,bool isShowPeak,bool isFineSweep = false) {
            actAxisChartBegin?.Invoke(chartSeriesLegendText);

            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            
            //input.StageAxis.SetAxisSpeed(5000, 20000, 0.01);
            input.StageAxis.SetAxisSpeedEx(0, 20000, 0.01);
            //input.StageAxis.MoveRelative(relMoveDistance);
            //input.StageAxis.OvercomeBacklash(1,4);
            double curvePeakPower = 0;;
            double curvePeakPos = 0;
            double weightPeakPower = 0;
            double weightPeakPos = 0;

            SingleAxisCouplingOutput result = null;
            for (int k = 0; k < 3; k++) {

                //粗耦合不做反向间隙处理                
                double relMoveDistance = -1.0 * input.MotionRange / 2 - input.Step;
                double tempMove = relMoveDistance;

                if (isFineSweep) {
                    input.StageAxis.MoveRelative(relMoveDistance - 4);
                    input.StageAxis.MoveRelative(4);
                } else {
                    input.StageAxis.MoveRelative(relMoveDistance);
                }                

                //Thread.Sleep(1000);

                double motionMoveToNegRange = watch.ElapsedMilliseconds / 1000.0;
                double curPos = input.StageAxis.Position();
                input.TriggerOutSettings["START_POSITION"] = curPos.ToString();

                IOneTriggerOneExecuting executing = new DefaultOneTriggerOneExecuting(input.TriggerOut, input.TriggerIn);
                if (!executing.Prepare(input.TriggerOutSettings, input.TriggerInSettings)) {
                    return SingleAxisCouplingOutput.FactoryFail(input.StageAxis.StageId, input.StageAxis.AxisId);
                }

                double PrepareTime = watch.ElapsedMilliseconds / 1000.0 - motionMoveToNegRange;

                executing.Start();

                double trigRunTime = watch.ElapsedMilliseconds / 1000.0 - motionMoveToNegRange - PrepareTime;
                //debug
                double curPos2 = input.StageAxis.Position();

                executing.WaitForCompleted();

                double waitCompTime = watch.ElapsedMilliseconds / 1000.0 - motionMoveToNegRange - PrepareTime - trigRunTime;
                var getResult = executing.GetData();
                if (!getResult.isOk) {
                    return SingleAxisCouplingOutput.FactoryFail(input.StageAxis.StageId, input.StageAxis.AxisId);
                }

                double readDataTime = watch.ElapsedMilliseconds / 1000.0 - motionMoveToNegRange - PrepareTime - trigRunTime - waitCompTime;
                this.positionList = getResult.triggerOutData;
                this.feedbackList = getResult.triggerInData;

                List<(double x, double feedback)> points = new List<(double x, double feedback)>();
                for (int i = 0; i < getResult.triggerOutData.Count; i++) {
                    relMoveDistance += input.Step;
                    points.Add((relMoveDistance, feedbackList[i]));
                }
                actAxisAddAllPoints?.Invoke(points);
                actGuiEnded?.Invoke(isShowPeak);

                int peakIndex = feedbackList.IndexOf(feedbackList.Max());
                curvePeakPower = feedbackList.Max();
                curvePeakPos = positionList[peakIndex];
                weightPeakPos = curvePeakPos;
                weightPeakPower = curvePeakPower;
                peakPosition = curvePeakPos;
                if (isFineSweep && IsWeight) {
                    DealwithData(positionList, feedbackList,3,out weightPeakPos,out weightPeakPower);
                    peakPosition = weightPeakPos;
                }

                result = SingleAxisCouplingOutput.FactorySuccess(
                    input.StageAxis.StageId,
                    input.StageAxis.AxisId,
                    getResult.triggerOutData,
                    getResult.triggerInData,
                    peakIndex,
                    feedbackList.Max(),
                    0,
                    weightPeakPower,
                    curvePeakPos,
                    weightPeakPos
                    );

                if (peakIndex < 3) {
                    input.StageAxis.MoveRelative(tempMove*2);
                    continue;
                }
                if (peakIndex> feedbackList.Count-3) {
                    continue;
                }

                double runEndTime = watch.ElapsedMilliseconds / 1000.0 - motionMoveToNegRange - PrepareTime - trigRunTime - waitCompTime - readDataTime;
                watch.Stop();
                break;
            }
            //debug

            return result;
        }

        private void DealwithData(List<double> posList, List<double> powerList, double threshold, out double peakPos, out double peakPower)
        {
            peakPos = 0;
            peakPower = 0;

            double maxPower = powerList.Max();
            int maxPowerIndex = powerList.IndexOf(maxPower);

            double powerThresh = maxPower - threshold;
            double powerEmpress = 0;
            double powerSum = 0;
            for (int i = 0; i < powerList.Count; i++)
            {
                if (powerList[i] >= powerThresh)
                {
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
            for (int j = 0; j < posList.Count; j++)
            {
                if (Math.Abs(posList[j] - posEmpress) <= minValue)
                {
                    minValue = Math.Abs(posList[j] - posEmpress);
                    indexEmpress = j;
                }
            }

            peakPower = powerList[indexEmpress];
        }

        public void GoToPeak(bool isBackSixPoint = false,bool isXAxis = true,bool isFineSweep = false) {

            double peakPos = 0;
            double backPoint = 3;
            if (isXAxis)  {
                backPoint = 3;
            }

            //peak点后退6个点
            if (isBackSixPoint) {
                double step = 0.1;
                if (peakPosition < positionList[0] + backPoint * step) {
                    peakPos = positionList[0];
                }  else  {
                    peakPos = peakPosition - backPoint * step;
                }
            }  else  {
                //peak点不退
                peakPos = peakPosition;
            }

            //粗耦合不做反向间隙处理
            if (isFineSweep) {
                input.StageAxis.MoveRelative(4);                
            }

            input.StageAxis.MoveAbsolute(peakPos);
        }

        private readonly SingleAxisCouplingTriggeredInput input;
        private Action<string> actAxisChartBegin;
        private Action<List<(double x, double feedback)>> actAxisAddAllPoints;
        private Action<bool> actGuiEnded;
        private readonly bool IsWeight;
        private double peakPosition;

        private List<double> positionList;
        private List<double> feedbackList;
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
