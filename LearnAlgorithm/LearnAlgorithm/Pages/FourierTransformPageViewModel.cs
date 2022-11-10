using LearnAlgorithm.Controls;
using LearnAlgorithm.Library;
using Stylet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LearnAlgorithm.Pages
{
    public class FourierTransformPageViewModel : Screen
    {
        private readonly int Sample = 10000; // Sample 
        private readonly double t = 0.1;      // Sample time
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

            this.Bind(s => Low, (o, e) => Invalidate());
            this.Bind(s => High, (o, e) => Invalidate());
        }

        public double Vdc { get; set; } = 0;

        public double F1 { get; set; } = 50;
        public double F2 { get; set; } = 150;
        public double F3 { get; set; } = 400;

        public double Vp1 { get; set; } = 200;
        public double Vp2 { get; set; } = 100;
        public double Vp3 { get; set; } = 50;

        public int Low { get; set; } = 10;
        public int High { get; set; } = 100;


        public WaveGraphicsData WaveData { get; set; }
        public WaveGraphicsData FreqencyData { get; set; }
        public WaveGraphicsData NewWaveData { get; set; }


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
                    + Vp1 * Math.Sin(2 * Math.PI * i / S1)
                    + Vp2 * Math.Sin(2 * Math.PI * i / S2)
                    + Vp3 * Math.Sin(2 * Math.PI * i / S3);
            }

            double[] Freq = FourierTransform.Forward(OriginalData);

            double[] NewData = FourierTransform.BandpassFilter(OriginalData, Low, High, (int)(1.0 / t));

            WaveData = new WaveGraphicsData
            {
                GraphicsType = GraphicsType.Wave,
                WaveData = OriginalData,
            };

            FreqencyData = new WaveGraphicsData
            {
                GraphicsType = GraphicsType.Frequency,
                WaveData = Freq,
            };

            NewWaveData = new WaveGraphicsData
            {
                GraphicsType = GraphicsType.Wave,
                WaveData = NewData,
            };
        }
    }
}
