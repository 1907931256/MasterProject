using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNCLib;
using DataLib;
using ProbeController;

namespace InspectionLib
{
    public class InspectionFileNameParser
    {
        double getVal(string str, string name)
        {
            double val = 0.0;
            str = str.ToUpper();
            str = str.Trim();
            int len = name.Length;
            if (str.StartsWith(name))
            {
                val = Convert.ToDouble(str.Substring(len));
            }
            return val;
        }
        double getVal(string str, string[] names)
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
        XAMachPostion FindStartPosition(string[] fileCodes, string axisName)
        {
            int firstX = 0;
            for (int i = 0; i < fileCodes.Length; i++)
            {
                string word = fileCodes[i].ToUpper();

                if (word.Contains(axisName))
                {
                    firstX = i;
                    break;
                }
            }
            string xword = fileCodes[firstX].ToUpper();
            int xIndex = xword.IndexOf(axisName);
            string pos = xword.Substring(xIndex + 1);
            double xpos = 0;
            double.TryParse(pos, out xpos);
            var mp = new XAMachPostion(xpos, 0);
            return mp;
        }
        XAMachPostion FindEndPosition(string[] fileCodes, string axisName)
        {
            int lastX = 0;
            for (int i = fileCodes.Length - 1; i >= 0; i--)
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
            var mp = new XAMachPostion(xpos, 0);
            return mp;
        }
        string[] ParseFilename(string filename)
        {
            var fileNoExt = System.IO.Path.GetFileNameWithoutExtension(filename);
            var splitters = new char[] { '_', '-' };
            var fileCodes = fileNoExt.Split(splitters);
            var fileCodeList = fileCodes.ToList();
            fileCodeList.Remove("out");
            return fileCodeList.ToArray();
        }
        XAMachPostion start;
        XAMachPostion end;
        string _linAxisName;
        string _rotAxisName;
        string _grooveName;
        string _landName;
        int rotations;
        double pitch;        
        string _filename;
        ScanFormat scanFormat;

        public ScanFormat ScanFormat { get { return scanFormat; } }
        public XAMachPostion Start { get { return start; } }
        public XAMachPostion End { get { return end; } }
        public int Rotations { get { return rotations; } }
        public double Pitch { get { return pitch; } }
        

        public void ParseSpiralname(string filename)
        {
            var fileCodes = ParseFilename(filename);
            start = new XAMachPostion();
            end = new XAMachPostion();
            start.X = getVal(fileCodes[2], _linAxisName);
            end.X = getVal(fileCodes[3], _linAxisName);
            start.Adeg = getVal(fileCodes[4], _rotAxisName);
            end.Adeg = getVal(fileCodes[5], _rotAxisName);
            var dx = end.X - start.X;
            var da = end.Adeg - start.Adeg;
            rotations = (int)Math.Ceiling(da / 360);
        }
        public void ParseRingFilename(string filename)
        {
            var fileCodes = ParseFilename(filename);
            var start = new XAMachPostion();
            var end = new XAMachPostion();
            start.X = getVal(fileCodes[2], _linAxisName);
            start.Adeg = getVal(fileCodes[3], _rotAxisName);
            end.X = start.X;
            end.Adeg = getVal(fileCodes[4], _rotAxisName);
        }
        Dictionary<string, ScanFormat> BuildMethodDictionary()
        {
            var dict = new Dictionary<string, ScanFormat>();
            dict.Add("RG", ScanFormat.RING);
            dict.Add("AX", ScanFormat.AXIAL);
            dict.Add("SP", ScanFormat.SPIRAL);
            dict.Add("CAL", ScanFormat.CAL);            
            dict.Add("GR", ScanFormat.GROOVE);
            dict.Add("SG", ScanFormat.SINGLE);
            dict.Add("RS", ScanFormat.RASTER);

            return dict;
        }
        ScanFormat GetMethod(string label)
        {
            try
            {
                label = label.Trim();
                label = label.ToUpper();
                var dict = BuildMethodDictionary();
                var methType = ScanFormat.AXIAL;
                dict.TryGetValue(label, out methType);
                return methType;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public InspectionFileNameParser(string filename)
        {
            _linAxisName = "X";
            _rotAxisName = "A";
            _landName = "LANDS";
            _grooveName = "GROOVES";
            _filename = filename;
        }
    }


    //public class InspectionScriptBuilder
    //{
    //    static BarrelLib.BarrelType GetBarrelType(string label)
    //    {
    //        label = label.Trim();
    //        label = label.ToUpper();
    //        var dict = BuildBarrelDictionary();
    //        var barType = GetBarrelType(label);
    //        return barType;
    //    }
       
    //    static Dictionary<string,BarrelLib.BarrelType>BuildBarrelDictionary()
    //    {
    //        var dict = new Dictionary<string, BarrelLib.BarrelType>();
    //        dict.Add("50CAL", BarrelLib.BarrelType.M2_50_Cal);
    //        dict.Add("M2", BarrelLib.BarrelType.M2_50_Cal);
    //        dict.Add("25MM", BarrelLib.BarrelType.M242_25mm);
    //        dict.Add("155MM", BarrelLib.BarrelType.M284_155mm);
    //        dict.Add("7.62MM", BarrelLib.BarrelType.M240_762mm);
    //        return dict;
    //    }
    //}
}
