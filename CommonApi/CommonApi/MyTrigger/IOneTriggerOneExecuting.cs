using System.Collections.Generic;

namespace CommonApi.MyTrigger {
    public interface IOneTriggerOneExecuting {
        (bool isOk, string errorText) CheckSettings(Dictionary<string, string> triggerOutSettings, Dictionary<string, string> triggerInSettings);
        bool Prepare(Dictionary<string, string> triggerOutSettings, Dictionary<string, string> triggerInSettings);
        bool Start();
        void WaitForCompleted();
        (bool isOk, List<double> triggerOutData, List<double> triggerInData) GetData();
    }
}
