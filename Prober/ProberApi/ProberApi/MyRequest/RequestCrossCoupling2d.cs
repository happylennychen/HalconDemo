using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using CommonApi.MyConstant;
using CommonApi.MyEnum;
using CommonApi.MyTrigger;
using CommonApi.MyUtility;

using MyMotionStageDriver.MyStageAxis;

using ProberApi.MyConstant;
using ProberApi.MyCoupling.Coupling2d;
using ProberApi.MyCoupling.Coupling2d.CrossCoupling;
using ProberApi.MyCoupling.Coupling2d.CrossCoupling.Stepped;
using ProberApi.MyCoupling.Coupling2d.CrossCoupling.Triggered;
using ProberApi.MyCoupling.CouplingParameter;
using ProberApi.MyUtility;

namespace ProberApi.MyRequest {
    public sealed class RequestCrossCoupling2d : AbstractRequest {
        public RequestCrossCoupling2d(ConcurrentDictionary<string, object> sharedObjects) : base(sharedObjects) {
            RequestType = CommonRequestType.CROSS_COUPLING_2D;
            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            this.stageAxisUsageDict = tempObj as Dictionary<string, StageAxis>;
            sharedObjects.TryGetValue(SharedObjectKey.CROSS_COUPLING_2D_PARAMETERS, out tempObj);
            var config = ((int continuousDeclinePointNumber, Dictionary<string, CrossCoupling2dParameter> parameterDict))tempObj;
            this.continuousDeclinePointNumber = config.continuousDeclinePointNumber;
            this.coupling2dParameterDict = config.parameterDict;
        }

        public override (int responseId, string runResult, object attachedData) Run() {
            List<(SingleAxisCouplingOutput firstAxisCoarseOutput, SingleAxisCouplingOutput secondAxisCoarseOutput, bool enabledRefineTraveling, SingleAxisCouplingOutput firstAxisRefineOutput, SingleAxisCouplingOutput secondAxisRefineOutput)> allCouplingResults = new List<(SingleAxisCouplingOutput firstAxisCoarseOutput, SingleAxisCouplingOutput secondAxisCoarseOutput, bool enabledRefineTraveling, SingleAxisCouplingOutput firstAxisRefineOutput, SingleAxisCouplingOutput secondAxisRefineOutput)>();
            try {
                foreach (AbstractCoupling2D coupling2D in coupling2DList) {
                    var couplingResult = coupling2D.Run();
                    if (couplingResult.responseId != ((int)EnumResponseId.PASS)) {
                        return (couplingResult.responseId, string.Empty, null);
                    }
                    var couplingData = ((SingleAxisCouplingOutput firstAxisCoarseOutput, SingleAxisCouplingOutput secondAxisCoarseOutput, bool enabledRefineTraveling, SingleAxisCouplingOutput firstAxisRefineOutput, SingleAxisCouplingOutput secondAxisRefineOutput))couplingResult.data;
                    allCouplingResults.Add(couplingData);
                }

                AbstractCoupling2D firstCoupling2D = coupling2DList.First();
                if (firstCoupling2D.CommonParameter.SavingRawData) {
                    if (!CrossCouplingUtility.SaveRawData(firstCoupling2D.CommonParameter.RawDataFileNamePrefix, allCouplingResults)) {
                        return ((int)EnumResponseId.FAIL, "Failed to save coupling raw data!", null);
                    }
                }

                return ((int)EnumResponseId.PASS, string.Empty, null);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return ((int)EnumResponseId.OCCURRED_EXCEPTION, string.Empty, null);
            } finally {
                coupling2DList.Clear();
                templateCrossCoupling2D = null;
            }
        }

        public override (bool isOk, string errorMessage) TryUpdateParameters(string parameters) {
            coupling2DList.Clear();
            string INVALID_PARAMETERS = $"Request cross-coupling-2d parameters(={parameters}) is invalid";

            parameters = parameters.Trim();
            if (parameters.EndsWith(",")) {
                parameters = parameters.Substring(0, parameters.Length - 1);
            }
            string[] parts = parameters.Trim().Split(',');
            string errorText = string.Empty;
            if (parts.Length < 3) {
                errorText = $"{INVALID_PARAMETERS} --- number of parameters should be >=3!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            string parameterId = parts[0].Trim();
            if (!coupling2dParameterDict.ContainsKey(parameterId)) {
                errorText = $"Cross coupling 2d paramter id(={parameterId}) does not exist!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }
            CrossCoupling2dParameter defaultParameter = coupling2dParameterDict[parameterId];

            int axisPairNumber = (parts.Length - 1) / 2;
            List<(StageAxis firstAxis, StageAxis secondAxis)> axisPairs = new List<(StageAxis firstAxis, StageAxis secondAxis)>();
            for (int i = 0; i < axisPairNumber; i++) {
                string firstAxisUsageId = parts[1 + 2 * i].Trim();
                string secondAxisUsageId = parts[2 + 2 * i].Trim();
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

            AbstractCouplingParameter copy = defaultParameter.DeepCopy();
            CrossCoupling2dParameter currentParameter = copy as CrossCoupling2dParameter;

            bool isTriggered = false;
            Dictionary<string, string> overwritingNecessaryTriggerInSettings = new Dictionary<string, string>();
            if ((parts.Length > 3) && (parts.Length % 2 == 0)) {
                string overwritingPart = parts[parts.Length - 1].Trim();
                if (!string.IsNullOrEmpty(overwritingPart)) {
                    if (overwritingPart.EndsWith(";")) {
                        overwritingPart = overwritingPart.Substring(0, overwritingPart.Length - 1);
                    }
                    string[] overwritingArray = overwritingPart.Split(new char[] { ';' });

                    Dictionary<string, string> overwritingItemDict = new Dictionary<string, string>();
                    for (int i = 0; i < overwritingArray.Length; i++) {
                        string one = overwritingArray[i].Trim();
                        string[] keyValuePairs = one.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        if (keyValuePairs.Length != 2) {
                            return (false, $"overwriting parameter item(={one}) is invalid!");
                        }
                        string key = keyValuePairs[0].ToUpperInvariant();
                        string value = keyValuePairs[1];
                        overwritingItemDict.Add(key, value);
                    }

                    if (overwritingItemDict.ContainsKey(EnumTriggerCommonSetting.IS_TRIGGERED.ToString())) {
                        string value = overwritingItemDict[EnumTriggerCommonSetting.IS_TRIGGERED.ToString()];
                        if (!bool.TryParse(value, out isTriggered)) {
                            return (false, $"overwriting parameter item: {EnumTriggerCommonSetting.IS_TRIGGERED.ToString()}(={value}) should be a boolean!");
                        }

                        if (isTriggered) {
                            foreach (var one in currentParameter.NecessaryTriggerInSettings) {
                                overwritingNecessaryTriggerInSettings.Add(one.Key, one.Value);
                            }
                            foreach (var one in overwritingItemDict) {
                                if (overwritingNecessaryTriggerInSettings.ContainsKey(one.Key)) {
                                    overwritingNecessaryTriggerInSettings[one.Key] = one.Value;
                                }
                            }

                            sharedObjects.TryGetValue(SharedObjectKey.COUPLING_FEEDBACK_CONFIG, out object tempObj);
                            var feedbackConfigDict = tempObj as Dictionary<string, (string instrumentUsageId, Dictionary<string, string> initSettingDict, Dictionary<string, string> triggerInSettingDict)>;
                            var feedbackConfig = feedbackConfigDict[currentParameter.CouplingFeedbackId];
                            InternalUtility internalUtility = new InternalUtility(sharedObjects);
                            var getResult = internalUtility.GetInstrumentByUsageId(feedbackConfig.instrumentUsageId);
                            object objInstrument = getResult.instrument;
                            ITriggerIn triggerIn = objInstrument as ITriggerIn;
                            var checkResult = triggerIn.CheckTriggerInSettings(overwritingNecessaryTriggerInSettings);
                            if (!checkResult.isOk) {
                                return (false, checkResult.errorText);
                            }
                        }
                    }

                    foreach (var one in overwritingItemDict) {
                        string key = one.Key;
                        string value = one.Value;

                        if (key.Equals(EnumTriggerCommonSetting.IS_TRIGGERED.ToString())) {
                            continue;
                        }
                        if (overwritingNecessaryTriggerInSettings.ContainsKey(key)) {
                            continue;
                        }

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
                                    if (!bool.TryParse(value,out bool dealwithWeight)) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a boolean!");
                                    }
                                    currentParameter.DealwithWeight = dealwithWeight;
                                    break;
                                case AbstractCouplingParameter.EnumCommonOverwrittenParameterId.THRESHOLD:
                                    if (!double.TryParse(value, out double threshold))
                                    {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a double!");
                                    }
                                    currentParameter.Threshold = threshold;
                                    break;
                                case AbstractCouplingParameter.EnumCommonOverwrittenParameterId.RAW_DATA_FILE_NAME_PREFIX:
                                    currentParameter.RawDataFileNamePrefix = value;
                                    break;
                            }
                        } else if (Enum.TryParse(key, out CrossCoupling2dParameter.EnumOverwrittenParameterId enumCrossKey)) {
                            switch (enumCrossKey) {
                                case CrossCoupling2dParameter.EnumOverwrittenParameterId.COARSE_MOTION_RANGE:
                                    if (!double.TryParse(value, out double coarseMotionRange)) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a positive double!");
                                    }
                                    if (coarseMotionRange <= 0.0) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a positive double!");
                                    }
                                    currentParameter.CoarseMotionRange = coarseMotionRange;
                                    break;
                                case CrossCoupling2dParameter.EnumOverwrittenParameterId.COARSE_STEP:
                                    if (!double.TryParse(value, out double coarseStep)) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a positive double!");
                                    }
                                    if (coarseStep <= 0.0) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a positive double!");
                                    }
                                    currentParameter.CoarseStep = coarseStep;
                                    break;
                                case CrossCoupling2dParameter.EnumOverwrittenParameterId.ENABLED_REFINED_TRAVELING:
                                    if (!bool.TryParse(value, out bool enabledRefinedTraveling)) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a boolean!");
                                    }
                                    currentParameter.EnabledRefinedTraveling = enabledRefinedTraveling;
                                    break;
                                case CrossCoupling2dParameter.EnumOverwrittenParameterId.REFINED_MOTION_RANGE:
                                    if (!double.TryParse(value, out double refinedMotionRange)) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a positive double!");
                                    }
                                    if (refinedMotionRange <= 0.0) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a positive double!");
                                    }
                                    currentParameter.RefinedMotionRange = refinedMotionRange;
                                    break;
                                case CrossCoupling2dParameter.EnumOverwrittenParameterId.REFINED_STEP:
                                    if (!double.TryParse(value, out double refinedStep)) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a positive double!");
                                    }
                                    if (refinedStep <= 0.0) {
                                        return (false, $"overwriting default currentParameter:{key}={value}, value(={value}) is invalid! It should be a positive double!");
                                    }
                                    currentParameter.RefinedStep = refinedStep;
                                    break;
                            }
                        } else {
                            return (false, $"overwriting default currentParameter:{key}={value}, key(={key}) is invalid!");
                        }
                    }
                }
            }

            if (isTriggered) {
                templateCrossCoupling2D = new CrossCoupling2DTriggered(currentParameter, overwritingNecessaryTriggerInSettings);
            } else {
                templateCrossCoupling2D = new CrossCoupling2DStepped(currentParameter, this.continuousDeclinePointNumber);
            }

            if (!templateCrossCoupling2D.Prepare()) {
                return (false, "Preparing for cross coupling 2D is failed!");
            }

            foreach (var axisPair in axisPairs) {
                AbstractCoupling2D copyCoupling2D = templateCrossCoupling2D.DeepCopy();
                copyCoupling2D.SetAxises(axisPair.firstAxis, axisPair.secondAxis);
                coupling2DList.Add(copyCoupling2D);
            }

            return (true, string.Empty);
        }

        public override AbstractRequest DeepCopyDefaultInstance() {
            RequestCrossCoupling2d result = new RequestCrossCoupling2d(sharedObjects);
            return result;
        }

        private readonly Dictionary<string, StageAxis> stageAxisUsageDict;
        private readonly int continuousDeclinePointNumber;
        private readonly Dictionary<string, CrossCoupling2dParameter> coupling2dParameterDict;
        private readonly List<AbstractCoupling2D> coupling2DList = new List<AbstractCoupling2D>();
        private AbstractCoupling2D templateCrossCoupling2D;
    }
}
