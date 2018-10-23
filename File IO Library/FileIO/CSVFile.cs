using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileIOLib
{

    public class CSVFileParser 
    { 
        static public string[,] ParseFile(string fileName)
        {
            try
            {
                List<string> dataList = FileIO.ReadDataTextFile(fileName);
                string[] firstLine = dataList[0].Split(',');
                int rowCount = dataList.Count;
                int colCount = firstLine.Length;

                var outputArr = new string[rowCount, colCount];
                for (int i = 0; i < rowCount; i++)
                {
                    string[] words = dataList[i].Split(',');
                    int TempColCount = Math.Min(words.Length, colCount);
                    for (int j = 0; j < TempColCount; j++)
                    {
                        outputArr[i, j] = words[j];
                    }
                }
                return outputArr;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        static public double[,] ToArray(string fileName,int headerRowCount)
        {
            try
            {
                List<string> dataList = FileIO.ReadDataTextFile(fileName);

                int columns = 0;
                int rows = 0;
                double[,] data = new double[rows, columns]; ;

                if (dataList.Count > headerRowCount)
                {
                    string[] words = dataList[headerRowCount].Split(',');
                    columns = words.Length;
                    rows = dataList.Count - headerRowCount;
                    double result = 0;
                    data = new double[rows, columns];
                    int row = 0;
                    int col = 0;

                    for (int i = headerRowCount; i < dataList.Count; i++)
                    {
                        string line = dataList[i];
                        words = line.Split(',');
                        col = 0;
                        foreach (string word in words)
                        {
                            result = 0;
                            if (double.TryParse(word, out result))
                            {
                                data[row, col] = result;
                            }
                            col++;
                        }
                        row++;
                    }
                }
                return data;
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        static public double[,] ToArray (string fileName)
        {
            try
            {
                return ToArray(fileName, 0);
            }
            catch (Exception)
            {

                throw;
            }
              

        }
    }
}
