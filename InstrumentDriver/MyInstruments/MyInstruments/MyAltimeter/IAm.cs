namespace MyInstruments.MyAltimeter
{
    public interface IAm
    {
        bool GetHeight(int port, out double height);

        bool EnterMeasureMode();
    }
}
