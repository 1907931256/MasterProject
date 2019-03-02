using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileIOLib;
namespace ToolpathLib
{
    public class CNCFileParser
    {
        static ToolPath5Axis CreatePath(List<string> file,string filename)
        {
            try
            {
                ToolPath5Axis toolpath = new ToolPath5Axis();
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
        static public ToolPath5Axis CreatePath(string fileName)
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
                return CreatePath(file,fileName);  

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
