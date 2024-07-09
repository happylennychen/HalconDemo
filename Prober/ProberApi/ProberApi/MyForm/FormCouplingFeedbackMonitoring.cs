using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Timers;
using System.Windows.Forms;

using CommonApi.MyI18N;

using MyInstruments.MyUtility;

using ProberApi.MyConstant;
using ProberApi.MyCoupling;

namespace ProberApi.MyForm {
    public partial class FormCouplingFeedbackMonitoring : Form {
        public FormCouplingFeedbackMonitoring(EnumLanguage language, ConcurrentDictionary<string, object> sharedObjects) {
            InitializeComponent();
            frc = new FormResourceCulture(this.GetType().FullName, Assembly.GetExecutingAssembly());
            frc.Language = language;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out object tempObj);
            instrumentsUsages = tempObj as List<InstrumentUsage>;

            sharedObjects.TryGetValue(SharedObjectKey.COUPLING_FEEDBACK_GUI_MONITORING, out tempObj);
            var value = ((int refreshIntervalInMs, Dictionary<string, string> config))tempObj;
            refreshIntervalInMs = value.refreshIntervalInMs;
            feedbackMonitoringConfig = value.config;

            sharedObjects.TryGetValue(SharedObjectKey.COUPLING_FEEDBACK_CONFIG, out tempObj);
            couplingFeedbackConfigDict = tempObj as Dictionary<string, (string instrumentUsageId, Dictionary<string, string> initSettingDict, Dictionary<string, string> triggerInSettingDict)>;
            
            foreach (var one in feedbackMonitoringConfig) {
                string couplingFeedbackId = one.Key;
                string titleResourceId = one.Value;
                var feedbackConfig = couplingFeedbackConfigDict[couplingFeedbackId];
                var list = instrumentsUsages.Where(x => x.UsageId.Equals(feedbackConfig.instrumentUsageId)).ToList();
                InstrumentUsage instrumentUsage = list.First();

                CouplingFeedbackUtility couplingFeedbackUtility = new CouplingFeedbackUtility(instrumentUsage);
                var getResult = couplingFeedbackUtility.GetFuncGetFeedback();
                monitoringList.Add((titleResourceId, getResult.funcGetFeedback));
            }

            Action actRestore = RestoreMonitoringFeedback;
            sharedObjects.AddOrUpdate(SharedObjectKey.COUPLING_RESTORE_FEEDBACK_MONITORING, actRestore, (key, oldValue) => actRestore);
            Action actDisable = DisabledMonitoringFeedback;
            sharedObjects.AddOrUpdate(SharedObjectKey.COUPLING_DISABLED_FEEDBACK_MONITORING, actDisable, (key, oldValue) => actDisable);
        }

        private void FormCouplingFeedbackMonitoring_Load(object sender, EventArgs e) {
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.ColumnCount = 3;
            tableLayoutPanel.RowCount = monitoringList.Count + 2;
            tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            for (int i = 0; i < tableLayoutPanel.ColumnCount; i++) {
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                //tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            }
            for (int i = 0; i < tableLayoutPanel.RowCount; i++) {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                //tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            }

            Label lblFormTitle = new Label();
            lblFormTitle.Name = nameof(lblFormTitle);
            lblFormTitle.Font = DEFAULT_FONT;
            lblFormTitle.Text = frc.GetString("txtCouplingFeedbackMonitoring");
            lblFormTitle.AutoSize = true;
            tableLayoutPanel.Controls.Add(lblFormTitle, 0, 0);
            //tableLayoutPanel.SetColumnSpan(lblFormTitle, 2);

            int index = 0;
            foreach (var one in monitoringList) {
                CheckBox ckbEnable = new CheckBox();
                ckbEnable.Name = $"{nameof(ckbEnable)}_{one.titleResourceId}";
                ckbEnable.Font = DEFAULT_FONT;                
                ckbEnable.Text = frc.GetString("txtEnable");
                ckbEnable.AutoSize = true;
                tableLayoutPanel.Controls.Add(ckbEnable, 0, 1 + index);
                checkBoxesForEachFeedback.Add(ckbEnable);
                allCheckBoxLastValues.Add(ckbEnable, false);

                Label lblFeedbackTitle = new Label();
                lblFeedbackTitle.Name = $"{nameof(lblFeedbackTitle)}_{one.titleResourceId}";
                lblFeedbackTitle.Font = DEFAULT_FONT;                
                lblFeedbackTitle.Text = frc.GetString(one.titleResourceId);
                lblFeedbackTitle.AutoSize = true;
                tableLayoutPanel.Controls.Add(lblFeedbackTitle, 1, 1 + index);

                TextBox tbFeedbackValue = new TextBox();
                tbFeedbackValue.Name = $"{nameof(tbFeedbackValue)}_{one.titleResourceId}";
                tbFeedbackValue.Font = DEFAULT_FONT;
                tbFeedbackValue.ReadOnly = true;
                tbFeedbackValue.ForeColor = Color.Gold;
                tbFeedbackValue.BackColor = Color.Black;
                tbFeedbackValue.Width = 150;
                tableLayoutPanel.Controls.Add(tbFeedbackValue, 2, 1 + index);
                textBoxesForEachFeedback.Add(tbFeedbackValue);

                checkboxTextboxFuncDict.Add(ckbEnable, (tbFeedbackValue, one.funcGetFeedback));
                ++index;
            }
            this.Controls.Add(tableLayoutPanel);

            timerRefreshGui.Interval = refreshIntervalInMs;
            timerRefreshGui.Elapsed += OnTimedEventRefreshGui;
            timerRefreshGui.Enabled = true;
        }

        private void RestoreMonitoringFeedback() {
            this.BeginInvoke(new Action(() => {
                foreach (var one in allCheckBoxLastValues) {
                    CheckBox checkBox = one.Key;
                    bool value = one.Value;
                    checkBox.Checked = value;
                }
                this.Enabled = true;
            }));
        }

        private void DisabledMonitoringFeedback() {
            this.Invoke(new Action(() => {
                foreach (CheckBox checkBox in checkboxTextboxFuncDict.Keys.ToHashSet()) {
                    allCheckBoxLastValues[checkBox] = checkBox.Checked;
                }
                foreach (CheckBox checkBox in checkboxTextboxFuncDict.Keys.ToHashSet()) {
                    checkBox.Checked = false;
                }
                this.Enabled = false;
            }));
        }

        private void OnTimedEventRefreshGui(object sender, ElapsedEventArgs e) {
            timerRefreshGui.Enabled = false;

            lock (mutex) {
                foreach (var one in checkboxTextboxFuncDict) {
                    bool selected = false;
                    CheckBox checkBox = one.Key;
                    this.Invoke(new Action(() => {
                        selected = checkBox.Checked;
                    }));
                    if (!selected) {
                        continue;
                    }

                    Func<double> feedbackFunc = one.Value.getFeedback;
                    double feedback = feedbackFunc.Invoke();
                    TextBox textBox = one.Value.textBox;
                    this.BeginInvoke(new Action(() => {
                        textBox.Text = feedback.ToString("0.0000");
                    }));
                }
            }

            timerRefreshGui.Enabled = true;
        }

        private readonly System.Timers.Timer timerRefreshGui = new System.Timers.Timer();
        private readonly List<CheckBox> checkBoxesForEachFeedback = new List<CheckBox>();
        private readonly List<TextBox> textBoxesForEachFeedback = new List<TextBox>();
        private readonly List<(string titleResourceId, Func<double> funcGetFeedback)> monitoringList = new List<(string titleResourceId, Func<double> funcGetFeedback)>();

        private readonly List<InstrumentUsage> instrumentsUsages;
        private readonly Dictionary<CheckBox, (TextBox textBox, Func<double> getFeedback)> checkboxTextboxFuncDict = new Dictionary<CheckBox, (TextBox textBox, Func<double> getFeedback)>();
        private readonly Dictionary<string, (string instrumentUsageId, Dictionary<string, string> initSettingDict, Dictionary<string, string> triggerInSettingDict)> couplingFeedbackConfigDict;

        private readonly int refreshIntervalInMs;
        private readonly Dictionary<string, string> feedbackMonitoringConfig;
        private readonly object mutex = new object();
        private readonly Font DEFAULT_FONT = new Font("Courier New", 12, FontStyle.Regular);

        private readonly Dictionary<CheckBox, bool> allCheckBoxLastValues = new Dictionary<CheckBox, bool>();
        private readonly FormResourceCulture frc;
    }
}
