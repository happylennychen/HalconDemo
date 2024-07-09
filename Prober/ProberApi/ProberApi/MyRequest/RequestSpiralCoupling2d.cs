using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using CommonApi.MyConstant;
using CommonApi.MyEnum;
using CommonApi.MyUtility;

using MyMotionStageDriver.MyStageAxis;

using ProberApi.MyConstant;
using ProberApi.MyCoupling.Coupling2d;
using ProberApi.MyCoupling.Coupling2d.SpiralCoupling;
using ProberApi.MyCoupling.CouplingParameter;

namespace ProberApi.MyRequest {
    public sealed class RequestSpiralCoupling2d : AbstractRequest {
        public RequestSpiralCoupling2d(ConcurrentDictionary<string, object> sharedObjects) : base(sharedObjects) {
            RequestType = CommonRequestType.SPIRAL_COUPLING_2D;
            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            this.stageAxisUsageDict = tempObj as Dictionary<string, StageAxis>;
            sharedObjects.TryGetValue(SharedObjectKey.SPIRAL_COUPLING_2D_PARAMETERS, out tempObj);
            this.coupling2dParameterDict = tempObj as Dictionary<string, SpiralCoupling2dParameter>;
        }

        public override (int responseId, string runResult, object attachedData) Run() {
            var allRawData = new List<(string firstAxisName, string secondAxisName, List<(double absPos1stAxis, double absPos2ndAxis, double feedback)> rawData)>();
            try {                
                foreach (SpiralCoupling2D coupling2D in coupling2DList) {
                    var couplingResult = coupling2D.Run();
                    if (couplingResult.responseId != ((int)EnumResponseId.PASS)) {
                        return (couplingResult.responseId, string.Empty, null);
                    }
                    var rawData = couplingResult.data as List<(double absPos1stAxis, double absPos2ndAxis, double feedback)>;
                    string firstStageAxisName = $"{coupling2D.FirstAxis.StageId}-{coupling2D.FirstAxis.AxisId}";
                    string secondStageAxisName = $"{coupling2D.SecondAxis.StageId}-{coupling2D.SecondAxis.AxisId}";
                    allRawData.Add((firstStageAxisName, secondStageAxisName, rawData));
                }

                if (templateCoupling2D.MyParameter.SavingRawData) {
                    if (!SpiralCouplingUtility.SaveRawData(allRawData)) {
                        return ((int)EnumResponseId.FAIL, "Failed to save coupling raw data!", null);
                    }                    
                }

                return ((int)EnumResponseId.PASS, string.Empty, null);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return ((int)EnumResponseId.OCCURRED_EXCEPTION, string.Empty, null);
            } finally {
                coupling2DList.Clear();
            }                       
        }

        public override (bool isOk, string errorMessage) TryUpdateParameters(string parameters) {
            coupling2DList.Clear();
            string INVALID_PARAMETERS = $"Request spiral-coupling-2d parameters(={parameters}) is invalid";

            string[] parts = parameters.Trim().Split(',');
            string errorText = string.Empty;
            if (parts.Length < 3) {
                errorText = $"{INVALID_PARAMETERS} --- number of parameters should be >=3!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            string parameterId = parts[0];
            if (!coupling2dParameterDict.ContainsKey(parameterId)) {
                errorText = $"Spiral coupling 2d paramter id(={parameterId}) does not exist!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }
            SpiralCoupling2dParameter templateDefaultParameter = coupling2dParameterDict[parameterId];

            int axisPairNumber = (parts.Length - 1) / 2;
            List<(StageAxis firstAxis, StageAxis secondAxis)> axisPairs = new List<(StageAxis firstAxis, StageAxis secondAxis)>();
            for (int i = 0; i < axisPairNumber; i++) {
                string firstAxisUsageId = parts[1 + 2 * i];
                string secondAxisUsageId = parts[2 + 2 * i];
                if (!stageAxisUsageDict.ContainsKey(firstAxisUsageId)) {
                    errorText = $"axis usage id(={firstAxisUsageId}) does not exist!";
                    LOGGER.Error(errorText);
                    return (false, errorText);
                }
                if (!stageAxisUsageDict.ContainsKey(secondAxisUsageId)) {
                    errorText = $"axis usage id(={secondAxisUsageId}) does not exist!";
                    LOGGER.Error(errorText);
                    return (false, errorText);
                }

                StageAxis firstAxis = stageAxisUsageDict[firstAxisUsageId];
                StageAxis secondAxis = stageAxisUsageDict[secondAxisUsageId];
                axisPairs.Add((firstAxis, secondAxis));
            }

            AbstractCouplingParameter defaultParameter = templateDefaultParameter.DeepCopy();
            SpiralCoupling2dParameter currentParameter = defaultParameter as SpiralCoupling2dParameter;
            if ((parts.Length > 3) && (parts.Length % 2 == 0)) {
                string strOverwriteDefault = parts[parts.Length - 1].Trim();
                if (!string.IsNullOrEmpty(strOverwriteDefault)) {
                    if (strOverwriteDefault.EndsWith(";")) {
                        strOverwriteDefault = strOverwriteDefault.Substring(0, strOverwriteDefault.Length - 1);
                    }
                    string[] parts2 = strOverwriteDefault.Trim().Split(';');
                    for (int i = 0; i < parts2.Length; i++) {
                        string one = parts2[i].Trim();
                        if (string.IsNullOrEmpty(one)) {
                            continue;
                        }
                        string[] parts3 = one.Split('=');
                        if (parts3.Length != 2) {
                            return (false, $"overwriting default currentParameter:{one} is invalid!");
                        }
                        string key = parts3[0].Trim().ToUpperInvariant();
                        string value = parts3[1].Trim();

                        if (Enum.TryParse(key, out AbstractCouplingParameter.EnumCommonOverwrittenParameterId enumCommonKey)) {
                            switch (enumCommonKey) {
                                case AbstractCouplingParameter.EnumCommonOverwrittenParameterId.SHOW_GUI:
                                    if (!bool.TryParse(value, out bool showGui)) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a boolean!");
                                    }
                                    currentParameter.ShowGui = showGui;
                                    break;
                                case AbstractCouplingParameter.EnumCommonOverwrittenParameterId.SETTING_IN_INSTRUMENT:
                                    if (!bool.TryParse(value, out bool settingInInstrument)) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a boolean!");
                                    }
                                    currentParameter.SettingCouplingInInstrument = settingInInstrument;
                                    break;
                                case AbstractCouplingParameter.EnumCommonOverwrittenParameterId.SETTING_FEEDBACK_INSTRUMENT:
                                    if (!bool.TryParse(value, out bool settingFeedbackInstrument)) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a boolean!");
                                    }
                                    currentParameter.SettingCouplingFeedbackInstrument = settingFeedbackInstrument;
                                    break;
                                case AbstractCouplingParameter.EnumCommonOverwrittenParameterId.SAVING_RAW_DATA:
                                    if (!bool.TryParse(value, out bool savingRawData)) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a boolean!");
                                    }
                                    currentParameter.SavingRawData = savingRawData;
                                    break;
                                case AbstractCouplingParameter.EnumCommonOverwrittenParameterId.DEALWITH_WEIGHT:
                                    if (!bool.TryParse(value, out bool dealwithWeight)) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a boolean!");
                                    }
                                    currentParameter.DealwithWeight = dealwithWeight;
                                    break;
                                case AbstractCouplingParameter.EnumCommonOverwrittenParameterId.THRESHOLD:
                                    if (!double.TryParse(value, out double threshold)) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a double!");
                                    }
                                    currentParameter.Threshold = threshold;
                                    break;
                            }
                        } else if (Enum.TryParse(key, out SpiralCoupling2dParameter.EnumOverwrittenParameterId enumKey)) {
                            switch (enumKey) {
                                case SpiralCoupling2dParameter.EnumOverwrittenParameterId.MOTION_RANGE:
                                    if (!double.TryParse(value, out double motionRange)) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a positive double!");
                                    }
                                    if (motionRange <= 0.0) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a positive double!");
                                    }
                                    currentParameter.MotionRange = motionRange;
                                    break;
                                case SpiralCoupling2dParameter.EnumOverwrittenParameterId.STEP:
                                    if (!double.TryParse(value, out double step)) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a positive double!");
                                    }
                                    if (step <= 0.0) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a positive double!");
                                    }
                                    currentParameter.Step = step;
                                    break;
                                case SpiralCoupling2dParameter.EnumOverwrittenParameterId.FEEDBACK_THRESHOLD:
                                    if (!double.TryParse(value, out double feedbackThreshold)) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a double!");
                                    }
                                    currentParameter.FeedbackThreshold = feedbackThreshold;
                                    break;
                            }
                        } else {
                            return (false, $"overwriting default currentParameter:{key}={value}, key(={key}) is invalid!");
                        }
                    }
                }
            }

            templateCoupling2D = new SpiralCoupling2D(currentParameter);
            if (!templateCoupling2D.Prepare()) {
                return (false, "Preparing for spiral coupling 2D is failed!");
            }

            foreach (var axisPair in axisPairs) {
                AbstractCoupling2D copyCoupling2D = templateCoupling2D.DeepCopy();
                SpiralCoupling2D crossCoupling2D = copyCoupling2D as SpiralCoupling2D;
                crossCoupling2D.SetAxises(axisPair.firstAxis, axisPair.secondAxis);
                coupling2DList.Add(crossCoupling2D);
            }

            return (true, string.Empty);
        }

        public override AbstractRequest DeepCopyDefaultInstance() {
            RequestSpiralCoupling2d result = new RequestSpiralCoupling2d(this.sharedObjects);
            return result;
        }

        private readonly Dictionary<string, StageAxis> stageAxisUsageDict;
        private readonly Dictionary<string, SpiralCoupling2dParameter> coupling2dParameterDict;
        private readonly List<SpiralCoupling2D> coupling2DList = new List<SpiralCoupling2D>();
        private SpiralCoupling2D templateCoupling2D;
    }
}
