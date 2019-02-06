using System;
using System.Collections.Generic;
using GeometryLib;

namespace DwgConverterLib
{

    public class DxfFile
    {
        string fileName;
        
        
        List<DwgEntity> entities;       
        BoundingBox boundingBox;

        public static string Filter = "DXF files (*.dxf)|*.dxf|All files (*.*)|*.*";
        public String ShortName
        {
            get
            {
                return System.IO.Path.GetFileName(fileName);                     
            }
        }
        public string Path
        {
            get
            {               
                return System.IO.Path.GetDirectoryName(fileName);               
            }
        }
       
        public string FileName
        { 
            get 
            { 
                return fileName; 
            } 
            set 
            { 
                fileName = value; 
            } 
        }

        public BoundingBox BoundingBox
        {
            get
            {
                return boundingBox;
            }
        }
       
        
        public List<DwgEntity> Entities{
            get
            {
                return entities;
            }
            set
            {
                entities = value;
            }
        }
       
       
        private void initFile(List<string> file,string fileNameIn)
        {
            fileName = fileNameIn;            
            entities = DxfFileParser.Parse(file);
            boundingBox = new BoundingBox();
            getBoundingBox();
        }
       

        private void getBoundingBox()
        {
            List<BoundingBox> extentList = new List<BoundingBox>();

            foreach (DwgEntity entity in entities)
            {
                if (entity is Line)
                {
                    Line line = entity as Line;
                    extentList.Add(line.BoundingBox());
                }
                if (entity is Arc)
                {
                    Arc arc = entity as Arc;
                    extentList.Add(arc.BoundingBox());
                }
                if (entity is Vector3)
                {
                    Vector3 pt = entity as Vector3;
                    extentList.Add(pt.BoundingBox());
                }
            }
            boundingBox = BoundingBoxBuilder.Union(extentList.ToArray());
        }

        public DxfFile(List<string> file, string fileName)
        {
            initFile(file, fileName);
        }
        public DxfFile(string fileName)
        {
            List<string> file = FileIOLib.FileIO.ReadDataTextFile(fileName);

            initFile(file, FileName);
        }
        public DxfFile()
        {
            entities = new List<DwgEntity>();
            boundingBox = new BoundingBox();
            fileName = "";
        }

    }
}
