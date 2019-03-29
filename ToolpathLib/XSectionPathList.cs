using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLib;
namespace ToolpathLib
{
    public class XSecPathList : List<XSectionPathEntity>
    {
        public double AlongLoc { get; set; }

        void AdjustFeedrates()
        {
            foreach (XSectionPathEntity xpe in this)
            {
                if (xpe.TargetDepth != 0)
                {
                    var newFeedrate = xpe.Feedrate * xpe.CurrentDepth / xpe.TargetDepth;
                    xpe.Feedrate = newFeedrate;
                }
            }
        }

        public void AdjustFeedrates(CartData depthData)
        {
            MeasureDepthsAtJetLocations(depthData);
            AdjustFeedrates();
        }
        public void AdjustFeedrates(CylData depthData)
        {
            MeasureDepthsAtJetLocations(depthData);
            AdjustFeedrates();
        }
        public void AdjustFeedrates(CartData depthData, CartData startData, CartData targetData, double averagingWindow)
        {
            MeasureDepthsAtJetLocations(depthData, startData, targetData, averagingWindow);
            AdjustFeedrates();
        }

        public void AdjustFeedrates(CylData depthData, CylData startData, CylData targetData, double averagingWindow)
        {
            MeasureDepthsAtJetLocations(depthData, startData, targetData, averagingWindow);
            AdjustFeedrates();
        }
        public void MeasureDepthsAtJetLocations(CylData depthData)
        {
            foreach (XSectionPathEntity xpe in this)
            {
                double xMeasure = xpe.CrossLoc;

                for (int i = 1; i < depthData.Count; i++)
                {
                    if ((xpe.CrossLoc >= depthData[i - 1].ThetaDeg && xpe.CrossLoc <= depthData[i].ThetaDeg) ||
                        (xpe.CrossLoc <= depthData[i - 1].ThetaDeg && xpe.CrossLoc >= depthData[i].ThetaDeg))
                    {
                        double denom = (depthData[i].ThetaDeg - depthData[i - 1].ThetaDeg);
                        double m = 0;
                        double depth = 0;
                        if (denom != 0)
                        {
                            m = (depthData[i].R - depthData[i - 1].R) / denom;
                            depth = m * (xpe.CrossLoc - depthData[i - 1].ThetaDeg) + depthData[i - 1].R;
                        }
                        else
                        {
                            depth = (depthData[i].R + depthData[i - 1].R) / 2.0;
                        }
                        xpe.CurrentDepth = depth - xpe.StartDepth;
                        break;
                    }
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
                        double depth = 0;
                        if (denom != 0)
                        {
                            m = (depthData[i].Y - depthData[i - 1].Y) / denom;
                            depth = m * (xpe.CrossLoc - depthData[i - 1].X) + depthData[i - 1].Y;
                        }
                        else
                        {
                            depth = (depthData[i].Y + depthData[i - 1].Y) / 2.0;
                        }
                        xpe.CurrentDepth = depth - xpe.StartDepth;

                        break;
                    }
                }
            }
        }

        public void MeasureDepthsAtJetLocations(CartData depthData, CartData startData, CartData targetData, double averagingWindow)
        {
            foreach (XSectionPathEntity xpe in this)
            {
                double xStart = xpe.CrossLoc - averagingWindow / 2;
                double xEnd = xpe.CrossLoc + averagingWindow / 2;
                //calc profile 
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
                //calc target 
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
                //calc start 
                count = 0;
                xSum = 0;
                for (int i = 1; i < startData.Count; i++)
                {
                    if ((startData[i].X > xStart && startData[i].X <= xEnd) || (startData[i].X <= xStart && startData[i].X > xEnd))
                    {
                        count++;
                        xSum += startData[i].Y;
                    }
                }
                if (count > 0)
                {
                    xSum /= count;
                }
                xpe.StartDepth = xSum;
                xpe.CurrentDepth -= xpe.StartDepth;
            }
        }
        public void MeasureDepthsAtJetLocations(CylData profileData, CylData startData, CylData targetData, double averagingWindow)
        {
            foreach (XSectionPathEntity xpe in this)
            {

                double xStart = xpe.CrossLoc - averagingWindow / 2;
                double xEnd = xpe.CrossLoc + averagingWindow / 2;
                //calc profile value
                double xSum = 0;
                int count = 0;
                for (int i = 1; i < profileData.Count; i++)
                {
                    if ((profileData[i].ThetaDeg > xStart && profileData[i].ThetaDeg <= xEnd) || (profileData[i].ThetaDeg <= xStart && profileData[i].ThetaDeg > xEnd))
                    {
                        count++;
                        xSum += profileData[i].R;
                    }
                }
                if (count > 0)
                {
                    xSum /= count;
                }
                xpe.CurrentDepth = xSum;
                //calc target depth
                count = 0;
                xSum = 0;
                for (int i = 1; i < targetData.Count; i++)
                {
                    if ((targetData[i].ThetaDeg > xStart && targetData[i].ThetaDeg <= xEnd) || (targetData[i].ThetaDeg <= xStart && targetData[i].ThetaDeg > xEnd))
                    {
                        count++;
                        xSum += targetData[i].R;
                    }
                }
                if (count > 0)
                {
                    xSum /= count;
                }
                //calc start depth
                count = 0;
                xSum = 0;
                for (int i = 1; i < startData.Count; i++)
                {
                    if ((startData[i].ThetaDeg > xStart && startData[i].ThetaDeg <= xEnd) || (startData[i].ThetaDeg <= xStart && startData[i].ThetaDeg > xEnd))
                    {
                        count++;
                        xSum += startData[i].R;
                    }
                }
                if (count > 0)
                {
                    xSum /= count;
                }
                xpe.StartDepth = xSum;
                xpe.CurrentDepth -= xpe.StartDepth;
            }
        }
        //public void MeasureDepthsAtJetLocations(CartData depthData,double averagingWindow)
        //{
        //    foreach (XSectionPathEntity xpe in this)
        //    {
        //        double xStart = xpe.CrossLoc-averagingWindow/2;
        //        double xEnd = xpe.CrossLoc + averagingWindow / 2;
        //        double xSum = 0;
        //        int count = 0;
        //        for (int i = 1; i < depthData.Count; i++)
        //        {
        //            if((depthData[i].X>xStart && depthData[i].X<=xEnd )||(depthData[i].X <= xStart && depthData[i].X > xEnd))
        //            {
        //                count++;
        //                xSum += depthData[i].Y;
        //            }
        //        }
        //        if(count>0)
        //        {
        //            xSum /= count;
        //        }
        //        xpe.CurrentDepth = xSum;
        //    }
        //}
        //public void MeasureDepthsAtJetLocations(CylData depthData, double averagingWindow)
        //{
        //    foreach (XSectionPathEntity xpe in this)
        //    {
        //        double xStart = xpe.CrossLoc - averagingWindow / 2;
        //        double xEnd = xpe.CrossLoc + averagingWindow / 2;
        //        double xSum = 0;
        //        int count = 0;
        //        for (int i = 1; i < depthData.Count; i++)
        //        {
        //            if ((depthData[i].ThetaDeg > xStart && depthData[i].ThetaDeg <= xEnd) || (depthData[i].ThetaDeg <= xStart && depthData[i].ThetaDeg > xEnd))
        //            {
        //                count++;
        //                xSum += depthData[i].R;
        //            }
        //        }
        //        if (count > 0)
        //        {
        //            xSum /= count;
        //        }
        //        xpe.CurrentDepth = xSum;
        //    }
        //}
        void SortByPassExcOrder()
        {
            var order = new List<int>();
            foreach (XSectionPathEntity xpe in this)
            {
                order.Add(xpe.PassExecOrder);
            }
            var orderArr = order.ToArray();
            var cspArr = this.ToArray();
            Array.Sort(orderArr, cspArr);
            Clear();
            AddRange(cspArr);
        }
        public List<string> AsCSVFile(string depthLoctionFilename, string inputFilename)
        {
            var lines = new List<string>();
            lines.Add("input file: " + inputFilename);
            lines.Add("depth location file: " + depthLoctionFilename);
            lines.Add("pathExOrder,xLocation(across channel),yLocation(along channel),feed, direction,Depth,StartDepth,TargetDepth");
            foreach (XSectionPathEntity xpe in this)
            {
                string line = xpe.PassExecOrder.ToString() + "," + xpe.CrossLoc.ToString() + "," + xpe.AlongLocation.ToString() + ","
                    + xpe.Feedrate.ToString() + "," + xpe.Direction.ToString() + "," + xpe.CurrentDepth.ToString() + "," + xpe.StartDepth.ToString() + "," + xpe.TargetDepth.ToString();
                lines.Add(line);
            }
            return lines;
        }
        public XSecPathList()
        {

        }
        public XSecPathList(string csvFilename, int firstDataRowZerobase, int firstDataColZerobase)
        {
            var stringArr = FileIOLib.CSVFileParser.ParseFile(csvFilename);
            //pathExOrder,xLocation(across channel),yLocation(along channel),feed, direction(+,-),Depth,StartDepth,TargetDepth

            int colCount = stringArr.GetLength(1);
            for (int i = firstDataRowZerobase; i < stringArr.GetLength(0); i++)
            {
                double depth = 0;
                double targetDepth = 0;
                double startDepth = 0;
                int.TryParse(stringArr[i, firstDataColZerobase], out int pathExOrder);
                double.TryParse(stringArr[i, firstDataColZerobase + 1], out double xlocation);
                double.TryParse(stringArr[i, firstDataColZerobase + 2], out double ylocation);
                double.TryParse(stringArr[i, firstDataColZerobase + 3], out double feed);
                int.TryParse(stringArr[i, firstDataColZerobase + 4], out int direction);
                if (colCount > 5)
                {
                    double.TryParse(stringArr[i, firstDataColZerobase + 5], out depth);
                    double.TryParse(stringArr[i, firstDataColZerobase + 6], out startDepth);
                    double.TryParse(stringArr[i, firstDataColZerobase + 7], out targetDepth);
                }
                if (pathExOrder > 0)
                {
                    var xpe = new XSectionPathEntity()
                    {
                        PassExecOrder = pathExOrder,
                        CrossLoc = xlocation,
                        AlongLocation = ylocation,
                        Feedrate = feed,
                        Direction = direction,
                        CurrentDepth = depth,
                        TargetDepth = targetDepth,
                        StartDepth = startDepth
                    };
                    Add(xpe);
                }

            }
            SortByPassExcOrder();
        }
    }
}
