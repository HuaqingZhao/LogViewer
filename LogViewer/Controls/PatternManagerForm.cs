using System;
using System.Windows.Forms;
using LogViewer.Entities;
using LogViewer.Utilities;

namespace LogViewer.Controls
{
    public partial class PatternManagerForm : Form
    {
        private Pattern selectedPattern;

        public PatternManagerForm()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            patternCtrl1.PatternSelected += OnPatternSelected;
        }

        private void OnPatternSelected(Pattern pattern)
        {
            selectedPattern = pattern;
            txtPatternName.Text = selectedPattern.PatternName;
            txtPattern.Text = selectedPattern.PatternValue;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPatternName.Text))
            {
                try
                {
                    PatternHelper.SavePattern(new Pattern() { PatternName = txtPatternName.Text.Trim(), PatternValue = txtPattern.Text.Replace(Environment.NewLine, "") });

                    patternCtrl1.LoadPattern();
                    MessageBox.Show("Save/Update pattern successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Save/Update pattern fail." + Environment.NewLine + ex.Message, "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPatternName.Text))
            {
                try
                {
                    PatternHelper.DeletePattern(new Pattern() { PatternName = txtPatternName.Text.Trim(), PatternValue = txtPattern.Text.Replace(Environment.NewLine, "") });

                    patternCtrl1.LoadPattern();
                    MessageBox.Show("Delete pattern successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Delete pattern fail." + Environment.NewLine + ex.Message, "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
