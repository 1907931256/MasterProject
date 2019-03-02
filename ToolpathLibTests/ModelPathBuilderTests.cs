using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolpathLib;
using CNCLib;
 
namespace ToolpathLibTests
{
    [TestClass]
    public class ModelPathBuilderTests
    {
        [TestMethod]
        public void modelpathbuilder_buildpath_isfiveAxisFalse()
        {
            string inputFile = "STRAIGHT-TEST-8-3-15.nc";
            double increment = .01;
            ToolPath5Axis toolpath = CNCFileParser.CreatePath(inputFile);
           
            ConstantDistancePathBuilder mpb = new ConstantDistancePathBuilder();
            ModelPath mp = mpb.Build(toolpath, increment);
            Assert.IsFalse(mp.IsFiveAxis);
        }
        [TestMethod]
        public void modelpathbuilder_buildpath_isfiveAxisTrue()
        {
            string inputFile = "2-CELL-TEST.2.nc";
            double increment = .01;
            ToolPath5Axis toolpath = CNCFileParser.CreatePath(inputFile);
            
            ConstantDistancePathBuilder mpb = new ConstantDistancePathBuilder();
            ModelPath mp = mpb.Build(toolpath, increment);
            Assert.IsTrue(mp.IsFiveAxis);
        }
       
        [TestMethod]
        public void ModelPathBuilder_NCfileToolpathIsGood_ReturnsModelPath()
        {
            string inputFile = "STRAIGHT-TEST-8-3-15.nc";
            double increment = .01;
            ToolPath5Axis toolpath = CNCFileParser.CreatePath(inputFile);
            Assert.AreEqual(1, toolpath[1].Position.X, "Xtp0");
            Assert.AreEqual(1, toolpath[1].Position.Y, "Xtp0");
            Assert.AreEqual(2, toolpath[1].Position.Z, "Xtp0");
            ConstantDistancePathBuilder mpb = new ConstantDistancePathBuilder();
            ModelPath mp = mpb.Build(toolpath, increment);
            int i = 0;
            
            while(!mp[i].JetOn)
            {
                i++;
            }
            Assert.AreEqual(1, mp[i].Position.X, "Xentity0");
            Assert.AreEqual(1, mp[i].Position.Y, "Yentity0");
            Assert.AreEqual(0, mp[i].Position.Z, "Zentity0");
            Assert.AreEqual(1.0089, Math.Round(mp[i+1].Position.X,4), "Xentity1");
            Assert.AreEqual(1.0045, Math.Round(mp[i+1].Position.Y,4), "Yentity1");
            Assert.AreEqual(0, Math.Round(mp[i+1].Position.Z,4), "Zentity1");
            Assert.AreEqual(10, mp[i+1].Feedrate.Value, "Fentity1");
            Assert.IsFalse(mp[0].JetOn, "Jentity0");
        }

        [TestMethod]
        public void ModelPathBuilder_NCIfileToolpathIsGood_ReturnsModelPath()
        {
            string inputFile = "SPLINE-CHANNEL-CONTOURPATH.NCI";
            double increment =.005;
            ToolPath5Axis toolpath = CNCFileParser.CreatePath(inputFile);

            ConstantDistancePathBuilder mpb = new ConstantDistancePathBuilder();
            ModelPath mp = mpb.Build(toolpath, increment);

            Assert.AreEqual(-3.876529, mp[0].Position.X, "Xentity0");
            Assert.AreEqual(0.05, mp[0].Position.Y, "Yentity0");
            Assert.AreEqual(3.307008, mp[0].Position.Z, "Zentity0");
            Assert.AreEqual(0, mp[0].Feedrate.Value, "Fentity0");
            Assert.IsFalse(mp[0].JetOn, "Jentity0");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModelPathBuilder_toolpathIsEmptyAndIncrementIsZero_ReturnsEmptyPath()
        {
            string inputPath = null;
            ToolPath5Axis toolpath = CNCFileParser.CreatePath(inputPath);

            double increment = 0;
            ConstantDistancePathBuilder mpb = new ConstantDistancePathBuilder();
            ModelPath mp = mpb.Build(toolpath, increment);            
            
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModelPathBuilder_PathIsGoodButIncrementIsZero_ReturnsEmptyPath()
        {
            string inputPath = "SPLINE-CHANNEL-CONTOURPATH.NCI";
            ToolPath5Axis toolpath = CNCFileParser.CreatePath(inputPath);

            double increment = 0;
            ConstantDistancePathBuilder mpb = new ConstantDistancePathBuilder();
            ModelPath mp = mpb.Build(toolpath, increment);
            
            
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModelPathBuilder_PathIsGoodButIncrementIsNeg_ReturnsEmptyPath()
        {
            string inputPath = "SPLINE-CHANNEL-CONTOURPATH.NCI";
            ToolPath5Axis toolpath = CNCFileParser.CreatePath(inputPath);

            double increment = -.001;
            ConstantDistancePathBuilder mpb = new ConstantDistancePathBuilder();
            ModelPath mp = mpb.Build(toolpath, increment);

           
        }
        [TestMethod]
        public void modelpathbuilder_buildpath_BoundingBoxOK()
        {
              string inputPath = "SPLINE-CHANNEL-CONTOURPATH.NCI";
            ToolPath5Axis toolpath = CNCFileParser.CreatePath(inputPath);

            double increment = .001;
            ConstantDistancePathBuilder mpb = new ConstantDistancePathBuilder();
            ModelPath mp = mpb.Build(toolpath, increment);

            Assert.AreEqual(-3.876529, mp.BoundingBox.Min.X, "min x");
            Assert.AreEqual(.05, mp.BoundingBox.Min.Y, "min y");
            Assert.AreEqual(0.465482, mp.BoundingBox.Min.Z, "min z;");
            Assert.AreEqual(4.621802, mp.BoundingBox.Max.X, "max x");
            Assert.AreEqual(.05, mp.BoundingBox.Max.Y, "max y");
            Assert.AreEqual(3.307008, mp.BoundingBox.Max.Z, "max z");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModelPathBuilder_toolpathIsEmpty_ReturnsEmptyPath()
        {
            string inputPath = null;
            ToolPath5Axis toolpath = CNCFileParser.CreatePath(inputPath);
           
            double increment = .005;
            ConstantDistancePathBuilder mpb = new ConstantDistancePathBuilder();
            ModelPath mp = mpb.Build(toolpath, increment);

        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModelPathBuilder_toolpathIsEmpty_modelPathIsNotNull()
        {
            string inputPath = "";
            ToolPath5Axis toolpath = CNCFileParser.CreatePath(inputPath);

           double increment = .005;
            ConstantDistancePathBuilder mpb = new ConstantDistancePathBuilder();
            ModelPath mp = mpb.Build(toolpath, increment);

            
            Assert.IsNotNull(mp);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModelPathBuilder_toolpathIsEmpty_modelpathIsEmpty()
        {
            string inputPath = "";
            ToolPath5Axis toolpath = CNCFileParser.CreatePath(inputPath);

            double increment = .005;
            ConstantDistancePathBuilder mpb = new ConstantDistancePathBuilder();
            ModelPath mp = mpb.Build(toolpath, increment);

            Assert.AreEqual(0, toolpath.Count);
           
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModelPathBuilder_toolpathIsNull_modelPathIsNotNull()
        {
            string inputPath = null;
            ToolPath5Axis toolpath = CNCFileParser.CreatePath(inputPath);

            double increment = .005;
            ConstantDistancePathBuilder mpb = new ConstantDistancePathBuilder();
            ModelPath mp = mpb.Build(toolpath, increment);


            Assert.IsNotNull(mp);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModelPathBuilder_toolpathIsNull_modelpathIsEmpty()
        {
            string inputPath = null;
            ToolPath5Axis toolpath = CNCFileParser.CreatePath(inputPath);

            double increment = .005;
            ConstantDistancePathBuilder mpb = new ConstantDistancePathBuilder();
            ModelPath mp = mpb.Build(toolpath, increment);

            Assert.AreEqual(0, toolpath.Count);

        }
    }
}
