using System;
using System.IO;
using System.Configuration;

namespace Commons
{
    public class LogController
    {
        private static string location = "";
        
        public static void WriteLog(string methodName, string errMsg)
        {
            if(location=="")
                location = ConfigurationManager.AppSettings["log_location"];

            DateTime thisNow = DateTime.Now;
            string strThisDate = thisNow.ToString("yyyyMMdd");
            string strThisTime = thisNow.ToString("HH:mm:ss");

            // Compose a string that consists of three lines.
            string lines = strThisDate + " " + strThisTime + " - (" + methodName + "): " + errMsg;
            StreamWriter file = null;
            try
            {
                // Write the string to a file.
                file = new StreamWriter(location + strThisDate + ".log", true);
                file.WriteLine(lines);

                file.Close();
            }
            catch (Exception ex)
            {
                string msg = ex.ToString();
                if (file != null)
                    file.Close();
            }

            System.Threading.Thread.Sleep(100);
        }

        public static void CheckLogFile()
        {
            location = ConfigurationManager.AppSettings["log_location"];
            DateTime thisNow = DateTime.Now;
            string strThisDate = thisNow.ToString("yyyyMMdd");
            string strBeginDate = thisNow.AddDays(-7).ToString("yyyyMMdd");

            FileInfo finfo = new FileInfo(location + strThisDate + ".log");

            if (!finfo.Exists)
            {
                // Create a reference to a file.
                FileInfo fi = new FileInfo(location + strThisDate + ".log");
                // Actually create the file.
                FileStream fs = fi.Create();
                // Modify the file as required, and then close the file.
                fs.Close();
            }
            else
            {
                FileInfo finfo1 = new FileInfo(location + strBeginDate + ".log");
                if (finfo1.Exists)
                {
                    finfo1.Delete();
                }
            }
        }
    }
}
