using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnAlgorithm.Controls
{
    public class ScatterplotData
    {
        public ScatterplotType ScatterplotType { get; set; }

        public double[] SampleX { get; set; }
        public double[] SampleY { get; set; }

        public (double p0, double p1) LinearRegressionResult { get; set; }
    }


    public enum ScatterplotType
    {
        LineRegression = 0,
    }
}
