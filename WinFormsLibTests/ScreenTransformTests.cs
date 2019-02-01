using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinFormsLib;
using System.Drawing;
using GeometryLib;
namespace WinFormsLibTests
{
    [TestClass]
    public class ScreenTransformTests
    {
        [TestMethod]
        public void ScreenTransform_GetScreenCoords()
        {
            var bbox = new RectangleF(0,0,1,1);
            var rectf = new RectangleF(0, 0, 300, 200);
            var st = new ScreenTransform(bbox, rectf, true);
            PointF ptscreen = st.GetScreenCoords(1, -2);
            Assert.AreEqual(300,ptscreen.X,7e-6);
            Assert.AreEqual(200,ptscreen.Y,7e-6);

        }
       // [TestMethod]
        //public  void ScreenTransform_GetPartCoords()
        //{
        //    var bbox = new BoundingBox(-2, -2, 0, 1, 1, 0);
        //    var rectf = new RectangleF(0, 0, 300, 200);
        //    var st = new ScreenTransform(bbox, rectf, true);
        //    Point screenpt = new Point(300, 200);
        //    PointF ptPart = st.GetPartCoords(screenpt);
        //    Assert.AreEqual(-2, ptPart.Y);
        //    Assert.AreEqual(1, ptPart.X);
        //}
    }
}
