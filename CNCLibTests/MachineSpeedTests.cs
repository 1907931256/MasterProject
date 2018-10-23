using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CNCLib;
namespace CNCLibTests
{
    [TestClass]
    public class MachSpeedTests
    {
        [TestMethod]
        public void MachineSpeed_DefConst_returnsOK()
        {
            MachineSpeed msp = new MachineSpeed();

            Assert.IsNotNull(msp);
            Assert.AreEqual(0, msp.RotaryF,"rot");             
            Assert.AreEqual(0, msp.LinearF,"Lin");
            Assert.AreEqual(0, msp.InverseFeed(0, 0),"inv feed");
        }

        [TestMethod]
        public void MachineSpeed_valuesSet_returnsCorrectInvFeed()
        {
            MachineSpeed msp = new MachineSpeed();
            msp.LinearF = 10;
            msp.RotaryF = 4;
            Assert.AreEqual(2, msp.InverseFeed(5, 0));
        }


    }
}
