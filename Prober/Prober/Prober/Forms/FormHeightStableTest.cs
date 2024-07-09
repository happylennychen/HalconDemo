using CommonApi.MyUtility;
using MyInstruments;
using MyInstruments.MyAltimeter;
using MyInstruments.MyEnum;
using MyInstruments.MyUtility;
using MyMotionStageDriver.MyStageAxis;
using NLog;
using Prober.Constant;
using ProberApi.MyConstant;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prober.Forms
{
    public partial class FormHeightStableTest : Form
    {
        StandaloneAm altimeter;
        StandaloneAm altimeterLD;
        StandaloneAm altimeterCap;
        bool isStopRecord = false;

        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        private readonly Dictionary<string, Instrument> instruments;
        private readonly List<InstrumentUsage> instrumentUsageList;
        private readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        private readonly Dictionary<string, StageAxis> stageAxisDic = new Dictionary<string, StageAxis>();

        public FormHeightStableTest(ConcurrentDictionary<string, object> sharedObjects)
        {
            InitializeComponent();

            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            stageAxisUsages = tempObj as Dictionary<string, StageAxis>;

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            //获取仪表
            var getResult2 = GetInstrument("ld_altimeter");
            getResult2 = GetInstrument("cap_altimeter");

            //获取轴
            GetStageAxisDic();
        }

        public bool GetStageAxisDic() {
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

            for (int i = 0; i < axisUseList.Count; i++) {
                var result = GetStageAxis(axisUseList[i]);
                if (!result.isOK) {
                    return false;
                }
                stageAxisDic.Add(axisUseList[i], result.stageAxis);
            }

            return true;
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
                        altimeterLD = instrument as StandaloneAm;
                        altimeter = altimeterLD;
                    }
                    else if (instrumentUsage.InstrumentId == "altimeter_cap")
                    {
                        altimeterCap = instrument as StandaloneAm;
                    }
                    else
                    {
                        errorText = $"{instrumentUsage.InstrumentId.ToString()} is not a valid instrument id of temperature stable monitor!";
                        LOGGER.Error(errorText);
                        return (false, errorText, null);
                    }
                    break;
                default:
                    errorText = $"{instrumentUsage.InstrumentCategory.ToString()} is not a valid instrument category of temperature stable monitor!";
                    LOGGER.Error(errorText);
                    return (false, errorText, null);
            }

            return (true, string.Empty, instrument);
        }       

        private void btn_Sample_1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string index = btn.Name.Split('_')[2];

            var lblX2 = panel_Height.Controls["lbl_X2_" + index] as Label;
            var lblY2 = panel_Height.Controls["lbl_Y2_" + index] as Label;
            var lblH = panel_Height.Controls["lbl_H_" + index] as Label;


            lblX2.Text = stageAxisDic[MyStageAxisKey.CHUCK_X].Position().ToString("f2");
            lblY2.Text = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position().ToString("f2");

            if (!GetHeight(Cap_Altimeter_Channel.Left_Channel, out double height))
            {
                MessageBox.Show("测高仪数据读取失败");
                return;
            }
            lblH.Text = height.ToString("f2");
            MessageBox.Show(this, "采样成功。", "Info:");
        }

        private bool GetHeight(int channel, out double Value)
        {
            Value = double.NaN;

            for (int i = 0; i < 5; i++)
            {
                if (altimeter.GetHeight(channel, out Value))
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

        public void MoveChuckXYAbs(double xPos, double yPos) {
            var t1 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(xPos);
            });

            var t2 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(yPos);
            });

            Task.WaitAll(new Task[] { t1, t2 });

            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
        }


        private void btn_Record_Start_Click(object sender, EventArgs e)
        {
            double[] x2 = new double[6];
            double[] y2 = new double[6];
            double[] hs = new double[6];

            for (int i = 0; i < x2.Length; i++)
            {
                x2[i] = Convert.ToDouble(panel_Height.Controls["lbl_X2_" + (i + 1)].Text);
                y2[i] = Convert.ToDouble(panel_Height.Controls["lbl_Y2_" + (i + 1)].Text);
            }

            lbl_Info.Text = "采集开始";

            Task.Run(() =>
            {
                string path = "HeightRecord";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }                    

                try
                {
                    string file = path + $"\\HeightInfo_{DateTime.Now.ToString("yyyyMMdd")}.csv";
                    if (!File.Exists(file))
                    {
                        File.AppendAllText(file, "Date,X,Y,H" + Environment.NewLine);
                    }

                    for (int i = 0; i < x2.Length; i++)
                    {
                        if (isStopRecord)
                        {
                            break;
                        }
                        MoveChuckXYAbs(x2[i], y2[i]);   

                        Thread.Sleep(500);
                        //hs[i] = altimeter.GetHeight(Cap_Altimeter_Channel.Left_Channel);

                        if (!GetHeight(Cap_Altimeter_Channel.Left_Channel, out hs[i]))
                        {
                            Invoke(new Action(() => { MessageBox.Show("高度读取失败"); }));
                            return;
                        }
                        Invoke(new Action(() => { panel_Height.Controls["lbl_H_" + (i + 1)].Text = hs[i].ToString("f2"); }));
                    }

                    for (int i = 0; i < x2.Length; i++) {
                        File.AppendAllText(file, $"{DateTime.Now.ToString()},{x2[i]},{y2[i]},{hs[i]}" + Environment.NewLine);
                    }                       

                    Invoke(new Action(() => { MessageBox.Show("高度扫描完毕"); }));
                }
                catch(Exception ex)
                {
                    Invoke(new Action(() => { MessageBox.Show($"高度监控异常:{ex.Message}"); }));
                }
                finally
                {
                    Invoke(new Action(() => { lbl_Info.Text = "采集结束"; }));
                }
            });
        }

        private void btn_Record_Stop_Click(object sender, EventArgs e)
        {
            if (isStopRecord != true)
            {
                if (MessageBox.Show("确定要停止监控吗？", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }

            lbl_Info.Text = "采集停止";
            isStopRecord = true;
        }

        private void btn_Record_View_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Application.StartupPath + "HeightRecord"))
            {
                Directory.CreateDirectory(Application.StartupPath + "HeightRecord");
            }

            Process.Start(Application.StartupPath + "\\HeightRecord");
        }

        private void chb_AmType_Cap_CheckedChanged(object sender, EventArgs e) {
            if(chb_AmType_Cap.Checked == true) {
                altimeter = altimeterCap;
            }
            else {
                altimeter = altimeterLD;
            }
        }
    }
}
