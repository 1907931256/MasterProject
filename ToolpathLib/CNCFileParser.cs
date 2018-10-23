using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileIOLib;
namespace ToolpathLib
{
    public class CNCFileParser
    {
        static public ToolPath ToPath(List<string> file,string filename)
        {
            try
            {
                ToolPath toolpath = new ToolPath();
                NCFileType fileType = selectFileType(filename);
                if (file.Count > 0)
                {
                    switch (fileType)
                    {
                        case NCFileType.NCIFile:
                            NciFileParser ncifile = new NciFileParser();
                            toolpath = ncifile.ParsePath(file);
                            break;

                        case NCFileType.NCFile:
                        
                            NcFileParser ncfile = new NcFileParser();
                            toolpath = ncfile.ParsePath(file);
                            break;
                    }
                }
                return toolpath;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        static public ToolPath ToPath(string fileName)
        {
            try
            {
                var file = new List<string>();
                NCFileType fileType = NCFileType.NCFile;

                if (fileName != null && fileName != "" && System.IO.File.Exists(fileName))
                {
                     file = FileIO.ReadDataTextFile(fileName);
                     fileType = selectFileType(fileName);
                }
                else
                {
                    throw new Exception("File Not readable");
                }
                return ToPath(file,fileName);  

            }
            catch
            {
                throw;
            }       
            
        }
        static private List<string> ncFileExtensions = new List<string>();
        static private string nciFileExt = "NCI";
        static private NCFileType selectFileType(string fileName)
        {
            string fileExt = System.IO.Path.GetExtension(fileName);
            fileExt = fileExt.ToUpper();
            if (fileExt.Contains(nciFileExt))
                return NCFileType.NCIFile;
            else
                return NCFileType.NCFile;
        }

    }
}
