using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;

namespace SurfaceModel
{
    public enum SurfaceType
    {
        Array2D,
        Octree3D
    }
    public interface ISurface<T> where T : SurfacePoint, new()
    {
        double MeshSize { get; }       
        BoundingBox BoundingBox {get;}
        SurfaceType Type { get; }
        void Insert(List<T> points);
        void Insert(T point);
        IntersectionRecord GetIntersection(Ray ray);
        Vector3 GetNormalAt(Vector3 pt);
        List<T> GetAllPoints();
        T GetPointAt(Vector3 pt);
        List<T> GetPointsInsideBox(BoundingBox box);
        List<T> CutPointsInsideBox(BoundingBox box);
        ISurface<T> Clone();
       
    }
}
