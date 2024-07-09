using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;

using CommonApi.MyConstant;
using CommonApi.MyUtility;

namespace ProberApi.MyQuery {
    public sealed class QuerySoftwareFrameworkVersion : AbstractQuery {
        public QuerySoftwareFrameworkVersion(ConcurrentDictionary<string, object> sharedObjects) : base(sharedObjects) {
            this.QueryType = CommonQueryType.SOFTWARE_FRAMEWORK_VERSION;
        }

        public override (bool isOk, string result) Query(string parameter) {
            try {
                string proberApiDllFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProberApi.dll");
                Assembly assembly = Assembly.LoadFrom(proberApiDllFullPath);
                AssemblyName assemblyName = assembly.GetName();
                string version = assemblyName.Version.ToString();
                return (true, version);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return (false, string.Empty);
            }
        }
    }
}
