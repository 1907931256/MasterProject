using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolpathLib
{
    public class ToolPath : List<PathEntity>
    {
        public int[] MiscIntegerArr;
        public double[] MiscRealArr;
        public int ProgNumber;
        public int SeqIncrement;
        public int StartNumber;
        public int ToolNumber;
        public int ToolDiamNumber;
        public int ToolLengthNumber;
        public double NomFeedrate;
        public double XHome;
        public double YHome;
        public double ZHome;
        public double OffsetDist;
        public double ToolDiameter;
        public int OpCode;
        public int CutPathCount;
        public string FilePath;
        public string OutputFileName;
        public string InputFileName;
        public string Title;

        public List<string> InputPath()
        {

            var _input = new List<string>();
            foreach(var pe in this)
            {
                _input.Add(pe.InputString);
            }
            return _input;
        }
        public ToolPath()
        {
            MiscIntegerArr = new int[10];
            MiscRealArr = new double[10];
            FilePath = "";
            OutputFileName = "";
            InputFileName = "";
            Title = "";

        }

    }
    
}
