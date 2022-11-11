using LearnAlgorithm.Controls;
using MathNet.Numerics;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LearnAlgorithm.Pages
{
    public class CurvePageViewModel : Screen
    {
        private readonly Random random = new Random();
        private double s = 0;

        public double p1 { get; set; } = 1;
        public double p2 { get; set; } = 2;
        public double p3 { get; set; } = 3;

        public double noise { get; set; } = 20;


        public double Result_P1 { get; set; }
        public string Result_P1_Str => $"a={Result_P1:0.000000}";

        public double Result_P2 { get; set; }
        public string Result_P2_Str => $"b={Result_P2:0.000000}";

        public double Result_P3 { get; set; }
        public string Result_P3_Str => $"c={Result_P3:0.000000}";


        public ScatterplotData ScatterplotData { get; set; }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            Invalidate();

            this.Bind(s => p1, (o, e) => Invalidate());
            this.Bind(s => p2, (o, e) => Invalidate());
            this.Bind(s => p3, (o, e) => Invalidate());
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
                double y = MyFunction(s, p1, p2, p3, x);
                y += y * (noise / 100) * (random.NextDouble() * 2 - 1);

                SampleX[i] = x;
                SampleY[i] = y;
            }

            var result = Fit.Curve(
                 SampleX,
                 SampleY,
                 (a, b, c, x) => MyFunction(s, a, b, c, x),
                 initialGuess0: 1,
                 initialGuess1: 1,
                 initialGuess2: 1,
                 maxIterations: 1000000);

            Result_P1 = result.P0;
            Result_P2 = result.P1;
            Result_P3 = result.P2;


            var result_func = Fit.CurveFunc(SampleX,
                 SampleY,
                 (a, b, c, x) => MyFunction(s, a, b, c, x),
                 initialGuess0: p1,
                 initialGuess1: p2,
                 initialGuess2: p3,
                 maxIterations: 1000000);

            ScatterplotData = new ScatterplotData()
            {
                ScatterplotType = ScatterplotType.LinearCombination,
                SampleX = SampleX,
                SampleY = SampleY,
                ResultFunc = result_func,
            };
        }

        //y=0+a*Sin(b*x+c)
        private static double MyFunction(double s, double a, double b, double c, double x)
        {
            double y;
            y = a * Math.Sin(b * x + c);

            return y;
        }

    }
}
