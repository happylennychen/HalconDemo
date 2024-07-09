using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;

using CommonApi.MyConstant;
using CommonApi.MyUtility;

namespace ProberApi.MyQuery {
    public sealed class QuerySoftwareVersion : AbstractQuery {
        public QuerySoftwareVersion(ConcurrentDictionary<string, object> sharedObjects) : base(sharedObjects) {
            this.QueryType = CommonQueryType.SOFTWARE_VERSION;
        }

        public override (bool isOk, string result) Query(string parameter) {
            try {
                string proberApiDllFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Prober.exe");
                Assembly assembly = Assembly.LoadFrom(proberApiDllFullPath);                
                Version version = assembly.GetName().Version;
                return (true, version.ToString());
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return (false, string.Empty);
            }
        }
    }
}
