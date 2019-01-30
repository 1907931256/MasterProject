using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
using System.Drawing;
namespace DataLib
{
    public class CartGridData : List<CartData>
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
        BoundingBox _boundingBox;
       
        public CartGridData ()
        {           
        }
    }
}
