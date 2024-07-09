using System;
using System.Collections.Generic;
using System.Xml.Linq;

using CommonApi.MyI18N;
using CommonApi.MyUtility;

using MyMotionStageDriver.MyStage;

using MyMotionStageUserControl.MyStageJog;

using NLog;

namespace MyMotionStageUserControl.MyUtility {
    public sealed class MyInitialization {
        public MyInitialization(EnumLanguage language, List<Stage> stageList) {
            stageJogUtility = new StageJogUtility(language, stageList);
        }

        public bool Initialize() {            
            try {
                XElement xeRoot = XElement.Load(@"Configuration/config_motion_stages.xml");
                XElement xeStageJogs = xeRoot.Element("stage_jogs");
                if (!stageJogUtility.Initialize(xeStageJogs)) {
                    LOGGER.Error("Initializing stage jogs is failed.");
                    return false;
                }

                LOGGER.Info("Initializing stage jogs is successful!");
                return true;
            } catch (Exception ex) {
                LOGGER.Error($"Occurred exception: {ex.Message}");
                return false;
            }
        }
        
        public readonly StageJogUtility stageJogUtility;
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
