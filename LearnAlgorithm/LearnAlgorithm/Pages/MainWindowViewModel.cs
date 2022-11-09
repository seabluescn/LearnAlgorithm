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

        #region 菜单

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
                new MenuItem(){Title="FourierTransform"},               
            };

            this.Bind(s => SelectedIndex, (o, e) => SelectedIndexChanged());
            SelectedIndexChanged();
        }

        private void SelectedIndexChanged()
        {
            switch (SelectedIndex)
            {
                case 0: ActivateItem(FourierTransformPageView ??= _viewFactory.FourierTransformPageViewModel()); break;              
            }
        }

        private FourierTransformPageViewModel FourierTransformPageView;
       

        #endregion
    }
}
