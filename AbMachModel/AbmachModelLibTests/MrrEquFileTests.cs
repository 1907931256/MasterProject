using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using AbMachModel;
namespace AbmachModelLibTests
{

    [TestClass]
    public class MrrEquFileTests
    {
        [TestMethod]
        public void MrrEqFile_Open_dictIsNotNull()
        {
            string DefFile="mrrEquations.xml";

            MrrEquDictionary equations = MrrEquFile.Open(DefFile);

            Assert.IsNotNull(equations);
            

        }
        [TestMethod]
        public void MrrEqFile_NoFile_returnsDefVals()
        {
            string DefFile = "";

            MrrEquDictionary equations = MrrEquFile.Open(DefFile);

            double[] array = equations.GetEquation(10);

            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(0, array[1]);
            Assert.AreEqual(0, array[2]);
            Assert.AreEqual(0, array[3]);
            Assert.AreEqual(0, array[4]);
            Assert.AreEqual(0, array[5]);
            Assert.AreEqual(0, array[6]);            
            Assert.AreEqual(0.23, array[7]);

        }
        [TestMethod]
        public void mrrEqFile_open_dictIsCorrectLength()
        {
              string DefFile="mrrEquations.xml";

              MrrEquDictionary equations = MrrEquFile.Open(DefFile);
            Assert.AreEqual(3, equations.Length);
        }
        [TestMethod]
        public void MrrEqFile_open_returnsVals()
        {
             string DefFile="mrrEquations.xml";

             MrrEquDictionary equations = MrrEquFile.Open(DefFile);

            double[] array = equations.GetEquation(2);

            Assert.AreEqual(0.621, array[0]);
            Assert.AreEqual(0.896, array[1]);
            Assert.AreEqual(-10.67, array[2]);
            Assert.AreEqual(67.71, array[3]);
            Assert.AreEqual(-161.37, array[4]);
            Assert.AreEqual(154.76, array[5]);
            Assert.AreEqual(-51.95, array[6]);
            Assert.AreEqual(1, array[7]);
            
        
        }
        [TestMethod]
      public void MrrEqFile_openBadIndex_returnsDefaultEq()
      {
          string DefFile = "mrrEquations.xml";

          MrrEquDictionary equations = MrrEquFile.Open(DefFile);
          double[] array = equations.GetEquation(10);

          Assert.AreEqual(1, array[0]);
          Assert.AreEqual(0 ,array[1]);
          Assert.AreEqual(0, array[2]);
          Assert.AreEqual(0, array[3]);
          Assert.AreEqual(0, array[4]);
          Assert.AreEqual(0, array[5]);
          Assert.AreEqual(0, array[6]);
          Assert.AreEqual(0.23, array[7]);
      }
        [TestMethod]
        public void MrrEquFile_save_SavesOk()
        {
            string DefFile = "mrrEquations.xml";
            MrrEquDictionary equations1 = MrrEquFile.Open(DefFile);

            string saveFile = "mrrEquSave.xml";
            MrrEquFile.Save(equations1, saveFile);

            MrrEquDictionary equations2 = MrrEquFile.Open(saveFile);
            double[] d1arr1 = equations1.GetEquation(2);
            double[] d2arr1 = equations2.GetEquation(2);

            Assert.AreEqual(.621, d1arr1[0],"defaultArray");
            Assert.AreEqual(.621,d2arr1[0], "newArray");
        }
      
    }
}
