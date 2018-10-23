using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryLib
{
    public interface IGeometryRoutines<T> where T : DwgEntity
    {
        T RotateX(Vector3 rotationPt, double angleRad);
        T RotateY(Vector3 rotationPt, double angleRad);
        T RotateZ(Vector3 rotationPt, double angleRad);
        
        T Translate(Vector3 translation);
        
        T Clone();
        BoundingBox BoundingBox();
    }
}
