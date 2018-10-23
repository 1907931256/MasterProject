using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
namespace SurfaceFileLib
{
    /// <summary>
    /// contains ply edge vertices
    /// </summary>
    public class PlyEdge
    {
        public int Vertex1 { get; set; }
        public int Vertex2 { get; set; }
        public  RGBColor Color { get; set; }
        public bool ContainsColor;
        public PlyEdge(bool containsColorIn)
        {
            ContainsColor = containsColorIn;
            if (ContainsColor)
                Color = new RGBColor();

        }
    }
}
