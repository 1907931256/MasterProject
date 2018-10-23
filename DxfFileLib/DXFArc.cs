using System;
using System.Collections.Generic;
using GeometryLib;

namespace DwgConverterLib
{
    public class DXFArc : Arc, IDXFEntity
    {
        public DxfColor DxfColor { get; set; }

        public List<string> AsDXFString()
        {

            List<string> entityList = new List<string>();
            entityList.Add("ARC");
            entityList.Add("5");
            entityList.Add(ID.ToString());
            entityList.Add("100");
            entityList.Add("AcDbEntity");
            entityList.Add("8");
            entityList.Add("1");
            entityList.Add("6");
            entityList.Add("SOLID");
            entityList.Add("62");
            entityList.Add((DXFColorConverter.ToDxfColor(Col)).ToString());
            entityList.Add("100");
            entityList.Add("AcDbCircle");
            entityList.Add("10");
            entityList.Add(Center.X.ToString("f5"));
            entityList.Add("20");
            entityList.Add(Center.Y.ToString("f5"));
            entityList.Add("30");
            entityList.Add(Center.Z.ToString("f5"));
            entityList.Add("40");
            entityList.Add(Radius.ToString("f5"));
            entityList.Add("210");
            entityList.Add("0.0");
            entityList.Add("220");
            entityList.Add("0.0");
            entityList.Add("230");
            entityList.Add("1.0");
            entityList.Add("100");
            entityList.Add("AcDbArc");
            entityList.Add("50");
            entityList.Add(StartAngleDeg.ToString("f5"));
            entityList.Add("51");
            entityList.Add(EndAngleDeg.ToString("f5"));
            entityList.Add("0");

            return entityList;
       

        }

        public DXFArc()
        {
            Center.X = 0;
            Center.Y = 0;
            Center.Z = 0;
            Radius = 0;
            int c = 7;
            Col = ColorConverter.ToColor(c);
            ClosedArc = false;
            StartAngleRad = 0;
            EndAngleRad = 0;
            ID = 0;
        }
        public DXFArc(Arc arc)
        {
            Center.X = arc.Center.X;
            Center.Y = arc.Center.Y;
            Center.Z = arc.Center.Z;
            Radius = arc.Radius;
            int c = 7;
            Col = ColorConverter.ToColor(c);
            ClosedArc = arc.ClosedArc;
            StartAngleRad = arc.StartAngleRad;
            EndAngleRad = arc.EndAngleRad;
            ID = arc.ID;
        }
        public DXFArc(List<string> fileSection, int entityNumber)
        {
            Center.X = Convert.ToDouble(fileSection[0]);
            Center.Y = Convert.ToDouble(fileSection[2]);
            Center.Z = Convert.ToDouble(fileSection[4]);
            Radius = Convert.ToDouble(fileSection[6]);
            double startAngle = Geometry.ToRadians(Convert.ToDouble(fileSection[10]));
            double endAngle = Geometry.ToRadians(Convert.ToDouble(fileSection[12]));
            if (startAngle < 0)
            {
                startAngle += 2 * Math.PI;
            }
            if (endAngle < 0)
            {
                endAngle += 2 * Math.PI;
            }
            StartAngleRad = startAngle;
            EndAngleRad = endAngle;
            ClosedArc = false;
            int c = 7;
            int.TryParse(fileSection[6], out c);
            Col = ColorConverter.ToColor(c);
            ID = entityNumber;
          
        }


      

    }
}
