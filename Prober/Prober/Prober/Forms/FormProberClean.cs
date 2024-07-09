using CommonApi.MyUtility;
using MyInstruments.MyUtility;
using MyInstruments;
using MyMotionStageDriver.MyStageAxis;
using NLog;
using Prober.Constant;
using Prober.WaferDef;
using ProberApi.MyConstant;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyInstruments.MyCamera;
using MyInstruments.MyElecLens;
using MyInstruments.MyEnum;
using Prober.Request;
using System.Runtime.InteropServices;
using HalconDotNet;
using MathNet.Numerics.Distributions;

namespace Prober.Forms
{
    public partial class FormProberClean : Form
    {
        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        protected static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        private readonly Dictionary<string, StageAxis> stageAxisDic = new Dictionary<string, StageAxis>();

        private readonly Dictionary<string, Instrument> instruments;
        private readonly List<InstrumentUsage> instrumentUsageList;

        private StandaloneCamera camera;
        private StandaloneElecLens eLens;

        private WaferManual waferHandle;
        private readonly ConcurrentDictionary<string, object> sharedObjects;

        public FormProberClean(ConcurrentDictionary<string, object> sharedObjects)
        {
            InitializeComponent();

            this.sharedObjects = sharedObjects;

            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            stageAxisUsages = tempObj as Dictionary<string, StageAxis>;

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            sharedObjects.TryGetValue(PrivateSharedObjectKey.WAFER_HANDLE, out tempObj);
            waferHandle = tempObj as WaferManual;

            //获取仪表
            var getResult2 = GetInstrument("top_camera");
            var getResult4 = GetInstrument("elens_zoom");

            //获取轴
            GetStageAxisDic();

            ClearProberInfo clearInfo = ConfigMgr.LoadClearProberInfo();
            if (clearInfo != null) {
                UIClass.ObjectToControl(clearInfo, panel_CleanProber);
            }

            ResetClearPaperCtlEx(clearInfo);
        }

        public bool GetStageAxisDic()
        {
            List<string> axisUseList = new List<string>();
            axisUseList.Add(MyStageAxisKey.LEFT_X);
            axisUseList.Add(MyStageAxisKey.LEFT_Y);
            axisUseList.Add(MyStageAxisKey.LEFT_Z);
            axisUseList.Add(MyStageAxisKey.LEFT_SX);
            axisUseList.Add(MyStageAxisKey.LEFT_SY);
            axisUseList.Add(MyStageAxisKey.LEFT_SZ);

            axisUseList.Add(MyStageAxisKey.RIGHT_X);
            axisUseList.Add(MyStageAxisKey.RIGHT_Y);
            axisUseList.Add(MyStageAxisKey.RIGHT_Z);
            axisUseList.Add(MyStageAxisKey.RIGHT_SX);
            axisUseList.Add(MyStageAxisKey.RIGHT_SY);
            axisUseList.Add(MyStageAxisKey.RIGHT_SZ);

            axisUseList.Add(MyStageAxisKey.CCD_X);
            axisUseList.Add(MyStageAxisKey.CCD_Y);
            axisUseList.Add(MyStageAxisKey.CCD_Z);
            axisUseList.Add(MyStageAxisKey.HEIGHT_U);

            axisUseList.Add(MyStageAxisKey.CHUCK_X);
            axisUseList.Add(MyStageAxisKey.CHUCK_Y);
            axisUseList.Add(MyStageAxisKey.CHUCK_Z);
            axisUseList.Add(MyStageAxisKey.CHUCK_SZ);

            for (int i = 0; i < axisUseList.Count; i++)
            {
                var result = GetStageAxis(axisUseList[i]);
                if (!result.isOK)
                {
                    return false;
                }
                stageAxisDic.Add(axisUseList[i], result.stageAxis);
            }

            return true;
        }

        public (bool isOk, string errorText, Instrument instrument) GetInstrument(string instrumentUsageId)
        {
            string errorText = string.Empty;
            var list = this.instrumentUsageList.Where(x => x.UsageId.Equals(instrumentUsageId)).ToList();
            InstrumentUsage instrumentUsage = list.First();
            if (list == null)
            {
                errorText = $"GetInstrumentUsage(={instrumentUsageId}) does not exist!";
                LOGGER.Error(errorText);
                return (false, errorText, null);
            }

            Instrument instrument = instruments[instrumentUsage.InstrumentId];
            switch (instrumentUsage.InstrumentCategory)
            {
                case EnumInstrumentCategory.CCD:
                    camera = instrument as StandaloneCamera;
                    break;
                case EnumInstrumentCategory.ELENS:
                    eLens = instrument as StandaloneElecLens;
                    break;
                default:
                    errorText = $"{instrumentUsage.InstrumentCategory.ToString()} is not a valid instrument category of coupling feedback!";
                    LOGGER.Error(errorText);
                    return (false, errorText, null);
            }

            return (true, string.Empty, instrument);
        }

        internal (bool isOK, string errorMessage, StageAxis stageAxis) GetStageAxis(string axisUsageId)
        {
            string errorText = string.Empty;
            string INVALID_PARAMETERS = $"input parameters(={axisUsageId}) is invalid";

            if (!stageAxisUsages.ContainsKey(axisUsageId))
            {
                errorText = $"{INVALID_PARAMETERS}";
                LOGGER.Error(errorText);
                return (false, errorText, null);
            }

            return (true, null, stageAxisUsages[axisUsageId]);
        }

        private void btn_ClearProber_Update_Click(object sender, EventArgs e)
        {
            txtClearProber_ChuckX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2).ToString();
            txtClearProber_ChuckY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2).ToString();
            txtClearProber_ChuckZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Z].Position(), 2).ToString();

            txtClearProber_CcdX.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_X].Position(), 2).ToString();
            txtClearProber_CcdY.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Y].Position(), 2).ToString();
            txtClearProber_CcdZ.Text = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Z].Position(), 2).ToString();

            camera.GetBaseParameters(out double ex, out double g, out double gama);
            txtClearProber_Exp.Text = ex.ToString();

            eLens.GetZoom(out double zoomValue);
            txtClearProber_Zoom.Text = Math.Round(zoomValue, 0).ToString();
        }

        private void btnClearProber_Save_Click(object sender, EventArgs e)
        {
            ClearProberInfo clearInfo = ConfigMgr.LoadClearProberInfo();
            UIClass.ControlToObject(panel_CleanProber, clearInfo);
            if (ConfigMgr.SaveClearProberInfo(clearInfo))
                MessageBox.Show(this, "保存成功", "Info:");
            else
                MessageBox.Show(this, "保存失败", "Info:");
        }

        private void btnClearProber_Load_Click(object sender, EventArgs e)
        {
            ClearProberInfo clearInfo = ConfigMgr.LoadClearProberInfo();
            if (clearInfo != null)
            {
                UIClass.ObjectToControl(clearInfo, panel_CleanProber);
            }
        }

        private void btnClearProber_Reset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要重置吗？", "Ask:", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            Task.Factory.StartNew(() => { 
                try {
                    var info = ConfigMgr.LoadClearProberInfo();
                    info.InitClearPaper();
                    if (ConfigMgr.SaveClearProberInfo(info)) {
                        ResetClearPaperCtlEx(info);
                        Invoke(new Action(() => { MessageBox.Show(this, "重置成功。", "Info:"); }));
                    } else {
                        Invoke(new Action(() => { MessageBox.Show(this, "重置失败。", "Info:"); }));
                    }
                } catch(Exception ex) {
                    LOGGER.Error(ex);
                }                               
            });
        }

        private void ResetClearPaperCtl(ClearProberInfo info)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    ResetClearPaperCtl(info);
                }));
            }
            else
            {
                table_ClearPaper.Controls.Clear();
                table_ClearPaper.RowStyles.Clear();
                table_ClearPaper.ColumnStyles.Clear();

                float rowPercent = table_ClearPaper.Height / info.RowCount;
                //float colPecent = 100.0f / info.ColumnCount;
                float colPecent = table_ClearPaper.Width / info.ColumnCount;
                table_ClearPaper.RowCount = info.RowCount;
                table_ClearPaper.ColumnCount = info.ColumnCount;
                for (int i = 0; i < info.RowCount; i++)
                {
                    table_ClearPaper.RowStyles.Add(new RowStyle(SizeType.Absolute, rowPercent));
                }
                for (int j = 0; j < info.ColumnCount; j++)
                {
                    table_ClearPaper.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, colPecent));
                }

                for (int i = 0; i < info.RowCount; i++)
                {
                    for (int j = 0; j < info.ColumnCount; j++)
                    {
                        var paperInfo = info.PaperPosList.FirstOrDefault(t => t.RowIndex == i && t.ColumnIndex == j);
                        Label btn = new Label();
                        btn.FlatStyle = FlatStyle.Flat;                        
                        btn.Text = string.Empty;
                        btn.Dock = DockStyle.Fill;
                        btn.AutoSize = true;
                        
                        if (paperInfo != null && paperInfo.IsUsed)
                        {
                            btn.BackColor = Color.Red;
                        }
                        else
                            btn.BackColor = Color.Green;
                        table_ClearPaper.Controls.Add(btn);
                        table_ClearPaper.SetCellPosition(btn, new TableLayoutPanelCellPosition(j, i));
                    }
                }
            }
        }

        private void ResetClearPaperCtlEx(ClearProberInfo info) {
            if (this.InvokeRequired) {
                BeginInvoke(new Action(() => {
                    ResetClearPaperCtlEx(info);
                }));
            } else {
                table_ClearPaper.SuspendLayout();
                table_ClearPaper.Controls.Clear();
                table_ClearPaper.ResumeLayout();
                table_ClearPaper.RowStyles.Clear();
                table_ClearPaper.ColumnStyles.Clear();

                float rowPercent = table_ClearPaper.Height / info.RowCount;
                //float colPecent = 100.0f / info.ColumnCount;
                float colPecent = table_ClearPaper.Width / info.ColumnCount;
                table_ClearPaper.RowCount = info.RowCount;
                table_ClearPaper.ColumnCount = info.ColumnCount;
                for (int i = 0; i < info.RowCount; i++) {
                    table_ClearPaper.RowStyles.Add(new RowStyle(SizeType.Absolute, rowPercent));
                }
                for (int j = 0; j < info.ColumnCount; j++) {
                    table_ClearPaper.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, colPecent));
                }

                List<Label> buttons = new List<Label>();
                for (int i = 0; i < info.RowCount; i++) {
                    for (int j = 0; j < info.ColumnCount; j++) {
                        var paperInfo = info.PaperPosList.FirstOrDefault(t => t.RowIndex == i && t.ColumnIndex == j);
                        Label btn = new Label();
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.Text = string.Empty;
                        btn.Dock = DockStyle.Fill;
                        btn.AutoSize = true;

                        if (paperInfo != null && paperInfo.IsUsed) {
                            btn.BackColor = Color.Red;
                        } else
                            btn.BackColor = Color.Green;

                        buttons.Add(btn);                        
                    }
                }

                table_ClearPaper.Controls.AddRange(buttons.ToArray());
                int count = buttons.Count();
                int col = 0;
                int row = 0;    
                for (int i = 0; i < count; i++) {
                    col = i % info.ColumnCount;
                    row = i / info.ColumnCount;                    
                    table_ClearPaper.SetCellPosition(buttons[i], new TableLayoutPanelCellPosition(col, row));
                }                
            }
        }

        private void UpdateClearPaperCtl(ClearProberInfo info) {
            if (this.InvokeRequired) {
                BeginInvoke(new Action(() => {
                    UpdateClearPaperCtl(info);
                }));
            } else {
                table_ClearPaper.RowStyles.Clear();
                table_ClearPaper.ColumnStyles.Clear();

                float rowPercent = table_ClearPaper.Height / info.RowCount;
                //float colPecent = 100.0f / info.ColumnCount;
                float colPecent = table_ClearPaper.Width / info.ColumnCount;
                table_ClearPaper.RowCount = info.RowCount;
                table_ClearPaper.ColumnCount = info.ColumnCount;
                for (int i = 0; i < info.RowCount; i++) {
                    table_ClearPaper.RowStyles.Add(new RowStyle(SizeType.Absolute, rowPercent));
                }
                for (int j = 0; j < info.ColumnCount; j++) {
                    table_ClearPaper.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, colPecent));
                }

                for (int i = 0; i < info.RowCount; i++) {
                    for (int j = 0; j < info.ColumnCount; j++) {
                        var paperInfo = info.PaperPosList.FirstOrDefault(t => t.RowIndex == i && t.ColumnIndex == j);
                        //Button btn = table_ClearPaper.Controls[index++] as Button;
                        var btn = table_ClearPaper.GetControlFromPosition(j, i) as Label;
                        if (paperInfo != null && paperInfo.IsUsed) {
                            btn.BackColor = Color.Red;
                        } else
                            btn.BackColor = Color.Green;                        
                    }
                }
            }
        }

        private void btnClearProber_Run_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要开始自动清针吗？", "提示：", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            bool enableMonitor = false;
            Task.Run(() =>
            {
                try
                {
                    sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_POSITION_MONITOR_ENABLE, out object enable);
                    enableMonitor = (bool)enable;
                    if (enableMonitor)
                    {
                        sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POSITION_MONITOR_ENABLE, false, (key, oldValue) => false);
                        waferHandle.DealWithChuckSafePos(false);
                    }                    

                    bool isOK = waferHandle.AutoClearProber(UpdateClearPaperCtl, out string error);

                    Invoke(new Action(() =>
                    {
                        if (isOK)
                        {
                            MessageBox.Show(this, "自动清针完成。", "Info:");
                        }
                        else
                        {
                            MessageBox.Show(this, "自动清针失败。" + error, "Info:");
                        }
                    }));
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() => { MessageBox.Show(this, "清针异常" + ex.Message, "Info:"); }));
                    LOGGER.Error(ex);
                }
                finally
                {
                    if (enableMonitor)
                    {
                        sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POSITION_MONITOR_ENABLE, true, (key, oldValue) => true);
                        waferHandle.DealWithChuckSafePos(true);
                    }
                }                
            });
        }        
    }
}
