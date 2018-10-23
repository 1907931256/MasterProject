using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNCLib
{
    public class LinearAxis : Axis
    {
        

        public double PositionInch(double counts)
        {
            double pos = (counts - encoderOffset) / encoderCtsPerUnit;
            return pos;
        }
        public double PositionCounts(double positionInch)
        {
            double counts = (positionInch * encoderCtsPerUnit) + encoderOffset;
            return counts;
        }
        public LinearAxis(int axisNumber, string name, string plcVariable, uint encoderCtsPerInch, uint encoderOffset)
            : base(axisNumber, name, AxisTypeEnum.Linear, plcVariable)
        {
            encoderCtsPerUnit = encoderCtsPerInch;
            this.encoderOffset = encoderOffset;
        }
        public LinearAxis(int axisNumber, string name,  string plcVariable)
            : base(axisNumber, name, AxisTypeEnum.Linear, plcVariable)
        {           
        }
        public LinearAxis():base(0,"",AxisTypeEnum.Linear,"")
        {    
        }
    }
}
