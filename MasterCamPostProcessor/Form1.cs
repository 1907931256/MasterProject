using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolpathLib;
using FileIOLib;

namespace MasterCamPostProcessor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string nciFilename;
        string ncFilename;
        string machineFilename;
        ToolPath5Axis toolpath;
        CNCMachineCode cncMachineCode;
        List<string> fileHeader;
        List<string> ncFile;
        List<string> nciFile;

        private void buttonOpenNciFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "nci files (*.nci)|*.nci";
            if(openFileDialog.ShowDialog()== DialogResult.OK)
            {
                nciFilename = openFileDialog.FileName;
                labelNciFilename.Text = nciFilename;
                nciFile = FileIO.ReadDataTextFile(nciFilename);
                toolpath = CNCFileParser.CreatePath(nciFilename);
                textBoxNCI.Lines = nciFile.ToArray();
            }
            
        }

        private void buttonCreateNCFile_Click(object sender, EventArgs e)
        {
            fileHeader = new List<string>();
            fileHeader.Add("testfilename");
            if(cncMachineCode == null)
            {
                cncMachineCode = new CNCMachineCode();
            }
            NcFileBuilder ncFileBuilder = new NcFileBuilder(cncMachineCode);
            ncFile = ncFileBuilder.Build(toolpath, false, fileHeader);
            textBoxNC.Lines = ncFile.ToArray();
        }

        private void buttonSaveNCFile_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "nc files (*.nc)|*.nc";
            if(sfd.ShowDialog()== DialogResult.OK)
            {
                FileIO.Save(textBoxNC.Lines, sfd.FileName);
            }
        }

        private void buttonSelectMachine_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
