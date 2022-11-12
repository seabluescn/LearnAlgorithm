using ControlzEx.Standard;
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
    public class CurveFuncPageViewModel : Screen
    {
        private readonly Random random = new Random();
        private double s = 0;

        public double p0 { get; set; } = 100;
        public double p1 { get; set; } = 10;
        public double p2 { get; set; } = 50;

        public double noise { get; set; } = 0;


        public double Result_P0 { get; set; }
        public string Result_P0_Str => $"A={Result_P0:0.000000}";

        public double Result_P1 { get; set; }
        public string Result_P1_Str => $"t1={Result_P1:0.000000}";

        public double Result_P2 { get; set; }
        public string Result_P2_Str => $"t2={Result_P2:0.000000}";


        public ScatterplotData ScatterplotData { get; set; }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            Invalidate();

            this.Bind(s => p0, (o, e) => Invalidate());
            this.Bind(s => p1, (o, e) => Invalidate());
            this.Bind(s => p2, (o, e) => Invalidate());
            this.Bind(s => noise, (o, e) => Invalidate());
        }

        private void Invalidate()
        {
            int Count = 200;

            double[] SampleX = new double[Count];
            double[] SampleY = new double[Count];

            for (int i = 0; i < Count; i++)
            {
                double x = i;
                double y = MyFunction2(s, p0, p1, p2, x);
                y += y * (noise / 100) * (random.NextDouble() * 2 - 1);

                SampleX[i] = x;
                SampleY[i] = y;
            }

            try
            {
                var result = Fit.Curve(
                     SampleX,
                     SampleY,
                     (a, b, c, x) => MyFunction2(s, a, b, c, x),
                     initialGuess0: 100,
                     initialGuess1: 10,
                     initialGuess2: 50,
                     maxIterations: 1000000);

                Result_P0 = result.P0;
                Result_P1 = result.P1;
                Result_P2 = result.P2;

                var result_func = Fit.CurveFunc(SampleX,
                     SampleY,
                     (a, b, c, x) => MyFunction2(s, a, b, c, x),
                     initialGuess0: 100,
                     initialGuess1: 10,
                     initialGuess2: 50,
                     maxIterations: 1000000);

                ScatterplotData = new ScatterplotData()
                {
                    ScatterplotType = ScatterplotType.Function,
                    SampleX = SampleX,
                    SampleY = SampleY,
                    ResultFunc = result_func,
                };
            }
            catch
            {
                Result_P0 = 0;
                Result_P1 = 0;
                Result_P2 = 0;
            }
        }

        //y=A*[e^(-x/t1)-e^(-x/t2)]
        private static double MyFunction2(double start, double A, double t1, double t2, double x)
        {
            double y;
            y = A * (Math.Exp(-x / t1) - Math.Exp(-x / t2));
            return y;
        }
    }
}
