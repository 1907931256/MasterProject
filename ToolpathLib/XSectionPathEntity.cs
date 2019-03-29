using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
namespace ToolpathLib
{
    public class XSectionPathEntity
    {
        public double AlongLocation { get; set; }
        public Vector2 JetVector { get; set; }
        public double CrossLoc { get; set; }
        public double Feedrate
        {
            get
            {
                if (FeedHistory.Count > 0)
                {
                    return FeedHistory[FeedHistory.Count - 1];
                }
                else
                {
                    return 0;
                }

            }
            set
            {
                FeedHistory.Add(value);
            }
        }

        public int PassExecOrder { get; set; }
        public int Direction { get; set; }
        public double StartDepth { get; set; }
        public double TargetDepth { get; set; }
        public double CurrentDepth { get; set; }
        public Vector2 SurfNormal { get; set; }
        public int CurrentRun { get; set; }
        public int TargetRunTotal { get; set; }
        public List<double> FeedHistory { get; set; }
        public XSectionPathEntity()
        {
            SurfNormal = new Vector2(0, 1);
            JetVector = new Vector2(0, 1);
            FeedHistory = new List<double>();
        }
    }
}
