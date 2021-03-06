﻿using System;
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
            double x = 0;
            double.TryParse(fileSection[2], out x);
            Center.X = x;
            double y = 0;
            double.TryParse(fileSection[4], out y);
            Center.Y = y;
            double z = 0;
            double.TryParse(fileSection[6], out z);
            Center.Z = z;
            double r = 0;
            double.TryParse(fileSection[8], out r);
            Radius = r;
            double sa = 0;
            double.TryParse(fileSection[12], out sa);
            StartAngleRad = GeomUtilities.ToRadians(sa);
            double ea = 0;
            double.TryParse(fileSection[14], out ea);
            EndAngleRad = GeomUtilities.ToRadians(ea);
            ClosedArc = false;
            int c = 7;
            //int.TryParse(fileSection[6], out c);
            Col = ColorConverter.ToColor(c);
            ID = entityNumber;
          
        }


      

    }
}
