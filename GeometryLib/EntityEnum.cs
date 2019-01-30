using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryLib
{
    public enum ViewPlane
    {
        XY,
        XZ,
        YZ,
        THETAR,
        ZR
    }
    public enum EntityType
    {
        
        Line,
        Arc,
        Circle,
        Spline,
        Text,
        Vector3,
        Vector2,
        PointCyl,
        Ray,
        Unknown
        
    }
}
