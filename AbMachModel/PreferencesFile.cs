using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileIOLib;
using AbMachModel;
namespace AbMachModel
{
    /// <summary>
    /// opens and saves preferences as xml seriliazed obj
    /// </summary>
    public class PreferencesFile:SettingsFile<Preferences>
    {

        
           public static string  Filter = "Preferences files(*.prfx)|*.prfx";
           public static string  TempFileName = "preferences.prfx";
           new public static string  Extension = ".prfx";
           new public static string  DefaultFileName = TempFileName;
        
       
        /// <summary>
        /// opens xml file and returns preferences
        /// </summary>
        /// <returns></returns>
        public static Preferences Open()
        {
            if (System.IO.File.Exists(DefaultFileName))
            {
                Preferences pf = FileIOLib.XmlSerializer.OpenXML<Preferences>(DefaultFileName);
                if (pf == null)
                {
                    return new Preferences();
                }
                else
                {
                    return pf;
                }
            }
            else
            {
                return new Preferences();
            }
        }
        public static void Save(Preferences prefs)
        {
            FileIOLib.XmlSerializer.SaveXML<Preferences>(prefs, DefaultFileName);
        }
       
    }
}
