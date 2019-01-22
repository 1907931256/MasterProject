using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;

namespace DataLib
{
    public class CylData : List<PointCyl>
    {
        public double NominalMinDiam { get; set; }
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
        public void RotateByThetaRad(double phaseShiftRad)
        {
            foreach(var pt in this)
            {
                var thetaShift = (pt.ThetaRad + phaseShiftRad) % (Math.PI * 2.0);
                pt.ThetaRad = thetaShift;
            }
            SortByTheta();
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
    }
}
