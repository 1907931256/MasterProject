using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNCLib;
using DataLib;
namespace InspectionLib
{
    public class InspectionScriptBuilder
    {

        static string _linAxisName;
        static string _rotAxisName;
        static string _grooveName;
        static string _landName;



        static double getVal(string str, string name)
        {
            double val = 0.0;
            str = str.Trim();
            int len = name.Length;
            if (str.StartsWith(name))
            {
                val = Convert.ToDouble(str.Substring(len));
            }
            return val;
        }
        static double getVal(string str, string[] names)
        {
            double val = 0.0;
            str = str.Trim();
            foreach (string name in names)
            {
                int len = name.Length;
                if (str.StartsWith(name))
                {
                    val = Convert.ToDouble(str.Substring(len));
                    break;
                }
            }
            return val;
        }
        static MachinePosition FindStartPosition(string[] fileCodes, string axisName)
        {
            int firstX = 0;
            for(int i=0;i<fileCodes.Length;i++)
            {
                string word = fileCodes[i].ToUpper();
                
                if(word.Contains(axisName))
                {
                    firstX = i;
                    break;
                }
            }
            string xword = fileCodes[firstX].ToUpper();
            int xIndex = xword.IndexOf(axisName);
            string pos = xword.Substring(xIndex+1);
            double xpos = 0;
            double.TryParse(pos, out xpos);
            var mp = new MachinePosition(MachineGeometry.CYLINDER);
            mp.X = xpos;
            return mp;
        }
        static MachinePosition FindEndPosition(string[] fileCodes, string axisName)
        {
            int lastX = 0;
            for (int i = fileCodes.Length-1; i>=0; i--)
            {
                string word = fileCodes[i].ToUpper();

                if (word.Contains(axisName))
                {
                    lastX = i;
                    break;
                }
            }
            string xword = fileCodes[lastX].ToUpper();
            int xIndex = xword.IndexOf(axisName);
            string pos = xword.Substring(xIndex);
            double xpos = 0;
            double.TryParse(pos, out xpos);
            var mp = new MachinePosition(MachineGeometry.CYLINDER);
            mp.X = xpos;
            return mp;
        }
        static string[] ParseFilename(string filename)
        {
            var fileNoExt = System.IO.Path.GetFileNameWithoutExtension(filename);
            var splitters = new char[] { '_', '-' };
            var fileCodes = fileNoExt.Split(splitters);
            var fileCodeList = fileCodes.ToList();
            fileCodeList.Remove("out");
            return fileCodeList.ToArray();
        }

        
       
        static ScanFormat GetMethod(string label)
        {
            label = label.Trim();
            label = label.ToUpper();
            var dict = BuildMethodDictionary();
            var methType = ScanFormat.UNKNOWN;
            dict.TryGetValue(label, out methType);
            return methType;
        }
        static BarrelLib.BarrelType GetBarrelType(string label)
        {
            label = label.Trim();
            label = label.ToUpper();
            var dict = BuildBarrelDictionary();
            var barType = GetBarrelType(label);
            return barType;
        }
       
        static Dictionary<string,BarrelLib.BarrelType>BuildBarrelDictionary()
        {
            var dict = new Dictionary<string, BarrelLib.BarrelType>();
            dict.Add("50CAL", BarrelLib.BarrelType.M2_50_Cal);
            dict.Add("M2", BarrelLib.BarrelType.M2_50_Cal);
            dict.Add("25MM", BarrelLib.BarrelType.M242_25mm);
            dict.Add("155MM", BarrelLib.BarrelType.M284_155mm);
            dict.Add("7.62MM", BarrelLib.BarrelType.M240_762mm);
            return dict;
        }
        static Dictionary<string, ScanFormat> BuildMethodDictionary()
        {
            var dict = new Dictionary<string, ScanFormat>();
            dict.Add("RG", ScanFormat.RING);
            dict.Add("AX", ScanFormat.AXIAL);
            dict.Add("SP", ScanFormat.SPIRAL);
            dict.Add("LD", ScanFormat.LAND);
            dict.Add("GV", ScanFormat.GROOVE);           
            dict.Add("CAL", ScanFormat.CAL);
            dict.Add("LN", ScanFormat.SINGLELINE);
            return dict;
        }
        static InspectionScriptBuilder()
        {
            _linAxisName = "X";
            _rotAxisName = "A";
            _landName = "LANDS";
            _grooveName = "GROOVES";
        }
        static CylInspScript BuildScript(string filename, double axisIncrement, double revs,
            double pitchInch, int ptsPerRev, ScanFormat method)
        {
            CylInspScript script;
            var fileCodes = ParseFilename(filename);

            int len = fileCodes.Length;

            // var start = new MachinePosition(MachineGeometry.CYLINDER);
            var start = FindStartPosition(fileCodes, _linAxisName);

            // var end = new MachinePosition(MachineGeometry.CYLINDER);
            var end = FindEndPosition(fileCodes, _linAxisName);
            //start.X = getVal(fileCodes[2].ToUpper(), _linAxisName);

            switch (method)
            {
                case ScanFormat.RING:
                    end.X = start.X;
                    script = new CylInspScript(method, start, end, pitchInch, ptsPerRev);
                    break;
                case ScanFormat.AXIAL:
                case ScanFormat.SPIRAL:
                    end.X = getVal(fileCodes[3].ToUpper(), _linAxisName);
                    start.Adeg = getVal(fileCodes[4].ToUpper(), _rotAxisName);
                    end.Adeg = getVal(fileCodes[5].ToUpper(), _rotAxisName);
                    script = new CylInspScript(method, start, end, pitchInch, ptsPerRev);
                    break;
                case ScanFormat.GROOVE:
                case ScanFormat.LAND:
                    var groovelabels = new string[] { _grooveName, _landName };
                    var groove1 = (int)getVal(fileCodes[4].ToUpper(), groovelabels);
                    var groove2 = (int)getVal(fileCodes[5].ToUpper(), groovelabels);
                    int[] grooves = new int[2] { groove1, groove2 };
                    script = new CylInspScript(method, start, end, axisIncrement, grooves);
                    break;
                default:
                    script = new CylInspScript(method, start, end);
                    break;
            }
            return script;
        }
        static public CylInspScript BuildScript(string filename, double axisIncrement, double revs, double pitchInch, int ptsPerRev)
        {
            var fileCodes = ParseFilename(filename);
            int len = fileCodes.Length;
            var method = GetMethod(fileCodes[len - 1]);
            if (method == ScanFormat.UNKNOWN)
            {
                method = ScanFormat.RING;
            }
            return BuildScript(filename, axisIncrement, revs, pitchInch, ptsPerRev, method);
        }
        static public CylInspScript BuildScript(ProbeController.ProbeType probeType, ScanFormat method,
            MachinePosition start, MachinePosition end, double axisIncrement, double revs, double pitchInch, int ptsPerRev, int[] grooves)
        {
            CylInspScript script;
            switch (method)
            {
                case ScanFormat.RING:
                    end.X = start.X;
                    script = new CylInspScript(method, start, end, pitchInch, ptsPerRev);
                    break;
                case ScanFormat.AXIAL:
                case ScanFormat.SPIRAL:

                    script = new CylInspScript(method, start, end, pitchInch, ptsPerRev);
                    break;
                case ScanFormat.GROOVE:
                case ScanFormat.LAND:
                    script = new CylInspScript(method, start, end, axisIncrement, grooves);
                    break;
                default:
                    script = new CylInspScript(method, start, end);
                    break;
            }
            return script;
        }
    }
}
