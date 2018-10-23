using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AbMachModel;
using SurfaceModel;
using GeometryLib;
namespace AbmachModelLibTests
{
    [TestClass]
    public class AbMachSurfTests
    {

        Vector3 min;
        Vector3 max;
        double meshSize;
        Surface2D<AbmachPoint> surface;
        public void init()
        {
            min = new Vector3(-1, -1,0);
            max = new Vector3(1, 1,0);
            meshSize = .005;
           
             surface = Surface2DBuilder<AbmachPoint>.Build(new BoundingBox(min, max), meshSize);
        }
        [TestMethod]
        public void abmachSurf_getSize()
        {
            init();
            
            Assert.AreEqual(max.X, surface.Max.X);
            Assert.AreEqual(max.Y, surface.Max.Y);
            Assert.AreEqual(min.X, surface.Min.X);
            Assert.AreEqual(min.Y, surface.Min.Y);

        }
        [TestMethod]
        public void abmachSurf_getminIndex_indexOK()
        {
            init();
           
            int xI = surface.Xindex(min.X);
            int yI = surface.Yindex(min.Y);
            Assert.AreEqual(0, xI,"xmin");
            Assert.AreEqual(0, yI,"ymin");
        }
        [TestMethod]
        public void abmachSurf_getcenterIndex()
        {
             init();
           
            int xI = surface.Xindex(0.5);
            int yI = surface.Yindex(0.5);
            Assert.AreEqual(299, xI);
            Assert.AreEqual(299, yI);

        }
        [TestMethod]
        public void abmachSurf_getMaxIndex_indexOK()
        {
            init();
          
            int xI = surface.Xindex(max.X);
            int yI = surface.Yindex(max.Y);
            int sX = surface.XSize;
            int sY = surface.YSize;
           
            Assert.AreEqual(sX-1, xI,  "x max");
            Assert.AreEqual(sY-1, yI,  "y max");

        }
     
      

      
       
      
        
    }
}
