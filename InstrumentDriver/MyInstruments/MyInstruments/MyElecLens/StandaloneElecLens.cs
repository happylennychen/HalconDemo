using MyInstruments.MyVisaDriver;

namespace MyInstruments.MyElecLens
{
    public abstract class StandaloneElecLens:Instrument,IElecLens
    {
        public StandaloneElecLens(string id) : base(id) { }

        public abstract bool Home();

        public abstract bool SetZoom(double value);

        public abstract bool GetZoom(out double value);

        public abstract bool IsMoveComplete();
    }
}
