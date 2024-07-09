using System.Collections.Generic;

using CommonApi.MyTrigger;

namespace MyInstruments.MyOpm {
    public abstract class StandaloneOpmWithTriggerIn : StandaloneOpm, ITriggerIn {
        public StandaloneOpmWithTriggerIn(string id) : base(id) {
        }

        #region trigger api
        public abstract (bool isOk, string errorText) CheckTriggerInSettings(Dictionary<string, string> settings);
        public abstract (bool isOk, string errorText) IsTriggerInSettingKeyMissed(Dictionary<string, string> settings);
        public abstract bool PrepareTriggeringIn(Dictionary<string, string> settings);
        public abstract bool StartTriggeringIn();
        public abstract void WaitForTriggeringInCompleted();
        public abstract (bool isOk, List<double> data) GetTriggeringInData();
        #endregion
    }
}
