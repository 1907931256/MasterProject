using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProbeController;
using BarrelLib;

namespace InspectionLib
{
    public class CalDataBuilder : DataBuilder
    {
        public CalDataSet BuildCalData(DataLib.ScanFormat scanFormat,MeasurementUnit outputUnits, int probeCount, double ringCalSizeInch, string filename,ProbeController.ProbeDirection probeDirection)
        {
            
            var rawData = new KeyenceSiDataSet(scanFormat, outputUnits, probeCount, filename);
            var data = rawData.GetData();
            var cal = new CalDataSet(ringCalSizeInch, data[0], data[ 1],probeDirection);

            return cal;
        }
        public CalDataBuilder(Barrel barrel) : base(barrel)
        {

        }
    }
}
