using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InspectionLib;
using WinFormsLib;
using FileIOLib;

namespace BarrelInspectionProcessorForm
{
    public partial class OptionsForm : Form
    {
        LogFile _logfile;
        Dictionary<string, int> colorCodeDict;
        public DataOutputOptions Options
        {
            get
            { return _opt; }
            set
            { _opt = value; }
        }
        DataOutputOptions _opt;
        // MONO
        //GREEN_RED
        //RAINBOW
        //MONO_RED
        //CONTOURS
        public OptionsForm(LogFile logfile)
        {
            try
            {
                InitializeComponent();
                var names = Enum.GetNames(typeof(DataLib.COLORCODE));
                colorCodeDict = new Dictionary<string, int>();
                for(int i=0;i<names.Length;i++)
                {
                    colorCodeDict.Add(names[i], i);
                }
                comboBoxColorCode.Items.Clear();
                foreach (var name in names)
                {
                    comboBoxColorCode.Items.Add(name);
                }
                _logfile = logfile;
                
                comboBoxColorCode.SelectedIndex = 0;
                radioButtonFlat.Checked = true;
                if (DataOutputOptions.FileName != null && System.IO.File.Exists(DataOutputOptions.FileName))
                {
                    loadObjToForm(DataOutputOptions.FileName);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
           
        }
        private void loadObjToForm(string filename)
        {
            try
            {
                _opt = DataOptionsFile.Open(filename);

                checkBoxUncorrectedCSV.Checked = _opt.SaveCSVUncorrected;
                radioButtonFlat.Checked = _opt.SaveSurfaceFlat;
                textBoxScaleFactor.Text = _opt.SurfaceFileScaleFactor.ToString();
                textBoxBarrelProfile.Text = _opt.DefProfileFilename;
                checkBoxUseBarrelFile.Checked = _opt.UseDefBarrelProfile;
                textBoxBrchRasterFile.Text = _opt.DefBreachRasterFilename;
                checkBoxUseBrchRaster.Checked = _opt.UseDefBrchRasterFile;
                textBoxMuzzRasterFile.Text = _opt.DefMuzzleRasterFilename;
                checkBoxUseMuzRaster.Checked = _opt.UseDefMuzzleRasterFile;
                comboBoxColorCode.SelectedIndex =colorCodeDict[ _opt.SurfaceColorCode.ToString()];
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        void loadFormToObj()
        {
            try
            {
                _opt = new DataOutputOptions();
                _opt.SurfaceColorCode = (DataLib.COLORCODE)Enum.Parse(typeof(DataLib.COLORCODE),comboBoxColorCode.SelectedItem.ToString());
                //switch (comboBoxColorCode.SelectedIndex)
                //{
                //    case 0: 
                //    default:
                //        _opt.SurfaceColorCode = DataLib.COLORCODE.MONO;
                //        break;
                //    case 1:
                //        _opt.SurfaceColorCode = DataLib.COLORCODE.GREEN_RED;
                //        break;
                //    case 2:
                //        _opt.SurfaceColorCode = DataLib.COLORCODE.RAINBOW;
                //        break;
                //    case 3:
                //        _opt.SurfaceColorCode = DataLib.COLORCODE.MONO_RED;
                //        break;
                //    case 4:
                //        _opt.SurfaceColorCode = DataLib.COLORCODE.CONTOURS;
                //        break;
                //}
                if (_opt.SurfaceColorCode == DataLib.COLORCODE.MONO)
                {
                    _opt.ColorCodeData = false;
                }
                else
                {
                    _opt.ColorCodeData = true;
                }

                _opt.SaveCSVUncorrected = checkBoxUncorrectedCSV.Checked;
                _opt.SaveSurfaceFlat = radioButtonFlat.Checked;
                var filename = textBoxMuzzRasterFile.Text;
                if (filename != "" && System.IO.File.Exists(filename))
                {
                    _opt.DefMuzzleRasterFilename = filename;
                    _opt.UseDefMuzzleRasterFile = checkBoxUseMuzRaster.Checked;

                }
                else
                {
                    _opt.DefMuzzleRasterFilename = "";
                    _opt.UseDefMuzzleRasterFile = false;
                }
                filename = textBoxBrchRasterFile.Text;
                if (filename != "" && System.IO.File.Exists(filename))
                {
                    _opt.DefBreachRasterFilename = filename;
                    _opt.UseDefBrchRasterFile = checkBoxUseBrchRaster.Checked;
                }
                else
                {
                    _opt.DefBreachRasterFilename = "";
                    _opt.UseDefBrchRasterFile = false;
                }
                filename = textBoxBarrelProfile.Text;
                if (filename != "" && System.IO.File.Exists(filename))
                {
                    _opt.DefProfileFilename = filename;
                    _opt.UseDefBarrelProfile = checkBoxUseBarrelFile.Checked;
                }
                else
                {
                    _opt.DefProfileFilename = "";
                    _opt.UseDefBarrelProfile = false;
                }
                double scaleFactor = 10;
                InputVerification.TryGetValue(textBoxScaleFactor, "must be >0", out scaleFactor);
                _opt.SurfaceFileScaleFactor = scaleFactor;
            }
            catch (Exception)
            {

                throw;
            }
           

        }
       
        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                loadFormToObj();
                DataOptionsFile.Save(_opt, DataOutputOptions.FileName);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void buttonBrowseRaster_Click(object sender, EventArgs e)
        {
            try
            {
                var ofd = new OpenFileDialog();
                ofd.Title = "Machine Speed & Raster Location Input file";
                ofd.Filter = "(*.csv)|*.csv";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBoxBrchRasterFile.Text = ofd.FileName;
                }
            }
            catch (Exception ex)
            {
                _logfile.SaveMessage(ex);
                MessageBox.Show(ex.Message);
            }

        }

        private void buttonBrowseBarrelProfile_Click(object sender, EventArgs e)
        {
            try
            {
                var ofd = new OpenFileDialog();
                ofd.Title = "Machine Speed & Raster Location Input file";
                ofd.Filter = "(*.csv)|*.csv";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBoxBarrelProfile.Text = ofd.FileName;
                }
            }
            catch (Exception ex)
            {
                _logfile.SaveMessage(ex);
                MessageBox.Show(ex.Message);
            }

        }

        private void buttonBrowseMuzRaster_Click(object sender, EventArgs e)
        {
            try
            {
                var ofd = new OpenFileDialog();
                ofd.Title = "Machine Speed & Raster Location Input file";
                ofd.Filter = "(*.csv)|*.csv";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBoxMuzzRasterFile.Text = ofd.FileName;
                }
            }
            catch (Exception ex)
            {
                _logfile.SaveMessage(ex);
                MessageBox.Show(ex.Message);
            }

        }
    }
}
