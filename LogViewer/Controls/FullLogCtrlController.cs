using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LogViewer.Controls
{
    public class FullLogCtrlController
    {
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
    }
}
