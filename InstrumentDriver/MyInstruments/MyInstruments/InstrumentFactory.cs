using CommonApi.MyUtility;
using MyInstruments.MyAltimeter;
using MyInstruments.MyCamera;
using MyInstruments.MyElecLens;
using MyInstruments.MyEnum;
using MyInstruments.MyException;
using MyInstruments.MyLed;
using MyInstruments.MyOpm;
using MyInstruments.MyPma;
using MyInstruments.MySmu;
using MyInstruments.MyTec;
using NLog;

namespace MyInstruments {
    public static class InstrumentFactory {
        public static (bool isOk, Instrument instrument) CreateRealInstrument(string instrumentId, EnumInstrumentCategory category, string vendor, string model) {
            Instrument instrument = null;

            string errorTextVendorNotSupported = $"In category={category}, vendor={vendor} does not be supported!";
            string errorTextModelNotSupported = $"In category={category} and vendor={vendor}, model={model} does not be supported!";
            switch (category) {
                case EnumInstrumentCategory.OPM:
                    StandaloneOpm standaloneOpm = StandaloneOpmFactory.CreateInstance(instrumentId, vendor, model);
                    if (standaloneOpm == null) {
                        LOGGER.Error(errorTextModelNotSupported);
                        return (false, null);
                    }
                    instrument = standaloneOpm;
                    break;
                case EnumInstrumentCategory.PMA:
                    Pma pma = PmaFactory.CreateInstance(instrumentId, vendor, model);
                    if (pma == null) {
                        LOGGER.Error(errorTextModelNotSupported);
                        return (false, null);
                    }
                    instrument = pma;
                    break;
                case EnumInstrumentCategory.SMU:
                    StandaloneSmu smu = StandaloneSmuFactory.CreateInstance(instrumentId, vendor, model);
                    if (smu == null) {
                        LOGGER.Error(errorTextModelNotSupported);
                        return (false, null);
                    }
                    instrument = smu;
                    break;
                case EnumInstrumentCategory.ALTIMETER:
                    StandaloneAm am = StandaloneAmFactory.CreateInstance(instrumentId, vendor, model); 
                    if (am == null) {
                        LOGGER.Error(errorTextModelNotSupported);
                        return (false, null);
                    }
                    instrument = am;
                    break;
                case EnumInstrumentCategory.TEC:
                    StandaloneTec tec = StandaloneTecFactory.CreateInstance(instrumentId, vendor, model);   
                    if (tec == null) {
                        LOGGER.Error(errorTextModelNotSupported);
                        return (false, null);
                    }
                    instrument = tec;
                    break;
                case EnumInstrumentCategory.CCD:
                    StandaloneCamera ccd = StandaloneCameraFactory.CreateInstance(instrumentId, vendor, model);
                    if (ccd == null) {
                        LOGGER.Error(errorTextModelNotSupported);
                        return (false, null);
                    }
                    instrument = ccd;
                    break;
                case EnumInstrumentCategory.ELENS:
                    StandaloneElecLens elens = StandaloneElecLensFactory.CreateInstance(instrumentId, vendor, model);   
                    if (elens == null) {
                        LOGGER.Error(errorTextModelNotSupported);
                        return (false, null);
                    }
                    instrument = elens;
                    break;
                case EnumInstrumentCategory.LED:
                    StandaloneLed led = StandaloneLedFactory.CreateInstance(instrumentId, vendor, model);
                    if (led == null)
                    {
                        LOGGER.Error(errorTextModelNotSupported);
                        return (false, null);
                    }
                    instrument = led;
                    break;
                default:
                    throw new UnsupportedInstrumentException(vendor, model);
            }

            instrument.IntrumentCategory = category;
            instrument.CurrentModel = model;
            return (true, instrument);
        }

        public static (bool isOk, Instrument instrument) CreateVirtualInstrument(string instrumentId, EnumInstrumentCategory category, EnumInstrumentVendor vendor, string model) {
            Instrument instrument = null;
            switch (category) {
                case EnumInstrumentCategory.OPM:
                    VirtualOpmWithTriggerIn virtualOpm = new VirtualOpmWithTriggerIn(instrumentId);
                    instrument = virtualOpm;
                    break;
                //case EnumInstrumentCategory.TLS:
                //    VirtualTls virtualTls = new VirtualTls();
                //    instrument = virtualTls;
                //    break;
                //case EnumInstrumentCategory.OS:
                //    VirtualOs virtualOs = new VirtualOs();
                //    instrument = virtualOs;
                //    break;
                //case EnumInstrumentCategory.OA:
                //    VirtualOa virtualOa = new VirtualOa();
                //    instrument = virtualOa;
                //    break;
                case EnumInstrumentCategory.PMA:
                    VirtualPma virtualPma = new VirtualPma(instrumentId);
                    instrument = virtualPma;
                    break;
                case EnumInstrumentCategory.SMU:
                    VirtualSmu virtualSmu = new VirtualSmu(instrumentId);
                    instrument = virtualSmu;
                    break;
                case EnumInstrumentCategory.ALTIMETER:
                    VirtualAm virtualAm = new VirtualAm(instrumentId);
                    instrument = virtualAm;
                    break;
                case EnumInstrumentCategory.TEC:
                    VirtualTec virtualTec = new VirtualTec(instrumentId);
                    instrument = virtualTec;
                    break;
                case EnumInstrumentCategory.CCD:
                    VirtualCamera virtualCamera = new VirtualCamera(instrumentId);
                    instrument = virtualCamera; 
                    break;
                case EnumInstrumentCategory.ELENS:
                   VirtualElecLens virtualEL = new VirtualElecLens(instrumentId);
                    instrument = virtualEL;
                    break;
                default:
                    LOGGER.Error($"virtual instrument category={category} does not be supported!");
                    return (false, null);
            }

            instrument.IntrumentCategory = category;
            instrument.CurrentModel = model;

            return (true, instrument);
        }

        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
