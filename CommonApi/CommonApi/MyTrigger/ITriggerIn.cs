using System.Collections.Generic;

namespace CommonApi.MyTrigger {
    public interface ITriggerIn {
        (bool isOk, string errorText) IsTriggerInSettingKeyMissed(Dictionary<string, string> settings);
        (bool isOk, string errorText) CheckTriggerInSettings(Dictionary<string, string> settings);
        bool PrepareTriggeringIn(Dictionary<string, string> settings);
        bool StartTriggeringIn();
        void WaitForTriggeringInCompleted();
        (bool isOk, List<double> data) GetTriggeringInData();
    }
}
