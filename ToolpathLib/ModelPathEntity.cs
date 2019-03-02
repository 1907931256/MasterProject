using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
namespace ToolpathLib
{
    
    public class ModelPathEntity:PathEntity5Axis
    {
        public List<int> IntersectionIndices { get; set; }
        public List<double> RadiusList { get; set; }
        public ModelPathEntity(PathEntity5Axis pEnt)
        {
            Ccomp = pEnt.Ccomp;
            CcompTangent = pEnt.CcompTangent;
            ControlFlag = pEnt.ControlFlag;
            Depth = pEnt.Depth;
            DirVector = new Vector3(pEnt.DirVector);
            Position = new CNCLib.XYZBCMachPosition(pEnt.Position);
            Feedrate = new ToolpathLib.Feedrate( pEnt.Feedrate);
            JetOn = pEnt.JetOn;
            JetVector = new Vector3( pEnt.JetVector);
            PathNumber = pEnt.PathNumber;
            LineNumber = pEnt.LineNumber;
            SurfNormal = new Vector3( pEnt.SurfNormal);
            Type = pEnt.Type;
            TravelTime = pEnt.TravelTime;
            CumulativeTime = pEnt.CumulativeTime;

        }
        public ModelPathEntity Clone()
        {
            var m = new ModelPathEntity();
            m.IntersectionIndices.AddRange(IntersectionIndices);          
            m.ActiveMcodes.AddRange(ActiveMcodes);
            m.Ccomp =  Ccomp;
            m.CcompTangent = CcompTangent;
            m.ContainsF = ContainsF;
            m.ContainsX = ContainsX;
            m.ContainsY = ContainsY;
            m.ContainsZ = ContainsZ;
            m.ControlFlag = ControlFlag;
            m.CumulativeTime = CumulativeTime;
            m.Depth = Depth;
            m.DirVector = new Vector3( DirVector);
            m.Feedrate = new Feedrate(Feedrate);
            m.JetOn = JetOn;
            m.JetVector = new Vector3(JetVector);
            m.Length = Length;
            m.PathNumber = PathNumber;
            m.LineNumber = LineNumber;
            m.Position = new CNCLib.XYZBCMachPosition();
            m.SurfNormal = new Vector3(SurfNormal);
            m.TargetDepth = TargetDepth;
            m.TravelTime = TravelTime;
            m.Type = Type;
            return m;
        }
        public ModelPathEntity()
        {
            IntersectionIndices = new List<int>();
            Ccomp = new CComp();
            CcompTangent = false;
            ControlFlag = new CtrlFlag();
            Depth = 0;
            DirVector = new Vector3();
            Position = new CNCLib.XYZBCMachPosition();
            Feedrate = new ToolpathLib.Feedrate();
            JetOn = false;
            JetVector = new Vector3();
            PathNumber = 0;
            SurfNormal = new Vector3();
            Type = BlockType.Unknown;
            TravelTime = 0;
            CumulativeTime = 0;
        }
    }
}
