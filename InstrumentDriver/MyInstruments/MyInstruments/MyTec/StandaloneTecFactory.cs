using System;
using MyInstruments.MyEnum;
using MyInstruments.MyException;
using MyInstruments.MyPma;

namespace MyInstruments.MyTec
{
    public static class StandaloneTecFactory
    {
        public static StandaloneTec CreateInstance(string instrumentId, string vendor, string model)
        {
            StandaloneTec result = null;

            if (!Enum.TryParse(vendor, out EnumInstrumentVendor enumVendor))
            {
                throw new UnsupportedTecException(vendor, model);
            }

            switch (enumVendor)
            {
                case EnumInstrumentVendor.OMRON:
                {
                    if (!Enum.TryParse(model, out EnumModelOmron enumModelOmron))
                    {
                        throw new UnsupportedTecException(vendor, model);
                    }

                    switch (enumModelOmron)
                    {
                        case EnumModelOmron._5ECC:
                            result = new TecOmron5ECC(instrumentId);
                            break;
                        default:
                            throw new UnsupportedTecException(vendor, model);
                    }
                    break;
                }
                case EnumInstrumentVendor.YUDIAN:
                {
                    if (!Enum.TryParse(model, out EnumModelYuDian enumModelYuDian))
                    {
                        throw new UnsupportedTecException(vendor, model);
                    }

                    switch(enumModelYuDian)
                    {
                        case EnumModelYuDian._AI:
                            result = new TecYuDian(instrumentId);
                            break;
                        default:
                            throw new UnsupportedTecException(vendor, model);
                    }
                    break;
                }
                case EnumInstrumentVendor.THKA: 
                {
                        if (!Enum.TryParse(model, out EnumModelThka enumModelThka)) {
                            throw new UnsupportedTecException(vendor, model);
                        }

                        switch (enumModelThka) {
                            case EnumModelThka._THKA:
                                result = new TecThka(instrumentId);
                                break;
                            default:
                                throw new UnsupportedTecException(vendor, model);
                        }
                        break;
                    }
                default:
                    throw new UnsupportedTecException(vendor, model);
            }

            return result;
        }
    }
}
