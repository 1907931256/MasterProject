using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GeometryLib
{
    public class Line2: DwgEntity
    {
        public Vector2 Point1 { get; set; }
        public Vector2 Point2 { get; set; }
        public double Length
        {
            get
            {
                return Math.Sqrt(Math.Pow((Point2.X - Point1.X), 2) +
                    Math.Pow((Point2.Y - Point1.Y), 2) );
            }
        }
        public double SlopeXY
        {
            get
            {
                double denom = Point2.X - Point1.X;
                double slope;
                if (denom == 0)
                {
                    slope = double.NaN;

                }
                else
                {
                    slope = (Point2.Y - Point1.Y) / denom;
                }

                return slope;
            }

        }
        public double ThetaRadians()
        {
            return Math.Atan2(Point2.Y - Point1.Y, Point2.X - Point1.X);
        }
        public Line2 Translate(Vector2 translation)
        {
            Line2 lineOut = new Line2(Point1 + translation, Point2 + translation);
            lineOut.Col = Col;
            return lineOut;
        }
       
        public Line2 RotateZ(Vector2 rotationPt, double angleRad)
        {
            Line2 lineRot = new Line2();
            lineRot.Point1 = Point1.RotateZ(rotationPt, angleRad);
            lineRot.Point2 = Point2.RotateZ(rotationPt, angleRad);
            lineRot.Col = Col;
            return lineRot;
        }

        public Line2 Clone()
        {
            return new Line2(Point1.X, Point1.Y, Point2.X, Point2.Y);
        }
        public Line2()
        {
            Point1 = new Vector2();
            Point2 = new Vector2();
            Type = EntityType.Line;
            Col = System.Drawing.Color.White;
        }
        public Line2(Vector2 Point1, Vector2 Point2)
        {
            this.Point1 = new Vector2(Point1);
            this.Point2 = new Vector2(Point2);


            Type = EntityType.Line;
            Col = new System.Drawing.Color();
        }
        public Line2(double x1In, double y1In,  double x2In, double y2In)
        {
            Point1 = new Vector2(x1In, y1In);
            Point2 = new Vector2(x2In, y2In);

            Type = EntityType.Line;
            Col = new System.Drawing.Color();
        }
    }
    public class Line : DwgEntity, IGeometryRoutines<Line>
    {

        //properties
        public Vector3 Point1 { get; set; }
        public Vector3 Point2 { get; set; }
        
        public static string Name = "Line";
        public override string ToString()
        {
            return Name + ";" + Point1.X + ";" + Point1.Y + ";" + Point1.Z 
                + ";" + Point1.X + ";" + Point1.Y + ";" + Point1.Z;
        }
       
        //read only
        public double Length 
        { 
            get 
            { 
                return Math.Sqrt( Math.Pow((Point2.X - Point1.X),2) +
                    Math.Pow((Point2.Y - Point1.Y),2) + Math.Pow((Point2.Z - Point1.Z), 2)); 
            } 
        }
        public double SlopeXY
        {
            get
            {
                double denom = Point2.X - Point1.X;
                double slope;
                if (denom == 0)
                {
                    slope = double.NaN;

                }
                else
                {
                    slope = (Point2.Y - Point1.Y) / denom;
                }
                
                return slope;
            }

        }
        public double PsiRadians()
        {
            return Length == 0 ? 0 : Math.Acos((Point2.Z - Point1.Z )/ Length);
        }
        public double ThetaRadians()
        {
            return Math.Atan2(Point2.Y - Point1.Y, Point2.X - Point1.X);
        }
        public BoundingBox BoundingBox()
        {           
                Vector3[] pts = new Vector3[] { Point1, Point2 };
                BoundingBox ext = BoundingBoxBuilder.FromPtArray(pts);               

                return ext;           
        }
        public Line Translate(Vector3 translation)
        {
            Line lineOut =  new Line(Point1 + translation, Point2 + translation);
            lineOut.Col = Col;
            return lineOut;
        }
        public Line RotateX(Vector3 rotationPt, double angleRad)
        {
            Line lineRot = new Line();
            lineRot.Point1 = Point1.RotateX(rotationPt, angleRad);
            lineRot.Point2 = Point2.RotateX(rotationPt, angleRad);
            lineRot.Col = Col;
            return lineRot;
        }
        public Line RotateY(Vector3 rotationPt, double angleRad)
        {
            Line lineRot = new Line();
            lineRot.Point1 = Point1.RotateY(rotationPt, angleRad);
            lineRot.Point2 = Point2.RotateY(rotationPt, angleRad);
            lineRot.Col = Col;
            return lineRot;
        }
        public Line RotateZ(Vector3 rotationPt, double angleRad)
        {           
            Line lineRot = new Line();
            lineRot.Point1 = Point1.RotateZ(rotationPt, angleRad);
            lineRot.Point2 = Point2.RotateZ(rotationPt, angleRad);
            lineRot.Col = Col;
            return lineRot;
        }

         public Line Clone()
         {
             return new Line(Point1.X, Point1.Y, Point1.Z,
                 Point2.X, Point2.Y, Point2.Z);
         }
         public Line()
         {
            Point1 = new Vector3();
            Point2 = new Vector3();
            Type = EntityType.Line;
            Col = System.Drawing.Color.White;
         }
         public Line(Vector3 Point1, Vector3 Point2)
         {
             this.Point1 = new Vector3(Point1);
             this.Point2 = new Vector3(Point2);


            Type = EntityType.Line;
            Col = new System.Drawing.Color();
         }
         public Line(double x1In, double y1In,double z1In, double x2In, double y2In,double z2In)
         {
            Point1 = new Vector3(x1In, y1In, z1In);
            Point2 = new Vector3(x2In, y2In, z2In);

            Type = EntityType.Line;
            Col = new System.Drawing.Color();
         }
         public Line(string s)
         {
            Point1 = new Vector3();
            Point2 = new Vector3();
            Type = EntityType.Line;
             string[] elements = s.Split(';');
             if (s.Contains("Line"))
             {
                 Point1.X = double.Parse(elements[1]);
                 Point1.Y = double.Parse(elements[2]);
                 Point1.Z = double.Parse(elements[3]);
                 Point2.X = double.Parse(elements[4]);
                 Point2.Y = double.Parse(elements[5]);
                 Point2.Z = double.Parse(elements[6]);
             }
            Col = new System.Drawing.Color();
         }

    }
}
