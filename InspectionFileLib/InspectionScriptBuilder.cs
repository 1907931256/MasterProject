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
    public class InspectionScriptBuilder
    {

        static string _linAxisName;
        static string _rotAxisName;
        static string _grooveName;
        static string _landName;



        static double getVal(string str, string name)
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
        static XAMachPostion FindStartPosition(string[] fileCodes, string axisName)
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
            var mp = new XAMachPostion(xpos,0);           
            return mp;
        }
        static XAMachPostion FindEndPosition(string[] fileCodes, string axisName)
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
            var mp = new XAMachPostion(xpos,0);            
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
            try
            {
                label = label.Trim();
                label = label.ToUpper();
                var dict = BuildMethodDictionary();
                var methType = ScanFormat.UNKNOWN;
                dict.TryGetValue(label, out methType);
                return methType;
            }
            catch (Exception)
            {

                throw;
            }
           
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
            dict.Add("CAL", ScanFormat.CAL);
            dict.Add("LN", ScanFormat.LINE);
            return dict;
        }
        static InspectionScriptBuilder()
        {
            _linAxisName = "X";
            _rotAxisName = "A";
            _landName = "LANDS";
            _grooveName = "GROOVES";
        }
        static public InspectionScript BuildScriptFromFile(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup,
            CalDataSet calDataSet, string filename, int ptsPerRev)
        {
            try
            {
                switch (scanFormat)
                {
                    case ScanFormat.RING:
                        return BuildRingScriptFromFilename(scanFormat, outputUnit, probeSetup,
                             calDataSet, filename, ptsPerRev);
                    case ScanFormat.SPIRAL:
                        return BuildSpirScriptFromFilename(scanFormat, outputUnit, probeSetup,
                             calDataSet, filename, ptsPerRev);
                    default:
                        return new InspectionScript(scanFormat, outputUnit, probeSetup, calDataSet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        static public InspectionScript BuildScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup,
            CalDataSet calDataSet, string filename, int ptsPerRev, double spiralPitchInch, XAMachPostion start, XAMachPostion end)
        {
            try
            {
                switch (scanFormat)
                {
                    case ScanFormat.RING:
                        return BuildRingScript(scanFormat, outputUnit, probeSetup,
                             calDataSet, filename, ptsPerRev, start, end);
                    case ScanFormat.SPIRAL:
                        return BuildSpirScript(scanFormat, outputUnit, probeSetup,
                             calDataSet, filename, ptsPerRev, spiralPitchInch, start, end);
                    default:
                        return new InspectionScript(scanFormat, outputUnit, probeSetup, calDataSet);
                }
            }
            catch (Exception)
            {

                throw;
            }           
        }

    static SpiralInspScript BuildSpirScriptFromFilename(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, CalDataSet calDataSet,
            string filename, int ptsPerRev)
        {
            var fileCodes = ParseFilename(filename);
            var start = new XAMachPostion();
            var end = new XAMachPostion();
            start.X = getVal(fileCodes[2], _linAxisName);
            end.X = getVal(fileCodes[3], _linAxisName);
            start.Adeg = getVal(fileCodes[4], _rotAxisName);
            end.Adeg = getVal(fileCodes[5], _rotAxisName);
            var dx = end.X - start.X;
            var da = end.Adeg - start.Adeg;
            var rotations = da / 360;
            double spiralPitchInch = dx / rotations;
            return new SpiralInspScript(scanFormat, outputUnit, probeSetup, calDataSet, start, end, ptsPerRev, spiralPitchInch);
        }
        static  SpiralInspScript BuildSpirScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, 
            CalDataSet calDataSet, string filename, int ptsPerRev, double spiralPitchInch, XAMachPostion start, XAMachPostion end)
        {
            return  new SpiralInspScript(scanFormat, outputUnit, probeSetup, calDataSet, start, end, ptsPerRev, spiralPitchInch);
        }
        static  RingInspScript BuildRingScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, 
            CalDataSet calDataSet, string filename,  int ptsPerRev,XAMachPostion start,XAMachPostion end)
        {
            return new RingInspScript(scanFormat, outputUnit, probeSetup, calDataSet, start, end, ptsPerRev);
        }
        static   RingInspScript BuildRingScriptFromFilename(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, 
            CalDataSet calDataSet, 
            string filename,  int ptsPerRev)
        {            
            var fileCodes = ParseFilename(filename);
            var start = new XAMachPostion();
            var end = new XAMachPostion();
            start.X = getVal(fileCodes[2], _linAxisName);           
            start.Adeg = getVal(fileCodes[3], _rotAxisName);
            end.X = start.X;
            end.Adeg = getVal(fileCodes[4], _rotAxisName);
            return new RingInspScript(scanFormat, outputUnit, probeSetup, calDataSet, start, end, ptsPerRev);
        }
    }
}
