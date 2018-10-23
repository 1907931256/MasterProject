using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CNCLib
{
    /// <summary>
    /// machine speed linear and rotary
    /// </summary>
    public class MachineSpeed
    {
        public double LinearF { get; set; }
        public double RotaryF { get; set; }
        public double InverseFeed(double linearDistance, double rotaryDistance)
        {
            double invTime = 0;
            if (LinearF == 0) LinearF = 1;
            if (RotaryF == 0) RotaryF = 1;
            if ((LinearF == 0 && RotaryF == 0)||(linearDistance == 0 && rotaryDistance == 0))
                return 0;

            double time = Math.Abs(linearDistance / LinearF) + Math.Abs(rotaryDistance / RotaryF);
            invTime = 1 / time;
          
            return invTime;
        }
    }
}
