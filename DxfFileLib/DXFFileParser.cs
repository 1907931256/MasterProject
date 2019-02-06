using System.Collections.Generic;
using GeometryLib;
using System;
namespace DwgConverterLib
{
    public class DxfFileParser
    {
        static public List<Vector3> BuildPointList(List<DwgEntity> entityList, double segmentLength)       
        {
            try
            {
                var ptList = new List<Vector3>();
                foreach(DwgEntity e in entityList)
                {
                    if(e is DXFLine line)
                    {
                        double delta = segmentLength / line.Length;
                        int count = (int) Math.Round(1.0 / delta);                       
                        for (int i = 0; i < count; i++)
                        {
                           double x  =  line.Point1.X +  (line.Point2.X - line.Point1.X) * delta * i;
                           double y  =line.Point1.Y +  (line.Point2.Y - line.Point1.Y) * delta * i;
                           double z  = line.Point1.Z +  (line.Point2.Z - line.Point1.Z) * delta * i;
                           ptList.Add(new Vector3(x, y, z));
                        }
                    }
                    if(e is DXFArc arc)
                    {
                        double delta = segmentLength / arc.Length;
                        double dAng = arc.SweepAngleRad * delta;
                        int count = (int)Math.Round(1.0 / delta);
                        double startAngle = Math.Min(arc.StartAngleRad, arc.EndAngleRad);
                        for (int i = 0; i < count; i++)
                        {
                            double x = arc.Center.X + arc.Radius * Math.Cos(startAngle + (i * dAng));
                            double y = arc.Center.Y + arc.Radius * Math.Sin(startAngle + (i * dAng));
                            double z = arc.Center.Z;
                            ptList.Add(new Vector3(x, y, z));
                        }
                    }
                }
                var xlist = new List<double>();
                foreach(Vector3 pt in ptList)
                {
                    xlist.Add(pt.X);
                }
                var ptArr = ptList.ToArray();
                Array.Sort(xlist.ToArray(), ptArr);
                ptList.Clear();
                ptList.AddRange(ptArr);
                return ptList;
            }
            catch (System.Exception)
            {

                throw;
            }
            
        }
        /// <summary>
        /// parses dxf file into list of DwgEntity objects
        /// </summary>
        /// <returns></returns>
        static public List<DwgEntity> Parse(List<string> text)
        {
            List<DwgEntity> entities = new List<DwgEntity>();

            int i = 0;
            int entityNumber = 1;
            List<string> fileSection = new List<string>();
            while (i < text.Count)
            {
                string str = text[i];
                if (str == "AcDbPoint")
                {
                    entities.Add(new DXFPoint(getFileSection(text, i , 7),entityNumber++));
                }
                if (str == "AcDbLine")
                {
                    entities.Add(new DXFLine(getFileSection(text, i - 2, 15),entityNumber++));
                    
                }
                if (str == "AcDbCircle")
                {

                    if (text[i + 10] == "AcDbArc")
                    {
                        entities.Add(new DXFArc(getFileSection(text, i , 26) ,entityNumber++));
                    }
                    else
                    {
                        entities.Add(new DXFCircle(getFileSection(text, i , 24), entityNumber++));
                    }
                }
                i++;
            }
            return entities;
        }
        static private List<string> getFileSection(List<string> file, int startingIndex, int sectionLength)
        {
            List<string> section = new List<string>();
            int index = startingIndex;
            while (index < file.Count && index < startingIndex + sectionLength)
            {
                section.Add(file[index++]);                
            }
            return section;
        }
    }
}
