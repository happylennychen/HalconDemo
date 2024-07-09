using System;

namespace ProberApi.MyException {
    public sealed class RequestSerialNumberDoesNotExistException : Exception{
        public RequestSerialNumberDoesNotExistException(string requestSerialNum) : base($"Request serial number(={requestSerialNum}) does not exist in RequestStatusBoard") { 
        }
    }
}
