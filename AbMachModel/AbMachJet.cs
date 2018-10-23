using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace AbMachModel
{
    /// <summary>
    /// contains abmach model jet
    /// </summary>
    public class XSecJet:AbMachJet
    {
        new public double RemovalRateAt(double radius)
        {
            int index =(int)Math.Ceiling( radius / _meshSize);
            return mrrEquation.GetSumAt(index);
        }
        public double RemovalRateAt(int jetRIndex)
        {
            return mrrEquation.GetSumAt(jetRIndex);
        }
        public XSecJet(double meshSize, double diameter,  int equationIndex):base(meshSize,diameter,equationIndex)
        {
            
        }
        public XSecJet():base()
        {

        }
    }
    public class AbMachJet
    {


        protected MRREquation mrrEquation;
       
        protected double _diameter;
        protected double _radius;
        protected double _meshSize;
        [XmlIgnore]
        public int EquationIndex { get; private set; }
        [XmlElement("EquationIndex")]
        public int xmlEquationIndex { get { return EquationIndex; } set { EquationIndex = value; } }
        [XmlIgnore]            
        public double Diameter { get { return _diameter; } private set { _diameter = value; } }
        [XmlElement("Diameter")]
        public double xmlDiameter { get { return Diameter; } set { Diameter = value; } }
       

       
        public double RemovalRateAt(double radius)
        {
            return removalRateAt(radius / _radius);
        }
       public double RemovalRateAt(int jetXindex,int jetYIndex)
        {
            return mrrEquation.GetPointAt(jetXindex, jetYIndex);
        }
      
        private double removalRateAt(double normalizedRadius)
        {            
            return (normalizedRadius >= 0 && normalizedRadius <= 1) ?
                mrrEquation.GetPointAt(normalizedRadius) : 0;           
        }      
      
        private void getDefValues()
        {
            _diameter = .1;
            _radius = _diameter / 2;        }
        private void initValues()
        {          
            mrrEquation = new MRREquation(EquationIndex,_meshSize, _radius);           
        }

        public AbMachJet(double meshSize, double diameter,  int equationIndex)
        {
            EquationIndex = equationIndex;
            _meshSize = meshSize;
            if ( diameter > 0)
            {
                _diameter = diameter;
                _radius = diameter / 2;
                
            }
            else
            {
                getDefValues();
            }
            initValues();
        }
       
        public AbMachJet()
        {
            EquationIndex = 1;
            _meshSize = .001;
            getDefValues();
            initValues();
        }
    }
}
