using CommonApi.MyI18N;

using MyMotionStageDriver.MyStageAxis;

namespace MyMotionStageUserControl {
    public static class UcStageAxisFactory {
        public static UcStageAxis CreateInstance(EnumLanguage language, StageAxis stageAxis) {
            return new UcStageAxis(language, stageAxis);
        }
    }
}
