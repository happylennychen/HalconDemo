using Ivi.Visa;

using NationalInstruments.Visa;
using System;
using System.Threading;
using static System.Collections.Specialized.BitVector32;

namespace MyInstruments {
    public class NiVisaDriverImpl : IVisaDriver {
        internal NiVisaDriverImpl() { }

        public void Connect(string visaResource) {
            ResourceManager rm = new ResourceManager();
            session = (MessageBasedSession)rm.Open(visaResource);            
        }

        public void Disconnect() {
            lock (mutex) {                
                session.Clear();
                session.Dispose();
                session = null;
            }
        }

        public void EnableTerminationCharacter(bool isEnabled) {
            session.TerminationCharacterEnabled = isEnabled;
        }
        /*
                public string QueryLine(string content) {
                    session.FormattedIO.WriteLine(content);
                    return session.FormattedIO.ReadLine().Trim();
                }
        */
        public string QueryLine(string content)
        {
            lock (mutex)
            {
                session.RawIO.Write(content);
                System.Threading.Thread.Sleep(10);
                return session.RawIO.ReadString();
            }
        }

        public byte[] QueryBytes(byte[] content, int numberToRead)
        {
            lock (mutex)
            {
                session.RawIO.Write(content);
                Thread.Sleep(50); 
                return session.RawIO.Read(numberToRead, out ReadStatus status);
            }
        }

        public byte[] ReadBytes(int numberToRead) {            
            lock (mutex) {
                return session.RawIO.Read(numberToRead, out ReadStatus status);
            }            
        }

        public byte[] ReadBytes() {
            lock (mutex) {
                return session.RawIO.Read();
            }
        }

        /*
                public string ReadLine() {
                    return session.FormattedIO.ReadLine();
                }
        */
        public string ReadLine()
        {
            lock (mutex)
            {
                return session.RawIO.ReadString();
            }
        }

        public void SetTerminationCharacter(byte terminationChar) {
            session.TerminationCharacter = terminationChar;
        }

        public void SetTimeoutInS(int timeoutInS) {
            session.TimeoutMilliseconds = timeoutInS * 1000;
        }

        public void WaitForOperationCompleted() {
            lock (mutex) {
                while (true) {
                    Thread.Sleep(100);

                    string result = QueryLine("*OPC?").Trim();
                    if (string.Equals("1", result, StringComparison.InvariantCultureIgnoreCase)) {
                        break;
                    }
                }
            }
        }

        public void WriteBytes(byte[] content) {
            lock (mutex) {
                session.RawIO.Write(content);
            }
        }

        /*
                public void WriteLine(string content) {
                    lock (mutex) {
                        session.FormattedIO.WriteLine(content);
                    }            
                }
        */
        public void WriteLine(string content)
        {
            lock (mutex)
            {
                session.RawIO.Write(content);
            }
        }
        /*
                public void SetComBaudRate(string rate)
                {
                    lock (mutex)
                    {
                        session.FormattedIO.WriteLine($"BAUD {rate}");
                    }
                }

                public void SetComDataBit(string bitNum)
                {
                    lock (mutex)
                    {
                        session.FormattedIO.WriteLine($"DATA {bitNum}");
                    }
                }

                public void SetComStopBit(string bitNum)
                {
                    lock (mutex)
                    {
                        session.FormattedIO.WriteLine($"STOP {bitNum}");
                    }
                }

                public void SetComParityBit(string bitNum)
                {
                    lock (mutex)
                    {
                        session.FormattedIO.WriteLine($"PARITY {bitNum}");
                    }
                }
        */
        public void SetComBaudRate(string rate)
        {
            lock (mutex)
            {
                session.RawIO.Write($"BAUD {rate}");
            }
        }

        public void SetComDataBit(string bitNum)
        {
            lock (mutex)
            {
                session.RawIO.Write($"DATA {bitNum}");
            }
        }

        public void SetComStopBit(string bitNum)
        {
            lock (mutex)
            {
                session.RawIO.Write($"STOP {bitNum}");
            }
        }

        public void SetComParityBit(string bitNum)
        {
            lock (mutex)
            {
                session.RawIO.Write($"PARITY {bitNum}");
            }
        }

        MessageBasedSession session;
        private readonly object mutex = new object();
    }
}
