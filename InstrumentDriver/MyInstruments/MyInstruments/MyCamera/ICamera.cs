using HalconDotNet;

namespace MyInstruments.MyCamera
{
    public interface ICamera
    {
        void Connect(string visaResource);
        void Disconnect();
        bool SetExposure(double v);
        bool SetGain(double v);
        bool SetGama(double v);

        bool GetBaseParameters(out double exposure, out double gain, out double gama);

        bool AutoBalance(bool enable);

        bool SetParameter(string paramName, object value);

        bool GetParameter(string paramName, out object value);

        bool SaveImage(string file, HObject image = null, ImageFormat format = ImageFormat.jpg,int compressLevel = 0);

        bool SignlShot(out HObject image);

        void ShowImgeToWindow(HObject image, HTuple wind);

        void StartContinuousShot();

        void StopContinuousShot();

        string GetErrorMsg();

        void SetHeight(int v);

        void SetWidth(int v);
    }

    public enum ImageFormat
    {
        jpg,
        bmp,
        png
    }
}
