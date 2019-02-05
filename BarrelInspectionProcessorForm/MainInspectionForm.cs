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
        PROCESSED
    }
    
    public partial class MainInspectionForm : Form
    {
        LogFile _logFile;
        InputFileType inputFileType;
        LengthUnitEnum _lengthUnits;
        int _probeCount;
        string barrelTypeStr;
        string scanFormatStr;
        string probeDirectionStr;
        string probeTypeStr;
        string knownDiamTypeStr;
        string manufStepStr;
        public MainInspectionForm()
        {
            try
            {
                var barrelTypeList = new List<string>() { "M2_50_Cal", "M242_25mm", "M284_155mm", "M240_762mm" ,"Flat Plate"};
                var scanFormatList = new List<string>() { "RING", "SPIRAL", "AXIAL", "LAND", "GROOVE", "CAL", "LINE" };
                var probeDirectionList = new List<string>() { "BORE I.D.", "ROD O.D." };
                var probeTypeList = new List<string>() { "SI Distance", "Line Scan" };
                var knownDiamList = new List<string>() { "Default Value", "Set Value", "Diameter Profile", "Ring Calibrated" };
                var manufStepList = new List<string>(){ "Pre-Boring ","Boring In-process","Post Boring","Post Honing","Groove Machining In-Process",
                                                    "Post Groove Machining","Post Final Honing","In Use"};
                InitializeComponent();
                _logFile = LogFile.GetLogFile();
                dataOuputText = new List<string>();

                barrelTypeStr = Properties.Settings.Default.barrelType;
                scanFormatStr = Properties.Settings.Default.scanFormat;
                probeDirectionStr = Properties.Settings.Default.probeDirection;
                probeTypeStr = Properties.Settings.Default.probeType;
                _probeCount = Properties.Settings.Default.probeCount;
                knownDiamTypeStr = Properties.Settings.Default.diamCalType;
                manufStepStr = Properties.Settings.Default.manufStep;

                ComboListBoxHelper.FillComboBox(comboBoxMethod, scanFormatList.ToArray());
                ComboListBoxHelper.FillComboBox(comboBoxBarrel, barrelTypeList.ToArray());
                ComboListBoxHelper.FillComboBox(comboBoxDiameterType, knownDiamList.ToArray());
                ComboListBoxHelper.FillComboBox(comboBoxProbeDirection, probeDirectionList.ToArray());
                ComboListBoxHelper.FillComboBox(comboBoxProbeType, probeTypeList.ToArray());
                ComboListBoxHelper.FillComboBox(comboBoxManStep, manufStepList.ToArray());

                comboBoxBarrel.SelectedIndex = ComboListBoxHelper.GetIndexOf(barrelTypeStr, comboBoxBarrel.Items);
                comboBoxMethod.SelectedIndex = ComboListBoxHelper.GetIndexOf(scanFormatStr, comboBoxMethod.Items);
                comboBoxProbeDirection.SelectedIndex = ComboListBoxHelper.GetIndexOf(probeDirectionStr, comboBoxProbeDirection.Items);
                comboBoxProbeType.SelectedIndex = ComboListBoxHelper.GetIndexOf(probeTypeStr, comboBoxProbeType.Items);
                comboBoxDiameterType.SelectedIndex = ComboListBoxHelper.GetIndexOf(knownDiamTypeStr, comboBoxDiameterType.Items);
                comboBoxManStep.SelectedIndex = ComboListBoxHelper.GetIndexOf(manufStepStr, comboBoxManStep.Items);


                labelMethod.Text = "";
                _lengthUnits = LengthUnitEnum.INCH;
                _lengthLabel = "inch";
                _angleLabel = "degs";

                radioButtonViewProcessed.Checked = true;
                checkBoxAngleCorrect.Checked = true;

                textBoxPitch.ReadOnly = true;
                textBoxProbeCount.Text = _probeCount.ToString();

                //_barrel = new Barrel(_barrelType);
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
        string _inputFileName;
        List<string> _inputFileNames;
        private void OpenRawDataFiles()
        {
            try
            {
                _useLandPts = false;

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
                        _inputFileName = _inputFileNames[0];
                        labelInputFIlename.Text = _inputFileName;
                        
                    }
                    else
                    {
                        labelInputFIlename.Text = "Multiple Files";
                    }
                    dataOuputText.Add("opening: ");
                    dataOuputText.AddRange(_inputFileNames);
                    _outputPath = System.IO.Path.GetDirectoryName(_inputFileNames[0]);                   
                    
                    labelStatus.Text = "Raw Files Read In OK";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void ResetOnOpen(InputFileType inputFileType)
        {
            _useLandPts = false;
            bool state = (inputFileType == InputFileType.RAW);

            radioButtonViewRaw.Enabled = state;
            buttonProcessFile.Enabled = state;
            //buttonExtractSections.Enabled = state;
            buttonGetAveAngle.Enabled = state;
            buttonCorrectMidpoint.Enabled = state;            
                     
            _dataSelection = DataSelection.NONE;
           // DisplayData();
            Clear3DView();
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
                    dataOuputText.AddRange(_inputFileNames);

                    OpenProcessedCartDataFiles(_inputFileNames);
                    
                    DisplayData();
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
            int firstRow = 0;
            foreach (string filename in filenames)
            {
                var inspDataSet = new CartDataSet(_barrel);
                var lines = FileIO.ReadDataTextFile(filename);
                for (int i = firstRow; i < lines.Count; i++)
                {
                    var words = FileIO.Split(lines[i]);
                    if(words.Length==2)
                    {
                        if (double.TryParse(words[0], out double x) && double.TryParse(words[1], out double y))
                        {
                            var pt = new Vector3(x, y, 0);
                            inspDataSet.CartData.Add(pt);
                        }
                    }
                    if(words.Length==3)
                    {
                        if (double.TryParse(words[0], out double x) && double.TryParse(words[1], out double y) && double.TryParse(words[1], out double z))
                        {
                            var pt = new Vector3(x, y, z);
                            inspDataSet.CartData.Add(pt);
                        }
                    }
                    

                }
                _inspScript = new CartInspScript(ScanFormat.FLATPLATE);
                _inspDataSetList.Add(inspDataSet);
            }
        }
        void OpenProcessedRingDataFiles(List<string> filenames)
        {
            int firstRow = 6;
            foreach (string filename in filenames)
            {
                var inspDataSet = new RingDataSet(_barrel);
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
                _inspScript = new CylInspScript(ScanFormat.RING);
                _inspDataSetList.Add(inspDataSet);
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

        KeyenceSiDataSet _rawSiDataSet;
        KeyenceLineScanDataSet _rawLineScanDataSet;

        string GetManufStep()
        {
            try
            {
                string s = "";
                if (comboBoxManStep.SelectedItem != null)
                {
                    s = comboBoxManStep.SelectedItem.ToString();
                }
                return s;
            }
            catch (Exception)
            {

                throw;
            }

        }
       
        Barrel BuildBarrelFromInputs()
        {
            var barrelType = Barrel.GetBarrelType(comboBoxBarrel.SelectedItem.ToString());
            var barrel = new Barrel(barrelType);
            string sn = textBoxSerialN.Text;
            barrel.ManufactureData.SerialNumber = sn;
            barrel.ManufactureData.CurrentManufStep = GetManufStep();
            barrel.ManufactureData.MiscData.AddRange(textBoxMiscManNotes.Lines);
            int currentPasses = 1;
            InputVerification.TryGetValue(textBoxCurrentPasses, "x>0", 0, int.MaxValue, out currentPasses);
            barrel.MachiningData.CurrentAWJPasses = currentPasses;
            int totalPasses = 4;
            InputVerification.TryGetValue(textBoxTotalPasses, "x>0", 0, int.MaxValue, out totalPasses);
            barrel.MachiningData.TotalAWJPasses = totalPasses;
            double nomDiam = barrel.DimensionData.LandNominalDiam;
            //read nominal barrel diameter


            InputVerification.TryGetValue(textBoxNomDiam, "NaN", out nomDiam);

            switch (comboBoxDiameterType.SelectedIndex)
            {
                case 0:// Default Value
                    _knownDiamType = DiamCalType.DEFAULT;
                    barrel.DimensionData.ActualLandDiam = barrel.DimensionData.LandNominalDiam;
                    barrel.BoreProfile = new BoreProfile(barrel.DimensionData.LandNominalDiam / 2.0, barrelType);
                    break;
                case 1://Set Value                 
                    _knownDiamType = DiamCalType.USER;
                    barrel.DimensionData.ActualLandDiam = nomDiam;
                    barrel.BoreProfile = new BoreProfile(nomDiam / 2.0, barrelType);

                    break;
                case 2://Diameter Profile
                    _knownDiamType = DiamCalType.BOREPROFILE;
                    string boreFilename = textBoxNomDiam.Text;
                    if (boreFilename != "" && System.IO.File.Exists(boreFilename))
                    {
                        barrel.BoreProfile = new BoreProfile(textBoxNomDiam.Text);

                    }
                    break;
                case 3://Ring Calibrated
                    _knownDiamType = DiamCalType.RINGCAL;
                    break;
            }
            int roundsFired = 0;
            InputVerification.TryGetValue(textBoxCurrentPasses, "x>=0", 0, int.MaxValue, out roundsFired);
            barrel.LifetimeData.RoundsFired = roundsFired;
            return barrel;
        }
       
        private void BuildScriptFromInputs(string dataFilename)
        {
            try
            {
                var barrel = BuildBarrelFromInputs();
                _probeType = Probe.GetProbeType(comboBoxProbeType.SelectedItem.ToString());
                double probeSpacing =0 ;
                double startA = 0;
                double startX = 0;
                int ptsPerRev = 1;
                var startPos= new CNCLib.MachinePosition(CNCLib.MachineGeometry.CYLINDER);
                var endPos = new CNCLib.MachinePosition(CNCLib.MachineGeometry.CYLINDER);
                double endA = 0;
                double endX = 0;
               

                //read probe count
                _probeCount = 1;
                InputVerification.TryGetValue(textBoxProbeCount, "x=1,2", 0, 3, out _probeCount);
                
                
                //read ring revolutions
                double ringRevs = 1;
                double axialInc = 0;
                if (textBoxRingRevs.Enabled)
                {
                    InputVerification.TryGetValue(textBoxRingRevs, "x>0", 0, double.MaxValue, out ringRevs);
                }
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

                double pitch = Math.Abs(startX - endX) / ringCount;
                textBoxPitch.Text = pitch.ToString("f3");


                if (_useFilenameData)
                { 
                        _inspScript = InspectionScriptBuilder.BuildScript(dataFilename, axialInc, ringRevs, pitch, ptsPerRev);
                }
                else
                {
                    InputVerification.TryGetValue(textBoxProbeSpacing, "x>0", 0, double.MaxValue, out probeSpacing);
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
                        ringRevs = (endA - startA) / 360.0;
                        pitch = (endX - startX) / ringRevs;
                        axialInc = Math.Abs((endX - startX) / (ptsPerRev * ringRevs));
                    }
                   
                    _inspScript = InspectionScriptBuilder.BuildScript(_probeType,_scanFormat, startPos, endPos, axialInc,
                        ringRevs, pitch, ptsPerRev, grooves);
                }
                
                _inspScript.ProbeSetup.UseDualProbeAve = (_probeCount==2);
                double probePhase = 0;
                if(_inspScript.ProbeSetup.UseDualProbeAve)
                {
                    InputVerification.TryGetValue(textBoxProbePhaseDeg, "360.0>x>=-360.0", -360.0, 360, out probePhase);
                    _inspScript.ProbeSetup.ProbePhaseDifferenceRad = Math.PI * probePhase / 180.0;
                }
                double ringCalDiameterInch=0;
                InputVerification.TryGetValue(textBoxRingCal, "x>0", out ringCalDiameterInch);
                _inspScript.ProbeSetup.ProbeDirection = _probeDirection;
                _inspScript.ProbeSetup.ProbeCount = _probeCount;
                _inspScript.ProbeSetup.ProbeList.AddRange(_probes);

                CalDataSet calDataSet = new CalDataSet(_barrel.DimensionData.LandNominalDiam/2);
                switch(_knownDiamType)
                {
                    case DiamCalType.RINGCAL:
                        if (_calFilename != "")
                        {
                            calDataSet = BuildCalibration(_calFilename,_inspScript.OutputUnit, _probeCount, ringCalDiameterInch, _inspScript.ProbeSetup.ProbeDirection);
                            _barrel.DimensionData.ActualLandDiam = calDataSet.NominalRadius * 2;
                        }
                        break;
                    case DiamCalType.BOREPROFILE:
                        break;
                    case DiamCalType.USER:
                    case DiamCalType.DEFAULT:
                    default:
                        _barrel.DimensionData.ActualLandDiam = _barrel.DimensionData.LandNominalDiam;
                        calDataSet = new CalDataSet(_barrel.DimensionData.LandNominalDiam / 2);
                        break;
                }
                _inspScript.CalDataSet = calDataSet;
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
        Task<InspDataSet> ProcessDataAsync(string dataFilename, CancellationToken ct, Progress<int> progress)
        {
            try
            {
                    
                    BuildScriptFromInputs(dataFilename);
                    var centerline = new PointCyl(0, 0, 0);
                if(_probeType==ProbeType.SI_DISTANCE)
                {
                    _rawSiDataSet = new KeyenceSiDataSet(_inspScript.ScanFormat, _inspScript.OutputUnit, _inspScript.ProbeSetup.ProbeCount, dataFilename);
                }
                 if(_probeType==ProbeType.LINE_SCAN)
                {
                    double minValue = _inspScript.ProbeSetup.ProbeList[0].StartMeasuringRange;
                    double maxValue = _inspScript.ProbeSetup.ProbeList[0].MeasuringRange + minValue;
                    _rawLineScanDataSet = new KeyenceLineScanDataSet(_inspScript.ScanFormat, _inspScript.OutputUnit, centerline, minValue, maxValue, dataFilename);
                }
                   

                    switch (_scanFormat)
                    {
                        case ScanFormat.AXIAL:                           
                            var axialbuilder = new AxialDataBuilder(_barrel);
                            return Task.Run(() => axialbuilder.BuildAxialAsync(ct, progress, _inspScript as CylInspScript, _rawSiDataSet, _dataOutOptions));

                        case ScanFormat.RING:                          
                            var ringBuilder = new RingDataBuilder(_barrel);                          
                             return Task.Run(() => ringBuilder.BuildRingAsync(ct, progress, _inspScript as CylInspScript, _rawSiDataSet, _dataOutOptions));

                        case ScanFormat.SPIRAL:                          
                            var spiralBuilder = new SpiralDataBuilder(_barrel);
                            return Task.Run(() => spiralBuilder.BuildSpiralAsync(ct, progress, _inspScript as CylInspScript, _rawSiDataSet, _dataOutOptions));
                       
                        case ScanFormat.FLATPLATE:
                        var lineBuilder = new CartesianDataBuilder(_barrel);
                        return Task.Run(() => lineBuilder.BuildSingleLineAsync(ct, progress, _inspScript as CartInspScript, _rawLineScanDataSet, _dataOutOptions));
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
                labelStatus.Text = status;
                foreach (string filename in _inputFileNames)
                {
                    var  inspDataSet = await ProcessDataAsync(filename, ct, new Progress<int>(p => ShowProgress(p)));
                    
                    _inspDataSetList.Add(inspDataSet);
                }
                labelStatus.Text = "Finished Processing File";
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
                DisplayData();
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
               
                if (_autoAngleCorrect  )
                {
                    CorrectForAveAngle();
                }
              
                ResetOnClick();
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message +":"+ ex.StackTrace + ": Unable to process file." );
            }
        }
        BarrelInspProfile _barrelInspProfile;
       
        private void BuildProfile()
        {
            try
            {
                var profileBuilder = new ProfileBuilder(_barrel);
                _barrelInspProfile = profileBuilder.Build(_inspDataSetList);
                //_displayData = new DisplayDataList(ScanFormat.AXIAL,ViewPlane.ZR);
                //_displayData.Add(_barrelInspProfile.MinLandProfile.AsScreenData(_displayData.ViewPlane));
                //_displayData.Add(_barrelInspProfile.AveLandProfile.AsScreenData(_displayData.ViewPlane));
                //_displayData.Add(_barrelInspProfile.AveGrooveProfile.AsScreenData(_displayData.ViewPlane));
               
                DisplayData();

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
                    
                    DisplayData();
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
            textBoxRingRevs.Enabled = true;

            //textBoxExtractX.Enabled = false;
            radioButtonViewRaw.Checked = false;
            radioButtonViewRaw.Enabled = true;
            textBoxProbeCount.Text = "1";
            //data outputoptions
            buttonBuildProfile.Enabled = true;
            buttonCorrectMidpoint.Enabled = true;
            buttonGetAveAngle.Enabled = true;
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
            textBoxRingRevs.Enabled = false;

            //textBoxExtractX.Enabled = true;

            radioButtonViewRaw.Checked = false;
            radioButtonViewRaw.Enabled = false;
            textBoxProbeCount.Text = "1";
            //data outputoptions
            buttonBuildProfile.Enabled = false;
            buttonCorrectMidpoint.Enabled = false;
            buttonGetAveAngle.Enabled = false;
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
            textBoxRingRevs.Enabled = false;
            //textBoxExtractX.Enabled = false;
            textBoxProbeCount.Text = "2";
            //data outputoptions
            buttonBuildProfile.Enabled = false;
            buttonCorrectMidpoint.Enabled = false;
            buttonGetAveAngle.Enabled = false;
            buttonSetRadius.Enabled = false;
        }
        private void SetLineSwitches()
        {
            

        }
        ScanFormat _scanFormat;
        ProbeController.ProbeDirection _probeDirection;
        private void ComboBoxProbeDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBoxProbeDirection.SelectedIndex)
                {
                    case 0://bore id
                        _probeDirection = ProbeController.ProbeDirection.ID;
                        break;
                    case 1: //rod od
                        _probeDirection = ProbeController.ProbeDirection.OD;
                        break;
                    
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        ProbeType _probeType;
        List<Probe> _probes;
        private void comboBoxProbeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _probes = new List<Probe>();
                switch (comboBoxProbeType.SelectedIndex)
                {
                    case 0://si distance
                        _probeType = ProbeType.SI_DISTANCE;
                        break;
                    case 1://line scan
                        var probe = new Probe();
                        probe.MeasurementUnit = new MeasurementUnit( LengthUnitEnum.INCH);
                        probe.StartMeasuringRange = 0;
                        probe.MeasuringRange = .62;
                        _probes.Add(probe);
                        _probeType = ProbeType.LINE_SCAN;
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
            //FLATPLATE 6 
            //FLATPLATEGRID 7

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
                        _scanFormat = ScanFormat.FLATPLATE;
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
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ": Unable to change barrel type.");
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
                MessageBox.Show(ex.Message + ": Unable to change points per rev type.");
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
                MessageBox.Show(ex.Message + ": ");
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
      
        List<PointF> _screenPts;
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
                if (range <= .001)
                    gridRange = .001;               
                if ((range > .001) && (range <= .01))
                    gridRange = .01;
                if (range > .01 & range < .1)
                {
                    gridRange = .1;
                }
                if (range >= .1 && range < 1.0)
                {
                    gridRange = 1;
                }
                if (range > 1 && range <= 10)
                {
                    gridRange = 10;
                }
                if (range > 10 && range <= 100)
                {
                    gridRange = 100;
                }
                if (range > 100 && range <= 1000)
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
        void DrawVGrid(int xGridCount,double dxGrid,double dyGrid, Font font, RectangleF rect)
        {
            try
            {
                for (int i = 0; i <= xGridCount; i++)
                {
                    double xLabel = rect.X + (i * dxGrid);
                    var ptX = _screenTransform.GetScreenCoords(xLabel, rect.Top);
                    var ptYGridStart = _screenTransform.GetScreenCoords(xLabel, rect.Top);
                    var ptYGridEnd = _screenTransform.GetScreenCoords(xLabel, rect.Top + xGridCount * dyGrid);
                    string xLabelstr = "";
                    var sizeXlabel = _graphics.MeasureString(xLabelstr, font);
                    float xLabelxOffset = (float)(sizeXlabel.Width / 2.0);
                    float xLabelyOffset = (float)(sizeXlabel.Height * 0.1);
                    switch (_scanFormat)
                    {
                        case ScanFormat.AXIAL:
                        case ScanFormat.FLATPLATE:
                            xLabelstr = xLabel.ToString("f3");
                            break;
                        case ScanFormat.RING:
                        case ScanFormat.SPIRAL:
                            xLabelstr = GeometryLib.Geometry.ToDegs(xLabel).ToString("f0");
                            break;
                    }
                    _graphics.DrawString(xLabelstr, font, System.Drawing.Brushes.Black, ptX.X - xLabelxOffset, ptX.Y + xLabelyOffset);
                    _graphics.DrawLine(_greenPen, ptYGridStart, ptYGridEnd);
                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        void DrawHGrid(int yGridCount, double dxGrid, double dyGrid, Font font, RectangleF rect)
        {
            try
            {
                for (int i = 0; i <= yGridCount; i++)
                {

                    double yLabel = rect.Top + (i * dyGrid);


                    var ptY = _screenTransform.GetScreenCoords(rect.Top, yLabel);
                    var ptXGridStart = _screenTransform.GetScreenCoords(rect.Left, yLabel);
                    var ptXGridEnd = _screenTransform.GetScreenCoords(rect.Left + yGridCount * dxGrid, yLabel);


                    string yLabelstr = yLabel.ToString("f4");

                    var sizeYlabel = _graphics.MeasureString(yLabelstr, font);



                    float yLabelxOffset = (float)(sizeYlabel.Width * 1.1);
                    float yLabelyOffset = (float)(sizeYlabel.Height / 2.0);


                    _graphics.DrawString(yLabelstr, font, System.Drawing.Brushes.Black, ptY.X - yLabelxOffset, ptY.Y - yLabelyOffset);
                    _graphics.DrawLine(_greenPen, ptXGridStart, ptXGridEnd);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        void DrawGrid(RectangleF rect  )
        {
            try
            {

                var sizeX = rect.Width;
                var sizeY = GetGridRange(rect.Height);
                int xGridCount;
                int yGridCount = 10;
                if (_scanFormat == ScanFormat.AXIAL ||  _scanFormat== ScanFormat.FLATPLATE)
                {
                    xGridCount = 10;
                }
                else
                {
                    xGridCount = _barrel.DimensionData.GrooveCount;
                }
                var rounding =(int)Math.Round( Math.Abs(Math.Log10(sizeY)));
                double dxGrid = sizeX / xGridCount;
                double dyGrid = Math.Round(sizeY / yGridCount,rounding);
                var font = new Font(this.Font, FontStyle.Regular);
                //VERTICAL LINE grids
                DrawVGrid(xGridCount, dxGrid, dyGrid, font, rect);
                //horizontal line grids
                DrawHGrid(xGridCount, dxGrid, dyGrid, font, rect);
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        void DrawCircles(System.Drawing.Pen pen, List<PointF> screenPoints, int screenRadius)
        {
            try
            {
                foreach (var pt in screenPoints)
                {
                    var rect = new RectangleF(pt.X - screenRadius, pt.Y - screenRadius,
                        screenRadius * 2, screenRadius * 2);
                    _graphics.DrawEllipse(pen, rect);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
      
        void DrawLines(System.Drawing.Pen pen, List<GeometryLib.Line2> lines)
        {
            try
            {
                foreach(GeometryLib.Line2 line in lines)
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
        RectangleF _dataRect;
        
        private void DisplayData()
        { 
            try
            {
                SetupDisplay();
                _mouseDownLocation = new Point();
                _mouseDownPart = new PointF();
                _mouseDownPartXY = new PointF();
                _prevMouseDownLocation = new Point();
                _prevMouseDownPartXY = new PointF();

                var screenPtsList = new List<List<PointF>>();
                var displayData = new DisplayData();
                _dataRect = (RectangleF) pictureBox1.DisplayRectangle;
                _screenTransform = new ScreenTransform(_dataRect, pictureBox1.DisplayRectangle, .9, true);
                foreach (InspDataSet dataSet in _inspDataSetList)
                {
                    if (dataSet is CartDataSet cartData)
                    {
                        _viewPlane = ViewPlane.XY;
                        displayData = cartData.ModCartData.AsScreenData(_viewPlane);
                    }
                    if(dataSet is AxialDataSet axialData)
                    {
                        _viewPlane = ViewPlane.ZR;
                        displayData = axialData.CorrectedCylData.AsScreenData(_viewPlane);
                    }
                    if (dataSet is RingDataSet ringData)
                    {
                        //textBoxDataOut.Lines = _lines.ToArray();
                        _viewPlane = ViewPlane.THETAR;
                        displayData = ringData.CorrectedCylData.AsScreenData(_viewPlane);
                       
                    }
                    _dataRect = displayData.BoundingRect();
                    _screenTransform = new ScreenTransform(_dataRect, pictureBox1.DisplayRectangle, .9, true);
                    // HilightLandPoints(currentTransform);
                    _screenPts = new List<PointF>();
                    foreach (var pt in displayData)
                    {
                        _screenPts.Add(_screenTransform.GetScreenCoords(pt.X, pt.Y));
                    }
                    screenPtsList.Add(_screenPts);
                }
                DrawGrid(_dataRect);

                foreach (List<PointF> pointList in screenPtsList)
                {
                    _graphics.DrawLines(_bluePen, pointList.ToArray());
                }
                pictureBox1.Image = _bitmap;

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
                var rect = new System.Drawing.Rectangle(start,new Size(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y)));
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
                    double xLen = GeometryLib.Geometry.ToDegs(Math.Abs(endPartCoords.X - startPartCoords.X));
                    double yLen = endPartCoords.Y - startPartCoords.Y;
                    measurement = xLen.ToString("f4") + "," + yLen.ToString("f5");
                    labelDxMeasured.Text = "DTheta: " + xLen.ToString("f4") + " " + _angleLabel;
                    labelDyMeasured.Text = "Dr: " + yLen.ToString("f5") + " " + _lengthLabel;
                }
                if(dataSelection == DataSelection.ROTATE)
                {
                    _dataRotationRad = Math.Atan2(endPartCoords.Y - startPartCoords.Y, endPartCoords.X - startPartCoords.X);
                    double angle = GeometryLib.Geometry.ToDegs(_dataRotationRad);
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
       

        
        
        bool _useLandPts;
        int _landCount;

        List<PointCyl> _selectedSidewallPoints;
       
        int _grooveIntersectionCount;
        PointCyl _knownRadiusPt;
        bool _mouseDown;
        Point _mouseDownLocation;
        Point _prevMouseDownLocation;
        PointF _mouseDownPart;
        PointF _mouseUpPart;
        PointF _mouseLocPart;
        Point _mouseUpLocation;
        Point _mouseLocation;
        PointF _mouseDownPartXY;
        PointF _prevMouseDownPartXY;
        PointF _mousePartXY;
        TextureBrush _imgBrush;
        int _mouseDownCount;


        private PointF GetNearestModelPoint(System.Windows.Forms.MouseEventArgs e)
        {
            try
            {

                var nearestScreenPt = PictureBoxUtilities.GetNearestPoint(e.Location, _screenPts);
                // _displayData.HiLightPts.Add(nearestScreenPt);
                var pt = _screenTransform.GetModelCoords(nearestScreenPt);
                return pt;
                 
            }
            catch (Exception)
            {

                throw;
            }
           
            
           
        }
       
        private void SetMidpointClick(System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                //_displayData.HiLightPts.Clear();
                var pt = GetNearestModelPoint(e);
                _midpoint = new PointCyl(pt.Y, pt.X, 0);
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        

        private void SetRadiusMouseClick(System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                var pt = GetNearestModelPoint(e);
                _knownRadiusPt = new PointCyl(pt.Y, pt.X, 0);
                textBoxCurrentRadius.Text = _knownRadiusPt.R.ToString("f5");
            }
            catch (Exception)
            {

                throw;
            }
              
        }
              
        private void PictureBox1_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (_inspDataSetList != null && _inspDataSetList.Count > 0)
                {
                    
                    switch (_dataSelection)
                    {
                        case DataSelection.MEASURELENGTH:
                            break;
                        case DataSelection.SETRADIUS:
                            SetRadiusMouseClick(e);
                            break;
                        case DataSelection.SELECTMIDPOINT:
                            SetMidpointClick(e);
                            break;
                        case DataSelection.NONE:
                        default:
                            break;
                    }
                    DisplayData();
                }
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);
            }
        }
        private void PictureBox1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (_inspDataSetList != null && _inspDataSetList.Count > 0)
                {

                    _mouseLocation = e.Location;
                    _mouseLocPart = _screenTransform.GetModelCoords(e.Location);
                    _mousePartXY = GetXYCoords(_mouseLocPart);
                    switch (_scanFormat )
                    {
                        case ScanFormat.AXIAL:
                            labelZPosition.Text = "Axial: " + _mouseLocPart.X.ToString("F6") + _lengthLabel;
                            labelYPosition.Text = "Radius: " + _mouseLocPart.Y.ToString("f6") + _lengthLabel;
                            //  labelXPosition.Text = "Angle: " + (180 * (_inspScript.StartThetaRad) / Math.PI).ToString("f6") + _angleLabel;
                            break;

                        case ScanFormat.RING:
                        case ScanFormat.SPIRAL:
                            double angle = GeometryLib.Geometry.ToDegs(_mouseLocPart.X);
                            labelXPosition.Text = "Angle: " + angle.ToString("f6") + " " + _angleLabel;
                            labelYPosition.Text = "Radius: " + _mouseLocPart.Y.ToString("f6") + " " + _lengthLabel;
                            break;
                        case ScanFormat.FLATPLATE:
                        default:
                            labelZPosition.Text = "X: " + _mouseLocPart.X.ToString("F6") + _lengthLabel;
                            labelYPosition.Text = "Y: " + _mouseLocPart.Y.ToString("f6") + _lengthLabel;
                            break;
                           
                    }

                    if (_mouseDownCount == 1)
                    {
                        if (_dataSelection == DataSelection.MEASURELENGTH || _dataSelection == DataSelection.ROTATE)
                        {                        
                            DrawMeasurement(_mouseDownLocation, _mouseLocation,_dataSelection);
                        }
                        if(_dataSelection == DataSelection.WINDOWDATA)
                        {
                            DrawWindow(_mouseDownLocation, _mouseLocation, _dataSelection);
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
                if (_inspDataSetList != null && _inspDataSetList.Count > 0)
                {
                    
                    _mouseDown = false;
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
        private void PictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (_inspDataSetList != null && _inspDataSetList.Count > 0)
                {  
                     
                    _mouseDownCount++;
                    _mouseDown = true;
                    var prevMouseDownLoc = _mouseDownLocation;
                    _mouseDownLocation = e.Location;

                    _mouseDownPart = _screenTransform.GetModelCoords(_mouseDownLocation);
                    _prevMouseDownPartXY = _mouseDownPartXY;
                    _mouseDownPartXY = GetXYCoords(_mouseDownPart);


                    var img = new Bitmap(pictureBox1.Image);
                    _imgBrush = new TextureBrush(img);
                    if (_dataSelection == DataSelection.MEASURELENGTH || _dataSelection == DataSelection.ROTATE ||
                        _dataSelection==DataSelection.WINDOWDATA)
                    {
                        if (_mouseDownCount == 2)
                        {
                            DrawMeasurement(prevMouseDownLoc, _mouseDownLocation,_dataSelection );
                            if (_dataSelection == DataSelection.ROTATE)
                            {
                                RotateDataToLine(_prevMouseDownPartXY,_mouseDownPartXY);
                                _dataSelection = DataSelection.NONE;
                            }
                            if(_dataSelection == DataSelection.WINDOWDATA)
                            {
                                WindowData(_prevMouseDownPartXY, _mouseDownPartXY);
                                _dataSelection = DataSelection.NONE;
                            }
                            if(_dataSelection == DataSelection.FITCIRCLE)
                            {
                                FitToCircle(_prevMouseDownPartXY, _mouseDownPartXY);
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
                        _screenTransform = new ScreenTransform(_dataRect, pictureBox1.DisplayRectangle, .9, true);
                        DisplayData();
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
                Bitmap memoryImage;
                memoryImage = new Bitmap(elementHost1.Width, elementHost1.Height);
                Size s = new Size(memoryImage.Width, memoryImage.Height);

                // Create graphics 

                Graphics memoryGraphics = Graphics.FromImage(memoryImage);

                // Copy data from screen 
                var origin = elementHost1.PointToScreen(elementHost1.Location);
                memoryGraphics.CopyFromScreen(origin.X, origin.Y, 0, 0, s);

                //That's it! Save the image in the directory and this will work like charm. 

                var sfd = new SaveFileDialog();

                string filename = string.Format(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + @"\Screenshot.png");
                sfd.FileName = filename;
                sfd.Filter = "(*.png)|*.png";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    memoryImage.Save(sfd.FileName);
                }
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
                    DisplayData();
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

                            dataOuputText.Add("Correction angle: " + GeometryLib.Geometry.ToDegs(da.CorrectionAngle).ToString("f5") + " degs");
                            dataOuputText.Add("Ave radius: " + da.AveRadius.ToString("f5"));
                           // _displayData.Add(ringData.CorrectedCylData.AsScreenData(ViewPlane.THETAR));                            
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

        private void correctAngleToolStripMenuItem_Click(object sender, EventArgs e)
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
        
        List<PointCyl> _depthMeasurePoints;
        BarrelXsectionProfile _grooveMeasurements;
       

       
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
        private void ToolStripButtonFileSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveAllOutputFiles();
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
            toolStripButtonGrooveMidpoint.BackColor = DefaultBackColor;
            
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
        private void toolStripButtonGrooveMidpoint_Click(object sender, EventArgs e)
        {
            ResetOnClick();
            _selectedSidewallPoints = new List<PointCyl>();
           
            _midpoint = new PointCyl();
            labelStatus.Text = "Manually select groove midpoint.";
            _dataSelection = DataSelection.SELECTMIDPOINT;
            toolStripButtonGrooveMidpoint.BackColor = System.Drawing.Color.Red;
        }
        #endregion
        #region MainMenu
        void SaveAllOutputFiles()
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
                            FileName = DataFileBuilder.BuildFileName(_inputFileNames[0], "_out", ".csv")
                            
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
                            fileNames.Add(DataFileBuilder.BuildFileName(_inputFileNames[i], "_out", ".csv"));
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
                                DataFileBuilder.SaveCSVFile(_barrel, _dataOutOptions, spiralData.CorrectedSpiralData,
                                    fileNames[i], new Progress<int>(p => ShowProgress(p)));
                                progressBarProcessing.Value = 0;
                                DataFileBuilder.SavePlyFile(_barrel, _dataOutOptions, spiralData.CorrectedSpiralData,
                                    _barrel.DimensionData.LandNominalDiam / 2.0, fileNames[i], new Progress<int>(p => ShowProgress(p)));
                                progressBarProcessing.Value = 0;
                            }
                            if (_inspDataSetList[i] is RingDataSet ringData)
                            {

                                DataFileBuilder.SaveCSVFile(_barrel, _dataOutOptions, ringData.CorrectedCylData,
                                    fileNames[i], new Progress<int>(p => ShowProgress(p)));
                                progressBarProcessing.Value = 0;
                                DataFileBuilder.SaveDXF(ringData.CorrectedCylData, fileNames[i], new Progress<int>(p => ShowProgress(p)));
                                progressBarProcessing.Value = 0;
                                if (_grooveMeasurements != null)
                                {
                                    _grooveMeasurements.SaveMeasurements(fileNames[i], fileNames[i], false, true);
                                }
                            }
                            if(_inspDataSetList[i] is CartDataSet cartData)
                            {
                                DataFileBuilder.SaveCSVFile(_barrel, _dataOutOptions, cartData.ModCartData,
                                   fileNames[i], new Progress<int>(p => ShowProgress(p)));
                                progressBarProcessing.Value = 0;
                                DataFileBuilder.SaveDXF(cartData.ModCartData, fileNames[i], new Progress<int>(p => ShowProgress(p)));
                                progressBarProcessing.Value = 0;
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
        string BuildAxialfileName(string filename)
        {
            var zStart = _barrelInspProfile.AveGrooveProfile[0].Z.ToString();
            var zEnd = _barrelInspProfile.AveGrooveProfile[_barrelInspProfile.AveGrooveProfile.Count - 1].Z.ToString();
            return DataFileBuilder.BuildFileName(_inputFileNames[0], "_profile x" + zStart + "-x" + zEnd, ".csv");
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
        async Task SaveProfile()
        {
            try
            {
                if (_inspDataSetList != null && _inspDataSetList[0] is RingDataSet ringData)
                {
                    var sfd = new SaveFileDialog
                    {
                        FileName = DataFileBuilder.BuildFileName(_inputFileName, "_out", ".csv"),
                        Filter = "(*.csv)|*.csv",
                        DefaultExt = ".csv",
                        Title = "Depth Profile Data File",
                        AddExtension = true
                    };
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        labelStatus.Text = "Saving CSV File";
                        await SaveCSVFile(sfd.FileName, ringData.CorrectedCylData, new Progress<int>(p => ShowProgress(p)));
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
     
      
        
        private  async void SaveProfileDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
               await SaveProfile();
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);

            }
        }
        void SaveDepths()
        {
            try
            {
                if (_grooveMeasurements != null)
                {
                    var sfd = new SaveFileDialog
                    {
                        Filter = "(*.csv)|*.csv",
                        FileName = DataFileBuilder.BuildFileName(_inputFileName, "_depths", ".csv"),
                        DefaultExt = ".csv",
                        Title = "Depth Measurement File",
                        AddExtension = true
                    };

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        labelStatus.Text = "Saving Groove CSV File";
                        _grooveMeasurements.SaveMeasurements(sfd.FileName, _inputFileName, true, true);
                        labelStatus.Text = "Finished Saving File";
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        private void SaveDepthDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveDepths();
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);

            }


        }
        List<InspDataSet> _inspDataSetList;
        //InspDataSet _inspDataSet;
        void SaveDxf()
        {
            try
            {
                if (_inspDataSetList != null && _inspDataSetList[0] is RingDataSet ringData)
                {
                    var sfd = new SaveFileDialog
                    {
                        Filter = "(*.dxf)|*.dxf",
                        FileName = DataFileBuilder.BuildFileName(_inputFileName, "_out", ".dxf"),
                        DefaultExt = ".dxf",
                        Title = "2D Profile File",
                        AddExtension = true
                    };

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        labelStatus.Text = "Saving DXF File";
                        DataFileBuilder.SaveDXF(ringData.CorrectedCylData, sfd.FileName, new Progress<int>(p => ShowProgress(p)));
                        labelStatus.Text = "Finished Saving File";
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        private void SaveDXFProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveDxf();
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
                        FileName = DataFileBuilder.BuildFileName(_inputFileName, "_3D", ".ply"),
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
                        FileName = DataFileBuilder.BuildFileName(_inputFileName, "_3D", ".stl"),
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
        private void SaveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveAllOutputFiles();
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
                            //_displayData.Add(resetData.AsScreenData(_viewPlane));
                            DisplayData();
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
        bool _autoAngleCorrect;
        private void CheckBoxAngleCorrect_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                _autoAngleCorrect = checkBoxAngleCorrect.Checked;
                buttonGetAveAngle.Enabled = !(_autoAngleCorrect);
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
                MessageBox.Show(ex.Message + ":" );
            }
           
        }
        CalDataSet BuildCalibration(string filename,MeasurementUnit outputUnit,int probeCount, double ringCalInch,ProbeController.ProbeDirection probeDirection)
        {
           
           
            var cal = new CalDataBuilder(_barrel);
            var calDataSet = cal.BuildCalData(_inspScript.ScanFormat,outputUnit, probeCount, ringCalInch, filename,probeDirection);
           
            textBoxNomDiam.Text = this.GetShortenedPathString(filename);
            labelCalStatus.Text = "Cal Offset(in):" + calDataSet.ProbeSpacingInch.ToString("f4");
            
            return calDataSet;
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
                MessageBox.Show(ex.Message + ":" );
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

                COLORCODE colorcode = _dataOutOptions.SurfaceColorCode;
                double maxDepth = (_barrel.DimensionData.GrooveMaxDiam - _barrel.DimensionData.LandMinDiam) / 2.0;
                double nominalRadius = _barrel.DimensionData.LandNominalDiam / 2.0;
                double scaleFactor = _dataOutOptions.SurfaceFileScaleFactor;
                double radialDirection = -1;

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
                            cartData.ModCartData.RotateDataToLine(
                                new Vector3(pt1PartCoords.X, pt1PartCoords.Y, 0),
                                new Vector3(pt2PartCoords.X, pt2PartCoords.Y, 0));                
                        }
                    }
                    DisplayData();
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
                            CartData mirData = new CartData();
                            foreach (Vector3 pt in cartData.ModCartData)
                            {
                                mirData.Add(new Vector3(-1 * pt.X, pt.Y, pt.Z));
                            }
                            cartData.ModCartData.Clear();
                            _mirrored = !_mirrored;
                            cartData.ModCartData.AddRange(mirData);
                        }
                    }
                    DisplayData();
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
                        CartData winData = new CartData();

                        foreach (Vector3 pt in cartDataSet.ModCartData)
                        {
                            if (pt.X >= minX && pt.X <= maxX && pt.Y >= minY && pt.Y < maxY)
                            {
                                winData.Add(new Vector3(pt.X, pt.Y, pt.Z));                              
                            }
                        }
                        winData.CenterToXMidpoint();
                        
                        cartDataSet.ModCartData.Clear();
                        cartDataSet.ModCartData.AddRange(winData);
                    }
                }
                DisplayData();
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
                dataOuputText.Add("input file: "+ _inputFileName);
                dataOuputText.Add("depth location file: "+ depthLocationsFilename);
                dataOuputText.Add("pass exec order, location, depth");
                var depthLines = new List<GeometryLib.Line2>();
                foreach (ToolpathLib.XSectionPathEntity xpe in xSecPathList)
                {
                    double xMeasure = xpe.CrossLoc;
                    foreach (InspDataSet dataset in _inspDataSetList)
                    {
                        if (dataset is CartDataSet cartData)
                        {
                            for (int i = 1; i < cartData.ModCartData.Count; i++)
                            {
                                if ((xMeasure >= cartData.ModCartData[i - 1].X && xMeasure <= cartData.ModCartData[i].X)||
                                    (xMeasure <= cartData.ModCartData[i - 1].X && xMeasure >= cartData.ModCartData[i].X))
                                {                                    
                                    xpe.CurrentDepth =( cartData.ModCartData[i - 1].Y + cartData.ModCartData[i ].Y)/2.0;
                                    break;
                                }
                            }
                        }
                    }
                    var line = new GeometryLib.Line2(xMeasure, 0,  xMeasure, xpe.CurrentDepth);
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
                    int headerrowCount = 1;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        depthLocationsFilename = ofd.FileName;
                        var xsecToolpath = new ToolpathLib.XSecPathList(depthLocationsFilename, headerrowCount);
                        MeasureDepths(ref xsecToolpath);
                        var lines = new List<string>();
                        lines.Add("input file: " + _inputFileName);
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
                var pt1 = new Vector3(pt2PartXYCoords.X, pt2PartXYCoords.Y,0);
                var pt2 = new Vector3(pt1PartXYCoords.X, pt1PartXYCoords.Y,0);
                var r = _barrel.DimensionData.ActualLandDiam;
                var centers = DataUtilities.FindCirclesofKnownR(pt1, pt2, r);
                var center = new Vector3();
                if(centers[0].Y>centers[1].Y)
                {
                    center = centers[0];
                }
                else
                {
                    center = centers[1];
                }
                if(_inspDataSetList[0].DataFormat!= ScanFormat.FLATPLATE)
                {
                    if(_inspDataSetList[0] is CartDataSet cartDataSet)
                    {
                        cartDataSet.ModCartData.RotateDataToLine(pt1, pt2);
                        cartDataSet.ModCartData.Translate(new Vector3(-1.0 * center.X, -1.0 * center.Y, 0));

                    }
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
            catch (Exception)
            {

                throw;
            }

        }
        private void MainInspectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.barrelType = comboBoxBarrel.SelectedItem.ToString();
            Properties.Settings.Default.probeType = comboBoxProbeType.SelectedItem.ToString();
            Properties.Settings.Default.probeDirection = comboBoxProbeDirection.SelectedItem.ToString();
            Properties.Settings.Default.scanFormat = comboBoxMethod.SelectedItem.ToString();
            Properties.Settings.Default.probeCount = _probeCount;
            Properties.Settings.Default.diamCalType = comboBoxDiameterType.SelectedItem.ToString();
            Properties.Settings.Default.manufStep = comboBoxManStep.SelectedItem.ToString();
            Properties.Settings.Default.Save();
        }

       
    }
}
