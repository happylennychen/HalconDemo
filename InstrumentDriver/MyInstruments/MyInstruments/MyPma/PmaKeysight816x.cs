using CommonApi.MyTrigger;
using CommonApi.MyUtility;

using MyInstruments.MyEnum;
using MyInstruments.MyOpm;
using MyInstruments.MyTls;
using MyInstruments.MyUtility;
using MyInstruments.MyVisaDriver;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;

namespace MyInstruments.MyPma {
    public sealed class PmaKeysight816x : Pma, ITriggerIn {
        public PmaKeysight816x(string id) : base(id) {
            Vendor = EnumInstrumentVendor.KEYSIGHT.ToString();
            SupportedModels = new HashSet<string> { "_816xB" };

            List<string> commonTlsSettingList = new List<string>(Enum.GetNames(typeof(EnumKeysightTlsSetting)));
            validTlsSettingKeySet.UnionWith(commonTlsSettingList.ToHashSet());

            List<string> commonOpmSettingList = new List<string>(Enum.GetNames(typeof(EnumKeysightOpmSetting)));
            validOpmSettingKeySet.UnionWith(commonOpmSettingList.ToHashSet());
        }

        public override void Connect(string visaResource) {
            LOGGER.Debug($"call PmaKeysight816x.Connect({visaResource})");
            this.visaDriver = VisaDriverFactory.CreateInstance();
            this.visaDriver.Connect(visaResource);
        }

        public override void Disconnect() {
            LOGGER.Debug($"call PmaKeysight816x.Disconnect()");
            this.visaDriver?.Disconnect();
        }

        public override double OpmFetchPower(string slot, string channel) {
            string command = $"FETC{slot}:CHAN{channel}:POW?";
            if (string.IsNullOrEmpty(channel)) {
                command = $"FETC{slot}:POW?";
            }

            string response = string.Empty;
            try {
                response = visaDriver.QueryLine(command);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                response = OpmConstant.INVALID_OPTIC_POWER_VALUE.ToString();
            }

            if (!double.TryParse(response, out double result)) {
                result = OpmConstant.INVALID_OPTIC_POWER_VALUE;
            }

            if ((result > OpmConstant.UPPER_OPTIC_POWER_VALUE) || (result < OpmConstant.LOWER_OPTIC_POWER_VALUE)) {
                result = OpmConstant.INVALID_OPTIC_POWER_VALUE;
            }

            return result;
        }

        public override double OpmReadPower(string slot, string channel) {
            int try_times = 0;
            double result = OpmConstant.INVALID_OPTIC_POWER_VALUE;
            string command = $"READ{slot}:CHAN{channel}:POW?";
            if (string.IsNullOrEmpty(channel)) {
                command = $"READ{slot}:POW?";
            }
            while (true) {
                try {
                    string response = visaDriver.QueryLine(command);
                    result = Convert.ToDouble(response);
                    if ((result > OpmConstant.UPPER_OPTIC_POWER_VALUE) || (result < OpmConstant.LOWER_OPTIC_POWER_VALUE)) {
                        result = OpmConstant.INVALID_OPTIC_POWER_VALUE;
                    }
                    break;
                } catch (Exception ex) {
                    ++try_times;
                    if (try_times > InstrumentConstant.MAX_RETRY_TIMES) {
                        throw ex;
                    }
                    Thread.Sleep(300);
                }
            }
            return result;
        }

        public override void OpmSetAverageTime(string slot, string channel, double averageTime, EnumOpmTimeUnitType timeUnitType) {
            string command = $"SENS{slot}:CHAN{channel}:POW:ATIM {averageTime}{timeUnitType.ToString()}";
            if (string.IsNullOrEmpty(channel)) {
                command = $"SENS{slot}:POW:ATIM {averageTime}{timeUnitType.ToString()}";
            }
            visaDriver.WriteLine(command);
        }

        public override void OpmSetWavelength(string slot, string channel, double waveLengthInNm) {
            string command = $"SENS{slot}:CHAN{channel}:POW:WAV {waveLengthInNm}NM";
            if (string.IsNullOrEmpty(channel)) {
                command = $"SENS{slot}:POW:WAV {waveLengthInNm}NM";
            }
            visaDriver.WriteLine(command);
        }

        public override void OpmSetPowerUnitType(string slot, string channel, EnumOpticPowerUnitType powerUnitType) {
            string flag = string.Empty;
            switch (powerUnitType) {
                case EnumOpticPowerUnitType.DBM:
                    flag = "0";
                    break;
                case EnumOpticPowerUnitType.W:
                    flag = "1";
                    break;
            }

            string command = $"SENS{slot}:CHAN{channel}:POW:UNIT {flag}";
            if (string.IsNullOrEmpty(channel)) {
                command = $"SENS{slot}:POW:UNIT {flag}";
            }
            visaDriver.WriteLine(command);
        }

        public override void OpmSetPowerRange(string slot, string channel, int rangeInDbm) {
            int actualRangeIndBm = 0;
            if (rangeInDbm % 10 == 0) {
                actualRangeIndBm = rangeInDbm;
            } else {
                int quotient = rangeInDbm / 10;
                actualRangeIndBm = (quotient + 1) * 10;
            }

            string command = $"SENS{slot}:CHAN{channel}:POW:RANG {actualRangeIndBm}DBM";
            if (string.IsNullOrEmpty(channel)) {
                command = $"SENS{slot}:POW:RANG {actualRangeIndBm}DBM";
            }
            visaDriver.WriteLine(command);
        }

        public override void OpmSetPowerRangeType(string slot, string channel, EnumOpmPowerRangeType powerRangeType) {
            string flag = string.Empty;
            switch (powerRangeType) {
                case EnumOpmPowerRangeType.MANUAL:
                    flag = "0";
                    break;
                case EnumOpmPowerRangeType.AUTO:
                    flag = "1";
                    break;
            }
            string command = $"SENS{slot}:CHAN{channel}:POW:RANG:AUTO {flag}";
            if (string.IsNullOrEmpty(channel)) {
                command = $"SENS{slot}:POW:RANG:AUTO {flag}";
            }
            visaDriver.WriteLine(command);
        }

        public override void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel) {
            //[gyh]: to be implemented!            
        }

        public override (bool isOk, string errorText) OpmAreSettingsValid(Dictionary<string, string> settings) {
            OpmSettingUtilityKeysight settingUtility = new OpmSettingUtilityKeysight();
            foreach (var setting in settings) {
                string key = setting.Key;
                string value = setting.Value;
                var checkResult = settingUtility.IsSettingValid(key, value);
                if (!checkResult.isOk) {
                    return (false, checkResult.errorText);
                }
            }

            return (true, string.Empty);
        }

        public override bool OpmBatchSetting(string slot, string channel, Dictionary<string, string> settings) {
            OpmSettingUtilityKeysight settingUtility = new OpmSettingUtilityKeysight();
            return settingUtility.BatchSetting(this, slot, channel, settings);
        }

        public override void TlsSetPowerUnitType(string slot, string channel, EnumOpticPowerUnitType powerUnitType) {
            string slotChannel = GetCommondSlotChannelPart(slot, channel);
            string command = $"SOUR{slotChannel}:POW:UNIT {powerUnitType.ToString()}";
            visaDriver.WriteLine(command);
        }

        public override EnumOpticPowerUnitType TlsGetPowerUnitType(string slot, string channel) {
            string slotChannel = GetCommondSlotChannelPart(slot, channel);
            string command = $"SOUR{slotChannel}:POW:UNIT?";
            string response = visaDriver.QueryLine(command);
            EnumOpticPowerUnitType result;
            if (response.Trim().Equals("0")) {
                result = EnumOpticPowerUnitType.DBM;
            } else {
                result = EnumOpticPowerUnitType.W;
            }
            return result;
        }

        public override void TlsSetWavelength(string slot, string channel, double waveLengthInNm) {
            string slotChannel = GetCommondSlotChannelPart(slot, channel);
            string command = $"SOUR{slotChannel}:WAV {waveLengthInNm}NM";
            visaDriver.WriteLine(command);
        }

        public override double TlsGetWavelength(string slot, string channel) {
            string slotChannel = GetCommondSlotChannelPart(slot, channel);
            string command = $"SOUR{slotChannel}:WAV?";
            string response = visaDriver.QueryLine(command);
            double wavelengthInM = Convert.ToDouble(response);
            double wavelengthInNM = wavelengthInM * 1E9;
            return Math.Round(wavelengthInNM, 2);
        }

        public override void TlsSetPower(string slot, string channel, double powerInDbm) {
            string slotChannel = GetCommondSlotChannelPart(slot, channel);
            string command = $"SOUR{slotChannel}:POW {powerInDbm}DBM";
            visaDriver.WriteLine(command);
        }

        public override double TlsGetPower(string slot, string channel) {
            string slotChannel = GetCommondSlotChannelPart(slot, channel);
            string command = $"SOUR{slotChannel}:POW?";
            string response = visaDriver.QueryLine(command);
            return Convert.ToDouble(response);
        }

        public override void TlsTurnOff(string slot, string channel) {
            TlsEnable(slot, channel, 0);
        }

        public override void TlsTurnOn(string slot, string channel) {
            TlsEnable(slot, channel, 1);
        }

        public override bool TlsIsTurnedOn(string slot, string channel) {
            string slotChannel = GetCommondSlotChannelPart(slot, channel);
            string command = $"OUTP{slotChannel}?";
            //[gyh]: 到底哪一个命令是正确的？
            //string command = $"SOUR{setting}:POW:STAT?";

            string response = visaDriver.QueryLine(command);
            return response.Trim().Equals("1");
        }

        private void TlsEnable(string slot, string channel, int flag) {
            string slotChannel = GetCommondSlotChannelPart(slot, channel);
            string command = $"OUTP{slotChannel} {flag}";
            //[gyh]: 到底哪一个命令是正确的？
            //string command = $"SOUR{setting}:POW:STAT {flag}";

            visaDriver.WriteLine(command);
        }

        public override bool TlsBatchSetting(string slot, string channel, Dictionary<string, string> settings) {
            TlsSettingUtilityKeysight settingUtility = new TlsSettingUtilityKeysight();
            return settingUtility.BatchSetting(this, slot, channel, settings);
        }

        public override (bool isOk, string errorText) TlsAreSettingsValid(Dictionary<string, string> settings) {
            TlsSettingUtilityKeysight settingUtility = new TlsSettingUtilityKeysight();
            foreach (var setting in settings) {
                string key = setting.Key;
                string value = setting.Value;
                var checkResult = settingUtility.IsSettingValid(key, value);
                if (!checkResult.isOk) {
                    return (false, checkResult.errorText);
                }
            }

            return (true, string.Empty);
        }

        private string GetCommondSlotChannelPart(string slot, string channel) {
            string trimSlot = slot.Trim();
            string trimChannel = channel.Trim();

            string slotPart = string.Empty;
            string channelPart = string.Empty;
            if (!string.IsNullOrEmpty(trimSlot)) {
                slotPart = trimSlot;
                if (!string.IsNullOrEmpty(trimChannel)) {
                    channelPart = $":CHAN{trimChannel}";
                }
            }

            return $"{slotPart}{channelPart}";
        }

        public (bool isOk, string errorText) IsTriggerInSettingKeyMissed(Dictionary<string, string> settings) {
            HashSet<string> validKeySet = new HashSet<string>();
            foreach (EnumTriggerCommonSetting setting in Enum.GetValues(typeof(EnumTriggerCommonSetting))) {
                validKeySet.Add(setting.ToString());
            }
            foreach (EnumTriggerInstrument slotChannel in Enum.GetValues(typeof(EnumTriggerInstrument))) {
                validKeySet.Add(slotChannel.ToString());
            }
            foreach (EnumTriggerInSetting setting in Enum.GetValues(typeof(EnumTriggerInSetting))) {
                validKeySet.Add(setting.ToString());
            }

            string errorText = string.Empty;
            var inputKeySet = settings.Keys.ToHashSet();
            if (!validKeySet.SetEquals(inputKeySet)) {
                string validKeys = $"{string.Join(",", validKeySet)}";
                string inputKeys = $"{string.Join(",", inputKeySet)}";
                errorText = $"Current keys of trigger-in-settings is invalid! Valid keys are {{{validKeys}}}; Current keys are{{{inputKeys}}}.";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            return (true, string.Empty);
        }

        public (bool isOk, string errorText) CheckTriggerInSettings(Dictionary<string, string> settings) {
            string errorText = string.Empty;

            foreach (var setting in settings) {
                string key = setting.Key;
                string value = setting.Value;

                if (!Enum.TryParse(key, out EnumTriggerInSetting enumKey)) {
                    errorText = $"PmaKeysight816x trigger-in-settings, key(={key}) is not a valid!";
                    LOGGER.Error(errorText);
                    return (false, errorText);
                }

                switch (enumKey) {
                    case EnumTriggerInSetting.AVERAGE_TIME_IN_MS:
                        if (!double.TryParse(value, out double averageTimeInMs)) {
                            errorText = $"PmaKeysight816x trigger-in-settings, {EnumTriggerInSetting.AVERAGE_TIME_IN_MS.ToString()}(={settings[EnumTriggerInSetting.AVERAGE_TIME_IN_MS.ToString()]}) should be an integer!";
                            LOGGER.Error(errorText);
                            return (false, errorText);
                        }
                        break;
                    case EnumTriggerInSetting.TIME_IN_MS_PER_POINT:
                        if (!double.TryParse(value, out double rangeInDbm)) {
                            errorText = $"PmaKeysight816x trigger-in-settings, {EnumTriggerInSetting.RANGE_IN_DBM.ToString()}(={settings[EnumTriggerInSetting.RANGE_IN_DBM.ToString()]}) should be an integer!";
                            LOGGER.Error(errorText);
                            return (false, errorText);
                        }
                        break;
                    case EnumTriggerInSetting.RANGE_IN_DBM:
                        if (!int.TryParse(value, out int timeInMsPerPoint)) {
                            errorText = $"PmaKeysight816x trigger-in-settings, {EnumTriggerInSetting.TIME_IN_MS_PER_POINT.ToString()}(={settings[EnumTriggerInSetting.TIME_IN_MS_PER_POINT.ToString()]}) should be an integer!";
                            LOGGER.Error(errorText);
                            return (false, errorText);
                        }
                        break;
                }
            }

            return (true, string.Empty);
        }

        public bool PrepareTriggeringIn(Dictionary<string, string> settings) {
            this.triggerInSlot = settings[EnumTriggerInstrument.SLOT.ToString()];
            this.triggerInChannel = settings[EnumTriggerInstrument.CHANNEL.ToString()];
            int.TryParse(settings[EnumTriggerCommonSetting.STEP_NUMBER.ToString()], out int stepNumber);
            double.TryParse(settings[EnumTriggerInSetting.AVERAGE_TIME_IN_MS.ToString()], out double averageTimeInMs);
            int.TryParse(settings[EnumTriggerInSetting.RANGE_IN_DBM.ToString()], out int rangeInDbm);
            //int.TryParse(settings[EnumTriggerInSetting.TIME_IN_MS_PER_POINT.ToString()], out int timeInMsPerPoint);

            try {
                /*
                                visaDriver.WriteLine($"SENS{triggerInSlot}:CHAN{triggerInChannel}:FUNC:STAT LOGG,STOP");                
                                visaDriver.WriteLine($"trig{triggerInSlot}:CHAN{triggerInChannel}:INP SME");
                                visaDriver.WriteLine($"trig:conf def");
                                //visaDriver.WriteLine($"SENS{triggerInSlot}:CHAN{triggerInChannel}:POW:UNIT 1") ;
                                //visaDriver.WriteLine($"SENS{triggerInSlot}:CHAN{triggerInChannel}:POW:ATIM {averageTimeInMs} MS");
                                visaDriver.WriteLine($"SENS{triggerInSlot}:CHAN{triggerInChannel}:POW:RANG:AUTO 0");
                                visaDriver.WriteLine($"SENS{triggerInSlot}:CHAN{triggerInChannel}:POW:RANG {rangeInDbm}DBM");
                                visaDriver.WriteLine($"SENS{triggerInSlot}:CHAN{triggerInChannel}:FUNC:PAR:LOGG {stepNumber},{averageTimeInMs}MS");
                                visaDriver.QueryLine($"SENS{triggerInSlot}:CHAN{triggerInChannel}:FUNC:STATE?");
                                Thread.Sleep(1000);
                */
                visaDriver.WriteLine($"sens{triggerInSlot}:chan{1}:func:stat logg,stop\n");

                visaDriver.WriteLine($"trig{triggerInSlot}:chan{1}:inp sme\n"
                           + "trig:conf def\n");                
                /*
                visaDriver.WriteLine($"sens{triggerInSlot}:chan{triggerInChannel}:pow:rang:auto 0\n"
                            + $"sens{triggerInSlot}:chan{triggerInChannel}:pow:rang {rangeInDbm}dbm\n"
                            + $"sens{triggerInSlot}:chan{triggerInChannel}:func:par:logg {stepNumber},{averageTimeInMs}ms\n");                        
                */
                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        public bool StartTriggeringIn() {
            try {
                visaDriver.WriteLine($"sens{triggerInSlot}:chan{1}:func:stat logg,star\n");
                //Thread.Sleep(50);  //增加延时，确保主从之间先后顺序
                Thread.Sleep(10);  //增加延时，确保主从之间先后顺序
                string response = visaDriver.QueryLine($"sens{triggerInSlot}:chan{1}:func:state?\n");
                return true;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }
        }

        public void WaitForTriggeringInCompleted() {

            DateTime timeStart = DateTime.Now;  
            while (true) {
                string response = string.Empty;
                Thread.Sleep(10);
                DateTime timeNow = DateTime.Now;
                TimeSpan span = timeNow - timeStart;
                if (span.TotalSeconds > 10) {
                    response = "pm816x wait for trigger complete timeout";
                    LOGGER.Error(response);
                    throw new Exception(response);                    
                }

                try {
                    response = visaDriver.QueryLine($"sens{triggerInSlot}:chan{1}:func:state?\n");
                }catch(Exception ex) {
                    LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                    response = string.Empty;
                }

                if (response.Contains("LOGGING_STABILITY,COMPLETE")) {
                    break;
                }
            }
        }

        public (bool isOk, List<double> data) GetTriggeringInData() {
            try {
                string command = $"sens{triggerInSlot}:chan{triggerInChannel}:func:res?";
                visaDriver.WriteLine(command);
                byte[] byteRet = visaDriver.ReadBytes();

                char[] chars = Encoding.ASCII.GetChars(byteRet);
                List<double> p_list = new List<double>();
                int offset = chars[1] - '0';
                int dataLen = (byteRet.Length - 4) / 4;
                for (int i = 0; i < dataLen; i++) {
                    float opower = BitConverter.ToSingle(byteRet, i * 4 + offset + 2);
                    p_list.Add(opower);
                }

                for (int i = 0; i < p_list.Count; i++) {
                    p_list[i] = 10 * Math.Log10(p_list[i] * 1000);
                    if (Double.IsNaN(p_list[i]) || Double.IsInfinity(p_list[i])) {
                        if (i > 0) {
                            p_list[i] = p_list[i - 1];
                        } else {
                            p_list[i] = -120;
                        }
                    }
                    if (p_list[i] > 10 && i > 0) {
                        p_list[i] = p_list[i - 1];
                    }
                }

                for (int i = 0; i < p_list.Count; i++) {
                    if (p_list[i] == -120) {
                        for (int j = i + 1; j < p_list.Count; j++) {
                            if (p_list[j] != -120) {
                                p_list[i] = p_list[j];
                                break;
                            }
                        }
                    }
                }

                return (true, p_list);
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return (false, null);
            }
            finally {
                visaDriver.WriteLine($"sens{triggerInSlot}:chan{1}:func:stat logg,stop\n"
                           + $"trig{triggerInSlot}:chan{1}:inp ign\n"
                           + "trig:conf dis\n");
            }
        }

        public override void OpmSetNoTriggerMode(string slot,string channel)
        {
            visaDriver.WriteLine($"sens{slot}:chan{1}:func:stat logg,stop\n"
                           + $"trig{slot}:chan{1}:inp ign\n"
                           + "trig:conf dis\n");
        }

        public override void OpmSetSamplePoint(string slot, string channel, int num,double averageTimeInMs)
        {
            visaDriver.WriteLine($"sens{slot}:chan{channel}:func:par:logg {num},{averageTimeInMs}ms\n");
        }



        //public override void OsSwitchTo(string slot, string channel, string inPort, string outPort) {
        //    LOGGER.Debug($"call PmaKeysight816x.OsSwitchTo({slot}，{channel}, {inPort}， {outPort})");
        //    //osVisa.OsSwitchTo(slot, inChannel, outChannel);
        //}

        //public override void OaSetAttenuation(string slot, string channel, double attenuationInDbm) {
        //    LOGGER.Debug($"call PmaKeysight816x.OaSetAttenuation({slot}， {channel}， {attenuationInDbm})");
        //    //oaVisa.OaSetAttenuation(slot, channel, attenuationInDbm);
        //}

        private readonly HashSet<string> validTlsSettingKeySet = new HashSet<string>();
        private readonly HashSet<string> validOpmSettingKeySet = new HashSet<string>();
        private IVisaDriver visaDriver;

        private string triggerInSlot;
        private string triggerInChannel;

        public enum EnumTriggerInSetting {
            AVERAGE_TIME_IN_MS,
            TIME_IN_MS_PER_POINT,
            RANGE_IN_DBM,
        }
    }
}
