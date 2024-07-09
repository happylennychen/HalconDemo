using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MyInstruments.MyEnum;
using MyInstruments.MyVisaDriver;
using MyInstruments.MyFunc;
using System.IO.Ports;

namespace MyInstruments.MyTec
{
    public sealed class TecYuDian:StandaloneTec
    {
        private string visaAddress = string.Empty;
        public TecYuDian(string id):base(id)
        {
            Vendor = EnumInstrumentVendor.YUDIAN.ToString();
            SupportedModels = new HashSet<string> { "_AI" };
        }

        /*
            _serial.BaudRate = 9600;
            _serial.StopBits = StopBits.Two;
            _serial.DataBits = 8;
            _serial.Parity = Parity.None;
        */
        public override void Connect(string visaResource)
        {
            LOGGER.Debug($"call TecYuDian.Connect()");

            visaAddress = visaResource;
            _serial.BaudRate = 9600;
            _serial.StopBits = StopBits.Two;
            _serial.DataBits = 8;
            _serial.Parity = Parity.None;
            _serial.ReadTimeout = 500;
            _serial.WriteTimeout = 500;

            _serial.PortName = visaResource;
            if (_serial.IsOpen)
                _serial.Close();
            _serial.Open();
         
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
        }

        public override void Disconnect()
        {
            _serial.Close();
            LOGGER.Debug($"call TecYuDian.Disconnect()");
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel)
        {
            ;
        }

        public override bool SetTecEnable(int port, bool isEnable)
        {         
            return true;
        }

        public override bool GetTecEnable(int port, out bool isEnable)
        {
            isEnable = false;
            return true;
        }

        public override bool SetTemp(int port, double temperature)
        {
            return true;
        }

        public override bool GetTempSet(int port, out double temperature)
        {
            temperature = 0.0;
            return true;
        }

        public override bool GetTempAll(out List<double> tempList) {
            tempList = new List<double>();
            return true;
        }

        public override bool GetTemp(int port, out double temperature)
        {
            lock(mutex)
            {
                temperature = double.NaN;
                byte addrTemp = (byte)(port - 1 + 0x80);
                byte addrNum = (byte)((port - 1) * 16 + 0x26);

                byte[] bCmdReadPoint = new byte[] { 0x01, 0x03, 0x00, 0x26, 0x00, 0x01, 0x00, 0x00 };
                byte[] bCmdReadTemp = new byte[] { 0x01, 0x03, 0x00, 0x80, 0x00, 0x01, 0x00, 0x00 };

                bCmdReadPoint[3] = addrNum;
                bCmdReadTemp[3] = addrTemp;

                //计算校验和
                byte[] bTempPoint = new byte[6];
                byte[] bTempTemp = new byte[6];
                for (int i = 0; i < 6; i++)
                {
                    bTempPoint[i] = bCmdReadPoint[i];
                    bTempTemp[i] = bCmdReadTemp[i];
                }

                UInt16 crcPoint = CommonFunc.CRC16(bTempPoint, 6);
                UInt16 crcTemp = CommonFunc.CRC16(bTempTemp, 6);

                bCmdReadPoint[6] = (byte)((crcPoint & 0xFF00) >> 8);
                bCmdReadPoint[7] = (byte)((crcPoint & 0x00FF));

                bCmdReadTemp[6] = (byte)((crcTemp & 0xFF00) >> 8);
                bCmdReadTemp[7] = (byte)((crcTemp & 0x00FF));

                try
                {
                    _serial.Write(bCmdReadPoint, 0, bCmdReadPoint.Length);
                    Thread.Sleep(50);
                    byte[] res = new byte[256];
                    _serial.Read(res, 0, res.Length);
                    int pointNum = res[4];


                    _serial.Write(bCmdReadTemp, 0, bCmdReadTemp.Length);
                    Thread.Sleep(50);
                    byte[] resTemp = new byte[256];
                    _serial.Read(resTemp, 0, resTemp.Length);

                    //读取温度AD值                    
                    short sTemp = (short)((short)(resTemp[3] << 8) + resTemp[4]);
                    //计算温度值
                    temperature = sTemp / Math.Pow(10, pointNum);

                    return true;
                }
                catch (Exception ex)
                {
                    LOGGER.Debug($"call YuDian Get Temp Exception {ex.Message} ，try connect");
                    Disconnect();
                    Thread.Sleep(200);
                    Connect(visaAddress);
                }

                return false;
            }            
        }

        public override bool EnableWrite()
        {
            return true;
        }

        private SerialPort _serial = new SerialPort();
        private readonly object mutex = new object();
    }
}
