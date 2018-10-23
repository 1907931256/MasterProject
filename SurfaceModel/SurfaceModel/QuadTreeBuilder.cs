using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
namespace SurfaceModel
{

   
    class QuadTreeBuilder<T> where T : SurfacePoint,new() 
    {
        
        static public QuadTree<T> Build(List<Vector3> points, double minPointSpacing)
        {
            try
            {
                BoundingBox boundingBox = BoundingBoxBuilder.CubeFromPtArray(points);
                QuadTree<T> tree = new QuadTree<T>(boundingBox, minPointSpacing);
                foreach (Vector3 pt in points)
                {
                    T octpt = new T
                    {
                        Position = pt
                    };
                    tree.Insert(octpt);
                }

                return tree;

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
