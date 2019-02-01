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
 
   
   
    /// <summary>
    /// contains properties of inspection script used to build NC inspection file
    /// </summary>
    /// 
   
    [System.Xml.Serialization.XmlInclude(typeof(CylInspScript))]
  
    public  class InspectionScript
    {        
        public string FileName { get; set; }       
        public ScanFormat ScanFormat { get; protected set; }
        public MeasurementUnit OutputUnit { get; protected set; }
       
        public ProbeSetup ProbeSetup { get; set; }
        public CalDataSet CalDataSet { get; set; }        
        public MachinePosition StartLocation { get; set; }
        public MachinePosition EndLocation { get; set; }
        public InspectionScript()
        {
            FileName = InspectionScriptFile.TempFileName;
            ScanFormat = ScanFormat.RING;
            OutputUnit = new MeasurementUnit(LengthUnitEnum.INCH);
            CalDataSet = new CalDataSet();
            ProbeSetup = new ProbeSetup();
            StartLocation = new MachinePosition(MachineGeometry.XYZBC);
            EndLocation = new MachinePosition(MachineGeometry.XYZBC);
        }
    }
}
