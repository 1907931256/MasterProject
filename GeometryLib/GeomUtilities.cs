using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryLib
{
    public class GeomUtilities
    {
        static public Vector2[] FindCirclesofKnownR(Vector2 p1, Vector2 p2, double radius)
        {
            try
            {
                if (radius < 0) throw new ArgumentException("Negative radius.");
                if (radius == 0)
                {
                    if (p1 == p2) return new[] { p1 };
                    else throw new InvalidOperationException("No circles.");
                }
                if (p1 == p2) throw new InvalidOperationException("Infinite number of circles.");

                double sqDistance = p1.Distance2To(p2);
                double sqDiameter = 4 * radius * radius;
                if (sqDistance > sqDiameter) throw new InvalidOperationException("Points are too far apart.");

                var midPoint = new Vector2((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                if (sqDistance == sqDiameter) return new[] { midPoint };

                double d = Math.Sqrt(radius * radius - sqDistance / 4);
                double distance = Math.Sqrt(sqDistance);
                double ox = d * (p2.X - p1.X) / distance, oy = d * (p2.Y - p1.Y) / distance;
                return new[] {
                    new Vector2(midPoint.X - oy, midPoint.Y + ox),
                    new Vector2(midPoint.X + oy, midPoint.Y - ox)
                };
            }
            catch (Exception)
            {

                throw;
            }

        }
        static public Vector3[] FindCirclesofKnownR(Vector3 p1, Vector3 p2, double radius)
        {
            try
            {
                if (radius < 0) throw new ArgumentException("Negative radius.");
                if (radius == 0)
                {
                    if (p1 == p2) return new[] { p1 };
                    else throw new InvalidOperationException("No circles.");
                }
                if (p1 == p2) throw new InvalidOperationException("Infinite number of circles.");

                double sqDistance = p1.Distance2To(p2);
                double sqDiameter = 4 * radius * radius;
                if (sqDistance > sqDiameter) throw new InvalidOperationException("Points are too far apart.");

                var midPoint = new Vector3((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2,0);
                if (sqDistance == sqDiameter) return new[] { midPoint };

                double d = Math.Sqrt(radius * radius - sqDistance / 4);
                double distance = Math.Sqrt(sqDistance);
                double ox = d * (p2.X - p1.X) / distance, oy = d * (p2.Y - p1.Y) / distance;
                return new[] {
                    new Vector3(midPoint.X - oy, midPoint.Y + ox,0),
                    new Vector3(midPoint.X + oy, midPoint.Y - ox,0)
                };
            }
            catch (Exception)
            {

                throw;
            }

        }
        static public System.Drawing.PointF[] FindCirclesofKnownR(System.Drawing.PointF p1, System.Drawing.PointF p2, double radius)
        {
            try
            {
                if (radius < 0) throw new ArgumentException("Negative radius.");
                if (radius == 0)
                {
                    if (p1 == p2) return new[] { p1 };
                    else throw new InvalidOperationException("No circles.");
                }
                if (p1 == p2) throw new InvalidOperationException("Infinite number of circles.");

                double sqDistance = Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2);
                double sqDiameter = 4 * radius * radius;
                if (sqDistance > sqDiameter) throw new InvalidOperationException("Points are too far apart.");

                var midPoint = new System.Drawing.PointF((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                if (sqDistance == sqDiameter) return new[] { midPoint };

                double d = Math.Sqrt(radius * radius - sqDistance / 4);
                double distance = Math.Sqrt(sqDistance);
                double ox = d * (p2.X - p1.X) / distance, oy = d * (p2.Y - p1.Y) / distance;
                return new[] {
                    new System.Drawing.PointF((float)(midPoint.X - oy), (float)(midPoint.Y + ox)),
                    new System.Drawing.PointF((float)(midPoint.X + oy), (float)(midPoint.Y - ox))
                };
            }
            catch (Exception)
            {

                throw;
            }

        }
        public static Circle2 FitCirleToThreePoints(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            try
            {
                double x1 = (p1.X + p2.X) / 2.0;
                double y1 = (p1.Y + p2.Y) / 2.0;
                double dy1 = p2.X - p1.X;
                double dx1 = -1 * (p2.Y - p1.Y);
                double x2 = (p3.X + p2.X) / 2.0;
                double y2 = (p3.Y + p2.Y) / 2.0;
                double dy2 = p3.X - p2.X;
                double dx2 = -1 * (p3.Y - p2.Y);
                var line1 = new Line2(new Vector2(x1, y1), new Vector2(x1 + dx1, y1 + dy1));
                var line2 = new Line2(new Vector2(x2, y2), new Vector2(x2 + dx2, y2 + dy2));
                var intersection = GeomUtilities.LineLineXYIntersect(line1, line2);
                if (!(intersection.Intersects))
                {
                    throw new Exception("Points are colinear, Couldn't find arc");
                }
                else
                {
                    var center = new Vector2(intersection.X, intersection.Y);
                    var dx = center.X - p1.X;
                    var dy = center.Y - p1.Y;
                    var r = Math.Sqrt(dx * dx + dy * dy);
                    return new Circle2(center, r);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static double ToDegsPosOnly(double ThetaRadians)
        {
            try
            {
                var degs = 180 * ThetaRadians / Math.PI;                
                while (degs < 0)
                {
                    degs += 360;
                }
                degs %= 360;
                return degs;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public static double ToDegs(double ThetaRadians)
        {
            try
            {
                var degs = 180 * ThetaRadians / Math.PI;
                return degs;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public static double ToRadians(double ThetaDegs)
        {
            double radians = Math.PI * ThetaDegs /180 ;
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
        public static IntersectionRecord2 RayRayXYIntersect(Line2 ray1, Line2 ray2 )
        {
            try
            {
                IntersectionRecord2 result = new IntersectionRecord2();
                Vector2 ptOut = new Vector2();

                if (ray1.SlopeXY == ray2.SlopeXY)
                {

                    double yt = (ray1.Point1.X - ray2.Point1.X) * (ray2.Point2.Y - ray2.Point1.Y);
                    double yt2 = (ray2.Point2.X - ray2.Point1.X) * (ray2.Point1.Y - ray1.Point1.Y);

                    if ((yt2 - yt) > float.Epsilon)
                    {
                        result.Intersects = false;
                    }
                    else
                    {
                        result = new IntersectionRecord2(new Vector2(ray2.Point1.X, ray2.Point1.Y), true);

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
                    result = new IntersectionRecord2(new Vector2(xOut, yOut), true);
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public static IntersectionRecord RayRayXYIntersect(Line ray1, Line ray2)
        {
            try
            {
                IntersectionRecord result = new IntersectionRecord();
                Vector3 ptOut = new Vector3();

                if (ray1.SlopeXY == ray2.SlopeXY)
                {

                    double yt = (ray1.Point1.X - ray2.Point1.X) * (ray2.Point2.Y - ray2.Point1.Y);
                    double yt2 = (ray2.Point2.X - ray2.Point1.X) * (ray2.Point1.Y - ray1.Point1.Y);

                    if ((yt2 - yt) > float.Epsilon)
                    {
                        result.Intersects = false;
                    }
                    else
                    {
                        result = new IntersectionRecord(new Vector3(ray2.Point1.X, ray2.Point1.Y,0), true);

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
                    result = new IntersectionRecord(new Vector3(xOut, yOut,0), true);
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
        public static IntersectionRecord2 LineLineXYIntersect(Line2 line1In, Line2 line2In)
        {
            try
            {
                IntersectionRecord2 result = new IntersectionRecord2();
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
        public static IntersectionRecord2 RayLineXYIntersect(Line2 rayIn, Line2 line2In)
        {
            try
            {
                double x2min = Math.Min(line2In.Point1.X, line2In.Point2.X);
                double y2min = Math.Min(line2In.Point1.Y, line2In.Point2.Y);
                double x2max = Math.Max(line2In.Point1.X, line2In.Point2.X);
                double y2max = Math.Max(line2In.Point1.Y, line2In.Point2.Y);
                IntersectionRecord2 result = RayRayXYIntersect(rayIn, line2In);
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
