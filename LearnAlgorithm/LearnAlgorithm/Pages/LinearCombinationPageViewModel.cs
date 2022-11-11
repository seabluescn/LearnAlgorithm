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
    public class LinearCombinationPageViewModel : Screen
    {
        private readonly Random random = new Random();
       
        public double p0 { get; set; } = 0;
        public double p1 { get; set; } = 2;
        public double p2 { get; set; } = 3;     
        public double noise { get; set; } = 10;      

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
            int Count = 100;

            double[] SampleX = new double[Count];
            double[] SampleY = new double[Count];

            for (int i = 0; i < Count; i++)
            {
                double x = random.NextDouble() * 2 - 1;
                double y = p0
                    + p1 * Math.Sin(2 * Math.PI * x)
                    + p2 * Math.Cos(2 * Math.PI * x);

                y += y * (noise / 100) * (random.NextDouble() * 2 - 1); //add noise

                SampleX[i] = x;
                SampleY[i] = y;
            }

            Func<double,double> resultFunc = Fit.LinearCombinationFunc(
                SampleX,
                SampleY,
                x => 1.0,
                x => Math.Sin(2 * Math.PI * x),
                x => Math.Cos(2 * Math.PI * x));           

            ScatterplotData = new ScatterplotData()
            {
                ScatterplotType = ScatterplotType.LinearCombination,
                SampleX = SampleX,
                SampleY = SampleY,
                LinearCombinationFunc=resultFunc,
            };
        }
    }
}
