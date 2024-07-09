using System.Collections.Generic;

using MyMotionStageDriver.MyStageAxis;

namespace ProberApi.MyUtility {
    public sealed class AxisUsageIdValidChecker {
        public AxisUsageIdValidChecker(Dictionary<string, StageAxis> stageAxisUsages) {
            this.stageAxisUsages = stageAxisUsages;
        }

        public bool Check(string axisUsageId) {
            return stageAxisUsages.ContainsKey(axisUsageId);
        }

        private readonly Dictionary<string, StageAxis> stageAxisUsages;
    }
}
