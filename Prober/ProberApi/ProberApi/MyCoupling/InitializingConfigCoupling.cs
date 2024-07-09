using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Xml.Linq;

using CommonApi.MyTrigger;
using CommonApi.MyUtility;

using MyInstruments;
using MyInstruments.MyEnum;
using MyInstruments.MyOpm;
using MyInstruments.MySmu;
using MyInstruments.MyTls;
using MyInstruments.MyUtility;

using MyMotionStageDriver.MyStage;

using NLog;

using ProberApi.MyConstant;
using ProberApi.MyCoupling.CouplingParameter;

namespace ProberApi.MyCoupling {
    public sealed class InitializingConfigCoupling {
        public InitializingConfigCoupling(ConcurrentDictionary<string, object> sharedObjects) {
            this.sharedObjects = sharedObjects;
            sharedObjects.TryGetValue(SharedObjectKey.STAGE_LIST, out object tempObj);
            stageList = tempObj as List<Stage>;
            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;
            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            instrumentsUsages = tempObj as List<InstrumentUsage>;
        }

        public bool Run() {
            try {
                XElement xeRoot = XElement.Load(@"Configuration/config_coupling.xml");

                XElement xeCouplingInList = xeRoot.Element("coupling_in_list");
                if (!ReadCouplingIn(xeCouplingInList)) {
                    return false;
                }

                XElement xeCouplingFeedbackList = xeRoot.Element("coupling_feedback_list");
                if (!ReadCouplingFeedback(xeCouplingFeedbackList)) {
                    return false;
                }

                XElement xeFeedbackGuiMonitorings = xeRoot.Element("coupling_feedback_gui_monitorings");
                if (!ReadFeedbackGuiMonitoring(xeFeedbackGuiMonitorings)) {
                    return false;
                }

                XElement xeCoupling2dParameters = xeRoot.Element("coupling_2d_parameters");
                XElement xeCrossCoupling2dParameters = xeCoupling2dParameters.Element("cross_coupling_2d_parameters");
                if (!ReadCrossCoupling2dParameters(xeCrossCoupling2dParameters)) {
                    return false;
                }

                XElement xeSpiralCoupling2dParameters = xeCoupling2dParameters.Element("spiral_coupling_2d_parameters");
                if (!ReadSpiralCoupling2dParameters(xeSpiralCoupling2dParameters)) {
                    return false;
                }

                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        private bool ReadCouplingIn(XElement xeCouplingInList) {
            couplingInConfig.Clear();
            try {
                string strEnabled = xeCouplingInList.Attribute("enabled").Value.Trim();
                if (string.IsNullOrEmpty(strEnabled)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_in_list enabled=>, enabled should not be empty!");
                    return false;
                }
                if (!bool.TryParse(strEnabled, out bool enabled)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_in_list enabled={strEnabled}>, enabled(={strEnabled}) should be a boolean!");
                    return false;
                }
                if (!enabled) {
                    return true;
                }

                List<XElement> xeCouplingIns = xeCouplingInList.Elements("coupling_in").ToList();
                foreach (XElement xeCouplingIn in xeCouplingIns) {
                    string couplingInId = xeCouplingIn.Attribute("id").Value.Trim();
                    if (string.IsNullOrEmpty(couplingInId)) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_in_list><coupling_in id=>, id should not be empty!");
                        return false;
                    }
                    if (couplingInConfig.ContainsKey(couplingInId)) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_in_list><coupling_in id={couplingInId}>, id(={couplingInId}) is duplicated!");
                        return false;
                    }

                    string instrumentUsageId = xeCouplingIn.Attribute("instrument_usage_id").Value.Trim();
                    if (string.IsNullOrEmpty(instrumentUsageId)) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_in_list><coupling_in id={couplingInId} instrument_usage_id=>, instrument_usage_id should not be empty!");
                        return false;
                    }
                    var list = instrumentsUsages.Where(x => x.UsageId.Equals(instrumentUsageId)).ToList();
                    if (list.Count == 0) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_in_list><coupling_in id={couplingInId} instrument_usage_id={instrumentUsageId}>, instrument_usage_id(={instrumentUsageId}) does not exist!");
                        return false;
                    }
                    InstrumentUsage instrumentUsage = list.First();

                    XElement xeInitSettings = xeCouplingIn.Element("initial_settings");
                    List<XElement> xeSettingList = xeInitSettings.Elements("setting").ToList();
                    Dictionary<string, string> settingDict = new Dictionary<string, string>();
                    foreach (var xeSetting in xeSettingList) {
                        string key = xeSetting.Attribute("key").Value.ToUpperInvariant().Trim();
                        string value = xeSetting.Attribute("value").Value.Trim();
                        if (string.IsNullOrEmpty(key)) {
                            LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_in_list><coupling_in id={couplingInId}><initial_settings><setting key=>, key should not be empty!");
                            return false;
                        }
                        if (string.IsNullOrEmpty(value)) {
                            LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_in_list><coupling_in id={couplingInId}><initial_settings><setting key={key} value=>, value should not be empty!");
                            return false;
                        }
                        if (settingDict.ContainsKey(key)) {
                            LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_in_list><coupling_in id={couplingInId}><initial_settings><setting key={key} value={value}>, key(={key}) is duplicated!");
                            return false;
                        }
                        settingDict.Add(key, value);
                    }

                    Instrument instrument = instruments[instrumentUsage.InstrumentId];
                    ITls tls = (ITls)instrument;
                    var checkResult = tls.TlsAreSettingsValid(settingDict);
                    if (!checkResult.isOk) {
                        LOGGER.Error(checkResult.errorText);
                        LOGGER.Error($"Please checking Configuration/config_coupling.xml, <coupling_in_list><coupling_in id={couplingInId}><initial_settings>!");
                        return false;
                    }

                    couplingInConfig.Add(couplingInId, (instrumentUsageId, settingDict));
                }

                sharedObjects.AddOrUpdate(SharedObjectKey.COUPLING_IN_CONFIG, couplingInConfig, (key, oldValue) => couplingInConfig);
                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        private bool ReadCouplingFeedback(XElement xeCouplingFeedbackList) {
            couplingFeedbackConfigDict.Clear();

            try {
                List<XElement> xeCouplingFeedbacks = xeCouplingFeedbackList.Elements("coupling_feedback").ToList();
                foreach (XElement xeCouplingFeedback in xeCouplingFeedbacks) {
                    string couplingFeedbackId = xeCouplingFeedback.Attribute("id").Value.Trim();
                    if (string.IsNullOrEmpty(couplingFeedbackId)) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_feedback_list><coupling_feedback id=>, id should not be empty!");
                        return false;
                    }
                    if (couplingFeedbackConfigDict.ContainsKey(couplingFeedbackId)) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_feedback_list><coupling_feedback id={couplingFeedbackId}>, id(={couplingFeedbackId}) is duplicated!");
                        return false;
                    }

                    string instrumentUsageId = xeCouplingFeedback.Attribute("instrument_usage_id").Value.Trim();
                    if (string.IsNullOrEmpty(instrumentUsageId)) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_feedback_list><coupling_feedback id={couplingFeedbackId} instrument_usage_id=>, instrument_usage_id should not be empty!");
                        return false;
                    }
                    var list = instrumentsUsages.Where(x => x.UsageId.Equals(instrumentUsageId)).ToList();
                    if (list.Count == 0) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_feedback_list><coupling_feedback id={couplingFeedbackId} instrument_usage_id={instrumentUsageId}>, instrument_usage_id(={instrumentUsageId}) does not exist!");
                        return false;
                    }
                    InstrumentUsage instrumentUsage = list.First();

                    XElement xeInitSettings = xeCouplingFeedback.Element("initial_settings");
                    List<XElement> xeSettingList = xeInitSettings.Elements("setting").ToList();
                    Dictionary<string, string> initSettingDict = new Dictionary<string, string>();
                    foreach (var xeSetting in xeSettingList) {
                        string key = xeSetting.Attribute("key").Value.ToUpperInvariant().Trim();
                        string value = xeSetting.Attribute("value").Value.Trim();
                        if (string.IsNullOrEmpty(key)) {
                            LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_feedback_list><coupling_feedback id={couplingFeedbackId}><initial_settings><setting key=>, key should not be empty!");
                            return false;
                        }
                        if (string.IsNullOrEmpty(value)) {
                            LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_feedback_list><coupling_feedback id={couplingFeedbackId}><initial_settings><setting key={key} value=>, value should not be empty!");
                            return false;
                        }
                        if (initSettingDict.ContainsKey(key)) {
                            LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_feedback_list><coupling_feedback id={couplingFeedbackId}><initial_settings><setting key={key} value={value}>, key(={key}) is duplicated!");
                            return false;
                        }

                        initSettingDict.Add(key, value);
                    }
                    Instrument instrument = instruments[instrumentUsage.InstrumentId];
                    (bool isOk, string errorText) checkResult = (false, string.Empty);
                    switch (instrumentUsage.InstrumentCategory) {
                        case EnumInstrumentCategory.OPM:
                            IOpm opm = (IOpm)instrument;
                            checkResult = opm.OpmAreSettingsValid(initSettingDict);
                            break;
                        case EnumInstrumentCategory.SMU:
                            ISmu smu = (ISmu)instrument;
                            checkResult = smu.SmuAreSettingsValid(initSettingDict);
                            break;
                    }
                    if (!checkResult.isOk) {
                        LOGGER.Error(checkResult.errorText);
                        LOGGER.Error($"Please checking Configuration/config_coupling.xml, <coupling_feedback_list><coupling_feedback id={couplingFeedbackId}><initial_settings>!");
                        return false;
                    }

                    Dictionary<string, string> triggerInSettingDict = new Dictionary<string, string>();
                    object tempObj = instrument;
                    if (tempObj is ITriggerIn) { // does instrument support trigger in?                        
                        XElement xeTriggerInSettings = xeCouplingFeedback.Element("trigger_in_settings");
                        xeSettingList = xeTriggerInSettings.Elements("setting").ToList();
                        foreach (var xeSetting in xeSettingList) {
                            string key = xeSetting.Attribute("key").Value.ToUpperInvariant().Trim();
                            string value = xeSetting.Attribute("value").Value.Trim();

                            if (string.IsNullOrEmpty(key)) {
                                LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_feedback_list><coupling_feedback id={couplingFeedbackId}><trigger_in_settings><setting key=>, key should not be empty!");
                                return false;
                            }
                            if (string.IsNullOrEmpty(value)) {
                                LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_feedback_list><coupling_feedback id={couplingFeedbackId}><trigger_in_settings><setting key={key} value=>, value should not be empty!");
                                return false;
                            }
                            if (initSettingDict.ContainsKey(key)) {
                                LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_feedback_list><coupling_feedback id={couplingFeedbackId}><trigger_in_settings><setting key={key} value={value}>, key(={key}) is duplicated!");
                                return false;
                            }
                            triggerInSettingDict.Add(key, value);
                        }

                        ITriggerIn triggerIn = tempObj as ITriggerIn;
                        checkResult = triggerIn.CheckTriggerInSettings(triggerInSettingDict);
                        if (!checkResult.isOk) {
                            LOGGER.Error(checkResult.errorText);
                            LOGGER.Error($"Please checking Configuration/config_coupling.xml, <coupling_feedback_list><coupling_feedback id={couplingFeedbackId}><trigger_in_settings>!");
                            return false;
                        }
                    }

                    couplingFeedbackConfigDict.Add(couplingFeedbackId, (instrumentUsageId, initSettingDict, triggerInSettingDict));
                }

                sharedObjects.AddOrUpdate(SharedObjectKey.COUPLING_FEEDBACK_CONFIG, couplingFeedbackConfigDict, (key, oldValue) => couplingFeedbackConfigDict);
                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        private bool ReadFeedbackGuiMonitoring(XElement xeFeedbackGuiMonitorings) {
            try {
                string strRefreshIntervalInMs = xeFeedbackGuiMonitorings.Attribute("refresh_interval").Value.Trim();
                if (string.IsNullOrEmpty(strRefreshIntervalInMs)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_feedback_gui_monitorings refresh_interval=>, refresh_interval should not be empty!");
                    return false;
                }
                if (!int.TryParse(strRefreshIntervalInMs, out int refreshIntervalInMs)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_feedback_gui_monitorings refresh_interval={strRefreshIntervalInMs}>, refresh_interval(={strRefreshIntervalInMs}) should be a positive integer!");
                    return false;
                }
                if (refreshIntervalInMs <= 0) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_feedback_gui_monitorings refresh_interval={strRefreshIntervalInMs}>, refresh_interval(={strRefreshIntervalInMs}) should be a positive integer!");
                    return false;
                }

                List<XElement> xeGuiMonitoringList = xeFeedbackGuiMonitorings.Elements("gui_monitoring").ToList();
                var config = new Dictionary<string, string>();
                foreach (XElement xeGuiMonitoring in xeGuiMonitoringList) {
                    string couplingFeedbackId = xeGuiMonitoring.Attribute("coupling_feedback_id").Value.Trim();
                    if (string.IsNullOrEmpty(couplingFeedbackId)) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_feedback_gui_monitorings><gui_monitoring coupling_feedback_id=>, coupling_feedback_id should not be empty!");
                        return false;
                    }
                    if (!couplingFeedbackConfigDict.ContainsKey(couplingFeedbackId)) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_feedback_gui_monitorings><gui_monitoring coupling_feedback_id={couplingFeedbackId}>, coupling_feedback_id(={couplingFeedbackId}) does not exist!");
                        return false;
                    }

                    string titleResourceId = xeGuiMonitoring.Attribute("title_resource_id").Value.Trim();
                    if (string.IsNullOrEmpty(titleResourceId)) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_feedback_gui_monitorings><gui_monitoring coupling_feedback_id={couplingFeedbackId} title_resource_id=>, title_resource_id should not be empty!");
                        return false;
                    }

                    config.Add(couplingFeedbackId, titleResourceId);
                }

                (int refreshIntervalInMs, Dictionary<string, string> config) value = (refreshIntervalInMs, config);
                sharedObjects.AddOrUpdate(SharedObjectKey.COUPLING_FEEDBACK_GUI_MONITORING, value, (key, oldValue) => value);
                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        private bool ReadSpiralCoupling2dParameters(XElement xeSpiralCoupling) {
            Dictionary<string, SpiralCoupling2dParameter> parameterDict = new Dictionary<string, SpiralCoupling2dParameter>();
            try {
                List<XElement> xeParameterList = xeSpiralCoupling.Elements("spiral_coupling_2d_parameter").ToList();
                foreach (XElement xeParameter in xeParameterList) {
                    string parameterId = xeParameter.Attribute("id").Value.Trim();
                    if (string.IsNullOrEmpty(parameterId)) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><spiral_coupling_2d_parameters><spiral_coupling_2d_parameter id=>, id should not be empty!");
                        return false;
                    }
                    if (parameterDict.ContainsKey(parameterId)) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><spiral_coupling_2d_parameters><spiral_coupling_2d_parameter id={parameterId}>, id(={parameterId}) is duplicated!");
                        return false;
                    }

                    XElement xeCouplingIn = xeParameter.Element("coupling_in");
                    var readInResult = ReadCouplingInParameters(parameterId, xeCouplingIn, "spiral");
                    if (!readInResult.isOk) {
                        return false;
                    }

                    XElement xeCouplingFeedback = xeParameter.Element("coupling_feedback");
                    var readFeedbackResult = ReadCouplingFeedbackParameters(parameterId, xeCouplingFeedback, "spiral");
                    if (!readFeedbackResult.isOk) {
                        return false;
                    }

                    XElement xeTraveling = xeParameter.Element("traveling");
                    var readTravelingResult = ReadSpiral2dTravelingParameters(parameterId, xeTraveling);
                    if (!readTravelingResult.isOk) {
                        return false;
                    }

                    XElement xeFeedbackThreshold = xeParameter.Element("feedback_threshold");
                    var readFeedbackThresholdResult = ReadSpiral2dFeedbackThreshold(parameterId, xeFeedbackThreshold);
                    if (!readFeedbackThresholdResult.isOk) {
                        return false;
                    }

                    SpiralCoupling2dParameter parameter = new SpiralCoupling2dParameter(sharedObjects);
                    parameter.EnabledCouplingIn = readInResult.parameters.enabledCouplingIn;
                    if (parameter.EnabledCouplingIn) {
                        parameter.CouplingInId = readInResult.parameters.couplingInId;
                    }
                    parameter.CouplingFeedbackId = readFeedbackResult.couplingFeedbackId;
                    parameter.MotionRange = readTravelingResult.config.motionRange;
                    parameter.Step = readTravelingResult.config.step;
                    parameter.FeedbackThreshold = readFeedbackThresholdResult.couplingThreshold;

                    parameterDict.Add(parameterId, parameter);
                }

                sharedObjects.AddOrUpdate(SharedObjectKey.SPIRAL_COUPLING_2D_PARAMETERS, parameterDict, (key, oldValue) => parameterDict);
                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        private (bool isOk, string parameterId, CrossCoupling2dParameter parameter) ReadSingleCrossCoupling2dParameter(XElement xeCouplingCross2dParameter, Dictionary<string, CrossCoupling2dParameter> parameterDict) {
            try {
                string parameterId = xeCouplingCross2dParameter.Attribute("id").Value.Trim();
                if (string.IsNullOrEmpty(parameterId)) {
                    LOGGER.Error($"Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id=>, id should not be empty!");
                    return (false, string.Empty, null);
                }
                if (parameterDict.ContainsKey(parameterId)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id={parameterId}>, id(={parameterId}) is duplicated!");
                    return (false, string.Empty, null);
                }

                XElement xeCouplingIn = xeCouplingCross2dParameter.Element("coupling_in");
                var readCouplingInResult = ReadCouplingInParameters(parameterId, xeCouplingIn, "cross");
                if (!readCouplingInResult.isOk) {
                    return (false, string.Empty, null);
                }

                XElement xeCouplingFeedback = xeCouplingCross2dParameter.Element("coupling_feedback");
                var readCouplingFeedbackResult = ReadCouplingFeedbackParameters(parameterId, xeCouplingFeedback, "cross");
                if (!readCouplingFeedbackResult.isOk) {
                    return (false, string.Empty, null);
                }

                XElement xeTraveling = xeCouplingCross2dParameter.Element("traveling");
                var readTravelingParamterResult = ReadCross2dTravelingParameters(parameterId, xeTraveling);
                if (!readTravelingParamterResult.isOk) {
                    return (false, string.Empty, null);
                }

                CrossCoupling2dParameter parameter = new CrossCoupling2dParameter(sharedObjects);
                parameter.EnabledCouplingIn = readCouplingInResult.parameters.enabledCouplingIn;
                parameter.CouplingInId = readCouplingInResult.parameters.couplingInId;
                parameter.CouplingFeedbackId = readCouplingFeedbackResult.couplingFeedbackId;
                parameter.CoarseMotionRange = readTravelingParamterResult.parameters.coarseMotionRange;
                parameter.CoarseStep = readTravelingParamterResult.parameters.coarseStep;
                parameter.EnabledRefinedTraveling = readTravelingParamterResult.parameters.enabledRefinedTraveling;
                parameter.RefinedMotionRange = readTravelingParamterResult.parameters.refinedMotionRange;
                parameter.RefinedStep = readTravelingParamterResult.parameters.refinedStep;

                return (true, parameterId, parameter);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return (false, string.Empty, null);
            }
        }

        private bool ReadCrossCoupling2dParameters(XElement xeCouplingCross2dParameters) {
            try {
                XElement xeAbortSteppedCouplingWhenContinuousDecline = xeCouplingCross2dParameters.Element("abort_stepped_coupling_when_continuous_decline");
                string strEnabled = xeAbortSteppedCouplingWhenContinuousDecline.Attribute("enabled").Value.Trim();
                if (string.IsNullOrEmpty(strEnabled)) {
                    LOGGER.Error($"Configuration/config_coupling.xml, <coupling_2d_parameters><abort_stepped_coupling_when_continuous_decline enabled=>, enabled should not be empty!");
                    return false;
                }
                if (!bool.TryParse(strEnabled, out bool enabled)) {
                    LOGGER.Error($"Configuration/config_coupling.xml, <coupling_2d_parameters><abort_stepped_coupling_when_continuous_decline enabled={strEnabled}>, enabled(={strEnabled}) should be a boolean!");
                    return false;
                }
                int pointNumber = -1;
                const int MIN_POINT_NUMBER = 3;
                if (enabled) {
                    string strPointNumber = xeAbortSteppedCouplingWhenContinuousDecline.Attribute("point_number").Value.Trim();
                    if (string.IsNullOrEmpty(strPointNumber)) {
                        LOGGER.Error($"Configuration/config_coupling.xml, <coupling_2d_parameters><abort_stepped_coupling_when_continuous_decline enabled={strEnabled} point_number=>, point_number should not be empty!");
                        return false;
                    }
                    if (!int.TryParse(strPointNumber, out pointNumber)) {
                        LOGGER.Error($"Configuration/config_coupling.xml, <coupling_2d_parameters><abort_stepped_coupling_when_continuous_decline enabled={strEnabled} point_number={strPointNumber}>, point_number(={strPointNumber}) should be an integer(>={MIN_POINT_NUMBER})!");
                        return false;
                    }
                    if (pointNumber < MIN_POINT_NUMBER) {
                        LOGGER.Error($"Configuration/config_coupling.xml, <coupling_2d_parameters><abort_stepped_coupling_when_continuous_decline enabled={strEnabled} point_number={strPointNumber}>, point_number(={strPointNumber}) should be an integer(>={MIN_POINT_NUMBER})!");
                        return false;
                    }
                }

                Dictionary<string, CrossCoupling2dParameter> parameterDict = new Dictionary<string, CrossCoupling2dParameter>();
                List<XElement> xeParameterList = xeCouplingCross2dParameters.Elements("cross_coupling_2d_parameter").ToList();
                foreach (XElement xeParameter in xeParameterList) {
                    var readResult = ReadSingleCrossCoupling2dParameter(xeParameter, parameterDict);
                    if (!readResult.isOk) {
                        return false;
                    }
                    parameterDict.Add(readResult.parameterId, readResult.parameter);
                }

                (int continuousDeclinePointNumber, Dictionary<string, CrossCoupling2dParameter> parameterDict) config = (pointNumber, parameterDict);
                sharedObjects.AddOrUpdate(SharedObjectKey.CROSS_COUPLING_2D_PARAMETERS, config, (key, oldValue) => config);
                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        private (bool isOk, (bool enabledCouplingIn, string couplingInId) parameters) ReadCouplingInParameters(string parameterId, XElement xeCouplingIn, string couplingType) {
            try {
                string strEnableCouplingIn = xeCouplingIn.Attribute("enabled").Value.Trim();
                if (string.IsNullOrEmpty(strEnableCouplingIn)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><{couplingType}_coupling_2d_parameters><{couplingType}_coupling_2d_parameter id={parameterId}><coupling_in enabled=>, enabled should not be empty!");
                    return (false, (false, string.Empty));
                }
                if (!bool.TryParse(strEnableCouplingIn, out bool enableCouplingIn)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><{couplingType}_coupling_2d_parameters><{couplingType}_coupling_2d_parameter id={parameterId}><coupling_in enabled={strEnableCouplingIn}>, enabled should be a boolean!");
                    return (false, (false, string.Empty));
                }
                if (!enableCouplingIn) {
                    return (true, (false, string.Empty));
                }

                string couplingInId = xeCouplingIn.Attribute("coupling_in_id").Value.Trim();
                if (string.IsNullOrEmpty(couplingInId)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><{couplingType}_coupling_2d_parameters><{couplingType}_coupling_2d_parameter id={parameterId}><coupling_in enabled={strEnableCouplingIn} coupling_in_id=>, coupling_in_id should not be empty!");
                    return (false, (false, string.Empty));
                }
                if (!couplingInConfig.ContainsKey(couplingInId)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><{couplingType}_coupling_2d_parameters><{couplingType}_coupling_2d_parameter id={parameterId}><coupling_in enabled={strEnableCouplingIn} coupling_in_id={couplingInId}>, coupling_in_id(={couplingInId}) does not exist!");
                    return (false, (false, string.Empty));
                }

                return (true, (true, couplingInId));
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return (false, (false, string.Empty));
            }
        }

        private (bool isOk, string couplingFeedbackId) ReadCouplingFeedbackParameters(string parameterId, XElement xeCouplingFeedback, string couplingType) {
            try {
                string couplingFeedbackId = xeCouplingFeedback.Attribute("coupling_feedback_id").Value.Trim();
                if (string.IsNullOrEmpty(couplingFeedbackId)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><{couplingType}_coupling_2d_parameters><{couplingType}_coupling_2d_parameter id={parameterId}><coupling_feedback coupling_feedback_id=>, coupling_feedback_id should not be empty!");
                    return (false, string.Empty);
                }
                if (!couplingFeedbackConfigDict.ContainsKey(couplingFeedbackId)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><{couplingType}_coupling_2d_parameters><{couplingType}_coupling_2d_parameter id={parameterId}><coupling_feedback coupling_feedback_id={couplingFeedbackId}>, coupling_feedback_id(={couplingFeedbackId}) does not exist!");
                    return (false, string.Empty);
                }

                return (true, couplingFeedbackId);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return (false, string.Empty);
            }
        }

        private (bool isOk, double couplingThreshold) ReadSpiral2dFeedbackThreshold(string parameterId, XElement xeFeedbackThreshold) {
            try {
                string strCouplingThreshold = xeFeedbackThreshold.Attribute("value").Value.Trim();
                if (string.IsNullOrEmpty(strCouplingThreshold)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><spiral_coupling_2d_parameters><spiral_coupling_2d_parameter id={parameterId}><feedback_threshold value=>, value should not be empty!");
                    return (false, 0);
                }
                if (!double.TryParse(strCouplingThreshold, out double feedbackThreshold)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><spiral_coupling_2d_parameters><spiral_coupling_2d_parameter id={parameterId}><feedback_threshold value={strCouplingThreshold}>, value(={strCouplingThreshold}) should be a double!");
                    return (false, 0);
                }

                return (true, feedbackThreshold);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return (false, 0);
            }
        }

        private (bool isOk, (double motionRange, double step) config) ReadSpiral2dTravelingParameters(string parameterId, XElement xeTraveling) {
            try {
                string strMotionRange = xeTraveling.Attribute("motion_range").Value.Trim();
                if (string.IsNullOrEmpty(strMotionRange)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><spiral_coupling_2d_parameters><spiral_coupling_2d_parameter id={parameterId}><traveling motion_range=>, motion_range should not be empty!");
                    return (false, (0, 0));
                }
                if (!double.TryParse(strMotionRange, out double motionRange)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><spiral_coupling_2d_parameters><spiral_coupling_2d_parameter id={parameterId}><traveling motion_range={strMotionRange}>, motion_range(={strMotionRange}) should be a positive double!");
                    return (false, (0, 0));
                }
                if (motionRange <= 0) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><spiral_coupling_2d_parameters><spiral_coupling_2d_parameter id={parameterId}><traveling motion_range={strMotionRange}>, motion_range(={strMotionRange}) should be a positive double!");
                    return (false, (0, 0));
                }

                string strStep = xeTraveling.Attribute("step").Value.Trim();
                if (string.IsNullOrEmpty(strStep)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><spiral_coupling_2d_parameters><spiral_coupling_2d_parameter id={parameterId}><traveling motion_range={strMotionRange} step=>, step should not be empty!");
                    return (false, (0, 0));
                }
                if (!double.TryParse(strStep, out double step)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><spiral_coupling_2d_parameters><spiral_coupling_2d_parameter id={parameterId}><traveling motion_range={strMotionRange} step={strStep}>, step(={strStep}) should be a positive double!");
                    return (false, (0, 0));
                }
                if (step <= 0) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><spiral_coupling_2d_parameters><spiral_coupling_2d_parameter id={parameterId}><traveling motion_range={strMotionRange} step={strStep}>, step(={strStep}) should be a positive double!");
                    return (false, (0, 0));
                }

                if (step > motionRange) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><spiral_coupling_2d_parameters><spiral_coupling_2d_parameter id={parameterId}><traveling motion_range={strMotionRange} step={strStep}>, step(={strStep}) should not be large than motion_range(={strMotionRange})!");
                    return (false, (0, 0));
                }

                return (true, (motionRange, step));
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return (false, (0, 0));
            }
        }

        private (bool isOk, (double coarseMotionRange, double coarseStep, bool enabledRefinedTraveling, double refinedMotionRange, double refinedStep) parameters) ReadCross2dTravelingParameters(string parameterId, XElement xeTraveling) {
            try {
                XElement xeCoarseTraveling = xeTraveling.Element("coarse_traveling");
                string strMotionRange = xeCoarseTraveling.Attribute("motion_range").Value.Trim();
                if (string.IsNullOrEmpty(strMotionRange)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id={parameterId}><traveling><coarse_traveling motion_range=>, motion_range should not be empty!");
                    return (false, (0, 0, false, 0, 0));
                }
                if (!double.TryParse(strMotionRange, out double coarseMotionRange)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id={parameterId}><traveling><coarse_traveling motion_range={strMotionRange}>, motion_range(={strMotionRange}) should be a positive double!");
                    return (false, (0, 0, false, 0, 0));
                }
                if (coarseMotionRange <= 0) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id={parameterId}><traveling><coarse_traveling motion_range={strMotionRange}>, motion_range(={strMotionRange}) should be a positive double!");
                    return (false, (0, 0, false, 0, 0));
                }
                string strStep = xeCoarseTraveling.Attribute("step").Value.Trim();
                if (string.IsNullOrEmpty(strStep)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id={parameterId}><traveling><coarse_traveling motion_range={strMotionRange} step=>, step should not be empty!");
                    return (false, (0, 0, false, 0, 0));
                }
                if (!double.TryParse(strStep, out double coarseStep)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id={parameterId}><traveling><coarse_traveling motion_range={strMotionRange} step={strStep}>, step(={strStep}) should be a positive double!");
                    return (false, (0, 0, false, 0, 0));
                }
                if (coarseStep <= 0) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id={parameterId}><traveling><coarse_traveling motion_range={strMotionRange} step={strStep}>, step(={strStep}) should be a positive double!");
                    return (false, (0, 0, false, 0, 0));
                }
                if (coarseStep > coarseMotionRange) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id={parameterId}><traveling><coarse_traveling motion_range={strMotionRange} step={strStep}>, step(={strStep}) should not be larger than motion_range(={strMotionRange})!");
                    return (false, (0, 0, false, 0, 0));
                }

                XElement xeRefinedTraveling = xeTraveling.Element("refined_traveling");
                string strEnableRefinedTraveling = xeRefinedTraveling.Attribute("enabled").Value.Trim();
                if (string.IsNullOrEmpty(strEnableRefinedTraveling)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id={parameterId}><traveling><refined_traveling enabled=>, enabled should not be empty!");
                    return (false, (0, 0, false, 0, 0));
                }
                if (!bool.TryParse(strEnableRefinedTraveling, out bool enabledRefinedTraveling)) {
                    LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id={parameterId}><traveling><refined_traveling enabled={strEnableRefinedTraveling}>, enabled(={strEnableRefinedTraveling}) should be a boolean!");
                    return (false, (0, 0, false, 0, 0));
                }
                double refinedMotionRange = double.MinValue;
                double refinedStep = double.MinValue;
                if (enabledRefinedTraveling) {
                    strMotionRange = xeRefinedTraveling.Attribute("motion_range").Value.Trim();
                    if (string.IsNullOrEmpty(strMotionRange)) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id={parameterId}><traveling><refined_traveling enabled=true motion_range=>, motion_range should not be empty!");
                        return (false, (0, 0, false, 0, 0));
                    }
                    if (!double.TryParse(strMotionRange, out refinedMotionRange)) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id={parameterId}><traveling><refined_traveling enabled=true motion_range={strMotionRange}>, motion_range(={strMotionRange}) should be a positive double!");
                        return (false, (0, 0, false, 0, 0));
                    }
                    if (refinedMotionRange <= 0) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id={parameterId}><traveling><refined_traveling enabled=true motion_range={strMotionRange}>, motion_range(={strMotionRange}) should be a positive double!");
                        return (false, (0, 0, false, 0, 0));
                    }
                    strStep = xeRefinedTraveling.Attribute("step").Value.Trim();
                    if (string.IsNullOrEmpty(strStep)) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id={parameterId}><traveling><refine_traveling enabled=true motion_range={strMotionRange} step=>, step should not be empty!");
                        return (false, (0, 0, false, 0, 0));
                    }
                    if (!double.TryParse(strStep, out refinedStep)) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id={parameterId}><traveling><refined_traveling enabled=true motion_range={strMotionRange} step={strStep}>, step(={strStep}) should be a positive double!");
                        return (false, (0, 0, false, 0, 0));
                    }
                    if (refinedStep <= 0) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id={parameterId}><traveling><refined_traveling enabled=true motion_range={strMotionRange} step={strStep}>, step(={strStep}) should be a positive double!");
                        return (false, (0, 0, false, 0, 0));
                    }
                    if (refinedStep > refinedMotionRange) {
                        LOGGER.Error($"In Configuration/config_coupling.xml, <coupling_2d_parameters><cross_coupling_2d_parameters><cross_coupling_2d_parameter id={parameterId}><traveling><refined_traveling enabled=true motion_range={strMotionRange} step={strStep}>, step(={strStep}) should not be larger than motion_range(={strMotionRange})!");
                        return (false, (0, 0, false, 0, 0));
                    }
                }

                return (true, (coarseMotionRange, coarseStep, enabledRefinedTraveling, refinedMotionRange, refinedStep));
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return (false, (0, 0, false, 0, 0));
            }
        }

        private readonly ConcurrentDictionary<string, object> sharedObjects;
        private readonly List<Stage> stageList;
        private readonly Dictionary<string, Instrument> instruments;
        private readonly List<InstrumentUsage> instrumentsUsages;
        private readonly Dictionary<string, (string instrumentUsageId, Dictionary<string, string> settings)> couplingInConfig = new Dictionary<string, (string instrumentUsageId, Dictionary<string, string> settings)>();
        private readonly Dictionary<string, (string instrumentUsageId, Dictionary<string, string> initSettingDict, Dictionary<string, string> triggerInSettingDict)> couplingFeedbackConfigDict = new Dictionary<string, (string instrumentUsageId, Dictionary<string, string> initSettingDict, Dictionary<string, string> triggerInSettingDict)>();
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
