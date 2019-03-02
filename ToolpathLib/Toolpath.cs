using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolpathLib
{
    public class ToolPath5Axis : List<PathEntity5Axis>
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
        public void FixRollovers()
        {
            
            foreach(PathEntity5Axis pe in this)
            {
                double deltaB = pe.Position.Bdeg - pe.PrevPosition.Bdeg;
                double deltaC = pe.Position.Cdeg - pe.PrevPosition.Cdeg;
                if (Math.Abs(deltaC)>90)
                {

                }
            }
        }
        public void FixWrapArounds(double minCaxis,double maxCaxis)
        {

        }
        public List<string> InputPath()
        {

            var _input = new List<string>();
            foreach(var pe in this)
            {
                _input.Add(pe.InputString);
            }
            return _input;
        }
        public ToolPath5Axis()
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
