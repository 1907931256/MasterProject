using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InspectionLib;
using FileIOLib;
using RifleToolpathLib;
namespace RiflePrgBuildTest
{
    class Program
    {
        static void buildProgram()
        {

            var _machineSpeedsList = new List<MachineRasterSpeeds>();
            Console.WriteLine("buildingfile");
            var rCncBuilder = new RifleCNCFileBuilder();
            var rPathBuilder = new RifleToolpathBuilder();
            var machSpeed1 = new MachineRasterSpeeds("50cal_x=5.25_mach_speeds.csv");
            var machSpeed2 = new MachineRasterSpeeds("50cal_x=46_mach_speeds.csv");
            _machineSpeedsList.Add(machSpeed1);
            _machineSpeedsList.Add(machSpeed2);
            var barrelProfile = new BarrelProfile("50cal_groove_depth_profile.csv");
            var depthMeasurement1 = new GrooveDepthProfile("180214-04-50cal_SN027-x-46.5.autoAveDepths.csv");
            var depthMeasurement2 = new GrooveDepthProfile("180214-04-50cal_SN027-x-46.5.autoAveDepths.csv");
            var dmList = new List<GrooveDepthProfile>();
            dmList.Add(depthMeasurement1);
            dmList.Add(depthMeasurement2);
            bool adjustSpeeds = true;
            double maxRpm = 16;
            rPathBuilder.BuildPath(_machineSpeedsList, dmList, barrelProfile,maxRpm,adjustSpeeds);
                  
        }
        static void Main(string[] args)
        {
            buildProgram();
        }
    }
}
