using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
using SurfaceModel;
namespace SurfaceModel
{
   

    public class Surface2DBuilder<T> where T : SurfacePoint, new()
    {

        public static Surface2D<T> Build(Vector3 min, Vector3 max,double meshSize)
        {
            return new Surface2D<T>(new BoundingBox(min, max), meshSize);            
        }
        public static Surface2D<T> Build(BoundingBox BoundingBox,double meshSize)
        {
            return new Surface2D<T>(BoundingBox, meshSize);
        }
        public static void SetTargetSurface(double targetValue)
        {
            //SetTarget(targetValue);
        }
        public static void SetStartSurface(double startValue)
        {
           // SetStart(startValue);
           // SetModel(startValue);
        }
        
    }
}
