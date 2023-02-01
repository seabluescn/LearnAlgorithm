using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnAlgorithm.Pages
{
    public interface IViewFactory
    {
        FourierTransformPageViewModel FourierTransformPageViewModel();
        FourierPhaseViewModel FourierPhaseViewModel();
        LinearRegressionPageViewModel LinearRegressionPageViewModel();
        PolynomialRegresionPageViewModel PolynomialRegresionPageViewModel();
        LinearCombinationPageViewModel LinearCombinationPageViewModel();
        CurvePageViewModel CurvePageViewModel();
        CurveFuncPageViewModel CurveFuncPageViewModel();
    }
}
