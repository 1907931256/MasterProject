using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileIOLib;
namespace BarrelLib
{

    public class DimensionData
    {

        public double Length { get; set; }
        public double LandMinDiam { get; set; }
        public double LandMaxDiam { get; set; }
        public int GrooveCount { get; set; }
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
        

        void InitValues(string filename)
        {
            var parms =  FileIO.ReadParamsTextFile(filename, new char[] { '=' });
            double val = 0;
            double.TryParse(parms[0],out val);
            Length = val;
            double.TryParse(parms[1], out val);
            LandMaxDiam = val;
            double.TryParse(parms[2], out val);
            LandMinDiam = val;
            int gc = 0;
            int.TryParse(parms[3], out gc);
            GrooveCount = gc;
            double.TryParse(parms[4],out val);
            GrooveMinDiam = val;
            double.TryParse(parms[5],out val);
            GrooveMaxDiam = val;
            double.TryParse(parms[6],out val);
            LandMinWidth = val;
            double.TryParse(parms[7],out val);
            LandMaxWidth = val;
            double.TryParse(parms[8],out val);
            FirstGrooveThetaOffset = val;

            MaxCircumference = Math.PI * LandMaxDiam;
            MinCircumference = Math.PI * LandMinDiam;
            LandNominalDiam = (LandMaxDiam + LandMinDiam) / 2.0;
            GrooveNominalDiam = (GrooveMinDiam + GrooveMaxDiam) / 2.0;
            NomCircumference = (MaxCircumference + MinCircumference) / 2.0;
            _grooveHWMax = Math.PI * (NomCircumference - (GrooveCount * LandMinWidth)) / GrooveCount;
            _grooveHWMin = Math.PI * (NomCircumference - (GrooveCount * LandMaxWidth)) / GrooveCount;
        }
        Barrel _barrel;
        public DimensionData(Barrel barrel,string filename)
        {
            _barrel = barrel;
            InitValues(filename);
        }
        double _grooveHWMax;
        double _grooveHWMin;
    }
}
