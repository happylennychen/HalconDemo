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
    public partial class FormCalibration : Form
    {
        private string CurWaferType = string.Empty;

        public FormCalibration(ConcurrentDictionary<string, object> sharedObjects, string waferType)
        {
            InitializeComponent();

            CurWaferType = waferType;

            FormWorkPositionSetting frmWorkPosSetting = new FormWorkPositionSetting(sharedObjects);
            frmWorkPosSetting.TopLevel = false;
            frmWorkPosSetting.Left = 2;
            frmWorkPosSetting.FormBorderStyle = FormBorderStyle.None;
            frmWorkPosSetting.Dock = DockStyle.Fill;    
            page_WorkPlace.Controls.Add(frmWorkPosSetting);
            frmWorkPosSetting.Show();   

            FormVisionMotionCalibration frmVisionMotionCali = new FormVisionMotionCalibration(sharedObjects);            
            frmVisionMotionCali.TopLevel = false;
            frmVisionMotionCali.Left = 2;
            frmVisionMotionCali.FormBorderStyle = FormBorderStyle.None;
            frmVisionMotionCali.Dock = DockStyle.Fill;
            page_VisionCamera.Controls.Add(frmVisionMotionCali);
            frmVisionMotionCali.Show();

            FormHeightCalibrate frmHeightCalibrate = new FormHeightCalibrate(sharedObjects, CurWaferType);
            frmHeightCalibrate.TopLevel = false;
            frmHeightCalibrate.Left = 2;
            frmHeightCalibrate.FormBorderStyle = FormBorderStyle.None;
            frmHeightCalibrate.Dock = DockStyle.Fill;
            page_HeightCalibrate.Controls.Add(frmHeightCalibrate);
            frmHeightCalibrate.Show();

            FormAngleHeightCali frmAngleCalibrate  = new FormAngleHeightCali(sharedObjects, CurWaferType);    
            frmAngleCalibrate.TopLevel = false;
            frmAngleCalibrate.Left = 2;
            frmAngleCalibrate.FormBorderStyle= FormBorderStyle.None;
            frmAngleCalibrate.Dock = DockStyle.Fill;
            Page_AngelCalibrate.Controls.Add(frmAngleCalibrate);
            frmAngleCalibrate.Show();

            FormProberClean frmProberClean = new FormProberClean(sharedObjects);
            frmProberClean.TopLevel = false;
            frmProberClean.Left = 2;
            frmProberClean.FormBorderStyle = FormBorderStyle.None;
            frmProberClean.Dock = DockStyle.Fill;
            Page_ProberClean.Controls.Add(frmProberClean);
            frmProberClean.Show();

            FormChuckHeightVerify frmHeightCali = new FormChuckHeightVerify(sharedObjects, CurWaferType);
            frmHeightCali.TopLevel = false;
            frmHeightCali.Left = 2;
            frmHeightCali.FormBorderStyle = FormBorderStyle.None;
            frmHeightCali.Dock = DockStyle.Fill;
            Page_HeightVerify.Controls.Add(frmHeightCali);
            frmHeightCali.Show();

            FormHeightScan frmHeightScan = new FormHeightScan(sharedObjects, CurWaferType);
            frmHeightScan.TopLevel = false;
            frmHeightScan.Left = 2;
            frmHeightScan.FormBorderStyle = FormBorderStyle.None;
            frmHeightScan.Dock = DockStyle.Fill;
            page_HeightScan.Controls.Add(frmHeightScan);
            frmHeightScan.Show();

            FormFARolling frmFARolling = new FormFARolling(sharedObjects);
            frmFARolling.TopLevel = false;
            frmFARolling.Left = 2;
            frmFARolling.FormBorderStyle = FormBorderStyle.None;
            frmFARolling.Dock = DockStyle.Fill;
            page_FaRolling.Controls.Add(frmFARolling);
            frmFARolling.Show();
        }
    }
}
