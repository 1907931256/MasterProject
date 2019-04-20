using System;
using System.Collections.Generic;
using GeometryLib;

namespace DwgConverterLib
{
    public class DXFLine:Line,IDXFEntity
    {
        
        public DxfColor DxfColor { get; set; }
       

        public List<string> AsDXFString()
        {
            try
            {
                List<string> entityList = new List<string>();
                entityList.Add("LINE");
                entityList.Add("5");
                entityList.Add(ID.ToString());
                entityList.Add("330");
                entityList.Add("1F");
                entityList.Add("100");
                entityList.Add("AcDbEntity");
                entityList.Add("  8");
                entityList.Add("0");
                entityList.Add(" 62");
                int c = (int)DxfColor;
                entityList.Add(" "+c.ToString());               
                entityList.Add("100");
                entityList.Add("AcDbLine");
                entityList.Add("10");
                entityList.Add(Point1.X.ToString("f5"));
                entityList.Add("20");
                entityList.Add(Point1.Y.ToString("f5"));
                entityList.Add("30");
                entityList.Add(Point1.Z.ToString("f5"));
                entityList.Add("11");
                entityList.Add(Point2.X.ToString("f5"));
                entityList.Add("21");
                entityList.Add(Point2.Y.ToString("f5"));
                entityList.Add("31");
                entityList.Add(Point2.Z.ToString("f5"));
                entityList.Add("0");

                return entityList;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public DXFLine(List<string> fileSection, int entityNumber)
        {
            Point1 = new Vector3();
            Point2 = new Vector3();
            Col = System.Drawing.Color.Red;
            DxfColor = new DxfColor();
            Point1.X = Convert.ToDouble(fileSection[4]);
            Point1.Y = Convert.ToDouble(fileSection[6]);
            Point1.Z = Convert.ToDouble(fileSection[8]);
            Point2.X = Convert.ToDouble(fileSection[10]);
            Point2.Y = Convert.ToDouble(fileSection[12]);
            Point2.Z = Convert.ToDouble(fileSection[14]);
            int c = 7;
            int.TryParse(fileSection[0], out c);
            DxfColor = DXFColorConverter.ToDxfColor(c);
            ID = entityNumber;
            Type = EntityType.Line;
        }
        public DXFLine(PointCyl pt1, PointCyl pt2)
        {
            try
            {
                double x1 = pt1.R * Math.Cos(pt1.ThetaRad);
                double y1 = pt1.R * Math.Sin(pt1.ThetaRad);
                double z1 = pt1.Z;
                double x2 = pt2.R * Math.Cos(pt2.ThetaRad);
                double y2 = pt2.R * Math.Sin(pt2.ThetaRad);
                double z2 = pt2.Z;

                Col = pt1.Col;
                Point1 = new Vector3(x1, y1, z1);
                Point2 = new Vector3(x2, y2, z2);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DXFLine(Vector3 pt1, Vector3 pt2)
        {
            try
            {
                Point1 = pt1;
                Point2 = pt2;

                Type = EntityType.Line;
                Col = pt1.Col;
                DxfColor = new DxfColor();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DXFLine(double x1In, double y1In, double z1In, double x2In, double y2In, double z2In)
         {
            Point1 = new Vector3(x1In, y1In, z1In);
            Point2 = new Vector3(x2In, y2In, z2In);

            Type = EntityType.Line;
            Col = System.Drawing.Color.Red;
            DxfColor = new DxfColor();
         }
        public DXFLine(double x1In, double y1In, double z1In, double x2In, double y2In, double z2In,System.Drawing.Color color)
        {
            Point1 = new Vector3(x1In, y1In, z1In);
            Point2 = new Vector3(x2In, y2In, z2In);

            Type = EntityType.Line;
            Col = color;
            DxfColor =  DXFColorConverter.ToDxfColor(color);

        }
        public DXFLine()
        {
            Point1 = new Vector3();
            Point2 = new Vector3();
            Col = System.Drawing.Color.Red;
            DxfColor = new DxfColor();
            Point1.X = 0;
            Point1.Y = 0;
            Point1.Z = 0;
            Point2.X = 0;
            Point2.Y = 0;
            Point2.Z = 0;
            int c = 7;

            DxfColor = DXFColorConverter.ToDxfColor(c);
            ID = 21;
            Type = EntityType.Line;
           
     
        }
       
        public DXFLine(Line lineIn)
        {
            Point1 = new Vector3(lineIn.Point1);
            Point2 = new Vector3(lineIn.Point2);
            Col = lineIn.Col;
            DxfColor = new DxfColor();

            DxfColor = DXFColorConverter.ToDxfColor(lineIn.Col); 
            ID = 21;
            Type = EntityType.Line;
           
        }
        
    }
}
