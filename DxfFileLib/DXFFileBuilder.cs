using System;
using System.Collections.Generic;
using GeometryLib;
namespace DwgConverterLib
{
    public class DxfFileBuilder
    {


        static public void Save(List<DwgEntity> entities, string fileName, IProgress<int> progress)
        {
            try
            {
                string header1 = "0";
                string header2 = "SECTION";
                string header3 = "2";
                string header4 = "ENTITIES";
                string header5 = "0";

                string footer1 = "ENDSEC";
                string footer2 = "0";
                string footer3 = "EOF";
                int entityCount = 0;
                int totalCount = entities.Count;
                int j = 1;
                if (fileName != null && fileName != "")
                {
                    List<string> entityList = new List<string>(0);
                    entityList.Add(header1);
                    entityList.Add(header2);
                    entityList.Add(header3);
                    entityList.Add(header4);
                    entityList.Add(header5);

                    foreach (DwgEntity entity in entities)
                    {
                        if ((entity is Line)||(entity is DXFLine))
                        {
                            DXFLine line = new DXFLine((Line)entity );

                            if(line.ID ==0)
                                line.ID = entityCount++;
                            
                            entityList.AddRange(line.AsDXFString());
                        }
                        if (entity is Vector3)
                        {
                            DXFPoint point; 
                            if(entity is DXFPoint)
                            {
                                point = entity as DXFPoint;
                            }
                            else
                            {
                                point = new DXFPoint(entity as Vector3);
                            }
                            if (point.ID == 0)
                                point.ID = entityCount++;

                            entityList.AddRange(point.AsDXFString());                            
                        }
                        if ((entity is Arc) ||(entity is DXFCircle)||(entity is DXFArc))
                        {
                            DXFArc circ = new DXFArc(entity as Arc);
                            if(circ.ID==0)
                                circ.ID = entityCount++;
                            entityList.AddRange(circ.AsDXFString());
                        }
                        int p = (int)(100 * j++ / totalCount);
                        if (p % 5 == 0)
                        {
                            progress.Report(p);
                        }

                    }
                    entityList.Add(footer1);
                    entityList.Add(footer2);
                    entityList.Add(footer3);
                    System.IO.File.WriteAllLines(fileName, entityList.ToArray());
                }
            }
            catch (Exception )
            {
                throw ;
            }

        }
        static public void Save(List<DwgEntity> entities, string fileName)
        {
            try
            {
                string header1 = "0";
                string header2 = "SECTION";
                string header3 = "2";
                string header4 = "ENTITIES";
                string header5 = "0";

                string footer1 = "ENDSEC";
                string footer2 = "0";
                string footer3 = "EOF";
                int entityCount = 0;
               
                if (fileName != null && fileName != "")
                {
                    List<string> entityList = new List<string>(0);
                    entityList.Add(header1);
                    entityList.Add(header2);
                    entityList.Add(header3);
                    entityList.Add(header4);
                    entityList.Add(header5);

                    foreach (DwgEntity entity in entities)
                    {
                        if ((entity is Line) || (entity is DXFLine))
                        {
                            DXFLine line = new DXFLine((Line)entity);

                            if (line.ID == 0)
                                line.ID = entityCount++;

                            entityList.AddRange(line.AsDXFString());
                        }
                        if (entity is Vector3)
                        {
                            DXFPoint point;
                            if (entity is DXFPoint)
                            {
                                point = entity as DXFPoint;
                            }
                            else
                            {
                                point = new DXFPoint(entity as Vector3);
                            }
                            if (point.ID == 0)
                                point.ID = entityCount++;

                            entityList.AddRange(point.AsDXFString());
                        }
                        if ((entity is Arc) || (entity is DXFCircle) || (entity is DXFArc))
                        {
                            DXFArc circ = new DXFArc(entity as Arc);
                            if (circ.ID == 0)
                                circ.ID = entityCount++;
                            entityList.AddRange(circ.AsDXFString());
                        }
                       

                    }
                    entityList.Add(footer1);
                    entityList.Add(footer2);
                    entityList.Add(footer3);
                    System.IO.File.WriteAllLines(fileName, entityList.ToArray());
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
