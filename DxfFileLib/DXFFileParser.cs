using System.Collections.Generic;
using GeometryLib;
namespace DwgConverterLib
{
    public class DxfFileParser
    {
        int entityNumber;

        /// <summary>
        /// parses dxf file into list of DwgEntity objects
        /// </summary>
        /// <returns></returns>
        public List<DwgEntity> Parse(List<string> text)
        {
            List<DwgEntity> entities = new List<DwgEntity>();

            int i = 0;
            entityNumber = 1;
            List<string> fileSection = new List<string>();
            while (i < text.Count)
            {
                string str = text[i];
                if (str == "AcDbPoint")
                {
                    entities.Add(new DXFPoint(getFileSection(text, i - 2, 7),entityNumber++));
                }
                if (str == "AcDbLine")
                {
                    entities.Add(new DXFLine(getFileSection(text, i - 2, 15),entityNumber++));
                    
                }
                if (str == "AcDbCircle")
                {

                    if (text[i + 10] == "AcDbArc")
                    {
                        entities.Add(new DXFArc(getFileSection(text, i - 10, 24) ,entityNumber++));
                    }
                    else
                    {
                        entities.Add(new DXFCircle(getFileSection(text, i - 2, 24), entityNumber++));
                    }
                }
                i++;
            }
            return entities;
        }
        private List<string> getFileSection(List<string> file, int startingIndex, int sectionLength)
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
