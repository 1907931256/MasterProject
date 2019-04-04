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
            var words = line.Split(seps, StringSplitOptions.None);
            return words;
        }
        static public string[] Split(string line,string[] seperators)
        {             
            var words = line.Split(seperators, StringSplitOptions.None);
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
                return file;
            }
            catch (Exception )
            {                
                throw;
            }
        }
        ///return <list> container of strings values from file with following format label delimiter value        
        static public List<string> ReadParamsTextFile(string fileName,  char[] delimiters)
        {
            List<string> file = new List<string>(0);
            try{
                if (fileName!= null && fileName != ""  && System.IO.File.Exists(fileName)) 
                {
                    using (System.IO.StreamReader sr = System.IO.File.OpenText(fileName))
                    {
                        string line = "";

                        while ((line = sr.ReadLine()) != null)
                        {
                            int index = line.IndexOfAny(delimiters);                            
                            if (line != "")
                            {                               
                              file.Add(line.Substring(index + 1));
                            }
                        }
                    }
                }
                return file;
            }
            catch (Exception )
            {                
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
            catch(Exception)
            {
                throw;
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
            catch (Exception)
            {
                throw;
            }
        }
      
    }
}
