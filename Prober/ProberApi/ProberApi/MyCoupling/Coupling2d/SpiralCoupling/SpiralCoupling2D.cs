using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using CommonApi.MyEnum;
using CommonApi.MyUtility;

using ProberApi.MyConstant;
using ProberApi.MyCoupling.CouplingChart;
using ProberApi.MyCoupling.CouplingParameter;
using ProberApi.MyUtility;

namespace ProberApi.MyCoupling.Coupling2d.SpiralCoupling {
    public sealed class SpiralCoupling2D : AbstractCoupling2D {
        public SpiralCoupling2dParameter MyParameter { get; private set; }

        public SpiralCoupling2D(AbstractCouplingParameter parameter) : base(parameter) {
            this.MyParameter = parameter as SpiralCoupling2dParameter;
        }

        public override (int responseId, object data) Run() {
            HashSet<AutoResetEvent> lockList = new HashSet<AutoResetEvent> {
                FirstAxis.TransactionLock,
                SecondAxis.TransactionLock,
                couplingFeedbackInstrument.TransactionLock
            };
            if (couplingInInstrument != null) {
                lockList.Add(couplingInInstrument.TransactionLock);
            }
            if (chartLock != null) {
                lockList.Add(chartLock);
            }

            actDisableMonitoringFeedback?.Invoke();
            MyCriticalSection cs = new MyCriticalSection(lockList);
            cs.Enter();
            try {
                var result = Execute();
                if (!result.isOk) {
                    return ((int)EnumResponseId.FAIL, result.couplingData);
                }

                return ((int)EnumResponseId.PASS, result.couplingData);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return ((int)EnumResponseId.OCCURRED_EXCEPTION, null);
            } finally {
                cs.Leave();
                actRestoreMonitoringFeedback?.Invoke();
            }
        }

        private (bool isOk, List<(double absPos1stAxis, double absPos2ndAxis, double feedback)> couplingData) Execute() {
            int totalStepNumber = (int)Math.Round(MyParameter.MotionRange / MyParameter.Step);
            totalStepNumber += 1;

            if (CommonParameter.SettingCouplingInInstrument) {
                if (!funcCouplingInBatchSetting.Invoke()) {
                    //[gyh]: 如果设置失败，应中止！！！
                }
            }
            if (CommonParameter.SettingCouplingFeedbackInstrument) {
                if (!funcFeedbackBatchSetting.Invoke()) {
                    //[gyh]: 如果设置失败，应中止！！！
                }
            }

            double beginAbsPos1stAxis = FirstAxis.Position();
            double beginAbsPos2ndAxis = SecondAxis.Position();
            actChartBegining?.Invoke("Spiral Coupling 2D");

            MyDirection directionSign = new MyDirection(MyDirection.NEGATIVE_DIRECTION);
            int stepNumberCurrentCycle = 0;
            double totalRelMove1stAxis = 0.0;
            double totalRelMove2ndAxis = 0.0;
            double feedback;
            var rawData = new List<(double absPos1stAxis, double absPos2ndAxis, double feedback)>();
            var feedbackList = new List<double>();
            while (true) {
                if (stepNumberCurrentCycle >= totalStepNumber) {
                    break;
                }

                ++stepNumberCurrentCycle;
                directionSign.Reverse();

                double relMoveDistance = directionSign.GetSign() * MyParameter.Step;
                for (int i = 0; i < stepNumberCurrentCycle; i++) {
                    FirstAxis.MoveRelative(relMoveDistance);
                    totalRelMove1stAxis += relMoveDistance;
                    feedback = funcGetFeedback.Invoke();
                    feedbackList.Add(feedback);
                    rawData.Add((beginAbsPos1stAxis + totalRelMove1stAxis, beginAbsPos2ndAxis + totalRelMove2ndAxis, feedback));
                    actChartAddAPoint?.Invoke(totalRelMove1stAxis, totalRelMove2ndAxis, feedback);
                    if (feedback >= MyParameter.FeedbackThreshold) {
                        actChartEnded?.Invoke();
                        return (true, rawData);
                    }
                }
                FirstAxis.GuiUpdatePosition();
                for (int i = 0; i < stepNumberCurrentCycle; i++) {
                    SecondAxis.MoveRelative(relMoveDistance);
                    totalRelMove2ndAxis += relMoveDistance;
                    feedback = funcGetFeedback.Invoke();
                    feedbackList.Add(feedback);
                    rawData.Add((beginAbsPos1stAxis + totalRelMove1stAxis, beginAbsPos2ndAxis + totalRelMove2ndAxis, feedback));
                    actChartAddAPoint?.Invoke(totalRelMove1stAxis, totalRelMove2ndAxis, feedback);
                    if (feedback >= MyParameter.FeedbackThreshold) {
                        actChartEnded?.Invoke();
                        return (true, rawData);
                    }
                }
                SecondAxis.GuiUpdatePosition();
            }

            int index = feedbackList.IndexOf(feedbackList.Max());
            var peak = rawData[index];            
            FirstAxis.MoveAbsolute(peak.absPos1stAxis);
            FirstAxis.GuiUpdatePosition();
            SecondAxis.MoveAbsolute(peak.absPos2ndAxis);
            SecondAxis.GuiUpdatePosition();

            actChartEnded?.Invoke();
            return (true, rawData);
        }

        public override AbstractCoupling2D DeepCopy() {
            AbstractCouplingParameter copyParameter = this.MyParameter.DeepCopy();
            SpiralCoupling2dParameter parameter = copyParameter as SpiralCoupling2dParameter;
            SpiralCoupling2D result = new SpiralCoupling2D(parameter);

            result.funcCouplingInBatchSetting = this.funcCouplingInBatchSetting;
            result.funcFeedbackBatchSetting = this.funcFeedbackBatchSetting;
            result.funcGetFeedback = this.funcGetFeedback;
            result.actDisableMonitoringFeedback = this.actDisableMonitoringFeedback;
            result.actRestoreMonitoringFeedback = this.actRestoreMonitoringFeedback;
            result.chartLock = this.chartLock;

            result.actChartBegining = this.actChartBegining;
            result.actChartAddAPoint = this.actChartAddAPoint;
            result.actChartEnded = this.actChartEnded;
            
            return result;
        }

        public override void EnabledGui() {
            base.EnabledGui();

            sharedObjects.TryGetValue(SharedObjectKey.SPIRAL_COUPLING_2D_CHART_CONFIG, out object tempObj);
            SpiralCoupling2dChart coupling2DChart = tempObj as SpiralCoupling2dChart;
            this.actChartBegining = coupling2DChart.BeginCoupling;
            this.actChartAddAPoint = coupling2DChart.AddAPoint;
            this.actChartEnded = coupling2DChart.EndCoupling;
            this.chartLock = coupling2DChart.ChartLock;
        }

        private Action<string> actChartBegining;
        private Action<double, double, double> actChartAddAPoint;
        private Action actChartEnded;

        private class MyDirection {
            internal MyDirection(int initialDirection) {
                sign = initialDirection;
            }

            internal int GetSign() {
                return sign;
            }

            internal void Reverse() {
                sign *= -1;
            }

            private int sign;
            internal const int NEGATIVE_DIRECTION = -1;
            internal const int POSITIVE_DIRECTION = 1;
        }
    }
}
