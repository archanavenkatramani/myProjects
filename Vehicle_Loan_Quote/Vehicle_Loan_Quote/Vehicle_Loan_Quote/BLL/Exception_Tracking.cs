using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Vehicle_Loan_Quote.BLL
{
    public class Exception_Tracking
    {
        #region MyRegion

        string strPath = "";
        string strFileName = "";
        string strFilePath = "";

        #endregion
        public void ErrorLog(string strException)
        {
            try
            {
                strFileName = "Log_" + DateTime.Now.Date.ToString("dd_MM_yyyy") + ".txt";
                var exePath = Path.GetDirectoryName(System.Reflection
                   .Assembly.GetExecutingAssembly().CodeBase);
                Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
                var appRoot = appPathMatcher.Match(exePath).Value;
                strPath = appRoot + "\\" + ConfigurationManager.AppSettings["logFilePath"].ToString();
                strFilePath = strPath + "\\" + strFileName;
                
                if (!File.Exists(strFilePath))
                {
                    File.Create(strFilePath).Dispose();

                    using (TextWriter tw = new StreamWriter("Error => " + strFilePath))
                    {
                        tw.WriteLine(strException);
                    }

                }
                else if (File.Exists(strFilePath))
                {
                    using (TextWriter tw = new StreamWriter(strFilePath,true))
                    {                        
                        tw.WriteLine(Environment.NewLine + "Error => " + strException);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}