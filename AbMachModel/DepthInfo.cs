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
        public double StartDepth { get; set; }
        public double CurrentDepth { get; set; }
        public double TargetDepth  { get; set; }
        public double DepthTolerance { get; set; }
        public DepthSearchType SearchType { get; set; }
        public bool ConstTargetDepth { get; set; }
        public DepthInfo()
        {
            LocationOfDepthMeasure = new GeometryLib.Vector3();
            
            SearchRadius = .001;
            
            SearchType = DepthSearchType.FindAveDepth;
        }
        public DepthInfo(DepthInfo info)
        {
            LocationOfDepthMeasure = info.LocationOfDepthMeasure;
            SearchRadius = info.SearchRadius;
            StartDepth = info.StartDepth;
            TargetDepth = info.TargetDepth;
            DepthTolerance = info.DepthTolerance;
            CurrentDepth = info.CurrentDepth;
            SearchType = info.SearchType;
            ConstTargetDepth = info.ConstTargetDepth;
        }
        public DepthInfo(GeometryLib.Vector3 locationOfMeasurment,DepthSearchType searchType,double searchRadius)
        {
            LocationOfDepthMeasure = locationOfMeasurment;
            SearchType = searchType;        
           
            SearchRadius = searchRadius;
        }
    }
}
