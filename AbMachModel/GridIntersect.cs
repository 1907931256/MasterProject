using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;

namespace AbMachModel
{
    public class GridIntersect
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 Normal { get; set; }
        public Ray2 IntersectRay { get; set; }
        public double Mrr { get; set; }
    }

}
