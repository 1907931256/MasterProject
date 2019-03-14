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
       
       
        public DataBuilder()
        {
        }

    }
    
  
}
