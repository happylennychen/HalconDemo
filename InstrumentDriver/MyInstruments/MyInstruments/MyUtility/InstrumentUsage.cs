using MyInstruments.MyEnum;

namespace MyInstruments.MyUtility {
    public sealed class InstrumentUsage {
        public string UsageId { get; private set; }
        public string InstrumentId { get; private set; }
        public string Slot { get; private set; }
        public string Channel { get; private set; }
        public EnumInstrumentCategory InstrumentCategory { get; private set; }
        public Instrument TheInstrument { get; private set; }

        public InstrumentUsage(string usageId, string instrumentId, string slot, string channel, EnumInstrumentCategory instrumentCategory) {
            this.UsageId = usageId;
            this.InstrumentId = instrumentId;
            this.Slot = slot;
            this.Channel = channel;
            this.InstrumentCategory = instrumentCategory;
            this.TheInstrument = null;
        }

        public void SetInstrument(Instrument instrument) {
            this.TheInstrument = instrument;
        }
    }
}
