using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AbMachModel
{
    public class MrrEquDictionary
    {
        Dictionary<int, double[]> eqDictionary;

        List<int> indexList;
        double[] defaultCoeffs;
        public int Length
        {
            get
            {
                return eqDictionary.Count;
            }
        }
        public void AddEquation(int index, double[] coeffs)
        {
            eqDictionary.Add(index, coeffs);
            indexList.Add(index);

        }
        public List<int> IndexList
        {
            get
            {
                return indexList;
            }
        }
        public double[] GetEquation(int index)
        {
            double[] equOut = new double[8];
            if (eqDictionary.TryGetValue(index, out equOut))
            {
                return equOut;
            }
            return defaultCoeffs;
        }
        public MrrEquDictionary()
        {
            eqDictionary = new Dictionary<int, double[]>();
            defaultCoeffs = new double[8] { 1, 0, 0, 0, 0, 0, 0, 0.23 };
            indexList = new List<int>();
        }
    }
}
