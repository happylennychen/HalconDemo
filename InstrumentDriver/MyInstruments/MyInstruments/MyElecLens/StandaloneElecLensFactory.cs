using MyInstruments.MyEnum;
using System;

namespace MyInstruments.MyElecLens
{
    public static class StandaloneElecLensFactory
    {
        public static StandaloneElecLens CreateInstance(string instrumentId, string vendor, string model)
        {
            StandaloneElecLens result = null;

            if (!Enum.TryParse(vendor, out EnumInstrumentVendor enumVendor))
            {
                throw new UnsupportedElecLensException(vendor, model);
            }

            switch (enumVendor)
            {
                case EnumInstrumentVendor.POMEAS:
                    if (!Enum.TryParse(model, out EnumModelPomeas enumModelPomeas))
                    {
                        throw new UnsupportedElecLensException(vendor, model);
                    }

                    switch(enumModelPomeas)
                    {
                        case EnumModelPomeas._POMEAS:
                            result = new ElecLensPomeas(instrumentId);
                            break;
                        default:
                            throw new UnsupportedElecLensException(vendor, model);
                    }
                    break;
                default:
                    throw new UnsupportedElecLensException(vendor, model);
            }
            return result;
        }
    }
}
