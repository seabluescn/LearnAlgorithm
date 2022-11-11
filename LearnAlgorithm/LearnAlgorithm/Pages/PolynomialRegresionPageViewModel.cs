using LearnAlgorithm.Controls;
using MathNet.Numerics;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnAlgorithm.Pages
{
    public class PolynomialRegresionPageViewModel : Screen
    {
        private readonly Random random = new Random();
        private readonly int Multiplys = 5;

        public double p0 { get; set; } = 1;
        public double p1 { get; set; } = -2;
        public double p2 { get; set; } = 3;
        public double p3 { get; set; } = -4;
        public double p4 { get; set; } = 5;
        public double p5 { get; set; } = -6;
        public double noise { get; set; } = 0;

        public double Result_P0 { get; set; }
        public string Result_P0_Str => $"p0={Result_P0:0.000000}";

        public double Result_P1 { get; set; }
        public string Result_P1_Str => $"p1={Result_P1:0.000000}";

        public double Result_P2 { get; set; }
        public string Result_P2_Str => $"p2={Result_P2:0.000000}";

        public double Result_P3 { get; set; }
        public string Result_P3_Str => $"p3={Result_P3:0.000000}";

        public double Result_P4 { get; set; }
        public string Result_P4_Str => $"p4={Result_P4:0.000000}";

        public double Result_P5 { get; set; }
        public string Result_P5_Str => $"p5={Result_P5:0.000000}";


        public ScatterplotData ScatterplotData { get; set; }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            Invalidate();

            this.Bind(s => p0, (o, e) => Invalidate());
            this.Bind(s => p1, (o, e) => Invalidate());
            this.Bind(s => p2, (o, e) => Invalidate());
            this.Bind(s => p3, (o, e) => Invalidate());
            this.Bind(s => p4, (o, e) => Invalidate());
            this.Bind(s => p5, (o, e) => Invalidate());
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
                double y = p0 + p1 * x
                    + p2 * Math.Pow(x, 2)
                    + p3 * Math.Pow(x, 3)
                    + p4 * Math.Pow(x, 4)
                    + p5 * Math.Pow(x, 5);
                y += y * (noise / 100) * (random.NextDouble() * 2 - 1);

                SampleX[i] = x;
                SampleY[i] = y;
            }

            double[] result = Fit.Polynomial(SampleX, SampleY, Multiplys);

            Result_P0 = result[0];
            Result_P1 = result[1];
            Result_P2 = result[2];
            Result_P3 = result[3];
            Result_P4 = result[4];
            Result_P5 = result[5];

            ScatterplotData = new ScatterplotData()
            {
                ScatterplotType = ScatterplotType.PolynomialRegresion,
                SampleX = SampleX,
                SampleY = SampleY,
                PolynomialRegresioResult = result,
            };
        }
    }
}
