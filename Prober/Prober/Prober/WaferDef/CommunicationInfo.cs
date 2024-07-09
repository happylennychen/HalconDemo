using System;

namespace Prober.WaferDef {
    [Serializable]
    public class CommunicationInfo
    {
        public string MotionWaferCom { get; set; } = "1";
        public string MotionFACom { get; set; } = "0";
        public string PDLPowerCom { get; set; } = "GPIB0::20::INSTR";
        public string PDLLaserCom { get; set; } = "TCPIP0::100.65.26.109::inst0::INSTR";
        public string PDLConfig { get; set; } = @"c:\users\Public\Documents\Photonic Application Suite\KtPasEngineLS.agconfig";
        public string DCPowerCom { get; set; } = "COM3";
        public string DCSource1Com { get; set; } = "GPIB0::05::INSTR";
        public string DCSource2Com { get; set; } = "GPIB0::06::INSTR";
        public string AltimeterCom { get; set; } = "192.168.0.1::6700";
        public string TopCameraCom { get; set; } = "26760166C8F7_Basler_acA547217uc";
        public string SideCameraCom { get; set; } = "26760166C8F7_Basler_acA547217uc";
        public string HorizontalCameraCom { get; set; } = "26760166C8F7_Basler_acA547217uc";
        public string OSwitchCom { get; set; } = "COM8";
        public string OmronCom { get; set; } = "COM4";
        public string Semight1Com { get; set; } = "PXI0::CHASSIS1::SLOT2::INSTR";
        public string Semight2Com { get; set; } = "PXI0::CHASSIS1::SLOT3::INSTR";
        public string Semight3Com { get; set; } = "PXI0::CHASSIS1::SLOT4::INSTR";
        public string Semight4Com { get; set; } = "PXI0::CHASSIS1::SLOT5::INSTR";
        public string PSCom { get; set; } = "192.168.0.10::5025";
    }
}
