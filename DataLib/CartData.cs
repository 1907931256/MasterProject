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

        public DisplayData AsScreenData(ViewPlane viewPlane)
        {
            var pts = new DisplayData();
            foreach(Vector3 v in this)
            {
                switch(viewPlane)
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
