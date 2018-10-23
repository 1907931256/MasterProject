using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;


namespace InspectionLib
{
    /// <summary>
    /// single depth measurment 
    /// </summary>
    public class DepthMeasurement
    {
        public PointCyl Datum { get { return _datum; } set { _datum = value; } }
        public double Depth { get { return _depth; } set { _depth = value; } }
        public double Theta { get { return _thetaLocRad; } }
        public int RasterOrder { get { return _rasterOrder; } }
        double _thetaLocRad;
        PointCyl _datum;
        double _depth;
        int _rasterOrder;
        public DepthMeasurement(PointCyl inputDatum, double thetaRad, int rasterOrder)
        {
            _datum = inputDatum;
            _thetaLocRad = thetaRad;
            _rasterOrder = rasterOrder;
            _depth = -1;
        }
        public DepthMeasurement(PointCyl inputDatum, double thetaRad, int rasterOrder, double depth)
        {
            _datum = inputDatum;
            _thetaLocRad = thetaRad;
            _rasterOrder = rasterOrder;
            _depth = depth;
        }
    }
}
