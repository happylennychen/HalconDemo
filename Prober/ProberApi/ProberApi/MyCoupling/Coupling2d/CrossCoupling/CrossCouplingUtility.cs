using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using CommonApi.MyUtility;

using NLog;

namespace ProberApi.MyCoupling.Coupling2d.CrossCoupling {
    public static class CrossCouplingUtility {
        public static int GetStepNumber(double motionRange, double step) {
            //int stepNumber = (int)Math.Ceiling(motionRange / step);
            int stepNumber = (int)(motionRange / step + 1.1);
            if (stepNumber < MINIMUM_STEP_NUMBER) {
                throw new Exception($"Step number(={stepNumber}) is too small in stepped single axis coupling! Minimum step number is {MINIMUM_STEP_NUMBER}.");
            }
            return stepNumber;
        }

        public static bool SaveDebugInfo(string fileNamePrefix, List<(SingleAxisCouplingOutput firstAxisCoarseOutput, SingleAxisCouplingOutput secondAxisCoarseOutput, bool enabledRefineTraveling, SingleAxisCouplingOutput firstAxisRefineOutput, SingleAxisCouplingOutput secondAxisRefineOutput)> allCouplingData) {
            string content = string.Empty;
            string title = string.Empty;

            try {
                string shortFileName = $"cross_2d_{DateTime.Now.ToString("yyyyMMdd_HHmmss_ffffff")}.csv";
                string trimFileNamePrefix = fileNamePrefix.Trim();
                if (!string.IsNullOrEmpty(trimFileNamePrefix)) {
                    shortFileName = $"{trimFileNamePrefix}_{shortFileName}";
                }
                string csvPath = $"CouplingRawData/{shortFileName}";

                string logInfo = string.Empty;
                /*
                title = "peakIndex, curvePeakPower, readPeakPower,peakIndex, curvePeakPower, readPeakPower,peakIndex, curvePeakPower, readPeakPower,peakIndex, curvePeakPower, readPeakPower," + Environment.NewLine;
                //填写信息
                foreach (var oneData in allCouplingData) {
                    content += $"{oneData.firstAxisCoarseOutput.PeakIndex}, {oneData.firstAxisCoarseOutput.curvePeakPower},{oneData.firstAxisCoarseOutput.readPeakPower}," +
                        $"{oneData.secondAxisCoarseOutput.PeakIndex}, {oneData.secondAxisCoarseOutput.curvePeakPower},{oneData.secondAxisCoarseOutput.readPeakPower},"; 
                    if (oneData.enabledRefineTraveling) {
                        content += $"{oneData.firstAxisRefineOutput.PeakIndex}, {oneData.firstAxisRefineOutput.curvePeakPower},{oneData.firstAxisRefineOutput.readPeakPower}," +
                        $"{oneData.secondAxisRefineOutput.PeakIndex}, {oneData.secondAxisRefineOutput.curvePeakPower},{oneData.secondAxisRefineOutput.readPeakPower},";
                    }
                }
                */
                title = "peakIndex, curvePeakPos,curvePeakPower, readPeakPower, weightPeakPos, weightPeakPower,peakIndex, curvePeakPos,curvePeakPower, readPeakPower, weightPeakPos, weightPeakPower,peakIndex, curvePeakPos,curvePeakPower, readPeakPower, weightPeakPos, weightPeakPower,peakIndex, curvePeakPos,curvePeakPower, readPeakPower,weightPeakPos, weightPeakPower" + Environment.NewLine;
                //填写信息
                foreach (var oneData in allCouplingData)
                {
                    content += $"{oneData.firstAxisCoarseOutput.PeakIndex}, {oneData.firstAxisCoarseOutput.curvePeakPos}, {oneData.firstAxisCoarseOutput.curvePeakPower},{oneData.firstAxisCoarseOutput.readPeakPower},{oneData.firstAxisCoarseOutput.weightPeakPos},{oneData.firstAxisCoarseOutput.weightPeakPower}," +
                        $"{oneData.secondAxisCoarseOutput.PeakIndex}, {oneData.secondAxisCoarseOutput.curvePeakPos}, {oneData.secondAxisCoarseOutput.curvePeakPower},{oneData.secondAxisCoarseOutput.readPeakPower},{oneData.secondAxisCoarseOutput.weightPeakPos},{oneData.secondAxisCoarseOutput.weightPeakPower},";
                    if (oneData.enabledRefineTraveling)
                    {
                        content += $"{oneData.firstAxisRefineOutput.PeakIndex}, {oneData.firstAxisRefineOutput.curvePeakPos}, {oneData.firstAxisRefineOutput.curvePeakPower},{oneData.firstAxisRefineOutput.readPeakPower},{oneData.firstAxisRefineOutput.weightPeakPos}, {oneData.firstAxisRefineOutput.weightPeakPower}," +
                        $"{oneData.secondAxisRefineOutput.PeakIndex}, {oneData.secondAxisRefineOutput.curvePeakPos}, {oneData.secondAxisRefineOutput.curvePeakPower},{oneData.secondAxisRefineOutput.readPeakPower},{oneData.secondAxisRefineOutput.weightPeakPos}, {oneData.secondAxisRefineOutput.weightPeakPower},";
                    }
                }

                logInfo = shortFileName +","+ content;
                LOGGER.Debug(logInfo);

                File.WriteAllText(csvPath, title + content);
                return true;
            } catch(Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }           
        }

        public static bool SaveRawData(string fileNamePrefix, List<(SingleAxisCouplingOutput firstAxisCoarseOutput, SingleAxisCouplingOutput secondAxisCoarseOutput, bool enabledRefineTraveling, SingleAxisCouplingOutput firstAxisRefineOutput, SingleAxisCouplingOutput secondAxisRefineOutput)> allCouplingData) {
            if (allCouplingData.Count == 0) {
                LOGGER.Info("No cross coupling data needs to be saved!");
                return true;
            }

            SaveDebugInfo(fileNamePrefix, allCouplingData);

            const string POSITION_STRING_FORMAT = "0.00";
            const string FEEDBACJ_STRING_FORMAT = "0.000000000";
            StringBuilder content = new StringBuilder();
            var data = allCouplingData.First();
            int maxDataCount = data.firstAxisCoarseOutput.AxisPositionList.Count;
            if (data.enabledRefineTraveling) {
                maxDataCount = data.firstAxisCoarseOutput.AxisPositionList.Count > data.firstAxisRefineOutput.AxisPositionList.Count ? data.firstAxisCoarseOutput.AxisPositionList.Count : data.firstAxisRefineOutput.AxisPositionList.Count;
            }                

            foreach (var oneData in allCouplingData) {
                content.AppendLine();

                List<string> itemList = null;
                string line = string.Empty;
                if (oneData.enabledRefineTraveling) {
                    itemList = new List<string> {
                    $"1st axis({oneData.firstAxisCoarseOutput.StageId}:{oneData.firstAxisCoarseOutput.AxisId}) coarse",
                    string.Empty,
                    string.Empty,
                    $"2nd axis({oneData.secondAxisCoarseOutput.StageId}:{oneData.secondAxisCoarseOutput.AxisId}) coarse",
                    string.Empty,
                    string.Empty,
                    $"1st axis({oneData.firstAxisRefineOutput.StageId}:{oneData.firstAxisRefineOutput.AxisId}) refined",
                    string.Empty,
                    string.Empty,
                    $"2nd axis({oneData.secondAxisRefineOutput.StageId}:{oneData.secondAxisRefineOutput.AxisId}) refined"
                    };
                    line = string.Join(",", itemList);
                    content.AppendLine(line);

                    itemList = new List<string> {
                    "position",
                    "feedback",
                    string.Empty,
                    "position",
                    "feedback",
                    string.Empty,
                    "position",
                    "feedback",
                    string.Empty,
                    "position",
                    "feedback"
                    };
                    line = string.Join(",", itemList);
                    content.AppendLine(line);
                } else {
                    itemList = new List<string> {
                    $"1st axis({oneData.firstAxisCoarseOutput.StageId}:{oneData.firstAxisCoarseOutput.AxisId}) coarse",
                    string.Empty,
                    string.Empty,
                    $"2nd axis({oneData.secondAxisCoarseOutput.StageId}:{oneData.secondAxisCoarseOutput.AxisId}) coarse",
                    string.Empty,
                    string.Empty,                    
                    };
                    line = string.Join(",", itemList);
                    content.AppendLine(line);

                    itemList = new List<string> {
                    "position",
                    "feedback",
                    string.Empty,
                    "position",
                    "feedback",
                    string.Empty,
                    };
                    line = string.Join(",", itemList);
                    content.AppendLine(line);
                }                               

                for (int i = 0; i < maxDataCount; i++) {
                    string coarsePosition1stAxis = string.Empty;
                    string coarseFeedback1stAxis = string.Empty;
                    if (i < oneData.firstAxisCoarseOutput.AxisPositionList.Count) {
                        coarsePosition1stAxis = oneData.firstAxisCoarseOutput.AxisPositionList[i].ToString(POSITION_STRING_FORMAT);
                        coarseFeedback1stAxis = oneData.firstAxisCoarseOutput.FeedbackList[i].ToString(FEEDBACJ_STRING_FORMAT);
                    }

                    string coarsePosition2ndAxis = string.Empty;
                    string coarseFeedback2ndAxis = string.Empty;
                    if (i < oneData.secondAxisCoarseOutput.AxisPositionList.Count) {
                        coarsePosition2ndAxis = oneData.secondAxisCoarseOutput.AxisPositionList[i].ToString(POSITION_STRING_FORMAT);
                        coarseFeedback2ndAxis = oneData.secondAxisCoarseOutput.FeedbackList[i].ToString(FEEDBACJ_STRING_FORMAT);
                    }

                    string refinedPosition1stAxis = string.Empty;
                    string refinedFeedback1stAxis = string.Empty;
                    string refinedPosition2ndAxis = string.Empty;
                    string refinedFeedback2ndAxis = string.Empty;

                    if (oneData.enabledRefineTraveling) {
                        if (i < oneData.firstAxisRefineOutput.AxisPositionList.Count) {
                            refinedPosition1stAxis = oneData.firstAxisRefineOutput.AxisPositionList[i].ToString(POSITION_STRING_FORMAT);
                            refinedFeedback1stAxis = oneData.firstAxisRefineOutput.FeedbackList[i].ToString(FEEDBACJ_STRING_FORMAT);
                        }

                        if (i < oneData.secondAxisRefineOutput.AxisPositionList.Count) {
                            refinedPosition2ndAxis = oneData.secondAxisRefineOutput.AxisPositionList[i].ToString(POSITION_STRING_FORMAT);
                            refinedFeedback2ndAxis = oneData.secondAxisRefineOutput.FeedbackList[i].ToString(FEEDBACJ_STRING_FORMAT);
                        }
                    }                    

                    itemList = new List<string> {
                    coarsePosition1stAxis,
                    coarseFeedback1stAxis,
                    string.Empty,
                    coarsePosition2ndAxis,
                    coarseFeedback2ndAxis,
                    string.Empty,
                    refinedPosition1stAxis,
                    refinedFeedback1stAxis,
                    string.Empty,
                    refinedPosition2ndAxis,
                    refinedFeedback2ndAxis
                    };

                    line = string.Join(",", itemList);
                    content.AppendLine(line);
                }
            }
           
            string shortFileName = $"cross_2d_{DateTime.Now.ToString("yyyyMMdd_HHmmss_ffffff")}.csv";
            string trimFileNamePrefix = fileNamePrefix.Trim();
            if (!string.IsNullOrEmpty(trimFileNamePrefix)) {
                shortFileName = $"{trimFileNamePrefix}_{shortFileName}";
            }
            string csvPath = $"CouplingRawData/{shortFileName}";
            try {
                StreamWriter streamWriter = new StreamWriter(csvPath, true, Encoding.UTF8);
                using (streamWriter) {
                    streamWriter.WriteLine(content.ToString());
                }

                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        public const int MINIMUM_STEP_NUMBER = 5;
    }
}
