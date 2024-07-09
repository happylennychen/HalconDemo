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
    public partial class FormMoveToDutTest : Form
    {
        public string dutName = string.Empty;
        public string subDieName = string.Empty;

        public FormMoveToDutTest()
        {
            InitializeComponent();
        }        

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void btn_Move_Click(object sender, EventArgs e)
        {
            dutName = txt_ReticleName.Text;
            subDieName = txt_SubDieNo.Text;
            if (!IsReticleNameRight(dutName))
            {
                MessageBox.Show("input reticle name illegal!");
                return;
            }

            this.DialogResult = DialogResult.Yes;
        }

        private bool IsReticleNameRight(string name)
        {
            string nameUpper = name.ToUpper();
            
            if (name.Length != 3)            
                return false;     
                       
            if (nameUpper[0] > 'Z' ||  nameUpper[0] < 'A') 
                return false;

            for (int i = 1; i < 3;i++)
            {
                if (nameUpper[i] > '9' || nameUpper[i] < '0')
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
