using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogsViewer.Utilities
{
    public class Extensions
    {
        public static void FireEvent(this Action obj)
        {
            var act = obj as Action;

            if (act != null)
                act();
        }

        public static void FireEvent(this Action<object> action, object obj)
        {
            var act = action as Action<object>;

            if (action != null)
                action(obj);
        }
    }
}
