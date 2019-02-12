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
        //linear interpolate between scan points to get mrr
        public double GetMrr(double xJet)
        {
            double xnorm = Math.Min(1,Math.Abs(xJet / _jetR));
            double mrr = 0;
            for(int i=0;i<mrrList.Count-1;i++)
            {
                double x1 = mrrList[i].Item1;
                double x2 = mrrList[i + 1].Item1;
                double mrr1 = mrrList[i].Item2;
                double mrr2 = mrrList[i + 1].Item2;
                if(xnorm>=x1 && xnorm<x2)
                {
                    double m = (mrr2 - mrr1) / (x2 - x1);
                    mrr = ((xnorm - x1) * m) + mrr1;
                    break;
                }
            }
            return mrr;
        }        
       
        List<Tuple<double, double>> mrrList;
        //build jet from normalized csv scan of jet. x and r normalized to 1
        private void BuildJet(string csvFilename)
        {
            var stringArr = CSVFileParser.ParseFile(csvFilename);
            //x,mrr csvfilename 
            int headerRowCount = 1;
            int rowCount = stringArr.GetLength(0);
            int colCount = stringArr.GetLength(1);
            mrrList = new List<Tuple<double, double>>();
            for(int i=headerRowCount;i<rowCount;i++)
            {
                double x = 0;
                
                double mrr = 0;
                if(double.TryParse(stringArr[i, 0], out x) && double.TryParse(stringArr[i, 1], out mrr))
                {
                    mrrList.Add(new Tuple<double, double>(x, mrr));
                }               
            }
            
        }
        void BuildJet()
        {
            mrrList = new List<Tuple<double, double>>();
            mrrList.Add(new Tuple<double, double>(0,1));
            mrrList.Add(new Tuple<double, double>(1, 1));
        }
        double _jetR;
        //build jet from csv file scan profile normalized 
        public XSecJet(string csvJetProfile, double jetDiameter)
        {
            _jetR = jetDiameter / 2.0;
            BuildJet(csvJetProfile);
        }
        public XSecJet(double jetDiameter)
        {
            _jetR = jetDiameter / 2.0;
            BuildJet();
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
