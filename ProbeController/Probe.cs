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
        public enum ProbeType
        {
            SI_F10,
            LJ_V7060
        }
        
        public double MinValue(MeasurementUnit units)
        {
            return _minValue * defaultUnits.ConversionFactor / units.ConversionFactor;            
        }
        public double MeasuringRange(MeasurementUnit units)
        {
            return _measureRange * defaultUnits.ConversionFactor / units.ConversionFactor;
        }
        public double MaxValue(MeasurementUnit units)
        {           
           return MinValue(units) + MeasuringRange(units);           
        }
        public double ProbePhaseRad { get; set; }
        public ProbeType Type { get; private set; } 
        public MeasurementUnit Units {get;set;}
        double _minValue;
        double _measureRange;
        MeasurementUnit defaultUnits;
        public static ProbeType GetProbeType(string probeType)
        {
            var pt = probeType.ToUpper();
            var type = ProbeType.LJ_V7060;
            if (probeType.Contains("SI_F10"))
                type = ProbeType.SI_F10;
            if (probeType.Contains("LJ_V7060"))
                type = ProbeType.LJ_V7060;
            return type;
        }
        //public Probe()
        //{
        //    this.Type = ProbeType.SI_F10;
        //    ProbePhaseRad = 0;
        //    SetupProbe();
        //}
        public Probe (ProbeType probeType,MeasurementUnit measurementUnit,double phaseAngleRad)
        {
            Units = measurementUnit;
            ProbePhaseRad = phaseAngleRad;
            this.Type = probeType;
            SetupProbe();
        }       
        void SetupProbe()
        {
            switch(Type)
            {
                case ProbeType.LJ_V7060:
                    _minValue = -8000;
                    _measureRange = 16000;
                    defaultUnits = new MeasurementUnit(LengthUnit.MICRON);
                    break;
                case ProbeType.SI_F10:
                    _minValue = 0;
                    _measureRange = 1050;
                    defaultUnits = new MeasurementUnit(LengthUnit.MICRON);
                    break;

            }
        }
    }
    
}
