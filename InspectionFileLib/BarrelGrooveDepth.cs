using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionLib
{
    public class BarrelGrooveDepth
    {

        public double XLocation { get; set; }
        public double ThetatDeg { get; set; }
        public double TwistDeg { get; set; }
        public double DiamFinal { get; set; }
        public double DiamAsIs { get; set; }
        public double TargetGrooveDepth { get; set; }
        public double FinalGrooveDepth { get; set; }
        public double DeltaX { get; set; }
        public double DeltaA { get; set; }
        public double SegLen { get; set; }

        public BarrelGrooveDepth(double x, double thetaDegs, double twistDegs, double diamFinal, double diamAsIs, double targetDepth, double finalDepth)
        {
            XLocation = x;
            ThetatDeg = thetaDegs;
            TwistDeg = twistDegs;
            DiamAsIs = diamAsIs;
            DiamFinal = DiamFinal;
            TargetGrooveDepth = targetDepth;
            FinalGrooveDepth = finalDepth;
        }
    }
}
