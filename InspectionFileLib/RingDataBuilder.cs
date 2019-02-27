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

        
      
        override protected CylData GetData(CylInspScript script, double[] data)
        {
            try
            {
                if(script is RingInspScript ringScript)
                {
                    var points = new CylData(ringScript.InputDataFileName);
                    double probeSpacing = ringScript.CalDataSet.ProbeSpacingInch;
                    int pointCt = Math.Min(ringScript.PointsPerRevolution, data.GetUpperBound(0));

                    double minDiam = double.MaxValue;
                    for (int i = 0; i < pointCt; i++)
                    {
                        var z = script.StartLocation.X;
                        var theta = script.ThetaDir * i * ringScript.AngleIncrement + Geometry.ToRadians(ringScript.StartLocation.Adeg);

                        var diam = data[i];
                        var sum = data[i];
                        if (sum < minDiam)
                        {
                            minDiam = sum;
                        }
                        var pt1 = new PointCyl((probeSpacing + sum) / 2.0, theta, z, i);

                        points.Add(pt1);

                    }
                    points.NominalMinDiam = minDiam + ringScript.CalDataSet.ProbeSpacingInch;
                    return points;
                }
                else
                {
                    return new CylData("");
                }
                
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
        InspDataSet BuildRingFromRadialData(RingInspScript script, double[] rawInputData)
        {
            try
            {
                var dataSet = new RingDataSet(_barrel,script.InputDataFileName);
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
        /// build ring data from raw and auto find lands
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="progress"></param>
        /// <param name="script"></param>
        /// <param name="rawDataSet"></param>
        /// <param name="options"></param>
        public InspDataSet BuildRingAsync(CancellationToken ct, IProgress<int> progress, RingInspScript script, double[] rawDataSet, DataOutputOptions options)
        {
            try
            {
                Init(options);
                _inputFileName = script.InputDataFileName;
                var dataSet = BuildRingFromRadialData(script, rawDataSet);               
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
