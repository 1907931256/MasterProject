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
    public class RingDataBuilder : DataBuilder
    {
        public PointCyl[] LandPoints { get; private set; }

        
        public PointCyl GetMinRadiusDualProbe(CylInspScript script, double[] data,int windowHalfWidth)
        {
            double minDiam = double.MaxValue;

            for(int i=windowHalfWidth;i<data.GetUpperBound(0)-windowHalfWidth;i++)
            {
                double sum = 0;
                for(int j=i-windowHalfWidth;j<i+windowHalfWidth;j++)
                {
                    sum += (data[j]);
                }
                sum /= (windowHalfWidth * 2);
                if(sum<minDiam)
                {
                    minDiam = sum;
                }
            }
            minDiam += script.CalDataSet.ProbeSpacingInch;
            return new PointCyl(minDiam / 2, 0, script.StartZ);
        }
      
        override protected CylData GetDualProbeData(CylInspScript script, double[] data)
        {
            try
            {

                var points = new CylData(ScanFormat.RING );

                int pointCt = Math.Min(script.PointsPerRevolution, data.GetUpperBound(0));
                int indexShift = (int)Math.Round(script.PointsPerRevolution * (script.ProbeSetup.ProbePhaseDifferenceRad / (2 * Math.PI)));
                double minDiam = double.MaxValue;
                for (int i = 0; i < pointCt; i++)
                {
                    var z = script.StartZ;
                    var theta = script.ThetaDir * i * script.AngleIncrement + script.StartThetaRad;
                    int probe2Index = (i + indexShift) % script.PointsPerRevolution;

                    var diam = data[i];
                    var sum = data[i];
                    if (sum < minDiam)
                    {
                        minDiam = sum;
                    }
                    var pt1 = new PointCyl(sum / 2.0, theta, z, i);

                    points.Add(pt1);

                }
                points.NominalMinDiam = minDiam + script.CalDataSet.ProbeSpacingInch;
                return points;
            }
            catch (Exception)
            {

                throw;
            }
        }
        override protected CylData GetSingleProbeData(CylInspScript script, double[] data)
        {
            try
            {
                var points = new CylData(ScanFormat.RING );
                for (int i = 0; i < data.Length; i++)
                {
                    var z = script.StartZ;
                    var theta = script.ThetaDir * i * script.AngleIncrement + script.StartThetaRad;
                    var r = data[i];
                    var pt = new PointCyl(r, theta, z, i);
                    points.Add(pt);
                }
                points.NominalMinDiam = script.CalDataSet.NominalRadius * 2;
                return points;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// build ring data from raw data and auto find lands
        /// </summary>
        /// <param name="script"></param>
        /// <param name="rawInputData"></param>
        InspDataSet BuildRingFromRadialData(CylInspScript script, KeyenceSiDataSet rawInputData)
        {
            try
            {
                var dataSet = new RingDataSet(_barrel);
                dataSet.UncorrectedCylData = GetUncorrectedData(script, rawInputData);
                dataSet.RawLandPoints = GetLandPoints(dataSet.UncorrectedCylData, script.PointsPerRevolution);
                dataSet.CorrectedCylData = CorrectRing(dataSet.UncorrectedCylData, dataSet.RawLandPoints, script.ProbeSetup.ProbeDirection);
                dataSet.CorrectedLandPoints = GetLandPoints(dataSet.CorrectedCylData, script.PointsPerRevolution);
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// build ring data from raw and user input lands after initial processing
        /// </summary>
        /// <param name="script"></param>
        /// <param name="rawInputData"></param>
        /// <param name="landPointArr"></param>
        InspDataSet BuildRingFromRadialData(CylInspScript script, KeyenceSiDataSet rawInputData, PointCyl[] landPointArr)
        {
            try
            {
                var dataSet = new RingDataSet(_barrel);
                
                dataSet.UncorrectedCylData = GetUncorrectedData(script, rawInputData);
                dataSet.RawLandPoints = landPointArr;
                dataSet.CorrectedCylData = CorrectRing(dataSet.UncorrectedCylData, dataSet.RawLandPoints, script.ProbeSetup.ProbeDirection);
                dataSet.CorrectedLandPoints = GetLandPoints(dataSet.CorrectedCylData, script.PointsPerRevolution);
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// build ring data from raw and auto find lands
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="progress"></param>
        /// <param name="script"></param>
        /// <param name="rawDataSet"></param>
        /// <param name="options"></param>
        public InspDataSet BuildRingAsync(CancellationToken ct, IProgress<int> progress, CylInspScript script, KeyenceSiDataSet rawDataSet, DataOutputOptions options)
        {
            try
            {
                Init(options, rawDataSet.Filename);
                var dataSet = BuildRingFromRadialData(script, rawDataSet);               
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// build ring from raw data and user input lands
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="progress"></param>
        /// <param name="script"></param>
        /// <param name="rawDataSet"></param>
        /// <param name="landPointArr"></param>
        /// <param name="options"></param>
        public InspDataSet BuildRingAsync(CancellationToken ct, IProgress<int> progress, CylInspScript script, KeyenceSiDataSet rawDataSet, PointCyl[] landPointArr, DataOutputOptions options)
        {
            try
            {
                Init(options, rawDataSet.Filename);      
                var dataSet = BuildRingFromRadialData(script, rawDataSet, landPointArr);
               
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public RingDataBuilder(Barrel barrel) : base(barrel)
        {

        }
    }
}
