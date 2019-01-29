using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeController
{
    public enum MeasurementUnitEnum
    {        
        MICRON=1,
        MM=1000,
        INCH = 25400
    }

    static public class MeasurementUnitDictionary
    {
        static public MeasurementUnitEnum GetUnits(string name)
        {
            MeasurementUnitEnum value = MeasurementUnitEnum.MICRON;
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

        static Dictionary<string, MeasurementUnitEnum> unitDictionary;
        static MeasurementUnitDictionary()
        {
            unitDictionary = new Dictionary<string, MeasurementUnitEnum>();
            unitDictionary.Add("INCH", MeasurementUnitEnum.INCH);
            unitDictionary.Add("IN", MeasurementUnitEnum.INCH);
            unitDictionary.Add("MICRON", MeasurementUnitEnum.MICRON);
            unitDictionary.Add("MM", MeasurementUnitEnum.MM);
            unitDictionary.Add("UM", MeasurementUnitEnum.MICRON);

        }
    }
}
