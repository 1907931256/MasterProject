using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AWJModel
{
    /// <summary>
    /// converts material to xml file and vice versa
    /// </summary>
    public class MaterialsFile
    {
        public static void Save(string fileName, MaterialDictionary materialDictionary)
        {
            
        }
        public static MaterialDictionary Open(string fileName)
        {
            MaterialDictionary dictionary = new MaterialDictionary();
            if (fileName != null && fileName != "" && System.IO.File.Exists(fileName))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                XmlNodeList materialList = doc.SelectNodes("materials/material");

                foreach (XmlNode material in materialList)
                {
                    string name = material.Attributes["name"].Value.ToString();
                    double cutMachI = double.Parse(material.Attributes["cutMachIndex"].Value);
                    double millMachI = double.Parse(material.Attributes["millMachIndex"].Value);
                    double thickness = double.Parse(material.Attributes["thickness"].Value);
                    double thetaCrit = double.Parse(material.Attributes["thetaCrit"].Value);
                    double modulus = double.Parse(material.Attributes["modulusElastic"].Value);
                    double yieldStr = double.Parse(material.Attributes["yieldStr"].Value);
                    double poissonsRatio = double.Parse(material.Attributes["poissonsRatio"].Value);
                    double density = double.Parse(material.Attributes["density"].Value);
                    MaterialType type = (MaterialType)Enum.Parse(typeof(MaterialType), material.Attributes["type"].Value, true);
                    dictionary.AddMaterial(new Material(type, name,thickness, millMachI, cutMachI, thetaCrit, modulus, yieldStr, poissonsRatio, density));
                }
            }
            return dictionary;

        }
      
    }
}
