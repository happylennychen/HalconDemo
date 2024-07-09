using System.Collections.Generic;

using MyMotionStageDriver.MyStageAxis;

namespace MyMotionStageDriver.MyStage {
    public sealed class Stage {
        internal Stage(string id) {
            Id = id;
            AxisList = new List<StageAxis>();
        }

        public string Id { get; private set; }
        public List<StageAxis> AxisList { get; private set; }
    }
}
