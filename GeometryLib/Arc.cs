using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GeometryLib
{
    public class Circle2
    {
        public Vector2 Center { get; set; }
        public double Radius { get; set; }
        public Circle2 (Vector2 center,double r)
        {
            Radius = r;
            Center = center;
        }
    }
    public class Arc : DwgEntity,IGeometryRoutines<Arc>
    {       
        protected bool closedArc;

        //properties
        
        public Vector3 Center { get; set; }
        public double Radius { get; set; }
        public double StartAngleRad { get; set;}
        public double EndAngleRad{ get; set;}
        public static string Name = "Arc";
        public override string ToString()
        {
            return Name + ";" + Center.X + ";" + Center.Y + ";" + Center.Z + ";" 
                + Radius + ";" + StartAngleRad + ";" + EndAngleRad + ";" + ClosedArc;
        }

        public double StartAngleDeg
        {
            get
            {
                return GeomUtilities.ToDegs(StartAngleRad);
            }
        }
        public double EndAngleDeg
        {
            get
            {
                return GeomUtilities.ToDegs(EndAngleRad);
            }
        }
        public bool ClosedArc
        {
            get { return closedArc; }
            set
            {
                closedArc = value; 
                if (closedArc)
                {
                    StartAngleRad = 0;
                    EndAngleRad = 2*Math.PI;
                }
            }
        }
        //read only properties

        public Vector3 StartPoint
        {
            get
            {
                return new Vector3(Center.X + Radius * Math.Cos(StartAngleRad),
                    Center.Y + Radius * Math.Sin(StartAngleRad), Center.Z + Radius);
            }
        }
        public Vector3 EndPoint {
            get 
            {
                return new Vector3(Center.X + Radius * Math.Cos(EndAngleRad),
                    Center.Y + Radius * Math.Sin(EndAngleRad), Center.Z);


            }
        }
        
        public double SweepAngleDeg 
        { 
            get
            {
                return GeomUtilities.ToDegs(SweepAngleRad); 
            }
        }
        public double SweepAngleRad
        {
            get
            {
                while (EndAngleRad < StartAngleRad)
                {
                    EndAngleRad += 2 * Math.PI;
                }
                return EndAngleRad - StartAngleRad; 
            }
        }
        public double Length 
        { 
            get 
            {
                return Radius * SweepAngleRad;
            } 
        }

        public BoundingBox BoundingBox()
        {
                BoundingBox ext = new BoundingBox();
                             
                ext.Max.X = Center.X + Radius;
                ext.Min.X = Center.X - Radius;
                ext.Max.Y = Center.Y + Radius;
                ext.Min.Y = Center.Y - Radius;
                ext.Max.Z = Center.Z; 
                ext.Min.Z = Center.Z;
                return ext;           
        }        
        public Arc Translate(Vector3 translation)
        {
            Arc arcTrans = new Arc();
            arcTrans.Center = Center.Translate(translation);
            arcTrans.StartAngleRad = StartAngleRad;
            arcTrans.EndAngleRad = EndAngleRad;            
            arcTrans.Radius = Radius;
            return arcTrans;
        }
        public Arc RotateX(Vector3 rotationPt, double angleRad)
        {
            Arc arcRot = new Arc();
            arcRot.Center = Center.RotateX(rotationPt,angleRad);
            arcRot.StartAngleRad = StartAngleRad + angleRad;
            arcRot.EndAngleRad = EndAngleRad + angleRad;
            arcRot.Radius = Radius;
            return arcRot;
        }
        public Arc RotateY(Vector3 rotationPt, double angleRad)
        {
            Arc arcRot = new Arc();
            arcRot.Center = Center.RotateY(rotationPt, angleRad);
            arcRot.StartAngleRad = StartAngleRad + angleRad;
            arcRot.EndAngleRad = EndAngleRad + angleRad;
            arcRot.Radius = Radius;
            return arcRot;
        }
        public Arc RotateZ(Vector3 rotationPt, double angleRad)
        {
            Arc arcRot = new Arc();
            arcRot.Center = Center.RotateZ(rotationPt, angleRad);
            arcRot.StartAngleRad = StartAngleRad + angleRad;
            arcRot.EndAngleRad = EndAngleRad + angleRad;
            arcRot.Radius = Radius;
            return arcRot;
        }
        public Arc Clone()
        {
            Arc a = new Arc(Center, Radius, StartAngleRad, EndAngleRad);
            a.Col = Col;
            return a;
        }
        public Arc(Vector3 center, double radius, double startAngle, double endAngle)
        {
            Type = EntityType.Arc;
            Center = new Vector3(center.X, center.Y, center.Z);
            Radius = radius;
            StartAngleRad = startAngle;
            EndAngleRad = endAngle;
            Col = new RGBColor(255, 255, 255);
        }
        public Arc()
        {
            
            Center = new Vector3();
            Type = EntityType.Arc;
            Col = new RGBColor(255, 255, 255);
        }
       
        public Arc(Vector3 point1,Vector3 point2,Vector3 point3)
        {
            //three point formula from the following assuming in xy plane
            //http://mathworld.wolfram.com/Circle.html

            double a = threePtArc_A(point1, point2, point3);
            double d = threePtArc_D(point1, point2, point3);
            double e = threePtArc_E(point1, point2, point3);
            double f = threePtArc_F(point1, point2, point3);

            Center = getCenter(d, e, a);
            Radius = getRadius(d, e, f, a); ;
            Type = EntityType.Arc;
            StartAngleRad = Math.Atan2(point1.Y-Center.Y,point1.X-Center.X);
            EndAngleRad = Math.Atan2(point3.Y - Center.Y, point3.X - Center.X);

            Col = new RGBColor(255, 255, 255);
        }
        public Arc(string s)
        {
            if (s.Contains("Arc"))
            {
                Center = new Vector3();
                string[] elements = s.Split(';');
                Center.X = double.Parse(elements[1]);
                Center.Y = double.Parse(elements[2]);
                Center.Z = double.Parse(elements[3]);
                Radius = double.Parse(elements[4]);
                StartAngleRad = double.Parse(elements[5]);
                EndAngleRad = double.Parse(elements[6]);
                closedArc = bool.Parse(elements[7]);
            }
        }
        private double getRadius(double dParam,double eParam, double fParam,double aParam)
        {
            return Math.Sqrt(((dParam * dParam + eParam * eParam) / (4 * aParam * aParam)) - (fParam / aParam));
        }
        private Vector3 getCenter(double dParam,double eParam, double aParam)
        {
            double xCenter = -1 * dParam / 2 * aParam;
            double yCenter = -1 * eParam / 2 * aParam;
            return new Vector3(xCenter,yCenter,0);
        }
        private double threePtArc_A(Vector3 Point1, Vector3 Point2, Vector3 pt3)
        {
            Matrix3x3 mat = new Matrix3x3(
                Point1.X,       Point1.Y,       1, 
                Point2.X,       Point2.Y,       1, 
                pt3.X,          pt3.Y,          1);
            double a = mat.Determinant();
            return a;
        }
        private double threePtArc_D(Vector3 pt1, Vector3 pt2, Vector3 pt3)
        {
            Matrix3x3 mat = new Matrix3x3(
                pt1.X * pt1.X + pt1.Y * pt1.Y,                pt1.Y,                 1,
                pt2.X * pt2.X + pt2.Y * pt2.Y,                pt2.Y,                 1, 
                pt3.X * pt3.X + pt3.Y * pt3.Y,                pt3.Y,                 1);
            double d = -1 * mat.Determinant();
            return d;
        }
        private double threePtArc_E(Vector3 pt1, Vector3 pt2, Vector3 pt3)
        {
            Matrix3x3 mat = new Matrix3x3(
                pt1.X * pt1.X + pt1.Y * pt1.Y,                pt1.X,                1,
                pt2.X * pt2.X + pt2.Y * pt2.Y,                pt2.X,                1,
                pt3.X * pt3.X + pt3.Y * pt3.Y,                pt3.X,                1);
            double e = mat.Determinant();
            return e;
        }
        private double threePtArc_F(Vector3 pt1, Vector3 pt2, Vector3 pt3)
        {
            Matrix3x3 mat = new Matrix3x3(
                pt1.X * pt1.X + pt1.Y * pt1.Y,                pt1.X,                 pt1.Y, 
                pt2.X * pt2.X + pt2.Y * pt2.Y,                pt2.X,                 pt2.Y,
                pt3.X * pt3.X + pt3.Y * pt3.Y,                pt3.X,                 pt3.Y);
            double f = -1 * mat.Determinant();
            return f;
        }
        
        
    }
   
}
