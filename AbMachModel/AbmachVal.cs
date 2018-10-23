using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbMachModel
{
    /// <summary>
    /// used in abmach surface
    /// </summary>
    internal class AbmachVal
    {
        public double Start {get;set;}
        public double Model { get; set; }
        public double Temp { get; set; }
        public double Target { get; set; } 

        public double Depth { get { return Start - Model; } }
        public double Remaining { get { return Model - Target; } }
        internal AbmachVal()
        {
            Start = 0;
            Model = 0;
            Temp = 0;
            Target = 0;
        }       
    }
   
}
