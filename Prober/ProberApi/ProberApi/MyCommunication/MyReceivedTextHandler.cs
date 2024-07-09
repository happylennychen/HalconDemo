namespace ProberApi.MyCommunication {
    public interface MyReceivedTextHandler {
        void PrepareHandling();
        string Handle(string receivedText);
        void AbortHandling();
    }
}
