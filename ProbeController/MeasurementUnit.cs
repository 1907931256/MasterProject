using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeController
{
    public enum LengthUnit
    {
        MICRON,
        UM,
        MM,
        NANOX10,
        NANO,
        INCH        
    }
    public class MeasurementUnit
    {
        public double ConversionFactor{ get; private set; }
        public string Name { get; private set; }
        public LengthUnit LengthUnits { get; private set; }
        static public MeasurementUnit GetMeasurementUnit(string[,] words)
        {
            try
            {
                var unitList = MeasurementUnitDictionary.MeasurementUnitNames();                
                LengthUnit lengthUnit = LengthUnit.MICRON;
                var inputUnit = new MeasurementUnit(lengthUnit);
                foreach (string unitStr in unitList)
                {
                    for (int i = 0; i < words.GetLength(0); i++)
                    {
                        for (int j = 0; j < words.GetLength(1); j++)
                        {
                            string upperw = words[i, j].ToUpper();
                            if (upperw.Contains(unitStr))
                            {
                                
                                Enum.TryParse(unitStr, out lengthUnit);
                                inputUnit = new MeasurementUnit(lengthUnit);
                                break;
                            }
                        }
                    }
                }
                return inputUnit;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public MeasurementUnit(LengthUnit lengthUnit)
        {
            ConversionFactor = MeasurementUnitDictionary.GetConversionFactor(lengthUnit);
            Name = lengthUnit.ToString();
            LengthUnits = lengthUnit;
        }
       
    }
   
}
