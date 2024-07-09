using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using MyInstruments.MyEnum;
using MyInstruments.MyVisaDriver;
using MyInstruments.MyFunc;
using System.Linq;

namespace MyInstruments.MyAltimeter
{
    public sealed class AmKunHang:StandaloneAm
    {
        public string visaAddress = string.Empty;
        public AmKunHang(string id) : base(id)
        {
            Vendor = EnumInstrumentVendor.KH.ToString();
            SupportedModels = new HashSet<string> { "_AQE" };
        }

        /*
         模块默认485参数：设备地址1,9600，N，8,1
         */
        public override void Connect(string visaResource)
        {
            visaAddress = visaResource;
            LOGGER.Debug($"call AmKunHang.Connect({visaResource})");
            this.visaDriver = VisaDriverFactory.CreateInstance();
            this.visaDriver.Connect(visaResource);
            this.visaDriver.SetTimeoutInS(30);
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            
            /*
            ComConfig comConifg = new ComConfig("9600", "8", "1", "N");
            this.visaDriver.SetComBaudRate(comConifg.BaudRate);
            this.visaDriver.SetComDataBit(comConifg.DataBit);
            this.visaDriver.SetComStopBit(comConifg.StopBits);
            this.visaDriver.SetComParityBit(comConifg.ParityBit);
            this.visaDriver.SetTimeoutInS(3);
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            */
        }

        public override void Disconnect()
        {
            LOGGER.Debug($"call AmKunHang.Disconnect()");
            this.visaDriver?.Disconnect();
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel)
        {
            ;
        }

        /*
        public override double GetHeight()
        {
            byte[] bs = new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, 0x00, 0x60, 0x00, 0x10 };

            visaDriver.WriteBytes(bs);
            Thread.Sleep(100);
            string dataRead = visaDriver.ReadLine();
            byte[] retTemp = Encoding.Unicode.GetBytes(dataRead);
            short sTemp = (short)(retTemp[9] << 8 + retTemp[10]);

            //计算电压值->高度值
            double temp = sTemp * 500 / 10000;
            return temp;
        }
        */

#if false
        public override bool GetHeight(int port, out double height)
        {
            height = double.NaN;
            if (port <= 0 || port > 16) {
                throw new ArgumentOutOfRangeException("GetHeight port {port} is out of range");
            }

            byte[] bs = new byte[] { 0x01, 0x03, 0x00, 0x60, 0x00, 0x10, 0x00, 0x00};

            byte[] bTemp = new byte[6];
            for (int i = 0; i < 6; i++)
            {
                bTemp[i] = bs[i];
            }

            //添加校验和
            UInt16 crcPoint = CommonFunc.CRC16(bTemp, 6);
            bs[6] = (byte)((crcPoint & 0xFF00) >> 8);
            bs[7] = (byte)((crcPoint & 0x00FF));

            try {
                //读取温度AD值
                visaDriver.WriteBytes(bs);
                Thread.Sleep(100);
                //5+32;

                //byte[] retPoint = visaDriver.ReadBytes(5);

                //if (retPoint[1] == 0x03) {
                //    byte[] retPointCrc = visaDriver.ReadBytes(32);
                //    byte[] retTemp = retPoint.Concat(retPointCrc).ToArray();
                byte[] retTemp = visaDriver.ReadBytes(37);
                List<double> tempList = new List<double>();

                    for (int i = 0; i < 16; i++) {
                        short sTemp = (short)((short)(retTemp[i * 2 + 3] << 8) + retTemp[i * 2 + 4]);
                        //电压转为V
                        double vol = sTemp / 1000.0;
                        //计算电压值->高度值
                        double temp = vol * 500 / 10;

                        tempList.Add(temp);
                    }

                    height = tempList[port - 1];
                    return true;
                    /*
                } else {
                    LOGGER.Debug("call KunHange Get Hegith Failed，try connect");
                    Disconnect();
                    Thread.Sleep(200);
                    Connect(visaAddress);
                    return false;
                }
                    */
            }
            catch(Exception ex) {
                LOGGER.Debug($"call KunHange Get Hegith Exception {ex.Message}，try connect");
                Disconnect();
                Thread.Sleep(200);
                Connect(visaAddress);
                return false;
            }            
        }
#endif
        public override bool GetHeight(int port, out double height)
        {
            height = double.NaN;
            int readDataLen = 41;
            if (port <= 0 || port > 16)
            {
                throw new ArgumentOutOfRangeException("GetHeight port {port} is out of range");
            }

            byte[] bs = new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x06,0x01, 0x03, 0x00, 0x60, 0x00, 0x10};

            lock (mutex) {
                try {
#if true
                    //读取温度AD值
                    visaDriver.WriteBytes(bs);
                    Thread.Sleep(10);
                    byte[] retTemp = visaDriver.ReadBytes(readDataLen);
#endif
                    //byte[] retTemp = visaDriver.QueryBytes(bs, readDataLen);

                    List<double> tempList = new List<double>();

                    if (retTemp.Length != readDataLen) {
                        LOGGER.Debug($"call KunHange Get Hegith Length Error {retTemp.Length}");
                        return false;
                    }

                    for (int i = 0; i < 16; i++) {
                        short sTemp = (short)((short)(retTemp[i * 2 + 9] << 8) + retTemp[i * 2 + 10]);
                        //电压转为V
                        double vol = sTemp / 1000.0;
                        //计算电压值->高度值
                        double temp = vol * 500 / 10;

                        tempList.Add(temp);
                    }

                    height = tempList[port - 1];
                    return true;
                } catch (Exception ex) {
                    LOGGER.Debug($"call KunHange Get Hegith Exception {ex.Message}，try connect");

                    Disconnect();
                    Thread.Sleep(200);
                    Connect(visaAddress);
                    return false;
                }
            }
        }



        public override bool EnterMeasureMode()
        {
            return true;
        }

        private IVisaDriver visaDriver;
        private readonly object mutex = new object();
    }
}
