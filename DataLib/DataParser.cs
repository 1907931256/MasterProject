﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
using System.Diagnostics;
using FileIOLib;
using DataLib;
namespace DataLib
{
    /// <summary>
    /// gets various subsets of data 
    /// </summary>
    public class DataParser
    {
        /// <summary>
        /// get first full ring from multi ring data set
        /// </summary>
        /// <param name="passIndex"></param>
        /// <param name="ring"></param>
        /// <returns></returns>
        static public CylData GetFirstRingFromMultiPassRing(int passIndex, CylData ring)
        {
            try
            {
                double startTh = ring[0].ThetaRad;
                double endTh = startTh + (Math.PI * 2.0);
                var result = new CylData(ring.FileName);
                foreach (PointCyl bp in ring)
                {
                    if (bp.ThetaRad >= startTh && bp.ThetaRad <= endTh)
                    {
                        result.Add(bp);
                    }
                }
                result.NominalMinDiam = ring.NominalMinDiam;
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// get single groove from ring data set
        /// </summary>
        /// <param name="grooveNumber"></param>
        /// <param name="ring"></param>
        /// <returns></returns>
        //static public CylData GetGrooveFromRing(Barrel barrel, int grooveNumber, CylData ring)
        //{
        //    try
        //    {
        //        double z = (ring[0].Z + ring[ring.Count - 1].Z) / 2.0;


        //        var groove = new CylData();
        //        foreach (PointCyl bp in ring)
        //        {
        //            int gn = barrel.GetGrooveNumber(bp.Z, bp.ThetaRad);
        //            if (gn == grooveNumber)
        //            {
        //                groove.Add(bp);
        //            }

        //        }
        //        return groove;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
        /// <summary>
        /// get section from ring data set
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="ring"></param>
        /// <returns></returns>
        static public CylData GetSectionFromRing(int startIndex, int endIndex, CylData ring)
        {
            try
            {
                double z = (ring[0].Z + ring[ring.Count - 1].Z) / 2.0;


                var groove = new CylData(ring.FileName);
                foreach (PointCyl bp in ring)
                {
                    if (bp.ID >= startIndex && bp.ID < endIndex)
                    {
                        groove.Add(bp);
                    }

                }
                return groove;
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// get first ring from spiral data set
        /// </summary>
        /// <param name="z"></param>
        /// <param name="spiralMap"></param>
        /// <param name="pointsPerRev"></param>
        /// <returns></returns>
        //static public CylData GetRingFromSpiralMap( double z, CylData spiralMap,int pointsPerRev)
        //{
        //    try
        //    {
        //        var result = new CylData();
        //        int startIndex = 0;
        //        for (int i = 0; i < spiralMap.Count - 1; i++)
        //        {
        //            double zhi = Math.Max(spiralMap[i].Z, spiralMap[i + 1].Z);
        //            double zlow = Math.Min(spiralMap[i].Z, spiralMap[i + 1].Z);
        //            if (z > zlow && z <= zhi)
        //            {
        //                startIndex = i;
        //                break;
        //            }
        //        }
        //        int endIndex = Math.Min(spiralMap.Count - 1, startIndex + pointsPerRev);
        //        for (int j = startIndex; j < endIndex; j++)
        //        {
        //            result.Add(spiralMap[j]);
        //        }
        //        return result;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
        /// <summary>
        /// get single groove from spiral data set
        /// </summary>
        /// <param name="grooveNumber"></param>
        /// <param name="z"></param>
        /// <param name="spiralMap"></param>
        /// <param name="pointsPerRev"></param>
        /// <returns></returns>
        //static public  CylData GetGrooveFromSpiralMap(Barrel barrel, int grooveNumber,double z, CylData spiralMap,int pointsPerRev)
        //{
        //    try
        //    {
        //        CylData ring = GetRingFromSpiralMap(z, spiralMap, pointsPerRev);
        //        CylData groove = GetGrooveFromRing(barrel, grooveNumber, ring);
        //        return groove;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
        /// <summary>
        /// build error map from unrolled grid data
        /// </summary>
        /// <param name="bpGrid"></param>
        /// <param name="colorCodeOption"></param>
        /// <returns></returns>
        //static public CartGridData BuildErrorMap(Barrel barrel, DataOutputOptions options, CylGridData bpGrid, ColorCodeOptions colorCodeOption)
        //{
        //    var cc = new DataColorCode();
        //    DataColorCode.ColorCodeData(barrel, options, bpGrid, colorCodeOption);
        //    var errorMap = new CartGridData();
        //    foreach (var strip in bpGrid)
        //    {
        //        var vstrip = new CartData();
        //        foreach (var bp in strip)
        //        {
        //            Vector3 v = UnrollCylPt(bp, strip[0].ThetaRad, options.ErrorMapScaleFactor, barrel.DimensionData.LandNominalDiam / 2.0);
        //            double radiusError = barrel.g
        //            vstrip.Add(new Vector3(v.X, v.Y, bp.RadiusError * options.ErrorMapScaleFactor.Z));
        //        }
        //        errorMap.Add(vstrip);
        //    }
        //    return errorMap;
        //}
        /// <summary>
        /// build error map histogram from data set
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="strip"></param>
        //static public Histogram BuildRadialErrorHistogram(int binCount, CylData strip,bool normalize)
        //{
        //    var errors = new Histogram();

        //    errors.BuildHistogram(binCount, strip, normalize);
        //    foreach (var bp in strip)
        //    {
        //        errors.InputValues.Add(bp.RadiusError);

        //        errors.PointCount++;
        //        if (bp.GrooveNumber >= 0)
        //        {
        //            errors.GroovePointCount++;
        //            if (!bp.InTolerance)
        //                errors.GrooveErrorPointCount++;
        //        }
        //        else
        //        {
        //            errors.LandPointCount++;
        //            if (!bp.InTolerance)
        //                errors.LandErrorPointCount++;
        //        }
        //    }

        //}
        /// <summary>
        /// unroll cylinder point to cartesian
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="startTheta"></param>
        /// <param name="scaling"></param>
        /// <param name="unrollingRadius"></param>
        /// <returns></returns>
    }
    public class DataConverter
    {
        public static CylData CenterToThetaRadMidpoint(CylData data)
        {
            int midCount = (int)Math.Round(data.Count / 2.0);
            var minMax = data.GetMinMaxR();
            var midRData = (minMax.Item1 + minMax.Item2) / 2.0;
            double x1 = 0;
            for (int i = 1; i < midCount; i++)
            {
                if ((data[i - 1].R < midRData && data[i].R > midRData)
                    || (data[i - 1].R > midRData && data[i].R < midRData))
                {
                    x1 = (data[i - 1].ThetaRad + data[i].ThetaRad) / 2.0;
                    break;
                }
            }
            double x2 = 0;
            for (int i = midCount; i < data.Count; i++)
            {
                if ((data[i - 1].R < midRData && data[i].R > midRData)
                    || (data[i - 1].R > midRData && data[i].R < midRData))
                {
                    x2 = (data[i - 1].ThetaRad + data[i].ThetaRad) / 2.0;
                    break;
                }
            }
            double midThetaRad = (x1 + x2) / 2.0;
            var transData = new CylData(data.FileName);
            foreach (var pt in data)
            {
                transData.Add(new PointCyl(pt.R,pt.ThetaRad - midThetaRad,pt.Z));
            }
            return transData;
        }
        public static DisplayData CenterToXMidpoint(DisplayData data)
        {
            int midCount = (int)Math.Round(data.Count / 2.0);
            var minMax = data.GetMinMaxY();
            var midYData = (minMax.Item1 + minMax.Item2) / 2.0;
            double x1 = 0;
            for (int i = 1; i < midCount; i++)
            {
                if ((data[i - 1].Y < midYData && data[i].Y > midYData)
                    || (data[i - 1].Y > midYData && data[i].Y < midYData))
                {
                    x1 = (data[i - 1].X + data[i].X) / 2.0;
                    break;
                }
            }
            double x2 = 0;
            for (int i = midCount; i < data.Count; i++)
            {
                if ((data[i - 1].Y < midYData && data[i].Y > midYData)
                    || (data[i - 1].Y > midYData && data[i].Y < midYData))
                {
                    x2 = (data[i - 1].X + data[i].X) / 2.0;
                    break;
                }
            }
            double midX = (x1 + x2) / 2.0;
            var transData = new DisplayData(data.FileName);
            foreach (var pt in data)
            {
                transData.Add(new System.Drawing.PointF((float)(pt.X - midX), pt.Y ));
            }
            return transData;
        }
        public static CylData AsCylData(CartData cartData)
        {
            try
            {
                var strip = new CylData(cartData.FileName);
                var thetaStart = new PointCyl(cartData[0]).ThetaRad;
                foreach (var pt in cartData)
                {
                    var ptCyl = new PointCyl(pt);
                    strip.Add(ptCyl);
                }
                return strip;
            }
            catch (Exception)
            {

                throw;
            }
        }
        static public Vector3 UnrollCylPt(PointCyl pt, double scaling,  double unrollingRadius)
        {
            try
            {
                var result = new Vector3(pt.ThetaRad  * unrollingRadius , pt.R * scaling , pt.Z  , pt.Col);
                return result;
            }

            catch (Exception)
            {
                
                throw;
            }
        }
        static public Vector3 UnrollCylPt(PointCyl pt,double thetaOffsetRad, double scaling, double unrollingRadius)
        {
            try
            {
                var result = new Vector3((pt.ThetaRad+thetaOffsetRad )* unrollingRadius, pt.R * scaling, pt.Z, pt.Col);
                return result;
            }

            catch (Exception)
            {

                throw;
            }
        }
        static public CartData UnrollCylinderRing(CartData cartData,double scaling,  double unrollRadius)
        {
            try
            {
                var strip = new CartData(cartData.FileName);
                var thetaStart = new PointCyl(cartData[0]).ThetaRad;
                double thetaoffset = Math.PI / 2.0;
                foreach (var pt in cartData)
                {
                    var ptCyl = new PointCyl(pt);
                    
                    strip.Add(UnrollCylPt(ptCyl, thetaoffset, scaling, unrollRadius));
                }
                return strip;
            }
            catch (Exception)
            {

                throw;
            }

        }
        static public CartData UnrollCylinderRing(CylData correctedRing, double scaling, double unrollRadius)
        {
            try
            {
                var strip = new CartData(correctedRing.FileName);
                foreach (var ptCyl in correctedRing)
                {
                    strip.Add(UnrollCylPt(ptCyl,  scaling, unrollRadius));                   
                }
                return strip;
            }
            catch (Exception)
            {

                throw;
            }
                        
        }
            /// <summary>
            /// unroll cylinder into grid
            /// </summary>
            /// <param name="correctedRingList"></param>
            /// <param name="scaling"></param>
            /// <param name="unrollRadius"></param>
            /// <returns></returns>
        static public CartGridData UnrollCylinder( CylGridData correctedRingList, double scaling, double unrollRadius)
        {
            try
            {
                var stripList = new CartGridData();
                foreach (var cylstrip in correctedRingList)
                {                   
                    stripList.Add(UnrollCylinderRing(cylstrip,scaling,unrollRadius));
                }
                return stripList;
            }
            catch (Exception)
            {

                throw;
            }

        }
        

    }
   
    public class Histogram
    {
        int _pointCount;
        
        
        internal double[] LimitBins;
        internal double[] ValueBins;
        int _binCount;
        internal double min = double.MaxValue;
        internal double max = double.MinValue;
        internal Histogram()
        {
            
        }

        internal void BuildHistogram(int binCount,List<double> inputValues,bool normalize)
        {
            _binCount = binCount;
            //find min and max error
            
            foreach (var e in inputValues)
            {
                if (e < min)
                    min = e;
                if (e > max)
                    max = e;
            }

            double range = max - min;
            double binSize = range / _binCount;
            LimitBins = new double[_binCount];
            ValueBins = new double[_binCount];

            //get bins limits
            for (int i = 0; i < _binCount; i++)
            {
                LimitBins[i] = min + (i * binSize);
            }
            //fill bins
            foreach (var e in inputValues)
            {
                for (int i = 0; i < _binCount - 1; i++)
                {
                    if (e >= LimitBins[i] && e < LimitBins[i + 1])
                    {
                        ValueBins[i]++;
                        _pointCount++;
                        break;
                    }
                }
            }
            if(normalize && _pointCount>0)
            {
                for (int i = 0; i < ValueBins.Length; i++)
                {
                    ValueBins[i] /= _pointCount;
                }
            }
           
        }

    }
}
