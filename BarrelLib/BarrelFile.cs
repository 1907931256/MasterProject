using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
using FileIOLib;
namespace BarrelLib
{
    public class BarrelFile:SettingsFile<Barrel>
    {
        public static string Filter = "Barrel files(*.brx)|*.brx";
        new public static string Extension = ".brx";
        public static string TempFileName = "TempBarrel.brx";
        new public static string DefaultFileName = TempFileName;
       
        
    }
}
