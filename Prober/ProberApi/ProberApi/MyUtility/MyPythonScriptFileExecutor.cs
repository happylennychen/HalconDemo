using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

using CommonApi.MyUtility;

using NLog;

namespace ProberApi.MyUtility {
    public sealed class MyPythonScriptFileExecutor {
        public MyPythonScriptFileExecutor(string pythonIntepreterFullPath, string mainPythonScriptFullPath, string runningParameter) {
            this.pythonIntepreterFullPath = pythonIntepreterFullPath;
            this.mainPythonScriptFullPath = mainPythonScriptFullPath;
            this.runningParameter = runningParameter;
        }

        public bool Run() {
            try {
                Process p = new Process();
                p.StartInfo.FileName = pythonIntepreterFullPath;
                if (string.IsNullOrEmpty(runningParameter)) {
                    p.StartInfo.Arguments = mainPythonScriptFullPath;
                } else {
                    string[] argArray = new string[] { mainPythonScriptFullPath, runningParameter };
                    p.StartInfo.Arguments = string.Join(" ", argArray);
                }
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;

                p.OutputDataReceived += P_OutputDataReceived;
                p.ErrorDataReceived += P_ErrorDataReceived;
                stdoutText.Clear();
                stderrText.Clear();

                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit();

                Thread.Sleep(TimeSpan.FromSeconds(3));

                LOGGER.Debug($"stdoutText={stdoutText.ToString()}");
                LOGGER.Debug($"stderrText={stderrText.ToString()}");

                string stdout = stdoutText.ToString().Trim().ToLowerInvariant();
                if (stdout.EndsWith("pass")) {
                    return true;
                } else {
                    return false;
                }
            } catch (Exception ex) {
                LOGGER.Error($"Occurred exception: {ex.Message}");
                return false;
            }
        }

        public void SetRunningParameter(string runningParameter) {
            this.runningParameter = runningParameter;
        }

        public MyPythonScriptFileExecutor DeepCopy() {
            MyPythonScriptFileExecutor result = new MyPythonScriptFileExecutor(pythonIntepreterFullPath, mainPythonScriptFullPath, runningParameter);
            return result;
        }

        private void P_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            if (!string.IsNullOrEmpty(e.Data)) {
                stderrText.AppendLine(e.Data);
            }
        }

        private void P_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            if (!string.IsNullOrEmpty(e.Data)) {
                stdoutText.AppendLine(e.Data);
            }
        }

        private readonly string pythonIntepreterFullPath;
        private readonly string mainPythonScriptFullPath;
        private string runningParameter;
        private readonly StringBuilder stdoutText = new StringBuilder();
        private readonly StringBuilder stderrText = new StringBuilder();
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
