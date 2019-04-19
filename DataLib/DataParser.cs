using System;
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
                result.MinRadius = ring.MinRadius;
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
