namespace BarrelInspectionProcessorForm
{
    partial class ImportFileDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelFileName = new System.Windows.Forms.Label();
            this.labelFirstColumn = new System.Windows.Forms.Label();
            this.labelFirstRow = new System.Windows.Forms.Label();
            this.dataGridViewImport = new System.Windows.Forms.DataGridView();
            this.panelDataGrid = new System.Windows.Forms.Panel();
            this.comboBoxMethod = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.numericUpDownFirstRow = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownFirstCol = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImport)).BeginInit();
            this.panelDataGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFirstRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFirstCol)).BeginInit();
            this.SuspendLayout();
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.Location = new System.Drawing.Point(12, 9);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(49, 13);
            this.labelFileName.TabIndex = 1;
            this.labelFileName.Text = "Filename";
            // 
            // labelFirstColumn
            // 
            this.labelFirstColumn.AutoSize = true;
            this.labelFirstColumn.Location = new System.Drawing.Point(9, 58);
            this.labelFirstColumn.Name = "labelFirstColumn";
            this.labelFirstColumn.Size = new System.Drawing.Size(64, 13);
            this.labelFirstColumn.TabIndex = 2;
            this.labelFirstColumn.Text = "First Column";
            // 
            // labelFirstRow
            // 
            this.labelFirstRow.AutoSize = true;
            this.labelFirstRow.Location = new System.Drawing.Point(9, 32);
            this.labelFirstRow.Name = "labelFirstRow";
            this.labelFirstRow.Size = new System.Drawing.Size(51, 13);
            this.labelFirstRow.TabIndex = 4;
            this.labelFirstRow.Text = "First Row";
            // 
            // dataGridViewImport
            // 
            this.dataGridViewImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewImport.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewImport.Name = "dataGridViewImport";
            this.dataGridViewImport.Size = new System.Drawing.Size(536, 273);
            this.dataGridViewImport.TabIndex = 7;
            // 
            // panelDataGrid
            // 
            this.panelDataGrid.Controls.Add(this.dataGridViewImport);
            this.panelDataGrid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelDataGrid.Location = new System.Drawing.Point(0, 174);
            this.panelDataGrid.Name = "panelDataGrid";
            this.panelDataGrid.Size = new System.Drawing.Size(536, 273);
            this.panelDataGrid.TabIndex = 8;
            // 
            // comboBoxMethod
            // 
            this.comboBoxMethod.FormattingEnabled = true;
            this.comboBoxMethod.Items.AddRange(new object[] {
            "Ring",
            "Spiral",
            "Axial",
            "SingleLineXY"});
            this.comboBoxMethod.Location = new System.Drawing.Point(98, 107);
            this.comboBoxMethod.Name = "comboBoxMethod";
            this.comboBoxMethod.Size = new System.Drawing.Size(100, 21);
            this.comboBoxMethod.TabIndex = 9;
            this.comboBoxMethod.SelectedIndexChanged += new System.EventHandler(this.comboBoxMethod_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Inspection Type";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(368, 145);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 11;
            this.buttonOK.Text = "Import";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(449, 145);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 12;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(287, 145);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 13;
            this.buttonBrowse.Text = "Browse...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // numericUpDownFirstRow
            // 
            this.numericUpDownFirstRow.Location = new System.Drawing.Point(98, 30);
            this.numericUpDownFirstRow.Name = "numericUpDownFirstRow";
            this.numericUpDownFirstRow.Size = new System.Drawing.Size(52, 20);
            this.numericUpDownFirstRow.TabIndex = 14;
            this.numericUpDownFirstRow.ValueChanged += new System.EventHandler(this.numericUpDownFirstRow_ValueChanged);
            // 
            // numericUpDownFirstCol
            // 
            this.numericUpDownFirstCol.Location = new System.Drawing.Point(98, 56);
            this.numericUpDownFirstCol.Name = "numericUpDownFirstCol";
            this.numericUpDownFirstCol.Size = new System.Drawing.Size(52, 20);
            this.numericUpDownFirstCol.TabIndex = 14;
            this.numericUpDownFirstCol.ValueChanged += new System.EventHandler(this.numericUpDownFirstCol_ValueChanged);
            // 
            // ImportFileDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 447);
            this.Controls.Add(this.numericUpDownFirstCol);
            this.Controls.Add(this.numericUpDownFirstRow);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxMethod);
            this.Controls.Add(this.panelDataGrid);
            this.Controls.Add(this.labelFirstRow);
            this.Controls.Add(this.labelFirstColumn);
            this.Controls.Add(this.labelFileName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportFileDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Import File";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImport)).EndInit();
            this.panelDataGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFirstRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFirstCol)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.Label labelFirstColumn;
        private System.Windows.Forms.Label labelFirstRow;
        private System.Windows.Forms.DataGridView dataGridViewImport;
        private System.Windows.Forms.Panel panelDataGrid;
        private System.Windows.Forms.ComboBox comboBoxMethod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.NumericUpDown numericUpDownFirstRow;
        private System.Windows.Forms.NumericUpDown numericUpDownFirstCol;
    }
}