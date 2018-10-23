using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace ToolpathLib
{
   
    /// <summary>
    /// holds string values for various CNC G code words 
    /// </summary>
    public class CNCMachineCode
    {
        public string InverseFeedGcode { get; set; }
        public string RapidGcode { get; set; }
        public string LinearMoveGcode { get; set; }
        public string FiveAxisGcode { get; set; }
        public string CwArcGcode { get; set; }
        public string CcwArcGcode { get; set; }
        public string DelayGcode { get; set; }
        public string N { get; set; }
        public string DelayAmountPrefix { get; set; }
        public string F { get; set; }
        public string EndofProg { get; set; }
        public string PFormat { get; set; }
        public string FFormat { get; set; }
        public string[] AxisNames { get; set; }
        public int AxisCount { get; set; }
        public string Sp { get; set; }
        public string ComStart { get; set; }
        public string ComEnd { get; set; }
        public string HeaderStart { get; set; }
        public string HeaderEnd { get; set; }
        public int CommentMaxLength { get; set; }
        public int StartingLineNumber { get; set; }
        public int LineNIndex { get; set; }
        public char[] ForbiddenChars { get; set; }
        public double DelayScaleFactor { get; set; }
        public string DelayStringFormat { get; set; }

        public string McodeFilename { get; set; }
        MCodeDictionary _mCodeDictionary;
        

        
        private void loadMachineFile(string fileName)
        {
            if (fileName != null && fileName != "" && System.IO.File.Exists(fileName))
            {
                CNCMachineCodeFile.Open();
            }
            else
            {
                loadDefMachineSettings();
            }
            
        }
        private void loadDefMachineSettings()
        {
             InverseFeedGcode = "G93";
             RapidGcode = "G0";
             LinearMoveGcode = "G01";
             FiveAxisGcode = "G01";
             CwArcGcode = "G02";
             CcwArcGcode = "G03";
             DelayGcode = "G04";
             N = "N";
             DelayAmountPrefix = "K";
             F = "F";
             EndofProg = "M30";
             PFormat = "f4";
             FFormat = "f3";        
             AxisCount = 2;
             Sp = " ";
             ComStart = ";";
             ComEnd = "";
             HeaderStart = "(";
             HeaderEnd = ",MX)";
             CommentMaxLength = 20;         
             DelayScaleFactor = 100;
             DelayStringFormat = "N0";
             McodeFilename = "";
             AxisNames = new string[] { "X", "Y" ,"Z","B","C"};
             ForbiddenChars = new char[] { ' ', '/' };
             StartingLineNumber = 100;
             LineNIndex = 10;
        }
        public CNCMachineCode()
        {
            loadDefMachineSettings();
            _mCodeDictionary = new MCodeDictionary();
            
        }
        public CNCMachineCode(string machineFileName)
        {
            loadMachineFile(machineFileName);
            _mCodeDictionary = new MCodeDictionary(McodeFilename);
            
        }
    }
}
