using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileIOLib;
namespace AbMachModel
{
    public class MRRFunction
    {
        double[] cfs;
        public Func<double, double> GetFunc()
        {
            return x => cfs[0] + cfs[1] * x + cfs[2] * Math.Pow(x, 2) + cfs[3] * Math.Pow(x, 3) + cfs[4] * Math.Pow(x, 4) + cfs[5] * Math.Pow(x, 5) + cfs[6] * Math.Pow(x, 6);
        }
        public MRRFunction(int equationIndex)
        {
            MrrEquDictionary equations = MrrEquFile.Open();
            cfs = equations.GetEquation(equationIndex);
        }
    }
    /// <summary>
    /// used by abmachjet class
    /// </summary>
    public class MRREquation
    {
        double[] mrrEquation;
        double[] mrrValues;
        double[,] mrrValueMatrix;
        double[] mrrRowSums;
        double mrrSum;
        int matSize;
        private static int mrrValueCount = 1010;
        
        /// <summary>
        /// returns value at radius from center of jet
        /// </summary>
        /// <param name="r">x coord from 0-1</param>
        /// <returns></returns>
        internal double GetPointAt(double r)
        {
            int rIndex = (int)Math.Round(r * 1000.0);
            return mrrValues[rIndex];
           
        }
        internal double GetPointAt(int jetXIndex,int jetYIndex)
        {

            int i =  Math.Abs(jetXIndex);
            int j =  Math.Abs(jetYIndex);
            return (i < matSize && j < matSize) ? mrrValueMatrix[i, j] : 0;
        }
      
        internal double GetSumAt(int jetRIndex )
        {
          int i = Math.Abs(jetRIndex);
          return (i < matSize ) ? mrrRowSums[i] : 0;
        }
        private double calcValueAt(double r)
        {
            double value = mrrEquation[6];
            int k;
            for (k = 5; k >= 0; k--)
            {
                value = (value * r) + mrrEquation[k];
            }
            value *= mrrEquation[7];

            return (value >= 0) ? value : 0;
        }
        private void fillValues(double integralValue)
        {
            List<string> mrr = new List<string>();
           
            for (int i = 0; i < mrrValueCount; i++)
            {
                double r = i / 1000.0;
                double mrrV = calcValueAt(r);
                mrrValues[i] = mrrV/integralValue;
                
                mrr.Add(i.ToString() + "," + mrrV.ToString("F6"));
            }
           
            FileIO.Save(mrr, "mrrValues.csv");
        }
        private double calcIntegral()
        {
            mrrSum = 0;
            for (int i = 0; i < mrrValueCount; i++)
            {
                double r = i / 1000.0;
                double mrrV = calcValueAt(r);
                mrrSum += mrrV / 1000.0;
            }
            mrrSum *= 2;
            return mrrSum;
        }
        private void fillMatrix(double meshSize, double jetRadius, double integralValue)
        {
            matSize = (int)(Math.Round(jetRadius / meshSize)) + 1;

            mrrValueMatrix = new double[matSize, matSize];
            mrrRowSums = new double[matSize];
           
            for (int i = 0; i < matSize; i++)
            {
                double sum = 0;
                for (int j = 0; j < matSize; j++)
                {
                    double r = (Math.Sqrt(Math.Pow(i*meshSize, 2) + Math.Pow(j*meshSize, 2))) / jetRadius;
                    double mrrv = 0; 
                    if (r<=1.0)
                    {
                         mrrv = calcValueAt(r) / integralValue;
                    }
                    
                    mrrValueMatrix[i, j] = mrrv;
                    sum += mrrv;
                }
                mrrRowSums[i] = sum;
            }
        }
       
        internal MRREquation(int equationIndex, double meshSize, double jetRadius,string fileName)
        {           
            MrrEquDictionary equations = MrrEquFile.Open(fileName);
            mrrEquation = equations.GetEquation(equationIndex);
            mrrValues = new double[mrrValueCount];
            mrrSum = calcIntegral();
            fillValues(mrrSum);
            fillMatrix(meshSize, jetRadius, mrrSum);
        }
        internal MRREquation(int equationIndex,double meshSize, double jetRadius)
        {
            MrrEquDictionary equations = MrrEquFile.Open();
            mrrEquation = equations.GetEquation(equationIndex);
            mrrValues = new double[mrrValueCount];
            mrrSum = calcIntegral();
            if(mrrSum==0)
            {
                mrrSum=1;
            }
            fillValues(mrrSum);
            fillMatrix(meshSize, jetRadius, mrrSum);
        }
       
    }
}
