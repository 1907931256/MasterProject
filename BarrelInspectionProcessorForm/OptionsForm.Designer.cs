namespace BarrelInspectionProcessorForm
{
    partial class OptionsForm
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.comboBoxColorCode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxScaleFactor = new System.Windows.Forms.TextBox();
            this.checkBoxUncorrectedCSV = new System.Windows.Forms.CheckBox();
            this.tabControlOutputOptions = new System.Windows.Forms.TabControl();
            this.tabPageSaveOpts = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButtonFlat = new System.Windows.Forms.RadioButton();
            this.radioButtonRolled = new System.Windows.Forms.RadioButton();
            this.tabPageFileLocs = new System.Windows.Forms.TabPage();
            this.textBoxMuzzRasterFile = new System.Windows.Forms.TextBox();
            this.checkBoxUseMuzRaster = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonBrowseMuzRaster = new System.Windows.Forms.Button();
            this.textBoxBrchRasterFile = new System.Windows.Forms.TextBox();
            this.textBoxBarrelProfile = new System.Windows.Forms.TextBox();
            this.checkBoxUseBarrelFile = new System.Windows.Forms.CheckBox();
            this.checkBoxUseBrchRaster = new System.Windows.Forms.CheckBox();
            this.buttonBrowseBarrelProfile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonBrowseBrchRaster = new System.Windows.Forms.Button();
            this.tabPageDataOptions = new System.Windows.Forms.TabPage();
            this.tabPageScanOptions = new System.Windows.Forms.TabPage();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControlOutputOptions.SuspendLayout();
            this.tabPageSaveOpts.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPageFileLocs.SuspendLayout();
            this.tabPageDataOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(370, 323);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(67, 38);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(296, 323);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(70, 38);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // comboBoxColorCode
            // 
            this.comboBoxColorCode.FormattingEnabled = true;
            this.comboBoxColorCode.Items.AddRange(new object[] {
            "MONO",
            "GREEN_RED",
            "RAINBOW",
            "MONO_RED",
            "CONTOURS"});
            this.comboBoxColorCode.Location = new System.Drawing.Point(77, 99);
            this.comboBoxColorCode.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxColorCode.Name = "comboBoxColorCode";
            this.comboBoxColorCode.Size = new System.Drawing.Size(157, 21);
            this.comboBoxColorCode.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 102);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Color Code";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 129);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Surface Scaling";
            // 
            // textBoxScaleFactor
            // 
            this.textBoxScaleFactor.Location = new System.Drawing.Point(103, 126);
            this.textBoxScaleFactor.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxScaleFactor.Name = "textBoxScaleFactor";
            this.textBoxScaleFactor.Size = new System.Drawing.Size(76, 20);
            this.textBoxScaleFactor.TabIndex = 11;
            this.textBoxScaleFactor.Text = "10";
            // 
            // checkBoxUncorrectedCSV
            // 
            this.checkBoxUncorrectedCSV.AutoSize = true;
            this.checkBoxUncorrectedCSV.Location = new System.Drawing.Point(14, 10);
            this.checkBoxUncorrectedCSV.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxUncorrectedCSV.Name = "checkBoxUncorrectedCSV";
            this.checkBoxUncorrectedCSV.Size = new System.Drawing.Size(228, 17);
            this.checkBoxUncorrectedCSV.TabIndex = 15;
            this.checkBoxUncorrectedCSV.Text = "Save Uncorrected Data in Output CSV File";
            this.checkBoxUncorrectedCSV.UseVisualStyleBackColor = true;
            // 
            // tabControlOutputOptions
            // 
            this.tabControlOutputOptions.Controls.Add(this.tabPageSaveOpts);
            this.tabControlOutputOptions.Controls.Add(this.tabPageFileLocs);
            this.tabControlOutputOptions.Controls.Add(this.tabPageDataOptions);
            this.tabControlOutputOptions.Controls.Add(this.tabPageScanOptions);
            this.tabControlOutputOptions.Location = new System.Drawing.Point(9, 10);
            this.tabControlOutputOptions.Margin = new System.Windows.Forms.Padding(2);
            this.tabControlOutputOptions.Name = "tabControlOutputOptions";
            this.tabControlOutputOptions.SelectedIndex = 0;
            this.tabControlOutputOptions.Size = new System.Drawing.Size(433, 309);
            this.tabControlOutputOptions.TabIndex = 16;
            // 
            // tabPageSaveOpts
            // 
            this.tabPageSaveOpts.Controls.Add(this.panel1);
            this.tabPageSaveOpts.Controls.Add(this.checkBoxUncorrectedCSV);
            this.tabPageSaveOpts.Controls.Add(this.textBoxScaleFactor);
            this.tabPageSaveOpts.Controls.Add(this.label2);
            this.tabPageSaveOpts.Controls.Add(this.label1);
            this.tabPageSaveOpts.Controls.Add(this.comboBoxColorCode);
            this.tabPageSaveOpts.Location = new System.Drawing.Point(4, 22);
            this.tabPageSaveOpts.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageSaveOpts.Name = "tabPageSaveOpts";
            this.tabPageSaveOpts.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageSaveOpts.Size = new System.Drawing.Size(425, 283);
            this.tabPageSaveOpts.TabIndex = 0;
            this.tabPageSaveOpts.Text = "Save Options";
            this.tabPageSaveOpts.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButtonFlat);
            this.panel1.Controls.Add(this.radioButtonRolled);
            this.panel1.Location = new System.Drawing.Point(14, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(164, 58);
            this.panel1.TabIndex = 16;
            // 
            // radioButtonFlat
            // 
            this.radioButtonFlat.AutoSize = true;
            this.radioButtonFlat.Location = new System.Drawing.Point(3, 26);
            this.radioButtonFlat.Name = "radioButtonFlat";
            this.radioButtonFlat.Size = new System.Drawing.Size(141, 17);
            this.radioButtonFlat.TabIndex = 1;
            this.radioButtonFlat.TabStop = true;
            this.radioButtonFlat.Text = "Save 3D as Flat Surface";
            this.radioButtonFlat.UseVisualStyleBackColor = true;
            // 
            // radioButtonRolled
            // 
            this.radioButtonRolled.AutoSize = true;
            this.radioButtonRolled.Location = new System.Drawing.Point(3, 3);
            this.radioButtonRolled.Name = "radioButtonRolled";
            this.radioButtonRolled.Size = new System.Drawing.Size(154, 17);
            this.radioButtonRolled.TabIndex = 0;
            this.radioButtonRolled.TabStop = true;
            this.radioButtonRolled.Text = "Save 3D as Rolled Surface";
            this.radioButtonRolled.UseVisualStyleBackColor = true;
            // 
            // tabPageFileLocs
            // 
            this.tabPageFileLocs.Controls.Add(this.textBoxMuzzRasterFile);
            this.tabPageFileLocs.Controls.Add(this.checkBoxUseMuzRaster);
            this.tabPageFileLocs.Controls.Add(this.label6);
            this.tabPageFileLocs.Controls.Add(this.buttonBrowseMuzRaster);
            this.tabPageFileLocs.Controls.Add(this.textBoxBrchRasterFile);
            this.tabPageFileLocs.Controls.Add(this.textBoxBarrelProfile);
            this.tabPageFileLocs.Controls.Add(this.checkBoxUseBarrelFile);
            this.tabPageFileLocs.Controls.Add(this.checkBoxUseBrchRaster);
            this.tabPageFileLocs.Controls.Add(this.buttonBrowseBarrelProfile);
            this.tabPageFileLocs.Controls.Add(this.label4);
            this.tabPageFileLocs.Controls.Add(this.label3);
            this.tabPageFileLocs.Controls.Add(this.buttonBrowseBrchRaster);
            this.tabPageFileLocs.Location = new System.Drawing.Point(4, 22);
            this.tabPageFileLocs.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageFileLocs.Name = "tabPageFileLocs";
            this.tabPageFileLocs.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageFileLocs.Size = new System.Drawing.Size(425, 283);
            this.tabPageFileLocs.TabIndex = 1;
            this.tabPageFileLocs.Text = "File Locations";
            this.tabPageFileLocs.UseVisualStyleBackColor = true;
            // 
            // textBoxMuzzRasterFile
            // 
            this.textBoxMuzzRasterFile.Location = new System.Drawing.Point(148, 100);
            this.textBoxMuzzRasterFile.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxMuzzRasterFile.Name = "textBoxMuzzRasterFile";
            this.textBoxMuzzRasterFile.Size = new System.Drawing.Size(213, 20);
            this.textBoxMuzzRasterFile.TabIndex = 14;
            // 
            // checkBoxUseMuzRaster
            // 
            this.checkBoxUseMuzRaster.AutoSize = true;
            this.checkBoxUseMuzRaster.Location = new System.Drawing.Point(148, 123);
            this.checkBoxUseMuzRaster.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxUseMuzRaster.Name = "checkBoxUseMuzRaster";
            this.checkBoxUseMuzRaster.Size = new System.Drawing.Size(135, 17);
            this.checkBoxUseMuzRaster.TabIndex = 13;
            this.checkBoxUseMuzRaster.Text = "Use Default Raster File";
            this.checkBoxUseMuzRaster.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 102);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Default Muzzle Raster File";
            // 
            // buttonBrowseMuzRaster
            // 
            this.buttonBrowseMuzRaster.Location = new System.Drawing.Point(365, 100);
            this.buttonBrowseMuzRaster.Margin = new System.Windows.Forms.Padding(2);
            this.buttonBrowseMuzRaster.Name = "buttonBrowseMuzRaster";
            this.buttonBrowseMuzRaster.Size = new System.Drawing.Size(56, 19);
            this.buttonBrowseMuzRaster.TabIndex = 11;
            this.buttonBrowseMuzRaster.Text = "Browse...";
            this.buttonBrowseMuzRaster.UseVisualStyleBackColor = true;
            this.buttonBrowseMuzRaster.Click += new System.EventHandler(this.buttonBrowseMuzRaster_Click);
            // 
            // textBoxBrchRasterFile
            // 
            this.textBoxBrchRasterFile.Location = new System.Drawing.Point(148, 53);
            this.textBoxBrchRasterFile.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxBrchRasterFile.Name = "textBoxBrchRasterFile";
            this.textBoxBrchRasterFile.Size = new System.Drawing.Size(213, 20);
            this.textBoxBrchRasterFile.TabIndex = 9;
            // 
            // textBoxBarrelProfile
            // 
            this.textBoxBarrelProfile.Location = new System.Drawing.Point(148, 152);
            this.textBoxBarrelProfile.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxBarrelProfile.Name = "textBoxBarrelProfile";
            this.textBoxBarrelProfile.Size = new System.Drawing.Size(213, 20);
            this.textBoxBarrelProfile.TabIndex = 8;
            // 
            // checkBoxUseBarrelFile
            // 
            this.checkBoxUseBarrelFile.AutoSize = true;
            this.checkBoxUseBarrelFile.Location = new System.Drawing.Point(148, 175);
            this.checkBoxUseBarrelFile.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxUseBarrelFile.Name = "checkBoxUseBarrelFile";
            this.checkBoxUseBarrelFile.Size = new System.Drawing.Size(144, 17);
            this.checkBoxUseBarrelFile.TabIndex = 5;
            this.checkBoxUseBarrelFile.Text = "Use Default Barrel Profile";
            this.checkBoxUseBarrelFile.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseBrchRaster
            // 
            this.checkBoxUseBrchRaster.AutoSize = true;
            this.checkBoxUseBrchRaster.Location = new System.Drawing.Point(148, 76);
            this.checkBoxUseBrchRaster.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxUseBrchRaster.Name = "checkBoxUseBrchRaster";
            this.checkBoxUseBrchRaster.Size = new System.Drawing.Size(135, 17);
            this.checkBoxUseBrchRaster.TabIndex = 4;
            this.checkBoxUseBrchRaster.Text = "Use Default Raster File";
            this.checkBoxUseBrchRaster.UseVisualStyleBackColor = true;
            // 
            // buttonBrowseBarrelProfile
            // 
            this.buttonBrowseBarrelProfile.Location = new System.Drawing.Point(365, 152);
            this.buttonBrowseBarrelProfile.Margin = new System.Windows.Forms.Padding(2);
            this.buttonBrowseBarrelProfile.Name = "buttonBrowseBarrelProfile";
            this.buttonBrowseBarrelProfile.Size = new System.Drawing.Size(56, 19);
            this.buttonBrowseBarrelProfile.TabIndex = 3;
            this.buttonBrowseBarrelProfile.Text = "Browse...";
            this.buttonBrowseBarrelProfile.UseVisualStyleBackColor = true;
            this.buttonBrowseBarrelProfile.Click += new System.EventHandler(this.buttonBrowseBarrelProfile_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 154);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Default Barrel Profile";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 55);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Default Breach Raster File";
            // 
            // buttonBrowseBrchRaster
            // 
            this.buttonBrowseBrchRaster.Location = new System.Drawing.Point(365, 53);
            this.buttonBrowseBrchRaster.Margin = new System.Windows.Forms.Padding(2);
            this.buttonBrowseBrchRaster.Name = "buttonBrowseBrchRaster";
            this.buttonBrowseBrchRaster.Size = new System.Drawing.Size(56, 19);
            this.buttonBrowseBrchRaster.TabIndex = 0;
            this.buttonBrowseBrchRaster.Text = "Browse...";
            this.buttonBrowseBrchRaster.UseVisualStyleBackColor = true;
            this.buttonBrowseBrchRaster.Click += new System.EventHandler(this.buttonBrowseRaster_Click);
            // 
            // tabPageDataOptions
            // 
            this.tabPageDataOptions.Controls.Add(this.button1);
            this.tabPageDataOptions.Controls.Add(this.label5);
            this.tabPageDataOptions.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataOptions.Name = "tabPageDataOptions";
            this.tabPageDataOptions.Size = new System.Drawing.Size(425, 283);
            this.tabPageDataOptions.TabIndex = 2;
            this.tabPageDataOptions.Text = "Data Options";
            this.tabPageDataOptions.UseVisualStyleBackColor = true;
            // 
            // tabPageScanOptions
            // 
            this.tabPageScanOptions.Location = new System.Drawing.Point(4, 22);
            this.tabPageScanOptions.Name = "tabPageScanOptions";
            this.tabPageScanOptions.Size = new System.Drawing.Size(425, 283);
            this.tabPageScanOptions.TabIndex = 3;
            this.tabPageScanOptions.Text = "Scan Options";
            this.tabPageScanOptions.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Plot Color";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(92, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(57, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Select";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 383);
            this.Controls.Add(this.tabControlOutputOptions);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.tabControlOutputOptions.ResumeLayout(false);
            this.tabPageSaveOpts.ResumeLayout(false);
            this.tabPageSaveOpts.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPageFileLocs.ResumeLayout(false);
            this.tabPageFileLocs.PerformLayout();
            this.tabPageDataOptions.ResumeLayout(false);
            this.tabPageDataOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ComboBox comboBoxColorCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxScaleFactor;
        private System.Windows.Forms.CheckBox checkBoxUncorrectedCSV;
        private System.Windows.Forms.TabControl tabControlOutputOptions;
        private System.Windows.Forms.TabPage tabPageSaveOpts;
        private System.Windows.Forms.TabPage tabPageFileLocs;
        private System.Windows.Forms.TextBox textBoxBrchRasterFile;
        private System.Windows.Forms.TextBox textBoxBarrelProfile;
        private System.Windows.Forms.CheckBox checkBoxUseBarrelFile;
        private System.Windows.Forms.CheckBox checkBoxUseBrchRaster;
        private System.Windows.Forms.Button buttonBrowseBarrelProfile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonBrowseBrchRaster;
        private System.Windows.Forms.TextBox textBoxMuzzRasterFile;
        private System.Windows.Forms.CheckBox checkBoxUseMuzRaster;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonBrowseMuzRaster;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButtonFlat;
        private System.Windows.Forms.RadioButton radioButtonRolled;
        private System.Windows.Forms.TabPage tabPageDataOptions;
        private System.Windows.Forms.TabPage tabPageScanOptions;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}