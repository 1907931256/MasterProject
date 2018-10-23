using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryLib
{
  
    public enum PlaneName
    {
        XY,
        XZ,
        YZ
    }
    public class Plane
    {
        
        public static Plane XYPlane
        {            
            get
            {               
                return new Plane(new Vector3(0, 0, 1));
            }

        }
        public static Plane XZPlane
        {
            get
            {
                return new Plane(new Vector3(0, 1, 0));
            }

        }
        public static Plane YZPlane
        {

            get
            {
                return new Plane(new Vector3(1, 0, 0));
            }

        }
        public Vector3 Normal { get; set; }
        
        public Plane (Vector3 normal)
        {
            Normal = normal;
        }
        public Plane(Vector3 v1,Vector3 v2)
        {
            Vector3 n = v1.Cross(v2);
            n.Normalize();
            Normal = n;
        }        
        public Plane (Vector3 v0, Vector3 v1,Vector3 v2)
        {
            Vector3 va = v0 - v1;
            Vector3 vb = v0 - v2;
            Vector3 n = va.Cross(vb);
            n.Normalize();
            Normal = n;
        }
    }
}
