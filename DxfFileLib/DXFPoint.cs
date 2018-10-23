using System;
using System.Collections.Generic;
using GeometryLib;
namespace DwgConverterLib
{
    public class DXFPoint:Vector3,IDXFEntity 
    {
        public DxfColor DxfColor { get; set; }
        public DXFPoint()
        {
            X = 0;
            Y = 0;
            Z = 0;
            ID = 0;           
            Col = new RGBColor();
            DxfColor = DxfColor.White;
        }
        public DXFPoint(Vector3 pt)
        {
            X = pt.X;
            Y = pt.Y;
            Z = pt.Z;
            ID = pt.ID;
            Col = pt.Col;
            DxfColor = DXFColorConverter.ToDxfColor(Col);
        }
        public DXFPoint(List<string> fileSection, int entityNumber)
        {
            X = Convert.ToDouble(fileSection[4]);
            Y = Convert.ToDouble(fileSection[6]);
            Z = Convert.ToDouble(fileSection[8]);
            ID = entityNumber++;
            int c = 4;
            int.TryParse(fileSection[0], out c);
            DxfColor = (DxfColor)c;

            Col = ColorConverter.ToColor(c);
        }
        public List<string> AsDXFString()
        {
            /*
            POINT
            5
            124
            330
            1F
            100
            AcDbEntity
              8
            0
            62
            color
            100
            AcDbPoint
             10
            -0.2027027027027035
             20
            6.112702702702703
             30
            0.0
              0
             * */

            List<string> entityList = new List<string>();
            entityList.Add("POINT");
            entityList.Add("5");
            entityList.Add(ID.ToString());
            entityList.Add("330");
            entityList.Add("1F");
            entityList.Add("100");
            entityList.Add("AcDbEntity");
            entityList.Add("8");
            entityList.Add("0");
            entityList.Add("62");
            entityList.Add(((int)DxfColor).ToString());//this should be color
            //entityList.Add(ColorConverter.ToDxfColor(Col).ToString());
            entityList.Add("100");
            entityList.Add("AcDbPoint");
            entityList.Add("10");
            entityList.Add(X.ToString("f5"));
            entityList.Add("20");
            entityList.Add(Y.ToString("f5"));
            entityList.Add("30");
            entityList.Add(Z.ToString("f5"));
            entityList.Add("0");

            return entityList;
        }
       

       
       
        
    }
}
