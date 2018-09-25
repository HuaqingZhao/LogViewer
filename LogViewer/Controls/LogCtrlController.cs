using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace LogViewer
{
    public class LogCtrlController
    {
        protected string pattern;
        protected bool isTimeChanged;
        protected DateTime from;
        protected DateTime to;

        public string Pattern { get; set; }

        public void DefaultFilter(IList<Log> displaylogs, IList<Log> temp)
        {
            var ps = Pattern.Split(';');

            foreach (var item in displaylogs)
            {
                if (isTimeChanged && (item.LogTime < from || item.LogTime > to)) continue;

                foreach (var p in ps)
                {
                    if (string.IsNullOrEmpty(p)) continue;

                    if (p.StartsWith("@"))
                    {
                        FieldFilter(item, temp, p);
                    }
                    else if (p.Contains("+++"))
                    {
                        var ss = p.Split(new[] { "+++" }, StringSplitOptions.None);

                        var ContaineAll = false;
                        for (int i = 0; i < ss.Length; i++)
                        {
                            if (!item.Message.SenseContains(ss[i]))
                            {
                                break;
                            }

                            // final item, match all
                            if (i == ss.Length - 1)
                                ContaineAll = true;
                        }

                        if (ContaineAll)
                        {
                            temp.Add(item);
                            break;
                        }
                    }
                    else if (item.Message.SenseContains(p))
                    {
                        temp.Add(item);
                        break;
                    }
                }
            }
        }

        public void LevFilter(IList<Log> displaylogs, IList<Log> temp)
        {
            var ps = Pattern.Substring(4).Split(';');

            foreach (var item in displaylogs)
            {
                if (isTimeChanged && (item.LogTime < from || item.LogTime > to)) continue;

                foreach (var p in ps)
                {
                    if (string.IsNullOrEmpty(p)) continue;

                    if (item.Severity.Equals(p))
                    {
                        temp.Add(item);
                        break;
                    }
                }
            }
        }

        public void FieldFilter(Log item, IList<Log> temp, string pattern)
        {
            var reg = new Regex("@.*:");

            if (reg.IsMatch(pattern))
            {
                var field = reg.Match(pattern).Value.ToString();

                if (!string.IsNullOrEmpty(field))
                {
                    var tempf = field;
                    field = tempf.Substring(1, field.Length - 2);

                    var pt = typeof(Log).GetProperty(field);
                    if (pt != null)
                    {
                        var ps = pattern.Substring(pattern.IndexOf(": ") + 2).Split('|');

                        if (isTimeChanged && (item.LogTime < from || item.LogTime > to)) return;

                        foreach (var p in ps)
                        {
                            if (string.IsNullOrEmpty(p)) continue;

                            var p1 = typeof(Log).GetProperty(field);
                            if (p1 != null)
                            {
                                var fieldVal = p1.GetValue(item, null);

                                if (fieldVal.Equals(p))
                                {
                                    temp.Add(item);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ExcFilter(IList<Log> displaylogs, IList<Log> temp)
        {
            var ps = Pattern.Substring(4).Split(';');

            foreach (var item in displaylogs)
            {
                if (isTimeChanged && (item.LogTime < from || item.LogTime > to)) continue;

                var found = false;
                foreach (var p in ps)
                {
                    if (string.IsNullOrEmpty(p)) continue;

                    if (item.Message.SenseString().Contains(p.SenseString()))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    temp.Add(item);
            }
        }

        public void ExpFilter(IList<Log> displaylogs, IList<Log> temp)
        {
            var ps = Pattern.Substring(4).Split(';');

            foreach (var p in ps)
            {
                try
                {
                    if (string.IsNullOrEmpty(p)) continue;
                    if (p.Equals("[")) continue;
                    if (p.Equals("]")) continue;
                    if (p.Equals("[]")) continue;

                    var pattern = "^" + p + "$";
                    var reg = new Regex(p, RegexOptions.IgnoreCase);
                    foreach (var item in displaylogs)
                    {
                        if (isTimeChanged && (item.LogTime < from || item.LogTime > to)) continue;
                        if (reg.IsMatch(item.Message.SenseString()))
                        {
                            temp.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }
        }

        public void FoundItem(IList<string> keys, int i, SearchPattern pattern, string val, IList<Log> logs)
        {
            if (keys.Count == 1)
            {
                var temp = keys[0];
                if (temp.StartsWith("@"))
                {
                    var reg = new Regex("@.*:");

                    if (reg.IsMatch(temp))
                    {
                        var field = reg.Match(temp).Value.ToString();

                        if (!string.IsNullOrEmpty(field))
                        {
                            var tempf = field;
                            field = tempf.Substring(1, field.Length - 2);

                            var fieldKey = temp.Substring(tempf.IndexOf(":") + 2);

                            if (logs != null)
                            {
                                var log = logs[i];

                                var p = typeof(Log).GetProperty(field);
                                if (p != null)
                                {
                                    var fieldVal = p.GetValue(log, null);

                                    if (fieldVal != null)
                                    {
                                        val = fieldVal.ToString();
                                        keys = new List<string>();
                                        keys.Add(fieldKey);
                                        pattern = SearchPattern.Equals;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public string FormatFields(IList<Log> logs, int rowIndex)
        {
            var sb = new StringBuilder();
            if (logs != null && logs.Count > rowIndex)
            {
                var logGuid = logs[rowIndex].LogGuid;

                sb.AppendLine("LogTime: " + logs[rowIndex].LogTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));
                sb.AppendLine("GUID: " + logs[rowIndex].LogGuid.ToString());
                sb.AppendLine("Severity: " + logs[rowIndex].Severity);
                sb.AppendLine("FileName: " + logs[rowIndex].FileName);
                sb.AppendLine("LineNumber: " + logs[rowIndex].LineNumber);
                sb.AppendLine("FunctionName: " + logs[rowIndex].FunctionName);
                sb.AppendLine("RaisedErrorNamespace: " + logs[rowIndex].RaisedErrorNamespace);
                sb.AppendLine("PatientGuid: " + logs[rowIndex].PatientGuid);
                sb.AppendLine("ImageGuid: " + logs[rowIndex].ImageGuid);
                sb.AppendLine("UserName: " + logs[rowIndex].UserName);
                sb.AppendLine("UserDefinedKey: " + logs[rowIndex].UserDefinedKey);
                sb.AppendLine("ErrorNumber: " + logs[rowIndex].ErrorNumber);
                sb.AppendLine("ExceptionName: " + logs[rowIndex].ExceptionName);
                sb.AppendLine("ExceptionString: " + logs[rowIndex].ExceptionString);
                sb.AppendLine("InnerException: " + logs[rowIndex].InnerException);
                sb.AppendLine("MachineName: " + logs[rowIndex].MachineName);
                sb.AppendLine("StackTrace: " + logs[rowIndex].StackTrace);
                sb.AppendLine("ThreadName: " + logs[rowIndex].ThreadName);
                sb.AppendLine("ThreadId: " + logs[rowIndex].ThreadId);
                sb.AppendLine("ProcessId: " + logs[rowIndex].ProcessId);
                sb.AppendLine("ProcessName: " + logs[rowIndex].ProcessName);
                sb.AppendLine("AssemblyName: " + logs[rowIndex].AssemblyName);
                sb.AppendLine("EventSequenceNumber: " + logs[rowIndex].EventSequenceNumber);
                sb.AppendLine("RequestSequenceNumber: " + logs[rowIndex].RequestSequenceNumber);
                sb.AppendLine("EventSourceInstance: " + logs[rowIndex].EventSourceInstance);
                sb.AppendLine("Message: " + logs[rowIndex].Message);
            }

            return sb.ToString();
        }
    }
}
