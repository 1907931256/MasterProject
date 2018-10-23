using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolpathLib
{
    public class CNCMachineCodeFile
    {
        static string xmlFileName = "CNCMachineCode.xml";
        public static CNCMachineCode Open()
        {

            if (xmlFileName != null && xmlFileName != "" && System.IO.File.Exists(xmlFileName))
            {
                CNCMachineCode mc= FileIOLib.XmlSerializer.OpenXML<CNCMachineCode>(xmlFileName);
                if (mc == null)
                {
                    return new CNCMachineCode();
                }
                else
                {
                    return mc;
                }
            }

             return new CNCMachineCode();
           
        }
        public static void Save(CNCMachineCode obj)
        {
            FileIOLib.XmlSerializer.SaveXML<CNCMachineCode>(obj, xmlFileName);
        }
    }
}
