using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
using System.Diagnostics;

namespace DataLib
{
    
    public class FitData 
    {
        class SegmentFitData
        {
            public PointCyl StartPoint { get; set; }
            public PointCyl EndPoint { get; set; }
            public Func<double, double> FitFunction { get; set; }
        }

        List<SegmentFitData> dataSegments;
        bool _segmentFit;
        int _polyOrder;
       
        Func<double,double> GetFunc(CylData points)
        {
            try
            {
                double[] x = new double[points.Count];
                double[] y = new double[points.Count];

                for (int i = 0; i < points.Count; i++)
                {
                    x[i] = points[i].ThetaRad;
                    y[i] = points[i].R;
                }
                if (_polyOrder > 1)
                {
                    return MathNet.Numerics.Fit.PolynomialFunc(x, y, _polyOrder);
                  
                }
                else
                {
                    return MathNet.Numerics.Fit.LineFunc(x, y);
                }
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        public CylData CorrectData(CylData points)
        {
            var result = new CylData(points.FileName);
            points.SortByTheta();
            for(int i=0;i<points.Count;i++)
            {
                foreach(SegmentFitData segmentFit in dataSegments)
                {
                    if(points[i].ThetaRad>=segmentFit.StartPoint.ThetaRad && points[i].ThetaRad<segmentFit.EndPoint.ThetaRad)
                    {
                        double r = points[i].R - segmentFit.FitFunction(points[i].ThetaRad) ;
                        var pt = new PointCyl(r, points[i].ThetaRad, points[i].Z);
                        result.Add(pt);
                        break;
                    }
                }
            }
            return result;
        }

        public void CalcFitCoeffs(CylData points)
        {
            try
            {
                
                if (points.Count <= _polyOrder)
                {
                    _polyOrder = points.Count - 1;
                }
                
                if (_segmentFit)
                {
                   for (int i = 0; i < points.Count  - 1 ; i++)
                   {
                        var segment = new CylData(points.FileName);
                        
                        segment.Add(points[i]);
                        segment.Add(points[i + 1]);
                        var func = GetFunc(segment);
                        var segFit = new SegmentFitData()
                        {
                            StartPoint = points[i],
                            EndPoint = points[i+1],
                            FitFunction = func
                        };
                        dataSegments.Add(segFit);
                   }
                }
                else
                {                        
                        var func = GetFunc(points);
                        var segFit = new SegmentFitData()
                        {
                            StartPoint = points[0],
                            EndPoint = points[points.Count-1],
                            FitFunction = func
                        };
                    dataSegments.Add(segFit);
                }
                
            }
            catch (Exception)
            {

                throw;
            }
            
           
        }
       
        public FitData(bool segmentFit, int polyOrder)
        {
            _segmentFit = segmentFit;          
            _polyOrder = polyOrder;
            dataSegments = new List<SegmentFitData>();

        }
    }
    public class DataUtil
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
                transData.Add(new PointCyl(pt.R, pt.ThetaRad - midThetaRad, pt.Z));
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
                transData.Add(new System.Drawing.PointF((float)(pt.X - midX), pt.Y));
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
        static public Vector3 UnrollCylPt(PointCyl pt, double scaling, double unrollingRadius)
        {
            try
            {
                var result = new Vector3(pt.ThetaRad * unrollingRadius, pt.R * scaling, pt.Z, pt.Col);
                return result;
            }

            catch (Exception)
            {

                throw;
            }
        }
        static public Vector3 UnrollCylPt(PointCyl pt, double thetaOffsetRad, double scaling, double unrollingRadius)
        {
            try
            {
                var result = new Vector3((pt.ThetaRad + thetaOffsetRad) * unrollingRadius, pt.R * scaling, pt.Z, pt.Col);
                return result;
            }

            catch (Exception)
            {

                throw;
            }
        }
        static public CartData UnrollCylinderRing(CartData cartData, double scaling, double unrollRadius)
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
                    strip.Add(UnrollCylPt(ptCyl, scaling, unrollRadius));
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
        static public CartGridData UnrollCylinder(CylGridData correctedRingList, double scaling, double unrollRadius)
        {
            try
            {
                var stripList = new CartGridData();
                foreach (var cylstrip in correctedRingList)
                {
                    stripList.Add(UnrollCylinderRing(cylstrip, scaling, unrollRadius));
                }
                return stripList;
            }
            catch (Exception)
            {

                throw;
            }

        }
        static public  CylData SmoothLinear(double minFeatureSize, CylData points)
        {
            var spacing =  points[0].DistanceTo(points[1]);
            var pointSpacing = Math.Round(minFeatureSize / spacing);
            var result = new CylData(points.FileName);
            if(pointSpacing<2.0)
            {
                result = points;
            }
            else
            {
                int startIndex = 0;
                int indexSpacing = (int)pointSpacing;
                int endIndex = indexSpacing;
                
                while(endIndex < points.Count)
                {
                    var x = new List<double>();
                    var y = new List<double>();
                    for(int i= startIndex;i<=endIndex;i++)
                    {
                        x.Add(points[i].ThetaRad);
                        y.Add(points[i].R);
                    }
                    var lineFunc = MathNet.Numerics.Fit.LineFunc(x.ToArray(), y.ToArray());
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        var newR = lineFunc(points[i].ThetaRad);
                        result.Add(new PointCyl(newR, points[i].ThetaRad, points[i].Z));
                    }
                    startIndex = endIndex;
                    endIndex = startIndex + indexSpacing;
                }

            }
            return result;
        }
        static public PointCyl[] SortByIndex(CylData points)
        {
            try
            {
                int i = 0;
                var indexArr = new int[points.Count];
                var pointArr = new PointCyl[points.Count];
                foreach (PointCyl pt in points)
                {
                    indexArr[i] = pt.ID;
                    pointArr[i] = pt.Clone();
                    i++;
                }
                Array.Sort(indexArr, pointArr);
                return pointArr;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public static BoundingBox BuildBoundingBox(List<CylData> dataSetList)
        {

            var bblist = new List<BoundingBox>();
            foreach (CylData set in dataSetList)
            {
                bblist.Add(set.BoundingBox);
            }
            return BoundingBoxBuilder.Union(bblist.ToArray());
        }
        public static BoundingBox GetBB(CartGridData data)
        {
            try
            {
                double maxX = double.MinValue;
                double minX = double.MaxValue;
                double maxY = double.MinValue;
                double minY = double.MaxValue;
                double maxZ = double.MinValue;
                double minZ = double.MaxValue;
                foreach (var strip in data)
                {
                    foreach (var pt in strip)
                    {
                        double x = pt.X;
                        if (x > maxX)
                        {
                            maxX = x;
                        }
                        if (x < minX)
                        {
                            minX = x;
                        }
                        double y = pt.Y;
                        if (y > maxY)
                        {
                            maxY = y;
                        }
                        if (y < minY)
                        {
                            minY = y;
                        }
                        if (pt.Z > maxZ)
                        {
                            maxZ = pt.Z;
                        }
                        if (pt.Z < minZ)
                        {
                            minZ = pt.Z;
                        }
                    }
                }


                var bb = new BoundingBox(minX, minY, minZ, maxX, maxY, maxZ);
                return bb;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static BoundingBox GetBB(CartData data)
        {
            try
            {
                double maxX = double.MinValue;
                double minX = double.MaxValue;
                double maxY = double.MinValue;
                double minY = double.MaxValue;
                double maxZ = double.MinValue;
                double minZ = double.MaxValue;
                foreach (var pt in data)
                {
                    double x = pt.X;
                    if (x > maxX)
                    {
                        maxX = x;
                    }
                    if (x < minX)
                    {
                        minX = x;
                    }
                    double y = pt.Y;
                    if (y > maxY)
                    {
                        maxY = y;
                    }
                    if (y < minY)
                    {
                        minY = y;
                    }
                    if (pt.Z > maxZ)
                    {
                        maxZ = pt.Z;
                    }
                    if (pt.Z < minZ)
                    {
                        minZ = pt.Z;
                    }
                }

                var bb = new BoundingBox(minX, minY, minZ, maxX, maxY, maxZ);
                return bb;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static BoundingBox GetBB(CylGridData data)
        {
            try
            {
                double maxX = double.MinValue;
                double minX = double.MaxValue;
                double maxY = double.MinValue;
                double minY = double.MaxValue;
                double maxZ = double.MinValue;
                double minZ = double.MaxValue;
                foreach (var strip in data)
                {
                    foreach (var pt in strip)
                    {
                        double x = pt.R;
                        if (x > maxX)
                        {
                            maxX = x;
                        }
                        if (x < minX)
                        {
                            minX = x;
                        }
                        double y =  pt.ThetaRad;
                        if (y > maxY)
                        {
                            maxY = y;
                        }
                        if (y < minY)
                        {
                            minY = y;
                        }
                        double z = pt.Z;
                        if (z > maxZ)
                        {
                            maxZ = z;
                        }
                        if (z < minZ)
                        {
                            minZ = z;
                        }
                    }
                }


                var bb = new BoundingBox(minX, minY, minZ, maxX, maxY, maxZ);
                return bb;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static BoundingBox GetBB(CylData data)
        {
            try
            {
                double maxX = double.MinValue;
                double minX = double.MaxValue;
                double maxY = double.MinValue;
                double minY = double.MaxValue;
                double maxZ = double.MinValue;
                double minZ = double.MaxValue;
                foreach (var pt in data)
                {
                    double x = pt.R;
                    if (x > maxX)
                    {
                        maxX = x;
                    }
                    if (x < minX)
                    {
                        minX = x;
                    }
                    double y = pt.ThetaRad;
                    if (y > maxY)
                    {
                        maxY = y;
                    }
                    if (y < minY)
                    {
                        minY = y;
                    }
                    double z = pt.Z;
                    if (z > maxZ)
                    {
                        maxZ = z;
                    }
                    if (z < minZ)
                    {
                        minZ = z;
                    }

                }

                var bb = new BoundingBox(minX, minY, minZ, maxX, maxY, maxZ);
                return bb;
            }
            catch (Exception)
            {

                throw;
            }
        }
             
        static PointCyl[] SortByZ(PointCyl[] points)
        {
            try
            {
                int i = 0;
                var indexArr = new double[points.Length];
                var pointArr = new PointCyl[points.Length];
                foreach (PointCyl pt in points)
                {
                    indexArr[i] = pt.Z;
                    pointArr[i] = pt.Clone();
                    i++;
                }
                Array.Sort(indexArr, pointArr);
                return pointArr;
            }
            catch (Exception)
            {

                throw;
            }

        }
        static PointCyl[] SortByR(PointCyl[] points)
        {
            try
            {
                int i = 0;
                var indexArr = new double[points.Length];
                var pointArr = new PointCyl[points.Length];
                foreach (PointCyl pt in points)
                {
                    indexArr[i] = pt.R;
                    pointArr[i] = pt.Clone();
                    i++;
                }
                Array.Sort(indexArr, pointArr);
                return pointArr;
            }
            catch (Exception)
            {

                throw;
            }

        }
        static  PointCyl[] SortByTheta(PointCyl[] points)
        {
            try
            {
                int i = 0;
                var indexArr = new double[points.Length];
                var pointArr = new PointCyl[points.Length];
                foreach (PointCyl pt in points)
                {
                    indexArr[i] = pt.ThetaRad;
                    pointArr[i] = pt.Clone();
                    i++;
                }
                Array.Sort(indexArr, pointArr);
                return pointArr;
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public static CartData ConvertToCartData(Vector2[] points,double z)
        {
            try
            {
                var cartdata = new CartData("");
                foreach(Vector2 pt in points)
                {
                    cartdata.Add(new Vector3(pt.X, pt.Y, z));
                }
                return cartdata;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
