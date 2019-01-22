using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DataLib
{
  
    public class CartData : List<Vector3>
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
    }
}
