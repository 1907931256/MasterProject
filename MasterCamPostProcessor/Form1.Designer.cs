namespace MasterCamPostProcessor
{
    partial class Form1
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
            this.buttonOpenNciFile = new System.Windows.Forms.Button();
            this.textBoxNCI = new System.Windows.Forms.TextBox();
            this.textBoxNC = new System.Windows.Forms.TextBox();
            this.buttonCreateNCFile = new System.Windows.Forms.Button();
            this.buttonSelectMachine = new System.Windows.Forms.Button();
            this.labelNciFilename = new System.Windows.Forms.Label();
            this.labelCNCMachineName = new System.Windows.Forms.Label();
            this.buttonSaveNCFile = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOpenNciFile
            // 
            this.buttonOpenNciFile.Location = new System.Drawing.Point(12, 509);
            this.buttonOpenNciFile.Name = "buttonOpenNciFile";
            this.buttonOpenNciFile.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenNciFile.TabIndex = 0;
            this.buttonOpenNciFile.Text = "Open NCI";
            this.buttonOpenNciFile.UseVisualStyleBackColor = true;
            this.buttonOpenNciFile.Click += new System.EventHandler(this.buttonOpenNciFile_Click);
            // 
            // textBoxNCI
            // 
            this.textBoxNCI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNCI.Location = new System.Drawing.Point(0, 0);
            this.textBoxNCI.Multiline = true;
            this.textBoxNCI.Name = "textBoxNCI";
            this.textBoxNCI.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxNCI.Size = new System.Drawing.Size(401, 394);
            this.textBoxNCI.TabIndex = 1;
            // 
            // textBoxNC
            // 
            this.textBoxNC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNC.Location = new System.Drawing.Point(0, 0);
            this.textBoxNC.Multiline = true;
            this.textBoxNC.Name = "textBoxNC";
            this.textBoxNC.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxNC.Size = new System.Drawing.Size(552, 394);
            this.textBoxNC.TabIndex = 2;
            // 
            // buttonCreateNCFile
            // 
            this.buttonCreateNCFile.Location = new System.Drawing.Point(111, 509);
            this.buttonCreateNCFile.Name = "buttonCreateNCFile";
            this.buttonCreateNCFile.Size = new System.Drawing.Size(75, 23);
            this.buttonCreateNCFile.TabIndex = 3;
            this.buttonCreateNCFile.Text = "Create NC";
            this.buttonCreateNCFile.UseVisualStyleBackColor = true;
            this.buttonCreateNCFile.Click += new System.EventHandler(this.buttonCreateNCFile_Click);
            // 
            // buttonSelectMachine
            // 
            this.buttonSelectMachine.Location = new System.Drawing.Point(12, 556);
            this.buttonSelectMachine.Name = "buttonSelectMachine";
            this.buttonSelectMachine.Size = new System.Drawing.Size(75, 36);
            this.buttonSelectMachine.TabIndex = 4;
            this.buttonSelectMachine.Text = "Select Machine";
            this.buttonSelectMachine.UseVisualStyleBackColor = true;
            this.buttonSelectMachine.Click += new System.EventHandler(this.buttonSelectMachine_Click);
            // 
            // labelNciFilename
            // 
            this.labelNciFilename.AutoSize = true;
            this.labelNciFilename.Location = new System.Drawing.Point(12, 419);
            this.labelNciFilename.Name = "labelNciFilename";
            this.labelNciFilename.Size = new System.Drawing.Size(26, 13);
            this.labelNciFilename.TabIndex = 5;
            this.labelNciFilename.Text = "File:";
            // 
            // labelCNCMachineName
            // 
            this.labelCNCMachineName.AutoSize = true;
            this.labelCNCMachineName.Location = new System.Drawing.Point(12, 445);
            this.labelCNCMachineName.Name = "labelCNCMachineName";
            this.labelCNCMachineName.Size = new System.Drawing.Size(82, 13);
            this.labelCNCMachineName.TabIndex = 6;
            this.labelCNCMachineName.Text = "Machine Name:";
            // 
            // buttonSaveNCFile
            // 
            this.buttonSaveNCFile.Location = new System.Drawing.Point(209, 509);
            this.buttonSaveNCFile.Name = "buttonSaveNCFile";
            this.buttonSaveNCFile.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveNCFile.TabIndex = 7;
            this.buttonSaveNCFile.Text = "Save NC";
            this.buttonSaveNCFile.UseVisualStyleBackColor = true;
            this.buttonSaveNCFile.Click += new System.EventHandler(this.buttonSaveNCFile_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textBoxNCI);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxNC);
            this.splitContainer1.Size = new System.Drawing.Size(957, 394);
            this.splitContainer1.SplitterDistance = 401;
            this.splitContainer1.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 604);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.buttonSaveNCFile);
            this.Controls.Add(this.labelCNCMachineName);
            this.Controls.Add(this.labelNciFilename);
            this.Controls.Add(this.buttonSelectMachine);
            this.Controls.Add(this.buttonCreateNCFile);
            this.Controls.Add(this.buttonOpenNciFile);
            this.Name = "Form1";
            this.Text = "MasterCam Post Processor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOpenNciFile;
        private System.Windows.Forms.TextBox textBoxNCI;
        private System.Windows.Forms.TextBox textBoxNC;
        private System.Windows.Forms.Button buttonCreateNCFile;
        private System.Windows.Forms.Button buttonSelectMachine;
        private System.Windows.Forms.Label labelNciFilename;
        private System.Windows.Forms.Label labelCNCMachineName;
        private System.Windows.Forms.Button buttonSaveNCFile;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

