using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Xml.Linq;

using CommonApi.MyInitialization;
using CommonApi.MyUtility;

using NLog;

using ProberApi.MyConstant;
using ProberApi.MyEnum;

namespace ProberApi.MyInitialization {
    public sealed class InitializingConfiguration : IInitializing {
        public InitializingConfiguration(ConcurrentDictionary<string, object> configBoard, Action<string> actSetGuiControlMode) {            
            this.configBoard = configBoard;
            this.actSetGuiControlMode = actSetGuiControlMode;
        }

        public bool Run() {
            try {
                XElement xeRoot = XElement.Load(@"Configuration/config.xml");
                XElement xeControlMode = xeRoot.Element("control_mode");
                string strControlMode = xeControlMode.Value.Trim();
                if (!Enum.TryParse(strControlMode, out EnumControlMode enumControlMode)) {
                    List<string> supportedModeList = new List<string>();
                    foreach (EnumControlMode mode in Enum.GetValues(typeof(EnumControlMode))) {
                        supportedModeList.Add(mode.ToString());
                    }
                    string allSupportedModes = string.Join(",", supportedModeList);
                    LOGGER.Error($"In config_settings.xml, <control_mode>{strControlMode}</control_mode>, control_mode(={strControlMode}) is invalid! Valid control modes are [{allSupportedModes}].");
                    return false;
                }
                configBoard.AddOrUpdate(ConfigKey.CONTROL_MODE, strControlMode, (key, oldValue) => strControlMode);
                actSetGuiControlMode(strControlMode);

                XElement xeListening = xeRoot.Element("listening");
                XElement xeIpV4 = xeListening.Element("ip_v4");
                XElement xePort = xeListening.Element("port");
                string strIpV4 = xeIpV4.Value.Trim();
                //if (!MyStaticUtility.IsLocalIpV4Valid(strIpV4)) {
                if (!MyStaticUtility.IsLocalIpV4Valid(strIpV4) && (!strIpV4.Equals("127.0.0.1"))) {
                    LOGGER.Error($"In Configuration/config.xml, <listening><ip_v4>{strIpV4} is not valid local IP!");
                    return false;
                }
                string strPort = xePort.Value.Trim();
                if (!int.TryParse(strPort, out int port)) {
                    LOGGER.Error($"In Configuration/config.xml, <listening><port>{strPort} is invalid! The range of listening port is [{MyStaticUtility.LISTENING_PORT_MIN}, {MyStaticUtility.LISTENING_PORT_MAX}]!");
                    return false;
                }
                if (!MyStaticUtility.IsProberListeningPortValid(port)) {
                    LOGGER.Error($"In Configuration/config.xml, <listening><port>{strPort} is invalid! The range of listening port is [{MyStaticUtility.LISTENING_PORT_MIN}, {MyStaticUtility.LISTENING_PORT_MAX}]!");
                    return false;
                }
                configBoard.AddOrUpdate(ConfigKey.LISTENING_IP, strIpV4, (key, oldValue) => strIpV4);
                configBoard.AddOrUpdate(ConfigKey.LISTENING_PORT, port, (key, oldValue) => port);
                
                return true;
            } catch (Exception ex) {
                LOGGER.Error(ex.Message);
                return false;
            }
        }
        
        private readonly ConcurrentDictionary<string, object> configBoard;
        private readonly Action<string> actSetGuiControlMode;
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
