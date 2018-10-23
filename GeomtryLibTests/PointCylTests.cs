using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometryLib;
namespace GeometryLibTests
{
    [TestClass]
    public class PointCylTests
    {
        [TestMethod]
        public void PointCyl_constFromVect3_returnsVal()
        {
            Vector3 v = new Vector3(2, 2, 1);
            PointCyl pt = new PointCyl(v);
            Assert.AreEqual(1d, pt.Z,.001);
            Assert.AreEqual(Math.Sqrt(8), pt.R,.001);
            Assert.AreEqual(Math.PI / 4, pt.ThetaRad,.001);
            Assert.AreEqual(45.0, pt.ThetaDeg(),.001);            
        }
        [TestMethod]
        public void PointCyl_translate_returnPt()
        {
            PointCyl pt = new PointCyl(1, Math.PI, 1);
            PointCyl ptOut = pt.Translate(new Vector3(1, 1, 1));
            Assert.AreEqual(2d,ptOut.Z,.001);
            Assert.AreEqual(pt.R, ptOut.R,.001);
            Assert.AreEqual(Math.PI/2, ptOut.ThetaRad, .001);

        }
    }
}
