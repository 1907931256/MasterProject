using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWJModel
{
    /// <summary>
    /// contains physical constants related to waterjet
    /// </summary>
    public class WaterJet
    {
        public double Dn { get; set; }
        public double Dm { get; set; }
        public double Pressure { get; set; }
        
        double rho = 1.936;
        double pumpEfficiency = .7;
        double dischargeCoefficient = .7;
        public double Speed()
        {
            return 12.84 * Math.Sqrt(Pressure);
        }
        public double Waterflow()
        {
            
            double flow = dischargeCoefficient * (Speed() * 12) * 60 * Math.PI * Math.Pow(Dn, 2) / 4;
            return flow;
        }
        public double JetForce()//lbs
        {
            return rho * Waterflow() / (12 * 12 * 12 * 60) * Speed();
        }
        public double PumpPower()//hp
        {
            return (Pressure * 144 * Waterflow() / (12 * 12 * 12 * 60)) / (550 * pumpEfficiency);
        }
        public WaterJet(double pressure, double dm, double dn)
        {
            Pressure = pressure;
            Dm = dm;
            Dn = dn;          
        }
        public WaterJet()
        {
            Pressure = 0;
            Dm = 0;          
            Dn = 0;
        }
    }
}
