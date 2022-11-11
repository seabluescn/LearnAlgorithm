using LearnAlgorithm.Controls;
using MathNet.Numerics;
using MathNet.Numerics.Random;
using Stylet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnAlgorithm.Pages
{
    public class LinearRegressionPageViewModel : Screen
    {
        private readonly Random random = new Random();

        public double p0 { get; set; } = 0;
        public double p1 { get; set; } = 1;
        public double noise { get; set; } = 10;

        public double Result_P0 { get; set; }
        public string Result_P0_Str => $"p0={Result_P0:0.000000}";

        public double Result_P1 { get; set; }
        public string Result_P1_Str => $"p0={Result_P1:0.000000}";


        public ScatterplotData ScatterplotData { get; set; }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            Invalidate();

            this.Bind(s => p0, (o, e) => Invalidate());
            this.Bind(s => p1, (o, e) => Invalidate());
            this.Bind(s => noise, (o, e) => Invalidate());
        }

        private void Invalidate()
        {
            int Count = 100;

            double[] SampleX = new double[Count];
            double[] SampleY = new double[Count];

            for (int i = 0; i < Count; i++)
            {
                double x = random.NextDouble() * 2 - 1;
                double y = p0 + p1 * x;
                y += y * (noise / 100) * (random.NextDouble() * 2 - 1);

                SampleX[i] = x;
                SampleY[i] = y;
            }

            (double p0, double p1) result = Fit.Line(SampleX, SampleY);

            Result_P0 = result.p0; 
            Result_P1=result.p1;          

            ScatterplotData = new ScatterplotData()
            {
                ScatterplotType = ScatterplotType.LineRegression,
                SampleX= SampleX,
                SampleY= SampleY,
                LinearRegressionResult = result,
            };
        }
    }
}
