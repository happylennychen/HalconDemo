using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace CommonApi.MyUtility {
    public static class MyLogUtility {
        public const string DEFAULT_FILE_LOGGER = "DefaultFileLogger";

        public static string GetStackTrace() {
            StackTrace stackTrace = new StackTrace();

            StringBuilder result = new StringBuilder();
            for (int i=0; i< stackTrace.FrameCount-1; i++) {
                StackFrame frame = stackTrace.GetFrame(i);
                MethodBase method = frame.GetMethod();
                string className = method.ReflectedType?.Name;  
                int lineNo = frame.GetFileLineNumber();
                result.AppendLine($"{className}.{method.Name}() [line:{lineNo}]");
            }

            return result.ToString();
        }

        public static string GenerateExceptionLog(Exception ex) {
            return $" occurred exception:{ex.Message}";
        }
    }
}
