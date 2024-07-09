using MyInstruments.MyEnum;
using MyInstruments.MyTec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInstruments.MyLed
{
    public static class StandaloneLedFactory
    {
        public static StandaloneLed CreateInstance(string instrumentId, string vendor, string model)
        {
            StandaloneLed result = null;

            if (!Enum.TryParse(vendor, out EnumInstrumentVendor enumVendor))
            {
                throw new UnsupportedLedException(vendor, model);
            }

            switch (enumVendor)
            {
                case EnumInstrumentVendor.ZS:
                    {
                        if (!Enum.TryParse(model, out EnumModelZS enumModel))
                        {
                            throw new UnsupportedLedException(vendor, model);
                        }

                        switch (enumModel)
                        {
                            case EnumModelZS._ZS:
                                result = new LedZs(instrumentId);
                                break;
                            default:
                                throw new UnsupportedLedException(vendor, model);
                        }
                        break;
                    }               
                default:
                    throw new UnsupportedLedException(vendor, model);
            }

            return result;
        }
    }
}
