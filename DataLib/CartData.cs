using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
namespace DataLib
{
  
    public class CartData : List<Vector3>
    {
        public void RotateDataToLine(Vector3 pt1, Vector3 pt2)
        {
            try
            {
                var _dataRotationRad = Math.Atan2(pt2.Y - pt1.Y, pt2.X - pt1.X);
                var origin = new Vector3(0, 0, 0);
                var start = new Vector3(pt1.X, pt1.Y, 0);
                var rotStart = start.RotateZ(origin, -1 * _dataRotationRad);
                var end = new Vector3(pt2.X, pt2.Y, 0);
                var rotEnd = end.RotateZ(origin, -1 * _dataRotationRad);
                var vTrans = new Vector3(0, -1 * rotEnd.Y, 0);
                var rotData = new List<Vector3>();
                foreach (Vector3 pt in this)
                {
                    var ptRot = pt.RotateZ(origin, -1 * _dataRotationRad);
                    var ptTrans = ptRot.Translate(vTrans);
                    rotData.Add(ptTrans);
                }
               Clear();
               AddRange(rotData);
               
            }
            catch (Exception)
            {

                throw;
            }
        }
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
        public void CenterToXMidpoint()
        {
            int midCount = (int)Math.Round(this.Count / 2.0);
            var minMax = GetMinMaxY();
            var midYData = (minMax.Item1 + minMax.Item2) / 2.0;
            double x1 = 0;
            for (int i = 1; i < midCount; i++)
            {
                if ((this[i - 1].Y < midYData && this[i].Y > midYData)
                    || (this[i - 1].Y > midYData && this[i].Y < midYData))
                {
                    x1 = (this[i - 1].X + this[i].X) / 2.0;
                    break;
                }
            }
            double x2 = 0;
            for (int i = midCount; i < this.Count; i++)
            {
                if ((this[i - 1].Y < midYData && this[i].Y > midYData)
                    || (this[i - 1].Y > midYData && this[i].Y < midYData))
                {
                    x2 = (this[i - 1].X + this[i].X) / 2.0;
                    break;
                }
            }
            double midX = (x1 + x2) / 2.0;
            CartData transData = new CartData();
            foreach (Vector3 pt in this)
            {
                transData.Add(new Vector3(pt.X - midX, pt.Y, pt.Z));
            }
        }
        public CylData AsCylData()
        {
            try
            {
                var cylData = new CylData();
                var pts = new List<PointCyl>();
                foreach (var pt in this)
                {
                    var newPt = new PointCyl(pt);
                    pts.Add(newPt);
                }
                cylData.AddRange(pts);
                return cylData;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public void Translate(Vector3 translation)
        {
            try
            {
                var newPts = new List<Vector3>();
                foreach (Vector3 pt in this)
                {
                    var newPt = pt.Translate(translation);
                    newPts.Add(newPt);
                }
                this.Clear();
                this.AddRange(newPts);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void RotateZ(double angleRad, Vector3 aboutPt)
        {
            try
            {
                var newPts = new List<Vector3>();
                foreach (Vector3 pt in this)
                {
                    var rotPt = pt.RotateZ(aboutPt, angleRad);
                    newPts.Add(rotPt);
                }
                this.Clear();
                this.AddRange(newPts);
            }
            catch (Exception)
            {

                throw;
            }
          
        }
        public DisplayData AsScreenData(ViewPlane viewPlane)
        {
            try
            {
                var pts = new DisplayData();
                foreach (Vector3 v in this)
                {
                    switch (viewPlane)
                    {
                        case ViewPlane.XZ:
                            pts.Add(new PointF((float)v.X, (float)v.Z));
                            break;
                        case ViewPlane.YZ:
                            pts.Add(new PointF((float)v.Y, (float)v.Z));
                            break;
                        case ViewPlane.XY:
                        default:
                            pts.Add(new PointF((float)v.X, (float)v.Y));
                            break;
                    }

                }
                return pts;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public BoundingBox BoundingBox
        {
            get
            {                
                if (_boundingBox == null)
                {
                    _boundingBox = DataUtilities.GetBB(this);

                }
                return _boundingBox;
            }
        }
        BoundingBox _boundingBox;
       
        public CartData()
        {          
        }
    }
}
