namespace SimulatedInstrument {
    public static class CmdHandler {
        public static void Initialize(string idn, Action<string> reportMessage) {
            CmdHandler.idn = idn;
            CmdHandler.reportMessage = reportMessage;
        }

        public static string Handle(string command) {
            if (command.Equals(IDN)) {
                return idn;
            } else if (command.StartsWith(READ_PREFIX)) {
                string actualCmd = command.Substring(READ_PREFIX.Length);
                reportMessage?.Invoke($"[Execute] {actualCmd}");
                Thread.Sleep(TimeSpan.FromMilliseconds(500));
                Random random = new Random();
                double randomValue = random.NextDouble() * 100;
                return $"0,{randomValue}";
            } else if(command.StartsWith(WRITE_PREFIX)) {
                string actualCmd = command.Substring(WRITE_PREFIX.Length);
                reportMessage?.Invoke($"[Execute] {actualCmd}");
                Thread.Sleep(TimeSpan.FromMilliseconds(500));
                return $"0,";
            }

            return string.Empty;
        }

        private static string idn = string.Empty;
        private static Action<string>? reportMessage;
        private const string IDN = "*IDN?";
        private const string WRITE_PREFIX = "*WRITE!";
        private const string READ_PREFIX = "*READ?";
    }
}
