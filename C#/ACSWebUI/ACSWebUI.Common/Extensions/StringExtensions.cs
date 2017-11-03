using System;
using System.IO;
using System.Net;
using System.Text;

namespace ACSWebUI.Common.Extensions {
    public static class StringExtensions {
        private static readonly object sync = new object();
        public static void ToLog(this string message) {
            try {
                var pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                if (!Directory.Exists(pathToLog))
                    Directory.CreateDirectory(pathToLog); // Создаем директорию, если нужно
                var filename = Path.Combine(pathToLog, $"{AppDomain.CurrentDomain.FriendlyName}_{DateTime.Now:dd.MM.yyy}.log");
                var fullText = $"[{DateTime.Now:dd.MM.yyy HH:mm:ss.fff}] {message}\r\n";
                lock (sync) {
                    File.AppendAllText(filename, fullText, Encoding.GetEncoding("Windows-1251"));
                }
            }
            catch {
                // ignored
            }
        }

        public static string FindCode(this string response) {
            if (!response.Contains("69 61 6D 61  6C 69 76 65"))
                return "false";
            response = response.Replace(" ", string.Empty);
            var a = response.IndexOf("69616D616C697665", StringComparison.Ordinal);
            var result = response.Substring(a + 16, 16);
            return result;
        }

        public static byte[] FromHex(this string hex) {
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++) {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        public static string ToHttpGetRequest(this string data, string url) {
            try {
                var request = (HttpWebRequest)WebRequest.Create(url + data);
                var cc = new CookieContainer();
                request.CookieContainer = cc;
                var response = (HttpWebResponse)request.GetResponse();

                foreach (Cookie c in response.Cookies) {
                    cc.Add(c);
                }

                var finalRequest = (HttpWebRequest)WebRequest.Create(url + data);
                finalRequest.CookieContainer = cc;
                var streamReader = new StreamReader(finalRequest.GetResponse().GetResponseStream() ?? throw new InvalidOperationException());
                var result = streamReader.ReadToEnd();
                streamReader.Close();
                return result;
            }
            catch (InvalidOperationException e) {
                e.ToLog();
                return "NoAnswer";
            }
            catch (Exception e) {
                e.ToLog();
                return "Unknow";
            }
        }
    }
}