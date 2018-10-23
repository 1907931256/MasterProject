using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WinFormsLib
{
    public class PictureBoxUtilities
    {
        public static PointF GetNearestPoint(Point mousePt, List<PointF> screenPoints)
        {
            try
            {
                double minDist2 = double.MaxValue;
                PointF minPt = new PointF();
                foreach (var p in screenPoints)
                {
                    var dist2 = Math.Pow(p.X - mousePt.X, 2) + Math.Pow(p.Y - mousePt.Y, 2);
                    if (dist2 < minDist2)
                    {
                        minDist2 = dist2;
                        minPt = new PointF(p.X, p.Y);
                    }
                }

                return minPt;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
