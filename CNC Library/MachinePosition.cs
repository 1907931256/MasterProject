using System;
using GeometryLib;

namespace CNCLib
{   
    public class MachinePosition
    {
        public double X { get { return _x; } set { _x = value; } }
        public double Y { get { return _y; } set { _y = value; } }
        public double Z { get { return _z; } set { _z = value; } }

        public double Adeg { get { return Geometry.ToDegs(_aRad); } set { _aRad = Geometry.ToRadians(value); } }
        public double Bdeg { get { return Geometry.ToDegs(_bRad); } set { _bRad = Geometry.ToRadians(value); } }
        public double Cdeg { get { return Geometry.ToDegs(_cRad); } set { _cRad = Geometry.ToRadians(value); } }
        public MachineGeometry MachineGeometry{ get { return _geometry; } }
        protected MachineGeometry _geometry;
        double _aRad;
        double _bRad;
        double _cRad;
        double _x;
        double _y;
        double _z;
        public double DistanceTo(MachinePosition pos)
        {
            var v1 = new Vector3(_x, _y, _z);
            var v2 = new Vector3(pos.X, pos.Y, pos.Z);
            double d = v1.DistanceTo(v2);
            return d;
        }
        public MachinePosition(double x, double y, double z , double bDegs,double cDegs)
        {
            _geometry = MachineGeometry.XYZBC;

            _bRad = Geometry.ToRadians(bDegs);
            _cRad = Geometry.ToRadians(cDegs);
            _x = x;
            _y = y;
            _z = y;

        }
        
        public MachinePosition(MachineGeometry geometry)
        {
            _geometry = geometry;

            _bRad = 0;
            _cRad = 0;
            _x = 0;
            _y = 0;
            _z = 0;
                     
        }
         public MachinePosition Clone()
        {
            return new MachinePosition(CNCLib.MachineGeometry.XYZBC) { X = this.X, Y = this.Y, Z = this.Z,Bdeg=this.Bdeg,Cdeg= this.Cdeg };
        }
        public Vector3 ToVector3()
        {
            var v = new Vector3(_x, _y, _z);
            return v;
        }
    }

  
  
}
