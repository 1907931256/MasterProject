using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbMachModel
{
    public class MatRemovalEquation
    {
        double[] mrrCoeff;
        static int maxIndex = 7;

        /// <summary>
        /// returns integral of mrr equation using trapezoid rule
        /// </summary>
        /// <param name="meshSize"></param>
        /// <returns></returns>
        public double Integral(double meshSize)
        {
            double result = 0;
            
            double dr = meshSize;
            double r = dr;
            while (r <= 1)
            {
                result += 0.5 * (GetValueAt(r) + GetValueAt(r - dr)) * meshSize;
                r += dr;
            }
            return result;
        }
        /// <summary>
        /// returns value at radius from center of jet
        /// </summary>
        /// <param name="r">x coord from 0-1</param>
        /// <returns></returns>
        public double GetValueAt(double r)
        {
            double value = mrrCoeff[7];
            int k;
            for (k = 6; k >= 0; k--)
            {
                value = (value * r) + mrrCoeff[k];
            }
            return value;

        }
        /// <summary>
        /// select shape of mrr equation all 6th-order polynomials defined from 0-1
        /// </summary>
        /// <param name="mrrType"></param>
        private void selectType(int mrrType)
        {
            switch (mrrType)
            {
                case 1:
                    // 'constant with r
                    mrrCoeff[0] = 1;
                    mrrCoeff[1] = 0;
                    mrrCoeff[2] = 0;
                    mrrCoeff[3] = 0;
                    mrrCoeff[4] = 0;
                    mrrCoeff[5] = 0;
                    mrrCoeff[6] = 0;
                    mrrCoeff[7] = 0.23;
                    break;
                case 2:
                    //'based on shape of particle density experiments
                    //'peak at .4 r
                    mrrCoeff[0] = 0.621;
                    mrrCoeff[1] = 0.896;
                    mrrCoeff[2] = -10.67;
                    mrrCoeff[3] = 67.71;
                    mrrCoeff[4] = -161.37;
                    mrrCoeff[5] = 154.76;
                    mrrCoeff[6] = -51.95;
                    mrrCoeff[7] = 0.202;
                    break;
                case 3:
                    //broad peak at .5 to .6 r
                    mrrCoeff[0] = 0.744;
                    mrrCoeff[1] = -0.28;
                    mrrCoeff[2] = 8.31;
                    mrrCoeff[3] = -29.03;
                    mrrCoeff[4] = 47.56;
                    mrrCoeff[5] = -38.66;
                    mrrCoeff[6] = 11.39;
                    mrrCoeff[7] = 0.195;
                    break;
                case 4:
                    //broad peak at .6 to .7 r
                    mrrCoeff[0] = 0.669;
                    mrrCoeff[1] = 0.107;
                    mrrCoeff[2] = 1.54;
                    mrrCoeff[3] = 6.74;
                    mrrCoeff[4] = -34.16;
                    mrrCoeff[5] = 48.08;
                    mrrCoeff[6] = -22.99;
                    mrrCoeff[7] = 0.75;
                    break;
                case 5:
                    //broad peak at .4 r
                    mrrCoeff[0] = 0.662;
                    mrrCoeff[1] = -1.011;
                    mrrCoeff[2] = 18.407;
                    mrrCoeff[3] = -60.678;
                    mrrCoeff[4] = 83.411;
                    mrrCoeff[5] = -50.819;
                    mrrCoeff[6] = 10.061;
                    mrrCoeff[7] = 0.28;
                    break;
                case 6:
                    //'gaussian =.6 at r=1
                    mrrCoeff[0] = 1;
                    mrrCoeff[1] = 0.1837;
                    mrrCoeff[2] = -6.7828;
                    mrrCoeff[3] = 9.2964;
                    mrrCoeff[4] = -3.6865;
                    mrrCoeff[5] = 0;
                    mrrCoeff[6] = 0;
                    mrrCoeff[7] = 0.24;
                    break;
                case 7:
                default:
                    mrrCoeff[0] = 0.65;
                    mrrCoeff[1] = 0;
                    mrrCoeff[2] = 0.85;
                    mrrCoeff[3] = 0;
                    mrrCoeff[4] = 0;
                    mrrCoeff[5] = 0;
                    mrrCoeff[6] = 0;
                    mrrCoeff[7] = 0;
                    break;
            }
        }
        public MatRemovalEquation()
        {
            mrrCoeff = new double[8];
            selectType(13);
        }

        public MatRemovalEquation(int equationIndex)
        {
            mrrCoeff = new double[8];
            selectType(equationIndex);
        }
        public MatRemovalEquation(double[] coeffs)
        {           
            if (coeffs.Length == 8)
            {
                mrrCoeff = coeffs;
            }
            else
            {
                mrrCoeff = new double[8];
                selectType(13);
            }

        }
    }
}
