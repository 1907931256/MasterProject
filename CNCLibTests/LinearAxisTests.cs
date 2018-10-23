using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CNCLib;

namespace CNCLibTests
{
    [TestClass]
    public class LinearAxisTests
    {
        [TestMethod]
        public void LinearAxis_DefConst_builtOK()
        {
            LinearAxis la = new LinearAxis();
            uint t = 0;
            Assert.AreEqual(t, la.EncoderCtsPerRev);
            Assert.AreEqual(AxisTypeEnum.Linear, la.Type);
            Assert.AreEqual("Axis0", la.Name);
        }
        [TestMethod]
        public void LinearAxis_defLinear_builtOK()
        {
            LinearAxis la = new LinearAxis(1, "test1", "plcX");
            uint t = 0;
            Assert.AreEqual(t, la.EncoderCtsPerRev);
            Assert.AreEqual(AxisTypeEnum.Linear, la.Type);

        }
        [TestMethod]
        public void LinearAxis_Linear_builtOK()
        { 
            uint offset = 155;
            uint countsPerRev = 3200;
            LinearAxis la = new LinearAxis(1, "test1", "plcX",countsPerRev,offset);

            double positionInch = la.PositionInch(3200);
            Assert.AreEqual(offset, la.EncoderOffset);
            Assert.AreEqual(countsPerRev, la.EncoderCtsPerRev);
            Assert.AreEqual(AxisTypeEnum.Linear, la.Type);
        }
        [TestMethod]
        public void LinearAxis_Linear_ReturnsPosition()
        {
            uint offset = 155;
            uint countsPerRev = 3200;
            LinearAxis la = new LinearAxis(1, "test1", "plcX", countsPerRev, offset);

            double Poscounts = 3200;
            double positionInch = la.PositionInch(Poscounts);
            double positionChk = (Poscounts - offset) / countsPerRev;
            double positionCts = la.PositionCounts(positionInch);
            Assert.AreEqual(positionChk, positionInch," inch pos");
            Assert.AreEqual(3200, positionCts);

        }

    }
}
