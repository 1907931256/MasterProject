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
        public RectangleF BoundingRect(float borderPercent)
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
            height = maxY - minY;
            float borderx = width * borderPercent/2;
            float bordery = height * borderPercent/2;
            return new RectangleF(minX - borderx, minY - bordery, (maxX - minX) + borderx, (maxY - minY) + bordery);
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
     
    }
   
    

}
