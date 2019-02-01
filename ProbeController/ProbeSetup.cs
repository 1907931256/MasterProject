using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeController
{
    public enum ProbeType
    {
        SI_DISTANCE,
        LINE_SCAN
    }
    public enum ProbeDirection
    {
        ID = 1,
        OD = -1
    }
    public class ProbeSetup
    {
        public bool UseDualProbeAve { get; set; }
        public double ProbePhaseDifferenceRad { get; set; }       
        public List<Probe> ProbeList;
        public ProbeDirection ProbeDirection { get; set; }
        public int ProbeCount { get; set; }
        public ProbeType ProbeType { get; protected set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public ProbeSetup()
        {
            ProbeList = new List<Probe>();
            ProbeDirection = ProbeDirection.ID;
            ProbeType = ProbeType.SI_DISTANCE;
        }
    }
}
