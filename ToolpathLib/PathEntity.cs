using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
using DataLib;
namespace ToolpathLib
{
   
    public class XSecPathList : List<XSectionPathEntity>
    {
        public double AlongLoc { get; set; }
        public void AdjustFeedrates(CartData depthData)
        {
            MeasureDepthsAtJetLocations(depthData);
            foreach (XSectionPathEntity xpe in this)
            {
                if (xpe.TargetDepth != 0)
                {
                    var newFeedrate = xpe.Feedrate * xpe.CurrentDepth / xpe.TargetDepth;
                    xpe.Feedrate = newFeedrate;
                }
            }

        }
        public void AdjustFeedrates(CartData depthData,CartData targetData, double averagingWindow)
        {
            MeasureDepthsAtJetLocations(depthData, targetData, averagingWindow);
            foreach (XSectionPathEntity xpe in this)
            {
                if (xpe.TargetDepth != 0)
                {
                    double feedRatio = (xpe.CurrentDepth / xpe.TargetDepth);
                    double currentF = xpe.Feedrate;
                    xpe.Feedrate = currentF * feedRatio;
                }
                else
                {
                    xpe.Feedrate = xpe.Feedrate;
                }
            }
        }
        public void MeasureDepthsAtJetLocations(CartData depthData)
        {
            foreach (XSectionPathEntity xpe in this)
            {
                double xMeasure = xpe.CrossLoc;  
                for (int i = 1; i < depthData.Count; i++)
                {
                    if ((xpe.CrossLoc >= depthData[i - 1].X && xpe.CrossLoc <= depthData[i].X) ||
                        (xpe.CrossLoc <= depthData[i - 1].X && xpe.CrossLoc >= depthData[i].X))
                    {
                        double denom = (depthData[i].X - depthData[i - 1].X);
                        double m = 0;
                        if (denom != 0)
                        {
                            m = (depthData[i].Y - depthData[i - 1].Y) / denom;
                            xpe.CurrentDepth = m * (xpe.CrossLoc - depthData[i - 1].X) + depthData[i - 1].Y;
                        }
                        else
                        {
                            xpe.CurrentDepth = (depthData[i].Y + depthData[i - 1].Y) / 2.0;
                        }
                                                
                        break;
                    }
                }
            }
        }
        public void MeasureDepthsAtJetLocations(CartData depthData,CartData targetData, double averagingWindow)
        {
            foreach (XSectionPathEntity xpe in this)
            {
                double xStart = xpe.CrossLoc - averagingWindow / 2;
                double xEnd = xpe.CrossLoc + averagingWindow / 2;
                double xSum = 0;
                int count = 0;
                for (int i = 1; i < depthData.Count; i++)
                {
                    if ((depthData[i].X > xStart && depthData[i].X <= xEnd) || (depthData[i].X <= xStart && depthData[i].X > xEnd))
                    {
                        count++;
                        xSum += depthData[i].Y;
                    }
                }
                if (count > 0)
                {
                    xSum /= count;
                }
                xpe.CurrentDepth = xSum;
                count = 0;
                xSum = 0;
                for (int i = 1; i < targetData.Count; i++)
                {
                    if ((targetData[i].X > xStart && targetData[i].X <= xEnd) || (targetData[i].X <= xStart && targetData[i].X > xEnd))
                    {
                        count++;
                        xSum += targetData[i].Y;
                    }
                }
                if (count > 0)
                {
                    xSum /= count;
                }
                xpe.TargetDepth = xSum;
            }
        }
        public void MeasureDepthsAtJetLocations(CartData depthData,double averagingWindow)
        {
            foreach (XSectionPathEntity xpe in this)
            {
                double xStart = xpe.CrossLoc-averagingWindow/2;
                double xEnd = xpe.CrossLoc + averagingWindow / 2;
                double xSum = 0;
                int count = 0;
                for (int i = 1; i < depthData.Count; i++)
                {
                    if((depthData[i].X>xStart && depthData[i].X<=xEnd )||(depthData[i].X <= xStart && depthData[i].X > xEnd))
                    {
                        count++;
                        xSum += depthData[i].Y;
                    }
                }
                if(count>0)
                {
                    xSum /= count;
                }
                xpe.CurrentDepth = xSum;
            }
        }
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
        public List<string>  AsCSVFile()
        {
            var lines = new List<string>();
            lines.Add("pathExOrder,xLocation(across channel),yLocation(along channel),feed, direction,Depth,TargetDepth");
            foreach (XSectionPathEntity xpe in this)
            {
                string line = xpe.PassExecOrder.ToString() + "," + xpe.CrossLoc.ToString() + "," + xpe.AlongLocation.ToString() + ","
                    + xpe.Feedrate.ToString() + "," + xpe.Direction.ToString() + "," + xpe.CurrentDepth.ToString() + "," + xpe.TargetDepth.ToString();
                lines.Add(line);
            }
            return lines;
        }
        public XSecPathList()
        {

        }
        public XSecPathList(string csvFilename,int firstDataRowZerobase, int firstDataColZerobase)
        {
            var stringArr = FileIOLib.CSVFileParser.ParseFile(csvFilename);
            //pathExOrder,xLocation(across channel),yLocation(along channel),feed, direction(+,-),Depth,TargetDepth
             
            int colCount = stringArr.GetLength(1);
            for(int i = firstDataRowZerobase; i<stringArr.GetLength(0);i++)
            {
                double depth = 0;
                double targetDepth = 0;
                int.TryParse(stringArr[i, firstDataColZerobase], out int pathExOrder);
                double.TryParse(stringArr[i, firstDataColZerobase+1], out double xlocation);
                double.TryParse(stringArr[i, firstDataColZerobase+2], out double ylocation);
                double.TryParse(stringArr[i, firstDataColZerobase+3], out double feed);
                int.TryParse(stringArr[i, firstDataColZerobase+4], out int direction);
                if(colCount==7)
                {
                    double.TryParse(stringArr[i, firstDataColZerobase+5], out depth);
                    double.TryParse(stringArr[i, firstDataColZerobase+6], out targetDepth);
                }
                if(pathExOrder>0)
                {
                    var xpe = new XSectionPathEntity()
                    {
                        PassExecOrder = pathExOrder,
                        CrossLoc = xlocation,
                        AlongLocation = ylocation,
                        Feedrate = feed,
                        Direction = direction,
                        CurrentDepth = depth,
                        TargetDepth = targetDepth
                    };
                    Add(xpe);
                }
               
            }
            SortByPassExcOrder();
        }
    }
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
        public double TargetDepth { get; set; }
        public double CurrentDepth { get; set; }
        public Vector2 SurfNormal { get; set; }
        public int CurrentRun { get; set; }
        public int TargetRunTotal { get; set; }
        public List<double> FeedHistory { get; set; }
        public XSectionPathEntity()
        {
            SurfNormal = new Vector2(0,  1);
            JetVector = new Vector2(0, 1);
            FeedHistory = new List<double>();
        }
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
