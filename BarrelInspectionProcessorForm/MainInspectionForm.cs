using BarrelLib;
using DataLib;
using FileIOLib;
using GeometryLib;
using InspectionLib;
using ProbeController;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsLib;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Media.Media3D;

namespace BarrelInspectionProcessorForm
{
    public enum InputFileType
    {
        RAW,
        PROCESSED,
        DXF,
        UNKNOWN
    }
    
    public partial class MainInspectionForm : Form
    {
        LogFile _logFile;
        InputFileType inputFileType;
        
        
        string barrelTypeStr;
        string scanFormatStr;
        string probeDirectionStr;
        string probeTypeStr;
        string knownDiamTypeStr;
        string manufStepStr;
        private void SetComboBoxes()
        {
            var barrelTypeList = Enum.GetNames(typeof(BarrelType)); //;new List<string>() { "M2_50_Cal", "M242_25mm", "M284_155mm", "M240_762mm","M_50mm", "Flat Plate" };
            var scanFormatList = Enum.GetNames(typeof(ScanFormat));// new List<string>() { "RING", "SPIRAL", "AXIAL", "LAND", "GROOVE", "CAL", "SINGLE","RASTER" };
            var probeDirectionList = new List<string>() { "BORE I.D.", "ROD O.D." };
            var probeConfigList = Enum.GetNames(typeof(ProbeConfig));// new List<string>() { "SINGLE_SI_F10", "DUAL_SI_F10","SINGLE_LJ_V7060","LJ_V7060_SI_F10" };
            var knownDiamList = new List<string>() { "Default Value", "Set Value", "Diameter Profile", "Ring Calibrated" };
            var manufStepList = new List<string>(){ "Pre-Boring ","Boring In-process","Post Boring","Post Honing","Groove Machining In-Process",
                                                    "Post Groove Machining","Post Final Honing","In Use"};
            ComboListBoxHelper.FillComboBox(comboBoxMethod, scanFormatList.ToArray());
            ComboListBoxHelper.FillComboBox(comboBoxBarrel, barrelTypeList.ToArray());
            ComboListBoxHelper.FillComboBox(comboBoxDiameterType, knownDiamList.ToArray());
            ComboListBoxHelper.FillComboBox(comboBoxProbeDirection, probeDirectionList.ToArray());
            ComboListBoxHelper.FillComboBox(comboBoxProbeConifg, probeConfigList.ToArray());
           

            comboBoxBarrel.SelectedIndex = ComboListBoxHelper.GetIndexOf(barrelTypeStr, comboBoxBarrel.Items);
            comboBoxMethod.SelectedIndex = ComboListBoxHelper.GetIndexOf(scanFormatStr, comboBoxMethod.Items);
            comboBoxProbeDirection.SelectedIndex = ComboListBoxHelper.GetIndexOf(probeDirectionStr, comboBoxProbeDirection.Items);
            comboBoxProbeConifg.SelectedIndex = ComboListBoxHelper.GetIndexOf(probeTypeStr, comboBoxProbeConifg.Items);
            comboBoxDiameterType.SelectedIndex = ComboListBoxHelper.GetIndexOf(knownDiamTypeStr, comboBoxDiameterType.Items);
            
        }
        private void GetPropertyValues()
        {
            barrelTypeStr = Properties.Settings.Default.barrelType;
            scanFormatStr = Properties.Settings.Default.scanFormat;
            probeDirectionStr = Properties.Settings.Default.probeDirection;
            probeTypeStr = Properties.Settings.Default.probeType;
            
            knownDiamTypeStr = Properties.Settings.Default.diamCalType;
            manufStepStr = Properties.Settings.Default.manufStep;
            _useFilenameData = Properties.Settings.Default._useFileNameData;
            checkBoxUseFilename.Checked = _useFilenameData;
        }
        private void SetPropertyValues()
        {
            Properties.Settings.Default.barrelType = comboBoxBarrel.SelectedItem.ToString();
            Properties.Settings.Default.probeType = comboBoxProbeConifg.SelectedItem.ToString();
            Properties.Settings.Default.probeDirection = comboBoxProbeDirection.SelectedItem.ToString();
            Properties.Settings.Default.scanFormat = comboBoxMethod.SelectedItem.ToString();
           
            Properties.Settings.Default.diamCalType = comboBoxDiameterType.SelectedItem.ToString();
            //Properties.Settings.Default.manufStep = comboBoxManStep.SelectedItem.ToString();
            Properties.Settings.Default._useFileNameData = _useFilenameData;
        }
        public MainInspectionForm()
        {
            try
            {
                
                InitializeComponent();
                _logFile = LogFile.GetLogFile();
                dataOuputText = new List<string>();
                GetPropertyValues();
                SetComboBoxes();

                labelMethod.Text = "";                
                _lengthLabel = "inch";
                _angleLabel = "degs";
                //_barrelType = Barrel.GetBarrelType(barrelTypeStr);
                //_barrel = new Barrel(_barrelType);

                radioButtonViewProcessed.Checked = true;
                

                textBoxPitch.ReadOnly = true;
                
                textBoxNomDiam.Text = _barrel.DimensionData.ActualLandDiam.ToString("f4");
                
                //_probeDirection = ProbeController.ProbeDirection.ID;
                Size = new Size(1600, 800);
                if (DataOutputOptions.FileName != null && System.IO.File.Exists(DataOutputOptions.FileName))
                {
                    _dataOutOptions = DataOptionsFile.Open(DataOutputOptions.FileName);
                }
                else
                {
                    _dataOutOptions = new DataOutputOptions();
                }
            }
            catch (Exception)
            {

                throw;
            }
            

        }
        #region OpenFile_Methods
        List<string> _inputFileNames;
        private void OpenRawDataFiles()
        {
            try
            {
                

                var ofd = new OpenFileDialog
                {
                    Filter = "(*.csv)|*.csv",
                    Multiselect = true
                };
               

                if (ofd.ShowDialog() == DialogResult.OK)
                {                    
                        inputFileType = InputFileType.RAW;
                        ResetOnOpen(inputFileType);
                        radioButtonViewRaw.Enabled = true;
                        _inputFileNames = new List<string>();
                        _inspDataSetList = new List<InspDataSet>();
                        _inputFileNames = ofd.FileNames.ToList();
                        if (_inputFileNames.Count == 1)
                        {
                            
                            labelInputFIlename.Text = _inputFileNames[0];

                        }
                        else
                        {
                            labelInputFIlename.Text = "Multiple Files";
                        }
                        dataOuputText.Add("opening: ");
                    foreach (var dataset in _inspDataSetList)
                    {
                        dataOuputText.Add(dataset.Filename);
                    }
                    _outputPath = System.IO.Path.GetDirectoryName(_inputFileNames[0]);
                    labelStatus.Text = "Raw Files Read In OK";                   
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        void ResetLists(bool mergeFile)
        {
            if (_inspDataSetList == null || !mergeFile)
            {
                _inspDataSetList = new List<InspDataSet>();
            }
            if (_inputFileNames == null || !mergeFile)
            {
                _inputFileNames = new List<string>();
            }
        }
        void OpenDxfFile(string fileName,bool mergeFile)
        {
            try
            {
                double segLength = .0008;
                if (fileName!= "" && System.IO.File.Exists(fileName))
                {
                    var stringList = FileIO.ReadDataTextFile(fileName);
                    var entityList = DwgConverterLib.DxfFileParser.Parse(fileName);
                    var pointList = DwgConverterLib.DxfFileParser.BuildPointList(entityList, segLength);
                    var cartDataSet = new CartDataSet(fileName);
                    cartDataSet.CartData.AddRange(pointList);
                    ResetLists(mergeFile);
                    _inspDataSetList.Add(cartDataSet);
                    _inputFileNames.Add(fileName);
                }
                else
                {
                    OpenDxfFile(mergeFile);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        void OpenDxfFile(bool mergeFile)
        {
            try
            {
                var ofd = new OpenFileDialog();
                
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string fileName = ofd.FileName;
                    OpenDxfFile(fileName, mergeFile);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        private void OpenDXFFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenDxfFile(mergeFile:false);
                DisplayData(BuildDisplayDataList());
            }
            catch (Exception ex)
            {

                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);
            }
        }
        private void ResetOnOpen(InputFileType inputFileType)
        {
            try
            {
                bool state = (inputFileType == InputFileType.RAW);
                radioButtonViewRaw.Enabled = state;
                buttonProcessFile.Enabled = state;

                _dataSelection = DataSelection.NONE;

                Clear3DView();
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        private void OpenProcessedFile(int firstRow)
        {
            try
            {

                
                var ofd = new OpenFileDialog();
                
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    inputFileType = InputFileType.PROCESSED;
                    ResetOnOpen(inputFileType);
                    
                    
                    _inputFileNames = ofd.FileNames.ToList();
                    _inspDataSetList = new List<InspDataSet>();
                   
                    if (_inputFileNames.Count==1)
                    {
                        string shortFileName = System.IO.Path.GetFileName(_inputFileNames[0]);                        
                        labelInputFIlename.Text = shortFileName;                        
                    }
                    else
                    {
                        labelInputFIlename.Text = "Multiple Files";                        
                    }
                    _outputPath = System.IO.Path.GetDirectoryName(_inputFileNames[0]);
                    dataOuputText.Add("opening: ");
                    foreach (var dataset in _inspDataSetList)
                    {
                        dataOuputText.Add(dataset.Filename);
                    }

                    OpenProcessedCartDataFiles(_inputFileNames);
                    
                   // DisplayData();
                    labelStatus.Text = "Processed File Read In OK";

                }
            }
            catch (Exception)
            {
                throw;
            }


        }
        void OpenProcessedCartDataFiles(List<string> filenames)
        {
            try
            {
                int firstRow = 0;
                foreach (string filename in filenames)
                {
                    var inspDataSet = new CartDataSet(filename);
                    var lines = FileIO.ReadDataTextFile(filename);
                    for (int i = firstRow; i < lines.Count; i++)
                    {
                        var words = FileIO.Split(lines[i]);
                        if (words.Length == 2)
                        {
                            if (double.TryParse(words[0], out double x) && double.TryParse(words[1], out double y))
                            {
                                var pt = new Vector3(x, y, 0);
                                inspDataSet.CartData.Add(pt);
                            }
                        }
                        if (words.Length == 3)
                        {
                            if (double.TryParse(words[0], out double x) && double.TryParse(words[1], out double y) && double.TryParse(words[1], out double z))
                            {
                                var pt = new Vector3(x, y, z);
                                inspDataSet.CartData.Add(pt);
                            }
                        }


                    }

                    _inspDataSetList.Add(inspDataSet);
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        void OpenProcessedRingDataFiles(List<string> filenames)
        {
            try
            {
                int firstRow = 6;
                foreach (string filename in filenames)
                {
                    var inspDataSet = new RingDataSet(filename);
                    var lines = FileIO.ReadDataTextFile(filename);
                    for (int i = firstRow; i < lines.Count; i++)
                    {
                        var words = FileIO.Split(lines[i]);
                        if (double.TryParse(words[1], out double th) && double.TryParse(words[2], out double r) &&
                        double.TryParse(words[3], out double z))
                        {
                            var pt = new PointCyl(r, th, z);
                            inspDataSet.CorrectedCylData.Add(pt);
                        }

                    }

                    _inspDataSetList.Add(inspDataSet);
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        private void rawFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenRawDataFiles();
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);

                MessageBox.Show(ex.Message + ": Unable to open raw data file.*********" + ex.StackTrace);
            }
        }

        private void processedCSVFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenProcessedFile(firstRow: 6);
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);

                MessageBox.Show(ex.Message + ": Unable to open processed data file.***********" + ex.StackTrace);
            }
        }
        #endregion

      
        void BuildBarrelFromInputs()
        {
            //_barrelType = Barrel.GetBarrelType(comboBoxBarrel.SelectedItem.ToString());
            //_barrel = new Barrel(_barrelType);
            string sn = textBoxSerialN.Text;
            //_barrel.ManufactureData.SerialNumber = sn;
           // _barrel.ManufactureData.CurrentManufStep = GetManufStep();
           //_barrel.ManufactureData.MiscData.AddRange(textBoxMiscManNotes.Lines);
            int currentPasses = 1;
           // InputVerification.TryGetValue(textBoxCurrentPasses, "x>0", 0, int.MaxValue, out currentPasses);
            //_barrel.MachiningData.CurrentAWJPasses = currentPasses;
            int totalPasses = 4;
           // InputVerification.TryGetValue(textBoxTotalPasses, "x>0", 0, int.MaxValue, out totalPasses);
            //_barrel.MachiningData.TotalAWJPasses = totalPasses;
            double nomDiam = _barrel.DimensionData.LandNominalDiam;
            //read nominal barrel diameter


            InputVerification.TryGetValue(textBoxNomDiam, "NaN", out nomDiam);

            switch (comboBoxDiameterType.SelectedIndex)
            {
                case 0:// Default Value
                    _knownDiamType = DiamCalType.DEFAULT;
                    _barrel.DimensionData.ActualLandDiam = _barrel.DimensionData.LandNominalDiam;
                    _barrel.BoreProfile = new BoreProfile(_barrel.DimensionData.LandNominalDiam / 2.0, _barrelType);
                    break;
                case 1://Set Value                 
                    _knownDiamType = DiamCalType.USER;
                    _barrel.DimensionData.ActualLandDiam = nomDiam;
                    _barrel.BoreProfile = new BoreProfile(nomDiam / 2.0, _barrelType);

                    break;
                case 2://Diameter Profile
                    _knownDiamType = DiamCalType.BOREPROFILE;
                    string boreFilename = textBoxNomDiam.Text;
                    if (boreFilename != "" && System.IO.File.Exists(boreFilename))
                    {
                        _barrel.BoreProfile = new BoreProfile(textBoxNomDiam.Text);

                    }
                    break;
                case 3://Ring Calibrated
                    _knownDiamType = DiamCalType.RINGCAL;
                    break;
            }
            int roundsFired = 0;
            //InputVerification.TryGetValue(textBoxCurrentPasses, "x>=0", 0, int.MaxValue, out roundsFired);
            //_barrel.LifetimeData.RoundsFired = roundsFired;           
        }

        CNCLib.XAMachPostion startPos;
        CNCLib.XAMachPostion endPos;
        int ptsPerRev;
        double axialInc;
        double pitch;
        CalDataSet calDataSet;
        ProbeSetup probeSetup;
        MeasurementUnit outputUnit;

        private void BuildScriptFromInputs(string dataFilename)
        {
            try
            {
                BuildBarrelFromInputs();                
                
                double startA = 0;
                double startX = 0;
                ptsPerRev = 1;
                startPos= new CNCLib.XAMachPostion(0,0);
                endPos = new CNCLib.XAMachPostion(0,0);
                double endA = 0;
                double endX = 0;   
                axialInc = 0;
                
                if (_scanFormat == ScanFormat.AXIAL)
                {       

                    if (radioButtonAngleInc.Checked)
                    {
                        InputVerification.TryGetValue(textBoxAngleInc, "x>0", 0, int.MaxValue, out axialInc);
                    }
                    else
                    {
                        double pointsPerInch = 1000;
                        InputVerification.TryGetValue(textBoxPtsPerRev, "x>0", 0, int.MaxValue, out pointsPerInch);
                        if (pointsPerInch == 0)
                            pointsPerInch = 1;
                        axialInc = 1 / pointsPerInch;
                    }
                }
                else
                {
                    if (radioButtonAngleInc.Checked)
                    {
                        double angleIncrement = 1;
                        InputVerification.TryGetValue(textBoxAngleInc, "x>0", 0, int.MaxValue, out angleIncrement);
                        ptsPerRev = (int)Math.Round(360.0 / angleIncrement);
                    }
                    else
                    {
                        InputVerification.TryGetValue(textBoxPtsPerRev, "x>0", 0, int.MaxValue, out ptsPerRev);
                    }
                }
                
                var grooves = new int[4];
                InputVerification.TryGetValues(textBoxGrooveList, "x>0", 1, _barrel.DimensionData.GrooveCount, out grooves);
                
               
                
                //read helix pitch
                var ringCount = Math.Abs(startA - endA) / 360.0;

                if (ringCount == 0)
                    ringCount = 1;

                pitch = Math.Abs(startX - endX) / ringCount;
                textBoxPitch.Text = pitch.ToString("f3");

                calDataSet = GetCalDataSet();
                probeSetup = GetProbeSetup();
                outputUnit = new MeasurementUnit(LengthUnit.INCH);

                if (_useFilenameData)
                { 
                        BuildScriptFromFilename(dataFilename);
                }
                else
                {
                   
                    //read start angle
                    InputVerification.TryGetValue(textBoxStartPosA, "x>0", 0, double.MaxValue, out startA);
                    //read start x 
                    InputVerification.TryGetValue(textBoxStartPosX, "x>0", 0, double.MaxValue, out startX);

                    if (textBoxEndPosA.Enabled)
                    {
                        InputVerification.TryGetValue(textBoxEndPosA, "x>0", out endA);
                        endPos.Adeg = endA;
                    }
                    else
                    {
                        endPos.Adeg = startA;
                    }
                    InputVerification.TryGetValue(textBoxEndPosX, "NaN", out endX);
                    endPos.X = endX;
                    startPos.Adeg = startA;
                    startPos.X = startX;

                    if(endA== startA)
                    {
                        axialInc = 0;
                        pitch = 0;
                    }
                    else
                    {
                        double ringRevs = (endA - startA) / 360.0;
                        pitch = (endX - startX) / ringRevs;
                        axialInc = Math.Abs((endX - startX) / (ptsPerRev * ringRevs));
                    }
                   
                    BuildScript(dataFilename);
                }
               
            }
            catch (Exception)
            {
                throw;
            }
        }
        void BuildScript(string filename)
        {
            try
            {
                switch (_scanFormat)
                {
                    case ScanFormat.RING:
                        _inspScript = new CylInspScript(_scanFormat, outputUnit, probeSetup, calDataSet, startPos, endPos, ptsPerRev);
                       
                        break;
                    case ScanFormat.SPIRAL:
                        _inspScript = new SpiralInspScript(_scanFormat, outputUnit, probeSetup, calDataSet,
                            startPos, endPos, ptsPerRev, pitch);
                        
                        break;
                    case ScanFormat.AXIAL:
                        _inspScript = new AxialInspScript(_scanFormat, outputUnit, probeSetup, calDataSet,
                            startPos, endPos, axialInc);
                            break;
                    case ScanFormat.CAL:
                    case ScanFormat.GROOVE:
                    case ScanFormat.LAND:
                    case ScanFormat.RASTER:
                    case ScanFormat.SINGLE:
                        _inspScript = new SingleCylInspScript(_scanFormat, outputUnit, probeSetup,
                                 calDataSet, startPos);                       
                        break;
                    default:
                        _inspScript = new InspectionScript(_scanFormat, outputUnit, probeSetup, calDataSet);
                        break;
                }
                _inspScript.InputDataFileName = filename;
            }
            catch (Exception)
            {

                throw;
            }
        }
        void BuildScriptFromFilename(string filename)
        {
            try
            {
                 var fileParser = new InspectionFileNameParser(filename);
                switch (_scanFormat)
                {
                    case ScanFormat.RING:
                        fileParser.ParseRingFilename(filename);
                        startPos = fileParser.Start;
                        endPos = fileParser.End;
                        _inspScript = new CylInspScript(_scanFormat, outputUnit, probeSetup, calDataSet, startPos, endPos, ptsPerRev);                        
                        break;
                    case ScanFormat.SPIRAL:
                        fileParser.ParseSpiralname(filename);
                        startPos = fileParser.Start;
                        endPos = fileParser.End;
                        var spiralPitch = fileParser.Pitch;
                        _inspScript = new SpiralInspScript(_scanFormat, outputUnit, probeSetup, calDataSet, startPos, endPos, ptsPerRev, spiralPitch);
                        
                        break;
                    case ScanFormat.AXIAL:                        
                        _inspScript = new AxialInspScript(_scanFormat, outputUnit, probeSetup, calDataSet, startPos, endPos, axialInc);
                            break;
                    case ScanFormat.CAL:
                    case ScanFormat.GROOVE:
                    case ScanFormat.LAND:
                    case ScanFormat.RASTER:
                    case ScanFormat.SINGLE:
                        _inspScript = new SingleCylInspScript(_scanFormat, outputUnit, probeSetup, calDataSet, startPos);                        
                        break;
                    default:
                        _inspScript = new InspectionScript(_scanFormat, outputUnit, probeSetup, calDataSet);
                        break;
                }
                _inspScript.InputDataFileName = filename;
            }
            catch (Exception)
            {

                throw;
            }
        }
        ProbeConfig _probeConfig;
        ProbeSetup GetProbeSetup()
        {
            try
            {
                double probePhase = 0;
                var probeSetup = new ProbeSetup();                
                probeSetup.Direction = _probeDirection;
                probeSetup.ProbeConfig = _probeConfig;
                InputVerification.TryGetValue(textBoxProbePhaseDeg, "360.0>x>=-360.0", -360.0, 360, out probePhase);
                double probePhaseRad = Math.PI * probePhase / 180.0;
                Probe probe;
                
                switch(_probeConfig)
                {
                    
                    case ProbeConfig.SINGLE_LJ_V7060:
                        probe = new Probe(Probe.ProbeType.LJ_V7060, new MeasurementUnit(LengthUnit.MICRON), probePhaseRad);                        
                        probeSetup.Add(probe);
                        break;
                    case ProbeConfig.SINGLE_SI_F10:
                        probe = new Probe(Probe.ProbeType.SI_F10, new MeasurementUnit(LengthUnit.MICRON), probePhaseRad);                        
                        probeSetup.Add(probe);
                        break;
                    case ProbeConfig.DUAL_SI_F10:
                        probe = new Probe(Probe.ProbeType.SI_F10, new MeasurementUnit(LengthUnit.MICRON), probePhaseRad);
                        probeSetup.Add(probe);
                        probe = new Probe(Probe.ProbeType.SI_F10,new MeasurementUnit(LengthUnit.MICRON),probePhaseRad);
                        probeSetup.Add(probe);
                        break;
                    case ProbeConfig.LJ_V7060_SI_F10:
                        probe = new Probe(Probe.ProbeType.LJ_V7060, new MeasurementUnit(LengthUnit.MICRON), probePhaseRad);
                        probeSetup.Add(probe);
                        probe = new Probe(Probe.ProbeType.SI_F10, new MeasurementUnit(LengthUnit.MICRON), probePhaseRad);
                        probeSetup.Add(probe);
                        break;
                }                       
                

                           
                
                return probeSetup;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        CalDataSet GetCalDataSet()
        {
            try
            {
                CalDataSet calDataSet = new CalDataSet(_barrel.DimensionData.LandNominalDiam / 2);

                switch (_knownDiamType)
                {
                    case DiamCalType.RINGCAL:
                        if (_calFilename != "" && System.IO.File.Exists(_calFilename))
                        {
                            double ringCalDiameterInch = 0.0;
                            InputVerification.TryGetValue(textBoxRingCal, "x>0", out ringCalDiameterInch);
                            calDataSet = CalDataBuilder.BuildCalData(_inspScript, ringCalDiameterInch, _calFilename);
                            _barrel.DimensionData.ActualLandDiam = calDataSet.NominalRadius * 2;
                        }
                        else
                        {
                            throw new Exception("Select Calibration File");
                        }
                        break;
                    case DiamCalType.BOREPROFILE:
                        break;
                    case DiamCalType.USER:
                        double userDiam = 0.0;
                        InputVerification.TryGetValue(textBoxNomDiam, ">0", out userDiam);
                        _barrel.DimensionData.ActualLandDiam = userDiam;
                        calDataSet = new CalDataSet(_barrel.DimensionData.ActualLandDiam / 2);
                        break;
                    case DiamCalType.DEFAULT:
                    default:
                        _barrel.DimensionData.ActualLandDiam = _barrel.DimensionData.LandNominalDiam;
                        calDataSet = new CalDataSet(_barrel.DimensionData.LandNominalDiam / 2);
                        break;
                }
                return calDataSet;
                
            }
            catch (Exception)
            {

                throw;
            }
            
        }
       // double _axialInc;
        string _outputPath;
        private string GetShortenedPathString(string path)
        {
            try
            {
                string result = path;

                int len = path.Length;
                if (len > 25)
                {
                    string firstPart = path.Substring(0, 10);
                    string lastPart = path.Substring(len - 10, 10);
                    result = string.Concat(firstPart, "...", lastPart);
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

        DataOutputOptions _dataOutOptions;
        InspectionScript _inspScript;
        Task<InspDataSet> ProcessSiDataAsync(string dataFilename, CancellationToken ct, Progress<int> progress)
        {
            try
            {
                
                double[] rawSiData = new double[1];
                if (_inspScript.ProbeSetup.ProbeConfig == ProbeConfig.SINGLE_SI_F10)
                {
                    var keyenceSiDataSet = new KeyenceSiDataSet(_inspScript, dataFilename);
                    rawSiData = keyenceSiDataSet.GetData();
                }
                else
                {
                    var keyenceMultiSiDataSet = new KeyenceDualSiDataSet(_inspScript, dataFilename);
                    rawSiData = keyenceMultiSiDataSet.GetData(_scanFormat);
                }
                switch (_inspScript.ScanFormat)
                {
                    case ScanFormat.AXIAL:                        
                        return Task.Run(() => AxialDataBuilder.BuildDataAsync(ct, progress, _inspScript as AxialInspScript, rawSiData));

                    case ScanFormat.RING:
                        
                        return Task.Run(() => RingDataBuilder.BuildDataAsync(ct, progress, _inspScript as CylInspScript, rawSiData,_barrel.DimensionData.GrooveCount));

                    case ScanFormat.SPIRAL:
                        
                        return Task.Run(() => SpiralDataBuilder.BuildDataAsync(ct, progress, _inspScript as SpiralInspScript, rawSiData, _barrel.DimensionData.GrooveCount));

                    //case ScanFormat.LINE:
                    //    var lineBuilder = new CartesianDataBuilder(_barrel);
                    //    return Task.Run(() => lineBuilder.BuildSingleLineAsync(ct, progress, _inspScript as CartInspScript, rawSiData, _dataOutOptions));
                    default:
                        return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        Task<InspDataSet> ProcessLJDataAsync(string dataFilename, CancellationToken ct, Progress<int> progress)
        {
            try
            {
              
                var keyenceLjDataSet = new KeyenceLJDataSet(_inspScript, dataFilename,new MeasurementUnit(LengthUnit.INCH));
                Vector2[] rawLJData = keyenceLjDataSet.GetData();
                switch (_inspScript.ScanFormat)
                {
                    //case ScanFormat.AXIAL:
                    //    var axialbuilder = new AxialDataBuilder(_barrel);
                    //    return Task.Run(() => axialbuilder.BuildAxialAsync(ct, progress, _inspScript as CylInspScript, rawLJData, _dataOutOptions));

                    //case ScanFormat.RING:
                    //    var ringBuilder = new RingDataBuilder(_barrel);
                    //    return Task.Run(() => ringBuilder.BuildRingAsync(ct, progress, _inspScript as CylInspScript, rawLJData, _dataOutOptions));

                    case ScanFormat.SINGLE:                        
                        return Task.Run(() => SingleScanDataBuilder.BuildDataAsync(ct, progress, _inspScript as SingleCylInspScript, rawLJData ));
                    default:
                        return null;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        Task<InspDataSet> ProcessDataAsync(string dataFilename, CancellationToken ct, Progress<int> progress)
        {
            try
            {          

                BuildScriptFromInputs(dataFilename);
                var centerline = new PointCyl(0, 0, 0);
                double[] rawSiData= new double[1] { 0 };
                Vector2[] rawLJData = new Vector2[1] { new Vector2() };
                switch(_probeConfig)
                {
                    case ProbeConfig.SINGLE_LJ_V7060:
                        return ProcessLJDataAsync(dataFilename, ct, progress);                        
                    case ProbeConfig.SINGLE_SI_F10:
                        return ProcessSiDataAsync(dataFilename, ct, progress);
                    case ProbeConfig.DUAL_SI_F10:
                        return ProcessSiDataAsync(dataFilename, ct, progress);
                    default:
                        return null;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        void ShowProgress(int p)
        {
            try
            {
                progressBarProcessing.Value = p;

            }
            catch (Exception)
            {

                throw;
            }

        }      
        private async Task ProcessFiles()
        {
            try
            {

                var cts = new CancellationTokenSource();
                var ct = cts.Token;
                string status = "Processing file";
                Refresh();
                labelStatus.Text = status;
                foreach (string filename in _inputFileNames)
                {
                    var  inspDataSet = await ProcessDataAsync(filename, ct, new Progress<int>(p => ShowProgress(p)));
                    
                    _inspDataSetList.Add(inspDataSet);
                }
                labelStatus.Text = "Finished Processing File";
                Refresh();
                ResetOnClick();               

                radioButtonViewProcessed.Checked = true;
                
                dataOuputText.Add("***Processing***");
                dataOuputText.Add(_inspScript.ScanFormat.ToString());
                dataOuputText.Add("ProbeSpacing: "+ _inspScript.CalDataSet.ProbeSpacingInch.ToString());

                if(_inspDataSetList!= null && _inspDataSetList.Count>=1)               
                {

                    foreach(InspDataSet dataset in _inspDataSetList)
                    {
                        dataOuputText.Add("Processing: " + dataset.Filename);
                        if (dataset is RingDataSet ringData)
                        {
                            
                            if (dataset.DataFormat == ScanFormat.RING)
                            {
                                dataOuputText.Add("Nominal Minimum Diameter: " + ringData.NominalMinDiam.ToString());
                                dataOuputText.Add("---Correcting Eccentricity---");
                                dataOuputText.Add("Raw land variation: " + ringData.GetRawLandVariation().ToString());
                                dataOuputText.Add("Corrected land variation: " + ringData.GetCorrectedLandVariation().ToString());
                            }
                        }
                        if(dataset is CartDataSet cartData)
                        {

                        }
                    }               
                }
                progressBarProcessing.Value = 0;
                textBoxDataOut.Lines = dataOuputText.ToArray();
               // DisplayData();
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async void ButtonProcessFile_Click(object sender, EventArgs e)
        {
            try
            {
                _inspDataSetList = new List<InspDataSet>();               
                Clear3DView();

                await ProcessFiles();               
                CorrectForAveAngle();
                ResetOnClick();
                DisplayData(BuildDisplayDataList());
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ex.StackTrace+ " : Unable to process file." );
            }
        }
        BarrelInspProfile _barrelInspProfile;
        string BuildAxialfileName(string filename)
        {
            var zStart = _barrelInspProfile.AveGrooveProfile[0].Z.ToString();
            var zEnd = _barrelInspProfile.AveGrooveProfile[_barrelInspProfile.AveGrooveProfile.Count - 1].Z.ToString();
            return DataFileBuilder.BuildFileName(_inputFileNames[0], "_profile x" + zStart + "-x" + zEnd, ".csv");
        }
        private void BuildProfile()
        {
            try
            {
               // var profileBuilder = new ProfileBuilder(_barrel);
                _barrelInspProfile = ProfileBuilder.Build(_inspDataSetList,_barrel.DimensionData.GrooveCount);
                var inspDisplayDataList = new List<DisplayData>();
                inspDisplayDataList.Add(_barrelInspProfile.MinLandProfile.AsDisplayData(ViewPlane.ZR));
                inspDisplayDataList.Add(_barrelInspProfile.AveLandProfile.AsDisplayData(ViewPlane.ZR));
                inspDisplayDataList.Add(_barrelInspProfile.AveGrooveProfile.AsDisplayData(ViewPlane.ZR));
               
                DisplayData(inspDisplayDataList);

                string fileName = BuildAxialfileName(_inputFileNames[0]);
                
                var sfd = new SaveFileDialog()
                {
                    FileName = fileName
                };
                
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SaveAxialProfileFile(sfd.FileName);
                }
               
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        void CorrectToMidpoint()
        {
            try
            {
                if(!_midpoint.Equals(null) )
                {
                    
                    var angErr = new AngleError(_barrel);
                    foreach (InspDataSet set in _inspDataSetList)
                    {
                        if(set is RingDataSet ringData)
                        {
                            var correctedCylData = angErr.CorrectToMidpoint(ringData.CorrectedCylData, _midpoint);
                            ringData.CorrectedCylData = correctedCylData;
                        }                       
                                        
                    }
                }
               
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void SetRingSwitches()
        {
            
            textBoxStartPosA.Enabled = true;
            textBoxStartPosX.Enabled = true;
            textBoxEndPosA.Enabled = false;
            textBoxEndPosX.Enabled = false;

            textBoxPitch.Enabled = false;
            textBoxPtsPerRev.Text = "8333";
            textBoxAngleInc.Text = ".0432";
            radioButtonAngleInc.Text = "Angle Increment(deg):";
            radioButtonPtsperRev.Text = "Points per Revolution:";
            radioButtonPtsperRev.Checked = true;
            
            radioButtonViewRaw.Checked = false;
            radioButtonViewRaw.Enabled = true;                        
            buttonBuildProfile.Enabled = true;          
            buttonSetRadius.Enabled = true;
        }
        private void SetSpiralSwitches()
        {
            
            textBoxStartPosA.Enabled = true;
            textBoxStartPosX.Enabled = true;
            textBoxEndPosA.Enabled = true;
            textBoxEndPosX.Enabled = true;

            textBoxPitch.Enabled = true;
            textBoxPtsPerRev.Text = "8333";
            textBoxAngleInc.Text = ".0432";
            radioButtonAngleInc.Text = "Angle Increment(deg):";
            radioButtonPtsperRev.Text = "Points per Revolution:";
            radioButtonPtsperRev.Checked = true;
            //textBoxRingRevs.Enabled = false;

            //textBoxExtractX.Enabled = true;

            radioButtonViewRaw.Checked = false;
            radioButtonViewRaw.Enabled = false;
            
            //data outputoptions
            buttonBuildProfile.Enabled = false;
            
            buttonSetRadius.Enabled = false;
        }
        //private void SetRasterSwitches()
        //{
        //    _method = ScanFormat.RASTER;
        //    textBoxStartPosA.Enabled = true;
        //    textBoxStartPosX.Enabled = true;
        //    textBoxEndPosA.Enabled = true;
        //    textBoxEndPosX.Enabled = true;

        //    textBoxPitch.Enabled = true;
        //    textBoxPtsPerRev.Enabled = true;
        //    textBoxAngleInc.Enabled = true;
        //    radioButtonAngleInc.Text = "Axial Increment(in):";
        //    radioButtonPtsperRev.Text = "Points per inch:";
        //    radioButtonPtsperRev.Checked = true;
        //    textBoxRingRevs.Enabled = false;

        //    //textBoxExtractX.Enabled = true;
        //    textBoxProbeCount.Text = "1";
        //}
        private void SetAxialSwitches()
        {
            
            textBoxStartPosA.Enabled = true;
            textBoxStartPosX.Enabled = true;
            textBoxEndPosA.Enabled = false;
            textBoxEndPosX.Enabled = true;

            textBoxPitch.Enabled = false;
            textBoxPtsPerRev.Text = "937";
            textBoxAngleInc.Text = ".001067";
            radioButtonAngleInc.Text = "Axial Inc(in):";
            radioButtonPtsperRev.Text = "Points per inch:";
            radioButtonPtsperRev.Checked = true;          
           
            buttonBuildProfile.Enabled = false;            
            buttonSetRadius.Enabled = false;
        }
        private void SetLineSwitches()
        {
            

        }
        ScanFormat _scanFormat;
        ProbeDirection _probeDirection;
        private void ComboBoxProbeDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBoxProbeDirection.SelectedIndex)
                {
                    case 0://bore id
                        _probeDirection = ProbeDirection.ID;
                        break;
                    case 1: //rod od
                        _probeDirection = ProbeDirection.OD;
                        break;
                    
                }
            }
            catch (Exception)
            {
                throw;
            }
        }        
        
        private void comboBoxProbeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            { 
               // "", "DUAL_SI_F10","SINGLE_LJ_V7060","LJ_V7060_SI_F10" };
                switch (comboBoxProbeConifg.SelectedIndex)
                {
                    case 0:
                        _probeConfig = ProbeConfig.SINGLE_SI_F10;
                        break;
                    case 1:
                        _probeConfig = ProbeConfig.DUAL_SI_F10;
                        break;
                    case 2:
                        _probeConfig = ProbeConfig.SINGLE_LJ_V7060;
                        break;
                    case 3:
                        _probeConfig = ProbeConfig.LJ_V7060_SI_F10;
                        break;
                    

                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ComboBoxMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            //RING 0
            //SPIRAL 1
            //AXIAL 2
            //LAND 3
            //GROOVE 4
            //CAL 5
            //SINGLE 6 
           

            try
            {
                switch (comboBoxMethod.SelectedIndex)
                {
                    case 0:
                        _scanFormat = ScanFormat.RING;
                        SetRingSwitches();
                        break;
                    case 1:
                        _scanFormat = ScanFormat.SPIRAL;
                        SetSpiralSwitches();
                        break;
                    case 2:
                        _scanFormat = ScanFormat.AXIAL;
                        SetAxialSwitches();
                        break;
                    case 3:
                        _scanFormat = ScanFormat.LAND;
                        break;
                    case 4:
                        _scanFormat = ScanFormat.GROOVE;
                        break;
                    case 5:
                        _scanFormat = ScanFormat.CAL;
                        break;
                    case 6:
                        _scanFormat = ScanFormat.SINGLE;
                        SetLineSwitches();
                        break;
                }
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ": Unable to change inspection method.");
            }

        }
        Barrel _barrel;
        BarrelType _barrelType;
        
        private void ComboBoxBarrel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _barrelType = Barrel.GetBarrelType(comboBoxBarrel.SelectedItem.ToString());
                _barrel = new Barrel(_barrelType);
                textBoxNomDiam.Text = _barrel.DimensionData.LandNominalDiam.ToString("f4");
                textBoxRingCal.Text = _barrel.DimensionData.RingCalibrationDiam.ToString("f4");
                _toleranceDisplayDataList = new List<DisplayData>();
                if(_barrel.ContainsMinProfile)
                {
                    var minData = _barrel.MinProfile.AsDisplayData();
                    minData.Color = System.Drawing.Color.Blue;
                    _toleranceDisplayDataList.Add(minData);
                }
                if(_barrel.ContainsMaxProfile)
                {
                    var maxData = _barrel.MaxProfile.AsDisplayData();
                    maxData.Color = System.Drawing.Color.Red;
                    _toleranceDisplayDataList.Add(maxData);
                }
                if(_barrel.ContainsNomProfile)
                {
                    var nomData = _barrel.NomProfile.AsDisplayData();
                    nomData.Color = System.Drawing.Color.Green;
                    _toleranceDisplayDataList.Add(nomData);
                }
               
            }           
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                
                MessageBox.Show(ex.Message + ex.StackTrace+ ": Unable to change barrel type.");
            }

        }


        private void RadioButtonPtsperRev_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                textBoxPtsPerRev.Enabled = radioButtonPtsperRev.Checked;
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message+ ex.StackTrace+ ": Unable to change points per rev type.");
            }

        }

        private void RadioButtonAngleInc_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                textBoxAngleInc.Enabled = radioButtonAngleInc.Checked;
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ex.StackTrace + ": ");
            }

        }
        string _lengthLabel;
        string _angleLabel;
        

        //
#region PictureBox1
        //

        


        System.Drawing.Pen _bluePen;
        System.Drawing.Pen _redPen;
        System.Drawing.Pen _greenPen;
        System.Drawing.Pen _blackPen;
        System.Drawing.Pen _orangePen;
        System.Drawing.Pen _lightGreenPen;
        Bitmap _bitmap;
      
        
        Graphics _graphics;
        ScreenTransform _screenTransform;

        private void SetupDisplay()
        {
            try
            {
                int bitmapW = pictureBox1.Width;
                int bitmapH = pictureBox1.Height;
                _bitmap = new Bitmap(bitmapW, bitmapH);
                _graphics = Graphics.FromImage(_bitmap);
                _bluePen = new System.Drawing.Pen(System.Drawing.Color.Blue);
                _redPen = new System.Drawing.Pen(System.Drawing.Color.Red);
                _greenPen = new System.Drawing.Pen(System.Drawing.Color.Green);
                _blackPen = new System.Drawing.Pen(System.Drawing.Color.Black);               
                _orangePen = new System.Drawing.Pen(System.Drawing.Color.Orange);
                _lightGreenPen = new System.Drawing.Pen(System.Drawing.Color.LightGreen);

                _prevMouseDownScr = new Point();
                _mouseDownScr = new Point();

                _prevMouseDownModel = new PointF();
                _mouseDownModel = new PointF();

                _prevMouseDownPartXY = new PointF();
                _mouseDownPartXY = new PointF();                
                
                _prevMouseNearestPart = new PointF();
                _mouseNearestPart = new PointF();
               
            }
            catch (Exception)
            {

                throw;
            }
        }
        double GetGridRange(double range)
        {
            try
            {
                double gridRange = range;
                if (range < .001)
                {
                    gridRange = .001;
                }
                if (range >= .001 & range < .005)
                {
                    gridRange = .005;
                }
                if (range >= .005 & range < .01)
                {
                    gridRange = .01;
                }
                if (range >= .01 & range < .05)
                {
                    gridRange = .05;
                }
                if (range >= .05 & range < .1)
                {
                    gridRange = .1;
                }
                if (range >= .1 && range < .5)
                {
                    gridRange = .5;
                }
                if (range >= .5 && range < 1.0)
                {
                    gridRange = 1;
                }
                if (range >= 1 && range < 5)
                {
                    gridRange = 5;
                }
                if (range >= 5 && range <= 10)
                {
                    gridRange = 10;
                }
                if (range >= 10 && range < 50)
                {
                    gridRange = 50;
                }
                if (range >= 50 && range < 100)
                {
                    gridRange = 100;
                }
                if (range >= 100 && range < 1000)
                {
                    gridRange = 1000;
                }
                return gridRange;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        string GetLabelFormat(int decimalPlaces)
        {
           return "f" + decimalPlaces.ToString();
        }
        void DrawVGridLine(double xLabel,RectangleF rect,Font font,int decimalPlaces )
        {
            var ptX = _screenTransform.GetScreenCoords(xLabel, rect.Top);
            var ptYGridStart = _screenTransform.GetScreenCoords(xLabel, rect.Top);
            var ptYGridEnd = _screenTransform.GetScreenCoords(xLabel, rect.Bottom);
            string labelFormat = GetLabelFormat(decimalPlaces);
            string xLabelstr = xLabel.ToString(labelFormat);
            SizeF sizeXlabel = _graphics.MeasureString(xLabelstr, font);
            float xLabelxOffset = (float)(sizeXlabel.Width * 0.5);
            float xLabelyOffset = 0f;
            _graphics.DrawString(xLabelstr, font, System.Drawing.Brushes.Black, ptX.X - xLabelxOffset, ptX.Y + xLabelyOffset);
            _graphics.DrawLine(_greenPen, ptYGridStart, ptYGridEnd);
        }
        void DrawVGrid(int xGridCount,double dxGrid, Font font, RectangleF rect,int decimalPlaces)
        {
            try
            {
                double xLabel = 0;
               if(rect.Left<0 && rect.Right>0)
               {
                    for (int i = 0; i <= (xGridCount/2); i++)
                    {
                        xLabel = (i * dxGrid);
                        DrawVGridLine(xLabel, rect, font,decimalPlaces);
                    }
                    for (int i = 1; i <= (xGridCount / 2); i++)
                    {
                        xLabel = -1* (i * dxGrid);
                        DrawVGridLine(xLabel, rect, font,decimalPlaces);
                    }
                }
               else
               {
                    for (int i = 0; i <= xGridCount; i++)
                    {
                        xLabel = rect.X + (i * dxGrid);
                        DrawVGridLine(xLabel, rect, font,decimalPlaces);
                    }
                }
                 
               
               
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        void DrawHGridLine(double labelValue, RectangleF rect, Font font, int decimalPlaces)
        {
            PointF labelPtF;
            if ((rect.Top>0 && rect.Bottom<0 )||(rect.Top < 0 && rect.Bottom > 0))
            {
                labelPtF = _screenTransform.GetScreenCoords(0, labelValue);
            }
            else
            {
                var mid = (rect.Left + rect.Right) / 2;
                labelPtF = _screenTransform.GetScreenCoords(mid, labelValue);
            }
                       
            var ptGridStart = _screenTransform.GetScreenCoords(rect.Left,labelValue);
            var ptGridEnd = _screenTransform.GetScreenCoords(rect.Right,labelValue);
            string labelFormat = GetLabelFormat(decimalPlaces);
            string labelstr = labelValue.ToString(labelFormat);
            var sizeLabel = _graphics.MeasureString(labelstr, font);
            float labelxOffset = (float)(sizeLabel.Width * 0.5);
            float labelyOffset = 0f;
            _graphics.DrawString(labelstr, font, System.Drawing.Brushes.Black, labelPtF.X - labelxOffset, labelPtF.Y - labelyOffset);
            _graphics.DrawLine(_greenPen, ptGridStart, ptGridEnd);
        }
        void DrawHGrid(int hGridCount, double dyGrid, Font font, RectangleF rect,int decimalPlaces)
        {
            try
            {
                for (int i = 0; i <= hGridCount; i++)
                {
                    double yLabel = rect.Top + (i * dyGrid);
                    DrawHGridLine(yLabel, rect, font, decimalPlaces);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        void DrawGrid(RectangleF rect,int decimalPlaces  )
        {
            try
            {
                double sizeX = 0;
                var sizeY = GetGridRange(rect.Height);
                int xGridCount;
                int yGridCount = 10;
                if (_scanFormat == ScanFormat.AXIAL ||  _scanFormat== ScanFormat.SINGLE)
                {
                    xGridCount = 10;
                    sizeX = GetGridRange(rect.Width);
                }
                else
                {
                    xGridCount = _barrel.DimensionData.GrooveCount;
                    sizeX = rect.Width;
                }
                float midX =(float)( (rect.Left + rect.Right) / 2.0);
                float midY = (float)((rect.Top + rect.Bottom) / 2.0);

                RectangleF gridRect = new RectangleF();
                gridRect.X = (float)(midX - sizeX / 2.0);
                gridRect.Y = (float)(midY - sizeY / 2.0);
                gridRect.Height =(float) sizeY;
                gridRect.Width = (float) sizeX;
                double dxGrid = sizeX / xGridCount;
                double dyGrid = sizeY / yGridCount;
                var font = new Font(this.Font, FontStyle.Regular);
                //VERTICAL LINE grids
                DrawVGrid(xGridCount, dxGrid, font, gridRect, decimalPlaces);
                //horizontal line grids
                DrawHGrid(yGridCount, dyGrid, font, gridRect, decimalPlaces);
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        void DrawCircle(System.Drawing.Pen pen, Point pt, int screenRadius)
        {
            try
            {               
                    var rect = new RectangleF(pt.X - screenRadius, pt.Y - screenRadius, screenRadius * 2, screenRadius * 2);
                    _graphics.DrawEllipse(pen, rect);                
            }
            catch (Exception)
            {

                throw;
            }

        }
      
        void DrawLines(System.Drawing.Pen pen, List<Line2> lines)
        {
            try
            {
                foreach(Line2 line in lines)
                {
                    var pt1 = _screenTransform.GetScreenCoords(line.Point1.X,line.Point1.Y);
                    var pt2 = _screenTransform.GetScreenCoords(line.Point2.X, line.Point2.Y);
                    _graphics.DrawLine(pen,pt1,pt2);
                }
                pictureBox1.Image = _bitmap;
            }
            catch (Exception)
            {

                throw;
            }
        }
        List<DisplayData> BuildDisplayDataList()
        {
            try
            {
                _displayDataList = new List<DisplayData>();
                foreach (InspDataSet dataSet in _inspDataSetList)
                {
                    var displayData = new DisplayData(dataSet.Filename);
                    if (dataSet is CartDataSet cartData)
                    {
                        _viewPlane = ViewPlane.XY;
                        displayData = cartData.CartData.AsDisplayData(_viewPlane);
                        
                        displayData.FileName = cartData.Filename;
                    }
                    if (dataSet is AxialDataSet axialData)
                    {
                        _viewPlane = ViewPlane.ZR;
                        displayData = axialData.CorrectedCylData.AsDisplayData(_viewPlane);
                        displayData.FileName = axialData.Filename;
                    }
                    if (dataSet is RingDataSet ringData)
                    {
                        _viewPlane = ViewPlane.THETAR;
                        if (_viewProcessed)
                        {
                            displayData = ringData.CorrectedCylData.AsDisplayData(_viewPlane);
                        }
                        else
                        {
                            displayData = ringData.UncorrectedCylData.AsDisplayData(_viewPlane);
                        }
                        displayData.FileName = ringData.Filename;
                    }
                    displayData.Color = System.Drawing.Color.HotPink;
                    _displayDataList.Add(displayData);
                }
                return _displayDataList;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
       
        RectangleF GetRectangle(List<DisplayData> displayDataList, float borderPercent, int decimalPlaces)
        {
            var rectList = new List<RectangleF>();
            foreach (var dd in displayDataList)
            {
                rectList.Add(dd.BoundingRect(borderPercent, decimalPlaces));
            }
            float xmin = float.MaxValue;
            float xmax = float.MinValue;
            float ymin = float.MaxValue;
            float ymax = float.MinValue;
            foreach (RectangleF r in rectList)
            {
                if (r.Left < xmin)
                    xmin = r.Left;
                if (r.Right > xmax)
                    xmax = r.Right;
                if (r.Top < ymin)
                    ymin = r.Top;
                if (r.Bottom > ymax)
                    ymax = r.Bottom;

            }
            return new RectangleF(xmin, ymin, xmax - xmin, ymax - ymin);
        }
       
        DisplayData BuildScreenPts(DisplayData displayData)
        {
            try
            {
                var screenPts = new DisplayData(displayData.FileName);
                screenPts.Color = displayData.Color;
                foreach (var pt in displayData)
                {
                    screenPts.Add(_screenTransform.GetScreenCoords(pt.X, pt.Y));
                }
                return screenPts;
            }
            catch (Exception)
            {

                throw;
            }

        }
        List<DisplayData> BuildScreenPtList(List<DisplayData>  displayDataList)
        {
            try
            {
                
                var screenPtsList = new List<DisplayData>();
               
                
                foreach (DisplayData displayData in  displayDataList)                {
                   
                   
                    screenPtsList.Add(BuildScreenPts(displayData));
                }
                return screenPtsList;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
       
        
        
        private void DisplayData(List<DisplayData> displayData )
        { 
            try
            {
                float borderPercent = .10f;
                int decimalPlaces = 4;
                bool stretchToFit = true;
                if (_inspDataSetList[0] is SpiralDataSet spiralData)
                {
                    Load3DView(spiralData.CorrectedSpiralData);
                }
                else
                {
                    

                    var mRect = GetRectangle(displayData, borderPercent, decimalPlaces);

                    if(_overlayMinMax)
                    {
                        var trimmedTolDisplayData = new List<DisplayData>();
                        foreach(var ddata in _toleranceDisplayDataList)
                        {
                            trimmedTolDisplayData.Add(ddata.TrimTo(mRect,true));
                            displayData.AddRange(trimmedTolDisplayData);
                        }
                        mRect = GetRectangle(displayData, borderPercent, decimalPlaces);
                    }

                    _screenTransform = new ScreenTransform(mRect, pictureBox1.DisplayRectangle, borderPercent, stretchToFit);
                    SetupDisplay();
                    var screenPtsList = BuildScreenPtList(displayData );
                    DrawGrid(mRect,decimalPlaces);
                    foreach (DisplayData dd in screenPtsList)
                    {
                        var pen = new System.Drawing.Pen(dd.Color);
                        _graphics.DrawLines(pen, dd.ToArray());
                    }
                    pictureBox1.Image = _bitmap;

                }                
            }
            catch (Exception)
            {

                throw;
            }
        }
        double _dataRotationRad;
        
        void DrawWindow(Point start, Point end, DataSelection dataSelection)
        {
            try
            {
                endPartCoords = _screenTransform.GetModelCoords(end);
                startPartCoords = _screenTransform.GetModelCoords(start);
                var bit2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                var gtemp = Graphics.FromImage(bit2);
                var dashpen = new System.Drawing.Pen(System.Drawing.Color.Red)
                {
                    DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
                };
                gtemp.FillRectangle(_imgBrush, 0, 0, pictureBox1.Width, pictureBox1.Height);
                var pt = new Point(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y));
                
                var rect = new System.Drawing.Rectangle(pt, new Size(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y)));
               
                
                gtemp.DrawRectangle(dashpen, rect);
                SolidBrush brush = new SolidBrush(System.Drawing.Color.White);               
                pictureBox1.Image = bit2;
            }
            catch (Exception)
            {

                throw;
            }

        }
        PointF endPartCoords;
        PointF startPartCoords;

        void DrawMeasurement(Point start, Point end,DataSelection dataSelection )
        {
            try
            {
                var bit2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                var gtemp = Graphics.FromImage(bit2);
                var dashpen = new System.Drawing.Pen(System.Drawing.Color.Red)
                {
                    DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
                };
                gtemp.FillRectangle(_imgBrush, 0, 0, pictureBox1.Width, pictureBox1.Height);

               
                endPartCoords = _screenTransform.GetModelCoords(end);
                startPartCoords = _screenTransform.GetModelCoords(start);
                string measurement = "";
                if (dataSelection ==DataSelection.MEASURELENGTH)
                {
                    double xLen = Math.Abs(endPartCoords.X - startPartCoords.X);
                    double yLen = endPartCoords.Y - startPartCoords.Y;
                    measurement = xLen.ToString("f4") + "," + yLen.ToString("f5");
                    labelDxMeasured.Text = "DTheta: " + xLen.ToString("f4") + " " + _angleLabel;
                    labelDyMeasured.Text = "Dr: " + yLen.ToString("f5") + " " + _lengthLabel;
                }
                if(dataSelection == DataSelection.ROTATE || dataSelection== DataSelection.FITCIRCLE)
                {
                    _dataRotationRad = Math.Atan2(endPartCoords.Y - startPartCoords.Y, endPartCoords.X - startPartCoords.X);
                    double angle = GeometryLib.GeomUtilities.ToDegs(_dataRotationRad);
                    measurement = angle.ToString("f4");
                }
                
                var size = _graphics.MeasureString(measurement, this.Font);
                int midX = (int)(start.X + (end.X - start.X - (size.Width / 2.0)) / 2.0);
                int midY = (int)(start.Y + (end.Y - start.Y - (size.Height / 2.0)) / 2.0);
                gtemp.DrawLine(_blackPen, end, start);
                SolidBrush brush = new SolidBrush(System.Drawing.Color.White);
                gtemp.FillRectangle(brush, midX, midY, size.Width, size.Height);
                gtemp.DrawRectangle(_blackPen, midX, midY, size.Width, size.Height);
                gtemp.DrawString(measurement, Font, System.Drawing.Brushes.Black, new PointF(midX, midY));                
                pictureBox1.Image = bit2;
            }
            catch (Exception)
            {

                throw;
            }
 
        }
        PointCyl _knownRadiusPt;
        
        Point _mouseDownScr;
        Point _prevMouseDownScr;

        PointF _prevMouseDownModel;
        PointF _mouseDownModel;

        PointF _mouseUpPart;
        PointF _mouseLocPart;

        Point _mouseUpLocation;
        Point _mouseLocation;

        PointF _mouseDownPartXY;
        PointF _prevMouseDownPartXY;

        PointF _mouseNearestPart;
        PointF _prevMouseNearestPart;

        PointF _mousePartXY;
        TextureBrush _imgBrush;
        int _mouseDownCount;
        List<DisplayData> _inspDisplayDataList;
        List<DisplayData> _toleranceDisplayDataList;
        List<DisplayData> _displayDataList;
        private string GetFilenameNearestPoint(PointF pt)
        {
            try
            {
                return DataLib.DisplayData.GetNearestFile(pt, _displayDataList);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private PointF GetNearestModelPoint(PointF pt)
        {
            try
            {
                
                return DataLib.DisplayData.GetNearestPoint(pt, _displayDataList);
            }
            catch (Exception)
            {
                throw;
            }
        }
       
        private void SetMidpointClick(PointF pt)
        {
            try
            {               
                
                _midpoint = new PointCyl(pt.Y, pt.X, 0);
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        

        private void SetRadiusMouseClick(PointF pt)
        {
            try
            {               
                _knownRadiusPt = new PointCyl(pt.Y, pt.X, 0);
                textBoxCurrentRadius.Text = _knownRadiusPt.R.ToString("f5");
            }
            catch (Exception)
            {

                throw;
            }
              
        }
  
        private void PictureBox1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (_inspDataSetList != null && _screenTransform!=null && _inspDataSetList.Count > 0)
                {

                    _mouseLocation = e.Location;
                    _mouseLocPart = _screenTransform.GetModelCoords(e.Location);
                    _mousePartXY = GetXYCoords(_mouseLocPart);
                    switch (_scanFormat )
                    {
                        case ScanFormat.AXIAL:
                            labelZPosition.Text = "Axial: " + _mouseLocPart.X.ToString("F6") + _lengthLabel;
                            labelYPosition.Text = "Radius: " + _mouseLocPart.Y.ToString("f6") + _lengthLabel;                            
                            break;
                        case ScanFormat.RING:
                        case ScanFormat.SPIRAL:
                            double angle = _mouseLocPart.X;
                            labelXPosition.Text = "Angle: " + angle.ToString("f6") + " " + _angleLabel;
                            labelYPosition.Text = "Radius: " + _mouseLocPart.Y.ToString("f6") + " " + _lengthLabel;
                            break;
                        case ScanFormat.SINGLE:
                        default:
                            labelZPosition.Text = "X: " + _mouseLocPart.X.ToString("F6") + _lengthLabel;
                            labelYPosition.Text = "Y: " + _mouseLocPart.Y.ToString("f6") + _lengthLabel;
                            break;
                           
                    }

                    if (_mouseDownCount == 1 )
                    {
                        if (_dataSelection == DataSelection.MEASURELENGTH
                            || _dataSelection == DataSelection.ROTATE 
                            ||  _dataSelection== DataSelection.FITCIRCLE)
                        {                        
                            DrawMeasurement(_mouseDownScr, _mouseLocation,_dataSelection);
                        }
                        if(_dataSelection == DataSelection.WINDOWDATA)
                        {
                            DrawWindow(_mouseDownScr, _mouseLocation, _dataSelection);
                        }
                    }  
                   
                }
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);
            }

        }
        private void PictureBox1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                _mouseDown = false;
                if (_inspDataSetList != null && _screenTransform != null && _inspDataSetList.Count > 0)
                {
                    _mouseUpLocation = e.Location;
                    _mouseUpPart = _screenTransform.GetModelCoords(_mouseUpLocation);                    
                }
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);
            }
        }
        bool _mouseDown;
        private void PictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                _mouseDown = true;
                if (_inspDataSetList != null && _screenTransform != null && _inspDataSetList.Count > 0)
                {


                   
                    _mouseDownCount++;
                    
                    var prevMouseDownScr = _mouseDownScr;
                    _mouseDownScr = e.Location;
                    _prevMouseDownModel = _mouseDownModel;
                    _mouseDownModel = _screenTransform.GetModelCoords(_mouseDownScr);

                    _prevMouseDownPartXY = _mouseDownPartXY;
                    _mouseDownPartXY = GetXYCoords(_mouseDownModel);

                    _prevMouseNearestPart = _mouseNearestPart;
                    _mouseNearestPart = GetNearestModelPoint(_mouseDownPartXY);

                   var nearestFile = "";
                   GetFilenameNearestPoint(_mouseDownPartXY);
                   labelNearestFilename.Text = "File: " + nearestFile;
                    switch (_dataSelection)
                    {

                        case DataSelection.SETRADIUS:
                            SetRadiusMouseClick(_mouseNearestPart);
                            DisplayData( BuildDisplayDataList());
                            break;
                        case DataSelection.SELECTMIDPOINT:
                            SetMidpointClick(_mouseNearestPart);
                            CorrectToMidpoint();
                            DisplayData(BuildDisplayDataList());
                            break;
                    }

                    var img = new Bitmap(pictureBox1.Image);
                    _imgBrush = new TextureBrush(img);
                    if (_dataSelection == DataSelection.MEASURELENGTH 
                        || _dataSelection == DataSelection.ROTATE 
                        || _dataSelection==DataSelection.WINDOWDATA
                        || _dataSelection== DataSelection.FITCIRCLE)
                    {
                        if (_mouseDownCount == 2)
                        {
                            DrawMeasurement(prevMouseDownScr, _mouseDownScr,_dataSelection );
                            if (_dataSelection == DataSelection.ROTATE)
                            {
                                RotateDataToLine(_prevMouseDownPartXY,_mouseDownPartXY);
                                _dataSelection = DataSelection.NONE;
                            }
                            
                            if(_dataSelection == DataSelection.FITCIRCLE)
                            {
                                FitToCircle(_prevMouseDownModel, _mouseDownModel);
                                _dataSelection = DataSelection.NONE;
                            }
                            if(_dataSelection == DataSelection.WINDOWDATA)
                            {
                                WindowData(_prevMouseDownPartXY, _mouseDownPartXY);
                                _dataSelection = DataSelection.NONE;
                            }
                        }
                        
                    }
                    if (_mouseDownCount >= 3)
                    {                  
                        pictureBox1.Image = _bitmap;
                        pictureBox1.Refresh();
                        _mouseDownCount = 1;

                    }
                }
            }
            catch (Exception ex)
            {

                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);
            }

        }
        private PointF GetXYCoords(PointF partCoords)
        {
            try
            {

                if(_viewPlane ==ViewPlane.THETAR)
                {
                    var x = Math.Cos(partCoords.X) * partCoords.Y;
                    var y = Math.Sin(partCoords.X) * partCoords.Y;
                    return new PointF((float)x, (float)y);
                }
                else
                {
                    return partCoords;
                }
               
            }
            catch (Exception)
            {

                throw;
            }

        }

#endregion
        private void MainInspectionForm_Resize(object sender, EventArgs e)
        {
            try
            {
                if (this.Height > 30)
                {
                    int pbW = this.Size.Width - 472;
                    int pbH = Size.Height - 110;
                    tabControlOutput.Size = new Size(pbW, pbH);
                    int gb1x = tabControlOutput.Location.X + tabControlOutput.Size.Width + 6;
                    int gb1y = 60;
                    tabControlParams.Location = new Point(gb1x, gb1y);
                    
                    if (_inspDataSetList != null && _inspDataSetList.Count>0)
                    {                        
                        DisplayData(BuildDisplayDataList());
                    }
                }

            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message);
            }
        }
        ViewPlane _viewPlane;       
        bool _viewProcessed;

        private void Save3DScreenView()
        {
            try
            {
                PictureBoxUtilities.SaveScreenShotasPicture(elementHost1);               
            }
            catch (Exception)
            {

                throw;
            }
           
           
        }
        private void RadioButtonViewProcessed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {               
                _viewProcessed = radioButtonViewProcessed.Checked;
                if (_inspDataSetList != null && _inspDataSetList.Count >0)
                {                   
                    DisplayData(BuildDisplayDataList());
                }
            }
            catch (Exception ex)
            {

                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ": Unable to change to/from processed view.");
            }

        }

        List<string> dataOuputText;
        void CorrectForAveAngle()
        {
            try
            {
               
                if (_inspDataSetList != null && _inspDataSetList.Count >0)
                {
                    
                    foreach (InspDataSet inspDataSet in _inspDataSetList)
                    {
                        var da = new AngleError(_barrel);
                        
                        CylData corrData;
                       if(inspDataSet is RingDataSet ringData)
                        {
                            corrData = da.CorrectForAngleError(ringData.CorrectedCylData);


                            ringData.CorrectedCylData = corrData;
                            ringData.CorrectedLandPoints = da.CorrectForError(ringData.CorrectedLandPoints);

                            dataOuputText.Add("***Correcting Angle****" + inspDataSet.Filename);
                            dataOuputText.Add("Correction angle: " + GeometryLib.GeomUtilities.ToDegs(da.CorrectionAngle).ToString("f5") + " degs");
                            dataOuputText.Add("Ave radius: " + da.AveRadius.ToString("f5"));
                                               
                        }
                        
                    }
                   
                   // DisplayData();
                }
                else
                {
                    textBoxDataOut.Text = "Process Data First";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ButtonGetAveAngle_Click(object sender, EventArgs e)
        {
            try
            {
                CorrectForAveAngle();
            }
            catch (Exception ex)
            {

                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ": Unable to auto correct for angle. Manually select sidewalls to correct for angle error.");
            }
        }

        
        //List<PointCyl> _depthMeasurePoints;
       // BarrelXsectionProfile _grooveMeasurements;
       

       
#region Toolstrip buttons

        private void ToolStripButtonFileOpen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenRawDataFiles();
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message );

            }
        }
        private async void ToolStripButtonFileSave_Click(object sender, EventArgs e)
        {
            try
            {
                await SaveAllOutputFiles(saveCSV:true,saveDXF:true);
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message  );

            }
        }

       
        void ResetOnClick()
        {
          
           
            
            toolStripButtonCursor.Checked = false;
            toolStripButtonCursor.BackColor = DefaultBackColor;
            toolStripButtonLength.Checked = false;
            toolStripButtonLength.BackColor = DefaultBackColor;
            toolStripButtonSetKnownRadius.Checked = false;
            toolStripButtonSetKnownRadius.BackColor = DefaultBackColor;           
            
            
        }
        enum DataSelection
        {
            SETRADIUS,
            SELECTGROOVES,
            SELECTLANDS,
            MEASURELENGTH,
            SELECTDATAFORFIT,
            SELECTMIDPOINT,
            ROTATE,
            MIRROR,
            WINDOWDATA,
            FITCIRCLE,                    
            NONE
        }
        private DataSelection _dataSelection;
        private void ToolStripButtonCursor_Click(object sender, EventArgs e)
        {
            _dataSelection = DataSelection.NONE;
            ResetOnClick();
            toolStripButtonCursor.BackColor = System.Drawing.Color.Red;
            labelStatus.Text = "";
        }
        
        private void ToolStripButtonLength_Click(object sender, EventArgs e)
        {
            try
            {
                ResetOnClick();

                //_displayData.HiLightPts.Clear();
                _dataSelection = DataSelection.MEASURELENGTH;
                toolStripButtonLength.BackColor = System.Drawing.Color.Red;                
               
            }
            catch (Exception ex)
            {

                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":");
            }
        }

        private void ToolStripButtonSetKnownRadius_Click(object sender, EventArgs e)
        {
            try
            {
                ResetOnClick();                
                _dataSelection = DataSelection.SETRADIUS;
                toolStripButtonSetKnownRadius.BackColor = System.Drawing.Color.Red;
                labelStatus.Text = "Select point to set to known radius.";
                _knownRadiusPt = new PointCyl();
            }
            catch (Exception ex)
            {

                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message );
            }

        }

       
        bool _selectMidpoint;
        PointCyl _midpoint;
       
        #endregion
        #region MainMenu
        async Task SaveAllOutputFiles(bool saveCSV,bool saveDXF)
        {
            try
            {
                List<string> fileNames = new List<string>();
                bool saveFile = false;
                if (_inspDataSetList != null && _inspDataSetList.Count >= 1)
                {
                    
                    if (_inspDataSetList.Count == 1)
                    {
                        var sfd = new SaveFileDialog
                        {
                            Title = "Save All Files",
                            FileName = DataFileBuilder.BuildFileName(_inspDataSetList[0].Filename, "_out", ".csv")
                            
                        };
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            fileNames.Add( sfd.FileName);
                            saveFile = true;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < _inspDataSetList.Count; i++)
                        {
                            fileNames.Add(DataFileBuilder.BuildFileName(_inspDataSetList[i].Filename, "_out", ".csv"));
                            saveFile = true;
                        }
                    }
                    if(saveFile)
                    {
                        for (int i = 0; i < _inspDataSetList.Count; i++)
                        {
                            textBoxDataOut.Text = "Saving Files " + fileNames[i];
                            string fileCount = (i + 1).ToString();
                            string totalFileCount = _inspDataSetList.Count.ToString();
                            labelStatus.Text = "Saving File " + fileCount + " of " + totalFileCount;
                            if (_inspDataSetList[i] is SpiralDataSet spiralData)
                            {
                                if(saveCSV)
                                {
                                   DataFileBuilder.SaveCSVFile(_barrel, _dataOutOptions, spiralData.CorrectedSpiralData,
                                   fileNames[i], new Progress<int>(p => ShowProgress(p)));
                                    progressBarProcessing.Value = 0;
                                }
                                if(saveDXF)
                                {
                                   DataFileBuilder.SavePlyFile(_barrel, _dataOutOptions, spiralData.CorrectedSpiralData,
                                    _barrel.DimensionData.LandNominalDiam / 2.0, fileNames[i], new Progress<int>(p => ShowProgress(p)));
                                    progressBarProcessing.Value = 0;
                                }
                               
                                
                            }
                            if (_inspDataSetList[i] is RingDataSet ringData)
                            {
                                if (saveCSV)
                                {
                                     DataFileBuilder.SaveCSVFile(_barrel, _dataOutOptions, ringData.CorrectedCylData,
                                    fileNames[i], new Progress<int>(p => ShowProgress(p)));
                                    progressBarProcessing.Value = 0;
                                }
                                if (saveDXF)
                                {
                                    DataFileBuilder.SaveDXF(ringData.CorrectedCylData, fileNames[i], new Progress<int>(p => ShowProgress(p)));
                                    progressBarProcessing.Value = 0;
                                }
                            }
                            if(_inspDataSetList[i] is CartDataSet cartData)
                            {
                                if (saveCSV)
                                {
                                    DataFileBuilder.SaveCSVFile(_barrel, _dataOutOptions, cartData.CartData,
                                   fileNames[i], new Progress<int>(p => ShowProgress(p)));
                                    progressBarProcessing.Value = 0;
                                }
                                if (saveDXF)
                                {
                                    DataFileBuilder.SaveDXF(cartData.CartData, fileNames[i], new Progress<int>(p => ShowProgress(p)));
                                    progressBarProcessing.Value = 0;
                                }
                            }
                        }
                       
                    }
                    labelStatus.Text = "Finished Saving Files ";
                }


                


            }
            catch (Exception)
            {

                throw;
            }
        }
        
        void SaveAxialProfileFile(string filename)
        {
            if (_barrelInspProfile != null)
            {
                DataFileBuilder.SaveProfileFile(_barrel, _dataOutOptions, _barrelInspProfile, filename,
                    new Progress<int>(p => ShowProgress(p)));
            }
        }
        
        private void OpenRawDataFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenRawDataFiles();
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);

            }
        }
        private void SaveProcessedFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private Task SaveCSVFile(string filename, CylData data, Progress<int> progress)
        {
            try
            {
                return Task.Run(() => DataFileBuilder.SaveCSVFile(_barrel, _dataOutOptions, data, filename, progress));
            }
            catch (Exception)
            {

                throw;
            }

        }
        
     
      
        
        private  async void SaveProfileDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
              await SaveAllOutputFiles(saveCSV: true, saveDXF: true);
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);

            }
        }
        //void SaveDepths()
        //{
        //    try
        //    {
        //        if (_grooveMeasurements != null)
        //        {
        //            var sfd = new SaveFileDialog
        //            {
        //                Filter = "(*.csv)|*.csv",
        //                FileName = DataFileBuilder.BuildFileName(_inputFileNames[0], "_depths", ".csv"),
        //                DefaultExt = ".csv",
        //                Title = "Depth Measurement File",
        //                AddExtension = true
        //            };

        //            if (sfd.ShowDialog() == DialogResult.OK)
        //            {
        //                labelStatus.Text = "Saving Groove CSV File";
        //                _grooveMeasurements.SaveMeasurements(sfd.FileName, _inputFileNames[0], true, true);
        //                labelStatus.Text = "Finished Saving File";
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
        //private void SaveDepthDataToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        SaveDepths();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logFile.SaveMessage(ex);
        //        MessageBox.Show(ex.Message + ":" + ex.StackTrace);

        //    }


        //}
        List<InspDataSet> _inspDataSetList;
       
        private void SaveDXFProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveAllOutputFiles(saveCSV: false, saveDXF: true);
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);

            }

        }
        private Task SavePlyFile(string filename,SpiralDataSet spiralData, IProgress<int> progress)
        {
            try
            {               
                return Task.Run(() => DataFileBuilder.SavePlyFile(_barrel, _dataOutOptions, spiralData.CorrectedSpiralData, _barrel.DimensionData.LandNominalDiam / 2.0, filename, progress));

            }
            catch (Exception)
            {

                throw;
            }

        }
        private async Task SavePlyFileAsync()
        {
            try
            {
                if (_inspDataSetList != null && _inspDataSetList[0] is SpiralDataSet spiralData)
                {
                    var sfd = new SaveFileDialog
                    {
                        Filter = "(*.ply)|*.ply",
                        FileName = DataFileBuilder.BuildFileName(_inspDataSetList[0].Filename, "_3D", ".ply"),
                        DefaultExt = ".ply",
                        Title = "3D PLY File",
                        AddExtension = true
                    };

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        labelStatus.Text = "Saving 3D Surface File";
                        await SavePlyFile(sfd.FileName,spiralData, new Progress<int>(p => ShowProgress(p)));
                        labelStatus.Text = "Finished Saving File";
                        progressBarProcessing.Value = 0;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async void Save3DSurfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await SavePlyFileAsync();
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);

            }
        }
        private Task SaveSTLFile(string filename,SpiralDataSet spiralData,IProgress<int>progress)        
        {
            return Task.Run(() => DataFileBuilder.SaveSTLFile(_barrel, _dataOutOptions, spiralData.CorrectedSpiralData, _barrel.DimensionData.LandNominalDiam / 2.0, filename, progress));
        }
        private async Task SaveSTLFileAsync()
        {
            try
            {
                if (_inspDataSetList != null && _inspDataSetList[0] is SpiralDataSet spiralData)
                {
                    var sfd = new SaveFileDialog
                    {
                        Filter = "(*.stl)|*.stl",
                        FileName = DataFileBuilder.BuildFileName(_inspDataSetList[0].Filename, "_3D", ".stl"),
                        DefaultExt = ".stl",
                        Title = "3D STL File",
                        AddExtension = true
                    };

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        labelStatus.Text = "Saving 3D Surface File";
                        await SaveSTLFile(sfd.FileName,spiralData, new Progress<int>(p => ShowProgress(p)));
                        labelStatus.Text = "Finished Saving File";
                        progressBarProcessing.Value = 0;
                    }
                }
            }
            catch (Exception )
            {
                throw;
            }
        }
        private async void  save3DSTLSurfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await SaveSTLFileAsync();
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);

            }
        }
        private async void SaveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await SaveAllOutputFiles(saveCSV: true, saveDXF: true);
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message +":" +ex.StackTrace);

            }
        }
        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var optForm = new OptionsForm(_logFile);
                if (optForm.ShowDialog() == DialogResult.OK)
                {
                    _dataOutOptions = optForm.Options;
                }
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message +":" +ex.StackTrace);

            }
        }

        private void ConnectToDAQToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                throw new Exception("not implemented");
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message +":" +ex.StackTrace);

            }
        }

        private void DisconnectFromDAQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                throw new Exception("not implemented");
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message +":" +ex.StackTrace);

            }
        }
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

#endregion
        
       
        private void ButtonSetRadius_Click(object sender, EventArgs e)
        {
            try
            {
                if (_inspDataSetList[0] != null && textBoxKnownRadius.Text != "" )
                {
                    double knownR = 0.0;
                    if(InputVerification.TryGetValue(textBoxKnownRadius, "x>=0",0,double.MaxValue, out knownR))
                    {
                        if (_inspDataSetList[0] is RingDataSet ringData)
                        {
                            var resetData = DataBuilder.ResetToKnownRadius(ringData.CorrectedCylData, _knownRadiusPt, knownR);                             
                            //_inspDisplayDataList.Clear();
                            var displayDataList = new List<DataLib.DisplayData>();
                            displayDataList.Add(resetData.AsDisplayData(ViewPlane.THETAR));
                            DisplayData(displayDataList);
                        }
                    }                  
                }
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message +": Unable to set radius. ");

            }

        }

        private void ComboBoxManStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message +":" );

            }
        }
        DiamCalType _knownDiamType;
        
        private void ComboBoxDiameterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBoxDiameterType.SelectedIndex)
                {
                    case 0://default diameter from dimension data
                        _knownDiamType = DiamCalType.DEFAULT;
                        textBoxNomDiam.Enabled = true;
                        buttonBrowse.Enabled = false;
                        labelNomDiam.Text = "Default Diameter:";
                        textBoxNomDiam.Text = _barrel.DimensionData.LandNominalDiam.ToString("f4");
                        labelCalStatus.Text = "Cal set to barrel default.";
                        textBoxNomDiam.Enabled = false;
                        break;
                    case 1://user set diameter
                        _knownDiamType = DiamCalType.USER;
                        textBoxNomDiam.Enabled = true;
                        buttonBrowse.Enabled = false;
                        labelNomDiam.Text = "User Diameter:";
                        textBoxNomDiam.Text = _barrel.DimensionData.LandNominalDiam.ToString("f4");
                        labelCalStatus.Text = "Cal set to user value.";
                        break;
                    case 2://bore profile file
                        _knownDiamType = DiamCalType.BOREPROFILE;
                        textBoxNomDiam.Enabled = true;
                        buttonBrowse.Enabled = true;
                        buttonBrowse.Text = "Bore File...";
                        labelNomDiam.Text = "Bore Profile:";
                        textBoxNomDiam.Text = "Bore filename.csv";
                        labelCalStatus.Text = "Cal set to bore profile.";
                        break;

                    case 3:// calibrated with ring gage
                        _knownDiamType = DiamCalType.RINGCAL;
                        
                        textBoxNomDiam.Enabled = false;
                        buttonBrowse.Enabled = true;
                        buttonBrowse.Text = "Cal File...";
                        labelNomDiam.Text = "Known Min Diam:";
                        textBoxNomDiam.Text = "Calibration filename";
                        labelCalStatus.Text = "Select CalFile.";
                        break;
                }
                Refresh();
            }
            catch (Exception ex)
            {

                _logFile.SaveMessage(ex);
               MessageBox.Show(ex.Message + ":" + ex.StackTrace);
            }
           
        }       
        string _calFilename;
        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                var ofd = new OpenFileDialog();
                ofd.Filter = "(*.csv)|*.csv";                
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _calFilename = ofd.FileName;
                }
            }
            catch (Exception ex)
            {

                _logFile.SaveMessage(ex);
               MessageBox.Show(ex.Message + ":" + ex.StackTrace);
            }
           

        }
        private void viewLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxDataOut.Lines = _logFile.GetContents().ToArray();
                tabControlOutput.SelectTab(1);
            }
            catch (Exception ex)
            {

                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);
            }
           
        }
      
        bool _useFilenameData; 
       
        private void checkBoxUseFilename_CheckedChanged_1(object sender, EventArgs e)
        {
            _useFilenameData = checkBoxUseFilename.Checked;

            textBoxStartPosX.Enabled = !_useFilenameData;
            textBoxStartPosA.Enabled = !_useFilenameData;
            textBoxEndPosA.Enabled = !_useFilenameData;
            textBoxEndPosX.Enabled = !_useFilenameData;
        }

     
       
       
      
      
        private void buttonBuildProfile_Click(object sender, EventArgs e)
        {
            if(_inspDataSetList != null && _inspDataSetList.Count>1)
            {
                BuildProfile();
            }
        }
       
        private void buttonCorrectMidpoint_Click(object sender, EventArgs e)
        {
            try
            {
                if (_inspDataSetList != null && _inspDataSetList.Count > 0)
                {
                    CorrectToMidpoint();
                }
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);
            }
           
        }

        private void saveAxialProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }
        // The main object model group.
        private Model3DGroup MainModel3Dgroup;
        private void Clear3DView()
        {
            MainModel3Dgroup = new Model3DGroup();     
            userControl11.MainViewport.Children.Clear();
           
        }
       
        private void Load3DView(CylGridData spiralScan)
        {
            try
            {
                var mb = new Model3DBuilder();
                MainModel3Dgroup = new Model3DGroup();
                userControl11.TheCamera = new PerspectiveCamera();
                userControl11.TheCamera.FieldOfView = 90;
                userControl11.MainViewport.Camera = userControl11.TheCamera;
                userControl11.PositionCamera();

                COLORCODE colorcode = COLORCODE.RAINBOW;
                double maxDepth = (_barrel.DimensionData.GrooveMaxDiam - _barrel.DimensionData.LandMinDiam) / 2.0;
                double nominalRadius = _barrel.DimensionData.LandNominalDiam / 2.0;
                double scaleFactor = _dataOutOptions.SurfaceFileScaleFactor;
                double radialDirection = -1;
                labelStatus.Text = "Building 3D Model";
                mb.BuildModel(ref MainModel3Dgroup, spiralScan, radialDirection, nominalRadius, maxDepth, scaleFactor, colorcode);

                ModelVisual3D model_visual = new ModelVisual3D();
                model_visual.Content = MainModel3Dgroup;
                // Display the main visual to the viewport
                userControl11.MainViewport.Children.Add(model_visual);
                tabControlOutput.SelectTab(2);
            }
            catch (Exception)
            {

                throw;
            }
           
        }  
        private void MainInspectionForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            

        }
        private void tabPage4_Click(object sender, EventArgs e)
        {
            userControl11.MainViewport.Focus();
        }
        private void MainInspectionForm_Load(object sender, EventArgs e)
        {
            // Create the WPF UserControl.
           // Load3DView();
        }
       
        private void dViewImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save3DScreenView();
        }
        private void RotateDataToLine(PointF pt1PartCoords, PointF pt2PartCoords)
        {
            try
            {
                if (_inspDataSetList != null && _inspDataSetList.Count > 0)
                {
                    
                    foreach (InspDataSet dataSet in _inspDataSetList)
                    {
                        if (dataSet is CartDataSet cartData)
                        {
                            cartData.CartData.RotateDataToLine(
                                new Vector3(pt1PartCoords.X, pt1PartCoords.Y, 0),
                                new Vector3(pt2PartCoords.X, pt2PartCoords.Y, 0));                
                        }
                    }
                    DisplayData(BuildDisplayDataList());
                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        bool _rotateActive;
        private void toolStripButtonRotate_Click(object sender, EventArgs e)
        {
            _dataSelection = DataSelection.ROTATE;
            _rotateActive = !_rotateActive;           
        }
        private void MirrorDataYAxis()
        {
            try
            {
                if (_inspDataSetList != null && _inspDataSetList.Count > 0)
                {
                    foreach (InspDataSet dataSet in _inspDataSetList)
                    {
                        if (dataSet is CartDataSet cartData)
                        {
                            CartData mirData = new CartData(dataSet.Filename);
                            foreach (Vector3 pt in cartData.CartData)
                            {
                                mirData.Add(new Vector3(-1 * pt.X, pt.Y, pt.Z));
                            }
                            cartData.CartData.Clear();
                            _mirrored = !_mirrored;
                            cartData.CartData.AddRange(mirData);
                        }
                    }
                    DisplayData(BuildDisplayDataList());
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        bool _mirrored;
        private void toolStripButtonMirror_Click(object sender, EventArgs e)
        {
            try
            {
                if (_inspDataSetList != null && _inspDataSetList.Count > 0)
                {
                    _dataSelection = DataSelection.MIRROR;
                    MirrorDataYAxis();
                }
            }
            catch (Exception ex)
            {

                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);
            }
           
        }
       
        void WindowData(PointF pt1PartXY,PointF pt2PartXY)
        {
            try
            {
                var minX = Math.Min(pt1PartXY.X, pt2PartXY.X);
                var maxX = Math.Max(pt1PartXY.X, pt2PartXY.X);
                var minY = Math.Min(pt1PartXY.Y, pt2PartXY.Y);
                var maxY = Math.Max(pt1PartXY.Y, pt2PartXY.Y);
              
                foreach (InspDataSet dataSet in _inspDataSetList)
                {
                    if (dataSet is CartDataSet cartDataSet)
                    {
                        CartData winData = new CartData(dataSet.Filename);

                        foreach (Vector3 pt in cartDataSet.CartData)
                        {
                            if (pt.X >= minX && pt.X <= maxX && pt.Y >= minY && pt.Y < maxY)
                            {
                                winData.Add(new Vector3(pt.X, pt.Y, pt.Z));                              
                            }
                        }
                        winData.CenterToXMidpoint();
                        
                        cartDataSet.CartData.Clear();
                        cartDataSet.CartData.AddRange(winData);
                    }
                }
                DisplayData(BuildDisplayDataList());
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        private void toolStripButtonWinData_Click(object sender, EventArgs e)
        {       
           if(_dataSelection!= DataSelection.WINDOWDATA)
                _dataSelection = DataSelection.WINDOWDATA;
        }
        private void labelMethod_Click(object sender, EventArgs e)
        {

        }
        void MeasureDepths(ref ToolpathLib.XSecPathList xSecPathList)
        {
            try
            {
                dataOuputText.Clear();
                dataOuputText.Add("input file: "+ _inspDataSetList[0].Filename);
                dataOuputText.Add("depth location file: "+ depthLocationsFilename);
                dataOuputText.Add("pass exec order, location, depth");
                var depthLines = new List<GeometryLib.Line2>();
                
               
                foreach (InspDataSet dataset in _inspDataSetList)
                {
                    if (_inspDataSetList[0] is CartDataSet cartDataSet)
                    {
                        xSecPathList.MeasureDepthsAtJetLocations(cartDataSet.CartData);
                    }
                }
                foreach(var xpe in xSecPathList)
                {
                    var line = new GeometryLib.Line2(xpe.CrossLoc, 0,  xpe.CrossLoc, xpe.CurrentDepth);
                    depthLines.Add(line);
                    dataOuputText.Add(xpe.PassExecOrder.ToString() + ", " + xpe.CrossLoc.ToString() + ", " + xpe.CurrentDepth.ToString());
                }
                
                
                textBoxDataOut.Lines = dataOuputText.ToArray();
                DrawLines(_blackPen, depthLines);
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        string depthLocationsFilename;
        private void buttonMeasureDepths_Click(object sender, EventArgs e)
        {
            try
            {
                if (_inspDataSetList != null && _inspDataSetList.Count > 0)
                {
                    var ofd = new OpenFileDialog();
                    ofd.Filter = "(*.csv)|*.csv";
                    ofd.Title = "Open Measurement Locations CSV file";
                    int firstDataRow = 1;
                    int firstDataCol = 0;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        depthLocationsFilename = ofd.FileName;
                        var xsecToolpath = new ToolpathLib.XSecPathList(depthLocationsFilename, firstDataRow,firstDataCol);
                        MeasureDepths(ref xsecToolpath);
                        var lines = new List<string>();
                        lines.Add("input file: " + _inputFileNames[0]);
                        lines.Add("depth location file: " + depthLocationsFilename);
                        lines.AddRange(xsecToolpath.AsCSVFile());
                        var sfd = new SaveFileDialog();
                        sfd.Filter = "(*.csv)|*.csv";
                        sfd.Title = "Save Measurements as CSV file";
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            FileIO.Save(lines, sfd.FileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);
            }
            
        }
        void FitToCircle(PointF pt1PartXYCoords, PointF pt2PartXYCoords)
        {
            try
            {
                if (_inspDataSetList[0] is CartDataSet cartDataSet)
                {                        
                    var pt1 = new Vector3(pt2PartXYCoords.X, pt2PartXYCoords.Y, 0);
                    var pt2 = new Vector3(pt1PartXYCoords.X, pt1PartXYCoords.Y, 0);
                    double fitR = _barrel.DimensionData.ActualLandDiam / 2.0;
                    double unrollR = (_barrel.DimensionData.ActualLandDiam + _barrel.DimensionData.NominalGrooveDiam) / 4;
                    cartDataSet.CartData.FitToCircleKnownR(pt1, pt2, fitR);                        
                    DisplayData(BuildDisplayDataList());
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        private void toolStripButtonFitToCircle_Click(object sender, EventArgs e)
        {
            try
            {
                _dataSelection = DataSelection.FITCIRCLE;
            }
            catch (Exception ex)
            {

                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);
            }

        }
      
        private void MainInspectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            SetPropertyValues();
            Properties.Settings.Default.Save();
            _logFile.GetFileName();            
        }
        bool _overlayMinMax;
        private void checkBoxOverLayCad_CheckedChanged(object sender, EventArgs e)
        {
            _overlayMinMax = checkBoxOverLayCad.Checked;            
        }

        private void viewLogFileToolStripMenuItem_Click_1(object sender, EventArgs e)
        {                         
            
            var logViewForm = new LogViewForm();
            logViewForm.SetLines(_logFile.GetContents());
            logViewForm.Show();
            Refresh();
        }
    }
}
