using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

using CommonApi.MyI18N;
using CommonApi.MyUtility;

using MyMotionStageDriver.MyStage;
using MyMotionStageDriver.MyStageAxis;

using MyMotionStageUserControl.MyConstant;

using NLog;

namespace MyMotionStageUserControl.MyStageJog {
    public sealed class StageJogUtility {
        public StageJogUtility(EnumLanguage language, List<Stage> stageList) {
            this.language = language;
            this.stageList = stageList;
        }

        public Dictionary<string, Form> Jogs {
            get {
                return jogs;
            }
        }

        public bool Initialize(XElement xeStageJogs) {
            userControlList.Clear();
            foreach (Stage stage in stageList) {
                foreach (StageAxis stageAxis in stage.AxisList) {
                    UcStageAxis ucStageAxis = UcStageAxisFactory.CreateInstance(language, stageAxis);
                    userControlList.Add(ucStageAxis);
                }
            }

            if (userControlList.Count == 0) {
                LOGGER.Error("Number of StageAxis user controls is 0! Stage jogs can not be initialized!");
                return false;
            }

            jogs.Clear();
            try {
                List<XElement> xeStageJogList = xeStageJogs.Elements("stage_jog").ToList();
                HashSet<string> jogIdSet = new HashSet<string>();
                foreach (XElement xeStageJog in xeStageJogList) {
                    string strEnabled = xeStageJog.Attribute("enabled").Value;
                    if (!bool.TryParse(strEnabled, out bool enabled)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <gui><stage_jogs><stage_jog>, enabled={strEnabled} is invalid! It should be true or false.");
                        return false;
                    }
                    if (!enabled) {
                        continue;
                    }

                    string jogId = xeStageJog.Attribute("id").Value.Trim();
                    if (string.IsNullOrEmpty(jogId)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog enabled=true id=>, id should not be empty!");
                        return false;
                    }
                    if (jogIdSet.Contains(jogId)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog enabled=true id={jogId}>, id(={jogId}) is duplicated!");
                        return false;
                    }
                    jogIdSet.Add(jogId);

                    XElement xeLayout = xeStageJog.Element("layout");
                    XElement xeRowNumber = xeLayout.Element("row_number");
                    string strRowNumber = xeRowNumber.Value.Trim();
                    if (!int.TryParse(strRowNumber, out int rowNumber)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog enabled=true id={jogId}><layout><row_number>{strRowNumber}, row_number(={strRowNumber}) should be a positive integer!");
                        return false;
                    }
                    if (rowNumber <= 0) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog enabled=true id={jogId}><layout><row_number>{strRowNumber}, row_number(={strRowNumber}) should be a positive integer.");
                        return false;
                    }
                    XElement xeColNumber = xeLayout.Element("col_number");
                    string strColNumber = xeColNumber.Value.Trim();
                    if (!int.TryParse(strColNumber, out int colNumber)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog enabled=true id={jogId}><layout><col_number>{strColNumber}, col_number(={strColNumber}) should be a positive integer.");
                        return false;
                    }
                    if (colNumber <= 0) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog enabled=true id={jogId}><layout><col_number>{strColNumber}, col_number(={strColNumber}) should be a positive integer.");
                        return false;
                    }
                    XElement xeRowSpan = xeLayout.Element("row_span");
                    string strRowSpan = xeRowSpan.Value.Trim();
                    if (!int.TryParse(strRowSpan, out int rowSpan)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog enabled=true id={jogId}><layout><row_span>{strRowSpan}, row_span(={strRowSpan}) should be a positive integer.");
                        return false;
                    }
                    if (rowSpan <= 0) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog enabled=true id={jogId}><layout><row_span>{strRowSpan}, row_span(={strRowSpan}) should be a positive integer.");
                        return false;
                    }
                    XElement xeColSpan = xeLayout.Element("col_span");
                    string strColSpan = xeColSpan.Value.Trim();
                    if (!int.TryParse(strColSpan, out int colSpan)) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog enabled=true id={jogId}><layout><col_span>{strColSpan}, col_span(={strColSpan}) should be a positive integer.");
                        return false;
                    }
                    if (colSpan <= 0) {
                        LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog enabled=true id={jogId}><layout><col_span>{strColSpan}, col_span(={strColSpan}) should be a positive integer.");
                        return false;
                    }
                    (int rowNum, int colNum, int rowSpan, int colSpan) layoutConfig = (rowNumber, colNumber, rowSpan, colSpan);

                    XElement xeArrage = xeStageJog.Element("arrange");
                    List<XElement> xePageList = xeArrage.Elements("page").ToList();
                    HashSet<string> pageIdSet = new HashSet<string>();
                    var arrangeConfig = new List<(string pageId, List<(int row, int col, string stageId, string axisId)> items)>();
                    foreach (XElement xePage in xePageList) {
                        List<RowColumnPair> rowColPairList = new List<RowColumnPair>();

                        string pageId = xePage.Attribute("id").Value.Trim();
                        if (string.IsNullOrEmpty(pageId)) {
                            LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog id={jogId}><arrange><page id=>, page id should not be empty!");
                            return false;
                        }
                        if (pageIdSet.Contains(pageId)) {
                            LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog id={jogId}><arrange><page id={pageId}>, page id(={pageId}) is duplicated!");
                            return false;
                        }
                        pageIdSet.Add(pageId);

                        List<XElement> xeItemList = xePage.Elements("item").ToList();
                        var itemList = new List<(int row, int column, string stageId, string axisId)>();
                        foreach (XElement xeItem in xeItemList) {
                            string strRow = xeItem.Attribute("row").Value.Trim();
                            if (!int.TryParse(strRow, out int row)) {
                                LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog id={jogId}><arrange><page id={pageId}><item row={strRow}>，row(={strRow}) should be a non-negative integer.");
                                return false;
                            }
                            if (row < 0) {
                                LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog id={jogId}><arrange><page id={pageId}><item row={strRow}>, row(={strRow}) should be a non-negative integer.");
                                return false;
                            }
                            if (row >= rowNumber) {
                                LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog id={jogId}><arrange><page id={pageId}><item row={strRow}>, row(={strRow}) should be < row_number(={rowNumber}).");
                                return false;
                            }

                            string strColumn = xeItem.Attribute("column").Value.Trim();
                            if (!int.TryParse(strColumn, out int column)) {
                                LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog id={jogId}><arrange><page id={pageId}><item column={strColumn}>, column(={strColumn}) should be a non-negative integer.");
                                return false;
                            }
                            if (column < 0) {
                                LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog id={jogId}><arrange><page id={pageId}><item column={strColumn}>, column(={strColumn}) should be a non-negative integer.");
                                return false;
                            }
                            if (column > colNumber) {
                                LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog id={jogId}><arrange><page id={pageId}><item column={strColumn}>, column(={strColumn}) should be < col_number(={colNumber}).");
                                return false;
                            }

                            RowColumnPair pair = new RowColumnPair { Row = row, Column = column };
                            var duplicatedList = rowColPairList.Where(one => one.Row == pair.Row && one.Column == pair.Column).ToList();
                            if (duplicatedList.Count > 0) {
                                LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog id={jogId}><arrange><page id={pageId}><item row={row} column={column}>, row & column are duplicated!");
                                return false;
                            }
                            rowColPairList.Add(pair);

                            string stageId = xeItem.Attribute("stage_id").Value.Trim();
                            if (string.IsNullOrEmpty(stageId)) {
                                LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog id={jogId}><arrange><page id={pageId}><item row={row} column={column} stage_id=>, stage_id should not be empty!");
                                return false;
                            }
                            var existStageList = stageList.Where(one => one.Id.Equals(stageId)).ToList();
                            if (existStageList.Count == 0) {
                                LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog id={jogId}><arrange><page id={pageId}><item row={row} column={column} stage_id={stageId}>, stage_id(={stageId}) does not exist.");
                                return false;
                            }

                            Stage stage = existStageList[0];
                            string axisId = xeItem.Attribute("axis_id").Value.Trim();
                            if (string.IsNullOrEmpty(axisId)) {
                                LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog id={jogId}><arrange><page id={pageId}><item row={row} column={column} stage_id={stageId} axis_id=>, axis_id should not be empty!");
                                return false;
                            }
                            var existAxisList = stage.AxisList.Where(one => one.AxisId.Equals(axisId)).ToList();
                            if (existAxisList.Count == 0) {
                                LOGGER.Error($"In Configuration/config_motion_stages.xml, <stage_jogs><stage_jog id={jogId}><arrange><page id={pageId}><item row={row} column={column} stage_id={stageId} axis_id={axisId}>, axis_id(={axisId}) does not exist.");
                                return false;
                            }

                            itemList.Add((row, column, stageId, axisId));
                        }

                        arrangeConfig.Add((pageId, itemList));
                    }

                    var generateResult = GenerateGui(jogId, layoutConfig, arrangeConfig);
                    if (!generateResult.isOk) {
                        return false;
                    }
                    jogs.Add(generateResult.result.jogId, generateResult.result.form);
                }

                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        public void EnableJogForm(Form jogForm, bool isEnabled) {
            foreach (Control c1 in jogForm.Controls) {
                if (c1 is TabControl) {
                    TabControl tabControl = c1 as TabControl;
                    foreach (TabPage tabPage in tabControl.TabPages) {
                        foreach (Control c2 in tabPage.Controls) {
                            if (c2 is UcStageAxis) {
                                UcStageAxis ucStageAxis = c2 as UcStageAxis;
                                ucStageAxis.Enable(isEnabled);
                            }
                        }
                    }
                }
            }
        }

        private (bool isOk, (string jogId, Form form) result) GenerateGui(string jogId, (int rowNum, int colNum, int rowSpan, int colSpan) layoutConfig,
            List<(string pageId, List<(int row, int col, string stageId, string axisId)> items)> arrangeConfig) {

            TabControl tabControl = new TabControl();
            tabControl.Left = JogConstant.COMMON_SPAN;
            tabControl.Top = JogConstant.COMMON_SPAN;

            UcStageAxis templateUserControl = userControlList[0];
            int tabPageHeight = JogConstant.COMMON_SPAN + layoutConfig.rowNum * (templateUserControl.Height + JogConstant.COMMON_SPAN);
            int tabPageWidth = JogConstant.COMMON_SPAN + layoutConfig.colNum * (templateUserControl.Width + JogConstant.COMMON_SPAN);
            tabControl.Width = tabPageWidth + JogConstant.COMMON_SPAN;
            tabControl.Height = tabPageHeight + JogConstant.COMMON_SPAN;

            foreach (var one in arrangeConfig) {
                TabPage tabPage = new TabPage();
                tabControl.Controls.Add(tabPage);
                tabPage.Text = one.pageId;
                tabPage.Left = JogConstant.COMMON_SPAN;
                tabPage.Top = JogConstant.COMMON_SPAN;
                tabPage.Height = tabPageHeight;
                tabPage.Width = tabPageWidth;

                foreach (var item in one.items) {
                    var lookupResult = LookUpUserControlStageAxis(item.stageId, item.axisId);
                    if (!lookupResult.isOk) {
                        LOGGER.Error($"In config_motion_stages.xml, <stage_jog id={jogId}><arrange><pages><page id={one.pageId}>, <item stage_id={item.stageId} axis_id={item.axisId}> is invalid!");
                        return (false, (string.Empty, null));
                    }

                    UcStageAxis ucStageAxis = lookupResult.ucStageAxis;
                    ucStageAxis.Left = item.col * ucStageAxis.Width + JogConstant.COMMON_SPAN;
                    ucStageAxis.Top = item.row * ucStageAxis.Height + JogConstant.COMMON_SPAN;
                    tabPage.Controls.Add(ucStageAxis);
                }
            }
            tabControl.SelectedIndex = 0;

            Form jogForm = new Form();
            jogForm.FormBorderStyle = FormBorderStyle.None;
            jogForm.TopLevel = false;
            jogForm.Width = tabControl.Width + 2 * JogConstant.COMMON_SPAN;
            jogForm.Height = tabControl.Height + 2 * JogConstant.COMMON_SPAN;
            jogForm.Controls.Add(tabControl);
            jogForm.Load += JogForm_Load;

            return (true, (jogId, jogForm));
        }

        private void JogForm_Load(object sender, EventArgs e) {
            Form jogForm = sender as Form;
            TabControl tabControl = null;
            foreach (Control control in jogForm.Controls) {
                if (control is TabControl) {
                    tabControl = control as TabControl;
                    break;
                }
            }

            foreach (TabPage tabPage in tabControl.TabPages) {
                foreach (Control control in tabPage.Controls) {
                    if (control is UcStageAxis) {
                        UcStageAxis ucStageAxis = control as UcStageAxis;
                        ucStageAxis.GuiUpdatePosition();
                    }
                }
            }
        }

        private (bool isOk, UcStageAxis ucStageAxis) LookUpUserControlStageAxis(string stageId, string axisId) {
            var list = userControlList.Where(item => item.CurrentStageAxis.StageId.Equals(stageId) && item.CurrentStageAxis.AxisId.Equals(axisId)).ToList();
            if (list.Count == 0) {
                LOGGER.Error($"UserControlStageAsix[stageId={stageId}, axisId={axisId}] does not exist!");
                return (false, null);
            }

            return (true, list[0]);
        }

        private readonly List<UcStageAxis> userControlList = new List<UcStageAxis>();
        private readonly EnumLanguage language;
        private readonly List<Stage> stageList;
        private readonly Dictionary<string, Form> jogs = new Dictionary<string, Form>();
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
