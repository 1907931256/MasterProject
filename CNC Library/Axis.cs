using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNCLib
{
      [System.Xml.Serialization.XmlInclude(typeof(LinearAxis))]
      [System.Xml.Serialization.XmlInclude(typeof(RotaryAxis))]
    public class Axis
    {
        protected uint encoderCtsPerUnit;
        protected uint encoderOffset;
        
        public int AxisNumber { get; set; }
        
        public AxisTypeEnum Type { get; set; }
        public string PLCVarName { get; set; }
        public string Name { get; set; }
        
        public uint EncoderCtsPerRev { get { return encoderCtsPerUnit; } set { encoderCtsPerUnit = value; } }
        public uint EncoderOffset { get { return encoderOffset; } set { encoderOffset = value; } }
 
        public Axis(int number, string name, AxisTypeEnum type, string plcVariable)
        {
            if (number >= 0)
                AxisNumber = number;
            else
                AxisNumber = 0;

            if (name != null && name != "")
                Name = name;
            else
                Name = "Axis" + AxisNumber.ToString();

            

            if(plcVariable != null && plcVariable!="")
                PLCVarName = plcVariable;
            else
                PLCVarName = "NONE" + AxisNumber.ToString();

                Type = type;
        }
        public Axis()
        {
            Name = "Axis";
            AxisNumber = 0;
            PLCVarName = "NONE";
            Type = AxisTypeEnum.Unknown;
        }
    }
}
