using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbMachModel
{
    public class Materials
    {
        Dictionary<string, Material> Dictionary;
       
    }
  
    public class Material
    {
        string name;
        double thickness;
        double millMachinability;
        double cutMachinability;
        double criticalRemovalAngle;
        MaterialType type;

        public double CriticalRemovalAngle { get { return criticalRemovalAngle; } set { criticalRemovalAngle = value; } }
        public string Name { get { return name; } set { name = value; } }
        public double Thickness { get { return thickness; } set { thickness = value; } }
        public double CutMachinability { get { return cutMachinability; } set { cutMachinability = value; } }
        public double MillMachinability { get { return millMachinability; } set { millMachinability = value; } }
        public MaterialType Type { get { return type; } set { type = value; } }

        public Material()
        {
            type = MaterialType.Unknown;
            name = "unknown";
            thickness = 0;
            cutMachinability = 100;
            millMachinability = 100;
            criticalRemovalAngle = 0;
        }
        public Material(MaterialType type, string name, double thickness, double millMachinabilityIndex, double cutMachinabilityIndex, double criticalAngleDeg)
        {
            this.type = type;
            this.name = name;
            this.thickness = thickness;
            this.millMachinability = millMachinabilityIndex;
            this.cutMachinability = cutMachinabilityIndex;
            this.criticalRemovalAngle = criticalAngleDeg;
        }

    }
}
