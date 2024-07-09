using Prober.Request;
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
    public partial class FormOtherSetting : Form
    {
        ConcurrentDictionary<string, object> sharedObjects;
        public FormOtherSetting(ConcurrentDictionary<string, object> sharedObjects)
        {
            this.sharedObjects = sharedObjects;
            InitializeComponent();
        }

        private void chb_EnableChuckSafePosMonitor_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_EnableChuckSafePosMonitor.Checked)
            {
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POSITION_MONITOR_ENABLE, true, (key, oldValue) => true);
            }
            else
            {
                sharedObjects.AddOrUpdate(PrivateSharedObjectKey.CHUCK_POSITION_MONITOR_ENABLE, false, (key, oldValue) => false);
            }
        }

        private void FormOtherSetting_Load(object sender, EventArgs e) {
            sharedObjects.TryGetValue(PrivateSharedObjectKey.CHUCK_POSITION_MONITOR_ENABLE, out object enable);
            bool enableMonitor = (bool)enable;

            chb_EnableChuckSafePosMonitor.Checked = enableMonitor;
        }
    }
}
