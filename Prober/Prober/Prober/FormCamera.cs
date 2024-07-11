using MyInstruments.MyCamera;
using MyInstruments.MyElecLens;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Forms;
using ProberApi.MyConstant;
using ProberApi.MyUtility;
using MyInstruments.MyUtility;
using MyInstruments;
using MyInstruments.MyEnum;
using NLog;
using CommonApi.MyUtility;
using System.Linq;
using MyInstruments.MyOpm;
using MyInstruments.MySmu;
using MyMotionStageDriver.MyStageAxis;

namespace Prober {
    public partial class FormCamera : Form {
        private StandaloneCamera cameraTop;
        private StandaloneCamera cameraFront;
        private StandaloneCamera cameraRear;
        private StandaloneElecLens eLens ;
        List<string> zoomList = new List<string>();

        private readonly Dictionary<string, Instrument> instruments;
        private readonly List<InstrumentUsage> instrumentUsageList;
        private readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        public Action<string> ReportMessage;

        public FormCamera(ConcurrentDictionary<string, object> sharedObjects){ 

            InitializeComponent();

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out object tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            //获取仪表
            //var getResult2 = GetInstrument("top_camera");   
            //var getResult4 = GetInstrument("front_camera");            
            //var getResult6 = GetInstrument("elens_zoom");
            var getResult8 = GetInstrument("rear_camera");
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
                    if (instrumentUsage.InstrumentId == "ccd_top")
                    {
                        cameraTop = instrument as StandaloneCamera;
                        cameraTop.SetWidth(4504);
                        cameraTop.SetHeight(4096);
                    }
                    else if (instrumentUsage.InstrumentId == "ccd_front")
                    {
                        cameraFront = instrument as StandaloneCamera;
                        cameraFront.SetWidth(2448);
                        cameraFront.SetHeight(2048);
                    }
                    else if (instrumentUsage.InstrumentId == "ccd_rear")
                    {
                        cameraRear = instrument as StandaloneCamera;
                        cameraRear.SetWidth(2448);
                        cameraRear.SetHeight(2048);
                    }
                    else
                    {
                        errorText = $"{instrumentUsage.InstrumentCategory.ToString()} is not a valid instrument category of coupling feedback!";
                        LOGGER.Error(errorText);
                        return (false, errorText, null);
                    }
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

        public void InitZoomList()
        {
            /*
            zoomList.Add("0.7");
            zoomList.Add("1.0");
            zoomList.Add("1.5");
            zoomList.Add("2.0");
            zoomList.Add("2.5");
            zoomList.Add("3.0");
            zoomList.Add("3.5");
            zoomList.Add("4.0");
            zoomList.Add("4.5");
            */
            zoomList.Clear();
            zoomList.AddRange(Constant.ZoomTable.zoomToValueDic.Keys);
        }

        private void FrmCamera_Load(object sender, EventArgs e)
        {
            try
            {
                InitZoomList();
                //hw_Top.ReportMessage = this.ReportMessage;
                //hw_Top.InitCamera(cameraTop,"Top",false);
                //hw_Top.MaxMinClick = TopCameraMaxMinClick;
                //hw_Top.ZoomSelect = TopCameraZoom;
                //hw_Top.SetZoomList(zoomList);
                //hw_Top.SetPixLenCoef(1.2); //待确认

                //hw_Front.ReportMessage = this.ReportMessage;
                //hw_Front.InitCamera(cameraFront, "Front",false);
                //hw_Front.MaxMinClick = SideCameraMaxMinClick;
                //hw_Front.SetPixLenCoef(1.2); //待确认
                                             //
                hw_Rear.ReportMessage = this.ReportMessage;
                hw_Rear.InitCamera(cameraRear, "Rear",false);
                hw_Rear.MaxMinClick = SideCameraMaxMinClick;
                hw_Rear.SetPixLenCoef(1.2); //待确认   
            }
            catch (Exception ex)
            {
                MessageBox.Show($"相机界面加载失败:{ex.Message}");
            }
        }

        Control single = null;
        public void DealWithMaxMin(int row, int col, bool setMax)
        {
            if (setMax)
            {
                var control = tableLayoutPanel_Camera.GetControlFromPosition(col, row);
                tableLayoutPanel_Camera.Visible = false;
                control.Parent = panel_Camera;
                single = control;
            }
            else
            {
                tableLayoutPanel_Camera.Visible = true;
                single.Parent = tableLayoutPanel_Camera;
                tableLayoutPanel_Camera.SetCellPosition(single, new TableLayoutPanelCellPosition(col,row));
            }
        }

        Control singleSide = null;
        public void DealWithMaxMinSide(int row, int col, bool setMax) {
            if (setMax) {
                var control = tableLayoutPanel_Camera.GetControlFromPosition(col, row);
                tableLayoutPanel_Camera.Visible = false;
                control.Parent = panel_Camera;
                singleSide = control;
            } else {
                tableLayoutPanel_Camera.Visible = true;
                singleSide.Parent = tableLayoutPanel_Camera;
                tableLayoutPanel_Camera.SetCellPosition(singleSide, new TableLayoutPanelCellPosition(col, row));
            }
        }

        public void DealWithZoom(int index, string zoom)
        {
            if (index == 0)
            {
                if (!eLens.SetZoom(Constant.ZoomTable.zoomToValueDic[zoom]))
                {
                    MessageBox.Show("相机放大倍数设置失败");
                }
            }
        }        

        public void TopCameraZoom(int zoomIndex)
        {
            try
            {
                DealWithZoom(0, zoomList[zoomIndex]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"相机放大倍数设置异常:{ex.Message}");
            }
        }

        public void TopCameraMaxMinClick(bool isMin)
        {
            if (isMin)
            {
                DealWithMaxMin(0, 0, true);
            }
            else
            {
                DealWithMaxMin(0, 0, false);
            }
        }

        public void SideCameraMaxMinClick(bool isMin) {
            if (isMin) {
                DealWithMaxMinSide(0, 1, true);
            } else {
                DealWithMaxMinSide(0, 1, false);
            }
        }

    }
}
