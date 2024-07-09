using System.Collections.Generic;

namespace MyInstruments.MySmu {
    public abstract class StandaloneSmu : Instrument, ISmu {
        protected StandaloneSmu(string id) : base(id) {
        }

        public abstract void SetSourceType(string slot, string channel, EnumSmuSourceType sourceType);
        public abstract void SetMeasurementType(string slot, string channel, EnumSmuMeasurementType measurementType);
        public abstract void SetSourceLevel(string slot, string channel, double level);
        public abstract void SetSourceLimit(string slot, string channel, double limit);
        public abstract double Measure(string slot, string channel);        
        public abstract void TurnOff(string slot, string channel);
        public abstract void TurnOn(string slot, string channel);

        public abstract bool SmuBatchSetting(string slot, string channel, Dictionary<string, string> settings);
        public abstract (bool isOk, string errorText) SmuAreSettingsValid(Dictionary<string, string> settings);        
    }
}
