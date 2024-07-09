using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using CommonApi.MyUtility;

using MyMotionStageDriver.MyEnum;
using MyMotionStageDriver.MyMotionController;
using MyMotionStageDriver.MyStageAxis;

using NLog;

namespace MyMotionStageDriver.MyStage {
    public class StageUtility {
        public List<Stage> StageList { get { return stageList; } }

        public StageUtility(Dictionary<string, AbstractMotionController> motionControllers) {
            this.motionControllers = motionControllers;
        }

        public bool Initialize(XElement xeStages) {
            stageList.Clear();
            try {
                List<XElement> xeStageList = xeStages.Elements("stage").ToList();
                HashSet<string> stageIdSet = new HashSet<string>();
                foreach (XElement xeStage in xeStageList) {
                    var initResult = InitializeStage(xeStage, stageIdSet);
                    if (!initResult.isOk) {
                        return false;
                    }

                    stageList.Add(initResult.stage);
                }

                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        private (bool isOk, Stage stage) InitializeStage(XElement xeStage, HashSet<string> stageIdSet) {
            try {
                string stageId = xeStage.Attribute("id").Value.Trim();
                if (string.IsNullOrEmpty(stageId)) {
                    LOGGER.Error($"In Configuration/config_motion_stages.xml, <stages><stage id=>, stage id should not be empty!");
                    return (false, null);
                }
                if (stageIdSet.Contains(stageId)) {
                    LOGGER.Error($"In Configuration/config_motion_stages.xml, <stages><stage id={stageId}>, stage id(={stageId}) is duplicated!");
                    return (false, null);
                }
                stageIdSet.Add(stageId);

                Stage stage = new Stage(stageId);
                List<XElement> xeAxisList = xeStage.Elements("axis").ToList();
                HashSet<string> axisIdSet = new HashSet<string>();
                foreach (XElement xeAxis in xeAxisList) {
                    string axisId = xeAxis.Attribute("id").Value.Trim();
                    if (string.IsNullOrEmpty(axisId)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stages><stage id={stageId}><axis id=>, axis_id should not be empty!");
                        return (false, null);
                    }
                    if (axisIdSet.Contains(axisId)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stages><stage id={stageId}><axis id={axisId}>, axis_id(={axisId}) is duplicated!");
                        return (false, null);
                    }
                    axisIdSet.Add(axisId);

                    string motionControllerTypeId = xeAxis.Attribute("motion_controller_type_id").Value.Trim();
                    if (string.IsNullOrEmpty(motionControllerTypeId)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stages><stage id={stageId}><axis id={axisId} motion_controller_type_id=>, motion_controller_type_id should not be empty!");
                        return (false, null);
                    }
                    if (!motionControllers.ContainsKey(motionControllerTypeId)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stages><stage id={stageId}><axis id={axisId} motion_controller_type_id={motionControllerTypeId}>, motion_controller_type_id(={motionControllerTypeId}) does not exist!");
                        return (false, null);
                    }

                    string boardAddress = xeAxis.Attribute("board_address").Value.Trim();
                    if (string.IsNullOrEmpty(boardAddress)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stages><stage id={stageId}><axis id={axisId} board_address=>, board_address should not be empty!");
                        return (false, null);
                    }
                    string channelId = xeAxis.Attribute("channel_id").Value.Trim();
                    if (string.IsNullOrEmpty(channelId)) {
                        LOGGER.Error($"InConfiguration/ config_motion_stages.xml, <stages><stage id={stageId}><axis id={axisId} channel_id=>, channel_id should not be empty!");
                        return (false, null);
                    }
                    AbstractMotionController motionController = motionControllers[motionControllerTypeId];
                    motionController.ThrowExceptionWhenAxisBoardAddressOrChannelIdIsInvalid(boardAddress, channelId);

                    string strAxisType = xeAxis.Attribute("axis_type").Value.Trim();
                    if (string.IsNullOrEmpty(strAxisType)) {
                        LOGGER.Error($"InConfiguration/ config_motion_stages.xml, <stages><stage id={stageId}><axis id={axisId} axis_type=>, axis_type should not be empty!");
                        return (false, null);
                    }
                    if (!Enum.TryParse(strAxisType, out EnumAxisType axisType)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stages><stage id={stageId}><axis id={axisId}>, axis_type={strAxisType} should be LINEAR or ROTATION!");
                        return (false, null);
                    }

                    string strBacklash = xeAxis.Attribute("backlash").Value.Trim();
                    if (string.IsNullOrEmpty(strBacklash)) {
                        LOGGER.Error($"InConfiguration/config_motion_stages.xml, <stages><stage id={stageId}><axis id={axisId} backlash=>, backlash should not be empty!");
                        return (false, null);
                    }
                    if (!double.TryParse(strBacklash, out double backlash)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stages><stage id={stageId}><axis id={axisId} backlash={strBacklash}>, backlash(={strBacklash}) should be a non-negative double!");
                        return (false, null);
                    }
                    if (backlash < 0) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stages><stage id={stageId}><axis id={axisId} backlash={strBacklash}>, backlash(={strBacklash}) should be a non-negative double!");
                        return (false, null);
                    }

                    string strPositiveDirection = xeAxis.Attribute("positive_direction").Value.Trim();
                    EnumAxisPositiveDirection enumAxisPositiveDirection;
                    if (axisType == EnumAxisType.LINEAR) {
                        if (string.IsNullOrEmpty(strPositiveDirection)) {
                            LOGGER.Error($"InConfiguration/config_motion_stages.xml, <stages><stage id={stageId}><axis id={axisId} axis_type={EnumAxisType.LINEAR.ToString()} positive_direction=>, backlash should not be empty!");
                            return (false, null);
                        }
                        if (!Enum.TryParse(strPositiveDirection, out enumAxisPositiveDirection)) {
                            LOGGER.Error($"InConfiguration/config_motion_stages.xml, <stages><stage id={stageId}><axis id={axisId} axis_type={EnumAxisType.LINEAR.ToString()} positive_direction={strPositiveDirection}>, positive_direction(={strPositiveDirection}) is invalid!");
                            return (false, null);
                        }
                    } else {
                        enumAxisPositiveDirection = EnumAxisPositiveDirection.None;
                    }                   
                    StageAxis stageAxis = StageAxisFactory.CreateInstance(motionController, boardAddress, channelId);
                    stageAxis.AxisType = axisType;
                    stageAxis.StageId = stageId;
                    stageAxis.AxisId = axisId;
                    stageAxis.Backlash = backlash;
                    stageAxis.PositiveDirection = enumAxisPositiveDirection;
                    stage.AxisList.Add(stageAxis);
                }

                return (true, stage);
            } catch (Exception ex) {
                LOGGER.Error($"Occurred exception: {ex.Message}");
                return (false, null);
            }
        }

        private readonly Dictionary<string, AbstractMotionController> motionControllers;
        private readonly List<Stage> stageList = new List<Stage>();
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
