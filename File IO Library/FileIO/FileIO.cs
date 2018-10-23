using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileIOLib
{
    public static class FileIO
    {
        public static string[] Splitter = { ":",";",","};
        static public string[] Split(string line)
        {
            var seps = new string[2] { ",", ":" };
            var words = line.Split(seps, StringSplitOptions.RemoveEmptyEntries);
            return words;
        }
        static public List<string> ReadDataTextFile(string fileName)
        {
            List<string> file = new List<string>();
            try{
                if (fileName!= null && fileName != ""  && System.IO.File.Exists(fileName)) 
                {
                    using (System.IO.StreamReader sr = System.IO.File.OpenText(fileName))
                    {
                        string line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            file.Add(line);
                        }
                    }
                }
                else
                {
                    throw new System.IO.FileNotFoundException(fileName + " Not Found");
                }
                return file;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }
        ///return <list> container of strings values from file with following format label delimiter value        
        static public List<string> ReadParamsTextFile(string fileName, bool delimited, char[] delimiters)
        {
            List<string> file = new List<string>(0);

            char[] sep = { ';', ',', '=' };

            if (delimited && delimiters.Length > 0)
            {
                sep = delimiters;
            }
            try{
                if (fileName!= null && fileName != ""  && System.IO.File.Exists(fileName)) 
                {
                    using (System.IO.StreamReader sr = System.IO.File.OpenText(fileName))
                    {
                        string line = "";

                        while ((line = sr.ReadLine()) != null)
                        {
                            int index = line.IndexOfAny(sep);                            
                            if (line != "")
                            {
                                if (delimited && index >= 0)
                                {
                                    file.Add(line.Substring(index + 1));                              
                                }
                                if (!delimited)
                                {
                                    file.Add(line);
                                }
                            }
                        }
                    }
                }
                return file;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
           
        }
        static public void Save(string[] file, string fileName)
        {
            try
            {
                if (fileName != "")
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName))
                    {
                        foreach (string line in file)
                        {
                            sw.WriteLine(line);
                        }
                    }
                }

            }
            catch (System.IO.IOException)
            {
                throw new System.IO.IOException("File in use. Please close File and try again.");
            }

        }
        static public void Save(List<String> file, string fileName)
        {
            try
            {
                if (fileName != "")
                {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName))
                {
                    foreach (string line in file)
                    {
                        sw.WriteLine(line);
                    }
                }
                }

            }
            catch(System.IO.IOException)
            {
                throw new System.IO.IOException("File in use. Please close File and try again.");
            }
           
        }
      
    }
}
