using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileIOLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FileIOLibTests
{

    [TestClass]
    public class FileIOTests
    {
        
        string fileNameEmpty = "";
        string filenameParamFile = "TextFile1.txt";
        [TestMethod]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void ReadDataTextFile_FileIsEmpty_ReturnsEmptyList()
        {
            List<string> fileContents;
            fileContents = FileIO.ReadDataTextFile(fileNameEmpty);
            Assert.IsNotNull(fileContents);
        }
        [TestMethod]
        public void ReadDataTextFile_FileHasContent_ContentIsOK()
        {
            List<string> fileContents;
            fileContents = FileIO.ReadDataTextFile(filenameParamFile);           
            Assert.AreEqual(fileContents[0], "line1=1");
            
        }
        [TestMethod]
        public void ReadParamsTextFile_FileIsEmpty_ReturnsEmptyList()
        {
            List<string> fileContents;
            char[] delims = new char[1] { '=' };
            fileContents = FileIO.ReadParamsTextFile(fileNameEmpty, true, delims);
            Assert.IsNotNull(fileContents);
            

        }
        [TestMethod]
        public void ReadParamsTextFile_FileHasContent_ReturnsContent()
        {
            List<string> fileContents;
            char[] delims = new char[1] { '=' };
            fileContents = FileIO.ReadParamsTextFile(filenameParamFile, true, delims);
            Assert.AreEqual(3, fileContents.Count, "file length");
            Assert.AreEqual("1", fileContents[0], "line0_OK");
            Assert.AreEqual("2", fileContents[1], "line1_OK");
        }
        
        
    }
}
