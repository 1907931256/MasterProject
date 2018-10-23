using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AbMachModel;

namespace AbmachModelLibTests
{
    [TestClass]
    public class MrrEquTests
    {
        [TestMethod]
        public void MrrEqu_defConst_returnsDefVals()
        {
            MrrEquDictionary equations = new MrrEquDictionary();
            double[] defArray = equations.GetEquation(0);
            Assert.AreEqual(1, defArray[0]);
            Assert.AreEqual(.23, defArray[7]);
        }
        [TestMethod]
        public void mrrEqu_addEqu_returnsVals()
        {
            MrrEquDictionary equations = new MrrEquDictionary();
            double[] newEqu = new double[8] { 2, 3, 4, 5, 6, 7, 8, 9 };
            equations.AddEquation(3, newEqu);
            Assert.AreEqual(1, equations.Length);
            double[] getEq = equations.GetEquation(3);
            Assert.AreEqual(2, getEq[0]);
        }
        [TestMethod]
        public void MrrEqu_defConst_notNull()
        {
            MrrEquDictionary equations = new MrrEquDictionary();
            Assert.IsNotNull(equations);
        }
    }
}
