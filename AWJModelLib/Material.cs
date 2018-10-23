using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace AWJModel
{


    /// <summary>
    /// contains material constants
    /// </summary>
    public class Material
    {

        public double CriticalRemovalAngle { get; set; }
        public string Name { get; set; }
        public double Thickness { get; set; }
        public double CutMachinability { get; set; }
        public double MillMachinability { get; set; }
        public MaterialType Type { get; set; }
        public double ModulusElasticity { get; set; }
        public double YieldStrength { get; set; }
        public double PoissonsRatio { get; set; }
        public double Density { get; set; }
        static public string InvalidName;

        public Material()
        {
            InvalidName = "Invalid";            
            Type = MaterialType.Metal;
            Name = "Inconel";
            Thickness = 1.0;
            CutMachinability = 100.0;
            MillMachinability = 100.0;
            CriticalRemovalAngle = 70.0;
            ModulusElasticity = 1e11;
            YieldStrength = 1.21e9;
            PoissonsRatio = .283;
            Density = 7.65e3;
        }
        public Material(
            MaterialType type, string name,double thickness,
            double millMachinabilityIndex,double cutMachinabilityIndex,
            double criticalAngleRadians, double modulus, double yieldStr,
            double poissonsRatio, double density)
        {
            Type = type;
            Name = name.ToUpper();
            Thickness = thickness;
            MillMachinability = millMachinabilityIndex;
            CutMachinability = cutMachinabilityIndex;
            CriticalRemovalAngle = criticalAngleRadians;
            ModulusElasticity = modulus;
            YieldStrength = yieldStr;
            PoissonsRatio = poissonsRatio;
            Density = density;
        }
        public Material(MaterialType type, string name, double thickness, double millMachinabilityIndex,
            double cutMachinabilityIndex, double criticalAngleRadians)
        {
            Type = type;
            Name = name.ToUpper();
            Thickness = thickness;
            MillMachinability = millMachinabilityIndex;
            CutMachinability = cutMachinabilityIndex;
            CriticalRemovalAngle = criticalAngleRadians;
        }
    
    }
}
