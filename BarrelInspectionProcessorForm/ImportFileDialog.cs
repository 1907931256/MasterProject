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
using BarrelLib;
namespace BarrelInspectionProcessorForm
{
    public partial class ImportFileDialog : Form
    {
        public InspDataSet InspDataSet { get; private set; }
        
        public ImportFileDialog(string filename,Barrel barrel)
        {
            InspDataSet = new InspDataSet(barrel,filename);
            InitializeComponent();
            OpenFile(filename);
        }
        private void buttonCancel_Click(object sender, EventArgs e)
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
            var separators = new string[1] { "," };
            foreach(var line in lines)
            {
                
                var words = FileIO.Split(line,separators);
                if(words.Length > maxColCount)
                {
                    maxColCount = words.Length;
                }      
            }
            for(int i=0;i<maxColCount;i++)
            {
                string colname = "column" + i.ToString();
                dataGridViewImport.Columns.Add(colname,colname);
            }
            for(int i=0;i<20;i++)
            {
                var words = FileIO.Split(lines[i],separators);
                dataGridViewImport.Rows.Add(words);                
            }
            _firstCol = 0;
            _firstRow = 0;
            SetColors();
            
        }
        private void SetColors()
        {           
            for(int i=0;i< dataGridViewImport.Rows.Count;i++)
            {
                for (int j = 0; j < dataGridViewImport.ColumnCount;j++)
                {
                    if (i >= _firstRow & j >= _firstCol)
                    {
                        dataGridViewImport.Rows[i].Cells[j].Style.BackColor = Color.AliceBlue;
                    }
                    else
                    {
                        dataGridViewImport.Rows[i].Cells[j].Style.BackColor = Color.OrangeRed;
                    }
                }

            }
            Refresh();
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
       
        private void numericUpDownFirstCol_ValueChanged(object sender, EventArgs e)
        {
            _firstCol = (int)numericUpDownFirstCol.Value;
            SetColors();
        }

        private void numericUpDownFirstRow_ValueChanged(object sender, EventArgs e)
        {
            _firstRow = (int)numericUpDownFirstRow.Value;
            SetColors();
        }

        private void comboBoxMethod_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
    }
}
