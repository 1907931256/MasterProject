using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarrelLib
{
    /// <summary>
    /// barrel twist value pair 
    /// </summary>
    public struct TwistValue
    {
        public double Z;
        public double ThetaRad;
        public TwistValue(double x, double aRad)
        {
            Z=x;
            ThetaRad = aRad;
        }
    }
}
