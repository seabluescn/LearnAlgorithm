using LearnAlgorithm.Controls;
using LearnAlgorithm.Library;
using MathNet.Numerics.IntegralTransforms;
using Stylet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LearnAlgorithm.Pages
{
    public class FourierPhaseViewModel : Screen
    {
        private readonly int Sample = 1000; // Sample 
        private readonly double time = 1;      // Sample time
        private readonly int Count = 1000;  // Count=Sample*t      

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            Invalidate();

            this.Bind(s => Vdc, (o, e) => Invalidate());
            this.Bind(s => Vp1, (o, e) => Invalidate());
            this.Bind(s => Vp2, (o, e) => Invalidate());
            this.Bind(s => Vp3, (o, e) => Invalidate());
            this.Bind(s => F1, (o, e) => Invalidate());
            this.Bind(s => F2, (o, e) => Invalidate());
            this.Bind(s => F3, (o, e) => Invalidate());
            this.Bind(s => Phase3, (o, e) => Invalidate());
        }

        public double Vdc { get; set; } = 0;

        public double F1 { get; set; } = 10;
        public double F2 { get; set; } = 20;
        public double F3 { get; set; } = 40;

        public double Vp1 { get; set; } = 200;
        public double Vp2 { get; set; } = 100;
        public double Vp3 { get; set; } = 50;

        public double Phase3 { get; set; } = 0;

        public double Phase3_result { get; set; }
        public string PhasesStr => $"Signal Ⅲ Phase:{Phase3_result:0.000}Π";

        public WaveGraphicsData WaveData { get; set; }
        public WaveGraphicsData FreqencyData { get; set; }

        private void Invalidate()
        {
            double S1 = Sample / F1;
            double S2 = Sample / F2;
            double S3 = Sample / F3;

            double[] OriginalData = new double[Count];
            for (int i = 0; i < Count; i++)
            {
                OriginalData[i]
                    = Vdc
                    + Vp1 * Math.Cos(2 * Math.PI * i / S1)
                    + Vp2 * Math.Cos(2 * Math.PI * i / S2)
                    + Vp3 * Math.Cos(2 * Math.PI * i / S3 + Phase3 * Math.PI);
            }

            double[] Freq = Forward(OriginalData);

            WaveData = new WaveGraphicsData
            {
                GraphicsType = GraphicsType.Wave,
                WaveData = OriginalData,
                sampletime = time,
            };

            FreqencyData = new WaveGraphicsData
            {
                GraphicsType = GraphicsType.Frequency,
                WaveData = Freq,
                sampletime = time
            };
        }

        public double[] Forward(double[] input)
        {
            int DataLength = input.Length;
            Complex[] samples = new Complex[DataLength];
            for (int i = 0; i < DataLength; i++)
            {
                samples[i] = new Complex(input[i], 0);
            }

            //傅立叶变换
            Fourier.Forward(samples);

            //求模
            int FreqLen = DataLength / 2 + 1;
            double[] freqMod = new double[FreqLen];
            double rate = Math.Sqrt(DataLength) / 2;

            for (int i = 0; i < FreqLen; i++)
            {
                var a = samples[i].Real;
                var b = samples[i].Imaginary;
                freqMod[i] = Math.Sqrt(a * a + b * b) / rate;

                if (i == (int)F3)
                {
                    double A;

                    if (a == 0)
                    {
                        if (b >= 0)
                        {
                            A = 0.5 * Math.PI;
                        }
                        else
                        {
                            A = 1.5 * Math.PI;
                        }
                    }
                    else
                    {
                        A = Math.Atan(b / a);

                        if (a > 0)
                        {
                            if(b<0)
                            {
                                A += 2 * Math.PI;
                            }
                        }
                        else
                        {
                            A += Math.PI;
                        }
                    }

                    Phase3_result = A / Math.PI;

                    Debug.WriteLine($"{i}:{freqMod[i]:0.00}({a / rate:0.00}+{b / rate:0.00}i) Phase={Phase3_result:0.000}Π");


                }
            }

            return freqMod;
        }
    }
}
