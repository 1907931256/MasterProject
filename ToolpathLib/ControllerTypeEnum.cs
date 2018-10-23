using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolpathLib
{
    public class Machine
    {
        public ControllerType Controller { get; set; }
        public string Name { get; set; }
        public int AxisCount { get; set; }
        public string[] AxisNames { get; set; }

    }
    
    public enum ControllerType
    {
        FAGOR8055,
        FAGOR8070,

    }
}
