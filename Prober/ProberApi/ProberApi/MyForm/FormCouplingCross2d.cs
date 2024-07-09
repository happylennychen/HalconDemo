using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

using CommonApi.MyI18N;
using CommonApi.MyTrigger;
using CommonApi.MyUtility;

using MyInstruments;
using MyInstruments.MyUtility;

using MyMotionStageDriver.MyStageAxis;

using NLog;

using ProberApi.MyConstant;
using ProberApi.MyCoupling.CouplingChart;
using ProberApi.MyCoupling.CouplingParameter;
using ProberApi.MyRequest;

namespace ProberApi.MyForm {
    public partial class FormCouplingCross2d : Form {
        public FormCouplingCross2d(EnumLanguage language, ConcurrentDictionary<string, object> sharedObjects, Action actActiveCross2dPage) {
            InitializeComponent();
            frc = new FormResourceCulture(this.GetType().FullName, Assembly.GetExecutingAssembly());
            frc.Language = language;
            this.sharedObjects = sharedObjects;

            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            this.stageAxisUsages = tempObj as Dictionary<string, StageAxis>;
            sharedObjects.TryGetValue(SharedObjectKey.CROSS_COUPLING_2D_PARAMETERS, out tempObj);
            var tempConfig = ((int continuousDeclinePointNumber, Dictionary<string, CrossCoupling2dParameter> parameterDict))tempObj;
            this.coupling2dParameterDict = tempConfig.parameterDict;
            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out tempObj);
            this.instruments = tempObj as Dictionary<string, Instrument>;
            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            sharedObjects.TryGetValue(SharedObjectKey.COUPLING_IN_CONFIG, out tempObj);
            this.couplingInConfig = tempObj as Dictionary<string, (string instrumentUsageId, Dictionary<string, string> settings)>;
            sharedObjects.TryGetValue(SharedObjectKey.COUPLING_FEEDBACK_CONFIG, out tempObj);
            this.couplingFeedbackConfigDict = tempObj as Dictionary<string, (string instrumentUsageId, Dictionary<string, string> initSettingDict, Dictionary<string, string> triggerInSettingDict)>;

            CrossCoupling2dSteppedChart steppedChart = new CrossCoupling2dSteppedChart(chart, this, actActiveCross2dPage);
            sharedObjects.AddOrUpdate(SharedObjectKey.CROSS_COUPLING_2D_STEPPED_CHART, steppedChart, (key, oldValue) => steppedChart);
            CrossCoupling2dTriggeredChart triggeredChart = new CrossCoupling2dTriggeredChart(chart, this, actActiveCross2dPage);
            sharedObjects.AddOrUpdate(SharedObjectKey.CROSS_COUPLING_2D_TRIGGERED_CHART, triggeredChart, (key, oldValue) => triggeredChart);
        }

        private void FormCrossCoupling2d_Load(object sender, EventArgs e) {
            List<string> parameterIdList = coupling2dParameterDict.Keys.ToList();
            foreach (string parameterId in parameterIdList) {
                cmbParameterId.Items.Add(parameterId);
            }

            List<string> axisUsageIdList = stageAxisUsages.Keys.ToList();
            foreach (string axisUsageId in axisUsageIdList) {
                cmbFirstAxis.Items.Add(axisUsageId);
                cmbSecondAxis.Items.Add(axisUsageId);
            }

            btnCouplingInInfo.Enabled = false;
            btnCouplingFeedbackInfo.Enabled = false;
            ckbEnableRefined.Checked = false;
            tbRefinedMotionRange.Enabled = false;
            tbRefinedStep.Enabled = false;
            ckbIsTriggered.Checked = false;
            btnTriggerCouplingFeedbackSetting.Enabled = false;

            HashSet<Control> excludedControls = new HashSet<Control> {
                btnAddAxisPair,
            };
            frc.ExcludedControls(excludedControls);
            frc.EnumerateControl(this);
        }

        private async void btnCoupling2D_Click(object sender, EventArgs e) {
            if (!CheckInput()) {
                return;
            }

            string parameterId = cmbParameterId.Items[cmbParameterId.SelectedIndex] as string;
            string allAxisPairs = GetAllAxisPairs();
            string showGui = $"{AbstractCouplingParameter.EnumCommonOverwrittenParameterId.SHOW_GUI.ToString()}={true.ToString()}";
            string settingInInstrument = $"{AbstractCouplingParameter.EnumCommonOverwrittenParameterId.SETTING_IN_INSTRUMENT.ToString()}={ckbSettingCouplingInInstrument.Checked.ToString()}";
            string settingFeedbackInstrument = $"{AbstractCouplingParameter.EnumCommonOverwrittenParameterId.SETTING_FEEDBACK_INSTRUMENT.ToString()}={ckbSettingFeedbackInstrument.Checked.ToString()}";
            string savingRawData = $"{AbstractCouplingParameter.EnumCommonOverwrittenParameterId.SAVING_RAW_DATA}={ckbSaveRawData.Checked.ToString()}";
            string dealwithWeight = $"{AbstractCouplingParameter.EnumCommonOverwrittenParameterId.DEALWITH_WEIGHT} ={ckbDataWithWeight.Checked.ToString()}";
            string threshold = $"{AbstractCouplingParameter.EnumCommonOverwrittenParameterId.THRESHOLD} ={txt_PeakThreshold.Text.Trim()}";
            string coarseMotionRange = $"{CrossCoupling2dParameter.EnumOverwrittenParameterId.COARSE_MOTION_RANGE.ToString()}={tbCoarseMotionRange.Text.Trim()}";
            string coarseStep = $"{CrossCoupling2dParameter.EnumOverwrittenParameterId.COARSE_STEP.ToString()}={tbCoarseStep.Text.Trim()}";
            string enabledRefinedTraveling = $"{CrossCoupling2dParameter.EnumOverwrittenParameterId.ENABLED_REFINED_TRAVELING}={ckbEnableRefined.Checked.ToString()}";
            string isTriggered = $"{EnumTriggerCommonSetting.IS_TRIGGERED}={ckbIsTriggered.Checked.ToString()}";
            List<string> settings = new List<string> {
                showGui,
                settingInInstrument,
                settingFeedbackInstrument,
                savingRawData,
                dealwithWeight,
                threshold,
                coarseMotionRange,
                coarseStep,
                enabledRefinedTraveling,
                isTriggered
            };
            if (ckbEnableRefined.Checked) {
                string refinedMotionRange = $"{CrossCoupling2dParameter.EnumOverwrittenParameterId.REFINED_MOTION_RANGE}={tbRefinedMotionRange.Text.Trim()}";
                string refinedStep = $"{CrossCoupling2dParameter.EnumOverwrittenParameterId.REFINED_STEP}={tbRefinedStep.Text.Trim()}";
                settings.Add(refinedMotionRange);
                settings.Add(refinedStep);
            }
            if (ckbIsTriggered.Checked) {
                var coupling2dParameter = coupling2dParameterDict[parameterId];
                foreach (var one in coupling2dParameter.NecessaryTriggerInSettings) {
                    settings.Add($"{one.Key}={one.Value}");
                }
            }
            string overwritingParameter = string.Join(";", settings.ToArray());
            string allParameter = $"{parameterId},{allAxisPairs},{overwritingParameter}";

            RequestCrossCoupling2d request = new RequestCrossCoupling2d(sharedObjects);
            request.TryUpdateParameters(allParameter);

            this.Enabled = false;
            await Task.Run(() => {
                request.Run();
            });
            this.Enabled = true;
        }

        private bool CheckInput() {
            if (!CheckAxisesPairsInput()) {
                return false;
            }

            return CheckCouplingParameterInput();
        }

        private void btnAddAxisPair_Click(object sender, EventArgs e) {
            if (cmbFirstAxis.SelectedIndex < 0) {
                MessageBox.Show(frc.GetString("txtChoose1stAxis"));
                cmbFirstAxis.Focus();
                return;
            }
            if (cmbSecondAxis.SelectedIndex < 0) {
                MessageBox.Show(frc.GetString("txtChoose2ndAxis"));
                cmbSecondAxis.Focus();
                return;
            }
            if (cmbFirstAxis.SelectedIndex == cmbSecondAxis.SelectedIndex) {
                MessageBox.Show(frc.GetString("txtAlert1st2ndAxisShouldNotBeSame"));
                cmbSecondAxis.Focus();
                return;
            }
            string firstAxis = cmbFirstAxis.Items[cmbFirstAxis.SelectedIndex].ToString();
            string secondAxis = cmbSecondAxis.Items[cmbSecondAxis.SelectedIndex].ToString();
            string line = $"{firstAxis},{secondAxis}";
            tbAxisesPairList.Text.Trim();
            if (string.IsNullOrEmpty(tbAxisesPairList.Text)) {
                tbAxisesPairList.Text += line;
            } else {
                tbAxisesPairList.Text += Environment.NewLine + line;
            }
        }

        private bool CheckAxisesPairsInput() {
            string content = tbAxisesPairList.Text.Trim();
            string[] lineSeperators = new string[] { Environment.NewLine, "\n", "\r\n" };
            List<string> lines = content.Split(lineSeperators, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (lines.Count == 0) {
                MessageBox.Show(frc.GetString("txtAxisPairShouldNotBeEmpty"));
                tbAxisesPairList.Focus();
                return false;
            }
            string refinedContent = string.Join(Environment.NewLine, lines);
            tbAxisesPairList.Text = refinedContent;

            HashSet<string> axisUsageIdSet = new HashSet<string>();
            foreach (var one in cmbFirstAxis.Items) {
                axisUsageIdSet.Add(one.ToString());
            }

            for (int i = 0; i < lines.Count; i++) {
                string line = lines[i];
                string[] partSeperator = new string[] { "," };
                List<string> parts = line.Split(partSeperator, StringSplitOptions.None).ToList();
                if (parts.Count != 2) {
                    MessageBox.Show($"{frc.GetString("txtRow")}[{i}], {frc.GetString("txtAxisPair")}(={line}) is invalid!");
                    tbAxisesPairList.Focus();
                    return false;
                }
                string firstAxisUsageId = parts[0];
                string secondAxisUsageId = parts[1];
                if (!axisUsageIdSet.Contains(firstAxisUsageId)) {
                    MessageBox.Show($"{frc.GetString("txtRow")}[{i}], {frc.GetString("txt1stAxis")}(={firstAxisUsageId}) does not exist!");
                    tbAxisesPairList.Focus();
                    return false;
                }
                if (!axisUsageIdSet.Contains(secondAxisUsageId)) {
                    MessageBox.Show($"{frc.GetString("txtRow")}[{i}], {frc.GetString("txt2ndAxis")}(={secondAxisUsageId}) does not exist!");
                    tbAxisesPairList.Focus();
                    return false;
                }
            }

            return true;
        }

        private string GetAllAxisPairs() {
            string content = tbAxisesPairList.Text.Trim();
            string[] lineSeperators = new string[] { Environment.NewLine };
            List<string> lines = content.Split(lineSeperators, StringSplitOptions.RemoveEmptyEntries).ToList();
            string result = string.Join(",", lines);
            return result;
        }

        private bool CheckCouplingParameterInput() {
            if (cmbParameterId.SelectedIndex < 0) {
                MessageBox.Show(frc.GetString("txtAlertChooseParamterId"));
                cmbParameterId.Focus();
                return false;
            }
            if (!double.TryParse(tbCoarseMotionRange.Text.Trim(), out double tempDouble)) {
                MessageBox.Show(frc.GetString("txtAlertCoarseRange"));
                tbCoarseMotionRange.Focus();
                return false;
            }
            if (tempDouble <= 0) {
                MessageBox.Show(frc.GetString("txtAlertCoarseRange"));
                tbCoarseMotionRange.Focus();
                return false;
            }
            if (!double.TryParse(tbCoarseStep.Text.Trim(), out tempDouble)) {
                MessageBox.Show(frc.GetString("txtAlertCoarseStep"));
                tbCoarseStep.Focus();
                return false;
            }
            if (tempDouble <= 0) {
                MessageBox.Show(frc.GetString("txtAlertCoarseStep"));
                tbCoarseStep.Focus();
                return false;
            }
            if (tbRefinedMotionRange.Enabled) {
                if (!double.TryParse(tbRefinedMotionRange.Text.Trim(), out tempDouble)) {
                    MessageBox.Show(frc.GetString("txtAlertRefinedRange"));
                    tbRefinedMotionRange.Focus();
                    return false;
                }
                if (tempDouble <= 0) {
                    MessageBox.Show(frc.GetString("txtAlertRefinedRange"));
                    tbRefinedMotionRange.Focus();
                    return false;
                }
            }
            if (tbRefinedStep.Enabled) {
                if (!double.TryParse(tbRefinedStep.Text.Trim(), out tempDouble)) {
                    MessageBox.Show(frc.GetString("txtAlertRefinedStep"));
                    tbRefinedStep.Focus();
                    return false;
                }
                if (tempDouble <= 0) {
                    MessageBox.Show(frc.GetString("txtAlertRefinedStep"));
                    tbRefinedStep.Focus();
                    return false;
                }
            }

            return true;
        }

        private void cmbParameterId_SelectedIndexChanged(object sender, EventArgs e) {
            if (cmbParameterId.SelectedIndex < 0) {
                return;
            }
            ckbIsTriggered.Checked = true;

            string parameterId = cmbParameterId.Items[cmbParameterId.SelectedIndex] as string;
            CrossCoupling2dParameter parameter = coupling2dParameterDict[parameterId];
            btnCouplingInInfo.Enabled = parameter.EnabledCouplingIn;
            btnCouplingFeedbackInfo.Enabled = true;
            if (!parameter.EnabledCouplingIn) {
                ckbSettingCouplingInInstrument.Checked = false;
                ckbSettingCouplingInInstrument.Enabled = false;
            } else {
                ckbSettingCouplingInInstrument.Enabled = true;
            }

            tbCoarseMotionRange.Text = parameter.CoarseMotionRange.ToString("0.00");
            tbCoarseStep.Text = parameter.CoarseStep.ToString("0.00");
            ckbEnableRefined.Checked = parameter.EnabledRefinedTraveling;
            if (parameter.EnabledRefinedTraveling) {
                tbRefinedMotionRange.Enabled = true;
                tbRefinedStep.Enabled = true;
                if (string.IsNullOrEmpty(tbRefinedMotionRange.Text.Trim())) {
                    tbRefinedMotionRange.Text = parameter.RefinedMotionRange.ToString("0.00");
                }
                if (string.IsNullOrEmpty(tbRefinedStep.Text.Trim())) {
                    tbRefinedStep.Text = parameter.RefinedStep.ToString("0.00");
                }
            } else {
                tbRefinedMotionRange.Enabled = false;
                tbRefinedStep.Enabled = false;
            }
        }

        private void ckbEnableRefine_CheckedChanged(object sender, EventArgs e) {
            tbRefinedMotionRange.Enabled = ckbEnableRefined.Checked;
            tbRefinedStep.Enabled = ckbEnableRefined.Checked;
        }

        private void btnCouplingInInfo_Click(object sender, EventArgs e) {
            if (cmbParameterId.SelectedIndex < 0) {
                return;
            }
            string parameterId = cmbParameterId.Items[cmbParameterId.SelectedIndex] as string;
            var coupling2dParameter = coupling2dParameterDict[parameterId];
            var couplingIn = couplingInConfig[coupling2dParameter.CouplingInId];
            var list = this.instrumentUsageList.Where(x => x.UsageId.Equals(couplingIn.instrumentUsageId)).ToList();
            InstrumentUsage instrumentUsage = list.First();
            Instrument instrument = instruments[instrumentUsage.InstrumentId];

            FormCouplingShowInfo form = new FormCouplingShowInfo(frc.Language, instrument, instrumentUsage.Slot, instrumentUsage.Channel, instrumentUsage.InstrumentCategory);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();
        }

        private void btnCouplingFeedbackInfo_Click(object sender, EventArgs e) {
            if (cmbParameterId.SelectedIndex < 0) {
                return;
            }
            string parameterId = cmbParameterId.Items[cmbParameterId.SelectedIndex] as string;
            var coupling2dParameter = coupling2dParameterDict[parameterId];
            var couplingFeedback = couplingFeedbackConfigDict[coupling2dParameter.CouplingFeedbackId];
            var list = this.instrumentUsageList.Where(x => x.UsageId.Equals(couplingFeedback.instrumentUsageId)).ToList();
            InstrumentUsage instrumentUsage = list.First();
            Instrument instrument = instruments[instrumentUsage.InstrumentId];

            FormCouplingShowInfo form = new FormCouplingShowInfo(frc.Language, instrument, instrumentUsage.Slot, instrumentUsage.Channel, instrumentUsage.InstrumentCategory);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();
        }

        private void ckbIsTriggered_CheckedChanged(object sender, EventArgs e) {
            btnTriggerCouplingFeedbackSetting.Enabled = ckbIsTriggered.Checked;
        }

        private void btnTriggerCouplingFeedbackSetting_Click(object sender, EventArgs e) {
            if (cmbParameterId.SelectedIndex < 0) {
                return;
            }
            string parameterId = cmbParameterId.Items[cmbParameterId.SelectedIndex] as string;
            var coupling2dParameter = coupling2dParameterDict[parameterId];
            var couplingFeedback = couplingFeedbackConfigDict[coupling2dParameter.CouplingFeedbackId];
            var list = this.instrumentUsageList.Where(x => x.UsageId.Equals(couplingFeedback.instrumentUsageId)).ToList();
            InstrumentUsage instrumentUsage = list.First();
            Instrument instrument = instruments[instrumentUsage.InstrumentId];
            object objInstrument = instrument;
            ITriggerIn triggerIn = objInstrument as ITriggerIn;

            FormCouplingFeedbackTriggerInSetting form = new FormCouplingFeedbackTriggerInSetting(frc.Language, coupling2dParameter.NecessaryTriggerInSettings, triggerIn);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();
        }

        private readonly ConcurrentDictionary<string, object> sharedObjects;
        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        private readonly Dictionary<string, CrossCoupling2dParameter> coupling2dParameterDict;
        private readonly Dictionary<string, Instrument> instruments;
        private readonly List<InstrumentUsage> instrumentUsageList;
        private readonly Dictionary<string, (string instrumentUsageId, Dictionary<string, string> settings)> couplingInConfig;
        private readonly Dictionary<string, (string instrumentUsageId, Dictionary<string, string> initSettingDict, Dictionary<string, string> triggerInSettingDict)> couplingFeedbackConfigDict;
        private readonly FormResourceCulture frc;
    }
}
