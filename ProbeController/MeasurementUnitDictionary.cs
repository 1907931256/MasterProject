using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeController
{
   

    static public class MeasurementUnitDictionary
    {

        static public double ConversionToMM(LengthUnit lengthUnit)
        {
            double value = 1.0;
            unitDictionary.TryGetValue(lengthUnit, out value);
            return value;
        }
        static public List<string> MeasurementUnitNames()
        {
            var keys = unitDictionary.Keys;
            var keyList = new List<string>();
            foreach (LengthUnit key in keys)
            {
                keyList.Add(key.ToString());
            }
            return keyList;
        }

        static Dictionary<LengthUnit, double> unitDictionary;
        static MeasurementUnitDictionary()
        {
            unitDictionary = new Dictionary<LengthUnit, double>();
            unitDictionary.Add(LengthUnit.INCH, .03937008);           
            unitDictionary.Add(LengthUnit.MICRON, 1000);
            unitDictionary.Add(LengthUnit.MM, 1);
            unitDictionary.Add(LengthUnit.UM, 1000);
           
        }
    }
}
