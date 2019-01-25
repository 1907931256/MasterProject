using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileIOLib;
using GeometryLib;
using BarrelLib;

namespace InspectionLib
{
    /// <summary>
    /// list of depth measurements across one groove
    /// </summary>
    public class GrooveDepthProfile : List<DepthMeasurement>
    {
        public int GrooveNumber { get { return _grooveNumber; } }
        public int CurrentPassCount { get { return _currentPassCt; } set { _currentPassCt = value; } }
        public int TargetPassCount { get { return _targetPassCt; } set { _targetPassCt = value; } }
        public Barrel Barrel { get { return _barrel; } set { _barrel = value; } }
        public double AxialLocation { get { return _axialLocation; } set { _axialLocation = value; } }
        public int RasterCount { get { return _rasterCount; } }

        int _rasterCount;        
        int _grooveNumber;
        int _currentPassCt;
        int _targetPassCt;
        Barrel _barrel;
        double _axialLocation;

        void ReadFile(string filename)
        {

            try
            {

                var lines = FileIO.ReadDataTextFile(filename);
                var words = FileIO.Split(lines[1]);
                BarrelType type = (BarrelType)Convert.ToInt32(words[1]);
                _barrel = new Barrel(type);
                words = FileIO.Split(lines[2]);
                _barrel.ManufactureData.SerialNumber = words[1];
                words = FileIO.Split(lines[3]);
                _axialLocation = Convert.ToDouble(words[1]);
                words = FileIO.Split(lines[4]);
                _currentPassCt = Convert.ToInt32(words[1]);
                words = FileIO.Split(lines[5]);
                _targetPassCt = Convert.ToInt32(words[1]);
                for (int i = 8; i < lines.Count; i++)
                {
                    words = FileIO.Split(lines[i]);
                    int index = Convert.ToInt32(words[0]);
                    double theta = Geometry.ToRadians(Convert.ToDouble(words[1]));
                    double depth = Convert.ToDouble(words[2]);
                    var dm = new DepthMeasurement(new PointCyl(), theta, index, depth);
                    this.Add(dm);
                }
                _rasterCount = this.Count;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public GrooveDepthProfile(string filename)
        {
            ReadFile(filename);

        }
        public GrooveDepthProfile(int grooveNumber, int currentPasses, int targetPasses, Barrel barrel)
        {
            _grooveNumber = grooveNumber;           
            _barrel = barrel;
        }
        public GrooveDepthProfile()
        {
            _grooveNumber = -1;
            _barrel = new Barrel();
        }
    }
}
