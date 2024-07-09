using System.Collections.Generic;

namespace MyInstruments.MySmu {
    public interface ISmu {
        void SetSourceType(string slot, string channel, EnumSmuSourceType sourceType);
        void SetMeasurementType(string slot, string channel, EnumSmuMeasurementType measurementType);
        void SetSourceLevel(string slot, string channel, double level);
        void SetSourceLimit(string slot, string channel, double limit);
        double Measure(string slot, string channel);
        void TurnOn(string slot, string channel);
        void TurnOff(string slot, string channel);

        (bool isOk, string errorText) SmuAreSettingsValid(Dictionary<string, string> settings);
        bool SmuBatchSetting(string slot, string channel, Dictionary<string, string> settings);
        
        //--------------------------------------------------------------------------------------
        //[gyh]: to be added trigger相关接口
    }
}
