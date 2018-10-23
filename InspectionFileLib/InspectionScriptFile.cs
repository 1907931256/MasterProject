using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileIOLib;
namespace InspectionLib
{
    /// <summary>
    /// saves and opens insp script files
    /// </summary>
    public class InspectionScriptFile:SettingsFile<InspectionScript>
    {
         public static string Filter = "Script Files (*.isx)|*.isx";
         new public static string  Extension = ".isx";
         public static string  TempFileName = "TempScript.isx";
         new public static string  DefaultFileName = TempFileName;    
    }
}
