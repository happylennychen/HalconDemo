using System;

namespace CommonApi.MyCommunication {
    public sealed class MyRemoteDisconnectedException : Exception {
        public MyRemoteDisconnectedException() : base("Remote endpoint has closed current connection.") { }
    }
}
