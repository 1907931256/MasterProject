using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
using DataLib;
namespace SurfaceModel
{
    public class TriMesh:List<Triangle>
    {
        public TriMesh()
        {
            boundingBox = new BoundingBox();
        }
        int GetMinStripLength(CartGridData pointStripList)
        {
            int minStripLen = int.MaxValue;

            foreach (var strip in pointStripList)
            {
                if (strip.Count < minStripLen)
                {
                    minStripLen = strip.Count;
                }
            }
            return minStripLen;
        }
        public void BuildFromGrid(CartGridData pointStripList)
        {
            int maxCount = GetMinStripLength(pointStripList);
            for (int i = 0; i < pointStripList.Count - 1; i++)
            {
               
                for (int j = 0; j < maxCount - 1; j++)
                {
                    var p1 = new Vector3(pointStripList[i][j]);
                    var p2 = new Vector3(pointStripList[i][j + 1]);
                    var p3 = new Vector3(pointStripList[i + 1][j]);
                    var p4 = new Vector3(pointStripList[i + 1][j + 1]);

                    var t1 = new Triangle(p1, p2, p3);
                    var t2 = new Triangle(p2, p4, p3);

                    this.Add(t1);
                    this.Add(t2);
                }
            }
            getBoundingBox();
        }
        BoundingBox boundingBox;
        public BoundingBox BoundingBox
        {
            get
            {
                return boundingBox;
            }
        }
        void getBoundingBox()
        {
            var points = new List<Vector3>() ;
            foreach (Triangle tri in this)
            {
                points.AddRange(tri.Vertices);
            }
            boundingBox = BoundingBoxBuilder.FromPtArray(points.ToArray());
        }
        
    }
}
