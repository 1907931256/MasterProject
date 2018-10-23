using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DwgConverterLib;
using GeometryLib;
using System.Collections.Generic;
namespace DwgConverterTests
{
    [TestClass]
    public class StlFileTests
    {
        [TestMethod]
        public void stlFileParser_openBinary_fileOK()
        {
            string filename = "SURF-TEST-1.STL";
            StlFile file = StlFileParser.Open(filename);
            Assert.AreNotEqual(0, file.Count);
        }
        [TestMethod]
        public void StlFileParser_parseAscii_fileOK()
        {
            string filename = "RECT-TEST-1.STL";
            StlFile file =  StlFileParser.Open(filename);
            Assert.AreEqual(2, file.Count);

            Assert.AreEqual(-2.965822, file[0].Vertices[0].X, .001);
            Assert.AreEqual(1.471885, file[0].Vertices[0].Y, .001);
            Assert.AreEqual(2.039691, file[1].Vertices[2].X, .001);
            Assert.AreEqual(-1.323043, file[1].Vertices[2].Y, .001);
            Assert.AreEqual(1.0, file[0].Normal.Z, .001);
        }
        [TestMethod]
        public void StlFileParser_saveAscii_fileOK()
        {
            string filename = "RECT-TEST-1.STL";
            string filename2 = "test-save-ascii.stl";
            StlFile file = StlFileParser.Open(filename);
            StlFileParser.SaveAscii(file, filename2);
            StlFile file2 = StlFileParser.Open(filename2);
            Assert.AreEqual(file.Count, file2.Count);
            Assert.AreEqual(file[0].Vertices[0].X, file2[0].Vertices[0].X, .001);
            Assert.AreEqual(file[1].Vertices[2].Y, file2[1].Vertices[2].Y, .001);
            Assert.AreEqual(file[0].Normal.Z, file2[0].Normal.Z, .001);
        }
        [TestMethod]
        public void stlFileParser_saveBinary_fileOK()
        {
            string filename = "RECT-TEST-1.STL";
            string filename2 = "test-save-binary.stl";
            StlFile file = StlFileParser.Open(filename);
            StlFileParser.SaveBinary(file, filename2);
            StlFile file2 = StlFileParser.Open(filename2);
            Assert.AreEqual(file.Count, file2.Count);
            Assert.AreEqual(file[0].Vertices[0].X, file2[0].Vertices[0].X, .001);
            Assert.AreEqual(file[1].Vertices[2].Y, file2[1].Vertices[2].Y, .001);
            Assert.AreEqual(file[0].Normal.Z, file2[0].Normal.Z, .001);
        }
        [TestMethod]
        public void stlFileParser_openAscii_createPointgrid()
        {
            string filename = "RECT-TEST-1.STL";
            StlFile file = StlFileParser.Open(filename);
            var pointList = new List<DwgEntity>();
            foreach(Triangle tri in file)
            {
                pointList.AddRange(tri.AsPointGrid(.1));
            }
            DxfFileBuilder.Save(pointList, "dxffromStl.dxf");
            Assert.AreEqual(1397, pointList.Count, 100);
           
        }
    }
}
