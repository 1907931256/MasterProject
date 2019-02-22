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
        public double StartMeasuringRange { get; set; }        
        public double MeasuringRange { get; set; }
        public ProbeType Type { get; private set; } 
        public MeasurementUnit MeasurementUnit {get;set;}
        public static ProbeType GetProbeType(string probeType)
        {
            var pt = probeType.ToUpper();
            var type = ProbeType.LJ;
            if (probeType.Contains("SI"))
                type = ProbeType.SI;
            if (probeType.Contains("LJ"))
                type = ProbeType.LJ;
            return type;
        }
        public Probe()
        {
            this.Type = ProbeType.SI;
            MeasurementUnit = new MeasurementUnit(LengthUnit.MICRON);
            StartMeasuringRange = 0;
            MeasuringRange = 0;
        }
        public Probe(string probeType)
        {
            this.Type = GetProbeType(probeType);
            //StartMeasuringRange = probe.StartMeasuringRange;
            //MeasuringRange = probe.MeasuringRange;            
        }
        public Probe(ProbeType probeType)
        {
            this.Type = probeType;
        }
        /// <summary>
        /// returns probe description as multiline string
        /// </summary>
        /// <returns></returns>
        public string Description()
        {
            StringBuilder description = new StringBuilder();
           // description.Append("Model:" + Name+'\r' +'\n');
           
           // description.Append("SerialNumber:" + SerialNumber + '\r' + '\n');
            description.Append("Start Measuring Range(in):" + StartMeasuringRange.ToString("f4") + '\r'+'\n');
            description.Append("Measuring Range(in):" + MeasuringRange.ToString("f4")+ '\r'+'\n');
            
            return description.ToString();
        }
       
    }
    
}
