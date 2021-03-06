﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNCLib;
using GeometryLib;
using ProbeController;
using InspectionLib;
namespace InspectionLib
{
    public class CartInspScript:InspectionScript
    {
        public XYZBCMachPosition StartLocation { get; set; }
        public XYZBCMachPosition EndLocation { get; set; }
        public double IncrementInch { get; set; }

        public CartInspScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, CalDataSet calDataSet, XYZBCMachPosition start, XYZBCMachPosition end,double incrementInch)
            :base(scanFormat,outputUnit,probeSetup,calDataSet)
        {
            StartLocation = start;
            EndLocation = end;
             
            IncrementInch = incrementInch;
        }
    }
    public class StitchedInspScript:InspectionScript
    {
        List<XAMachPostion> Positions { get; set; }

        public StitchedInspScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, CalDataSet calDataSet
            , List<XAMachPostion> positions)
            : base(scanFormat, outputUnit, probeSetup, calDataSet)
        {
            Positions = positions;
        }
    }
    public class RasterInspScript : InspectionScript
    {
        public int PointsPerRevolution { get; set; }
        public double AngleIncrement { get; protected set; }
        public double AxialIncrement { get; set; }
        public XAMachPostion StartLocation { get; set; }
        public XAMachPostion EndLocation { get; set; }
        public RasterInspScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, CalDataSet calDataSet
           , XAMachPostion start, XAMachPostion end, int pointsPerRev, double axialInc)
            : base(scanFormat, outputUnit, probeSetup, calDataSet)
        {

            PointsPerRevolution = pointsPerRev;
            var sign = Math.Sign(EndLocation.X - StartLocation.X);
            AxialIncrement = sign * axialInc;
            
            var scanDir = Math.Sign(EndLocation.Adeg - StartLocation.Adeg);
            if (scanDir == 0)
                scanDir = 1;
            AngleIncrement = scanDir * Math.PI * 2 / PointsPerRevolution;
        }
    }

    public class CylInspScript: InspectionScript
    {
        
        public int PointsPerRevolution { get;  set; }
        public double AngleIncrement { get; protected set; }
        public XAMachPostion StartLocation { get; set; }
        public XAMachPostion EndLocation { get; set; }
       
        public CylInspScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, CalDataSet calDataSet
            ,XAMachPostion start,XAMachPostion end,int pointsPerRev)
            : base(scanFormat, outputUnit, probeSetup, calDataSet)
        {
            PointsPerRevolution = pointsPerRev;
            StartLocation = start;
            EndLocation = end;
            var scanDir = Math.Sign(EndLocation.Adeg - StartLocation.Adeg);
            if (scanDir == 0)
                scanDir = 1;
            AngleIncrement = scanDir * Math.PI * 2 / PointsPerRevolution;
        }
    }
    public class MultiRingScript : CylInspScript
    {
        public int PointsToSkip { get; set; }
        public double RingSpacing { get; set; }
        public MultiRingScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, CalDataSet calDataSet
           , XAMachPostion start, XAMachPostion end, int pointsPerRev, int pointsToSkip,double ringSpacing)
            : base(scanFormat, outputUnit, probeSetup, calDataSet,start,end,pointsPerRev)
        {
            var scanDir = Math.Sign(EndLocation.X - StartLocation.X) * Math.Sign(EndLocation.Adeg - StartLocation.Adeg);
            if (scanDir == 0)
                scanDir = 1;
            PointsToSkip = pointsToSkip;
            RingSpacing = ringSpacing;
        }
    }

    public class SpiralInspScript: CylInspScript
    {
        public double PitchInch { get; set; }
         
        public  SpiralInspScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, CalDataSet calDataSet
            , XAMachPostion start, XAMachPostion end, int pointsPerRev,double spiralPitchInch)
            : base(scanFormat, outputUnit, probeSetup, calDataSet,start,end,pointsPerRev)
        {
            var scanDir = Math.Sign(EndLocation.X - StartLocation.X) * Math.Sign(EndLocation.Adeg-StartLocation.Adeg);
            if (scanDir == 0)
                scanDir = 1;
            PitchInch = scanDir * spiralPitchInch;
        }
    }
    public class AxialInspScript: CylInspScript
    {
        public double AxialIncrement { get;  set; }       

        public AxialInspScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, CalDataSet calDataSet
            , XAMachPostion start, XAMachPostion end, double axialInc)
            : base(scanFormat, outputUnit, probeSetup, calDataSet, start, end, 0)
        {
            StartLocation = start;
            EndLocation = end;
            int scanDir = Math.Sign(EndLocation.X - StartLocation.X);
            if (scanDir == 0)
                scanDir = 1;
            AxialIncrement = scanDir * axialInc;
            
        }
    }
    public class GrooveInspScript : AxialInspScript
    {

        public BarrelLib.TwistProfile TwistProfile { get; set; }

        public GrooveInspScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, CalDataSet calDataSet
            , XAMachPostion start, XAMachPostion end, double axialInc, BarrelLib.TwistProfile twistProfile)
            : base(scanFormat, outputUnit, probeSetup, calDataSet, start, end, axialInc)
        {
            TwistProfile = twistProfile;
        }
    }

    public class SingleCylInspScript:InspectionScript
    {
        public XAMachPostion Location { get; set; }
        public SingleCylInspScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, CalDataSet calDataSet,
           XAMachPostion location) :base(scanFormat,outputUnit,probeSetup,calDataSet)
        {
            Location = location;
        }
    }
    public class SingleCartInspScript : InspectionScript
    {
        public XYZBCMachPosition Location { get; set; }
        public SingleCartInspScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, CalDataSet calDataSet,
           XYZBCMachPosition location) : base(scanFormat, outputUnit, probeSetup, calDataSet)
        {
            Location = location;
        }
    }
   
}
