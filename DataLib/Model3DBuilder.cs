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

            // Save the bitmap into a file.
            wbitmap.Save("Texture.png");
        }

        double dxInput;
        double dxTarget;
        double dzInput;
        double dzTarget;

        private double[,] MakeData(CylGridData scan, double radialDirection, double nominalRadius, double scalingFactor)
        {
            int zCount = scan.Count;
            int xCount = scan[0].Count;
            foreach (CylData strip in scan)
            {
                if (strip.Count < xCount)
                {
                    xCount = strip.Count;
                }
            }
            double[,] inputValues = new double[xCount, zCount];
            int zi = 0;
            foreach (CylData strip in scan)
            {
                for (int xi = 0; xi < xCount; xi++)
                {
                    inputValues[xi, zi] = Math.Sign(radialDirection) * strip[xi].R;
                }
                zi++;
            }



            var p00 = new PointCyl(scan[0][0].R + nominalRadius, scan[0][0].ThetaRad, scan[0][0].Z);
            var p01 = new PointCyl(scan[0][1].R + nominalRadius, scan[0][1].ThetaRad, scan[0][1].Z);
            var p10 = new PointCyl(scan[1][0].R + nominalRadius, scan[1][0].ThetaRad, scan[1][0].Z);
            dxInput = p00.DistanceTo(p01);
            dzInput = p00.DistanceTo(p10);
            dxTarget = .002;
            dzTarget = .002;
            int xSpacing = 1;
            double xScaling = dxTarget / dxInput;
            if (xScaling > 1)
            {
                xSpacing = (int)Math.Ceiling(xScaling);
            }

            int zOutCount = (int)(zCount * dzInput / dzTarget);
            int xOutCount = xCount / xSpacing;

            double[,] valuesXAve = new double[xOutCount, zCount];

            double clippingValue = 20 * _maxToleranceValue;

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

        private void DefineLights()
        {
            AmbientLight ambient_light = new AmbientLight(Colors.Gray);
            DirectionalLight directional_light = new DirectionalLight(Colors.Gray, new Vector3D(-1.0, -3.0, -2.0));
            model_group.Children.Add(ambient_light);
            model_group.Children.Add(directional_light);
        }

        // A dictionary to hold points for fast lookup.
        private Dictionary<Point3D, int> pointDictionary;

        // If the point already exists, return its index.
        // Otherwise create the point and return its new index.
        private int AddPoint(Point3DCollection points, PointCollection texture_coords, Point3D point)
        {

            // If the point is in the point dictionary,
            // return its saved index.
            if (pointDictionary.ContainsKey(point))
                return pointDictionary[point];

            // We didn't find the point. Create it.
            points.Add(point);
            pointDictionary.Add(point, points.Count - 1);

            // Set the point's texture coordinates.
            texture_coords.Add(
                new System.Windows.Point(
                    (point.X - xIndexMin) * texture_xscale,
                    (point.Z - zIndexMax) * texture_zscale));

            // Return the new point's index.
            return points.Count - 1;
        }
        private void AddTriangle(MeshGeometry3D mesh, Point3D point1, Point3D point2, Point3D point3)
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
        // Add the model to the Model3DGroup.
        private void DefineModel(double[,] values)
        {
            // Make a mesh to hold the surface.
            MeshGeometry3D mesh = new MeshGeometry3D();
            pointDictionary = new Dictionary<Point3D, int>();
            // Make the surface's points and triangles.
            float offset_x = xIndexMax / 2f;
            float offset_z = zIndexMax / 2f;
            double xScaling = dxTarget;
            double zScaling = dzTarget;
            for (int x = xIndexMin; x <= xIndexMax - dxIndex; x += dxIndex)
            {
                for (int z = zIndexMin; z <= zIndexMax - dzIndex; z += dzIndex)
                {
                    // Make points at the corners of the surface
                    // over (x, z) - (x + dx, z + dz).
                    double x0 = (x - offset_x) * xScaling;
                    double z0 = (z - offset_z) * zScaling;
                    double x1 = x0 + xScaling;
                    double z1 = z0 + zScaling;
                    Point3D p00 = new Point3D(x0, values[x, z], z0);
                    Point3D p10 = new Point3D(x1, values[x + dxIndex, z], z0);
                    Point3D p01 = new Point3D(x0, values[x, z + dzIndex], z1);
                    Point3D p11 = new Point3D(x1, values[x + dxIndex, z + dzIndex], z1);

                    // Add the triangles.
                    AddTriangle(mesh, p00, p01, p11);
                    AddTriangle(mesh, p00, p11, p10);
                }
            }


            // Make the surface's material using an image brush.
            ImageBrush texture_brush = new ImageBrush();

            texture_brush.ImageSource = new BitmapImage(new Uri("Texture.png", UriKind.Relative));
            DiffuseMaterial surface_material = new DiffuseMaterial(texture_brush);

            // Make the mesh's model.
            GeometryModel3D surface_model = new GeometryModel3D(mesh, surface_material);

            // Make the surface visible from both sides.
            surface_model.BackMaterial = surface_material;

            // Add the model to the model groups.
            model_group.Children.Add(surface_model);

        }
        double _maxToleranceValue;
        public void BuildModel(ref Model3DGroup modelgroup, CylGridData spiralScan, double radialDirection, double nominalRadius, double maxToleranceValue, double scalingFactor, COLORCODE colorCode)
        {
            model_group = modelgroup;
            _maxToleranceValue = maxToleranceValue;
            DefineLights();
            var values = MakeData(spiralScan, radialDirection, nominalRadius, scalingFactor);
            CreateAltitudeMap(values, maxToleranceValue * scalingFactor, colorCode);
            DefineModel(values);

        }
    }
}
