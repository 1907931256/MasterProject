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
        /// <summary>
        /// convert cylinder grid to cartesian grid
        /// </summary>
        /// <param name="correctedRingList"></param>
        /// <returns></returns>
        public CartGridData AsCartGridData()
        {
            try
            {
                var stripList = new CartGridData();
                foreach (var cylstrip in this)
                {
                    var strip = new CartData(cylstrip.FileName);
                    foreach (var ptCyl in cylstrip)
                    {
                        strip.Add(new Vector3(ptCyl));
                    }
                    stripList.Add(strip);
                }
                return stripList;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public CylData AsCylData()
        {
            
            var stripd = new CylData(this[0].FileName);
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
       
        public CylGridData()
        {
        }
    }
}
