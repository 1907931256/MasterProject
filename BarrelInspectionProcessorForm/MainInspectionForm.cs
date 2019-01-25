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
        public MainInspectionForm()
        {
            InitializeComponent();
            _logFile = LogFile.GetLogFile();
            _lines = new List<string>();
            comboBoxBarrel.SelectedIndex = 0;
            comboBoxMethod.SelectedIndex = 0;
            comboBoxProbeDirection.SelectedIndex = 0;
            comboBoxManStep.SelectedIndex = 4;
            comboBoxDiameterType.SelectedIndex = 3;
            labelMethod.Text = "";
            _units = LengthUnits.INCH;
            _lengthLabel = "inch";
            _angleLabel = "degs";
            radioButtonViewRolled.Checked = false;
            radioButtonViewProcessed.Checked = true;
            checkBoxAngleCorrect.Checked = true;           
            checkBoxLandPoints.Checked = false;            
            checkBoxAutoSaveOutput.Checked = true;
            checkBoxDualProbeAve.Checked = true;
            textBoxPitch.ReadOnly = true;
            textBoxProbeCount.Text = "2";
            _barrel = new Barrel();
            _probeDirection = ProbeController.ProbeDirection.ID;          
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
                    _lines.Add("opening: ");
                    _lines.AddRange(_inputFileNames);
                    _outputPath = System.IO.Path.GetDirectoryName(_inputFileNames[0]);                   
                    
                    labelStatus.Text = "Raw Files Read In OK";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        class DisplayDataSet:List<CylData>
        {
            public ScanFormat DataFormat { get; set; }
            public List<PointCyl> HiLightPoints { get; set; }
            public DisplayDataSet()
            {
                HiLightPoints = new List<PointCyl>();
                DataFormat = ScanFormat.UNKNOWN;
            }
        }

        DisplayDataSet _displayData;
        private void ResetOnOpen(InputFileType inputFileType)
        {
            _useLandPts = false;
            bool state = (inputFileType == InputFileType.RAW);

            radioButtonViewRaw.Enabled = state;
            buttonProcessFile.Enabled = state;
            //buttonExtractSections.Enabled = state;
            buttonGetAveAngle.Enabled = state;
            buttonCorrectMidpoint.Enabled = state;

            _selectedLandPoints = new List<PointCyl>();
            _selectedSidewallPoints = new List<PointCyl>();           
            _hilightPts = new List<PointF>();
            _displayData = new DisplayDataSet();
            _displayData.DataFormat = ScanFormat.UNKNOWN;
            _dataSelection = DataSelection.NONE;
            DisplayData();
            Clear3DView();
        }
        private void OpenProcessedFile(int firstRow)
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
                    _lines.Add("opening: ");
                    _lines.AddRange(_inputFileNames);

                    foreach (string filename in _inputFileNames)
                    {
                        var inspDataSet = new InspDataSet();
                        var lines = FileIO.ReadDataTextFile(filename);
                        for (int i = firstRow; i < lines.Count; i++)
                        {
                            var words = FileIO.Split(lines[i]);
                            bool dataOK = double.TryParse(words[1], out double th);
                            dataOK &= double.TryParse(words[2], out double r);
                            dataOK &= double.TryParse(words[3], out double z);
                            if (dataOK)
                            {
                                var pt = new PointCyl(r, th, z);
                                inspDataSet.CorrectedCylData.Add(pt);
                            }

                        }
                        inspDataSet.DataFormat = ScanFormat.RING;
                        _inspScript = new CylInspScript(ScanFormat.RING, new CNCLib.MachinePosition(CNCLib.MachineGeometry.CYLINDER),
                            new CNCLib.MachinePosition(CNCLib.MachineGeometry.CYLINDER));
                        _displayData.Add(inspDataSet.CorrectedCylData);
                        _inspDataSetList.Add(inspDataSet);
                    }
                    
                    DisplayData();
                    labelStatus.Text = "Processed File Read In OK";

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

        KeyenceSiDataSet _rawSiDataSet;
        KeyenceLineScanDataSet _rawLineScanDataSet;

        private void BuildMinDProfile()
        {
            try
            {

                _barrelInspProfile = new BarrelInspProfile();
                double minFeatureSize = .002;
                foreach(string filename in _inputFileNames)
                {
                    BuildScriptFromInputs(filename);
                    _rawSiDataSet = new KeyenceSiDataSet(_inspScript.ScanFormat,_inspScript.OutputUnit, _inspScript.ProbeSetup.ProbeCount, filename);
                    var pb = new ProfileBuilder(_barrel);
                    var pt = pb.BuildMinDProfile(_inspScript, filename,minFeatureSize);
                    _barrelInspProfile.MinLandProfile.Add(pt);
                    _barrelInspProfile.AveGrooveProfile.Add(pt);
                    _barrelInspProfile.AveLandProfile.Add(pt);
                }
                var sfd = new SaveFileDialog();
                if(sfd.ShowDialog()== DialogResult.OK)
                {
                    SaveAxialProfileFile(sfd.FileName);
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }
        private  void buttonMinDProfile_Click(object sender, EventArgs e)
        {
            try
            {
                _displayData = new DisplayDataSet();
                _inspDataSetList = new List<InspDataSet>();
                _hilightPts = new List<PointF>();
                 BuildMinDProfile();
            }
            catch (Exception ex)
            {

                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ":" + ex.StackTrace + ": Unable to process file.");
            }
        }

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
                ProbeType probeType = Probe.GetProbeType(comboBoxProbeType.SelectedItem.ToString());
                double probeSpacing =0 ;
                double startA = 0;
                double startX = 0;
                int ptsPerRev = 1;
                var startPos= new CNCLib.MachinePosition(CNCLib.MachineGeometry.CYLINDER);
                var endPos = new CNCLib.MachinePosition(CNCLib.MachineGeometry.CYLINDER);
                double endA = 0;
                double endX = 0;
               

                //read probe count
                int probeCount = 2;
                InputVerification.TryGetValue(textBoxProbeCount, "x=1,2", 0, 3, out probeCount);
                
                
                //read ring revolutions
                double ringRevs = 1;
                double axialInc = 0;
                if (textBoxRingRevs.Enabled)
                {
                    InputVerification.TryGetValue(textBoxRingRevs, "x>0", 0, double.MaxValue, out ringRevs);
                }
                if (_method == ScanFormat.AXIAL)
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
                //var extractX = new double[10];
                //read x extraction values for spiral each x is save as a ring
                //if (checkBoxExtractX.Checked)
                //{
                //    if (textBoxExtractX.Enabled)
                //    {
                //        if (textBoxExtractX.Text != "")
                //        {
                //            InputVerification.TryGetValues(textBoxExtractX, "start<x<end", double.MinValue, double.MaxValue, out extractX);
                //        }
                //    }
                //}
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
                   
                    _inspScript = InspectionScriptBuilder.BuildScript(probeType,_method, startPos, endPos, axialInc,
                        ringRevs, pitch, ptsPerRev, grooves);
                }
                
                _inspScript.ProbeSetup.UseDualProbeAve = _useDualProbeAve;
                double probePhase = 0;
                if(_useDualProbeAve)
                {
                    InputVerification.TryGetValue(textBoxProbePhaseDeg, "360.0>x>=-360.0", -360.0, 360, out probePhase);
                    _inspScript.ProbeSetup.ProbePhaseDifferenceRad = Math.PI * probePhase / 180.0;
                }
                double ringCalDiameterInch=0;
                InputVerification.TryGetValue(textBoxRingCal, "x>0", out ringCalDiameterInch);
                _inspScript.ProbeSetup.ProbeDirection = _probeDirection;
                _inspScript.ProbeSetup.ProbeCount = probeCount;
                CalDataSet calDataSet = new CalDataSet(_barrel.DimensionData.LandNominalDiam/2);
                switch(_knownDiamType)
                {
                    case DiamCalType.RINGCAL:
                        if (_calFilename != "")
                        {
                            calDataSet = BuildCalibration(_calFilename,_inspScript.OutputUnit, probeCount, ringCalDiameterInch, _inspScript.ProbeSetup.ProbeDirection);
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



        CylInspScript _inspScript;
        Task<InspDataSet> ProcessDataAsync(string dataFilename, CancellationToken ct, Progress<int> progress)
        {
            try
            {
                    
                    BuildScriptFromInputs(dataFilename);
                
                    _rawSiDataSet =  new KeyenceSiDataSet(_inspScript.ScanFormat,_inspScript.OutputUnit, _inspScript.ProbeSetup.ProbeCount, dataFilename);
                    

                    switch (_method)
                    {
                        case ScanFormat.AXIAL:
                            //_inspScript = new CylInspScript(_method, _startPos, _endPos, _axialInc);
                            var axialbuilder = new AxialDataBuilder(_barrel);
                            return Task.Run(() => axialbuilder.BuildAxialAsync(ct, progress, _inspScript, _rawSiDataSet, _dataOutOptions));

                        case ScanFormat.RING:
                            //_inspScript = new CylInspScript(_method, _startPos,_endPos, _ringRevs, _ptsPerRev);
                            var ringBuilder = new RingDataBuilder(_barrel);
                            if (_useLandPts)
                            {
                                return Task.Run(() => ringBuilder.BuildRingAsync(ct, progress, _inspScript, _rawSiDataSet, _selectedLandPoints.ToArray(), _dataOutOptions));
                            }
                            else
                            {
                                return Task.Run(() => ringBuilder.BuildRingAsync(ct, progress, _inspScript, _rawSiDataSet, _dataOutOptions));
                            }


                        case ScanFormat.SPIRAL:
                           // _inspScript = new CylInspScript(_method, _startPos, _endPos, _pitch, _ptsPerRev)
                            //{
                           //     ExtractLocations = _extractX
                           // };
                            var spiralBuilder = new SpiralDataBuilder(_barrel);
                            return Task.Run(() => spiralBuilder.BuildSpiralAsync(ct, progress, _inspScript, _rawSiDataSet, _dataOutOptions));

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
        private void HilightLandPoints(ScreenTransform transform)
        {
            try
            {
                _landHilightPoints = new List<PointF>();
                if(_inspDataSetList != null && _inspDataSetList.Count>0)
                {
                    
                    foreach (InspDataSet dataset in _inspDataSetList)
                    {
                        if (dataset.CorrectedLandPoints != null && dataset.RawLandPoints != null)
                        {
                            if (_viewProcessed)
                            {

                                if (_viewRolled)
                                {
                                    foreach (PointCyl pt in dataset.CorrectedLandPoints)
                                    {
                                        if(!(pt is null))
                                        {
                                            _landHilightPoints.Add(transform.GetScreenCoords(Math.Cos(pt.ThetaRad) * pt.R, Math.Sin(pt.ThetaRad) * pt.R));
                                        }
                                        

                                    }
                                }
                                else
                                {
                                    foreach (PointCyl pt in dataset.CorrectedLandPoints)
                                    {
                                        if (!(pt is null))
                                        {                                        
                                            _landHilightPoints.Add(transform.GetScreenCoords(pt.ThetaRad, pt.R));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (_viewRolled)
                                {
                                    foreach (PointCyl pt in dataset.RawLandPoints)
                                    {
                                        if (!(pt is null))
                                        {
                                            _landHilightPoints.Add(transform.GetScreenCoords(Math.Cos(pt.ThetaRad) * pt.R, Math.Sin(pt.ThetaRad) * pt.R));
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (PointCyl pt in dataset.RawLandPoints)
                                    {
                                        if (!(pt is null))
                                        {
                                            _landHilightPoints.Add(transform.GetScreenCoords(pt.ThetaRad, pt.R));
                                        }
                                    }
                                }
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
                textBoxLandList.Text = "";
                labelLandCount.Text = "";

                radioButtonViewProcessed.Checked = true;
                radioButtonViewUnrolled.Checked = true;
                _lines.Add("***Processing***");
                _lines.Add(_inspScript.ScanFormat.ToString());
                _lines.Add("ProbeSpacing: "+ _inspScript.CalDataSet.ProbeSpacingInch.ToString());

                if(_inspDataSetList!= null && _inspDataSetList.Count>=1)               
                {

                    foreach(InspDataSet dataset in _inspDataSetList)
                    {
                        _lines.Add("Processing: " + dataset.Filename);
                        if(dataset.DataFormat== ScanFormat.RING)
                        {
                            _lines.Add("Nominal Minimum Diameter: " + dataset.CorrectedCylData.NominalMinDiam.ToString());
                            _lines.Add("---Correcting Eccentricity---");
                            _lines.Add("Raw land variation: " + dataset.GetRVariation(dataset.RawLandPoints).ToString());
                            _lines.Add("Corrected land variation: " + dataset.GetRVariation(dataset.CorrectedLandPoints).ToString());
                        }
                    }
                    
                    
                    _displayData.DataFormat = _inspDataSetList[0].DataFormat;
                    if(!_autoAngleCorrect && _displayData.DataFormat == ScanFormat.RING)
                    {
                        _displayData = SetDisplayData(_inspDataSetList);
                        DisplayData();
                    }
                    if(_displayData.DataFormat == ScanFormat.SPIRAL)
                    {                       
                      Load3DView(_inspDataSetList[0].CorrectedSpiralData);
                    }
                    
                }

                progressBarProcessing.Value = 0;



            }
            catch (Exception)
            {

                throw;
            }
        }
        void SmoothData()
        {
            foreach(InspDataSet set in _inspDataSetList)
            {
               var smoothedData =  DataUtilities.SmoothLinear(_minFeatureSize, set.CorrectedCylData);
                set.CorrectedCylData = smoothedData;
            }
            
        }
        
        bool _smoothData;
        double _minFeatureSize;
        private async void ButtonProcessFile_Click(object sender, EventArgs e)
        {
            try
            {
                _smoothData = true;
                _minFeatureSize = .002;
                _displayData = new DisplayDataSet();
                _inspDataSetList = new List<InspDataSet>();
                _hilightPts = new List<PointF>();
                Clear3DView();

                await ProcessFiles();
               
                if (_autoAngleCorrect  )
                {
                    CorrectForAveAngle();
                }
               
                if (_autoSaveOutput )
                {
                    SaveAllOutputFiles();
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
                _displayData = new DisplayDataSet();
                _displayData.Add(_barrelInspProfile.MinLandProfile);
                _displayData.Add(_barrelInspProfile.AveLandProfile);
                _displayData.Add(_barrelInspProfile.AveGrooveProfile);
                _displayData.DataFormat = ScanFormat.AXIAL;
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
                    _displayData = new DisplayDataSet();
                    var angErr = new AngleError(_barrel);
                    foreach (InspDataSet set in _inspDataSetList)
                    {
                        var correctedCylData = angErr.CorrectToMidpoint(set.CorrectedCylData, _midpoint);
                        set.CorrectedCylData = correctedCylData;
                        _displayData.Add(set.CorrectedCylData);                        
                    }
                    _displayData.DataFormat = ScanFormat.RING;
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
            _method = ScanFormat.RING;
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
            _method = ScanFormat.SPIRAL;
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
        private void SetRasterSwitches()
        {
            _method = ScanFormat.RASTER;
            textBoxStartPosA.Enabled = true;
            textBoxStartPosX.Enabled = true;
            textBoxEndPosA.Enabled = true;
            textBoxEndPosX.Enabled = true;

            textBoxPitch.Enabled = true;
            textBoxPtsPerRev.Enabled = true;
            textBoxAngleInc.Enabled = true;
            radioButtonAngleInc.Text = "Axial Increment(in):";
            radioButtonPtsperRev.Text = "Points per inch:";
            radioButtonPtsperRev.Checked = true;
            textBoxRingRevs.Enabled = false;

            //textBoxExtractX.Enabled = true;
            textBoxProbeCount.Text = "1";
        }
        private void SetAxialSwitches()
        {
            _method = ScanFormat.AXIAL;
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
        ScanFormat _method;
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
        private void comboBoxProbeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch(comboBoxProbeType.SelectedIndex)
                {
                    case 0:
                        break;
                    case 1:
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
            try
            {
                switch (comboBoxMethod.SelectedIndex)
                {
                    case 0:
                        SetRingSwitches();
                        break;
                    case 1:
                        SetSpiralSwitches();
                        break;
                    case 2:
                        SetRasterSwitches();
                        break;
                    case 3:
                        SetAxialSwitches();
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
      
        
        private void ComboBoxBarrel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //barrelType = Barrel.GetBarrelType(comboBoxBarrel.SelectedItem.ToString());
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
        LengthUnits _units;

        enum LengthUnits
        {
            METRIC_MM,
            INCH,
            METRIC_MICRONS,
            MICRO_INCH
        }

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

        ScreenTransform GetTransform(BoundingBox bbox, bool viewRolled, ScanFormat displayMethod)
        {
            try
            {
                ScreenTransform currentTransform;
                BoundingBox bb;
                switch (displayMethod )
                {
                    case ScanFormat.AXIAL:
                    bb = new BoundingBox(bbox.Min.Z, bbox.Min.X, 0, bbox.Max.Z, bbox.Max.X, 0);
                    currentTransform = new ScreenTransform(bb, pictureBox1.DisplayRectangle, .9, true);
                        break;
                    case ScanFormat.RING:
                        if (viewRolled)
                        {
                            bb = new BoundingBox(bbox.Max.X * -1, bbox.Max.X * -1, 0, bbox.Max.X, bbox.Max.X, 0);
                            currentTransform = new ScreenTransform(bb, pictureBox1.DisplayRectangle, .9, false);
                        }
                        else
                        {
                            bb = new BoundingBox(bbox.Min.Y, bbox.Min.X, 0, bbox.Max.Y, bbox.Max.X, 0);
                            currentTransform = new ScreenTransform(bb, pictureBox1.DisplayRectangle, .9, true);
                        }
                        break;
                    default:
                        bb = new BoundingBox(0, 0, 0, 1,1, 0);
                        currentTransform = new ScreenTransform(bb, pictureBox1.DisplayRectangle, .9, true);
                        break;

                }
            
                return currentTransform;
            }
            catch (Exception)
            {

                throw;
            }

        }
        private void SetupDisplay(ScreenTransform currentTransform)
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
                if (range <= .005)
                    gridRange = .005;
                if (range <= .01)
                    gridRange = .01;
                if (range > .01 & range < .1)
                {
                    gridRange = Math.Ceiling(range * 100) / 100;
                }
                if (range >= .1 && range < 1.0)
                {
                    gridRange = Math.Ceiling(range * 10) / 10;
                }
                if (range > 1 && range <= 10)
                {
                    gridRange = Math.Ceiling(range);
                }
                if (range > 10 && range <= 100)
                {
                    gridRange = 10 * Math.Ceiling(range / 10.0);
                }
                if (range > 100 && range <= 1000)
                {
                    gridRange = 100 * Math.Ceiling(range / 100.0);
                }
                return gridRange;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        void DrawGrid(BoundingBox bb, ScreenTransform currentTransform,ScanFormat method)
        {
            try
            {
                double sizeX = 0;
                double sizeY = 0;
                double minX = 0;
                double minY = 0;
                double maxX = 0;
                double maxY = 0;

                switch (method)
                {
                    case ScanFormat.AXIAL:
                        sizeX = bb.Size.Z;
                        sizeY = bb.Size.X;
                        minX = bb.Min.Z;
                        minY = bb.Min.X;
                        maxX = bb.Max.Z;
                        maxY = bb.Max.X;
                        break;
                    case ScanFormat.RING:
                    case ScanFormat.SPIRAL:                    
                    default:
                        sizeX = bb.Size.Y;
                        sizeY = bb.Size.X;
                        minX = bb.Min.Y;
                        minY = bb.Min.X;
                        maxX = bb.Max.Y;
                        maxY = bb.Max.X;
                        break;
                }

                sizeY = maxY - minY;
                sizeY = GetGridRange(sizeY);
                int xGridCount;
                int yGridCount = 10;
                if (_method == ScanFormat.AXIAL)
                {
                    xGridCount = 10;
                }
                else
                {
                    xGridCount = _barrel.DimensionData.GrooveCount;
                }

                double dxGrid = sizeX / xGridCount;
                double dyGrid = sizeY / yGridCount;
                var font = new Font(this.Font, FontStyle.Regular);
                //horizontal grids
                for (int i = 0; i <= xGridCount; i++)
                {
                    double xLabel = minX + (i * dxGrid);
                    var ptX = currentTransform.GetScreenCoords(xLabel, minY);
                    var ptYGridStart = currentTransform.GetScreenCoords(xLabel, minY);
                    var ptYGridEnd = currentTransform.GetScreenCoords(xLabel, minY + xGridCount* dyGrid);
                    string xLabelstr = "";
                    var sizeXlabel = _graphics.MeasureString(xLabelstr, font);
                    float xLabelxOffset = (float)(sizeXlabel.Width / 2.0);
                    float xLabelyOffset = (float)(sizeXlabel.Height * 0.1);
                    if (_method == ScanFormat.AXIAL)
                    {
                        xLabelstr = xLabel.ToString("f3");
                    }
                    else
                    {
                        xLabelstr = GeometryLib.Geometry.ToDegs(xLabel).ToString("f0");
                    }
                    _graphics.DrawString(xLabelstr, font, System.Drawing.Brushes.Black, ptX.X - xLabelxOffset, ptX.Y + xLabelyOffset);
                    _graphics.DrawLine(_greenPen, ptYGridStart, ptYGridEnd);
                }
                //vertical grids
                for (int i = 0; i <= yGridCount; i++)
                {

                    double yLabel = minY + (i * dyGrid);


                    var ptY = currentTransform.GetScreenCoords(minX, yLabel);
                    var ptXGridStart = currentTransform.GetScreenCoords(minX, yLabel);
                    var ptXGridEnd = currentTransform.GetScreenCoords(minX + yGridCount * dxGrid, yLabel);


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
        void DrawCircles(System.Drawing.Pen pen, List<PointF> points, int radius)
        {
            try
            {
                foreach (var pt in points)
                {
                    var rect = new RectangleF(pt.X - radius, pt.Y - radius,
                        radius * 2, radius * 2);
                    _graphics.DrawEllipse(pen, rect);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        BoundingBox _bbOverall;
        private void DisplayData()
        { 
            try
            {
                textBoxDataOut.Lines = _lines.ToArray();
                if(_displayData.DataFormat != ScanFormat.SPIRAL)
                {
                    if (_displayData != null && _displayData.Count != 0)
                    {
                        _bbOverall = DataUtilities.BuildBoundingBox(_displayData);
                        var screenPtsList = new List<List<PointF>>();
                        var currentTransform = GetTransform(_bbOverall, _viewRolled, _displayData.DataFormat);
                        HilightLandPoints(currentTransform);


                        SetupDisplay(currentTransform);

                        foreach (CylData data in _displayData)
                        {
                            _screenPts = new List<PointF>();
                            if (data != null && data.Count != 0)
                            {

                                int hiLightScreenRadius = (int)(Math.Min(5, .005 * pictureBox1.Width));
                                PointF xAxis1 = new PointF(0, 0);
                                PointF xAxis2 = new PointF(0, 0);
                                PointF yAxis1 = new PointF(0, 0);
                                PointF yAxis2 = new PointF(0, 0);

                                if (_displayData.DataFormat == ScanFormat.AXIAL)
                                {
                                    foreach (var pt in data)
                                    {
                                        _screenPts.Add(currentTransform.GetScreenCoords(pt.Z, pt.R));
                                    }
                                    xAxis1 = currentTransform.GetScreenCoords(_bbOverall.Min.Z, _bbOverall.Min.X);
                                    xAxis2 = currentTransform.GetScreenCoords(_bbOverall.Max.Z, _bbOverall.Min.X);
                                    yAxis1 = currentTransform.GetScreenCoords(_bbOverall.Min.Z, _bbOverall.Min.X);
                                    yAxis2 = currentTransform.GetScreenCoords(_bbOverall.Min.Z, _bbOverall.Max.X);

                                }
                                else
                                {
                                    if (_viewRolled)
                                    {
                                        foreach (var pt in data)
                                        {
                                            _screenPts.Add(currentTransform.GetScreenCoords(Math.Cos(pt.ThetaRad) * pt.R, Math.Sin(pt.ThetaRad) * pt.R));
                                        }
                                        xAxis1 = currentTransform.GetScreenCoords(0, 0);
                                        xAxis2 = currentTransform.GetScreenCoords(_bbOverall.Max.X, 0);
                                        yAxis1 = currentTransform.GetScreenCoords(0, 0);
                                        yAxis2 = currentTransform.GetScreenCoords(0, _bbOverall.Max.X);
                                    }
                                    else
                                    {
                                        foreach (var pt in data)
                                        {
                                            _screenPts.Add(currentTransform.GetScreenCoords(pt.ThetaRad, pt.R));
                                        }

                                        if (_landHilightPoints != null)
                                        {
                                            DrawCircles(_redPen, _landHilightPoints, hiLightScreenRadius);

                                        }
                                        if (_hilightPts != null)
                                        {
                                            DrawCircles(_orangePen, _hilightPts, hiLightScreenRadius);

                                        }
                                        if (_depthMeasurePoints != null)
                                        {
                                            List<PointF> depthPoints = new List<PointF>();
                                            foreach (var pt in _depthMeasurePoints)
                                            {
                                                depthPoints.Add(currentTransform.GetScreenCoords(pt.ThetaRad, pt.R));
                                            }
                                            DrawCircles(_orangePen, depthPoints, hiLightScreenRadius);
                                        }



                                    }
                                }


                                //_graphics.DrawLine(_greenPen, xAxis1, xAxis2);
                                // _graphics.DrawLine(_greenPen, yAxis1, yAxis2);
                            }
                            screenPtsList.Add(_screenPts);
                        }
                        DrawGrid(_bbOverall, currentTransform, _displayData.DataFormat);

                        foreach (List<PointF> pointList in screenPtsList)
                        {
                            _graphics.DrawLines(_bluePen, pointList.ToArray());
                        }
                        pictureBox1.Image = _bitmap;
                    }
               
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
        //private void DisplayData(List<CylData> dataSetList)
        //{
        //    try
        //    {
        //        textBoxDataOut.Lines = _lines.ToArray();
        //        if (dataSetList != null && dataSetList.Count != 0)
        //        {
        //            _bbOverall  = DataUtilities.BuildBoundingBox(dataSetList);
        //            var screenPtsList = new List<List<PointF>>();
        //            HilightLandPoints();

        //            var currentTransform = GetTransform(_bbOverall, _viewRolled);
        //            SetupDisplay(currentTransform);

        //            foreach (CylData data in dataSetList)
        //            {
        //                _screenPts = new List<PointF>();
        //                if (data != null && data.Count != 0)
        //                {

        //                    int hiLightScreenRadius = (int)(Math.Min(5, .005 * pictureBox1.Width));
        //                    PointF xAxis1 = new PointF(0, 0);
        //                    PointF xAxis2 = new PointF(0, 0);
        //                    PointF yAxis1 = new PointF(0, 0);
        //                    PointF yAxis2 = new PointF(0, 0);

        //                    if (_method == ScanFormat.AXIAL)
        //                    {
        //                        foreach (var pt in data)
        //                        {
        //                            _screenPts.Add(currentTransform.GetScreenCoords(pt.Z, pt.R));
        //                        }
        //                        xAxis1 = currentTransform.GetScreenCoords(_bbOverall.Min.Z, _bbOverall.Min.X);
        //                        xAxis2 = currentTransform.GetScreenCoords(_bbOverall.Max.Z, _bbOverall.Min.X);
        //                        yAxis1 = currentTransform.GetScreenCoords(_bbOverall.Min.Z, _bbOverall.Min.X);
        //                        yAxis2 = currentTransform.GetScreenCoords(_bbOverall.Min.Z, _bbOverall.Max.X);
        //                    }
        //                    else
        //                    {
        //                        if (_viewRolled)
        //                        {
        //                            foreach (var pt in data)
        //                            {
        //                                _screenPts.Add(currentTransform.GetScreenCoords(Math.Cos(pt.ThetaRad) * pt.R, Math.Sin(pt.ThetaRad) * pt.R));
        //                            }
        //                            xAxis1 = currentTransform.GetScreenCoords(0, 0);
        //                            xAxis2 = currentTransform.GetScreenCoords(_bbOverall.Max.X, 0);
        //                            yAxis1 = currentTransform.GetScreenCoords(0, 0);
        //                            yAxis2 = currentTransform.GetScreenCoords(0, _bbOverall.Max.X);
        //                        }
        //                        else
        //                        {
        //                            foreach (var pt in data)
        //                            {
        //                                _screenPts.Add(currentTransform.GetScreenCoords(pt.ThetaRad, pt.R));
        //                            }
                                                                     
        //                            if(_landHilightPoints != null)
        //                            {
        //                                DrawCircles(_redPen, _landHilightPoints, hiLightScreenRadius);
                                        
        //                            }
        //                            if (_hilightPts != null)
        //                            {
        //                                DrawCircles(_orangePen, _hilightPts, hiLightScreenRadius);
                                       
        //                            }
        //                            if (_depthMeasurePoints != null)
        //                            {
        //                                List<PointF> depthPoints = new List<PointF>();
        //                                foreach (var pt in _depthMeasurePoints)
        //                                {
        //                                    depthPoints.Add(currentTransform.GetScreenCoords(pt.ThetaRad, pt.R));
        //                                }
        //                                DrawCircles(_orangePen, depthPoints, hiLightScreenRadius);
        //                            }

        //                            DrawGrid(_bbOverall, currentTransform);

        //                        }
        //                    }

                           
        //                    _graphics.DrawLine(_greenPen, xAxis1, xAxis2);
        //                    _graphics.DrawLine(_greenPen, yAxis1, yAxis2);
        //                }
        //                screenPtsList.Add(_screenPts);
        //            }
                    

        //            foreach(List<PointF> pointList in screenPtsList)
        //            { 
        //                _graphics.DrawLines(_bluePen, pointList.ToArray());
        //            }
        //            pictureBox1.Image = _bitmap;
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
        
      
        void DrawMeasurement(Point start, Point end, ScreenTransform currentTransform)
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

                double xLen = 0;
                double yLen = 0;
                var endPartCoords = currentTransform.GetModelCoords(end);
                var startPartCoords = currentTransform.GetModelCoords(start);
                if (!_viewRolled)
                {
                    xLen = GeometryLib.Geometry.ToDegs(Math.Abs(endPartCoords.X - startPartCoords.X));
                    yLen = endPartCoords.Y - startPartCoords.Y;
                }
                string length = xLen.ToString("f4") + "," + yLen.ToString("f5");
                var size = _graphics.MeasureString(length, this.Font);
                int midX = (int)(start.X + (end.X - start.X - (size.Width / 2.0)) / 2.0);
                int midY = (int)(start.Y + (end.Y - start.Y - (size.Height / 2.0)) / 2.0);
                gtemp.DrawLine(_blackPen, end, start);
                SolidBrush brush = new SolidBrush(System.Drawing.Color.White);
                gtemp.FillRectangle(brush, midX, midY, size.Width, size.Height);
                gtemp.DrawRectangle(_blackPen, midX, midY, size.Width, size.Height);
                gtemp.DrawString(length, Font, System.Drawing.Brushes.Black, new PointF(midX, midY));
                labelDxMeasured.Text = "DTheta: " + xLen.ToString("f4") + " " + _angleLabel;
                labelDyMeasured.Text = "Dr: " + yLen.ToString("f5") + " " + _lengthLabel;
                pictureBox1.Image = bit2;
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
                if (_inspDataSetList != null && _inspDataSetList.Count>0)
                {
                    var currentTransform = GetTransform(_bbOverall, _viewRolled,_displayData.DataFormat);
                    _mouseLocation = e.Location;
                    _mouseLocPart = currentTransform.GetModelCoords(e.Location);
                    _mousePartXY = GetXYCoords(_mouseLocPart);
                    if (_method == ScanFormat.AXIAL)
                    {
                        labelZPosition.Text = "Axial: " + _mouseLocPart.X.ToString("F6") + _lengthLabel;
                        labelYPosition.Text = "Radius: " + _mouseLocPart.Y.ToString("f6") + _lengthLabel;
                        labelXPosition.Text = "Angle: " + (180*(_inspScript.StartThetaRad)/Math.PI).ToString("f6") + _angleLabel;
                    }
                    else
                    {
                        if (_viewRolled)
                        {
                            double r = Math.Sqrt(Math.Pow(_mouseLocPart.X, 2.0) + Math.Pow(_mouseLocPart.Y, 2.0));
                            double th = GeometryLib.Geometry.ToDegs(Math.Atan2(_mouseLocPart.Y, _mouseLocPart.X));
                            labelXPosition.Text = "Angle: " + th.ToString("f6") + " " + _angleLabel;
                            labelYPosition.Text = "Radius: " + r.ToString("f6") + " " + _lengthLabel;

                        }
                        else
                        {
                            double angle = GeometryLib.Geometry.ToDegs(_mouseLocPart.X);
                            labelXPosition.Text = "Angle: " + angle.ToString("f6") + " " + _angleLabel;
                            labelYPosition.Text = "Radius: " + _mouseLocPart.Y.ToString("f6") + " " + _lengthLabel;

                        }
                        labelZPosition.Text = "Axial: " + _inspScript.StartZ.ToString("F5") + _lengthLabel;
                    }



                    if (_dataSelection==DataSelection.MEASURELENGTH)
                    {
                        if (_mouseDownCount == 1)
                        {
                            DrawMeasurement(_mouseDownLocation, _mouseLocation, currentTransform);
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

        
        
        bool _useLandPts;
        int _landCount;

        List<PointCyl> _selectedSidewallPoints;
        List<PointF> _hilightPts;
        List<PointCyl> _selectedLandPoints;
        List<PointF> _landHilightPoints;
        int _grooveIntersectionCount;
        PointCyl _knownRadiusPt;
        bool _mouseDown;
        Point _mouseDownLocation;
        PointF _mouseDownPart;
        PointF _mouseUpPart;
        PointF _mouseLocPart;
        Point _mouseUpLocation;
        Point _mouseLocation;
        PointF _mouseDownPartXY;
        PointF _mousePartXY;
        TextureBrush _imgBrush;
        int _mouseDownCount;


        private PointF GetNearestModelPoint(System.Windows.Forms.MouseEventArgs e, ScreenTransform currentTransform)
        {
            try
            {
                if (_hilightPts == null)
                {
                    _hilightPts = new List<PointF>();
                }
                var nearestScreenPt = PictureBoxUtilities.GetNearestPoint(e.Location, _screenPts);
                _hilightPts.Add(nearestScreenPt);
                var pt = currentTransform.GetModelCoords(nearestScreenPt);
                return pt;
            }
            catch (Exception)
            {

                throw;
            }
           
            
           
        }
        

        private void SelectGroovesMouseClick(System.Windows.Forms.MouseEventArgs e, ScreenTransform currentTransform)
        {
            try
            {
                var pt = GetNearestModelPoint(e, currentTransform);
                _grooveIntersectionCount++;

                _selectedSidewallPoints.Add(new PointCyl(pt.Y, pt.X, 0));

                textBoxLandList.Text += GeometryLib.Geometry.ToDegs(pt.X).ToString("f4") + "," + pt.Y.ToString("f5") + " ";
                labelLandCount.Text = "Groove Intersections: " + _grooveIntersectionCount;
            }
            catch (Exception)
            {

                throw;
            }
          
        }
        private void SelectLandsMouseClick(System.Windows.Forms.MouseEventArgs e, ScreenTransform currentTransform)
        {
            try
            {
                var pt = GetNearestModelPoint(e, currentTransform);
                _landCount++;
                _useLandPts = true;
                _selectedLandPoints.Add(new PointCyl(pt.Y, pt.X, 0));
            }
            catch (Exception)
            {

                throw;
            }
          
            
        }
        private void SetMidpointClick(System.Windows.Forms.MouseEventArgs e, ScreenTransform currentTransform)
        {
            try
            {
                _hilightPts = new List<PointF>();
                var pt = GetNearestModelPoint(e, currentTransform);
                _midpoint = new PointCyl(pt.Y, pt.X, 0);
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        

        private void SetRadiusMouseClick(System.Windows.Forms.MouseEventArgs e, ScreenTransform currentTransform)
        {
            try
            {
                var pt = GetNearestModelPoint(e, currentTransform);
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
                if (_displayData != null && _displayData.Count > 0)
                {
                    var currentTransform = GetTransform(_bbOverall, _viewRolled,_displayData.DataFormat);
                    switch (_dataSelection)
                    {
                        case DataSelection.SELECTGROOVES:
                            SelectGroovesMouseClick(e, currentTransform);
                            break;
                        case DataSelection.MEASURELENGTH:
                            break;
                        case DataSelection.SELECTLANDS:
                            SelectLandsMouseClick(e, currentTransform);
                            break;
                        case DataSelection.SETRADIUS:
                            SetRadiusMouseClick(e, currentTransform);
                            break;
                        case DataSelection.SELECTMIDPOINT:
                            SetMidpointClick(e, currentTransform);
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
        private void PictureBox1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (_displayData != null && _displayData.Count >0)
                {
                    var currentTransform = GetTransform(_bbOverall, _viewRolled,_displayData.DataFormat);
                    _mouseDown = false;
                    _mouseUpLocation = e.Location;
                    _mouseUpPart = currentTransform.GetModelCoords(_mouseUpLocation);

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
                if (_displayData != null && _displayData.Count >0)
                {
                    var currentTransform = GetTransform(_bbOverall, _viewRolled, _displayData.DataFormat);
                    _mouseDownCount++;
                    _mouseDown = true;
                    var prevMouseDownLoc = _mouseDownLocation;
                    _mouseDownLocation = e.Location;
                    _mouseDownPart = currentTransform.GetModelCoords(_mouseDownLocation);
                    _mouseDownPartXY = GetXYCoords(_mouseDownPart);


                    var img = new Bitmap(pictureBox1.Image);
                    _imgBrush = new TextureBrush(img);
                    if (_dataSelection==DataSelection.MEASURELENGTH)
                    {
                        if (_mouseDownCount == 2)
                        {
                            DrawMeasurement(prevMouseDownLoc, _mouseDownLocation,currentTransform);
                        }
                        if (_mouseDownCount == 3)
                        {
                            pictureBox1.Image = _bitmap;
                            pictureBox1.Refresh();
                            _mouseDownCount = 1;
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
        private PointF GetXYCoords(PointF partCoords)
        {
            try
            {
                var x = Math.Cos(partCoords.X) * partCoords.Y;
                var y = Math.Sin(partCoords.X) * partCoords.Y;
                return new PointF((float)x, (float)y);
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
                    
                    if (_displayData != null)
                    {
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
        DisplayDataSet SetDisplayData(List<InspDataSet> inspDataSetList)
        {
            try
            {
                var displayDataList = new DisplayDataSet();
                if (inspDataSetList != null)
                {
                    
                    foreach (InspDataSet inspDataSet in inspDataSetList)
                    {                      
                            displayDataList.Add(SetDisplayData(inspDataSet));                                           
                    }
                    displayDataList.DataFormat = inspDataSetList[0].DataFormat;
                }
                
                return displayDataList;
            }
            catch (Exception)
            {

                throw;
            }
        }
       
        CylData SetDisplayData(InspDataSet inspDataSet)
        {
            try
            {
                var displayData = new CylData();
                if (inspDataSet != null )
                {
                    if (_viewProcessed)
                    {
                        return inspDataSet.CorrectedCylData;
                    }
                    else
                    {
                        return inspDataSet.UncorrectedCylData;
                    }
                }
                return displayData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        bool _viewRolled;
        private void RadioButtonViewRolled_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                _viewRolled = radioButtonViewRolled.Checked;
                if (_inspDataSetList != null && _inspDataSetList.Count >0)
                {
                    if (_displayData != null)
                        DisplayData();
                }
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ": Unable to change to/from rolled view.");
            }
        }

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
                    _displayData = SetDisplayData(_inspDataSetList);
                    DisplayData();
                }


            }
            catch (Exception ex)
            {

                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message + ": Unable to change to/from processed view.");
            }

        }

        List<string> _lines;
        void CorrectForAveAngle()
        {
            try
            {
               
                if (_inspDataSetList != null && _inspDataSetList.Count >0)
                {
                    _displayData = new DisplayDataSet();
                    foreach (InspDataSet inspDataSet in _inspDataSetList)
                    {
                        var da = new AngleError(_barrel);
                        
                        CylData corrData;
                        if (_processManualGrooveIntersects)
                        {
                            corrData = da.CorrectForAngleError(inspDataSet.CorrectedCylData, _selectedSidewallPoints);
                        }
                        else
                        {
                            corrData = da.CorrectForAngleError(inspDataSet.CorrectedCylData);
                        }

                         inspDataSet.CorrectedCylData = corrData;
                         inspDataSet.CorrectedLandPoints = da.CorrectForError(inspDataSet.CorrectedLandPoints);
                        _lines.Add("***Correcting Angle****" + inspDataSet.Filename);                      
                        
                        _lines.Add("Correction angle: " + GeometryLib.Geometry.ToDegs(da.CorrectionAngle).ToString("f5") + " degs");                      
                        _lines.Add("Ave radius: " + da.AveRadius.ToString("f5"));                                         
                        _displayData.Add(inspDataSet.CorrectedCylData);
                        _displayData.DataFormat = inspDataSet.DataFormat;
                    }
                   
                    DisplayData();
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
        private void MeasureDepths()
        {
            try
            {
                string machineSpeedFilename = "";
                if (_dataOutOptions.UseDefBrchRasterFile && _dataOutOptions.DefBreachRasterFilename != ""
                    && System.IO.File.Exists(_dataOutOptions.DefBreachRasterFilename))
                {

                    machineSpeedFilename = _dataOutOptions.DefBreachRasterFilename;
                }
                else
                {
                    var ofd = new OpenFileDialog
                    {
                        Title = "Machine Speed & Raster Location Input file",
                        Filter = "(*.csv)|*.csv"
                    };
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        machineSpeedFilename = ofd.FileName;
                    }
                }

                BuildScriptFromInputs(_inputFileName);
                
                _grooveMeasurements = new BarrelXsectionProfile(_barrel, _dataOutOptions, _barrel.MachiningData.CurrentAWJPasses, _inspScript.StartZ, machineSpeedFilename);
                _grooveMeasurements.CalcAverageDepths(_displayData[0]);
                textBoxDataOut.Lines = _grooveMeasurements.AsStringList().ToArray();
                _depthMeasurePoints = new List<PointCyl>();
                foreach (var groove in _grooveMeasurements)
                {
                    foreach (var dm in groove)
                    {
                        _depthMeasurePoints.Add(dm.Datum);
                    }
                }

                DisplayData();
                

            }
            catch (Exception)
            {

                throw;
            }
        }
        List<PointCyl> _depthMeasurePoints;
        BarrelXsectionProfile _grooveMeasurements;
        private void ButtonMeasure_Click(object sender, EventArgs e)
        {
            try
            {
                MeasureDepths();

            }
            catch (Exception ex)
            {

                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message );
            }
        }

        private void measureDepthsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MeasureDepths();

            }
            catch (Exception ex)
            {

                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message );
            }
        }
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
          
           
            toolStripButtonLand.Checked = false;
            toolStripButtonLand.BackColor = DefaultBackColor;
            toolStripButtonCursor.Checked = false;
            toolStripButtonCursor.BackColor = DefaultBackColor;
            toolStripButtonLength.Checked = false;
            toolStripButtonLength.BackColor = DefaultBackColor;
            toolStripButtonSetKnownRadius.Checked = false;
            toolStripButtonSetKnownRadius.BackColor = DefaultBackColor;
            toolStripButtonSelectGrooves.Checked = false;
            toolStripButtonSelectGrooves.BackColor = DefaultBackColor;
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
        private void ToolStripButtonLand_Click(object sender, EventArgs e)
        {
            try
            {

                ResetOnClick();
                _hilightPts = new List<PointF>();
                _selectedLandPoints = new List<PointCyl>();
                checkBoxLandPoints.Checked = true;
                _dataSelection = DataSelection.SELECTLANDS;
                checkBoxGrooveInter.Checked = false;
                radioButtonViewRaw.Checked = true;
                radioButtonViewUnrolled.Checked = true;

                toolStripButtonLand.BackColor = System.Drawing.Color.Red;
                labelStatus.Text = "Manually select lands.";
                _displayData = SetDisplayData(_inspDataSetList);
                DisplayData();


            }
            catch (Exception ex)
            {

                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message );
            }
        }
        private void ToolStripButtonLength_Click(object sender, EventArgs e)
        {
            try
            {
                ResetOnClick();

                _hilightPts = new List<PointF>();
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
                _hilightPts = new List<PointF>();
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

        private void ToolStripButtonSelectGrooves_Click(object sender, EventArgs e)
        {
            try
            {
                ResetOnClick();
                _selectedSidewallPoints = new List<PointCyl>();
                _hilightPts = new List<PointF>();
                labelStatus.Text = "Manually select groove sidewalls.";
               _dataSelection = DataSelection.SELECTGROOVES;
                toolStripButtonSelectGrooves.BackColor = System.Drawing.Color.Red;
                checkBoxGrooveInter.Checked = true;
                checkBoxLandPoints.Checked = false; 
              
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
            _hilightPts = new List<PointF>();
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
                            if (_inspDataSetList[i].CorrectedSpiralData.Count != 0)
                            {
                                DataFileBuilder.SaveCSVFile(_barrel, _dataOutOptions, _inspDataSetList[i].CorrectedSpiralData,
                                    fileNames[i], new Progress<int>(p => ShowProgress(p)));
                                progressBarProcessing.Value = 0;
                                DataFileBuilder.SavePlyFile(_barrel, _dataOutOptions, _inspDataSetList[i].CorrectedSpiralData,
                                    _barrel.DimensionData.LandNominalDiam / 2.0, fileNames[i], new Progress<int>(p => ShowProgress(p)));
                                progressBarProcessing.Value = 0;
                            }
                            if (_inspDataSetList[i].CorrectedCylData.Count != 0)
                            {

                                DataFileBuilder.SaveCSVFile(_barrel, _dataOutOptions, _inspDataSetList[i].CorrectedCylData,
                                    fileNames[i], new Progress<int>(p => ShowProgress(p)));
                                progressBarProcessing.Value = 0;
                                DataFileBuilder.SaveDXF(_inspDataSetList[i].CorrectedCylData, fileNames[i], new Progress<int>(p => ShowProgress(p)));
                                progressBarProcessing.Value = 0;
                                if (_grooveMeasurements != null)
                                {
                                    _grooveMeasurements.SaveMeasurements(fileNames[i], fileNames[i], false, true);
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
                if (_inspDataSetList != null && _inspDataSetList[0].CorrectedCylData.Count != 0)
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
                        await SaveCSVFile(sfd.FileName, _inspDataSetList[0].CorrectedCylData, new Progress<int>(p => ShowProgress(p)));
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
                if (_inspDataSetList != null && _inspDataSetList[0].CorrectedCylData.Count != 0)
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
                        DataFileBuilder.SaveDXF(_inspDataSetList[0].CorrectedCylData, sfd.FileName, new Progress<int>(p => ShowProgress(p)));
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
        private Task SavePlyFile(string filename, IProgress<int> progress)
        {
            try
            {
                return Task.Run(() => DataFileBuilder.SavePlyFile(_barrel, _dataOutOptions, _inspDataSetList[0].CorrectedSpiralData, _barrel.DimensionData.LandNominalDiam / 2.0, filename, progress));

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
                if (_inspDataSetList != null && _inspDataSetList[0].CorrectedSpiralData.Count != 0)
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
                        await SavePlyFile(sfd.FileName, new Progress<int>(p => ShowProgress(p)));
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
        private Task SaveSTLFile(string filename,IProgress<int>progress)        
        {
            return Task.Run(() => DataFileBuilder.SaveSTLFile(_barrel, _dataOutOptions, _inspDataSetList[0].CorrectedSpiralData, _barrel.DimensionData.LandNominalDiam / 2.0, filename, progress));
        }
        private async Task SaveSTLFileAsync()
        {
            try
            {
                if (_inspDataSetList != null && _inspDataSetList[0].CorrectedSpiralData.Count != 0)
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
                        await SaveSTLFile(sfd.FileName, new Progress<int>(p => ShowProgress(p)));
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
                        if (_displayData != null)
                        {
                            var resetData = DataBuilder.ResetToKnownRadius(_displayData[0], _knownRadiusPt, knownR);
                            _displayData.Add(resetData);
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
        private void CheckBoxLandPoints_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                _useLandPts = checkBoxLandPoints.Checked;
                textBoxLandList.Text = "";
            }
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message +": Unable to select lands.");

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
        bool _autoDepthMeasure;
        private void CheckBoxDepthMeasure_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
               // _autoDepthMeasure = checkBoxDepthMeasure.Checked;
                //buttonMeasure.Enabled = !(_autoDepthMeasure);
            }
           
            catch (Exception ex)
            {
                _logFile.SaveMessage(ex);
                MessageBox.Show(ex.Message +":" );

            }
        }
    bool _autoSaveOutput;
        private void CheckBoxAutoSaveOutput_CheckedChanged(object sender, EventArgs e)
        {
            _autoSaveOutput = checkBoxAutoSaveOutput.Checked;
        }
        bool _saveAllDepths;
        //private void CheckBoxSaveAllDepths_CheckedChanged(object sender, EventArgs e)
        //{
        //    _saveAllDepths = checkBoxSaveAllDepths.Checked;
        //}
        //bool _saveAveDepths;
        //private void CheckBoxSaveAveDepths_CheckedChanged(object sender, EventArgs e)
        //{
        //    _saveAveDepths = checkBoxSaveAveDepths.Checked;
        //}
        bool _processManualGrooveIntersects;
        private void CheckBoxGrooveInter_CheckedChanged(object sender, EventArgs e)
        {
            _processManualGrooveIntersects = checkBoxGrooveInter.Checked;
            
        }

       
        enum DiamCalType
        {
            DEFAULT,
            USER,
            BOREPROFILE,
            RINGCAL
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
        private async Task ExtractSections(Progress<int>progress)
        {
            if (_inspDataSetList != null && _inspDataSetList[0].CorrectedSpiralData.Count != 0)
            {
                BuildScriptFromInputs(_inputFileName);
                var spiralBuilder = new SpiralDataBuilder(_barrel);
                var rings = spiralBuilder.ExtractRings(_inspDataSetList[0].CorrectedSpiralData, _inspScript);
               
                foreach (var ring in rings)
                {
                    string  xLocation ="ring-x"+ ring[0].Z.ToString("f2");
                    string filename = DataFileBuilder.BuildFileName(_inspDataSetList[0].Filename, xLocation, "csv");
                    await SaveCSVFile(filename,ring, progress);
                }
            }
        }
        private async void buttonExtractSections_Click(object sender, EventArgs e)
        {
            try
            {
                await ExtractSections(new Progress<int>(p => ShowProgress(p)));
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

     
        bool _useDualProbeAve;
        private void checkBoxDualProbeAve_CheckedChanged(object sender, EventArgs e)
        {
            _useDualProbeAve = checkBoxDualProbeAve.Checked;
            textBoxProbePhaseDeg.Enabled = _useDualProbeAve;
            if (_useDualProbeAve)
            {
                textBoxProbeCount.Text = "2";
            }
            else
            {
                textBoxProbeCount.Text = "1";
            }
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

            mb.BuildModel(ref MainModel3Dgroup, spiralScan,radialDirection, nominalRadius, maxDepth, scaleFactor, colorcode);

            ModelVisual3D model_visual = new ModelVisual3D();
            model_visual.Content = MainModel3Dgroup;
            // Display the main visual to the viewport
            userControl11.MainViewport.Children.Add(model_visual);
            tabControlOutput.SelectTab(2);
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

        private void toolStripButtonRotate_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonMirror_Click(object sender, EventArgs e)
        {

        }

        
    }
}
