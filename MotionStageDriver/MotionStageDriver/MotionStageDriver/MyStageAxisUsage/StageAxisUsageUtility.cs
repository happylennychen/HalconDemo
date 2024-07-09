using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using CommonApi.MyUtility;

using MyMotionStageDriver.MyStage;
using MyMotionStageDriver.MyStageAxis;

using NLog;

namespace MyMotionStageDriver.MyStageAxisUsage {
    public sealed class StageAxisUsageUtility {
        public StageAxisUsageUtility(List<Stage> stageList) {
            this.stageList = stageList;
            StageAxisUsages = new Dictionary<string, StageAxis>();
        }

        public Dictionary<string, StageAxis> StageAxisUsages { get; private set; }

        public bool Initialize(XElement xeAxisUsages) {
            StageAxisUsages.Clear();

            try {
                List<XElement> xeAxisUsageList = xeAxisUsages.Elements("axis_usage").ToList();
                foreach (XElement xeAxisUsage in xeAxisUsageList) {
                    string usageId = xeAxisUsage.Attribute("id").Value.Trim();
                    if (string.IsNullOrEmpty(usageId)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <axis_usages><axis_usage id=>, id should not be empty!");
                        return false;
                    }
                    if (StageAxisUsages.ContainsKey(usageId)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <axis_usages><axis_usage id={usageId}>, id(={usageId}) is duplicated!");
                        return false;
                    }

                    string stageId = xeAxisUsage.Attribute("stage_id").Value.Trim();
                    if (string.IsNullOrEmpty(stageId)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <axis_usages><axis_usage id={usageId} stage_id=>, stage_id should not be empty!");
                        return false;
                    }
                    var choosenStageList = stageList.Where(s => s.Id.Equals(stageId)).ToList();
                    if (choosenStageList.Count == 0) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <axis_usages><axis_usage id={usageId} stage_id={stageId}>, stage_id(={stageId}) does not exist!");
                        return false;
                    }
                    Stage stage = choosenStageList.First();

                    string axisId = xeAxisUsage.Attribute("axis_id").Value.Trim();
                    if (string.IsNullOrEmpty(axisId)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <axis_usages><axis_usage id={usageId} stage_id={stageId} axis_id=>, axis_id should not be empty!");
                        return false;
                    }
                    var choosenAxisList = stage.AxisList.Where(a => a.AxisId.Equals(axisId)).ToList();
                    if (choosenAxisList.Count == 0) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <axis_usages><axis_usage id={usageId} stage_id={stageId} axis_id={axisId}>, axis_id(={axisId}) does not exist!");
                        return false;
                    }
                    StageAxis stageAxis = choosenAxisList.First();

                    StageAxisUsages.Add(usageId, stageAxis);
                }

                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        private readonly List<Stage> stageList;
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
