using CommonApi.MyConstant;
using MyMotionStageDriver.MyStageAxis;
using ProberApi.MyConstant;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ProberApi.MyQuery
{
    public sealed class QueryAxisPosition:AbstractQuery {
        public QueryAxisPosition(ConcurrentDictionary<string,object> sharedObjects) : base(sharedObjects) {
            this.QueryType = CommonQueryType.AXIS_POSITION;
        }

        public override (bool isOk, string result) Query(string parameter) {
            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            Dictionary<string, StageAxis> StageAxisUsages = tempObj as Dictionary<string, StageAxis>;

            string axisUsageId = parameter;
            if (!StageAxisUsages.ContainsKey(axisUsageId)) {
                return (false, "Invalid stage axis usage ID");
            }

            StageAxis stageAxis = StageAxisUsages[axisUsageId];
            double position = stageAxis.Position();
            return (true, position.ToString("f3"));
        }        
    }
}
