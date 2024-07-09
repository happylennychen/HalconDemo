using MyInstruments.MyEnum;
using MyInstruments.MyPma;
using System;

namespace MyInstruments.MySmu {
    public static class StandaloneSmuFactory {
        public static StandaloneSmu CreateInstance(string instrumentId, string vendor, string model) {
            StandaloneSmu result = null;

            if (!Enum.TryParse(vendor, out EnumInstrumentVendor enumVendor)) {
                throw new UnsupportedPmaException(vendor, model);
            }

            switch (enumVendor) {
                case EnumInstrumentVendor.KEITHLEY:
                    if (!Enum.TryParse(model, out EnumModelKeithley enumModelKeithley)) {
                        throw new UnsupportedPmaException(vendor, model);
                    }

                    switch (enumModelKeithley) {
                        case EnumModelKeithley._2602B:
                            result = new SmuKeithley2602B(instrumentId);
                            break;
                        default:
                            throw new UnsupportedPmaException(vendor, model);
                    }
                    break;
                default:
                    throw new UnsupportedPmaException(vendor, model);
            }

            return result;
        }
    }
}
