using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryLib
{
    public class IntersectionRecord2:Vector2
    {
        public bool Intersects { get; set; }
        public IntersectionRecord2()
        {
            Intersects = false;
            X = 0;
            Y = 0;
           
        }
        public IntersectionRecord2(Vector2 intersection, bool intersects)
        {
            X = intersection.X;
            Y = intersection.Y;
           
            Intersects = intersects;
        }
    }
    public class IntersectionRecord:Vector3
    {
        
        public bool Intersects { get; set; }
        public IntersectionRecord()
        {
            Intersects = false;
            X = 0;
            Y = 0;
            Z = 0;
        }
        public IntersectionRecord(Vector3 intersection,bool intersects)
        {
            X = intersection.X;
            Y = intersection.Y;
            Z = intersection.Z;
            Intersects = intersects;
        }
    }
}
