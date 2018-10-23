using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWJModel
{   
    public class MachiningParameters
    {
        public MachiningOpType MachiningOp{ get; set; }
        public double SOD { get; set; }
        public Abrasive Abrasive{ get; set; }
        public WaterJet Waterjet{ get; set; }

        public MachiningParameters()
        {
            Abrasive = new Abrasive();
            Waterjet = new WaterJet();
            MachiningOp = MachiningOpType.Unknown;
            SOD = 0;
        }
        public MachiningParameters(WaterJet wj, Abrasive abr,MachiningOpType op,double sod)
        {
            Abrasive = abr;
            Waterjet = wj;
            MachiningOp = op;
            SOD = sod;
        }
    }
}
