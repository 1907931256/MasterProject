using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ToolpathLib;
using CNCLib;
using ProbeController;
using DataLib;
namespace InspectionLib
{
   
    public abstract class RawDataFile<T>:List<T>
    {
        public string Filename { get; protected set; }
        public MeasurementUnit InputUnits { get; protected set; }
        public MeasurementUnit OutputUnits { get; protected set; }
        public int ProbeCount { get; protected set; }
        public RawDataFormat DataFormat { get; protected set; }
        public ScanFormat ScanFormat { get; protected set; }
        protected int _headerRowCount;
        protected int _rowCount;
        protected int _colCount;
        protected int _firstDataRow;
        virtual protected void ScaleData()
        {

        }
    }
    public class KeyenceLineScanDataSet : RawDataFile<GeometryLib.Vector2>
    {
        public GeometryLib.PointCyl ScanCenterLine { get; private set; }
        public GeometryLib.Vector2[] GetData()
        {
            return this.ToArray();
        }
        void ProcessFile()
        {
            try
            {
                var words = FileIOLib.CSVFileParser.ParseFile(Filename);
                _rowCount = words.GetLength(0);
                _colCount = words.GetLength(1);
                if (_rowCount == 0 || _colCount == 0 || _headerRowCount >= _rowCount)
                {
                    throw new Exception("File does not contain data.");
                }
                this.AddRange(ExtractData(words));
            }
            catch (Exception)
            {

                throw;
            }
           
            
        }
      
        List<GeometryLib.Vector2> ExtractData(string[,] words)
        {
            try
            {

                var data = new List<GeometryLib.Vector2>();
                var colList = new List<int>();
                if (words.GetLength(0) < _firstDataRow)
                {
                    throw new Exception("Data rows not found");
                }
                int columnCount = words.GetLength(1);
                
                if(columnCount==2)
                {
                    for (int i = _headerRowCount; i < _rowCount; i++)
                    {
                        double x = 0;
                        double y = 0;
                        if (double.TryParse(words[i, 0], out x) && double.TryParse(words[i, 1], out y))
                        {         
                            if(Math.Abs(y)<=Math.Abs(_maxValue) && Math.Abs(y)>=Math.Abs(_minValue))
                            {
                                data.Add(new GeometryLib.Vector2(x, y));
                            }                                                                                
                        }
                    }
                }
                ScaleData();
                return data ;
            }
            catch (Exception)
            {
                throw;
            }

        }
        double _minValue;
        double _maxValue;
        public KeyenceLineScanDataSet(ScanFormat format, MeasurementUnit outputUnit,
           GeometryLib.PointCyl scanCenterline,double minValue,double maxValue, string CsvFileName)
        {
            try
            {
                DataFormat = RawDataFormat.XY;
                _firstDataRow = 0;
                ScanFormat = format;
                ScanCenterLine = scanCenterline;
                _headerRowCount = 0;
                _minValue = minValue;
                _maxValue = maxValue;
                Filename = CsvFileName;
                OutputUnits = outputUnit;
                InputUnits = new MeasurementUnit(LengthUnitEnum.INCH);
                ProcessFile();
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
    /// <summary>
    /// holds raw sensor data 
    /// </summary>
    public class KeyenceSiDataSet: RawDataFile<double>
    {
       
       
        public double[] GetData()
        {
            return this.ToArray();
        }
        void ScaleData(double scalingFactor)
        {
            try
            {
                for (int i = 0; i < this.Count; i++)
                {
                    this[i]*=scalingFactor;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        List<double> ExtractDualProbeData(string[,]words)
        {
            try
            {
                
                var colList = new List<int>();
                var pointList = new List<double>();
                if (words.GetLength(0) < _firstDataRow)
                {
                    throw new Exception("Data rows not found");
                }
                int columnCount = words.GetUpperBound(1);
                for (int i = _headerRowCount; i < _rowCount; i++)
                {
                    double result1 = 0;
                    double result2 = 0;
                    if (double.TryParse(words[i, 2], out result1) && double.TryParse(words[i, 3], out result2))
                    {
                        pointList.Add(result1 + result2);
                    }
                }
                return pointList;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        List<double> ExtractSingleProbeData(string[,] words)
        {
            try
            {

                
                var colList = new List<int>();
                var pointList = new List<double>();
                if (words.GetLength(0) < _firstDataRow)
                {
                    throw new Exception("Data rows not found");
                }
                int columnCount = words.GetUpperBound(1);
                for (int i = _headerRowCount; i < _rowCount; i++)
                {                    
                    foreach (int col in colList)
                    {
                        double result = 0;
                        if (double.TryParse(words[i, col], out result))
                        {
                            pointList.Add(result);
                        }
                    }
                }
                return pointList;
            }
            catch (Exception)
            {
                throw;
            }

        }
        MeasurementUnit GetMeasurementUnit(string[,] words)
        {
            try
            {                               
                var unitList = MeasurementUnitDictionary.MeasurementUnitNames();
                var inputUnit = new MeasurementUnit(LengthUnitEnum.MICRON);
                foreach (string unitStr in unitList)
                {
                    for (int i = 0; i < 6 ; i++)
                    {
                        for (int j = 0; j < words.GetLength(1); j++)
                        {
                            string upperw = words[i, j].ToUpper();
                            if (upperw.Contains(unitStr))
                            {                               
                                inputUnit=  new MeasurementUnit(MeasurementUnitDictionary.GetUnits(unitStr));
                                break;
                            }                            
                        }                       
                    }                   
                }
                return inputUnit;
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
                var words = FileIOLib.CSVFileParser.ParseFile(Filename);             


                _rowCount = words.GetLength(0);
                _colCount = words.GetLength(1);
                if (_rowCount == 0 || _colCount == 0 || _headerRowCount >= _rowCount)
                {
                    throw new Exception("File does not contain data.");
                }
               

                InputUnits = GetMeasurementUnit(words);

                if (InputUnits == null)
                {
                    InputUnits = new MeasurementUnit(LengthUnitEnum.MICRON);
                }
                if(ProbeCount ==1)
                {
                    this.AddRange(ExtractSingleProbeData(words));
                }
                if(ProbeCount == 2)
                {
                    this.AddRange(ExtractDualProbeData(words));
                }
                double scalingFactor = InputUnits.ConversionFactor / OutputUnits.ConversionFactor;
                ScaleData(scalingFactor);
                
            }
            catch (Exception)
            {

                throw;
            }

        }
       
        public KeyenceSiDataSet(ScanFormat format,MeasurementUnit outputUnits, int probeCount, string CsvFileName)
        {
            try
            {
                ScanFormat = format;
                DataFormat = RawDataFormat.RADIAL;              
                ProbeCount = probeCount;
                Filename = CsvFileName;
                _headerRowCount = 0;
                _firstDataRow = 4;
                OutputUnits = outputUnits;               
                ProcessFile();
            }
            catch (Exception)
            {

                throw;
            }
             
        }

           
    }   
}
       