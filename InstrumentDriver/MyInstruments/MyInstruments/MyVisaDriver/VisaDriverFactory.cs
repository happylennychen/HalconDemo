namespace MyInstruments.MyVisaDriver {
    public static class VisaDriverFactory {
        public static IVisaDriver CreateInstance() {
            return new NiVisaDriverImpl();
        }
    }
}
