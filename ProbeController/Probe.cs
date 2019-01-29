using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
namespace ProbeController
{
    /// <summary>
    /// contains probe properties
    /// </summary>
    
    public class Probe
    {
        
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public double StartMeasuringRange { get; set; }        
        public double MeasuringRange { get; set; }
         
        public MeasurementUnit MeasurementUnit {get;set;}
        public static ProbeType GetProbeType(string probeType)
        {
            var type = ProbeType.LINE_SCAN;
            if (probeType == "SI_DISTANCE")
                type = ProbeType.SI_DISTANCE;
            if (probeType == "LINE_SCAN")
                type = ProbeType.LINE_SCAN;
            return type;
        }
        public Probe()
        {
            Name = "Not Connected";
            SerialNumber = "";
            MeasurementUnit = new MeasurementUnit(MeasurementUnitEnum.MICRON);
            StartMeasuringRange = 0;
            MeasuringRange = 0;
        }
        public Probe(Probe probe)
        {
            Name = probe.Name;         
            
            StartMeasuringRange = probe.StartMeasuringRange;
            MeasuringRange = probe.MeasuringRange;            
        }
       
        /// <summary>
        /// returns probe description as multiline string
        /// </summary>
        /// <returns></returns>
        public string Description()
        {
            StringBuilder description = new StringBuilder();
            description.Append("Model:" + Name+'\r' +'\n');
           
            description.Append("SerialNumber:" + SerialNumber + '\r' + '\n');
            description.Append("Start Measuring Range(in):" + StartMeasuringRange.ToString("f4") + '\r'+'\n');
            description.Append("Measuring Range(in):" + MeasuringRange.ToString("f4")+ '\r'+'\n');
            
            return description.ToString();
        }
       
    }
    
}
