using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeController
{
    public class MeasurementUnit
    {
        public double ConversionFactor
        {
            get
            {
                return _value;
            }
        }
        public string Name
        {
            get
            {
                return upperName;
            }
        }
        double _value;
        string upperName;
        public MeasurementUnit(string name)
        {
            _value = 1;
            upperName = name.ToUpper();
            _value = MeasurementUnitDictionary.GetConversionFactor(upperName);
        }

    }
   
}
