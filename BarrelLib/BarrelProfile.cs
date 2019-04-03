using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
using ToolpathLib;
using CNCLib;
using System.Drawing;
using DataLib;
using SurfaceModel; 

namespace BarrelLib
{
    /// <summary>
    /// holds cross section dxf entities and twist profile
    /// </summary>
    public class BarrelProfile :XSection
    {
        public TwistProfile Twist { get; private set; }
       
        public double RadiusAt(double z, double thetaRaD)
        {
            double r = -1;           
            double thetaRotation = Twist.ThetaRadAt(z);
            
            PointCyl ptCyl = new PointCyl(10, thetaRotation + thetaRaD , z);
            Vector3 pt1 = new Vector3(ptCyl);
            Vector3 pt2 = new Vector3(0, 0, z);
            Line ray = new Line(pt1, pt2);
           
            Vector3 result = new Vector3();            
            foreach (DwgEntity entity in Entities)
            {
                IntersectionRecord intersectionRecord;
                if (entity is Arc arc) 
                {
                    intersectionRecord = GeomUtilities.RayArcXYIntersect(arc, ray);
                    if (intersectionRecord.Intersects)
                    {
                        r =  Math.Sqrt(intersectionRecord.X * intersectionRecord.X *+ intersectionRecord.Y*intersectionRecord.Y); 
                    }
                }
                else if (entity is Line line)
                {
                    intersectionRecord = GeomUtilities.RayLineXYIntersect(ray, line);
                    if (intersectionRecord.Intersects)
                    {
                        r =  Math.Sqrt(intersectionRecord.X * intersectionRecord.X * +intersectionRecord.Y * intersectionRecord.Y);
                       
                    }
                }
               
            }
            return r;
        }

       
        
        BarrelProfileType xSectionType;
        BarrelType barrelType;
        
        public BarrelProfile( BarrelType barrelType,string dxfFilename,  BarrelProfileType type,double meshSize) : base(dxfFilename,meshSize)
        {
            this.barrelType = barrelType;
            Twist = new TwistProfile(barrelType);
            xSectionType = type;
           
        }
        public BarrelProfile( BarrelType barrelType,string dxfFilename, TwistProfile twistIn, BarrelProfileType type, double meshSize) : base(dxfFilename,meshSize)
        {
            this.barrelType = barrelType;
            xSectionType = type;
            Twist = twistIn;
        }
    }
}
