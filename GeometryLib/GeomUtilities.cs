using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryLib
{
    public class Geometry
    {

        public static double ToDegs(double ThetaRadians)
        {
            return 180 * ThetaRadians / Math.PI;
        }
        public static double ToRadians(double ThetaDegs)
        {
            double radians = Math.PI * ThetaDegs /180  ;
            //radians %= 2 * Math.PI;
            return radians;

        }
        public static bool isBetweenAngles(double angleRadians, double minRadians, double maxRadians)
        {          
            while (minRadians > maxRadians)
            {
                maxRadians += 2 * Math.PI;                
            }
            return (angleRadians < maxRadians && angleRadians > minRadians);
        }
        public static double HelixLength(double diameter, double pitch, double turns)
        {
             return turns * Math.Sqrt(Math.Pow(Math.PI, 2) * Math.Pow(diameter, 2) + Math.Pow(pitch, 2));           
        }
        public static List<Vector3> BreakMany(Line line, double breakLen)
        {
            try
            {
                var points = new List<Vector3>();

                if (line.Length <= breakLen)
                {
                    points.Add(line.Point1);
                    points.Add(line.Point2);
                    return points;
                }

                int parseCount = (int)Math.Round(line.Length / breakLen);
                if (parseCount == 0)
                    parseCount = 1;

                Vector3 delta = line.Point2 - line.Point1;

                double dpc = parseCount;
                for (int i = 0; i <= parseCount; i++)
                {
                    double di = i;
                     
                    Vector3 v = line.Point1 + (di/ dpc) * delta;
                    points.Add(v);
                }
                return points;
            }
            catch (Exception)
            {

                throw;
            }
            

        }
        /// <summary>
        /// returns true if infinite ray and arc intersect, stores intersection(s) in result
        /// </summary>
        /// <param name="arcIn"></param>
        /// <param name="lineIn"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static IntersectionRecord RayArcXYIntersect(Arc arcIn, Line lineIn)
        {
            try
            {
                IntersectionRecord result = new IntersectionRecord();
                Vector3 Point1;
                IntersectionRecord ir = new IntersectionRecord();
                //transform arc and line to center arc at origin
                Arc arcT = arcIn.Translate(new Vector3(-1 * arcIn.Center.X, -1 * arcIn.Center.Y, 0));
                Line lineT = lineIn.Translate(new Vector3(-1 * arcIn.Center.X, -1 * arcIn.Center.Y, 0));
                //parameterize line
                double dy = lineT.Point2.Y - lineT.Point1.Y;
                double dx = lineT.Point2.X - lineT.Point1.X;

                //calc quadratic solution
                double a = Math.Pow(dx, 2) + Math.Pow(dy, 2);
                double b = 2 * (dx) * (lineT.Point1.X - arcT.Center.X) + 2 * (dy) * (lineT.Point1.Y - arcT.Center.Y);
                double c = Math.Pow(lineT.Point1.X - arcT.Center.X, 2) + Math.Pow(lineT.Point1.Y - arcT.Center.Y, 2) - Math.Pow(arcT.Radius, 2);
                double discrim = Math.Pow(b, 2) - 4 * a * c;

                if (discrim >= 0)//check solution is real
                {
                    double tPos = (-b + Math.Sqrt(discrim)) / (2 * a);
                    double tNeg = (-b - Math.Sqrt(discrim)) / (2 * a);
                    double xpos = dx * tPos + lineT.Point1.X;
                    double ypos = dy * tPos + lineT.Point1.Y;
                    double xneg = dx * tNeg + lineT.Point1.X;
                    double yneg = dy * tNeg + lineT.Point1.Y;
                    double angneg = Math.Atan2(yneg, xneg);
                    double angpos = Math.Atan2(ypos, xpos);

                    if (angneg < 0) angneg += 2 * Math.PI;
                    if (angpos < 0) angpos += 2 * Math.PI;

                    //transform point back before returning
                    if ((angneg >= arcT.StartAngleRad) && (angneg <= arcT.EndAngleRad))
                    {

                        Point1 = new Vector3(xneg, yneg, 0);
                        result = (IntersectionRecord)Point1.Translate(new Vector3(arcIn.Center.X, arcIn.Center.Y, 0.0));
                        result.Intersects = true;
                    }
                    if ((angpos >= arcT.StartAngleRad) && (angpos <= arcT.EndAngleRad))
                    {

                        Point1 = new Vector3(xpos, ypos, 0);
                        result = (IntersectionRecord)Point1.Translate(new Vector3(arcIn.Center.X, arcIn.Center.Y, 0.0));
                        result.Intersects = true;
                    }

                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
           

        }
        private static double signp(double x)
        {
            if (x < 0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        /// <summary>
        /// returns true if line segment and arc intersect, stores intersection(s) in result
        /// </summary>
        /// <param name="arcIn"></param>
        /// <param name="lineIn"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static IntersectionRecord LineArcXYIntersect(Arc arcIn, Line line1In )
        {
            try
            {
                IntersectionRecord ir = new IntersectionRecord();

                double x1min = Math.Min(line1In.Point1.X, line1In.Point2.X);
                double y1min = Math.Min(line1In.Point1.Y, line1In.Point2.Y);
                double x1max = Math.Max(line1In.Point1.X, line1In.Point2.X);
                double y1max = Math.Max(line1In.Point1.Y, line1In.Point2.Y);
                ir = RayArcXYIntersect(arcIn, line1In);
                if (ir.Intersects)
                {
                    if ((ir.X <= x1max) && (ir.X >= x1min) && (ir.Y <= y1max) && (ir.Y >= y1min))
                    {
                        ir.Intersects = true;
                    }
                    else
                    {
                        ir.Intersects = false;
                    }

                }
                return ir;
            }
            catch (Exception)
            {

                throw;
            }
            

        }
        /// <summary>
        /// returns true if 2D coplanar infinite rays intersect and stores intersection in result
        /// </summary>
        /// <param name="ray1"></param>
        /// <param name="ray2"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static IntersectionRecord RayRayXYIntersect(Line ray1, Line ray2 )
        {
            try
            {
                IntersectionRecord result = new IntersectionRecord();
                Vector3 ptOut = new Vector3();

                if (Math.Abs(ray1.SlopeXY) == Math.Abs(ray2.SlopeXY))
                {

                    double yt = (ray1.Point1.X - ray2.Point1.X) * (ray2.Point2.Y - ray2.Point1.Y);
                    double yt2 = (ray2.Point2.X - ray2.Point1.X) * (ray2.Point1.Y - ray1.Point1.Y);

                    if ((yt2 - yt) > float.Epsilon)
                    {
                        result.Intersects = false;
                    }
                    else
                    {
                        result = new IntersectionRecord(new Vector3(ray2.Point1.X, ray2.Point1.Y, ray2.Point1.Z), true);

                    }
                }
                else
                {

                    double denom = (ray1.Point1.X - ray1.Point2.X) * (ray2.Point1.Y - ray2.Point2.Y) - (ray2.Point1.X - ray2.Point2.X) * (ray1.Point1.Y - ray1.Point2.Y);
                    double xOut = 0;
                    double yOut = 0;
                    double line1Det = ray1.Point1.X * ray1.Point2.Y - ray1.Point2.X * ray1.Point1.Y;
                    double line2Det = ray2.Point1.X * ray2.Point2.Y - ray2.Point2.X * ray2.Point1.Y;
                    xOut = (line1Det * (ray2.Point1.X - ray2.Point2.X) - line2Det * (ray1.Point1.X - ray1.Point2.X)) / denom;
                    yOut = (line1Det * (ray2.Point1.Y - ray2.Point2.Y) - line2Det * (ray1.Point1.Y - ray1.Point2.Y)) / denom;
                    result = new IntersectionRecord(new Vector3(xOut, yOut, ray2.Point1.Z), true);
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        /// <summary>
        /// return true if 2d coplanar line segments intersect and stores intersection in result
        /// </summary>
        /// <param name="line1In"></param>
        /// <param name="line2In"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static IntersectionRecord LineLineXYIntersect(Line line1In, Line line2In)
        {
            try
            {
                IntersectionRecord result = new IntersectionRecord();
                double x2min = Math.Min(line2In.Point1.X, line2In.Point2.X);
                double y2min = Math.Min(line2In.Point1.Y, line2In.Point2.Y);
                double x2max = Math.Max(line2In.Point1.X, line2In.Point2.X);
                double y2max = Math.Max(line2In.Point1.Y, line2In.Point2.Y);
                double x1min = Math.Min(line1In.Point1.X, line1In.Point2.X);
                double y1min = Math.Min(line1In.Point1.Y, line1In.Point2.Y);
                double x1max = Math.Max(line1In.Point1.X, line1In.Point2.X);
                double y1max = Math.Max(line1In.Point1.Y, line1In.Point2.Y);
                result = RayRayXYIntersect(line1In, line2In);
                if (result.Intersects)
                {

                    if ((result.X <= x1max) && (result.X >= x1min) && (result.X <= x2max) && (result.X >= x2min) &&
                        (result.Y <= y1max) && (result.Y >= y1min) && (result.Y <= y2max) && (result.Y >= y2min))
                    {
                        result.Intersects = true;
                    }
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        /// <summary>
        /// returns true if ray and line segment intersect and stores intersection in result
        /// </summary>
        /// <param name="RayIn"></param>
        /// <param name="line2In"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static IntersectionRecord RayLineXYIntersect(Line rayIn, Line line2In)
        {
            try
            {
                double x2min = Math.Min(line2In.Point1.X, line2In.Point2.X);
                double y2min = Math.Min(line2In.Point1.Y, line2In.Point2.Y);
                double x2max = Math.Max(line2In.Point1.X, line2In.Point2.X);
                double y2max = Math.Max(line2In.Point1.Y, line2In.Point2.Y);
                IntersectionRecord result = RayRayXYIntersect(rayIn, line2In);
                if (result.Intersects)
                {
                    if ((result.X > x2max) || (result.X < x2min) || (result.Y > y2max) || (result.Y < y2min))
                    {
                        result.Intersects = false;
                    }

                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public static IntersectionRecord RayTriangleIntersect(Ray ray,Triangle plane)
        {
            try
            {
                IntersectionRecord result = new IntersectionRecord();
                Vector3 op = plane.Vertices[0] - ray.Origin;
                ray.Direction.Normalize();
                double denom = ray.Direction.Dot(plane.Normal);
                if (denom != 0)
                {
                    double t = -1 * op.Dot(plane.Normal) / denom;
                    result = new IntersectionRecord(new Vector3(ray.Origin.X + t * ray.Direction.X, ray.Origin.Y + t * ray.Direction.Y, ray.Origin.Z + t * ray.Direction.Z), true);

                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public static double RayPointDistance(Ray ray, Vector3 point)
        {
            try
            {
                double distance;
                double denom = (ray.PointOnRayAt(1) - ray.Origin).Length;

                Vector3 rayEnd = ray.Direction + ray.Origin;
                Vector3 v1 = point - ray.Origin;
                Vector3 v2 = point - rayEnd;
                double numerator = (v1.Cross(v2)).Length;

                distance = numerator / denom;

                return distance;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}
