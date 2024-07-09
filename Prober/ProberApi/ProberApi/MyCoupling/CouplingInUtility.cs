using System;
using System.Collections.Generic;

using CommonApi.MyUtility;

using MyInstruments.MyTls;
using MyInstruments.MyUtility;

using NLog;

namespace ProberApi.MyCoupling {
    internal sealed class CouplingInUtility {
        internal CouplingInUtility(InstrumentUsage instrumentUsage, Dictionary<string, string> settings) {
            this.instrumentUsage = instrumentUsage;
            this.settings = settings;
        }

        internal (bool isOk, Func<bool> funcCouplingInBatchSetting) Run() {
            Func<bool> funcBatchSetting = null;
            switch (instrumentUsage.InstrumentCategory) {
                case MyInstruments.MyEnum.EnumInstrumentCategory.TLS:
                    ITls tls = instrumentUsage.TheInstrument as ITls;
                    var checkResult = tls.TlsAreSettingsValid(settings);
                    if (!checkResult.isOk) {
                        LOGGER.Error(checkResult.errorText);
                        return (false, null);
                    }
                    funcBatchSetting = new Func<bool>(() => {
                        return tls.TlsBatchSetting(instrumentUsage.Slot, instrumentUsage.Channel, settings);
                    });
                    break;
                default:
                    LOGGER.Error($"Invalid coupling in instrument category(={instrumentUsage.InstrumentCategory.ToString()})!");
                    return (false, null);
            }
            return (true, funcBatchSetting);
        }

        internal readonly InstrumentUsage instrumentUsage;
        internal readonly Dictionary<string, string> settings;
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
