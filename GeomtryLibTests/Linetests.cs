using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometryLib;

namespace GeometryLibTests
{
    [TestClass]
    public class LineTests
    {
        [TestMethod]
        public void Line_defConst_returnsZeroLengthLine()
        {
            Line line = new Line();
            Assert.AreEqual(0, line.Length);

        }
        [TestMethod]
        public void Line_defConst_notNull()
        {
            Line line = new Line();
            Assert.IsNotNull(line);

        }
        [TestMethod]
        public void Line_const_lengthCorrect()
        {
            Line line = new Line(1, 1, 1, 2, 2, 2);
            Assert.AreEqual(Math.Sqrt(3), line.Length, "len");
        }
        [TestMethod]
        public void Line_translate_returnsVal()
        {
            Line line = new Line(1, 1, 1, 2, 2, 2);
            Line ltrans = line.Translate(new Vector3(1, 1, 1));
            
            Assert.AreEqual(2, ltrans.Point1.X,"x1");
            Assert.AreEqual(2, ltrans.Point1.Y, "y1");
            Assert.AreEqual(2, ltrans.Point1.Z, "z1");
        }
        [TestMethod]
        public void Line_rotateZ_returnsVal()
        {
            Line line = new Line(1, 1, 0, 2, 2, 0);
            Vector3 pc = new Vector3(0, 0, 0);

            Line lrot = line.RotateZ(pc, Math.PI );

            Assert.AreEqual(line.Length, lrot.Length, "len");
            Assert.AreEqual(-1d, Math.Round(lrot.Point1.X,8), "x1");
            Assert.AreEqual(-1d, Math.Round(lrot.Point1.Y,8), "y1");
            Assert.AreEqual(0d, Math.Round(lrot.Point1.Z,8), "z1");
        }
        [TestMethod]
        public void Line_rotateX_returnsVal()
        {
            Line line = new Line(1, 1, 0, 2, 2, 0);
            Vector3 pc = new Vector3(0, 0, 0);

            Line lrot = line.RotateX(pc, Math.PI);

            Assert.AreEqual(line.Length,lrot.Length, "len");
            Assert.AreEqual(1d, Math.Round(lrot.Point1.X, 8), "x1");
            Assert.AreEqual(-1d, Math.Round(lrot.Point1.Y, 8), "y1");
            Assert.AreEqual(0d, Math.Round(lrot.Point1.Z, 8), "z1");
            Assert.AreEqual(2d, Math.Round(lrot.Point2.X, 8), "x2");
            Assert.AreEqual(-2d, Math.Round(lrot.Point2.Y, 8), "y2");
            Assert.AreEqual(0d, Math.Round(lrot.Point2.Z, 8), "z2");
        }
        [TestMethod]
        public void Line_rotateY_returnsVal()
        {
            Line line = new Line(1, 1, 0, 2, 2, 0);
            Vector3 pc = new Vector3(0, 0, 0);

            Line lrot = line.RotateY(pc, Math.PI);

            Assert.AreEqual(line.Length, lrot.Length, "len");
            Assert.AreEqual(-1d, Math.Round(lrot.Point1.X, 8), "x1");
            Assert.AreEqual(1d, Math.Round(lrot.Point1.Y, 8), "y1");
            Assert.AreEqual(0d, Math.Round(lrot.Point1.Z, 8), "z1");
            Assert.AreEqual(-2d, Math.Round(lrot.Point2.X, 8), "x2");
            Assert.AreEqual(2d, Math.Round(lrot.Point2.Y, 8), "y2");
            Assert.AreEqual(0d, Math.Round(lrot.Point2.Z, 8), "z2");
        }
    }
}
