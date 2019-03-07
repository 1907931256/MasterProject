using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeController
{
    public enum ProbeConfig
    {
        SINGLE_SI_F10,
        DUAL_SI_F10,
        SINGLE_LJ_V7060,
        LJ_V7060_SI_F10
    }
    public enum ProbeDirection
    {
        ID = 1,
        OD = -1
    }
    public class ProbeSetup : List<Probe>
    {
        public ProbeConfig ProbeConfig{get;set;}
        public ProbeDirection Direction { get; set; }
        public ProbeSetup()
        {
                   
        }
    }
}
