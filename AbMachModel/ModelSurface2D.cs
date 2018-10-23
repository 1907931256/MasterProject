using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbMachModel
{
    class ModelSurface2DGrid
    {
        double[,] values;

        public void setValue(double value,int i,int j)
        {
            if (i >= 0 && i < xSize && j >= 0 && j < ySize)
            {
                values[i, j] = value;
            }

        }
        public void initValue(double value)
        {
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < ySize; j++)
                {
                    values[i, j] = value;
                }
            }
        }
        public double getValue(int i, int j)
        {
            double value = 0;
            if (i < xSize && j < ySize && i >= 0 && j >= 0)
                value = values[i, j];

            return value;
        }
        public double getValue(double xPosition, double yPosition)
        {
            int i = getXindex(xPosition);
            int j = getYindex(yPosition);
            return getValue(i, j);

        }
        public int getXindex(double val)
        {
            int index = -1;
            index = (int)Math.Round(xSize*(val-xMin)/(xMax-xMin));
            if (index >= xSize) index = xSize - 1;
            if (index < 0) index = 0;
            return index;
        }
        public int getYindex(double val)
        {
            int index = -1;
            index = (int)Math.Round(ySize * (val - yMin) / (yMax - yMin));
            if (index >= ySize) index = ySize - 1;
            if (index < 0) index = 0;

            return index;
        }
        public DrawingIO.Vector3 Normal(double xPosition, double yPosition)
        {
            int i = getXindex(xPosition);
            int j = getYindex(yPosition);
            double zx1 = getValue(i - 1, j);
            double zx2 = getValue(i + 1, j);
            double zy1 = getValue(i, j - 1);
            double zy2 = getValue(i, j + 1);
            var vx = new DrawingIO.Vector3(2 * meshSize, 0, zx2 - zx1);
            var vy = new DrawingIO.Vector3(0, 2 * meshSize, zy2 - zy1);
            return vx.Cross(vy);
        }
        public double MeshSize { get { return meshSize; } }

        double meshSize;
        int xSize;
        int ySize;
        double xMin;
        double yMin;
        double xMax;
        double yMax;

        public ModelSurface2DGrid(double xMinIn, double yMinIn, double xMaxIn, double yMaxIn, double meshSizeIn)
        {
            meshSize = meshSizeIn;
            xSize = Convert.ToInt32(Math.Round((xMaxIn - xMinIn) / meshSize));
            ySize = Convert.ToInt32(Math.Round((yMaxIn - yMinIn) / meshSize));
            values = new double[xSize, ySize];
            
        }


    }
}
