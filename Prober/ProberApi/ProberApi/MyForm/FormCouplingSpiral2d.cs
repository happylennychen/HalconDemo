using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

using CommonApi.MyI18N;

using MyInstruments;
using MyInstruments.MyUtility;

using MyMotionStageDriver.MyStageAxis;

using ProberApi.MyConstant;
using ProberApi.MyCoupling.CouplingChart;
using ProberApi.MyCoupling.CouplingParameter;
using ProberApi.MyRequest;

namespace ProberApi.MyForm {
    public partial class FormCouplingSpiral2d : Form {
        public FormCouplingSpiral2d(EnumLanguage language, ConcurrentDictionary<string, object> sharedObjects, Action actActiveSpiral2DPage) {
            InitializeComponent();
            frc = new FormResourceCulture(this.GetType().FullName, Assembly.GetExecutingAssembly());
            frc.Language = language;
            this.sharedObjects = sharedObjects;

            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            this.stageAxisUsages = tempObj as Dictionary<string, StageAxis>;
            sharedObjects.TryGetValue(SharedObjectKey.SPIRAL_COUPLING_2D_PARAMETERS, out tempObj);
            this.coupling2dParameterDict = tempObj as Dictionary<string, SpiralCoupling2dParameter>;
            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out tempObj);
            this.instruments = tempObj as Dictionary<string, Instrument>;
            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            sharedObjects.TryGetValue(SharedObjectKey.COUPLING_IN_CONFIG, out tempObj);
            this.couplingInConfig = tempObj as Dictionary<string, (string instrumentUsageId, Dictionary<string, string> settings)>;
            sharedObjects.TryGetValue(SharedObjectKey.COUPLING_FEEDBACK_CONFIG, out tempObj);            
            this.couplingFeedbackConfigDict = tempObj as Dictionary<string, (string instrumentUsageId, Dictionary<string, string> initSettingDict, Dictionary<string, string> triggerInSettingDict)>;

            SpiralCoupling2dChart coupling2dChart = new SpiralCoupling2dChart(chart, this, actActiveSpiral2DPage);
            sharedObjects.AddOrUpdate(SharedObjectKey.SPIRAL_COUPLING_2D_CHART_CONFIG, coupling2dChart, (key, oldValue) => coupling2dChart);
        }

        private void FormSpiralCoupling2d_Load(object sender, EventArgs e) {
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

            HashSet<Control> excludedControls = new HashSet<Control>();
            excludedControls.Add(btnAddAxisPair);
            frc.ExcludedControls(excludedControls);
            frc.EnumerateControl(this);
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

        private void cmbParameterId_SelectedIndexChanged(object sender, EventArgs e) {
            if (cmbParameterId.SelectedIndex < 0) {
                return;
            }
            string parameterId = cmbParameterId.Items[cmbParameterId.SelectedIndex] as string;
            SpiralCoupling2dParameter parameter = coupling2dParameterDict[parameterId];
            btnCouplingInInfo.Enabled = parameter.EnabledCouplingIn;
            btnCouplingFeedbackInfo.Enabled = true;
            if (!parameter.EnabledCouplingIn) {
                ckbSettingCouplingInInstrument.Checked = false;
                ckbSettingCouplingInInstrument.Enabled = false;
            } else {
                ckbSettingCouplingInInstrument.Enabled = true;
            }

            tbMotionRange.Text = parameter.MotionRange.ToString("0.00");
            tbStep.Text = parameter.Step.ToString("0.00");
            tbFeedbackThreshold.Text = parameter.FeedbackThreshold.ToString("0.00");
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

        private async void btnDoCoupling2d_Click(object sender, EventArgs e) {
            if (!CheckInput()) {
                return;
            }

            string parameterId = cmbParameterId.Items[cmbParameterId.SelectedIndex] as string;
            string allAxisPairs = GetAllAxisPairs();
            string showGui = $"{AbstractCouplingParameter.EnumCommonOverwrittenParameterId.SHOW_GUI.ToString()}={true.ToString()}";
            string settingInInstrument = $"{AbstractCouplingParameter.EnumCommonOverwrittenParameterId.SETTING_IN_INSTRUMENT.ToString()}={ckbSettingCouplingInInstrument.Checked.ToString()}";
            string settingFeedbackInstrument = $"{AbstractCouplingParameter.EnumCommonOverwrittenParameterId.SETTING_FEEDBACK_INSTRUMENT.ToString()}={ckbSettingFeedbackInstrument.Checked.ToString()}";
            string savingRawData = $"{AbstractCouplingParameter.EnumCommonOverwrittenParameterId.SAVING_RAW_DATA}={ckbSaveRawData.Checked.ToString()}";
            string dealwithWeight = $"{AbstractCouplingParameter.EnumCommonOverwrittenParameterId.DEALWITH_WEIGHT}={ckbDataWithWeight.Checked.ToString()}";
            string motionRange = $"{SpiralCoupling2dParameter.EnumOverwrittenParameterId.MOTION_RANGE.ToString()}={tbMotionRange.Text.Trim()}";
            string step = $"{SpiralCoupling2dParameter.EnumOverwrittenParameterId.STEP.ToString()}={tbStep.Text.Trim()}";
            string feedbackThreshold = $"{SpiralCoupling2dParameter.EnumOverwrittenParameterId.FEEDBACK_THRESHOLD}={tbFeedbackThreshold.Text.Trim()}";

            List<string> settings = new List<string> {
                showGui,
                settingInInstrument,
                settingFeedbackInstrument,
                savingRawData,
                dealwithWeight,
                motionRange,
                step,
                feedbackThreshold
            };
            string overwritingParameter = string.Join(";", settings.ToArray());
            string allParameter = $"{parameterId},{allAxisPairs},{overwritingParameter}";

            RequestSpiralCoupling2d request = new RequestSpiralCoupling2d(sharedObjects);
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

        private bool CheckCouplingParameterInput() {
            if (cmbParameterId.SelectedIndex < 0) {
                MessageBox.Show(frc.GetString("txtAlertChooseParamterId"));
                cmbParameterId.Focus();
                return false;
            }
            if (!double.TryParse(tbMotionRange.Text.Trim(), out double tempDouble)) {
                MessageBox.Show(frc.GetString("txtAlertRange"));
                tbMotionRange.Focus();
                return false;
            }
            if (tempDouble <= 0) {
                MessageBox.Show(frc.GetString("txtAlertRange"));
                tbMotionRange.Focus();
                return false;
            }
            if (!double.TryParse(tbStep.Text.Trim(), out tempDouble)) {
                MessageBox.Show(frc.GetString("txtAlertStep"));
                tbStep.Focus();
                return false;
            }
            if (tempDouble <= 0) {
                MessageBox.Show(frc.GetString("txtAlertStep"));
                tbStep.Focus();
                return false;
            }
            if (!double.TryParse(tbFeedbackThreshold.Text.Trim(), out tempDouble)) {
                MessageBox.Show(frc.GetString("txtAlertThreshold"));
                tbFeedbackThreshold.Focus();
                return false;
            }

            return true;
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

        private readonly FormResourceCulture frc;
        private readonly ConcurrentDictionary<string, object> sharedObjects;
        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        private readonly Dictionary<string, SpiralCoupling2dParameter> coupling2dParameterDict;
        private readonly Dictionary<string, Instrument> instruments;
        private readonly List<InstrumentUsage> instrumentUsageList;
        private readonly Dictionary<string, (string instrumentUsageId, Dictionary<string, string> settings)> couplingInConfig;
        private readonly Dictionary<string, (string instrumentUsageId, Dictionary<string, string> initSettingDict, Dictionary<string, string> triggerInSettingDict)> couplingFeedbackConfigDict;
    }
}
