using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarrelLib;
using DataLib;
using GeometryLib;
using System.Threading;

namespace InspectionLib
{
    public class CartesianDataBuilder : DataBuilder
    {
       
        InspDataSet BuildCartDataFromLineData(CylInspScript script, KeyenceLineScanDataSet rawDataSet)
        {
            try
            {
                var dataSet = new CartDataSet(_barrel);               
                dataSet.CartData = DataUtilities.ConvertToCartData(rawDataSet.GetData(),script.StartZ);               
               
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public InspDataSet BuildSingleLineAsync(CancellationToken ct, IProgress<int> progress, CylInspScript script,
           KeyenceLineScanDataSet rawDataSet, DataOutputOptions options)
        {
            try
            {
                Init(options, rawDataSet.Filename);
                var dataSet = BuildCartDataFromLineData(script, rawDataSet);               
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }       
        public CartesianDataBuilder(Barrel barrel) : base(barrel)
        {

        }
    }
}
