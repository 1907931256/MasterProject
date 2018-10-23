using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometryLib;
namespace GeometryLibTests
{
    [TestClass]
    public class GeomUtilTests
    {
        [TestMethod]
        public void GeometryUtil_rayPointDistance_distanceOK()
        {
            Vector3 pt = new Vector3(6,8,2);
            Vector3 rayOrigin = new Vector3(3, 4, 1);
            Vector3 rayDirection = new Vector3(0, 0, 1);
            Ray ray = new Ray(rayOrigin, rayDirection);
            double dist = Geometry.RayPointDistance(ray, pt);
            Assert.AreEqual(5, dist, .001);
        }
        [TestMethod]
        public void GeometryUtil_breakMany_pointsOK()
        {
            Vector3 pt1 = new Vector3(6, 8, 1);
            Vector3 pt2 = new Vector3(3, 4, 1);
            var line = new Line(pt1, pt2);
            double len = pt1.DistanceTo(pt2);
            double spacing = .1;
            int count = (int)(Math.Round(len/spacing))+1;
            List<Vector3> points = Geometry.BreakMany(line, spacing);
            Assert.AreEqual(count, points.Count);
            double seglen = points[0].DistanceTo(points[1]);
            Assert.AreEqual(spacing, seglen, .01);
        }
    }
}
