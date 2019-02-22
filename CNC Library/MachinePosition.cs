using System;
using GeometryLib;

namespace CNCLib
{   public class XAMachPostion:MachinePosition
    {
        
        double _aRad;
        double _x;
        public double Adeg { get { return Geometry.ToDegs(_aRad); } set { _aRad = Geometry.ToRadians(value); } }
        public double X { get { return _x; } set { _x = value; } }
        public XAMachPostion(double x,double aDegrees)
        {
            _geometry = MachineGeometry.XA;
            _aRad = Geometry.ToRadians(aDegrees);
            _x = x;
        }
        public XAMachPostion()
        {
            _geometry = MachineGeometry.XA;
            _aRad = 0;
            _x = 0;
        }
    }
    public class XYZBCMachPosition:MachinePosition
    {
        public double X { get { return _x; } set { _x = value; } }
        public double Y { get { return _y; } set { _y = value; } }
        public double Z { get { return _z; } set { _z = value; } }
        public double Bdeg { get { return Geometry.ToDegs(_bRad); } set { _bRad = Geometry.ToRadians(value); } }
        public double Cdeg { get { return Geometry.ToDegs(_cRad); } set { _cRad = Geometry.ToRadians(value); } }
        double _bRad;
        double _cRad;
        double _x;
        double _y;
        double _z;
        public double DistanceTo(XYZBCMachPosition pos)
        {
            var v1 = new Vector3(_x, _y, _z);
            var v2 = new Vector3(pos.X, pos.Y, pos.Z);
            double d = v1.DistanceTo(v2);
            return d;
        }
        public XYZBCMachPosition(XYZBCMachPosition p)
        {
            _geometry = MachineGeometry.XYZBC;

            _bRad = Geometry.ToRadians(p.Bdeg);
            _cRad = Geometry.ToRadians(p.Cdeg);
            _x = p.X;
            _y = p.Y;
            _z = p.Z;

        }
        public XYZBCMachPosition(double x, double y, double z, double bDegs, double cDegs)
        {
            _geometry = MachineGeometry.XYZBC;

            _bRad = Geometry.ToRadians(bDegs);
            _cRad = Geometry.ToRadians(cDegs);
            _x = x;
            _y = y;
            _z = Z;

        }
        public XYZBCMachPosition()
        {
            _geometry = MachineGeometry.XYZBC;
            _bRad = 0;
            _cRad =0;
            _x = 0;
            _y = 0;
            _z = 0;

        }
    }
    public class MachinePosition
    {
        public MachineGeometry MachineGeometry { get { return _geometry; } }
        protected MachineGeometry _geometry;
    }

  
  
}
