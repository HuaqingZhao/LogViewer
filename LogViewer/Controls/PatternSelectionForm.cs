using System;
using System.Windows.Forms;
using LogViewer.Entities;

namespace LogViewer.Controls
{
    public partial class PatternSelectionForm : Form
    {
        public Action<Pattern> PatternSelected;

        public PatternSelectionForm()
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
            var localEventHandler = PatternSelected;
            if (localEventHandler != null)
                localEventHandler(pattern);
        }
    }
}
