using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryLib
{
    public class Vector2:DwgEntity
    {
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                length = 0;
            }

        }
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                length = 0;
            }

        }
      
        public double Length
        {
            get
            {
                if (length == 0)
                {
                    length = (double)Math.Sqrt((double)(x * x + y * y ));
                }
                return length;
            }
        }
        double length;
        double x;
        double y;
        
        public static string Name = "Vector2";
        public override string ToString()
        {
            return Name + ";" + x + ";" + y;
        }

        public static Vector2 Origin { get { return new Vector2(0, 0); } }
        public BoundingBox BoundingBox()
        {
            BoundingBox ext = new BoundingBox();
            return ext;
        }

        public Vector2()
        {
            Type = EntityType.Vector2;
            x = 0;
            y = 0;

            Col = System.Drawing.Color.White;

        }
        public void Normalize()
        {
            double l = Length;
            if (l != 0)
            {
                x /= l;
                y /= l;
               
            }
            length = 0;
        }

      
        public double Dot(Vector2 v2)
        {
            double result = (x * v2.X + y * v2.Y );
            return result;
        }

        public double DistanceTo(Vector2 p2)
        {
            return (double)Math.Sqrt(Math.Pow((double)(p2.X - x), 2) + Math.Pow((double)(p2.Y - y), 2) );
        }
        public double Distance2To(Vector2 p2)
        {

            return (p2.X - x) * (p2.X - x) + (p2.Y - y) * (p2.Y - y) ;
        }
        public double AngleTo(Vector2 p2)
        {
            double p2L = p2.Length;
            double p1L = Length;
            if (p1L == 0)
                p1L = 1.0;
            if (p2L == 0)
                p2L = 1.0;
            double aCos = Dot(p2) / (p2L * p1L);
            aCos = aCos > 1 ? 1 : aCos;
            aCos = aCos < 0 ? 0 : aCos;
            double result = Math.Acos(aCos);
            return result;
        }
        public Vector2 Translate(Vector2 translation)
        {
            return new Vector2(x + translation.X, y + translation.Y, Col);
        }
      
        public Vector2 RotateZ(Vector2 rotationPt, double angleRad)
        {
            Vector2 ptTrans = Translate(-1.0 * rotationPt);
            Rotation rot = new Rotation();
            var sinR = Math.Sin(angleRad);
            var cosR = Math.Cos(angleRad);
            Vector2 ptRot = new Vector2(ptTrans.X * cosR - ptTrans.Y * sinR, ptTrans.X * sinR + ptTrans.Y * cosR);
            Vector2 ptOut = ptRot.Translate(rotationPt);
            ptOut.Col = Col;
            return ptOut;
        }
        public static Vector2 operator -(Vector2 p1, Vector2 p2)
        {
            Vector2 pt = new Vector2();
            pt.X = p1.X - p2.X;
            pt.Y = p1.Y - p2.Y;

            pt.Col = p1.Col;
            return pt;
        }
        public static Vector2 operator +(Vector2 p1, Vector2 p2)
        {
            Vector2 pt = new Vector2();
            pt.X = p1.X + p2.X;
            pt.Y = p1.Y + p2.Y;

            pt.Col = p1.Col;
            return pt;
        }
        public static Vector2 operator *(double scalar, Vector2 p1)
        {
            Vector2 pt = new Vector2();
            pt.X = scalar * p1.X;
            pt.Y = scalar * p1.Y;
           
            return pt;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator ==(Vector2 p1, Vector2 p2)
        {
            bool equal = false;

            if ((Math.Abs(p1.X - p2.X) <= double.Epsilon) &&
                (Math.Abs(p1.Y - p2.Y) <= double.Epsilon) )
            {
                equal = true;
            }
            return equal;
        }
        public static bool operator !=(Vector2 p1, Vector2 p2)
        {
            bool equal = false;
            if ((Math.Abs(p1.X - p2.X) >= double.Epsilon) ||
                (Math.Abs(p1.Y - p2.Y) >= double.Epsilon) )              
            {
                equal = true;
            }
            return equal;
        }
        public Vector2 Clone()
        {
            return new Vector2(x, y,  Col);

        }

        public static Vector2 XAxis
        {
            get
            {
                return new Vector2(1, 0);
            }
        }
        public static Vector2 YAxis
        {
            get
            {
                return new Vector2(0, 1);
            }
        }
       

        public Vector2(Vector2 Vector2)
        {
            x = Vector2.X;
            y = Vector2.Y;
            
            Col = Vector2.Col;
        }
        public Vector2(PointCyl ptIn)
        {
            x = (double)(ptIn.R * Math.Cos(ptIn.ThetaRad));
            y = (double)(ptIn.R * Math.Sin(ptIn.ThetaRad));
           
            Col = ptIn.Col;

        }
        public Vector2(double xIn, double yIn,  System.Drawing.Color c)
        {
            x = xIn;
            y = yIn;
          
            Type = EntityType.Vector2;
            Col = c;

        }
        public Vector2(double xIn, double yIn)
        {
            x = xIn;
            y = yIn;
           
            Type = EntityType.Vector2;
            Col = System.Drawing.Color.White;

        }
    
      
    }
}
