using System;
using System.Collections.Generic;
using GeometryLib;
using System.Threading.Tasks;

namespace SurfaceModel
{
    
    public class OctreeBuilder<T> where T : SurfacePoint,new() 
    {
               
        static Vector3[] sortNormalToPlane(List<Vector3>points)
        {
            try
            {
                Vector3 n = new Vector3(points[1] - points[0]);
                n.Normalize();
                double xdot = n.Dot(Vector3.XAxis);
                double ydot = n.Dot(Vector3.YAxis);
                double zdot = n.Dot(Vector3.ZAxis);

                List<double> keys = new List<double>();
                if (xdot > ydot && xdot > zdot)
                {
                    foreach (Vector3 pt in points)
                        keys.Add(pt.X);
                }
                if (ydot > xdot && ydot > zdot)
                {
                    foreach (Vector3 pt in points)
                        keys.Add(pt.Y);
                }
                if (zdot > xdot && zdot > ydot)
                {
                    foreach (Vector3 pt in points)
                        keys.Add(pt.Z);

                }

                Vector3[] pointArray = points.ToArray();
                Array.Sort(keys.ToArray(), pointArray);
                return pointArray;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
       
        static public Octree<T> Build(TriMesh surface, double minPointSpacing)
        {
            try
            {
                var gridPoints = new List<Vector3>();
                foreach (Triangle tri in surface)
                {                    
                    gridPoints.AddRange(tri.AsPointGrid(minPointSpacing));
                }
                return Build(gridPoints, minPointSpacing * .5);

            }
            catch (Exception)
            {
                throw;
            }

        }
        static public Octree<T> Build(List<Line> alongContour, Line acrossContour, double minPointSpacing)
        {
            try
            {
                var alongPoints = new List<Vector3>();
                var acrossPoints = new List<Vector3>();
                var gridPoints = new List<Vector3>();

                foreach (Line ent in alongContour)
                {
                    List<Vector3> ptList = GeomUtilities.BreakMany(ent, minPointSpacing);
                    alongPoints.AddRange(ptList);
                }
                acrossPoints = GeomUtilities.BreakMany(acrossContour, minPointSpacing);

                Vector3[] sortedPts = sortNormalToPlane(acrossPoints);

                foreach (Vector3 alongPoint in alongPoints)
                {
                    Vector3 trans = alongPoint - sortedPts[0];
                    foreach (Vector3 acrossPt in sortedPts)
                    {
                        Vector3 transPt = acrossPt.Translate(trans);
                        gridPoints.Add(transPt);
                    }
                }

                return Build(gridPoints, minPointSpacing * .5);
            }
            catch (Exception)
            {

                throw;
            }


        }
      
        static public Octree<T> Build(List<Vector3> points,double minPointSpacing)
        {
            try
            {
                BoundingBox boundingBox = BoundingBoxBuilder.CubeFromPtArray(points);
                Octree<T> octree = new Octree<T>(boundingBox, minPointSpacing);
               // int index = 0;
                foreach (Vector3 pt in points)
                {
                    T octpt = new T();
                    //octpt.Index = index++;
                    octpt.Position = pt;
                    octree.Insert(octpt);
                   
                }
                
                return octree;

            }
            catch (Exception)
            {

                throw;
            }
           
        }
        
        static public Octree<T> Build(List<T> Points, double minPointSpacing)
        {
            try
            {
                List<Vector3> points = new List<Vector3>();
                foreach (T pt in Points)
                {
                    points.Add(pt.Position);
                }
                BoundingBox boundingBox = BoundingBoxBuilder.CubeFromPtArray(points);


                Octree<T> octree = new Octree<T>(boundingBox, minPointSpacing);
               
                octree.Insert(Points);
                
                return octree;
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        static public Octree<T> Build(List<T> Points,BoundingBox boundingBox,double minPointSpacing)
        {
            try
            {
                Octree<T> octree = new Octree<T>(boundingBox, minPointSpacing);
                
                octree.Insert(Points);

                return octree;
            }
            catch (Exception)
            {
                throw;
            }
        }
       
    }
}
