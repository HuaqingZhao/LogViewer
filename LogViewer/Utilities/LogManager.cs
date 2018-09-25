using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using System.Linq;

namespace LogViewer
{
    public class LogManager
    {
        private static IList<Log> logList;

        private static IDictionary<string, string> patterns;

        public static Action<string, Guid> LogSelectedChanged;

        public static Action LoadControl;

        public static Action StyleChanged;

        public static Action CaseSensitiveChanged;

        public static Action<string> ShowMask;

        public static Action HideMask;

        public static IList<Guid> SelectedLogList { get; set; }

        public static Color BackGroundColor { get; set; }

        public static Brush BackGroundBrush { get; set; }

        public static Color ForceColor { get; set; }

        public static Brush ForceBrush { get; set; }

        public static bool CaseSensitive { get; set; }

        public static Action<DateTime, DateTime> EventTimeChanged;

        public static IList<Log> LogList
        {
            get
            {
                return OrderSameTime(logList.OrderByDescending(x => x.LogTime.Ticks).ToList());
            }
        }

        private static IList<Log> OrderSameTime(IList<Log> logList)
        {
            var res = logList;

            var maxCount = logList.Count;
            var sameTimeCount = 0;
            IList<Log> tempLogList;

            for (int index = 0; index < maxCount; )
            {
                sameTimeCount = 1;
                tempLogList = new List<Log>();
                tempLogList.Add(logList[index]);
                for (int j = 1; j < 100; j++)
                {
                    if (index + j < maxCount && logList[index].LogTime.Equals(logList[index + j].LogTime))
                    {
                        tempLogList.Add(logList[index + j]);
                        sameTimeCount++;
                    }
                    else
                        break;
                }

                if (sameTimeCount > 1)
                {
                    tempLogList = tempLogList.Reverse().ToList<Log>();

                    for (int i = 0; i < sameTimeCount; i++)
                    {
                        logList[index + i] = tempLogList[i];
                    }

                    index += sameTimeCount;
                }
                else
                    index++;
            }

            return logList;
        }

        static LogManager()
        {
            SelectedLogList = new List<Guid>();
            LogManager.ForceBrush = Brushes.Black;
            LogManager.ForceColor = Color.Black;
            LogManager.BackGroundBrush = Brushes.White;
            LogManager.BackGroundColor = Color.White;
        }

        public static IDictionary<string, string> Patterns
        {
            get
            {
                return patterns;
            }
        }

        public static void Load(IList<string> paths)
        {
            patterns = LoadPattern();

            logList = new List<Log>();

            foreach (var path in paths)
            {
                ExtractLog(path);
            }
        }

        private static IDictionary<string, string> LoadPattern()
        {
            var pats = new Dictionary<string, string>();
            var patternPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pattern.xml");

            var doc = new XmlDocument();
            doc.Load(patternPath);
            var nodes = doc.SelectNodes("//pattern");

            foreach (XmlNode item in nodes)
            {
                var name = item.Attributes["name"].Value.ToString();
                var val = item.Attributes["value"].Value.ToString();

                if (!string.IsNullOrEmpty(name) && !pats.ContainsKey(name))
                {
                    pats.Add(name, val);
                }
            }
            return pats;
        }

        private static void ExtractLog(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            var source = File.ReadAllText(path, Encoding.GetEncoding(1251));

            if (string.IsNullOrEmpty(source)) return;

            string splitor = string.Empty;

            // get splitor from file name
            var fileName = Path.GetFileNameWithoutExtension(path);

            splitor = fileName.Substring(fileName.IndexOf("_") + 1, 5);

            // get splitor from content if splitor still null
            if (string.IsNullOrEmpty(splitor))
            {
                if (source.StartsWith("0001-"))
                {
                    for (int i = 0; i < 100; i++)
                    {
                        splitor = string.Format("20{0:D2}-", i);
                        if (source.IndexOf(splitor) > -1)
                        {
                            break;
                        }
                    }
                }
                else
                    splitor = source.Substring(0, 5);
            }

            if (string.IsNullOrEmpty(splitor)) return;

            var pros = source.Split(new[] { splitor }, StringSplitOptions.None);

            var tempLogList = new List<Log>();

            foreach (var item in pros)
            {
                var log = new Log();

                var logProperties = item.Split(new[] { "§" }, StringSplitOptions.None);

                if (logProperties.Length != 29) continue;
                if (logProperties[0].Contains("0001-")) continue;

                log.LogGuid = Guid.NewGuid();
                log.LogTime = DateTime.Parse(splitor + logProperties[0]);
                log.Severity = logProperties[1];
                log.FileName = logProperties[2];
                log.LineNumber = logProperties[3];
                log.FunctionName = logProperties[4];
                log.RaisedErrorNamespace = logProperties[5];
                log.PatientGuid = logProperties[6];
                log.ImageGuid = logProperties[7];
                log.UserName = logProperties[8];
                log.UserDefinedKey = logProperties[9];
                log.ErrorNumber = logProperties[10];
                log.ExceptionName = logProperties[12];
                log.ExceptionString = logProperties[13];
                log.InnerException = logProperties[14];
                log.MachineName = logProperties[15];
                log.StackTrace = logProperties[16];
                log.ThreadName = logProperties[17];
                log.ThreadId = logProperties[18];
                log.ProcessId = logProperties[19];
                log.ProcessName = logProperties[20];
                log.AssemblyName = logProperties[23];
                log.EventSequenceNumber = logProperties[25];
                log.RequestSequenceNumber = logProperties[24];
                log.EventSourceInstance = logProperties[26];
                log.Message = logProperties[11];

                logList.Add(log);

                logProperties = null;
            }

            pros = null;
        }
    }
}
