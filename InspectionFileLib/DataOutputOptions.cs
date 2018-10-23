using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;

namespace InspectionLib
{
    public class DataOutputOptions
    {
        public static string FileName = "DataOptions.xml";
        public double SurfaceFileScaleFactor { get; set; }
        public bool SaveSurfaceFlat { get; set; }
        public bool SaveCSVUncorrected { get; set; }           
        public bool ColorCodeData { get; set; }
        public DataLib.COLORCODE SurfaceColorCode { get; set; }
        public string DefMuzzleRasterFilename { get; set; }
        public string DefBreachRasterFilename { get; set; }
        public string DefProfileFilename { get; set; }
        public bool UseDefMuzzleRasterFile { get; set; }
        public bool UseDefBrchRasterFile { get; set; }
        public bool UseDefBarrelProfile { get; set; }
       
        public DataOutputOptions()
        {
            SurfaceFileScaleFactor = 10.0;
            SaveCSVUncorrected = false;
            DefMuzzleRasterFilename = "";
            DefBreachRasterFilename = "";
            DefProfileFilename = "";
            UseDefBarrelProfile = false;
            UseDefBrchRasterFile = false;
            UseDefMuzzleRasterFile = false;
            SaveSurfaceFlat = true;            
            ColorCodeData = true;
            SurfaceColorCode = DataLib.COLORCODE.MONO;
         
        }
    }

}
