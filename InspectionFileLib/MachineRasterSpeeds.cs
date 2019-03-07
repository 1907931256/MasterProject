using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileIOLib;
using AWJModel;

namespace InspectionLib
{
   
    
    public class MachineRasterSpeeds : List<MachineRasterSpeed>
    {
        public double XLocation { get { return _xLocation; } }
        public int TargetPasses { get { return _targetPasses; } }
        public int RasterCount { get { return _rasterCount; } }
        public double RasterOffsetAngle { get { return _rasterOffsetAngle; } }
        MachiningParameters MachiningParameters { get { return _machiningParameters; } }

        double _rasterOffsetAngle;
        MachiningParameters _machiningParameters;
        double _xLocation;
        int _targetPasses;
        int _rasterCount;


        public MachineRasterSpeeds(string filename)
        {
            try
            {
                var fileList = FileIO.ReadDataTextFile(filename);

                string[] words = FileIO.Split(fileList[1]);
                words = FileIO.Split(fileList[1]);
                double jp = Convert.ToDouble(words[1]);

                _machiningParameters = new MachiningParameters();
                words = FileIO.Split(fileList[2]);
                double dn = Convert.ToDouble(words[1]);
                words = FileIO.Split(fileList[3]);
                double dm = Convert.ToDouble(words[1]);
                words = FileIO.Split(fileList[4]);
                double ma = Convert.ToDouble(words[1]);
                words = FileIO.Split(fileList[5]);
                string ab = words[1];
                Abrasive abr = new Abrasive(ab, ma);
                WaterJet wj = new WaterJet(jp, dm, dn);
                _machiningParameters = new MachiningParameters(wj, abr, MachiningOpType.SingleChannel, 0);
                words = FileIO.Split(fileList[6]);
                _xLocation = Convert.ToDouble(words[1]);
                words = FileIO.Split(fileList[7]);
                _targetPasses = Convert.ToInt32(words[1]);
                words = FileIO.Split(fileList[8]);
                _rasterOffsetAngle = GeometryLib.GeomUtilities.ToRadians(Convert.ToDouble(words[1]));
                for (int i = 10; i < fileList.Count; i++)
                {
                    words = FileIO.Split(fileList[i]);
                    if (words.Length == 4)
                    {
                        int rasterIndex = Convert.ToInt32(words[0]);
                        double thetaRel = GeometryLib.GeomUtilities.ToRadians(Convert.ToDouble(words[1]));
                        double speed = Convert.ToDouble(words[2]);
                        double depth = Convert.ToDouble(words[3]);
                        var mrs = new MachineRasterSpeed(rasterIndex, thetaRel, speed, depth);
                        this.Add(mrs);
                    }
                }
                _rasterCount = this.Count;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
