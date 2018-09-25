using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogViewer.Controls;

namespace LogViewer.Utilities
{
    public class PatternSelectionFormSingleton
    {
        public static readonly PatternSelectionForm Instance = new PatternSelectionForm();

        public static void ShowForm()
        {
            Instance.BringToFront();
            Instance.Show();
        }
    }
}
