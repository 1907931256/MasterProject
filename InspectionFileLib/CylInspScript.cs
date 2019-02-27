using System;
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
        public CartInspScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, CalDataSet calDataSet)
            :base(scanFormat,outputUnit,probeSetup,calDataSet)
        {
            ScanFormat = scanFormat;
            StartLocation = new XYZBCMachPosition(0,0,0,0,0);
            EndLocation = new XYZBCMachPosition(0,0,0,0,0);
        }
    }
    public class RingInspScript:CylInspScript
    {
        
        public int PointsPerRevolution { get;  set; }
        public double AngleIncrement { get;  set; }
        public RingInspScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, CalDataSet calDataSet
            ,XAMachPostion start,XAMachPostion end,int pointsPerRev)
            : base(scanFormat, outputUnit, probeSetup, calDataSet,start,end)
        {
            PointsPerRevolution = pointsPerRev;
            AngleIncrement = Math.PI * 2 / PointsPerRevolution;            
        }
    }
    public class SpiralInspScript:RingInspScript
    {
        public double PitchInch { get; set; }
        
        public  SpiralInspScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, CalDataSet calDataSet
            , XAMachPostion start, XAMachPostion end, int pointsPerRev,double spiralPitchInch)
            : base(scanFormat, outputUnit, probeSetup, calDataSet,start,end,pointsPerRev)
        {
            PitchInch = spiralPitchInch;
        }
    }
    public class AxialInspScript:CylInspScript
    {
        public double AxialIncrement { get;  set; }
        public AxialInspScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, CalDataSet calDataSet
            , XAMachPostion start, XAMachPostion end, double axialInc)
            : base(scanFormat, outputUnit, probeSetup, calDataSet, start, end)
        {
            AxialIncrement = axialInc;
        }
    }
    public class CylInspScript : InspectionScript
    {
        public XAMachPostion StartLocation { get; set; }
        public XAMachPostion EndLocation { get; set; }
        public int[] Grooves;       
       
        public int ThetaDir { get; protected set; }
        public int ZDir { get; protected set; }

        void Init()
        {
            ThetaDir = Math.Sign(EndLocation.Adeg - StartLocation.Adeg);
            if (ThetaDir == 0)
                ThetaDir = 1;
            ZDir = Math.Sign(EndLocation.X - StartLocation.X);
            if (ZDir == 0)
                ZDir = 1;
        }


        //groove land constructor
        public CylInspScript(ScanFormat scanFormat, MeasurementUnit outputUnit, ProbeSetup probeSetup, CalDataSet calDataSet,
            XAMachPostion start, XAMachPostion end) :base(scanFormat,outputUnit,probeSetup,calDataSet)
        {
            StartLocation = start;
            EndLocation = end;
            Init();
        }

    }
}
