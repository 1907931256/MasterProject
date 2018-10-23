using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolpathLib;

namespace ToolpathLibTests
{
    [TestClass]
    public class ModelPathConstTests
    {
        [TestMethod]
        public void ModelPath_defaultConstructor_pathisEmpty()
        {
            
            ModelPath tp = new ModelPath();
            Assert.AreEqual(0, tp.Count);
        }
        [TestMethod]
        public void ModelPath_defaultConstructor_pathisNotNull()
        {
            
            ModelPath tp = new ModelPath();
            Assert.IsNotNull(tp);
        }
    
    }
}
