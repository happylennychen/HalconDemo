using System.Collections.Generic;

namespace CommonApi.MyTrigger {
    public interface ITriggerOut {
        (bool isOk, string errorText) IsTriggerOutSettingKeyMissed(Dictionary<string, string> settings);
        (bool isOk, string errorText) CheckTriggerOutSettings(Dictionary<string, string> settings);
        bool PrepareTriggeringOut(Dictionary<string, string> settings);
        bool StartTriggeringOut();
        void WaitForTriggeringOutCompleted();
        (bool isOk, List<double> data) GetTriggeringOutData();
    }
}
