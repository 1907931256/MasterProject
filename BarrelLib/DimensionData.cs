using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrelLib
{
    
    public class DimensionData
    {
        
        public double Length { get; set; }        
        public double LandMinDiam{ get; set; }
        public double LandMaxDiam { get; set; }
        public int GrooveCount{ get; set; }        
        public double LandActualDiam { get; set; }
        public double LandMinWidth { get; set; }
        public double LandMaxWidth { get; set; }       
        public double GrooveMinDiam { get; set; }
        public double GrooveMaxDiam { get; set; }
        public double FirstGrooveThetaOffset { get; set; }
        public double LandNominalDiam { get; set; }
        public double GrooveNominalDiam { get; set; }
        public double GrooveMaxWidthTheta { get; set; }
        public double GrooveMinWidthTheta { get; set; }
        public double GrooveMaxWidth { get; set; }
        public double GrooveMinWidth { get; set; }
        public double MaxCircumference { get; set; }
        public double MinCircumference { get; set; }
        public double NomCircumference { get; set; }
        public double RingCalibrationDiam { get; set; }
        
        TwistProfile _twistProfile;

        void ReadFile(string fileName)
        {
          var b =   BarrelFile.Open(fileName);
            Length = b.DimensionData.Length;
            GrooveCount = b.DimensionData.GrooveCount;
            LandMaxWidth = b.DimensionData.LandMaxWidth;
            LandMaxDiam = b.DimensionData.LandMaxDiam;
            LandMinDiam = b.DimensionData.LandMinDiam;
            GrooveMaxDiam = b.DimensionData.GrooveMaxDiam;
            GrooveMinDiam = b.DimensionData.GrooveMinDiam;
        }
        Tuple<double, double> GetGrooveMinEnds(double z)
        {            
            double thStart = (FirstGrooveThetaOffset + _twistProfile.ThetaRadAt(z))-_grooveHWMin;           
            double thEnd = thStart + (_twistProfile.DirectionSign * 2* _grooveHWMin);           
            var ends = new Tuple<double, double>(thStart, thEnd);
            return ends;
        }
        Tuple<double, double> GetGrooveMaxEnds(double z)
        {            
            double thStart = (FirstGrooveThetaOffset + _twistProfile.ThetaRadAt(z)) - _grooveHWMax;
            double thEnd = thStart + (_twistProfile.DirectionSign * 2 * _grooveHWMax);           
            var ends = new Tuple<double, double>(thStart, thEnd);
            return ends;
        }
        public int GetGrooveNumber(double z, double thetaRad)
        {
            var pi2 = Math.PI * 2.0;
            thetaRad %= pi2;
            var minEnds = GetGrooveMinEnds(z);
            var maxEnds = GetGrooveMaxEnds(z);
            var thWrap = thetaRad + pi2;
            double dTh = pi2 / GrooveCount;
            int grooveNumber = -1;
            for (int i = 0; i < GrooveCount; i++)
            {
                double minWStart = (minEnds.Item1 + (_twistProfile.DirectionSign * i * dTh)) % pi2;   
                double minWEnd = (minEnds.Item2 + (_twistProfile.DirectionSign * i * dTh))% pi2;
                double maxWStart = (minEnds.Item1 + (_twistProfile.DirectionSign * i * dTh)) % pi2;
                double maxWEnd = (minEnds.Item2 + (_twistProfile.DirectionSign * i * dTh)) % pi2;

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
                r = (GrooveNominalDiam/ 2.0) ;
            }
            else
            {
                r = LandNominalDiam / 2.0;
            }
            return r;
        }
        public double MaxRadiusAt(double z, double thetaRad)
        {
            double r = 0;
            if (GetGrooveNumber(z, thetaRad) >= 0)
            {
                r = GrooveMaxDiam / 2.0;
            }
            else
            {
                r = LandMaxDiam / 2.0;
            }
            return r;
        }
        
        public double MinRadiusAt(double z, double thetaRad)
        {
            double r = 0;
            if (GetGrooveNumber(z, thetaRad) >= 0)
            {
                r = GrooveMinDiam / 2.0;
            }
            else
            {
                r = LandMinDiam / 2.0;
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
            
            BuildM2Dims();
            _twistProfile = new TwistProfile(BarrelType.M2_50_Cal);
            InitValues();
        }
       void InitValues()
        {            
            MaxCircumference = Math.PI * LandMaxDiam;
            MinCircumference = Math.PI * LandMinDiam;
            LandNominalDiam = (LandMaxDiam + LandMinDiam) / 2.0; 
            GrooveNominalDiam = (GrooveMinDiam + GrooveMaxDiam) / 2.0;
            NomCircumference = (MaxCircumference + MinCircumference) / 2.0;
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
            Length = 48;
            RingCalibrationDiam = .5;
            LandMaxDiam = .501;
            LandMinDiam = .4985;
            LandNominalDiam = (LandMinDiam + LandMaxDiam) / 2.0;
            LandActualDiam = LandNominalDiam;
            GrooveCount = 8;
            GrooveMinDiam = .509;
            GrooveMaxDiam = .513;            
            LandMinWidth = .0537;
            LandMaxWidth = .0589;            
            FirstGrooveThetaOffset = 0;
        }
        void BuildM284Dims()
        {
            Length = 240;
            RingCalibrationDiam = 6.0;
            LandMaxDiam = 6.115;
            LandMinDiam = 6.113;
            LandNominalDiam = (LandMinDiam + LandMaxDiam) / 2.0;
            LandActualDiam = LandNominalDiam;
            GrooveCount = 48;
            GrooveMinDiam = 6.210;
            GrooveMaxDiam = 6.205;
            LandMinWidth = .1554;
            LandMaxWidth = .1632;
            FirstGrooveThetaOffset = 0;
        }
        void BuildM240Dims()
        {
            Length = 48;
            RingCalibrationDiam = .3;
            LandMaxDiam = .3022;
            LandMinDiam = .2996;
            LandNominalDiam = (LandMinDiam + LandMaxDiam) / 2.0;
            LandActualDiam = LandNominalDiam;
            GrooveCount = 4;
            GrooveMinDiam = .3065;
            GrooveMaxDiam = .3095;           
            LandMinWidth = .0457;
            LandMaxWidth = .060;
            FirstGrooveThetaOffset = 0;
        }
        void Build50mmDims()
        {
            Length = 48;
            RingCalibrationDiam = 2.0;
            LandMaxDiam = 1.969;
            LandMinDiam = 1.972;
            LandNominalDiam = (LandMinDiam + LandMaxDiam) / 2.0;
            LandActualDiam = LandNominalDiam;
            GrooveCount = 24;
            GrooveMinDiam = 2.007;
            GrooveMaxDiam = 2.023;
            GrooveMaxWidth = .1457;
            GrooveMinWidth = .129;
            FirstGrooveThetaOffset = 0;
        }
        double _grooveHWMin;
        double _grooveHWMax;
        void BuildM242Dims()
        {
            Length = 78;
            RingCalibrationDiam = 1.0;
            LandMaxDiam = .9882;
            LandMinDiam = .985;
            LandNominalDiam = (LandMinDiam + LandMaxDiam) / 2.0;
            LandActualDiam = LandNominalDiam;
            GrooveCount = 18;
            GrooveMinDiam = 1.0236;
            GrooveMaxDiam = 1.0315;           
            LandMinWidth = .0578;
            LandMaxWidth = .0718;           
            FirstGrooveThetaOffset = 0;
        }
    }
}
