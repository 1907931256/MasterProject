using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ToolpathLib;
using CNCLib;
using ProbeController;
namespace InspectionLib
{
    public class RawDistanceProbePt
    {
        public  double Distance { get { return _dist; } }        
        
        private  double _dist;        
        

        public RawDistanceProbePt(double value)
        {           
            _dist = value;
        }
    }
    public class RawDataSet
    {
        public string Filename { get { return _fileName; } }
      
        protected string _fileName;
        protected MeasurementUnit _measurementUnit;
        protected int _probeCount;
        protected double[,] _data;

        public int ProbeCount
        {
            get
            {
                return _probeCount;
            }
        }

        public double[] GetSingleProbeData()
        {
            if (_probeCount > _data.GetLength(1))
                _probeCount = _data.GetLength(1);
            if (_probeCount <= 0)
                _probeCount = 1;

            var result = new double[_data.GetLength(0)];
            for(int i =0;i<result.Length;i++)
            {
                result[i] = _data[i, _probeCount - 1];
            }
            return result;  
        }
       
        public double[,] GetAllProbeData()
        {

            return _data;
            
        }
        public MeasurementUnit Units
        {
            get
            {
                return _measurementUnit;
            }
        }
    }
    
    /// <summary>
    /// holds raw sensor data 
    /// </summary>
    public class KeyenceSISensorDataSet:RawDataSet
    {
        
        int _headerRowCount;
        
        int _rowCount;
        int _colCount;

        
       
        void ScaleData(double scalingFactor)
        {
            try
            {
                for (int i = 0; i < _data.GetLength(0); i++)
                {                    
                    for (int j=0;j<_data.GetLength(1);j++)
                    {
                        _data[i, j]*=scalingFactor;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        void ExtractData(string[,] words)
        {
            try
            {
                          
                int firstDataRow = 4;
                var colList = new List<int>();
                if (words.GetLength(0)< firstDataRow)
                {
                    throw new Exception("Data rows not found");
                }
                int columnCount = words.GetUpperBound(1);
                if(_probeCount==1 || columnCount>=2)
                {
                    colList.Add(2);
                }
                if(_probeCount==2 && _colCount>=3)
                {
                    colList.Add(2);
                    colList.Add(3);
                }
                for (int i = _headerRowCount; i < _rowCount; i++)
                {
                    int j = 0;

                    foreach(int col in colList)
                    {                        
                        double result = 0;
                        double.TryParse(words[i, col], out result);
                        _data[i - _headerRowCount, j++] = result;                       
                    }
                }
                
            }
            catch (Exception)
            {
                throw;
            }

        }
        void GetMeasurementUnit(string[,] words)
        {
            try
            {                               
                var unitList = MeasurementUnitDictionary.MeasurementUnitNames();              

                foreach (string unit in unitList)
                {
                    for (int i = 0; i < 6 ; i++)
                    {
                        for (int j = 0; j < _colCount; j++)
                        {
                            string upperw = words[i, j].ToUpper();
                            if (upperw.Contains(unit))
                            {                               
                                _measurementUnit = new MeasurementUnit(unit);
                                break;
                            }
                            
                        }
                       
                    }
                   
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
       
        void ProcessFile()
        {
            try
            {
                var words = FileIOLib.CSVFileParser.ParseFile(_fileName);
                _colCount = _probeCount;


                _rowCount = words.GetLength(0);
                if (_rowCount == 0 || _colCount == 0 || _headerRowCount >= _rowCount)
                {
                    throw new Exception("File does not contain data.");
                }
                _data = new double[_rowCount - _headerRowCount, _probeCount];

                GetMeasurementUnit(words);

                if (_measurementUnit == null)
                {
                    _measurementUnit = new MeasurementUnit("micron");
                }
                ExtractData(words);
                var outputUnit = new MeasurementUnit("inch");

                double scalingFactor = _measurementUnit.ConversionFactor / outputUnit.ConversionFactor;
                ScaleData(scalingFactor);
            }
            catch (Exception)
            {

                throw;
            }

        }
        
        public KeyenceSISensorDataSet(int probeCount, string CsvFileName, int headerRowCount)
        {
            try
            {                
                _probeCount = probeCount;
                _fileName = CsvFileName;
                _headerRowCount = headerRowCount;
                ProcessFile();
            }
            catch (Exception)
            {

                throw;
            }
             
        }

           
    }   
}
       