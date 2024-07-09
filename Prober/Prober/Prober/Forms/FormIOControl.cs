using HalconDotNet;
using MyMotionStageDriver.MyMotionController.Leisai;
using Prober.WaferDef;
using ProberApi.MyConstant;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Windows.Forms;



namespace Prober.Forms {
    public partial class FormIOControl : Form
    {
        private readonly MotionControllerLeisaiDmc3000 motion;
        private byte chuckSize = 0;
        private IOInfo IO_Info = new IOInfo();  

        public FormIOControl(ConcurrentDictionary<string, object> sharedObjects)
        {
            InitializeComponent();

            sharedObjects.TryGetValue(SharedObjectKey.LEISAI_DM3000_INSTANCE, out object tempObj);
            motion = tempObj as MotionControllerLeisaiDmc3000;

            IO_Info = ConfigMgr.LoadIoInfo();
        }


        private void btn_AssistVacOpen_Click(object sender, System.EventArgs e) {
            try {

                //motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.RfPlat.ToString(), false);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.RfPlat.ToString(), true);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.SizeAll.ToString(), true);
                
                btn_AssistVacOpen.BackColor = Color.FromArgb(230, 244, 241);
                btn_AssistVacClose.BackColor = SystemColors.Control;
            } catch (Exception ex) {
                MessageBox.Show($"操作异常:{ex.Message}");
            }
        }

        private void btn_AssistVacClose_Click(object sender, System.EventArgs e) {
            try {
                //motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.RfPlat.ToString(), true);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.RfPlat.ToString(), false);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.SizeAll.ToString(), true);

                btn_AssistVacClose.BackColor = Color.FromArgb(230, 244, 241);
                btn_AssistVacOpen.BackColor = SystemColors.Control;
            } catch (Exception ex) {
                MessageBox.Show($"操作异常:{ex.Message}");
            }
        }

        private void btn_PDVacOpen_Click(object sender, System.EventArgs e) {
            try {
                //motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.PdPlat.ToString(), false);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.PdPlat.ToString(), true);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.SizeAll.ToString(), true);

                btn_PDVacOpen.BackColor = Color.FromArgb(230, 244, 241);
                btn_PDVacClose.BackColor = SystemColors.Control;
            } catch (Exception ex) {
                MessageBox.Show($"操作异常:{ex.Message}");
            }
        }

        private void btn_PDVacClose_Click(object sender, System.EventArgs e) {
            try {
                //motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.PdPlat.ToString(), true);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.PdPlat.ToString(), false);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.SizeAll.ToString(), true);

                btn_PDVacClose.BackColor = Color.FromArgb(230, 244, 241);
                btn_PDVacOpen.BackColor = SystemColors.Control;
            } catch (Exception ex) {
                MessageBox.Show($"操作异常:{ex.Message}");
            }
        }

        private void btn_CCDOpen_Click(object sender, System.EventArgs e) {
            try {
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Light.ToString(), false);

                btn_CCDOpen.BackColor = Color.FromArgb(230, 244, 241);
                btn_CCDClose.BackColor = SystemColors.Control;
            } catch (Exception ex) {
                MessageBox.Show($"操作异常:{ex.Message}");
            }
        }

        private void btn_CCDClose_Click(object sender, System.EventArgs e) {             
            try {
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Light.ToString(), true);

                btn_CCDClose.BackColor = Color.FromArgb(230, 244, 241);
                btn_CCDOpen.BackColor = SystemColors.Control;
            } catch (Exception ex) {
                MessageBox.Show($"操作异常:{ex.Message}");
            }
        }

        public bool EnableChuck(int size) {
            try {
                if (size >= 1) {
                    //motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Chip.ToString(), false);
                    motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Chip.ToString(), true);
                }

                if (size >= 2) {
                    //motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size2.ToString(), false);
                    motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size2.ToString(), true);
                } else {
                    //motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size2.ToString(), true);
                    motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size2.ToString(), false);
                }


                if (size >= 4) {
                    //motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size4.ToString(), false);
                    motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size4.ToString(), true);
                } else {
                    //motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size4.ToString(), true);
                    motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size4.ToString(), false);
                }

                if (size >= 6) {
                    //motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size6.ToString(), false);
                    motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size6.ToString(), true);
                } else {
                    //motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size6.ToString(), true);
                    motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size6.ToString(), false);
                }

                if (size >= 8) {
                    //motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size8.ToString(), false);
                    motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size8.ToString(), true);
                } else {
                    //motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size8.ToString(), true);
                    motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size8.ToString(), false);
                }

                if (size >= 12) {
                    //motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size12.ToString(), false);
                    motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size12.ToString(), true);
                } else {
                    //motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size12.ToString(), true);
                    motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size12.ToString(), false);
                }

                //总输入0关闭
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.SizeAll.ToString(), true);
                return true;

            } catch (Exception ex) {
                MessageBox.Show($"操作异常:{ex.Message}");
                return false;
            }                      
        }

        private void rbtn_Chip_CheckedChanged(object sender, System.EventArgs e) {
            chuckSize = 1;
            EnableChuck(chuckSize);
        }

        private void rbtn_2Inch_CheckedChanged(object sender, System.EventArgs e) {
            chuckSize = 2;
            EnableChuck(chuckSize);
        }

        private void rbtn_4Inch_CheckedChanged(object sender, System.EventArgs e) {
            chuckSize = 4;
            EnableChuck(chuckSize);
        }

        private void rbtn_6Inch_CheckedChanged(object sender, System.EventArgs e) {
            chuckSize = 6;
            EnableChuck(chuckSize);
        }

        private void rbtn_8Inch_CheckedChanged(object sender, System.EventArgs e) {
            chuckSize = 8;
            EnableChuck(chuckSize);
        }

        private void rbtn_12Inch_CheckedChanged(object sender, System.EventArgs e) {
            chuckSize = 12;
            EnableChuck(chuckSize);
        }

        private void rbtn_CloseAll_CheckedChanged(object sender, System.EventArgs e) {
            try {
                /*
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Chip.ToString(), true);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size2.ToString(), true);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size4.ToString(), true);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size6.ToString(), true);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size8.ToString(), true);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size12.ToString(), true);
                */
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Chip.ToString(), false);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size2.ToString(), false);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size4.ToString(), false);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size6.ToString(), false);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size8.ToString(), false);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Size12.ToString(), false);
            } catch (Exception ex) {
                MessageBox.Show($"操作异常:{ex.Message}");
            }              
        }

        public bool ChangeSignalLamp(SignalColor color) {
            try {
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Red.ToString(), true);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Green.ToString(), true);
                motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Yellow.ToString(), true);
                switch (color) {
                    case SignalColor.RED:
                        motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Red.ToString(), false);
                        break;
                    case SignalColor.GREEN:
                        motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Green.ToString(), false);
                        break;
                    case SignalColor.YELLOW:
                        motion.WriteOutput(IO_Info.CardIndex.ToString(), IO_Info.Yellow.ToString(), false);
                        break;
                    default:
                        break;
                }

                return true;
            } catch(Exception ex) {
                MessageBox.Show($"操作异常:{ex.Message}");
                return false;
            }                      
        }

        private void radioRed_CheckedChanged(object sender, EventArgs e) {
            ChangeSignalLamp(SignalColor.RED);
        }

        private void radioBlue_CheckedChanged(object sender, EventArgs e) {
            ChangeSignalLamp(SignalColor.GREEN);
        }

        private void radioYellow_CheckedChanged(object sender, EventArgs e) {
            ChangeSignalLamp(SignalColor.YELLOW);
        }

        private void FormIOControl_Load(object sender, EventArgs e)
        {
            //三色灯
            LoadThreeColorLightStatus();

            //光源，辅助平台
            LoadOtherIOStatus();

            //Chuck真空吸附
            LoadChuckVacStatus();
        }

        private void LoadChuckVacStatus()
        {
            bool chipStatus = motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.Chip.ToString());
            bool size2Status = motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.Size2.ToString());
            bool size4Status = motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.Size4.ToString());
            bool size6Status = motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.Size6.ToString());
            bool size8Status = motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.Size8.ToString());
            bool size12Status = motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.Size12.ToString());
            /*
            if (chipStatus && size2Status && size4Status && size6Status && size8Status && size12Status)
            {
                rbtn_CloseAll.Checked = true;
            }
            else if (!chipStatus && !size2Status && !size4Status && !size6Status && !size8Status && !size12Status)
            {
                rbtn_12Inch.Checked = true; 
            }
            else if (!chipStatus && !size2Status && !size4Status && !size6Status && !size8Status && size12Status)
            {
                rbtn_8Inch.Checked = true;
            }
            else if (!chipStatus && !size2Status && !size4Status && !size6Status && size8Status && size12Status)
            {
                rbtn_6Inch.Checked = true;
            }
            else if (!chipStatus && !size2Status && !size4Status && size6Status && size8Status && size12Status)
            {
                rbtn_4Inch.Checked = true;
            }
            else if (!chipStatus && !size2Status && size4Status && size6Status && size8Status && size12Status)
            {
                rbtn_2Inch.Checked = true;
            }
            else 
            {
                rbtn_Chip.Checked = true;
            }
            */
            if (!chipStatus && !size2Status && !size4Status && !size6Status && !size8Status && !size12Status)
            {
                rbtn_CloseAll.Checked = true;
            }
            else if (chipStatus && size2Status && size4Status && size6Status && size8Status && size12Status)
            {
                rbtn_12Inch.Checked = true;
            }
            else if (chipStatus && size2Status && size4Status && size6Status && size8Status && !size12Status)
            {
                rbtn_8Inch.Checked = true;
            }
            else if (chipStatus && size2Status && size4Status && size6Status && !size8Status && !size12Status)
            {
                rbtn_6Inch.Checked = true;
            }
            else if (chipStatus && size2Status && size4Status && !size6Status && !size8Status && !size12Status)
            {
                rbtn_4Inch.Checked = true;
            }
            else if (chipStatus && size2Status && !size4Status && !size6Status && !size8Status && !size12Status)
            {
                rbtn_2Inch.Checked = true;
            }
            else
            {
                rbtn_Chip.Checked = true;
            }
        }

        private void LoadOtherIOStatus()
        {
            //相机光源
            bool LightStatus = motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.Light.ToString());
            if(LightStatus)
            {
                btn_CCDClose.BackColor = Color.FromArgb(230, 244, 241);
                btn_CCDOpen.BackColor = SystemColors.Control;
            }
            else
            {
                btn_CCDOpen.BackColor = Color.FromArgb(230, 244, 241);
                btn_CCDClose.BackColor = SystemColors.Control;
            }

            //光阑吸附
            bool PdVacStatus = motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.PdPlat.ToString());
            if (!PdVacStatus)
            {
                btn_PDVacClose.BackColor = Color.FromArgb(230, 244, 241);
                btn_PDVacOpen.BackColor = SystemColors.Control;
            }
            else
            {
                btn_PDVacOpen.BackColor = Color.FromArgb(230, 244, 241);
                btn_PDVacClose.BackColor = SystemColors.Control;
            }

            //辅助台吸附
            bool AssistStatus = motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.RfPlat.ToString());
            if (!AssistStatus)
            {
                btn_AssistVacClose.BackColor = Color.FromArgb(230, 244, 241);
                btn_AssistVacOpen.BackColor = SystemColors.Control;
            }
            else
            {
                btn_AssistVacOpen.BackColor = Color.FromArgb(230, 244, 241);
                btn_AssistVacClose.BackColor = SystemColors.Control;
            }
        }

        private void LoadThreeColorLightStatus()
        {
            //读取当前IO状态，并显示到界面上
            bool RedLightStatus = motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.Red.ToString());
            bool GreenLightStatus = motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.Green.ToString());
            bool YellowLightStatus = motion.ReadOutput(IO_Info.CardIndex.ToString(), IO_Info.Yellow.ToString());

            if (!RedLightStatus)
            {
                radioRed.Checked = true;
            }

            if (!GreenLightStatus)
            {
                radioBlue.Checked = true;
            }

            if (!YellowLightStatus)
            {
                radioYellow.Checked = true;
            }
        }
    }
}
