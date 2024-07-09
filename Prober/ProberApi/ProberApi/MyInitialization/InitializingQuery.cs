using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using CommonApi.MyInitialization;
using CommonApi.MyUtility;

using NLog;

using ProberApi.MyQuery;

namespace ProberApi.MyInitialization {
    public sealed class InitializingQuery : IInitializing {
        public List<AbstractQuery> AllSupportedQueries { get; private set; } = new List<AbstractQuery>();

        public InitializingQuery(ConcurrentDictionary<string, object> sharedObjects) {
            this.sharedObjects = sharedObjects;           
        }

        public bool Run() {
            try {
                XElement xeRoot = XElement.Load(@"Configuration/config_query.xml");

                XElement xeCommons = xeRoot.Element("common_queries");
                List<XElement> xeCommonList = xeCommons.Elements("query").ToList();
                List<string> commonFullClassNames = new List<string>();
                foreach (XElement xeCommon in xeCommonList) {
                    string fullClassName = xeCommon.Attribute("full_class_name").Value.Trim();
                    if (string.IsNullOrEmpty(fullClassName)) {
                        LOGGER.Error($"In Configuration/config_query.xml, <common_queries><query full_class_name=>, full_class_name should not be empty!");
                        return false;
                    }

                    commonFullClassNames.Add(fullClassName);
                }

                XElement xePrivates = xeRoot.Element("private_queries");
                List<XElement> xePrivateList = xePrivates.Elements("query").ToList();
                List<string> privateFullClassNames = new List<string>();
                foreach (XElement xePrivate in xePrivateList) {
                    string fullClassName = xePrivate.Attribute("full_class_name").Value.Trim();
                    if (string.IsNullOrEmpty(fullClassName)) {
                        LOGGER.Error($"In Configuration/config_query.xml, <private_queries><query full_class_name=>, full_class_name should not be empty!");
                        return false;
                    }
                    privateFullClassNames.Add(fullClassName);
                }
                
                var reflectResult = ReflectQueries(false, commonFullClassNames);
                if (!reflectResult.isOk) {
                    return false;
                }
                AllSupportedQueries.AddRange(reflectResult.result);

                reflectResult = ReflectQueries(false, privateFullClassNames);
                if (!reflectResult.isOk) {
                    return false;
                }
                AllSupportedQueries.AddRange(reflectResult.result);
                
                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        private (bool isOk, List<AbstractQuery> result) ReflectQueries(bool isFromProberExe, List<string> fullClassNameList) {
            Assembly assembly = null;
            List<AbstractQuery> result = new List<AbstractQuery>();
            try {
                if (isFromProberExe) {
                    string exeFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Prober.exe");
                    assembly = Assembly.LoadFrom(exeFullPath);
                } else {
                    string dllFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProberApi.dll");
                    assembly = Assembly.LoadFrom(dllFullPath);
                }

                HashSet<string> queryTypeSet = new HashSet<string>();
                foreach (string fullClassName in fullClassNameList) {
                    Type type = assembly.GetType(fullClassName);
                    if (type == null) {
                        LOGGER.Error($"Query class(={fullClassName}) does not exist in assembly!");
                        return (false, null);
                    }
                    object[] parameters = new object[] { sharedObjects };
                    object instance = Activator.CreateInstance(type, parameters);
                    if (!(instance is AbstractQuery)) {
                        LOGGER.Error($"Query class(={fullClassName}) is not a sub-class of AbstractQuery!");
                        return (false, null);
                    }

                    AbstractQuery query = instance as AbstractQuery;
                    if (queryTypeSet.Contains(query.QueryType)) {
                        LOGGER.Error($"Query class(={fullClassName}) is duplicated!");
                        return (false, null);
                    }
                    queryTypeSet.Add(query.QueryType);
                    result.Add(query);
                }

                return (true, result);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return (false, null);
            }
        }

        private readonly ConcurrentDictionary<string, object> sharedObjects;            
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
