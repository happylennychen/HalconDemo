using MyInstruments.MyEnum;
using MyInstruments.MyPma;
using System;

namespace MyInstruments.MyOpm {
    public static class StandaloneOpmFactory {
        public static StandaloneOpm CreateInstance(string instrumentId, string vendor, string model) {
            if (!Enum.TryParse(vendor, out EnumInstrumentVendor enumVendor)) {
                throw new UnsupportedPmaException(vendor, model);
            }

            StandaloneOpm result;
            switch (enumVendor) {
                case EnumInstrumentVendor.KEYSIGHT:
                    if (!Enum.TryParse(model, out EnumModelKeysight enumModelKeysight)) {
                        throw new UnsupportedPmaException(vendor, model);
                    }

                    switch (enumModelKeysight) {
                        case EnumModelKeysight.N774x:
                            result = new OpmKeysightN774x(instrumentId);
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
