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
using NLog;
using ProberApi.MyUtility;
using MyMotionStageDriver.MyStageAxis;
using MyMotionStageUserControl;
using CommonApi.MyUtility;
using MyInstruments.MyEnum;
using MyInstruments.MyUtility;
using MyInstruments;
using MyInstruments.MyCamera;
using MyInstruments.MyElecLens;
using ProberApi.MyConstant;
using Prober.Constant;
using HalconDotNet;
using Prober.WaferDef;
using System.Threading;
using System.Dynamic;

namespace Prober.Forms
{
    public partial class FormVisionMotionCalibration : Form
    {
        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        protected static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);

        private readonly List<InstrumentUsage> instrumentUsageList;
        private readonly Dictionary<string, Instrument> instruments;

        private StandaloneCamera camera;
        private StandaloneElecLens eLens;
        private readonly Dictionary<string, StageAxis> stageAxisDic = new Dictionary<string, StageAxis>();

        private HTuple _modelID;
        private HTuple _FRow;
        private HTuple _FColumn;

        State MotionState = State.Ready;
        public PlatCalibrate VisionMotionCalibrate { get; set; } = new PlatCalibrate("VisionMotion");

        public FormVisionMotionCalibration(ConcurrentDictionary<string, object> sharedObjects)
        {
            InitializeComponent();

            sharedObjects.TryGetValue(SharedObjectKey.STAGE_AXIS_USAGE_DICT, out object tempObj);
            stageAxisUsages = tempObj as Dictionary<string, StageAxis>;

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            //获取仪表
            var getResult2 = GetInstrument("top_camera");
            //var getResult4 = GetInstrument("elens_zoom"); 
            
            //获取轴
            GetStageAxisDic();
        }

        public bool GetStageAxisDic()
        {
            List<string> axisUseList = new List<string>();
            axisUseList.Add(MyStageAxisKey.LEFT_X);
            axisUseList.Add(MyStageAxisKey.LEFT_Y);
            axisUseList.Add(MyStageAxisKey.LEFT_Z);
            axisUseList.Add(MyStageAxisKey.LEFT_SX);
            axisUseList.Add(MyStageAxisKey.LEFT_SY);
            axisUseList.Add(MyStageAxisKey.LEFT_SZ);

            axisUseList.Add(MyStageAxisKey.RIGHT_X);
            axisUseList.Add(MyStageAxisKey.RIGHT_Y);
            axisUseList.Add(MyStageAxisKey.RIGHT_Z);
            axisUseList.Add(MyStageAxisKey.RIGHT_SX);
            axisUseList.Add(MyStageAxisKey.RIGHT_SY);
            axisUseList.Add(MyStageAxisKey.RIGHT_SZ);

            axisUseList.Add(MyStageAxisKey.CCD_X);
            axisUseList.Add(MyStageAxisKey.CCD_Y);
            axisUseList.Add(MyStageAxisKey.CCD_Z);
            axisUseList.Add(MyStageAxisKey.HEIGHT_U);

            axisUseList.Add(MyStageAxisKey.CHUCK_X);
            axisUseList.Add(MyStageAxisKey.CHUCK_Y);
            axisUseList.Add(MyStageAxisKey.CHUCK_Z);
            axisUseList.Add(MyStageAxisKey.CHUCK_SZ);

            for (int i = 0; i < axisUseList.Count; i++)
            {
                var result = GetStageAxis(axisUseList[i]);
                if (!result.isOK)
                {
                    return false;
                }
                stageAxisDic.Add(axisUseList[i], result.stageAxis);
            }

            return true;
        }

        public (bool isOk, string errorText, Instrument instrument) GetInstrument(string instrumentUsageId)
        {
            string errorText = string.Empty;
            var list = this.instrumentUsageList.Where(x => x.UsageId.Equals(instrumentUsageId)).ToList();
            InstrumentUsage instrumentUsage = list.First();
            if (list == null)
            {
                errorText = $"GetInstrumentUsage(={instrumentUsageId}) does not exist!";
                LOGGER.Error(errorText);
                return (false, errorText, null);
            }

            Instrument instrument = instruments[instrumentUsage.InstrumentId];
            switch (instrumentUsage.InstrumentCategory)
            {
                case EnumInstrumentCategory.CCD:
                    camera = instrument as StandaloneCamera;
                    break;
                case EnumInstrumentCategory.ELENS:
                    eLens = instrument as StandaloneElecLens;
                    break;
                default:
                    errorText = $"{instrumentUsage.InstrumentCategory.ToString()} is not a valid instrument category of coupling feedback!";
                    LOGGER.Error(errorText);
                    return (false, errorText, null);
            }

            return (true, string.Empty, instrument);
        }


        internal (bool isOK, string errorMessage, StageAxis stageAxis) GetStageAxis(string axisUsageId)
        {
            string errorText = string.Empty;
            string INVALID_PARAMETERS = $"input parameters(={axisUsageId}) is invalid";

            if (!stageAxisUsages.ContainsKey(axisUsageId))
            {
                errorText = $"{INVALID_PARAMETERS}";
                LOGGER.Error(errorText);
                return (false, errorText, null);
            }

            return (true, null, stageAxisUsages[axisUsageId]);
        }        

        private void btn_VisionMotion_Select_Click(object sender, EventArgs e)
        {
            try
            {
                if (!camera.SignlShot(out HObject image))
                {
                    MessageBox.Show(this, "拍图失败！", "提示：");
                    return;
                }

                hw_VisionMotion.HalconWindow.ClearWindow();
                hw_VisionMotion.Focus();
                image.DispObj(hw_VisionMotion.HalconWindow);
                if (!VisionMgr.SelectMark(hw_VisionMotion.HalconWindow, image, out _modelID, out _FRow, out _FColumn))
                {
                    MessageBox.Show(this, "选取mark特征失败！", "提示：");
                    return;
                }

                //移动到相机中央,此处需要确认下方向
                /*
                double desRow = camera.Height / 2;
                double desCol = camera.Width / 2;
                double disY = -(desRow - _FRow) * 1.2;
                double disX = (desCol - _FColumn) * 1.2;
                Dim2MoveRel(MyStageAxisKey.CCD_X, MyStageAxisKey.CCD_Y, disX, disY);
                */
                MessageBox.Show(this, "选取mark特征完成！", "提示：");
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, $"选取mark特征异常:{ex.Message}！", "警告：");
            }            
        }

        public void Dim2MoveAbs(string axisX, string axisY, double disX, double disY)
        {
            var t1 = Task.Run(() =>
            {
                stageAxisDic[axisX].MoveAbsolute(disX);
            });
            var t2 = Task.Run(() =>
            {
                stageAxisDic[axisY].MoveAbsolute(disY);
            });
            Task.WaitAll(new Task[] { t1, t2 });

            stageAxisDic[axisX].GuiUpdatePosition();
            stageAxisDic[axisY].GuiUpdatePosition();

            return;
        }

        public void Dim2MoveRel(string axisX, string axisY, double disX, double disY)
        {
            var t1 = Task.Run(() =>
            {
                stageAxisDic[axisX].MoveRelative(disX);
            });
            var t2 = Task.Run(() =>
            {
                stageAxisDic[axisY].MoveRelative(disY);
            });
            Task.WaitAll(new Task[] { t1, t2 });

            return;
        }

        private void btn_VisionMotion_Calibrate_Click(object sender, EventArgs e)
        {
            if (MotionState != State.Ready)
            {
                MessageBox.Show(this, EValue.NOREADYINFO, "提示：");
                return;
            }

            double ChuckX = stageAxisDic[MyStageAxisKey.CHUCK_X].Position();
            double ChuckY = stageAxisDic[MyStageAxisKey.CHUCK_Y].Position();
            double step = 400;
            List<double[]> posList = new List<double[]>();
            posList.Add(new double[] { ChuckX + step, ChuckY - step });
            posList.Add(new double[] { ChuckX - step, ChuckY - step });
            posList.Add(new double[] { ChuckX - step, ChuckY + step });
            posList.Add(new double[] { ChuckX + step, ChuckY + step });

            Task.Run(() =>
            {
                try
                {
                    MotionState = State.Busy;

                    //四点定位
                    for (int i = 0; i < posList.Count; i++)
                    {
                        Dim2MoveAbs(MyStageAxisKey.CHUCK_X, MyStageAxisKey.CHUCK_Y, posList[i][0], posList[i][1]);

                        Thread.Sleep(1000);
                        camera.SignlShot(out HObject image);
                        if (!VisionMgr.FindPlatMark(image, _modelID, out double row, out double col))
                        {
                            Invoke(new Action(() =>
                            {
                                MessageBox.Show(this, "校准失败！搜索Mark点失败。", "警告：");
                            }));
                            return;
                        }
                        Invoke(new Action(() =>
                        {
                            panel_VisionMotion.Controls[$"txt_VisionMotion_Row_{i + 1}"].Text = row.ToString("f2");
                            panel_VisionMotion.Controls[$"txt_VisionMotion_Column_{i + 1}"].Text = col.ToString("f2");
                            panel_VisionMotion.Controls[$"txt_VisionMotion_ChuckX_{i + 1}"].Text = posList[i][0].ToString("f2");
                            panel_VisionMotion.Controls[$"txt_VisionMotion_ChuckY_{i + 1}"].Text = posList[i][1].ToString("f2");
                        }));
                    }

                    //图像移动到原点
                    Dim2MoveAbs(MyStageAxisKey.CHUCK_X, MyStageAxisKey.CHUCK_Y, ChuckX, ChuckY);

                    Invoke(new Action(() =>
                    {
                        MessageBox.Show(this, "校准完成！", "提示：");
                    }));
                }
                catch(Exception ex)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show(this, $"校准过程异常:{ex.Message}！", "警告：");
                    }));
                }
                finally
                {
                    MotionState = State.Ready;
                }
            });
        }

        private void FormVisionMotionCalibration_Load(object sender, EventArgs e) {
            PlatInfo plat = ConfigMgr.LoadVisionMotionPlatInfo();
            if (plat != null) {
                for (int i = 0; i < plat.Px.Length; i++) {
                    panel_VisionMotion.Controls[$"txt_VisionMotion_Row_{i + 1}"].Text = plat.Px[i].ToString();
                    panel_VisionMotion.Controls[$"txt_VisionMotion_Column_{i + 1}"].Text = plat.Py[i].ToString();
                    panel_VisionMotion.Controls[$"txt_VisionMotion_ChuckX_{i + 1}"].Text = plat.Qx[i].ToString();
                    panel_VisionMotion.Controls[$"txt_VisionMotion_ChuckY_{i + 1}"].Text = plat.Qy[i].ToString();
                }
            }
        }


        private void btn_VisionMotion_Save_Click(object sender, EventArgs e)
        {
            PlatInfo plat = new PlatInfo(4);
            for (int i = 0; i < plat.Px.Length; i++)
            {
                plat.Px[i] = Convert.ToDouble(panel_VisionMotion.Controls[$"txt_VisionMotion_Row_{i + 1}"].Text);
                plat.Py[i] = Convert.ToDouble(panel_VisionMotion.Controls[$"txt_VisionMotion_Column_{i + 1}"].Text);
                plat.Qx[i] = Convert.ToDouble(panel_VisionMotion.Controls[$"txt_VisionMotion_ChuckX_{i + 1}"].Text);
                plat.Qy[i] = Convert.ToDouble(panel_VisionMotion.Controls[$"txt_VisionMotion_ChuckY_{i + 1}"].Text);
            }

            if (!ConfigMgr.SaveVisionMotionPlatInfo(plat))
            {
                MessageBox.Show(this, "保存失败！", "提示：");
                return;
            }
            else
            {
                MessageBox.Show(this, "保存成功！", "提示：");
            }

            //*****确认下这个函数，感觉是不需要放在这里的*********//
            VisionMotionCalibrate.GeneratePlatCalibration(plat); 
        }
    }
}
