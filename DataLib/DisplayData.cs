using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLib
{
    public class DisplayData<T>:List<T> 
    {
        public ScanFormat DataFormat { get; set; }
        public List<T> HiLightPoints { get; set; }
        public DisplayData()
        {
            HiLightPoints = new List<T>();
            DataFormat = ScanFormat.UNKNOWN;
        }
    }
}
