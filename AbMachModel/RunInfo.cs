using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbMachModel
{
    /// <summary>
    /// contains info on abmach model run
    /// </summary>
    public class RunInfo
    {
        public int Runs { get; set; }    
        public int Iterations { get; set; }    
        public int CurrentRun { get; set; }    
        public int CurrentIteration { get; set; }    
        public ModelRunType RunType { get; set; }
        public RunInfo()
        {
            Runs = 1;
            Iterations = 1;
            CurrentRun = 0;
            CurrentIteration = 0;
            RunType = ModelRunType.RunAsIs;
        }
        public RunInfo(int runs, int iterations,ModelRunType runType)
        {
            Runs = runs;
            Iterations = iterations;
            RunType = runType;
        }
    }
}
