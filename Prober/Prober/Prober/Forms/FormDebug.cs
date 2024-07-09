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
    public partial class FormDebug : Form
    {
        public FormDebug(ConcurrentDictionary<string, object> sharedObjects)
        {
            InitializeComponent();

            FormPowerStableTest frmPowerStableTest = new FormPowerStableTest(sharedObjects);
            frmPowerStableTest.TopLevel = false;
            frmPowerStableTest.Left = 2;
            frmPowerStableTest.FormBorderStyle = FormBorderStyle.None;
            frmPowerStableTest.Dock = DockStyle.Fill;
            page_PowerMonitor.Controls.Add(frmPowerStableTest);
            frmPowerStableTest.Show();
            
            FormTempStableTest frmTempStableTest = new FormTempStableTest(sharedObjects);            
            frmTempStableTest.TopLevel = false;
            frmTempStableTest.Left = 2;
            frmTempStableTest.FormBorderStyle = FormBorderStyle.None;
            frmTempStableTest.Dock = DockStyle.Fill;
            page_TempMonitor.Controls.Add(frmTempStableTest);
            frmTempStableTest.Show();
            
            FormHeightStableTest frmHeightStableTest = new FormHeightStableTest(sharedObjects);
            frmHeightStableTest.TopLevel = false;
            frmHeightStableTest.Left = 2;
            frmHeightStableTest.FormBorderStyle = FormBorderStyle.None;
            frmHeightStableTest.Dock = DockStyle.Fill;
            page_HeightMonitor.Controls.Add(frmHeightStableTest);
            frmHeightStableTest.Show();                       
        }
    }
}
