using MyInstruments.MyAltimeter;
using MyInstruments.MyTec;
using ProberApi.MyConstant;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;
using MyInstruments.MyEnum;
using MyInstruments.MyUtility;
using MyInstruments;
using System.Collections.Generic;
using CommonApi.MyUtility;
using NLog;
using MyInstruments.MyCamera;
using MyInstruments.MyElecLens;
using System.Linq;
using MyInstruments.MyOpm;
using Prober.WaferDef;

namespace Prober.Forms
{
    public partial class FormTempStableTest : Form
    {
        private readonly List<InstrumentUsage> instrumentUsageList;
        private readonly Dictionary<string, Instrument> instruments;        
        private readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        StandaloneTec tec;
        StandaloneAm altimeter;
        StandaloneTec tecChuck;
        StandaloneAm altimeterLD;
        StandaloneTec tecGrat;
        StandaloneAm altimeterCap;
        bool isStopRecord = false;
        private IOpm opm;
        private string Power1_Slot = string.Empty;
        private string Power2_Slot = string.Empty;
        private string Power1_Channel = string.Empty;
        private string Power2_Channel = string.Empty;
        private double tecCoeffK = 1.24495;
        private double tecCoeffB = 7.0927;

        public FormTempStableTest(ConcurrentDictionary<string, object> sharedObjects)
        {
            InitializeComponent();

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out object tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>; 

            //获取仪表
            var getResult2 = GetInstrument("ld_altimeter");
            getResult2 = GetInstrument("cap_altimeter");
            getResult2 = GetInstrument("chuck_temperature");
            getResult2 = GetInstrument("grating_temperature");
            getResult2 = GetInstrument("pma1_2_2_opm");
            getResult2 = GetInstrument("pma1_2_1_opm");
            GetCoeff();
        }

        public void GetCoeff() {
            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
            if (info != null) {
                tecCoeffK = info.TecCoeffK;
                tecCoeffB = info.TecCoeffB;
            }
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
                case EnumInstrumentCategory.TEC:
                    if (instrumentUsage.InstrumentId == "tec_chuck")
                    {
                        tecChuck = instrument as StandaloneTec;
                        tec = tecChuck;
                    }
                    else if (instrumentUsage.InstrumentId == "tec_grat")
                    {
                        tecGrat = instrument as StandaloneTec;
                    }
                    else
                    {
                        errorText = $"{instrumentUsage.InstrumentId.ToString()} is not a valid instrument id of temperature stable monitor!";
                        LOGGER.Error(errorText);
                        return (false, errorText, null);
                    }
                    break;
                case EnumInstrumentCategory.OPM:
                    if (instrumentUsage.UsageId == "pma1_2_1_opm") {
                        opm = instrumentUsage.TheInstrument as IOpm;
                        Power1_Slot = instrumentUsage.Slot;
                        Power1_Channel = instrumentUsage.Channel;
                    } else if (instrumentUsage.UsageId == "pma1_2_2_opm") {
                        opm = instrumentUsage.TheInstrument as IOpm;
                        Power2_Slot = instrumentUsage.Slot;
                        Power2_Channel = instrumentUsage.Channel;
                    } else {
                        errorText = $"{instrumentUsage.InstrumentId.ToString()} is not a valid instrument id of FA Rolling!";
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
#if false
        private void btn_Record_Start_Click(object sender, EventArgs e)
        {
            bool isMonitorH = chbox_IsMonitorH.Checked;

            if (MessageBox.Show("确定要开始吗？", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            double minutes = Convert.ToDouble(num_Record_Time.Value);
            int interval = Convert.ToInt32(num_Record_Interval.Value);
            int total = (int)(minutes * 60 * 1000 / interval) + 1;
            isStopRecord = false;
            lbl_Info.Text = "数据采集开始";

            Task.Run(() =>
            {
                string path = "TemperatureRecord";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                try
                {
                    string file = path + $"\\RecordData_{DateTime.Now.ToString("yyyyMMdd HHmmss")}.csv";
                    if (!isMonitorH)
                    {
                        File.AppendAllText(file, "Date,Temperature(℃)" + Environment.NewLine);
                    }
                    else
                    {
                        File.AppendAllText(file, "Date,Temperature(℃),Height(um)" + Environment.NewLine);
                    }

                    double temp = 0;
                    double h = 0;
                    for (int i = 0; i < total; i++)
                    {
                        if (isStopRecord)     
                            break;                        

                        if (!GetTemperature(1,out temp))
                        {
                            Invoke(new Action(() => { MessageBox.Show("温度读取失败"); }));
                            return ;
                        }

                        if (isMonitorH)
                        {
                            if (!GetHeight(1, out h))
                            {
                                Invoke(new Action(() => { MessageBox.Show("高度读取失败"); }));
                                return ;
                            }
                            File.AppendAllText(file, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{temp.ToString("f2")},{h.ToString("f2")}" + Environment.NewLine);
                        }
                        else
                        {
                            File.AppendAllText(file, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{temp.ToString("f2")}" + Environment.NewLine);
                        }

                        Invoke(new Action(() => {lbl_Record_Temp.Text = temp.ToString("f2"); lbl_Record_Height.Text = h.ToString("f2");}));
                        Thread.Sleep(interval);
                    }

                    if (isStopRecord)
                    {
                        Invoke(new Action(() => { MessageBox.Show($"已经停止温度记录"); }));
                    }
                    else
                    {
                        Invoke(new Action(() => { MessageBox.Show($"记录完成");}));
                    }                        
                }
                catch(Exception ex)
                {
                    Invoke(new Action(() => { MessageBox.Show($"温度监控异常:{ex.Message}");}));
                }   
				finally
				{
					Invoke(new Action(() => { lbl_Info.Text = "数据采集结束"; }));
				}            
            });
        }
#endif

        private void ClearCharPoint()
        {
            foreach (var item in chart_Temp.Series)
            {
                item.Points.Clear();
            }

            foreach (var item in chart_Power.Series)
            {
                item.Points.Clear();
            }

            foreach (var item in chart_Height.Series)
            {
                item.Points.Clear();
            }
        }

        private void btn_Record_Start_Click(object sender, EventArgs e) {
            bool isMonitorH = chbox_IsMonitorH.Checked;

            btn_Record_Start.Enabled = false;

            if (MessageBox.Show("确定要开始吗？", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            double minutes = Convert.ToDouble(num_Record_Time.Value);
            int interval = Convert.ToInt32(num_Record_Interval.Value);
            int total = (int)(minutes * 60 * 1000 / interval) + 1;
            isStopRecord = false;
            lbl_Info.Text = "数据采集开始";
			DateTime timeStart = DateTime.Now;

            ClearCharPoint();

            Task.Run(() => {
                string path = "TemperatureRecord";
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }

                try {
                    string file = path + $"\\RecordData_{DateTime.Now.ToString("yyyyMMdd HHmmss")}.csv";
                    File.AppendAllText(file, "Date,LdHeight,CapHeight1,CapHeight2,ChuckTemp,GratTemp1,GratTemp2,GratTemp3,GratTemp4,PowerMonitor_2,PowerMonitor_1" + Environment.NewLine);

                    Invoke(new Action(() => { txt_TestStartTime.Text = timeStart.ToString("yyyy-MM-dd HH:mm:ss"); }));

                    double temp = 0;
                    double h = 0;
                    double h2 = 0;
                    double PowerMonitor = 0;
                    double PowerMonitor2 = 0;
                    for (int i = 0; i < total; i++) {
                        TimeSpan timeSpan = DateTime.Now - timeStart;
                        if (isStopRecord || (timeSpan.TotalSeconds > minutes * 60))
                            break;

                        if (!GetLDHeight(1, out temp)) {
                            Invoke(new Action(() => { MessageBox.Show("LD高度读取失败"); }));
                            return;
                        }                        
                           
                        if (!GetCapHeight(2, out h)) {
                            Invoke(new Action(() => { MessageBox.Show("电容1高度读取失败"); }));
                            return;
                        }

                        if (!GetCapHeight(1, out h2)) {
                            Invoke(new Action(() => { MessageBox.Show("电容2高度读取失败"); }));
                            return;
                        }

                        if (!GetOmronTemperature(1, out double tempOmron)) {
                            Invoke(new Action(() => { MessageBox.Show("omron读取失败"); }));
                            return;
                        }
                        tempOmron = Math.Round(tempOmron * tecCoeffK - tecCoeffB,1);
#if false
                        if (!GetGratTemperature(1, out double tempGrat1)) {
                            Invoke(new Action(() => { MessageBox.Show("Gratting4读取失败"); }));
                            return;
                        }

                        if (!GetGratTemperature(2, out double tempGrat2)) {
                            Invoke(new Action(() => { MessageBox.Show("Gratting4读取失败"); }));
                            return;
                        }

                        if (!GetGratTemperature(3, out double tempGrat3)) {
                            Invoke(new Action(() => { MessageBox.Show("Gratting4读取失败"); }));
                            return;
                        }

                        if (!GetGratTemperature(4, out double tempGrat4)) {
                            Invoke(new Action(() => { MessageBox.Show("Gratting4读取失败"); }));
                            return;
                        }
#endif

                        if (!GetGratTemperature(out double tempGrat1, out double tempGrat2, out double tempGrat3,out double tempGrat4)) {
                            Invoke(new Action(() => { MessageBox.Show("Gratting读取失败"); }));
                            return;
                        }
                        
                        PowerMonitor = opm.OpmFetchPower(Power2_Slot, Power2_Channel);
                        PowerMonitor2 = opm.OpmFetchPower(Power1_Slot, Power1_Channel);

                        File.AppendAllText(file, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{temp.ToString("f3")},{h.ToString("f3")},{h2.ToString("f3")},{tempOmron.ToString("f3")},{tempGrat1.ToString("f3")},{tempGrat2.ToString("f3")},{tempGrat3.ToString("f3")},{tempGrat4.ToString("f3")},{PowerMonitor.ToString("f3")},{PowerMonitor2.ToString("f3")}" + Environment.NewLine);                       
                        Invoke(new Action(() => { 
                            lbl_Chuck_Temp.Text = tempOmron.ToString("f3");
                            lbl_AxisX_Temp.Text = tempGrat1.ToString("f3");
                            lbl_AxisY_Temp.Text = tempGrat2.ToString("f3");
                            lbl_AxisZ_Temp.Text = tempGrat3.ToString("f3");
                            lbl_Enviroment_Temp.Text = tempGrat4.ToString("f3");
                            lbl_Am_Ld.Text = temp.ToString("f3");
                            lbl_Am_CapL.Text = h.ToString("f3");
                            lbl_Am_CapR.Text = h2.ToString("f3");
                            lbl_Power_Ch1.Text = PowerMonitor2.ToString("f3");
                            lbl_Power_Ch2.Text = PowerMonitor.ToString("f3");

                            chart_Temp.Series[0].Points.AddXY(DateTime.Now, tempOmron);
                            chart_Temp.Series[1].Points.AddXY(DateTime.Now, tempGrat1);
                            chart_Temp.Series[2].Points.AddXY(DateTime.Now, tempGrat2);
                            chart_Temp.Series[3].Points.AddXY(DateTime.Now, tempGrat3);
                            chart_Temp.Series[4].Points.AddXY(DateTime.Now, tempGrat4);

                            //chart_Height.Series[0].Points.AddXY(DateTime.Now, temp);
                            chart_Height.Series[0].Points.AddXY(DateTime.Now, h);
                            chart_Height.Series[1].Points.AddXY(DateTime.Now, h2);

                            chart_Power.Series[0].Points.AddXY(DateTime.Now, PowerMonitor2);
                            chart_Power.Series[1].Points.AddXY(DateTime.Now, PowerMonitor);

                            txt_TestEllapseTime.Text = timeSpan.TotalSeconds.ToString("0.00");
                        }));

                        Thread.Sleep(interval);

                        Invoke(new Action(() => { lbl_Info.Text = $"数据采集{i}"; }));
                    }

                    if (isStopRecord) {
                        Invoke(new Action(() => { MessageBox.Show($"已经停止温度记录"); }));
                    } else {
                        Invoke(new Action(() => { MessageBox.Show($"记录完成"); }));
                    }
                } catch (Exception ex) {
                    Invoke(new Action(() => { MessageBox.Show($"温度监控异常:{ex.Message}"); }));
                } finally {
                    Invoke(new Action(() => { lbl_Info.Text = "数据采集结束"; 
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
            if (Directory.Exists(Application.StartupPath + "TemperatureRecord"))
            {
                Directory.CreateDirectory(Application.StartupPath + "TemperatureRecord");
            }

            Process.Start(Application.StartupPath + "\\TemperatureRecord");
        }

        private void chb_AmType_Cap_CheckedChanged(object sender, EventArgs e) {
            if(chb_AmType_Cap.Checked == true) {
                altimeter = altimeterCap;
            }
            else {
                altimeter = altimeterLD;
            }
        }

        private bool GetTemperature(int channel, out double Value)
        {
            Value = double.NaN;

            for (int i = 0; i < 5; i++ )
            {
                if (tec.GetTemp(channel, out Value))
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

        private bool GetGratTemperature(out double ValueCh1, out double ValueCh2, out double ValueCh3, out double ValueCh4) {
            ValueCh1 = double.NaN;
            ValueCh2 = double.NaN;
            ValueCh3 = double.NaN;
            ValueCh4 = double.NaN;

            List<double> tempList = new List<double>();
            for (int i = 0; i < 5; i++) {
                if (tecGrat.GetTempAll(out tempList)) {
                    ValueCh1 = tempList[0];
                    ValueCh2 = tempList[1];
                    ValueCh3 = tempList[2];
                    ValueCh4 = tempList[3];

                    return true;
                } else {
                    Thread.Sleep(500);
                }
            }

            return false;
        }

        private bool GetGratTemperature(int channel, out double Value) {
            Value = double.NaN;

            for (int i = 0; i < 5; i++) {
                if (tecGrat.GetTemp(channel, out Value)) {
                    return true;
                } else {
                    Thread.Sleep(500);
                }
            }

            return false;
        }

        private bool GetOmronTemperature(int channel, out double Value) {
            Value = double.NaN;

            for (int i = 0; i < 5; i++) {
                if (tecChuck.GetTemp(channel, out Value)) {                   
                    return true;
                } else {
                    Thread.Sleep(500);
                }
            }

            return false;
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

        private bool GetLDHeight(int channel, out double Value) {
            Value = double.NaN;

            for (int i = 0; i < 5; i++) {
                if (altimeterLD.GetHeight(channel, out Value)) {
                    return true;
                } else {
                    Thread.Sleep(500);
                }
            }

            return false;
        }

        private bool GetCapHeight(int channel, out double Value) {
            Value = double.NaN;

            for (int i = 0; i < 5; i++) {
                if (altimeterCap.GetHeight(channel, out Value)) {
                    return true;
                } else {
                    Thread.Sleep(500);
                }
            }

            return false;
        }


        private void chb_TecType_Grat_CheckedChanged(object sender, EventArgs e) {
            if (chb_TecType_Grat.Checked == true) {
                tec = tecGrat;
            }
            else {
                tec = tecChuck;
            }
        }
    }
}
