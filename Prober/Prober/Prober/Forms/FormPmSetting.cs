using CommonApi.MyUtility;
using HalconDotNet;
using MyInstruments.MyOpm;
using MyInstruments.MyUtility;
using MyInstruments;
using NLog;
using Prober.WaferDef;
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
using ProberApi.MyConstant;
using MyInstruments.MyEnum;

namespace Prober.Forms
{
    public partial class FormPmSetting : Form
    {
        ConcurrentDictionary<string, object> sharedObjects;
        private readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        private readonly Dictionary<string, Instrument> instruments;
        private readonly List<InstrumentUsage> instrumentUsageList;
        private IOpm opm;
        private string Power1_Slot = string.Empty;
        private string Power2_Slot = string.Empty;
        private string Power1_Channel = string.Empty;
        private string Power2_Channel = string.Empty;

        public FormPmSetting(ConcurrentDictionary<string, object> sharedObjects)
        {
            InitializeComponent();
            PmTriggerSetting setting =  ConfigMgr.LoadPmTriggerInfo();
            if (null != setting)
            {
                InitPmTriggerSetting(setting);
            }

            this.sharedObjects = sharedObjects;

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out var tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            //获取仪表
            var getResult2 = GetInstrument("pma1_2_1_opm");
            getResult2 = GetInstrument("pma1_2_2_opm");

            InitPmSlotChannelInfo();
        }

        public void InitPmTriggerSetting(PmTriggerSetting setting) {
            txt_ScanRange1.Text = setting.ScanRange1.ToString();
            txt_ScanStep1.Text = setting.ScanStep1.ToString();
            txt_PmNplc1.Text = setting.PmNplc1.ToString();
            txt_PmRange1.Text = setting.PmRange1.ToString();

            txt_ScanRange2.Text = setting.ScanRange2.ToString();
            txt_ScanStep2.Text = setting.ScanStep2.ToString();
            txt_PmNplc2.Text = setting.PmNplc2.ToString();
            txt_PmRange2.Text = setting.PmRange2.ToString();
        }

        public void InitPmSlotChannelInfo() {
            txt_PmSlot1.Text = Power1_Slot.ToString();
            txt_PmChannel1.Text = Power1_Channel.ToString();
            txt_PmSlot2.Text = Power2_Slot.ToString();
            txt_PmChannel2.Text = Power2_Channel.ToString();
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
                    if (instrumentUsage.UsageId == "pma1_2_1_opm")
                    {
                        opm = instrumentUsage.TheInstrument as IOpm;
                        Power1_Slot = instrumentUsage.Slot;
                        Power1_Channel = instrumentUsage.Channel;
                    }
                    else if (instrumentUsage.UsageId == "pma1_2_2_opm")
                    {
                        opm = instrumentUsage.TheInstrument as IOpm;
                        Power2_Slot = instrumentUsage.Slot;
                        Power2_Channel = instrumentUsage.Channel;
                    }
                    else
                    {
                        errorText = $"{instrumentUsage.InstrumentId.ToString()} is not a valid instrument id of FA Rolling!";
                        LOGGER.Error(errorText);
                        return (false, errorText, null);
                    }
                    break;
                default:
                    errorText = $"{instrumentUsage.InstrumentCategory.ToString()} is not a valid instrument category of FA Rolling!";
                    LOGGER.Error(errorText);
                    return (false, errorText, null);
            }

            return (true, string.Empty, instrument);
        }

        private void btn_PmTriggerSetting1_Click(object sender, EventArgs e)
        {
            try
            {
                opm.OpmSetPowerRangeType(Power1_Slot, Power1_Channel, EnumOpmPowerRangeType.MANUAL);
                opm.OpmSetPowerRange(Power1_Slot, Power1_Channel, Convert.ToInt32(txt_PmRange1.Text));
                //opm.OpmSetSamplePoint(Power1_Slot, Power1_Channel, Convert.ToInt32(txt_ScanRange1.Text), Convert.ToDouble(txt_PmNplc1.Text));
                int scanPoint = (int)(Convert.ToDouble(txt_ScanRange1.Text) / Convert.ToDouble(txt_ScanStep1.Text) + 1.1);
                opm.OpmSetSamplePoint(Power1_Slot, Power1_Channel, scanPoint, Convert.ToDouble(txt_PmNplc1.Text));
                MessageBox.Show("参数设置成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"参数设置异常:{ex.Message}，稍后再试");
                return;
            }
        }

        private void btn_PmAutoSetting1_Click(object sender, EventArgs e)
        {
            try
            {
                opm.OpmSetNoTriggerMode(Power1_Slot, Power1_Channel);
                opm.OpmSetPowerRangeType(Power1_Slot, Power1_Channel, EnumOpmPowerRangeType.AUTO);
                MessageBox.Show("参数设置成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"参数设置异常:{ex.Message}，稍后再试");
                return;
            }
        }

        private void btn_PmTriggerSetting2_Click(object sender, EventArgs e)
        {
            try
            {
                opm.OpmSetPowerRangeType(Power2_Slot, Power2_Channel, EnumOpmPowerRangeType.MANUAL);
                opm.OpmSetPowerRange(Power2_Slot, Power2_Channel, Convert.ToInt32(txt_PmRange2.Text));
                //opm.OpmSetSamplePoint(Power2_Slot, Power2_Channel, Convert.ToInt32(txt_ScanRange2.Text), Convert.ToDouble(txt_PmNplc2.Text));

                int scanPoint = (int)(Convert.ToDouble(txt_ScanRange2.Text) / Convert.ToDouble(txt_ScanStep2.Text) + 1.1);
                opm.OpmSetSamplePoint(Power2_Slot, Power2_Channel, scanPoint, Convert.ToDouble(txt_PmNplc2.Text));
                MessageBox.Show("参数设置成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"参数设置异常:{ex.Message}，稍后再试");
                return;
            }
        }

        private void btn_PmAutoSetting2_Click(object sender, EventArgs e)
        {
            try
            {
                opm.OpmSetNoTriggerMode(Power2_Slot, Power2_Channel);
                opm.OpmSetPowerRangeType(Power2_Slot, Power2_Channel, EnumOpmPowerRangeType.AUTO);
                MessageBox.Show("参数设置成功");
            }
            catch(Exception ex)
            {
                MessageBox.Show($"参数设置异常:{ex.Message}，稍后再试");
                return;
            }
        }

        private void btn_SaveSetting_Click(object sender, EventArgs e)
        {
            PmTriggerSetting info = ConfigMgr.LoadPmTriggerInfo();
            info.ScanRange1 = Convert.ToDouble(txt_ScanRange1.Text);
            info.ScanRange2 = Convert.ToInt32(txt_ScanRange2.Text);
            info.ScanStep1 = Convert.ToDouble(txt_ScanStep1.Text);
            info.ScanStep2 = Convert.ToDouble(txt_ScanStep2.Text);
            info.PmRange1 = Convert.ToInt32(txt_PmRange1.Text);
            info.PmRange2 = Convert.ToInt32(txt_PmRange2.Text);
            info.PmNplc1 = Convert.ToDouble(txt_PmNplc1.Text);
            info.PmNplc2 = Convert.ToDouble(txt_PmNplc2.Text);

            ConfigMgr.SavePmTriggerInfo(info);
        }
    }
}
