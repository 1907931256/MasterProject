using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AbMachModel;
using ToolpathLib;
using SurfaceModel;
using GeometryLib;
namespace AbmachModelLibTests
{
    [TestClass]
    public class AbmachSim2DTests
    {
        AbMachParameters parms;
        Surface2D<AbmachPoint> surface;
        ModelPath path;
        
        Vector3 min;
        Vector3 max;
        double meshSize;
        double diameter;

        void initParms()
        {
            meshSize = .005;
            diameter = .04;
            double nominalSurfaceSpeed = 40;
            double depthPerPass = .001;
            int runs = 1;
            int iterations = 1;
            int equationIndex = 2;
            double searchRadius = diameter * .1;
            var jet = new AbMachJet(diameter, equationIndex);
            var runInfo = new RunInfo(runs, iterations, ModelRunType.NewFeedrates);
            var removalRate = new RemovalRate(nominalSurfaceSpeed, depthPerPass);
            var depthInfo = new DepthInfo(new Vector3(1, 1, 0), DepthSearchType.FindAveDepth,searchRadius);
            var op = AbMachOperation.ROCKETCHANNEL;
            var mat = new AWJModel.Material(AWJModel.MaterialType.Metal, "Aluminum", .25, 123, 456, 789, 143, 345, 543,1);

            parms = AbMachParamBuilder.Build(op, runInfo, removalRate,
                mat, jet, depthInfo, meshSize);
        }

        
        void initSurface()
        {
            max = new Vector3(path.BoundingBox.Max.X+diameter, path.BoundingBox.Max.Y+diameter,path.BoundingBox.Max.Z);
            min = new Vector3(path.BoundingBox.Min.X-diameter,path.BoundingBox.Min.Y-diameter,path.BoundingBox.Min.Z);


            surface = Surface2DBuilder<AbmachPoint>.Build(new BoundingBox(min, max) , meshSize);
            
             
            
        }

        void initPath()
        {
            string inputFile = "STRAIGHT-TEST-8-3-15.nc";
           
            ToolPath toolpath = CNCFileParser.ToPath(inputFile);

            var mpb = new ConstantDistancePathBuilder();
            path = mpb.Build(toolpath, meshSize);
        }
        void initModel()
        {
            initParms();
            initPath();
            initSurface();
        }
        [TestMethod]
        public void abmachSim_buildpath_pathOK()
        {
            initParms();
            initPath();           
            
            Assert.AreNotEqual(0, path.Count,"modelpath");
           
        }
        [TestMethod]
        public void abmachsim2d_buildSurfaceFromPath_surfOK()
        {
            initParms();
            initPath();
            initSurface();
            AbmachSimModel2D model = new AbmachSimModel2D(surface, surface, path, parms);
            ISurface<AbmachPoint> modelSurface = model.GetSurface();
            bool ptOK = false;
            

        }
    }
}
