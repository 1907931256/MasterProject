using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
using System.Diagnostics;
using System.Threading;
using DataLib;
using BarrelLib;

namespace InspectionLib
{
    public class AxialDataBuilder : DataBuilder
    {
        static protected PointCyl GetPoint(int i, AxialInspScript script, double r)
        {
            var z =  i * script.AxialIncrement + script.StartLocation.X;
            var theta = GeomUtilities.ToRadians(script.StartLocation.Adeg);
            var pt = new PointCyl(r, theta, z, i);
            return pt;
        }
        /// <summary>
        /// build axial data set from raw data
        /// </summary>
        /// <param name="script"></param>
        /// <param name="rawInputData"></param>
        static InspDataSet BuildAxialPoints(AxialInspScript script, double[] data)
        {
            try
            {
                var points = new CylData(script.InputDataFileName);
                var len = data.Length;
                if (len == 0)
                {
                    throw new Exception("Data file length cannot equal zero");
                }
                script.AxialIncrement = Math.Abs((script.EndLocation.X - script.StartLocation.X) / len);
                if (script.AxialIncrement == 0)
                {
                    throw new Exception("Axial increment cannot equal zero.");
                }
                var dataSet = new CylDataSet( script.InputDataFileName);               
                for (int i = 0; i < len; i++)
                {
                    var pt = GetPoint(i, script, data[i] + script.CalDataSet.ProbeSpacingInch / 2.0);
                    dataSet.CylData.Add(pt);
                    dataSet.UncorrectedCylData.Add(pt);
                }
                dataSet.DataFormat = script.ScanFormat;
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }

        }
        

        /// <summary>
        /// build axial data from raw set
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="progress"></param>
        /// <param name="script"></param>
        /// <param name="rawDataSet"></param>
        /// <param name="options"></param>
        static public InspDataSet BuildDataAsync(CancellationToken ct, IProgress<int> progress, AxialInspScript script, double[] rawDataSet )
        {
            try
            {
               // Init(options);                
                return BuildAxialPoints(script, rawDataSet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public AxialDataBuilder()
        {

        }
    }
}
