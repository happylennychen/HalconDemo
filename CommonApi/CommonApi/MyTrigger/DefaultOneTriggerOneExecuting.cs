using System.Collections.Generic;

using CommonApi.MyUtility;

using NLog;

namespace CommonApi.MyTrigger {
    public class DefaultOneTriggerOneExecuting : IOneTriggerOneExecuting {
        public DefaultOneTriggerOneExecuting(ITriggerOut triggerOut, ITriggerIn triggerIn) {
            this.triggerOut = triggerOut;
            this.triggerIn = triggerIn;
        }

        public virtual (bool isOk, string errorText) CheckSettings(Dictionary<string, string> triggerOutSettings, Dictionary<string, string> triggerInSettings) {
            var checkResult = triggerOut.CheckTriggerOutSettings(triggerOutSettings);
            if (!checkResult.isOk) {
                LOGGER.Error($"Checking trigger-out-settings is failed: {checkResult.errorText}");
                return (false, checkResult.errorText);
            }

            checkResult = triggerIn.CheckTriggerInSettings(triggerInSettings);
            if (!checkResult.isOk) {
                LOGGER.Error($"Checking trigger-in-settings is failed: {checkResult.errorText}");
                return (false, checkResult.errorText);
            }

            return (true, string.Empty);
        }

        public virtual bool Prepare(Dictionary<string, string> triggerOutSettings, Dictionary<string, string> triggerInSettings) {
            if (!triggerOut.PrepareTriggeringOut(triggerOutSettings)) {
                LOGGER.Error("Preparing trigger-out is failed!");
                return false;
            }

            if (!triggerIn.PrepareTriggeringIn(triggerInSettings)) {
                LOGGER.Error("Preparing trigger-in is failed!");
                return false;
            }

            return true;
        }

        public virtual bool Start() {
            if (!triggerIn.StartTriggeringIn()) {
                LOGGER.Error("Starting trigger-in is failed!");
                return false;
            }

            if (!triggerOut.StartTriggeringOut()) {
                LOGGER.Error("Starting trigger-out is failed!");
                return false;
            }

            return true;
        }

        public virtual void WaitForCompleted() {
            triggerOut.WaitForTriggeringOutCompleted();
            triggerIn.WaitForTriggeringInCompleted();
        }

        public virtual (bool isOk, List<double> triggerOutData, List<double> triggerInData) GetData() {
            var triggerOutResult = triggerOut.GetTriggeringOutData();
            if (!triggerOutResult.isOk) {
                LOGGER.Error($"Getting trigger-out data is failed!");
                return (false, null, null);
            }

            var triggerInResult = triggerIn.GetTriggeringInData();
            if (!triggerInResult.isOk) {
                LOGGER.Error($"Getting trigger-in data is failed!");
                return (false, null, null);
            }

            return (true, triggerOutResult.data, triggerInResult.data);
        }

        protected readonly ITriggerOut triggerOut;
        protected readonly ITriggerIn triggerIn;
        protected static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
