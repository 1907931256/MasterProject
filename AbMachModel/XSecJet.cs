using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileIOLib;
namespace AbMachModel
{
    public class XSecJet
    {
        public double Diameter { get; private set; }
        //linear interpolate between scan points to get mrr
        public double GetMrr(double xJet)
        {

            double xnorm = Math.Min(1, Math.Abs(2 * xJet / Diameter));
            double mrr = 0;
            for (int i = 0; i < mrrList.Count - 1; i++)
            {
                double x1 = mrrList[i].Item1;
                double x2 = mrrList[i + 1].Item1;
                double mrr1 = mrrList[i].Item2;
                double mrr2 = mrrList[i + 1].Item2;
                if (xnorm >= x1 && xnorm < x2)
                {
                    double m = (mrr2 - mrr1) / (x2 - x1);
                    mrr = ((xnorm - x1) * m) + mrr1;
                    break;
                }
            }
            return mrr;
        }

        List<Tuple<double, double>> mrrList;
        //build jet from normalized csv scan of jet. x and r normalized to 1
        private void BuildJet(string csvFilename)
        {
            var stringArr = CSVFileParser.ParseFile(csvFilename);
            //x,mrr csvfilename 
            int headerRowCount = 1;
            int rowCount = stringArr.GetLength(0);
            int colCount = stringArr.GetLength(1);
            mrrList = new List<Tuple<double, double>>();
            for (int i = headerRowCount; i < rowCount; i++)
            {
                double x = 0;

                double mrr = 0;
                if (double.TryParse(stringArr[i, 0], out x) && double.TryParse(stringArr[i, 1], out mrr))
                {
                    mrrList.Add(new Tuple<double, double>(x, mrr));
                }
            }

        }
        void BuildJet()
        {
            mrrList = new List<Tuple<double, double>>();
            mrrList.Add(new Tuple<double, double>(0, 1));
            mrrList.Add(new Tuple<double, double>(1, 1));
        }

        //build jet from csv file scan profile normalized 
        public XSecJet(string csvJetProfile, double jetDiameter)
        {
            Diameter = jetDiameter;
            BuildJet(csvJetProfile);
        }
        public XSecJet(double jetDiameter)
        {
            Diameter = jetDiameter;
            BuildJet();
        }

    }
}
