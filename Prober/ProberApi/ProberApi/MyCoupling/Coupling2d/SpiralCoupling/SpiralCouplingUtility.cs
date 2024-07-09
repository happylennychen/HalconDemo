using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using CommonApi.MyUtility;

using NLog;

namespace ProberApi.MyCoupling.Coupling2d.SpiralCoupling {
    public static class SpiralCouplingUtility {        
        public static bool SaveRawData(List<(string firstAxisName, string secondAxisName, List<(double absPos1stAxis, double absPos2ndAxis, double feedback)> rawData)> rowDataList) {            
            if (rowDataList.Count == 0) {
                LOGGER.Info("No spiral coupling data needs to be saved!");
                return true;
            }

            StringBuilder content = new StringBuilder();
            foreach (var rawData in rowDataList) {
                content.AppendLine("");
                string title = $"{rawData.firstAxisName},{rawData.secondAxisName},feedback";
                content.AppendLine(title);
                foreach (var one in rawData.rawData) {
                    string line = $"{one.absPos1stAxis.ToString("0.00")},{one.absPos2ndAxis.ToString("0.00")},{one.feedback.ToString("0.000000000")}";
                    content.AppendLine(line);
                }
            }                       
            
            string csvPath = $"CouplingRawData/spiral_2d_{DateTime.Now.ToString("yyyyMMdd_HHmmss_ffffff")}.csv";
            try {
                StreamWriter streamWriter = new StreamWriter(csvPath, true, Encoding.UTF8);
                using (streamWriter) {
                    streamWriter.WriteLine(content.ToString());
                }

                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
