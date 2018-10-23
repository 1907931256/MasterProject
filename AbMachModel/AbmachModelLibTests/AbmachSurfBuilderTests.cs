using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AbMachModel;
using GeometryLib;
using SurfaceModel;
using ToolpathLib;
namespace AbmachModelLibTests
{
    [TestClass]
    public class AbmachSurfBuilderTests
    {
        GeometryLib.Vector3 min;
        GeometryLib.Vector3 max;
        double meshSize;
        
        void init()
        {
            min = new GeometryLib.Vector3(-1, -1,0);
            max = new GeometryLib.Vector3(1, 1,0);
            meshSize = .005;
           
        }
        [TestMethod]
        public void abmSurfBuilder_buildFromPath_returnSurfOK()
        {
            string inputFile = "STRAIGHT-TEST-8-3-15.nc";
            double increment = .01;
            ToolPath toolpath = CNCFileParser.ToPath(inputFile);

            ConstantDistancePathBuilder mpb = new ConstantDistancePathBuilder();
            ModelPath mp = mpb.Build(toolpath, increment);

        }
        [TestMethod]
        public void abmsurfbuilder_buildfromBoundingBox_returnSurf()
        {
            double xMin = -1.0;
            double yMin = -2.0;
            
            double xMax = 1.0;
            double yMax = 2.0;

            meshSize = .005;
            min = new Vector3(xMin, yMin,0);
            max = new Vector3(xMax, yMax,0);

            Surface2D < SurfacePoint > surface =  Surface2DBuilder<SurfacePoint>.Build(new BoundingBox(min,max),meshSize);

            
            Assert.AreEqual(xMin, surface.Min.X);
            Assert.AreEqual(yMin, surface.Min.Y);

        }
       
        [TestMethod]
        public void abmSurfTest_defBuild_returnsSurface()
        {
            init();
       
         // Assert.AreEqual(0, st);
        }

        [TestMethod]
        public void abmSurfTest_BuildConstStartTarget_returnSurface()
        {
            init();
            double start = 1;
           
        }

    }
}
