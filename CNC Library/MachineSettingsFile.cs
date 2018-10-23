using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CNCLib
{
    /// <summary>
    /// saves and opens machine settings file in xml 
    /// </summary>
    public class MachineSettingsFile
    {
        static string xmlFileName = "MachineSettings.msx";
        public static MachineSettings Open()
        {

            return Open(xmlFileName);
        }
        public static MachineSettings Open(string fileName)
        {

            if (fileName != null && fileName != "" && System.IO.File.Exists(fileName))
            {
                MachineSettings ms = FileIOLib.XmlSerializer.OpenXML<MachineSettings>(fileName);
                if (ms == null)
                {
                    return new MachineSettings();
                }
                else
                {
                    return ms;
                }
            }
            else
            {
                return new MachineSettings();
            }
        }
        public static void Save(MachineSettings obj,string fileName)
        {
            FileIOLib.XmlSerializer.SaveXML<MachineSettings>(obj, fileName);
        }
        public static void Save(MachineSettings obj)
        {
            Save(obj, xmlFileName);
        }
    }
}
