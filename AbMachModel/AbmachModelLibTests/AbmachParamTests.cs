using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AbMachModel;
namespace AbmachModelLibTests
{
    [TestClass]
    public class AbmachParamtests
    {
        [TestMethod]
        public void abmTests_defConst_returnsNOtNull()
        {
            double meshSize = .005;
            AbMachParameters parms = AbMachParamBuilder.Build(
                AbMachOperation.ROCKETCHANNEL,
                new RunInfo(),
                new RemovalRate(),
                new AWJModel.Material(),
                new AbMachJet(),
                new DepthInfo(),
                meshSize);
            Assert.IsNotNull(parms);

        }
       
        
        [TestMethod]
        public void abmTests_Const_valuesAreCorrect()
        {
            double meshSize = .005;
            double diameter = .04;
            double nominalSurfaceSpeed = 40;
            double depthPerPass = .001;
            int runs = 3;
            int iterations = 1;
            int equationIndex = 2;

            var jet = new AbMachJet(diameter,equationIndex);
            var runInfo = new RunInfo(runs,iterations,ModelRunType.NewFeedrates);
            var removalRate = new RemovalRate(nominalSurfaceSpeed,depthPerPass);
            var depthInfo = new DepthInfo(new GeometryLib.Vector3(1,1,0),DepthSearchType.FindAveDepth, diameter / 10);
            var op = AbMachOperation.ROCKETCHANNEL;
            var mat = new AWJModel.Material(AWJModel.MaterialType.Metal, "Aluminum", .25, 1, 1, 1, 1, 1, 1,1);

            AbMachParameters parms = AbMachParamBuilder.Build(op, runInfo, removalRate, 
                mat, jet, depthInfo, meshSize);
           
            Assert.AreEqual(nominalSurfaceSpeed, parms.RemovalRate.SurfaceSpeed, "surfspeed");

        }
    }
}
