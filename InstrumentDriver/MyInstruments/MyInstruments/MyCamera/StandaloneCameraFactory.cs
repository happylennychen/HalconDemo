using MyInstruments.MyEnum;
using System;

namespace MyInstruments.MyCamera
{
    public static class StandaloneCameraFactory
    {
        public static StandaloneCamera CreateInstance(string instrumentId, string vendor, string model)
        {
            USBCamera result = null;

            if (!Enum.TryParse(vendor, out EnumInstrumentVendor enumVendor))
            {
                throw new UnsupportedCameraException(vendor, model);
            }

            switch (enumVendor)
            {
                case EnumInstrumentVendor.DAHENG:
                { 
                    if (!Enum.TryParse(model, out EnumModelDH enumModelDH))
                    {
                        throw new UnsupportedCameraException(vendor, model);
                    }

                    switch(enumModelDH)
                    {
                        case EnumModelDH._DH:
                            result = new USBCamera(instrumentId);
                            break;
                        default:
                            throw new UnsupportedCameraException(vendor, model);
                    }
                    break;
                }                   
                default:
                    throw new UnsupportedCameraException(vendor, model);
            }
            return result;
        }
    }
}
