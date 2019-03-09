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


        static public CylData GetUncorrectedData(CylInspScript script, double[] rawInputData)
        {
            try
            {
                return GetData(script, rawInputData);
            }
            catch (Exception)
            {

                throw;
            }
        }
        static protected CylData GetData(CylInspScript script, double[] data)
        {
            try
            {
                if(script is CylInspScript ringScript)
                {
                    var points = new CylData(ringScript.InputDataFileName);
                    double probeSpacing = ringScript.CalDataSet.ProbeSpacingInch;
                    int pointCt = Math.Min(ringScript.PointsPerRevolution, data.GetUpperBound(0));

                    double minDiam = double.MaxValue;
                    for (int i = 0; i < pointCt; i++)
                    {
                        var z = script.StartLocation.X;
                        var theta =  i * ringScript.AngleIncrement + GeomUtilities.ToRadians(ringScript.StartLocation.Adeg);

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
        static InspDataSet BuildRingFromRadialData(CylInspScript script, double[] rawInputData,int grooveCount)
        {
            try
            {
                var dataSet = new RingDataSet(script.InputDataFileName);
                dataSet.UncorrectedCylData = GetUncorrectedData(script, rawInputData);
                dataSet.RawLandPoints = GetLandPoints(dataSet.UncorrectedCylData, script.PointsPerRevolution,grooveCount);
                dataSet.CorrectedCylData = CorrectRing(dataSet.UncorrectedCylData, dataSet.RawLandPoints, script.ProbeSetup.Direction);
                dataSet.CorrectedLandPoints = GetLandPoints(dataSet.CorrectedCylData, script.PointsPerRevolution,grooveCount);
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
        static public InspDataSet BuildDataAsync(CancellationToken ct, IProgress<int> progress, CylInspScript script, double[] rawDataSet,int grooveCount )
        {
            try
            {
                //Init(options);
                _inputFileName = script.InputDataFileName;
                var dataSet = BuildRingFromRadialData(script, rawDataSet,grooveCount);               
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
      
        public RingDataBuilder()
        {

        }
    }
}
