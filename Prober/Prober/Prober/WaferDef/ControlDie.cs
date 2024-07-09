using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Prober.WaferDef 
{
    public partial class ControlDie : UserControl
    {
        public static DieState CurState = DieState.None;
        public static ControlDie PreSelectedDie = null;
        public static ControlDie tempDie = null;
        public static ControlDie PreHomeDie = null;
        public static DieType CurType = DieType.Full;
        public string ParentFormName = string.Empty;
        public static ControlDie hightLightDie = null;


        public ControlDie()
        {
            InitializeComponent();
        }

        private string CurName = string.Empty;
        private string CurOrdinate = string.Empty;

        public DieInfo Info;
        public string DieName { get => lbl_Name.Text; set { lbl_Name.Text = value; CurName = value; } }
        public string DieOrdinate { get => lbl_Ordinate.Text; set { lbl_Ordinate.Text = value; CurOrdinate = value; } }


        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value)
                    return;
                _isSelected = value;
                SetSelectedState(value);
            }
        }



        private bool _isActived;
        public bool IsActived
        {
            get
            {
                if (_isActived || _isHalfed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                _isActived = value;
                SetActivedState(value);
            }
        }

        private bool _isHalfed = false;
        public bool IsHalfed
        {
            get { return _isHalfed; }
            set 
            {
                _isHalfed = value;
                SetHalfState(value);
            }
        }       

        private bool _isHome;

        public bool IsHome
        {
            get { return _isHome; }
            set
            {
                if (_isHome == value)
                    return;
                _isHome = value;
                SetHomeState(value);
            }
        }



        public bool IsSelectedState()
        {
            return this.BackColor == Color.Lime;
        }

        public void SetHomeState(bool isHome)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    SetActivedState(isHome);
                }));
            }
            else
            {
                //lbl_Name.Visible = !isHome;
                if (IsHome)
                {
                    //CurName = lbl_Ordinate.Text;
                    //lbl_Ordinate.Text = string.Empty;
                    lbl_Ordinate.Image = global::Prober.Properties.Resources.回到原点;                    
                }
                else
                {
                    //lbl_Ordinate.Text = CurName;
                    lbl_Ordinate.Image = null;
                }
            }
        }

        /// <summary>
        /// 设置die控件为激活状态
        /// </summary>
        /// <param name="isActived"></param>
        public void SetActivedState(bool isActived)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    SetActivedState(isActived);
                }));
            }
            else
            {
                if (!isActived)
                {
                    IsHalfed = false;
                    lbl_Name.Text = string.Empty;
                    lbl_Ordinate.Text = string.Empty;
                    
                    lbl_Name.BackColor = Color.DarkGray;
                    lbl_Ordinate.BackColor = Color.DarkGray;

                }
                else
                {
                    lbl_Name.Text = CurName;
                    lbl_Ordinate.Text = CurOrdinate;

                    lbl_Name.BackColor = Color.FromArgb(111, 168, 220);
                    lbl_Ordinate.BackColor = Color.FromArgb(111, 168, 220);
                }
            }
        }

        public void SetHalfState(bool isHalf)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    SetHalfState(isHalf);
                }));
            }
            else
            {
                if ((!_isHalfed))
                {
                    lbl_Name.Text = string.Empty;
                    lbl_Ordinate.Text = string.Empty;
                }
                else
                {
                    lbl_Name.Text = CurName;
                    lbl_Ordinate.Text = CurOrdinate;

                    lbl_Name.BackColor = Color.MediumSlateBlue;
                    lbl_Ordinate.BackColor = Color.MediumSlateBlue;
                }
            }
        }

        /// <summary>
        /// 设置die控件为选中状态
        /// </summary>
        /// <param name="isSelected"></param>
        public void SetSelectedState(bool isSelected)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    SetSelectedState(isSelected);
                }));
            }
            else
            {
                if (!_isActived && !_isHalfed)
                    return;                

                if (isSelected && this.BackColor == Color.Yellow) {
                    ;
                }
                else {
                    this.BackColor = isSelected ? Color.Lime : Color.FromArgb(111, 168, 220);
                }                
            }
        }
        
        /// <summary>
        /// 设置HighLight状态
        /// </summary>
        /// <param name="isHighLight"></param>
        public void SetHightLightState(bool isHighLight)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    SetHightLightState(isHighLight);
                }));
            }
            else
            {
                if (!_isActived && !_isHalfed)
                    return;

                this.BackColor = isHighLight ? Color.Yellow : Color.FromArgb(111, 168, 220);
            }
        }

        public void SetReferenceState(bool isReference) 
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    SetReferenceState(isReference);
                }));
            }
            else
            {
                if (!_isActived && !_isHalfed)
                    return;

                lbl_Reference.Visible = isReference;
            }
        }

        public bool IsReferenceState()
        {
            return lbl_Reference.Visible;
        }

        public bool IsHighLightState()
        {
            return this.BackColor == Color.Yellow;
        }


        private void lbl_Ordinate_Click(object sender, EventArgs e)
        {
            bool bHighlight = false;
            switch (ControlDie.CurState)
            {
                case DieState.None:
                case DieState.GoToDisable:
                case DieState.GoToEnable:
                    if (!_isActived && !_isHalfed)
                        return;
                    if (ControlDie.PreSelectedDie != null)
                    {                       
                        if (ControlDie.PreSelectedDie.BackColor == Color.Yellow) {
                            bHighlight = true;
                        }

                        ControlDie.PreSelectedDie.IsSelected = false;
                        if (bHighlight) {
                            ControlDie.PreSelectedDie.BackColor = Color.Yellow;
                        }

                        if (ControlDie.CurState == DieState.GoToEnable) {
                            tempDie = ControlDie.PreSelectedDie;
                        }                       
                    }
                        
                    IsSelected = true;
                    ControlDie.PreSelectedDie = this;
                    break;
                case DieState.Home:
                    if (ControlDie.PreHomeDie != null)
                        ControlDie.PreHomeDie.IsHome = false;
                    IsHome = true;
                    PreHomeDie = this;
                    break;
                case DieState.Active:
                    IsActived = true;
                    break;
                case DieState.DeActive:
                    IsActived = false;
                    break;
                case DieState.Halfed:
                    IsHalfed = true;
                    break;
                default:
                    break;
            }

            if (ControlDie.CurState == DieState.GoToEnable)
            {
                //发送消息
                //IntPtr hWnd = FindWindow(null, "Prober");
                IntPtr hWnd = FindWindow(null, ParentFormName);                
                SendMessage(hWnd, WM_GOTO, 0, (IntPtr)0);
            }  
            
/*
            switch (ControlDie.CurType)
            {
                case DieType.Full:
                    this.BackColor = Color.FromArgb(111, 168, 220);
                    break;
                case DieType.Half:
                    this.BackColor = Color.FromArgb(255, 192, 192);
                    break;
                default:
                    break;
            }
*/
        }


        private void lbl_Ordinate_MouseEnter(object sender, EventArgs e)
        {
            if ((!_isActived) && !_isHalfed)
                return;
            if (_isHalfed)
            {
                lbl_Name.BackColor = Color.BlueViolet;
                lbl_Ordinate.BackColor = Color.BlueViolet;
            }
            else
            {
                lbl_Name.BackColor = Color.FromArgb(131, 188, 240);
                lbl_Ordinate.BackColor = Color.FromArgb(131, 188, 240);
            }
        }

        private void lbl_Ordinate_MouseLeave(object sender, EventArgs e)
        {
            if ((!_isActived) && !_isHalfed)
                return;
            if (_isHalfed)
            {
                lbl_Name.BackColor = Color.MediumSlateBlue;
                lbl_Ordinate.BackColor = Color.MediumSlateBlue;
            }
            else
            {
                lbl_Name.BackColor = Color.FromArgb(111, 168, 220);
                lbl_Ordinate.BackColor = Color.FromArgb(111, 168, 220);
            }
            
        }

        public const int WM_GOTO = 0xF112;

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr IParam);


        [DllImport("User32.dll")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

    }

    public enum DieState
    {
        None,
        Home,
        Active,
        Selected,
        DeActive,
        GoToEnable,
        GoToDisable,
        Halfed
    }

    public enum DieType
    {
        Full,
        Half
    }
}
