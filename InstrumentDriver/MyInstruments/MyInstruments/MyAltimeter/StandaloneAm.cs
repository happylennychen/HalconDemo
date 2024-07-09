namespace MyInstruments.MyAltimeter
{
    public abstract class StandaloneAm: Instrument, IAm
    {
        protected StandaloneAm(string id) : base(id) {  }

        public abstract bool GetHeight(int port, out double height);

        public abstract bool EnterMeasureMode();
    }
}
