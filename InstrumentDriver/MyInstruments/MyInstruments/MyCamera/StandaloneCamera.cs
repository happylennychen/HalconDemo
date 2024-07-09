using HalconDotNet;
using System;

namespace MyInstruments.MyCamera
{
    public abstract class StandaloneCamera:Instrument,ICamera
    {
        protected StandaloneCamera(string id) : base(id) { }

        public abstract bool SetExposure(double v);
        public abstract bool SetGain(double v);
        public abstract bool SetGama(double v);

        public abstract bool GetBaseParameters(out double exposure, out double gain, out double gama);

        public abstract bool AutoBalance(bool enable);

        public abstract bool SetParameter(string paramName, object value);

        public abstract bool GetParameter(string paramName, out object value);

        public abstract bool SaveImage(string file, HObject image = null, ImageFormat format = ImageFormat.jpg, int compressLevel = 0);

        public abstract bool SignlShot(out HObject image);

        public abstract void ShowImgeToWindow(HObject image, HTuple wind);

        public abstract void StartContinuousShot();

        public abstract void StopContinuousShot();

        public abstract string GetErrorMsg();

        public abstract void SetHeight(int v);

        public abstract void SetWidth(int v);

        protected HTuple my_AcqHandle = new HTuple();
        public bool IsContinous = false;        
        protected string _errorMsg = string.Empty;

        public bool IsOpen = false;
        public int Height = 600;
        public int Width = 800;
        public Action<HObject> ContinuShotFunc;

        public string ExposureName { get; set; } = "ExposureTime";
        public string GainName { get; set; } = "Gain";
        public string GamaName { get; set; } = "Gama";
        public string WhiteBalanceName { get; set; } = "BalanceWhiteAuto";        
    }
}
