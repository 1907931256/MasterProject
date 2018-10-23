using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileIOLib;
namespace InspectionLib
{
   public class DataOptionsFile:SettingsFile<DataOutputOptions>
    {
        public DataOptionsFile()
        {
            Extension = ".xml";
            DefaultFileName = "DataOptions.xml";
        }
    }
}
