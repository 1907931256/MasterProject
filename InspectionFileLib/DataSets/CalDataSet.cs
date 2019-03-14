using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionLib
{   
    public class CalDataSet
    {
        public double ProbeSpacingInch { get; private set; }
        public double NominalRadius { get; private set; }

        //public CalDataSet()
        //{
        //    ProbeSpacingInch = 0;
        //    NominalRadius = 0;
        //}

        public CalDataSet(double nominalRadius)
        {
            NominalRadius = nominalRadius;
        }
        public CalDataSet(double ringGageInch, double probeValue, ProbeController.ProbeDirection probeDirection)
        {
            
            NominalRadius = ringGageInch / 2;
          
            if(probeDirection == ProbeController.ProbeDirection.ID)
            {
                ProbeSpacingInch = ringGageInch -  probeValue;
            }
            else
            {
                ProbeSpacingInch = ringGageInch +  probeValue;
            }
        }
    }
}
