

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LogViewer.Utilities;
namespace LogViewer.Controls
{
    public partial class FullLogCtrl : UserControl
    {
        private FullLogCtrlController controller;

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

        public FullLogCtrl()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            controller = new FullLogCtrlController();

            dgvMain.Dock = DockStyle.Fill;
            Dock = DockStyle.Fill;

            BackColor = LogManager.BackGroundColor;
            ForeColor = LogManager.ForceColor;

            splitContainer1.SplitterDistance = (int)(Width * 0.7d);

            InitializeHighLightCmb();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            //dgvMain.DoubleClick -= dgvMain_DoubleClick;
            //dgvMain.KeyUp -= dgvMain_KeyUp;
            //LogManager.LogSelectedChanged -= OnLogSelectedChanged;
            dgvMain.SelectionChanged -= dgvMain_SelectionChanged;
            dgvMain.Resize -= dgvMain_Resize;
            LogManager.LoadControl -= OnLoadControl;
            txtHighlight.TextChanged -= txtHighlight_TextChanged;
            btnFindDown.Click -= btnFindDown_Click;
            btnFindUp.Click -= btnFindUp_Click;
            //LogManager.StyleChanged -= OnStyleChanged;
            //LogManager.CaseSensitiveChanged -= OnCaseSensitiveChanged;
            //LogManager.EventTimeChanged -= OnEventTimeChanged;

            //dgvMain.DoubleClick += dgvMain_DoubleClick;
            //dgvMain.KeyUp += dgvMain_KeyUp;
            ////dgvMain.CellFormatting += dgvMain_CellFormatting;
            //LogManager.LogSelectedChanged += OnLogSelectedChanged;
            dgvMain.SelectionChanged += dgvMain_SelectionChanged;
            LogManager.LoadControl += OnLoadControl;
            dgvMain.Resize += dgvMain_Resize;
            txtHighlight.TextChanged += txtHighlight_TextChanged;
            btnFindDown.Click += btnFindDown_Click;
            btnFindUp.Click += btnFindUp_Click;
            //LogManager.StyleChanged += OnStyleChanged;
            //LogManager.CaseSensitiveChanged += OnCaseSensitiveChanged;
            //LogManager.EventTimeChanged += OnEventTimeChanged;
        }

        private void txtHighlight_TextChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvMain.Rows)
            {
                row.Cells[4].Style.BackColor = LogManager.BackGroundColor;
            }

            if (string.IsNullOrEmpty(txtHighlight.Text)) return;

            var highLights = txtHighlight.Text.Split(new string[] { ";" }, StringSplitOptions.None);

            var highed = new List<string>();

            for (int i = 0; i < highLights.Length; i++)
            {
                if (string.IsNullOrEmpty(highLights[i])) continue;
                if (highed.Contains(highLights[i].ToUpper())) continue;

                foreach (DataGridViewRow row in dgvMain.Rows)
                {
                    if (row.Cells[4].Value.ToString().ToUpper().Contains(highLights[i].ToUpper()))
                    {
                        row.Cells[4].Style.BackColor = Color.FromArgb((i * 15 + 163) % 255, (i * 112 + 226) % 255, (i * 158 + 646) % 255);
                        highed.Add(highLights[i].ToUpper());
                    }
                }
            }
        }

        private void OnLoadControl()
        {
            ShowLog(LogManager.LogList);
        }

        public void ShowLog(IList<Log> displaylogs)
        {
            dgvMain.DataSource = displaylogs;
            lblCount.Text = displaylogs.Count.ToString();
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

        private void ScrollToRow(int selectedIndex)
        {
            if (dgvMain.Rows.Count - 1 >= selectedIndex)
            {
                dgvMain.CurrentCell = dgvMain[4, selectedIndex];
                //dgvMain.Update();
            }
        }

        private void btnHighLight_Click(object sender, EventArgs e)
        {
            var highLightManagerForm = new HighLightManagerForm();
            highLightManagerForm.HighLightChanged += HighLightManagerForm_HighLightChanged;
            highLightManagerForm.Show();
        }

        private void HighLightManagerForm_HighLightChanged()
        {
            InitializeHighLightCmb();
        }

        private void InitializeHighLightCmb()
        {
            cmbHighLight.Items.Clear();
            foreach (var item in HighLightHelper.LoadHighLights())
            {
                cmbHighLight.Items.Add(item.HighLightValue);
            }
        }

        private void cmbHighLight_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtHighlight.Text += ";" + cmbHighLight.Text;
        }
    }
}
