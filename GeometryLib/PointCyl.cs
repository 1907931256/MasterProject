using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryLib
{
    
    public class PointCyl : DwgEntity, IGeometryRoutines<PointCyl>
    {
        public double R { get; set; }
        public double ThetaRad { get; set; }
        public double ThetaDegPosOnly { get { return GeomUtilities.ToDegsPosOnly(ThetaRad); }  }
        public double ThetaDeg  { get { return GeomUtilities.ToDegs(ThetaRad); }  }
        public double Z { get; set; }
       
        public BoundingBox BoundingBox()
        {
                Vector3 v = new Vector3(this);
                BoundingBox ext = new BoundingBox(0,0,0,v.X,v.Y,v.Z); 
                return ext;
            
        }
        public PointCyl Translate(Vector3 translation)
        {
            Vector3 ptXYZ = new Vector3(this);
            Vector3 ptTrans = ptXYZ.Translate(translation);
            PointCyl ptCylTrans = new PointCyl(ptTrans);
            ptCylTrans.Col = Col;
            return ptCylTrans;
        }
        public PointCyl RotateX(Vector3 rotationPt, double angleRad)
        {
            Vector3 ptXYZ = new Vector3(this);
            Vector3 ptRot = ptXYZ.RotateX(rotationPt, angleRad);            
            PointCyl ptCylRot =  new PointCyl(ptRot);
            ptCylRot.Col = Col;
            return ptCylRot;
        }
        public PointCyl RotateY(Vector3 rotationPt, double angleRad)
        {
            Vector3 ptXYZ = new Vector3(this);
            Vector3 ptRot = ptXYZ.RotateY(rotationPt, angleRad);
            PointCyl ptCylRot = new PointCyl(ptRot);
            ptCylRot.Col = Col;
            return ptCylRot;
        }
        public PointCyl RotateZ(Vector3 rotationPt, double angleRad)
        {
            Vector3 ptXYZ = new Vector3(this);
            Vector3 ptRot = ptXYZ.RotateZ(rotationPt, angleRad);
            PointCyl ptCylRot = new PointCyl(ptRot);
            ptCylRot.Col = Col;        
            return ptCylRot;
        }
        public double DistanceTo(PointCyl p2)
        {
            Vector3 p1c = new Vector3(this);
            Vector3 p2c = new Vector3(p2);
            return p1c.DistanceTo(p2c);
            
        }
         public static PointCyl operator -(PointCyl p1, PointCyl p2)
         {
             PointCyl pt = new PointCyl();
             pt.R = p1.R - p2.R;
             pt.ThetaRad = p1.ThetaRad - p2.ThetaRad;
             pt.Z = p1.Z - p2.Z;
             return pt;
         }
         public static PointCyl operator +(PointCyl p1, PointCyl p2)
         {
             PointCyl pt = new PointCyl();
             pt.R = p1.R + p2.R;
             pt.ThetaRad = p1.ThetaRad + p2.ThetaRad;
             pt.Z = p1.Z + p2.Z;
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
         public static bool operator ==(PointCyl p1, PointCyl p2)
         {
            try
            {
                bool equal = false;
                if ((Math.Abs(p1.R - p2.R) < double.Epsilon) &&
                     (Math.Abs(p1.ThetaRad - p2.ThetaRad) < double.Epsilon) &&
                     (Math.Abs(p1.Z - p2.Z) < double.Epsilon))
                { 
                  equal = true;
                }

               
      
                 return equal;
            }
            catch (Exception)
            {

                throw;
            }
         
         }
        public static bool operator ==(PointCyl p1, object p2)
        {
            try
            {
                bool equal = false;
                if (p2.Equals(p1))
                    equal = true;
                return equal;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public static bool operator !=(PointCyl p1, object p2)
         {
            try
            {
                bool equal = false;
                if (p2.Equals(p1))
                    equal = true;
                

                return equal;
            }
            catch (Exception)
            {

                throw;
            }


        }
        public static bool operator !=(PointCyl p1, PointCyl p2)
         {
            try
            {
                bool equal = false;
                
                if ((Math.Abs(p1.R - p2.R) >= double.Epsilon) ||
                     (Math.Abs(p1.ThetaRad - p2.ThetaRad) >= double.Epsilon) ||
                     (Math.Abs(p1.Z - p2.Z) >= double.Epsilon))
                {
                        equal = true;
                }
                
                return equal;
            }
            catch (Exception)
            {

                throw;
            }
           
           
         }
         public PointCyl Clone()
         {
             return new PointCyl(R, ThetaRad, Z, Col,ID);
         }
         public PointCyl()
         {
            Type = EntityType.PointCyl;
            Col = new RGBColor(255, 255, 255);
         }
         public PointCyl(Vector3 pt)
         {
             R = Math.Sqrt(Math.Pow(pt.X, 2) + Math.Pow(pt.Y, 2));
             ThetaRad = Math.Atan2(pt.Y, pt.X);
             Z = pt.Z;
            Type = EntityType.PointCyl;
             Col = pt.Col;
         }
        public PointCyl(double rIn, double thetaInRad, double zIn, RGBColor col, int id)
        {
            R = rIn;
            ThetaRad = thetaInRad;
            Z = zIn;
            ID = id;
            Type = EntityType.PointCyl;
            Col =  new RGBColor( col.Red,col.Green,col.Blue);
        }
        public PointCyl(double rIn, double thetaInRad, double zIn,int id)
        {
            R = rIn;
            ThetaRad = thetaInRad;
            Z = zIn;
            ID = id;
            Type = EntityType.PointCyl;
            Col = new RGBColor(255, 255, 255);
        }
        public PointCyl(double rIn, double thetaInRad, double zIn)
         {
             R = rIn;
             ThetaRad = thetaInRad;
             Z = zIn;
            Type = EntityType.PointCyl;
            Col = new RGBColor(255, 255, 255);
         }
         public PointCyl(double rIn, double thetaInRad, double zIn, RGBColor col)
         {
             R = rIn;
             ThetaRad = thetaInRad;
             Z = zIn;
            Type = EntityType.PointCyl;
             Col = col;
         }
         
    }
}
