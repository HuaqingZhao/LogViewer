using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogViewer.Controls;
using LogViewer.Entities;
using LogViewer.Utilities;

namespace LogViewer
{
    public partial class LogCtrl : UserControl
    {
        #region Fields
        private string originalPattern;
        private string pattern;
        private bool isTimeChanged;
        private DateTime from = DateTime.MinValue;
        private DateTime to = DateTime.MaxValue;
        private Dictionary<Guid, int> LogIndex;
        private Guid previousLogGuid;
        private LogCtrlController controller;
        private static object guiLock = new object();
        private Timer timer;
        private Color HighlightColor = Color.Red;

        private PatternSelectionForm patternSelectionForm;
        private bool isLeave;
        #endregion

        #region Properties
        public string TabName { get; set; }

        private int SelectedRowIndex
        {
            get
            {
                if (dgvMain.SelectedRows.Count > 0)
                {
                    return dgvMain.SelectedRows[0].Index;
                }

                return -1;
            }
        }

        private IList<Log> LogList
        {
            get
            {
                return dgvMain.DataSource as List<Log>;
            }
        }
        #endregion

        #region Ctor.
        public LogCtrl()
        {
            InitializeComponent();

            Initialize();
        }

        #endregion

        #region LogManager events

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            if (patternSelectionForm != null && !patternSelectionForm.IsDisposed)
            {
                isLeave = true;
                patternSelectionForm.Close();
            }
        }

        private void OnCaseSensitiveChanged()
        {
            ShowLog(LogManager.LogList);
        }

        private void OnStyleChanged()
        {
            RenderGridView();

            txtOutput.SelectionColor = Color.Red;
        }

        private void OnLoadControl()
        {
            ShowLog(LogManager.LogList);
        }

        private void OnLogSelectedChanged(string tabName, Guid logGuid)
        {
            previousLogGuid = logGuid;

            lock (guiLock)
            {
                var selectedIndex = -1;

                LogIndex.TryGetValue(logGuid, out selectedIndex);

                if (selectedIndex > 0)
                {
                    ToggleColor(selectedIndex - 1);
                }
            }
        }

        private void OnEventTimeChanged(DateTime from, DateTime to)
        {
            SetEventTime(from, to);
        }

        #endregion

        #region Override

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (LogIndex != null)
            {
                var rowIndex = -1;
                LogIndex.TryGetValue(previousLogGuid, out rowIndex);

                if (rowIndex > 0)
                    ScrollToRow(--rowIndex);
            }

            if (Visible)
            {
                isLeave = false;
            }
        }

        #endregion

        #region Private methods

        private void Initialize()
        {
            controller = new LogCtrlController();

            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += Time_Do;

            Dock = DockStyle.Fill;

            BackColor = LogManager.BackGroundColor;
            ForeColor = LogManager.ForceColor;

            splitContainer1.SplitterDistance = (int)(Width * 0.7d);

            RegisterEvents();
        }

        private void RegisterEvents()
        {
            dgvMain.DoubleClick -= dgvMain_DoubleClick;
            dgvMain.KeyUp -= dgvMain_KeyUp;
            LogManager.LogSelectedChanged -= OnLogSelectedChanged;
            LogManager.LoadControl -= OnLoadControl;
            LogManager.StyleChanged -= OnStyleChanged;
            LogManager.CaseSensitiveChanged -= OnCaseSensitiveChanged;
            LogManager.EventTimeChanged -= OnEventTimeChanged;

            dgvMain.DoubleClick += dgvMain_DoubleClick;
            dgvMain.KeyUp += dgvMain_KeyUp;
            //dgvMain.CellFormatting += dgvMain_CellFormatting;
            LogManager.LogSelectedChanged += OnLogSelectedChanged;
            LogManager.LoadControl += OnLoadControl;
            LogManager.StyleChanged += OnStyleChanged;
            LogManager.CaseSensitiveChanged += OnCaseSensitiveChanged;
            LogManager.EventTimeChanged += OnEventTimeChanged;
        }

        private void RenderGridView()
        {
            ShowLog(LogManager.LogList);

            dgvMain.RowHeadersDefaultCellStyle.BackColor = LogManager.BackGroundColor;
            dgvMain.ColumnHeadersDefaultCellStyle.BackColor = LogManager.BackGroundColor;

            dgvMain.RowsDefaultCellStyle.BackColor = LogManager.BackGroundColor;
            dgvMain.RowsDefaultCellStyle.ForeColor = LogManager.ForceColor;

            dgvMain.BackgroundColor = LogManager.BackGroundColor;

            //ToggleColor(0);
            //dgvMain.Refresh();
        }

        private bool NavigateToRow(int columnIndex, SearchOrientation SearchOrientation, SearchPattern pattern, IList<string> keys)
        {
            var found = false;

            switch (SearchOrientation)
            {
                case SearchOrientation.Up:
                    for (int i = SelectedRowIndex - 1; i > -1; i--)
                    {
                        found = FoundItem(columnIndex, keys, found, i, pattern);
                        if (found) break;
                    }
                    break;
                case SearchOrientation.Down:
                    // find next
                    for (int i = SelectedRowIndex + 1; i < dgvMain.Rows.Count; i++)
                    {
                        found = FoundItem(columnIndex, keys, found, i, pattern);
                        if (found) break;
                    }
                    break;
            }

            return found;
        }

        private bool FoundItem(int columnIndex, IList<string> keys, bool found, int i, SearchPattern pattern)
        {
            var val = dgvMain.Rows[i].Cells[columnIndex].Value.ToString();

            controller.FoundItem(keys, i, pattern, val, LogList);

            foreach (var item in keys)
            {
                switch (pattern)
                {
                    case SearchPattern.Equals:

                        if (val.SenseEquals(item))
                        {
                            found = true;
                            ScrollToRow(i);
                        }
                        break;

                    case SearchPattern.Contains:

                        if (val.SenseContains(item))
                        {
                            found = true;
                            ScrollToRow(i);
                        }
                        break;
                }
                if (found) break;
            }
            return found;
        }

        private void RowSelected()
        {
            if (dgvMain.SelectedRows.Count < 1) return;

            var guid = dgvMain.SelectedRows[0].Cells["LogGuid"].Value.ToString();

            LogManager.SelectedLogList.Add(new Guid(guid));

            if (LogManager.LogSelectedChanged != null)
            {
                Task.Factory.StartNew(() =>
                    {
                        LogManager.LogSelectedChanged(TabName, new Guid(guid));
                    });
            }
        }

        private void ToggleColor(int index)
        {
            this.BeginInvokeOnGui(() =>
            {
                if (dgvMain.Rows.Count <= index) return;

                if (dgvMain.Rows[index].Cells[4].Style.BackColor == Color.Empty)
                {
                    dgvMain.Rows[index].Cells[4].Style.BackColor = HighlightColor;
                }
                else
                {

                    dgvMain.Rows[index].Cells[4].Style.BackColor = Color.Empty;
                }
            });
        }

        private void Filter()
        {
            pattern = string.IsNullOrEmpty(txtPattern.Text) ? TabName.Equals("All")
                                            ? "" : originalPattern : txtPattern.Text;
            txtPattern.Text = pattern;
            controller.Pattern = pattern;
            ShowLog(LogManager.LogList);
        }

        private void HighlightText(string word)
        {
            if (word == "")
            {
                return;
            }

            var myRtb = txtOutput;
            int s_start = myRtb.SelectionStart, startIndex = 0, index;

            while ((index = myRtb.Text.SenseString().IndexOf(word.SenseString(), startIndex)) != -1)
            {
                myRtb.Select(index, word.Length);
                myRtb.SelectionFont = new Font(myRtb.Font, FontStyle.Bold);

                startIndex = index + word.Length;
            }

            myRtb.SelectionBackColor = Color.Yellow;
            myRtb.SelectionColor = Color.Red;
            myRtb.SelectionStart = s_start;
            myRtb.SelectionLength = 0;
        }

        private void ScrollToRow(int selectedIndex)
        {
            if (dgvMain.Rows.Count - 1 >= selectedIndex)
            {
                dgvMain.CurrentCell = dgvMain[4, selectedIndex];
                //dgvMain.Update();
            }
        }

        private bool Find(SearchOrientation SearchOrientation)
        {
            var found = false;
            switch (SearchOrientation)
            {
                case SearchOrientation.Up:
                    found = NavigateToRow(4, SearchOrientation.Up, SearchPattern.Contains, new[] { txtFind.Text });

                    break;
                case SearchOrientation.Down:
                    found = NavigateToRow(4, SearchOrientation.Down, SearchPattern.Contains, new[] { txtFind.Text });
                    break;
            }

            HighlightText(txtFind.Text);

            return found;
        }
        #endregion

        #region Public methods

        public void SetEventTime(DateTime from, DateTime to)
        {
            isTimeChanged = true;
            this.from = from;
            this.to = to;

            ShowLog(FilterLogList(LogManager.LogList));
        }

        private List<Log> FilterLogList(IList<Log> logs)
        {
            var temp = new List<Log>();

            foreach (var item in logs)
            {
                if (item.LogTime >= from && item.LogTime <= to)
                {
                    temp.Add(item);
                }
            }
            return temp;
        }

        public void SetPatterns(string p)
        {
            controller.Pattern = originalPattern = pattern = p;
            txtPattern.Text = originalPattern;
        }

        public void ShowLog(IList<Log> displaylogs)
        {
            IList<Log> temp = new List<Log>();
            LogIndex = new Dictionary<Guid, int>();

            displaylogs = FilterLogList(displaylogs);

            if (!string.IsNullOrEmpty(pattern))
            {
                var filter = string.Empty;

                if (pattern.Length > 4)
                    filter = pattern.Substring(0, 4).ToUpper();

                switch (filter)
                {
                    case "EXC=":
                        controller.ExcFilter(displaylogs, temp);
                        break;
                    case "LEV=":
                        controller.LevFilter(displaylogs, temp);
                        break;
                    case "EXP=":
                        controller.ExpFilter(displaylogs, temp);
                        break;
                    default:
                        controller.DefaultFilter(displaylogs, temp);
                        break;
                }
            }
            else
                temp = displaylogs;

            for (int i = 0; i < temp.Count; i++)
            {
                if (!LogIndex.ContainsKey(temp[i].LogGuid))
                    LogIndex.Add(temp[i].LogGuid, i + 1);
            }

            this.BeginInvokeOnGui(() =>
                            {
                                dgvMain.DataSource = temp;

                                var selectedLogList = LogManager.SelectedLogList;

                                if (selectedLogList != null)
                                {
                                    foreach (Guid item in selectedLogList)
                                    {
                                        var index = -1;
                                        LogIndex.TryGetValue(item, out index);

                                        if (index > 0)
                                        {
                                            ToggleColor(--index);
                                        }
                                    }
                                }

                                lblCount.Text = dgvMain.Rows.Count.ToString();
                            });
        }

        #endregion

        #region GUI events
        private void dgvMain_DoubleClick(object sender, EventArgs e)
        {
            RowSelected();
        }

        private void dgvMain_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMain.SelectedRows.Count > 0)
            {
                // ignor the row header cell event
                if (SelectedRowIndex == -1) return;

                txtOutput.Text = controller.FormatFields(LogList, SelectedRowIndex);
            }
        }

        private void dgvMain_Resize(object sender, EventArgs e)
        {
            var width = dgvMain.Width;
            var messageClumWidth = width - 280;
            if (messageClumWidth > 0)
                dgvMain.Columns[4].Width = messageClumWidth;
        }

        private void dgvMain_KeyUp(object sender, KeyEventArgs e)
        {
            //p = 80, n = 78
            if (e.Control) // ctrl keyborad pressed
            {
                switch (e.KeyCode)
                {
                    case Keys.K:
                        RowSelected();
                        break;
                    case Keys.P:
                        for (int i = SelectedRowIndex - 1; i > -1; i--)
                        {
                            if (dgvMain.Rows[i].Cells[4].Style.BackColor.Equals(HighlightColor))
                            {
                                ScrollToRow(i);
                                break;
                            }
                        }
                        break;
                    case Keys.N:
                        for (int i = SelectedRowIndex + 1; i < dgvMain.Rows.Count; i++)
                        {
                            if (dgvMain.Rows[i].Cells[4].Style.BackColor.Equals(HighlightColor))
                            {
                                ScrollToRow(i);
                                break;
                            }
                        }
                        break;
                }
            }

            switch (e.KeyCode)
            {
                case Keys.F7:
                    if (e.Shift)
                    {
                        NavigateToRow(1, SearchOrientation.Up, SearchPattern.Equals, new[] { "1" });
                    }
                    else
                    {
                        NavigateToRow(1, SearchOrientation.Down, SearchPattern.Equals, new[] { "1" });
                    }
                    break;
                case Keys.F8:
                    if (e.Shift)
                    {
                        NavigateToRow(1, SearchOrientation.Up, SearchPattern.Equals, new[] { "1", "2" });
                    }
                    else
                    {
                        NavigateToRow(1, SearchOrientation.Down, SearchPattern.Equals, new[] { "1", "2" });
                    }
                    break;
            }
        }

        private void btnFindUp_Click(object sender, EventArgs e)
        {
            var found = Find(SearchOrientation.Up);

            if (!found)
                MessageBox.Show("No found!");
        }

        private void btnFindDown_Click(object sender, EventArgs e)
        {
            var found = Find(SearchOrientation.Down);

            if (!found)
                MessageBox.Show("No found!");
        }

        private void txtFind_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.KeyCode.Equals(Keys.Enter)) || e.KeyCode.Equals(Keys.Enter))
            {
                var found = false;

                if (e.Control && e.KeyCode.Equals(Keys.Enter))
                {
                    found = Find(SearchOrientation.Up);
                }
                else if (e.KeyCode.Equals(Keys.Enter))
                {
                    found = Find(SearchOrientation.Down);
                }

                if (!found)
                {
                    MessageBox.Show("No found!");
                    btnFindUp.Focus();
                }
            }
        }

        private void txtPattern_KeyUp(object sender, KeyEventArgs e)
        {
            timer.Stop();
            timer.Start();
        }

        private void Time_Do(object sender, EventArgs e)
        {
            Filter();
            timer.Stop();
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            isLeave = false;
            if (patternSelectionForm == null || patternSelectionForm.IsDisposed)
                patternSelectionForm = new PatternSelectionForm();

            var pattern = new Pattern();
            patternSelectionForm.PatternSelected += (p) =>
            {
                pattern = p;
            };

            patternSelectionForm.FormClosed += (s, ea) =>
            {
                if (!isLeave)
                {
                    lblPatternName.Text = pattern.PatternName;
                    txtPattern.Text = pattern.PatternValue;
                    Filter();
                }
            };
            patternSelectionForm.BringToFront();
            patternSelectionForm.Show();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                PatternHelper.SavePattern(new Pattern() { PatternName = lblPatternName.Text.Trim(), PatternValue = txtPattern.Text.Replace(Environment.NewLine, "") });

                MessageBox.Show("Save/Update pattern successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save/Update pattern fail." + Environment.NewLine + ex.Message, "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
