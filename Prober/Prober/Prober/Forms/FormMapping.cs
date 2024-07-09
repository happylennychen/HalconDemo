using CommonApi.MyUtility;
using HalconDotNet;
using MyInstruments;
using MyInstruments.MyCamera;
using MyInstruments.MyElecLens;
using MyInstruments.MyEnum;
using MyInstruments.MyUtility;
using MyMotionStageDriver.MyStageAxis;
using NLog;
using Prober.Constant;
using Prober.WaferDef;
using ProberApi.MyConstant;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Prober.Forms {
    public partial class FormMapping : Form
    {
        public Dictionary<string, double[]> SubDiePosition;
        State MotionState = State.Ready;
        bool IsWaferLoad = false;
        public WaferManual waferHandle = null;
        public HCalibrationInfo HeightCalibrationInfo = new HCalibrationInfo();

        private readonly Dictionary<string, StageAxis> stageAxisUsages;
        private readonly Dictionary<string, Instrument> instruments;
        private readonly List<InstrumentUsage> instrumentUsageList;        
        private readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        private readonly Dictionary<string, StageAxis> stageAxisDic = new Dictionary<string, StageAxis>();

        public PlatCalibrate VisionMotionCalibrate { get; set; } = new PlatCalibrate("VisionMotion");
        public PlatCalibrate WaferMotionPlat { get; set; } = new PlatCalibrate("WaferMotion");

        //轴的运动控制
        //相机控制
        //Chuck吸附//三色灯//IO控制
        private StandaloneCamera camera;
        private StandaloneElecLens eLens;

        public Action UpdateWaferTypeList;

        public FormMapping(ConcurrentDictionary<string, object> sharedObjects)
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
            var getResult4 = GetInstrument("elens_zoom");

            //获取轴
            GetStageAxisDic();

            waferHandle = new WaferManual(sharedObjects);
            waferHandle.ReportMessage = this.ReportMessage;
            waferHandle.UpdateWaferMap = t => { UIClass.ObjectToControl(t, pn_WaferInfo); };
            waferHandle.SetSelectedDie = ctlWafer_Main.SetSelectedDie;
            waferHandle.SetSelectedDieIndexByHome = ctlWafer_Main.SetSelectedDieIndexByHome;
            waferHandle.SetDieHighLight = ctlWafer_Main.SetDieHighLight;            
            waferHandle.SetDieHighLightWithIndex = ctlWafer_Main.SetDieHighLightWithIndex;
            waferHandle.SetDieHighLightWithIndexByHome = ctlWafer_Main.SetDieHighLightWithIndexByHome;

            SetControlState(IsWaferLoad);
            ctlWafer_Main.ChangeMode(true);
            ctlWafer_Main.ParentFormName = this.Text;
            ctlWafer_Main.MoveToMarkPos = waferHandle.MoveToMarkPos;
            ctlWafer_Main.UpdateMapInfo = t => { UIClass.ObjectToControl(t, pn_WaferInfo); };        
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
                    ReportMessage(errorText);
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

        private void btn_CreateWaferType_Click(object sender, EventArgs e)
        {
            if (cbox_WaferType.Text.Trim() == string.Empty)
            {
                ReportMessage("晶圆类型不能为空");
                MessageBox.Show(this, "晶圆类型不能为空。", "Info:");                
                return;
            }

            if (ConfigMgr.IsWaferTypeExist(cbox_WaferType.Text))
            {
                ReportMessage($"晶圆类型{cbox_WaferType.Text}已经存在");
                MessageBox.Show(this, "晶圆类型已经存在", "Info:");                
                return;
            }

            WaferMapInfo newMap = new WaferMapInfo();
            newMap.Type = cbox_WaferType.Text;
            ConfigMgr.SaveWaferMapInfobyType(newMap);

            cbox_WaferType.Items.Clear();
            cbox_WaferType.Items.AddRange(ConfigMgr.GetWaferMapNames());

            UpdateWaferInfo(newMap);
            if (SubDiePosition != null)
            {
                SubDiePosition.Clear();
            }

            //更新类型列表
            UpdateWaferTypeList();

            ReportMessage($"创建晶圆类型{cbox_WaferType.Text}成功");
            MessageBox.Show(this, "创建成功。", "Info:");            
        }

        private void UpdateWaferInfo(WaferMapInfo newMap, bool isUpdateUI = true)
        {
            Invoke(new Action(() =>
            {
                UIClass.ObjectToControl(newMap, pn_WaferInfo);
                //c00519410 此处屏蔽了更新DUT晶圆位置坐标信息start
                //dgv_WaferPositions.DataSource = newMap.SubDies;
                //c00519410 此处屏蔽了更新DUT晶圆位置坐标信息end
                if (isUpdateUI)
                {
                    ctlWafer_Main.GenerateMap(newMap);
                }
            }));

        }

        private void btn_DeleteWaferType_Click(object sender, EventArgs e)
        {
            if (cbox_WaferType.Text == string.Empty)
            {
                ReportMessage("请选择晶圆类型");
                MessageBox.Show(this, "请选择晶圆类型。", "Info:");                
                return;
            }

            if (MessageBox.Show("确定要删除该晶圆类型吗？", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                ReportMessage($"用户取消删除晶圆类型{cbox_WaferType.Text}");
                return;
            }

            if (!ConfigMgr.IsWaferTypeExist(cbox_WaferType.Text))
            {
                ReportMessage($"晶圆类型{cbox_WaferType.Text}不存在");
                MessageBox.Show(this, $"晶圆类型{cbox_WaferType.Text}不存在", "Info:");                
                return;
            }

            ConfigMgr.DeleteWaferMapByType(cbox_WaferType.Text);

            cbox_WaferType.Text = string.Empty;
            cbox_WaferType.Items.Clear();
            cbox_WaferType.Items.AddRange(ConfigMgr.GetWaferMapNames());

            //用一个空的信息填充界面
            WaferMapInfo tempMap = new WaferMapInfo();
            UpdateWaferInfo(tempMap);

            //更新类型列表
            UpdateWaferTypeList();

            ReportMessage($"删除晶圆类型{cbox_WaferType.Text}成功");            
        }

        private void cbox_WaferType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var map = ConfigMgr.LoadWaferMapInfoByType(cbox_WaferType.Text);
            if (map == null)
            {
                ReportMessage($"晶圆类型{cbox_WaferType.Text}Map图不存在");
                MessageBox.Show(this, $"晶圆类型{cbox_WaferType.Text}Map图不存在", "Info:");                
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            UpdateWaferInfo(map);
            this.Cursor = Cursors.Default;
        }

        private void btn_Upload_Click(object sender, EventArgs e)
        {
            bool isWaferInfoValid = false;

            //1:提示探针抬起
            if (MessageBox.Show("确定要运动上料位置吗？如果有探针，请将探针抬起到安全高度！！！", "提示：", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                ReportMessage("上料操作检查失败:用户取消");
                return ;
            }

            //2：上料条件检查
            if (!waferHandle.BeforeUploadCheck(cbox_WaferType.Text,out isWaferInfoValid,out string errMsg))
            {
                MessageBox.Show(errMsg);
                return;
            }

            //3：上料任务
            var map = ConfigMgr.LoadWaferMapInfoByType(cbox_WaferType.Text);
            Task.Run(() =>
            {
                try
                {
                    MotionState = State.Busy;

                    if (waferHandle.AutoUploadWafer(map, isWaferInfoValid, true, out  errMsg))
                    {
                        Invoke(new Action(() =>
                        {
                            cbox_WaferType.Enabled = false;
                            btn_CreateWaferType.Enabled = false;
                            btn_DeleteWaferType.Enabled = false;
                            IsWaferLoad = true;
                            SetControlState(IsWaferLoad);
                            ConfigMgr.SaveWaferMapInfobyType(map);
                            UpdateWaferInfo(map);
                            if (isWaferInfoValid) {
                                ctlWafer_Main.SetSelectedDie(map.MarkDieRowIndex, map.MarkDieColumnIndex);
                                ctlWafer_Main.SetDieHighLightWithIndex(map.MarkDieRowIndex,map.MarkDieColumnIndex);
                            }                            
                            ReportMessage("上料完成");
                            MessageBox.Show(this, "上料完成。", "提示：");
                        }));
                    }
                    else
                    {
                        ReportMessage($"上料失败:{errMsg}");
                        Invoke(new Action(() => { MessageBox.Show(this, $"上料失败:{errMsg}", "提示："); }));
                    }                   
                }
                catch(Exception ex)
                {
                    Invoke(new Action(() =>
                    {
                        ReportMessage($"上料异常:{ex.Message}");
                        MessageBox.Show($"上料异常:{ex.Message}");
                    }));                    
                    return;
                }
                finally
                {
                    MotionState = State.Ready;
                }                                             
            });
        }        

        public void FindMark(HObject image, HTuple id, out double row, out double col,bool throwException = true) 
        {
            row = 0;
            col = 0;
            try 
            {
                VisionMgr.FindPlatMark(image, id, out row, out col);                 
            }
            catch (Exception ex){
                if(throwException) {
                    throw ex;                    
                }               
            }
        }                
                    

        private void btn_Download_Click(object sender, EventArgs e)
        {
            if (!BeforeDownloadCheck())
            {
                return ;
            }

            EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
            if (info == null)
            {
                ReportMessage("下料前检查失败:加载校准文件失败");
                MessageBox.Show(this, "加载校准文件失败！", "提示：");
                return ;
            }

            Task.Run(() =>
            {
                try
                {
                    MotionState = State.Busy;
                    Invoke(new Action(() => { ReportMessage("开始运动到下料位置"); }));
                    IsWaferLoad = false;

                    if (waferHandle.MoveToRemovePosition(info,out string errMsg))
                    {
                        Invoke(new Action(() =>
                        {
                            cbox_WaferType.Enabled = true;
                            btn_CreateWaferType.Enabled = true;
                            btn_DeleteWaferType.Enabled = true;
                            if (SubDiePosition != null)
                            {
                                SubDiePosition.Clear();
                            }

                            SetControlState(IsWaferLoad);

                            ReportMessage("运动到下料位置完成");
                            MessageBox.Show(this, "运动到下料位置完成。", "提示：");
                        }));
                    }   
                    else
                    {
                        Invoke(new Action(() =>
                        {
                            ReportMessage($"运动到下料位置失败:{errMsg}");
                            MessageBox.Show(this, $"运动到下料位置失败:{errMsg}。", "提示："); 
                        }));
                    }
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() =>
                    {
                        ReportMessage($"下料异常:{ex.Message}");
                        MessageBox.Show($"下料异常:{ex.Message}");
                    }));                  
                    return;
                }
                finally
                {
                    MotionState = State.Ready;
                }
            });
        }

        public bool BeforeDownloadCheck()
        {           
            if (MotionState != State.Ready) 
            {
                ReportMessage($"下料前检查失败:{EValue.NOREADYINFO}");
                MessageBox.Show(this, EValue.NOREADYINFO, "提示：");                
                return false;
            }

            if (MessageBox.Show("确定要运动下料位置吗？", "提示：", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                ReportMessage("下料前检查失败:用户取消");
                return false;
            }

            return true;
        }

        private void btn_SelectMark_Click(object sender, EventArgs e)
        {
            if (MotionState != State.Ready)
            {
                ReportMessage($"Mark点选取检查失败:{EValue.NOREADYINFO}");
                MessageBox.Show(this, EValue.NOREADYINFO, "提示：");                
                return;
            }

            //1:获取晶圆类型
            var map = ConfigMgr.LoadWaferMapInfoByType(cbox_WaferType.Text);
            if (map == null)
            {
                ReportMessage($"不存在该晶圆类型:{cbox_WaferType.Text}");
                MessageBox.Show(this, "不存在该晶圆类型。", "Info:");
                return;
            }

            Task.Run(() => {
                try {
                    MotionState = State.Busy;

                    //2:加载机台校准文件
                    EquipmentCalibrationInfo info = ConfigMgr.LoadEquipmentCalibration();
                    if (info == null) {
                        ReportMessage($"加载校准文件失败");
                        Invoke(new Action(()=>{ MessageBox.Show(this, "加载校准文件失败！", "提示："); }));
                        return;
                    }

                    //3:相机移动到基准位置
                    MoveCCDToBasePos(info.Base_CcdX, info.Base_CcdY, info.Base_CcdZ);

                    //4:自动聚焦并拍照
                    Invoke(new Action(() => { this.Cursor = Cursors.WaitCursor; }));
                    AutoFocus(50);
                    camera.SignlShot(out HObject image);
                    Invoke(new Action(() => { this.Cursor = Cursors.Default; }));

                    //5:弹出Mask选择对话框
                    FormSelectWaferMark frm = new FormSelectWaferMark(image);
                    if (frm.ShowDialog() != DialogResult.Yes) {
                        Invoke(new Action(() =>
                        {
                            ReportMessage("用户未正常保存Mark点");
                            MessageBox.Show(this, "用户未正常保存Mark点", "提示：");
                        }));
                        return;
                    }

                    //6:保存模板信息，并显示当前位置到界面上
                    if (frm.IsFinished) {                        
                        Invoke(new Action(() => {
                            UpdateMarkInfo(map, frm);
                            ReportMessage("Mark点信息保存成功");
                            MessageBox.Show(this, "Mark点信息保存成功", "提示：");
                        }));
                    }
                } catch(Exception ex) {                        
                    Invoke(new Action(() => 
                    {
                        ReportMessage($"Mark点选择异常:{ex.Message}");
                        MessageBox.Show(this, $"Mark点选择异常:{ex.Message}", "提示："); 
                    }));
                } finally {
                    MotionState = State.Ready;
                }
            });            
        }
        

        public void UpdateMarkInfo(WaferMapInfo map,FormSelectWaferMark frm)
        {
            map.MarkRow = Math.Round(frm.row.D, 2);
            map.MarkColumn = Math.Round(frm.col.D, 2);

            HOperatorSet.WriteShapeModel(frm.id, $"Configuration\\Wafer\\{map.Type}.shm");
           
            map.MarkChuckX = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_X].Position(),2);
            map.MarkChuckY = Math.Round(stageAxisDic[MyStageAxisKey.CHUCK_Y].Position(),2);
            map.MarkCcdX = Math.Round(stageAxisDic[MyStageAxisKey.CCD_X].Position(),2);
            map.MarkCcdY = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Y].Position(), 2);
            map.MarkCcdZ = Math.Round(stageAxisDic[MyStageAxisKey.CCD_Z].Position(), 2);

            map.MarkDeltaChuckX = 0;
            map.MarkDeltaChuckY = 0;
            camera.GetBaseParameters(out double ex, out double gain, out double gama);
            map.MarkExposure = ex;

            //c00519410
            if (!eLens.GetZoom(out double zoomValue))
            {
                ReportMessage("获取相机放大倍数失败");
                MessageBox.Show(this, "获取相机放大倍数失败", "提示：");
                return;
            }
            map.MarkZoom = Math.Round(zoomValue, 0);

            UIClass.ObjectToControl(map, pn_WaferInfo);
            ConfigMgr.SaveWaferMapInfobyType(map);
        }

        public void AutoFocus(double step)
        {
            int dir = 1;
            HObject image = new HObject();
            double preShape = 0;
            double curShape = 0;

            for (int i = 0; i < 20; i++)
            {
                image.Dispose();
                Thread.Sleep(500);
                camera.SignlShot(out image);
                curShape = VisionMgr.GetImageShape(image);
                if (preShape == 0)
                    preShape = curShape;

                if (curShape < preShape)
                {
                    //result.stageAxis.MoveRelative(-dir * step);
                    stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(-dir * step);
                    stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

                    dir = -dir;
                    if (dir == 1 || (dir == -1 && i > 2))
                        break;
                }
                else
                {
                    preShape = curShape;
                }
                stageAxisDic[MyStageAxisKey.CCD_Z].MoveRelative(dir * step);
                stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();
            }
        }

        public void MoveChuckXYAbs(double xPos, double yPos) 
        {
            var t1 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.CHUCK_X].MoveAbsolute(xPos);
            });

            var t2 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveAbsolute(yPos);
            });

            Task.WaitAll(new Task[] { t1, t2 });
            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
        }

        public void MoveChuckXYRel(double xPos, double yPos) {
            var t1 = Task.Run(() => {
                stageAxisDic[MyStageAxisKey.CHUCK_X].MoveRelative(xPos);
            });

            var t2 = Task.Run(() => {
                stageAxisDic[MyStageAxisKey.CHUCK_Y].MoveRelative(yPos);
            });

            Task.WaitAll(new Task[] { t1, t2 });
            stageAxisDic[MyStageAxisKey.CHUCK_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CHUCK_Y].GuiUpdatePosition();
        }

        public bool MoveCCDToBasePos(double PosX, double PosY,double PosZ)
        {           
            var t1 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.CCD_X].MoveAbsolute(PosX);
            });
            var t2 = Task.Run(() =>
            {
                stageAxisDic[MyStageAxisKey.CCD_Y].MoveAbsolute(PosY);
            });
            Task.WaitAll(new Task[] { t1, t2 });
            stageAxisDic[MyStageAxisKey.CCD_X].GuiUpdatePosition();
            stageAxisDic[MyStageAxisKey.CCD_Y].GuiUpdatePosition();

            stageAxisDic[MyStageAxisKey.CCD_Z].MoveAbsolute(PosZ);
            stageAxisDic[MyStageAxisKey.CCD_Z].GuiUpdatePosition();

            return true;
        }

        private void btn_MoveToMark_Click(object sender, EventArgs e)
        {
            if (MotionState != State.Ready)
            {
                ReportMessage("AutoMap前检查失败：设备未准备好");
                MessageBox.Show(this, "设备未准备好。", "Info:");
                return;
            }

            if (MessageBox.Show("确定要运动到Mark点位置吗？", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                ReportMessage("运动到Mark点失败:用户取消");
                return;
            }

            var map = ConfigMgr.LoadWaferMapInfoByType(cbox_WaferType.Text);
            Task.Run(() =>
            {
                try
                {
                    MotionState = State.Busy;
                    bool bRet = waferHandle.MoveToMarkPosByWaferType(map,out string errMsg);

                    if (bRet)
                    {                        
                        ctlWafer_Main.SetSelectedDie(map.MarkDieRowIndex, map.MarkDieColumnIndex);
                        ctlWafer_Main.SetDieHighLightWithIndex(map.MarkDieRowIndex, map.MarkDieColumnIndex);
                        Invoke(new Action(() =>
                        {
                            ReportMessage("运动到Mark点完成");
                            MessageBox.Show(this, "运动到Mark点完成。", "Info:");
                        }));
                    }
                    else
                    {
                        Invoke(new Action(() =>
                        {
                            ReportMessage($"运动到Mark点失败:{errMsg}");
                            MessageBox.Show(this, $"运动到Mark点失败:{errMsg}", "Info:");
                        }));
                    }
                }
                catch(Exception ex)
                {                    
                    Invoke(new Action(() =>
                    {
                        ReportMessage($"运动到Mark点异常:{ex.Message}");
                        MessageBox.Show(this, $"运动到Mark点异常:{ex.Message}", "Info:");
                    }));
                    return;
                }
                finally
                {
                    MotionState = State.Ready;
                }
            });
        }

        public bool IsAltimeterInSafePosition(double safePos)
        {
            double curU = stageAxisDic[MyStageAxisKey.HEIGHT_U].Position();
            if (curU >= safePos && curU != 0)
            {
                return true;
            }
            return false;
        }                                             

        /// <summary>
        /// 点击上料后，才使能auto map、选取Mark点这些功能
        /// </summary>
        /// <param name="bActive"></param>
        private void SetControlState(bool bActive)
        {
            if(bActive)
            {
                btn_SelectMark.Enabled = true;
                btn_MoveToMark.Enabled = true;
                btn_SetRef1.Enabled = true;
                btn_SetRef2.Enabled = true;
                btn_SetRef3.Enabled = true;
                btn_SetRef4.Enabled = true;
                btn_ConfirmRefDie.Enabled = true;
                btn_AutoMap.Enabled = true;
                btn_StopMap.Enabled = false;
            }
            else
            {
                btn_SelectMark.Enabled = true;
                btn_MoveToMark.Enabled = false;
                btn_SetRef1.Enabled = false;
                btn_SetRef2.Enabled = false;
                btn_SetRef3.Enabled = false;
                btn_SetRef4.Enabled = false;
                btn_ConfirmRefDie.Enabled = false;
                btn_AutoMap.Enabled = false;
                btn_StopMap.Enabled = false;
            }
        }

        private void btn_SetRef1_Click(object sender, EventArgs e)
        {
            SelectRefDie(1);
        }

        private void btn_SetRef4_Click(object sender, EventArgs e)
        {
            SelectRefDie(4);
        }

        private void btn_SetRef2_Click(object sender, EventArgs e)
        {
            SelectRefDie(2);
        }

        private void btn_SetRef3_Click(object sender, EventArgs e)
        {
            SelectRefDie(3);
        }

        private void SelectRefDie(int dieIndex)
        {
            try
            {
                var die = ctlWafer_Main.GetSelectedDie();
                if (die == null)
                {
                    ReportMessage("设置基准Die失败:未选择Die");
                    MessageBox.Show(this, "请选择一个Die。", "Info:");
                    return;
                }

                var map = ConfigMgr.LoadWaferMapInfoByType(cbox_WaferType.Text);
                if (map.NameType == 1)
                {
                    switch (dieIndex)
                    {
                        case 1:
                            lbl_RefDie1Name.Text = die.OrdName;
                            break;
                        case 2:
                            lbl_RefDie2Name.Text = die.OrdName;
                            break;
                        case 3:
                            lbl_RefDie3Name.Text = die.OrdName;
                            break;
                        case 4:
                            lbl_RefDie4Name.Text = die.OrdName;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (dieIndex)
                    {
                        case 1:
                            lbl_RefDie1Name.Text = $"({die.X},{die.Y})";
                            break;
                        case 2:
                            lbl_RefDie2Name.Text = $"({die.X},{die.Y})";
                            break;
                        case 3:
                            lbl_RefDie3Name.Text = $"({die.X},{die.Y})";
                            break;
                        case 4:
                            lbl_RefDie4Name.Text = $"({die.X},{die.Y})";
                            break;
                        default:
                            break;
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show($"选择参考点失败:{ex.Message}");
            } 

                      
        }

        private void btn_ConfirmRefDie_Click(object sender, EventArgs e)
        {
            var map = ConfigMgr.LoadWaferMapInfoByType(cbox_WaferType.Text);

            var refDie1 = map.GetDieByName(lbl_RefDie1Name.Text);
            var refDie2 = map.GetDieByName(lbl_RefDie2Name.Text);
            var refDie3 = map.GetDieByName(lbl_RefDie3Name.Text);
            var refDie4 = map.GetDieByName(lbl_RefDie4Name.Text);

            //Die有效性检查
            if (!refDieCheck(refDie1,refDie2,refDie3,refDie4))
            {
                return;
            }
            
            map.RefDieRowIndex1 = refDie1.RowIndex;
            map.RefDieRowIndex2 = refDie2.RowIndex;
            map.RefDieRowIndex3 = refDie3.RowIndex;
            map.RefDieRowIndex4 = refDie4.RowIndex;
            map.RefDieColumnIndex1 = refDie1.ColumnIndex;
            map.RefDieColumnIndex2 = refDie2.ColumnIndex;
            map.RefDieColumnIndex3 = refDie3.ColumnIndex;
            map.RefDieColumnIndex4 = refDie4.ColumnIndex;
            ConfigMgr.SaveWaferMapInfobyType(map);

            ReportMessage("设置基准Die成功");
            MessageBox.Show(this, "设置基准Die成功。", "Info:");            
        }

        public bool refDieCheck(DieInfo refDie1, DieInfo refDie2, DieInfo refDie3, DieInfo refDie4)
        {
            if (refDie1 == null || refDie2 == null || refDie3 == null || refDie4 == null)
            {
                ReportMessage("设置基准Die检查失败：4个基准Die未全部选择");
                MessageBox.Show(this, "4个基准Die未全部选择。", "Info:");                
                return false;
            }

            if (refDie1.RowIndex > refDie2.RowIndex || refDie1.ColumnIndex > refDie3.ColumnIndex || refDie1.RowIndex > refDie3.RowIndex || refDie1.ColumnIndex > refDie4.ColumnIndex)
            {
                ReportMessage("设置基准Die检查失败：基准Die1需要位于晶圆左上角");
                MessageBox.Show(this, "基准Die 1需要位于晶圆左上角。", "Info:");                
                return false;
            }

            if (refDie1.RowIndex != refDie4.RowIndex)
            {
                ReportMessage("设置基准Die检查失败：基准Die 1和基准Die 4需要位于同一行");
                MessageBox.Show(this, "基准Die 1和基准Die 4需要位于同一行。", "Info:");                
                return false;
            }

            if (refDie1.ColumnIndex != refDie2.ColumnIndex)
            {
                ReportMessage("设置基准Die检查失败：基准Die 1和基准Die 2需要位于同一列");
                MessageBox.Show(this, "基准Die 1和基准Die 2需要位于同一列。", "Info:");                
                return false;
            }

            return true;
        }

        private void btn_AutoMap_Click(object sender, EventArgs e)
        {
            if (!BeforeAutoMapCheck())
            {
                return;
            }

            var map = ConfigMgr.LoadWaferMapInfoByType(cbox_WaferType.Text);
            if (map == null)
            {
                ReportMessage("不存在该晶圆类型");
                MessageBox.Show(this, "不存在该晶圆类型。", "Info:");
                return;
            }

            //线程耗时长，用该方法
            Task.Factory.StartNew(() =>
            {
                try
                {
                    MotionState = State.Busy;
                    Invoke(new Action(() => { 
                        ReportMessage("开始AutoMap");
                        btn_StopMap.Enabled = true;
                        btn_AutoMap.Enabled = false;
                    }));

                    waferHandle.MotionState = State.Ready;

                    if (waferHandle.AutoMap(map, out string errMsg))
                    {
                        Invoke(new Action(() =>
                        {
                            ctlWafer_Main.GenerateMap(map);
                            UIClass.ObjectToControl(map, pn_WaferInfo);
                            ConfigMgr.SaveWaferMapInfobyType(map);
                            ReportMessage("AutoMap完成");
                            MessageBox.Show(this, "AutoMap完成。", "Info:");
                        }));
                    }
                    else
                    {
                        Invoke(new Action(() =>
                        {
                            ReportMessage($"AutoMap失败:{errMsg}");
                            MessageBox.Show(this, $"AutoMap失败:{errMsg}", "Info:"); 
                        }));
                    }
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() =>
                    {
                        ReportMessage($"AutoMap异常:{ex.Message}");
                        MessageBox.Show($"AutoMap异常:{ex.Message}");
                    }));                    
                    return;
                }
                finally
                {
                    MotionState = State.Ready ;
                    Invoke(new Action(() =>
                    {
                        btn_StopMap.Enabled = true;
                        btn_AutoMap.Enabled = true;
                    }));
                }
            });
        }

        public bool BeforeAutoMapCheck()
        {
            if (MotionState != State.Ready)
            {
                ReportMessage("AutoMap前检查失败：设备未准备好");
                MessageBox.Show(this, "设备未准备好。", "Info:");
                return false;
            }

            if (!File.Exists($"Configuration\\Wafer\\{cbox_WaferType.Text}.shm"))
            {
                ReportMessage("AutoMap前检查失败：未找到Mark信息");
                MessageBox.Show(this, "未找到Mark信息，请先选择Mark。", "Info:");
                return false;
            }

            if (MessageBox.Show("确定要开始自动生成Map吗？", "Info:", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                ReportMessage("AutoMap前检查失败：用户取消");
                return false;
            }

            return true;
        }                    


        private void btn_StopMap_Click(object sender, EventArgs e)
        {
            waferHandle.MotionState = State.Stop;
        }

        private void ReportMessage(string message)
        {
            this.BeginInvoke(new Action(() => {
                rtbMsgBox.AppendText(Environment.NewLine + $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] : {message}");
            }));
        }

        private void FormMapping_Load(object sender, EventArgs e) {
            //加载map类型
            cbox_WaferType.Items.Clear();
            cbox_WaferType.Items.AddRange(ConfigMgr.GetWaferMapNames());
        }  
                     
        private void rtbMsgBox_TextChanged(object sender, EventArgs e)
        {
            rtbMsgBox.SelectionStart = rtbMsgBox.Text.Length;
            rtbMsgBox.ScrollToCaret();
        }

        public const int WM_GOTO = 0xF112;
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_GOTO:
                    DieGotoProcess();
                    break;
                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        private void DieGotoProcess()
        {
            WaferMapInfo map = ConfigMgr.LoadWaferMapInfoByType(cbox_WaferType.Text);
            if (map == null)
            {
                MessageBox.Show(this, "晶圆类型为空", "Info:");
                return;
            }

            var selectDie = ctlWafer_Main.GetSelectedDie();
            var preDie = ctlWafer_Main.GetPreSelectedDie();

            if ((preDie == null) || (selectDie == null))
            {
                MessageBox.Show(this, "请选择一个Reticle", "Info:");
                return;
            }

            waferHandle.MoveToNextReticle(selectDie, preDie, map);
        }
    }
}
