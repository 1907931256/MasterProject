using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeController
{
    public class MeasurementUnit
    {
        public double ConversionFactor{ get; private set; }
        public string Name { get; private set; }

        public MeasurementUnit(string name)
        {
            ConversionFactor = 1;
            Name = name.ToUpper();
            ConversionFactor = MeasurementUnitDictionary.GetConversionFactor(Name);
        }

    }
   
}
