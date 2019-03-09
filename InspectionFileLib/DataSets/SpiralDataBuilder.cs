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

        static protected PointCyl GetPoint(int i, SpiralInspScript script, double r)
        {
            var z =  i / script.PitchInch + script.StartLocation.X;
            var theta =  i * script.AngleIncrement + GeomUtilities.ToRadians(script.StartLocation.Adeg);
            var pt = new PointCyl(r, theta, z, i);
            return pt;
        }
        static protected CylData GetData(SpiralInspScript script, double[] data)
        {
            try
            {
                var points = new CylData(script.InputDataFileName);
                for (int i = 0; i < data.Length; i++)
                {                   
                    points.Add(GetPoint(i,script,data[i]));
                }
                return points;
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
        static CylGridData BuildGridList(CylData uncorrectedData, int thetaDirection)
        {
            try
            {
                int pointCountPerRev = 0;
                int revCount = 0;
                int pointIndex = 0;
                double thetaStart = Math.Abs(uncorrectedData[0].ThetaRad);
                double thetaEnd = Math.Abs(thetaStart + (thetaDirection * pi2));
                var pointList = new CylData(uncorrectedData.FileName);
                var uncorrectedGridData = new CylGridData();

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
                        pointList = new CylData(uncorrectedData.FileName);
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
        static SpiralDataSet BuildSpiralFromRadialData(IProgress<int> progress,SpiralInspScript script, double[] rawInputData,int grooveCount )
        {
            try
            {
               

                var data = RingDataBuilder.GetUncorrectedData(script, rawInputData);
                var dataSet = new SpiralDataSet(script.InputDataFileName);
                var ringData = new RingDataSet(script.InputDataFileName);
                ringData.UncorrectedCylData = RingDataBuilder.GetUncorrectedData(script, rawInputData);
                dataSet.UncorrectedSpiralData = BuildGridList(ringData.UncorrectedCylData,Math.Sign(script.AngleIncrement));
                int totalrings = dataSet.UncorrectedSpiralData.Count;                

                //find all land points
                int i = 1;
                foreach (var row in dataSet.UncorrectedSpiralData)
                {
                   
                    var landPointArr = RingDataBuilder.GetLandPoints(row, script.PointsPerRevolution,grooveCount);
                    //find polynomial fit for lands and correct for eccentricity        
                    
                    var correctedRing = CorrectRing(row, landPointArr, script.ProbeSetup.Direction);
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
        static public InspDataSet BuildDataAsync(CancellationToken ct, IProgress<int> progress, SpiralInspScript script, double[] rawDataSet,int grooveCount)
        {
            try
            {
                //Init(options);
                var sw = new Stopwatch();
                progress.Report(sw.Elapsed.Seconds);              
               
                var dataSet = BuildSpiralFromRadialData(progress,script, rawDataSet, grooveCount);

                return dataSet;
            }
            catch (Exception)
            {

                throw;
            }

        }
       
        public SpiralDataBuilder()
        {

        }
    }
}
