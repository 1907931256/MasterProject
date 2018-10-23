using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using AbMachModel;
using GeometryLib;
using DwgConverterLib;
using ToolpathLib;
using FileIOLib;
using SurfaceModel;
using AWJModel;
using CNCLib;


namespace Abmach3TestForm
{
    public partial class Form1 : Form
    {
        AbMachParameters parms;
        ModelPath path;
        ISurface<AbmachPoint> surface3D;
        ISurface<AbmachPoint> targetSurface3D;
        Abmach2DSurface surface2D;
        
        AbmachSimModel2D model2D;
        AbmachSimModel3D model3D;
        CancellationTokenSource cts;
        CancellationToken ct;
        double meshSize;

        public Form1()
        {
            
            InitializeComponent();
            buttonRun.Enabled = false;

        }

        private   void buttonInit_Click(object sender, EventArgs e)
        {
            try
            {
                label1.Text = "Running";                
                initModel();
                buttonRun.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private async void  buttonRun_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime start  = DateTime.Now;
                label1.Text = "Running";
                cts = new CancellationTokenSource();
                ct = cts.Token;
                buttonStop.Enabled = true;
                buttonRun.Enabled = false;
                buttonSave.Enabled = false;
                await runModelAsync(ct, new Progress<int>(p => progressBar1.Value = p));
                label1.Text = "Finished Run";
                DateTime finish = DateTime.Now;
                TimeSpan timeSpan = finish - start;
                
                string secs = timeSpan.TotalSeconds.ToString();
                
                string time = secs + " secs" ;
                label3.Text = time;
                buttonSave.Enabled = true;
                buttonRun.Enabled = true;
                List<string> depths = new List<string>();
                int runIter = 1;
                foreach (double d in parms.DepthInfo.CurrentDepthAtLocation)
                {
                    string dr = runIter.ToString() + "," + d.ToString("f5");
                    depths.Add(dr);
                    runIter++;
                }
                textBox1.Lines = depths.ToArray();
            }
            catch (Exception ex)
            {
                if (cts != null)
                {
                    cts.Cancel();
                    label1.Text = "Model Error";
                }
                buttonRun.Enabled = true;
                buttonStop.Enabled = true;
                buttonSave.Enabled = false;
                MessageBox.Show(ex.Message +":"+ ex.StackTrace);
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (cts != null)
                {
                    cts.Cancel();
                }
                label1.Text = "Canceled";
                buttonRun.Enabled = true;
                buttonSave.Enabled = false;
                buttonStop.Enabled = false;
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation canceled.");
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            label1.Text = "Saving";
            Refresh();
            saveSurface("finalSurface.dxf","finalPointlist.xyz");
            label1.Text = "Finished Saving";
            Refresh();
        }
       

        Task runModelAsync(CancellationToken ct, Progress<int> progress)
        {
            try
            {
                if(model2dMode)
                {
                    model2D = new AbmachSimModel2D(surface2D,  path, parms);
                    saveSurface("initialSurface.dxf", "initialPointlist.xyz");
                    var t = Task.Run(() => model2D.Run(ct, progress));
                    return t;
                }
                else
                {
                    model3D = new AbmachSimModel3D(surface3D, targetSurface3D, path, parms);
                    saveSurface("initialSurface.dxf", "initialPointlist.xyz");
                    var t = Task.Run(() => model3D.Run(ct, progress));
                    return t;
                }              
                
               
            }
            catch (Exception)
            {
                throw;
            }
        }
        Task initModelAsync(CancellationToken ct, Progress<int> progress)
        {
            var t = Task.Run(() => initModelAsync(progress));
            return t;
        }
        void initModel()
        {
            buildParms();
            buildToolpath();
            buildSurface();
        }
        void initModelAsync(IProgress<int> progress)
        {
            buildParms();
            progress.Report( 33);
            buildToolpath();
            progress.Report(66);
            buildSurface();
            progress.Report(100);
        }        
        void buildToolpath()
        {
            //buildPathFromEquation();
            buildPathFromFile();
        }
        void buildPathFromEquation()
        {
            path = new ModelPath();
            for (int i = 0; i <= 100; i++)
            {
                double di = i;
                var pos = new CNCLib.MachinePosition(MachineGeometry.XYZBC);
                pos.X = .1 + (di / 200);
                pos.Y = .5;
                pos.Z = 1;
                pos.Bdeg = 0;
                pos.Cdeg = 0;
                Vector3 jet = new Vector3(0, 0, 1);
                ModelPathEntity mpEnt = new ModelPathEntity();
                mpEnt.Position = pos;
                mpEnt.JetVector = jet;
                mpEnt.JetOn = true;
                mpEnt.Feedrate.Value = 10;
                mpEnt.TargetDepth = .05;
                path.Add(mpEnt);
            }
            for (int i = 0; i <= 200; i++)
            {
                double di = i;
                var pos = new CNCLib.MachinePosition(MachineGeometry.XYZBC);
                pos.X = .5 ;
                pos.Y = .1+ (di / 200);
                pos.Z = 1;
                pos.Bdeg = 0;
                pos.Cdeg = 0;
                Vector3 jet = new Vector3(0, 0, 1);
                ModelPathEntity mpEnt = new ModelPathEntity();
                mpEnt.Position = pos;
                mpEnt.JetVector = jet;
                mpEnt.JetOn = true;
                mpEnt.Feedrate.Value = 8;
                mpEnt.TargetDepth = .05;
                path.Add(mpEnt);
            }
        }
        void buildPathFromFile()
        {
            string inputFile;
            inputFile= "STRAIGHT-TEST-8-3-15.nc";
            //inputFile = "STRAIGHT-4.NC";
            //string inputFile = "SURF-TEST-1.nc";
            ToolPath toolpath = CNCFileParser.ToPath(inputFile);
            ConstantDistancePathBuilder mpb = new ConstantDistancePathBuilder();
            path = mpb.Build(toolpath, meshSize);
            int pathCount = path.Count;
            label2.Text = "path count:" + pathCount.ToString();
            label4.Text= path.BoundingBox.Min.X.ToString();
            label5.Text=path.BoundingBox.Min.Y.ToString();
            label6.Text=path.BoundingBox.Min.Z.ToString();
            label7.Text=path.BoundingBox.Max.X.ToString();
            label8.Text=path.BoundingBox.Max.Y.ToString();
            label9.Text=path.BoundingBox.Max.Z.ToString();
            Refresh();
        }

        void buildSurface()
        {
            label1.Text = "Building Surface";
            Refresh();
            
            //buildSurfaceFromFile();
            //buildSurfaceFromEquation();
            buildSurfaceFromToolpath(path, jetDiameter, meshSize);
            label1.Text = "Finished Building Surface";
            Refresh();
        }
        void buildSurfaceFromEquation()
        {
            List<Line> lines = new List<Line>();
            for (int i = 0; i <= 10; i++)
            {
                double di = i;
                //Line l = new Line(di/10.0, 0, .2*Math.Sin(Math.PI * di /5), (di + 1)/10, 0, .2*Math.Sin(Math.PI * (di + 1) / 5));
                Line l = new Line(di / 10.0, 0, 1.0, (di + 1) / 10.0, 0, 1.0);
                lines.Add(l);

            }
            Line acrossV = new Line(0, 0, 0, 0, 1, 0);
          //  surface3D = OctreeBuilder<AbmachPoint>.Build(lines, acrossV, meshSize);
        }
        void buildSurfaceFromFile()
        {
            string startFileName = "SURF-TEST-1.STL";
           // string targetFileName = "";
           // StlFile startStlFile = StlFileParser.Open(startFileName);
            //StlFile targetStlFile = StlFileParser.Open(targetFileName);
            
           // surface3D = OctreeBuilder<AbmachPoint>.Build(startStlFile, meshSize);
            //Octree<AbmachPoint> targetSurface = OctreeBuilder<AbmachPoint>.Build(targetStlFile, meshSize);

        }
        void buildSurfaceFromToolpath(ModelPath path,double toolDiameter,double meshSize)
        {
            double xMin = path.JetOnBoundingBox.Min.X - toolDiameter;
            double yMin = path.JetOnBoundingBox.Min.Y - toolDiameter;
            double xMax = path.JetOnBoundingBox.Max.X + toolDiameter;
            double yMax = path.JetOnBoundingBox.Max.Y + toolDiameter;
            double zMin = path.JetOnBoundingBox.Min.Z - toolDiameter;
            double zMax = path.JetOnBoundingBox.Max.Z + toolDiameter;
            List<Vector3> pathAsVectors = new List<Vector3>();
            foreach (ModelPathEntity mpe in path)
            {
                pathAsVectors.Add(mpe.PositionAsVector);
            }
            BoundingBox boundingBox = new BoundingBox(xMin,yMin, path.JetOnBoundingBox.Min.Z, xMax,yMax, path.JetOnBoundingBox.Max.Z);
            if(model2dMode)
            {
                surface2D = new Abmach2DSurface(boundingBox, meshSize, toolDiameter);
                //surface2D = Surface2DBuilder<AbmachPoint>.Build(boundingBox, meshSize);
                //targetSurface2D = Surface2DBuilder<AbmachPoint>.Build(boundingBox,  meshSize);
                

            }
            else
            {
               // surface3D = OctreeBuilder<AbmachPoint>.Build(pathAsVectors, meshSize);

            }
            
        }

        double jetDiameter ;
        void buildParms()
        {
            buildParmsFromValues();
            //buildParmsFromFile("params.xml");
        }
        void buildParmsFromFile(string fileName)
        {
           parms = AbMachParametersFile.Open(fileName);
        }
        void buildParmsFromValues()
        {
            try
            {
                meshSize = .002;
                jetDiameter = .050;
                double nominalSurfaceSpeed = 47.0;
                double depthPerPass = .002; 
                int runs = 10;
                int iterations = 1;
                int equationIndex = 2;
                double searchRadius = jetDiameter * .1;

                var jet = new AbMachJet(meshSize, jetDiameter, equationIndex);
                var runInfo = new RunInfo(runs, iterations, ModelRunType.RunAsIs);
                var removalRate = new RemovalRate(nominalSurfaceSpeed, depthPerPass);
                var depthInfo = new DepthInfo(new Vector3(1.005, 2.5, 0), DepthSearchType.FindAveDepth,searchRadius);
                var op = AbMachOperation.ROCKETCHANNEL;
                var mat = new Material(
                    MaterialType.Metal,
                    "Aluminum", 
                    thickness:.25,
                    millMachinabilityIndex: 1,
                    cutMachinabilityIndex: 1,
                    criticalAngleRadians:Math.PI* 70.0/180 );
                parms = AbMachParamBuilder.Build(op, runInfo, removalRate, mat, jet, depthInfo, meshSize);
                AbMachParametersFile.Save(parms, "params.xml");                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        void saveSurface(string dxfFilename, string txtFileName)
        {
            try
            {
                ISurface<AbmachPoint> surfOut = model3D.GetSurface();
                List<AbmachPoint> ptlist = surfOut.GetAllPoints();                            
                List<DwgEntity> entityList = new List<DwgEntity>();                
                var pointList = new List<string>();
                foreach (AbmachPoint v in ptlist)
                {
                    pointList.Add(v.Position.X.ToString("f4") + " " + v.Position.Y.ToString("f4") + " " + v.Position.Z.ToString("f4"));
                    DXFPoint pt = new DXFPoint(v.Position);
                    if (v.JetHit)
                    {
                        if(v.Normal.Length != 0.0)
                        {
                            double scaleF = meshSize / v.Normal.Length;
                            DXFLine line = new DXFLine(v.Position.X, v.Position.Y, v.Position.Z, scaleF * v.Normal.X + v.Position.X, scaleF * v.Normal.Y + v.Position.Y, scaleF * v.Normal.Z + v.Position.Z);
                            entityList.Add(line);
                        }
                        pt.DxfColor = DxfColor.Green;
                        entityList.Add(pt);
                    }
                    else
                    {
                        pt.DxfColor = DxfColor.Red;
                        entityList.Add(pt);
                    }

                }
                FileIO.Save(pointList, txtFileName);
                DxfFileBuilder.Save(entityList, dxfFilename);                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            radioButton2D.Checked = true;
        }
        bool model2dMode;

        private void radioButton2D_CheckedChanged(object sender, EventArgs e)
        {
            model2dMode = radioButton2D.Checked;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
