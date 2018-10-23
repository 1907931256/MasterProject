using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace WinFormsLib
{
    public class InputVerification
    {
        string[] numbers = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "."};
        string[] mathSymbols = { "+", "-", "*", "/", "%", "^" };
        string[] numberListSymbols = { ".", ",", "-" };
        public static bool TryGetValue(TextBox textBox, string badValMessage, out double result)
        {
            double tmp=0;

            if (double.TryParse(textBox.Text, out tmp))
            {
                textBox.ForeColor = Control.DefaultForeColor;
                result = tmp;
                return true;
            }
            else
            {
                textBox.ForeColor = Color.Red;
                textBox.Text = badValMessage;
                result = 0;
                return false;
            }
        }
        public static bool TryGetValue(TextBox textBox, string badValMessage, out int result)
        {
            int intTmp = 0;
           
            if (int.TryParse(textBox.Text, out intTmp))
            {
                textBox.ForeColor = Control.DefaultForeColor;
                result = intTmp;
                return true;
            }
            else
            {
                textBox.ForeColor = Color.Red;
                textBox.Text = badValMessage;
                result = 0;
                return false;
            }
        }
        /// <summary>
        /// returns true and value as ref from text box if double and inside bounds
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="badValMessage"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetValue(TextBox textBox, string badValMessage, double min, double max, out double result)
        {
            try
            {
                textBox.ForeColor = Control.DefaultForeColor;
                double tmp = 0;
                result = 0;
                bool valueOk = false;
                if (double.TryParse(textBox.Text, out tmp))
                {
                    if (tmp < min)
                    {
                        textBox.ForeColor = Color.Red;                        
                        result = min;
                        textBox.Text = result.ToString();
                        valueOk = false;
                    }
                    else if (tmp > max)
                    {
                        textBox.ForeColor = Color.Red;
                        result = max;
                        textBox.Text = result.ToString();
                        valueOk = false;
                    }
                    else
                    {
                        textBox.ForeColor = Control.DefaultForeColor;
                        result = tmp;
                        valueOk = true;
                    }
                }
                else
                {
                    textBox.ForeColor = Color.Red;
                    result= ((min + max) / 2);
                    textBox.Text = badValMessage;
                    valueOk = false;
                }
                return valueOk;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// returns true and value as ref from text box if integer and inside bounds
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="badValMessage"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetValue(TextBox textBox, string badValMessage, int min, int max, out int result)
        {
            try
            {
                double tmp = 0;
                int intTmp = 0;
                result = 0;
                bool valueOk = false;
                textBox.ForeColor = Control.DefaultForeColor;
                if (double.TryParse(textBox.Text, out tmp))
                {
                    intTmp = (int)Math.Round(tmp);

                    if (tmp < min)
                    {
                        textBox.ForeColor = Color.Red;
                        valueOk = false;
                        result = 0;
                        textBox.Text = badValMessage;
                    }
                    else if (tmp > max)
                    {
                        textBox.ForeColor = Color.Red;
                        result = 0;
                        valueOk = false;
                        textBox.Text = badValMessage;
                    }
                    else if (intTmp != tmp)
                    {
                        textBox.ForeColor = Color.Red;
                        result = 0;
                        valueOk = false;
                        textBox.Text = badValMessage;
                    }
                    else
                    {
                        textBox.ForeColor = Control.DefaultForeColor;
                        result = intTmp;
                        valueOk = true;
                    }
                }
                else
                {
                    textBox.ForeColor = Color.Red;

                    textBox.Text = badValMessage;
                    valueOk = false;
                }
                return valueOk;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// returns true and value as ref from text box if long and inside bounds
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="badValMessage"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetValue(TextBox textBox, string badValMessage, long min, long max, out long result)
        {
            double tmp = 0;
            long intTmp = 0;
            result = 0;
            bool valueOk = false;
            textBox.ForeColor = Control.DefaultForeColor;
            if (double.TryParse(textBox.Text, out tmp))
            {
                intTmp = (long)Math.Round(tmp);

                if (tmp < min)
                {
                    textBox.ForeColor = Color.Red;

                    result = min;
                    textBox.Text = badValMessage;
                }
                else if (tmp > max)
                {
                    textBox.ForeColor = Color.Red;
                    result = max;
                    textBox.Text = badValMessage;
                }
                else if (intTmp != tmp)
                {
                    textBox.ForeColor = Color.Red;
                    result = intTmp;
                    textBox.Text = result.ToString();
                }
                else
                {
                    textBox.ForeColor = Control.DefaultForeColor;
                    result = intTmp;
                    valueOk = true;
                }
            }
            else
            {
                textBox.ForeColor = Color.Red;
                result = (long)Math.Round((double)(min + max) / 2);
                textBox.Text = result.ToString();
            }
            return valueOk;
        }
        public static bool TryGetValues (TextBox textBox, string badValMessage, double min, double max, out double[] results) 
        {

            var values = new List<double>();
            string input = textBox.Text.ToUpper();
            textBox.ForeColor = Control.DefaultForeColor;
            input = input.Trim();
            string[] grooveN = input.Split(',');
            //add in values
            foreach (string s in grooveN)
            {
                double g = 0;
                double.TryParse(s, out g);
                values.Add(g);
            }
            bool listOK = true;
            foreach(var v in values)
            {
                if(v>max || v<min)
                {
                    listOK = false;
                    textBox.Text = badValMessage;
                    break;
                }
            }
            results = values.ToArray();
            return listOK;
         
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textBox"> returns array of ints and true if input is comma or dash separated values or ALL</param>
        /// <param name="badValMessage"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        public static bool TryGetValues(TextBox textBox, string badValMessage, int min, int max, out int[] results)
        {

            List<int> values = new List<int>();         
            string input =  textBox.Text.ToUpper();
            textBox.ForeColor = Control.DefaultForeColor;
            input = input.Trim();
            //add all values to list
            if (input.Contains("ALL"))
            {
                for (int j = min; j <= max; j++)
                {
                    values.Add(j);
                }
            }
            else
            {
                //look for dashes 
                char[] charArr = input.ToCharArray();
                List<int> dashIndex = new List<int>();
                
                for (int i = 0; i < charArr.Length; i++)
                {
                    if (charArr[i] == '-')
                    {
                        dashIndex.Add(i);
                    }
                }

                string[] grooveN = input.Split(',', '-');
                //add in values
                foreach (string s in grooveN)
                {
                    int g = 0;
                    int.TryParse(s, out g);
                    values.Add(g);
                }
                //add in values between dashes
                foreach (int dash in dashIndex)
                {
                    if (dash > 0 && dash < input.Length - 1)
                    {
                        string setStart = input.Substring(dash - 1, 1);
                        string setEnd = input.Substring(dash + 1, 1);
                        int start = 0;
                        int end = 0;
                        int.TryParse(setStart, out start);
                        int.TryParse(setEnd, out end);
                        for (int i = start + 1; i < end; i++)
                        {
                            values.Add(i);
                        }
                    }
                }
            }
            //sort and remove duplicates
            int[] vArray = values.ToArray();
            Array.Sort(vArray);
            List<int> valOut = new List<int>();
            foreach (int g in vArray)
            {
                if (g > 0 && g <= max)
                {                    
                    if (!valOut.Contains(g))
                    {
                        valOut.Add(g);
                    }
                }
            }
            results = valOut.ToArray();
            if (results.Length > 0)
            {
                textBox.ForeColor = Control.DefaultForeColor; 
                return true;
            }
            else
            {
                textBox.ForeColor = Color.Red;
                textBox.Text = badValMessage;
                return false;
            }
        }        
    }
}
