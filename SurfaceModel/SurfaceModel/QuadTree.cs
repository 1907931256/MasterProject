using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
namespace SurfaceModel
{
    public class Tree<T> where T : SurfacePoint, new()
    {
        public  Tree<T>[] Children;
        public BoundingBox BoundingBox { get { return _boundingBox; } }
        public T Data { get; set; }
        
        public Vector3 Origin { get { return origin; } }
        public double MeshSize { get { return minSeparation; } }
        public bool IsLeaf { get { return isLeaf; } }
        public List<Tree<T>> Neighbors;
        public  Tree<T> Parent;

        protected double minSeparation;
        protected BoundingBox _boundingBox;
        protected bool isLeaf;

        protected double minSepSquared;
        protected Vector3 origin;
        public T GetPointAt(Vector3 searchPoint)
        {
            try
            {
                List<T> points = GetKNearestPoints(searchPoint, 1);
                return points[0];
            }
            catch (Exception)
            {

                throw;
            }

        }
        protected void GetNearestCell(Vector3 point, ref Tree<T> tree)
        {
            if (isLeaf)
            {
                tree = this;
                return;
            }
            else
            {
                int octant = GetSegmentContaining(point);
                Children[octant].GetNearestCell(point, ref tree);
            }

        }
        public List<T> GetNeighbors(Vector3 searchPoint)
        {
            try
            {
                T point = GetPointAt(searchPoint);
                List<T> results = new List<T>();
                var tree = new Tree<T>();

                GetNearestCell(point.Position, ref tree);
                if (tree.Parent != null)
                {
                    foreach (Octree<T> child in tree.Parent.Children)
                    {
                        if (child != null && child.Data != null)
                        {
                            double dist = child.Data.Position.Distance2To(point.Position);
                            if (dist > 0)
                                results.Add(child.Data);
                        }

                    }
                }

                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void GetAllPoints(ref List<T> results)
        {
            if (isLeaf)
            {
                if (Data != null)
                {
                    results.Add(Data);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Children[i] != null)
                        Children[i].GetAllPoints(ref results);
                }
            }
        }
        public void Insert(List<T> points)
        {
            try
            {
                foreach (T pt in points)
                {
                    if (pt != null)
                        Insert(pt);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void Insert(T point)
        {
            try
            {
                if (point != null)
                {
                    if (isLeaf)
                    {
                        // if  at a leafnode and there's no data then put it here
                        if (Data == null)
                        {
                            Data = point;

                            return;
                        }
                        else//there is data then divide into octants and re-insert old data and new data if point is >min distance
                        {
                            double separationSquared = point.Position.Distance2To(Data.Position);


                            if (separationSquared >= minSepSquared)
                            {
                                var oldPt = new T();
                                oldPt.Position = new Vector3(Data.Position);
                                //oldPt.Index = Data.Index;
                                oldPt.Normal = new Vector3(Data.Normal);

                                int oldQuadrant = GetSegmentContaining(oldPt.Position);
                                int newQuadrant = GetSegmentContaining(point.Position);
                                Data = null;

                                if (Children[oldQuadrant] == null)
                                {
                                    MakeChild(oldQuadrant);
                                }
                                if (Children[newQuadrant] == null)
                                {
                                    MakeChild(newQuadrant);
                                }
                                // insert old point and new point into children
                                Children[oldQuadrant].Insert(oldPt);
                                Children[newQuadrant].Insert(point);
                            }
                        }
                    }
                    else//not at a leaf node then continue to child
                    {

                        int quadrant = GetSegmentContaining(point.Position);

                        if (Children[quadrant] == null)
                        {
                            MakeChild(quadrant);
                        }
                        Children[quadrant].Insert(point);
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }


        }
        virtual protected void MakeChild(int quadrant)
        {
            Vector3 newOrigin = new Vector3(Origin);
            Vector3 newSize = .5 * _boundingBox.Size;
            newOrigin.X += ((quadrant & 2) == 0 ? 0 : newSize.X);
            newOrigin.Y += ((quadrant & 1) == 0 ? 0 : newSize.Y);
            Children[quadrant] = new QuadTree<T>(new BoundingBox(newOrigin, newOrigin + newSize), minSeparation);
            isLeaf = false;
            Children[quadrant].Parent = this;
        }
        public List<T> CutPointsInsideBox(BoundingBox box)
        {
            List<T> results = new List<T>();
            CutPointsInBox(box, ref results);
            return results;
        }
        protected void CutPointsInBox(BoundingBox box, ref List<T> results)
        {
            try
            {
                if (isLeaf && Data != null)
                {
                    if (box.Contains(Data.Position))
                    {
                        results.Add(Data);
                        Data = null;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (Children[i] != null && Children[i].BoundingBox.Overlaps(box))
                        {
                            Children[i].CutPointsInBox(box, ref results);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void GetPointsInBox(BoundingBox box, ref List<T> results)
        {
            try
            {
                if (isLeaf && Data != null)
                {
                    if (box.Contains(Data.Position))
                    {
                        results.Add(Data);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    for (int i = 0; i < Children.Length; i++)
                    {
                        if (Children[i] != null && Children[i].BoundingBox.Overlaps(box))
                        {
                            Children[i].GetPointsInBox(box, ref results);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<T> GetPointsInsideBox(BoundingBox box)
        {
            List<T> results = new List<T>();
            GetPointsInBox(box, ref results);
            return results;
        }
        public List<T> GetKNearestPoints(Vector3 searchPoint, int searchQty)
        {
            try
            {
                int resultCount = 0;
                var results = new List<T>();

                if (searchQty == 0)
                {
                    return results;
                }

                double initSearchWidth = minSeparation * Math.Min(searchQty, 3);

                double searchSize = 1;
                double maxSearchDim = 2 * Math.Max((searchPoint - _boundingBox.Min).Length, (_boundingBox.Max - searchPoint).Length);
                Vector3 searchHalfDimension = new Vector3(initSearchWidth, initSearchWidth, initSearchWidth);
                // search and expand search box until point count is >= k
                do
                {
                    var box = new BoundingBox(searchPoint - searchHalfDimension, searchPoint + searchHalfDimension);

                    results = new List<T>();
                    GetPointsInBox(box, ref results);

                    resultCount = results.Count;
                    if (resultCount == 0)
                    {
                        searchSize *= 2;
                    }
                    else
                    {
                        searchSize *= ((double)searchQty / (double)resultCount);
                    }

                    searchHalfDimension = new Vector3(searchSize * initSearchWidth, searchSize * initSearchWidth, searchSize * initSearchWidth);
                } while (resultCount < searchQty && searchHalfDimension.Length < maxSearchDim);

                double[] distances = new double[resultCount];
                T[] resultArray = results.ToArray();

                // find closest k points to search point and add to output 
                if (resultCount > searchQty)
                {
                    for (int i = 0; i < resultCount; i++)
                    {
                        distances[i] = searchPoint.Distance2To(resultArray[i].Position);
                    }
                    Array.Sort(distances, resultArray);

                    var resultList = new List<T>();
                    for (int i = 0; i < searchQty; i++)
                    {
                        resultList.Add(resultArray[i]);
                    }
                    return resultList;
                }
                else
                {
                    return results;
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        virtual protected int GetSegmentContaining(Vector3 point)
        {
            int quad = 0;
            if (point.X >= Origin.X) quad |= 2;
            if (point.Y >= Origin.Y) quad |= 1;

            return quad;
        }
        public void Remove(T point)
        {
            try
            {
                if (isLeaf)
                {
                    if (Data != null && point.Position.Distance2To(Data.Position) <= minSepSquared)
                    {
                        Data = null;
                    }
                    return;
                }
                else
                {
                    int octant = GetSegmentContaining(point.Position);
                    if (Children[octant] == null)
                    {
                        MakeChild(octant);
                    }
                    Children[octant].Remove(point);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
    public class QuadTree<T> :Tree<T> where T : SurfacePoint, new()
    {

       
       // double minSeparation;
       

        public QuadTree()
        {
            initQuadtree();
        }
        
        public QuadTree(BoundingBox boundingBox, double minimumSeparation)
        {
            initQuadtree();
        }
        void initQuadtree()
        {
            
            Children = new QuadTree<T>[4];
            Neighbors = new List< Tree<T>>();
        }
       
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
       
        //public T GetPointAt(Vector3 searchPoint)
        //{
        //    try
        //    {
        //        List<T> points = GetKNearestPoints(searchPoint, 1);
        //        return points[0];
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

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
    }

}
