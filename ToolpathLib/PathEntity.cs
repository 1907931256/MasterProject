using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
using DataLib;

namespace ToolpathLib
{
   
   
    /// <summary>
    /// holds location, jet direction feedrate of xsection path entity
    /// </summary>
  
    public class PathEntity
    {

    }
    public  class PathEntity5Axis
    {
        public BlockType Type {get; set;}        
        public CNCLib.XYZBCMachPosition Position { get; set; }
        public CNCLib.XYZBCMachPosition PrevPosition { get; set; }
        public Vector3 PositionAsVector3
        {
            get
            {
                return new Vector3(Position.X, Position.Y, Position.Z);
            }
        }
        public bool BAxisRolloverFlag { get; set; }
        public Vector3 DirVector {get; set;}	    
	    public CComp Ccomp { get; set;}
        public Vector3 JetVector { get; set; }
        public Vector3 SurfNormal { get; set; }
	    public Feedrate Feedrate { get; set;}
	    public CtrlFlag ControlFlag { get; set;}
	    public bool JetOn { get; set;}
        public List<string> ActiveMcodes { get; set; }
        public int LineNumber { get; set; }
        public int PathNumber { get; set;}        
        public bool CcompTangent {get; set;}
        public double Length { get; set; }
        public double Depth { get; set; }
        public double TargetDepth { get; set; }
        public double CumulativeTime { get; set; }
        public double TravelTime { get; set; }
        public bool ContainsX { get; set; }
        public bool ContainsY { get; set; }
        public bool ContainsZ { get; set; }
        public bool ContainsF { get; set; }
        public bool ContainsN { get; set; }
        public string InputString { get; set; }
        public PathEntity5Axis()
        {
            ActiveMcodes = new List<string>();
            Position = new CNCLib.XYZBCMachPosition();
            PrevPosition = new CNCLib.XYZBCMachPosition();
            Type = BlockType.Unknown;
            Ccomp = CComp.NoChange;
            ControlFlag = CtrlFlag.Unknown;
            DirVector = new Vector3();
            JetVector = new Vector3();
            SurfNormal = new Vector3();
            Feedrate = new Feedrate();
            Depth = 0;
        }
      
    }
}
