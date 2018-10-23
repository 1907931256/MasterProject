using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AbMachModel;
namespace AbmachModelLibTests
{
    [TestClass]
    public class paramFileTests
    {
        [TestMethod]
        public void paramFile_save_fileOK()
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
            var depthInfo = new DepthInfo(new GeometryLib.Vector3(1,1,0),DepthSearchType.FindAveDepth,diameter/10);
            var op = AbMachOperation.ROCKETCHANNEL;
            var mat = new AWJModel.Material(AWJModel.MaterialType.Metal, "Aluminum", .25, 123, 456, 789, 143, 345, 543,1);

            AbMachParameters parms = AbMachParamBuilder.Build(op, runInfo, removalRate, mat, jet, depthInfo, meshSize);
            string fileName= "paramSaveTest.prx";
            AbMachParametersFile.Save(parms, fileName);
        }
        [TestMethod]
        public void paramFile_openSavedFile_parmsOK()
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
            var mat = new AWJModel.Material(AWJModel.MaterialType.Metal,"Aluminum",.25,123,456,789,143,345,543,1);

            AbMachParameters parms = AbMachParamBuilder.Build(op, runInfo, removalRate, mat, jet, depthInfo, meshSize);
            string fileName= "paramSaveTest.prx";
            AbMachParametersFile.Save(parms, fileName);
            AbMachParameters parmsOpen = AbMachParametersFile.Open(fileName);
            
            Assert.AreEqual(parms.Material.CriticalRemovalAngle, parmsOpen.Material.CriticalRemovalAngle);
           
        }
    }
}
