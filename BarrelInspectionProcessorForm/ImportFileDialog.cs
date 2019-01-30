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
using FileIOLib;

namespace BarrelInspectionProcessorForm
{
    public partial class ImportFileDialog : Form
    {
        public InspDataSet InspDataSet { get; private set; }

        public ImportFileDialog()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            
        }
        void OpenFile(string filename)
        {
            var lines = FileIO.ReadDataTextFile(filename);
            _rowCount = lines.Count;
            int maxColCount=0;
            foreach(var line in lines)
            {
                var words = FileIO.Split(line);
                if(words.Length > maxColCount)
                {
                    maxColCount = words.Length;
                }
            }
        }
        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "(*.csv)|*.csv";

            if (ofd.ShowDialog()==DialogResult.OK)
            {
                _filename = ofd.FileName;
                OpenFile(_filename);
            }
        }
        int _rowCount;
        int _colCount;
        int _firstRow;
        int _firstCol;
        int _totalCols;

        /// _inspectionMethod;
        string _filename;
        void SetColumns()
        {
            dataGridViewImport.ColumnCount = _totalCols;
        }
        private void numericUpDownColCount_ValueChanged(object sender, EventArgs e)
        {
           _colCount=(int) numericUpDownColCount.Value;
            _totalCols = _colCount + _firstCol;

        }

        private void numericUpDownFirstCol_ValueChanged(object sender, EventArgs e)
        {
            _firstCol = (int)numericUpDownFirstCol.Value-1;
        }

        private void numericUpDownFirstRow_ValueChanged(object sender, EventArgs e)
        {
            _firstRow = (int)numericUpDownFirstRow.Value-1;
        }

        private void comboBoxMethod_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
    }
}
