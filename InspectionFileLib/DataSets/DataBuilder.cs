using System;
using System.Collections.Generic;
using GeometryLib;

using System.Diagnostics;
using System.Threading;

using DataLib;
using BarrelLib;
using ProbeController;
namespace InspectionLib
{
    // barrel specific data utilities


   
    /// <summary>
    /// builds inspection data from raw data and script
    /// </summary>
    public class DataBuilder
    {
        static InspDataSet BuildCartDataFromLineData(SingleCylInspScript script, Vector2[] rawDataSet)
        {
            try
            {

                var cartData = DataUtil.ConvertToCartData(rawDataSet, script.Location.X);
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

        static public InspDataSet BuildDataAsync(CancellationToken ct, IProgress<int> progress, SingleCylInspScript script, Vector2[] rawDataSet)
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
        public static CalDataSet BuildCalData(InspectionScript script, double ringGageDiamInch, string CsvFileName)
        {
            try
            {
                if (script is CylInspScript cylScript)
                {
                    double data = 0;
                    if (cylScript.ProbeSetup.Count == 1)
                    {
                        var singleData = new KeyenceSiDataSet(script, CsvFileName);
                        data = singleData.GetData()[0];
                    }
                    else
                    {
                        var dualData = new KeyenceDualSiDataSet(script, CsvFileName);

                        data = dualData.GetData(ScanFormat.CAL)[0];
                    }
                    return new CalDataSet(ringGageDiamInch, data, cylScript.ProbeSetup.Direction);
                }
                else
                {
                    return new CalDataSet(script.CalDataSet.NominalRadius);
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                throw new Exception("Cal file not found");
            }
            catch (Exception)
            {

                throw;
            }

        }
        static protected PointCyl GetPoint(int i, SpiralInspScript script, double r)
        {
            var z = i / script.PitchInch + script.StartLocation.X;
            var theta = i * script.AngleIncrement + GeomUtilities.ToRadians(script.StartLocation.Adeg);
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
                    points.Add(GetPoint(i, script, data[i]));
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
                double thetaEnd = Math.Abs(thetaStart + (thetaDirection * Math.PI * 2.0));
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
                        thetaEnd = Math.Abs(thetaStart + (thetaDirection * Math.PI * 2.0));
                    }
                }
                return uncorrectedGridData;
            }
            catch (Exception)
            {
                throw;
            }

        }
        static SpiralDataSet BuildSpiralFromRadialData(IProgress<int> progress, SpiralInspScript script, double[] rawInputData, int grooveCount)
        {
            try
            {


                var data = DataBuilder.GetUncorrectedData(script, rawInputData);
                var dataSet = new SpiralDataSet(script.InputDataFileName);
                var ringData = new RingDataSet(script.InputDataFileName);
                ringData.UncorrectedCylData = DataBuilder.GetUncorrectedData(script, rawInputData);
                dataSet.UncorrectedSpiralData = BuildGridList(ringData.UncorrectedCylData, Math.Sign(script.AngleIncrement));
                int totalrings = dataSet.UncorrectedSpiralData.Count;

                //find all land points
                int i = 1;
                foreach (var row in dataSet.UncorrectedSpiralData)
                {

                    var landPointArr = DataBuilder.GetLandPoints(row, script.PointsPerRevolution, grooveCount);
                    //find polynomial fit for lands and correct for eccentricity        

                    var correctedRing = CorrectRing(row, landPointArr, script.ProbeSetup.Direction, script.CalDataSet.NominalRadius);
                    dataSet.SpiralData.Add(correctedRing);
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
        static public InspDataSet BuildDataAsync(CancellationToken ct, IProgress<int> progress, SpiralInspScript script, double[] rawDataSet, int grooveCount)
        {
            try
            {
                //Init(options);
                var sw = new Stopwatch();
                progress.Report(sw.Elapsed.Seconds);

                var dataSet = BuildSpiralFromRadialData(progress, script, rawDataSet, grooveCount);

                return dataSet;
            }
            catch (Exception)
            {

                throw;
            }

        }

        static InspDataSet BuildRasterPoints(RasterInspScript script, double[] data)
        {
            try
            {
                var points = new CylData(script.InputDataFileName);
                double theta = script.StartLocation.Adeg;
                double z = script.StartLocation.X;
                double r = 0;
                double nextZ = script.StartLocation.X + script.AxialIncrement;
                double direction = 1;
                var dataSet = new CylDataSet(script.InputDataFileName);
                for (int i = 0; i < data.Length; i++)
                {

                    theta = i * script.AngleIncrement + script.StartLocation.Adeg; ;

                    if (theta >= script.EndLocation.Adeg && z < nextZ)
                    {
                        direction = -1;
                        z = i * script.AxialIncrement + script.StartLocation.X;

                    }
                    if (theta <= script.EndLocation.Adeg && z < nextZ)
                    {
                        direction = 1;
                        z = i * script.AxialIncrement + script.StartLocation.X;

                    }
                    if (theta < script.EndLocation.Adeg && theta > script.StartLocation.Adeg)
                    {
                        if (direction > 0)
                            theta = direction * i * script.AngleIncrement + script.StartLocation.Adeg;
                        if (direction < 0)
                            theta = direction * i * script.AngleIncrement + script.EndLocation.Adeg;
                        if (z >= nextZ)
                        {
                            nextZ += script.AxialIncrement;
                        }
                    }

                    r = data[i];
                    var pt = new PointCyl(r, GeomUtilities.ToRadians(theta), z, i);
                    dataSet.CylData.Add(pt);
                }
                return dataSet;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static InspDataSet BuildDataAsync(CancellationToken ct, IProgress<int> progress, RasterInspScript script, double[] rawDataSet, DataOutputOptions options)
        {
            try
            {
                //Init(options);
                var sw = new Stopwatch();
                progress.Report(sw.Elapsed.Seconds);

                var dataSet = BuildRasterPoints(script, rawDataSet);

                return dataSet;
            }
            catch (Exception)
            {

                throw;
            }

        }
        //protected Barrel _barrel;

        static public string InputFileName
        {
            get
            {
                return _inputFileName;
            }
        }
        
        static protected string _inputFileName;
        //long _id;
        protected BoundingBox _boundingBox;

        //static protected virtual CylData GetData(CylInspScript script, double[] data)
        //{
        //   return new CylData(script.InputDataFileName);
        //}
        //static protected CylData GetUncorrectedData(CylInspScript script, double[] rawInputData)
        //{
        //    try
        //    {               
        //        return GetData(script, rawInputData);              
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        static double GetMinRadiusFromPointList(CylData singleRing, PointCyl[] landPoints)
        {
            try
            {
                var pointDictionary = new Dictionary<int, PointCyl>();
                
                var foundPoints = new CylData(singleRing.FileName);

                
                foreach (PointCyl pt in singleRing)
                {
                    if(!pointDictionary.ContainsKey(pt.ID))
                        pointDictionary.Add(pt.ID, pt);
                }
                foreach (PointCyl landpt in landPoints)
                {
                    var pt = new PointCyl();
                    if (pointDictionary.TryGetValue(landpt.ID, out pt))
                    {
                        foundPoints.Add(pt);
                    }
                }
                double rSum = 0;
                double rAve = 0;
               
                if (foundPoints.Count > 0)
                {
                    foreach (PointCyl pt in foundPoints)
                    {
                        if (!(pt is null))
                        {
                            rSum += pt.R;
                            
                        }
                    }
                    rAve = rSum / foundPoints.Count;
                }

                return rAve;
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        /// <summary>
        /// find min radius and reset radii to nominal 
        /// </summary>
        /// <param name="singleRing"></param>
        /// <param name="setPt"></param>
        /// <param name="knownRadius"></param>
        /// <returns></returns>
        static public CylData ResetToKnownRadius(CylData singleRing, PointCyl setPt,  double knownRadius)
        {
            try
            {                
                var result = new CylData(singleRing.FileName);
                double rCorrection = knownRadius - setPt.R;
                
                foreach (var pt in singleRing)
                {
                    PointCyl newPt;                   
                    newPt = new PointCyl(pt.R + rCorrection, pt.ThetaRad, pt.Z);                    

                    result.Add(newPt);
                }
                result.NominalMinDiam = singleRing.NominalMinDiam;
                return result;
            }
            catch (Exception)
            {
                
                throw;
            }

        }
        static protected double GetCorrectionRadius(CylData singleRing, double nominalRadius)
        {
            double minR = GetMinAveRadius(singleRing, 5);
            double rCorrection = nominalRadius - minR;
            return rCorrection;
        }
        /// <summary>
        /// find min radius and reset radii to nominal 
        /// </summary>
        /// <param name="singleRing"></param>
        /// <param name="_nominalRadius"></param>
        /// <returns></returns>
        static protected CylData CorrectRadius(CylData singleRing,double rCorrection, ProbeDirection probeDir)
        {
            try
            {
                
               
                var result = new CylData(singleRing.FileName);
                
                foreach (var pt in singleRing)
                {
                    PointCyl newPt;
                    double r = 0;
                    if (probeDir == ProbeDirection.ID)
                    {
                        r = pt.R+rCorrection ;
                    }
                    else
                    {
                        r = rCorrection - pt.R;
                    }
                    newPt = new PointCyl(r, pt.ThetaRad, pt.Z, pt.ID);
                    result.Add(newPt);

                }
                result.NominalMinDiam = singleRing.NominalMinDiam;
                return result;
            }
            catch (Exception)
            {
                
                throw;
            }

        }


        //}
        /// <summary>
        /// find min radius point with rolling average
        /// /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        static protected double GetMinAveRadius(List<PointCyl> data,int searchWindowHalfW)
        {
            try
            {
               
                double minR = double.MaxValue;
                int minPtIndex = 0;
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].R < minR)
                    {
                        minPtIndex = i;
                        minR = data[i].R;
                    }
                }
                int startSearch = Math.Max(0, minPtIndex - searchWindowHalfW);
                int endSearch = Math.Min(data.Count - 1, minPtIndex + searchWindowHalfW);
                double aveCt = 0;
                double sumR=0;
                for(int j=startSearch; j<endSearch;j++)
                {
                    sumR += data[j].R;
                    aveCt++;
                }
                sumR /= aveCt;
                return sumR;
            }
            catch (Exception)
            {
             
                throw;
            }
            
        }
        //}
        /// <summary>
        /// find min radius point with rolling average
        /// /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        static protected PointCyl GetMinRadiusPt(List<PointCyl> data)
        {
            try
            {
                int rollingAveWidth = 5;
                double aveCt = rollingAveWidth * 2.0;
                double minR = double.MaxValue;
                int minPtIndex = 0;
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].R < minR)
                    {
                        minPtIndex = i;
                        minR = data[i].R;
                    }
                }
                return data[minPtIndex];
            }
            catch (Exception)
            {
                
                throw;
            }

        }

        /// <summary>
        /// get land points from data and assume minimum radius found is land
        /// </summary>
        /// <param name="rawSingleRing"></param>
        /// <returns></returns>
        static protected CylData GetLandPoints(CylData ring,int pointsPerRevolution,int grooveCount)
        {
            try
            {

                var outputList = new CylData(ring.FileName);
                double thMax = Math.Max(ring[0].ThetaRad, ring[ring.Count - 1].ThetaRad);
                double thMin = Math.Min(ring[0].ThetaRad, ring[ring.Count - 1].ThetaRad);
               
                var minPt = GetMinRadiusPt(ring);
               
               
                int searchHalfWindow = (int)((pointsPerRevolution / grooveCount) / 2.0);
             

                double dth = Math.PI*2.0 / grooveCount;
                var landLocations = new List<double>();

                landLocations.Add(minPt.ThetaRad);
                for (int i = 1; i < grooveCount; i++)
                {
                    double land = (minPt.ThetaRad + (i * dth));// % Math.PI*2.0;
                   

                    if(land <= thMax && land >= thMin)
                    {
                        landLocations.Add(land);
                        if (Math.Abs(land % (Math.PI*2.0)) < .02)
                        {
                            landLocations.Add(0.0);
                        }
                    }
                    land = (minPt.ThetaRad - (i * dth));
                    if (land <= thMax && land >= thMin)
                    {
                        landLocations.Add(land);
                    }
                    
                }
                landLocations.Sort();
                bool useLocalMinPt = false;
                for (int i = 0; i < landLocations.Count; i++)
                {
                    if(useLocalMinPt)
                    {
                        for (int j = 0; j < ring.Count - 1; j++)
                        {
                            var p1 = ring[j].ThetaRad;
                            var p2 = ring[j + 1].ThetaRad;
                            if (landLocations[i] < p2 && landLocations[i] >= p1)
                            {
                                int searchStart = Math.Max(0, j - searchHalfWindow);
                                int searchEnd = Math.Min(ring.Count, j + searchHalfWindow);
                                
                                double minR = double.MaxValue;
                                PointCyl localMinPt = new PointCyl();
                                //find local min
                                var fit = new FitData(false,  2);
                                //var segment = new SegmentFitData();
                                
                                for (int k = searchStart; k < searchEnd; k++)
                                {

                                    if (ring[k].R < minR)
                                    {
                                        minR = ring[k].R;
                                        localMinPt = ring[k].Clone();

                                    }
                                }
                                outputList.Add(localMinPt);
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < ring.Count - 1; j++)
                        {
                           
                            if (landLocations[i] < ring[j+1].ThetaRad && landLocations[i] >= ring[j].ThetaRad)
                            {
                                double r = (ring[j].R + ring[j + 1].R) / 2.0;
                                double th = (ring[j].ThetaRad + ring[j + 1].ThetaRad) / 2.0;
                                double z = (ring[j].Z + ring[j + 1].Z) / 2.0;
                                outputList.Add(new PointCyl(r, th, z));
                            }
                        }
                    }
                   
                }
                outputList.SortByIndex();
                return outputList ; 
               
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }
        static protected void AddSelfPoints(ref CylData lands, CylData pointArr)
        {
            try
            {
                double th = pointArr[pointArr.Count - 1].ThetaRad - Math.PI*2.0;
                lands.Add(new PointCyl(pointArr[pointArr.Count - 1].R, th, pointArr[pointArr.Count - 1].Z, pointArr[pointArr.Count - 1].ID));

                th = pointArr[0].ThetaRad + Math.PI*2.0;
                lands.Add(new PointCyl(pointArr[0].R, th, pointArr[0].Z, pointArr[0].ID));
            }
            catch (Exception)
            {

                throw;
            } 
        }
        static protected CylData CorrectRing(CylData singleRing, CylData landPointArr, ProbeDirection probeDirection,double nominalRadius)
        {
            try
            {
                int polyOrder = 1;
                bool segmentFit = true;

                var coeffList = new List<double[]>();
                var landPointList = new CylData(singleRing.FileName);

                landPointList.AddRange(landPointArr);

                AddSelfPoints(ref landPointList, landPointArr);
                landPointList.SortByTheta();
               
                var fitData = new FitData(segmentFit, polyOrder);

                fitData.CalcFitCoeffs(landPointList);
                var eccData = fitData.CorrectData(singleRing);

                eccData.SortByTheta();               
                eccData.NominalMinDiam = singleRing.NominalMinDiam;

                double rCorrection =  GetCorrectionRadius(eccData,nominalRadius) ;
                var correctedData = CorrectRadius(eccData,rCorrection, probeDirection);
                return correctedData;
            }
            catch (Exception)
            {
                throw;
            }
        }

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
                if (script is CylInspScript ringScript)
                {
                    var points = new CylData(ringScript.InputDataFileName);
                    double probeSpacing = ringScript.CalDataSet.ProbeSpacingInch;
                    int pointCt = Math.Min(ringScript.PointsPerRevolution, data.GetUpperBound(0));
                    var probeSign = script.ProbeSetup[0].DirectionSign;
                    double minDiam = double.MaxValue;
                    for (int i = 0; i < pointCt; i++)
                    {
                        var z = script.StartLocation.X;
                        var theta = i * ringScript.AngleIncrement + GeomUtilities.ToRadians(ringScript.StartLocation.Adeg);

                        var diam = data[i];
                        var sum = data[i];
                        if (sum < minDiam)
                        {
                            minDiam = sum;
                        }
                        double r = probeSign * (probeSpacing + sum) / 2.0;
                        var pt1 = new PointCyl(r, theta, z, i);

                        points.Add(pt1);

                    }
                    points.NominalMinDiam = minDiam + ringScript.CalDataSet.ProbeSpacingInch;
                    // var trans = new Vector3(0, -1 * script.CalDataSet.NominalRadius, 0);
                    // points.Translate(trans);
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
        static InspDataSet BuildRingFromRadialData(CylInspScript script, double[] rawInputData, int grooveCount)
        {
            try
            {
                var dataSet = new RingDataSet(script.InputDataFileName);
                dataSet.UncorrectedCylData = GetUncorrectedData(script, rawInputData);
                dataSet.RawLandPoints = GetLandPoints(dataSet.UncorrectedCylData, script.PointsPerRevolution, grooveCount);
                dataSet.CylData = CorrectRing(dataSet.UncorrectedCylData, dataSet.RawLandPoints, script.ProbeSetup.Direction, script.CalDataSet.NominalRadius);
                dataSet.CorrectedLandPoints = GetLandPoints(dataSet.CylData, script.PointsPerRevolution, grooveCount);
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
        static public InspDataSet BuildDataAsync(CancellationToken ct, IProgress<int> progress, CylInspScript script, double[] rawDataSet, int grooveCount)
        {
            try
            {
                //Init(options);
                _inputFileName = script.InputDataFileName;
                var dataSet = BuildRingFromRadialData(script, rawDataSet, grooveCount);
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataBuilder()
        {
        }

    }
    
  
}
