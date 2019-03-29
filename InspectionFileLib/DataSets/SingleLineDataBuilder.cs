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
                         
                var cartData = DataUtil.ConvertToCartData(rawDataSet,script.Location.X);
                cartData.Translate(new Vector3(0, -1 * script.CalDataSet.NominalRadius, 0));
                var dataset = new CartDataSet(script.InputDataFileName);
                dataset.CartData = cartData;
                return dataset;
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
