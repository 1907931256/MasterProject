using System;
using System.Collections.Generic;

using GeometryLib;
using System.Diagnostics;
using System.Threading;
using DataLib;
using BarrelLib;

namespace InspectionLib
{
    public class SpiralDataBuilder : DataBuilder
    {


        override protected CylData GetSingleProbeData(CylInspScript script, double[] data)
        {
            try
            {
                var points = new CylData(ScanFormat.SPIRAL );
                for (int i = 0; i < data.Length; i++)
                {
                    var z = script.ZDir * i * script.AxialIncrement + script.StartZ;
                    var theta = script.ThetaDir * i * script.AngleIncrement + script.StartThetaRad;
                    var r = data[i];
                    var pt = new PointCyl(r, theta, z, i);
                    points.Add(pt);
                }
                return points;
            }
            catch (Exception)
            {
                throw;
            }
        }
        override protected CylData GetDualProbeData(CylInspScript script, double[] data)
        {
            try
            {
                try
                {

                    var points = new CylData(ScanFormat.SPIRAL);

                    int pointCt = Math.Min(script.PointsPerRevolution, data.GetUpperBound(0));
                    int indexShift = (int)Math.Round(script.PointsPerRevolution * (script.ProbeSetup.ProbePhaseDifferenceRad / (2 * Math.PI)));
                    double minSum = double.MaxValue;
                    for (int i = 0; i < pointCt; i++)
                    {
                        var z = script.ZDir * i * script.AxialIncrement + script.StartZ;
                        var theta = script.ThetaDir * i * script.AngleIncrement + script.StartThetaRad;
                        int probe2Index = (i + indexShift) % script.PointsPerRevolution;
                        var sum = data[i];
                        if (sum < minSum)
                        {
                            minSum = sum;
                        }
                        var pt1 = new PointCyl(sum / 2.0, theta, z, i);
                        points.Add(pt1);
                    }
                    points.NominalMinDiam = minSum + script.CalDataSet.ProbeSpacingInch;
                    return points;
                }
                catch (Exception)
                {

                    throw;
                }
               
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// separate point list into rings from spiral
        /// </summary>
        /// <returns></returns>
        CylGridData BuildGridList(CylData uncorrectedData, int thetaDirection)
        {
            try
            {
                int pointCountPerRev = 0;
                int revCount = 0;
                int pointIndex = 0;
                double thetaStart = Math.Abs(uncorrectedData[0].ThetaRad);
                double thetaEnd = Math.Abs(thetaStart + (thetaDirection * pi2));
                var pointList = new CylData(ScanFormat.SPIRAL);
                var uncorrectedGridData = new CylGridData(ScanFormat.SPIRAL);

                while (pointIndex < uncorrectedData.Count)
                {
                    var p = uncorrectedData[pointIndex];

                    var thA = Math.Abs(p.ThetaRad);
                    if (thA >= thetaStart && thA < thetaEnd)
                    {
                        pointList.Add(p);
                        pointCountPerRev++;
                        pointIndex++;
                    }
                    if (thA >= thetaEnd)
                    {

                        revCount++;

                        pointCountPerRev = 0;
                        uncorrectedGridData.Add(pointList);
                        pointList = new CylData(ScanFormat.SPIRAL );
                        thetaStart = thetaEnd;
                        thetaEnd = Math.Abs(thetaStart + (thetaDirection * pi2));
                    }
                }
                return uncorrectedGridData;
            }
            catch (Exception)
            {
                throw;
            }

        }
        SpiralDataSet BuildSpiralFromRadialData(CylInspScript script, KeyenceSiDataSet rawInputData, IProgress<int> progress)
        {
            try
            {
               

                var data = GetUncorrectedData(script, rawInputData);
                var dataSet = new SpiralDataSet(_barrel);
                var ringData = new RingDataSet(_barrel);
                ringData.UncorrectedCylData = GetUncorrectedData(script, rawInputData);
                dataSet.UncorrectedSpiralData = BuildGridList(ringData.UncorrectedCylData, script.ThetaDir);
                int totalrings = dataSet.UncorrectedSpiralData.Count;                

                //find all land points
                int i = 1;
                foreach (var row in dataSet.UncorrectedSpiralData)
                {
                   
                    var landPointArr = GetLandPoints(row, script.PointsPerRevolution);
                    //find polynomial fit for lands and correct for eccentricity        
                    
                    var correctedRing = CorrectRing(row, landPointArr, script.ProbeSetup.ProbeDirection);
                    dataSet.CorrectedSpiralData.Add(correctedRing);
                    int p = (int)(100 * i / totalrings);
                    progress.Report(p);
                    i++;
                }

                return dataSet;
            }

            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// build grid data from spiral raw set
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="progress"></param>
        /// <param name="script"></param>
        /// <param name="rawDataSet"></param>
        /// <param name="options"></param>
        public InspDataSet BuildSpiralAsync(CancellationToken ct, IProgress<int> progress, CylInspScript script, KeyenceSiDataSet rawDataSet, DataOutputOptions options)
        {
            try
            {
                Init(options, rawDataSet.Filename);
                var sw = new Stopwatch();
                progress.Report(sw.Elapsed.Seconds);              
               
                var dataSet = BuildSpiralFromRadialData(script, rawDataSet, progress);

                return dataSet;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<CylData> ExtractRings(CylGridData data, CylInspScript script)
        {
            try
            {
                var ringList = new List<CylData>();
                if (script.ExtractLocations.Length != 0)
                {
                    var tempList = new CylData(ScanFormat.SPIRAL );
                    foreach (var ring in data)
                    {
                        tempList.AddRange(ring);
                    }

                    double zMax = data.BoundingBox.Max.Z;
                    double zMin = data.BoundingBox.Min.Z;

                    var zList = new List<double>();
                    foreach (double z in script.ExtractLocations)
                    {
                        if (z >= zMin && z <= zMax)
                        {
                            zList.Add(z);
                        }
                    }
                    foreach (double z in zList)
                    {
                        ringList.Add(DataParser.GetRingFromSpiralMap(z, tempList, script.PointsPerRevolution));
                    }
                }
                return ringList;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public SpiralDataBuilder(Barrel barrel) : base(barrel)
        {

        }
    }
}
