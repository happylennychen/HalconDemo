using MyInstruments.MyEnum;

namespace MyInstruments.MyUtility {
    public sealed class InstrumentConfig {
        public string Id { get; set; }
        public EnumInstrumentCategory Category { get; set; }
        public EnumInstrumentVendor Vendor { get; set; }
        public string Model { get; set; }
        public string VisaResource { get; set; }        
    }
}
