using System;
using System.Windows.Forms;
using LogViewer.Entities;
using LogViewer.Utilities;

namespace LogViewer.Controls
{
    public partial class HighLightManagerForm : Form
    {
        private HighLight selectedHighLight;

        public Action HighLightChanged;

        public HighLightManagerForm()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            highLightCtrl1.HighLightSelected += OnHighLightSelected;
        }

        private void OnHighLightSelected(HighLight highLight)
        {
            selectedHighLight = highLight;
            txtHighLightName.Text = selectedHighLight.HighLightName;
            txtHighLight.Text = selectedHighLight.HighLightValue;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtHighLightName.Text))
            {
                try
                {
                    HighLightHelper.SaveHighLight(new HighLight() { HighLightName = txtHighLightName.Text.Trim(), HighLightValue = txtHighLight.Text.Replace(Environment.NewLine, "") });

                    highLightCtrl1.LoadHighLight();
                    MessageBox.Show("Save/Update high light successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (HighLightChanged != null)
                        HighLightChanged();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Save/Update high light fail." + Environment.NewLine + ex.Message, "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtHighLightName.Text))
            {
                try
                {
                    HighLightHelper.DeleteHighLight(new HighLight() { HighLightName = txtHighLightName.Text.Trim(), HighLightValue = txtHighLight.Text.Replace(Environment.NewLine, "") });

                    highLightCtrl1.LoadHighLight();
                    MessageBox.Show("Delete high light successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (HighLightChanged != null)
                        HighLightChanged();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Delete high light fail." + Environment.NewLine + ex.Message, "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
