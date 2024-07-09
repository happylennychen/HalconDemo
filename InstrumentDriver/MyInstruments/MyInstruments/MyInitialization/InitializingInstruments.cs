using CommonApi.MyInitialization;

using MyInstruments.MyUtility;

namespace MyInstruments.MyInitialization {
    public sealed class InitializingInstruments : IInitializing {
        public InstrumentUtility MyInstrumentsUtility { get; } = new InstrumentUtility();

        public bool Run() {
            return MyInstrumentsUtility.Initialize();
        }
    }
}
