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
        public double[] PolynomialRegresioResult { get; set; }
        public Func<double, double> ResultFunc { get; set; }
    }
   

    public enum ScatterplotType
    {
        None = 0,

        /// <summary>
        /// 直线
        /// </summary>
        LineRegression = 1,

        /// <summary>
        /// 多项式
        /// </summary>
        PolynomialRegresion = 2,

        /// <summary>
        /// 函数y=F(x)
        /// </summary>
        Function = 3,
    }
}
