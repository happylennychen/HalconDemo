using System;
using System.Security.Cryptography;

namespace CommonApi.MyUtility {
    public static class MyRandomUtility {
        public static double GetRealRandomDouble() {
            lock (mutex) {
                Array.Clear(bytesRandom, 0, bytesRandom.Length);
                randomGenerator.GetBytes(bytesRandom);
                UInt32 randomUint = BitConverter.ToUInt32(bytesRandom, 0);
                double randomDouble = randomUint / (UInt32.MaxValue + 1.0);
                return randomDouble;
            }
        }

        private static readonly object mutex = new object();
        private static readonly byte[] bytesRandom = new byte[4];
        private static readonly RandomNumberGenerator randomGenerator = RandomNumberGenerator.Create();
    }
}
