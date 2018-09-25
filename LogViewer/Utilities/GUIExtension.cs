using System;
using System.Windows.Forms;

namespace LogViewer
{
    public static class GUIExtension
    {
        /// <summary>
        /// Invoke passed in action asynchronously on the GUI thread of this item.  Note: If this method is called
        /// on the GUI thread, the action will be performed synchronously. If it is called from another thread, it
        /// is invoked asynchronoulsly.
        /// </summary>
        /// <param name="me">The control.</param>
        /// <param name="action">Action that will be performed on the GUI thread.</param>
        public static void BeginInvokeOnGui(this UserControl me, Action action)
        {
            if (me.InvokeRequired)
                me.BeginInvoke(action);
            else
                action();
        }

        /// <summary>
        /// Invoke passed in action synchronously on the GUI thread of this item.  Note: If this method is called
        /// on the GUI thread, the action will be performed synchronously. If it is called from another thread, it
        /// is invoked asynchronoulsly.
        /// </summary>
        /// <param name="me">The control.</param>
        /// <param name="action">Action that will be performed on the GUI thread.</param>
        public static void InvokeOnGui(this UserControl me, Action action)
        {
            if (me.InvokeRequired)
                me.Invoke(action);
            else
                action();
        }

        /// <summary>
        /// Invoke passed in action synchronously on the GUI thread of this item.  Note: If this method is called
        /// on the GUI thread, the action will be performed synchronously. If it is called from another thread, it
        /// is invoked asynchronoulsly.
        /// </summary>
        /// <param name="me">The control.</param>
        /// <param name="action">Action that will be performed on the GUI thread.</param>
        public static void InvokeOnGui(this Form me, Action action)
        {
            if (me.InvokeRequired)
                me.Invoke(action);
            else
                action();
        }

        /// <summary>
        /// Invoke passed in action asynchronously on the GUI thread of this item.  Note: If this method is called
        /// on the GUI thread, the action will be performed synchronously. If it is called from another thread, it
        /// is invoked asynchronoulsly.
        /// </summary>
        /// <param name="me">The control.</param>
        /// <param name="action">Action that will be performed on the GUI thread.</param>
        public static void BeginInvokeOnGui(this Form me, Action action)
        {
            if (me.InvokeRequired)
                me.BeginInvoke(action);
            else
                action();
        }
    }
}
