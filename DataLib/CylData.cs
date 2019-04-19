using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
using System.Drawing;
namespace DataLib
{
   
    public class CylData : List<PointCyl>
    {
        public string FileName { get; private set; }
        public double MinRadius { get; set; }
        public DisplayData AsDisplayData( ViewPlane viewplane)
        {
            var pts = new DisplayData(FileName);
            foreach (PointCyl v in this)
            {
                switch (viewplane)
                {
                    case ViewPlane.ZR:
                        pts.Add(new PointF((float)v.Z, (float)v.R));
                        break;
                    case ViewPlane.THETAR:
                    default:
                        pts.Add(new PointF((float)v.ThetaDeg , (float)v.R));
                        break;                    
                }
                
            }
            return pts;
        }

        public BoundingBox BoundingBox
        {
            get
            {
                if (_boundingBox == null)
                {
                    _boundingBox = DataUtil.GetBB(this);

                }
                return _boundingBox;
            }
        }
        public void Translate(Vector3 translation)
        {
            try
            {
                var cartData = this.AsCartData();
                cartData.Translate(translation);
                this.Clear();
                this.AddRange(cartData.AsCylData());                    
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CartData AsCartData()
        {
            try
            {
                var cartData = new CartData(FileName);
                var pts = new List<Vector3>();
                foreach (var pt in this)
                {
                    var newPt = new Vector3(pt);
                    pts.Add(newPt);
                }
                cartData.AddRange(pts);
                return cartData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void RotateByThetaRad(double phaseShiftRad)
        {
            foreach(var pt in this)
            {
                var thetaShift = (pt.ThetaRad + phaseShiftRad) % (Math.PI * 2.0);
                pt.ThetaRad = thetaShift;
            }
            SortByTheta();
        }
        public Tuple<double, double> GetMinMaxR()
        {
            double maxYData = double.MinValue;
            double minYData = double.MaxValue;
            foreach (var pt in this)
            {
                if (pt.R > maxYData)
                {
                    maxYData = pt.R;
                }
                if (pt.R < minYData)
                {
                    minYData = pt.R;
                }
            }
            return new Tuple<double, double>(minYData, maxYData);
        }
        public void SortByR()
        {
            var rList = new List<double>();
            var ptList = new List<PointCyl>();
            foreach (var pt in this)
            {
                rList.Add(pt.R);
                ptList.Add(pt);
            }
            var arr = ptList.ToArray();
            Array.Sort(rList.ToArray(), arr);
            this.Clear();
            this.AddRange(arr);
        }
        public void SortByZ()
        {
            var zList = new List<double>();
            var ptList = new List<PointCyl>();
            foreach (var pt in this)
            {
                zList.Add(pt.Z);
                ptList.Add(pt);
            }
            var arr = ptList.ToArray();
            Array.Sort(zList.ToArray(), arr);
            this.Clear();
            this.AddRange(arr);
        }
        public void SortByTheta()
        {
            var thetaList = new List<double>();
            var ptList = new List<PointCyl>();
            foreach (var pt in this)
            {
                thetaList.Add(pt.ThetaRad);
                ptList.Add(pt);
            }
            var arr = ptList.ToArray();
            Array.Sort(thetaList.ToArray(), arr);
            this.Clear();
            this.AddRange(arr);
        }
        public void SortByIndex()
        {
            var thetaList = new List<int>();
            var ptList = new List<PointCyl>();
            foreach (var pt in this)
            {
                thetaList.Add(pt.ID);
                ptList.Add(pt);
            }
            var arr = ptList.ToArray();
            Array.Sort(thetaList.ToArray(), arr);
            this.Clear();
            this.AddRange(arr);
        }
        BoundingBox _boundingBox;

        public CylData(string filename)
        {
            FileName = filename;
        }
        public CylData()
        {
            
        }
    }
}
