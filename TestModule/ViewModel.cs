using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace TestModule
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        
        public DelegateCommand<string> NavigateCommandMain { get; private set; }
        public DelegateCommand<string> NavigateCommandListShow { get; private set; }
        public DelegateCommand<string> NavigateCommandListMovie { get; private set; }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandMain = new DelegateCommand<string>(NavigateMain);
            NavigateCommandListShow = new DelegateCommand<string>(NavigateListShow);
            NavigateCommandListMovie = new DelegateCommand<string>(NavigateListMovie);
        }

        private void NavigateMain(string navigatePath)
        {
            if (navigatePath != null)
            { _regionManager.RequestNavigate("MainRegion", navigatePath);}
        }

        private void NavigateListShow(string type)
        {
            var parameters = new NavigationParameters {{"type", type}};
            if (type != null)
            { _regionManager.RequestNavigate("ListRegion", "ShowsList", parameters);}
        }

        private void NavigateListMovie(string type)
        {
            var parameters = new NavigationParameters {{"type", type}};

            if (type != null)
            { _regionManager.RequestNavigate("ListRegion", "MoviesList", parameters);}
        }
    }
}