using System;
using System.Drawing;
using System.Reflection;
namespace LogViewer
{
    public class Log
    {
        static Image performenceImage;
        static Image errorImage;
        static Image warnImage;
        static Image infoImage;
        static Image verboseImage;

        static Log()
        {
            var assembly = Assembly.GetExecutingAssembly();
            infoImage = new Bitmap(assembly.GetManifestResourceStream("LogViewer.Ico.info.ico"));
            performenceImage =  new Bitmap(assembly.GetManifestResourceStream("LogViewer.Ico.performence.ico"));
            errorImage = new Bitmap(assembly.GetManifestResourceStream("LogViewer.Ico.error.ico"));
            warnImage = new Bitmap(assembly.GetManifestResourceStream("LogViewer.Ico.warn.ico"));
            verboseImage = new Bitmap(assembly.GetManifestResourceStream("LogViewer.Ico.verbose.ico"));
        }

        public Log()
        {
        }

        public Guid LogGuid { get; set; }
        public DateTime LogTime { get; set; }
        public Image LogLevelImage
        {
            get
            {
                Image result = null;
                switch (Severity)
                {
                    case "0":
                        result = performenceImage;
                        break;
                    case "1":
                        result = errorImage;
                        break;
                    case "2":
                        result = warnImage;
                        break;
                    case "3":
                        result = infoImage;
                        break;
                    case "4":
                        result = verboseImage;
                        break;
                }

                return result;
            }
        }
        public string Severity { get; set; }
        public string FileName { get; set; }
        public string LineNumber { get; set; }
        public string FunctionName { get; set; }
        public string RaisedErrorNamespace { get; set; }
        public string PatientGuid { get; set; }
        public string ImageGuid { get; set; }
        public string UserName { get; set; }
        public string UserDefinedKey { get; set; }
        public string ErrorNumber { get; set; }
        public string ExceptionName { get; set; }
        public string ExceptionString { get; set; }
        public string InnerException { get; set; }
        public string MachineName { get; set; }
        public string StackTrace { get; set; }
        public string ThreadName { get; set; }
        public string ThreadId { get; set; }
        public string ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string AssemblyName { get; set; }
        public string EventSequenceNumber { get; set; }
        public string RequestSequenceNumber { get; set; }
        public string EventSourceInstance { get; set; }
        public string Message { get; set; }
    }
}
