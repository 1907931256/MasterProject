using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurfaceFileLib
{
    /// <summary>
    /// contains property name and type
    /// </summary>
    public class PlyProperty
    {

        public string Name { get; set; }
        string typeName;
        public string TypeName
        {
            get
            {
                return typeName;
            }
            set
            {
                typeName = value;
            }
        }
        public PlyPropertyType Type { get; set; }
        public bool IsList { get; set; }
        public string ListCountTypeName { get; set; }

        public PlyProperty()
        {
            Type = PlyPropertyType.other;
        }

    }
}
