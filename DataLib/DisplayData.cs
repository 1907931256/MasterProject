using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GeometryLib;
namespace DataLib
{
    public class DisplayData : List<PointF>
    {
        public string FileName { get; set; }
        public Color Color { get; set; }
        public string ShortFileName
        {
            get
            {
                return System.IO.Path.GetFileName(FileName);
            }
        }
        static string filename;
        static PointF minPt;
        public Tuple<double, double> GetMinMaxY()
        {
            double maxYData = double.MinValue;
            double minYData = double.MaxValue;
            foreach (var pt in this)
            {
                if (pt.Y > maxYData)
                {
                    maxYData = pt.Y;
                }
                if (pt.Y < minYData)
                {
                    minYData = pt.Y;
                }
            }
            return new Tuple<double, double>(minYData, maxYData);
        }
        static void findNearest(PointF mousePt, List<DisplayData> displayDataList)
        {
            try
            {
                double minDist2 = double.MaxValue;
                filename = "";
                minPt = new PointF();
                foreach (var display in displayDataList)
                {
                    foreach (var p in display)
                    {
                        var dist2 = Math.Pow(p.X - mousePt.X, 2) + Math.Pow(p.Y - mousePt.Y, 2);
                        if (dist2 < minDist2)
                        {
                            minDist2 = dist2;
                            minPt = new PointF(p.X, p.Y);
                            filename = display.ShortFileName;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetNearestFile(PointF mousePt, List<DisplayData> displayDataList)
        {
            try
            {
                findNearest(mousePt, displayDataList);
                return filename;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static PointF GetNearestPoint(PointF mousePt, List<DisplayData> displayDataList)
        {
            try
            {
                findNearest(mousePt, displayDataList);
                return minPt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DisplayData TrimTo(RectangleF window)
        {
            try
            {
                                
                var trimmedDisplay = new DisplayData(this.FileName);
                trimmedDisplay.Color = this.Color;
                var xList = new List<double>();
                var ptList = new List<PointF>();

                foreach (PointF pt in this)
                {
                    if(pt.X>=window.Left && pt.X<=window.Right )
                    {
                        if ( window.Bottom<=0  && window.Top<=0 && pt.Y<0)
                        {                           
                           ptList.Add(new PointF(pt.X, pt.Y));
                           xList.Add(pt.X);                                                     
                        }
                        if(window.Bottom >= 0 && window.Top >= 0 && pt.Y > 0)
                        {
                            ptList.Add(new PointF(pt.X, pt.Y));
                            xList.Add(pt.X);
                        }
                    }
                }
                var ptArr = ptList.ToArray();               
               
                Array.Sort(xList.ToArray(), ptArr);
                              
                trimmedDisplay.AddRange(ptArr);
                return trimmedDisplay;
            }
            catch (Exception)
            {

                throw;
            }
        }
         
        public RectangleF BoundingRect(float borderPercent,int decimalPlaces)
        {
            try
            {
                float maxX = float.MinValue;
                float minX = float.MaxValue;
                float maxY = float.MinValue;
                float minY = float.MaxValue;
                float width = 0;
                float height = 0;
                foreach (PointF pt in this)
                {
                    maxX = Math.Max(pt.X, maxX);
                    minX = Math.Min(pt.X, minX);
                    maxY = Math.Max(pt.Y, maxY);
                    minY = Math.Min(pt.Y, minY);
                }
                width = maxX - minX;
                double round = Math.Pow(10, decimalPlaces);
                float bordery = height * borderPercent / 2;
                float minYRound = (float)(Math.Floor((minY - bordery) * round) / round);
                float maxYRound = (float)(Math.Ceiling((maxY + bordery) * round) / round);
                height = (float)(maxYRound - minYRound);
                return new RectangleF(minX, minYRound, width, height);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public RectangleF BoundingRect()
        {
            try
            {
                float maxX = float.MinValue;
                float minX = float.MaxValue;
                float maxY = float.MinValue;
                float minY = float.MaxValue;
                foreach (PointF pt in this)
                {
                    maxX = Math.Max(pt.X, maxX);
                    minX = Math.Min(pt.X, minX);
                    maxY = Math.Max(pt.Y, maxY);
                    minY = Math.Min(pt.Y, minY);
                }
                return new RectangleF(minX, minY, maxX - minX, maxY - minY);
            }
            catch (Exception)
            {

                throw;
            }
               
        }
       
        public DisplayData(string filename)
        {
            FileName = filename;
            Color = Color.Gray;
        }
    }
   
    

}
