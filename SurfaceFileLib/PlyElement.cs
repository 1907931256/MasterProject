using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SurfaceFileLib
{
    public class PlyElement
    {
        public PlyElementType Type { get; set; }
        private string name;
        public string Name
        {
            get
            {
                return Type.ToString();
            }
            set
            {
                name = value;
            }
        }
        public bool containsVertex { get; set; }
        public bool containsNormal { get; set; }
        public bool containsColor { get; set; }
        public int Count { get; set; }
        public List<PlyProperty> Properties { get; private set; }
        public void AddProperty(PlyProperty property)
        {
            Properties.Add(property);
            //Properties.Add(property.Name, property);
        }

        public PlyElement()
        {
            Properties = new List<PlyProperty>();
            Type = PlyElementType.other;
        }
    }
}
