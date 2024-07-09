using System;
using System.Collections.Generic;
using System.Net;

namespace CommonApi.MyUtility {
    public static class MyStaticUtility {
        public static bool IsValidIpV4(string ip) {
            if (!IPAddress.TryParse(ip, out IPAddress ipAddress)) {
                return false;
            }

            return ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;
        }

        public static bool IsLocalIpV4Valid(string ipV4) {
            string hostName = Dns.GetHostName();
            IPAddress[] ipv4Addresses = Array.FindAll(
                Dns.GetHostEntry(hostName).AddressList,
                a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            foreach (IPAddress ipv4Address in ipv4Addresses) {
                if (ipV4.Equals(ipv4Address.ToString())) {
                    return true;
                }
            }

            return false;
        }

        public const int LISTENING_PORT_MIN = 50000;
        public const int LISTENING_PORT_MAX = 65535;
        public static bool IsProberListeningPortValid(int port) {
            return (LISTENING_PORT_MIN <= port) && (port <= LISTENING_PORT_MAX);
        }

        public const double SMALL_ENOUGH_DOUBLE = 1E-6;
        public static bool DoesTwoDoublesEqual(double d1, double d2) {
            return Math.Abs(d1 - d2) <= SMALL_ENOUGH_DOUBLE;
        }

        public static double DbmToMw(double valueInDbm) {
            return Math.Pow(10, (valueInDbm / 10));
        }

        public static double MwToDbm(double valueInMw) {
            return 10 * Math.Log10(valueInMw);
        }

        public static bool IsMonotonicDecreasing<T>(List<T> list) where T : IComparable<T> {
            if (list == null) {
                throw new ArgumentNullException(nameof(list), "Parameter: list should not be null!");
            }

            if (list.Count < 2) {
                return true;
            }

            for (int i = 1; i < list.Count; i++) {
                if (list[i].CompareTo(list[i - 1]) >= 0) {
                    return false;
                }
            }

            return true;
        }

        public static bool IsMonotonicIncreasing<T>(List<T> list) where T : IComparable<T> {
            if (list == null) {
                throw new ArgumentNullException(nameof(list), "Parameter: list should not be null!");
            }

            if (list.Count < 2) {
                return true;
            }

            for (int i = 1; i < list.Count; i++) {
                if (list[i].CompareTo(list[i - 1]) <= 0) {
                    return false;
                }
            }

            return true;
        }
    }
}
