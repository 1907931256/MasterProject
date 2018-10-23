using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileIOLib
{
    public abstract class SettingsFile<T>
    {

        public static string Extension;
        protected static string DefaultFileName;
        
        /// <summary>
        /// opens xml serialized file and returns obj
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T Open(string fileName)
        {
            try
            {
                if (fileName != null && fileName != "" && System.IO.File.Exists(fileName))
                {
                    return OpenXML(fileName);
                }
                else
                {
                    throw new Exception(fileName + " File not found.");
                }
            }
            catch (Exception )
            {
                throw ;
            }
        }
       
        /// <summary>
        /// saves obj as xml serialized file
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fileName"></param>
        public static void Save(T obj, string fileName)
        {
            try
            {
                if (obj != null && fileName != null && fileName != "")
                {
                    System.IO.Path.ChangeExtension(fileName, Extension);
                    SaveXML(obj, fileName);
                }
            }
            catch (Exception )
            {
                throw ;
            }
        }
       
        static T OpenXML(string fileName)
        {
            T c = FileIOLib.XmlSerializer.OpenXML<T>(fileName);
            return c;
        }
       
       
        static void SaveXML(T obj, string fileName)
        {
            FileIOLib.XmlSerializer.SaveXML<T>(obj, fileName);

        }
    }
}
