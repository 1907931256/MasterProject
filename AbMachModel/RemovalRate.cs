using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbMachModel
{
    /// <summary>
    /// contains removal rate values
    /// </summary>
    public class RemovalRate
    {
        public double RemovalAt(double speed)
        {
            return DepthPerPass * speed / NominalSurfaceSpeed;
        }
        public double NominalSurfaceSpeed { get; set; }
        public double DepthPerPass { get; set; }
        public RemovalRate()
        {
            NominalSurfaceSpeed = 10;
            DepthPerPass = .001;
        }
        public RemovalRate(double nominalSurfaceSpeed, double depthPerPass)
        {
            NominalSurfaceSpeed = nominalSurfaceSpeed;
            DepthPerPass = depthPerPass;
        }
    }
}
