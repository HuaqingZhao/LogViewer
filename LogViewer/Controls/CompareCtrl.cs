using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LogViewer
{
    public partial class CompareCtrl : UserControl
    {
        public CompareCtrl()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            LogManager.StyleChanged += OnStyleChanged;

            ShowLog();
        }

        void ShowLog()
        {
            dgvLeft.DataSource = LogManager.LogList;
            dgvRight.DataSource = LogManager.LogList;

            renderDv();
        }

        private void OnStyleChanged()
        {
            renderDv();
        }

        void renderDv()
        {
            dgvRight.RowHeadersDefaultCellStyle.BackColor = LogManager.BackGroundColor;
            dgvRight.ColumnHeadersDefaultCellStyle.BackColor = LogManager.BackGroundColor;

            dgvRight.RowsDefaultCellStyle.BackColor = LogManager.BackGroundColor;
            dgvRight.RowsDefaultCellStyle.ForeColor = LogManager.ForceColor;

            this.dgvRight.BackgroundColor = LogManager.BackGroundColor;

            dgvRight.Refresh();

            dgvLeft.RowHeadersDefaultCellStyle.BackColor = LogManager.BackGroundColor;
            dgvLeft.ColumnHeadersDefaultCellStyle.BackColor = LogManager.BackGroundColor;

            dgvLeft.RowsDefaultCellStyle.BackColor = LogManager.BackGroundColor;
            dgvLeft.RowsDefaultCellStyle.ForeColor = LogManager.ForceColor;

            this.dgvLeft.BackgroundColor = LogManager.BackGroundColor;

            dgvLeft.Refresh();
        }

        private void dgvLeft_Resize(object sender, EventArgs e)
        {
            var width = dgvLeft.Width;
            var messageClumWidth = width - 210;
            if (messageClumWidth > 0)
                dgvLeft.Columns[4].Width = messageClumWidth;
        }

        private void dgvRight_Resize(object sender, EventArgs e)
        {
            var width = dgvRight.Width;
            var messageClumWidth = width - 210;
            if (messageClumWidth > 0)
                dgvRight.Columns[1].Width = messageClumWidth;
        }

        private void RowSelected(DataGridView dv)
        {
            var index = dv.SelectedRows[0].Index;
            if (dv.Rows.Count <= index) return;

            if (dv.Rows[index].DefaultCellStyle.BackColor == Color.Empty)
            {
                dv.Rows[index].DefaultCellStyle.BackColor = Color.Red;
            }
            else
            {
                dv.Rows[index].DefaultCellStyle.BackColor = Color.Empty;
            }
        }

        private void MessageRowSelected(DataGridView dv, int rowIndex)
        {
            var messageIndex = dv.Name.Equals("dgvLeft") ? 4 : 1;

            if (dv.Rows.Count <= rowIndex) return;

            dv.Rows[rowIndex].Cells[messageIndex].Style.BackColor = Color.Orange;
        }

        private void CompareRowSelected(DataGridView dv)
        {
            var index = dv.SelectedRows[0].Index;

            var compareDV = dv.Name.Equals("dgvLeft") ? dgvRight : dgvLeft;
            var messageIndex = dv.Name.Equals("dgvLeft") ? 4 : 1;
            var comMessageIndex = dv.Name.Equals("dgvLeft") ? 1 : 4;
            var imageIndex = dv.Name.Equals("dgvLeft") ? 1 : 2;

            if (dv.Rows.Count <= index) return;

            var message = dv.SelectedRows[0].Cells[messageIndex].Value.ToString();

            //message = message.Length > 10 ? message.Substring(0, 10) : message;

            var logFlagIndexes = GetCompareFlagIndexs(compareDV);

            if (logFlagIndexes.Count == 2)
            {
                for (int i = logFlagIndexes[0]; i < logFlagIndexes[1] - 1; i++)
                {
                    compareDV.Rows[i].Cells[comMessageIndex].Style.BackColor = Color.Empty;
                }
            }

            var logFlagIndexes1 = GetCompareFlagIndexs(dv);

            if (logFlagIndexes1.Count == 2)
            {
                for (int i = logFlagIndexes1[0]; i < logFlagIndexes1[1] - 1; i++)
                {
                    dv.Rows[i].Cells[messageIndex].Style.BackColor = Color.Empty;
                }
            }

            if (logFlagIndexes.Count == 2)
            {
                for (int i = logFlagIndexes[0]; i < logFlagIndexes[1] - 1; i++)
                {
                    if (compareDV.Rows[i].Cells[comMessageIndex].Value.ToString().ToUpper().Equals(message.ToString().ToUpper()))
                    {
                        MessageRowSelected(compareDV, i);
                    }
                }
            }
        }

        IList<int> GetCompareFlagIndexs(DataGridView dv)
        {
            var result = new List<int>();
            var imageIndex = dv.Name.Equals("dgvLeft") ? 1 : 2;

            for (int i = 0; i < dv.Rows.Count - 1; i++)
            {
                if (dv.Rows[i].Cells[imageIndex].Style.BackColor == Color.Orange)
                {
                    result.Add(i);
                }
            }

            return result;
        }

        private void ImageRowSelected(DataGridView dv)
        {
            var cellIndex = dv.Name.Equals("dgvLeft") ? 1 : 2;
            var index = dv.SelectedRows[0].Index;
            if (dv.Rows.Count <= index) return;

            if (dv.Rows[index].Cells[cellIndex].Style.BackColor == Color.Empty)
            {
                dv.Rows[index].Cells[cellIndex].Style.BackColor = Color.Orange;
            }
            else
            {
                dv.Rows[index].Cells[cellIndex].Style.BackColor = Color.Empty;
            }
        }

        protected void ScrollToRow(DataGridView dv, int selectedIndex)
        {
            if (dv.Rows.Count - 1 >= selectedIndex)
            {
                dv.CurrentCell = dv[4, selectedIndex];
                dv.Update();
            }
        }

        private void dgvKeyUp(object sender, KeyEventArgs e)
        {
            var activeDV = ((DataGridView)sender).Name.Equals("dgvLeft") ? dgvLeft : dgvRight;

            //p = 80, n = 78
            if (e.Control) // ctrl keyborad pressed
            {
                switch (e.KeyCode)
                {
                    case Keys.K:
                        RowSelected(activeDV);
                        break;
                    case Keys.P:
                        var currentRow1 = activeDV.SelectedRows[0].Index;
                        for (int i = currentRow1 - 1; i > -1; i--)
                        {
                            if (activeDV.Rows[i].DefaultCellStyle.BackColor.Equals(Color.Red))
                            {
                                ScrollToRow(activeDV, i);
                                break;
                            }
                        }
                        break;
                    case Keys.N:
                        var currentRow = activeDV.SelectedRows[0].Index;
                        for (int i = currentRow + 1; i < activeDV.Rows.Count; i++)
                        {
                            if (activeDV.Rows[i].DefaultCellStyle.BackColor.Equals(Color.Red))
                            {
                                ScrollToRow(activeDV, i);
                                break;
                            }
                        }
                        break;
                    case Keys.M:
                        ImageRowSelected(activeDV);
                        break;
                }
            }
            else if (e.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                    case Keys.Down:
                        CompareRowSelected(activeDV);
                        break;
                }
            }
        }
    }
}
