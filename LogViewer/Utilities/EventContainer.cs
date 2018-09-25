using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogsViewer.Utilities
{
    public class EventContainer
    {
        /// <summary>
        /// Using to notify that the pattern items was changed
        /// </summary>
        public static Action PatternCountChanged { get; set; }

        /// <summary>
        /// Using to notify that a pattern was selected. 
        /// </summary>
        public static Action PatternCountChanged { get; set; }
    }
}
