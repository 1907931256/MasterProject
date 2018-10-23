using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;

namespace DataLib
{
    public class CylGridData : List<CylData>
    {
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
         public CylData AsCylData()
        {
            var stripd = new CylData();
            foreach (var strip in this)
            {
                foreach (var pt in strip)
                {
                    var ptnew = new PointCyl(pt.R, pt.ThetaRad, pt.Z, pt.Col, pt.ID);
                    stripd.Add(ptnew);
                }
            }
            return stripd;
        }
        BoundingBox _boundingBox;
    }
}
