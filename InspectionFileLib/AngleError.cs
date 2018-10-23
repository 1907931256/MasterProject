using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
using DataLib;
using System.Diagnostics;
using BarrelLib;
namespace InspectionLib
{
    public class AngleError
    {
        public double AveRadius { get; private set; }
        public double CorrectionAngle { get; private set; }

       
        Barrel _barrel;
     
        public AngleError(Barrel barrel)
        {
            _barrel = barrel;
            _grooveSpacingRad = Math.PI * 2.0 / _barrel.DimensionData.GrooveCount;
        }


        double[] GetNomMidpointThetas()
        {
            try
            {
               
                double[] ths = new double[_barrel.DimensionData.GrooveCount];
                //generate nominal theta for mid points
                for (int k = 0; k < _barrel.DimensionData.GrooveCount; k++)
                {
                    ths[k] = _barrel.DimensionData.FirstGrooveThetaOffset + (k * _grooveSpacingRad);
                }
                return ths;
            }
            catch (Exception)
            {

                throw;
            }

        }
        void CalcAveGrooveAngleError(double[] noms, double[] actuals)
        {
            try
            {
                CorrectionAngle = 0;
                var errors = new List<double>();
                int minLen = Math.Min(noms.Length, actuals.Length);                
                for(int j=0; j<minLen;j++)
                {
                    errors.Add(actuals[j] - noms[j]);

                }
                CorrectionAngle = errors.Average();
               
            }
            catch (Exception)
            {

                throw;
            }
        }
        List<int> GetIntersectionIndexList(CylData points, List<PointCyl> intersectionsList)
        {
            try
            {
                var indexList = new List<int>();
                foreach (PointCyl pt in intersectionsList)
                {
                    indexList.Add(GetIndexofIntersectionAt(pt.ThetaRad, points));
                }
                return indexList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        

        void CalcAveAngleError(CylData points, List<int> intersectionsList)
        {
            try
            {
               
                var nomMidPointThetas = GetNomMidpointThetas();
                intersectionsList.Sort();
                var midpointThetas = new List<double>();
                var pt0 = new PointCyl();
                var pt1 = new PointCyl();
                var ptMid = new PointCyl();
                double rAve = 0;
                double thetaMid = 0;
                for(int i=0;i<intersectionsList.Count-1;i++)
                {
                    pt0 = points[intersectionsList[i]];
                    pt1 = points[intersectionsList[i + 1]];
                    rAve = (pt0.R + pt1.R) / 2.0;
                    thetaMid = (pt0.ThetaRad + pt1.ThetaRad) / 2.0;
                    ptMid = GetIntersectionAt(thetaMid, points);
                    int midIndex = GetIndexofIntersectionAt(thetaMid, points);
                    if(ptMid.R > rAve)
                    {
                        midpointThetas.Add(thetaMid);
                    }                   
                }
                pt0 = points[intersectionsList[intersectionsList.Count - 1]];
                pt1 = points[intersectionsList[0]];
                rAve = (pt0.R + pt1.R) / 2.0;
                thetaMid = (pt0.ThetaRad + pt1.ThetaRad) / 2.0;
                ptMid = GetIntersectionAt(thetaMid, points);
                if (ptMid.R > rAve)
                {
                    midpointThetas.Add(thetaMid);
                }
                if(midpointThetas.Count>=1)
                {
                     CalcAveGrooveAngleError(nomMidPointThetas, midpointThetas.ToArray());
                }
                else
                {
                    throw new Exception("Unable to find groove intersections for angle correction. Correct Manually");
                }             
                
            }
            catch (Exception)
            {

                throw;
            }
        }
    
        /// <summary>
        /// get average angle of midpoint of grooves relative to nominal
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        void CalcAveAngleError(CylData points)
        {
            try
            {
                var stats = GetMinMaxAveR(points);                
                AveRadius = stats.Item3;                
                //get intersections of data set with average radius circle
                var intersectionsList = GetIntersectionsAt(AveRadius, points);
                 CalcAveAngleError(points, intersectionsList);
               
            }
            catch (Exception)
            {

                throw;
            }

        }

    

        /// <summary>
        /// get intersections points at given radius
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        List<int> GetIntersectionsAt(double radius, List<PointCyl> points)
        {
            try
            {
                var indexList = new List<int>();
                for (int i = 0; i < points.Count - 1; i++)
                {
                    if ((radius >= points[i].R && radius < points[i + 1].R ) 
                        ||(radius<points[i].R && radius >= points[i+1].R))
                    {                                            
                        indexList.Add(i);                       
                    }
                }
                return indexList; 
            }
            catch (Exception)
            {

                throw;
            }

        }
        int GetIndexofIntersectionAt(double thetaRad, List<PointCyl> points)
        {
            try
            {
                int index = 0;
                for (int i = 0; i < points.Count - 1; i++)
                {
                    if (thetaRad >= points[i].ThetaRad  && thetaRad < points[i + 1].ThetaRad  ) 
                    {
                        index = i;
                        break;
                    }
                }
                return index;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// get intersection point at given theta
        /// </summary>
        /// <param name="thetaRad"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        PointCyl GetIntersectionAt(double thetaRad, List<PointCyl> points)
        {
            try
            {
                PointCyl pt = new PointCyl(); ;
                for (int i = 0; i < points.Count - 1; i++)
                {
                    if (thetaRad>=points[i].ThetaRad   && thetaRad<points[i + 1].ThetaRad ) 
                    {
                        double dr = (points[i + 1].R - points[i].R);
                        double dz = points[i + 1].Z - points[i].Z;
                        double dth = points[i + 1].ThetaRad - points[i].ThetaRad;
                        double rMin = Math.Min(points[i + 1].R, points[i].R);
                        double thMin = Math.Min(points[i + 1].ThetaRad, points[i].ThetaRad);
                        double zMin = Math.Min(points[i + 1].Z, points[i].Z);
                        if (dth == 0)
                        {

                            pt = new PointCyl(points[i].R, thetaRad, points[i].Z);
                        }
                        else
                        {

                            double thInc = (thetaRad - thMin) / dth;
                            double r = dr * thInc + rMin;
                            double z = dz * thInc + zMin;
                            pt = new PointCyl(r, thetaRad, z);
                        }

                        break;
                    }
                }
                return pt;
            }
            catch (Exception)
            {

                throw;
            }

        }

        double _grooveSpacingRad;
        Tuple<double,double,double> GetMinMaxAveR(CylData points)
        {
            try
            {
                double maxR = double.MinValue;
                double minR = double.MaxValue;   
                foreach (PointCyl pt in points)
                {
                    if (pt.R < minR)
                        minR = pt.R;
                    if (pt.R > maxR)
                        maxR = pt.R;
                }     
                var ave = (minR + maxR) / 2.0;                     
                return new Tuple<double,double,double>(minR,maxR, ave);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public PointCyl[] CorrectForError(PointCyl[] points)
        {
            try
            {
                var resultPts = new CylData();
                foreach (var pt in points)
                {
                    var newPt = pt.Clone();
                    var thetaRad = pt.ThetaRad - CorrectionAngle;
                    if (thetaRad < 0)
                    {
                        thetaRad += Math.PI * 2.0;
                    }
                    if (thetaRad > Math.PI * 2.0)
                    {
                        thetaRad -= Math.PI * 2.0;
                    }
                    newPt.ThetaRad = thetaRad;
                    resultPts.Add(newPt);
                }
                resultPts.SortByTheta();
                
                return resultPts.ToArray();
            }
            catch (Exception)
            {

                throw;
            }
        }
        CylData CorrectForError(CylData points)
        {
            try
            {
                var resultPts = new CylData();
                foreach (var pt in points)
                {
                    var newPt = pt.Clone();
                    var thetaRad = pt.ThetaRad - CorrectionAngle;
                    if (thetaRad < 0)
                    {
                        thetaRad += Math.PI * 2.0;
                    }
                    if(thetaRad>Math.PI*2.0)
                    {
                        thetaRad -= Math.PI * 2.0;
                    }
                    newPt.ThetaRad = thetaRad;
                    resultPts.Add(newPt);
                }
                resultPts.SortByTheta();
                resultPts.NominalMinDiam = points.NominalMinDiam;
                return resultPts;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CylData CorrectToMidpoint(CylData points, PointCyl midPoint)
        {
            try
            {
                CorrectionAngle = midPoint.ThetaRad;
                return CorrectForError(points);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// correct data set for clocking error using average error from midpoint of each groove
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public CylData CorrectForAngleError(CylData points)
        {
            try
            {                
                CalcAveAngleError(points);
                return CorrectForError(points);
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// correct data set for clocking error using average error from midpoint of each groove
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public CylData CorrectForAngleError(CylData points,List<PointCyl>intersections)
        {
            try
            {
                var resultPts = new CylData();
                var indexList = GetIntersectionIndexList(points, intersections);
                CalcAveAngleError(points, indexList);
                return CorrectForError(points);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
