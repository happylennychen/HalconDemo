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

using ProberApi.MyConstant;
using ProberApi.MyRequest;

namespace ProberApi.MyInitialization {
    public sealed class InitializingRequest : IInitializing {
        public InitializingRequest(ConcurrentDictionary<string, object> sharedObjects) {
            this.sharedObjects = sharedObjects;
        }

        public bool Run() {
            try {
                XElement xeRoot = XElement.Load(@"Configuration/config_requests.xml");

                XElement xeCommonRequests = xeRoot.Element("common_requests");
                List<XElement> xeCommonRequestList = xeCommonRequests.Elements("request").ToList();
                List<string> commonFullClassNames = new List<string>();
                foreach (XElement xeCommonRequest in xeCommonRequestList) {
                    string fullClassName = xeCommonRequest.Attribute("full_class_name").Value.Trim();
                    if (string.IsNullOrEmpty(fullClassName)) {
                        LOGGER.Error($"In Configuration/config_requests.xml, <common_requests><request full_class_name=>, full_class_name should not be empty!");
                        return false;
                    }

                    commonFullClassNames.Add(fullClassName);
                }

                XElement xePrivateRequests = xeRoot.Element("private_requests");
                List<XElement> xePrivateRequestList = xePrivateRequests.Elements("request").ToList();
                List<string> privateFullClassNames = new List<string>();
                foreach (XElement xePrivateRequest in xePrivateRequestList) {
                    string fullClassName = xePrivateRequest.Attribute("full_class_name").Value.Trim();
                    if (string.IsNullOrEmpty(fullClassName)) {
                        LOGGER.Error($"In Configuration/config_requests.xml, <private_requests><request full_class_name=>, full_class_name should not be empty!");
                        return false;
                    }
                    privateFullClassNames.Add(fullClassName);
                }

                List<AbstractRequest> allSupportedRequests = new List<AbstractRequest>();
                var reflectResult = ReflectRequests(false, commonFullClassNames);
                if (!reflectResult.isOk) {
                    return false;
                }
                allSupportedRequests.AddRange(reflectResult.requests);

                reflectResult = ReflectRequests(true, privateFullClassNames);
                if (!reflectResult.isOk) {
                    return false;
                }
                allSupportedRequests.AddRange(reflectResult.requests);

                sharedObjects.AddOrUpdate(SharedObjectKey.TEMPLATE_REQUEST_LIST, allSupportedRequests, (key, oldValue) => allSupportedRequests);
                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        private (bool isOk, List<AbstractRequest> requests) ReflectRequests(bool isFromProberExe, List<string> fullClassNameList) {
            Assembly assembly = null;
            List<AbstractRequest> requests = new List<AbstractRequest>();
            try {

                if (isFromProberExe) {
                    string exeFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Prober.exe");
                    assembly = Assembly.LoadFrom(exeFullPath);
                } else {
                    string dllFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProberApi.dll");
                    assembly = Assembly.LoadFrom(dllFullPath);
                }

                HashSet<string> requestTypeSet = new HashSet<string>();
                foreach (string fullClassName in fullClassNameList) {
                    Type type = assembly.GetType(fullClassName);
                    if (type == null) {
                        LOGGER.Error($"Request class(={fullClassName}) does not exist in assembly!");
                        return (false, null);
                    }
                    object[] parameters = new object[] { sharedObjects };
                    object instance = Activator.CreateInstance(type, parameters);
                    if (!(instance is AbstractRequest)) {
                        LOGGER.Error($"Request class(={fullClassName}) is not a sub-class of AbstractRequest!");
                        return (false, null);
                    }

                    AbstractRequest request = instance as AbstractRequest;
                    if (requestTypeSet.Contains(request.RequestType)) {
                        LOGGER.Error($"Request class(={fullClassName}) is added repeatly!");
                        return (false, null);
                    }
                    requestTypeSet.Add(request.RequestType);
                    requests.Add(request);
                }

                return (true, requests);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return (false, null);
            }
        }

        private readonly ConcurrentDictionary<string, object> sharedObjects;
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
