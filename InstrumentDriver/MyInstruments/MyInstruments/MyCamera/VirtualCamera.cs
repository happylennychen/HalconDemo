using System;
using System.Collections.Generic;
using System.Threading;
using HalconDotNet;

namespace MyInstruments.MyCamera
{
    public sealed class VirtualCamera:StandaloneCamera
    {
        public VirtualCamera(string id) : base(id)
        {
            Vendor = "Virtual camera vendor";
            SupportedModels = new HashSet<string> { "Virtual camera model" };
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel)
        {
            ;
        }

        public override void Connect(string visaResource)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualAm.Connect({visaResource})");
        }

        public override void Disconnect()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualAm.Disconnect()");
        }

        public override bool SetExposure(double v)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualCamera.SetExposure({v})");
            return true;
        }

        public override bool SetGain(double v)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualCamera.SetGain({v})");
            return true;
        }

        public override bool SetGama(double v)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualCamera.SetGama({v})");
            return true;
        }

        public override bool GetBaseParameters(out double exposure, out double gain, out double gama)
        {
            Random randomExp = new Random();
            double valueExp = randomExp.NextDouble();
            exposure = valueExp;

            Random randomGain = new Random();
            double valueGain = randomGain.NextDouble();
            gain = valueGain;

            Random randomGama = new Random();
            double valueGama = randomGama.NextDouble();
            gama = valueGama;

            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualCamera.GetBaseParameters({valueExp},{valueGain},{valueGama})");
            return true;
        }

        public override bool AutoBalance(bool enable)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualCamera.AutoBalance({enable})");
            return true;
        }

        public override bool SetParameter(string paramName, object value)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualCamera.SetParameter({paramName},{value})");
            return true;
        }

        public override bool GetParameter(string paramName, out object value)
        {
            Random random = new Random();
            value = random.NextDouble();           

            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualCamera.GetParameter({paramName},{value}");
            return true;
        }

        public override bool SaveImage(string file, HObject image = null, ImageFormat format = ImageFormat.jpg, int compressLevel = 0)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualCamera.SaveImage({file})");
            return true;
        }

        public override bool SignlShot(out HObject image)
        {
            image = null;
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualCamera.SignlShot()");
            return true;
        }

        public override void ShowImgeToWindow(HObject image, HTuple wind)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualCamera.ShowImgeToWindow()");
        }

        public override void StartContinuousShot()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualCamera.StartContinuousShot()");
        }

        public override void StopContinuousShot()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualCamera.StartContinuousShot()");
        }

        public override string GetErrorMsg()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualCamera.GetErrorMsg()");
            return _errorMsg;
        }

        public override void SetHeight(int v)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualCamera.SetHeight()");
        }

        public override void SetWidth(int v)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            LOGGER.Info($"call VirtualCamera.SetWidth()");
        }
    }
}
