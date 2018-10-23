using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileIOLib
{
    sealed public class LogFile
    {
        private static string _fileName;
        private static readonly LogFile _logfile = new LogFile();
        public string GetFileName()
        {
            return _fileName;
        }
        static public LogFile GetLogFile()
        {
            return _logfile;
        }
        private LogFile()
        {
            _fileName = "LogFile.txt";
        }
        public void ClearLog()
        {
            System.IO.File.Delete(_fileName);
           
        }
        public List<string> GetContents()
        {
            var fileContents = new List<string>();
            if (System.IO.File.Exists(_fileName))
                fileContents = FileIO.ReadDataTextFile(_fileName);
            return fileContents;
        }
        public void SaveMessage(string message)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(_fileName, append: true))
            {
              
                sw.Write(System.DateTime.Now.ToShortDateString() + ":");
                sw.WriteLine(System.DateTime.Now.ToLongTimeString() );
                sw.WriteLine(message);
                sw.WriteLine("******");
            }
        }
        public void SaveMessage(Exception ex)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(_fileName, append: true))
            {
                
                sw.Write(System.DateTime.Now.ToShortDateString() + ":");
                sw.WriteLine(System.DateTime.Now.ToLongTimeString() );
                sw.WriteLine("Exception: " + ex.ToString());
                if( ex.InnerException != null)
                { 
                   sw.WriteLine(ex.InnerException.ToString());
                }
                sw.WriteLine("StackTrace: "+ ex.StackTrace);
                sw.WriteLine("******");
            }
        }

    }
}
