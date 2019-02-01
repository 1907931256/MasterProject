using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeController
{
    public enum LengthUnitEnum
    {        
        MICRON=1,
        MM=1000,
        INCH = 25400
    }

    static public class MeasurementUnitDictionary
    {
        static public LengthUnitEnum GetUnits(string name)
        {
            LengthUnitEnum value = LengthUnitEnum.MICRON;
            unitDictionary.TryGetValue(name, out value);
            return value;
        }
        static public List<string> MeasurementUnitNames()
        {
            var keys = unitDictionary.Keys;
            var keyList = new List<string>();
            foreach (string key in keys)
            {
                keyList.Add(key);
            }
            return keyList;
        }

        static Dictionary<string, LengthUnitEnum> unitDictionary;
        static MeasurementUnitDictionary()
        {
            unitDictionary = new Dictionary<string, LengthUnitEnum>();
            unitDictionary.Add("INCH", LengthUnitEnum.INCH);
            unitDictionary.Add("IN", LengthUnitEnum.INCH);
            unitDictionary.Add("MICRON", LengthUnitEnum.MICRON);
            unitDictionary.Add("MM", LengthUnitEnum.MM);
            unitDictionary.Add("UM", LengthUnitEnum.MICRON);

        }
    }
}
