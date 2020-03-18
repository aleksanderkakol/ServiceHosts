using System;
using System.Globalization;
using System.IO;

namespace Hosts
{
    class Log : IDisposable
    {
        public void WriteToFile(string Message, string type = "info")
        {
            string datetostring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("pl-PL"));
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            string pre = "[INFO] ";
            if (type == "error")
            {
                pre = "[" + type.ToUpper() + "] ";
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\Hosts.log";
            if (!System.IO.File.Exists(filepath))
            {
                using (StreamWriter sw = System.IO.File.CreateText(filepath))
                {
                    sw.WriteLine("[" + datetostring + "]" + pre + Message);
                }
            }
            else
            {
                using (StreamWriter sw = System.IO.File.AppendText(filepath))
                {
                    sw.WriteLine("[" + datetostring + "]" + pre + Message);
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
