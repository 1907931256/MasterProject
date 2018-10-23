using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
namespace SurfaceModel
{
    public class SurfacePoint
    {
      
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        
        public SurfacePoint()
        {
            Position = new Vector3();
            Normal = new Vector3();           
        }
        public SurfacePoint(Vector3 position)
        {
            Position = new Vector3(position);
            Normal = new Vector3();
        }
        public SurfacePoint(Vector3 position, Vector3 normal)
        {
            Position = new Vector3(position);
            Normal = new Vector3(normal); ;
        }
        public SurfacePoint(SurfacePoint pt)
        {
            Position = new Vector3(pt.Position);
            Normal = new Vector3(pt.Normal);            
        }
    }
 
}
