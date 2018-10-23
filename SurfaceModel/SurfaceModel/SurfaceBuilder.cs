using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
namespace SurfaceModel
{
    class SurfaceBuilder<T>where T :SurfacePoint,new()
    {
        //static public ISurface<T> Build(Type surfaceType,List<T>points, BoundingBox boundingBox,double meshSize)
        //{
        //    if(surfaceType == typeof(Octree<T>))
        //    {
        //        return OctreeBuilder<T>.Build(points, boundingBox, meshSize);
        //    }
        //    //else
        //    //{
        //    //    return Surface2DBuilder<T>.Build( boundingBox, meshSize);
        //    //}
        //}
        static public Surface2D<T> Build2D(BoundingBox boundingBox, double meshSize)
        {
            return Surface2DBuilder<T>.Build(boundingBox, meshSize);
        }

    }
}
