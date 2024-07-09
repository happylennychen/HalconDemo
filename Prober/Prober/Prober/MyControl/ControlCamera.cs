using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using MyInstruments.MyCamera;
using Prober.Properties;

namespace Prober.MyControl
{
    public partial class ControlCamera : UserControl
    {
        private StandaloneCamera camera;
        public Action<bool> MaxMinClick;
        public Action<int> ZoomSelect;
        public Action<string> ReportMessage;
        private object _lockerShow = new object();
        public ImageShowInfo _showInfo = new ImageShowInfo();

        private int h_y;
        private bool h_move = false;

        private int v_x;
        private bool v_move = false;

        private double pixLenCoef = 1.2;
        private string zoomSize = "2";

        public ControlCamera()
        {
            InitializeComponent();
        }

        public void SetPixLenCoef(double pixLenCoef)
        {
            this.pixLenCoef = pixLenCoef;
        }

        public void SetZoomList(List<string> zoomList)
        {
            cmb_Zoom.Items.Clear();
            for (int i = 0; i < zoomList.Count; i++)
            {
                cmb_Zoom.Items.Add(zoomList[i]);
            }
        }

        public void InitCamera(StandaloneCamera camera,string Type,bool isAutoBalance = true)
        {
            this.camera = camera;

            if (!camera.IsOpen)
            {
                MessageBox.Show($"{Type} 相机未打开，请检查相机设置");
                ReportMessage($"{Type} 相机未打开，请检查相机设置");
                return;
            }
            //2448  2048
            //camera.Width = 5496;
            //camera.Height = 3672;

            //hw_Window.ImagePart = new Rectangle(0, 0, camera.Width, camera.Height);
            hw_Window.HalconWindow.SetPart(0,0,camera.Width,camera.Height);
            camera.ContinuShotFunc = ShowImage;
            camera.StartContinuousShot();

            if (isAutoBalance)
            {
                camera.SetParameter("BalanceWhiteAuto", "Continuous");//Off -关闭
            }            

            panel_tool.Enabled = true;

            foreach (Control item in panel_tool.Controls)
            {
                item.Enabled = true;
            }

        }

        public void ChangeShowInfo(ImageShowInfo info)
        {
            lock (_lockerShow)
            {
                _showInfo = info;
                if (_showInfo.RotationAngle == 90 || _showInfo.RotationAngle == 180)
                {
                    hw_Window.ImagePart = new Rectangle(0, 0, camera.Height, camera.Width);
                }
                else
                {
                    hw_Window.ImagePart = new Rectangle(0, 0, camera.Width, camera.Height);
                }
            }
        }

        private void FrmCamera_Load(object sender, EventArgs e)
        {
            InitCrossPosition();

            panel_tool.Enabled = false;

            foreach (Control item in panel_tool.Controls)
            {
                item.Enabled = false;
            }
        }

        public void ShowImage(HObject image)
        {
            lock (_lockerShow)
            {
                if (_showInfo.RotationAngle == 90 || _showInfo.RotationAngle == 180)
                {
                    HOperatorSet.RotateImage(image, out image, _showInfo.RotationAngle, "constant");
                }

                if (_showInfo.IsMirrorByRow)
                {
                    HOperatorSet.MirrorImage(image, out image, "row");
                }

                if (_showInfo.IsMirrorByCol)
                {
                    HOperatorSet.MirrorImage(image, out image, "column");
                }

                if (_showInfo.Regions.Count > 0)
                {
                    HTuple color = new HTuple(0, 255, 0);
                    _showInfo.Regions.ForEach(t =>
                    {
                        HOperatorSet.PaintRegion(t, image, out image, color, "fill");
                    });
                }

                if (_showInfo.Xlds.Count > 0)
                {
                    HTuple color = new HTuple(0, 255, 0);
                    _showInfo.Xlds.ForEach(t =>
                    {
                        HOperatorSet.PaintXld(t, image, out image, color);
                    });
                }
            }

            HOperatorSet.DispObj(image, hw_Window.HalconWindow);
        }

        private void InitCrossPosition()
        {
            int hx = hw_Window.Location.X + (hw_Window.Width - lbl_Cross_H.Width) / 2;
            int hy = hw_Window.Location.Y + (hw_Window.Height - lbl_Cross_H.Height) / 2;
            lbl_Cross_H.Location = new Point(hx, hy);

            int vx = hw_Window.Location.X + (hw_Window.Width - lbl_Cross_V.Width) / 2;
            int vy = hw_Window.Location.Y + (hw_Window.Height - lbl_Cross_V.Height) / 2;
            lbl_Cross_V.Location = new Point(vx, vy);
        }


        private void btn_MeasureDistance_Click(object sender, EventArgs e)
        {
            //1:如果是实时显示模式，则停止改为shot模式
            if (camera.IsContinous)
            {
                camera.StopContinuousShot();
            }

            camera.SignlShot(out HObject _measureImage);
            ShowImage(_measureImage);

            HOperatorSet.SetColor(hw_Window.HalconWindow, "red");
            MessageBox.Show(this, "请选择第一个点", "Info");
            hw_Window.Focus();
            HOperatorSet.DrawPoint(hw_Window.HalconWindow, out HTuple row1, out HTuple col1);
            HOperatorSet.DispCross(hw_Window.HalconWindow, row1, col1, 10, 0);

            MessageBox.Show(this, "请选择第二个点", "Info");
            hw_Window.Focus();
            HOperatorSet.DrawPoint(hw_Window.HalconWindow, out HTuple row2, out HTuple col2);
            HOperatorSet.DispCross(hw_Window.HalconWindow, row2, col2, 10, 0);

            HOperatorSet.DistancePp(row1, col1, row2, col2, out HTuple distance);

            HOperatorSet.GenRegionLine(out HObject line, row1, col1, row2, col2);
            HOperatorSet.DispObj(line, hw_Window.HalconWindow);

            HOperatorSet.DispText(hw_Window.HalconWindow, $"{(distance.D * pixLenCoef * 2 / Convert.ToDouble(zoomSize)).ToString("f1")}um", "image", row2 - 10, col2 + 10, "red", null, null);                        

            btn_ExitMeasure.Enabled = true;
        }

        private void btn_MeasureAngle_Click(object sender, EventArgs e)
        {
            //1:如果是实时显示模式，则停止改为shot模式
            if (camera.IsContinous)
            {
                camera.StopContinuousShot();
            }

            camera.SignlShot(out HObject _measureImage);
            ShowImage(_measureImage);

            HOperatorSet.SetColor(hw_Window.HalconWindow, "red");
            MessageBox.Show(this, "请绘制第一条直线", "Info");
            hw_Window.Focus();
            HOperatorSet.DrawLine(hw_Window.HalconWindow, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);
            HOperatorSet.GenRegionLine(out HObject line1, row1, col1, row2, col2);
            HOperatorSet.DispObj(line1, hw_Window.HalconWindow);

            MessageBox.Show(this, "请绘制第二条直线", "Info");
            hw_Window.Focus();
            HOperatorSet.DrawLine(hw_Window.HalconWindow, out HTuple row3, out HTuple col3, out HTuple row4, out HTuple col4);
            HOperatorSet.GenRegionLine(out HObject line2, row3, col3, row4, col4);
            HOperatorSet.DispObj(line2, hw_Window.HalconWindow);

            HOperatorSet.AngleLl(row1, col1, row2, col2, row3, col3, row4, col4, out HTuple angel);
            HOperatorSet.TupleDeg(angel, out HTuple deg);

            HOperatorSet.DispText(hw_Window.HalconWindow, $"{deg.D.ToString("f2")}°", "image", row4 - 10, col4 + 10, "red", null, null);

            btn_ExitMeasure.Enabled = true;
        }

        private void btn_ExitMeasure_Click(object sender, EventArgs e)
        {
            //退出shot模式，进入连续抓图模式
            if (!camera.IsContinous)
            {
                camera.StartContinuousShot();
            }

            btn_ExitMeasure.Enabled = false;
        }

        private void btn_RefLine_Click(object sender, EventArgs e)
        {
            if (btn_cross_horizon_1.Visible)
            {
                btn_cross_horizon_1.Visible = false;
                btn_cross_vetical_4.Visible = false;
                lbl_Cross_H.Visible = false;
                lbl_Cross_V.Visible = false;

                InitCrossPosition();
            }
            else
            {
                btn_cross_horizon_1.Visible = true;
                btn_cross_vetical_4.Visible = true;
                lbl_Cross_H.Visible = true;
                lbl_Cross_V.Visible = true;
            }
        }

        private void btn_Increase_Click(object sender, EventArgs e)
        {
            double exposure = Convert.ToDouble(num_ExStep.Value);
            camera.GetBaseParameters(out double exp, out double gain, out double gama);
            camera.SetExposure(exp + exposure);
        }

        private void btn_Reduce_Click(object sender, EventArgs e)
        {
            double exposure = Convert.ToDouble(num_ExStep.Value);
            camera.GetBaseParameters(out double exp, out double gain, out double gama);

            if (exp - exposure < 0)
            {
                return;
            }
            camera.SetExposure(exp - exposure);
        }

        private void btn_SaveImamge_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "(*.jpg)|*.jpg";
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string file = dlg.FileName;
            bool bRet = camera.SaveImage(file);

            if (bRet)
            {
                MessageBox.Show(this, "保存成功！", "提示：");
            }
            else
            {
                MessageBox.Show(this, "保存失败！", "提示：");
            }
        }

        private void btn_FullScreen_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string pos = btn.Tag.ToString();

            hw_Window.ImagePart = new Rectangle(0, 0, camera.Width, camera.Height);
        }

        bool isMin = true;

        private void btn_MaxMin_Click(object sender, EventArgs e)
        {
            //对外发送一个消息，由外部决定显示
            MaxMinClick?.Invoke(isMin);

            Button btn = sender as Button;
            btn.BackgroundImage = isMin ? Resources.Min : Resources.Max;

            btn_cross_horizon_1.Width = (int)(hw_Window.Width);
            btn_cross_vetical_4.Height = (int)(hw_Window.Height);

            isMin = !isMin;
        }

        private void cmb_Zoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmb_Zoom.SelectedIndex == -1) {
                return;
            }

            ZoomSelect(cmb_Zoom.SelectedIndex);
            zoomSize = cmb_Zoom.SelectedItem.ToString();    
        }

        private void hw_Window_HMouseWheel(object sender, HMouseEventArgs e)
        {
            HTuple winID = hw_Window.HalconWindow;
            HTuple Zoom, Row, Col;
            HTuple Row0, Col0, Row00, Col00, Ht, Wt, r1, c1, r2, c2;
            try
            {
                if (e.Delta > 0)
                {
                    Zoom = 1.25;
                }
                else
                {
                    Zoom = 0.75;
                }

                if (_showInfo.RotationAngle == 0 || _showInfo.RotationAngle == 180)
                {
                    Row = camera.Height / 2;
                    Col = camera.Width / 2;
                }
                else
                {
                    Row = camera.Width / 2;
                    Col = camera.Height / 2;
                }

                HOperatorSet.GetPart(winID, out Row0, out Col0, out Row00, out Col00);
                Ht = Row00 - Row0;
                Wt = Col00 - Col0;
                if (Ht * Wt < 32000 * 32000 || Zoom == 1.25)
                {
                    r1 = Row0 + (1 - 1 / Zoom) * (Row - Row0);
                    c1 = Col0 + (1 - 1 / Zoom) * (Col - Col0);
                    r2 = r1 + Ht / Zoom;
                    c2 = c1 + Wt / Zoom;
                    HOperatorSet.SetPart(winID, r1, c1, r2, c2);
                    HOperatorSet.ClearWindow(winID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void btn_cross_horizon_1_MouseDown(object _showInfosender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            this.Cursor = Cursors.Hand;
            h_move = true;
            h_y = e.Y;
        }

        private void btn_cross_horizon_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!h_move) return;
            if (e.Button != MouseButtons.Left) return;
            Button btn = sender as Button;
            int delta = e.Y - h_y;
            int y = btn.Location.Y + delta;
            if (y > hw_Window.Location.Y + hw_Window.Height - btn.Height) return;
            if (y < hw_Window.Location.Y) return;
            btn.Location = new Point(btn.Location.X, y);
        }

        private void btn_cross_horizon_1_MouseUp(object sender, MouseEventArgs e)
        {
            h_move = false;
            this.Cursor = Cursors.Default;
        }

        private void btn_cross_vetical_4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            v_move = true;
            this.Cursor = Cursors.Hand;
            v_x = e.X;
        }

        private void btn_cross_vetical_4_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            Button btn = sender as Button;
            if (!v_move) return;
            int delta = e.X - v_x;
            int x = btn.Location.X + delta;
            if (x < hw_Window.Location.X) return;
            if (x > hw_Window.Location.X + hw_Window.Width - btn.Width - 100) return;

            btn.Location = new Point(x, btn.Location.Y);
        }

        private void btn_cross_vetical_4_MouseUp(object sender, MouseEventArgs e)
        {
            v_move = false;
            this.Cursor = Cursors.Default;
        }

        bool isRotate = false;

        private void btn_Rotate_Click(object sender, EventArgs e)
        {
            isRotate = !isRotate;
            ImageShowInfo info = null;
            if (isRotate)
            {
                info = new ImageShowInfo() { RotationAngle = 0, IsMirrorByRow = false };
            }
            else
            {
                info = new ImageShowInfo() { RotationAngle = 90, IsMirrorByRow = true };
            }

            ChangeShowInfo(info);
        }

        private HTuple _measureAngle1;
        private HTuple _measureAngle2;
        private CancellationTokenSource _measureAngleSource;

        private void btn_LineRotate_Click(object sender, EventArgs e) {
            if (btn_LineRotate.BackColor == Color.FromArgb(230, 244, 241)) {
                HOperatorSet.CreateDrawingObjectLine(hw_Window.ImagePart.Height / 4, hw_Window.ImagePart.Width / 4, hw_Window.ImagePart.Height / 4, hw_Window.ImagePart.Width / 4 + 1000, out _measureAngle1);
                HOperatorSet.CreateDrawingObjectLine(hw_Window.ImagePart.Height / 4, hw_Window.ImagePart.Width / 4, hw_Window.ImagePart.Height / 4 + 1000, hw_Window.ImagePart.Width / 4, out _measureAngle2);
                HOperatorSet.SetDrawingObjectParams(_measureAngle1, "color", "red");
                HOperatorSet.SetDrawingObjectParams(_measureAngle2, "color", "red");
                HOperatorSet.AttachDrawingObjectToWindow(hw_Window.HalconWindow, _measureAngle1);
                HOperatorSet.AttachDrawingObjectToWindow(hw_Window.HalconWindow, _measureAngle2);
                btn_LineRotate.BackColor = Color.Pink;
                lbl_MeasureValue.Visible = true;
                HTuple res1;
                HTuple res2;
                _measureAngleSource = new CancellationTokenSource();
                Task.Run(() =>
                {
                    while (!_measureAngleSource.IsCancellationRequested) {
                        HOperatorSet.GetDrawingObjectParams(_measureAngle1, new HTuple("row1", "column1", "row2", "column2"), out res1);
                        HOperatorSet.GetDrawingObjectParams(_measureAngle2, new HTuple("row1", "column1", "row2", "column2"), out res2);
                        HOperatorSet.AngleLl(res1[0], res1[1], res1[2], res1[3], res2[0], res2[1], res2[2], res2[3], out HTuple h_angle);
                        double angle = h_angle.TupleDeg().D;
                        BeginInvoke(new Action(() =>
                        {
                            lbl_MeasureValue.Text = Math.Round(angle, 3).ToString() + "°";
                        }));
                        Thread.Sleep(50);
                    }
                }, _measureAngleSource.Token);
            } else {
                _measureAngleSource.Cancel();
                btn_LineRotate.BackColor = Color.FromArgb(230, 244, 241);
                HOperatorSet.DetachDrawingObjectFromWindow(hw_Window.HalconWindow, _measureAngle1);
                HOperatorSet.DetachDrawingObjectFromWindow(hw_Window.HalconWindow, _measureAngle2);
                lbl_MeasureValue.Visible = false;                
            }
        }

        private void btn_AutoBalance_Click(object sender, EventArgs e)
        {
            btn_AutoBalance.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            Task.Run(() =>
            {
                camera.SetParameter("BalanceWhiteAuto", "Continuous");//Off -关闭                
                Thread.Sleep(2000);                
                camera.SetParameter("BalanceWhiteAuto", "Off");//Off -关闭
                Invoke(new Action(() =>
                {
                    btn_AutoBalance.Enabled = true;
                    this.Cursor = Cursors.Default;
                }));
            });          
        }
    }

    public class ImageShowInfo
    {
        public bool IsMirrorByRow { get; set; }
        public bool IsMirrorByCol { get; set; }
        public double RotationAngle { get; set; } = 0;
        public List<HObject> Regions { get; set; } = new List<HObject>();
        public List<HObject> Xlds { get; set; } = new List<HObject>();
    }
}
