using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SurfaceModel;
using GeometryLib;
using System.Collections.Generic;
using DwgConverterLib;
using AbMachModel;
namespace SurfaceModelLibTests
{
    
    [TestClass]
    public class OctreeTests
    {
        [TestMethod]
        public void AbmachOctree_getPointsInBox_pointsOK()
        {
            List<Vector3> vectorList = new List<Vector3>();
            for (int i = 0; i <= 100; i++)
            {
                for (int j = 0; j <= 100; j++)
                {
                    vectorList.Add(new Vector3(i * .1, j * .1, 1.0));
                }
            }
            Octree<AbmachPoint> Octree = OctreeBuilder<AbmachPoint>.Build(vectorList, .1);
            Vector3 searchPoint = new Vector3(5, 6, 3);
            Vector3 direction = new Vector3(0, 0, -1);
            Ray ray = new Ray(searchPoint, direction);
            double searchRadius = Octree.MeshSize * 3;
            var searchBox = BoundingBoxBuilder.GetSearchCylinder(Octree.BoundingBox, searchPoint, direction ,  searchRadius);
            List<AbmachPoint> points = Octree.GetPointsInsideBox(searchBox);
            Assert.AreNotEqual(0, points.Count);
        }
        [TestMethod]
        public void AbmachOctree_getIntersection_pointOK()
        {
            List<Vector3> vectorList = new List<Vector3>();
            for (int i = 0; i <= 100; i++)
            {
                for (int j = 0; j <= 100; j++)
                {
                    vectorList.Add(new Vector3(i * .1, j * .1, 1.0));
                }
            }
            Octree<AbmachPoint> Octree = OctreeBuilder<AbmachPoint>.Build(vectorList, .1);
            Vector3 searchPoint = new Vector3(5, 6, 3);
            Vector3 direction = new Vector3(0, 0, -1);
            Ray ray = new Ray(searchPoint, direction);
            IntersectionRecord ir = Octree.GetIntersection(ray);
            Assert.IsTrue(ir.Intersects);
            Assert.AreEqual(5.0, ir.X, .01);
            Assert.AreEqual(6.0, ir.Y, .01);
            Assert.AreEqual(1.0, ir.Z, .01);
        }
        [TestMethod]
        public void Octree_getpointsInboxfromSearchBox_pointsOK()
        {
            List<Vector3> vectorList = new List<Vector3>();
            for (int i = 0; i <= 100; i++)
            {
                for (int j = 0; j <= 100; j++)
                {
                    vectorList.Add(new Vector3(i * .1, j * .1, 1.0));
                }
            }
            Octree<SurfacePoint> Octree = OctreeBuilder<SurfacePoint>.Build(vectorList, .1);
            Vector3 searchPoint = new Vector3(5, 6, 3);
            Vector3 direction = new Vector3(0, 0, 1);
            Ray ray = new Ray(searchPoint, direction);
            double searchRadius = Octree.MeshSize * 3;
            var searchBox = BoundingBoxBuilder.GetSearchCylinder(Octree.BoundingBox,searchPoint, direction, searchRadius);
            List<SurfacePoint> points = Octree.GetPointsInsideBox(searchBox);
            Assert.AreNotEqual(0, points.Count);
        }
        [TestMethod]
        public void Octree_getIntersection_pointOK()
        {
            List<Vector3> vectorList = new List<Vector3>();
            for (int i = 0; i <= 100; i++)
            {
                for (int j = 0; j <= 100; j++)
                {
                    vectorList.Add(new Vector3(i * .1, j * .1, 1.0));
                }
            }
            Octree<SurfacePoint> Octree = OctreeBuilder<SurfacePoint>.Build(vectorList, .1);
            Vector3 searchPoint = new Vector3(5, 6, 3);
            Vector3 direction = new Vector3(0, 0, -1);
            Ray ray = new Ray(searchPoint, direction);
            IntersectionRecord ir = Octree.GetIntersection(ray);
            Assert.IsTrue(ir.Intersects);
            Assert.AreEqual(5.0, ir.X, .01);
            Assert.AreEqual(6.0, ir.Y, .01);
            Assert.AreEqual(1.0, ir.Z, .01);
            
        }
        [TestMethod]
        public void Octree_getNormal_normalOK()
        {
            List<Vector3> vectorList = new List<Vector3>();
            for (int i = 0; i <= 100; i++)
            {
                for (int j = 0; j <= 100; j++)
                {
                    vectorList.Add(new Vector3(i * .1, j * .1, i*.1));
                }
            }
            Octree<SurfacePoint> Octree = OctreeBuilder<SurfacePoint>.Build(vectorList, .1);
            var pt = new SurfacePoint(new Vector3(5, 5, 1));
            Vector3 normal = Octree.GetNormalAt(pt.Position);
            normal.Normalize();
            Assert.AreEqual(-.707, normal.X, .01);
            Assert.AreEqual(0.0, normal.Y, .01);
            Assert.AreEqual(.707, normal.Z, .01);
        }
        [TestMethod]
        public void Octree_getNearestPoint_pointOK()
        {
            List<Vector3> vectorList = new List<Vector3>();
            for (int i = 0; i <= 100; i++)
            {
                for (int j = 0; j <= 100; j++)
                {
                    vectorList.Add(new Vector3(i * .1, j * .1,  1.0));                   
                }
            }
            Octree<SurfacePoint> Octree = OctreeBuilder<SurfacePoint>.Build(vectorList, .1);
            Vector3 searchPoint = new Vector3(5, 6, 6);
            SurfacePoint resultPt =  Octree.GetPointAt(searchPoint);
            Assert.AreEqual(5.0, resultPt.Position.X, .01);
            Assert.AreEqual(6.0, resultPt.Position.Y, .01);
            Assert.AreEqual(1.0, resultPt.Position.Z, .01);
        }
        [TestMethod]
        public void Octree_buildFromLineList_pointsOK()
        {
            List<Line> lines = new List<Line>();
            for (int i = 0; i <= 10; i++)
            {

                Line l = new Line(i,Math.Sin(Math.PI*i/5), 0, i+1, Math.Sin(Math.PI * (i+1) / 5), 0);
              
                lines.Add(l);

            }
            Line acrossV = new Line(0,0,0,0, 0, 10);
            Octree<SurfacePoint> octree = OctreeBuilder<SurfacePoint>.Build(lines, acrossV, .1);
            List<Vector3> positions = octree.GetAllPositions();
            List<DwgEntity> dwgEntities = new List<DwgEntity>();
            foreach(Vector3 p in positions)
            {
                dwgEntities.Add((DwgEntity)p);
            }
            DwgConverterLib.DxfFileBuilder.Save(dwgEntities, "dxfFileName.dxf");
            Assert.AreEqual(12625, positions.Count);

        }
        [TestMethod]
        public void OctreeBuilder_buildFromSTLFile_octreeOK()
        {
            string fileName = "Tbinary.STL";
            StlFile stlFile = StlFileParser.Open(fileName);
            double minPointSpacing = .005;
            Octree<AbmachPoint> octree = OctreeBuilder<AbmachPoint>.Build(stlFile , minPointSpacing);
            List<AbmachPoint> points = octree.GetAllPoints();
           
            Assert.AreNotEqual(0, points.Count);
        }
        [TestMethod]
        public void OctreeBuild_buildFromPtList_originOK()
        {
            List<Vector3> vectorList = new List<Vector3>();
            for (int i = 0; i <= 10;i++)
            {
                for (int j=0;j<=10;j++)
                {
                    for(int k=0;k<=10;k++)
                    {
                        vectorList.Add(new Vector3(i * 1.0, j * 1.0, k * 1.0));
                    }
                }
            }
            Octree<SurfacePoint> Octree = OctreeBuilder<SurfacePoint>.Build(vectorList, .1);
          
           
            Assert.AreEqual(0, Octree.Origin.X, .001);
            Assert.AreEqual(0, Octree.Origin.Y, .001);
            Assert.AreEqual(0, Octree.Origin.Z, .001);

        }
        
        [TestMethod]
        public void OctreeBuild_buildFromPtList_sizeOK()
        {
            List<Vector3> vectorList = new List<Vector3>();
            for (int i = 0; i <= 10; i++)
            {
                for (int j = 0; j <= 10; j++)
                {
                    for (int k = 0; k <= 10; k++)
                    {
                        vectorList.Add(new Vector3(i * 1.0, j * 1.0, k * 1.0));
                    }
                }
            }
            Octree<SurfacePoint> Octree = OctreeBuilder<SurfacePoint>.Build(vectorList, .1);
            

           
            Assert.AreEqual(10, Octree.BoundingBox.Size.X, .001);
            Assert.AreEqual(10, Octree.BoundingBox.Size.Y, .001);
            Assert.AreEqual(10, Octree.BoundingBox.Size.Z, .001);
           
        }       
        [TestMethod]
        public void OctreeBuild_GetAllPoints_CountOK()
        {
            List<Vector3> vectorList = new List<Vector3>();
            for (int i = 0; i <= 10; i++)
            {
                for (int j = 0; j <= 10; j++)
                {
                    for (int k = 0; k <= 10; k++)
                    {
                        vectorList.Add(new Vector3(i * 1.0, j * 1.0, k * 1.0));
                    }
                }
            }
            Octree<SurfacePoint> Octree = OctreeBuilder<SurfacePoint>.Build(vectorList, .1);
            var pointList = Octree.GetAllPoints();           
           
             Assert.AreEqual(vectorList.Count, pointList.Count);
        }    

        [TestMethod]
        public void Octree_planarPoints_getNearestPts_pointsOK()
        {
            List<Vector3> vectorList = new List<Vector3>();
            for (int j = 0; j <= 10; j++)
            {
                for (int i = 0; i <= 10; i++)
                {
                    vectorList.Add(new Vector3(i * 1.0, j * 1.0, 2.0));
                }
            }

            Octree< SurfacePoint > Octree = OctreeBuilder< SurfacePoint >.Build(vectorList, .1);

            Vector3 searchPt = new Vector3(3, 3, 2);
            int searchQty = 5;
            List<SurfacePoint> pointList = Octree.GetKNearestPoints(searchPt, searchQty);

            
            double dMin = double.MaxValue;
            double dMax = double.MinValue;
            List<double> distances = new List<double>();
            string fileName = "octreeNearestPointTestList.txt";
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName))
                foreach (SurfacePoint pt in pointList)
                {
                    double d = searchPt.DistanceTo(pt.Position);
                    distances.Add(d);
                    if (d < dMin)
                        dMin = d;
                    if (d > dMax)
                        dMax = d;
                    sw.WriteLine(pt.Position.X.ToString() + "," + pt.Position.Y.ToString() + "," + pt.Position.Z.ToString());
                }
            

            Assert.AreEqual(9, pointList.Count);
            

        }
        [TestMethod]
        public void Octree_getNeighbors_pointsOK()
        {
            
            List<Vector3> vectorList = new List<Vector3>();
            for (int i = 0; i <= 10; i++)
            {
                for (int j = 0; j <= 10; j++)
                {
                    
                        vectorList.Add(new Vector3(i * 1.0, j * 1.0,2.0));
                    
                }
            }
            Octree<SurfacePoint> Octree = OctreeBuilder<SurfacePoint>.Build(vectorList, .1);
            Vector3 searchPt = new Vector3(3.1, 3.1, 2);
            List<SurfacePoint> points = Octree.GetNeighbors(searchPt);
            Assert.AreNotEqual(0, points.Count);
        }
        [TestMethod]
        public void Octree_cutPointsInBox_pointsOK()
        {
            List<Vector3> vectorList = new List<Vector3>();
            for (int j = 0; j <= 10; j++)
            {
                for (int i = 0; i <= 10; i++)
                {
                    vectorList.Add(new Vector3(i * 1.0, j * 1.0, 2.0));
                }
            }

            Octree<SurfacePoint> Octree = OctreeBuilder<SurfacePoint>.Build(vectorList, .1);
            List<SurfacePoint> initialList = Octree.GetAllPoints();
            int initalCount = initialList.Count;
            Assert.AreEqual(121, initalCount, "initial count");
            Vector3 searchPt = new Vector3(3, 3, 2);
            BoundingBox box = new BoundingBox(1.5, 1.5, 1.5, 4.5, 4.5, 2.5);
            List<SurfacePoint> pointList = Octree.CutPointsInsideBox(box);
            int ptCount = pointList.Count;
            Assert.AreEqual(9, ptCount,"cut count");
            List<SurfacePoint> remainderList = Octree.GetAllPoints();
            int remCount = remainderList.Count;
            Assert.AreEqual(121 - 9, remCount,"remainder count");
        }
        [TestMethod]
        public void Octree_getPointsInBox_pointsOK()
        {
            List<Vector3> vectorList = new List<Vector3>();
            for (int j = 0; j <= 10; j++)
            {
                for (int i = 0; i <= 10; i++)
                {
                    vectorList.Add(new Vector3(i * 1.0, j * 1.0, 2.0));
                }
            }

            Octree<SurfacePoint> Octree = OctreeBuilder<SurfacePoint>.Build(vectorList, .1);

            Vector3 searchPt = new Vector3(3, 3, 2);
            BoundingBox box = new BoundingBox(1.5, 1.5, 1.5,4.5, 4.5, 2.5);
           

            List<SurfacePoint> pointList = Octree.GetPointsInsideBox(box);
            int ptCount = pointList.Count;
            Assert.AreEqual(9, ptCount);
            string fileName = "pointsInsideBoxList.txt";
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName))
            foreach(SurfacePoint pt in pointList)
            {
                sw.WriteLine(pt.Position.X.ToString() + "," + pt.Position.Y.ToString() + "," + pt.Position.Z.ToString());
            }
        }
        [TestMethod]
        public void Octree_removePoints_PointCountOK()
        {
            List<Vector3> vectorList = new List<Vector3>();
            for (int j = 0; j <= 10; j++)
            {
                for (int i = 0; i <= 10; i++)
                {
                    vectorList.Add(new Vector3(i * 1.0, j * 1.0, 2.0));
                }
            }
            Vector3 searchPt = new Vector3(3, 3, 2);
            BoundingBox box = new BoundingBox(1.5, 1.5, 1.5,4.5, 4.5, 2.5);
            
            Octree<SurfacePoint> Octree = OctreeBuilder<SurfacePoint>.Build(vectorList, .1);
            List<SurfacePoint> allPointList = Octree.GetAllPoints();
            int totalCount = allPointList.Count;
            Assert.AreEqual(11 * 11, totalCount, "total Count");
            List<SurfacePoint> pointList = Octree.GetPointsInsideBox(box);
            int ptCount = pointList.Count;
            Assert.AreEqual(9, ptCount,"pointsInBox Count");
            Octree.Remove(pointList);
            List<SurfacePoint> someRemovedPointsList = Octree.GetAllPoints();
            int newTotalCount = someRemovedPointsList.Count;
            Assert.AreEqual(121 - 9, newTotalCount, "new total");
        }
    }
}
