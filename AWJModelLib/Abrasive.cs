using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWJModel
{
    /// <summary>
    /// contains abrasive parameters
    /// </summary>
    public class Abrasive
    {
        public string Name { get; set; }
        public double FlowRate { get; set; }

        public Abrasive(string name, double flowRate)
        {
            Name = name;
            FlowRate = flowRate;
        }
        public Abrasive()
        {
            Name = "";
            FlowRate = 0;
        }
    }
}
