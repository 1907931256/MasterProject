using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;

namespace SurfaceModel
{
    public  class Triangle
    {
        public Vector3 Vert0 { get; set; }
        public Vector3 Vert1 { get; set; }
        public Vector3 Vert2 { get; set; }
        public Vector3 Normal { get; set; }
        public UInt32 Index { get; set; }
        public List<UInt32> Neighbors { get; set; }
        public BoundingBox BoundingBox { get { return BoundingBox; } }
        public UInt16 Attrib { get; set; }
        private BoundingBox boundingBox;
        public Triangle()
        {
            Vert1 = new Vector3();
            Vert2 = new Vector3();
            Vert0 = new Vector3();
            Normal = new Vector3();
            
            Attrib = 0;
            boundingBox = new BoundingBox();
        }
        public Triangle(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 norm,UInt16 attribute,UInt32 index)
        {
            Vert1 = v1;
            Vert2 = v2;
            Vert0 = v0;
            Normal = norm;
            Index = index;
            Attrib = attribute;
            getBoundingBox();
        }
        public Triangle(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            Vector3 v12 = new Vector3(Vert1.X - Vert0.X, Vert1.Y - Vert0.Y, Vert1.Z - Vert0.Z);
            Vector3 v23 = new Vector3(Vert2.X - Vert1.X, Vert2.Y - Vert1.Y, Vert2.Z - Vert1.Z);
            Normal = v12.Cross(v23);
            Attrib = 0;
            Index = 0;
            getBoundingBox();
        }
        /// <summary>
        /// splits triangle into 3 smaller triangles from intersect point of ray
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="rayOrigin"></param>
        /// <returns></returns>
        public List<Triangle> Split(Vector3 ray, Vector3 rayOrigin)
        {
            Vector3 pt;
            List<Triangle> tris = new List<Triangle>();
            if(intersectedBy(ray,rayOrigin,out pt))
            {
                Triangle tri1 = new Triangle(Vert0, Vert1, pt, Normal, Attrib, Index);
                Triangle tri2 = new Triangle(Vert1, Vert2, pt, Normal, Attrib, Index);
                Triangle tri3 = new Triangle(Vert2, Vert0, pt, Normal, Attrib, Index);
                tris.Add(tri1);
                tris.Add(tri2);
                tris.Add(tri3);
            }
            return tris;

        }
        /// <summary>
        /// tests if triangle is intersected by vector from rayOrigin
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="rayOrigin"></param>
        /// <returns></returns>
        public bool intersectedBy(Vector3 ray, Vector3 rayOrigin, out Vector3 projPt)
        {           
            bool intersects = false;            
            
            if (project(ray, rayOrigin, out projPt))
            {
                if(contains(projPt))
                    intersects = true;
            }
            
                       
            return intersects;
        }
        /// <summary>
        /// test if triangle contains point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
         bool contains(Vector3 pt)
        {
            Vector3 v12 = new Vector3(Vert1.X - Vert0.X, Vert1.Y - Vert0.Y, Vert1.Z - Vert0.Z);
            Vector3 v23 = new Vector3(Vert2.X - Vert1.X, Vert2.Y - Vert1.Y, Vert2.Z - Vert1.Z);
            Vector3 v31 = new Vector3(Vert2.X - Vert0.X, Vert2.Y - Vert0.Y, Vert2.Z - Vert0.Z);

            Vector3 v1pt = new Vector3(Vert0.X - pt.X, Vert0.Y - pt.Y, Vert0.Z - pt.Z);
            Vector3 v2pt = new Vector3(Vert1.X - pt.X, Vert1.Y - pt.Y, Vert1.Z - pt.Z); 
            Vector3 v3pt = new Vector3(Vert2.X - pt.X, Vert2.Y - pt.Y, Vert2.Z - pt.Z);
            double testSide1 = v12.Cross(v1pt).Dot(Normal);
            double testSide2 = v23.Cross(v2pt).Dot(Normal);
            double testSide3 = v31.Cross(v3pt).Dot(Normal);
            if ((testSide1 >= 0) && (testSide2 >= 0) && (testSide3 >= 0))
                return true;
            else
                return false;
        }
        /// <summary>
        /// test if ray projects onto triangle plane returns true and projected point in out
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="rayOrigin"></param>
        /// <param name="projPt"></param>
        /// <returns></returns>
        public bool project(Vector3 ray, Vector3 rayOrigin, out Vector3 projPt)
        {
            
            Vector3 op = new Vector3(Vert0.X - rayOrigin.X, Vert0.Y - rayOrigin.Y, Vert0.Z - rayOrigin.Z);
            ray.Normalize();
            double denom = ray.Dot(Normal);
            if (denom != 0)
            {
                double t = -1 * op.Dot(Normal) / denom;
                projPt = new Vector3(rayOrigin.X + t * ray.X, rayOrigin.Y + t * ray.Y, rayOrigin.Z + t * ray.Z);

                return true;
            }
            else
            {
                projPt = new Vector3();
                return false;
            }
        }
        private void getBoundingBox()
        {
            boundingBox = BoundingBoxBuilder.FromPtArray(new Vector3[] { Vert0,Vert1,Vert2 });          
            
        }
    }
}
