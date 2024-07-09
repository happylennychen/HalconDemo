using System;

using MyInstruments.MyEnum;

namespace MyInstruments.MyPma {
    public static class PmaFactory {
        public static Pma CreateInstance(string instrumentId, string vendor, string model) {
            Pma result = null;

            if (!Enum.TryParse(vendor, out EnumInstrumentVendor enumVendor)) {
                throw new UnsupportedPmaException(vendor, model);
            }

            switch (enumVendor) {
                case EnumInstrumentVendor.KEYSIGHT:
                    if (!Enum.TryParse(model, out EnumModelKeysight enumModelKeysight)) {
                        throw new UnsupportedPmaException(vendor, model);
                    }

                    switch (enumModelKeysight) {
                        case EnumModelKeysight._816xB:
                            result = new PmaKeysight816x(instrumentId);
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
