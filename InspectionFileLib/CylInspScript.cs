using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNCLib;
using GeometryLib;
using ProbeController;
namespace InspectionLib
{
    public class CartInspScript:InspectionScript
    {
        public XYZBCMachPosition StartLocation { get; set; }
        public XYZBCMachPosition EndLocation { get; set; }
        public CartInspScript(DataLib.ScanFormat scanFormat)
        {
            ScanFormat = scanFormat;
            StartLocation = new XYZBCMachPosition(0,0,0,0,0);
            EndLocation = new XYZBCMachPosition(0,0,0,0,0);
        }
    }
    public class CylInspScript : InspectionScript
    {
        public XAMachPostion StartLocation { get; set; }
        public XAMachPostion EndLocation { get; set; }
       
        public double AngleIncrement { get; set; }
        public double AxialIncrement { get; set; }
        public int ZDir { get { return _zDir; } }
        public int[] Grooves;       
        public int PointsPerRevolution { get { return _pointsPerRev; } }
        public int ThetaDir { get { return _thDir; } }
        protected int _zDir;
        int _pointsPerRev;
        int _thDir;
        double[] _extractX;
        public CylInspScript(DataLib.ScanFormat scanformat, XAMachPostion start, XAMachPostion end, double pitchInch, int pointsPerRevolution,CalDataSet calDataSet)
        {
            init(scanformat, start, end);
            CalDataSet = calDataSet;
            _pointsPerRev = pointsPerRevolution;
            CalcIncrement(pitchInch, pointsPerRevolution);
        }
        //spiral ring constructor
        public CylInspScript(DataLib.ScanFormat scanformat, XAMachPostion start, XAMachPostion end, double pitchInch, int pointsPerRevolution) 
        {
            init(scanformat, start, end);
            _pointsPerRev = pointsPerRevolution;
            CalcIncrement(pitchInch,  pointsPerRevolution);
        }
        void CalcIncrement(double pitchInch, int pointsPerRevolution)
        {
            
            AxialIncrement = pitchInch / _pointsPerRev;
            AngleIncrement = Math.PI * 2 / _pointsPerRev;
            _thDir = Math.Sign(EndLocation.Adeg - StartLocation.Adeg);
            if (_thDir == 0)
                _thDir = 1;
            _zDir = Math.Sign(EndLocation.X - StartLocation.X);
            if (_zDir == 0)
                _zDir = 1;
        }
        

       //groove land constructor
        public CylInspScript(DataLib.ScanFormat scanformat, XAMachPostion start, XAMachPostion end, double axialInc, int[] grooves) 
        {
            init(scanformat, start, end);
            AxialIncrement = axialInc;
            _zDir = Math.Sign(EndLocation.X - StartLocation.X);
            if (_zDir == 0)
                _zDir = 1;
            Grooves = grooves;
        }
        //axial constructor
        public CylInspScript(DataLib.ScanFormat scanformat, XAMachPostion start, XAMachPostion end, double axialInc) 
        {
            init(scanformat, start, end);

            AxialIncrement = axialInc;
            _zDir = Math.Sign(EndLocation.X - StartLocation.X);
            if (_zDir == 0)
                _zDir = 1;
        }
        public CylInspScript(DataLib.ScanFormat scanformat, XAMachPostion start, XAMachPostion end)
        {
            init(scanformat, start, end);
        }
        public CylInspScript(DataLib.ScanFormat scanFormat)
        {
            ScanFormat = scanFormat;
            StartLocation = new XAMachPostion();
            EndLocation = new XAMachPostion();
        }
        void init(DataLib.ScanFormat scanformat, XAMachPostion start, XAMachPostion end)
        {
            ScanFormat = scanformat;
            StartLocation = start;
            EndLocation = end;            
            ProbeSetup = new ProbeController.ProbeSetup();
            CalDataSet = new CalDataSet(0,0, ProbeController.ProbeDirection.ID);
        }
    }
}
