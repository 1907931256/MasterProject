using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbMachModel
{
    /// <summary>
    /// converts abmach params file to xml and vice versa
    /// </summary>
    public class AbMachParametersFile
    {
        public static AbMachParameters Open(string fileName)
        {            
            AbMachParameters parms= FileIOLib.XmlSerializer.OpenXML<AbMachParameters>(fileName);
            if(parms==null)
            {
                return new AbMachParameters();
            }
            else
            {
                return parms;
            }
            
        }
        public static void Save(AbMachParameters parameters, string fileName)
        {
            FileIOLib.XmlSerializer.SaveXML(parameters,fileName);
            
        }
    }
}
