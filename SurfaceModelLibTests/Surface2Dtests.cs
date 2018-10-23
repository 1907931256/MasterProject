using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometryLib;
using SurfaceModel;

namespace SurfaceModelLibTests
{
    [TestClass]
    public class Surface2Dtests
    {
        Surface2D<SurfacePoint> surf;
        double meshSize;
        BoundingBox boundingBox;
        void initSurf()
        {
            boundingBox = new BoundingBox(0, 0, 0, 2, 2, 1);
            meshSize = .005;
            surf = Surface2DBuilder<SurfacePoint>.Build(boundingBox, meshSize);
        }
        [TestMethod]
        public void Surface2D_ctorFromBox_surfOK()
        {
            initSurf();
            var testPt = new Vector3(0, 0, 0);
            var surfPt = surf.GetPointAt(testPt);
            double distance = surfPt.Position.DistanceTo(testPt);
            Assert.AreEqual(0, distance, .001);
            var surfPtList = surf.GetAllPoints();
            int ptCount = surfPtList.Count;
            Assert.AreEqual(160801, ptCount);
        }
        [TestMethod]
        public void Surface2D_getPointsInBox_pointsOK()
        {
            initSurf();
            var searchBox = new BoundingBox(0.5, 0.5, 0.5, 1.001, 1.001, 0);
            var surfPtList = surf.GetPointsInsideBox(searchBox);
            var maxX = double.MinValue;
            var minX = double.MaxValue;
            var maxY = double.MinValue;
            var minY = double.MaxValue;
            foreach(SurfacePoint pt in surfPtList)
            {
                maxX = Math.Max(maxX, pt.Position.X);
                minX = Math.Min(minX, pt.Position.X);
                maxY = Math.Max(maxY, pt.Position.Y);
                minY = Math.Min(minY, pt.Position.Y);
            }
            Assert.AreEqual(.5, minX, .0001,"minX");
            Assert.AreEqual(.5, minY, .0001,"minY");
            Assert.AreEqual(1, maxX, .0001,"maxX");
            Assert.AreEqual(1, maxY, .0001,"maxY");

            Assert.AreEqual(10201, surfPtList.Count);
        }
        [TestMethod]
        public void Surface2d_BuildsubSurface_subSurfOK()
        {
            initSurf();
            var searchBox = new BoundingBox(0, 0, 0, 1, 1, 0);
            var surfPtList = surf.GetPointsInsideBox(searchBox);
            var localSurface = Surface2DBuilder<SurfacePoint>.Build(surfPtList,  meshSize);
            Assert.AreEqual(0, localSurface.BoundingBox.Min.DistanceTo(searchBox.Min), .001);
            Assert.AreEqual(0, localSurface.BoundingBox.Max.DistanceTo(searchBox.Max), .001);
            var localPts = localSurface.GetAllPoints();
            Assert.AreEqual(surfPtList.Count, localPts.Count);
        }
    }
}
