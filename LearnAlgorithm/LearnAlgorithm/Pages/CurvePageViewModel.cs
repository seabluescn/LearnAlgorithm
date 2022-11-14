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

        public double p0 { get; set; } = 1;
        public double p1 { get; set; } = 2;
        public double p2 { get; set; } = 3;

        public double noise { get; set; } = 10;

        public double Result_P0 { get; set; }
        public string Result_P0_Str => $"a={Result_P0:0.000000}";

        public double Result_P1 { get; set; }
        public string Result_P1_Str => $"b={Result_P1:0.000000}";

        public double Result_P2 { get; set; }
        public string Result_P2_Str => $"c={Result_P2:0.000000}";


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
                double x = random.NextDouble() * 2 - 1;
                double y = p0 * Math.Sin(p1 * x + p2);
                y += y * (noise / 100) * (random.NextDouble() * 2 - 1);

                SampleX[i] = x;
                SampleY[i] = y;
            }

            var result = Fit.Curve(
                 SampleX,
                 SampleY,
                 (a, b, c, x) => a * Math.Sin(b * x + c),
                 initialGuess0: 1,
                 initialGuess1: 1,
                 initialGuess2: 1,
                 maxIterations: 10000000);

            Result_P0 = result.P0;
            Result_P1 = result.P1;
            Result_P2 = result.P2;

            var result_func = Fit.CurveFunc(SampleX,
                 SampleY,
                 (a, b, c, x) => a * Math.Sin(b * x + c),
                 initialGuess0: 1,
                 initialGuess1: 1,
                 initialGuess2: 1,
                 maxIterations: 10000000);

            ScatterplotData = new ScatterplotData()
            {
                ScatterplotType = ScatterplotType.Function,
                SampleX = SampleX,
                SampleY = SampleY,
                ResultFunc = result_func,
            };
        } 
    }
}
