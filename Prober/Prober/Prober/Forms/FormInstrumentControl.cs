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

namespace Prober.Forms
{
    public partial class FormInstrumentControl : Form
    {
        ConcurrentDictionary<string, object> sharedObject;
        public FormInstrumentControl(ConcurrentDictionary<string, object> sharedObjects)
        {
            this.sharedObject = sharedObjects;
            InitializeComponent();
        }

        private void FormInstrumentControl_Load(object sender, EventArgs e)
        {
            FormIOControl frmIOControl = new FormIOControl(sharedObject);
            frmIOControl.TopLevel = false;
            frmIOControl.Left = 2;
            frmIOControl.FormBorderStyle = FormBorderStyle.None;
            frmIOControl.Dock = DockStyle.Fill;
            page_IOControl.Controls.Add(frmIOControl);
            frmIOControl.Show();

            FormTecControl frmTecControl = new FormTecControl(sharedObject);
            frmTecControl.TopLevel = false;
            frmTecControl.Left = 2;
            frmTecControl.FormBorderStyle = FormBorderStyle.None;
            frmTecControl.Dock = DockStyle.Fill;
            page_TempControl.Controls.Add(frmTecControl);
            frmTecControl.Show();

            FormPmSetting frmPmSetting = new FormPmSetting(sharedObject);
            frmPmSetting.TopLevel = false;
            frmPmSetting.Left = 2;
            frmPmSetting.FormBorderStyle = FormBorderStyle.None;
            frmPmSetting.Dock = DockStyle.Fill;
            page_PmSetting.Controls.Add(frmPmSetting);
            frmPmSetting.Show();            
        }
    }
}
