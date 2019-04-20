using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
namespace SurfaceFileLib
{
    /// <summary>
    /// contains ply vertex, normal,color
    /// </summary>
    public class PlyVertex : Vector3
    {
        
        
        public Vector3 Normal
        {
            get { return _normal; }
        }
        public bool UsedInFace { get; set; }
        public bool ContainsColor { get; set; }
        public bool ContainsNormal
        {
            get { return _containsNormal; }
        }
        int _normalCount;
        Vector3 _normal;
        bool _containsNormal;
        
        public void AddNormal(Vector3 newNormal)
        {
            if (newNormal.Length != 0)
            {
                _normalCount++;
                _containsNormal = true;
            }
            _normal = _normal + newNormal;
        }
        public PlyVertex()
        {
            ContainsColor = false;
            _containsNormal = false;
            
           _normal = new Vector3();
           
        }
        public PlyVertex(Vector3 vert)
        {
            ContainsColor = true;
            _containsNormal = false;
            X = vert.X;
            Y = vert.Y;
            Z = vert.Z;
            ID = vert.ID;
            _normal = new Vector3();
            Col = vert.Col;
            
        }
        public PlyVertex(Vector3 vert,int id)
        {
            ContainsColor = true;
            _containsNormal = false;
            X = vert.X;
            Y = vert.Y;
            Z = vert.Z;
            ID = id;
            _normal = new Vector3();
            Col = vert.Col;

        }
        public PlyVertex(Vector3 vert, Vector3 normal)
        {
            ContainsColor = true;
            _containsNormal = true;
            _normalCount++;
            X = vert.X;
            Y = vert.Y;
            Z = vert.Z;
            _normal = normal;
            Col = vert.Col;
            
        }
        public PlyVertex(Vector3 vert, Vector3 normal,System.Drawing.Color color)
        {
            ContainsColor = true;
            _containsNormal = true;
            _normalCount++;
            X = vert.X;
            Y = vert.Y;
            Z = vert.Z;
            _normal = normal;
            Col = color;
        }

    }
}
