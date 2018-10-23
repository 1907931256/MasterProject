using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileIOLib;
namespace InspectionLib
{
    /// <summary>
    /// contains list of target BarrelGrooveDepths as length profile
    /// used to create program based on removal rate
    /// </summary>
    public class BarrelProfile : List<BarrelGrooveDepth>
    {
        public double BarrelBlankLength { get { return _barrelBlankLength; } }
        public double BarrelStartLocation { get { return _xBarrelStartLocation; } }
        public double BarrelEndLocation { get { return _xBarrelEndLocation; } }

        private double _xBarrelStartLocation;
        private double _xBarrelEndLocation;
        private double _barrelBlankLength;


        public BarrelProfile(string filename)
        {
            try
            {
                var fileList = FileIO.ReadDataTextFile(filename);
                var words = FileIO.Split(fileList[1]);
                _barrelBlankLength = Convert.ToDouble(words[1]);
                words = FileIO.Split(fileList[2]);
                _xBarrelStartLocation = Convert.ToDouble(words[1]);
                words = FileIO.Split(fileList[3]);
                _xBarrelEndLocation = Convert.ToDouble(words[1]);

                for (int i = 5; i < fileList.Count; i++)
                {
                    words = FileIO.Split(fileList[i]);
                    if (words.Length == 7)
                    {
                        double xLocation = Convert.ToDouble(words[0]);
                        double thetaDegs = Convert.ToDouble(words[1]);
                        double twistDegs = Convert.ToDouble(words[2]);
                        double diamFinal = Convert.ToDouble(words[3]);
                        double diamAsIs = Convert.ToDouble(words[4]);
                        double targetDepth = Convert.ToDouble(words[5]);
                        double finalDepth = Convert.ToDouble(words[6]);
                        var bgd = new BarrelGrooveDepth(xLocation, thetaDegs, twistDegs, diamFinal, diamAsIs, targetDepth, finalDepth);
                        this.Add(bgd);
                    }

                }
                for (int j = 1; j < this.Count; j++)
                {
                    this[j].DeltaA = this[j].ThetatDeg - this[j - 1].ThetatDeg;
                    this[j].DeltaX = this[j].XLocation - this[j - 1].XLocation;
                    this[j].SegLen = this[j].DeltaX / (Math.Cos(this[j].TwistDeg * Math.PI / 180.0));

                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
