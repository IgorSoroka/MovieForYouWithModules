﻿using Microsoft.Practices.Unity;
using ModuleMainModule.Views;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace MainModule
{
    public class MovieForYouMainModule : IModule
    {
        private  readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        public MovieForYouMainModule(RegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(StartView));
            _regionManager.RegisterViewWithRegion("ListRegion", typeof(MoviesList));

            _container.RegisterTypeForNavigation<MoviesList>();
            _container.RegisterTypeForNavigation<ShowsList>();
            _container.RegisterTypeForNavigation<ActorsList>();
            _container.RegisterTypeForNavigation<MovieView>();
            _container.RegisterTypeForNavigation<ShowView>();
            _container.RegisterTypeForNavigation<ActorView>();
            _container.RegisterTypeForNavigation<MovieSearchView>();
            _container.RegisterTypeForNavigation<ActorSearchView>();
            _container.RegisterTypeForNavigation<ShowSearchView>();
            _container.RegisterTypeForNavigation<Player>();
        }
    }
}