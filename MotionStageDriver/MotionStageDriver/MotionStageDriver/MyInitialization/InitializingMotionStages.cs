using System;
using System.Collections.Generic;
using System.Xml.Linq;

using CommonApi.MyInitialization;
using CommonApi.MyUtility;

using MyMotionStageDriver.MyMotionController;
using MyMotionStageDriver.MyStage;
using MyMotionStageDriver.MyStageAxisUsage;

using NLog;

namespace MyMotionStageDriver.MyInitialization {
    public sealed class InitializingMotionStages : IInitializing {
        public InitializingMotionStages() {
            motionControllerUtility = new MotionControllerUtility();
            stageUtility = new StageUtility(motionControllerUtility.MotionControllerDict);
        }

        public bool Run() {
            try {
                XElement xeRoot = XElement.Load(@"Configuration/config_motion_stages.xml");
                configVersion = xeRoot.Attribute("version").Value.Trim();
                if (string.IsNullOrEmpty(configVersion)) {
                    LOGGER.Error($"in Configuration/config_motion_stages.xml, <config version=>, version should not be empty!");
                    return false;
                }
                string strIsVirtualRunning = xeRoot.Attribute("is_virtual_running").Value.Trim();
                if (!bool.TryParse(strIsVirtualRunning, out isVirtualRunning)) {
                    LOGGER.Error($"in Configuration/config_motion_stages.xml, <config version={configVersion} is_virtual_running={strIsVirtualRunning}>, is_virtual_running(={strIsVirtualRunning}) should be a boolean!");
                    return false;
                }

                XElement xeMotionControllers = xeRoot.Element("motion_controllers");
                if (!InitializeMotionControllers(xeMotionControllers)) {
                    return false;
                }

                XElement xeStages = xeRoot.Element("stages");
                if (!InitializeStages(xeStages)) {
                    return false;
                }

                XElement xeAxisUsages = xeRoot.Element("axis_usages");
                return InitializeStageUseages(xeAxisUsages);
            } catch (Exception ex) {
                LOGGER.Error($"Occurred exception: {ex.Message}");
                return false;
            }
        }

        public bool InitializeMotionControllers(XElement xeMotionControllers) {
            if (!motionControllerUtility.InitializeAllMotionControllers(isVirtualRunning, xeMotionControllers)) {
                LOGGER.Error("Initializing motion controllers is failed.");
                return false;
            }

            motionControllerDict = motionControllerUtility.MotionControllerDict;
            LOGGER.Info("Initializing motion controllers is successful!");
            return true;
        }

        public bool InitializeStages(XElement xeStages) {
            if (!stageUtility.Initialize(xeStages)) {
                LOGGER.Error("Initializing stages is failed.");
                return false;
            }

            LOGGER.Info("Initializing stages is successful!");
            return true;
        }

        public bool InitializeStageUseages(XElement xeAxisUsages) {
            stageAxisUsageUtility = new StageAxisUsageUtility(stageUtility.StageList);
            if (!stageAxisUsageUtility.Initialize(xeAxisUsages)) {
                LOGGER.Error("Initializing axis usages is failed.");
                return false;
            }

            LOGGER.Info("Initializing axis usages is successful!");
            return true;
        }

        private string configVersion;
        private bool isVirtualRunning;

        internal readonly MotionControllerUtility motionControllerUtility;
        public readonly StageUtility stageUtility;
        public StageAxisUsageUtility stageAxisUsageUtility;
        public Dictionary<string, AbstractMotionController> motionControllerDict;
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
