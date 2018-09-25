using System;
using System.Windows.Forms;

namespace LogViewer
{
    public class GUIHelper
    {
        public static void ChangeStyle(Control c)
        {
            foreach (Control ctrl in c.Controls)
            {
                if (ctrl.HasChildren) ChangeStyle(ctrl);

                ctrl.BackColor = LogManager.BackGroundColor;
                ctrl.ForeColor = LogManager.ForceColor;
            }
        }
    }
}
