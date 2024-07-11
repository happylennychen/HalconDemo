using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

using CommonApi.MyUtility;

using MyInstruments.MyEnum;
using MyInstruments.MyOpm;
using MyInstruments.MySmu;
using MyInstruments.MyTls;

using NLog;

namespace MyInstruments.MyUtility {
    public sealed class InstrumentUtility {
        public string ConfigVersion { get { return configVersion; } }
        public bool IsVirtualRunning { get { return isVirtualRunning; } }
        public Dictionary<string, Instrument> Instruments { get { return instruments; } }
        public List<InstrumentUsage> InstrumentsUsages { get { return instrumentsUsages; } }

        public bool TryConnectAllInstruments() {
            foreach (var one in instruments) {
                Instrument instrument = one.Value;
                try {
                    instrument.Connect(instrument.VisaResource);
                } catch (Exception ex) {
                    LOGGER.Error($"Connecting {instrument.VisaResource} occurred exception: {ex.Message}");
                    return false;
                }
                Thread.Sleep(TimeSpan.FromMilliseconds(100));
            }

            return true;
        }

        public void DisconnectAllInstruments() {
            foreach (var one in instruments) {
                Instrument instrument = one.Value;
                try {
                    instrument.Disconnect();
                } catch (Exception ex) {
                    LOGGER.Error($"Connecting {instrument.VisaResource} occurred exception: {ex.Message}");
                }
                Thread.Sleep(TimeSpan.FromMilliseconds(500));
            }
        }

        public bool Initialize() {
            try {
                if (!ReadConfiguration()) {
                    return false;
                }

                if (!TryCreateInstrumentInstances()) {
                    return false;
                }

                if (!TryConnectAllInstruments()) {
                    return false;
                }

                if (!CheckInstrumentsUsages()) {
                    return false;
                }

                return InitialSettingInstruments();
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        #region private methods
        private bool CheckInstrumentsUsages() {
            for (int i = 0; i < instrumentsUsages.Count; i++) {
                InstrumentUsage instrumentUsage = instrumentsUsages[i];

                Instrument instrument = instruments[instrumentUsage.InstrumentId];
                try {
                    instrument.ThrowExceptionWhenSlotOrChannelInvalid(instrumentUsage.Slot, instrumentUsage.Channel);
                } catch {
                    LOGGER.Error($"In Configuration/config_instruments.xml, <instrument_usages><instrument_usage id={instrumentUsage.UsageId} instrument_id={instrumentUsage.InstrumentId} slot={instrumentUsage.Slot} channel={instrumentUsage.Channel} category={instrumentUsage.InstrumentCategory.ToString()}>, either slot(={instrumentUsage.Slot}) or channel(={instrumentUsage.Channel}) is invalid!");
                    return false;
                }
                instrumentUsage.SetInstrument(instrument);
            }

            return true;
        }

        private bool InitialSettingInstruments() {
            foreach (var one in instrumentInitialSettingsConfig) {
                string instrumentUsageId = one.Key;
                Dictionary<string, string> settings = one.Value;

                var list = instrumentsUsages.Where(x => x.UsageId.Equals(instrumentUsageId)).ToList();
                InstrumentUsage instrumentUsage = list.First();
                Instrument instrument = instruments[instrumentUsage.InstrumentId];

                switch (instrumentUsage.InstrumentCategory) {
                    case EnumInstrumentCategory.OPM:
                        IOpm opm = (IOpm)instrument;
                        if (!opm.OpmBatchSetting(instrumentUsage.Slot, instrumentUsage.Channel, settings)) {
                            return false;
                        }
                        break;
                    case EnumInstrumentCategory.TLS:
                        ITls tls = (ITls)instrument;
                        if (!tls.TlsBatchSetting(instrumentUsage.Slot, instrumentUsage.Channel, settings)) {
                            return false;
                        }
                        break;
                    case EnumInstrumentCategory.SMU:
                        ISmu smu = (ISmu)instrument;
                        if (!smu.SmuBatchSetting(instrumentUsage.Slot, instrumentUsage.Channel, settings)) {
                            return false;
                        }
                        break;
                }
            }
            return true;
        }

        private bool TryCreateInstrumentInstances() {
            if (isVirtualRunning) {
                return CreateAllVirtualInstrumentInstances();
            } else {
                return CreateAllRealInstrumentInstances();
            }
        }

        private bool CreateAllRealInstrumentInstances() {
            instruments.Clear();
            foreach (var one in instrumentsConfig) {
                var createResult = InstrumentFactory.CreateRealInstrument(one.Id, one.Category, one.Vendor.ToString(), one.Model);
                if (!createResult.isOk) {
                    return false;
                }

                createResult.instrument.VisaResource = one.VisaResource;
                instruments.Add(one.Id, createResult.instrument);
            }

            return true;
        }

        private bool CreateAllVirtualInstrumentInstances() {
            instruments.Clear();
            foreach (var one in instrumentsConfig) {
                var createResult = InstrumentFactory.CreateVirtualInstrument(one.Id, one.Category, one.Vendor, one.Model);
                if (!createResult.isOk) {
                    return false;
                }
                createResult.instrument.VisaResource = one.VisaResource;
                instruments.Add(one.Id, createResult.instrument);
            }

            return true;
        }

        private bool ReadInstrumentsUsages(XElement xeInstrumentUsages) {
            try {
                List<XElement> xeInstrumentUsageList = xeInstrumentUsages.Elements("instrument_usage").ToList();

                instrumentsUsages.Clear();
                foreach (var xeInstrumentUsage in xeInstrumentUsageList) {
                    string usageId = xeInstrumentUsage.Attribute("id").Value.Trim();
                    if (string.IsNullOrEmpty(usageId)) {
                        LOGGER.Error($"In Configuration/config_instruments.xml, <instrument_usages><instrument_usage id=>, id should not be empty!");
                        return false;
                    }
                    var list = instrumentsUsages.Where(x => x.UsageId.Equals(usageId)).ToList();
                    if (list.Count > 0) {
                        LOGGER.Error($"In Configuration/config_instruments.xml, <instrument_usages><instrument_usage id={usageId}>, id(={usageId}) is duplicated!");
                        return false;
                    }

                    string instrumentId = xeInstrumentUsage.Attribute("instrument_id").Value.Trim();
                    if (string.IsNullOrEmpty(instrumentId)) {
                        LOGGER.Error($"In Configuration/config_instruments.xml, <instrument_usages><instrument_usage id={usageId} instrument_id=>, instrument_id should not be empty!");
                        return false;
                    }
                    var list2 = instrumentsConfig.Where(x => x.Id.Equals(instrumentId)).ToList();
                    if (list2.Count == 0) {
                        LOGGER.Error($"In Configuration/config_instruments.xml, <instrument_usages><instrument_usage id={usageId} instrument_id={instrumentId}>, instrument_id(={instrumentId}) does not exist!");
                        return false;
                    }

                    string slot = xeInstrumentUsage.Attribute("slot").Value.Trim();
                    string channel = xeInstrumentUsage.Attribute("channel").Value.Trim();
                    string strCategory = xeInstrumentUsage.Attribute("category").Value.Trim();
                    if (!Enum.TryParse(strCategory, out EnumInstrumentCategory enumCategory)) {
                        LOGGER.Error($"In Configuration/config_instruments.xml, <instrument_usages><instrument_usage id={usageId} instrument_id={instrumentId} slot={slot} channel={channel} category={strCategory}>, category(={strCategory}) is invalid!");
                        return false;
                    }
                    if (IsMultiFunctionCategory(enumCategory)) {
                        LOGGER.Error($"In Configuration/config_instruments.xml, <instrument_usages><instrument_usage id={usageId} instrument_id={instrumentId} slot={slot} channel={channel} category={strCategory}>, category(={strCategory}) should not be multi-function category({{{EnumInstrumentCategory.PMA.ToString()},{EnumInstrumentCategory.DMM.ToString()}}})!");
                        return false;
                    }

                    InstrumentUsage instrumentUsage = new InstrumentUsage(usageId, instrumentId, slot, channel, enumCategory);
                    instrumentsUsages.Add(instrumentUsage);
                }

                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        private bool ReadInstrumentInitialSettings(XElement xeInstrumentInitialSettings) {
            instrumentInitialSettingsConfig.Clear();
            try {
                List<XElement> xeInstrumentInitialSettingList = xeInstrumentInitialSettings.Elements("instrument_initial_setting").ToList();
                foreach (XElement xeInstrumentInitialSetting in xeInstrumentInitialSettingList) {
                    string instrumentUsageId = xeInstrumentInitialSetting.Attribute("instrument_usage_id").Value.Trim();
                    if (string.IsNullOrEmpty(instrumentUsageId)) {
                        LOGGER.Error($"In Configuration/config_instruments.xml, <instrument_initial_settings><instrument_setting instrument_usage_id=>, instrument_usage_id should not be empty!");
                        return false;
                    }
                    var list = instrumentsUsages.Where(x => x.UsageId.Equals(instrumentUsageId)).ToList();
                    if (list.Count == 0) {
                        LOGGER.Error($"In Configuration/config_instruments.xml, <instrument_initial_settings><instrument_setting instrument_usage_id={instrumentUsageId}>, instrument_usage_id(={instrumentUsageId}) does not exist!");
                        return false;
                    }
                    InstrumentUsage instrumentUsage = list.First();

                    List<XElement> xeSettingList = xeInstrumentInitialSetting.Elements("setting").ToList();
                    Dictionary<string, string> initialSettings = new Dictionary<string, string>();
                    foreach (XElement xeSetting in xeSettingList) {
                        string settingKey = xeSetting.Attribute("key").Value.ToUpperInvariant().Trim();
                        if (string.IsNullOrEmpty(settingKey)) {
                            LOGGER.Error($"In Configuration/config_instruments.xml, <instrument_initial_settings><instrument_setting instrument_usage_id={instrumentUsageId}><setting key=>, key should not be empty!");
                            return false;
                        }
                        string settingValue = xeSetting.Attribute("value").Value.Trim();
                        if (string.IsNullOrEmpty(settingValue)) {
                            LOGGER.Error($"In Configuration/config_instruments.xml, <instrument_initial_settings><instrument_setting instrument_usage_id={instrumentUsageId}><setting key={settingKey} value=>, value should not be empty!");
                            return false;
                        }

                        initialSettings.Add(settingKey, settingValue);
                    }

                    instrumentInitialSettingsConfig.Add(instrumentUsageId, initialSettings);
                }

                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        private bool ReadConfiguration() {
            try {
                XElement xeRoot = XElement.Load(@"Configuration/config_instruments.xml");
                configVersion = xeRoot.Attribute("version").Value.Trim();
                if (string.IsNullOrEmpty(configVersion)) {
                    LOGGER.Error($"in Configuration/config_instruments.xml, <config version=>, version should not be empty!");
                    return false;
                }
                string strIsVirtualRunning = xeRoot.Attribute("is_virtual_running").Value.Trim();
                if (!bool.TryParse(strIsVirtualRunning, out isVirtualRunning)) {
                    LOGGER.Error($"in Configuration/config_instruments.xml, <config is_virtual_running={strIsVirtualRunning}>, is_virtual_running(={strIsVirtualRunning}) is not a valid boolean settingValue!");
                    return false;
                }

                XElement xeInstruments = xeRoot.Element("instruments");
                if (!ReadInstruments(xeInstruments)) {
                    return false;
                }

                XElement xeInstrumentUsages = xeRoot.Element("instrument_usages");
                if (!ReadInstrumentsUsages(xeInstrumentUsages)) {
                    return false;
                }

                //XElement xeInstrumentInitialSettings = xeRoot.Element("instrument_initial_settings");
                //return ReadInstrumentInitialSettings(xeInstrumentInitialSettings);
                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        private bool ReadInstruments(XElement xeInstruments) {
            instrumentsConfig.Clear();
            try {
                List<XElement> xeInstrumentList = xeInstruments.Elements("instrument").ToList();
                foreach (XElement xeInstrument in xeInstrumentList) {
                    string instrumentId = xeInstrument.Attribute("id").Value.Trim();
                    if (string.IsNullOrEmpty(instrumentId)) {
                        LOGGER.Error($"in config_instruments.xml, <instruments><instrument id=>, id should not be empty!");
                        return false;
                    }
                    var list = instrumentsConfig.Where(x => x.Id.Equals(instrumentId)).ToList();
                    if (list.Count > 0) {
                        LOGGER.Error($"in config_instruments.xml, <instruments><instrument id={instrumentId}>, id(={instrumentId}) is duplicated!");
                        return false;
                    }

                    string category = xeInstrument.Attribute("category").Value.Trim();
                    if (string.IsNullOrEmpty(category)) {
                        LOGGER.Error($"in config_instruments.xml, <instruments><instrument id={instrumentId} category=>, category should not be empty!");
                        return false;
                    }
                    string vendor = xeInstrument.Attribute("vendor").Value.Trim();
                    if (string.IsNullOrEmpty(vendor)) {
                        LOGGER.Error($"in config_instruments.xml, <instruments><instrument id={instrumentId} category={category} vendor=>, vendor should not be empty!");
                        return false;
                    }
                    string model = xeInstrument.Attribute("model").Value.Trim();
                    if (string.IsNullOrEmpty(model)) {
                        LOGGER.Error($"in config_instruments.xml, <instruments><instrument id={instrumentId} category={category} vendor={vendor} model=>, model should not be empty!");
                        return false;
                    }
                    string visaResource = xeInstrument.Attribute("visa_resource").Value.Trim();
                    if (string.IsNullOrEmpty(visaResource)) {
                        LOGGER.Error($"in config_instruments.xml, <instruments><instrument id={instrumentId} category={category} vendor={vendor} model={model} visa_resource=>, visa_resource should not be empty!");
                        return false;
                    }

                    var convertResult = TryConvertVendorAndModel(instrumentId, category, vendor, model, visaResource);
                    if (!convertResult.isOk) {
                        return false;
                    }
                    instrumentsConfig.Add(convertResult.instrumentConfig);
                }
                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        private (bool isOk, InstrumentConfig instrumentConfig) TryConvertVendorAndModel(string instrumentId, string category, string vendor, string model, string visaResource) {
            string errorTextCategoryNotSupported = $"in config_instruments.xml, <instruments><instrument id={instrumentId} category={category}>, category(={category}) is not supported!";
            string errorTextVendorNotSupported = $"in config_instruments.xml, <instruments><instrument id={instrumentId} category={category} vendor={vendor}>, vendor(={vendor}) is not supported!";
            string errorTextModelNotSupported = $"in config_instruments.xml, <instruments><instrument id={instrumentId} category={category} vendor={vendor} model={model}>, model(={model}) is not supported!";
            string errorTextVisaResourceIsEmpty = $"in config_instruments.xml, <config><instruments><instrument id={instrumentId} category={category} vendor={vendor} model={model} visa_resource=>, visa_resource should not be empty!";

            if (!Enum.TryParse(category, out EnumInstrumentCategory instrumentCategory)) {
                LOGGER.Error(errorTextCategoryNotSupported);
                return (false, null);
            }

            if (!Enum.TryParse(vendor, out EnumInstrumentVendor instrumentVendor)) {
                LOGGER.Error(errorTextVendorNotSupported);
                return (false, null);
            }

            switch (instrumentVendor) {
                case EnumInstrumentVendor.KEYSIGHT:
                    if (!Enum.TryParse(model, out EnumModelKeysight keysightModel)) {
                        LOGGER.Error(errorTextModelNotSupported);
                        return (false, null);
                    }
                    break;
                case EnumInstrumentVendor.KEITHLEY:
                    if (!Enum.TryParse(model, out EnumModelKeithley keithleyModel)) {
                        LOGGER.Error(errorTextModelNotSupported);
                        return (false, null);
                    }
                    break;
                case EnumInstrumentVendor.KEYENCE:
                    if (!Enum.TryParse(model, out EnumModelKeyence keyenceModel)) {
                        LOGGER.Error(errorTextModelNotSupported);
                        return(false, null);
                    }
                    break;
                case EnumInstrumentVendor.KH:
                    if (!Enum.TryParse(model, out EnumModelKH khModel)) {
                        LOGGER.Error(errorTextModelNotSupported);
                        return(false, null);
                    }
                    break;
                case EnumInstrumentVendor.YUDIAN:
                    if (!Enum.TryParse(model, out EnumModelYuDian ydModel)) {
                        LOGGER.Error(errorTextModelNotSupported);
                        return (false, null);
                    }
                    break;
                case EnumInstrumentVendor.OMRON:
                    if (!Enum.TryParse(model, out EnumModelOmron omronModel)) {
                        LOGGER.Error(errorTextModelNotSupported);
                        return (false, null);
                    }
                    break;
                case EnumInstrumentVendor.DAHENG:
                    if (!Enum.TryParse(model, out EnumModelDH dhModel)) {
                        LOGGER.Error(errorTextModelNotSupported);
                        return (false, null);
                    }
                    break;
                case EnumInstrumentVendor.POMEAS:
                    if (!Enum.TryParse(model, out EnumModelPomeas elModel)) {
                        LOGGER.Error(errorTextModelNotSupported);
                        return (false, null);
                    }
                    break;
                case EnumInstrumentVendor.THKA: 
                    if (!Enum.TryParse(model, out EnumModelThka thkModel)) {
                        LOGGER.Error(errorTextModelNotSupported);
                        return (false, null);
                    }
                    break;
                case EnumInstrumentVendor.NI:
                    if (!Enum.TryParse(model, out EnumModelNI niModel)) {
                        LOGGER.Error(errorTextModelNotSupported);
                        return (false, null);   
                    }
                    break;
                case EnumInstrumentVendor.ZS:
                    if (!Enum.TryParse(model, out EnumModelZS zsModel))
                    {
                        LOGGER.Error(errorTextModelNotSupported);
                        return (false, null);
                    }
                    break;
                default:
                    LOGGER.Error(errorTextVendorNotSupported);
                    return (false, null);
            }

            InstrumentConfig instrumentConfig = new InstrumentConfig();
            instrumentConfig.Id = instrumentId;
            instrumentConfig.Category = instrumentCategory;
            instrumentConfig.Vendor = instrumentVendor;
            instrumentConfig.Model = model;
            instrumentConfig.VisaResource = visaResource;

            return (true, instrumentConfig);
        }

        private static bool IsMultiFunctionCategory(EnumInstrumentCategory instrumentCategory) {
            return multiFunctionInstrumentCategories.Contains(instrumentCategory);
        }
        #endregion

        private string configVersion;
        private bool isVirtualRunning;
        internal readonly List<InstrumentConfig> instrumentsConfig = new List<InstrumentConfig>();
        private readonly Dictionary<string, Instrument> instruments = new Dictionary<string, Instrument>();
        private readonly List<InstrumentUsage> instrumentsUsages = new List<InstrumentUsage>();
        private readonly Dictionary<string, Dictionary<string, string>> instrumentInitialSettingsConfig = new Dictionary<string, Dictionary<string, string>>();

        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        private static readonly HashSet<EnumInstrumentCategory> multiFunctionInstrumentCategories = new HashSet<EnumInstrumentCategory>() { EnumInstrumentCategory.PMA, EnumInstrumentCategory.DMM };
    }
}
