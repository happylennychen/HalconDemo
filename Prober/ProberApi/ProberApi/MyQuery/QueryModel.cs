using System;
using System.Collections.Concurrent;
using System.Xml.Linq;

using CommonApi.MyConstant;
using CommonApi.MyUtility;

namespace ProberApi.MyQuery {
    public sealed class QueryModel : AbstractQuery {
        public QueryModel(ConcurrentDictionary<string, object> sharedObjects) : base(sharedObjects) {
            this.QueryType = CommonQueryType.MODEL;
        }

        public override (bool isOk, string result) Query(string parameter) {
            try {
                XElement xeRoot = XElement.Load(@"Configuration/config_static_information.xml");
                XElement xeModel = xeRoot.Element("model");
                string model = xeModel.Value.Trim();
                if (string.IsNullOrEmpty(model)) {
                    LOGGER.Error($"In Configuration/config.xml, <model> should not be empty!");
                    return (false, string.Empty);
                }

                return (true, model);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return (false, string.Empty);
            }
        }
    }
}
