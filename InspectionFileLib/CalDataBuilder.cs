using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BarrelLib;

namespace InspectionLib
{
    public class CalDataBuilder : DataBuilder
    {
        public CalDataSet BuildCalData(int probeCount, double ringCalSizeInch, string filename,ProbeController.ProbeDirection probeDirection)
        {
            var rawData = new KeyenceSISensorDataSet(probeCount, filename, 0);
            var data = rawData.GetAllProbeData();
            var cal = new CalDataSet(ringCalSizeInch, data[0, 0], data[0, 1],probeDirection);

            return cal;
        }
        public CalDataBuilder(Barrel barrel) : base(barrel)
        {

        }
    }
}
