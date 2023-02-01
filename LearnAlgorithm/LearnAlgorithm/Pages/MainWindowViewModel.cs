using Stylet;
using Stylet.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnAlgorithm.Pages
{
    public class MainWindowViewModel : Conductor<IScreen>.Collection.OneActive
    {

        private readonly IWindowManager _windowManager;
        private readonly IViewFactory _viewFactory;

        public MainWindowViewModel(IWindowManager windowManager, IViewFactory viewFactory)
        {
            _windowManager = windowManager;
            _viewFactory = viewFactory;
        }

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();
            InitMenuItemList();
        }

        #region Menu

        public class MenuItem
        {
            public string? Title { get; set; }
        }

        public List<MenuItem> MenuItemList { get; set; }
        public int SelectedIndex { get; set; } = 0;

        private void InitMenuItemList()
        {
            MenuItemList = new List<MenuItem>()
            {
                new MenuItem(){Title="Fourier Transform"},
                new MenuItem(){Title="Fourier Phase"},
                new MenuItem(){Title="Linear Regression"},
                new MenuItem(){Title="Polynomial Regresion"},
                new MenuItem(){Title="Linear Combination"},
                new MenuItem(){Title="Curve 1"},
                new MenuItem(){Title="Curve 2"},
            };

            this.Bind(s => SelectedIndex, (o, e) => SelectedIndexChanged());
            SelectedIndexChanged();
        }

        private void SelectedIndexChanged()
        {
            switch (SelectedIndex)
            {
                case 0: ActivateItem(FourierTransformPageView ??= _viewFactory.FourierTransformPageViewModel()); break;
                case 1: ActivateItem(FourierPhaseView ??= _viewFactory.FourierPhaseViewModel()); break;
                case 2: ActivateItem(LinearRegressionPageView ??= _viewFactory.LinearRegressionPageViewModel()); break;
                case 3: ActivateItem(PolynomialRegresionPageView ??= _viewFactory.PolynomialRegresionPageViewModel()); break;
                case 4: ActivateItem(LinearCombinationPageView ??= _viewFactory.LinearCombinationPageViewModel()); break;
                case 5: ActivateItem(CurvePageView ??= _viewFactory.CurvePageViewModel()); break;
                case 6: ActivateItem(CurveFuncPageView ??= _viewFactory.CurveFuncPageViewModel()); break;
            }
        }

        private FourierTransformPageViewModel FourierTransformPageView;
        private FourierPhaseViewModel FourierPhaseView;
        private LinearRegressionPageViewModel LinearRegressionPageView;
        private PolynomialRegresionPageViewModel PolynomialRegresionPageView;
        private LinearCombinationPageViewModel LinearCombinationPageView;
        private CurvePageViewModel CurvePageView;
        private CurveFuncPageViewModel CurveFuncPageView;

        #endregion
    }
}
