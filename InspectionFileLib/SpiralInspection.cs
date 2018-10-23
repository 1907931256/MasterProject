using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
using CNCLib;
namespace InspectionLib
{
   // public class SpiralInspection : CylInspScript
    //{
        
        
    //    public int PointsPerRevolution { get { return _pointsPerRev; } }
    //    public int ThetaDir { get { return _thDir; } }
    //    public int ZDir { get { return _zDir; } }
    //    public double[] ExtractLocations { get { return _extractX; }set { _extractX = value; } }

    //    double _pitchInch;
    //    int _thDir;
    //    int _zDir;
    //    int _pointsPerRev;
    //    double[] _extractX;

    //    public SpiralInspection(MachinePosition start, MachinePosition end, double pitchInch,int pointsPerRevolution):base(start,end)
    //    {
    //        _method = InspectionMethod.SPIRAL;
    //        _pitchInch = pitchInch;
    //        _pointsPerRev = pointsPerRevolution;            
    //        CalcIncrement();            
    //    }
    //    void CalcIncrement()
    //    {

    //        AxialIncrement = _pitchInch / _pointsPerRev;
    //        AngleIncrement = Math.PI * 2 / _pointsPerRev;
    //        _thDir = Math.Sign(EndThetaRad - StartThetaRad);
    //        if (_thDir == 0)
    //            _thDir = 1;
    //        _zDir = Math.Sign(EndZ - StartZ);
    //        if (_zDir == 0)
    //            _zDir = 1;
    //    }
    //}
}
