using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLib;
using BarrelLib;
namespace InspectionLib
{
    public class BarrelInspProfile
    {
        public CylData MinLandProfile { get; set; }
        public CylData AveLandProfile { get; set; }
        public CylData AveGrooveProfile { get; set; }
       // public Barrel Barrel { get; set; }
        public BarrelInspProfile()
        {
            MinLandProfile = new CylData("");
            AveLandProfile = new CylData("");
            AveGrooveProfile = new CylData("");
           // Barrel = new Barrel();
        }
    }
}
