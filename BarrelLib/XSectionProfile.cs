using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
using ToolpathLib;
using CNCLib;
using System.Drawing;
using DataLib;

namespace BarrelLib
{
    /// <summary>
    /// holds cross section dxf entities and twist profile
    /// </summary>
    public class XSectionProfile
    {
        public int EntityCount { get { return dwgEntities.Count;  } }
        public List<DwgEntity> Entities { get { return dwgEntities; } }
        public BoundingBox BoundingBox { get { return _boundingBox; } }

        private BoundingBox _boundingBox;
        List<DwgEntity> dwgEntities;
       
        public TwistProfile twist { get; set; }
        DisplayData cartDisplayData;

        public DisplayData AsCartDisplayData( )        
        {            
            return cartDisplayData;
        }
        
        public DisplayData AsTrimmedCartDisplayData(RectangleF window)
        {
            return cartDisplayData.TrimTo(window);
        }
        public DisplayData AsCylDisplayData()
        {
            return cylDisplayData;
        }
        public DisplayData AsTrimmedCylDisplayData(RectangleF window)
        {
            return cylDisplayData.TrimTo(window);
        }
        /// <summary>
        /// returns radius of cross section at current machine position
        /// </summary>
        /// <param name="pos">machine position</param>
        /// <returns></returns>
        public double RadiusAt(double z, double thetaRaD)
        {
            double r = -1;
            bool intersection = false;
            double thetaRotation = twist.ThetaRadAt(z);
            
            PointCyl ptCyl = new PointCyl(10, thetaRotation + thetaRaD , z);
            Vector3 pt1 = new Vector3(ptCyl);
            Vector3 pt2 = new Vector3(0, 0, z);
            Line ray = new Line(pt1, pt2);
           
            Vector3 result = new Vector3();            
            foreach (DwgEntity entity in dwgEntities)
            {
                if (entity is Arc) 
                {
                    Arc arc = entity as Arc;
                    if (GeomUtilities.RayArcXYIntersect(arc, ray).Intersects)
                    {
                        intersection = true;
                        
                        break;
                    }
                }
                if (entity is Line) 
                {
                    Line line = entity as Line;
                    if (GeomUtilities.RayLineXYIntersect(ray, line).Intersects)
                    {
                        intersection = true;
                        
                        break;
                    }
                }
            }
            if (intersection)
            {
                PointCyl ptC1 = new PointCyl(result);
                r = ptC1.R;
            }                    
            return r;
        }

        /// <summary>
        /// return list of entities rotated at current positon
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public List<DwgEntity> rotateEntities(double thetaRadians)
        {
            
            List<DwgEntity> rotEntities = new List<DwgEntity>();
            Vector3 origin = new Vector3(0,0,0);
            foreach (DwgEntity entity in dwgEntities)
            {                
                if (entity is Arc)
                {
                    Arc arc = entity as Arc;
                    rotEntities.Add(arc.RotateZ(origin, thetaRadians));
                }
                if (entity is Line)
                {
                    Line line = entity as Line;
                    rotEntities.Add(line.RotateZ(origin, thetaRadians));
                }                
            }
            return rotEntities;
        }
        BoundingBox getBoundingBox(List<DwgEntity> entities)
        {
            BoundingBox ext = new BoundingBox();
            var boundingBoxList = new List<BoundingBox>();
            foreach (DwgEntity entity in entities)
            {
                if (entity is Line)
                {
                    Line line = entity as Line;

                    boundingBoxList.Add(line.BoundingBox());
                }
                if (entity is Arc)
                {
                    Arc arc = entity as Arc;

                    boundingBoxList.Add(arc.BoundingBox());
                }
            }
            ext = BoundingBoxBuilder.Union(boundingBoxList.ToArray());
            return ext;
        }
        XSectionType xSectionType;
        DisplayData cylDisplayData;
        void init(string filename, TwistProfile twistIn, XSectionType type)
        {
            xSectionType = type;
            twist = twistIn;
            dwgEntities = DwgConverterLib.DxfFileParser.Parse(filename);
            
            _boundingBox = getBoundingBox(dwgEntities);
            double segmentLength = .001;
            cartDisplayData = DwgConverterLib.DxfFileParser.AsDisplayData(dwgEntities, segmentLength, ViewPlane.XY);
            cartDisplayData.FileName = filename;
            cylDisplayData = new DisplayData(filename);
            foreach(var pt in cartDisplayData)
            {
                PointCyl ptc = new PointCyl(new Vector3(pt.X, pt.Y, 0));
                PointF ptf = new PointF((float)ptc.ThetaDeg(), (float)ptc.R);
                cylDisplayData.Add(ptf);
            }
            cylDisplayData.FileName = filename;
        }
        public XSectionProfile( BarrelType barrelType,string filename,  XSectionType type)
        {
            twist = new TwistProfile(barrelType);
            init(filename, twist, type);
        }
        public XSectionProfile( BarrelType barrelType,string filename, TwistProfile twistIn, XSectionType type)
        {
            init(filename, twistIn, type);
        }
    }
}
