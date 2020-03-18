using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Hosts
{
    partial class File : Log, IDisposable
    {
        public bool Change(string find, string replace)
        {
            string pathToFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers/etc/hosts");
            try
            {
                string content = string.Empty;
                using (StreamReader sr = new StreamReader(pathToFile))
                {
                    content = sr.ReadToEnd();
                    sr.Close();
                }

                if (content.Contains(find))
                {
                    content = Regex.Replace(content, find, replace);
                } else
                {
                    content = string.Empty;
                    return false;
                }

                using (StreamWriter sw = new StreamWriter(pathToFile))
                {
                    sw.Write(content);
                    sw.Close();
                }
                WriteToFile("File changed");
                content = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                WriteToFile("File Error, " + ex.Message, "error");
                return false;
            }
        }
    }
}
