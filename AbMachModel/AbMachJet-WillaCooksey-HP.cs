using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbMachModel
{
    public class AbMachJet
    {
       
        double diameter;
        MatRemovalEquation mrrEquation;
        double meshSize;
        double[,] mrrValues;
        int jetRadius;
        int jetDiameter;
        int equationIndex;

        public int EquationIndex { get { return equationIndex; } }

        public double Diameter { get { return diameter; } }

        public double FootPrint(int x, int y)
        {

            if (x >= 0 && x < mrrValues.GetLength(0) && y >= 0 && y < mrrValues.GetLength(1))
            {
                return mrrValues[x, y];
            }
            else
            {
                return 0;
            }

        }
       
        public int JetRadius
        {
            get
            {
                return jetRadius;
            }
        }


        double removalRate(double radius)
        {            
            return (radius >= 0 && radius <= 1) ? mrrEquation.GetValueAt(radius) : 0;
        }
        double diameterFactor(double diameter)
        {
            return .5896 * Math.Pow(diameter, -.23);
        }
        void fillFootPrint()
        {
            for (int i = 0; i < jetDiameter; i++)
            {
                for (int j = 0; j < jetDiameter; j++)
                {
                    double radius = Math.Sqrt(Math.Pow(i - jetRadius, 2) + Math.Pow(j - jetRadius, 2))/jetRadius;
                    mrrValues[i, j] = removalRate(radius);
                }
            }
        }

        void normalize()
        {
            
            double sum = 0;
            for (int j = 0; j < mrrValues.GetLength(1)-1; j++)
            {
                for (int i = 0; i < mrrValues.GetLength(0)-1; i++)
                {
                    sum += meshSize * meshSize * (mrrValues[i, j] + mrrValues[i + 1, j] + mrrValues[i, j + 1] + mrrValues[i + 1, j + 1]) / 4;
                }
            }


            for (int i = 0; i < mrrValues.GetLength(0); i++)
            {
                for (int j = 0; j < mrrValues.GetLength(1); j++)
                {
                    mrrValues[i, j] /= sum ;
                }
            }
        }

        public AbMachJet(double diameter, double meshSize, int equationIndex)
        {
            this.equationIndex = equationIndex;
            mrrEquation = new MatRemovalEquation(equationIndex);
            this.diameter = diameter;
            jetRadius = (int)Math.Round(.5 * diameter / meshSize);
            jetDiameter = 2 * jetRadius;

            mrrValues = new double[jetDiameter, jetDiameter];
            fillFootPrint();
            normalize();
        }
        public AbMachJet()
        {
            mrrEquation = new MatRemovalEquation();
            diameter = 1;
            jetRadius = 1;
            jetDiameter = 2 * jetRadius;
            mrrValues = new double[jetDiameter, jetDiameter];

        }
    }
}
