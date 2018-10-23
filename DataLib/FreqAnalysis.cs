using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet;

namespace DataLib
{
    public class FourierPt
    {
        public double Frequency { get; set; }
        public double Amplitude { get; set; }

    }
    public class FreqAnalysis
    {
        static public FourierPt[] FFT(CylData input)
        {
            try
            {
                int len = 0;

               var  fourierOptions= MathNet.Numerics.IntegralTransforms.FourierOptions.Default;
                if (input.Count % 2 == 0)
                {
                    len = input.Count + 2;
                }
                else
                {
                    len = input.Count + 1;
                }
                var data = new double[len];
                for(int j =0;j<input.Count;j++)
                {
                    data[j] = input[j].R;
                }
                double sampleRate = input.Count;
                return GetFFT(data, input.Count, sampleRate, fourierOptions);
            }
            catch (Exception)
            {

                throw;
            }

        }
        static public FourierPt[] FFT(double[] input,double sampleRate)
        {
            try
            {
                int len = 0;
                var fourierOptions = MathNet.Numerics.IntegralTransforms.FourierOptions.NoScaling;
                if (input.Length % 2 == 0)
                {
                    len = input.Length + 2;
                }
                else
                {
                    len = input.Length + 1;
                }
                var data = new double[len];
                for (int j = 0; j < input.Length; j++)
                {
                    data[j] = input[j];
                }
                return GetFFT(data, input.Length, sampleRate, fourierOptions);
                
            }
            catch (Exception)
            {

                throw;
            }          
        }
        static FourierPt[] GetFFT(double[] data,int inputLength,double sampleRate, MathNet.Numerics.IntegralTransforms.FourierOptions fourierOptions)
        {
            MathNet.Numerics.IntegralTransforms.Fourier.ForwardReal(data, inputLength, fourierOptions);
            var freqs = MathNet.Numerics.IntegralTransforms.Fourier.FrequencyScale(inputLength, sampleRate);
            var fftOut = new List<FourierPt>();
           
            for (int i = 0; i < inputLength-1; i+=2)
            {
                if(freqs[i]>=0)
                {
                    var mag = Math.Sqrt(Math.Pow(data[i], 2) + Math.Pow(data[i + 1], 2.0));
                    fftOut.Add(new FourierPt { Frequency = i/2.0, Amplitude = mag });
                }
                
            }
            return fftOut.ToArray();
        }
        
    }
}
