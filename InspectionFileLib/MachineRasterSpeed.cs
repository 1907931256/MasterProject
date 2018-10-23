using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionLib
{
    public class MachineRasterSpeed
    {
        public int RasterIndex { get; set; }
        public double ThetaRel { get; set; }
        public double CurrentSpeed { get; set; }
        public double TargetDepth { get; set; }
        public double NextSpeed { get; set; }

        public MachineRasterSpeed(int rasterIndex, double thetaRelative, double speed, double targetDepth)
        {
            RasterIndex = rasterIndex;
            ThetaRel = thetaRelative;
            CurrentSpeed = speed;
            TargetDepth = targetDepth;
        }
    }
}
