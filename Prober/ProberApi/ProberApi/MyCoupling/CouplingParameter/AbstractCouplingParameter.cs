using System.Collections.Concurrent;

namespace ProberApi.MyCoupling.CouplingParameter {
    public abstract class AbstractCouplingParameter {
        public AbstractCouplingParameter(ConcurrentDictionary<string, object> sharedObjects) {
            this.SharedObjects = sharedObjects;
        }

        public bool EnabledCouplingIn { get; set; } = false;
        public string CouplingInId { get; set; }
        public virtual string CouplingFeedbackId { get; set; }
        public bool ShowGui { get; set; } = false;
        public bool SettingCouplingInInstrument { get; set; } = false;
        public bool SettingCouplingFeedbackInstrument { get; set; } = false;
        public bool SavingRawData { get; set; } = false;
        public bool DealwithWeight { get; set; } = false;
        public double Threshold { get; set; } = double.NaN;
        public string RawDataFileNamePrefix { get; set; } = string.Empty;

        public abstract AbstractCouplingParameter DeepCopy();

        public ConcurrentDictionary<string, object> SharedObjects { get; private set; }

        //[Attention]: EnabledCouplingIn, CouplingInId, CouplingFeedbackId can not be overwritten!
        public enum EnumCommonOverwrittenParameterId {
            SHOW_GUI = 0,                 //bool
            SETTING_IN_INSTRUMENT,        //bool
            SETTING_FEEDBACK_INSTRUMENT,  //bool 
            SAVING_RAW_DATA,              //bool
            DEALWITH_WEIGHT,              //bool
            THRESHOLD,                    //double
            RAW_DATA_FILE_NAME_PREFIX     //string
        }
    }
}
