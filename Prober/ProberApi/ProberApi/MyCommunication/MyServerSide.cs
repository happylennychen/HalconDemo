using System;
using System.Collections.Concurrent;
using System.Net.Sockets;

using CommonApi.MyUtility;

using NLog;

namespace ProberApi.MyCommunication {
    public abstract class MyServerSide {        
        public abstract void Listen(string localIp, int port, int maxConnectionNum);
        public abstract bool IsListening();
        public abstract void Close();
        public abstract void DiconnectClient();
        public abstract void AbortHandling();

        public MyReceivedTextHandler ReceivedTextHandler { get { return receivedTextHandler; } }
        public Action<string> ReportMessage { get { return reportMessage; } }        
        public MyServerSide(ConcurrentDictionary<string, object> sharedObjects, Action<bool> actSetGuiEnableStatus) {
            this.sharedObjects = sharedObjects;
            this.receivedTextHandler = new MyReceivedTextHandlerImpl(sharedObjects);
            this.actSetGuiEnableStatus = actSetGuiEnableStatus;
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void AttachReportMessage(Action<string> reportMessage) {
            this.reportMessage = reportMessage;
        }

        public abstract MyServerSide DeepCopy();

        protected readonly ConcurrentDictionary<string, object> sharedObjects;
        protected readonly MyReceivedTextHandler receivedTextHandler;
        protected readonly Action<bool> actSetGuiEnableStatus;
        protected static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        protected Action<string> reportMessage;        
        protected Socket serverSocket;
    }
}
