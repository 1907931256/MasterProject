using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace GeometryLib
{
    public class Vector3 : DwgEntity, IGeometryRoutines<Vector3>
    {
       
        public double X {
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
        public double Z
        {
            get
            {
                return z;
            }
            set
            {
                z = value;
                length = 0;
            }

        }
        public double Length
        {
            get
            {
                if (length == 0)
                {
                    length =Math.Sqrt(x*x + y*y + z*z);
                }
                return length;                                  
            }
        }
        double length;
        double x;
        double y;
        double z;
        public static string Name = "Vector3";
        public override string ToString()
        {
            return Name + ";" + x + ";" + y + ";" + z;
        }
        
        public static Vector3 Origin { get { return new Vector3(0, 0, 0); } }
        public BoundingBox BoundingBox()
        {
            BoundingBox ext = new BoundingBox();
            return ext;            
        }
        
        public Vector3()
        {
            Type = EntityType.Vector3;
            x = 0;
            y = 0;
            z = 0;
            Col = new RGBColor(255, 255, 255);

        }
        public void Normalize()
        {
            double l = Length;
            if (l != 0)
            {
                x /= l;
                y /= l;
                z /= l;
            }
            length = 0;
        }

        public Vector3 Cross(Vector3 v2)
        {
            double xm = y * v2.Z - z * v2.Y;
            double ym = z * v2.X - x * v2.Z;
            double zm = x * v2.Y - y * v2.X;
            Vector3 vOut = new Vector3(xm, ym, zm);
            return vOut;

        }
        public double Dot(Vector3 v2)
        {
            double result = (x * v2.X + y * v2.Y + z * v2.Z);
            return result;
        }
       
        public double DistanceTo(Vector3 p2)
        {
            return  Math.Sqrt(Math.Pow(p2.X - x, 2) + Math.Pow(p2.Y - y, 2) + Math.Pow(p2.Z - z, 2));
        }
        public double Distance2To(Vector3 p2)
        {

            return (p2.X - x) * (p2.X - x) + (p2.Y - y) * (p2.Y - y) + (p2.Z - z) * (p2.Z - z);
        }
        public double AngleTo(Vector3 p2)
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
        public Vector3 Translate(Vector3 translation)
        {            
            return new Vector3(x + translation.X, y + translation.Y, z + translation.Z, Col); 
        }
        public Vector3 RotateX(Vector3 rotationPt, double angleRad)
        {            
            Vector3 ptTrans = Translate(-1.0 * rotationPt);           
            Rotation rot = new Rotation();
            Vector3 ptRot = rot.AboutX(angleRad) * ptTrans;
            Vector3 ptOut = ptRot.Translate(rotationPt);
            ptOut.Col = Col;
            return ptOut;
        }
        public Vector3 RotateY(Vector3 rotationPt, double angleRad)
        {
            Vector3 ptTrans = Translate(-1.0 * rotationPt);           
            Rotation rot = new Rotation();            
            Vector3 ptRot = rot.AboutY(angleRad) * ptTrans;
            Vector3 ptOut = ptRot.Translate(rotationPt);
            ptOut.Col = Col;
            return ptOut;
        }
        public Vector3 RotateZ(Vector3 rotationPt, double angleRad)
        {
            Vector3 ptTrans = Translate(-1.0 * rotationPt);           
            Rotation rot = new Rotation();
            Vector3 ptRot = rot.AboutZ(angleRad) * ptTrans;
            Vector3 ptOut = ptRot.Translate(rotationPt);
            ptOut.Col = Col;
            return ptOut;
        }
        public static Vector3 operator -(Vector3 p1, Vector3 p2)
        {
            Vector3 pt = new Vector3();
            pt.X = p1.X - p2.X;
            pt.Y = p1.Y - p2.Y;
            pt.Z = p1.Z - p2.Z;
            pt.Col = new RGBColor((p1.Col.Red + p2.Col.Red) / 2, (p1.Col.Green + p2.Col.Green) / 2, (p1.Col.Blue + p2.Col.Blue) / 2);
            return pt;
        }
        public static Vector3 operator +(Vector3 p1, Vector3 p2)
        {
            Vector3 pt = new Vector3();
            pt.X = p1.X + p2.X;
            pt.Y = p1.Y + p2.Y;
            pt.Z = p1.Z + p2.Z;
            pt.Col = new RGBColor((p1.Col.Red + p2.Col.Red) / 2, (p1.Col.Green + p2.Col.Green) / 2, (p1.Col.Blue + p2.Col.Blue) / 2);
            return pt;
        }
        public static Vector3 operator *(double scalar,Vector3 p1)
        {
            Vector3 pt = new Vector3();
            pt.X = scalar * p1.X;
            pt.Y = scalar * p1.Y;
            pt.Z = scalar * p1.Z;
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
        public static bool operator ==(Vector3 p1, Vector3 p2)
        {
            bool equal = false;
            
            if ((Math.Abs(p1.X - p2.X) <= double.Epsilon ) &&
                (Math.Abs(p1.Y - p2.Y) <= double.Epsilon) &&
                (Math.Abs(p1.Z - p2.Z) <= double.Epsilon))
            {
                equal = true;
            }
            return equal;
        }
        public static bool operator !=(Vector3 p1, Vector3 p2)
        {
            bool equal = false;
            if ((Math.Abs(p1.X - p2.X) >= double.Epsilon) ||
                (Math.Abs(p1.Y - p2.Y) >= double.Epsilon) ||
                (Math.Abs(p1.Z - p2.Z) >= double.Epsilon))
            {
                equal = true;
            }
            return equal;
        }
        public Vector3 Clone()
        {
            return new Vector3(x, y, z, Col);            
           
        }
        
        public static Vector3 XAxis
        {
            get
            {
                return new Vector3(1, 0, 0);
            }
        }
        public static Vector3 YAxis
        {
            get
            {
                return new Vector3(0, 1, 0);
            }
        }
        public static Vector3 ZAxis
        {
            get
            {
                return new Vector3(0, 0, 1);
            }
        }
       
        public Vector3(Vector3 vector3)
        {
            x = vector3.X;
            y = vector3.Y;
            z = vector3.Z;
            Col = vector3.Col;
        }
        public Vector3(PointCyl ptIn)
        {
            x = (double)(ptIn.R * Math.Cos(ptIn.ThetaRad));
            y = (double)(ptIn.R * Math.Sin(ptIn.ThetaRad));
            z = (double)(ptIn.Z);
            Col = ptIn.Col;

        }
        public Vector3(double xIn, double yIn, double zIn, RGBColor c)
        {
           
            
            x = xIn;
            y = yIn;
            z = zIn;
            Type = EntityType.Vector3;
            Col = c;

        }
        public Vector3(double xIn, double yIn, double zIn)
        {
            x = xIn;
            y = yIn;
            z = zIn;
            Type = EntityType.Vector3;
            Col = new RGBColor(255, 255, 255);

        }
    }
}
