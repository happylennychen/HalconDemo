using System.Collections.Generic;

using CommonApi.MyTrigger;

namespace MyInstruments.MyTls {
    public abstract class StandaloneTlsWithTriggerOut : StandaloneTls, ITriggerOut {
        protected StandaloneTlsWithTriggerOut(string id) : base(id) {
        }

        public abstract (bool isOk, string errorText) CheckTriggerOutSettings(Dictionary<string, string> settings);

        public abstract (bool isOk, List<double> data) GetTriggeringOutData();

        public abstract (bool isOk, string errorText) IsTriggerOutSettingKeyMissed(Dictionary<string, string> settings);

        public abstract bool PrepareTriggeringOut(Dictionary<string, string> settings);

        public abstract bool StartTriggeringOut();

        public abstract void WaitForTriggeringOutCompleted();
    }
}
