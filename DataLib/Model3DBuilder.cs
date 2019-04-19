using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DataLib
{
    public class Model3DBuilder
    {
        private int xIndexMin, xIndexMax, dxIndex, zIndexMin, zIndexMax, dzIndex;
        private double texture_xscale, texture_zscale;
        private Model3DGroup model_group;

        // Create the altitude map texture bitmap.
        private void CreateAltitudeMap(double[,] values, double maxToleranceValue, COLORCODE colorcode)
        {
            try
            {
                // Calculate the function's value over the area.
                int xwidth = values.GetUpperBound(0) + 1;
                int zwidth = values.GetUpperBound(1) + 1;
                double dx = (xIndexMax - xIndexMin) / xwidth;
                double dz = (zIndexMax - zIndexMin) / zwidth;

                // Get the upper and lower bounds on the values.
                var get_values =
                    from double value in values
                    select value;
                double ymin = get_values.Min();
                double ymax = get_values.Max();

                // Make the BitmapPixelMaker.
                BitmapPixelMaker bm_maker = new BitmapPixelMaker(xwidth, zwidth);
                double[] contours = new double[10];
                double dc = (ymax - ymin) / contours.Length;
                for (int ci = 0; ci < contours.Length; ci++)
                {
                    contours[ci] = ci * dc + ymin;
                }
                // Set the pixel colors.
                for (int ix = 0; ix < xwidth; ix++)
                {
                    for (int iz = 0; iz < zwidth; iz++)
                    {
                        RGBColor color = new RGBColor(100, 100, 100, 255);
                        switch (colorcode)
                        {
                            case COLORCODE.CONTOURS:
                                color = ColorCoder.MapContours(values[ix, iz], ymin, ymax, contours);
                                break;
                            case COLORCODE.GREEN_RED:
                                color = ColorCoder.MapGreenRedColor(values[ix, iz], maxToleranceValue);
                                break;
                            case COLORCODE.MONO:
                                color = ColorCoder.MapMonoColor();
                                break;
                            case COLORCODE.MONO_RED:
                                color = ColorCoder.MapMonoRedColor(values[ix, iz], maxToleranceValue);
                                break;
                            case COLORCODE.RAINBOW:
                            default:
                                color = ColorCoder.MapRainbowColor(values[ix, iz], ymin, ymax);
                                break;
                        }

                        bm_maker.SetPixel(ix, iz, color);
                    }
                }

                // Convert the BitmapPixelMaker into a WriteableBitmap.
                WriteableBitmap wbitmap = bm_maker.MakeBitmap(96, 96);
                string filename = "texture.png";
                
                // Save the bitmap into a file.
                wbitmap.Save(filename);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        double dxInput;
        double dxTarget;
        double dzInput;
        double dzTarget;

        private double[,] BuildHeightArray(CylGridData scan, double radialDirection, double nominalRadius, double scalingFactor)
        {
            try
            {
                int zCount = scan.Count;
                int xCount = scan[0].Count;
                //find smallest strip to dim array
                foreach (CylData strip in scan)
                {
                    if (strip.Count < xCount)
                    {
                        xCount = strip.Count;
                    }
                }
                double[,] inputValues = new double[xCount, zCount];
                int zi = 0;
                //move strip values into array of heights
                foreach (CylData strip in scan)
                {
                    for (int xi = 0; xi < xCount; xi++)
                    {
                        inputValues[xi, zi] = Math.Sign(radialDirection) * strip[xi].R;
                    }
                    zi++;
                }
                //calc x and z spacing 
                dxInput =  Math.Abs(nominalRadius * (scan[0][0].ThetaRad - scan[0][1].ThetaRad));
                dzInput = Math.Abs(scan[0][0].Z - scan[1][0].Z);
                dxTarget = .004;
                dzTarget = .004;
                int xSpacing = 1;
                double xScaling = dxTarget / dxInput;
                if (xScaling > 1)
                {
                    xSpacing = (int)Math.Ceiling(xScaling);
                }
                else
                {
                    xSpacing = 1;
                }
                int zOutCount = (int)(zCount * dzInput / dzTarget);
                int xOutCount = xCount / xSpacing;

                double[,] valuesXAve = new double[xOutCount, zCount];

                double clippingValue = 20 * _maxToleranceValue;
                //average x values 
                for (int zIndex = 0; zIndex < inputValues.GetUpperBound(1); zIndex++)
                {
                        int xi = 0;
                        int xstart = 0;
                        while (xi < xOutCount)
                        {
                            int xend = Math.Min(xstart + xSpacing, inputValues.GetUpperBound(0));
                            double sum = 0;
                            for (int xsub = xstart; xsub < xend; xsub++)
                            {
                                sum += inputValues[xsub, zIndex];
                            }
                            int sumCount = xend - xstart;
                            sum /= sumCount;
                            valuesXAve[xi, zIndex] = sum * scalingFactor;
                            xi++;
                            xstart += xSpacing;
                        }

                }
                

                double[,] valuesOut = new double[xOutCount, zOutCount];


                //poly fit z values
                for (int xio = 0; xio < xOutCount; xio++)
                {

                    int zOutIndex = 0;
                    for (int zio = 0; zio < zCount - 2; zio++)
                    {
                        double y0 = valuesXAve[xio, zio];
                        double z0 = zio * dzInput;
                        double z1 = (zio + 1) * dzInput;

                        double z = z0;
                        double[] x = new double[3];
                        double[] y = new double[3];
                        y[0] = valuesXAve[xio, zio];
                        y[1] = valuesXAve[xio, zio + 1];
                        y[2] = valuesXAve[xio, zio + 2];
                        x[0] = zio * dzInput;
                        x[1] = (zio + 1) * dzInput;
                        x[2] = (zio + 2) * dzInput;
                        var func = MathNet.Numerics.Fit.PolynomialFunc(x, y, 2);                        
                        while (z < z1 && zOutIndex < zOutCount)
                        {
                            valuesOut[xio, zOutIndex] = func(z);
                            z += dzTarget;
                            zOutIndex++;
                        }
                    }
                }
                xIndexMin = 0;
                xIndexMax = valuesOut.GetUpperBound(0);
                dxIndex = 1;
                zIndexMin = 0;
                zIndexMax = valuesOut.GetUpperBound(1);
                dzIndex = 1;

                texture_xscale = (xIndexMax - xIndexMin);
                texture_zscale = (zIndexMax - zIndexMin);

                return valuesOut;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        private void DefineLights()
        {
            try
            {
                AmbientLight ambient_light = new AmbientLight(Colors.DarkGray);
                DirectionalLight directional_light = new DirectionalLight(Colors.Gray, new Vector3D(-1.0, -3.0, -2.0));
                model_group.Children.Add(ambient_light);
                model_group.Children.Add(directional_light);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        // A dictionary to hold points for fast lookup.
        private Dictionary<Point3D, int> pointDictionary;

        // If the point already exists, return its index.
        // Otherwise create the point and return its new index.
        private int AddPoint(Point3DCollection points, PointCollection texture_coords, Point3D point)
        {
            try
            {
                // If the point is in the point dictionary,
                // return its saved index.
               // if (pointDictionary.ContainsKey(point))
               //     return pointDictionary[point];

                // We didn't find the point. Create it.
                points.Add(point);
               // pointDictionary.Add(point, points.Count - 1);

                // Set the point's texture coordinates.
                texture_coords.Add(
                    new System.Windows.Point(
                        (point.X - xIndexMin) * texture_xscale,
                        (point.Z - zIndexMax) * texture_zscale));

                // Return the new point's index.
                return points.Count - 1;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        private void AddTriangle(MeshGeometry3D mesh, Point3D point1, Point3D point2, Point3D point3)
        {
            try
            {
                // Get the points' indices.
                int index1 = AddPoint(mesh.Positions, mesh.TextureCoordinates, point1);
                int index2 = AddPoint(mesh.Positions, mesh.TextureCoordinates, point2);
                int index3 = AddPoint(mesh.Positions, mesh.TextureCoordinates, point3);

                // Create the triangle.
                mesh.TriangleIndices.Add(index1);
                mesh.TriangleIndices.Add(index2);
                mesh.TriangleIndices.Add(index3);
            }
            catch (Exception)
            {

                throw;
            }
          
        }
        // Add the model to the Model3DGroup.
        private void DefineModel(double[,] values)
        {
            try
            {
                // Make a mesh to hold the surface.
                MeshGeometry3D mesh = new MeshGeometry3D();
                pointDictionary = new Dictionary<Point3D, int>();
                // Make the surface's points and triangles.
                var midIndex_x = xIndexMax / 2.0;
                var midIndex_z = zIndexMax / 2.0;
                
                for (int x = xIndexMin; x < xIndexMax ; x ++)
                {
                    for (int z = zIndexMin; z < zIndexMax ; z ++)
                    {
                        // Make points at the corners of the surface
                        // over (x, z) - (x + dx, z + dz).
                        double x0 = (x - midIndex_x) * dxTarget;
                        double z0 = (z - midIndex_z) * dzTarget;
                        double x1 = x0 + dxTarget;
                        double z1 = z0 + dzTarget;
                        Point3D p00 = new Point3D(x0, values[x, z], z0);
                        Point3D p10 = new Point3D(x1, values[x + 1, z], z0);
                        Point3D p01 = new Point3D(x0, values[x, z + 1], z1);
                        Point3D p11 = new Point3D(x1, values[x + 1, z + 1], z1);

                        // Add the triangles.
                        AddTriangle(mesh, p00, p01, p11);
                        AddTriangle(mesh, p00, p11, p10);
                    }
                }


                // Make the surface's material using an image brush.
                ImageBrush texture_brush = new ImageBrush();
                //ImageBrush grid_brush = new ImageBrush();
                string filename = "texture.png";
                //filename = "Grid.png";
                var  uri = new Uri(filename,UriKind.Relative);
                texture_brush.ImageSource = new BitmapImage(uri);
                //grid_brush.ImageSource = new BitmapImage(new Uri(filename, UriKind.Relative));
                var surface_material = new DiffuseMaterial(texture_brush );
                //var surface_material = new DiffuseMaterial(grid_brush);
                // Make the mesh's model.
                GeometryModel3D surface_model = new GeometryModel3D(mesh, surface_material);

                var xOrigin = (xIndexMin - midIndex_x) * dxTarget;
                var yOrigin = 0;
                var zOrigin = (zIndexMin - midIndex_z) * dzTarget;


                var axis = new Rect3D(xOrigin, yOrigin, zOrigin, dxIndex * dzInput, dxIndex * dxTarget, dzIndex * dzTarget);
                var axisBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                var axisMaterial = new DiffuseMaterial(axisBrush);
                //var axisModel = new GeometryModel3D(axis, axisMaterial);
                // Make the surface visible from both sides.
                surface_model.BackMaterial = surface_material;

                // Add the model to the model groups.
                model_group.Children.Add(surface_model);
               // model_group.Children.Add(axis);
            }
            catch (Exception)
            {

                throw;
            }
            

        }
        double _maxToleranceValue;
        public void BuildModel(ref Model3DGroup modelgroup, CylGridData spiralScan, double radialDirection, double nominalRadius, double maxToleranceValue, double scalingFactor, COLORCODE colorCode)
        {
            try
            {
                model_group = modelgroup;
                _maxToleranceValue = maxToleranceValue;
                DefineLights();
                var values = BuildHeightArray(spiralScan, radialDirection, nominalRadius, scalingFactor);
                CreateAltitudeMap(values, maxToleranceValue * scalingFactor, colorCode);
                DefineModel(values);
            }
            catch (Exception)
            {

                throw;
            }
           

        }
    }
}
