using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileIOLib;
namespace BarrelLib
{
    public enum BarrelStatus
    {        
        MACHINING,    
        IN_USE,
        TESTING,
        RETIRED,
        OTHER
    }
    public class LifetimeData
    {
        public List<string> MiscData { get; set; }
        public int RoundsFired { get; set; }
        public BarrelStatus Status { get; set; }
        public LifetimeData()
        {
            MiscData = new List<string>();
            RoundsFired = 0;
            Status = BarrelStatus.MACHINING;
        }
    }
    public class MachiningData
    {
        public string Method { get; set; }
        public int TotalAWJPasses { get; set; }
        public int CurrentAWJPasses { get; set; }
    }
    public class ManufacturingData
    {
        public string SerialNumber { get { return _serialNumber; } set { _serialNumber = value; } }
        public string PartNumber { get { return _partNumber; } set {_partNumber = value; } }
        public string CurrentManufStep { get { return _currentManStep; }set { _currentManStep = value; } }
        public List<string> MiscData { get { return _miscData; } set { _miscData = value; } }

        string _currentManStep;
        string _partNumber;
        string _serialNumber;       
        List<string> _miscData;
       
        override public string ToString()
        {
            string s = "";
          
            foreach(var l in asStringList())
            {
                s += l + '\n';
            }
            return s;

        }
         List<string> asStringList()
        {
            var _dataList = new List<string>();
            _dataList.Add("SN:" + _serialNumber );
            _dataList.Add("PN:" + _partNumber);
            _dataList.Add("Current Status:"+ _currentManStep);
            _dataList.AddRange(_miscData);
            return _dataList;

        }
        public void Save(string filename)
        {
            FileIO.Save(this.asStringList(), filename);
        }
        void readFile(string fileName)
        {
            _currentManStep = "IN PROCESS GROOVE MACHINING";
            _partNumber = "PN 0";
            _serialNumber = "SN 0";
            _miscData = new List<string>();
        }
        public ManufacturingData(string fileName)
        {            
            readFile(fileName);
        }
        public ManufacturingData()
        {
            _currentManStep = "IN PROCESS GROOVE MACHINING";
            _partNumber = "PN 0";
            _serialNumber = "SN 0";
            _miscData = new List<string>();
        }
    }
}
