using System.Collections.Generic;

namespace ProberApi.MyConstant {
    public static class CommonRedGreenLigthKey {
        public const string CONNECTED_ALL_INSTRUMENT = "Connected all allInstruments";

        public static List<string> GetAllKeys() {
            var result = new List<string>() {
                //CONNECTED_ALL_INSTRUMENT,
            };

            return result;
        }
    }
}
