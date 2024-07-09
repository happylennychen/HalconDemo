using System;
using System.Collections.Generic;
using System.Threading;

using CommonApi.MyEnum;
using CommonApi.MyUtility;

using ProberApi.MyConstant;
using ProberApi.MyCoupling.CouplingChart;
using ProberApi.MyCoupling.CouplingParameter;
using ProberApi.MyUtility;

namespace ProberApi.MyCoupling.Coupling2d.CrossCoupling.Stepped {
    public sealed class CrossCoupling2DStepped : AbstractCoupling2D {
        public CrossCoupling2dParameter MyParameter { get; private set; }

        public CrossCoupling2DStepped(CrossCoupling2dParameter parameter, int continuousDeclinePointNumber) : base(parameter) {
            this.MyParameter = parameter;
            this.continuousDeclinePointNumber = continuousDeclinePointNumber;
        }

        public override (int responseId, object data) Run() {
            (SingleAxisCouplingOutput firstAxisCoarseOutput
            , SingleAxisCouplingOutput secondAxisCoarseOutput
            , bool enabledRefineTraveling
            , SingleAxisCouplingOutput firstAxisRefineOutput
            , SingleAxisCouplingOutput secondAxisRefineOutput) result;
            try {
                result = Execute();
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return ((int)EnumResponseId.OCCURRED_EXCEPTION, null);
            }
            return ((int)EnumResponseId.PASS, result);
        }

        private (SingleAxisCouplingOutput firstAxisCoarseOutput
            , SingleAxisCouplingOutput secondAxisCoarseOutput
            , bool enabledRefineTraveling
            , SingleAxisCouplingOutput firstAxisRefineOutput
            , SingleAxisCouplingOutput secondAxisRefineOutput) Execute() {

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
                this.actChartBegining?.Invoke((-1 * MyParameter.CoarseMotionRange / 2).ToString());

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

                string firstSeriesLegendText = "1st_axis";
                string secondSeriesLegendText = "2nd_axis";
                string thirdSeriesLegendText = "";
                string fourthSeriesLegendText = "";
                if (MyParameter.EnabledRefinedTraveling) {
                    firstSeriesLegendText = "1st_axis_coarse";
                    secondSeriesLegendText = "2nd_axis_coarse";
                    thirdSeriesLegendText = "1st_axis_refined";
                    fourthSeriesLegendText = "2nd_axis_refined";
                }

                double beginPosition1stAxis = FirstAxis.Position();
                double beginPosition2ndAxis = SecondAxis.Position();
                SingleAxisCouplingSteppedInput firstAxisCoarseCouplingInput = SingleAxisCouplingSteppedInput.Factory(FirstAxis, MyParameter.CoarseMotionRange, MyParameter.CoarseStep, this.funcGetFeedback, this.continuousDeclinePointNumber,MyParameter.DealwithWeight, MyParameter.Threshold);
                SingleAxisCouplingStepped firstAxisCoarseCoupling = new SingleAxisCouplingStepped(firstAxisCoarseCouplingInput);
                firstAxisCoarseCoupling.AttatchGuiAction(actAxisChartBegin, actAxisChartAddAPoint, actAxisChartPeak, actAxisChartContinuousDeclineFloor);
                var firstAxisCoarseCouplingOutput = firstAxisCoarseCoupling.Run(firstSeriesLegendText,false,true);
                firstAxisCoarseCoupling.GoToPeak();
                FirstAxis.GuiUpdatePosition();
                Thread.Sleep(10);
                firstAxisCoarseCouplingOutput.readPeakPower = funcGetFeedback();

                SingleAxisCouplingSteppedInput secondAxisCoarseCouplingInput = SingleAxisCouplingSteppedInput.Factory(SecondAxis, MyParameter.CoarseMotionRange, MyParameter.CoarseStep, this.funcGetFeedback, this.continuousDeclinePointNumber, MyParameter.DealwithWeight, MyParameter.Threshold);
                SingleAxisCouplingStepped secondAxisCoarseCoupling = new SingleAxisCouplingStepped(secondAxisCoarseCouplingInput);
                secondAxisCoarseCoupling.AttatchGuiAction(actAxisChartBegin, actAxisChartAddAPoint, actAxisChartPeak, actAxisChartContinuousDeclineFloor);

                bool isShowPeak = false;
                if (!MyParameter.EnabledRefinedTraveling) {
                    isShowPeak = true;
                }
                var secondAxisCoarseCouplingOutput = secondAxisCoarseCoupling.Run(secondSeriesLegendText, isShowPeak,true);
                secondAxisCoarseCoupling.GoToPeak();
                SecondAxis.GuiUpdatePosition();
                Thread.Sleep(10);
                secondAxisCoarseCouplingOutput.readPeakPower = funcGetFeedback();

                SingleAxisCouplingStepped secondAxisRefineCoupling = null;
                SingleAxisCouplingOutput firstAxisRefineCouplingOutput = null;
                SingleAxisCouplingOutput secondAxisRefineCouplingOutput = null;
                if (MyParameter.EnabledRefinedTraveling) {                    
                    SingleAxisCouplingSteppedInput firstAxisRefineCouplingInput = SingleAxisCouplingSteppedInput.Factory(FirstAxis, MyParameter.RefinedMotionRange, MyParameter.RefinedStep, this.funcGetFeedback, this.continuousDeclinePointNumber, MyParameter.DealwithWeight, MyParameter.Threshold);
                    SingleAxisCouplingStepped firstAxisRefineCoupling = new SingleAxisCouplingStepped(firstAxisRefineCouplingInput);
                    firstAxisRefineCoupling.AttatchGuiAction(actAxisChartBegin, actAxisChartAddAPoint, actAxisChartPeak, actAxisChartContinuousDeclineFloor);
                    firstAxisRefineCouplingOutput = firstAxisRefineCoupling.Run(thirdSeriesLegendText,false,true);
                    firstAxisRefineCoupling.GoToPeak();
                    FirstAxis.GuiUpdatePosition();
                    Thread.Sleep(10);
                    firstAxisRefineCouplingOutput.readPeakPower = funcGetFeedback();

                    SingleAxisCouplingSteppedInput secondAxisRefineCouplingInput = SingleAxisCouplingSteppedInput.Factory(SecondAxis, MyParameter.RefinedMotionRange, MyParameter.RefinedStep, this.funcGetFeedback, this.continuousDeclinePointNumber, MyParameter.DealwithWeight, MyParameter.Threshold);
                    secondAxisRefineCoupling = new SingleAxisCouplingStepped(secondAxisRefineCouplingInput);
                    secondAxisRefineCoupling.AttatchGuiAction(actAxisChartBegin, actAxisChartAddAPoint, actAxisChartPeak, actAxisChartContinuousDeclineFloor);
                    secondAxisRefineCouplingOutput = secondAxisRefineCoupling.Run(fourthSeriesLegendText,true,true);
                    secondAxisRefineCoupling.GoToPeak();
                    SecondAxis.GuiUpdatePosition();
                    Thread.Sleep(10);
                    secondAxisRefineCouplingOutput.readPeakPower = funcGetFeedback();
                }

                return (firstAxisCoarseCouplingOutput
                , secondAxisCoarseCouplingOutput
                , MyParameter.EnabledRefinedTraveling
                , firstAxisRefineCouplingOutput
                , secondAxisRefineCouplingOutput);
            } finally {
                cs.Leave();
                actRestoreMonitoringFeedback?.Invoke();
            }
        }

        public override AbstractCoupling2D DeepCopy() {
            AbstractCouplingParameter copyParameter = this.MyParameter.DeepCopy();
            CrossCoupling2dParameter parameter = copyParameter as CrossCoupling2dParameter;
            CrossCoupling2DStepped result = new CrossCoupling2DStepped(parameter, this.continuousDeclinePointNumber);
            result.funcCouplingInBatchSetting = this.funcCouplingInBatchSetting;
            result.funcFeedbackBatchSetting = this.funcFeedbackBatchSetting;
            result.funcGetFeedback = this.funcGetFeedback;
            result.actDisableMonitoringFeedback = this.actDisableMonitoringFeedback;
            result.actRestoreMonitoringFeedback = this.actRestoreMonitoringFeedback;
            result.chartLock = this.chartLock;

            result.actChartBegining = this.actChartBegining;
            result.actAxisChartBegin = this.actAxisChartBegin;
            result.actAxisChartAddAPoint = this.actAxisChartAddAPoint;
            result.actAxisChartPeak = this.actAxisChartPeak;
            result.actAxisChartContinuousDeclineFloor = this.actAxisChartContinuousDeclineFloor;

            return result;
        }

        public override void EnabledGui() {
            base.EnabledGui();

            sharedObjects.TryGetValue(SharedObjectKey.CROSS_COUPLING_2D_STEPPED_CHART, out object tempObj);
            CrossCoupling2dSteppedChart coupling2dChart = tempObj as CrossCoupling2dSteppedChart;
            this.actChartBegining = coupling2dChart.BeginCoupling;
            this.actAxisChartBegin = coupling2dChart.AxisCouplingBegin;
            this.actAxisChartAddAPoint = coupling2dChart.AxisCouplingAddAPoint;
            this.actAxisChartPeak = coupling2dChart.AxisCouplingPeak;
            this.actAxisChartContinuousDeclineFloor = coupling2dChart.AxisCouplingContinuousDeclineFloor;
            this.chartLock = coupling2dChart.ChartLock;
        }

        private readonly int continuousDeclinePointNumber;
        private Action<string> actChartBegining;
        private Action<string> actAxisChartBegin;
        private Action<double, double> actAxisChartAddAPoint;
        private Action<bool> actAxisChartPeak;
        private Action<int> actAxisChartContinuousDeclineFloor;
    }
}
