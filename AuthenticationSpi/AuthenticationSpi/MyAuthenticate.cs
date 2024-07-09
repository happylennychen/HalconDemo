namespace AuthenticationSpi {
    public interface MyAuthenticate {
        (bool isOk, MyRole role) Authenticate(string userName, string password);
    }
}
