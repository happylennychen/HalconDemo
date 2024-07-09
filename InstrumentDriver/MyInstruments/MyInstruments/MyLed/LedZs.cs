using HalconDotNet;
using MyInstruments.MyEnum;
using MyInstruments.MyFunc;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyInstruments.MyLed
{
    public sealed class LedZs : StandaloneLed
    {
        private string visaAddress = string.Empty;
        private SerialPort _serial = new SerialPort();
        private readonly object mutex = new object();

        public LedZs(string id) : base(id)
        {
            Vendor = EnumInstrumentVendor.ZS.ToString();
            SupportedModels = new HashSet<string> { "_ZS" };
        }

        public override void Connect(string visaResource)
        {
            LOGGER.Debug($"call LedZs.Connect()");

            visaAddress = visaResource;
            _serial.BaudRate = 9600;
            _serial.StopBits = StopBits.One;
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
            LOGGER.Debug($"call TecThka.Disconnect()");
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel)
        {
            ;
        }

        public override bool SetValue(int data, byte pointNum)
        {
            //当前版本的LED仅能显示4段数据，范围-999~9999
            if((data > 9999) || (data < -999) || pointNum > 3)
            {
                return false;
            }

            lock (mutex)
            {
                try
                {
                    byte[] bCmd = new byte[] { 0x01, 0x10, 0x00, 0x06, 0x00, 0x02, 0x04, 0x00, 0x00, 0x00, 0x00,0x00,0x00 };

                    //写入显示值
                    int input = Math.Abs(data);
                    bCmd[9] = (byte)((input & 0x0000FF00) >> 8);
                    bCmd[10] = (byte)((input & 0x000000FF));

                    //写入小数点和正负号
                    bCmd[7] = pointNum;
                    if (data < 0) 
                    {
                        bCmd[7] &= 0x1F;
                    }                   

                    //计算校验和
                    byte[] bTempPoint = new byte[11];
                    for (int i = 0; i < 11; i++)
                    {
                        bTempPoint[i] = bCmd[i];
                    }

                    UInt16 crcPoint = CommonFunc.CRC16(bTempPoint, 11);
                    bCmd[11] = (byte)((crcPoint & 0xFF00) >> 8);
                    bCmd[12] = (byte)((crcPoint & 0x00FF));

                    _serial.Write(bCmd, 0, bCmd.Length);
                    Thread.Sleep(50);
                    byte[] res = new byte[256];
                    _serial.Read(res, 0, res.Length);

                    //返回状态码判断
                    return (res[1] == 0x10);
                }
                catch (Exception ex)
                {
                    LOGGER.Error(ex);
                    return false;
                }
            }
        }
    }
}
