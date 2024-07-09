using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using MyInstruments.MyEnum;
using MyInstruments.MyVisaDriver;

namespace MyInstruments.MyElecLens
{
    public sealed class ElecLensPomeas:StandaloneElecLens
    {
        private string visaAddress = string.Empty;
        public ElecLensPomeas(string id):base(id)
        {
            Vendor = EnumInstrumentVendor.POMEAS.ToString();
            SupportedModels = new HashSet<string> { "_POMEAS" };
        }

        public override void Connect(string visaResource)
        {
            LOGGER.Debug($"call ElecLensPomeas.Connect()");

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
            LOGGER.Debug($"call ElecLensPomeas.Disconnect()");
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel)
        {
            ;
        }

        public override bool Home()
        {
            int sendlen = 3;
            byte[] cmd = new byte[sendlen];
            string scmd = $"XH" + "\r";
            for (int i = 0; i < sendlen; i++)
            {
                cmd[i] = (byte)scmd[i];
            }
            _serial.Write(cmd, 0, cmd.Length);

            return true;
        }

        public override bool SetZoom(double value)
        {
            int sendlen = 9;
            int iMultiple = (int)value;
            byte[] cmd = new byte[sendlen];
            string scmd = $"XG" + iMultiple.ToString("x6") + "\r";
            scmd.Replace('0', ' ');
            for (int i = 0; i < sendlen; i++)
            {
                cmd[i] = (byte)scmd[i];
            }

            _serial.Write(cmd, 0, cmd.Length);

            return true;
        }

        public override bool GetZoom(out double valRet)
        {
            lock(mutex)
            {
                valRet = 0;
                int recvlen = 8;
                int sendlen = 3;
                int dataLen = 5;

                byte[] cmd = new byte[sendlen];
                byte[] recv = new byte[recvlen];

                string scmd = $"XN" + "\r";
                for (int i = 0; i < sendlen; i++)
                {
                    cmd[i] = (byte)scmd[i];
                }
                //visaDriver.WriteBytes(cmd);
                _serial.Write(cmd, 0, cmd.Length);
                Thread.Sleep(100);
                //recv = visaDriver.ReadBytes(recvlen);
                _serial.Read(recv, 0, recvlen);
                byte[] data = new byte[dataLen];
                Buffer.BlockCopy(recv, 2, data, 0, dataLen);
                if (!convertToDouble(data, out double val))
                {
                    return false;
                }
                valRet = (int)((val - 500) * 1.3);

                return true;
            }            
        }

        bool convertToDouble(byte[] data, out double value)
        {
            value = 0;
            int dataLen = data.Length;
            int temp = 0;
            for (int i = 0; i < dataLen; i++)
            {
                if (data[i] >= 0x30 && data[i] <= 0x39)
                {
                    temp = data[i] - 0x30;
                }
                else if (data[i] >= 0x41 && data[i] <= 0x46)
                {
                    temp = data[i] - 0x41 + 10;
                }
                else if (data[i] == 0x20)
                {
                    temp = 0;
                }
                else
                {
                    return false;
                }

                value += temp * Math.Pow(16, dataLen - i - 1);
            }

            return true;
        }

        public override bool IsMoveComplete()
        {
            int recvlen = 1;
            int sendlen = 3;
            byte[] cmd = new byte[sendlen];
            byte[] recv = new byte[recvlen];

            string scmd = $"XZ" + "\r";
            for (int i = 0; i < sendlen; i++)
            {
                cmd[i] = (byte)scmd[i];
            }
            //visaDriver.WriteBytes(cmd);
            _serial.Write(cmd, 0, cmd.Length);
            Thread.Sleep(100);

            try
            {
                //recv = visaDriver.ReadBytes(recvlen);
                _serial.Read(recv, 0, recvlen);
                if (recv[0] == 0x55)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //异常不用处理，超时报错即可
            catch
            {
                return false;
            }
        }

        private SerialPort _serial = new SerialPort();
        private readonly object mutex = new object();
    }
}
