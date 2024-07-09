using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Prober.WaferDef;

namespace Prober.Forms
{
    public partial class FormSelectWaferMark : Form
    {
        public HObject Image;
        private int State = 0;
        public bool IsFinished = false;
        public HTuple id;
        public HTuple row;
        public HTuple col;

        public FormSelectWaferMark(HObject image)
        {
            InitializeComponent();
            Image = image;
            this.TopMost = true;
        }

        private void btn_Rectangle_Click(object sender, EventArgs e)
        {
            if (State != 0)
            {
                MessageBox.Show(this, "请右键结束操作。", "Info:");
                return;
            }
            try
            {
                HOperatorSet.ClearWindow(hw_Base.HalconWindow);
                HOperatorSet.DispObj(Image, hw_Base.HalconWindow);
                State = 1;
                hw_Base.Focus();
                if (!VisionMgr.SelectMark(hw_Base.HalconWindow, Image, out id, out row, out col))
                {
                    MessageBox.Show(this, "选择Mark失败。", "Info:");
                    return;
                }
                IsFinished = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, $"选择Mark异常:{ex.Message}。", "Info:");
            }
            finally
            {
                State = 0;
            }
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            if (IsFinished)
            {
                this.DialogResult = DialogResult.Yes;
            }
            else
            {
                MessageBox.Show(this, "请选择Mark", "Info:");
                return;
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            if (State != 0)
            {
                MessageBox.Show(this, "请右键结束操作。", "Info:");
                return;
            }
            this.DialogResult = DialogResult.No;
        }

        private void FrmSelectWaferMark_FormClosing(object sender, FormClosingEventArgs e)
        {            
            if (State != 0)
            {
                MessageBox.Show(this, "请右键结束操作。", "Info:");
                return;
            }
        }

        private void FrmSelectWaferMark_Load(object sender, EventArgs e)
        {
            HOperatorSet.SetColor(hw_Base.HalconWindow, "red");
            if (Image == null)
            {
                MessageBox.Show(this, "没有图片，请检查相机是否连接正常。", "Info:");
                this.Close();
            }
            HOperatorSet.DispObj(Image, hw_Base.HalconWindow);
        }

        private void hw_Base_HMouseWheel(object sender, HMouseEventArgs e)
        {
            HTuple winID = hw_Base.HalconWindow;
            HTuple Zoom, Row, Col, Button;
            HTuple Row0, Col0, Row00, Col00, Ht, Wt, r1, c1, r2, c2;
            try
            {
                if (e.Delta > 0)
                {
                    Zoom = 1.5;
                }
                else
                {
                    Zoom = 0.5;
                }
                HOperatorSet.GetMposition(winID, out Row, out Col, out Button);
                HOperatorSet.GetPart(winID, out Row0, out Col0, out Row00, out Col00);
                Ht = Row00 - Row0;
                Wt = Col00 - Col0;
                if (Ht * Wt < 32000 * 32000 || Zoom == 1.5)
                {
                    r1 = Row0 + (1 - 1 / Zoom) * (Row - Row0);
                    c1 = Col0 + (1 - 1 / Zoom) * (Col - Col0);
                    r2 = r1 + Ht / Zoom;
                    c2 = c1 + Wt / Zoom;
                    HOperatorSet.SetPart(winID, r1, c1, r2, c2);
                    HOperatorSet.ClearWindow(winID);
                    HOperatorSet.DispObj(Image, hw_Base.HalconWindow);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"图像放大失败:{ex.Message}");
            }
        }
    }
}
