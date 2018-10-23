using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AbMachModel
{
    public class MaterialsFile
    {
        public static void Save(string fileName, Dictionary<string, Material> materialDictionary)
        {
            foreach (Material mat in materialDictionary.Values.ToList())
            {
                FileUtilities.XmlSerializer.SaveXML<
            }
        }
        public static Dictionary<string, Material> Open()
        {


            XmlDocument doc = new XmlDocument();
            doc.Load("materials.xml");
            XmlNodeList materialList = doc.SelectNodes("materials/material");
            var materialDictionary = new Dictionary<string, Material>();

            foreach (XmlNode material in materialList)
            {
                string name = material.Attributes["name"].Value.ToString();
                double cutMachI = double.Parse(material.Attributes["cutMachIndex"].Value);
                double millMachI = double.Parse(material.Attributes["millMachIndex"].Value);
                double thickness = double.Parse(material.Attributes["thickness"].Value);
                double thetaCrit = double.Parse(material.Attributes["thetaCrit"].Value);
                MaterialType type = (MaterialType)Enum.Parse(typeof(MaterialType), material.Attributes["type"].Value, true);
                materialDictionary.Add(name, new Material(type, name, thickness, millMachI, cutMachI, thetaCrit));
            }
            return materialDictionary;

        }
    }
}
