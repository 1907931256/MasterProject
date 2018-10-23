using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FileIOLib;
namespace AbMachModel
{   

    public class MrrEquFile
    {
        static private string defaultFileName = "mrrEquations.xml";

        static public MrrEquDictionary Open()
        {
           return Open(defaultFileName);
        }

        static public MrrEquDictionary Open(string fileName)
        {
            var mrrEquations = new MrrEquDictionary();

            if (fileName != null && fileName != "" && System.IO.File.Exists(fileName))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                XmlNodeList coeffList = doc.SelectNodes("MRR/coeffs");

                foreach (XmlNode coeff in coeffList)
                {
                    string t = coeff.Attributes["type"].Value;
                    int type = 0;
                    if (int.TryParse(t, out type))
                    {
                        List<double> coeffs = new List<double>();
                        foreach (XmlNode value in coeff)
                        {
                            string indexS = value.Attributes["index"].Value;
                            int indexOut = 0;
                            if (int.TryParse(indexS, out indexOut))
                            {
                                string valS = value.InnerText;
                                double valOut = 0;
                                if (double.TryParse(valS, out valOut))
                                {
                                    coeffs.Insert(indexOut, valOut);
                                }
                            }
                        }
                        double[] valArray = coeffs.ToArray();
                        mrrEquations.AddEquation(type, valArray);
                    }
                }
            }

            return mrrEquations;
        }
        static public void Save(MrrEquDictionary equations, string fileName)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CloseOutput = true;
            settings.Indent = true;
            
            XmlWriter writer = XmlWriter.Create(fileName,settings);
            writer.WriteStartDocument(true);
            writer.WriteStartElement("MRR");
            foreach(int index in equations.IndexList)
            {
                writer.WriteStartElement("coeffs");
                writer.WriteAttributeString("type", index.ToString());
                double[] coefficients = equations.GetEquation(index);
                for (int i=0;i<coefficients.Length;i++)
                {
                    writer.WriteStartElement("coeff");
                    writer.WriteAttributeString("index", i.ToString());
                    writer.WriteString(coefficients[i].ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

        }
        
        
    }
        
}
