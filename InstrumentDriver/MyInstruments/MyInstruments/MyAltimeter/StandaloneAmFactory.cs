using MyInstruments.MyEnum;
using System;

namespace MyInstruments.MyAltimeter
{
    public static class StandaloneAmFactory
    {
        public static StandaloneAm CreateInstance(string instrumentId, string vendor, string model)
        {
            StandaloneAm result = null;

            if (!Enum.TryParse(vendor, out EnumInstrumentVendor enumVendor))
            {
                throw new UnsupportedAltimeterException(vendor, model);
            }

            switch (enumVendor)
            {
                case EnumInstrumentVendor.KEYENCE:
                {
                    if (!Enum.TryParse(model, out EnumModelKeyence enumModelKeyence))
                    {
                        throw new UnsupportedAltimeterException(vendor, model);
                    }

                    switch (enumModelKeyence)
                    {
                        case EnumModelKeyence._CL3000:
                            result = new AmKeyenceCL3000(instrumentId);
                            break;
                        default:
                            throw new UnsupportedAltimeterException(vendor, model);

                    }
                    break;
                }                                     
                case EnumInstrumentVendor.KH:
                {
                    if (!Enum.TryParse(model, out EnumModelKH enumModelKH))
                    {
                        throw new UnsupportedAltimeterException(vendor, model);
                    }

                    switch (enumModelKH)
                    {
                        case EnumModelKH._AQE:
                            result = new AmKunHang(instrumentId);
                            break;
                        default:
                            throw new UnsupportedAltimeterException(vendor, model);

                    }
                    break;
                }
                case EnumInstrumentVendor.NI:
                    if (!Enum.TryParse(model, out EnumModelNI enumModel))
                    {
                        throw new UnsupportedAltimeterException(vendor, model);
                    }

                    switch (enumModel)
                    {
                        case EnumModelNI._6000:
                        case EnumModelNI._6002:
                            result = new AmNI600X(instrumentId);
                            break;
                        default:
                            throw new UnsupportedAltimeterException(vendor, model);

                    }
                    break;
                default:
                    throw new UnsupportedAltimeterException(vendor, model);
            }

            return result;
        }
    }
}
