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
        public string ShortFileName
        {
            get
            {
                return System.IO.Path.GetFileName(FileName);
            }
        }
        public static string GetNearestFile(Point mousePt, List<DisplayData> displayDataList)
        {
            try
            {
                double minDist2 = double.MaxValue;
                string filename = "";
                PointF minPt = new PointF();
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

                return filename;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static PointF GetNearestPoint(Point mousePt, List<DisplayData> displayDataList)
        {
            try
            {
                double minDist2 = double.MaxValue;
                string filename = "";
                PointF minPt = new PointF();
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

                return minPt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public RectangleF BoundingRect(float borderPercent,int decimalPlaces)
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
            float minYRound = (float)(Math.Floor((minY-bordery) * round)/round);
            float maxYRound = (float)(Math.Ceiling((maxY+bordery) * round) / round);
            height = (float)(maxYRound - minYRound);                    
            return new RectangleF(minX , minYRound , width , height );
        }
        public RectangleF BoundingRect()
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
        //public DisplayData()
        //{
        //    FileName = "";

        //}
        public DisplayData(string filename)
        {
            FileName = filename;
        }
    }
   
    

}
