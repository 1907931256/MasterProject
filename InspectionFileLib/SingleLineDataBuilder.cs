using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarrelLib;
using DataLib;
using GeometryLib;
using System.Threading;

namespace InspectionLib
{
    public class SingleLineDataBuilder : DataBuilder
    {
       
        InspDataSet BuildSingleLineFromLineData(CylInspScript script, KeyenceLineScanDataSet rawDataSet)
        {
            try
            {
                var dataSet = new InspDataSet();
               
                dataSet.UncorrectedCylData = DataUtilities.ConverToCylData(rawDataSet.GetData());
                dataSet.CorrectedCylData = dataSet.UncorrectedCylData;
               
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public InspDataSet BuildSingleLineAsync(CancellationToken ct, IProgress<int> progress, CylInspScript script,
           KeyenceLineScanDataSet rawDataSet, DataOutputOptions options)
        {
            try
            {
                Init(options, rawDataSet.Filename);
                var dataSet = BuildSingleLineFromLineData(script, rawDataSet);
                dataSet.DataFormat = ScanFormat.SINGLELINE;
                dataSet.Filename = rawDataSet.Filename;
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public InspDataSet BuildSingleLineAsync(CancellationToken ct, IProgress<int> progress, CylInspScript script, 
            KeyenceLineScanDataSet rawDataSet,DataOutputOptions options, PointCyl[] landPointArr)
        {
            try
            {
                Init(options, rawDataSet.Filename);
                var dataSet = BuildSingleLineFromLineData(script, rawDataSet);
                dataSet.DataFormat = ScanFormat.SINGLELINE;
                dataSet.Filename = rawDataSet.Filename;
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public SingleLineDataBuilder(Barrel barrel) : base(barrel)
        {

        }
    }
}
