using MyInstruments.MyPma;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;
using CommonApi.MyUtility;
using MyInstruments.MyEnum;
using MyInstruments.MyUtility;
using MyInstruments;
using NLog;
using System.Collections.Generic;
using ProberApi.MyConstant;
using MyInstruments.MyAltimeter;
using MyInstruments.MyTec;
using System.Linq;
using MyInstruments.MyOpm;
using MyInstruments.MyCamera;
using MyInstruments.MyElecLens;

namespace Prober.Forms
{
    public partial class FormPowerStableTest : Form
    {
        IOpm pm;
        bool isStopRecord = false;
        private readonly List<InstrumentUsage> instrumentUsageList;
        private readonly Dictionary<string, Instrument> instruments;        
        private readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);

        public FormPowerStableTest(ConcurrentDictionary<string, object> sharedObjects)
        {
            InitializeComponent();

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out object tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            //获取仪表
            var getResult2 = GetInstrument("pma1_2_1_opm");
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
                case EnumInstrumentCategory.OPM:
                    pm = instrument as IOpm;
                    break;
                default:
                    errorText = $"{instrumentUsage.InstrumentCategory.ToString()} is not a valid instrument category of temperature stable monitor!";
                    LOGGER.Error(errorText);
                    return (false, errorText, null);
            }

            return (true, string.Empty, instrument);
        }        

        private void btn_Record_Start_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要开始吗？", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            btn_Record_Start.Enabled = false;

            double minutes = Convert.ToDouble(num_Record_Time.Value);
            int interval = Convert.ToInt32(num_Record_Interval.Value);
            int total = (int)(minutes * 60 * 1000 / interval) + 1;
            isStopRecord = false;
            lbl_Info.Text = "数据采集开始";

            string channel = num_Pm_Channel.Value.ToString();
            string slot = num_Pm_Slot.Value.ToString();

            foreach (var item in chart_Power.Series) { 
                item.Points.Clear();
            }

            Task.Run(() =>
            {
                DateTime timeStart = DateTime.Now;
                Invoke(new Action(() => { txt_TestStartTime.Text = timeStart.ToString("yyyy-MM-dd HH:mm:ss"); }));

                string path = "PowerRecord";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                try
                {
                    string file = path + $"\\RecordData_{DateTime.Now.ToString("yyyyMMdd HHmmss")}.csv";
                    File.AppendAllText(file, "Date,Power(dBm)" + Environment.NewLine);

                    double pw = 0;
                    for (int i = 0; i < total; i++)
                    {
                        TimeSpan timeSpan = DateTime.Now - timeStart;
                        if ((timeSpan.TotalSeconds > minutes * 60) || isStopRecord)                       
                            break;

                        //pw = pm.OpmReadPower(slot,channel);
                        pw = pm.OpmFetchPower(slot, channel);
                        File.AppendAllText(file, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{pw.ToString()}" + Environment.NewLine);
                        Invoke(new Action(() => {
                            lbl_Record_Value.Text = pw.ToString();
                            txt_TestEllapseTime.Text = timeSpan.TotalSeconds.ToString("0.00");
                            chart_Power.Series[0].Points.AddXY(DateTime.Now, pw);
                        }));

                        Thread.Sleep(interval);
                        Invoke(new Action(() => { lbl_Info.Text = $"数据采集{i}"; }));
                    }

                    if (isStopRecord)
                    {
                        Invoke(new Action(() => { MessageBox.Show($"已经停止功率记录"); }));
                    }
                    else
                    {
                        Invoke(new Action(() => { MessageBox.Show($"记录完成"); }));
                    }
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() => { MessageBox.Show($"功率监控异常:{ex.Message}"); }));
                }
                finally
                {
                    Invoke(new Action(() => {
                        lbl_Info.Text = "数据采集停止"; 
                        btn_Record_Start.Enabled = true; 
                    }));                    
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

            lbl_Info.Text = "数据采集停止";
            isStopRecord = true;
            btn_Record_Start.Enabled = true;
        }

        private void btn_Record_View_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Application.StartupPath + "PowerRecord"))
            {
                Directory.CreateDirectory(Application.StartupPath + "PowerRecord");
            }

            Process.Start(Application.StartupPath + "\\PowerRecord");
        }
    }
}
