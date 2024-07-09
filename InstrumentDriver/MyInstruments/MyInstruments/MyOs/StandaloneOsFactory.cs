using MyInstruments.MyAltimeter;
using MyInstruments.MyEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInstruments.MyOs
{
    public static class StandaloneOsFactory
    {
        public static StandaloneOs CreateInstance(string instrumentId, string vendor, string model)
        {
            StandaloneOs result = null;
            if (!Enum.TryParse(vendor, out EnumInstrumentVendor enumVendor))
            {
                throw new UnsupportedOsException(vendor, model);
            }

            switch (enumVendor)
            {
                case EnumInstrumentVendor.JDSU:
                    {
                        if (!Enum.TryParse(model, out EnumModelJdsu enumModelJdsu))
                        {
                            throw new UnsupportedOsException(vendor, model);
                        }

                        switch (enumModelJdsu)
                        {
                            case EnumModelJdsu._SB:
                                result = new OsJdsuSB(instrumentId);
                                break;
                            default:
                                throw new UnsupportedOsException(vendor, model);

                        }
                        break;
                    }               
                default:
                    throw new UnsupportedAltimeterException(vendor, model);
            }

            return result;
        }
    }
}
