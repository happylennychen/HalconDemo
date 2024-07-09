namespace MyInstruments {
    public interface IVisaDriver {
        void Connect(string visaResource);
        void Disconnect();
        void WriteLine(string content);
        string ReadLine();
        string QueryLine(string content);
        void WriteBytes(byte[] content);
        byte[] QueryBytes(byte[] content, int numberToRead);
        byte[] ReadBytes(int numberToRead);
        byte[] ReadBytes(); 
        void WaitForOperationCompleted();
        void SetTimeoutInS(int timeoutInS);
        void SetTerminationCharacter(byte terminationChar);
        void EnableTerminationCharacter(bool isEnabled);

        void SetComBaudRate(string rate);
        void SetComDataBit(string bitNum);
        void SetComStopBit(string bitNum);
        void SetComParityBit(string bitNum);
    }
}
