using Prober.WaferDef;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prober.Forms
{
    public partial class FormLeadscrewCompensate : Form
    {
        public FormLeadscrewCompensate()
        {
            InitializeComponent();
        }

        private void btn_SelectFileX_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "(*.rtl)|*.rtl";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            txt_compDataPath_X.Text = dlg.FileName;
        }

        private void btn_SelectFileY_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "(*.rtl)|*.rtl";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            txt_compDataPath_Y.Text = dlg.FileName;
        }

        private void btn_SelectFileZ_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "(*.rtl)|*.rtl";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            txt_compDataPath_Z.Text = dlg.FileName;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            CompParamInfo info = new CompParamInfo();
            info.coderReverse = Convert.ToUInt16(txt_coderReverse_X.Text);
            info.sevonLevel = Convert.ToUInt16(txt_sevonLevel_X.Text);
            info.limitPos = Convert.ToInt32(txt_limitPos_X.Text);
            info.homeOffset = Convert.ToInt32(txt_homeOffset_X.Text);
            info.numPos = Convert.ToUInt16(txt_numPos_X.Text);
            info.posStart = Convert.ToInt32(txt_posStart_X.Text);
            info.posDis = Convert.ToInt32(txt_posDis_X.Text);
            info.compDataPath = txt_compDataPath_X.Text;
            info.distancePerPulse = Convert.ToDouble(txt_distancePerPulse_X.Text);
            info.minVel = Convert.ToDouble(txt_minVel_X.Text);
            info.maxVel = Convert.ToDouble(txt_maxVel_X.Text);
            info.posLimit = Convert.ToInt32(txt_posLimit_X.Text);
            info.negLimit = Convert.ToInt32(txt_negLimit_X.Text);
            ConfigMgr.SaveLeadscrwCompInfo("X", info);

            info.coderReverse = Convert.ToUInt16(txt_coderReverse_Y.Text);
            info.sevonLevel = Convert.ToUInt16(txt_sevonLevel_Y.Text);
            info.limitPos = Convert.ToInt32(txt_limitPos_Y.Text);
            info.homeOffset = Convert.ToInt32(txt_homeOffset_Y.Text);
            info.numPos = Convert.ToUInt16(txt_numPos_Y.Text);
            info.posStart = Convert.ToInt32(txt_posStart_Y.Text);
            info.posDis = Convert.ToInt32(txt_posDis_Y.Text);
            info.compDataPath = txt_compDataPath_Y.Text;
            info.distancePerPulse = Convert.ToDouble(txt_distancePerPulse_Y.Text);
            info.minVel = Convert.ToDouble(txt_minVel_Y.Text);
            info.maxVel = Convert.ToDouble(txt_maxVel_Y.Text);
            info.posLimit = Convert.ToInt32(txt_posLimit_Y.Text);
            info.negLimit = Convert.ToInt32(txt_negLimit_Y.Text);
            ConfigMgr.SaveLeadscrwCompInfo("Y", info);

            info.coderReverse = Convert.ToUInt16(txt_coderReverse_Z.Text);
            info.sevonLevel = Convert.ToUInt16(txt_sevonLevel_Z.Text);
            info.limitPos = Convert.ToInt32(txt_limitPos_Z.Text);
            info.homeOffset = Convert.ToInt32(txt_homeOffset_Z.Text);
            info.numPos = Convert.ToUInt16(txt_numPos_Z.Text);
            info.posStart = Convert.ToInt32(txt_posStart_Z.Text);
            info.posDis = Convert.ToInt32(txt_posDis_Z.Text);
            info.compDataPath = txt_compDataPath_Z.Text;
            info.distancePerPulse = Convert.ToDouble(txt_distancePerPulse_Z.Text);
            info.minVel = Convert.ToDouble(txt_minVel_Z.Text);
            info.maxVel = Convert.ToDouble(txt_maxVel_Z.Text);
            info.posLimit = Convert.ToInt32(txt_posLimit_Z.Text);
            info.negLimit = Convert.ToInt32(txt_negLimit_Z.Text);
            ConfigMgr.SaveLeadscrwCompInfo("Z", info);

            MessageBox.Show("补偿信息设置保存完毕");
        }

        private void btn_Load_Click(object sender, EventArgs e)
        {
            InitCompInfo();
        }

        private void InitCompInfo()
        {
            CompParamInfo infoX = ConfigMgr.LoadLeadscrwCompInfo("X");
            txt_coderReverse_X.Text = infoX.coderReverse.ToString();
            txt_sevonLevel_X.Text = infoX.sevonLevel.ToString();
            txt_limitPos_X.Text = infoX.limitPos.ToString();
            txt_homeOffset_X.Text = infoX.homeOffset.ToString();
            txt_numPos_X.Text = infoX.numPos.ToString();
            txt_posStart_X.Text = infoX.posStart.ToString();
            txt_posDis_X.Text = infoX.posDis.ToString();
            txt_compDataPath_X.Text = infoX.compDataPath;
            txt_minVel_X.Text = infoX.minVel.ToString();
            txt_maxVel_X.Text = infoX.maxVel.ToString();
            txt_posLimit_X.Text = infoX.posLimit.ToString();
            txt_negLimit_X.Text = infoX.negLimit.ToString();
            txt_distancePerPulse_X.Text = infoX.distancePerPulse.ToString();    

            CompParamInfo infoY = ConfigMgr.LoadLeadscrwCompInfo("Y");
            txt_coderReverse_Y.Text = infoY.coderReverse.ToString();
            txt_sevonLevel_Y.Text = infoY.sevonLevel.ToString();
            txt_limitPos_Y.Text = infoY.limitPos.ToString();
            txt_homeOffset_Y.Text = infoY.homeOffset.ToString();
            txt_numPos_Y.Text = infoY.numPos.ToString();
            txt_posStart_Y.Text = infoY.posStart.ToString();
            txt_posDis_Y.Text = infoY.posDis.ToString();
            txt_compDataPath_Y.Text = infoY.compDataPath;
            txt_minVel_Y.Text = infoY.minVel.ToString();
            txt_maxVel_Y.Text = infoY.maxVel.ToString();
            txt_posLimit_Y.Text = infoY.posLimit.ToString();
            txt_negLimit_Y.Text = infoY.negLimit.ToString();
            txt_distancePerPulse_Y.Text = infoY.distancePerPulse.ToString();

            CompParamInfo infoZ = ConfigMgr.LoadLeadscrwCompInfo("Z");
            txt_coderReverse_Z.Text = infoZ.coderReverse.ToString();
            txt_sevonLevel_Z.Text = infoZ.sevonLevel.ToString();
            txt_limitPos_Z.Text = infoZ.limitPos.ToString();
            txt_homeOffset_Z.Text = infoZ.homeOffset.ToString();
            txt_numPos_Z.Text = infoZ.numPos.ToString();
            txt_posStart_Z.Text = infoZ.posStart.ToString();
            txt_posDis_Z.Text = infoZ.posDis.ToString();
            txt_compDataPath_Z.Text = infoZ.compDataPath;
            txt_minVel_Z.Text = infoZ.minVel.ToString();
            txt_maxVel_Z.Text = infoZ.maxVel.ToString();
            txt_posLimit_Z.Text = infoZ.posLimit.ToString();
            txt_negLimit_Z.Text = infoZ.negLimit.ToString();
            txt_distancePerPulse_Z.Text = infoZ.distancePerPulse.ToString();
        }

        private void FormLeadscrewCompensate_Load(object sender, EventArgs e)
        {
            InitCompInfo();
        }
    }
}
