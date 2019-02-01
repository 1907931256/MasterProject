﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
using System.Diagnostics;
using DwgConverterLib;
using DataLib;
using BarrelLib;
using SurfaceFileLib;
using SurfaceModel;
namespace InspectionLib
{
    /// <summary>
    /// builds various output data files from data set
    /// </summary>
    public class DataFileBuilder
    {
        
        
        static public string BuildFileName(string fileName,string suffix,string extension)
        {
            var _filenoExt = System.IO.Path.GetFileNameWithoutExtension(fileName);
            var outputDirectory = System.IO.Path.GetDirectoryName(fileName);
            var outputFilename = string.Concat(outputDirectory, "\\", _filenoExt,suffix,extension);
            return outputFilename;
        }
        static List<string> BuildFileHeader(string inputFilename)
        {
            var outputFileHeader = new List<string>();
            string creationDate = System.DateTime.Now.ToShortDateString();
            string creationTime = System.DateTime.Now.ToShortTimeString();
            outputFileHeader.Add(inputFilename);
            outputFileHeader.Add(creationDate + "," + creationTime);
            return outputFileHeader;
        }
        static List<string> BuildFileHeader(Barrel barrel, string inputFilename)
        {
            var outputFileHeader = new List<string>();
            string creationDate = System.DateTime.Now.ToShortDateString();
            string creationTime = System.DateTime.Now.ToShortTimeString();
            outputFileHeader.Add(inputFilename);
            outputFileHeader.Add(creationDate + "," + creationTime);

            string line = "S/N:" + barrel.ManufactureData.SerialNumber;
            outputFileHeader.Add(line);
            line = "TYPE:" + barrel.Type.ToString();
            outputFileHeader.Add(line);
            line = "STATUS:" + barrel.ManufactureData.CurrentManufStep;
            outputFileHeader.Add(line);
            line = "Land Diameter:" + barrel.DimensionData.ActualLandDiam.ToString("F5");
            outputFileHeader.Add(line);
            if (barrel.ManufactureData.MiscData.Count > 0)
            {
                outputFileHeader.Add("NOTES:");
                foreach (string note in barrel.ManufactureData.MiscData)
                {
                    outputFileHeader.Add(note);
                }
            }
            
            return outputFileHeader;
        }
         static public void SavePlyFile(Barrel barrel, DataOutputOptions options, CylGridData correctedRingList,double unrollRadius, string fileName, IProgress<int> progress)
        {
            try
            {
                if (options.SaveSurfaceFlat)
                {
                    SaveFlatPlyFile(barrel, options, correctedRingList, unrollRadius, fileName,progress);
                }
                else
                {
                    SaveRolledPlyFile(barrel, options, correctedRingList, fileName,progress);
                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        static void SavePly(CartGridData map, string fileName, IProgress<int> progress)
        {
            try
            {
                var outputFilename = BuildFileName(fileName,"", ".ply");
                Debug.WriteLine("writing PLY file " + fileName);
                var plyF = new PlyFile();
                plyF.BuildFromGrid2(map);
                plyF.SaveAscii(outputFilename);

                Debug.WriteLine("finished writing ply file");
            }
            catch (Exception)
            {

                throw;
            }

        }
        static void SaveRolledPlyFile(Barrel barrel, DataOutputOptions options, CylGridData correctedRingList, string fileName, IProgress<int> progress)
        {
            try
            {
                var outputFilename = BuildFileName(fileName,"_rolled", ".ply");
                
               var colorCodedData =  DataColorCode.ColorCodeData(barrel,options, correctedRingList, options.SurfaceColorCode);
               
                var v3StripList = DataConverter.ConvertToCartesian(colorCodedData);
                SavePly(v3StripList, outputFilename,progress);
               
            }
            catch (Exception)
            {
                throw;
            }
        }
       static void SaveFlatPlyFile(Barrel barrel, DataOutputOptions options,CylGridData correctedRingList,double unrollRadius, string fileName, IProgress<int> progress)
        {
            try
            {
                var outputFilename = BuildFileName(fileName,"_flat", ".ply");
                var colorCodedData = DataColorCode.ColorCodeData(barrel, options, correctedRingList, options.SurfaceColorCode);

                var v3FlatStripList = DataParser.UnrollCylinder(colorCodedData,new Vector3(1,1, options.SurfaceFileScaleFactor), unrollRadius);
                 SavePly(v3FlatStripList, outputFilename, progress);
                
            }
            catch (Exception)
            {
                throw;
            }
        }
        static public void SaveSTLFile(Barrel barrel, DataOutputOptions options, CylGridData correctedRingList, double unrollRadius, string fileName, IProgress<int> progress)
        {
            try
            {
                if (options.SaveSurfaceFlat)
                {
                    SaveFlatSTLFile(barrel, options, correctedRingList, unrollRadius, fileName,progress);
                }
                else
                {
                    SaveRolledSTLFile(barrel, options, correctedRingList, unrollRadius, fileName,progress);
                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        static  void SaveFlatSTLFile(Barrel barrel, DataOutputOptions options, CylGridData correctedRingList, double unrollRadius, string fileName, IProgress<int> progress)
        {
            try
            {
                Debug.WriteLine("writing STL file " + fileName);
                var _flatStlFile = new StlFile();
                
                
                var v3flatptList = DataParser.UnrollCylinder(correctedRingList, new Vector3(1,1,options.SurfaceFileScaleFactor), unrollRadius);
                var trimesh = new TriMesh();
                trimesh.BuildFromGrid(v3flatptList);
                StlFile.SaveBinary(trimesh, fileName);
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// build stl files from ring data
        /// </summary>
        /// <param name="correctedRingList"></param>
        /// <param name="flatFileName"></param>
        /// <param name="rolledFileName"></param>
        static void SaveRolledSTLFile(Barrel barrel, DataOutputOptions options, CylGridData correctedRingList, double unrollRadius, string fileName, IProgress<int> progress)
        {
            try
            {
                var v3rolledPtList = DataConverter.ConvertToCartesian(correctedRingList);
                var _flatStlFile = new StlFile();
                var trimesh = new TriMesh();
                trimesh.BuildFromGrid(v3rolledPtList);
                StlFile.SaveBinary(trimesh, fileName);
            }
            catch (Exception)
            {

                throw;
            }
        }
      
       
        static public void SaveSurfaceGridasDXF( CylGridData grid,string fileName, IProgress<int> progress)
        {
            try
            {
                var outputFilename = BuildFileName(fileName, "", ".dxf");
                List<GeometryLib.DwgEntity> entityList = new List<GeometryLib.DwgEntity>();
                List<string> DwgConverterLibList = new List<string>();
                foreach(CylData strip in grid)
                {
                    for(int i =0;i<strip.Count-1;i++)
                    {
                        entityList.Add(BuildLine(strip[i], strip[i + 1]));
                    }
                }
                DxfFileBuilder.Save(entityList, outputFilename, progress);
            }
            catch (Exception)
            {

                throw;
            }
        }
        static DXFLine BuildLine(PointCyl pt1, PointCyl pt2)
        {
            try
            {
                double x1 = pt1.R * Math.Cos(pt1.ThetaRad);
                double y1 = pt1.R * Math.Sin(pt1.ThetaRad);
                double z1 = pt1.Z;
                double x2 = pt2.R * Math.Cos(pt2.ThetaRad);
                double y2 = pt2.R * Math.Sin(pt2.ThetaRad);
                double z2 = pt2.Z;
                RGBColor c;
                c = new RGBColor(0, 255, 0);
                DXFLine line = new DXFLine(x1, y1, z1, x2, y2, z2, c);
                return line;
            }
            catch (Exception)
            {
                throw;
            }           
        }
        static DXFLine BuildLine(Vector3 pt1, Vector3 pt2)
        {
            try
            {
                double x1 = pt1.X;
                double y1 = pt1.Y;
                double z1 = pt1.Z;
                double x2 = pt2.X;
                double y2 = pt2.Y;
                double z2 = pt2.Z;
                RGBColor c;
                c = new RGBColor(0, 255, 0);
                DXFLine line = new DXFLine(x1, y1, z1, x2, y2, z2, c);
                return line;
            }
            catch (Exception)
            {
                throw;
            }
        }
        static public void SaveDXF( CylData strip, string fileName, IProgress<int> progress)
        {

            try
            {
                
                var outputFilename = BuildFileName(fileName, "", ".dxf");
                List<GeometryLib.DwgEntity> entityList = new List<GeometryLib.DwgEntity>();
                List<string> DwgConverterLibList = new List<string>();

                for (int i = 1; i < strip.Count - 1; i++)
                {

                    entityList.Add(BuildLine(strip[i], strip[i + 1]));

                }

                DxfFileBuilder.Save(entityList, outputFilename,progress);
            }
            catch (Exception)
            {

                throw;
            }

        }
        static public void SaveDXF(CartData strip, string fileName, IProgress<int> progress)
        {

            try
            {

                var outputFilename = BuildFileName(fileName, "", ".dxf");
                List<GeometryLib.DwgEntity> entityList = new List<GeometryLib.DwgEntity>();
                List<string> DwgConverterLibList = new List<string>();

                for (int i = 1; i < strip.Count - 1; i++)
                {

                    entityList.Add(BuildLine(strip[i], strip[i + 1]));

                }

                DxfFileBuilder.Save(entityList, outputFilename, progress);
            }
            catch (Exception)
            {

                throw;
            }

        }
        static public void SaveProfileFile(Barrel barrel, DataOutputOptions options,
            BarrelInspProfile barrelInspProfile,string fileName, IProgress<int> progress)
        {
            try
            {
                var headings = BuildFileHeader(barrel, fileName);                
                var filePoints = new List<string>();
                filePoints.AddRange(headings);
                filePoints.Add("Z(in),Min Land Diameter(in),Ave Land Diam(in), Ave Groove Diameter(in)");
                for( int i =0;i<barrelInspProfile.AveGrooveProfile.Count;i++)
                {
                    double aveGrooveDiam = 2.0 * barrelInspProfile.AveGrooveProfile[i].R;
                    double minLandDiam = 2.0 * barrelInspProfile.MinLandProfile[i].R;
                    double aveLandDiam = 2.0 * barrelInspProfile.AveLandProfile[i].R;
                    double z = barrelInspProfile.AveLandProfile[i].Z;
                    string line = string.Concat(z.ToString(), ",", minLandDiam.ToString(), ",", aveLandDiam.ToString(), ",",
                        aveGrooveDiam.ToString() );
                    filePoints.Add(line);
                }
                FileIOLib.FileIO.Save(filePoints, fileName);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// output csv file
        /// </summary>
        /// <param name="ring"></param>
        /// <param name="csvFileName"></param>
        static public void SaveCSVFile(Barrel barrel, DataOutputOptions options, 
            CylData ring, string fileName, IProgress<int> progress)
        {
            try
            {
                //var outputFilename = BuildFileName(fileName,"_out", ".csv");
                var headings = BuildFileHeader(barrel, fileName);
                
                var filePoints = new List<string>();
                filePoints.AddRange(headings);
                filePoints.Add("ID,Theta(Radians),R(in),Z(in)");
                foreach (var pt in ring)
                {
                    string line = string.Concat(pt.ID.ToString() + "," + pt.ThetaRad.ToString(), ",", pt.R.ToString(), ",", pt.Z.ToString());
                    filePoints.Add(line);
                }                
                FileIOLib.FileIO.Save(filePoints, fileName);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        /// <summary>
        /// output csv file
        /// </summary>
        /// <param name="ring"></param>
        /// <param name="csvFileName"></param>
        static public void SaveCSVFile(Barrel barrel, DataOutputOptions options,
            CartData data, string fileName, IProgress<int> progress)
        {
            try
            {
                //var outputFilename = BuildFileName(fileName,"_out", ".csv");
                var headings = BuildFileHeader(barrel, fileName);

                var filePoints = new List<string>();
                filePoints.AddRange(headings);
                filePoints.Add("ID,X(in),Y(in),Z(in)");
                foreach (var pt in data)
                {
                    string line = string.Concat(pt.ID.ToString() + "," + pt.X.ToString(), ",", pt.Y.ToString(), ",", pt.Z.ToString());
                    filePoints.Add(line);
                }
                FileIOLib.FileIO.Save(filePoints, fileName);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// output csv file from ring data
        /// </summary>
        /// <param name="correctedRingList"></param>
        /// <param name="csvFileName"></param>
        static public void SaveCSVFile(Barrel barrel,DataOutputOptions options, CylGridData correctedRingList, string fileName, IProgress<int> progress)
        {
            try
            {
                
                var headings = BuildFileHeader(fileName);
               
                var filePoints = new List<string>();
                filePoints.AddRange(headings);
                filePoints.Add("ID,Theta(Radians),R(in),Z(in)");
                foreach (var ring in correctedRingList)
                {
                    foreach (var pt in ring)
                    {
                        string line = string.Concat(pt.ID.ToString() + "," + pt.ThetaRad.ToString(), ",",pt.R.ToString() , ",", pt.Z.ToString());
                        filePoints.Add(line);
                    }
                }
                FileIOLib.FileIO.Save(filePoints, fileName);
            }
            catch (Exception)
            {
                
                throw;
            }


        }
      
    }
}
