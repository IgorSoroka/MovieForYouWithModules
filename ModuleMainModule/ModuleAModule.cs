using Microsoft.Practices.Unity;
using ModuleMainModule.Views;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace MainModule
{
    public class ModuleAModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public ModuleAModule(RegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(StartView));

            _container.RegisterTypeForNavigation<MoviesList>();
            _container.RegisterTypeForNavigation<ShowsList>();
            _container.RegisterTypeForNavigation<ActorsList>();

            _container.RegisterTypeForNavigation<MovieView>();
            _container.RegisterTypeForNavigation<ShowView>();
            _container.RegisterTypeForNavigation<ActorView>();

            _container.RegisterTypeForNavigation<MovieSearchView>();
            _container.RegisterTypeForNavigation<ActorSearchView>();

            //_regionManager.RequestNavigate("ListRegion", "MoviesList");
        }
    }
}
