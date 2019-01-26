﻿using System;
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

    }
    public class CylInspScript : InspectionScript
    {
        public double StartZ { get { return _start.Z; } }
        public double EndZ { get { return _end.Z; } }
        public double StartThetaRad { get { return _start.ThetaRad; } }
        public double EndThetaRad { get { return _end.ThetaRad; } }
        public double AngleIncrement { get; set; }
        public double AxialIncrement { get; set; }
        public int ZDir { get { return _zDir; } }
        public int[] Grooves;       
        public int PointsPerRevolution { get { return _pointsPerRev; } }
        public int ThetaDir { get { return _thDir; } }
        public double[] ExtractLocations { get { return _extractX; } set { _extractX = value; } }
        public ProbeSetup ProbeSetup { get; set; }
       
        public CalDataSet CalDataSet { get; set; }
        protected MachinePosition _startPos;
        protected MachinePosition _endPos;
        protected PointCyl _start;
        protected PointCyl _end;
        
        protected int _zDir;
        
        
        int _pointsPerRev;
        int _thDir;
        double[] _extractX;
        public CylInspScript(DataLib.ScanFormat scanformat, MachinePosition start, MachinePosition end, double pitchInch, int pointsPerRevolution,CalDataSet calDataSet)
        {
            init(scanformat, start, end);
            CalDataSet = calDataSet;
            _pointsPerRev = pointsPerRevolution;
            CalcIncrement(pitchInch, pointsPerRevolution);
        }
        //spiral ring constructor
        public CylInspScript(DataLib.ScanFormat scanformat, MachinePosition start, MachinePosition end, double pitchInch, int pointsPerRevolution) 
        {
            init(scanformat, start, end);
            _pointsPerRev = pointsPerRevolution;
            CalcIncrement(pitchInch,  pointsPerRevolution);
        }
        void CalcIncrement(double pitchInch, int pointsPerRevolution)
        {
            
            AxialIncrement = pitchInch / _pointsPerRev;
            AngleIncrement = Math.PI * 2 / _pointsPerRev;
            _thDir = Math.Sign(EndThetaRad - StartThetaRad);
            if (_thDir == 0)
                _thDir = 1;
            _zDir = Math.Sign(EndZ - StartZ);
            if (_zDir == 0)
                _zDir = 1;
        }
        

       //groove land constructor
        public CylInspScript(DataLib.ScanFormat scanformat, MachinePosition start, MachinePosition end, double axialInc, int[] grooves) 
        {
            init(scanformat, start, end);
            AxialIncrement = axialInc;
            _zDir = Math.Sign(EndZ - StartZ);
            if (_zDir == 0)
                _zDir = 1;
            Grooves = grooves;
        }
        //axial constructor
        public CylInspScript(DataLib.ScanFormat scanformat, MachinePosition start, MachinePosition end, double axialInc) 
        {
            init(scanformat, start, end);

            AxialIncrement = axialInc;
            _zDir = Math.Sign(EndZ - StartZ);
            if (_zDir == 0)
                _zDir = 1;
        }
        public CylInspScript(DataLib.ScanFormat scanformat, MachinePosition start, MachinePosition end)
        {
            init(scanformat, start, end);
        }
        void init(DataLib.ScanFormat scanformat, MachinePosition start, MachinePosition end)
        {
            ScanFormat = scanformat;
            _startPos = start;
            _endPos = end;
            _start = new PointCyl(1, Geometry.ToRadians(_startPos.Adeg), _startPos.X);
            _end = new PointCyl(1, Geometry.ToRadians(_endPos.Adeg), _endPos.X);
            ProbeSetup = new ProbeController.ProbeSetup();
            CalDataSet = new CalDataSet(0,0,0, ProbeController.ProbeDirection.ID);
        }
    }
}
