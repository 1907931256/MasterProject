using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProbeController;
using BarrelLib;
namespace InspectionLib
{
    public class CalDataBuilder 
    {
        public static CalDataSet BuildCalData(InspectionScript script,double ringGageDiamInch, string CsvFileName)
        {
            try
            {
                if (script is CylInspScript cylScript)
                {                    
                    double data = 0;
                    if (cylScript.ProbeSetup.Count == 1)
                    {
                        var singleData = new KeyenceSiDataSet(script, CsvFileName);
                        data = singleData.GetData()[0];
                    }
                    else
                    {
                        var dualData = new KeyenceDualSiDataSet(script, CsvFileName);

                        data = dualData.GetData(ScanFormat.CAL)[0];
                    }
                    return new CalDataSet(ringGageDiamInch, data, cylScript.ProbeSetup.Direction);
                }
                else
                {
                    return new CalDataSet(script.CalDataSet.NominalRadius);
                }
            }       
            catch (System.IO.FileNotFoundException )
            {
                throw new Exception("Cal file not found");
            }
            catch (Exception)
            {

                throw;
            }
            
        }       
    }
}
