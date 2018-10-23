using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurfaceFileLib
{
    /// <summary>
    /// contains ply Face indices
    /// </summary>
    public class PlyFace
    {
        public List<int> Indices { get { return _indices; } }
        List<int> _indices;
        
        public PlyFace(List<int> indices)
        {
            _indices = indices;
        }
        public PlyFace(GeometryLib.Triangle tri)
        {
            _indices = new List<int>();
            _indices.Add(tri.Vertices[0].ID);
            _indices.Add(tri.Vertices[1].ID);
            _indices.Add(tri.Vertices[2].ID);
        }
    }
}
