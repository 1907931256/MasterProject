using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolpathLib;
using CNCLib;
namespace TubeInspection
{
    public class GrooveInspection : CylInspScript
    {
        public double ZStart { get; set; }
        public double ZEnd { get; set; }
        public double ZIncrement { get; set; }
        public List<int> Grooves { get; set; }
        public TwistProfile Twist { get; set; }
        public int GrooveCount { get; set; }
        public GrooveInspection(Barrel barrel, MachineSettings machineSettings)
        {
            method = InspectionMethod.GROOVES;
            Barrel = barrel;
            Twist = barrel.TwistProfile;
            Grooves = new List<int>();
            SensorEncoderTriggerAxis = machineSettings.XAxis;
            PositionAxis1 = machineSettings.AAxis;
            PositionAxis2 = null;
        }
        public GrooveInspection(MachineSettings machineSettings)
        {
            method = InspectionMethod.GROOVES;
            Barrel = new TubeInspection.Barrel(); ;
            Twist = new TwistProfile();
            Grooves = new List<int>();
            SensorEncoderTriggerAxis = machineSettings.XAxis;
            PositionAxis1 = machineSettings.AAxis;
            PositionAxis2 = null;
        }
        public GrooveInspection()
        {
            method = InspectionMethod.GROOVES;
            Barrel = new TubeInspection.Barrel();

            ZStart = 0;
            ZEnd = 1;
            ZIncrement = 0.1;
            Twist = new TwistProfile();
            Grooves = new List<int>();
            SensorEncoderTriggerAxis = new Axis(0, "X", AxisTypeEnum.Rotary, "APOSX");
            PositionAxis1 = new Axis(0, "A", AxisTypeEnum.Rotary, "APOSA");
            PositionAxis2 = null;
        }

    }
}
