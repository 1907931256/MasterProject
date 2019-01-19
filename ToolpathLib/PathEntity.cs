using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;

namespace ToolpathLib
{
   
    public class XSecPathList : List<XSectionPathEntity>
    {
        public double AlongLoc { get; set; }        
        void SortByPassExcOrder()
        {
            var order = new List<int>();
            foreach(XSectionPathEntity xpe in this)
            {
                order.Add(xpe.PassExecOrder);
            }
            var orderArr = order.ToArray();
            var cspArr = this.ToArray();
            Array.Sort(orderArr, cspArr);
            Clear();
            AddRange(cspArr);
        }        
        public XSecPathList(string csvFilename,int headerRowCount)
        {
            var stringArr = FileIOLib.CSVFileParser.ParseFile(csvFilename);
            //pathExOrder,xLocation(across channel),yLocation(along channel),feed, direction(+,-)
            if (headerRowCount < 0)
                headerRowCount = 0;
            for(int i =headerRowCount;i<stringArr.GetLength(0);i++)
            {
                int pathExOrder = 1;
                double feed = 1.0;
                double xlocation = 0;
                double ylocation = 0;
                int direction = 1;
                int.TryParse(stringArr[i,0], out pathExOrder);
                double.TryParse(stringArr[i, 1], out xlocation);
                double.TryParse(stringArr[i, 2], out ylocation);
                double.TryParse(stringArr[i, 3], out feed);
                int.TryParse(stringArr[i, 4], out direction);
                var xpe = new XSectionPathEntity() { PassExecOrder = pathExOrder, CrossLoc = xlocation, Feedrate = feed, Direction = direction };
                this.Add(xpe);
            }
            SortByPassExcOrder();
        }
    }
    public class XSectionPathEntity
    {
        public CNCLib.MachinePosition MachinePosition { get; set; }
        public Vector2 JetVector { get; set; }
        public double CrossLoc { get; set; }       
        public double Feedrate { get; set; }
        public int PassExecOrder { get; set; }
        public int Direction { get; set; }
    }
    public  class PathEntity
    {
        public BlockType Type {get; set;}        
        public CNCLib.MachinePosition Position { get; set; }
        public Vector3 PositionAsVector
        {
            get
            {
                return new Vector3(Position.X, Position.Y, Position.Z);
            }
        }
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
        public PathEntity()
        {
            ActiveMcodes = new List<string>();
            Position = new CNCLib.MachinePosition(CNCLib.MachineGeometry.XYZBC);
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
