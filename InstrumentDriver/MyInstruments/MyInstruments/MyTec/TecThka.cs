using MyInstruments.MyEnum;
using MyInstruments.MyFunc;
using MyInstruments.MyVisaDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;

namespace MyInstruments.MyTec {
    public sealed class TecThka : StandaloneTec {
        private string visaAddress = string.Empty;
        public TecThka(string id) : base(id) {
            Vendor = EnumInstrumentVendor.THKA.ToString();
            SupportedModels = new HashSet<string> { "_THKA" };
        }

        /*
            _serial.BaudRate = 9600;
            _serial.StopBits = StopBits.Two;
            _serial.DataBits = 8;
            _serial.Parity = Parity.None;
        */
        public override void Connect(string visaResource) {
            LOGGER.Debug($"call TecThka.Connect()");

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

        public override void Disconnect() {
            _serial.Close();
            LOGGER.Debug($"call TecThka.Disconnect()");            
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel) {
            ;
        }

        public override bool SetTecEnable(int port, bool isEnable) {
            return true;
        }

        public override bool GetTecEnable(int port, out bool isEnable) {
            isEnable = false;
            return true;
        }

        public override bool SetTemp(int port, double temperature) {
            return true;
        }

        public override bool GetTempSet(int port, out double temperature) {
            temperature = 0.0;
            return true;
        }

        public override bool GetTempAll(out List<double> tempList) {
            tempList = new List<double>();
            lock (mutex) {
                byte[] bCmdReadTemp = new byte[] { 0x01, 0x04, 0x00, 0x00, 0x00, 0x04, 0xF1, 0xC9 };              

                try {
                    /*
                    _serial.Write(bCmdReadTemp, 0, bCmdReadTemp.Length);
                    Thread.Sleep(50);
                    byte[] res = new byte[13];
                    _serial.Read(res, 0, res.Length);
                    Thread.Sleep(50);
                    */
                    _serial.Write(bCmdReadTemp, 0, bCmdReadTemp.Length);
                    Thread.Sleep(10);
                    int totalCount = 13;
                    byte[] res = new byte[totalCount];
                    byte[] dataReal = new byte[totalCount];
                    int curCountSum = 0;
                    //此处需要特殊处理
                    for (int i = 0; i < 10; i++) {
                        int count = _serial.Read(res, 0, res.Length);
                        Array.Copy(res, 0, dataReal, curCountSum, count);
                        curCountSum += count;
                        if (curCountSum == totalCount) {
                            break;
                        } else {
                            ;
                        }
                    }
                    Thread.Sleep(10);

                    for (int port = 1; port <= 4; port++) {
                        int dataTemp = (port - 1) * 2 + 3;
                        short sTemp = (short)((short)(dataReal[dataTemp] << 8) + dataReal[dataTemp + 1]);
                        //计算温度值
                        tempList.Add(sTemp / 10.0);
                    }                   

                    return true;
                } catch (Exception ex) {
                    LOGGER.Debug($"call TecThka Get Temp Exception {ex.Message} ，try connect");
                    Disconnect();
                    Thread.Sleep(200);
                    Connect(visaAddress);
                }

                return false;
            }
        }

        public override bool GetTemp(int port, out double temperature) {
            lock (mutex) {
                temperature = double.NaN;
                byte[] bCmdReadTemp = new byte[] { 0x01, 0x04, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00 };

                bCmdReadTemp[3] = (byte)(port - 1);

                //计算校验和
                byte[] bTempPoint = new byte[6];
                for (int i = 0; i < 6; i++) {
                    bTempPoint[i] = bCmdReadTemp[i];
                }

                UInt16 crcPoint = CommonFunc.CRC16(bTempPoint, 6);
                bCmdReadTemp[6] = (byte)((crcPoint & 0xFF00) >> 8);
                bCmdReadTemp[7] = (byte)((crcPoint & 0x00FF));

                if (port > 4) {
                    LOGGER.Error($"call TecThka Error Port Wrong {port}");
                    return false;
                }

                try {
                    /*
                    _serial.Write(bCmdReadTemp, 0, bCmdReadTemp.Length);
                    Thread.Sleep(50);
                    byte[] res = new byte[13];
                    _serial.Read(res, 0, res.Length);
                    Thread.Sleep(50);
                    */
                    _serial.Write(bCmdReadTemp, 0, bCmdReadTemp.Length);
                    Thread.Sleep(10);
                    int totalCount = 7;
                    byte[] res = new byte[totalCount];
                    byte[] dataReal = new byte[totalCount];
                    int curCountSum = 0;
                    //此处需要特殊处理
                    for (int i = 0; i < 10; i++) {
                        int count = _serial.Read(res, 0, res.Length);
                        Array.Copy(res, 0, dataReal, curCountSum, count);
                        curCountSum += count;
                        if (curCountSum == totalCount) {
                            break;
                        } else {
                            ;
                        }
                    }
                    Thread.Sleep(10);

                    short sTemp = (short)((short)(dataReal[3] << 8) + dataReal[4]);
                    //计算温度值
                    temperature = sTemp / 10.0;

                    return true;                                              
                } catch (Exception ex) {
                    LOGGER.Debug($"call TecThka Get Temp Exception {ex.Message} ，try connect");
                    Disconnect();
                    Thread.Sleep(200);
                    Connect(visaAddress);
                }

                return false;
            }
        }

        public override bool EnableWrite() {
            return true;
        }

        private SerialPort _serial = new SerialPort();
        private readonly object mutex = new object();
    }
}
