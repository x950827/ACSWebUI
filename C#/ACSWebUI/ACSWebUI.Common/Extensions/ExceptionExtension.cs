using System;
using System.IO;
using System.Text;

namespace ACSWebUI.Common.Extensions {
    public static class ExceptionExtension {
        private static readonly object sync = new object();
        public static void ToLog(this Exception e) {
            try {
                var pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                if (!Directory.Exists(pathToLog))
                    Directory.CreateDirectory(pathToLog);
                var filename = Path.Combine(pathToLog, $"{AppDomain.CurrentDomain.FriendlyName}_{DateTime.Now:dd.MM.yyy}.log");
                var fullText = $"[{DateTime.Now:dd.MM.yyy HH:mm:ss.fff}] [{e.TargetSite.DeclaringType}.{e.TargetSite.Name}()] {e.Message}\r\n";
                lock (sync) {
                    File.AppendAllText(filename, fullText, Encoding.GetEncoding("Windows-1251"));
                }
            }
            catch {
                // ignored
            }
        }
    }
}