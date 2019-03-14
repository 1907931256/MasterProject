using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileIOLib;
namespace BarrelInspectionProcessorForm
{
    public partial class LogViewForm : Form
    {
        public LogViewForm()
        {
            InitializeComponent();             

        }
        public bool ClearLog { get; private set; }
        public void SetLines(List<string> lines)
        {
            textBoxLog.Lines = lines.ToArray();
            Refresh();
        }
        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearLog = true;
            textBoxLog.Clear();
            Refresh();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.Filter = "(*.txt)|*.txt";
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                FileIO.Save(textBoxLog.Lines, sfd.FileName);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
