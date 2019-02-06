namespace BarrelInspectionProcessorForm
{
    partial class MainInspectionForm
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
                if(_bitmap!=null)
                    _bitmap.Dispose();
                if(_bitmap!=null)
                    _blackPen.Dispose();
                if(_bluePen!=null)
                    _bluePen.Dispose();
                if(_redPen!=null)
                    _redPen.Dispose();
                if(_greenPen!=null)
                    _greenPen.Dispose();
                if(_imgBrush!=null)
                    _imgBrush.Dispose();
                if (_orangePen != null)
                    _orangePen.Dispose();
                if (_lightGreenPen != null)
                    _lightGreenPen.Dispose(); 
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainInspectionForm));
            this.labelInputFIlename = new System.Windows.Forms.Label();
            this.buttonProcessFile = new System.Windows.Forms.Button();
            this.textBoxStartPosX = new System.Windows.Forms.TextBox();
            this.labelStartPos = new System.Windows.Forms.Label();
            this.comboBoxProbeDirection = new System.Windows.Forms.ComboBox();
            this.radioButtonAngleInc = new System.Windows.Forms.RadioButton();
            this.radioButtonPtsperRev = new System.Windows.Forms.RadioButton();
            this.comboBoxMethod = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelRingRevs = new System.Windows.Forms.Label();
            this.labelPitch = new System.Windows.Forms.Label();
            this.labelEndPos = new System.Windows.Forms.Label();
            this.labelMethod = new System.Windows.Forms.Label();
            this.textBoxProbeCount = new System.Windows.Forms.TextBox();
            this.textBoxRingRevs = new System.Windows.Forms.TextBox();
            this.textBoxPitch = new System.Windows.Forms.TextBox();
            this.textBoxAngleInc = new System.Windows.Forms.TextBox();
            this.textBoxPtsPerRev = new System.Windows.Forms.TextBox();
            this.textBoxEndPosA = new System.Windows.Forms.TextBox();
            this.textBoxStartPosA = new System.Windows.Forms.TextBox();
            this.textBoxEndPosX = new System.Windows.Forms.TextBox();
            this.comboBoxBarrel = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelDxMeasured = new System.Windows.Forms.Label();
            this.labelDyMeasured = new System.Windows.Forms.Label();
            this.labelRadiusMeasured = new System.Windows.Forms.Label();
            this.labelZPosition = new System.Windows.Forms.Label();
            this.labelXPosition = new System.Windows.Forms.Label();
            this.labelYPosition = new System.Windows.Forms.Label();
            this.buttonSetRadius = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxCurrentRadius = new System.Windows.Forms.TextBox();
            this.labelInputValue = new System.Windows.Forms.Label();
            this.textBoxKnownRadius = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.radioButtonViewProcessed = new System.Windows.Forms.RadioButton();
            this.radioButtonViewRaw = new System.Windows.Forms.RadioButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openRawDataFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rawFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processedCSVFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProcessedFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProfileDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDepthDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDXFProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.save3DSurfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.save3DSTLSurfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dViewImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAxialProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataAquisitionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToDAQToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectFromDAQToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelStatus = new System.Windows.Forms.Label();
            this.buttonGetAveAngle = new System.Windows.Forms.Button();
            this.checkBoxAngleCorrect = new System.Windows.Forms.CheckBox();
            this.textBoxSerialN = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxCurrentPasses = new System.Windows.Forms.TextBox();
            this.labelCurrentPasses = new System.Windows.Forms.Label();
            this.textBoxTotalPasses = new System.Windows.Forms.TextBox();
            this.labelTotalPasses = new System.Windows.Forms.Label();
            this.toolStripButtonCursor = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonFileOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFileSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonLength = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSetKnownRadius = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonGrooveMidpoint = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRotate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMirror = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonWinData = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFitToCircle = new System.Windows.Forms.ToolStripButton();
            this.tabControlParams = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.labelCalStatus = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxRingCal = new System.Windows.Forms.TextBox();
            this.labelNomDiam = new System.Windows.Forms.Label();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.comboBoxDiameterType = new System.Windows.Forms.ComboBox();
            this.textBoxNomDiam = new System.Windows.Forms.TextBox();
            this.labelNotes = new System.Windows.Forms.Label();
            this.textBoxMiscManNotes = new System.Windows.Forms.TextBox();
            this.labelRoundsFired = new System.Windows.Forms.Label();
            this.textBoxRoundsFired = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBoxManStep = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBoxProbeType = new System.Windows.Forms.ComboBox();
            this.textBoxGrooveList = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxProbeSpacing = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.buttonMeasureDepths = new System.Windows.Forms.Button();
            this.buttonCorrectMidpoint = new System.Windows.Forms.Button();
            this.buttonBuildProfile = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxProbePhaseDeg = new System.Windows.Forms.TextBox();
            this.checkBoxUseFilename = new System.Windows.Forms.CheckBox();
            this.tabControlOutput = new System.Windows.Forms.TabControl();
            this.tabPageGraph = new System.Windows.Forms.TabPage();
            this.tabPageData = new System.Windows.Forms.TabPage();
            this.textBoxDataOut = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.userControl11 = new BarrelInspectionProcessorForm.UserControl1();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.progressBarProcessing = new System.Windows.Forms.ProgressBar();
            this.OpenDXFFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabControlParams.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabControlOutput.SuspendLayout();
            this.tabPageGraph.SuspendLayout();
            this.tabPageData.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelInputFIlename
            // 
            this.labelInputFIlename.AutoSize = true;
            this.labelInputFIlename.Location = new System.Drawing.Point(5, 3);
            this.labelInputFIlename.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelInputFIlename.Name = "labelInputFIlename";
            this.labelInputFIlename.Size = new System.Drawing.Size(49, 13);
            this.labelInputFIlename.TabIndex = 0;
            this.labelInputFIlename.Text = "Filename";
            // 
            // buttonProcessFile
            // 
            this.buttonProcessFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonProcessFile.Location = new System.Drawing.Point(7, 534);
            this.buttonProcessFile.Margin = new System.Windows.Forms.Padding(2);
            this.buttonProcessFile.Name = "buttonProcessFile";
            this.buttonProcessFile.Size = new System.Drawing.Size(112, 40);
            this.buttonProcessFile.TabIndex = 3;
            this.buttonProcessFile.Text = "Process File";
            this.buttonProcessFile.UseVisualStyleBackColor = true;
            this.buttonProcessFile.Click += new System.EventHandler(this.ButtonProcessFile_Click);
            // 
            // textBoxStartPosX
            // 
            this.textBoxStartPosX.Location = new System.Drawing.Point(145, 60);
            this.textBoxStartPosX.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxStartPosX.Name = "textBoxStartPosX";
            this.textBoxStartPosX.Size = new System.Drawing.Size(35, 20);
            this.textBoxStartPosX.TabIndex = 0;
            this.textBoxStartPosX.Text = "5";
            // 
            // labelStartPos
            // 
            this.labelStartPos.AutoSize = true;
            this.labelStartPos.Location = new System.Drawing.Point(49, 63);
            this.labelStartPos.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelStartPos.Name = "labelStartPos";
            this.labelStartPos.Size = new System.Drawing.Size(92, 13);
            this.labelStartPos.TabIndex = 1;
            this.labelStartPos.Text = "Start Position (x,a)";
            // 
            // comboBoxProbeDirection
            // 
            this.comboBoxProbeDirection.FormattingEnabled = true;
            this.comboBoxProbeDirection.Items.AddRange(new object[] {
            "BORE I.D.",
            "ROD O.D."});
            this.comboBoxProbeDirection.Location = new System.Drawing.Point(100, 249);
            this.comboBoxProbeDirection.Name = "comboBoxProbeDirection";
            this.comboBoxProbeDirection.Size = new System.Drawing.Size(82, 21);
            this.comboBoxProbeDirection.TabIndex = 7;
            this.comboBoxProbeDirection.SelectedIndexChanged += new System.EventHandler(this.ComboBoxProbeDirection_SelectedIndexChanged);
            // 
            // radioButtonAngleInc
            // 
            this.radioButtonAngleInc.AutoSize = true;
            this.radioButtonAngleInc.Location = new System.Drawing.Point(12, 130);
            this.radioButtonAngleInc.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonAngleInc.Name = "radioButtonAngleInc";
            this.radioButtonAngleInc.Size = new System.Drawing.Size(129, 17);
            this.radioButtonAngleInc.TabIndex = 4;
            this.radioButtonAngleInc.TabStop = true;
            this.radioButtonAngleInc.Text = "Angle Increment (deg)";
            this.radioButtonAngleInc.UseVisualStyleBackColor = true;
            this.radioButtonAngleInc.CheckedChanged += new System.EventHandler(this.RadioButtonAngleInc_CheckedChanged);
            // 
            // radioButtonPtsperRev
            // 
            this.radioButtonPtsperRev.AutoSize = true;
            this.radioButtonPtsperRev.Location = new System.Drawing.Point(15, 106);
            this.radioButtonPtsperRev.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonPtsperRev.Name = "radioButtonPtsperRev";
            this.radioButtonPtsperRev.Size = new System.Drawing.Size(126, 17);
            this.radioButtonPtsperRev.TabIndex = 4;
            this.radioButtonPtsperRev.TabStop = true;
            this.radioButtonPtsperRev.Text = "Points per Revolution";
            this.radioButtonPtsperRev.UseVisualStyleBackColor = true;
            this.radioButtonPtsperRev.CheckedChanged += new System.EventHandler(this.RadioButtonPtsperRev_CheckedChanged);
            // 
            // comboBoxMethod
            // 
            this.comboBoxMethod.FormattingEnabled = true;
            this.comboBoxMethod.Items.AddRange(new object[] {
            "RING",
            "SPIRAL",
            "AXIAL",
            "LAND",
            "GROOVE",
            "CAL",
            "FLATPLATELINE",
            "FLATPLATEGRID"});
            this.comboBoxMethod.Location = new System.Drawing.Point(101, 35);
            this.comboBoxMethod.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxMethod.Name = "comboBoxMethod";
            this.comboBoxMethod.Size = new System.Drawing.Size(139, 21);
            this.comboBoxMethod.TabIndex = 3;
            this.comboBoxMethod.SelectedIndexChanged += new System.EventHandler(this.ComboBoxMethod_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 253);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Probe Direction";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 208);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Probe Count";
            // 
            // labelRingRevs
            // 
            this.labelRingRevs.AutoSize = true;
            this.labelRingRevs.Location = new System.Drawing.Point(9, 180);
            this.labelRingRevs.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRingRevs.Name = "labelRingRevs";
            this.labelRingRevs.Size = new System.Drawing.Size(88, 13);
            this.labelRingRevs.TabIndex = 1;
            this.labelRingRevs.Text = "Ring Revolutions";
            // 
            // labelPitch
            // 
            this.labelPitch.AutoSize = true;
            this.labelPitch.Location = new System.Drawing.Point(49, 156);
            this.labelPitch.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPitch.Name = "labelPitch";
            this.labelPitch.Size = new System.Drawing.Size(48, 13);
            this.labelPitch.TabIndex = 1;
            this.labelPitch.Text = "Pitch (in)";
            // 
            // labelEndPos
            // 
            this.labelEndPos.AutoSize = true;
            this.labelEndPos.Location = new System.Drawing.Point(52, 85);
            this.labelEndPos.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelEndPos.Name = "labelEndPos";
            this.labelEndPos.Size = new System.Drawing.Size(89, 13);
            this.labelEndPos.TabIndex = 1;
            this.labelEndPos.Text = "End Position (x,a)";
            // 
            // labelMethod
            // 
            this.labelMethod.AutoSize = true;
            this.labelMethod.Location = new System.Drawing.Point(31, 38);
            this.labelMethod.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMethod.Name = "labelMethod";
            this.labelMethod.Size = new System.Drawing.Size(67, 13);
            this.labelMethod.TabIndex = 1;
            this.labelMethod.Text = "Scan Format";
            this.labelMethod.Click += new System.EventHandler(this.labelMethod_Click);
            // 
            // textBoxProbeCount
            // 
            this.textBoxProbeCount.Location = new System.Drawing.Point(99, 204);
            this.textBoxProbeCount.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxProbeCount.Name = "textBoxProbeCount";
            this.textBoxProbeCount.Size = new System.Drawing.Size(39, 20);
            this.textBoxProbeCount.TabIndex = 0;
            this.textBoxProbeCount.Text = "2";
            // 
            // textBoxRingRevs
            // 
            this.textBoxRingRevs.Location = new System.Drawing.Point(101, 177);
            this.textBoxRingRevs.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxRingRevs.Name = "textBoxRingRevs";
            this.textBoxRingRevs.Size = new System.Drawing.Size(39, 20);
            this.textBoxRingRevs.TabIndex = 0;
            this.textBoxRingRevs.Text = "2";
            // 
            // textBoxPitch
            // 
            this.textBoxPitch.Location = new System.Drawing.Point(101, 154);
            this.textBoxPitch.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPitch.Name = "textBoxPitch";
            this.textBoxPitch.Size = new System.Drawing.Size(82, 20);
            this.textBoxPitch.TabIndex = 0;
            // 
            // textBoxAngleInc
            // 
            this.textBoxAngleInc.Location = new System.Drawing.Point(145, 129);
            this.textBoxAngleInc.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxAngleInc.Name = "textBoxAngleInc";
            this.textBoxAngleInc.Size = new System.Drawing.Size(82, 20);
            this.textBoxAngleInc.TabIndex = 0;
            // 
            // textBoxPtsPerRev
            // 
            this.textBoxPtsPerRev.Location = new System.Drawing.Point(145, 106);
            this.textBoxPtsPerRev.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPtsPerRev.Name = "textBoxPtsPerRev";
            this.textBoxPtsPerRev.Size = new System.Drawing.Size(35, 20);
            this.textBoxPtsPerRev.TabIndex = 0;
            this.textBoxPtsPerRev.Text = "8333";
            // 
            // textBoxEndPosA
            // 
            this.textBoxEndPosA.Location = new System.Drawing.Point(184, 84);
            this.textBoxEndPosA.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEndPosA.Name = "textBoxEndPosA";
            this.textBoxEndPosA.Size = new System.Drawing.Size(53, 20);
            this.textBoxEndPosA.TabIndex = 0;
            this.textBoxEndPosA.Text = "720";
            // 
            // textBoxStartPosA
            // 
            this.textBoxStartPosA.Location = new System.Drawing.Point(184, 60);
            this.textBoxStartPosA.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxStartPosA.Name = "textBoxStartPosA";
            this.textBoxStartPosA.Size = new System.Drawing.Size(53, 20);
            this.textBoxStartPosA.TabIndex = 0;
            this.textBoxStartPosA.Text = "0";
            // 
            // textBoxEndPosX
            // 
            this.textBoxEndPosX.Location = new System.Drawing.Point(145, 84);
            this.textBoxEndPosX.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEndPosX.Name = "textBoxEndPosX";
            this.textBoxEndPosX.Size = new System.Drawing.Size(35, 20);
            this.textBoxEndPosX.TabIndex = 0;
            this.textBoxEndPosX.Text = "5";
            // 
            // comboBoxBarrel
            // 
            this.comboBoxBarrel.FormattingEnabled = true;
            this.comboBoxBarrel.Items.AddRange(new object[] {
            "M2_50_Cal",
            "M242_25mm",
            "M284_155mm",
            "M240_762mm"});
            this.comboBoxBarrel.Location = new System.Drawing.Point(72, 17);
            this.comboBoxBarrel.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxBarrel.Name = "comboBoxBarrel";
            this.comboBoxBarrel.Size = new System.Drawing.Size(92, 21);
            this.comboBoxBarrel.TabIndex = 7;
            this.comboBoxBarrel.SelectedIndexChanged += new System.EventHandler(this.ComboBoxBarrel_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Barrel Type";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(2, 2);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(500, 390);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseClick);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseUp);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelDxMeasured);
            this.panel2.Controls.Add(this.labelDyMeasured);
            this.panel2.Controls.Add(this.labelRadiusMeasured);
            this.panel2.Controls.Add(this.labelZPosition);
            this.panel2.Controls.Add(this.labelXPosition);
            this.panel2.Controls.Add(this.labelYPosition);
            this.panel2.Location = new System.Drawing.Point(7, 181);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(173, 310);
            this.panel2.TabIndex = 11;
            // 
            // labelDxMeasured
            // 
            this.labelDxMeasured.AutoSize = true;
            this.labelDxMeasured.Location = new System.Drawing.Point(7, 85);
            this.labelDxMeasured.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDxMeasured.Name = "labelDxMeasured";
            this.labelDxMeasured.Size = new System.Drawing.Size(105, 13);
            this.labelDxMeasured.TabIndex = 2;
            this.labelDxMeasured.Text = "Dx: 0.000000 inches";
            // 
            // labelDyMeasured
            // 
            this.labelDyMeasured.AutoSize = true;
            this.labelDyMeasured.Location = new System.Drawing.Point(7, 110);
            this.labelDyMeasured.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDyMeasured.Name = "labelDyMeasured";
            this.labelDyMeasured.Size = new System.Drawing.Size(105, 13);
            this.labelDyMeasured.TabIndex = 1;
            this.labelDyMeasured.Text = "Dy: 0.000000 inches";
            // 
            // labelRadiusMeasured
            // 
            this.labelRadiusMeasured.AutoSize = true;
            this.labelRadiusMeasured.Location = new System.Drawing.Point(7, 135);
            this.labelRadiusMeasured.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRadiusMeasured.Name = "labelRadiusMeasured";
            this.labelRadiusMeasured.Size = new System.Drawing.Size(125, 13);
            this.labelRadiusMeasured.TabIndex = 0;
            this.labelRadiusMeasured.Text = "Radius: 0.000000 inches";
            // 
            // labelZPosition
            // 
            this.labelZPosition.AutoSize = true;
            this.labelZPosition.Location = new System.Drawing.Point(7, 60);
            this.labelZPosition.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelZPosition.Name = "labelZPosition";
            this.labelZPosition.Size = new System.Drawing.Size(114, 13);
            this.labelZPosition.TabIndex = 0;
            this.labelZPosition.Text = "Axial: 0.000000 inches";
            // 
            // labelXPosition
            // 
            this.labelXPosition.AutoSize = true;
            this.labelXPosition.Location = new System.Drawing.Point(7, 35);
            this.labelXPosition.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelXPosition.Name = "labelXPosition";
            this.labelXPosition.Size = new System.Drawing.Size(111, 13);
            this.labelXPosition.TabIndex = 0;
            this.labelXPosition.Text = "Angle: 0.000000 degs";
            // 
            // labelYPosition
            // 
            this.labelYPosition.AutoSize = true;
            this.labelYPosition.Location = new System.Drawing.Point(7, 10);
            this.labelYPosition.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelYPosition.Name = "labelYPosition";
            this.labelYPosition.Size = new System.Drawing.Size(100, 13);
            this.labelYPosition.TabIndex = 0;
            this.labelYPosition.Text = "R: 0.000000 inches";
            // 
            // buttonSetRadius
            // 
            this.buttonSetRadius.Location = new System.Drawing.Point(7, 213);
            this.buttonSetRadius.Name = "buttonSetRadius";
            this.buttonSetRadius.Size = new System.Drawing.Size(115, 45);
            this.buttonSetRadius.TabIndex = 29;
            this.buttonSetRadius.Text = "Set Radius";
            this.buttonSetRadius.UseVisualStyleBackColor = true;
            this.buttonSetRadius.Click += new System.EventHandler(this.ButtonSetRadius_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(165, 228);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Current R(in):";
            // 
            // textBoxCurrentRadius
            // 
            this.textBoxCurrentRadius.Location = new System.Drawing.Point(156, 244);
            this.textBoxCurrentRadius.Name = "textBoxCurrentRadius";
            this.textBoxCurrentRadius.ReadOnly = true;
            this.textBoxCurrentRadius.Size = new System.Drawing.Size(78, 20);
            this.textBoxCurrentRadius.TabIndex = 27;
            // 
            // labelInputValue
            // 
            this.labelInputValue.AutoSize = true;
            this.labelInputValue.Location = new System.Drawing.Point(166, 180);
            this.labelInputValue.Name = "labelInputValue";
            this.labelInputValue.Size = new System.Drawing.Size(68, 13);
            this.labelInputValue.TabIndex = 26;
            this.labelInputValue.Text = "Known R(in):";
            // 
            // textBoxKnownRadius
            // 
            this.textBoxKnownRadius.Location = new System.Drawing.Point(156, 196);
            this.textBoxKnownRadius.Name = "textBoxKnownRadius";
            this.textBoxKnownRadius.Size = new System.Drawing.Size(78, 20);
            this.textBoxKnownRadius.TabIndex = 25;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.radioButtonViewProcessed);
            this.panel3.Controls.Add(this.radioButtonViewRaw);
            this.panel3.Location = new System.Drawing.Point(7, 59);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(114, 57);
            this.panel3.TabIndex = 12;
            // 
            // radioButtonViewProcessed
            // 
            this.radioButtonViewProcessed.AutoSize = true;
            this.radioButtonViewProcessed.Location = new System.Drawing.Point(6, 32);
            this.radioButtonViewProcessed.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonViewProcessed.Name = "radioButtonViewProcessed";
            this.radioButtonViewProcessed.Size = new System.Drawing.Size(101, 17);
            this.radioButtonViewProcessed.TabIndex = 1;
            this.radioButtonViewProcessed.TabStop = true;
            this.radioButtonViewProcessed.Text = "View Processed";
            this.radioButtonViewProcessed.UseVisualStyleBackColor = true;
            this.radioButtonViewProcessed.CheckedChanged += new System.EventHandler(this.RadioButtonViewProcessed_CheckedChanged);
            // 
            // radioButtonViewRaw
            // 
            this.radioButtonViewRaw.AutoSize = true;
            this.radioButtonViewRaw.Checked = true;
            this.radioButtonViewRaw.Location = new System.Drawing.Point(6, 11);
            this.radioButtonViewRaw.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonViewRaw.Name = "radioButtonViewRaw";
            this.radioButtonViewRaw.Size = new System.Drawing.Size(73, 17);
            this.radioButtonViewRaw.TabIndex = 0;
            this.radioButtonViewRaw.TabStop = true;
            this.radioButtonViewRaw.Text = "View Raw";
            this.radioButtonViewRaw.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.dataAquisitionToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1120, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openRawDataFileToolStripMenuItem,
            this.saveProcessedFileToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openRawDataFileToolStripMenuItem
            // 
            this.openRawDataFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rawFileToolStripMenuItem,
            this.processedCSVFileToolStripMenuItem,
            this.OpenDXFFileToolStripMenuItem});
            this.openRawDataFileToolStripMenuItem.Name = "openRawDataFileToolStripMenuItem";
            this.openRawDataFileToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.openRawDataFileToolStripMenuItem.Text = "Open ";
            // 
            // rawFileToolStripMenuItem
            // 
            this.rawFileToolStripMenuItem.Name = "rawFileToolStripMenuItem";
            this.rawFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.rawFileToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.rawFileToolStripMenuItem.Text = "Raw File";
            this.rawFileToolStripMenuItem.Click += new System.EventHandler(this.rawFileToolStripMenuItem_Click);
            // 
            // processedCSVFileToolStripMenuItem
            // 
            this.processedCSVFileToolStripMenuItem.Name = "processedCSVFileToolStripMenuItem";
            this.processedCSVFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.processedCSVFileToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.processedCSVFileToolStripMenuItem.Text = "Processed CSV file";
            this.processedCSVFileToolStripMenuItem.Click += new System.EventHandler(this.processedCSVFileToolStripMenuItem_Click);
            // 
            // saveProcessedFileToolStripMenuItem
            // 
            this.saveProcessedFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveProfileDataToolStripMenuItem,
            this.saveDepthDataToolStripMenuItem,
            this.saveDXFProfileToolStripMenuItem,
            this.save3DSurfaceToolStripMenuItem,
            this.save3DSTLSurfaceToolStripMenuItem,
            this.dViewImageToolStripMenuItem,
            this.saveAxialProfileToolStripMenuItem,
            this.saveAllToolStripMenuItem});
            this.saveProcessedFileToolStripMenuItem.Name = "saveProcessedFileToolStripMenuItem";
            this.saveProcessedFileToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.saveProcessedFileToolStripMenuItem.Text = "Save Processed File...";
            this.saveProcessedFileToolStripMenuItem.Click += new System.EventHandler(this.SaveProcessedFileToolStripMenuItem_Click);
            // 
            // saveProfileDataToolStripMenuItem
            // 
            this.saveProfileDataToolStripMenuItem.Name = "saveProfileDataToolStripMenuItem";
            this.saveProfileDataToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveProfileDataToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.saveProfileDataToolStripMenuItem.Text = "Save CSV Profile";
            this.saveProfileDataToolStripMenuItem.Click += new System.EventHandler(this.SaveProfileDataToolStripMenuItem_Click);
            // 
            // saveDepthDataToolStripMenuItem
            // 
            this.saveDepthDataToolStripMenuItem.Name = "saveDepthDataToolStripMenuItem";
            this.saveDepthDataToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.saveDepthDataToolStripMenuItem.Text = "Save CSV Depths";
            this.saveDepthDataToolStripMenuItem.Click += new System.EventHandler(this.SaveDepthDataToolStripMenuItem_Click);
            // 
            // saveDXFProfileToolStripMenuItem
            // 
            this.saveDXFProfileToolStripMenuItem.Name = "saveDXFProfileToolStripMenuItem";
            this.saveDXFProfileToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.saveDXFProfileToolStripMenuItem.Text = "Save 2D DXF Profile";
            this.saveDXFProfileToolStripMenuItem.Click += new System.EventHandler(this.SaveDXFProfileToolStripMenuItem_Click);
            // 
            // save3DSurfaceToolStripMenuItem
            // 
            this.save3DSurfaceToolStripMenuItem.Name = "save3DSurfaceToolStripMenuItem";
            this.save3DSurfaceToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.save3DSurfaceToolStripMenuItem.Text = "Save 3D PLY Surface";
            this.save3DSurfaceToolStripMenuItem.Click += new System.EventHandler(this.Save3DSurfaceToolStripMenuItem_Click);
            // 
            // save3DSTLSurfaceToolStripMenuItem
            // 
            this.save3DSTLSurfaceToolStripMenuItem.Name = "save3DSTLSurfaceToolStripMenuItem";
            this.save3DSTLSurfaceToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.save3DSTLSurfaceToolStripMenuItem.Text = "Save 3D STL Surface";
            this.save3DSTLSurfaceToolStripMenuItem.Click += new System.EventHandler(this.save3DSTLSurfaceToolStripMenuItem_Click);
            // 
            // dViewImageToolStripMenuItem
            // 
            this.dViewImageToolStripMenuItem.Name = "dViewImageToolStripMenuItem";
            this.dViewImageToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.dViewImageToolStripMenuItem.Text = "Save 3D View Image";
            this.dViewImageToolStripMenuItem.Click += new System.EventHandler(this.dViewImageToolStripMenuItem_Click);
            // 
            // saveAxialProfileToolStripMenuItem
            // 
            this.saveAxialProfileToolStripMenuItem.Name = "saveAxialProfileToolStripMenuItem";
            this.saveAxialProfileToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.saveAxialProfileToolStripMenuItem.Text = "Save Axial Profile ";
            this.saveAxialProfileToolStripMenuItem.Click += new System.EventHandler(this.saveAxialProfileToolStripMenuItem_Click);
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.saveAllToolStripMenuItem.Text = "Save All ";
            this.saveAllToolStripMenuItem.Click += new System.EventHandler(this.SaveAllToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.optionsToolStripMenuItem.Text = "Options...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.OptionsToolStripMenuItem_Click);
            // 
            // dataAquisitionToolStripMenuItem
            // 
            this.dataAquisitionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToDAQToolStripMenuItem1,
            this.disconnectFromDAQToolStripMenuItem});
            this.dataAquisitionToolStripMenuItem.Name = "dataAquisitionToolStripMenuItem";
            this.dataAquisitionToolStripMenuItem.Size = new System.Drawing.Size(100, 20);
            this.dataAquisitionToolStripMenuItem.Text = "Data Aquisition";
            // 
            // connectToDAQToolStripMenuItem1
            // 
            this.connectToDAQToolStripMenuItem1.Enabled = false;
            this.connectToDAQToolStripMenuItem1.Name = "connectToDAQToolStripMenuItem1";
            this.connectToDAQToolStripMenuItem1.Size = new System.Drawing.Size(190, 22);
            this.connectToDAQToolStripMenuItem1.Text = "Connect To DAQ";
            this.connectToDAQToolStripMenuItem1.Click += new System.EventHandler(this.ConnectToDAQToolStripMenuItem1_Click);
            // 
            // disconnectFromDAQToolStripMenuItem
            // 
            this.disconnectFromDAQToolStripMenuItem.Enabled = false;
            this.disconnectFromDAQToolStripMenuItem.Name = "disconnectFromDAQToolStripMenuItem";
            this.disconnectFromDAQToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.disconnectFromDAQToolStripMenuItem.Text = "Disconnect from DAQ";
            this.disconnectFromDAQToolStripMenuItem.Click += new System.EventHandler(this.DisconnectFromDAQToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewLogFileToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // viewLogFileToolStripMenuItem
            // 
            this.viewLogFileToolStripMenuItem.Name = "viewLogFileToolStripMenuItem";
            this.viewLogFileToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.viewLogFileToolStripMenuItem.Text = "View Log File";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(4, 502);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(48, 16);
            this.labelStatus.TabIndex = 15;
            this.labelStatus.Text = "Status:";
            // 
            // buttonGetAveAngle
            // 
            this.buttonGetAveAngle.Location = new System.Drawing.Point(7, 324);
            this.buttonGetAveAngle.Name = "buttonGetAveAngle";
            this.buttonGetAveAngle.Size = new System.Drawing.Size(115, 30);
            this.buttonGetAveAngle.TabIndex = 16;
            this.buttonGetAveAngle.Text = "Correct Angle";
            this.buttonGetAveAngle.UseVisualStyleBackColor = true;
            this.buttonGetAveAngle.Click += new System.EventHandler(this.ButtonGetAveAngle_Click);
            // 
            // checkBoxAngleCorrect
            // 
            this.checkBoxAngleCorrect.AutoSize = true;
            this.checkBoxAngleCorrect.Location = new System.Drawing.Point(7, 29);
            this.checkBoxAngleCorrect.Name = "checkBoxAngleCorrect";
            this.checkBoxAngleCorrect.Size = new System.Drawing.Size(115, 17);
            this.checkBoxAngleCorrect.TabIndex = 24;
            this.checkBoxAngleCorrect.Text = "Auto Angle Correct";
            this.checkBoxAngleCorrect.UseVisualStyleBackColor = true;
            this.checkBoxAngleCorrect.CheckedChanged += new System.EventHandler(this.CheckBoxAngleCorrect_CheckedChanged);
            // 
            // textBoxSerialN
            // 
            this.textBoxSerialN.Location = new System.Drawing.Point(42, 43);
            this.textBoxSerialN.Name = "textBoxSerialN";
            this.textBoxSerialN.Size = new System.Drawing.Size(195, 20);
            this.textBoxSerialN.TabIndex = 21;
            this.textBoxSerialN.Text = "000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "S/N";
            // 
            // textBoxCurrentPasses
            // 
            this.textBoxCurrentPasses.Location = new System.Drawing.Point(97, 112);
            this.textBoxCurrentPasses.Name = "textBoxCurrentPasses";
            this.textBoxCurrentPasses.Size = new System.Drawing.Size(140, 20);
            this.textBoxCurrentPasses.TabIndex = 21;
            this.textBoxCurrentPasses.Text = "000";
            // 
            // labelCurrentPasses
            // 
            this.labelCurrentPasses.AutoSize = true;
            this.labelCurrentPasses.Location = new System.Drawing.Point(14, 115);
            this.labelCurrentPasses.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCurrentPasses.Name = "labelCurrentPasses";
            this.labelCurrentPasses.Size = new System.Drawing.Size(78, 13);
            this.labelCurrentPasses.TabIndex = 22;
            this.labelCurrentPasses.Text = "Current Passes";
            // 
            // textBoxTotalPasses
            // 
            this.textBoxTotalPasses.Location = new System.Drawing.Point(97, 137);
            this.textBoxTotalPasses.Name = "textBoxTotalPasses";
            this.textBoxTotalPasses.Size = new System.Drawing.Size(140, 20);
            this.textBoxTotalPasses.TabIndex = 21;
            this.textBoxTotalPasses.Text = "000";
            // 
            // labelTotalPasses
            // 
            this.labelTotalPasses.AutoSize = true;
            this.labelTotalPasses.Location = new System.Drawing.Point(10, 140);
            this.labelTotalPasses.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTotalPasses.Name = "labelTotalPasses";
            this.labelTotalPasses.Size = new System.Drawing.Size(68, 13);
            this.labelTotalPasses.TabIndex = 22;
            this.labelTotalPasses.Text = "Total Passes";
            // 
            // toolStripButtonCursor
            // 
            this.toolStripButtonCursor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCursor.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCursor.Image")));
            this.toolStripButtonCursor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCursor.Name = "toolStripButtonCursor";
            this.toolStripButtonCursor.Size = new System.Drawing.Size(30, 30);
            this.toolStripButtonCursor.Text = "Cursor";
            this.toolStripButtonCursor.Click += new System.EventHandler(this.ToolStripButtonCursor_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(26, 26);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonFileOpen,
            this.toolStripButtonFileSave,
            this.toolStripSeparator1,
            this.toolStripButtonCursor,
            this.toolStripButtonLength,
            this.toolStripButtonSetKnownRadius,
            this.toolStripButtonGrooveMidpoint,
            this.toolStripButtonRotate,
            this.toolStripButtonMirror,
            this.toolStripButtonWinData,
            this.toolStripButtonFitToCircle});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1120, 33);
            this.toolStrip1.TabIndex = 14;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonFileOpen
            // 
            this.toolStripButtonFileOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFileOpen.Image")));
            this.toolStripButtonFileOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFileOpen.Name = "toolStripButtonFileOpen";
            this.toolStripButtonFileOpen.Size = new System.Drawing.Size(30, 30);
            this.toolStripButtonFileOpen.Text = "Open";
            this.toolStripButtonFileOpen.Click += new System.EventHandler(this.ToolStripButtonFileOpen_Click);
            // 
            // toolStripButtonFileSave
            // 
            this.toolStripButtonFileSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFileSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFileSave.Image")));
            this.toolStripButtonFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFileSave.Name = "toolStripButtonFileSave";
            this.toolStripButtonFileSave.Size = new System.Drawing.Size(30, 30);
            this.toolStripButtonFileSave.Text = "Save ";
            this.toolStripButtonFileSave.ToolTipText = "Save  All";
            this.toolStripButtonFileSave.Click += new System.EventHandler(this.ToolStripButtonFileSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // toolStripButtonLength
            // 
            this.toolStripButtonLength.CheckOnClick = true;
            this.toolStripButtonLength.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLength.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLength.Image")));
            this.toolStripButtonLength.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLength.Name = "toolStripButtonLength";
            this.toolStripButtonLength.Size = new System.Drawing.Size(30, 30);
            this.toolStripButtonLength.Text = "Measure Length";
            this.toolStripButtonLength.Click += new System.EventHandler(this.ToolStripButtonLength_Click);
            // 
            // toolStripButtonSetKnownRadius
            // 
            this.toolStripButtonSetKnownRadius.CheckOnClick = true;
            this.toolStripButtonSetKnownRadius.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSetKnownRadius.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSetKnownRadius.Image")));
            this.toolStripButtonSetKnownRadius.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSetKnownRadius.Name = "toolStripButtonSetKnownRadius";
            this.toolStripButtonSetKnownRadius.Size = new System.Drawing.Size(30, 30);
            this.toolStripButtonSetKnownRadius.Text = "Set Known Radius";
            this.toolStripButtonSetKnownRadius.Click += new System.EventHandler(this.ToolStripButtonSetKnownRadius_Click);
            // 
            // toolStripButtonGrooveMidpoint
            // 
            this.toolStripButtonGrooveMidpoint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonGrooveMidpoint.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGrooveMidpoint.Image")));
            this.toolStripButtonGrooveMidpoint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGrooveMidpoint.Name = "toolStripButtonGrooveMidpoint";
            this.toolStripButtonGrooveMidpoint.Size = new System.Drawing.Size(30, 30);
            this.toolStripButtonGrooveMidpoint.Text = "Select Groove Vertical Midpoint";
            this.toolStripButtonGrooveMidpoint.Click += new System.EventHandler(this.toolStripButtonGrooveMidpoint_Click);
            // 
            // toolStripButtonRotate
            // 
            this.toolStripButtonRotate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRotate.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRotate.Image")));
            this.toolStripButtonRotate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRotate.Name = "toolStripButtonRotate";
            this.toolStripButtonRotate.Size = new System.Drawing.Size(30, 30);
            this.toolStripButtonRotate.Text = "Rotate";
            this.toolStripButtonRotate.Click += new System.EventHandler(this.toolStripButtonRotate_Click);
            // 
            // toolStripButtonMirror
            // 
            this.toolStripButtonMirror.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMirror.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMirror.Image")));
            this.toolStripButtonMirror.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMirror.Name = "toolStripButtonMirror";
            this.toolStripButtonMirror.Size = new System.Drawing.Size(30, 30);
            this.toolStripButtonMirror.Text = "Mirror";
            this.toolStripButtonMirror.Click += new System.EventHandler(this.toolStripButtonMirror_Click);
            // 
            // toolStripButtonWinData
            // 
            this.toolStripButtonWinData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonWinData.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonWinData.Image")));
            this.toolStripButtonWinData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonWinData.Name = "toolStripButtonWinData";
            this.toolStripButtonWinData.Size = new System.Drawing.Size(30, 30);
            this.toolStripButtonWinData.Text = "Window Data";
            this.toolStripButtonWinData.Click += new System.EventHandler(this.toolStripButtonWinData_Click);
            // 
            // toolStripButtonFitToCircle
            // 
            this.toolStripButtonFitToCircle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFitToCircle.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFitToCircle.Image")));
            this.toolStripButtonFitToCircle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFitToCircle.Name = "toolStripButtonFitToCircle";
            this.toolStripButtonFitToCircle.Size = new System.Drawing.Size(30, 30);
            this.toolStripButtonFitToCircle.Text = "Fit to Circle";
            this.toolStripButtonFitToCircle.ToolTipText = "Fit to Circle";
            this.toolStripButtonFitToCircle.Click += new System.EventHandler(this.toolStripButtonFitToCircle_Click);
            // 
            // tabControlParams
            // 
            this.tabControlParams.Controls.Add(this.tabPage1);
            this.tabControlParams.Controls.Add(this.tabPage2);
            this.tabControlParams.Controls.Add(this.tabPage3);
            this.tabControlParams.Location = new System.Drawing.Point(760, 23);
            this.tabControlParams.Name = "tabControlParams";
            this.tabControlParams.SelectedIndex = 0;
            this.tabControlParams.Size = new System.Drawing.Size(323, 610);
            this.tabControlParams.TabIndex = 24;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.labelCalStatus);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.textBoxRingCal);
            this.tabPage1.Controls.Add(this.labelNomDiam);
            this.tabPage1.Controls.Add(this.buttonBrowse);
            this.tabPage1.Controls.Add(this.comboBoxDiameterType);
            this.tabPage1.Controls.Add(this.textBoxNomDiam);
            this.tabPage1.Controls.Add(this.labelNotes);
            this.tabPage1.Controls.Add(this.textBoxMiscManNotes);
            this.tabPage1.Controls.Add(this.labelRoundsFired);
            this.tabPage1.Controls.Add(this.textBoxRoundsFired);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.comboBoxManStep);
            this.tabPage1.Controls.Add(this.textBoxSerialN);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.comboBoxBarrel);
            this.tabPage1.Controls.Add(this.labelTotalPasses);
            this.tabPage1.Controls.Add(this.labelCurrentPasses);
            this.tabPage1.Controls.Add(this.textBoxCurrentPasses);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.textBoxTotalPasses);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(315, 584);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Barrel Data";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // labelCalStatus
            // 
            this.labelCalStatus.AutoSize = true;
            this.labelCalStatus.Location = new System.Drawing.Point(20, 273);
            this.labelCalStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCalStatus.Name = "labelCalStatus";
            this.labelCalStatus.Size = new System.Drawing.Size(58, 13);
            this.labelCalStatus.TabIndex = 34;
            this.labelCalStatus.Text = "Cal Status:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 246);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(91, 13);
            this.label10.TabIndex = 33;
            this.label10.Text = "Cal Ring Diam (in)";
            // 
            // textBoxRingCal
            // 
            this.textBoxRingCal.Location = new System.Drawing.Point(116, 244);
            this.textBoxRingCal.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxRingCal.Name = "textBoxRingCal";
            this.textBoxRingCal.Size = new System.Drawing.Size(82, 20);
            this.textBoxRingCal.TabIndex = 32;
            this.textBoxRingCal.Text = "0.500";
            // 
            // labelNomDiam
            // 
            this.labelNomDiam.AutoSize = true;
            this.labelNomDiam.Location = new System.Drawing.Point(19, 218);
            this.labelNomDiam.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelNomDiam.Name = "labelNomDiam";
            this.labelNomDiam.Size = new System.Drawing.Size(93, 13);
            this.labelNomDiam.TabIndex = 31;
            this.labelNomDiam.Text = "Nominal Diameter:";
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(162, 187);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(89, 23);
            this.buttonBrowse.TabIndex = 30;
            this.buttonBrowse.Text = "Cal File...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.ButtonBrowse_Click);
            // 
            // comboBoxDiameterType
            // 
            this.comboBoxDiameterType.FormattingEnabled = true;
            this.comboBoxDiameterType.Items.AddRange(new object[] {
            "Default Value",
            "Set Value",
            "Diameter Profile",
            "Ring Calibrated"});
            this.comboBoxDiameterType.Location = new System.Drawing.Point(22, 189);
            this.comboBoxDiameterType.Name = "comboBoxDiameterType";
            this.comboBoxDiameterType.Size = new System.Drawing.Size(134, 21);
            this.comboBoxDiameterType.TabIndex = 29;
            this.comboBoxDiameterType.SelectedIndexChanged += new System.EventHandler(this.ComboBoxDiameterType_SelectedIndexChanged);
            // 
            // textBoxNomDiam
            // 
            this.textBoxNomDiam.Location = new System.Drawing.Point(116, 215);
            this.textBoxNomDiam.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxNomDiam.Name = "textBoxNomDiam";
            this.textBoxNomDiam.Size = new System.Drawing.Size(121, 20);
            this.textBoxNomDiam.TabIndex = 28;
            this.textBoxNomDiam.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelNotes
            // 
            this.labelNotes.AutoSize = true;
            this.labelNotes.Location = new System.Drawing.Point(5, 290);
            this.labelNotes.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelNotes.Name = "labelNotes";
            this.labelNotes.Size = new System.Drawing.Size(38, 13);
            this.labelNotes.TabIndex = 27;
            this.labelNotes.Text = "Notes:";
            // 
            // textBoxMiscManNotes
            // 
            this.textBoxMiscManNotes.Location = new System.Drawing.Point(8, 309);
            this.textBoxMiscManNotes.Multiline = true;
            this.textBoxMiscManNotes.Name = "textBoxMiscManNotes";
            this.textBoxMiscManNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMiscManNotes.Size = new System.Drawing.Size(220, 127);
            this.textBoxMiscManNotes.TabIndex = 24;
            // 
            // labelRoundsFired
            // 
            this.labelRoundsFired.AutoSize = true;
            this.labelRoundsFired.Location = new System.Drawing.Point(10, 168);
            this.labelRoundsFired.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRoundsFired.Name = "labelRoundsFired";
            this.labelRoundsFired.Size = new System.Drawing.Size(70, 13);
            this.labelRoundsFired.TabIndex = 26;
            this.labelRoundsFired.Text = "Rounds Fired";
            // 
            // textBoxRoundsFired
            // 
            this.textBoxRoundsFired.Location = new System.Drawing.Point(97, 165);
            this.textBoxRoundsFired.Name = "textBoxRoundsFired";
            this.textBoxRoundsFired.Size = new System.Drawing.Size(140, 20);
            this.textBoxRoundsFired.TabIndex = 25;
            this.textBoxRoundsFired.Text = "000000";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 66);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "Manufacturing Step";
            // 
            // comboBoxManStep
            // 
            this.comboBoxManStep.FormattingEnabled = true;
            this.comboBoxManStep.Items.AddRange(new object[] {
            "Pre-Boring ",
            "Boring In-process",
            "Post Boring",
            "Post Honing",
            "Groove Machining In-Process",
            "Post Groove Machining",
            "Post Final Honing ",
            "In Use"});
            this.comboBoxManStep.Location = new System.Drawing.Point(13, 82);
            this.comboBoxManStep.Name = "comboBoxManStep";
            this.comboBoxManStep.Size = new System.Drawing.Size(224, 21);
            this.comboBoxManStep.TabIndex = 23;
            this.comboBoxManStep.SelectedIndexChanged += new System.EventHandler(this.ComboBoxManStep_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.comboBoxProbeType);
            this.tabPage2.Controls.Add(this.textBoxGrooveList);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.textBoxEndPosA);
            this.tabPage2.Controls.Add(this.textBoxStartPosA);
            this.tabPage2.Controls.Add(this.comboBoxProbeDirection);
            this.tabPage2.Controls.Add(this.comboBoxMethod);
            this.tabPage2.Controls.Add(this.labelMethod);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.labelStartPos);
            this.tabPage2.Controls.Add(this.labelInputFIlename);
            this.tabPage2.Controls.Add(this.textBoxPitch);
            this.tabPage2.Controls.Add(this.textBoxStartPosX);
            this.tabPage2.Controls.Add(this.textBoxAngleInc);
            this.tabPage2.Controls.Add(this.labelEndPos);
            this.tabPage2.Controls.Add(this.labelRingRevs);
            this.tabPage2.Controls.Add(this.textBoxRingRevs);
            this.tabPage2.Controls.Add(this.textBoxEndPosX);
            this.tabPage2.Controls.Add(this.textBoxPtsPerRev);
            this.tabPage2.Controls.Add(this.labelPitch);
            this.tabPage2.Controls.Add(this.radioButtonAngleInc);
            this.tabPage2.Controls.Add(this.textBoxProbeSpacing);
            this.tabPage2.Controls.Add(this.textBoxProbeCount);
            this.tabPage2.Controls.Add(this.radioButtonPtsperRev);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(315, 584);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Inspection Params";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(32, 303);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 13);
            this.label11.TabIndex = 11;
            this.label11.Text = "Probe Type";
            // 
            // comboBoxProbeType
            // 
            this.comboBoxProbeType.FormattingEnabled = true;
            this.comboBoxProbeType.Items.AddRange(new object[] {
            "SI Distance",
            "Line Scan"});
            this.comboBoxProbeType.Location = new System.Drawing.Point(99, 300);
            this.comboBoxProbeType.Name = "comboBoxProbeType";
            this.comboBoxProbeType.Size = new System.Drawing.Size(121, 21);
            this.comboBoxProbeType.TabIndex = 10;
            this.comboBoxProbeType.SelectedIndexChanged += new System.EventHandler(this.comboBoxProbeType_SelectedIndexChanged);
            // 
            // textBoxGrooveList
            // 
            this.textBoxGrooveList.Location = new System.Drawing.Point(132, 275);
            this.textBoxGrooveList.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxGrooveList.Name = "textBoxGrooveList";
            this.textBoxGrooveList.Size = new System.Drawing.Size(82, 20);
            this.textBoxGrooveList.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 278);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Grooves/Lands(1,5...)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 228);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Probe Spacing";
            // 
            // textBoxProbeSpacing
            // 
            this.textBoxProbeSpacing.Location = new System.Drawing.Point(100, 225);
            this.textBoxProbeSpacing.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxProbeSpacing.Name = "textBoxProbeSpacing";
            this.textBoxProbeSpacing.Size = new System.Drawing.Size(42, 20);
            this.textBoxProbeSpacing.TabIndex = 0;
            this.textBoxProbeSpacing.Text = "0";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.buttonMeasureDepths);
            this.tabPage3.Controls.Add(this.buttonSetRadius);
            this.tabPage3.Controls.Add(this.buttonCorrectMidpoint);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.buttonBuildProfile);
            this.tabPage3.Controls.Add(this.textBoxCurrentRadius);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.labelInputValue);
            this.tabPage3.Controls.Add(this.textBoxProbePhaseDeg);
            this.tabPage3.Controls.Add(this.textBoxKnownRadius);
            this.tabPage3.Controls.Add(this.checkBoxUseFilename);
            this.tabPage3.Controls.Add(this.buttonProcessFile);
            this.tabPage3.Controls.Add(this.buttonGetAveAngle);
            this.tabPage3.Controls.Add(this.checkBoxAngleCorrect);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(315, 584);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Data processing";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // buttonMeasureDepths
            // 
            this.buttonMeasureDepths.Location = new System.Drawing.Point(7, 480);
            this.buttonMeasureDepths.Name = "buttonMeasureDepths";
            this.buttonMeasureDepths.Size = new System.Drawing.Size(115, 30);
            this.buttonMeasureDepths.TabIndex = 46;
            this.buttonMeasureDepths.Text = "MeasureDepths";
            this.buttonMeasureDepths.UseVisualStyleBackColor = true;
            this.buttonMeasureDepths.Click += new System.EventHandler(this.buttonMeasureDepths_Click);
            // 
            // buttonCorrectMidpoint
            // 
            this.buttonCorrectMidpoint.Location = new System.Drawing.Point(7, 426);
            this.buttonCorrectMidpoint.Name = "buttonCorrectMidpoint";
            this.buttonCorrectMidpoint.Size = new System.Drawing.Size(115, 30);
            this.buttonCorrectMidpoint.TabIndex = 45;
            this.buttonCorrectMidpoint.Text = "Correct To Midpoint";
            this.buttonCorrectMidpoint.UseVisualStyleBackColor = true;
            this.buttonCorrectMidpoint.Click += new System.EventHandler(this.buttonCorrectMidpoint_Click);
            // 
            // buttonBuildProfile
            // 
            this.buttonBuildProfile.Location = new System.Drawing.Point(7, 375);
            this.buttonBuildProfile.Name = "buttonBuildProfile";
            this.buttonBuildProfile.Size = new System.Drawing.Size(115, 30);
            this.buttonBuildProfile.TabIndex = 44;
            this.buttonBuildProfile.Text = "Build profile ";
            this.buttonBuildProfile.UseVisualStyleBackColor = true;
            this.buttonBuildProfile.Click += new System.EventHandler(this.buttonBuildProfile_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 74);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "Probe Phase Angle (deg)";
            // 
            // textBoxProbePhaseDeg
            // 
            this.textBoxProbePhaseDeg.Location = new System.Drawing.Point(156, 71);
            this.textBoxProbePhaseDeg.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxProbePhaseDeg.Name = "textBoxProbePhaseDeg";
            this.textBoxProbePhaseDeg.Size = new System.Drawing.Size(36, 20);
            this.textBoxProbePhaseDeg.TabIndex = 37;
            this.textBoxProbePhaseDeg.Text = "179.5";
            // 
            // checkBoxUseFilename
            // 
            this.checkBoxUseFilename.AutoSize = true;
            this.checkBoxUseFilename.Location = new System.Drawing.Point(7, 7);
            this.checkBoxUseFilename.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxUseFilename.Name = "checkBoxUseFilename";
            this.checkBoxUseFilename.Size = new System.Drawing.Size(151, 17);
            this.checkBoxUseFilename.TabIndex = 32;
            this.checkBoxUseFilename.Text = "Extract data from file name";
            this.checkBoxUseFilename.UseVisualStyleBackColor = true;
            this.checkBoxUseFilename.CheckedChanged += new System.EventHandler(this.checkBoxUseFilename_CheckedChanged_1);
            // 
            // tabControlOutput
            // 
            this.tabControlOutput.Controls.Add(this.tabPageGraph);
            this.tabControlOutput.Controls.Add(this.tabPageData);
            this.tabControlOutput.Controls.Add(this.tabPage4);
            this.tabControlOutput.Location = new System.Drawing.Point(184, 52);
            this.tabControlOutput.Margin = new System.Windows.Forms.Padding(2);
            this.tabControlOutput.Name = "tabControlOutput";
            this.tabControlOutput.SelectedIndex = 0;
            this.tabControlOutput.Size = new System.Drawing.Size(512, 420);
            this.tabControlOutput.TabIndex = 25;
            // 
            // tabPageGraph
            // 
            this.tabPageGraph.Controls.Add(this.pictureBox1);
            this.tabPageGraph.Location = new System.Drawing.Point(4, 22);
            this.tabPageGraph.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageGraph.Name = "tabPageGraph";
            this.tabPageGraph.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageGraph.Size = new System.Drawing.Size(504, 394);
            this.tabPageGraph.TabIndex = 0;
            this.tabPageGraph.Text = "Profile";
            this.tabPageGraph.UseVisualStyleBackColor = true;
            // 
            // tabPageData
            // 
            this.tabPageData.Controls.Add(this.textBoxDataOut);
            this.tabPageData.Location = new System.Drawing.Point(4, 22);
            this.tabPageData.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageData.Name = "tabPageData";
            this.tabPageData.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageData.Size = new System.Drawing.Size(504, 394);
            this.tabPageData.TabIndex = 1;
            this.tabPageData.Text = "Data Output";
            this.tabPageData.UseVisualStyleBackColor = true;
            // 
            // textBoxDataOut
            // 
            this.textBoxDataOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDataOut.Location = new System.Drawing.Point(2, 2);
            this.textBoxDataOut.Multiline = true;
            this.textBoxDataOut.Name = "textBoxDataOut";
            this.textBoxDataOut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDataOut.Size = new System.Drawing.Size(500, 390);
            this.textBoxDataOut.TabIndex = 19;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.elementHost1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(504, 394);
            this.tabPage4.TabIndex = 2;
            this.tabPage4.Text = "3D View";
            this.tabPage4.UseVisualStyleBackColor = true;
            this.tabPage4.Click += new System.EventHandler(this.tabPage4_Click);
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(504, 394);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.userControl11;
            // 
            // progressBarProcessing
            // 
            this.progressBarProcessing.Location = new System.Drawing.Point(9, 532);
            this.progressBarProcessing.Name = "progressBarProcessing";
            this.progressBarProcessing.Size = new System.Drawing.Size(171, 23);
            this.progressBarProcessing.TabIndex = 26;
            // 
            // OpenDXFFileToolStripMenuItem
            // 
            this.OpenDXFFileToolStripMenuItem.Name = "OpenDXFFileToolStripMenuItem";
            this.OpenDXFFileToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.OpenDXFFileToolStripMenuItem.Text = "DXF File ";
            this.OpenDXFFileToolStripMenuItem.Click += new System.EventHandler(this.OpenDXFFileToolStripMenuItem_Click);
            // 
            // MainInspectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 635);
            this.Controls.Add(this.progressBarProcessing);
            this.Controls.Add(this.tabControlOutput);
            this.Controls.Add(this.tabControlParams);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainInspectionForm";
            this.Text = "Barrel Inspection Processor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainInspectionForm_FormClosing);
            this.Load += new System.EventHandler(this.MainInspectionForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainInspectionForm_KeyDown);
            this.Resize += new System.EventHandler(this.MainInspectionForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControlParams.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabControlOutput.ResumeLayout(false);
            this.tabPageGraph.ResumeLayout(false);
            this.tabPageData.ResumeLayout(false);
            this.tabPageData.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelInputFIlename;
        private System.Windows.Forms.Button buttonProcessFile;
        private System.Windows.Forms.TextBox textBoxStartPosX;
        private System.Windows.Forms.Label labelStartPos;
        private System.Windows.Forms.ComboBox comboBoxMethod;
        private System.Windows.Forms.Label labelEndPos;
        private System.Windows.Forms.Label labelMethod;
        private System.Windows.Forms.TextBox textBoxPtsPerRev;
        private System.Windows.Forms.TextBox textBoxEndPosA;
        private System.Windows.Forms.TextBox textBoxStartPosA;
        private System.Windows.Forms.TextBox textBoxEndPosX;
        private System.Windows.Forms.TextBox textBoxAngleInc;
        private System.Windows.Forms.ComboBox comboBoxBarrel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelPitch;
        private System.Windows.Forms.TextBox textBoxPitch;
        private System.Windows.Forms.RadioButton radioButtonAngleInc;
        private System.Windows.Forms.RadioButton radioButtonPtsperRev;
        private System.Windows.Forms.Label labelRingRevs;
        private System.Windows.Forms.TextBox textBoxRingRevs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxProbeCount;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelRadiusMeasured;
        private System.Windows.Forms.Label labelZPosition;
        private System.Windows.Forms.Label labelXPosition;
        private System.Windows.Forms.Label labelYPosition;
        private System.Windows.Forms.Label labelDxMeasured;
        private System.Windows.Forms.Label labelDyMeasured;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton radioButtonViewProcessed;
        private System.Windows.Forms.RadioButton radioButtonViewRaw;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openRawDataFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProcessedFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataAquisitionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToDAQToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem disconnectFromDAQToolStripMenuItem;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button buttonGetAveAngle;
        private System.Windows.Forms.TextBox textBoxSerialN;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxCurrentPasses;
        private System.Windows.Forms.Label labelCurrentPasses;
        private System.Windows.Forms.TextBox textBoxTotalPasses;
        private System.Windows.Forms.Label labelTotalPasses;
        private System.Windows.Forms.ComboBox comboBoxProbeDirection;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolStripButton toolStripButtonCursor;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonLength;
        private System.Windows.Forms.Label labelInputValue;
        private System.Windows.Forms.TextBox textBoxKnownRadius;
        private System.Windows.Forms.ToolStripButton toolStripButtonSetKnownRadius;
        private System.Windows.Forms.Button buttonSetRadius;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxCurrentRadius;
        private System.Windows.Forms.TabControl tabControlParams;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label labelRoundsFired;
        private System.Windows.Forms.TextBox textBoxRoundsFired;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBoxManStep;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripButton toolStripButtonFileOpen;
        private System.Windows.Forms.ToolStripButton toolStripButtonFileSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.CheckBox checkBoxAngleCorrect;
        private System.Windows.Forms.ToolStripMenuItem saveProfileDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveDepthDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveDXFProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem save3DSurfaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControlOutput;
        private System.Windows.Forms.TabPage tabPageGraph;
        private System.Windows.Forms.TabPage tabPageData;
        private System.Windows.Forms.TextBox textBoxDataOut;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxProbeSpacing;
        private System.Windows.Forms.Label labelNomDiam;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.ComboBox comboBoxDiameterType;
        private System.Windows.Forms.TextBox textBoxNomDiam;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem rawFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processedCSVFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem save3DSTLSurfaceToolStripMenuItem;
        private System.Windows.Forms.ProgressBar progressBarProcessing;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewLogFileToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox checkBoxUseFilename;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxProbePhaseDeg;
        private System.Windows.Forms.TextBox textBoxGrooveList;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxRingCal;
        private System.Windows.Forms.Label labelNotes;
        private System.Windows.Forms.TextBox textBoxMiscManNotes;
        private System.Windows.Forms.Label labelCalStatus;
        private System.Windows.Forms.Button buttonBuildProfile;
        private System.Windows.Forms.ToolStripButton toolStripButtonGrooveMidpoint;
        private System.Windows.Forms.Button buttonCorrectMidpoint;
        private System.Windows.Forms.ToolStripMenuItem saveAxialProfileToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private UserControl1 userControl11;
        private System.Windows.Forms.ToolStripMenuItem dViewImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonRotate;
        private System.Windows.Forms.ToolStripButton toolStripButtonMirror;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBoxProbeType;
        private System.Windows.Forms.ToolStripButton toolStripButtonWinData;
        private System.Windows.Forms.Button buttonMeasureDepths;
        private System.Windows.Forms.ToolStripButton toolStripButtonFitToCircle;
        private System.Windows.Forms.ToolStripMenuItem OpenDXFFileToolStripMenuItem;
    }
}

