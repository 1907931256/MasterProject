using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
namespace SurfaceModel
{
    public class Octree<T> :Tree<T> where T : SurfacePoint, new()
    {
       
        public int PointCount;
       
        public int Level;
       
        
        public Octree()
        {
            
            initOctree();
        }
        public Octree(BoundingBox boundingBox,double minimumSeparation)
        {

            Children = new Tree<T>[8];
            Neighbors = new List<Tree<T>>();
            origin = new Vector3();
            isLeaf = true;
            _boundingBox = new BoundingBox();
            minSeparation = minimumSeparation;
            minSepSquared = minSeparation * minSeparation;
            _boundingBox = boundingBox;
            origin = _boundingBox.Min;
        }   
        void initOctree()
        {
           
        }
        
        
       
       
        public List<T> GetAllPoints()
        {
            try
            {
                List<T> results = new List<T>();
                GetAllPoints(ref results);
                return results;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<Vector3> GetAllPositions()
        {
            try
            {
                List<T> results = GetAllPoints();
                List<Vector3> points = new List<Vector3>();
                foreach (T oPt in results)
                {
                    points.Add(oPt.Position);
                }
                return points;
            }
            catch (Exception)
            {

                throw;
            }

        }
        //public void Insert(List<T> points)
        //{
        //    try
        //    {
        //        foreach (T pt in points)
        //        {
        //            if (pt != null)
        //                Insert(pt);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
        //public void Insert(T point)
        //{
        //    try
        //    {
        //        if (point != null)
        //        {
        //            if (isLeaf)
        //            {
        //                // if  at a leafnode and there's no data then put it here
        //                if (Data == null)
        //                {
        //                    Data = point;

        //                    return;
        //                }
        //                else//there is data then divide into octants and re-insert old data and new data if point is >min distance
        //                {
        //                    double separationSquared = point.Position.Distance2To(Data.Position);


        //                    if (separationSquared >= minSepSquared)
        //                    {
        //                        var oldPt = new T();
        //                        oldPt.Position = new Vector3(Data.Position);
        //                        //oldPt.Index = Data.Index;
        //                        oldPt.Normal = new Vector3(Data.Normal);

        //                        int oldOctant = getSegmentContaining(oldPt.Position);
        //                        int newOctant = getSegmentContaining(point.Position);
        //                        Data = null;

        //                        if (Children[oldOctant] == null)
        //                        {
        //                            makeChild(oldOctant);
        //                        }
        //                        if (Children[newOctant] == null)
        //                        {
        //                            makeChild(newOctant);
        //                        }
        //                        // insert old point and new point into children
        //                        Children[oldOctant].Insert(oldPt);
        //                        Children[newOctant].Insert(point);
        //                    }
        //                }
        //            }
        //            else//not at a leaf node then continue to child
        //            {

        //                int octant = getSegmentContaining(point.Position);

        //                if (Children[octant] == null)
        //                {
        //                    makeChild(octant);
        //                }
        //                Children[octant].Insert(point);
        //            }
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }


        //}
        public void Remove(List<T> points)
        {
            try
            {
                foreach (T pt in points)
                {
                    Remove(pt);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        //public void Remove(T point)
        //{
        //    try
        //    {
        //        if (isLeaf)
        //        {
        //            if (Data != null && point.Position.Distance2To(Data.Position) <= minSepSquared)
        //            {
        //                Data = null;
        //            }
        //            return;
        //        }
        //        else
        //        {
        //            int octant = getSegmentContaining(point.Position);
        //            if (Children[octant] == null)
        //            {
        //                makeChild(octant);
        //            }
        //            Children[octant].Remove(point);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
        //public List<T> CutPointsInsideBox(BoundingBox box)
        //{
        //    List<T> results = new List<T>();
        //    cutPointsInBox(box, ref results);
        //    return results;
        //}
        public Vector3 GetNormalAt(Vector3 point)
        {
            Vector3 normal = new Vector3();
            Vector3 v1 = new Vector3();
            Vector3 v2 = new Vector3();

            T nearestPt = GetPointAt(point);

            List<T> localPoints = GetNeighbors(nearestPt.Position);

            int i = 0;

            double v1Length = 0;
            while (i < localPoints.Count && v1Length == 0)
            {
                v1 = localPoints[i].Position - nearestPt.Position;
                v1Length = v1.Length;
                i++;
            }

            var normalList = new List<Vector3>();
            while (i < localPoints.Count)
            {
                v2 = localPoints[i].Position - nearestPt.Position;
                normal = v1.Cross(v2);
                if (normal.Length != 0)
                {
                    if (normal.Z < 0)
                    {
                        normal = v2.Cross(v1);
                    }
                    normalList.Add(normal);
                }

                i++;
            }
            if (normalList.Count > 1)
            {
                double xAve = 0;
                double yAve = 0;
                double zAve = 0;

                foreach (Vector3 n in normalList)
                {
                    xAve += n.X;
                    yAve += n.Y;
                    zAve += n.Z;
                }
                xAve /= normalList.Count;
                yAve /= normalList.Count;
                zAve /= normalList.Count;
                normal = new Vector3(xAve, yAve, zAve);
            }


            return normal;
        }
      
        //public List<T> GetKNearestPoints(Vector3 searchPoint, int searchQty)
        //{
        //    try
        //    {
        //        int resultCount = 0;
        //        var results = new List<T>();

        //        if (searchQty == 0)
        //        {
        //            return results;
        //        }

        //        double initSearchWidth = minSeparation * Math.Min(searchQty, 3);

        //        double searchSize = 1;
        //        double maxSearchDim = 2 * Math.Max((searchPoint - _boundingBox.Min).Length, (_boundingBox.Max - searchPoint).Length);
        //        Vector3 searchHalfDimension = new Vector3(initSearchWidth, initSearchWidth, initSearchWidth);
        //        // search and expand search box until point count is >= k
        //        do
        //        {
        //            var box = new BoundingBox(searchPoint - searchHalfDimension, searchPoint + searchHalfDimension);

        //            results = new List<T>();
        //            getPointsInBox(box, ref results);

        //            resultCount = results.Count;
        //            if (resultCount == 0)
        //            {
        //                searchSize *= 2;
        //            }
        //            else
        //            {
        //                searchSize *= ((double)searchQty / (double)resultCount);
        //            }

        //            searchHalfDimension = new Vector3(searchSize * initSearchWidth, searchSize * initSearchWidth, searchSize * initSearchWidth);
        //        } while (resultCount < searchQty && searchHalfDimension.Length < maxSearchDim);

        //        double[] distances = new double[resultCount];
        //        T[] resultArray = results.ToArray();

        //        // find closest k points to search point and add to output 
        //        if (resultCount > searchQty)
        //        {
        //            for (int i = 0; i < resultCount; i++)
        //            {
        //                distances[i] = searchPoint.Distance2To(resultArray[i].Position);
        //            }
        //            Array.Sort(distances, resultArray);

        //            return resultArray.ToList();

        //        }
        //        else
        //        {
        //            return results;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }


        //}
        public IntersectionRecord GetIntersection(Ray ray)
        {
            try
            {

                var searchBox = BoundingBoxBuilder.GetSearchCylinder(_boundingBox, ray.Origin, ray.Direction, minSeparation * 3);

                List<T> points = GetPointsInsideBox(searchBox);
                Vector3 intPt = new Vector3();

                double denom = (ray.Direction - ray.Origin).Length;
                Vector3 rayEnd = ray.Direction - ray.Origin;
                if (denom > 0 && points.Count == 1)
                {
                    return new IntersectionRecord(points[0].Position, true);
                }
                if (denom > 0 && points.Count > 1)
                {
                    double[] distArr = new double[points.Count];
                    Vector3[] ptArr = new Vector3[points.Count];
                    int i = 0;
                    foreach (T pt in points)
                    {

                        distArr[i] = GeomUtilities.RayPointDistance(ray, pt.Position);
                        ptArr[i] = pt.Position;
                        i++;
                    }
                    Array.Sort(distArr, ptArr);
                    return new IntersectionRecord(ptArr[0], true);
                }
                return new IntersectionRecord();
            }
            catch (Exception)
            {

                throw;
            }

        }
       
      
        protected override void MakeChild(int octant)
        {
            try
            {
                
                Vector3 newOrigin = new Vector3(origin);
                Vector3 newSize = .5 * _boundingBox.Size;
                newOrigin.X += ((octant & 4) == 0 ? 0 : newSize.X);
                newOrigin.Y += ((octant & 2) == 0 ? 0 : newSize.Y);
                newOrigin.Z += ((octant & 1) == 0 ? 0 : newSize.Z);
                Children[octant] = new Octree<T>(new BoundingBox(newOrigin, newOrigin + newSize), minSeparation);
                isLeaf = false;
                Children[octant].Parent = this;
                foreach (Octree<T> child in Children)
                {
                    if (child != null)
                        Children[octant].Neighbors.Add(child);
                }
            }
            catch (Exception)
            {

                throw;
            }           
            
        }        
       
       


    }

}
