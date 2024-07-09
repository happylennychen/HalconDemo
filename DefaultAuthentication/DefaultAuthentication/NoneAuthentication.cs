using AuthenticationSpi;

namespace DefaultAuthentication {
    public class NoneAuthentication : MyAuthenticate {
        public (bool isOk, MyRole role) Authenticate(string userName, string password) {
            return(true, MyRole.OPERATOR);
        }
    }
}
