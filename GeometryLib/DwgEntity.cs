using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GeometryLib
{
    [System.Xml.Serialization.XmlInclude(typeof (Arc))]
    [System.Xml.Serialization.XmlInclude(typeof(Line))]    
    [System.Xml.Serialization.XmlInclude(typeof(Vector3))]
    public abstract class DwgEntity 
    {

        public EntityType Type { get; set; }
        public System.Drawing.Color Col { get; set; }
        public int ID { get; set; }
    
        
    }
}
