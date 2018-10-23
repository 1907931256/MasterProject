using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;

namespace SurfaceModel
{
    
    public class PointCloud:List<SurfacePoint>
    {


       
        public bool ContainsNormals{ get { return _containsNormals; } }
        public BoundingBox BoundingBox { get { return _boundingBox; } }
        
        bool _containsNormals;
        BoundingBox _boundingBox;

        private void getExtents()
        {
            List<Vector3> points = new List<Vector3>();
            foreach(SurfacePoint sp in this)
            {
                points.Add(sp.Position);
            }
            _boundingBox = BoundingBoxBuilder.FromPtArray(points.ToArray());
        }

       
       
        public PointCloud(List<PointCyl> pts)
        {
            
            foreach (PointCyl pt in pts)
            {
                Vector3 pt3d = new Vector3(pt);
                Add(new SurfacePoint(pt3d));
            }
           
            _containsNormals = false;
            getExtents();
        }
        public PointCloud(List<Vector3> pts)
        {
            foreach (Vector3 pt in pts)
            {
               
                Add(new SurfacePoint(pt));
            }
            _containsNormals = false;
            getExtents();
        }
        public PointCloud(List<Vector3> pts,List<Vector3> norms)
        {
            int count = Math.Min(pts.Count, norms.Count);
            for(int i=0; i<count;i++)
            {
                Add(new SurfacePoint(pts[i],norms[i]));
            }
            _containsNormals = true;
            getExtents();
        }
       

    }

}
