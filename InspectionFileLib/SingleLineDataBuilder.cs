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
    public class SingleScanDataBuilder : DataBuilder
    {
       
        InspDataSet BuildCartDataFromLineData(CartInspScript script, Vector2[] rawDataSet)
        {
            try
            {
                var dataSet = new CartDataSet(_barrel,script.InputDataFileName);               
                dataSet.CartData = DataUtilities.ConvertToCartData(rawDataSet,script.StartLocation.X);                
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }

        }
       
        public InspDataSet BuildSingleLineAsync(CancellationToken ct, IProgress<int> progress, CartInspScript script,
           Vector2[] rawDataSet, DataOutputOptions options)
        {
            try
            {
                Init(options);
                var dataSet = BuildCartDataFromLineData(script, rawDataSet);               
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
       
        public SingleScanDataBuilder(Barrel barrel) : base(barrel)
        {

        }
    }
}
