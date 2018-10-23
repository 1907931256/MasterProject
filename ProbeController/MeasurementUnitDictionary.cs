using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeController
{
    static public class MeasurementUnitDictionary
    {
        static public double GetConversionFactor(string name)
        {
            double value = 1;
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

        static Dictionary<string, double> unitDictionary;
        static MeasurementUnitDictionary()
        {
            unitDictionary = new Dictionary<string, double>();
            unitDictionary.Add("INCH", 25400);
            unitDictionary.Add("IN", 25400);
            unitDictionary.Add("MICRON", 1);
            unitDictionary.Add("MM", 1000);
            unitDictionary.Add("UM", 1);

        }
    }
}
