using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

using CommonApi.MyUtility;

using MyMotionStageUserControl.MyStageJog;

using NLog;

namespace Prober.InitializeConfiguration {
    internal sealed class InitializingConfigurationGui {
        internal InitializingConfigurationGui(ConcurrentDictionary<string, object> sharedObjects, StageJogUtility stageJogUtility, Panel panelStageJogs) {
            this.sharedObjects = sharedObjects;
            this.stageJogUtility = stageJogUtility;
            this.panelStageJogs = panelStageJogs;
        }

        internal bool Run() {
            Dictionary<string, (int left, int top)> jogsLeftTop = new Dictionary<string, (int left, int top)>();
            try {
                XElement xeRoot = XElement.Load(@"Configuration/config_gui.xml");
                XElement xeStageJogsArea = xeRoot.Element("stage_jogs_area");
                List<XElement> xeStageJogList = xeStageJogsArea.Elements("stage_jog").ToList();
                HashSet<string> idSet = new HashSet<string>();
                foreach (XElement xeStageJog in xeStageJogList) {
                    string jogId = xeStageJog.Attribute("id").Value.Trim();
                    if (idSet.Contains(jogId)) {
                        LOGGER.Error($"In config_gui.xml, <stage_jogs_area><stage_jog id={jogId}>, id(={jogId}) is duplicated!");
                        return false;
                    }

                    if (!stageJogUtility.Jogs.ContainsKey(jogId)) {
                        LOGGER.Error($"In config_gui.xml, <stage_jogs_area><stage_jog id={jogId}>, id(={jogId}) does not exist!");
                        return false;
                    }
                    idSet.Add(jogId);

                    string strLeft = xeStageJog.Attribute("left").Value.Trim();
                    if (string.IsNullOrEmpty(strLeft)) {
                        LOGGER.Error($"In config_gui.xml, <stage_jogs_area><stage_jog id={jogId} left=>, left should not be empty!");
                        return false;
                    }
                    if (!int.TryParse(strLeft, out int left)) {
                        LOGGER.Error($"In config_gui.xml, <stage_jogs_area><stage_jog id={jogId} left={strLeft}>, left(={strLeft}) should be a non-negative integer!");
                        return false;
                    }
                    if (left < 0) {
                        LOGGER.Error($"In config_gui.xml, <stage_jogs_area><stage_jog id={jogId} left={strLeft}>, left(={strLeft}) should be a non-negative integer!");
                        return false;
                    }

                    string strTop = xeStageJog.Attribute("top").Value.Trim();
                    if (string.IsNullOrEmpty(strTop)) {
                        LOGGER.Error($"In config_gui.xml, <stage_jogs_area><stage_jog id={jogId} top=>, top should not be empty!");
                        return false;
                    }
                    if (!int.TryParse(strTop, out int top)) {
                        LOGGER.Error($"In config_gui.xml, <stage_jogs_area><stage_jog id={jogId} top={strTop}>, top(={strTop}) should be a non-negative integer!");
                    }
                    if (top < 0) {
                        LOGGER.Error($"In config_gui.xml, <stage_jogs_area><stage_jog id={jogId} top={strTop}>, top(={strTop}) should be a non-negative integer!");
                        return false;
                    }

                    jogsLeftTop.Add(jogId, (left, top));
                }
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }

            foreach (var one in jogsLeftTop) {
                string jogId = one.Key;
                int left = one.Value.left;
                int top = one.Value.top;

                Form form = stageJogUtility.Jogs[jogId];
                panelStageJogs.BeginInvoke(new Action(() => {
                    form.Left = left;
                    form.Top = top;
                    panelStageJogs.Controls.Add(form);
                    form.Show();
                }));
            }

            return true;
        }

        private readonly ConcurrentDictionary<string, object> sharedObjects;
        private readonly StageJogUtility stageJogUtility;
        private readonly Panel panelStageJogs;
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
