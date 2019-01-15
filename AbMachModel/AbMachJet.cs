using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FileIOLib;
using GeometryLib;
namespace AbMachModel
{
    /// <summary>
    /// contains abmach model jet
    /// </summary>
    public class XSecJet
    {
        int GetIndex(double x)
        {
          return  Math.Min(_mrrX.Count - 1, (int)Math.Round(_mrrX.Count * (Math.Abs(x) / _jetR)));
        }
        public double GetMrr(double xJet)
        {                       
           return _mrrX[GetIndex(xJet)];
        }        
        List<double> _mrrX;
        private void BuildJet(int equationIndex,double dx)
        {
            double x = 0;
            double y = 0;
            var mrr = new MRRFunction(equationIndex);
            var f = mrr.GetFunc();
            _mrrX = new List<double>();
            var maxSum = double.MinValue;
            while (y <= 1)
            {
                double sum = 0;
                x = 0;
                while(x<=1)
                {
                    double r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                    double m = f(r);
                    if(m>0)
                    {
                        sum += m;
                    }
                    x += dx;
                }
                if(sum>maxSum)
                {
                    maxSum = sum;
                }
                _mrrX.Add(sum);
                y += dx;
            }
            var lines = new List<string>();
            var line = "";
            for(int i=0;i<_mrrX.Count;i++)
            {
                _mrrX[i] /= maxSum;
                line = _mrrX[i].ToString();
                lines.Add(line);
            }
           
           
            FileIO.Save(lines, "mrrGridTest.csv");
        }
        public List<Ray2> JetRays;
        double _jetR;
        double _dx;
        double _meshSize;

        public XSecJet( int equationIndex,double jetDiameter,double pointSpacing)
        {
            _jetR = jetDiameter/2;
            _dx = .001;
            BuildJet(equationIndex, _dx);
            _meshSize = pointSpacing;
            JetRays = new List<Ray2>();
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
