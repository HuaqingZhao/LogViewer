using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LogViewer.Utilities;
using LogViewer.Entities;

namespace LogViewer.Controls
{
    public partial class HighLightCtrl : UserControl
    {
        public Action<HighLight> HighLightSelected;

        public HighLightCtrl()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadHighLight();
            SetColumnWidth();
        }

        public void LoadHighLight()
        {
            if (!this.DesignMode)
                dgvHighLights.DataSource = HighLightHelper.LoadHighLights();
;
        }

        private void dgvHighLights_Resize(object sender, EventArgs e)
        {
            SetColumnWidth();
        }

        private void SetColumnWidth()
        {
            var width = dgvHighLights.Width;
            var messageClumWidth = width;

            if (messageClumWidth > 0)
            {
                dgvHighLights.Columns[0].Width = 200;
                dgvHighLights.Columns[1].Width = messageClumWidth - 224;
            }
        }

        private void dgvHighLights_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            OnHighLightSelected();
        }

        private void OnHighLightSelected()
        {
            if (dgvHighLights.SelectedRows.Count == 1)
            {
                var highLight = new HighLight();
                highLight.HighLightName = dgvHighLights.SelectedRows[0].Cells[0].Value.ToString();
                highLight.HighLightValue = dgvHighLights.SelectedRows[0].Cells[1].Value.ToString();

                var localEventHandler = HighLightSelected;
                if (localEventHandler != null)
                    localEventHandler(highLight);
            }
        }

        private void dgvHighLights_SelectionChanged(object sender, EventArgs e)
        {
            OnHighLightSelected();
        }
    }
}
