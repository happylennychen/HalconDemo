using System;
using System.Collections.Concurrent;
using System.Xml.Linq;

using CommonApi.MyConstant;
using CommonApi.MyUtility;

namespace ProberApi.MyQuery {
    public sealed class QuerySerialNumber : AbstractQuery {
        public QuerySerialNumber(ConcurrentDictionary<string, object> sharedObjects) : base(sharedObjects) {
            this.QueryType = CommonQueryType.SERIAL_NUMBER;
        }

        public override (bool isOk, string result) Query(string parameter) {
            try {
                XElement xeRoot = XElement.Load(@"Configuration/config_static_information.xml");
                XElement xeSerialNumber = xeRoot.Element("serial_number");
                string serialNumber = xeSerialNumber.Value.Trim();
                if (string.IsNullOrEmpty(serialNumber)) {
                    LOGGER.Error($"In Configuration/config.xml, <serial_number> should not be empty!");
                    return (false, string.Empty);
                }

                return (true, serialNumber);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return (false, string.Empty);
            }
        }
    }
}
