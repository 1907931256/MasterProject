using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
namespace GeometryLib
{
    public class Triangle
    {
        public Vector3[] Vertices { get { return vert; } }
        public Vector3 Normal { get { return normal; } }
        public UInt32 Index { get { return index; } }
        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
        }

        Vector3 v01;
        Vector3 v12;
        Vector3 v20;
        Vector3[] vert;
        Vector3 normal;
        BoundingBox boundingBox;
        UInt32 index;
       
        private void getBoundingBox()
        {            
            boundingBox = BoundingBoxBuilder.FromPtArray(vert);
        }   
        public List<Vector3> AsPointGrid(double pointSpacing)
        {
            try
            {
                var gridPoints = new List<Vector3>();
                var side1Points = new List<Vector3>();
                var side2Points = new List<Vector3>();
                var side1 = new Line(Vertices[0], Vertices[1]);
                var side2 = new Line(Vertices[0], Vertices[2]);
                side1Points.AddRange(Geometry.BreakMany(side1, pointSpacing));
                side2Points.AddRange(Geometry.BreakMany(side2, pointSpacing));
                foreach (Vector3 side1Point in side1Points)
                {
                    Vector3 translation = side1Point - Vertices[0];
                    foreach (Vector3 side2Point in side2Points)
                    {
                        Vector3 s2Trans = side2Point.Translate(translation);
                        if (contains(s2Trans))
                        {
                            gridPoints.Add(s2Trans);
                        }
                    }
                }
                return gridPoints;
            }
            catch (Exception)
            {

                throw;
            }
          
        }
        /// <summary>
        /// splits triangle into 3 smaller triangles from intersect point of ray
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="rayOrigin"></param>
        /// <returns></returns>
        public List<Triangle> Split(Ray ray, Vector3 rayOrigin)
        {
            try
            {
                Vector3 pt;
                List<Triangle> tris = new List<Triangle>();
                if (IntersectedBy(ray, out pt))
                {
                    tris.Add(new Triangle(vert[0], vert[1], pt, index));
                    tris.Add(new Triangle(vert[1], vert[2], pt, index));
                    tris.Add(new Triangle(vert[2], vert[0], pt, index));
                }
                else
                {
                    tris.Add(this);
                }
                return tris;
            }
            catch (Exception)
            {

                throw;
            }
           

        }
        /// <summary>
        /// tests if triangle is intersected by vector from rayOrigin
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="rayOrigin"></param>
        /// <returns></returns>
        public bool IntersectedBy(Ray ray, out Vector3 projPt)
        {
            try
            {
                bool intersects = false;

                if (IntersectsPlane(ray, out projPt) && contains(projPt))
                {
                    intersects = true;
                }

                return intersects;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        /// <summary>
        /// test if triangle contains point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        private bool contains(Vector3 pt)
        {
            try
            {
                getSideVectors();
                Vector3 v0pt = vert[0] - pt;
                Vector3 v1pt = vert[1] - pt;
                Vector3 v2pt = vert[2] - pt;
                double testSide0 = v01.Cross(v0pt).Dot(Normal);
                double testSide1 = v12.Cross(v1pt).Dot(Normal);
                double testSide2 = v20.Cross(v2pt).Dot(Normal);
                if ((testSide0 >= 0) && (testSide1 >= 0) && (testSide2 >= 0))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        /// <summary>
        /// test if ray projects onto triangle plane returns true and projected point in out
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="rayOrigin"></param>
        /// <param name="projPt"></param>
        /// <returns></returns>
        public bool IntersectsPlane(Ray ray,  out Vector3 projPt)
        {
            try
            {
                Vector3 op = vert[0] - ray.Origin;
                ray.Direction.Normalize();
                double denom = ray.Direction.Dot(Normal);
                if (denom != 0)
                {
                    double t = -1 * op.Dot(Normal) / denom;
                    projPt = ray.Origin + t * ray.Direction;

                    return true;
                }
                else
                {
                    projPt = new Vector3();
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        
        void getSideVectors()
        {
            v01 = vert[1] - vert[0];
            v12 = vert[2] - vert[1];
            v20 = vert[0] - vert[2];
        }
        public Triangle()
        {
            vert = new Vector3[3];
            normal = new Vector3();
            index = 0;
            boundingBox = new BoundingBox();
        }
        public Triangle(Vector3 v0, Vector3 v1, Vector3 v2, UInt32 index)
        {
            vert = new Vector3[3];
            vert[0] = v0;
            vert[1] = v1;
            vert[2] = v2;            
            normal = v01.Cross(v12);
            this.index = index;
            getBoundingBox();
            getSideVectors();
        }
        public Triangle(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 norm, UInt32 index)
        {
            vert = new Vector3[3];
            vert[0] = v0;
            vert[1] = v1;
            vert[2] = v2;

            normal = norm;
            this.index = index;
            getBoundingBox();
            getSideVectors();
        }
    }
}
