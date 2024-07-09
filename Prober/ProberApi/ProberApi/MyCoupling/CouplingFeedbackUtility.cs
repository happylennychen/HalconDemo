using System;
using System.Collections.Generic;

using CommonApi.MyUtility;

using MyInstruments.MyEnum;
using MyInstruments.MyOpm;
using MyInstruments.MySmu;
using MyInstruments.MyUtility;

using NLog;

namespace ProberApi.MyCoupling {
    public sealed class CouplingFeedbackUtility {
        public CouplingFeedbackUtility(InstrumentUsage instrumentUsage) {
            this.instrumentUsage = instrumentUsage;            
        }

        public (bool isOk, Func<double> funcGetFeedback) GetFuncGetFeedback() {
            Func<double> result = null;
            switch (instrumentUsage.InstrumentCategory) {
                case EnumInstrumentCategory.OPM:
                    IOpm opm = instrumentUsage.TheInstrument as IOpm;
                    result = new Func<double>(() => {
                        return opm.OpmFetchPower(instrumentUsage.Slot, instrumentUsage.Channel);
                        //return opm.OpmReadPower(instrumentUsage.Slot, instrumentUsage.Channel);
                    });
                    break;
                case EnumInstrumentCategory.SMU:
                    ISmu smu = instrumentUsage.TheInstrument as ISmu;
                    result = new Func<double>(() => {
                        return smu.Measure(instrumentUsage.Slot, instrumentUsage.Channel);
                    });
                    break;
                default:
                    LOGGER.Error($"Invalid coupling feedback instrument category(={instrumentUsage.InstrumentCategory.ToString()})!");
                    return (false, null);
            }

            return (true, result);
        }

        public (bool isOk, Func<bool> funcBatchSetting) GetFuncBatchSetting(Dictionary<string, string> settings) {
            Func<bool> funcBatchSetting = null;
            switch (instrumentUsage.InstrumentCategory) {
                case EnumInstrumentCategory.OPM:
                    IOpm opm = instrumentUsage.TheInstrument as IOpm;
                    var opmCheckResult = opm.OpmAreSettingsValid(settings);
                    if (!opmCheckResult.isOk) {
                        LOGGER.Error(opmCheckResult.errorText);
                        return (false, null);
                    }
                    funcBatchSetting = new Func<bool>(() => {
                        return opm.OpmBatchSetting(instrumentUsage.Slot, instrumentUsage.Channel, settings);
                    });
                    break;
                case EnumInstrumentCategory.SMU:
                    ISmu smu = instrumentUsage.TheInstrument as ISmu;
                    var smuCheckResult = smu.SmuAreSettingsValid(settings);
                    if (!smuCheckResult.isOk) {
                        LOGGER.Error(smuCheckResult.errorText);
                        return (false, null);
                    }
                    funcBatchSetting = new Func<bool>(() => {
                        return smu.SmuBatchSetting(instrumentUsage.Slot, instrumentUsage.Channel, settings);
                    });
                    break;
                default:
                    LOGGER.Error($"Invalid coupling feedback instrument category(={instrumentUsage.InstrumentCategory.ToString()})!");
                    return (false, null);
            }

            return (true, funcBatchSetting);
        }

        internal readonly InstrumentUsage instrumentUsage;        
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
