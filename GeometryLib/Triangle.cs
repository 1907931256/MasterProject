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
                var side12 = new Line(vert[1], vert[2]);
                var side01 = new Line(vert[0], vert[1]);
                var side02 = new Line(vert[0], vert[2]);
                Vector3 v20 = vert[0] - vert[2];
                Vector3 v02 = vert[2] - vert[0];
                Vector3 v01 = vert[1] - vert[0];
                Vector3 v12 = vert[2] - vert[1];
                Vector3 v21 = vert[1] - vert[2];
                Vector3 v10 = vert[0] - vert[1];
                double theta0 = Math.Acos(v01.Dot(v02) / (v01.Length * v02.Length));
                double theta1= Math.Acos(v10.Dot(v12) / (v01.Length * v12.Length));
                double theta2 = Math.Acos(v20.Dot(v21) / (v02.Length * v12.Length));
                double thetaMax = Math.Max(theta1, Math.Max(theta0, theta2));
                double side1Spacing = pointSpacing / Math.Sin(thetaMax);
                Vector3 basePoint = new Vector3(vert[0]);
                if(theta0==thetaMax)
                {
                    side1Points.AddRange(GeomUtilities.BreakMany(side01, side1Spacing));
                    side2Points.AddRange(GeomUtilities.BreakMany(side02, pointSpacing));
                    basePoint = vert[0];
                }
                else
                {
                    if (theta1 == thetaMax)
                    {
                        side1Points.AddRange(GeomUtilities.BreakMany(side01, side1Spacing));
                        side2Points.AddRange(GeomUtilities.BreakMany(side12, pointSpacing));
                        basePoint = vert[1];
                    }
                    else
                    {
                        if (theta2 == thetaMax)
                        {
                            side1Points.AddRange(GeomUtilities.BreakMany(side02, side1Spacing));
                            side2Points.AddRange(GeomUtilities.BreakMany(side12, pointSpacing));
                            basePoint = vert[2];
                        }
                    }                    
                }
               
                foreach (Vector3 side1Point in side1Points)
                {
                    Vector3 translation = side1Point - basePoint;
                    foreach (Vector3 side2Point in side2Points)
                    {
                        Vector3 s2Trans = side2Point.Translate(translation);
                        if (Contains(s2Trans))
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
       
        public List<Triangle> Split(Ray ray)
        {
            try
            {
               
                List<Triangle> tris = new List<Triangle>();
                IntersectionRecord pt = IntersectedBy(ray);
                if (pt.Intersects)
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
       
        public IntersectionRecord IntersectedBy(Ray ray)
        {
            try
            {
                IntersectionRecord pt = IntersectsTriPlane(ray);
                if (pt.Intersects && Contains(pt))
                {
                    pt.Intersects = true;
                }
                else
                {
                    pt.Intersects = false;
                }
                return pt;
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
        public bool Contains(Vector3 pt)
        {
            try
            {
                getSideVectors();
                Vector3 v0pt = pt - vert[0];
                Vector3 v1pt = pt - vert[1];
                Vector3 v2pt = pt - vert[2];
                double testSide0 = v01.Cross(v0pt).Dot(Normal);
                double testSide1 = v12.Cross(v1pt).Dot(Normal);
                double testSide2 = v20.Cross(v2pt).Dot(Normal);
                return ((testSide0 >= 0) && (testSide1 >= 0) && (testSide2 >= 0));
             
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
        public IntersectionRecord IntersectsTriPlane(Ray ray)
        {
            try
            {
                Vector3 op =  ray.Origin-vert[0];
                ray.Direction.Normalize();
                double denom = ray.Direction.Dot(Normal);
                if (denom != 0)
                {
                    double t = -1 * op.Dot(Normal) / denom;
                    Vector3 projPt = ray.Origin + t * ray.Direction;

                    return new IntersectionRecord(projPt,true);
                }
                else
                {
                    return new IntersectionRecord();
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
        public Triangle(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            vert = new Vector3[3];
            vert[0] = v0;
            vert[1] = v1;
            vert[2] = v2;
           
            this.index = 0;
            getBoundingBox();
            getSideVectors();
            normal = v01.Cross(v12);
            normal.Normalize();
        }

        public Triangle(Vector3 v0, Vector3 v1, Vector3 v2, UInt32 index)
        {
            vert = new Vector3[3];
            vert[0] = v0;
            vert[1] = v1;
            vert[2] = v2;            
           
            this.index = index;
            getBoundingBox();
            getSideVectors();
            normal = v01.Cross(v12);
            normal.Normalize();
        }
        public Triangle(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 norm, UInt32 index)
        {
            vert = new Vector3[3];
            vert[0] = v0;
            vert[1] = v1;
            vert[2] = v2;

            normal = norm;
            normal.Normalize();
            this.index = index;
            getBoundingBox();
            getSideVectors();
        }
    }
}
