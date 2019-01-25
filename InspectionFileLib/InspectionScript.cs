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
        public ProbeType ProbeType { get; protected set; }
        public InspectionScript()
        {
            FileName = InspectionScriptFile.TempFileName;
            ScanFormat = ScanFormat.RING;
            OutputUnit = new MeasurementUnit("inch");
        }
    }
}
