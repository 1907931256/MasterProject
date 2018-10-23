using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
using ToolpathLib;
using CNCLib;
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
        public InspectionMethod Method { get { return _method; } }
        public MachineSpeed InspectionFeedRate { get; set; }
        public double DataCollectionTimeSec { get; set; }
        protected InspectionMethod _method;
        
       
        public InspectionScript()
        {
            FileName = InspectionScriptFile.TempFileName;    
            _method = InspectionMethod.RING;            
            DataCollectionTimeSec = .1;
           
            InspectionFeedRate = new MachineSpeed()
            {
                LinearF = 1,
                RotaryF = 1
            };
                   
        }
    }
}
