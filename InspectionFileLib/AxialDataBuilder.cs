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
        /// <summary>
        /// build axial data set from raw data
        /// </summary>
        /// <param name="script"></param>
        /// <param name="rawInputData"></param>
        InspDataSet BuildAxialPoints(CylInspScript script, double[] data)
        {
            try
            {
                var points = new CylData();
                var len = data.GetLength(0);
                if (len == 0)
                {
                    throw new Exception("Data file length cannot equal zero");
                }
                script.AxialIncrement = Math.Abs((script.EndZ - script.StartZ) / len);
                if (script.AxialIncrement == 0)
                {
                    throw new Exception("Axial increment cannot equal zero.");
                }

                for (int i = 0; i < len; i++)
                {
                    var z = script.ZDir * i * script.AxialIncrement + script.StartZ;
                    var theta = script.StartThetaRad;
                    var r = (data[i] + script.CalDataSet.ProbeSpacingInch) / 2.0;
                    var pt = new PointCyl(r, theta, z, i);
                    points.Add(pt);
                }
                var dataSet = new InspDataSet();
                dataSet.CorrectedCylData = points;
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }

        }
        InspDataSet BuildFromAxial(CylInspScript script, KeyenceSiDataSet rawInputData)
        {
            try
            {
                Debug.WriteLine("building data from axial inspection");
                var data = rawInputData.GetData();
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
        public InspDataSet BuildAxialAsync(CancellationToken ct, IProgress<int> progress, CylInspScript script, KeyenceSiDataSet rawDataSet, DataOutputOptions options)
        {
            try
            {
                Init(options, rawDataSet.Filename);
                var dataSet = new InspDataSet();
                dataSet = BuildFromAxial(script, rawDataSet);
                dataSet.DataFormat = ScanFormat.AXIAL;
                return dataSet;
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
