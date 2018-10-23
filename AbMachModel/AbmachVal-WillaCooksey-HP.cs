using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbMachModel
{
    public class AbmachVal
    {
        public double Start;
        public double Model;
        public double Temp;
        public double Target;
        public double MachIndex;
        public double Mask;
        public double Depth { get { return Math.Abs(Start - Model); } }
        public double Remaining { get { return Math.Abs(Target - Model); } }
    }
}
