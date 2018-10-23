using System;
using System.Collections.Generic;
using GeometryLib;
namespace DwgConverterLib
{

    public class DXFCircle: Arc, IDXFEntity
    {
        public DxfColor DxfColor { get; set; }

        public List<string> AsDXFString()
        {
            List<string> entityList = new List<string>();
            entityList.Add("CIRCLE");
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
            entityList.Add("0");

            return entityList;
        }
  
        public DXFCircle()
        {
            Center.X = 0;
            Center.Y = 0;
            Center.Z = 0;
            Radius = 0;
            int c = 7;
            Col = ColorConverter.ToColor(c);
            ClosedArc = true;
            ID = 0;
        }
        public DXFCircle(List<string> fileSection, int entityNumber)
        {
            Center.X = Convert.ToDouble(fileSection[4]);
            Center.Y = Convert.ToDouble(fileSection[6]);
            Center.Z = Convert.ToDouble(fileSection[8]);
            Radius = Convert.ToDouble(fileSection[10]);
            StartAngleRad = GeometryLib.Geometry.ToRadians(Convert.ToDouble(fileSection[20]));
            EndAngleRad = Geometry.ToRadians(Convert.ToDouble(fileSection[22]));
            int c = 7;
            int.TryParse(fileSection[0], out c);
            Col = ColorConverter.ToColor(c);
            if (Math.Abs(StartAngleRad - EndAngleRad) == 2 * Math.PI)
                ClosedArc = true;
            else
                closedArc = false;
            ID = entityNumber;
        }
       
      
        
        
    }
}
