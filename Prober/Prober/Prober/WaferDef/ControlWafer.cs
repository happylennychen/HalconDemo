using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prober.WaferDef {
    public partial class ControlWafer : UserControl
    {
        public int Radius { get { return this.Width / 2; } set { this.Height = value * 2 + 40; this.Width = value * 2; this.Invalidate(); } }
        private string CurWaferType = string.Empty;
        private bool bMoveEnable = false;
        private int TestDir = (int)TEST_DIR.Dir_Z_P;
        public Action<WaferMapInfo> UpdateMapInfo = null;
        public Func<string, bool> UploadWafer = null;
        public Func<string, bool> DownloadWafer = null;
        public Func<string,string, bool> ReticleMove = null;
        public Func<string, bool> MoveToMarkPos = null;
        public Func<string, bool> HeightVerify = null;
        public string ParentFormName = string.Empty;
        private int homeDieRowIndex = 0;
        private int homeDieColumnIndex = 0;

        public ControlWafer()
        {
            InitializeComponent(); 
        }

        public void SetWaferType(string WaferType)
        {
            CurWaferType = WaferType;
        }

        public string GetWaferType() 
        {
            return CurWaferType;
        }

        public void InitWaferTypeList(string[] wafeTypeList) 
        {
            cbox_WaferType.Items.Clear();
            cbox_WaferType.Items.AddRange(wafeTypeList);
        }

        public void ChangeMode(bool MappingMode)
        {
            //Map模式
            if (!MappingMode)
            {
                this.btn_Active.Visible = false;
                this.btn_Delete.Visible = false;
                this.btn_HalfDieSet.Visible = false;
                this.btn_Home.Visible = false;
                this.btn_goto.Visible = true;
                this.btn_Origin.Visible = true;

                this.lbl_WaferType.Visible = true;
                this.cbox_WaferType.Visible = true;
                this.btn_Upload.Visible = true;
                this.btn_Download.Visible = true;

                this.btn_OrderZColRight.Visible = false;
                this.btn_OrderZColLeft.Visible = false;
                this.btn_OrderZRowRight.Visible = false;
                this.btn_OrderZRowLeft.Visible = false;
                this.btn_HeightVerify.Visible = true;  
            }
            //测试模式
            else
            {
                this.btn_Active.Visible = true;
                this.btn_Delete.Visible = true;
                this.btn_HalfDieSet.Visible = true;
                this.btn_Home.Visible = true;
                this.btn_goto.Visible = true;
                this.btn_Origin.Visible = false;

                this.lbl_WaferType.Visible = false;
                this.cbox_WaferType.Visible = false;
                this.btn_Upload.Visible = false;
                this.btn_Download.Visible = false;

                this.btn_OrderZColRight.Visible = true;
                this.btn_OrderZColLeft.Visible = true;
                this.btn_OrderZRowRight.Visible = true;
                this.btn_OrderZRowLeft.Visible = true;
                this.btn_HeightVerify.Visible = false;
            }
        }

        public ControlDie GetSelectedDieCtrl() {
            foreach (var item in table_Main.Controls) {
                var die = item as ControlDie;
                if (die.IsSelected) {
                    return die;
                }
            }

            return null;
        }
        
        public DieInfo GetSelectedDie()
        {
            DieInfo info = null;

            foreach (var item in table_Main.Controls)
            {
                var die = item as ControlDie;
                if (die.IsSelected)
                {
                    info = new DieInfo();
                    info.Name = die.DieName;
                    info.RowIndex = table_Main.GetRow(die);
                    info.ColumnIndex = table_Main.GetColumn(die);
                    
                    if (die.DieOrdinate.Contains(","))
                    {
                        string[] split = die.DieOrdinate.Split('(', ',', ')');
                        info.X = Convert.ToInt32(split[1]);
                        info.Y = Convert.ToInt32(split[2]);

                    }
                    else
                    {
                        DieNameToOrdinary(die.DieOrdinate, out int x, out int y);
                        info.X = x;
                        info.Y = y;
                    }                        
                    info.OrdName = die.DieOrdinate;
                    break;
                }
            }

            return info;
        }

        public void unHighLightAllDie() {            
            foreach (var item in table_Main.Controls) {
                var ctl = item as ControlDie;
                if (ctl.IsHighLightState()) {
                    ctl.SetHightLightState(false);  
                }
            }
        }

        public ControlDie GetHighLightDie() {
            foreach (var item in table_Main.Controls) {
                var ctl = item as ControlDie;
                if (ctl.IsHighLightState()) {
                    return ctl;
                }
            }

            return null;
        }

        /// <summary>
        /// row index 是tablayout panel序号
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        public void SetDieHighLightWithIndex(int rowIndex, int columnIndex)
        {
            unHighLightAllDie();
            var die = table_Main.GetControlFromPosition(columnIndex, rowIndex) as ControlDie;
            die.SetHightLightState(true);
        }

        /// <summary>
        /// row index是相对于Home的序号
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        public void SetDieHighLightWithIndexByHome(int rowIndex, int columnIndex)
        {
            unHighLightAllDie();
            rowIndex += homeDieRowIndex;
            columnIndex += homeDieColumnIndex;
            var die = table_Main.GetControlFromPosition(columnIndex, rowIndex) as ControlDie;
            die.SetHightLightState(true);
        }

        public void SetDieHighLight(DieInfo info)
        {
            unHighLightAllDie();
            foreach (var item in table_Main.Controls)
            {
                var die = item as ControlDie;
                if (die.DieOrdinate == info.OrdName)
                {
                    die.SetHightLightState(true);
                }
            }
        }

        public void unReferenceAllDie()
        {
            foreach (var item in table_Main.Controls)
            {
                var ctl = item as ControlDie;
                if (ctl.IsReferenceState())
                {
                    ctl.SetReferenceState(false);
                }
            }
        }

        public ControlDie GetReferenceDie()
        {
            foreach (var item in table_Main.Controls)
            {
                var ctl = item as ControlDie;
                if (ctl.IsReferenceState())
                {
                    return ctl;
                }
            }

            return null;
        }

        public void SetDieReferenceWithIndex(int rowIndex, int columnIndex)
        {
            unReferenceAllDie();
            rowIndex += homeDieRowIndex;
            columnIndex += homeDieColumnIndex;
            var die = table_Main.GetControlFromPosition(columnIndex, rowIndex) as ControlDie;
            die.SetReferenceState(true);
        }

        public void SetDieReference(DieInfo info)
        {
            unReferenceAllDie();
            foreach (var item in table_Main.Controls)
            {
                var die = item as ControlDie;
                if (die.DieOrdinate == info.OrdName)
                {
                    die.SetReferenceState(true);
                }
            }
        }

        public DieInfo GetPreSelectedDie()
        {
            DieInfo info = null;

            if (ControlDie.tempDie != null)
            {
                info = new DieInfo();
                info.Name = ControlDie.tempDie.DieName;
                info.RowIndex = table_Main.GetRow(ControlDie.tempDie);
                info.ColumnIndex = table_Main.GetColumn(ControlDie.tempDie);
                
                if (ControlDie.tempDie.DieOrdinate.Contains(","))
                {
                    string[] split = ControlDie.tempDie.DieOrdinate.Split('(', ',', ')');
                    info.X = Convert.ToInt32(split[1]);
                    info.Y = Convert.ToInt32(split[2]);
                }
                else
                {
                    DieNameToOrdinary(ControlDie.tempDie.DieOrdinate, out int x, out int y);
                    info.X = x;
                    info.Y = y;
                }

                info.OrdName = ControlDie.tempDie.DieOrdinate;                
            }

            return info;
        }

        public void DieNameToOrdinary(string dieName, out int x, out int y)
        {
            x = 0;
            y = 0;
            try
            {
                x = dieName.ElementAt(0) - 'A';
                y = Convert.ToInt32(dieName.Substring(1, 2)) - 1;
            }
            catch(Exception ex)
            {
                throw (new Exception($"DieNameToOrdinary;{ex.Message}"));
            }
            
        }

        //y长度不会超过100；
        public void DieOrdinaryToName(int x, int y, out string dieName)
        {
            dieName = string.Empty;

            try
            {
                char a = (char)('A' + x);
                string b = (y + 1).ToString("00");

                dieName = a + b;
            }
            catch (Exception ex)
            {                
                throw (new Exception($"DieOrdinaryToName异常;{ex.Message}"));
            }            
        }                   

        private void btn_Home_Click(object sender, EventArgs e)
        {
            var map = ConfigMgr.LoadWaferMapInfoByType(CurWaferType);
            if (map == null)
            {
                MessageBox.Show(this, "不存在晶圆类型。", "Info:");
                return;
            }

            if (ControlDie.CurState == DieState.None)
            {
                btn_Home.BackColor = Color.Lime;
                ControlDie.CurState = DieState.Home;
                Cursor = Cursors.Hand;
                btn_Active.Enabled = false;
            }
            else
            {
                ControlDie.CurState = DieState.None;
                btn_Home.BackColor = Color.Transparent;
                Cursor = Cursors.Default;
                btn_Active.Enabled = true;

                //更新die的编号
                if (-1 == TestDir)
                {
                    UpdateDieNo(TEST_DIR.Dir_Z_P);
                }
                else
                {
                    UpdateDieNo((TEST_DIR)TestDir);
                }

                UpdateOrdinator(map);
                //保存die信息
                map.Dies.Clear();
                map.Dies = GetActivedDies();

                ConfigMgr.SaveWaferMapInfobyType(map);
                if (UpdateMapInfo != null) {
                    UpdateMapInfo(map);
                }
            }
        }

        public void UpdateOrdinator(WaferMapInfo map)
        {
            //更新坐标
            if (ControlDie.PreHomeDie != null)
            {
                int rowBase = table_Main.GetRow(ControlDie.PreHomeDie);
                int colBase = table_Main.GetColumn(ControlDie.PreHomeDie);
                map.HomeDieColIndex = colBase;
                map.HomeDieRowIndex = rowBase;

                foreach (var item in table_Main.Controls)
                {
                    var tempCtlDie = item as ControlDie;
                    if (!tempCtlDie.IsActived)
                        continue;
                    int rowItem = table_Main.GetRow(tempCtlDie);
                    int colItem = table_Main.GetColumn(tempCtlDie);
                    //int y = rowBase - rowItem;
                    int y = rowItem - rowBase;
                    int x = colItem - colBase;
                    if (1 == map.NameType)
                    {
                        DieOrdinaryToName(x, y, out string name);
                        tempCtlDie.DieOrdinate = $"{name}";
                    }
                    else
                    {
                        tempCtlDie.DieOrdinate = $"({x},{y})";
                    }                    
                }                
            }

            homeDieRowIndex = map.HomeDieRowIndex;
            homeDieColumnIndex = map.HomeDieColIndex;
        }

        public void SetSelectedDie(int rowIndex, int columnIndex)
        {
            if (ControlDie.PreSelectedDie != null)
            {
                ControlDie.tempDie = ControlDie.PreSelectedDie;
                ControlDie.PreSelectedDie.IsSelected = false;
            }

            var curDie = table_Main.GetControlFromPosition(columnIndex, rowIndex) as ControlDie;
            curDie.IsSelected = true;
            ControlDie.PreSelectedDie = curDie;
        }

        public void SetSelectedDieIndexByHome(int rowIndex, int columnIndex)
        {
            if (ControlDie.PreSelectedDie != null)
            {
                ControlDie.tempDie = ControlDie.PreSelectedDie;
                ControlDie.PreSelectedDie.IsSelected = false;
            }

            columnIndex += homeDieColumnIndex;
            rowIndex += homeDieRowIndex;
            var curDie = table_Main.GetControlFromPosition(columnIndex, rowIndex) as ControlDie;
            curDie.IsSelected = true;
            ControlDie.PreSelectedDie = curDie;
        }

        public List<DieInfo> GetActivedDies()
        {
            List<DieInfo> dies = new List<DieInfo>();
            foreach (var item in table_Main.Controls)
            {
                var ctl = item as ControlDie;
                if (ctl.IsActived)
                {
                    int rowItem = table_Main.GetRow(ctl);
                    int colItem = table_Main.GetColumn(ctl);
                    DieInfo die = new DieInfo();
                    die.Name = ctl.DieName;
                    die.RowIndex = rowItem;
                    die.ColumnIndex = colItem;
                    
                    if (ctl.DieOrdinate.Contains(","))
                    {
                        string[] split = ctl.DieOrdinate.Split('(', ',', ')');
                        die.X = Convert.ToInt32(split[1]);
                        die.Y = Convert.ToInt32(split[2]);
                    }
                    else
                    {
                        DieNameToOrdinary(ctl.DieOrdinate, out int x, out int y);
                        die.X = x;
                        die.Y = y;
                    }

                    die.OrdName = ctl.DieOrdinate;                    
                    die.isFullDie = !ctl.IsHalfed;
                    dies.Add(die);
                }
            }
            return dies;
        }
        
        private void btn_Active_Click(object sender, EventArgs e)
        {
            var map = ConfigMgr.LoadWaferMapInfoByType(CurWaferType);
            if (map == null)
            {
                MessageBox.Show(this, "不存在晶圆类型。", "Info:");
                return;
            }

            if (ControlDie.CurState == DieState.None)
            {
                btn_Active.BackColor = Color.Lime;
                ControlDie.CurState = DieState.Active;
                Cursor = Cursors.Hand;
                btn_Home.Enabled = false;
                if (ControlDie.PreSelectedDie != null)
                {
                    ControlDie.PreSelectedDie.IsSelected = false;
                }
            }
            else
            {
                ControlDie.CurState = DieState.None;
                btn_Active.BackColor = Color.Transparent;
                Cursor = Cursors.Default;
                btn_Home.Enabled = true;

                //更新die的编号
                if (-1 == TestDir)
                {
                    UpdateDieNo(TEST_DIR.Dir_Z_P);
                }
                else
                {
                    UpdateDieNo((TEST_DIR)TestDir);
                }

                UpdateOrdinator(map);
                //保存die信息
                map.Dies.Clear();
                map.Dies = GetActivedDies();
                if (map.Dies.Count > 0)
                {
                    var allRow = map.Dies.Select(t => t.RowIndex).ToList();
                    var allCol = map.Dies.Select(t => t.ColumnIndex).ToList();
                    map.DieRows = allRow.Max() - allRow.Min() + 1;
                    map.DieColumns = allCol.Max() - allCol.Min() + 1;
                }
                else
                {
                    map.DieRows = 0;
                    map.DieColumns = 0;
                }

                ConfigMgr.SaveWaferMapInfobyType(map);
                if (UpdateMapInfo != null)
                {  
                    UpdateMapInfo(map); 
                }                    
            }
        }

        public void UpdateDieNoByZP()
        {
            int index = 1;
            int dir = 1;
            bool isHaveDie = false;
            for (int i = 0; i < table_Main.RowCount; i++)
            {
                if (dir == 1)
                {
                    for (int j = 0; j < table_Main.ColumnCount; j++)
                    {
                        var ctl = table_Main.GetControlFromPosition(j, i) as ControlDie;
                        if (ctl.IsActived)
                        {
                            ctl.DieName = $"{index}#";
                            index++;
                            isHaveDie = true;
                        }
                    }
                }
                else
                {
                    for (int j = table_Main.ColumnCount - 1; j >= 0; j--)
                    {
                        var ctl = table_Main.GetControlFromPosition(j, i) as ControlDie;
                        if (ctl.IsActived)
                        {
                            ctl.DieName = $"{index}#";
                            index++;
                            isHaveDie = true;
                        }
                    }
                }
                if (isHaveDie)
                {
                    dir = -dir;
                }
            }
        }

        public void UpdateDieNoByNP()
        {            
            int index = 1;
            int dir = 1;
            bool isHaveDie = false;
            for (int i = table_Main.ColumnCount - 1; i >= 0; i--)
            {
                if (dir == 1)
                {
                    for (int j = 0; j < table_Main.RowCount; j++)
                    {
                        var ctl = table_Main.GetControlFromPosition(i, j) as ControlDie;
                        if (ctl.IsActived)
                        {
                            ctl.DieName = $"{index}#";
                            index++;
                            isHaveDie = true;
                        }
                    }
                }
                else
                {
                    for (int j = table_Main.RowCount - 1; j >= 0; j--)
                    {
                        var ctl = table_Main.GetControlFromPosition(i, j) as ControlDie;
                        if (ctl.IsActived)
                        {
                            ctl.DieName = $"{index}#";
                            index++;
                            isHaveDie = true;
                        }
                    }
                }
                if (isHaveDie)
                {
                    dir = -dir;
                }
            }
        }

        public void UpdateDieNoByNR()
        {            
            int index = 1;
            int dir = 1;
            bool isHaveDie = false;
            for (int i = 0; i < table_Main.ColumnCount; i++)
            {
                if (dir == 1)
                {
                    for (int j = 0; j < table_Main.RowCount; j++)
                    {
                        var ctl = table_Main.GetControlFromPosition(i, j) as ControlDie;
                        if (ctl.IsActived)
                        {
                            ctl.DieName = $"{index}#";
                            index++;
                            isHaveDie = true;
                        }
                    }
                }
                else
                {
                    for (int j = table_Main.RowCount - 1; j >= 0; j--)
                    {
                        var ctl = table_Main.GetControlFromPosition(i, j) as ControlDie;
                        if (ctl.IsActived)
                        {
                            ctl.DieName = $"{index}#";
                            index++;
                            isHaveDie = true;
                        }
                    }
                }
                if (isHaveDie)
                {
                    dir = -dir;
                }
            }
        }

        public void UpdateDieNoByZR()
        {
            int index = 1;
            int dir = 1;
            bool isHaveDie = false;
            for (int i = 0; i <= table_Main.RowCount - 1; i++)
            {
                if (dir == 1)
                {
                    for (int j = table_Main.ColumnCount - 1; j >= 0; j--)
                    {
                        var ctl = table_Main.GetControlFromPosition(j, i) as ControlDie;
                        if (ctl.IsActived)
                        {
                            ctl.DieName = $"{index}#";
                            index++;
                            isHaveDie = true;
                        }
                    }                    
                }
                else
                {
                    for (int j = 0; j < table_Main.ColumnCount; j++)
                    {
                        var ctl = table_Main.GetControlFromPosition(j, i) as ControlDie;
                        if (ctl.IsActived)
                        {
                            ctl.DieName = $"{index}#";
                            index++;
                            isHaveDie = true;
                        }
                    }
                }
                if (isHaveDie)
                {
                    dir = -dir;
                }
            }
        }

        public void UpdateDieNo(TEST_DIR dir)
        {
            switch (dir) 
            {
                case TEST_DIR.Dir_N_P:
                {
                    UpdateDieNoByNP();
                    break;
                }
                case TEST_DIR.Dir_N_R: 
                {
                    UpdateDieNoByNR();
                    break;
                }
                case TEST_DIR.Dir_Z_P:
                {
                    UpdateDieNoByZP();
                    break;
                }
                case TEST_DIR.Dir_Z_R:
                {
                    UpdateDieNoByZR();
                    break;
                }
                default: 
                {
                    break;
                }
            }
        }

        private void ControlWafer_Load(object sender, EventArgs e)
        {
            ;
        }

        public void GenerateMap(WaferMapInfo map)
        {
            if (map.DieRows == 0 || map.DieColumns == 0 || map.CtlRows == 0 || map.CtlCols == 0)
            {
                return;
            }
            table_Main.Visible = false;

            table_Main.RowCount = map.CtlRows;
            table_Main.ColumnCount = map.CtlCols;
            table_Main.Controls.Clear();
            table_Main.RowStyles.Clear();
            table_Main.ColumnStyles.Clear();
            for (int i = 0; i < map.CtlRows; i++)
            {
                table_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
                //table_Main.RowStyles.Add(new RowStyle(SizeType.Absolute,82));
            }
            for (int i = 0; i < map.CtlCols; i++)
            {
                table_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
                //table_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,82));
            }

            for (int i = 0; i < table_Main.RowCount; i++)
            {
                for (int j = 0; j < table_Main.ColumnCount; j++)
                {
                    ControlDie die = new ControlDie();
                    die.Dock = DockStyle.Fill;
                    die.Margin = new Padding(1);
                    die.DieName = "1#";
                    die.ParentFormName = ParentFormName;
                    if (1 == map.NameType)
                    {
                        die.DieOrdinate = "A01";
                    }
                    else
                    {
                        die.DieOrdinate = "(1,1)";
                    }

                    TableLayoutPanelCellPosition position = new TableLayoutPanelCellPosition(j, i);
                    table_Main.Controls.Add(die);
                    table_Main.SetCellPosition(die, position);
                    die.IsActived = false;
                    die.IsHalfed = false;
                }
            }

            //显示激活的die和home die
            foreach (var item in map.Dies)
            {
                var ctl = table_Main.GetControlFromPosition(item.ColumnIndex, item.RowIndex) as ControlDie;
                ctl.DieName = item.Name;
                if (1 == map.NameType)
                {
                    DieOrdinaryToName(item.X, item.Y, out string dieName);
                    ctl.DieOrdinate = $"{dieName}";
                }
                else
                {
                    ctl.DieOrdinate = $"({item.X},{item.Y})";
                }

                ctl.IsActived = true;
                if (!item.isFullDie) {
                    ctl.IsHalfed = true;
                }
            }

            var ctlHome = table_Main.GetControlFromPosition(map.HomeDieColIndex, map.HomeDieRowIndex) as ControlDie;

            ctlHome.IsHome = true;
            ControlDie.PreHomeDie = ctlHome;
            CurWaferType = map.Type;
            table_Main.Visible = true;

            homeDieRowIndex = map.HomeDieRowIndex;
            homeDieColumnIndex = map.HomeDieColIndex;
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            //var maps = ConfigMgr.LoadWaferMapsInfo();
            //var map = maps.FirstOrDefault(t => t.Type == CurWaferType);
            var map = ConfigMgr.LoadWaferMapInfoByType(CurWaferType);
            if (map == null)
            {
                MessageBox.Show(this, "不存在晶圆类型。", "Info:");
                return;
            }

            if (ControlDie.CurState == DieState.None)
            {
                btn_Delete.BackColor = Color.Lime;
                ControlDie.CurState = DieState.DeActive;
                Cursor = Cursors.Hand;
                btn_Home.Enabled = false;
                if (ControlDie.PreSelectedDie != null)
                {
                    ControlDie.PreSelectedDie.IsSelected = false;
                }
            }
            else
            {
                ControlDie.CurState = DieState.None;
                btn_Delete.BackColor = Color.Transparent;
                Cursor = Cursors.Default;
                btn_Home.Enabled = true;

                //更新die的编号
                //更新die的编号
                if (-1 == TestDir)
                {
                    UpdateDieNo(TEST_DIR.Dir_Z_P);
                }
                else
                {
                    UpdateDieNo((TEST_DIR)TestDir);
                }

                UpdateOrdinator(map);
                //保存die信息
                map.Dies.Clear();
                map.Dies = GetActivedDies();
                if (map.Dies.Count > 0)
                {
                    var allRow = map.Dies.Select(t => t.RowIndex).ToList();
                    var allCol = map.Dies.Select(t => t.ColumnIndex).ToList();
                    map.DieRows = allRow.Max() - allRow.Min() + 1;
                    map.DieColumns = allCol.Max() - allCol.Min() + 1;
                }
                else
                {
                    map.DieRows = 0;
                    map.DieColumns = 0;
                }

                ConfigMgr.SaveWaferMapInfobyType(map);
                if (UpdateMapInfo != null)
                {
                    UpdateMapInfo(map);
                }
            }
        }

        private void btn_goto_Click(object sender, EventArgs e)
        {
            //如果当前使能，点击后变为不使能，背景为黑色
            if (bMoveEnable)
            {
                ControlDie.CurState = DieState.GoToDisable;
                bMoveEnable = false;
                Cursor = Cursors.Hand;
                btn_goto.BackColor = Color.Transparent;
            }
            //如果当前不使能，点击后变为使能，背景为白色
            else
            {
                //将当前高亮显示的reticle设置为select状态
                ControlDie selectDie = GetSelectedDieCtrl();
                ControlDie highDie = GetHighLightDie();

                if (selectDie != null && highDie != selectDie) {
                    selectDie.IsSelected = false;
                }
                
                if (highDie != null) {
                    highDie.IsSelected = true;
                    ControlDie.PreSelectedDie = highDie;
                    highDie.SetHightLightState(true);
                }
                ControlDie.CurState = DieState.GoToEnable;
                bMoveEnable = true;
                Cursor = Cursors.Default;
                btn_goto.BackColor = Color.Lime;
            }           
        }

        public bool getMoveEnableStatus()
        {
            return bMoveEnable;
        }

        private void btn_HalfDieSet_Click(object sender, EventArgs e)
        {
            //晶圆类型得存在
            //var maps = ConfigMgr.LoadWaferMapsInfo();
            //var map = maps.FirstOrDefault(t => t.Type == CurWaferType);
            var map = ConfigMgr.LoadWaferMapInfoByType(CurWaferType);
            if (map == null)
            {
                MessageBox.Show(this, "不存在晶圆类型。", "Info:");
                return;
            }

            if (ControlDie.CurState == DieState.None)
            {
                btn_HalfDieSet.BackColor = Color.Lime;
                ControlDie.CurState = DieState.Halfed;
                Cursor = Cursors.Hand;
                btn_Home.Enabled = false;
                if (ControlDie.PreSelectedDie != null)
                {
                    ControlDie.PreSelectedDie.IsSelected = false;
                }
            }
            else
            {
                ControlDie.CurState = DieState.None;
                btn_HalfDieSet.BackColor = Color.Transparent;
                Cursor = Cursors.Default;
                btn_Home.Enabled = true;

                //更新die的编号
                if (-1 == TestDir)
                {
                    UpdateDieNo(TEST_DIR.Dir_Z_P);
                }
                else
                {
                    UpdateDieNo((TEST_DIR)TestDir);
                }

                UpdateOrdinator(map);
                //保存die信息
                map.Dies.Clear();
                map.Dies = GetActivedDies();
                if (map.Dies.Count > 0)
                {
                    var allRow = map.Dies.Select(t => t.RowIndex).ToList();
                    var allCol = map.Dies.Select(t => t.ColumnIndex).ToList();
                    map.DieRows = allRow.Max() - allRow.Min() + 1;
                    map.DieColumns = allCol.Max() - allCol.Min() + 1;
                }
                else
                {
                    map.DieRows = 0;
                    map.DieColumns = 0;
                }

                ConfigMgr.SaveWaferMapInfobyType(map);
                if (UpdateMapInfo != null)
                {
                    UpdateMapInfo(map);
                }
            }
        }

        private void btn_OrderZRowRight_Click(object sender, EventArgs e)
        {
            TestDir = (int)(TEST_DIR.Dir_Z_P);
        }

        private void btn_OrderZRowLeft_Click(object sender, EventArgs e)
        {
            TestDir = (int)(TEST_DIR.Dir_Z_R);
        }

        private void btn_OrderZColLeft_Click(object sender, EventArgs e)
        {
            TestDir = (int)(TEST_DIR.Dir_N_R);
        }

        private void btn_OrderZColRight_Click(object sender, EventArgs e)
        {
            TestDir = (int)(TEST_DIR.Dir_N_P);
        }

        private void cbox_WaferType_SelectedIndexChanged(object sender, EventArgs e) {
            //根据选择wafer类型，加载相应的MAP图
            var map = ConfigMgr.LoadWaferMapInfoByType(cbox_WaferType.Text);
            if (map == null) {
                MessageBox.Show(this, $"晶圆类型{cbox_WaferType.Text}Map图不存在", "Info:");
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            if (UpdateMapInfo != null)
            {
                UpdateMapInfo(map);
            }
            GenerateMap(map);
            this.Cursor = Cursors.Default;

            CurWaferType = cbox_WaferType.Text;
        }

        private void btn_Upload_Click(object sender, EventArgs e)
        {
            if ((UploadWafer != null) && (CurWaferType != string.Empty))
            {
                //提示探针抬起
                if (MessageBox.Show("确定要运动上料位置吗？如果有探针，请将探针抬起到安全高度！！！", "提示：", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return ;
                }

                UploadWafer(CurWaferType);
            }            
        }

        private void btn_Download_Click(object sender, EventArgs e)
        {
            if ((DownloadWafer != null) && (CurWaferType != string.Empty))
            {
                if (MessageBox.Show("确定要运动下料位置吗？", "提示：", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return ;
                }

                DownloadWafer(CurWaferType);
            }               
        }

        private void btn_Origin_Click(object sender, EventArgs e)
        {
            if ((MoveToMarkPos != null) && (CurWaferType != string.Empty)){
                if (MessageBox.Show("确定要运动到Mark点吗？", "提示：", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }

                MoveToMarkPos(CurWaferType);
            }
        }

        private void btn_HeightVerify_Click(object sender, EventArgs e)
        {
            if ((HeightVerify != null) && (CurWaferType != string.Empty))
            {                
                if (MessageBox.Show("确定要进行高度校验吗？如果有探针，请将探针抬起到安全高度！！！", "提示：", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }

                HeightVerify(CurWaferType);
            }
        }
    }

    public enum TEST_DIR
    {
        Dir_N_P = 0,
        Dir_N_R,
        Dir_Z_P,
        Dir_Z_R
    }
}
