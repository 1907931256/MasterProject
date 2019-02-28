using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrelLib
{
    
    public class DimensionData
    {
        
        public double Length { get { return _length; } }        
        public double LandMinDiam{get{ return _landMinDiam;}}
        public double LandMaxDiam { get { return _landMaxDiam; } }
        public int GrooveCount{ get{ return _grooveCount;}}        
        public double ActualLandDiam {get { return _landActualDiam; }  set  {  _landActualDiam = value;  }   }
        public double LandMinWidth { get { return _landMinWidth; } }
        public double LandMaxWidth { get { return _landMaxWidth; } }       
        public double GrooveMinDiam { get { return _grooveMinDiam ; }}
        public double GrooveMaxDiam { get { return _grooveMaxDiam ; } }
        public double FirstGrooveThetaOffset { get { return _firstGrooveThetaOffset; }}
        public double LandNominalDiam {get{  return _landNomDiam; } }
        public double GrooveNominalDiam { get { return _grooveNomDiam ; } }
        public double GrooveMaxWidthTheta {get{ return _grooveMaxWidthTheta;  } }
        public double GrooveMinWidthTheta { get {  return _grooveMinWidthTheta;     } }
        public double GrooveMaxWidth { get{   return _grooveMaxWidth;} }
        public double GrooveMinWidth { get {   return _grooveMinWidth; } }
        public double MaxCircumference { get { return _maxCircumference; } }
        public double MinCircumference {  get { return _minCircumference; } }
        public double NomCircumference { get { return _nomCircumference; } }

        double _landActualDiam;
        double pi2;
        double _grooveMaxWidth;
        double _grooveMinWidth;
        double _minCircumference;
        double _maxCircumference;
        double _nomCircumference;
        double _grooveMaxWidthTheta;
        double _grooveMinWidthTheta;

        double _length;

        double _landMaxDiam;
        double _landMinDiam;
        double _landNomDiam;

        int _grooveCount;

        double _grooveNomDiam;
        double _grooveMinDiam;
        double _grooveMaxDiam;
      
        double _landMinWidth;
        double _landMaxWidth;
        
        double _firstGrooveThetaOffset;        
        double _grooveSpacingTheta;       
        TwistProfile _twistProfile;

        void ReadFile(string fileName)
        {
          var b =   BarrelFile.Open(fileName);
            _length = b.DimensionData.Length;
            _grooveCount = b.DimensionData.GrooveCount;
            _landMaxWidth = b.DimensionData.LandMaxWidth;
            _landMaxDiam = b.DimensionData.LandMaxDiam;
            _landMinDiam = b.DimensionData.LandMinDiam;
            _grooveMaxDiam = b.DimensionData.GrooveMaxDiam;
            _grooveMinDiam = b.DimensionData.GrooveMinDiam;
        }
        Tuple<double, double> GetGrooveMinEnds(double z)
        {            
            double thStart = (_firstGrooveThetaOffset + _twistProfile.ThetaRadAt(z))-_grooveHWMin;           
            double thEnd = thStart + (_twistProfile.Direction * 2* _grooveHWMin);           
            var ends = new Tuple<double, double>(thStart, thEnd);
            return ends;
        }
        Tuple<double, double> GetGrooveMaxEnds(double z)
        {            
            double thStart = (_firstGrooveThetaOffset + _twistProfile.ThetaRadAt(z)) - _grooveHWMax;
            double thEnd = thStart + (_twistProfile.Direction * 2 * _grooveHWMax);           
            var ends = new Tuple<double, double>(thStart, thEnd);
            return ends;
        }
        public int GetGrooveNumber(double z, double thetaRad)
        {
            thetaRad %= pi2;
            var minEnds = GetGrooveMinEnds(z);
            var maxEnds = GetGrooveMaxEnds(z);
            var thWrap = thetaRad + pi2;
            double dTh = pi2 / _grooveCount;
            int grooveNumber = -1;
            for (int i = 0; i < _grooveCount; i++)
            {
                double minWStart = (minEnds.Item1 + (_twistProfile.Direction * i * dTh)) % pi2;   
                double minWEnd = (minEnds.Item2 + (_twistProfile.Direction * i * dTh))% pi2;
                double maxWStart = (minEnds.Item1 + (_twistProfile.Direction * i * dTh)) % pi2;
                double maxWEnd = (minEnds.Item2 + (_twistProfile.Direction * i * dTh)) % pi2;

                if (_twistProfile.Direction < 0 )
                {
                    minWStart += pi2;
                    minWEnd += pi2;
                    maxWStart += pi2;
                    maxWEnd += pi2;

                    double swap = minWEnd;
                    minWEnd = minWStart;
                    minWStart = swap;

                    swap = maxWEnd;
                    maxWEnd = maxWStart;
                    maxWStart = swap;

                }
                    
                double thWindow = maxWEnd - maxWStart;
                //check for wrap around or reverse order
                if (maxWStart > maxWEnd)
                {
                    maxWEnd += pi2;                       
                }
                if(minWStart>minWEnd)
                {
                    minWEnd += pi2;
                }
                if ((thetaRad <= minWEnd && thetaRad >= minWStart) || (thWrap <= minWEnd && thWrap >= minWStart))
                {
                    grooveNumber = i;
                    break;
                }
                else if ((thetaRad <= maxWEnd && thetaRad >= minWEnd) || (thWrap <= maxWEnd && thWrap >= minWEnd)||
                         (thetaRad <= minWStart && thetaRad >= maxWStart) || (thWrap <= minWStart && thWrap >= maxWStart))

                {
                    grooveNumber = -2;
                    break;
                }
            }
            return grooveNumber;
        }
             
        
        public double NomRadiusAt(double z, double thetaRad)
        {
            double r = 0;
            if (GetGrooveNumber(z, thetaRad)>=0)
            {
                r = (_grooveNomDiam/ 2.0) ;
            }
            else
            {
                r = _landNomDiam / 2.0;
            }
            return r;
        }
        public double MaxRadiusAt(double z, double thetaRad)
        {
            double r = 0;
            if (GetGrooveNumber(z, thetaRad) >= 0)
            {
                r = _grooveMaxDiam / 2.0;
            }
            else
            {
                r = _landMaxDiam / 2.0;
            }
            return r;
        }
        
        public double MinRadiusAt(double z, double thetaRad)
        {
            double r = 0;
            if (GetGrooveNumber(z, thetaRad) >= 0)
            {
                r = _grooveMinDiam / 2.0;
            }
            else
            {
                r = _landMinDiam / 2.0;
            }
           
            return r;
        }
        public DimensionData(string fileName)
        {
            ReadFile(fileName);
            InitValues();
        }
        
        public DimensionData()
        {
            pi2 = Math.PI * 2;
            BuildM2Dims();
            _twistProfile = new TwistProfile(BarrelType.M2_50_Cal);
            InitValues();
        }
       void InitValues()
        {
            pi2 = Math.PI * 2;
            _grooveSpacingTheta = Math.PI * 2 / _grooveCount;

            _maxCircumference = Math.PI * _landMaxDiam;
            _minCircumference = Math.PI * _landMinDiam;
            
           // _grooveMaxWidth = (_maxCircumference - (_landMinWidth * _grooveCount)) / _grooveCount;
           // _grooveMinWidth = (_minCircumference - (_landMaxWidth * _grooveCount)) / _grooveCount;
           // _grooveMaxWidthTheta = pi2 * (_grooveMaxWidth / _maxCircumference);
           // _grooveMinWidthTheta = pi2 * (_grooveMinWidth / _minCircumference);
           // _grooveHWMin = _grooveMinWidthTheta / 2.0;
           // _grooveHWMax = _grooveMaxWidthTheta / 2.0;
            _landNomDiam = (_landMaxDiam + _landMinDiam) / 2.0; 
            _grooveNomDiam = (_grooveMinDiam + _grooveMaxDiam) / 2.0;
            _nomCircumference = (_maxCircumference + _minCircumference) / 2.0;
        }
        public DimensionData(BarrelType type)
        {
           
            switch(type)
            {
                case BarrelType.M2_50_Cal:
                default:
                    BuildM2Dims();
                    break;
                case BarrelType.M242_25mm:
                    BuildM242Dims();
                    break;               
                case BarrelType.M284_155mm:
                    BuildM284Dims();
                    break;
                case BarrelType.M240_762mm:
                    BuildM240Dims();
                    break;
                case BarrelType.M_50mm:
                    Build50mmDims();
                    break;
            }
            _twistProfile = new TwistProfile(type);
            InitValues();
        }
        void BuildM2Dims()
        {
            _length = 48;
            _landMaxDiam = .501;
            _landMinDiam = .4985;
            _landNomDiam = (_landMinDiam + _landMaxDiam) / 2.0;
            _landActualDiam = _landNomDiam;
            _grooveCount = 8;
            _grooveMinDiam = .509;
            _grooveMaxDiam = .513;            
            _landMinWidth = .0537;
            _landMaxWidth = .0589;            
            _firstGrooveThetaOffset = 0;
        }
        void BuildM284Dims()
        {
            _length = 240;
            _landMaxDiam = 6.103;
            _landMinDiam = 6.100;
            _landNomDiam = (_landMinDiam + _landMaxDiam) / 2.0;
            _landActualDiam = _landNomDiam;
            _grooveCount = 48;
            _grooveMinDiam = 6.206;
            _grooveMaxDiam = 6.200;
            _landMinWidth = .1554;
            _landMaxWidth = .1632;
            _firstGrooveThetaOffset = 0;
        }
        void BuildM240Dims()
        {
            _length = 48;
            _landMaxDiam = .3022;
            _landMinDiam = .2996;
            _landNomDiam = (_landMinDiam + _landMaxDiam) / 2.0;
            _landActualDiam = _landNomDiam;
            _grooveCount = 4;
            _grooveMinDiam = .3065;
            _grooveMaxDiam = .3095;           
            _landMinWidth = .0457;
            _landMaxWidth = .060;
            _firstGrooveThetaOffset = 0;
        }
        void Build50mmDims()
        {
            _length = 48;
            _landMaxDiam = 1.969;
            _landMinDiam = 1.972;
            _landNomDiam = (_landMinDiam + _landMaxDiam) / 2.0;
            _landActualDiam = _landNomDiam;
            _grooveCount = 24;
            _grooveMinDiam = 2.007;
            _grooveMaxDiam = 2.023;
            _grooveMaxWidth = .1457;
            _grooveMinWidth = .129;
            _firstGrooveThetaOffset = 0;
        }
        double _grooveHWMin;
        double _grooveHWMax;
        void BuildM242Dims()
        {           
            _length = 78;
            _landMaxDiam = .9882;
            _landMinDiam = .985;
            _landNomDiam = (_landMinDiam + _landMaxDiam) / 2.0;
            _landActualDiam = _landNomDiam;
            _grooveCount = 18;
            _grooveMinDiam = 1.0236;
            _grooveMaxDiam = 1.0315;           
            _landMinWidth = .0578;
            _landMaxWidth = .0718;           
            _firstGrooveThetaOffset = 0;
        }
    }
}
