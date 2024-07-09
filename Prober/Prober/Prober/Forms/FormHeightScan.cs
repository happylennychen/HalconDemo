using CommonApi.MyUtility;
using MyInstruments.MyAltimeter;
using MyInstruments.MyUtility;
using MyInstruments;
using MyMotionStageDriver.MyStageAxis;
using NLog;
using Prober.WaferDef;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProberApi.MyConstant;
using System.Collections.Concurrent;
using Prober.Constant;
using MyInstruments.MyEnum;
using HalconDotNet;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Prober.Forms
{
    public partial class FormHeightScan : Form
    {
        State MotionState = State.Ready;
        public HCalibrationInfo TempHCalibrateInfo = new HCalibrationInfo();
        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        private readonly Dictionary<string, StageAxis> stageAxisDic = new Dictionary<string, StageAxis>();
        protected static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);

        private readonly List<InstrumentUsage> instrumentUsageList;
        private readonly Dictionary<string, Instrument> instruments;

        private StandaloneAm Am_Cap;    //电容测高仪
        private StandaloneAm Am_LD;     //激光测高仪
        private StandaloneAm Altimeter;

        string CurrentWaferType = string.Empty;
        public FormHeightScan(ConcurrentDictionary<string, object> sharedObjects, string waferType)
        {
            InitializeComponent();

            CurrentWaferType = waferType;

            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            stageAxisUsages = tempObj as Dictionary<string, StageAxis>;

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;
            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            //获取轴
            GetStageAxisDic();

            //获取仪表 
            var getResult2 = GetInstrument("ld_altimeter");
            var getResult4 = GetInstrument("cap_altimeter");

            Altimeter = Am_LD;
        }

        private void FormHeightScan_Load(object sender, EventArgs e)
        {

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
                case EnumInstrumentCategory.ALTIMETER:
                    if (instrumentUsage.InstrumentId == "altimeter_ld")
                    {
                        Am_LD = instrument as StandaloneAm;
                        Altimeter = Am_LD;
                    }
                    else if (instrumentUsage.InstrumentId == "altimeter_cap")
                    {
                        Am_Cap = instrument as StandaloneAm;
                    }
                    else
                    {
                        errorText = $"{instrumentUsage.InstrumentCategory.ToString()} is not a valid instrument category of coupling feedback!";
                        LOGGER.Error(errorText);
                        return (false, errorText, null);
                    }
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

        private void btn_HeightCalibration_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("确定要生成新的采样数据吗？", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }

                UIClass.ControlToObject(panel_HeightCalibrate, TempHCalibrateInfo);

                //获取晶圆中心位置坐标
                var equipInfo = ConfigMgr.LoadEquipmentCalibration();
                TempHCalibrateInfo.CenterX = equipInfo.Base_ChuckX;
                TempHCalibrateInfo.CenterY = equipInfo.Base_ChuckY;
                txt_CenterX2.Text = equipInfo.Base_ChuckX.ToString("F2");
                txt_CenterY2.Text = equipInfo.Base_ChuckY.ToString("F2");

                //获取Wafer Reticle宽度和高度信息
                var map = ConfigMgr.LoadWaferMapInfoByType(CurrentWaferType);
                if (map == null)
                {
                    MessageBox.Show($"当前不存在{CurrentWaferType}类型的晶圆Map图");
                    return;
                }
                TempHCalibrateInfo.ReticleWidth = map.DieWidth;
                TempHCalibrateInfo.ReticleHeight = map.DieHeight;
                txt_ReticleHeight.Text = map.DieHeight.ToString();
                txt_ReticleWidth.Text = map.DieWidth.ToString();

                TempHCalibrateInfo.BasicPoints = GetBasicPosition();

                if (!TempHCalibrateInfo.GeneratePointInfoWithBasicPos())
                {
                    MessageBox.Show(this, "生成采样点数据失败！", "提示：");
                }
                else
                {
                    ShowHPoints(TempHCalibrateInfo);
                    lv_H.Items.Clear();
                    btn_RunHeightCalibrate.Enabled = true;
                    MessageBox.Show(this, "生成采样点数据成功！", "提示：");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"高度计算异常:{ex.Message}");
                return;  
            }         
        }

        private void ShowHPoints(HCalibrationInfo info)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => ShowHPoints(info)));
            }
            else
            {
                chart_H.Series[0].Points.Clear();
                foreach (var item in info.Points)
                {
                    chart_H.Series[0].Points.AddXY(item.WaferX, item.WaferY);
                }
            }
        }


        private void btn_Add_Click(object sender, EventArgs e)
        {
            double x2 = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(), 2);
            double y2 = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(), 2);

            if (!GetHeight(Cap_Altimeter_Channel.Left_Channel, out double h))
            {
                MessageBox.Show("高度采集失败");
                return;
            }

            dgv_BasicSamplePoint.Rows.Add(x2,y2,h);
        }

        public List<HeightScanBasicPosition> GetBasicPosition()
        {
            List<HeightScanBasicPosition> list = new List<HeightScanBasicPosition>();
            foreach (DataGridViewRow row in dgv_BasicSamplePoint.Rows)
            {
                HeightScanBasicPosition pos = new HeightScanBasicPosition();
                try
                {
                    pos.X = Math.Round(Convert.ToDouble(row.Cells[0].Value), 2);
                    pos.Y = Math.Round(Convert.ToDouble(row.Cells[1].Value), 2);
                    pos.H = Math.Round(Convert.ToDouble(row.Cells[2].Value), 2);
                }
                catch (Exception)
                {
                    continue;
                }
                list.Add(pos);
            }

            return list;
        }

        private void btn_SaveBasicPos_Click(object sender, EventArgs e)
        {
            List<HeightScanBasicPosition> list = new List<HeightScanBasicPosition>();
            foreach(DataGridViewRow row in dgv_BasicSamplePoint.Rows)
            {
                HeightScanBasicPosition pos = new HeightScanBasicPosition();
                try
                {
                    pos.X = Math.Round(Convert.ToDouble(row.Cells[0].Value),2);
                    pos.Y = Math.Round(Convert.ToDouble(row.Cells[1].Value), 2);
                    pos.H = Math.Round(Convert.ToDouble(row.Cells[2].Value), 2);
                }
                catch (Exception)
                {
                    continue;
                }
                list.Add(pos);
            }

            ConfigMgr.SaveHeightScanBasicPosition(CurrentWaferType, list);
        }

        private void btn_LoadBasicPos_Click(object sender, EventArgs e)
        {
            List<HeightScanBasicPosition> list = ConfigMgr.LoadHeightScanBasicPosition(CurrentWaferType);
            foreach(var pos in list)
            {
                dgv_BasicSamplePoint.Rows.Add(pos.X,pos.Y,pos.H);
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            while(dgv_BasicSamplePoint.Rows.Count > 0)
            {
                dgv_BasicSamplePoint.Rows.RemoveAt(0);
            }
        }

        private bool GetHeight(int channel, out double Value)
        {
            Value = double.NaN;

            for (int i = 0; i < 5; i++)
            {
                if (Altimeter.GetHeight(channel, out Value))
                {
                    return true;
                }
                else
                {
                    Thread.Sleep(500);
                }
            }

            return false;
        }

        private void chb_AltimeterTypeCap_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_AltimeterTypeCap.Checked == true)
            {
                Altimeter = Am_Cap;
            }
            else
            {
                Altimeter = Am_LD;
            }
        }

        private void btn_HeightCalibration_Loading_Click(object sender, EventArgs e)
        {
            var info = ConfigMgr.LoadHCalibration(CurrentWaferType, "1");

            if (info != null)
            {
                TempHCalibrateInfo = info;
                UIClass.ObjectToControl(info, panel_HeightCalibrate);
                ShowHPoints(info);
                ShowSamplintPoint();
                ShowBasicPoint();
                btn_RunHeightCalibrate.Enabled = false;
                MessageBox.Show(this, "加载成功！", "提示：");
            }
            else
            {
                MessageBox.Show(this, "加载失败！", "提示：");
            }
        }

        private void ShowSamplintPoint()
        {
            lv_H.Items.Clear();
            foreach (var point in TempHCalibrateInfo.Points)
            {
                var item = lv_H.Items.Add((lv_H.Items.Count + 1).ToString());
                item.SubItems.Add(point.X.ToString());
                item.SubItems.Add(point.Y.ToString());
                item.SubItems.Add(point.Z.ToString());
            }
        }

        private void ShowBasicPoint()
        {
            while (dgv_BasicSamplePoint.Rows.Count > 0)
            {
                dgv_BasicSamplePoint.Rows.RemoveAt(0);
            }

            foreach (var point in TempHCalibrateInfo.BasicPoints)
            {
                dgv_BasicSamplePoint.Rows.Add(point.X, point.Y, point.H);
            }
        }

        private void btn_RunHeightCalibrate_Click(object sender, EventArgs e)
        {
            if (CurrentWaferType == string.Empty)
            {
                MessageBox.Show(this, "请选择晶圆类型！", "提示：");
                return;
            }

            //补充设置wafer移动速度接口
            btn_RunHeightCalibrate.Enabled = false;
            lv_H.Items.Clear();
            btn_StopHeightCalibrate.Enabled = true;

            Task.Run(() => {
                try
                {
                    MotionState = State.Busy;

                    if (ScanWaferHeight())
                    {
                        Invoke(new Action(() => { MessageBox.Show(this, "高度扫描完成！", "提示："); }));
                    }
                    else
                    {
                        if (MotionState == State.Stop)
                        {
                            Invoke(new Action(() => { MessageBox.Show(this, "高度扫描停止！", "提示："); }));
                        }
                        else
                        {
                            Invoke(new Action(() => { MessageBox.Show(this, "高度扫描失败！", "提示："); }));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() => { MessageBox.Show(this, $"高度扫描过程异常:{ex.Message}！", "警告："); }));
                }
                finally
                {
                    MotionState = State.Ready;
                    Invoke(new Action(() => {
                        btn_StopHeightCalibrate.Enabled = false;
                        btn_RunHeightCalibrate.Enabled = true;
                    }));
                }
            });
        }

        public bool Point5MoveWafer(double step, out double avg, int port = Cap_Altimeter_Channel.Left_Channel, int delay = 0) {
            avg = 0;

            double x2 = stageAxisDic[MyStageAxisKey.CHUCK_X].Position();
            double y2 = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position();
            double x0 = 0;
            double y0 = step;
            double[] xArray = new double[5];
            double[] yArray = new double[5];
            for (int i = 0; i < 5; i++) {
                HOperatorSet.TupleRad(i * 72, out HTuple rad);
                xArray[i] = x0 * Math.Cos(rad.D) - y0 * Math.Sin(rad.D);
                yArray[i] = x0 * Math.Sin(rad.D) + y0 * Math.Cos(rad.D);
                xArray[i] += x2;
                yArray[i] += y2;
            }

            List<double> hList = new List<double>();
            for (int i = 0; i < 5; i++) {
                Dim2MoveAbs(MyStageAxisKey.CHUCK_X, MyStageAxisKey.CHUCK_Y, xArray[i], yArray[i]);
                Thread.Sleep(delay);

                if (!GetHeight(port, out double h)) {
                    return false;
                }

                hList.Add(h);
            }

            //此处滤掉切割槽数据的处理方式待思考
            double avgTemp = hList.Average();
            List<double> listTemp = new List<double>();
            for (int i = 0; i < hList.Count; i++) {
                if (hList[i] > avgTemp - 8)
                    listTemp.Add(hList[i]);
            }
            avg = listTemp.Average();

            return true;
        }

        public bool ScanWaferHeight()
        {
            for (int time = 0; time < 1; time++) {
            
                int delay = 0;
                var equipmentInfo = ConfigMgr.LoadEquipmentCalibration();
                MoveAllToSafePos(equipmentInfo);

                stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipmentInfo.ChuckZ);
                stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

                //激光测高仪需要移动
                if (!chb_AltimeterTypeCap.Checked)
                {
                    stageAxisDic[MyStageAxisKey.HEIGHT_U].MoveAbsolute(equipmentInfo.HSenserWorkU);
                    stageAxisDic[MyStageAxisKey.HEIGHT_U].GuiUpdatePosition();
                }
                else
                {
                    Invoke(new Action(() => { MessageBox.Show(this, "请将电容测高仪移动到Wafer中心位置！", "提示："); }));
                }

                List<RecordPointInfo> outList = new List<RecordPointInfo>();
                var map = ConfigMgr.LoadWaferMapInfoByType(CurrentWaferType);
                int rowCount = TempHCalibrateInfo.GetRowCountByReticle();
                int colCount = TempHCalibrateInfo.GetRowCountByReticle();
                int orderCount = TempHCalibrateInfo.BasicPoints.Count();
                int dir = 1;
                double heightCenter = double.NaN;
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        int colIndex = dir > 0 ? j + 1 : colCount - j;
                        for (int k = 0; k < orderCount; k++)
                        {
                            var item = TempHCalibrateInfo.Points.FirstOrDefault(t => t.Row == i + 1 && t.Column == colIndex && t.Order == k + 1);
                            if (item == null)
                                continue;
                            if (MotionState == State.Stop)
                                return false;

                            Dim2MoveAbs(MyStageAxisKey.CHUCK_X, MyStageAxisKey.CHUCK_Y, item.X, item.Y);
                            Thread.Sleep(delay);

                            if (!GetHeight(Cap_Altimeter_Channel.Left_Channel, out double h)) {
                                return false;
                            }
    #if false
                            if (!Point5MoveWafer(50, out double h)) {
                                return false;
                            }
    #endif
                            item.Z = h;
                            //只是测量高度，暂时不排除点
                            /*
                            if (Math.Abs(curH - map.HeightH2) > 60)
                                outList.Add(item);
                            */
                            //记录晶圆中心点的高度为比较高度
                            if ((i == rowCount / 2) && (j == colCount / 2)) {
                                heightCenter = h;
                            }

                            ShowCurrentSamplingPoint(item);
                        }                    
                    }
                    dir = -dir;
                }

                //与中心点高度相差60um的点均认为是异常点，删除
                foreach (var item in TempHCalibrateInfo.Points)
                {                
                    if (Math.Abs(item.Z - heightCenter) > TempHCalibrateInfo.HeightThreshold)
                        outList.Add(item);
                }

                //记录一下高度信息
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("X2,Y2,H");
                foreach (var item in TempHCalibrateInfo.Points)
                {
                    sb.AppendLine($"{item.X},{item.Y},{item.Z}");
                }
                File.AppendAllText($"{CurrentWaferType}_WaferHeightData_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv", sb.ToString());

                foreach (var item in outList)
                {
                    TempHCalibrateInfo.Points.Remove(item);
                }

                TempHCalibrateInfo.GenerateLinesEx();
                if (!ConfigMgr.SaveHCalibration(CurrentWaferType, TempHCalibrateInfo,"1"))
                {
                    return false;
                }

                if (!chb_AltimeterTypeCap.Checked)
                {
                    stageAxisDic[MyStageAxisKey.HEIGHT_U].MoveAbsolute(equipmentInfo.Safe_U);
                    stageAxisDic[MyStageAxisKey.HEIGHT_U].GuiUpdatePosition();
                }
            }

            return true;
        }

        private void ShowCurrentSamplingPoint(RecordPointInfo info)
        {
            //BeginInvoke(new Action(() =>
            Invoke(new Action(() =>
            {
                if (chart_H.Series[0].Points.Count > TempHCalibrateInfo.Points.Count)
                {
                    chart_H.Series[0].Points.RemoveAt(chart_H.Series[0].Points.Count - 1);
                }
                int index = chart_H.Series[0].Points.AddXY(info.WaferX, info.WaferY);
                chart_H.Series[0].Points[index].MarkerSize = 12;
                chart_H.Series[0].Points[index].MarkerColor = Color.Red;

                var item = lv_H.Items.Add((lv_H.Items.Count + 1).ToString());
                item.SubItems.Add(info.X.ToString());
                item.SubItems.Add(info.Y.ToString());
                item.SubItems.Add(info.Z.ToString());
                item.Selected = true;
                item.EnsureVisible();
            }));
        }

        public void Dim2MoveAbs(string axisX, string axisY, double disX, double disY)
        {
            var t1 = Task.Run(() =>
            {
                stageAxisDic[axisX].MoveAbsolute(disX);
            });
            var t2 = Task.Run(() =>
            {
                stageAxisDic[axisY].MoveAbsolute(disY);
            });
            Task.WaitAll(new Task[] { t1, t2 });

            return;
        }

        public void Dim2MoveRel(string axisX, string axisY, double disX, double disY)
        {
            var t1 = Task.Run(() =>
            {
                stageAxisDic[axisX].MoveRelative(disX);
            });
            var t2 = Task.Run(() =>
            {
                stageAxisDic[axisY].MoveRelative(disY);
            });
            Task.WaitAll(new Task[] { t1, t2 });

            return;
        }

        public void SetLRAxisSpeedToNormal()
        {
            double maxSpeed = stageAxisDic[MyStageAxisKey.LEFT_X].speedMaxConfig;
            double minSpeed = stageAxisDic[MyStageAxisKey.LEFT_X].speedMinConfig;
            double accTime = stageAxisDic[MyStageAxisKey.LEFT_X].accConfig;
            stageAxisDic[MyStageAxisKey.LEFT_X].SetAxisSpeed(minSpeed, maxSpeed, accTime);

            maxSpeed = stageAxisDic[MyStageAxisKey.LEFT_Y].speedMaxConfig;
            minSpeed = stageAxisDic[MyStageAxisKey.LEFT_Y].speedMinConfig;
            accTime = stageAxisDic[MyStageAxisKey.LEFT_Y].accConfig;
            stageAxisDic[MyStageAxisKey.LEFT_Y].SetAxisSpeed(minSpeed, maxSpeed, accTime);

            maxSpeed = stageAxisDic[MyStageAxisKey.RIGHT_X].speedMaxConfig;
            minSpeed = stageAxisDic[MyStageAxisKey.RIGHT_X].speedMinConfig;
            accTime = stageAxisDic[MyStageAxisKey.RIGHT_X].accConfig;
            stageAxisDic[MyStageAxisKey.RIGHT_X].SetAxisSpeed(minSpeed, maxSpeed, accTime);

            maxSpeed = stageAxisDic[MyStageAxisKey.RIGHT_Y].speedMaxConfig;
            minSpeed = stageAxisDic[MyStageAxisKey.RIGHT_Y].speedMinConfig;
            accTime = stageAxisDic[MyStageAxisKey.RIGHT_Y].accConfig;
            stageAxisDic[MyStageAxisKey.RIGHT_Y].SetAxisSpeed(minSpeed, maxSpeed, accTime);
        }

        public void MoveAllToSafePos(EquipmentCalibrationInfo calInfo)
        {
            SetLRAxisSpeedToNormal();

            //相机退回到安全位置
            var t0 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(calInfo.Safe_CcdZ);
            });
            var t8 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(calInfo.Safe_ChuckZ);
            });
            Task.WaitAll(new Task[] { t0, t8 });
            stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

            //测高仪退回到安全位置
            var t1 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.HEIGHT_U].MoveAbsolute(calInfo.Safe_U);
            });
            //左右六轴Z1/Z3
            var t2 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.LEFT_Z].MoveAbsolute(calInfo.Safe_LeftZ);
            });
            var t3 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.RIGHT_Z].MoveAbsolute(calInfo.Safe_RightZ);
            });
            Task.WaitAll(new Task[] { t1, t2, t3 });
            stageAxisDic[MyStageAxisKey.HEIGHT_U].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Z].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Z].GuiUpdatePosition();

            //左右六轴X1/Y1/X3/Y3
            var t4 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.LEFT_X].MoveAbsolute(calInfo.Safe_LeftX);
            });
            var t5 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.LEFT_Y].MoveAbsolute(calInfo.Safe_LeftY);
            });
            var t6 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.RIGHT_X].MoveAbsolute(calInfo.Safe_RightX);
            });
            var t7 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.RIGHT_Y].MoveAbsolute(calInfo.Safe_RightY);
            });
            Task.WaitAll(new Task[] { t4, t5, t6, t7 });
            stageAxisDic[MyStageAxisKey.LEFT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.LEFT_Y].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.RIGHT_Y].GuiUpdatePosition();
        }

        private void btn_StopHeightCalibrate_Click(object sender, EventArgs e)
        {
            MotionState = State.Stop;
        }

        private void lv_H_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lv_H.SelectedIndices.Count < 1)
            {
                return;
            }
            int index = lv_H.SelectedIndices[0];
            double x2 = Convert.ToDouble(lv_H.Items[index].SubItems[1].Text);
            double y2 = Convert.ToDouble(lv_H.Items[index].SubItems[2].Text);
            var point = TempHCalibrateInfo.Points.FirstOrDefault(t => Math.Abs(t.X - x2) < 0.01 && Math.Abs(t.Y - y2) < 0.01);
            if (point == null)
                return;
            if (chart_H.Series[0].Points.Count > TempHCalibrateInfo.Points.Count)
            {
                chart_H.Series[0].Points.RemoveAt(chart_H.Series[0].Points.Count - 1);
            }
            int p_index = chart_H.Series[0].Points.AddXY(point.WaferX, point.WaferY);
            chart_H.Series[0].Points[p_index].MarkerSize = 12;
            chart_H.Series[0].Points[p_index].MarkerColor = Color.Red;
        }

        private void btn_InitPos_Click(object sender, EventArgs e) {
            var equipmentInfo = ConfigMgr.LoadEquipmentCalibration();
            MoveAllToSafePos(equipmentInfo);

            stageAxisDic[MyStageAxisKey.CHUCK_Z].MoveAbsolute(equipmentInfo.ChuckZ);
            stageAxisDic[MyStageAxisKey.CHUCK_Z].GuiUpdatePosition();

            //增加Chuck移动到中心位置
            stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(equipmentInfo.Base_ChuckX);
            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();

            stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(equipmentInfo.Base_ChuckY);
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();

            //激光测高仪需要移动
            if (!chb_AltimeterTypeCap.Checked) {
                stageAxisDic[MyStageAxisKey.HEIGHT_U].MoveAbsolute(equipmentInfo.HSenserWorkU);
                stageAxisDic[MyStageAxisKey.HEIGHT_U].GuiUpdatePosition();
            } else {
                Invoke(new Action(() => { MessageBox.Show(this, "请将电容测高仪移动到Wafer中心位置！", "提示："); }));
            }

            Invoke(new Action(() => { MessageBox.Show(this, "各轴运动到初始位置成功", "提示："); }));
        }

        private void btn_AutoSerch_Click(object sender, EventArgs e) {
            if (MessageBox.Show("确定要开始自动搜索吗？", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            int count = 100;    //扫描范围5mm,最大搜索范围5mm*5mm
            double step = 50;   //扫描步长50um
            double limit = 1;   //限制1um

            Task.Run(() => {

                try {
                    while (dgv_BasicSamplePoint.Rows.Count > 0) {
                        dgv_BasicSamplePoint.Rows.RemoveAt(0);
                    }

                    //起始位置
                    double xPosStart = stageAxisDic[MyStageAxisKey.CHUCK_X].Position();
                    double yPosStart = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position();
                    
                    //X方向查找
                    for (int col = 0; col < count; col++) {
                        List<double> xPosList = new List<double>();
                        List<double> yPosList = new List<double>();
                        List<double> hPosList = new List<double>();

                        stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(xPosStart + col * step);
                        stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(yPosStart);

                        //Y方向查找
                        if (serchVertical(step, count, limit, out xPosList, out yPosList, out hPosList)) {
                            //判断所有满足要求的点
                            if (isHorizonTestOk(step, limit, xPosList, yPosList, out double xPos, out double yPos, out double height)) {                               
                                Invoke(new Action(() => {
                                    dgv_BasicSamplePoint.Rows.Add(xPos, yPos, height);
                                    MessageBox.Show($"X：{xPos} Y：{yPos} H:{height}为平坦区,扫描结束"); 
                                }));                               
                                return;
                            }
                        }
                    }

                    Invoke(new Action(() => { MessageBox.Show($"当前区域未找到平坦区,请更换位置后重新扫描"); }));
                }
                catch(Exception ex) {
                    Invoke(new Action(() => { MessageBox.Show($"平坦区扫描异常:{ex.Message}"); }));
                } 
            });
        }

        private void ReportMessage(string message) {
            this.BeginInvoke(new Action(() => {
                string info = Environment.NewLine + $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] : {message}";
                rtbMsgBox.AppendText(info);
            }));
        }

        private bool isHorizonTestOk(double step, double limit, List<double> xList, List<double> yList, out double xPos, out double yPos, out double heitht) {
            xPos = 0;
            yPos = 0;
            heitht = 0;
            double moveDis = 0;
            string info = string.Empty;

            for (int i = 0; i < xList.Count; i++) {
                ReportMessage(string.Format("开始{0}横向扫描", yList[i]));

                stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(xList[i]);
                stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(yList[i]);
                double xPos1 = stageAxisDic[MyStageAxisKey.CHUCK_X].Position();
                double yPos1 = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position();
                Altimeter.GetHeight(1, out double hPos1);
                info = string.Format("X:{0} Y:{1} Height:{2}", xPos1, yPos1, hPos1);
                ReportMessage(info);

                moveDis = -50;
                stageAxisDic[MyStageAxisKey.CHUCK_X].MoveRelative(moveDis);
                double xPos2 = stageAxisDic[MyStageAxisKey.CHUCK_X].Position();
                double yPos2 = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position();
                Altimeter.GetHeight(1, out double hPos2);
                info = string.Format("X:{0} Y:{1} Height:{2}", xPos2, yPos2, hPos2);
                ReportMessage(info);

                moveDis = 100;
                stageAxisDic[MyStageAxisKey.CHUCK_X].MoveRelative(moveDis);
                double xPos3 = stageAxisDic[MyStageAxisKey.CHUCK_X].Position();
                double yPos3 = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position();
                Altimeter.GetHeight(1, out double hPos3);
                info = string.Format("X:{0} Y:{1} Height:{2}", xPos3, yPos3, hPos3);
                ReportMessage(info);

                double ret1 = Math.Abs(hPos1 - hPos2);
                double ret2 = Math.Abs(hPos1 - hPos3);
                double ret3 = Math.Abs(hPos2 - hPos3);
                if ((ret1 < limit) && (ret2 < limit) && (ret3 < limit)) {
                    xPos = xPos1;
                    yPos = yPos1;
                    heitht = hPos1;

                    info = string.Format("X:{0} X:{1} X:{2} Y:{3} Height:{4} Height:{5} Height:{6} 纵向满足要求 {7}", xPos2, xPos1, xPos3, yPos1, hPos2, hPos1, hPos3, limit);
                    ReportMessage(info);

                    return true;
                }
            }

            return false;
        }

        //Y方向查找
        private bool serchVertical(double step, int count, double limit, out List<double> xList, out List<double> yList, out List<double> hList) {
            xList = new List<double>();
            yList = new List<double>();
            hList = new List<double>();
            double moveDis = 0;
            double height = 0;
            string info = string.Empty;

            List<double> xListTemp = new List<double>();
            List<double> yListTemp = new List<double>();
            List<double> hListTemp = new List<double>();

            for (int i = 0; i < count; i++) {
                moveDis = step;
                stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveRelative(moveDis);
                xListTemp.Add(stageAxisDic[MyStageAxisKey.CHUCK_X].Position());
                yListTemp.Add(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position());
                Altimeter.GetHeight(1, out height);
                hListTemp.Add(height);

                if (i == 0) {
                    ReportMessage(string.Format("纵向 {0} 扫描开始...", yListTemp[0]));
                }
                info = string.Format("X:{0} Y:{1} Height:{2}", xListTemp[i], yListTemp[i], hListTemp[i]);
                ReportMessage(info);
            }

            //连续3个点的高度差小于limit，则认为中间的一个点是满足要求的
            for (int i = 0; i < count - 2; i++) {
                double ret1 = Math.Abs(hListTemp[i] - hListTemp[i + 1]);
                double ret2 = Math.Abs(hListTemp[i] - hListTemp[i + 2]);
                double ret3 = Math.Abs(hListTemp[i + 1] - hListTemp[i + 2]);
                if ((ret1 < limit) && (ret2 < limit) && (ret3 < limit)) {
                    xList.Add(xListTemp[i + 1]);
                    yList.Add(yListTemp[i + 1]);
                    hList.Add(hListTemp[i + 1]);
                    info = string.Format("X:{0} Y:{1} Y:{2} Y:{3} Height:{4} Height:{5} Height:{6} 纵向满足要求 {7}", xListTemp[i], yListTemp[i], yListTemp[i + 1], yListTemp[i + 2], hListTemp[i], hListTemp[i + 1], hListTemp[i + 2], limit);
                    ReportMessage(info);
                }
            }

            ReportMessage("纵向扫描结束...");

            return hList.Count > 0;
        }

        private void rtbMsgBox_TextChanged(object sender, EventArgs e) {
            rtbMsgBox.SelectionStart = rtbMsgBox.Text.Length;
            rtbMsgBox.ScrollToCaret();
        }
    }
}
