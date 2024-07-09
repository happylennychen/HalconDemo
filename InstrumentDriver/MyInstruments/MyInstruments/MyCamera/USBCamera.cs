using System;
using System.Collections.Generic;
using MyInstruments.MyEnum;
using HalconDotNet;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace MyInstruments.MyCamera
{
    public sealed class USBCamera:StandaloneCamera
    {
        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel)
        {
            ;
        }

        public USBCamera(string id) : base(id)
        {
            Vendor = EnumInstrumentVendor.DAHENG.ToString();
            SupportedModels = new HashSet<string> { "_DH" };
        }

        public override void SetHeight(int v)
        {
            Height = v;
        }

        public override void SetWidth(int v)
        {
            Width = v;
        }

        public override void Connect(string visaResource)
        {
            string type = "USB3Vision";
            if (visaResource[visaResource.Length - 2] == 'G')
                type = "GigEVision2";

            my_AcqHandle.Dispose();
            try
            {
                HOperatorSet.OpenFramegrabber(type, 0, 0, 0, 0, 0, 0, "progressive",-1, "default", -1, "false", "default", visaResource, 0, -1, out my_AcqHandle);
                HOperatorSet.SetFramegrabberParam(my_AcqHandle, "GainAuto", "Off");
                HOperatorSet.SetFramegrabberParam(my_AcqHandle, "ExposureAuto", "Off");
                HOperatorSet.SetFramegrabberParam(my_AcqHandle, "AcquisitionFrameRate", 1);
                HOperatorSet.GrabImageStart(my_AcqHandle, -1);
                IsOpen = true;
                AutoBalance(true);
            }
            catch(Exception ex)
            {
                string err = ex.Message;
                return;
            }

        }
        public override void Disconnect()
        {
            if (IsContinous)
            {
                StopContinuousShot();
                Thread.Sleep(200);
            }
            HOperatorSet.CloseFramegrabber(my_AcqHandle);
            my_AcqHandle.Dispose();
            IsOpen = false;
        }

        public override bool SetExposure(double v)
        {
            HOperatorSet.SetFramegrabberParam(my_AcqHandle, ExposureName, v);
            return true;
        }

        public override bool SetGain(double v)
        {
            HOperatorSet.SetFramegrabberParam(my_AcqHandle, GainName, v);
            return true;
        }
        public override bool SetGama(double v)
        {
            HOperatorSet.SetFramegrabberParam(my_AcqHandle, GamaName, v);
            return true;
        }

        public override bool GetBaseParameters(out double exposure, out double gain, out double gama)
        {
            exposure = -1;
            gain = -1;
            gama = -1;

            //exposure
            HOperatorSet.GetFramegrabberParam(my_AcqHandle, ExposureName, out HTuple exValue);
            exposure = exValue.D;

            //gain
            HOperatorSet.GetFramegrabberParam(my_AcqHandle, GainName, out HTuple gainValue);
            gain = gainValue.D;

            //gama
            //HOperatorSet.GetFramegrabberParam(my_AcqHandle, GamaName, out HTuple gamaValue);
            //gama = gamaValue.D;

            return true;
        }

        public override bool AutoBalance(bool enable)
        {
            string cmd = enable ? "Continuous" : "Off";

            HOperatorSet.SetFramegrabberParam(my_AcqHandle, WhiteBalanceName, cmd);
            return true;
        }

        public override bool SetParameter(string paramName, object value)
        {
            HTuple h = new HTuple(value);
            HOperatorSet.SetFramegrabberParam(my_AcqHandle, paramName, h);
            return true;
        }

        public override bool GetParameter(string paramName, out object value)
        {
            HOperatorSet.GetFramegrabberParam(my_AcqHandle, paramName, out HTuple h);
            value = h.O;
            return true;
        }

        public override bool SaveImage(string file, HObject image = null, ImageFormat format = ImageFormat.jpg, int compressLevel = 100)
        {
            if (image == null)
            {
                if (!SignlShot(out image))
                    return false;
            }

            //HOperatorSet.WriteImage(image, format.ToString(), '0', file);
            if (format == ImageFormat.bmp)
            {
                HOperatorSet.WriteImage(image, format.ToString(), '0', file);
            }
            else
            {
                string cmd = format.ToString() + " " + compressLevel.ToString();
                HOperatorSet.WriteImage(image, cmd, '0', file);
            }
            
            return true;
        }

        
        public override bool SignlShot(out HObject image)
        {
            image = new HObject();
            bool isC = IsContinous;
            IsContinous = false;
            lock (_lock)
            {
                try
                {
                    HOperatorSet.GrabImage(out image, my_AcqHandle);

                }
                catch (Exception ex)
                {
                    _errorMsg = "CameraBase.SignleShot() error！" + ex.Message;
                    return false;
                }
            }
            IsContinous = isC;
            return true;
        }

        /*
        public override bool SignlShot(out HObject image) {
            image = new HObject();

            StopContinuousShot(); 
            Thread.Sleep(100);
            
            if(!SignlShotEx(out image)) {
                StartContinuousShot();
                return false;
            }

            HOperatorSet.WriteImage(image, ImageFormat.jpg.ToString(), '0', "C:\\temp.jpg");

            StartContinuousShot();
            return true;
        }
        
        public  bool SignlShotEx(out HObject image) {
            image = new HObject();
            lock (_lock) {
                try {
                    HOperatorSet.GrabImage(out image, my_AcqHandle);
                } catch (Exception ex) {
                    _errorMsg = "CameraBase.SignleShot() error！" + ex.Message;
                    return false;
                }
            }
            return true;
        }
        */

        public override void ShowImgeToWindow(HObject image, HTuple wind)
        {
            HOperatorSet.DispImage(image, wind);
        }

        public override void StartContinuousShot()
        {
            _tokenSource?.Dispose();
            _tokenSource = new CancellationTokenSource();
            HObject image = new HObject();
            Task.Factory.StartNew(() =>
            {
                IsContinous = true;
                while (true)
                {
                    try
                    {
                        if (_tokenSource.IsCancellationRequested)
                            break;
                        if (!IsOpen || !IsContinous)
                        {
                            Thread.Sleep(500);
                            continue;
                        }
                        lock (_lock)
                        {
                            try
                            {
                                image?.Dispose();
                                HOperatorSet.GrabImageAsync(out image, my_AcqHandle, -1);
                                if (ContinuShotFunc != null)
                                    ContinuShotFunc.Invoke(image);
                            }
                            catch (Exception ex)
                            {
                                _errorMsg = "CameraBase.ContinueShot() error！" + ex.Message;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _errorMsg = "CameraBase.ContinueShot() error！" + ex.Message;
                        break;
                    }
                }
            }, _tokenSource.Token);
        }

        public override void StopContinuousShot()
        {
            _tokenSource?.Cancel();
            IsContinous = false;
        }

        public override string GetErrorMsg()
        {
            return _errorMsg;
        }

        private object _lock = new object();
        private CancellationTokenSource _tokenSource;
    }
}
