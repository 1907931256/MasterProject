using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
using ToolpathLib;
using CNCLib;
using DataLib;
using ProbeController;
namespace InspectionLib
{

    public enum ScanFormat
    {
        RING,
        SPIRAL,
        AXIAL,       
        CAL,
        SINGLE,
        RASTER,
        MULTIRING
    }

    /// <summary>
    /// contains properties of inspection script used to build NC inspection file
    /// </summary>
    /// 

    [System.Xml.Serialization.XmlInclude(typeof(CylInspScript))]
  
    public  class InspectionScript
    {        
        
        //public string FileName { get; set; }       
        public ScanFormat ScanFormat { get; protected set; }
        public MeasurementUnit OutputUnit { get; protected set; }
        public string InputDataFileName { get; set; }
        public ProbeSetup ProbeSetup { get; set; }
        public CalDataSet CalDataSet { get; set; }        
        
        public InspectionScript(ScanFormat scanFormat,MeasurementUnit outputUnit, ProbeSetup probeSetup,CalDataSet calDataSet)
        {

            ScanFormat = scanFormat;
            OutputUnit = outputUnit;
            CalDataSet = calDataSet;
            ProbeSetup = probeSetup;
            InputDataFileName = "unknownfile";
            
            
        }
    }
}
