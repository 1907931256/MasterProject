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
    
    public abstract class RawDataSet
    {
        public MeasurementUnit InputUnits { get; protected set; }
        public MeasurementUnit OutputUnits { get; set; }
        public RawDataFormat DataFormat { get; protected set; }
        public ScanFormat ScanFormat { get; protected set; }
        public string Filename { get; protected set; }        
        protected int _headerRowCount;
        protected int _rowCount;
        protected int _colCount;
        protected int _firstDataRow;
        virtual protected void ScaleData()
        {

        }
    }
    public class KeyenceLJSiDataSet
    {
        KeyenceSiDataSet KeyenceSiDataSet;
        KeyenceLJDataSet KeyenceLJDataSet;
        public KeyenceLJSiDataSet(KeyenceSiDataSet keyenceSiDataSet,KeyenceLJDataSet keyenceLJDataSet)
        {
            KeyenceLJDataSet = keyenceLJDataSet;
            KeyenceSiDataSet = keyenceSiDataSet;

        }
    }
    public class KeyenceLJDataSet : RawDataSet
    {
        List<GeometryLib.Vector2> data;
        public int Count
        {
            get
            {
                return data.Count;
            }
        }
        public GeometryLib.Vector2[] GetData()
        {
            return data.ToArray();
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
                data.AddRange(ExtractData(words));
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

                var points = new List<GeometryLib.Vector2>();
                var colList = new List<int>();
                if (words.GetLength(0) < _firstDataRow)
                {
                    throw new Exception("Data rows not found");
                }
                int columnCount = words.GetLength(1);
                double scalingFactor = InputUnits.ConversionFactor / OutputUnits.ConversionFactor;
                if (columnCount==2)
                {
                    for (int i = _headerRowCount; i < _rowCount; i++)
                    {
                        double x = 0;
                        double y = 0;
                        if (double.TryParse(words[i, 0], out x) && double.TryParse(words[i, 1], out y))
                        {
                            x *= scalingFactor;
                            y *= scalingFactor;

                            points.Add(new GeometryLib.Vector2(x, y));                                                                                                            
                        }
                    }
                }
               
                
                return points;
            }
            catch (Exception)
            {
                throw;
            }

        }
       
        double _minValue;
        double _maxValue;
       
        public KeyenceLJDataSet(InspectionScript script, string CsvFileName, MeasurementUnit fileUnits)
        {
            try
            {
                DataFormat = RawDataFormat.XY;
                _firstDataRow = 0;
                ScanFormat = script.ScanFormat;
                
                _headerRowCount = 0;
                
                Filename = CsvFileName;
                OutputUnits = script.OutputUnit;
                InputUnits = fileUnits;
                _minValue = script.ProbeSetup[0].MinValue(fileUnits);
                _maxValue = script.ProbeSetup[0].MaxValue(fileUnits);
                data = new List<GeometryLib.Vector2>();
                ProcessFile();
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        
    }
    public class KeyenceDualSiDataSet
    {
        List<KeyenceSiDataSet> probeData;
        
        int probeIndexOffset;
        int GetMinDataCount()
        {
            int minCount = int.MaxValue;
            foreach(var dataset  in probeData)
            {
                if (dataset.Count < minCount)
                    minCount = dataset.Count;
            }
            return minCount;
        }
        double[] GetDualProbeValues(bool getSum)
        {
            try
            {
                var count = GetMinDataCount();
                var values = new List<double>();
                var data1 = probeData[0].GetData();
                var data2 = probeData[1].GetData();
                for (int i=0;i<count;i++)
                {

                    int probe2Index = (i + probeIndexOffset) % count;      
                    
                    if(getSum)
                    {
                        values.Add(data1[i] + data2[i]);
                    }
                    else
                    {
                        values.Add(data1[i] + data2[probe2Index]);
                    }
                    
                }
                return values.ToArray();
            }
            catch (Exception)
            {

                throw;
            }

        }
      
        public double[] GetData(ScanFormat format)
        {
            bool getSumValues = true;
            switch(format)
            {
                case ScanFormat.AXIAL:
                case ScanFormat.LAND:
                case ScanFormat.GROOVE:
                case ScanFormat.CAL:
                    getSumValues = true;
                    break;
                case ScanFormat.RING:               
                case ScanFormat.SPIRAL:
                default:
                    getSumValues = false;
                    break;
            }
            return GetDualProbeValues(getSumValues);
        }
        public KeyenceDualSiDataSet(InspectionScript inspScript, string CsvFileName)
        {
            probeData = new List<KeyenceSiDataSet>();
            if (inspScript is CylInspScript cylScript)
            {
                double phaseDifference = (cylScript.ProbeSetup[1].ProbePhaseRad - cylScript.ProbeSetup[0].ProbePhaseRad) / (2 * Math.PI);
                probeIndexOffset = (int)(Math.Round(cylScript.PointsPerRevolution * phaseDifference));

                probeData.Add(new KeyenceSiDataSet(cylScript, CsvFileName, 1));
                probeData.Add(new KeyenceSiDataSet(cylScript, CsvFileName, 2));
            }
        }
       
    }
    /// <summary>
    /// holds raw sensor data 
    /// </summary>
    public class KeyenceSiDataSet:RawDataSet
    {
        List<double> data;
        public int Count
        {
            get
            {
                return data.Count;
            }
        }
        public double[] GetData()
        {
            return data.ToArray();
        }
        void ScaleData(double scalingFactor)
        {
            try
            {
                for (int i = 0; i < this.Count; i++)
                {
                    data[i]*=scalingFactor;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        List<double> ExtractProbeData(string[,] words,int column)
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
                     double result = 0;
                     if (double.TryParse(words[i, column], out result))
                     {
                         pointList.Add(result);
                     }
                   
                }
                return pointList;
            }
            catch (Exception)
            {
                throw;
            }

        }
        
       
        void ProcessFile(int dataColumn)
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
                if( dataColumn>_colCount-1)
                {
                    throw new Exception("No data in specified column. Likely incorrect probe count or type.");
                }

                InputUnits = MeasurementUnit.GetMeasurementUnit(words);

                if (InputUnits == null)
                {
                    InputUnits = new MeasurementUnit(LengthUnit.MICRON);
                }                
                data.AddRange(ExtractProbeData(words,dataColumn));
                
               
                double scalingFactor = InputUnits.ConversionFactor/OutputUnits.ConversionFactor;
                ScaleData(scalingFactor);
                
            }
            catch (Exception)
            {

                throw;
            }

        }
        void initialize(InspectionScript script, string CsvFileName)
        {
            OutputUnits = script.OutputUnit;
            ScanFormat = script.ScanFormat;
            DataFormat = RawDataFormat.RADIAL;
            Filename = CsvFileName;
            _headerRowCount = 0;
            _firstDataRow = 4;
            _firstDataCol = 2;
            data = new List<double>();

        }
        int _firstDataCol;
        public KeyenceSiDataSet(InspectionScript script, string CsvFileName)
        {
            try
            {
                initialize(script, CsvFileName);
                            
                ProcessFile(_firstDataCol);
            }
            catch (Exception)
            {

                throw;
            }
             
        }
        public KeyenceSiDataSet(InspectionScript script, string CsvFileName,int probeNumber)
        {
            try
            {
                initialize(script, CsvFileName);
                int dataColumn = _firstDataCol + probeNumber - 1;
                ProcessFile(dataColumn);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public KeyenceSiDataSet()
        {
            
        }
    }   
}
       