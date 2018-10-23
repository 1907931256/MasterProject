using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbMachModel
{
    public enum DepthSearchType
    {
        FindMaxDepth,
        FindMinDepth,
        FindAveDepth
    }
    /// <summary>
    /// contains depth measurment location info for model run
    /// </summary>
    public class DepthInfo
    {
        public GeometryLib.Vector3 LocationOfDepthMeasure { get; set; }
        public double SearchRadius { get; set; }
        public List<double> CurrentDepthAtLocation;
        public double TargetDepthAtLocation { get; set; }
        public DepthSearchType SearchType { get; set; }
        public double TargetDepth { get; set; }
        public double StartDepth { get; set; }
        public double DepthTolerance { get; set; }
        public bool ConstTargetDepth { get; set; }
        public bool ConstStartDepth { get; set; }
        public string TargetDepthFilename { get; set; }
        public string StartDepthFilename { get; set; }

        public DepthInfo()
        {
            LocationOfDepthMeasure = new GeometryLib.Vector3();
            CurrentDepthAtLocation = new List<double>();
            SearchRadius = 0;
            TargetDepthAtLocation = 0;
            SearchType = DepthSearchType.FindAveDepth;
        }
        public DepthInfo(GeometryLib.Vector3 locationOfMeasurment,DepthSearchType searchType,double searchRadius)
        {
            LocationOfDepthMeasure = locationOfMeasurment;
            SearchType = searchType;
            CurrentDepthAtLocation = new List<double>();
            TargetDepthAtLocation = 0;
            SearchRadius = searchRadius;
        }
    }
}
