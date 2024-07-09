using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using MyInstruments.MyEnum;
using MyInstruments.MyVisaDriver;
using MyInstruments.MyFunc;
using System.IO.Ports;

namespace MyInstruments.MyTec
{
    public sealed class TecOmron5ECC:StandaloneTec
    {
        private string visaAddress = string.Empty;
        public TecOmron5ECC(string id):base(id)
        {
            Vendor = EnumInstrumentVendor.OMRON.ToString();
            SupportedModels = new HashSet<string> { "_5ECC" };
        }

        //此处需要考虑串口通信配置       
        public override void Connect(string visaResource)
        {
            LOGGER.Debug($"call TecOmron5ECC.Connect()");

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
            LOGGER.Debug($"call TecOmron5ECC.Disconnect()");
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel)
        {
            ;
        }

        public override bool SetTemp(int port, double temperature)
        {
           lock (mutex)
            {
                try {
                    //21 03
                    byte[] bCmd = new byte[] { 0x01, 0x10, 0x21, 0x03, 0x00, 0x01, 0x02, 0x00, 0x00, 0x00, 0x00 };

                    //写入温度值
                    int input = (int)(temperature * 10 + 0.1);
                    bCmd[7] = (byte)((input & 0x0000FF00) >> 8);
                    bCmd[8] = (byte)((input & 0x000000FF));

                    //计算校验和
                    byte[] bTempPoint = new byte[9];
                    for (int i = 0; i < 9; i++) {
                        bTempPoint[i] = bCmd[i];
                    }

                    UInt16 crcPoint = CommonFunc.CRC16(bTempPoint, 9);
                    bCmd[9] = (byte)((crcPoint & 0xFF00) >> 8);
                    bCmd[10] = (byte)((crcPoint & 0x00FF));

                    _serial.Write(bCmd, 0, bCmd.Length);
                    Thread.Sleep(50);
                    byte[] res = new byte[256];
                    _serial.Read(res, 0, res.Length);

                    //返回状态码判断
                    return (res[1] == 0x10);
                }catch(Exception ex){
                    LOGGER.Error(ex);   
                    return false;
                }                           
            }              
        }

        public override bool GetTempSet(int port, out double temperature)
        {
            lock (mutex)
            {
                temperature = 0;
                byte[] bCmdReadPoint = new byte[] { 0x01, 0x03, 0x21, 0x03, 0x00, 0x02, 0x00, 0x00 };

                //计算校验和
                byte[] bTempPoint = new byte[6];
                for (int i = 0; i < 6; i++)
                {
                    bTempPoint[i] = bCmdReadPoint[i];
                }

                UInt16 crcPoint = CommonFunc.CRC16(bTempPoint, 6);

                bCmdReadPoint[6] = (byte)((crcPoint & 0xFF00) >> 8);
                bCmdReadPoint[7] = (byte)((crcPoint & 0x00FF));

                try
                {
                    _serial.Write(bCmdReadPoint, 0, bCmdReadPoint.Length);
                    Thread.Sleep(50);
                    byte[] res = new byte[256];
                    _serial.Read(res, 0, res.Length);

                    short sTemp = (short)((short)(res[3] << 8) + res[4]);
                    //计算温度值
                    temperature = sTemp / 10.0;
                    return true;                  
                }
                catch (Exception ex)
                {
                    LOGGER.Debug($"call Omron Get Temp Exception{ex.Message} ，try connect");
                    Disconnect();
                    Thread.Sleep(200);
                    Connect(visaAddress);
                    return false;
                }
            }
        }

        public override bool GetTempAll(out List<double> tempList) {
            tempList = new List<double>();
            return true;
        }

        public override bool GetTemp(int port,out double temperature)
        {
            lock (mutex)
            {               
                temperature = double.NaN;
                byte[] bCmdReadPoint = new byte[] { 0x01, 0x03, 0x24, 0x10, 0x00, 0x01, 0x00, 0x00 };
                byte[] bCmdReadTemp = new byte[] { 0x01, 0x03, 0x20, 0x00, 0x00, 0x01, 0x8F, 0xCA };

                //计算校验和
                byte[] bTempPoint = new byte[6];
                for (int i = 0; i < 6; i++)
                {
                    bTempPoint[i] = bCmdReadPoint[i];
                }

                UInt16 crcPoint = CommonFunc.CRC16(bTempPoint, 6);

                bCmdReadPoint[6] = (byte)((crcPoint & 0xFF00) >> 8);
                bCmdReadPoint[7] = (byte)((crcPoint & 0x00FF));

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

                    short sTemp = (short)((short)(resTemp[3] << 8) + resTemp[4]);
                    //计算温度值
                    temperature = sTemp / Math.Pow(10, pointNum);

                    return true;
                }
                catch (Exception ex)
                {
                    LOGGER.Debug($"call Omron Get Temp Exception{ex.Message} ，try connect");
                    Disconnect();
                    Thread.Sleep(200);
                    Connect(visaAddress);
                    return false;
                }
            }                         
        }

        public override bool GetTecEnable(int port, out bool isEnable)
        {
            lock (mutex)
            {
                isEnable = false;
                byte[] bCmdReadPoint = new byte[] { 0x01, 0x03, 0x24, 0x07, 0x00, 0x02, 0x00, 0x00 };

                //计算校验和
                byte[] bTempPoint = new byte[6];
                for (int i = 0; i < 6; i++)
                {
                    bTempPoint[i] = bCmdReadPoint[i];
                }

                UInt16 crcPoint = CommonFunc.CRC16(bTempPoint, 6);

                bCmdReadPoint[6] = (byte)((crcPoint & 0xFF00) >> 8);
                bCmdReadPoint[7] = (byte)((crcPoint & 0x00FF));

                try
                {
                    _serial.Write(bCmdReadPoint, 0, bCmdReadPoint.Length);
                    Thread.Sleep(50);
                    byte[] res = new byte[256];
                    _serial.Read(res, 0, res.Length);

                    //1是停止，0是运行
                    if ((res[3] & 0x01) == 0)
                    {
                        isEnable = true;
                    }
                    else
                    {
                        isEnable = false;
                    }                  

                    return true;
                }
                catch (Exception ex)
                {
                    LOGGER.Debug($"call Omron Get Temp Exception{ex.Message} ，try connect");
                    Disconnect();
                    Thread.Sleep(200);
                    Connect(visaAddress);
                    return false;
                }
            }                         
        }

        public override bool SetTecEnable(int port, bool isEnable)
        {
            lock (mutex)
            {
                try {
                    //0运行，1停止
                    byte[] cmd = new byte[] { 0x01, 0x06, 0x00, 0x00, 0x01, 0x01, 0x49, 0x9A };

                    cmd[5] = isEnable ? (byte)0 : (byte)1;

                    //计算校验和
                    byte[] bTempPoint = new byte[6];
                    for (int i = 0; i < 6; i++) {
                        bTempPoint[i] = cmd[i];
                    }

                    UInt16 crcPoint = CommonFunc.CRC16(bTempPoint, 6);
                    cmd[6] = (byte)((crcPoint & 0xFF00) >> 8);
                    cmd[7] = (byte)((crcPoint & 0x00FF));

                    _serial.Write(cmd, 0, cmd.Length);
                    Thread.Sleep(50);
                    byte[] res = new byte[256];
                    _serial.Read(res, 0, res.Length);

                    //返回状态码判断
                    return (res[1] == 0x06);
                } catch (Exception ex) {
                    LOGGER.Error(ex);
                    return false;
                }               
            }                      
        }

        public override bool EnableWrite()
        {
            lock(mutex)
            {
                try {
                    //0禁止，1允许
                    byte[] cmd = new byte[] { 0x01, 0x06, 0x00, 0x00, 0x00, 0x01, 0x49, 0x9A };

                    //计算校验和
                    byte[] bTempPoint = new byte[6];
                    for (int i = 0; i < 6; i++) {
                        bTempPoint[i] = cmd[i];
                    }

                    UInt16 crcPoint = CommonFunc.CRC16(bTempPoint, 6);

                    cmd[6] = (byte)((crcPoint & 0xFF00) >> 8);
                    cmd[7] = (byte)((crcPoint & 0x00FF));

                    _serial.Write(cmd, 0, cmd.Length);
                    Thread.Sleep(50);
                    byte[] res = new byte[256];
                    _serial.Read(res, 0, res.Length);

                    // 返回状态码判断
                    return (res[1] == 0x06);
                } catch (Exception ex) {
                    LOGGER.Error(ex);
                    return false;
                }                            
            }            
        }  

        private SerialPort _serial = new SerialPort();
        private readonly object mutex = new object();
    }
}
