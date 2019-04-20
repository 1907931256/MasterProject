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
        
        string[] ParseFilename(string filename)
        {
            var fileNoExt = System.IO.Path.GetFileNameWithoutExtension(filename);
            var splitters = new char[] { '_' };
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
             start = new XAMachPostion();
             end = new XAMachPostion();
            start.X = getVal(fileCodes[2], _linAxisName);
            start.Adeg = getVal(fileCodes[3], _rotAxisName);
            end.X = start.X;
            end.Adeg = getVal(fileCodes[4], _rotAxisName);
        }
        public void ParseAxialFilename(string filename)
        {
            var fileCodes = ParseFilename(filename);
             start = new XAMachPostion();
             end = new XAMachPostion();
            start.X = getVal(fileCodes[2], _linAxisName);
            end.X = getVal(fileCodes[3], _linAxisName);
            start.Adeg = 0;
            end.Adeg = 0;
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
}
