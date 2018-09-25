using System.Windows.Forms;
using LogViewer.Utilities;
using LogViewer.Entities;
using System;

namespace LogViewer.Controls
{
    public partial class PatternCtrl : UserControl
    {
        public Action<Pattern> PatternSelected;

        public PatternCtrl()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadPattern();
            SetColumnWidth();
        }

        public void LoadPattern()
        {
            if (!this.DesignMode)
                dgvPatterns.DataSource = PatternHelper.LoadPatterns();
        }

        private void dgvPatterns_Resize(object sender, EventArgs e)
        {
            SetColumnWidth();
        }

        private void SetColumnWidth()
        {
            var width = dgvPatterns.Width;
            var messageClumWidth = width;

            if (messageClumWidth > 0)
            {
                dgvPatterns.Columns[0].Width = 200;
                dgvPatterns.Columns[1].Width = messageClumWidth - 224;
            }
        }

        private void dgvPatterns_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            OnPatternSelected();
        }

        private void OnPatternSelected()
        {
            if (dgvPatterns.SelectedRows.Count == 1)
            {
                var pattern = new Pattern();
                pattern.PatternName = dgvPatterns.SelectedRows[0].Cells[0].Value.ToString();
                pattern.PatternValue = dgvPatterns.SelectedRows[0].Cells[1].Value.ToString();

                var localEventHandler = PatternSelected;
                if (localEventHandler != null)
                    localEventHandler(pattern);
            }
        }

        private void dgvPatterns_SelectionChanged(object sender, EventArgs e)
        {
            OnPatternSelected();
        }
    }
}
