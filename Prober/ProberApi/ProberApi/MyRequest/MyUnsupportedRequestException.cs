using System;

namespace ProberApi.MyRequest {
    public sealed class MyUnsupportedRequestException : Exception{
        public MyUnsupportedRequestException(string RequestType) : base($"Unsupported request type : {RequestType}!") { }
    }
}
