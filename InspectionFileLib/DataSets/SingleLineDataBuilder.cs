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
       
        static InspDataSet BuildCartDataFromLineData(SingleCylInspScript script, Vector2[] rawDataSet)
        {
            try
            {
                var dataSet = new CartDataSet(script.InputDataFileName);               
                dataSet.CartData = DataUtilities.ConvertToCartData(rawDataSet,script.Location.X);                
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }

        }
       
        static public InspDataSet BuildDataAsync(CancellationToken ct, IProgress<int> progress, SingleCylInspScript script, Vector2[] rawDataSet )
        {
            try
            {
                //Init(options);
                return BuildCartDataFromLineData(script, rawDataSet);
            }
            catch (Exception)
            {
                throw;
            }
        }
       
        public SingleScanDataBuilder() 
        {

        }
    }
}
