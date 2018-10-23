using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

namespace ProbeController
{
    public interface ISensorComms
    {

        string GetProbeSettings(CancellationToken ct);        
        bool Connect();
        bool Disconnect();
        double[] GetAllData(CancellationToken ct);
        double[] GetDataArray(int points, CancellationToken ct);
        double GetSingleDataPoint(CancellationToken ct);
    }
    public class ProbeController:ISensorComms
    {
         
       
        protected int probeCount;
        
        
        protected double[] dataArray;
        
           
        public string GetProbeSettings(CancellationToken ct)
        {
            string settings = "";
            return settings;
        }
        public double[] GetAllData(CancellationToken ct)
        {
            var data = new List<double>();
            return data.ToArray();

        }
        public double[] GetDataArray(int points,CancellationToken ct)
        {
            var data = new List<double>();
            return data.ToArray();
        }
        public double GetSingleDataPoint(CancellationToken ct)
        {
            double pt = 0;
            return pt;
        }
        public bool Connect()
        {
            return false;
        }
        public bool Disconnect()
        {
            return true;
        }
         
        public ProbeController()
        {
                  
        }

    }
}
