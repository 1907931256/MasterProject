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
        protected PointCyl GetPoint(int i, AxialInspScript script, double r)
        {
            var z = script.ZDir * i * script.AxialIncrement + script.StartLocation.X;
            var theta = Geometry.ToRadians(script.StartLocation.Adeg);
            var pt = new PointCyl(r, theta, z, i);
            return pt;
        }
        /// <summary>
        /// build axial data set from raw data
        /// </summary>
        /// <param name="script"></param>
        /// <param name="rawInputData"></param>
        InspDataSet BuildAxialPoints(AxialInspScript script, double[] data)
        {
            try
            {
                var points = new CylData(script.InputDataFileName);
                var len = data.GetLength(0);
                if (len == 0)
                {
                    throw new Exception("Data file length cannot equal zero");
                }
                script.AxialIncrement = Math.Abs((script.EndLocation.X - script.StartLocation.X) / len);
                if (script.AxialIncrement == 0)
                {
                    throw new Exception("Axial increment cannot equal zero.");
                }

                for (int i = 0; i < len; i++)
                {                   
                    points.Add(GetPoint(i,script, (data[i] + script.CalDataSet.ProbeSpacingInch) / 2.0));
                }
                var dataSet = new AxialDataSet(_barrel,script.InputDataFileName);
                dataSet.CorrectedCylData = points;
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }

        }
        InspDataSet BuildFromAxial(AxialInspScript script, double[] rawInputData)
        {
            try
            {
                Debug.WriteLine("building data from axial inspection");
                var data = rawInputData;
                return BuildAxialPoints(script, data);

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
        public InspDataSet BuildAxialAsync(CancellationToken ct, IProgress<int> progress, AxialInspScript script, double[] rawDataSet, DataOutputOptions options)
        {
            try
            {
                Init(options);                
                return BuildFromAxial(script, rawDataSet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public AxialDataBuilder(Barrel barrel) : base(barrel)
        {

        }
    }
}
