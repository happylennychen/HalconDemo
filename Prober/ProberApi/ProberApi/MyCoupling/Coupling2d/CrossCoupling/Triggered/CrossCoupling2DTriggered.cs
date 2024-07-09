using System;
using System.Collections.Generic;
using System.Threading;

using CommonApi.MyEnum;
using CommonApi.MyTrigger;
using CommonApi.MyUtility;

using ProberApi.MyConstant;
using ProberApi.MyCoupling.CouplingChart;
using ProberApi.MyCoupling.CouplingParameter;
using ProberApi.MyUtility;

using static MyMotionStageDriver.MyMotionController.AbstractMotionController;

namespace ProberApi.MyCoupling.Coupling2d.CrossCoupling.Triggered {
    public sealed class CrossCoupling2DTriggered : AbstractCoupling2D {
        public CrossCoupling2dParameter MyParameter { get; private set; }

        public CrossCoupling2DTriggered(CrossCoupling2dParameter parameter, Dictionary<string, string> overwritingTriggerInSettings) : base(parameter) {
            this.MyParameter = parameter;
            foreach (var one in overwritingTriggerInSettings) {
                this.allTriggerInSettings.Add(one.Key, one.Value);
            }
        }

        public override (int responseId, object data) Run() {
            bool backSixPoint = false;
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
                actChartBegining?.Invoke((-1 * MyParameter.CoarseMotionRange / 2).ToString());
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

                object tempObj = FirstAxis.MotionController;
                ITriggerOut triggerOut = tempObj as ITriggerOut;
                tempObj = couplingFeedbackInstrument;
                ITriggerIn triggerIn = tempObj as ITriggerIn;

                System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                watch.Start();

                double startPosition = (FirstAxis.Position() - MyParameter.CoarseMotionRange / 2 - MyParameter.CoarseStep);
                int stepNumber = CrossCouplingUtility.GetStepNumber(MyParameter.CoarseMotionRange, MyParameter.CoarseStep);
                Dictionary<string, string> triggerOutSettings = new Dictionary<string, string> {
                    { EnumTriggerMotionController.BOARD_ADDRESS.ToString(), FirstAxis.MotionControllerBoardAddress },
                    { EnumTriggerMotionController.CHANNEL_ID.ToString(), FirstAxis.MotionControllerChannelId },
                    { EnumTriggerOutSetting.STEP.ToString(), MyParameter.CoarseStep.ToString("0.00") },
                    { EnumTriggerOutSetting.START_POSITION.ToString(), startPosition.ToString("0.00") },
                    { EnumTriggerCommonSetting.STEP_NUMBER.ToString(), stepNumber.ToString() }
                };
                Dictionary<string, string> triggerInSettings = new Dictionary<string, string>();
                foreach (var one in allTriggerInSettings) {
                    triggerInSettings.Add(one.Key, one.Value);
                }
                triggerInSettings.Add(EnumTriggerInstrument.SLOT.ToString(), couplingFeedbackInstrumentUsage.Slot);
                triggerInSettings.Add(EnumTriggerInstrument.CHANNEL.ToString(), couplingFeedbackInstrumentUsage.Channel);
                triggerInSettings.Add(EnumTriggerCommonSetting.STEP_NUMBER.ToString(), stepNumber.ToString());

                SingleAxisCouplingTriggeredInput input = new SingleAxisCouplingTriggeredInput(
                    FirstAxis,
                    triggerOut,
                    triggerOutSettings,
                    triggerIn,
                    triggerInSettings,
                    MyParameter.CoarseMotionRange,
                    MyParameter.CoarseStep,
                    MyParameter.DealwithWeight,
                    MyParameter.Threshold
                    );
                SingleAxisCouplingTriggered coupling = new SingleAxisCouplingTriggered(input);
                coupling.AttatchGuiAction(actAxisChartBegin, actAxisAddAllPoints, actAxisChartEnded);
                SingleAxisCouplingOutput output1stAxisCoarse = coupling.Run("1st_axis_coarse",false);
                if (!output1stAxisCoarse.IsOk) {
                    //[gyh]: 如果失败，应中止！！！
                }
                double beforegotopeak = watch.ElapsedMilliseconds / 1000.0;
                coupling.GoToPeak(backSixPoint,true,false);
                Thread.Sleep(10);
                output1stAxisCoarse.readPeakPower = funcGetFeedback();
                double firstRunTime = watch.ElapsedMilliseconds / 1000.0 - beforegotopeak;
                FirstAxis.GuiUpdatePosition();
                double updateGUITime = watch.ElapsedMilliseconds / 1000.0 - firstRunTime - beforegotopeak;

                System.Diagnostics.Stopwatch watch2 = new System.Diagnostics.Stopwatch();
                watch2.Start();

                startPosition = (SecondAxis.Position() - MyParameter.CoarseMotionRange / 2 - MyParameter.CoarseStep);
                triggerOutSettings = new Dictionary<string, string> {
                    { EnumTriggerMotionController.BOARD_ADDRESS.ToString(), SecondAxis.MotionControllerBoardAddress },
                    { EnumTriggerMotionController.CHANNEL_ID.ToString(), SecondAxis.MotionControllerChannelId },
                    { EnumTriggerOutSetting.STEP.ToString(), MyParameter.CoarseStep.ToString("0.00") },
                    { EnumTriggerOutSetting.START_POSITION.ToString(), startPosition.ToString("0.00") },
                    { EnumTriggerCommonSetting.STEP_NUMBER.ToString(), stepNumber.ToString() }
                };
                input = new SingleAxisCouplingTriggeredInput(
                    SecondAxis,
                    triggerOut,
                    triggerOutSettings,
                    triggerIn,
                    triggerInSettings,
                    MyParameter.CoarseMotionRange,
                    MyParameter.CoarseStep,
                    MyParameter.DealwithWeight,
                    MyParameter.Threshold
                    );
                coupling = new SingleAxisCouplingTriggered(input);

                bool isShowPeak = false;
                if (!MyParameter.EnabledRefinedTraveling) {
                    isShowPeak = true;
                }
                coupling.AttatchGuiAction(actAxisChartBegin, actAxisAddAllPoints, actAxisChartEnded);
                SingleAxisCouplingOutput output2ndAxisCoarse = coupling.Run("2nd_axis_coarse", isShowPeak);
                if (!output2ndAxisCoarse.IsOk) {
                    //[gyh]: 如果失败，应中止！！！
                }
                coupling.GoToPeak(backSixPoint,false,false);
                Thread.Sleep(10);
                output2ndAxisCoarse.readPeakPower = funcGetFeedback();
                double  sendRunTime = watch.ElapsedMilliseconds / 1000.0;
                SecondAxis.GuiUpdatePosition();
                double updateGUITime2 = watch.ElapsedMilliseconds / 1000.0 - firstRunTime;

                backSixPoint = true;

                SingleAxisCouplingOutput output1stAxisRefined = null;
                SingleAxisCouplingOutput output2ndAxisRefined = null;
                if (MyParameter.EnabledRefinedTraveling) {
                    startPosition = (FirstAxis.Position() - MyParameter.RefinedMotionRange / 2 - MyParameter.RefinedStep);
                    stepNumber = CrossCouplingUtility.GetStepNumber(MyParameter.RefinedMotionRange, MyParameter.RefinedStep);
                    triggerOutSettings = new Dictionary<string, string> {
                        { EnumTriggerMotionController.BOARD_ADDRESS.ToString(), FirstAxis.MotionControllerBoardAddress },
                        { EnumTriggerMotionController.CHANNEL_ID.ToString(), FirstAxis.MotionControllerChannelId },
                        { EnumTriggerOutSetting.STEP.ToString(), MyParameter.RefinedStep.ToString("0.00") },
                        { EnumTriggerOutSetting.START_POSITION.ToString(), startPosition.ToString("0.00") },
                        { EnumTriggerCommonSetting.STEP_NUMBER.ToString(), stepNumber.ToString() }
                    };
                    input = new SingleAxisCouplingTriggeredInput(
                    FirstAxis,
                    triggerOut,
                    triggerOutSettings,
                    triggerIn,
                    triggerInSettings,
                    MyParameter.RefinedMotionRange,
                    MyParameter.RefinedStep,
                    MyParameter.DealwithWeight,
                    MyParameter.Threshold
                    );
                    coupling = new SingleAxisCouplingTriggered(input);
                    coupling.AttatchGuiAction(actAxisChartBegin, actAxisAddAllPoints, actAxisChartEnded);
                    output1stAxisRefined = coupling.Run("1st_axis_refined", false,true);
                    if (!output1stAxisRefined.IsOk) {
                        //[gyh]: 如果失败，应中止！！！
                    }
                    coupling.GoToPeak(backSixPoint,true,true);
                    Thread.Sleep(10);
                    output1stAxisRefined.readPeakPower = funcGetFeedback();
                    FirstAxis.GuiUpdatePosition();

                    startPosition = (SecondAxis.Position() - MyParameter.RefinedMotionRange / 2 - MyParameter.RefinedStep);
                    triggerOutSettings = new Dictionary<string, string> {
                        { EnumTriggerMotionController.BOARD_ADDRESS.ToString(), SecondAxis.MotionControllerBoardAddress },
                        { EnumTriggerMotionController.CHANNEL_ID.ToString(), SecondAxis.MotionControllerChannelId },
                        { EnumTriggerOutSetting.STEP.ToString(), MyParameter.RefinedStep.ToString("0.00") },
                        { EnumTriggerOutSetting.START_POSITION.ToString(), startPosition.ToString("0.00") },
                        { EnumTriggerCommonSetting.STEP_NUMBER.ToString(), stepNumber.ToString() }
                    };
                    input = new SingleAxisCouplingTriggeredInput(
                    SecondAxis,
                    triggerOut,
                    triggerOutSettings,
                    triggerIn,
                    triggerInSettings,
                    MyParameter.RefinedMotionRange,
                    MyParameter.RefinedStep,
                    MyParameter.DealwithWeight,
                    MyParameter.Threshold
                    );
                    coupling = new SingleAxisCouplingTriggered(input);
                    coupling.AttatchGuiAction(actAxisChartBegin, actAxisAddAllPoints, actAxisChartEnded);
                    output2ndAxisRefined = coupling.Run("2nd_axis_refined",true,true);
                    if (!output2ndAxisRefined.IsOk) {
                        //[gyh]: 如果失败，应中止！！！
                    }
                    coupling.GoToPeak(backSixPoint,false,true);
                    Thread.Sleep(10);
                    output2ndAxisRefined.readPeakPower = funcGetFeedback();
                    SecondAxis.GuiUpdatePosition();
                }

                (SingleAxisCouplingOutput firstAxisCoarseOutput
            , SingleAxisCouplingOutput secondAxisCoarseOutput
            , bool enabledRefineTraveling
            , SingleAxisCouplingOutput firstAxisRefineOutput
            , SingleAxisCouplingOutput secondAxisRefineOutput) data =
            (output1stAxisCoarse,
             output2ndAxisCoarse,
             MyParameter.EnabledRefinedTraveling,
             output1stAxisRefined,
             output2ndAxisRefined
            );
                return ((int)EnumResponseId.PASS, data);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return ((int)EnumResponseId.OCCURRED_EXCEPTION, null);
            } finally {
                cs.Leave();
                actRestoreMonitoringFeedback?.Invoke();
            }
        }

        public override AbstractCoupling2D DeepCopy() {
            AbstractCouplingParameter copyParameter = this.MyParameter.DeepCopy();
            CrossCoupling2dParameter parameter = copyParameter as CrossCoupling2dParameter;
            CrossCoupling2DTriggered result = new CrossCoupling2DTriggered(parameter, allTriggerInSettings);

            result.funcCouplingInBatchSetting = this.funcCouplingInBatchSetting;
            result.funcFeedbackBatchSetting = this.funcFeedbackBatchSetting;
            result.funcGetFeedback = this.funcGetFeedback;
            result.actDisableMonitoringFeedback = this.actDisableMonitoringFeedback;
            result.actRestoreMonitoringFeedback = this.actRestoreMonitoringFeedback;
            result.chartLock = this.chartLock;

            result.actChartBegining = this.actChartBegining;
            result.actAxisChartBegin = this.actAxisChartBegin;
            result.actAxisAddAllPoints = this.actAxisAddAllPoints;
            result.actAxisChartEnded = this.actAxisChartEnded;

            result.triggerOutSettings = this.triggerOutSettings;

            return result;
        }

        public override void EnabledGui() {
            base.EnabledGui();

            sharedObjects.TryGetValue(SharedObjectKey.CROSS_COUPLING_2D_TRIGGERED_CHART, out object tempObj);
            CrossCoupling2dTriggeredChart coupling2dChart = tempObj as CrossCoupling2dTriggeredChart;
            this.actChartBegining = coupling2dChart.BeginCoupling;
            this.actAxisChartBegin = coupling2dChart.AxisCouplingBegin;
            this.actAxisAddAllPoints = coupling2dChart.AxisCouplingAddAllPoints;
            this.actAxisChartEnded = coupling2dChart.AxisCouplingPeak;
            this.chartLock = coupling2dChart.ChartLock;
        }

        private readonly Dictionary<string, string> allTriggerInSettings = new Dictionary<string, string>();
        private Dictionary<string, string> triggerOutSettings;
        private Action<string> actChartBegining;
        private Action<string> actAxisChartBegin;
        private Action<List<(double x, double feedback)>> actAxisAddAllPoints;
        private Action<bool> actAxisChartEnded;
    }
}
