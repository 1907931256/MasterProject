using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AbMachModel;

namespace AbmachModelLibTests
{
    [TestClass]
    public class AbMachJetTests
    {
        [TestMethod]
        public void AbmachJet_defConst_returnsEmptyJet()
        {
            AbMachJet jet = new AbMachJet();
            Assert.IsNotNull(jet);
            
        }
        [TestMethod]
        public void AbmachJet_ctor_returnsJet()
        {
            double jetDiam = .1;
            double jetRadius = jetDiam / 2;
            AbMachJet jet = new AbMachJet(jetDiam, 2);
            double mrr1 = jet.RemovalRate(.01 * jetRadius);
            Assert.AreEqual(0.628959112, mrr1, .005);
            double mrr0 = jet.RemovalRate(.051*jetRadius);
            Assert.AreEqual(0.646885911, mrr0,.005);
            double mrr3 = jet.RemovalRate(.418*jetRadius);
            Assert.AreEqual(0.847790721, mrr3, .005);
            double mrr4 = jet.RemovalRate(.649 * jetRadius);
            Assert.AreEqual(0.525730358, mrr4, .005);
            double mrr5 = jet.RemovalRate(1.001 * jetRadius);
            Assert.AreEqual(0.0, mrr5,.001); 
        }
    }
}
